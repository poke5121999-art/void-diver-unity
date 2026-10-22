using System;
using System.Linq;

namespace SomeNet.Utility
{
	public static class TypeExtensions
	{
		public static string GetNameWithGenericArguments(this Type t)
		{
			string name = t.Name;
			int tick = name.IndexOf('`');
			if (tick != -1)
			{
				name = name.Substring(0, tick);
			}
			Type[] arguments = t.GetGenericArguments();
			if (arguments != null && arguments.Count() != 0)
			{
				name = name + "<" + string.Join(", ", arguments.Select((Type v) => v.Name)) + ">";
			}
			return name;
		}

		public static string GetNameWithoutGenericArguments(this Type t)
		{
			string name = t.Name;
			int tick = name.IndexOf('`');
			if (tick != -1)
			{
				name = name.Substring(0, tick);
			}
			return name;
		}
	}
}
