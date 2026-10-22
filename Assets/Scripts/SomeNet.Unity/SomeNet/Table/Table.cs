using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public abstract class Table<TKey, TValue> : ITable<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		private ConcurrentDictionary<TKey, TValue> _table = new ConcurrentDictionary<TKey, TValue>();

		public Type KeyType => typeof(TKey);

		public Type ValueType => typeof(TValue);

		public int Count => _table.Count;

		public TValue this[TKey id] => _table[id];

		public IEnumerable<TKey> Keys => _table.Keys;

		public IEnumerable<TValue> Values => _table.Values;

		public bool TryGetValue(TKey id, out TValue value)
		{
			return _table.TryGetValue(id, out value);
		}

		public TValue TryGetValue(TKey id)
		{
			_table.TryGetValue(id, out TValue value);
			return value;
		}

		public bool ContainsKey(TKey id)
		{
			return _table.ContainsKey(id);
		}

		public bool TryAdd(TKey key, TValue value)
		{
			return _table.TryAdd(key, value);
		}

		public bool TryRemove(TKey key, out TValue value)
		{
			return _table.TryRemove(key, out value);
		}

		public void Clear()
		{
			_table.Clear();
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _table.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _table.GetEnumerator();
		}

		public void Load(ITableLoader<TKey, TValue> loader)
		{
			foreach (KeyValuePair<TKey, TValue> pair in loader.Load())
			{
				if (!_table.TryAdd(pair.Key, pair.Value))
				{
					throw new InvalidOperationException($"duplicate key {pair.Key} in {typeof(TValue).Name}");
				}
			}
		}

		public bool Validate()
		{
			bool ok = true;
			foreach (TValue value in _table.Values)
			{
				ok &= value.Validate();
			}
			return ok;
		}

		public string ToCsv()
		{
			var lines = new List<string>();
			foreach (TValue value in _table.Values)
			{
				lines.Add(value.ToCsv());
			}
			return string.Join("\n", lines);
		}
	}
}
