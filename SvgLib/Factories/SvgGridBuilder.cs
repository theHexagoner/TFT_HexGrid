using HexGridInterfaces.Factories;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using SvgLib.Grids;
using System.Collections.Generic;

namespace SvgLib.Factories
{
    public sealed class SvgGridBuilder : ISvgGridBuilder
    {

        public ISvgGrid Build(IDictionary<int, ISvgHexagon> svgHexagons, IDictionary<int, SvgMegagon> svgMegagons, SvgViewBox viewBox)
        {
            return new SvgGrid(svgHexagons, svgMegagons, viewBox);
        }

    }
}
