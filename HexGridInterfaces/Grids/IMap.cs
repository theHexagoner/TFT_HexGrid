using System.Collections.Generic;

namespace HexGridInterfaces.Grids
{
    public interface IMap
    {
        IEnumerable<KeyValuePair<int, IEdge>> Edges { get; }
        IEnumerable<KeyValuePair<int, IHexagon>> Hexagons { get; }

        void AddHexagon(int ID);
        void RemoveHexagon(int ID);

    }
}
