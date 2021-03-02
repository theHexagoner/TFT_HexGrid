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

        public SvgHexagon(int id, GridPoint[] points) : this(id, points, true) { }

        public SvgHexagon(int id, GridPoint[] points, bool isSelected)
        {
            Id = id;
            Points = string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
            IsSelected = isSelected;
        }
    }

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

        /// <summary>
        /// return the path D for megagon lines of given hex
        /// </summary>
        /// <returns>System.String containing the D for the SVG path</returns>
        public static string GetPathD(Hexagon hex)
        {
            string pathD = string.Empty;

            // path D will vary based on location in megagon
            MegaLocation locationInMegagon = hex.MegaLocation;

            GridPoint[] p;

            switch(locationInMegagon)
            {
                case MegaLocation.A:
                    p = new[] { hex.Points[5], hex.Points[0], hex.Points[1], hex.Points[2] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.B:
                    p = new[] { hex.Points[4], hex.Points[5], hex.Points[0], hex.Points[1] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.C:
                    p = new[] { hex.Points[3], hex.Points[4], hex.Points[5], hex.Points[0] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.D:
                    p = new[] { hex.Points[2], hex.Points[3], hex.Points[4], hex.Points[5] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.E:
                    p = new[] { hex.Points[1], hex.Points[2], hex.Points[3], hex.Points[4] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.F:
                    p = new[] { hex.Points[0], hex.Points[1], hex.Points[2], hex.Points[3] };
                    pathD = GetPathD(p);
                    break;

                default:
                    break; // for X and N there is no path drawn
            }

            return pathD;
        }

        /// <summary>
        /// return the SVG path d for megagon lines of given hex and its neighbors
        /// </summary>
        /// <param name="hexagons">the context from which to return neighbors</param>
        /// <param name="hex">the hex for which to calculate path d</param>
        /// <returns>System.String containing the path d</returns>
        public static string GetPathD(ICollection<Hexagon> hexagons, Hexagon hex)
        {
            string pathD = string.Empty;

            // path D will vary based on location in megagon
            MegaLocation locationInMegagon = hex.MegaLocation;

            // and by presence/absence of neighbors?
            Cube[] adjs = Cube.GetAdjacents(hex.CubicLocation);
            Hexagon[] neighbors = new Hexagon[6];

            for (int i = 0; i < 6; i++)
            {
                neighbors[i] = hexagons.SingleOrDefault(h => adjs[i] == h.CubicLocation);
            }

            GridPoint[] p;
            
            GridPoint[] B = Array.Empty<GridPoint>();
            GridPoint[] A = Array.Empty<GridPoint>();
            GridPoint[] F = Array.Empty<GridPoint>();
            GridPoint[] E = Array.Empty<GridPoint>();
            GridPoint[] D = Array.Empty<GridPoint>();
            GridPoint[] C = Array.Empty<GridPoint>();

            switch (locationInMegagon)
            {
                case MegaLocation.A:
                    if (neighbors[(int)MegaLocation.B - 1] != null) B = new[] { hex.Points[5], hex.Points[0] }; else B = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.A - 1] != null) A = new[] { hex.Points[0], hex.Points[1] }; else A = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.F - 1] != null) F = new[] { hex.Points[1], hex.Points[2] }; else F = Array.Empty<GridPoint>();

                    p = new GridPoint[B.Length + A.Length + F.Length];
                    B.CopyTo(p, 0);
                    A.CopyTo(p, B.Length);
                    F.CopyTo(p, B.Length + A.Length);

                    pathD = GetPathD(p);
                    break;

                case MegaLocation.B:
                    if (neighbors[(int)MegaLocation.C - 1] != null) C = new[] { hex.Points[4], hex.Points[5] }; else C = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.B - 1] != null) B = new[] { hex.Points[5], hex.Points[0] }; else B = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.A - 1] != null) A = new[] { hex.Points[0], hex.Points[1] }; else A = Array.Empty<GridPoint>();

                    p = new GridPoint[C.Length + B.Length + A.Length];
                    C.CopyTo(p, 0);
                    B.CopyTo(p, C.Length);
                    A.CopyTo(p, C.Length + B.Length);

                    pathD = GetPathD(p);
                    break;

                case MegaLocation.C:
                    if (neighbors[(int)MegaLocation.D - 1] != null) D = new[] { hex.Points[3], hex.Points[4] }; else D = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.C - 1] != null) C = new[] { hex.Points[4], hex.Points[5] }; else C = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.B - 1] != null) B = new[] { hex.Points[5], hex.Points[0] }; else B = Array.Empty<GridPoint>();

                    p = new GridPoint[D.Length + C.Length + B.Length];
                    D.CopyTo(p, 0);
                    C.CopyTo(p, D.Length);
                    B.CopyTo(p, D.Length + C.Length);

                    pathD = GetPathD(p);
                    break;

                case MegaLocation.D:
                    if (neighbors[(int)MegaLocation.E - 1] != null) E = new[] { hex.Points[2], hex.Points[3] }; else E = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.D - 1] != null) D = new[] { hex.Points[3], hex.Points[4] }; else D = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.C - 1] != null) C = new[] { hex.Points[4], hex.Points[5] }; else C = Array.Empty<GridPoint>();

                    p = new GridPoint[E.Length + D.Length + C.Length];
                    E.CopyTo(p, 0);
                    D.CopyTo(p, E.Length);
                    C.CopyTo(p, E.Length + D.Length);

                    pathD = GetPathD(p);
                    break;

                case MegaLocation.E:
                    if (neighbors[(int)MegaLocation.F - 1] != null) F = new[] { hex.Points[1], hex.Points[2] }; else F = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.E - 1] != null) E = new[] { hex.Points[2], hex.Points[3] }; else E = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.D - 1] != null) D = new[] { hex.Points[3], hex.Points[4] }; else D = Array.Empty<GridPoint>();

                    p = new GridPoint[F.Length + E.Length + D.Length];
                    F.CopyTo(p, 0);
                    E.CopyTo(p, F.Length);
                    D.CopyTo(p, F.Length + E.Length);

                    pathD = GetPathD(p);
                    break;

                case MegaLocation.F:
                    if (neighbors[(int)MegaLocation.A - 1] != null) A = new[] { hex.Points[0], hex.Points[1] }; else A = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.F - 1] != null) F = new[] { hex.Points[1], hex.Points[2] }; else F = Array.Empty<GridPoint>();
                    if (neighbors[(int)MegaLocation.E - 1] != null) E = new[] { hex.Points[2], hex.Points[3] }; else E = Array.Empty<GridPoint>();

                    p = new GridPoint[F.Length + E.Length + D.Length];
                    F.CopyTo(p, 0);
                    E.CopyTo(p, F.Length);
                    D.CopyTo(p, F.Length + E.Length);

                    pathD = GetPathD(p);
                    break;

                default:
                    break; // for X and N there is no path drawn
            }

            return pathD;
        }

        private static string GetPathD(GridPoint[] points)
        {
            // use a string builder to build up the D for the path
            var sb = new StringBuilder();

            if(points.Length > 1)
            {
                var radius = Math.Round(GridPoint.GetDistance(points[0], points[1]));

                sb.Append(string.Format("M{0},{1} ", points[0].X, points[0].Y));

                for (int i = 1; i < points.Length; i++)
                {
                    // get the distance from previous point
                    var distance = Math.Round(points[i].GetDistanceTo(points[i - 1]));

                    if (distance > 0 && distance > radius)
                    {
                        sb.Append(string.Format("M{0},{1} ", points[i].X, points[i].Y));
                    }
                    else if (distance > 0)
                    {
                        sb.Append(string.Format("L{0},{1} ", points[i].X, points[i].Y));
                    }
                }
            }

            return sb.ToString();

        }
    
    }

}
