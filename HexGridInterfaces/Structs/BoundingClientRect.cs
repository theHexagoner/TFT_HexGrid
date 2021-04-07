
using System.Text.Json.Serialization;

namespace HexGridInterfaces.Structs
{
    public struct BoundingClientRect
    {
        [JsonInclude] public readonly double Width;
        [JsonInclude] public readonly double Height;
        [JsonInclude] public readonly double OffsetLeft;
        [JsonInclude] public readonly double OffsetTop;

        [JsonConstructor]
        public BoundingClientRect(double width, double height, double offsetLeft, double offsetTop)
        {
            Width = width;
            Height = height;
            OffsetLeft = offsetLeft;
            OffsetTop = offsetTop;
        }

    }
}
