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
        public readonly string StarD;

        public SvgHexagon(int id, GridPoint[] points) : this(id, points, true) { }

        public SvgHexagon(int id, GridPoint[] points, bool isSelected)
        {
            Id = id;
            Points = string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
            IsSelected = isSelected;

            // figure out where and how big to draw the star:
            GridPoint midPoint = new GridPoint((points[3].X + points[0].X) / 2, (points[3].Y + points[0].Y) / 2);

            double outerRadius = points[0].GetDistanceTo(points[1]) / 64;
            double innerRadius = outerRadius / 3;

            StarD = SvgStar.GetPathD(midPoint, outerRadius, innerRadius);
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
    /// helper functions to generate the SVG path information for Megagons
    /// </summary>
    public static class SvgMegagonsFactory
    {
        /// <summary>
        /// sets the location within the "parent" megahex for each of a set of hexagons, depending on offset scheme
        /// </summary>
        /// <param name="offsetScheme">Offset scheme used to lay out the grid</param>
        /// <param name="hexagons">Array of hexagon objects, generally this would be all the hexes in a grid</param>
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

        /// <summary>
        /// finds all the center hexagons for EvenQ schemes, and for each calls SetMegaLocations(Hexagon, Hexagon[])
        /// </summary>
        /// <param name="hexagons">Array of hexagon objects, generally this would be all the hexes in a grid</param>
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

        /// <summary>
        /// finds all the center hexagons for OddQ schemes, and for each calls SetMegaLocations(Hexagon, Hexagon[])
        /// </summary>
        /// <param name="hexagons">Array of hexagon objects, generally this would be all the hexes in a grid</param>
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

        /// <summary>
        /// finds all the center hexagons for EvenR schemes, and for each calls SetMegaLocations(Hexagon, Hexagon[])
        /// </summary>
        /// <param name="hexagons">Array of hexagon objects, generally this would be all the hexes in a grid</param>
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

        /// <summary>
        /// finds all the center hexagons for OddR schemes, and for each calls SetMegaLocations(Hexagon, Hexagon[])
        /// </summary>
        /// <param name="hexagons">Array of hexagon objects, generally this would be all the hexes in a grid</param>
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

        /// <summary>
        /// sets the location within the "parent" megahex for each of a set of hexagons
        /// this is called by the various "WalkScheme" methods for each of the identified center hexagons
        /// </summary>
        /// <param name="h"></param>
        /// <param name="hexagons"></param>
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

        /// <summary>
        /// gets the SVG path information for an edge 
        /// </summary>
        /// <param name="edge">the edge from which to derive the SVG path data</param>
        /// <returns>System.String containing SVG Path d content</returns>
        public static string GetPathD(GridEdge edge)
        {
            return string.Format("M{0},{1} L{2},{3} ", edge.PointA.X, edge.PointA.Y, edge.PointB.X, edge.PointB.Y);
        }

    }

    public static class SvgStar
    {

        /// <summary>
        /// Return an array of 10 points to be used to create an SVG path d
        /// </summary>
        /// <param name="center"> The origin is the middle of the star</param>
        /// <param name="outerRadius">Radius of the surrounding circle</param>
        /// <param name="innerRadius">Radius of the circle for the "inner" points</param>
        /// <returns>Array of 10 GridPoint structures</returns>
        public static string GetPathD(GridPoint center, double outerRadius, double innerRadius)
        {
            // conversions to radians
            double Ang36 = Math.PI / 5.0;   // 36Â° x PI/180
            double Ang72 = 2.0 * Ang36;     // 72Â° x PI/180
            
            // some sine and cosine values we need
            double Sin36 = Math.Sin(Ang36);
            double Sin72 = Math.Sin(Ang72);
            double Cos36 = Math.Cos(Ang36);
            double Cos72 = Math.Cos(Ang72);

            // this star has 10 points
            GridPoint[] points = new GridPoint[10];

            // top off the star, or on a clock this is 0:00 hours
            points[0] = new GridPoint(center.X, center.Y - outerRadius);
            points[1] = new GridPoint(center.X + (innerRadius * Sin36), center.Y - (innerRadius * Cos36)); // 0:06 hours
            points[2] = new GridPoint(center.X + (outerRadius * Sin72), center.Y - (outerRadius * Cos72)); // 0:12 hours
            points[3] = new GridPoint(center.X + (innerRadius * Sin72), center.Y + (innerRadius * Cos72)); // 0:18
            points[4] = new GridPoint(center.X + (outerRadius * Sin36), center.Y + (outerRadius * Cos36)); // 0:24 
            points[5] = new GridPoint(center.X, center.Y + innerRadius);
            points[6] = new GridPoint(center.X - (outerRadius * Sin36), center.Y + (outerRadius * Cos36)); // 0:36 
            points[7] = new GridPoint(center.X - (innerRadius * Sin72), center.Y + (innerRadius * Cos72)); // 0:42
            points[8] = new GridPoint(center.X - (outerRadius * Sin72), center.Y - (outerRadius * Cos72)); // 0:48 
            points[9] = new GridPoint(center.X - (innerRadius * Sin36), center.Y - (innerRadius * Cos36)); // 0:54 hours

            return GetPathD(points);
        }

        private static string GetPathD(GridPoint[] points)
        {
            // use a string builder to build up the D for the path
            var sb = new StringBuilder();

            // move to the first point
            sb.Append(string.Format("M{0},{1} ", points[0].X, points[0].Y));

            // draw lines to the rest
            for (int i = 1; i < points.Length; i++)
            {
                sb.Append(string.Format("L{0},{1} ", points[i].X, points[i].Y));
            }

            sb.Append(" Z");

            return sb.ToString();
        }

    }
}
