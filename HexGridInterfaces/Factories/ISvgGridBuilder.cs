using HexGridInterfaces.Structs;
using HexGridInterfaces.SvgHelpers;

namespace HexGridInterfaces.Factories
{
    public interface ISvgGridBuilder
    {

        ISvgGrid Build(int rowCount, int colCount, GridPoint size, GridPoint origin, OffsetSchema schema, SvgViewBox viewBox);

    }
}
