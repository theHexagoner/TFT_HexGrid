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
            Hexagons.OnDictionaryAddItem += AddingHexagon;
            Hexagons.OnDictionaryRemoveItem += RemovingHexagon;
            Hexagons.OnDictionaryClear += ClearingHexagons;

            SvgHexagons = new Dictionary<int, SvgHexagon>();
            SvgMegagons = new Dictionary<int, SvgMegagon>();

            foreach (Hexagon h in Hexagons.Values)
            {
                string pathD = SvgMegagonsFactory.GetPathD(Hexagons.Values, h) ;
                SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Points));
                SvgMegagons.Add(h.ID, new SvgMegagon(h.ID, pathD));
            }
        }

        public Dictionary<int, SvgHexagon> SvgHexagons { get; }
        public Dictionary<int, SvgMegagon> SvgMegagons { get; }

        #region Hexagons

        public HexDictionary<int, Hexagon> Hexagons { get; private set; }
        
        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            var hex = e.Value;
            string pathD = SvgMegagonsFactory.GetPathD(Hexagons.Values, hex);
            SvgHexagons.Add(hex.ID, new SvgHexagon(hex.ID, hex.Points, true));
            SvgMegagons.Add(hex.ID, new SvgMegagon(hex.ID, pathD));

            Console.WriteLine(string.Format("Adding at Row: {0}  Col: {1} ", hex.OffsetLocation.Row.ToString(), hex.OffsetLocation.Col.ToString()));
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            Console.WriteLine(string.Format("Removing at Row: {0}  Col: {1} ", e.Value.OffsetLocation.Row.ToString(), e.Value.OffsetLocation.Col.ToString()));

            SvgHexagons.Remove(e.Key);
            SvgMegagons.Remove(e.Key);

            // for each neighbor, recalculate its corresponding SvgMegagon
            Cube[] adjs = Cube.GetAdjacents(e.Value.CubicLocation);
            Hexagon[] neighbors = new Hexagon[6];

            for (int i = 0; i < 6; i++)
            {
                var hex = Hexagons.Values.SingleOrDefault(h => adjs[i] == h.CubicLocation);

                if (hex != null)
                {
                    string pathD = SvgMegagonsFactory.GetPathD(Hexagons.Values, hex);
                    SvgMegagons[hex.ID] = new SvgMegagon(hex.ID, pathD);

                    Console.WriteLine(string.Format("Updating Mega at Row: {0}  Col: {1} ", hex.OffsetLocation.Row.ToString(), hex.OffsetLocation.Col.ToString()));
                }
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
