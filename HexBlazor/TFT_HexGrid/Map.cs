using TFT_HexGrid.Grids;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Text;
using TFT_HexGrid.SvgHelpers;
using System.Linq;

namespace TFT_HexGrid.Maps
{
    public class Map
    {
        private Map() { }

        internal Map(Grid grid, SvgMegagonsFactory megaFactory)
        {
            Hexagons = new MapHexDictionary<int, Hexagon>(grid.Hexagons);
            Hexagons.OnDictionaryAddItem += AddingHexagon;
            Hexagons.OnDictionaryRemoveItem += RemovingHexagon;
            Hexagons.OnDictionaryClear += ClearingHexagons;

            SvgHexagons = new Dictionary<int, SvgHexagon>();

            foreach (Hexagon h in Hexagons.Values)
            {
                SvgHexagons.Add(h.ID, new SvgHexagon(h.ID, h.Points));
            }

            MegaFactory = megaFactory;
            SvgMegagons = MegaFactory.GetMegagons();

        }

        #region Hexagons

        public MapHexDictionary<int, Hexagon> Hexagons { get; private set; }
        public Dictionary<int, SvgHexagon> SvgHexagons { get; }

        private void AddingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            var hex = e.Value;
            SvgHexagons.Add(hex.ID, new SvgHexagon(hex.ID, hex.Points, true));
        }

        private void RemovingHexagon(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Remove(e.Key);
        }

        private void ClearingHexagons(object sender, DictionaryChangingEventArgs<int, Hexagon> e)
        {
            SvgHexagons.Clear();
        }

        #endregion

        #region Megagons

        private SvgMegagonsFactory MegaFactory { get; }

        public Dictionary<int, SvgMegagon> SvgMegagons { get; }

        #endregion

    }

    public class DictionaryChangingEventArgs<K, V> : EventArgs
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

    // maybe implement this as Immutable Dictionary to support undo/redo?
    // how to cancel these events?

    public class MapHexDictionary<K, V> : IDictionary<K, V>
    {
        public delegate void DictionaryAddItem(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryAddItem OnDictionaryAddItem;

        public delegate void DictionaryRemoveItem(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryRemoveItem OnDictionaryRemoveItem;

        public delegate void DictionaryClear(object sender, DictionaryChangingEventArgs<K, V> e);
        public event DictionaryClear OnDictionaryClear;

        private readonly IDictionary<K, V> innerDict;

        public MapHexDictionary(Dictionary<K, V> gridHexDictionary)
        {
            innerDict = new Dictionary<K, V>(gridHexDictionary);
        }

        public void Add(K key, V value)
        {
            OnDictionaryAddItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = key, Value = value });
            innerDict.Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            OnDictionaryAddItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = item.Key, Value = item.Value });
            innerDict.Add(item);
        }

        public void Clear()
        {
            OnDictionaryClear?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = default, Value = default });
            innerDict.Clear();
        }

        public bool Remove(K key)
        {
            OnDictionaryRemoveItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = key, Value = default });
            return innerDict.Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            OnDictionaryRemoveItem?.Invoke(this, new DictionaryChangingEventArgs<K, V>() { Key = item.Key, Value = item.Value });
            return innerDict.Remove(item);
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
}
