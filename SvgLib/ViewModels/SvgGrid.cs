
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using System.Collections.Generic;

namespace SvgLib.Grids
{
    public class SvgGrid : ISvgGrid
    {
        // no default constructor
        private SvgGrid() { }

        // call this from the builder
        internal SvgGrid(IEnumerable<KeyValuePair<int, ISvgHexagon>> svgHexagons,
                         IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons,
                         SvgViewBox viewBox)
        {
            SvgHexagons = svgHexagons;
            SvgMegagons = svgMegagons;
            SvgViewBox = viewBox;
        }

        public IEnumerable<KeyValuePair<int, ISvgHexagon>> SvgHexagons { get; private set; }

        public IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; private set; }

        public SvgViewBox SvgViewBox { get; private set; }

    }
}
