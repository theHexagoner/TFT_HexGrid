using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HexGridInterfaces.Structs;
using HexGridInterfaces.Grids;

namespace HexGridLib.Grids
{

    public class Hexagon : IHexagon
    {
        // no default constructor
        private Hexagon() { }

        internal Hexagon(Grid grid, CubicCoordinate cubicCoords, OffsetCoordinate offsetCoords)
        {
            OffsetLocation = offsetCoords;
            CubicLocation = cubicCoords;

            ID = grid.GetHashcodeForCube(cubicCoords);
            Points = grid.GetHexCornerPoints(cubicCoords);
            Edges = GetEdgesFromPoints(Points);

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
        public CubicCoordinate CubicLocation { get; private set; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        public OffsetCoordinate OffsetLocation { get; private set; }

        public IEdge[] Edges { get; private set; }

        /// <summary>
        /// location of hex within its associated megagon
        /// </summary>
        public MegaLocation MegaLocation { get; private set; }

        /// <summary>
        /// sets the location in its megagon
        /// </summary>
        /// <param name="locationInMegagon">the enumerated location within the megagon</param>
        public void SetLocationInMegagon(MegaLocation locationInMegagon)
        {
            MegaLocation = locationInMegagon;
        }

        /// <summary>
        /// iterate over a set of points and return an array of GridEdge objects
        /// </summary>
        /// <param name="points">Array of GridPoint objects, generally this would be the points belonging to a hexagon</param>
        /// <returns>Array of GridEdge objects</returns>
        private static IEdge[] GetEdgesFromPoints(GridPoint[] points)
        {
            IEdge[] edges = new Edge[6];

            for (int i = 0; i < 5; i++)
            {
                edges[i] = new Edge(points[i], points[i + 1]);
            }

            edges[5] = new Edge(points[5], points[0]);

            return edges;
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
