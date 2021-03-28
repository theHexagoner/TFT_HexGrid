using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;

namespace HexGridInterfaces.ViewModels
{
    public interface IHexGridPageVM
    {
        ISvgGrid Grid { get; }
        ISvgMap Map { get; }
        IHitTester HitTester { get; }
    }
}
