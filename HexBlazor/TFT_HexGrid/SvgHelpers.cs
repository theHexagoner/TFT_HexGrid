using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFT_HexGrid.Grids;

namespace TFT_HexGrid.SvgHelpers
{

    public struct SvgHexagon
    {
        public readonly int Id;
        public readonly string Points;
        public readonly bool IsSelected;
        public readonly string PathD;

        public SvgHexagon(int id, GridPoint[] points) : this(id, points, true) { }

        public SvgHexagon(int id, GridPoint[] points, bool isSelected)
        {
            Id = id;
            Points = string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
            IsSelected = isSelected;
            PathD = SvgMegagonsFactory.GetPathD();
        }
    }

    public enum MegaLocation
    {
        N = -1,  // not set
        X = 0,  // center
        A = 1,  // flat = lower-right   pointy = right
        B = 2,  // flat = upper-right   pointy = upper-right
        C = 3,  // flat = top           pointy = upper-left
        D = 4,  // flat = upper-left    pointy = left
        E = 5,  // flat = lower-left    pointy = lower-left
        F = 6   // flat = bottom        pointy = lower-right
    }

    public struct SvgMegagon
    {
        public readonly int Id;
        public readonly string D; //e.g. <path d = "M20,230 Q40,205 50,230 T90,230" />

        public SvgMegagon(int id, string d)
        {
            Id = id;
            D = d;
        }

    }

    /// <summary>
    /// utility class to help assign hexagons to megagons
    /// </summary>
    public static class SvgMegagonsFactory
    {
        // derive various megahex layouts from offset coordinates according to offset scheme

        public static void SetMegaLocations(OffsetScheme offsetScheme, Hexagon[] hexagons)
        {
            switch (offsetScheme)
            {
                case OffsetScheme.Even_Q:
                    WalkEvenQ(hexagons);
                    break;
                case OffsetScheme.Even_R:
                    WalkEvenR(hexagons);
                    break;
                case OffsetScheme.Odd_Q:
                    WalkOddQ(hexagons);
                    break;
                case OffsetScheme.Odd_R:
                    WalkOddR(hexagons);
                    break;
            }
        }

        private static void WalkEvenQ(Hexagon[] hexagons)
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

        private static void WalkOddQ(Hexagon[] hexagons)
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
                int c11 = (h.OffsetLocation.Col + 15551) % 14; // multiples of 14, +3
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

        private static void WalkEvenR(Hexagon[] hexagons)
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

        private static void WalkOddR(Hexagon[] hexagons)
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

        private static void SetMegaLocations(Hexagon h, Hexagon[] hexagons)
        {
            h.SetLocationInMegagon(MegaLocation.X);

            // get this hexagon's neighbors and also set their Mega location
            var adjs = Cube.GetAdjacents(h.CubicLocation);

            for (int i = 0; i < adjs.Length; i++)
            {
                Hexagon hex = hexagons.SingleOrDefault(h => h.CubicLocation.Equals(adjs[i]));
                if (hex != null) hex.SetLocationInMegagon((MegaLocation)(i+1));
            }
        }

        //private SvgMegagon GetSvgMegagon(Hexagon center)
        //{
        //    try
        //    {
        //        int id = center.ID;

        //        GridPoint[] centerPoints = center.Points;
        //        Cube[] adjs = Cube.GetAdjacents(center.CubicLocation);

        //        var somePoints = new List<GridPoint>();

        //        // loop over the adjacent cubes and add the hex corner points for each
        //        foreach (Cube c in adjs)
        //        {
        //            // figure out if the cube is part of the grid
        //            Hexagon hex = Hexagons.SingleOrDefault(h => h.CubicLocation.Equals(c) && Overscan.Contains(h.ID) == false);

        //            if (hex != null) // get the points
        //            {
        //                somePoints.AddRange(hex.Points);
        //            }
        //        }

        //        // get rid of duplicates and then any points that belong to the center hex
        //        // put them in clockwise order around the reference point of the center hex
        //        GridPoint[] outline = somePoints.Distinct().Except(centerPoints)
        //            .OrderBy(x => Math.Atan2(x.X - centerPoints[0].X, x.Y - centerPoints[0].Y)).ToArray();

        //        if (outline.Length > 0)
        //        {
        //            // use a string builder to build up the D for the path
        //            var sb = new StringBuilder();
        //            sb.Append(string.Format("M{0},{1} ", outline[0].X, outline[0].Y));

        //            for (int i = 1; i < outline.Length; i++)
        //            {
        //                // get the distance from previous point
        //                var distance = Math.Round(outline[i].GetDistanceTo(outline[i - 1]));

        //                if (distance > Radius)
        //                {
        //                    sb.Append(string.Format("M{0},{1} ", outline[i].X, outline[i].Y));
        //                }
        //                else
        //                {
        //                    sb.Append(string.Format("L{0},{1} ", outline[i].X, outline[i].Y));
        //                }
        //            }

        //            var distanceForLast = Math.Round(outline[^1].GetDistanceTo(outline[0]));

        //            if (distanceForLast <= Radius)
        //                sb.Append(string.Format("L{0},{1} ", outline[0].X, outline[0].Y));

        //            string d = sb.ToString();
        //            return new SvgMegagon(id, d);
        //        }

        //        return new SvgMegagon(0, "");

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new SvgMegagon(0, "");
        //    }

        //}

        public static string GetPathD()
        {


            return string.Empty;

        }


    }

}
