using HexGridInterfaces.SvgHelpers;

namespace HexGridInterfaces.ViewModels
{
    public interface IHexGridPageVM
    {
        ISvgGrid Grid { get; }
        ISvgMap Map { get; }

        // other properties used by the view

    }
}
