

using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;

namespace HexGridInterfaces.Grids
{
    public interface IHitTester
    {
        int? HitTest(GridPoint point);
    }
}
