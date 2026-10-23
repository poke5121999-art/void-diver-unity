namespace SomeNet.Table
{
	public static class Extensions
	{
		// TODO(decomp): generic method has no pseudo-C in the binary; clone through the CSV row (ToCsv + Parse) is assumed
		public static T Clone<T, TKey>(this T data) where T : ITableData<TKey>, new()
		{
			var clone = new T();
			clone.Parse(CsvHelper.Parse(data.ToCsv(), false)[0]);
			return clone;
		}
	}
}
