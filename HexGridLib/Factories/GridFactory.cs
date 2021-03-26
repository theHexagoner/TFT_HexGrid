using HexGridInterfaces.Factories;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using HexGridLib.Grids;

namespace HexGridLib.Factories
{
    public class GridFactory : IGridFactory
    {
        public IGrid Build(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema)
        {
            return new Grid(rowCount, colCount, radius, origin, schema);
        }
    }
}
