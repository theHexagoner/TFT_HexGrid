using HexGridInterfaces.Structs;
using HexGridInterfaces.Grids;

namespace HexGridLib.Grids
{

    public class Hexagon : IHexagon
    {
        // no default constructor
        private Hexagon() { }

        internal Hexagon(Grid grid, CubicCoordinate cubicCoords, OffsetCoordinate offsetCoords)
        {
            OffsetLocation = offsetCoords;
            CubicLocation = cubicCoords;

            ID = grid.GetHashcodeForCube(cubicCoords);
            Points = grid.GetHexCornerPoints(cubicCoords);
            Edges = GetEdgesFromPoints(Points);

            foreach (Edge e in Edges)
            {
                if (grid.Edges.ContainsKey(e.ID))
                {
                    var edge = grid.Edges[e.ID];
                    edge.Hexagons.Add(ID, this);
                }
                else
                {
                    e.Hexagons.Add(ID, this);
                    grid.Edges.Add(e.ID, e);
                }
            }

            MegaLocation = MegaLocation.N;
        }

        public int ID { get; private set; }

        public int Row
        {
            get
            {
                return OffsetLocation.Row;
            }
        }

        public int Col
        {
            get
            {
                return OffsetLocation.Col;
            }
        }

        /// <summary>
        /// the points that define the six corners of the hexagon
        /// </summary>
        public GridPoint[] Points { get; private set; }

        /// <summary>
        /// cubic coordinates of the hexagon within the grid
        /// X, Y and Z dimensions to represent 2d grid as 3d matrix
        /// used to calculate most things like distance to other hexes, line of sight, etc.
        /// </summary>        
        public CubicCoordinate CubicLocation { get; private set; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        public OffsetCoordinate OffsetLocation { get; private set; }

        public IEdge[] Edges { get; private set; }

        /// <summary>
        /// location of hex within its associated megagon
        /// </summary>
        public MegaLocation MegaLocation { get; private set; }

        /// <summary>
        /// sets the location in its megagon
        /// </summary>
        /// <param name="locationInMegagon">the enumerated location within the megagon</param>
        public void SetLocationInMegagon(MegaLocation locationInMegagon)
        {
            MegaLocation = locationInMegagon;
        }

        /// <summary>
        /// iterate over a set of points and return an array of GridEdge objects
        /// </summary>
        /// <param name="points">Array of GridPoint objects, generally this would be the points belonging to a hexagon</param>
        /// <returns>Array of GridEdge objects</returns>
        private static IEdge[] GetEdgesFromPoints(GridPoint[] points)
        {
            IEdge[] edges = new Edge[6];

            for (int i = 0; i < 5; i++)
            {
                edges[i] = new Edge(points[i], points[i + 1]);
            }

            edges[5] = new Edge(points[5], points[0]);

            return edges;
        }

    }

}
