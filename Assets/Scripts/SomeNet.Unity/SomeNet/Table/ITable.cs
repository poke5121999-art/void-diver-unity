using System;
using System.Collections;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public interface ITable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		Type KeyType { get; }

		Type ValueType { get; }

		int Count { get; }

		IEnumerable<TKey> Keys { get; }

		TValue TryGetValue(TKey id);

		bool ContainsKey(TKey id);
	}
}
