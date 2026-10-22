using System;

namespace SomeNet.Table
{
	public interface ITableData<TKey>
	{
		Action<ITableData<TKey>> OnDataLoaded { get; set; }

		TKey Pk { get; set; }

		void Parse(string[] columns);

		bool Validate();

		string ToCsv();
	}
}
