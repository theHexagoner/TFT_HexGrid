using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using System.Collections.Generic;

namespace HexGridInterfaces.Factories
{
    public interface ISvgGridBuilder
    {

        ISvgGrid Build(IDictionary<int, ISvgHexagon> svgHexagons, IDictionary<int, SvgMegagon> svgMegagons, SvgViewBox viewBox);

    }
}
