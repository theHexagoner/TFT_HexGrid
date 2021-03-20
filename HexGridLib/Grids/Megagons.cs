using System.Linq;
using HexGridLib.Coordinates;

namespace HexGridLib.Grids
{
    /// <summary>
    /// identifies the location of a hex within a megahex
    /// </summary>
    public enum MegaLocation
    {
        N = -1, // not set
        X = 0,  // center
        A = 1,  // flat = lower-right   pointy = right
        B = 2,  // flat = upper-right   pointy = upper-right
        C = 3,  // flat = top           pointy = upper-left
        D = 4,  // flat = upper-left    pointy = left
        E = 5,  // flat = lower-left    pointy = lower-left
        F = 6   // flat = bottom        pointy = lower-right
    }

    //public struct SvgMegagon
    //{
    //    public readonly int Id;
    //    public readonly string D; //e.g. <path d = "M20,230 Q40,205 50,230 T90,230" />

    //    public SvgMegagon(int id, string d)
    //    {
    //        Id = id;
    //        D = d;
    //    }

    //}

    internal sealed class SvgMegagonsFactory
    {
        private static readonly SvgMegagonsFactory _instance = new SvgMegagonsFactory();

        private SvgMegagonsFactory()
        {
        }

        public static SvgMegagonsFactory Instance
        {
            get
            {
                return _instance ?? new SvgMegagonsFactory();
            }
        }

        #region Set Location of Hex in Megagon

        public void SetMegaLocations(OffsetSchema schema, Hexagon[] hexagons)
        {
            if (schema.Style == HexagonStyle.Flat && schema.Offset == OffsetPush.Even && schema.Skew == MegagonSkew.Right)
            {
                FindFlatEvenRightCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Flat && schema.Offset == OffsetPush.Even && schema.Skew == MegagonSkew.Left)
            {
                FindFlatEvenLeftCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Flat && schema.Offset == OffsetPush.Odd && schema.Skew == MegagonSkew.Right)
            {
                FindFlatOddRightCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Flat && schema.Offset == OffsetPush.Odd && schema.Skew == MegagonSkew.Left)
            {
                FindFlatOddLeftCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Pointy && schema.Offset == OffsetPush.Even && schema.Skew == MegagonSkew.Right)
            {
                FindPointyEvenRightCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Pointy && schema.Offset == OffsetPush.Even && schema.Skew == MegagonSkew.Left)
            {
                FindPointyEvenLeftCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Pointy && schema.Offset == OffsetPush.Odd && schema.Skew == MegagonSkew.Right)
            {
                FindPointyOddRightCenters(hexagons);
            }
            else if (schema.Style == HexagonStyle.Pointy && schema.Offset == OffsetPush.Odd && schema.Skew == MegagonSkew.Left)
            {
                FindPointyOddLeftCenters(hexagons);
            }
        }

        #region Find Centers

