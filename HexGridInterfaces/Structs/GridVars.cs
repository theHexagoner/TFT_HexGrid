
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public struct GridVars
    {
        [JsonInclude] public readonly int RowCount;
        [JsonInclude] public readonly int ColCount;
        [JsonInclude] public readonly GridPoint Radius;
        [JsonInclude] public readonly GridPoint Origin;
        [JsonInclude] public readonly OffsetSchema Schema;
        [JsonInclude] public readonly SvgViewBox ViewBox;

        [JsonConstructor]
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
