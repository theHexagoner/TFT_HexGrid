
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
        internal SvgGrid(IDictionary<int, ISvgHexagon> hexagons, 
                         IDictionary<int, SvgMegagon> megagons,
                         SvgViewBox viewBox)
        {
            SvgHexagons = hexagons;
            SvgMegagons = megagons;
            SvgViewBox = viewBox;
        }

        public IDictionary<int, ISvgHexagon> SvgHexagons { get; private set; }

        public IDictionary<int, SvgMegagon> SvgMegagons { get; private set; }

        public SvgViewBox SvgViewBox { get; private set; }

    }
}
