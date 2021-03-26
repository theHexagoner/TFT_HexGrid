using HexGridInterfaces.Structs;
using HexGridInterfaces.ViewModels;

namespace HexGridInterfaces.Factories
{
    public interface IHexGridPageVmBuilder
    {
        IHexGridPageVM Build(int rowCount, int colCount, GridPoint size, GridPoint origin, OffsetSchema schema, SvgViewBox viewBox);

    }
}
