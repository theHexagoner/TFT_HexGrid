
using HexGridInterfaces.Factories;
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;
using SvgLib.ViewModels;
using System.Collections.Generic;

namespace SvgLib.Factories
{
    public class HitTesterFactory : IHitTesterFactory
    {
        public IHitTester Build(int rowCount, int colCount, GridPoint radius, GridPoint origin, OffsetSchema schema, IEnumerable<int> hexagonIDs)
        {
            return new HitTester(rowCount, colCount, radius, origin, schema, hexagonIDs);
        }
    }
}
