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

            ID = grid.GetUniqueId(cubicCoords);
            Points = grid.GetHexCornerPoints(cubicCoords);
            SetEdgesFromPoints();

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

        public int ID { get; }

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
        public GridPoint[] Points { get; }

        /// <summary>
        /// cubic coordinates of the hexagon within the grid
        /// X, Y and Z dimensions to represent 2d grid as 3d matrix
        /// used to calculate most things like distance to other hexes, line of sight, etc.
        /// </summary>        
        public CubicCoordinate CubicLocation { get; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        public OffsetCoordinate OffsetLocation { get; }

        public IEdge[] Edges { get; private set; }

        /// <summary>
        /// the Edges property is derived from the Points array
        /// </summary>
        private void SetEdgesFromPoints()
        {
            IEdge[] edges = new Edge[6];

            for (int i = 0; i < 5; i++)
            {
                edges[i] = new Edge(Points[i], Points[i + 1]);
            }

            edges[5] = new Edge(Points[5], Points[0]);

            Edges = edges;
        }

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


    }

}
