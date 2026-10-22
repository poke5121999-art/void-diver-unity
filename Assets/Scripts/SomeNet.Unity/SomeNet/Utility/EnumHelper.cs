using System;

namespace SomeNet.Utility
{
	public static class EnumHelper
	{
		public static T Parse<T>(string s)
		{
			// TODO(decomp): generic method has no body of its own in the binary (@0x0); written from the signature as Enum.Parse.
			return (T)Enum.Parse(typeof(T), s);
		}
	}
}
