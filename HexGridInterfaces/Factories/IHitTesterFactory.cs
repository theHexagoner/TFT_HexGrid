using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.Factories
{
    public interface IHitTesterFactory
    {
        IHitTester Build(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema, IEnumerable<int> hexagonIDs);
    }
}
