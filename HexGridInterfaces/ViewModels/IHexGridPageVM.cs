using HexGridInterfaces.Structs;

namespace HexGridInterfaces.ViewModels
{
    public interface IHexGridPageVM
    {
        SvgGrid Grid { get; }
        //ISvgMap Map { get; }
        //IHitTester HitTester { get; }
    }
}
