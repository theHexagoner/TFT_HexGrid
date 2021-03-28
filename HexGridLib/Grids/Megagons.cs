using System.Linq;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;

namespace HexGridLib.Grids
{


    internal sealed class MegagonLocationSetter
    {
        private MegagonLocationSetter()
        {
        }

        #region Set Location of Hex in Megagon

        public static void SetMegaLocations(OffsetSchema schema, IHexagon[] hexagons)
        {
            if (schema.HexStyle == HexagonStyle.Flat && schema.OffsetPush == OffsetPush.Even && schema.MegahexSkew == MegagonSkew.Right)
            {
                FindFlatEvenRightCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Flat && schema.OffsetPush == OffsetPush.Even && schema.MegahexSkew == MegagonSkew.Left)
            {
                FindFlatEvenLeftCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Flat && schema.OffsetPush == OffsetPush.Odd && schema.MegahexSkew == MegagonSkew.Right)
            {
                FindFlatOddRightCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Flat && schema.OffsetPush == OffsetPush.Odd && schema.MegahexSkew == MegagonSkew.Left)
            {
                FindFlatOddLeftCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Pointy && schema.OffsetPush == OffsetPush.Even && schema.MegahexSkew == MegagonSkew.Right)
            {
                FindPointyEvenRightCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Pointy && schema.OffsetPush == OffsetPush.Even && schema.MegahexSkew == MegagonSkew.Left)
            {
                FindPointyEvenLeftCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Pointy && schema.OffsetPush == OffsetPush.Odd && schema.MegahexSkew == MegagonSkew.Right)
            {
                FindPointyOddRightCenters(hexagons);
            }
            else if (schema.HexStyle == HexagonStyle.Pointy && schema.OffsetPush == OffsetPush.Odd && schema.MegahexSkew == MegagonSkew.Left)
            {
                FindPointyOddLeftCenters(hexagons);
            }
        }

        #region Find Centers

