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
            string pathD = SvgMegagonsFactory.GetFlatPathD(Hexagons.Values, hex);
            SvgHexagons.Add(hex.ID, new SvgHexagon(hex.ID, hex.Points, true));
            SvgMegagons.Add(hex.ID, new SvgMegagon(hex.ID, pathD));

            Console.WriteLine(string.Format("Adding at Row: {0}  Col: {1} ", hex.OffsetLocation.Row.ToString(), hex.OffsetLocation.Col.ToString()));
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            Console.WriteLine(string.Format("Removing at Row: {0}  Col: {1} ", e.Value.OffsetLocation.Row.ToString(), e.Value.OffsetLocation.Col.ToString()));

            SvgHexagons.Remove(e.Key);

            // recalculate the edges and resulting SvgMegagons

        }

        private void ClearingHexagons(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Clear();
            SvgMegagons.Clear();
        }

        #endregion

    }

}
