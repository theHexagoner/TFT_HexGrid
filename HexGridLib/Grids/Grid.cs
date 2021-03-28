using System;
using System.Collections.Generic;
using System.Linq;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;

// notes:   Hexagonal grids can be thought of as two-dimensional representation of a three-dimensional (cubic) coordinate system.
//          Each hexagon on our grid can then be thought of as a cube that exists in a three dimensional matrix.
//          Under the covers, the hexagon at the origin of our grid is really the cube that is located at coordinates 0, 0, 0.
//
//          In theory, the grid could extend to int.MaxValue in each direction along the three dimensions.
//          In practice, the visible area of the grid will be limited to some practical size defined by screen or print resolution and size.
//          
//          There are a variety of layouts and two-dimensional coordinate systems that can be used to define the practical limits of
//          a hexagonal grid. Since video screens and printed materials are generally rectangular, this API uses the familiar concepts 
//          of rows and columns of hexagons within a rectangular layout to define the extents of the grid. There are eight ways to do
//          this; see the OffsetScheme struct.

namespace HexGridLib.Grids
{
    public class Grid : IGrid
    {
        #region Construction

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
        internal Grid(int rows, int cols, GridPoint radius, GridPoint origin, OffsetSchema schema)
        {
            RowCount = rows;
            ColCount = cols;
            OffsetSchema = schema;
            Layout = new HexLayout(OffsetSchema.HexStyle, radius, origin);
            Edges = new Dictionary<int, IEdge>();
            Hexagons = GetHexagons();

            // set the megagon location of each hexagon
            MegagonLocationSetter.SetMegaLocations(OffsetSchema, Hexagons.Values.ToArray());

            // trim hexagons outside the requested offset limits for the grid
            TrimHexagons();
        }

        private Dictionary<int, IHexagon> GetHexagons()
        {
            var hexagons = new Dictionary<int, IHexagon>();

            // iterate over requested colums and rows and create hexes, adding them to hash table
            // create some "overscan" to facilitate getting partial megagons
            Overscan = new List<int>();

            for (int r = SplitRows - 1; r < HalfRows + 2; r++)
            {
                for (int c = SplitCols - 1; c < HalfCols + 2; c++)
                {
                    var offsetCoord = new OffsetCoordinate(r, c);
                    var cubicCoord = OffsetSchema.GetCubicCoords(offsetCoord);

                    var hex = new Hexagon(this, cubicCoord, offsetCoord);
                    var hash = hex.ID;
                    hexagons.Add(hash, hex);

                    if (GetIsOutOfBounds(hex.OffsetLocation))
                        Overscan.Add(hex.ID);
                }
            }

            return hexagons;
        }

        private void TrimHexagons()
        {
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

                if (Hexagons.ContainsKey(id))
                    Hexagons.Remove(id);
            });

            Overscan.Clear();
        }

        private bool GetIsOutOfBounds(OffsetCoordinate offsetLocation)
        {
            return offsetLocation.Row < SplitRows + 1 ||
                    offsetLocation.Row > HalfRows ||
                    offsetLocation.Col < SplitCols + 1 ||
                    offsetLocation.Col > HalfCols;
        }

        #endregion

        #region Layout

        /// <summary>
        /// which permutation of a rectangular offset coordinate scheme this grid uses
        /// </summary>
        private OffsetSchema OffsetSchema { get; }

        private HexLayout Layout { get; set; }

        /// <summary>
        /// size of one dimension of the matrix of hexagons
        /// </summary>
        private int RowCount { get; }

        /// <summary>
        /// size of the other dimension of the matrix of hexagons
        /// </summary>
        private int ColCount { get; }

        private int HalfRows => (int)Math.Floor(RowCount / 2d);
        private int SplitRows => HalfRows - RowCount;
        private int HalfCols => (int)Math.Floor(ColCount / 2d);
        private int SplitCols => HalfCols - ColCount;

        private List<int> Overscan { get; set; }

        #endregion

        #region Hexes

        public IDictionary<int, IHexagon> Hexagons { get; }
        public IDictionary<int, IEdge> Edges { get; }

        internal int GetUniqueId(CubicCoordinate cube)
        {
            return cube.GetUniqueID(OffsetSchema.HexStyle, OffsetSchema.OffsetPush, OffsetSchema.MegahexSkew, RowCount, ColCount);
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
