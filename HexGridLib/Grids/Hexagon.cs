using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HexGridLib.Coordinates;

namespace HexGridLib.Grids
{

    internal class Hexagon
    {
        // no default constructor
        private Hexagon() { }

        /// <summary>
        /// construct a Hexagon by passing the grid and offset coordinates
        /// </summary>
        /// <param name="grid">the parent grid that contains this hexagon</param>
        /// <param name="offsetCoords">the offset coordinates of the hexagon</param>
        public Hexagon(Grid grid, Offset offsetCoords) : this(grid, grid.OffsetSchema.GetCubicCoords(offsetCoords), offsetCoords) { }

        /// <summary>
        /// construct a hexagon by passing the grid and cubic coordinates
        /// </summary>
        /// <param name="grid">the parent grid that contains this hex</param>
        /// <param name="cubicCoords">the cubic coordinates of the hexagon</param>
        public Hexagon(Grid grid, Cube cubicCoords) : this(grid, cubicCoords, grid.OffsetSchema.GetOffsetCoords(cubicCoords)) { }

        private Hexagon(Grid grid, Cube cubeCoords, Offset offsetCoords)
        {
            OffsetLocation = offsetCoords;
            CubicLocation = cubeCoords;

            if (OffsetLocation.Col != CubicLocation.X || OffsetLocation.Row != CubicLocation.Y)
                Console.WriteLine("Shame");

            ID = grid.GetHashcodeForCube(cubeCoords);
            Points = grid.GetHexCornerPoints(cubeCoords);
            Edges = SvgMegagonsFactory.GetEdgesFromPoints(Points);

            foreach (GridEdge e in Edges)
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

        /// <summary>
        /// cubic coordinates of the hexagon within the grid
        /// X, Y and Z dimensions to represent 2d grid as 3d matrix
        /// used to calculate most things like distance to other hexes, line of sight, etc.
        /// </summary>        
        public Cube CubicLocation { get; private set; }

        /// <summary>
        /// offset (row and column) coordinates of the hexagon within the grid
        /// </summary>
        public Offset OffsetLocation { get; private set; }

        /// <summary>
        /// the points that define the six corners of the hexagon
        /// </summary>
        public GridPoint[] Points { get; private set; }

        public GridEdge[] Edges { get; private set; }

        /// <summary>
        /// location of hex within its associated megagon
        /// </summary>
        public MegaLocation MegaLocation { get; private set; }

        /// <summary>
        /// sets the location in its megagon
        /// </summary>
        /// <param name="locationInMegagon">the enumerated location within the megagon</param>
        internal void SetLocationInMegagon(MegaLocation locationInMegagon)
        {
            MegaLocation = locationInMegagon;
        }
    }

    internal class GridEdge
    {
        private GridEdge() { }

        public GridEdge(GridPoint gpa, GridPoint gpb)
        {
            // get the midpoint of gpa and gpb 
            GridPoint midPoint = new GridPoint((gpa.X + gpb.X) / 2, (gpa.Y + gpb.Y) / 2);
            ID = HashCode.Combine(midPoint.X, midPoint.Y);
            Hexagons = new HexDictionary<int, Hexagon>() { };
            PointA = gpa;
            PointB = gpb;
        }

        public int ID { get; private set; }

        public HexDictionary<int, Hexagon> Hexagons { get; }

        public GridPoint PointA { get; }

        public GridPoint PointB { get; }

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
