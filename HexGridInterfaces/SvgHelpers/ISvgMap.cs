using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.SvgHelpers
{
    public interface ISvgMap
    {
        SvgViewBox SvgViewBox { get; }
        IDictionary<int, ISvgHexagon> SvgHexagons { get; }
        IDictionary<int, SvgMegagon> SvgMegagons { get; }

        void AddHexagon(int ID);
        void RemoveHexagon(int ID);

    }
}
