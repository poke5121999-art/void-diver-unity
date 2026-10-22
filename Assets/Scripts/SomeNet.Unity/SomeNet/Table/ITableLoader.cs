using System;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public interface ITableLoader<TKey, TValue> where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		IEnumerable<KeyValuePair<TKey, TValue>> Load();
	}
}
