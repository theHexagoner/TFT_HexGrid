using HexGridInterfaces.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexGridLib.Maps
{
    public class Map : IMap
    {

        private Map() { }

        internal Map(IGrid grid)
        {
            GridHexagons = grid.Hexagons;
            MapHexagons = new HexDictionary<int, IHexagon>();
            EdgeDict = new Dictionary<int, IEdge>(grid.Edges);

            MapHexagons.OnDictionaryAddItem += AddingHexagon;
            MapHexagons.OnDictionaryRemoveItem += RemovingHexagon;
            MapHexagons.OnDictionaryClear += ClearingHexagons;

            //SvgHexagons = new Dictionary<int, SvgHexagon>();
            //SvgMegagons = new Dictionary<int, SvgMegagon>();

            foreach (IHexagon h in grid.Hexagons.Values)
            {
                MapHexagons.Add(h.ID, h);
            }

            //// get the SVG data for hexagons
            //foreach (Hexagon h in MapHexagons.Values)
            //{
            //    SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.OffsetLocation.Row, h.OffsetLocation.Col, h.Points));
            //}

            //// build the SvgMegagons
            //foreach (GridEdge edge in EdgeDict.Values)
            //{
            //    if (SvgMegagonsFactory.GetEdgeIsMegaLine(edge))
            //    {
            //        // add a new SvgMegagon
            //        SvgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, SvgMegagonsFactory.GetPathD(edge)));
            //    }
            //}
        }

        public IEnumerable<KeyValuePair<int, IEdge>> Edges
        {
            get { return EdgeDict; }
        }

        public IEnumerable<KeyValuePair<int, IHexagon>> Hexagons
        {
            get { return MapHexagons; }
        }

        public void AddHexagon(int ID)
        {
            if (MapHexagons.ContainsKey(ID) == false)
                MapHexagons.Add(ID, GridHexagons[ID]);
        }

        public void RemoveHexagon(int ID)
        {
            if (MapHexagons.ContainsKey(ID))
                MapHexagons.Remove(ID);
        }

        #region Manage Hexagons

        private Dictionary<int, IEdge> EdgeDict { get; }

        private HexDictionary<int, IHexagon> MapHexagons { get; set; }

        private IDictionary<int, IHexagon> GridHexagons { get; set; }

        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
        {
            var hex = e.Value;
            //SvgHexagons.Add(e.Key, new SvgHexagon(e.Key, hex.OffsetLocation.Row, hex.OffsetLocation.Col,  hex.Points, true));

            // update the map's edges and revise the SvgMegagons as necessary
            foreach (IEdge edge in hex.Edges)
            {
                if (EdgeDict.ContainsKey(edge.ID))
                {
                    if (EdgeDict[edge.ID].Hexagons.ContainsKey(e.Key) == false)
                        EdgeDict[edge.ID].Hexagons.Add(e.Key, hex);

                    //if (SvgMegagonsFactory.GetEdgeIsMegaLine(Edges[edge.ID]) &&
                    //    SvgMegagons.ContainsKey(edge.ID) == false)
                    //    SvgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, SvgMegagonsFactory.GetPathD(edge)));
                }
            }
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
        {
            var hex = e.Value;
            //SvgHexagons.Remove(e.Key);

            // update the map's edges and revise the SvgMegagons as necessary
            foreach (IEdge edge in hex.Edges)
            {
                if (EdgeDict.ContainsKey(edge.ID))
                {
                    if (EdgeDict[edge.ID].Hexagons.ContainsKey(e.Key))
                        EdgeDict[edge.ID].Hexagons.Remove(e.Key);

                    //if (SvgMegagonsFactory.GetEdgeIsMegaLine(Edges[edge.ID]) == false
                    //    && SvgMegagons.ContainsKey(edge.ID))
                    //    SvgMegagons.Remove(edge.ID);
                }
            }
        }

        private void ClearingHexagons(object sender, DictionaryChangingEventArgs<int, IHexagon> e)
        {
            //SvgHexagons.Clear();
            //SvgMegagons.Clear();
        }

        #endregion



    }
}
