using HexGridInterfaces.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HexGridLib.Maps
{
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

        private readonly IDictionary<K, V> innerDict;

        public HexDictionary()
        {
            innerDict = new Dictionary<K, V>();
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

        public void Clear()
        {
            innerDict.Clear();
        }

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
