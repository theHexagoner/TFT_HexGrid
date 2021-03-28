using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.Grids
{
    public interface IEdge
    {
        int ID { get; }

        IDictionary<int, IHexagon> Hexagons { get; }

        string PathD { get; }

        GridPoint PointA { get; }

        GridPoint PointB { get; }

        bool IsMegaLine { get; }

    }
}
