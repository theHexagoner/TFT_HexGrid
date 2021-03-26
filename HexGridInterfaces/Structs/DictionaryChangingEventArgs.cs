using System;

namespace HexGridInterfaces.Structs
{
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

}
