using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public abstract class GroupTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue[]>>, IEnumerable where TKey : IComparable where TValue : new()
	{
		private ConcurrentDictionary<TKey, TValue[]> _table;

		public Type KeyType => null;

		public Type ValueType => null;

		public int Count => 0;

		public TValue[] this[TKey id] => null;

		public IEnumerable<TKey> Keys => null;

		public IEnumerable<TValue[]> Values => null;

		public bool TryGetValue(TKey id, out TValue[] value)
		{
			value = null;
			return false;
		}

		public TValue[] TryGetValue(TKey id)
		{
			return null;
		}

		public bool ContainsKey(TKey id)
		{
			return false;
		}

		public IEnumerator<KeyValuePair<TKey, TValue[]>> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		public void Init(Dictionary<TKey, TValue[]> table)
		{
		}
	}
}
