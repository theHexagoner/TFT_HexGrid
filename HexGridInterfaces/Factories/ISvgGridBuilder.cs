using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;
using System.Collections.Generic;

namespace HexGridInterfaces.Factories
{
    public interface ISvgGridBuilder
    {

        ISvgGrid Build(IEnumerable<KeyValuePair<int, ISvgHexagon>> svgHexagons, IEnumerable<KeyValuePair<int, SvgMegagon>> svgMegagons, SvgViewBox viewBox);

    }
}
