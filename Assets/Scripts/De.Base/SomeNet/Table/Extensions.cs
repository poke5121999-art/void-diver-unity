using System.Linq;
using System.Text.RegularExpressions;

namespace SomeNet.Table
{
	public static class Extensions
	{
		// Extensions.Clone<T,TKey> @0x180c01d70 and its lambda b__0_0 @0x180f56f50 (machine code checked): the row is re-split by its own quote handling, not by CsvHelper
		public static T Clone<T, TKey>(this T data) where T : ITableData<TKey>, new()
		{
			T clone = new T();
			string text = data.ToCsv().Replace("\"\"", "##");
			foreach (Match item in Regex.Matches(text, "\"([^\"]*)\""))
			{
				string value = item.Value;
				text = text.Replace(value, value.Replace("\"", "").Replace(',', ';'));
			}
			clone.Parse(text.Split(',').Select((string s) => s.Replace("##", "\"\"").Replace(';', ',')).ToArray());
			return clone;
		}
	}
}
