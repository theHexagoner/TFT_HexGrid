using HexGridInterfaces.Structs;
using System.Collections.Generic;

namespace HexGridInterfaces.Grids
{
    public interface IMap
    {
        IDictionary<int, IEdge> Edges { get; }
        IDictionary<int, IHexagon> Hexagons { get; }

        void AddHexagon(int ID);
        void RemoveHexagon(int ID);

        delegate void MapDictionaryAddItem(object sender, DictionaryChangingEventArgs<int, IHexagon> e);
        event MapDictionaryAddItem OnMapDictionaryAddItem;

        delegate void MapDictionaryRemoveItem(object sender, DictionaryChangingEventArgs<int, IHexagon> e);
        event MapDictionaryRemoveItem OnMapDictionaryRemoveItem;

    }
}
