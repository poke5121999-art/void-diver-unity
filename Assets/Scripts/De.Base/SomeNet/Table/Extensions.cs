namespace SomeNet.Table
{
	public static class Extensions
	{
		public static T Clone<T, TKey>(this T data) where T : ITableData<TKey>, new()
		{
			return default;
		}
	}
}
