using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SomeNet.Collections;

namespace SomeNet.Table
{
	public abstract class SubTable<K1, V>
	{
		private ConcurrentDictionary<K1, V> _table = new ConcurrentDictionary<K1, V>();

		public Type KeyType => typeof(K1);

		public Type ValueType => typeof(V);

		public int Count => _table.Count;

		public V this[K1 key1]
		{
			get
			{
				return _table[key1];
			}
			set
			{
				_table[key1] = value;
			}
		}

		public bool ContainsKey(K1 key1)
		{
			return _table.ContainsKey(key1);
		}

		public bool TryGetValue(K1 key1, out V value)
		{
			return _table.TryGetValue(key1, out value);
		}

		public void Init(IEnumerable<(K1 k1, V v)> source)
		{
			_table = new ConcurrentDictionary<K1, V>();
			foreach ((K1 k1, V v) item in source)
			{
				_table[item.k1] = item.v;
			}
		}
	}
	public abstract class SubTable<K1, K2, V>
	{
		private ConcurrentDictionary<KeyTuple<K1, K2>, V> _table = new ConcurrentDictionary<KeyTuple<K1, K2>, V>();

		public Type KeyType => typeof(KeyTuple<K1, K2>);

		public Type ValueType => typeof(V);

		public int Count => _table.Count;

		public V this[K1 key1, K2 key2]
		{
			get
			{
				return _table[new KeyTuple<K1, K2>(key1, key2)];
			}
			set
			{
				_table[new KeyTuple<K1, K2>(key1, key2)] = value;
			}
		}

		public bool ContainsKey(K1 key1, K2 key2)
		{
			return _table.ContainsKey(new KeyTuple<K1, K2>(key1, key2));
		}

		public bool TryGetValue(K1 key1, K2 key2, out V value)
		{
			return _table.TryGetValue(new KeyTuple<K1, K2>(key1, key2), out value);
		}

		public void Init(IEnumerable<(K1 k1, K2 k2, V v)> source)
		{
			_table = new ConcurrentDictionary<KeyTuple<K1, K2>, V>();
			foreach ((K1 k1, K2 k2, V v) item in source)
			{
				_table[new KeyTuple<K1, K2>(item.k1, item.k2)] = item.v;
			}
		}
	}
}
