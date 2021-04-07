
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public struct SvgViewBox
    {
        [JsonInclude] public readonly double OriginX;
        [JsonInclude] public readonly double OriginY;
        [JsonInclude] public readonly double Width;
        [JsonInclude] public readonly double Height;

        [JsonConstructor]
        public SvgViewBox(double originX, double originY, double width, double height)
        {
            OriginX = originX;
            OriginY = originY;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", OriginX, OriginY, Width, Height);
        }

    }

}