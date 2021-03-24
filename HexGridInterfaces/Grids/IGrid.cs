

using System.Collections.Generic;

namespace HexGridInterfaces.Grids
{
    public interface IGrid
    {
        IDictionary<int, IEdge> Edges { get; }
        IDictionary<int, IHexagon> Hexagons { get; }
    }
}
