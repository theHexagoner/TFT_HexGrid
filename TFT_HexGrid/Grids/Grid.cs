using System;
using System.Collections.Generic;
using System.Linq;
using TFT_HexGrid.Coordinates;
using TFT_HexGrid.Maps;
using TFT_HexGrid.SvgHelpers;

namespace TFT_HexGrid.Grids
{
    /// <summary>
    /// enumeration describing the four different ways to organize a hex grid into rectangular pattern of offset coordinates
    /// </summary>
    public enum OffsetScheme
    {
        Even_Q, // flat-top hexes, shoves even columns down
        Odd_Q,  // flat-too hexes, shoves odd columns down
        Odd_R,  // pointy-top hexes, shoves odd rows towards right
        Even_R  // pointy-top hexes, shoves even rows towards right
    }

    public enum HexagonStyle
    {
        Flat = 0,
        Pointy = 1
    }

    public enum MegagonSkew
    {
        Left = 0,
        Right = 1
    }

    public enum OffsetPush
    {
        Even = 0,
        Odd = 1
    }

    public struct OffsetSchema
    {
        public readonly HexagonStyle Style;
        public readonly OffsetPush Offset;
        public readonly MegagonSkew Skew;

        public OffsetSchema(HexagonStyle style, OffsetPush offset, MegagonSkew skew)
        {
            Style = style;
            Offset = offset;
            Skew = skew;
        }

        public OffsetSchema(bool isPointy, bool isOdd, bool isRight)
        {
            Style = isPointy ? HexagonStyle.Pointy : HexagonStyle.Flat;
            Offset = isOdd ? OffsetPush.Odd : OffsetPush.Even;
            Skew = isRight ? MegagonSkew.Right : MegagonSkew.Left;
        }

    }

    public class Grid
    {
        // notes:   Hexagonal grids can be thought of as two-dimensional representation of a three-dimensional (cubic) coordinate system.
        //          Each hexagon on our grid can then be thought of as a cube that exists in a three dimensional matrix.
        //          Under the covers, the hexagon at the origin of our grid is really the cube that is located at coordinates 0, 0, 0.
        //
        //          In theory, the grid could extend to int.MaxValue in each direction along the three dimensions.
        //          In practice, the visible area of the grid will be limited to some practical size defined by screen or print resolution and size.
        //          
        //          There are a variety of layouts and two-dimensional coordinate systems that can be used to define the practical limits of
        //          a hexagonal grid. Since video screens and printed materials are generally rectangular, this API uses the familiar concepts 
        //          of rows and columns of hexagons within a rectangular layout to define the extents of the grid. There are four ways to do
        //          this; see the OffsetScheme enum.

        /// <summary>
        /// there is no default constructor
        /// </summary>
        private Grid() {}

        /// <summary>
        /// if you pass no OffsetSchems, it will default to using flat hexes, even offset, and right megahex skew
        /// </summary>
        /// <param name="rows">the number of rows in the grid</param>
        /// <param name="cols">the number of columns in the grid</param>
        /// <param name="size">controls size of hexs, using a point allows for "squishy" hexes</param>
        /// <param name="origin">sets the coordinates for the center of hex 0,0,0</param>
        public Grid(int rows, int cols, GridPoint size, GridPoint origin) : this(rows, cols, size, origin, new OffsetSchema()) {}

