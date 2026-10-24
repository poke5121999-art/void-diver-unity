using System;

namespace SomeNet.Utility
{
	public static class EnumHelper
	{
		public static T Parse<T>(string s)
		{
			// Enum.Parse(typeof(T), s) (shared body @0x180bc5c10 machine code checked)
			return (T)Enum.Parse(typeof(T), s);
		}
	}
}
