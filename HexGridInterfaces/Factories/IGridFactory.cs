﻿
using HexGridInterfaces.Grids;
using HexGridInterfaces.Structs;

namespace HexGridInterfaces.Factories
{
    public interface IGridFactory
    {
        //_rowCount, _colCount, size, origin, schema
        IGrid Build(int rowCount, int colCount, GridPoint size, GridPoint origin, OffsetSchema schema);


    }
}