        /// <summary>
        /// constructor for Grid, passing all configuration options
        /// </summary>
        /// <param name="rows">the number of rows in the grid</param>
        /// <param name="cols">the number of columns in the grid</param>
        /// <param name="size">controls size of hexs, using a point allows for "squishy" hexes</param>
        /// <param name="origin">sets the coordinates for the center of hex 0,0,0</param>
        /// <param name="schema">determines orientation and offsets of hexes and megahexes</param>
        public Grid(int rows, int cols, GridPoint size, GridPoint origin, OffsetSchema schema)
        {
            Rows = rows;
            Cols = cols;

            switch (schema.Offset)
            {
                case OffsetPush.Even:
                    OffsetScheme = schema.Style == HexagonStyle.Flat ? OffsetScheme.Even_Q : OffsetScheme.Even_R;
                    break;
                case OffsetPush.Odd:
                    OffsetScheme = schema.Style == HexagonStyle.Flat ? OffsetScheme.Odd_Q : OffsetScheme.Odd_R;
                    break;
            }

            HexGeometry geometry = OffsetScheme == OffsetScheme.Even_Q || OffsetScheme == OffsetScheme.Odd_Q ?
                new HexGeometry(1.5d, 0d, Math.Sqrt(3d) / 2d, Math.Sqrt(3d), 2d / 3d, 0d, -1d / 3d, Math.Sqrt(3d) / 3d, 0d) : // flat
                new HexGeometry(Math.Sqrt(3d), Math.Sqrt(3d) / 2d, 0d, 1.5d, Math.Sqrt(3d) / 3d, -1d / 3d, 0d, 2d / 3d, 0.5d); // pointy

            Layout = new HexLayout(geometry, size, origin);
            Hexagons = new Dictionary<int, Hexagon>();
            SvgHexagons = new Dictionary<int, SvgHexagon>();
            SvgMegagons = new Dictionary<int, SvgMegagon>();
            Edges = new Dictionary<int, GridEdge>();

            var halfRows = (int)Math.Floor(rows / 2d);
            var splitRows = halfRows - rows;
            var halfCols = (int)Math.Floor(cols / 2d);
            var splitCols = halfCols - cols;

            // iterate over requested colums and rows and create hexes, adding them to hash table
            // create some "overscan" to facilitate getting partial megagons
            Overscan = new List<int>();

            for (int r = splitRows - 1; r < halfRows + 2; r++)
            {
                for (int c = splitCols - 1; c < halfCols + 2; c++)
                {
                    var hex = new Hexagon(this, new Offset(r, c));
                    var hash = hex.ID;
                    Hexagons.Add(hash, hex);

                    if (GetIsOutOfBounds(Rows, Cols, hex.OffsetLocation))
                        Overscan.Add(hex.ID);
                }
            }

            // get the megagon location of each hexagon
            SvgMegagonsFactory.Instance.SetMegaLocations(OffsetScheme, Hexagons.Values.ToArray());

            // get the SVG data for each hexagon
            foreach (Hexagon h in Hexagons.Values)
            {
                SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.OffsetLocation.Row, h.OffsetLocation.Col, h.Points));
            }

            // TRIM hexagons outside the requested offset limits for the grid
            Overscan.ForEach(id => {
                foreach (GridEdge edge in Hexagons[id].Edges)
                {
                    if (edge.Hexagons.ContainsKey(id))
                        edge.Hexagons.Remove(id);

                    if (edge.Hexagons.Count == 0 && Edges.ContainsKey(edge.ID))
                    {
                        Edges.Remove(edge.ID);
                    }
                }

                Hexagons.Remove(id);
                SvgHexagons.Remove(id);
            });

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



        #region Layout

        /// <summary>
        /// which version of the rectangular offset coordinate scheme this grid uses
        /// </summary>
        internal OffsetScheme OffsetScheme { get; private set; }

        private HexLayout Layout { get; set; }

        /// <summary>
        /// size of one dimension of the matrix of hexagons
        /// </summary>
        private int Rows { get; }

        /// <summary>
        /// size of the other dimension of the matrix of hexagons
        /// </summary>
        private int Cols { get; }

        private List<int> Overscan { get; set; }

        private struct HexLayout
        {
            public readonly HexGeometry Geometry;
            public readonly GridPoint Size; // point allows for varying "squishyness" of rendered view
            public readonly GridPoint Origin; // the origin of the grid?

            public HexLayout(HexGeometry geometry, GridPoint size, GridPoint origin)
            {
                Geometry = geometry;
                Size = size;
                Origin = origin;
            }

            /// <summary>
            /// get the center point of a given cubic coordinate
            /// </summary>
            /// <param name="hex">the cubic coordinate to check</param>
            /// <returns>Point</returns>
            public GridPoint CubeToPoint(Cube hex)
            {
                HexGeometry M = Geometry;
                double x = (M.F0 * hex.X + M.F1 * hex.Y) * Size.X;
                double y = (M.F2 * hex.X + M.F3 * hex.Y) * Size.Y;
                return new GridPoint(x + Origin.X, y + Origin.Y);
            }