        private void FindFlatEvenLeftCenters(Hexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 17 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 17 mod 14 == 6
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 17 mod 14 == 12
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 17 mod 14 == 4
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 17 mod 14 == 10
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 17 mod 14 == 2
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 17 mod 14 = 8

            foreach (Hexagon h in hexagons)
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

        private void FindFlatEvenRightCenters(Hexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 11 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 11 mod 14 == 8
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 11 mod 14 == 2
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 11 mod 14 == 10
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 11 mod 14 == 4
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 11 mod 14 == 12
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 11 mod 14 == 6

            foreach (Hexagon h in hexagons)
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

        private void FindFlatOddLeftCenters(Hexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 11 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 11 mod 14 == 6
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 11 mod 14 == 12
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 11 mod 14 == 4
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 11 mod 14 == 10
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 11 mod 14 == 2
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 11 mod 14 == 8

            foreach (Hexagon h in hexagons)
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
        
        private void FindFlatOddRightCenters(Hexagon[] hexagons)
        {
            // For Even Q scheme the hex is the central hex in a megagon if:
            // (Row mod 7 == 0) => Col + 14 mod 14 == 0  OR ELSE Col + 17 mod 14 == 0
            // (Row mod 7 == 1) => Col + 14 mod 14 == 6  OR ELSE Col + 17 mod 14 == 8
            // (Row mod 7 == 2) => Col + 14 mod 14 == 12 OR ELSE Col + 17 mod 14 == 2
            // (Row mod 7 == 3) => Col + 14 mod 14 == 4  OR ELSE Col + 17 mod 14 == 10
            // (Row mod 7 == 4) => Col + 14 mod 14 == 10 OR ELSE Col + 17 mod 14 == 4
            // (Row mod 7 == 5) => Col + 14 mod 14 == 2  OR ELSE Col + 17 mod 14 == 12
            // (Row mod 7 == 6) => Col + 14 mod 14 == 8  OR ELSE Col + 17 mod 14 == 6

            foreach (Hexagon h in hexagons)
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

        private void FindPointyEvenLeftCenters(Hexagon[] hexagons)
        {
            // For Even R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 17 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 6  OR ELSE Row + 17 mod 14 == 6
            // (Col mod 7 == 2) => Row + 14 mod 14 == 12 OR ELSE Row + 17 mod 14 == 12
            // (Col mod 7 == 3) => Row + 14 mod 14 == 4  OR ELSE Row + 17 mod 14 == 4
            // (Col mod 7 == 4) => Row + 14 mod 14 == 10 OR ELSE Row + 17 mod 14 == 10
            // (Col mod 7 == 5) => Row + 14 mod 14 == 2  OR ELSE Row + 17 mod 14 == 2
            // (Col mod 7 == 6) => Row + 14 mod 14 == 8  OR ELSE Row + 17 mod 14 = 8

            foreach (Hexagon h in hexagons)
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

        private void FindPointyEvenRightCenters(Hexagon[] hexagons)
        {
            // For Even R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 11 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 8  OR ELSE Row + 11 mod 14 == 8
            // (Col mod 7 == 2) => Row + 14 mod 14 == 2  OR ELSE Row + 11 mod 14 == 2
            // (Col mod 7 == 3) => Row + 14 mod 14 == 10 OR ELSE Row + 11 mod 14 == 10
            // (Col mod 7 == 4) => Row + 14 mod 14 == 4  OR ELSE Row + 11 mod 14 == 4
            // (Col mod 7 == 5) => Row + 14 mod 14 == 12 OR ELSE Row + 11 mod 14 == 12
            // (Col mod 7 == 6) => Row + 14 mod 14 == 6  OR ELSE Row + 11 mod 14 == 6

            foreach (Hexagon h in hexagons)
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

        private void FindPointyOddLeftCenters(Hexagon[] hexagons)
        {
            // For Odd R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 11 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 6  OR ELSE Row + 11 mod 14 == 6
            // (Col mod 7 == 2) => Row + 14 mod 14 == 12 OR ELSE Row + 11 mod 14 == 12
            // (Col mod 7 == 3) => Row + 14 mod 14 == 4  OR ELSE Row + 11 mod 14 == 4
            // (Col mod 7 == 4) => Row + 14 mod 14 == 10 OR ELSE Row + 11 mod 14 == 10
            // (Col mod 7 == 5) => Row + 14 mod 14 == 2  OR ELSE Row + 11 mod 14 == 2
            // (Col mod 7 == 6) => Row + 14 mod 14 == 8  OR ELSE Row + 11 mod 14 = 8

            foreach (Hexagon h in hexagons)
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
        
        private void FindPointyOddRightCenters(Hexagon[] hexagons)
        {
            // For Odd R scheme the hex is the central hex in a megagon if:
            // (Col mod 7 == 0) => Row + 14 mod 14 == 0  OR ELSE Row + 17 mod 14 == 0
            // (Col mod 7 == 1) => Row + 14 mod 14 == 8  OR ELSE Row + 17 mod 14 == 8
            // (Col mod 7 == 2) => Row + 14 mod 14 == 2  OR ELSE Row + 17 mod 14 == 2
            // (Col mod 7 == 3) => Row + 14 mod 14 == 10 OR ELSE Row + 17 mod 14 == 10
            // (Col mod 7 == 4) => Row + 14 mod 14 == 4  OR ELSE Row + 17 mod 14 == 4
            // (Col mod 7 == 5) => Row + 14 mod 14 == 12 OR ELSE Row + 17 mod 14 == 12
            // (Col mod 7 == 6) => Row + 14 mod 14 == 6  OR ELSE Row + 17 mod 14 == 6

            foreach (Hexagon h in hexagons)
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
        private static void SetMegaLocations(Hexagon center, Hexagon[] hexagons)
        {
            // the center hex
            center.SetLocationInMegagon(MegaLocation.X);

            // get the adjacent hexagons and also set their Mega location
            var adjs = Cube.GetAdjacents(center.CubicLocation);

            for (int i = 0; i < adjs.Length; i++)
            {
                Hexagon hex = hexagons.SingleOrDefault(h => h.CubicLocation.Equals(adjs[i]));
                if (hex != null) hex.SetLocationInMegagon((MegaLocation)(i + 1));
            }
        }

        #endregion

        #region Edges

        /// <summary>
        /// iterate over a set of points and return an array of GridEdge objects
        /// </summary>
        /// <param name="points">Array of GridPoint objects, generally this would be the points belonging to a hexagon</param>
        /// <returns>Array of GridEdge objects</returns>
        public static GridEdge[] GetEdgesFromPoints(GridPoint[] points)
        {
            GridEdge[] edges = new GridEdge[6];

            for (int i = 0; i < 5; i++)
            {
                edges[i] = new GridEdge(points[i], points[i + 1]);
            }

            edges[5] = new GridEdge(points[5], points[0]);

            return edges;
        }

        /// <summary>
        /// determines if the given edge should be displayed as part of a megagon
        /// </summary>
        /// <param name="edge">the edge to examine</param>
        /// <returns>Boolean, true if the edge should be represented by a megagon</returns>
        public static bool GetEdgeIsMegaLine(GridEdge edge)
        {
            bool isMegaLine = false;

            if (edge.Hexagons != null && edge.Hexagons.Count == 2)
            {
                Hexagon[] hexagons = edge.Hexagons.Values.ToArray();
                isMegaLine = hexagons[0].MegaLocation == MegaLocation.A && hexagons[1].MegaLocation == MegaLocation.D
                          || hexagons[0].MegaLocation == MegaLocation.A && hexagons[1].MegaLocation == MegaLocation.C
                          || hexagons[0].MegaLocation == MegaLocation.A && hexagons[1].MegaLocation == MegaLocation.E

                          || hexagons[0].MegaLocation == MegaLocation.B && hexagons[1].MegaLocation == MegaLocation.E
                          || hexagons[0].MegaLocation == MegaLocation.B && hexagons[1].MegaLocation == MegaLocation.D
                          || hexagons[0].MegaLocation == MegaLocation.B && hexagons[1].MegaLocation == MegaLocation.F

                          || hexagons[0].MegaLocation == MegaLocation.C && hexagons[1].MegaLocation == MegaLocation.F
                          || hexagons[0].MegaLocation == MegaLocation.C && hexagons[1].MegaLocation == MegaLocation.E
                          || hexagons[0].MegaLocation == MegaLocation.C && hexagons[1].MegaLocation == MegaLocation.A

                          || hexagons[0].MegaLocation == MegaLocation.D && hexagons[1].MegaLocation == MegaLocation.A
                          || hexagons[0].MegaLocation == MegaLocation.D && hexagons[1].MegaLocation == MegaLocation.F
                          || hexagons[0].MegaLocation == MegaLocation.D && hexagons[1].MegaLocation == MegaLocation.B

                          || hexagons[0].MegaLocation == MegaLocation.E && hexagons[1].MegaLocation == MegaLocation.B
                          || hexagons[0].MegaLocation == MegaLocation.E && hexagons[1].MegaLocation == MegaLocation.A
                          || hexagons[0].MegaLocation == MegaLocation.E && hexagons[1].MegaLocation == MegaLocation.C

                          || hexagons[0].MegaLocation == MegaLocation.F && hexagons[1].MegaLocation == MegaLocation.C
                          || hexagons[0].MegaLocation == MegaLocation.F && hexagons[1].MegaLocation == MegaLocation.B
                          || hexagons[0].MegaLocation == MegaLocation.F && hexagons[1].MegaLocation == MegaLocation.D;
            }

            return isMegaLine;
        }

        /// <summary>
        /// gets the SVG path information for an edge 
        /// </summary>
        /// <param name="edge">the edge from which to derive the SVG path data</param>
        /// <returns>System.String containing SVG Path d content</returns>
        public static string GetPathD(GridEdge edge)
        {
            return string.Format("M{0},{1} L{2},{3} ", edge.PointA.X, edge.PointA.Y, edge.PointB.X, edge.PointB.Y);
        }


        #endregion

    }
}