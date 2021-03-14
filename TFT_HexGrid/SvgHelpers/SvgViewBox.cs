
namespace TFT_HexGrid.SvgHelpers
{
    public struct SvgViewBox
    {
        public readonly double OriginX;
        public readonly double OriginY;
        public readonly double Width;
        public readonly double Height;

        public SvgViewBox(double x, double y, double w, double h)
        {
            OriginX = x;
            OriginY = y;
            Width = w;
            Height = h;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", OriginX, OriginY, Width, Height);
        }

    }
}
