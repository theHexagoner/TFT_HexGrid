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

        #region PathDs

        #region PathDs for Grid

        public static string GetFlatPathD(Hexagon hex)
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

        public static string GetPointyPathD(Hexagon hex)
        {
            string pathD = string.Empty;

            // path D will vary based on location in megagon
            MegaLocation locationInMegagon = hex.MegaLocation;

            GridPoint[] p;

            switch (locationInMegagon)
            {
                case MegaLocation.A:
                    p = new[] { hex.Points[0], hex.Points[1], hex.Points[2], hex.Points[3] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.B:
                    p = new[] { hex.Points[5], hex.Points[0], hex.Points[1], hex.Points[2] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.C:
                    p = new[] { hex.Points[4], hex.Points[5], hex.Points[0], hex.Points[1] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.D:
                    p = new[] { hex.Points[3], hex.Points[4], hex.Points[5], hex.Points[0] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.E:
                    p = new[] { hex.Points[2], hex.Points[3], hex.Points[4], hex.Points[5] };
                    pathD = GetPathD(p);
                    break;

                case MegaLocation.F:
                    p = new[] { hex.Points[1], hex.Points[2], hex.Points[3], hex.Points[4] };
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

            if (points.Length > 1)
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

        #endregion

        #region PathDs for Maps

        // remove megagon lines from hexes where the opposite neighbor is null

        public static string GetFlatPathD(ICollection<Hexagon> hexagons, Hexagon hex)
        {
            try
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

                GridPoint[] B = Array.Empty<GridPoint>();
                GridPoint[] A = Array.Empty<GridPoint>();
                GridPoint[] F = Array.Empty<GridPoint>();
                GridPoint[] E = Array.Empty<GridPoint>();
                GridPoint[] D = Array.Empty<GridPoint>();
                GridPoint[] C = Array.Empty<GridPoint>();

                var radius = Math.Round(GridPoint.GetDistance(hex.Points[0], hex.Points[1]));
                StringBuilder sb = new StringBuilder();

                switch (locationInMegagon)
                {
                    case MegaLocation.A:
                        if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                            if (neighbors[(int)MegaLocation.A - 1] != null)
                            {
                                A = new[] { hex.Points[0], hex.Points[1] };
                                sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                                if (neighbors[(int)MegaLocation.F - 1] != null)
                                {
                                    F = new[] { hex.Points[1], hex.Points[2] };
                                    sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                            if (neighbors[(int)MegaLocation.F - 1] != null)
                            {
                                F = new[] { hex.Points[1], hex.Points[2] };
                                sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.B:
                        if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                            if (neighbors[(int)MegaLocation.B - 1] != null)
                            {
                                B = new[] { hex.Points[5], hex.Points[0] };
                                sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                                if (neighbors[(int)MegaLocation.A - 1] != null)
                                {
                                    A = new[] { hex.Points[0], hex.Points[1] };
                                    sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                            if (neighbors[(int)MegaLocation.A - 1] != null)
                            {
                                A = new[] { hex.Points[0], hex.Points[1] };
                                sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.C:
                        if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                            if (neighbors[(int)MegaLocation.C - 1] != null)
                            {
                                C = new[] { hex.Points[4], hex.Points[5] };
                                sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                                if (neighbors[(int)MegaLocation.B - 1] != null)
                                {
                                    B = new[] { hex.Points[5], hex.Points[0] };
                                    sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                            if (neighbors[(int)MegaLocation.B - 1] != null)
                            {
                                B = new[] { hex.Points[5], hex.Points[0] };
                                sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.D:
                        if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                            if (neighbors[(int)MegaLocation.D - 1] != null)
                            {
                                D = new[] { hex.Points[3], hex.Points[4] };
                                sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                                if (neighbors[(int)MegaLocation.C - 1] != null)
                                {
                                    C = new[] { hex.Points[4], hex.Points[5] };
                                    sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                            if (neighbors[(int)MegaLocation.C - 1] != null)
                            {
                                C = new[] { hex.Points[4], hex.Points[5] };
                                sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.E:
                        if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                            if (neighbors[(int)MegaLocation.E - 1] != null)
                            {
                                E = new[] { hex.Points[2], hex.Points[3] };
                                sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                                if (neighbors[(int)MegaLocation.D - 1] != null)
                                {
                                    D = new[] { hex.Points[3], hex.Points[4] };
                                    sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                            if (neighbors[(int)MegaLocation.D - 1] != null)
                            {
                                D = new[] { hex.Points[3], hex.Points[4] };
                                sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.F:
                        if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                            if (neighbors[(int)MegaLocation.F - 1] != null)
                            {
                                F = new[] { hex.Points[1], hex.Points[2] };
                                sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                                if (neighbors[(int)MegaLocation.E - 1] != null)
                                {
                                    E = new[] { hex.Points[2], hex.Points[3] };
                                    sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                            if (neighbors[(int)MegaLocation.E - 1] != null)
                            {
                                E = new[] { hex.Points[2], hex.Points[3] };
                                sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    default:
                        break; // for X and N there is no path drawn
                }

                return pathD;
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public static string GetPointyPathD(ICollection<Hexagon> hexagons, Hexagon hex)
        {
            try
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

                GridPoint[] B = Array.Empty<GridPoint>();
                GridPoint[] A = Array.Empty<GridPoint>();
                GridPoint[] F = Array.Empty<GridPoint>();
                GridPoint[] E = Array.Empty<GridPoint>();
                GridPoint[] D = Array.Empty<GridPoint>();
                GridPoint[] C = Array.Empty<GridPoint>();

                var radius = Math.Round(GridPoint.GetDistance(hex.Points[0], hex.Points[1]));
                StringBuilder sb = new StringBuilder();

                switch (locationInMegagon)
                {
                    case MegaLocation.A:
                        if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                            if (neighbors[(int)MegaLocation.A - 1] != null)
                            {
                                A = new[] { hex.Points[0], hex.Points[1] };
                                sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                                if (neighbors[(int)MegaLocation.F - 1] != null)
                                {
                                    F = new[] { hex.Points[1], hex.Points[2] };
                                    sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                            if (neighbors[(int)MegaLocation.F - 1] != null)
                            {
                                F = new[] { hex.Points[1], hex.Points[2] };
                                sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.B:
                        if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                            if (neighbors[(int)MegaLocation.B - 1] != null)
                            {
                                B = new[] { hex.Points[5], hex.Points[0] };
                                sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                                if (neighbors[(int)MegaLocation.A - 1] != null)
                                {
                                    A = new[] { hex.Points[0], hex.Points[1] };
                                    sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));

                            if (neighbors[(int)MegaLocation.A - 1] != null)
                            {
                                A = new[] { hex.Points[0], hex.Points[1] };
                                sb.Append(string.Format("L{0},{1} ", A[0].X, A[0].Y));
                                sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.C:
                        if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                            if (neighbors[(int)MegaLocation.C - 1] != null)
                            {
                                C = new[] { hex.Points[4], hex.Points[5] };
                                sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                                if (neighbors[(int)MegaLocation.B - 1] != null)
                                {
                                    B = new[] { hex.Points[5], hex.Points[0] };
                                    sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));

                            if (neighbors[(int)MegaLocation.B - 1] != null)
                            {
                                B = new[] { hex.Points[5], hex.Points[0] };
                                sb.Append(string.Format("L{0},{1} ", B[0].X, B[0].Y));
                                sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.B - 1] != null)
                        {
                            B = new[] { hex.Points[5], hex.Points[0] };
                            sb.Append(string.Format("M{0},{1} ", B[0].X, B[0].Y));
                            sb.Append(string.Format("L{0},{1} ", B[1].X, B[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.D:
                        if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                            if (neighbors[(int)MegaLocation.D - 1] != null)
                            {
                                D = new[] { hex.Points[3], hex.Points[4] };
                                sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                                if (neighbors[(int)MegaLocation.C - 1] != null)
                                {
                                    C = new[] { hex.Points[4], hex.Points[5] };
                                    sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));

                            if (neighbors[(int)MegaLocation.C - 1] != null)
                            {
                                C = new[] { hex.Points[4], hex.Points[5] };
                                sb.Append(string.Format("L{0},{1} ", C[0].X, C[0].Y));
                                sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.C - 1] != null)
                        {
                            C = new[] { hex.Points[4], hex.Points[5] };
                            sb.Append(string.Format("M{0},{1} ", C[0].X, C[0].Y));
                            sb.Append(string.Format("L{0},{1} ", C[1].X, C[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.E:
                        if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                            if (neighbors[(int)MegaLocation.E - 1] != null)
                            {
                                E = new[] { hex.Points[2], hex.Points[3] };
                                sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                                if (neighbors[(int)MegaLocation.D - 1] != null)
                                {
                                    D = new[] { hex.Points[3], hex.Points[4] };
                                    sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));

                            if (neighbors[(int)MegaLocation.D - 1] != null)
                            {
                                D = new[] { hex.Points[3], hex.Points[4] };
                                sb.Append(string.Format("L{0},{1} ", D[0].X, D[0].Y));
                                sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.D - 1] != null)
                        {
                            D = new[] { hex.Points[3], hex.Points[4] };
                            sb.Append(string.Format("M{0},{1} ", D[0].X, D[0].Y));
                            sb.Append(string.Format("L{0},{1} ", D[1].X, D[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    case MegaLocation.F:
                        if (neighbors[(int)MegaLocation.A - 1] != null)
                        {
                            A = new[] { hex.Points[0], hex.Points[1] };
                            sb.Append(string.Format("M{0},{1} ", A[0].X, A[0].Y));
                            sb.Append(string.Format("L{0},{1} ", A[1].X, A[1].Y));

                            if (neighbors[(int)MegaLocation.F - 1] != null)
                            {
                                F = new[] { hex.Points[1], hex.Points[2] };
                                sb.Append(string.Format("L{0},{1} ", F[0].X, F[0].Y));
                                sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                                if (neighbors[(int)MegaLocation.E - 1] != null)
                                {
                                    E = new[] { hex.Points[2], hex.Points[3] };
                                    sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                    sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                                }
                            }
                        }
                        else if (neighbors[(int)MegaLocation.F - 1] != null)
                        {
                            F = new[] { hex.Points[1], hex.Points[2] };
                            sb.Append(string.Format("M{0},{1} ", F[0].X, F[0].Y));
                            sb.Append(string.Format("L{0},{1} ", F[1].X, F[1].Y));

                            if (neighbors[(int)MegaLocation.E - 1] != null)
                            {
                                E = new[] { hex.Points[2], hex.Points[3] };
                                sb.Append(string.Format("L{0},{1} ", E[0].X, E[0].Y));
                                sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                            }
                        }
                        else if (neighbors[(int)MegaLocation.E - 1] != null)
                        {
                            E = new[] { hex.Points[2], hex.Points[3] };
                            sb.Append(string.Format("M{0},{1} ", E[0].X, E[0].Y));
                            sb.Append(string.Format("L{0},{1} ", E[1].X, E[1].Y));
                        }

                        pathD = sb.ToString();
                        break;

                    default:
                        break; // for X and N there is no path drawn
                }

                return pathD;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        #endregion

        #endregion


        // edges stuff



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

        public static bool GetEdgeIsMegaLine(GridEdge edge)
        {
            bool isMegaLine = false;

            if(edge.Hexagons != null && edge.Hexagons.Count == 2)
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

        public static string GetPathD(GridEdge edge)
        {
            return string.Format("M{0},{1} L{2},{3} ", edge.PointA.X, edge.PointA.Y, edge.PointB.X, edge.PointB.Y);
        }

    }

}
