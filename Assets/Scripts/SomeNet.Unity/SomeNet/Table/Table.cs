using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public abstract class Table<TKey, TValue> : ITable<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		private ConcurrentDictionary<TKey, TValue> _table;

		public Type KeyType => null;

		public Type ValueType => null;

		public int Count => 0;

		public TValue this[TKey id] => null;

		public IEnumerable<TKey> Keys => null;

		public IEnumerable<TValue> Values => null;

		public bool TryGetValue(TKey id, out TValue value)
		{
			value = null;
			return false;
		}

		public TValue TryGetValue(TKey id)
		{
			return null;
		}

		public bool ContainsKey(TKey id)
		{
			return false;
		}

		public bool TryAdd(TKey key, TValue value)
		{
			return false;
		}

		public bool TryRemove(TKey key, out TValue value)
		{
			value = null;
			return false;
		}

		public void Clear()
		{
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		public void Load(ITableLoader<TKey, TValue> loader)
		{
		}

		public bool Validate()
		{
			return false;
		}

		public string ToCsv()
		{
			return null;
		}
	}
}
