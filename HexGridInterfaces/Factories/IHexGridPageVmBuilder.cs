using HexGridInterfaces.Structs;
using HexGridInterfaces.ViewModels;

namespace HexGridInterfaces.Factories
{
    public interface IHexGridPageVmBuilder
    {
        IHexGridPageVM Build(GridVars vars);

    }
}
