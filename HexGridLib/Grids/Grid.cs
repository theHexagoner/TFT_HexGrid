using System;
using System.Collections.Generic;
using System.Linq;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;

namespace HexGridLib.Grids
{


    //public enum HexagonStyle
    //{
    //    Flat = 0,
    //    Pointy = 1
    //}

    //public enum MegagonSkew
    //{
    //    Left = 0,
    //    Right = 1
    //}

    //public enum OffsetPush
    //{
    //    Even = 0,
    //    Odd = 1
    //}

    //public struct OffsetSchema
    //{
    //    public readonly HexagonStyle Style;
    //    public readonly OffsetPush Offset;
    //    public readonly MegagonSkew Skew;

    //    public OffsetSchema(HexagonStyle style, OffsetPush offset, MegagonSkew skew)
    //    {
    //        Style = style;
    //        Offset = offset;
    //        Skew = skew;
    //    }

    //    public OffsetSchema(bool isPointy, bool isOdd, bool isRight)
    //    {
    //        Style = isPointy ? HexagonStyle.Pointy : HexagonStyle.Flat;
    //        Offset = isOdd ? OffsetPush.Odd : OffsetPush.Even;
    //        Skew = isRight ? MegagonSkew.Right : MegagonSkew.Left;
    //    }

    //    #region Conversions

    //    private static readonly int EVEN = 1;
    //    private static readonly int ODD = -1;

    //    #region Offset to Cubic Coords

    //    internal CubicCoordinate GetCubicCoords(OffsetCoordinate hex)
    //    {
    //        return Style switch
    //        {
    //            HexagonStyle.Flat => OffsetToCubeQ(Offset == OffsetPush.Even ? EVEN : ODD, hex),
    //            HexagonStyle.Pointy => OffsetToCubeR(Offset == OffsetPush.Even ? EVEN : ODD, hex),
    //            _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", Style))
    //        };
    //    }

    //    private static CubicCoordinate OffsetToCubeQ(int push, OffsetCoordinate h)
    //    {
    //        int x = h.Col;
    //        int y = h.Row - (h.Col + push * (h.Col & 1)) / 2;
    //        int z = -x - y;
    //        return new Cube(x, y, z);
    //    }

    //    private static CubicCoordinate OffsetToCubeR(int push, OffsetCoordinate h)
    //    {
    //        int x = h.Col - (h.Row + push * (h.Row & 1)) / 2;
    //        int y = h.Row;
    //        int z = -x - y;
    //        return new Cube(x, y, z);
    //    }

    //    #endregion

    //    #region Cubic to Offset Coords

    //    /// <summary>
    //    /// convert a cube to offset (row, column) coordinates
    //    /// </summary>
    //    /// <param name="hex">the hex for which you want the offset coordinates</param>
    //    /// <returns>Offset</returns>
    //    internal Offset GetOffsetCoords(Cube hex)
    //    {
    //        return Style switch
    //        {
    //            HexagonStyle.Flat => GetOffsetQ(Offset == OffsetPush.Even ? EVEN : ODD, hex),
    //            HexagonStyle.Pointy => GetOffsetR(Offset == OffsetPush.Even ? EVEN : ODD, hex),
    //            _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", Style))
    //        };
    //    }

    //    private static Offset GetOffsetQ(int push, Cube hex)
    //    {
    //        int col = hex.X;
    //        int row = hex.Y + ((hex.X + push * (hex.X & 1)) / 2);
    //        return new Offset(row, col);
    //    }

    //    private static Offset GetOffsetR(int push, Cube hex)
    //    {
    //        int col = hex.X + (hex.Y + push * (hex.Y & 1)) / 2;
    //        int row = hex.Y;
    //        return new Offset(col, row);
    //    }

    //    #endregion

    //    #endregion

    //}

    public class Grid : IGrid
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
        private Grid() { }

        /// <summary>
        /// constructor for Grid, passing all configuration options
        /// </summary>
        /// <param name="rows">the number of rows in the grid</param>
        /// <param name="cols">the number of columns in the grid</param>
        /// <param name="radius">distance from the center of a hexagon, in px, to any of its corner points</param>
        /// <param name="origin">sets the cartesian coordinates for the center of cube a location 0,0,0</param>
        /// <param name="schema">determines orientation and offsets of hexes and megahexes</param>
        public Grid(int rows, int cols, GridPoint radius, GridPoint origin, OffsetSchema schema)
        {
            Rows = rows;
            Cols = cols;
            OffsetSchema = schema;

            Layout = new HexLayout(OffsetSchema.Style, radius, origin);
            Hexagons = new Dictionary<int, IHexagon>();
            Edges = new Dictionary<int, IEdge>();

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
                    var offsetCoord = new OffsetCoordinate(r, c);
                    var cubicCoord = OffsetSchema.GetCubicCoords(offsetCoord);

                    var hex = new Hexagon(this, cubicCoord, offsetCoord);
                    var hash = hex.ID;
                    Hexagons.Add(hash, hex);

                    if (GetIsOutOfBounds(Rows, Cols, hex.OffsetLocation))
                        Overscan.Add(hex.ID);
                }
            }

            // set the megagon location of each hexagon
            MegagonLocationSetter.SetMegaLocations(OffsetSchema, Hexagons.Values.ToArray());

            // TRIM hexagons outside the requested offset limits for the grid
            Overscan.ForEach(id =>
            {
                foreach (IEdge edge in Hexagons[id].Edges)
                {
                    if (edge.Hexagons.ContainsKey(id))
                        edge.Hexagons.Remove(id);

                    if (!edge.Hexagons.Any() && Edges.ContainsKey(edge.ID))
                    {
                        Edges.Remove(edge.ID);
                    }
                }

                if(Hexagons.ContainsKey(id))
                    Hexagons.Remove(id);
            });
        }

        #region Layout

        /// <summary>
        /// which permutation of a rectangular offset coordinate scheme this grid uses
        /// </summary>
        private OffsetSchema OffsetSchema { get; }

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

        #endregion

        #region Hexes

        public IDictionary<int, IHexagon> Hexagons { get; }
        public IDictionary<int, IEdge> Edges { get; }

        private static bool GetIsOutOfBounds(int rows, int cols, OffsetCoordinate offsetLocation)
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

        internal int GetHashcodeForCube(CubicCoordinate cube)
        {
            return HashCode.Combine(OffsetSchema.Style, OffsetSchema.Offset, OffsetSchema.Skew, Rows, Cols, cube.GetHashCode());
        }

        internal GridPoint[] GetHexCornerPoints(CubicCoordinate hex, double factor = 1)
        {
            return Layout.GetHexCornerPoints(hex, factor);
        }

        #endregion

        #region Map

        public IMap InitMap()
        {
            return new Maps.Map(this);
        }

        #endregion

    }





}
