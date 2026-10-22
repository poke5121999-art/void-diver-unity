using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public abstract class GroupTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue[]>>, IEnumerable where TKey : IComparable where TValue : new()
	{
		private ConcurrentDictionary<TKey, TValue[]> _table = new ConcurrentDictionary<TKey, TValue[]>();

		public Type KeyType => typeof(TKey);

		public Type ValueType => typeof(TValue);

		public int Count => _table.Count;

		public TValue[] this[TKey id] => _table[id];

		public IEnumerable<TKey> Keys => _table.Keys;

		public IEnumerable<TValue[]> Values => _table.Values;

		public bool TryGetValue(TKey id, out TValue[] value)
		{
			return _table.TryGetValue(id, out value);
		}

		public TValue[] TryGetValue(TKey id)
		{
			_table.TryGetValue(id, out TValue[] value);
			return value;
		}

		public bool ContainsKey(TKey id)
		{
			return _table.ContainsKey(id);
		}

		public IEnumerator<KeyValuePair<TKey, TValue[]>> GetEnumerator()
		{
			return _table.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _table.GetEnumerator();
		}

		public void Init(Dictionary<TKey, TValue[]> table)
		{
			_table = new ConcurrentDictionary<TKey, TValue[]>(table);
		}
	}
}
