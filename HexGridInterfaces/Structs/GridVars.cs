using System;

namespace HexGridInterfaces.Structs
{
    public struct GridVars
    {
        public readonly int RowCount;
        public readonly int ColCount;
        public readonly GridPoint Radius;
        public readonly GridPoint Origin;
        public readonly OffsetSchema Schema;
        public readonly SvgViewBox ViewBox;

        public GridVars(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema, SvgViewBox viewBox)
        {
            RowCount = rowCount;
            ColCount = colCount;
            Radius = radius;
            Origin = origin;
            Schema = schema;
            ViewBox = viewBox;
        }
    }
}