            /// <summary>
            /// given a point (e.g. screen coordinates) get the floating-point cubic coordinates
            /// </summary>
            /// <param name="p">the point to convert</param>
            /// <returns>CubeF</returns>
            public CubeF PointToCubeF(GridPoint p)
            {
                HexGeometry M = Geometry;
                GridPoint pt = new GridPoint((p.X - Origin.X) / Size.X, (p.Y - Origin.Y) / Size.Y);

                double x = M.B0 * pt.X + M.B1 * pt.Y;
                double y = M.B2 * pt.X + M.B3 * pt.Y;

                return new CubeF(x, y, -x - y);
            }

            /// <summary>
            /// the points that define the six corners of the hexagon
            /// </summary>
            /// <param name="hex">Cubic coordinated for the hex</param>
            /// <returns>List of Point structures</returns>
            public GridPoint[] GetHexCornerPoints(Cube hex, double factor = 1)
            {
                GridPoint[] corners = new GridPoint[6];
                GridPoint center = CubeToPoint(hex);

                for (int i = 0; i < 6; i++)
                {
                    GridPoint offset = HexCornerOffset(i);
                    corners[i] = (GridPoint.GetRound(new GridPoint(center.X + offset.X * factor, center.Y + offset.Y * factor), 3));
                }

                return corners;
            }

            /// <summary>
            /// calculates the offset of the hex corner relative to the hex center
            /// </summary>
            /// <param name="corner">for which corner of the hexagon to calculate the offset</param>
            /// <returns>Point</returns>
            private GridPoint HexCornerOffset(int corner)
            {
                double angle = 2.0 * Math.PI * (Geometry.StartAngle - corner) / 6.0;
                return new GridPoint(Size.X * Math.Cos(angle), Size.Y * Math.Sin(angle));
            }

        }

        /// <summary>
        /// structure for storing the forward and inverse matrices used to calculate hex-to-pixel and pixel-to-hex
        /// as well as the starting angle for drawing the hex corners on screen
        /// </summary>
        private struct HexGeometry
        {
            public readonly double F0;
            public readonly double F1;
            public readonly double F2;
            public readonly double F3;
            public readonly double B0;
            public readonly double B1;
            public readonly double B2;
            public readonly double B3;
            public readonly double StartAngle;

            public HexGeometry(double f0, double f1, double f2, double f3, // forward matrix
                               double b0, double b1, double b2, double b3, // inverse of forward matrix
                               double startAngle)
            {
                F0 = f0;
                F1 = f1;
                F2 = f2;
                F3 = f3;
                B0 = b0;
                B1 = b1;
                B2 = b2;
                B3 = b3;
                StartAngle = startAngle;
            }
        }

        #endregion

        #region Hexes

        internal Dictionary<int, Hexagon> Hexagons { get; }

        public Dictionary<int, SvgHexagon> SvgHexagons { get; }

        public SvgHexagon GetHexAt(GridPoint point)
        {
            // turn the point into a CubeF
            var cubeF = Layout.PointToCubeF(point);

            // round the CubeF to get the Cubic coordinates
            var cube = cubeF.Round();

            // get the Hexagon from the hash of the cubic coordinates
            SvgHexagons.TryGetValue(GetHashcodeForCube(cube), out SvgHexagon hex);

            if (hex != null)
                return hex;

            return null;
        }

        private static bool GetIsOutOfBounds(int rows, int cols, Offset offsetLocation)
        {
            var halfRows = (int)Math.Floor(rows / 2d);
            var splitRows = halfRows - rows;
            var halfCols = (int)Math.Floor(cols / 2d);
            var splitCols = halfCols - cols;

            return offsetLocation.Row < splitRows + 1 ||
                    offsetLocation.Row > halfRows ||
                    offsetLocation.Col < splitCols + 1 ||
                    offsetLocation.Col > halfCols;
        }

        internal int GetHashcodeForCube(Cube cube)
        {
            return HashCode.Combine(OffsetScheme, Rows, Cols, cube.GetHashCode());
        }

        internal GridPoint[] GetHexCornerPoints(Cube hex, double factor = 1)
        {
            return Layout.GetHexCornerPoints(hex, factor);
        }

        #endregion

        #region Megas

        public Dictionary<int, SvgMegagon> SvgMegagons { get; }

        internal Dictionary<int, GridEdge> Edges { get; }

        #endregion

        #region Maps

        public Map InitMap()
        {
            var map = new Map(this);
            return map;
        }

        #endregion

    }





}
