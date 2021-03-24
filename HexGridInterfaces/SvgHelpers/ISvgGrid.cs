using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.SvgHelpers
{
    public interface ISvgGrid
    {
        SvgViewBox SvgViewBox { get; }
        IDictionary<int, ISvgHexagon> SvgHexagons { get; }
        IDictionary<int, SvgMegagon> SvgMegagons { get; }

    }
}