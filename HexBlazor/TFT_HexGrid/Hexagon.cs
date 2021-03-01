using TFT_HexGrid.SvgHelpers;

namespace TFT_HexGrid.Grids
{
    public class Hexagon
    {
        // no default constructor
        private Hexagon() { }

        /// <summary>
        /// construct a Hexagon by passing the grid and offset coordinates
        /// </summary>
        /// <param name="grid">the parent grid that contains this hexagon</param>
        /// <param name="coords">the offset coordinates of the hexagon</param>
        public Hexagon(Grid grid, Offset coords) : this(grid, Cube.GetCubeFromOffset(grid.OffsetScheme, coords), coords) { }

        /// <summary>
        /// construct a hexagon by passing the grid and cubic coordinates
        /// </summary>
        /// <param name="grid">the parent grid that contains this hex</param>
        /// <param name="coords">the cubic coordinates of the hexagon</param>
        public Hexagon(Grid grid, Cube coords) : this(grid, coords, Offset.GetOffset(grid.OffsetScheme, coords)) {}

        private Hexagon(Grid grid, Cube cubeCoords, Offset offsetCoords)
        {
            OffsetLocation = offsetCoords;
            CubicLocation = cubeCoords;
            ID = grid.GetHashcodeForCube(cubeCoords);
            Points = grid.GetHexCornerPoints(cubeCoords);
            MegaLocation = MegaLocation.N;
            PathD = string.Empty;
        }

        public int ID { get; private set; }

        /// <summary>
        /// cubic coordinates of the hexagon within the grid
        /// X, Y and Z dimensions to represent 2d grid as 3d matrix
        /// used to calculate most things like distance to other hexes, line of sight, etc.
        /// </summary>        
        public Cube CubicLocation { get; private set; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        public Offset OffsetLocation { get; private set; }

        /// <summary>
        /// the points that define the six corners of the hexagon
        /// </summary>
        public GridPoint[] Points { get; private set; }

        /// <summary>
        /// location of hex within its associated megagon
        /// </summary>
        public MegaLocation MegaLocation { get; set; }

        /// <summary>
        /// the SVG path data to draw this hexagon on the map or grid
        /// </summary>
        public string PathD { get; set; }

    }

}
