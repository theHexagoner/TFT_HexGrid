using System;
using System.Collections.Generic;
using System.Linq;
using TFT_HexGrid.Grids;
using TFT_HexGrid.SvgHelpers;

namespace TFT_HexGrid.Maps
{
    public class Map
    {
        private Map() { }

        internal Map(Grid grid)
        {
            Hexagons = new HexDictionary<int, Hexagon>(grid.Hexagons);
            Edges = new Dictionary<int, GridEdge>(grid.Edges);
            Hexagons.OnDictionaryAddItem += AddingHexagon;
            Hexagons.OnDictionaryRemoveItem += RemovingHexagon;
            Hexagons.OnDictionaryClear += ClearingHexagons;

            SvgHexagons = new Dictionary<int, SvgHexagon>();
            SvgMegagons = new Dictionary<int, SvgMegagon>();

            // get the SVG data for hexagons
            foreach (Hexagon h in Hexagons.Values)
            {
                SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Points));
            }

            // build the SvgMegagons
            foreach (GridEdge edge in Edges.Values)
            {
                if (SvgMegagonsFactory.GetEdgeIsMegaLine(edge))
                {
                    // add a new SvgMegagon
                    SvgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, SvgMegagonsFactory.GetPathD(edge)));
                }
            }
        }

        public Dictionary<int, SvgHexagon> SvgHexagons { get; }
        public Dictionary<int, SvgMegagon> SvgMegagons { get; }
        public Dictionary<int, GridEdge> Edges { get; }

        #region Hexagons

        public HexDictionary<int, Hexagon> Hexagons { get; private set; }
        
        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            var hex = e.Value;
            SvgHexagons.Add(e.Key, new SvgHexagon(e.Key, hex.Points, true));

            // update the map's edges and revise the SvgMegagons as necessary
            foreach (GridEdge edge in hex.Edges)
            {
                Edges[edge.ID].Hexagons.Add(e.Key, hex);
                if (SvgMegagonsFactory.GetEdgeIsMegaLine(Edges[edge.ID]))
                    SvgMegagons.Add(edge.ID, new SvgMegagon(edge.ID, SvgMegagonsFactory.GetPathD(edge)));
            }
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            var hex = e.Value;
            SvgHexagons.Remove(e.Key);

            // update the map's edges and revise the SvgMegagons as necessary
            foreach(GridEdge edge in hex.Edges)
            {
                Edges[edge.ID].Hexagons.Remove(e.Key);
                if (SvgMegagonsFactory.GetEdgeIsMegaLine(Edges[edge.ID]) == false)
                    SvgMegagons.Remove(edge.ID);
            }    
        }

        private void ClearingHexagons(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Clear();
            SvgMegagons.Clear();
        }

        #endregion

    }

}
