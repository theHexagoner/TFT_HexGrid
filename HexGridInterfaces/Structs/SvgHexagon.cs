using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public struct SvgHexagon
    {
        [JsonInclude] public readonly int ID;
        [JsonInclude] public readonly string Points;
        [JsonInclude] public readonly string CenterD;
        [JsonInclude] public readonly int Row;
        [JsonInclude] public readonly int Col;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">Supply a unique ID for SvgHexagon</param>
        /// <param name="points">The points from which to derive the SVG</param>
        /// <param name="isSelected">Flag indicating how the client might fill the polygon</param>
        [JsonConstructor]
        public SvgHexagon(int id, int row, int col, string points, string centerD)
        {
            ID = id;
            Row = row;
            Col = col;
            Points = points;
            CenterD = centerD;
        }

    }
}
