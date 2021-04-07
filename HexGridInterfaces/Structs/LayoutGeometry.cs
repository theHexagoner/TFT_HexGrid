using System;

namespace HexGridInterfaces.Structs
{
    public struct HexLayout
    {
        private readonly HexGeometry _M;
        public readonly GridPoint Size; // point allows for varying "squishyness" of rendered view
        public readonly GridPoint Origin; // the origin of the grid?

        public HexLayout(HexagonStyle hexStyle, GridPoint size, GridPoint origin)
        {
            _M = hexStyle == HexagonStyle.Flat ?
                new HexGeometry(1.5d, 0d, Math.Sqrt(3d) / 2d, Math.Sqrt(3d), 2d / 3d, 0d, -1d / 3d, Math.Sqrt(3d) / 3d, 0d) : // flat
                new HexGeometry(Math.Sqrt(3d), Math.Sqrt(3d) / 2d, 0d, 1.5d, Math.Sqrt(3d) / 3d, -1d / 3d, 0d, 2d / 3d, 0.5d); // pointy

            Size = size;
            Origin = origin;
        }

        /// <summary>
        /// get the center point of a given cubic coordinate
        /// </summary>
        /// <param name="hex">the cubic coordinate to check</param>
        /// <returns>Point</returns>
        public GridPoint CubeToPoint(CubicCoordinate hex)
        {
            double x = (_M.F0 * hex.X + _M.F1 * hex.Y) * Size.X;
            double y = (_M.F2 * hex.X + _M.F3 * hex.Y) * Size.Y;
            return new GridPoint(x + Origin.X, y + Origin.Y);
        }

        /// <summary>
        /// given a point (e.g. screen coordinates) get the floating-point cubic coordinates
        /// </summary>
        /// <param name="p">the point to convert</param>
        /// <returns>CubeF</returns>
        public CubeF PointToCubeF(GridPoint p)
        {
            GridPoint pt = new GridPoint((p.X - Origin.X) / Size.X, (p.Y - Origin.Y) / Size.Y);

            double x = _M.B0 * pt.X + _M.B1 * pt.Y;
            double y = _M.B2 * pt.X + _M.B3 * pt.Y;

            return new CubeF(x, y, -x - y);
        }

        /// <summary>
        /// the points that define the six corners of the hexagon
        /// </summary>
        /// <param name="hex">Cubic coordinated for the hex</param>
        /// <returns>List of Point structures</returns>
        public GridPoint[] GetHexCornerPoints(CubicCoordinate hex, double factor = 1)
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
            double angle = 2.0 * Math.PI * (_M.StartAngle - corner) / 6.0;
            return new GridPoint(Size.X * Math.Cos(angle), Size.Y * Math.Sin(angle));
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

    }
}
