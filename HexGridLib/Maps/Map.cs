using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridLib.Maps
{
    public class Map : IMap
    {
        #region Events

        public event IMap.MapDictionaryAddItem OnMapDictionaryAddItem;
        public event IMap.MapDictionaryRemoveItem OnMapDictionaryRemoveItem;

        #endregion

        private Map() { }

        internal Map(IGrid grid)
        {
            GridHexagons = grid.Hexagons;
            MapHexagons = new HexDictionary<int, IHexagon>();
            EdgeDict = new Dictionary<int, IEdge>(grid.Edges);

            MapHexagons.OnDictionaryAddItem += AddingHexagon;
            MapHexagons.OnDictionaryRemoveItem += RemovingHexagon;

            foreach (IHexagon h in grid.Hexagons.Values)
            {
                MapHexagons.Add(h.ID, h);
            }
        }

        public IDictionary<int, IEdge> Edges
        {
            get { return EdgeDict; }
        }

        public IDictionary<int, IHexagon> Hexagons
        {
            get { return MapHexagons; }
        }

        public void AddHexagon(int ID)
        {
            if (MapHexagons.ContainsKey(ID) == false)
            {
                MapHexagons.Add(ID, GridHexagons[ID]);
                OnMapDictionaryAddItem?.Invoke(this, 
                    new DictionaryChangingEventArgs<int, IHexagon>() { Key = ID, Value = GridHexagons[ID] });
            }
        }

        public void RemoveHexagon(int ID)
        {
            if (MapHexagons.ContainsKey(ID))
            {
                var success = MapHexagons.Remove(ID);
                if (success) OnMapDictionaryRemoveItem?.Invoke(this, 
                    new DictionaryChangingEventArgs<int, IHexagon>() { Key = ID, Value = GridHexagons[ID] });
            }
        }

        #region Manage Hexagons

        private Dictionary<int, IEdge> EdgeDict { get; }

        private HexDictionary<int, IHexagon> MapHexagons { get; set; }

        private IDictionary<int, IHexagon> GridHexagons { get; set; }

        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
        {
            var hex = e.Value;

            // update the map's edges
            foreach (IEdge edge in hex.Edges)
            {
                if (EdgeDict.ContainsKey(edge.ID))
                {
                    if (EdgeDict[edge.ID].Hexagons.ContainsKey(e.Key) == false)
                        EdgeDict[edge.ID].Hexagons.Add(e.Key, hex);
                }
            }
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
        {
            var hex = e.Value;

            // update the map's edges and revise the SvgMegagons as necessary
            foreach (IEdge edge in hex.Edges)
            {
                if (EdgeDict.ContainsKey(edge.ID))
                {
                    if (EdgeDict[edge.ID].Hexagons.ContainsKey(e.Key))
                        EdgeDict[edge.ID].Hexagons.Remove(e.Key);
                }
            }
        }

        #endregion

    }
}
