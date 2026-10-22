using System;
using System.Collections.Generic;

namespace SomeNet.Collections
{
	public static class Extension
	{
		// TODO(decomp): generic method has no body in the binary (open generic); written from the signature
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (T item in sequence)
			{
				action(item);
			}
		}
	}
}
