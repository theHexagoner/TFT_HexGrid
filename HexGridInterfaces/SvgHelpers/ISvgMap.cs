using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.SvgHelpers
{
    public interface ISvgMap
    {
        SvgViewBox SvgViewBox { get; }
        IEnumerable<KeyValuePair<int, ISvgHexagon>> SvgHexagons { get; }
        IEnumerable<KeyValuePair<int, SvgMegagon>> SvgMegagons { get; }

        void AddHexagon(int ID);
        void RemoveHexagon(int ID);

    }
}
