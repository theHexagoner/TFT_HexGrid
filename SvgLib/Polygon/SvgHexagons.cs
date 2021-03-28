using System.Linq;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using SvgLib.Paths;

namespace SvgLib.Polygons
{
    /// <summary>
    /// encapsulate information useful for drawing a hexagon as SVG
    /// </summary>
    public class SvgHexagon : ISvgHexagon
    {
        public int ID { get; }
        public string Points { get; }
        public string CenterD { get; }

        public int Row { get; }
        public int Col { get; }

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
            GridPoint midPoint = new((points[3].X + points[0].X) / 2, (points[3].Y + points[0].Y) / 2);
            double outerRadius = points[0].GetDistanceTo(points[1]) / 64;

            CenterD = SvgPathDFactory.Instance.GetPathD(SvgPathDFactory.Type.Star, midPoint, outerRadius);
        }

        public bool IsSelected { get; private set; }

        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

    }
}