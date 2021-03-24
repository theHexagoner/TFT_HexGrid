using System;

namespace HexGridInterfaces.Structs
{
    public struct HexGridParams
    {
        public readonly int RowCount;
        public readonly int ColCount;
        public readonly GridPoint Radius;
        public readonly GridPoint Origin;
        public readonly OffsetSchema Schema;
        public readonly SvgViewBox ViewBox;

        public HexGridParams(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema, SvgViewBox viewBox)
        {
            RowCount = rowCount;
            ColCount = colCount;
            Radius = radius;
            Origin = origin;
            Schema = schema;
            ViewBox = viewBox;
        }
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

        #region Conversions

        private static readonly int EVEN = 1;
        private static readonly int ODD = -1;

        #region Offset to Cubic Coords

        public CubicCoordinate GetCubicCoords(OffsetCoordinate hex)
        {
            return Style switch
            {
                HexagonStyle.Flat => OffsetToCubeQ(Offset == OffsetPush.Even ? EVEN : ODD, hex),
                HexagonStyle.Pointy => OffsetToCubeR(Offset == OffsetPush.Even ? EVEN : ODD, hex),
                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", Style))
            };
        }

        private static CubicCoordinate OffsetToCubeQ(int push, OffsetCoordinate h)
        {
            int x = h.Col;
            int y = h.Row - (h.Col + push * (h.Col & 1)) / 2;
            int z = -x - y;
            return new CubicCoordinate(x, y, z);
        }

        private static CubicCoordinate OffsetToCubeR(int push, OffsetCoordinate h)
        {
            int x = h.Col - (h.Row + push * (h.Row & 1)) / 2;
            int y = h.Row;
            int z = -x - y;
            return new CubicCoordinate(x, y, z);
        }

        #endregion

        #region Cubic to Offset Coords

        /// <summary>
        /// convert a cube to offset (row, column) coordinates
        /// </summary>
        /// <param name="hex">the hex for which you want the offset coordinates</param>
        /// <returns>Offset</returns>
        internal OffsetCoordinate GetOffsetCoords(CubicCoordinate hex)
        {
            return Style switch
            {
                HexagonStyle.Flat => GetOffsetQ(Offset == OffsetPush.Even ? EVEN : ODD, hex),
                HexagonStyle.Pointy => GetOffsetR(Offset == OffsetPush.Even ? EVEN : ODD, hex),
                _ => throw new ArgumentException(string.Format("Invalid Style {0} specified for OffsetSchema", Style))
            };
        }

        private static OffsetCoordinate GetOffsetQ(int push, CubicCoordinate hex)
        {
            int col = hex.X;
            int row = hex.Y + ((hex.X + push * (hex.X & 1)) / 2);
            return new OffsetCoordinate(row, col);
        }

        private static OffsetCoordinate GetOffsetR(int push, CubicCoordinate hex)
        {
            int col = hex.X + (hex.Y + push * (hex.Y & 1)) / 2;
            int row = hex.Y;
            return new OffsetCoordinate(col, row);
        }

        #endregion

        #endregion

    }
}