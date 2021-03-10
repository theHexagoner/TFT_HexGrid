using System.Linq;
using TFT_HexGrid.Coordinates;

namespace TFT_HexGrid.SvgHelpers
{
    /// <summary>
    /// encapsulate information useful for drawing a hexagon as SVG
    /// </summary>
    public class SvgHexagon
    {
        public readonly int ID;
        public readonly string Points;
        public readonly string StarD;

        public readonly int Row;
        public readonly int Col;

        /// <summary>
        /// constructor, if you don't specify IsSelected it will default to true
        /// </summary>
        /// <param name="id">Supply a unique ID for SvgHexagon</param>
        /// <param name="points">The points from which to derive the SVG</param>
        public SvgHexagon(int id, int row, int col, GridPoint[] points) : this(id, row, col, points, true) { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">Supply a unique ID for SvgHexagon</param>
        /// <param name="points">The points from which to derive the SVG</param>
        /// <param name="isSelected">Flag indicating how the client might fill the polygon</param>
        public SvgHexagon(int id, int row, int col, GridPoint[] points, bool isSelected)
        {
            ID = id;
            Row = row;
            Col = col;

            Points = string.Join(" ", points.Select(p => string.Format("{0},{1}", p.X, p.Y)));
            IsSelected = isSelected;

            // figure out where and how big to draw the star:
            GridPoint midPoint = new GridPoint((points[3].X + points[0].X) / 2, (points[3].Y + points[0].Y) / 2);
            double outerRadius = points[0].GetDistanceTo(points[1]) / 64;

            StarD = SvgPathDFactory.Instance.GetPathD(SvgPathDFactory.Type.Star, midPoint, outerRadius);
        }

        public bool IsSelected { get; set; }

    }
}