using System;
using System.Collections.Generic;

namespace SomeNet.Table
{
	public class CsvTableLoader<TKey, TValue> : ITableLoader<TKey, TValue> where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		private string _csv;

		public CsvTableLoader(string csv)
		{
			_csv = csv;
		}

		public IEnumerable<KeyValuePair<TKey, TValue>> Load()
		{
			string[][] datas = CsvHelper.Parse(_csv, includeOptions: false);
			for (int i = 2; i < datas.Length; i++)
			{
				var value = new TValue();
				value.Parse(datas[i]);
				value.OnDataLoaded?.Invoke(value);
				yield return new KeyValuePair<TKey, TValue>(value.Pk, value);
			}
		}
	}
}