        private static void FindFlatEvenLeftCenters(IHexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 17 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 17 mod 14 == 6
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 17 mod 14 == 12
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 17 mod 14 == 4
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 17 mod 14 == 10
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 17 mod 14 == 2
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 17 mod 14 = 8

            foreach (IHexagon h in hexagons)
            {
                int r7 = (h.OffsetLocation.Row + 7777) % 7;    // multiples of 7
                int c14 = (h.OffsetLocation.Col + 15554) % 14; // multiples of 14
                int c17 = (h.OffsetLocation.Col + 15557) % 14; // multiples of 14, +3
                bool isCenterHex = false;

                switch (r7)
                {
                    case 0:
                        isCenterHex = c14 == 0 || c17 == 0;
                        break;
                    case 1:
                        isCenterHex = c14 == 6 || c17 == 6;
                        break;
                    case 2:
                        isCenterHex = c14 == 12 || c17 == 12;
                        break;
                    case 3:
                        isCenterHex = c14 == 4 || c17 == 4;
                        break;
                    case 4:
                        isCenterHex = c14 == 10 || c17 == 10;
                        break;
                    case 5:
                        isCenterHex = c14 == 2 || c17 == 2;
                        break;
                    case 6:
                        isCenterHex = c14 == 8 || c17 == 8;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        private static void FindFlatEvenRightCenters(IHexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 11 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 11 mod 14 == 8
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 11 mod 14 == 2
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 11 mod 14 == 10
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 11 mod 14 == 4
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 11 mod 14 == 12
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 11 mod 14 == 6

            foreach (IHexagon h in hexagons)
            {
                int r7 = (h.OffsetLocation.Row + 7777) % 7;    // multiples of 7
                int c14 = (h.OffsetLocation.Col + 15554) % 14; // multiples of 14
                int c11 = (h.OffsetLocation.Col + 15551) % 14; // multiples of 14, -3
                bool isCenterHex = false;

                switch (r7)
                {
                    case 0:
                        isCenterHex = c14 == 0 || c11 == 0;
                        break;
                    case 1:
                        isCenterHex = c14 == 8 || c11 == 8;
                        break;
                    case 2:
                        isCenterHex = c14 == 2 || c11 == 2;
                        break;
                    case 3:
                        isCenterHex = c14 == 10 || c11 == 10;
                        break;
                    case 4:
                        isCenterHex = c14 == 4 || c11 == 4;
                        break;
                    case 5:
                        isCenterHex = c14 == 12 || c11 == 12;
                        break;
                    case 6:
                        isCenterHex = c14 == 6 || c11 == 6;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        private static void FindFlatOddLeftCenters(IHexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 11 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 11 mod 14 == 6
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 11 mod 14 == 12
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 11 mod 14 == 4
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 11 mod 14 == 10
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 11 mod 14 == 2
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 11 mod 14 == 8

            foreach (IHexagon h in hexagons)
            {
                int r7 = (h.OffsetLocation.Row + 7777) % 7;    // multiples of 7
                int c14 = (h.OffsetLocation.Col + 15554) % 14; // multiples of 14
                int c11 = (h.OffsetLocation.Col + 15551) % 14; // multiples of 14, -3
                bool isCenterHex = false;

                switch (r7)
                {
                    case 0:
                        isCenterHex = c14 == 0 || c11 == 0;
                        break;
                    case 1:
                        isCenterHex = c14 == 6 || c11 == 6;
                        break;
                    case 2:
                        isCenterHex = c14 == 12 || c11 == 12;
                        break;
                    case 3:
                        isCenterHex = c14 == 4 || c11 == 4;
                        break;
                    case 4:
                        isCenterHex = c14 == 10 || c11 == 10;
                        break;
                    case 5:
                        isCenterHex = c14 == 2 || c11 == 2;
                        break;
                    case 6:
                        isCenterHex = c14 == 8 || c11 == 8;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }
        
        private static void FindFlatOddRightCenters(IHexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 17 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 17 mod 14 == 8
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 17 mod 14 == 2
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 17 mod 14 == 10
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 17 mod 14 == 4
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 17 mod 14 == 12
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 17 mod 14 == 6

            foreach (IHexagon h in hexagons)
            {
                int r7 = (h.OffsetLocation.Row + 7777) % 7;    // multiples of 7
                int c14 = (h.OffsetLocation.Col + 15554) % 14; // multiples of 14
                int c17 = (h.OffsetLocation.Col + 15557) % 14; // multiples of 14, +3
                bool isCenterHex = false;

                switch (r7)
                {
                    case 0:
                        isCenterHex = c14 == 0 || c17 == 0;
                        break;
                    case 1:
                        isCenterHex = c14 == 8 || c17 == 8;
                        break;
                    case 2:
                        isCenterHex = c14 == 2 || c17 == 2;
                        break;
                    case 3:
                        isCenterHex = c14 == 10 || c17 == 10;
                        break;
                    case 4:
                        isCenterHex = c14 == 4 || c17 == 4;
                        break;
                    case 5:
                        isCenterHex = c14 == 12 || c17 == 12;
                        break;
                    case 6:
                        isCenterHex = c14 == 6 || c17 == 6;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        private static void FindPointyEvenLeftCenters(IHexagon[] hexagons)
        {
            // For Even R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 17 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 6  OR ELSE Row + 17 mod 14 == 6
            // (Col mod 7 == 2) => Row + 14 mod 14 == 12 OR ELSE Row + 17 mod 14 == 12
            // (Col mod 7 == 3) => Row + 14 mod 14 == 4  OR ELSE Row + 17 mod 14 == 4
            // (Col mod 7 == 4) => Row + 14 mod 14 == 10 OR ELSE Row + 17 mod 14 == 10
            // (Col mod 7 == 5) => Row + 14 mod 14 == 2  OR ELSE Row + 17 mod 14 == 2
            // (Col mod 7 == 6) => Row + 14 mod 14 == 8  OR ELSE Row + 17 mod 14 = 8

            foreach (IHexagon h in hexagons)
            {
                int c7 = (h.OffsetLocation.Col + 7777) % 7;    // multiples of 7
                int r14 = (h.OffsetLocation.Row + 15554) % 14; // multiples of 14
                int r17 = (h.OffsetLocation.Row + 15557) % 14; // multiples of 14, +3
                bool isCenterHex = false;

                switch (c7)
                {
                    case 0:
                        isCenterHex = r14 == 0 || r17 == 0;
                        break;
                    case 1:
                        isCenterHex = r14 == 6 || r17 == 6;
                        break;
                    case 2:
                        isCenterHex = r14 == 12 || r17 == 12;
                        break;
                    case 3:
                        isCenterHex = r14 == 4 || r17 == 4;
                        break;
                    case 4:
                        isCenterHex = r14 == 10 || r17 == 10;
                        break;
                    case 5:
                        isCenterHex = r14 == 2 || r17 == 2;
                        break;
                    case 6:
                        isCenterHex = r14 == 8 || r17 == 8;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        private static void FindPointyEvenRightCenters(IHexagon[] hexagons)
        {
            // For Even R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 11 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 8  OR ELSE Row + 11 mod 14 == 8
            // (Col mod 7 == 2) => Row + 14 mod 14 == 2  OR ELSE Row + 11 mod 14 == 2
            // (Col mod 7 == 3) => Row + 14 mod 14 == 10 OR ELSE Row + 11 mod 14 == 10
            // (Col mod 7 == 4) => Row + 14 mod 14 == 4  OR ELSE Row + 11 mod 14 == 4
            // (Col mod 7 == 5) => Row + 14 mod 14 == 12 OR ELSE Row + 11 mod 14 == 12
            // (Col mod 7 == 6) => Row + 14 mod 14 == 6  OR ELSE Row + 11 mod 14 == 6

            foreach (IHexagon h in hexagons)
            {
                int c7 = (h.OffsetLocation.Col + 7777) % 7;    // multiples of 7
                int r14 = (h.OffsetLocation.Row + 15554) % 14; // multiples of 14
                int r11 = (h.OffsetLocation.Row + 15551) % 14; // multiples of 14, -3
                bool isCenterHex = false;

                switch (c7)
                {
                    case 0:
                        isCenterHex = r14 == 0 || r11 == 0;
                        break;
                    case 1:
                        isCenterHex = r14 == 8 || r11 == 8;
                        break;
                    case 2:
                        isCenterHex = r14 == 2 || r11 == 2;
                        break;
                    case 3:
                        isCenterHex = r14 == 10 || r11 == 10;
                        break;
                    case 4:
                        isCenterHex = r14 == 4 || r11 == 4;
                        break;
                    case 5:
                        isCenterHex = r14 == 12 || r11 == 12;
                        break;
                    case 6:
                        isCenterHex = r14 == 6 || r11 == 6;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        private static void FindPointyOddLeftCenters(IHexagon[] hexagons)
        {
            // For Odd R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 11 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 6  OR ELSE Row + 11 mod 14 == 6
            // (Col mod 7 == 2) => Row + 14 mod 14 == 12 OR ELSE Row + 11 mod 14 == 12
            // (Col mod 7 == 3) => Row + 14 mod 14 == 4  OR ELSE Row + 11 mod 14 == 4
            // (Col mod 7 == 4) => Row + 14 mod 14 == 10 OR ELSE Row + 11 mod 14 == 10
            // (Col mod 7 == 5) => Row + 14 mod 14 == 2  OR ELSE Row + 11 mod 14 == 2
            // (Col mod 7 == 6) => Row + 14 mod 14 == 8  OR ELSE Row + 11 mod 14 = 8

            foreach (IHexagon h in hexagons)
            {
                int c7 = (h.OffsetLocation.Col + 7777) % 7;    // multiples of 7
                int r14 = (h.OffsetLocation.Row + 15554) % 14; // multiples of 14
                int r11 = (h.OffsetLocation.Row + 15551) % 14; // multiples of 14, +3
                bool isCenterHex = false;

                switch (c7)
                {
                    case 0:
                        isCenterHex = r14 == 0 || r11 == 0;
                        break;
                    case 1:
                        isCenterHex = r14 == 6 || r11 == 6;
                        break;
                    case 2:
                        isCenterHex = r14 == 12 || r11 == 12;
                        break;
                    case 3:
                        isCenterHex = r14 == 4 || r11 == 4;
                        break;
                    case 4:
                        isCenterHex = r14 == 10 || r11 == 10;
                        break;
                    case 5:
                        isCenterHex = r14 == 2 || r11 == 2;
                        break;
                    case 6:
                        isCenterHex = r14 == 8 || r11 == 8;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }
        
        private static void FindPointyOddRightCenters(IHexagon[] hexagons)
        {
            // For Odd R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 17 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 8  OR ELSE Row + 17 mod 14 == 8
            // (Col mod 7 == 2) => Row + 14 mod 14 == 2  OR ELSE Row + 17 mod 14 == 2
            // (Col mod 7 == 3) => Row + 14 mod 14 == 10 OR ELSE Row + 17 mod 14 == 10
            // (Col mod 7 == 4) => Row + 14 mod 14 == 4  OR ELSE Row + 17 mod 14 == 4
            // (Col mod 7 == 5) => Row + 14 mod 14 == 12 OR ELSE Row + 17 mod 14 == 12
            // (Col mod 7 == 6) => Row + 14 mod 14 == 6  OR ELSE Row + 17 mod 14 == 6

            foreach (IHexagon h in hexagons)
            {
                int c7 = (h.OffsetLocation.Col + 7777) % 7;    // multiples of 7
                int r14 = (h.OffsetLocation.Row + 15554) % 14; // multiples of 14
                int r17 = (h.OffsetLocation.Row + 15557) % 14; // multiples of 14, +3
                bool isCenterHex = false;

                switch (c7)
                {
                    case 0:
                        isCenterHex = r14 == 0 || r17 == 0;
                        break;
                    case 1:
                        isCenterHex = r14 == 8 || r17 == 8;
                        break;
                    case 2:
                        isCenterHex = r14 == 2 || r17 == 2;
                        break;
                    case 3:
                        isCenterHex = r14 == 10 || r17 == 10;
                        break;
                    case 4:
                        isCenterHex = r14 == 4 || r17 == 4;
                        break;
                    case 5:
                        isCenterHex = r14 == 12 || r17 == 12;
                        break;
                    case 6:
                        isCenterHex = r14 == 6 || r17 == 6;
                        break;
                }

                if (isCenterHex)
                {
                    SetMegaLocations(h, hexagons);
                }
            }
        }

        #endregion

        /// <summary>
        /// sets the location within the "parent" megahex for each of a set of hexagons
        /// which lie adjacent to the hexagon defined by h parameter
        /// </summary>
        /// <param name="center">the center hex</param>
        /// <param name="hexagons">the adjacent hexes</param>
        private static void SetMegaLocations(IHexagon center, IHexagon[] hexagons)
        {
            // the center hex
            center.SetLocationInMegagon(MegaLocation.X);

            // get the adjacent hexagons and also set their Mega location
            var adjs = CubicCoordinate.GetAdjacents(center.CubicLocation);

            for (int i = 0; i < adjs.Length; i++)
            {
                IHexagon hex = hexagons.SingleOrDefault(h => h.CubicLocation.Equals(adjs[i]));
                if (hex != null) hex.SetLocationInMegagon((MegaLocation)(i + 1));
            }
        }

        #endregion

    }
}