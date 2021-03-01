using TFT_HexGrid.Grids;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Text;
using TFT_HexGrid.SvgHelpers;
using System.Linq;

namespace TFT_HexGrid.Maps
{
    public class Map
    {
        private Map() { }

        internal Map(Grid grid)
        {
            Hexagons = new HexDictionary<int, Hexagon>(grid.Hexagons);
            Hexagons.OnDictionaryAddItem += AddingHexagon;
            Hexagons.OnDictionaryRemoveItem += RemovingHexagon;
            Hexagons.OnDictionaryClear += ClearingHexagons;

            SvgHexagons = new Dictionary<int, SvgHexagon>();

            foreach (Hexagon h in Hexagons.Values)
            {
                h.PathD = SvgMegagonsFactory.GetPathD(h);
                SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Points, h.PathD));
            }

            // this is going away
            SvgMegagons = new Dictionary<int, SvgMegagon>();

        }

        public Dictionary<int, SvgHexagon> SvgHexagons { get; }

        #region Hexagons

        public HexDictionary<int, Hexagon> Hexagons { get; private set; }
        
        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            var hex = e.Value;
            hex.PathD = SvgMegagonsFactory.GetPathD(hex);
            SvgHexagons.Add(hex.ID, new SvgHexagon(hex.ID, hex.Points, hex.PathD, true));
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Remove(e.Key);
        }

        private void ClearingHexagons(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Clear();
        }

        #endregion

        #region Megagons

        // going away
        public Dictionary<int, SvgMegagon> SvgMegagons { get; }

        #endregion

    }

}
