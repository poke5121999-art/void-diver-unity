using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SomeNet.Collections;

namespace SomeNet.Table
{
	public abstract class SubTable<K1, V>
	{
		private ConcurrentDictionary<K1, V> _table;

		public Type KeyType => null;

		public Type ValueType => null;

		public int Count => 0;

		public V this[K1 key1]
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public bool ContainsKey(K1 key1)
		{
			return false;
		}

		public bool TryGetValue(K1 key1, out V value)
		{
			value = default;
			return false;
		}

		public void Init(IEnumerable<(K1 k1, V v)> source)
		{
		}
	}
	public abstract class SubTable<K1, K2, V>
	{
		private ConcurrentDictionary<KeyTuple<K1, K2>, V> _table;

		public Type KeyType => null;

		public Type ValueType => null;

		public int Count => 0;

		public V this[K1 key1, K2 key2]
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public bool ContainsKey(K1 key1, K2 key2)
		{
			return false;
		}

		public bool TryGetValue(K1 key1, K2 key2, out V value)
		{
			value = default;
			return false;
		}

		public void Init(IEnumerable<(K1 k1, K2 k2, V v)> source)
		{
		}
	}
}
