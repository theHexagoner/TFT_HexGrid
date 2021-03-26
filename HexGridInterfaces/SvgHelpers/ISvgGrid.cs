using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.SvgHelpers
{
    public interface ISvgGrid
    {
        SvgViewBox SvgViewBox { get; }
        IEnumerable<KeyValuePair<int, ISvgHexagon>> SvgHexagons { get; }
        IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }

    }
}