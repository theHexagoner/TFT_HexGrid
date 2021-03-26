using HexGridInterfaces.Factories;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using SvgLib.Grids;
using System.Collections.Generic;

namespace SvgLib.Factories
{
    public sealed class SvgGridBuilder : ISvgGridBuilder
    {

        public ISvgGrid Build(IEnumerable<KeyValuePair<int, ISvgHexagon>> svgHexagons, IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons, SvgViewBox viewBox)
        {
            return new SvgGrid(svgHexagons, svgMegagons, viewBox);
        }

    }
}
