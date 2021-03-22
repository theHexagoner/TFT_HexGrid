using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HexGridLib.Coordinates;
using HexGridLib.Converters;

namespace HexGridLib.Grids
{

    public class Hexagon
    {
        // no default constructor
        private Hexagon() { }

        internal Hexagon(Grid grid, Cube cubicCoords, Offset offsetCoords)
        {
            OffsetLocation = offsetCoords;
            CubicLocation = cubicCoords;

            ID = grid.GetHashcodeForCube(cubicCoords);
            Points = grid.GetHexCornerPoints(cubicCoords);
            Edges = Converter.GetEdgesFromPoints(Points);

            foreach (Edge e in Edges)
            {
                if (grid.Edges.ContainsKey(e.ID))
                {
                    var edge = grid.Edges[e.ID];
                    edge.Hexagons.Add(ID, this);
                }
                else
                {
                    e.Hexagons.Add(ID, this);
                    grid.Edges.Add(e.ID, e);
                }
            }

            MegaLocation = MegaLocation.N;
        }

        public int ID { get; private set; }

        public int Row
        {
            get
            {
                return OffsetLocation.Row;
            }
        }
        
        public int Col
        {
            get
            {
                return OffsetLocation.Col;
            }
        }

        /// <summary>
        /// the points that define the six corners of the hexagon
        /// </summary>
        public GridPoint[] Points { get; private set; }

        /// <summary>
        /// cubic coordinates of the hexagon within the grid
        /// X, Y and Z dimensions to represent 2d grid as 3d matrix
        /// used to calculate most things like distance to other hexes, line of sight, etc.
        /// </summary>        
        internal Cube CubicLocation { get; private set; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        internal Offset OffsetLocation { get; private set; }

        internal Edge[] Edges { get; private set; }

        /// <summary>
        /// location of hex within its associated megagon
        /// </summary>
        internal MegaLocation MegaLocation { get; private set; }

        /// <summary>
        /// sets the location in its megagon
        /// </summary>
        /// <param name="locationInMegagon">the enumerated location within the megagon</param>
        internal void SetLocationInMegagon(MegaLocation locationInMegagon)
        {
            MegaLocation = locationInMegagon;
        }
    }

    
    /// <summary>
    /// custom dictionary to support events for adding and removing hexagons 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    internal class HexDictionary<K, V> : IDictionary<K, V>
    {
        public delegate void DictionaryAddItem(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryAddItem OnDictionaryAddItem;

        public delegate void DictionaryRemoveItem(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryRemoveItem OnDictionaryRemoveItem;

        public delegate void DictionaryClear(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryClear OnDictionaryClear;

        private readonly IDictionary<K, V> innerDict;

        public HexDictionary()
        {
            innerDict = new Dictionary<K, V>();
        }

        public HexDictionary(HexDictionary<K, V> hexDictionary)
        {
            innerDict = new Dictionary<K, V>(hexDictionary);
        }

        public void Add(K key, V value)
        {
            innerDict.Add(key, value);
            OnDictionaryAddItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = key, Value = value });
        }

        public void Add(KeyValuePair<K, V> item)
        {
            innerDict.Add(item);
            OnDictionaryAddItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = item.Key, Value = item.Value });
        }

        public void Clear()
        {
            innerDict.Clear();
            OnDictionaryClear?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = default, Value = default });
        }

        public bool Remove(K key)
        {
            var item = innerDict[key];
            var success = innerDict.Remove(key);
            if (success) OnDictionaryRemoveItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = key, Value = item });
            return success;
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            var success = innerDict.Remove(item);
            if (success) OnDictionaryRemoveItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = item.Key, Value = item.Value });
            return success;
        }

        #region Other IDictionary overrides

        public ICollection<K> Keys => innerDict.Keys;

        public ICollection<V> Values => innerDict.Values;

        public int Count => innerDict.Count;

        public bool IsReadOnly => innerDict.IsReadOnly;

        public V this[K key] { get => innerDict[key]; set => innerDict[key] = value; }

        public bool ContainsKey(K key)
        {
            return innerDict.ContainsKey(key);
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return innerDict.TryGetValue(key, out value);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return innerDict.Contains(item);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            innerDict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        #endregion

    }

    internal class DictionaryChangingEventArgs<K, V> : EventArgs
    {
        public K Key
        {
            get;
            set;
        }

        public V Value
        {
            get;
            set;
        }
    }


}
