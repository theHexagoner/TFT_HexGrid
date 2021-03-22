using HexGridLib.Coordinates;
using HexGridLib.Grids;

namespace HexGridLib.Converters
{
    internal sealed class Converter
    {
        /// <summary>
        /// iterate over a set of points and return an array of GridEdge objects
        /// </summary>
        /// <param name="points">Array of GridPoint objects, generally this would be the points belonging to a hexagon</param>
        /// <returns>Array of GridEdge objects</returns>
        public static Edge[] GetEdgesFromPoints(GridPoint[] points)
        {
            Edge[] edges = new Edge[6];

            for (int i = 0; i < 5; i++)
            {
                edges[i] = new Edge(points[i], points[i + 1]);
            }

            edges[5] = new Edge(points[5], points[0]);

            return edges;
        }



    }
}
