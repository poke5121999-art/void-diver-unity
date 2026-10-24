using System;
using System.Collections.Generic;

namespace SomeNet.Collections
{
	public static class Extension
	{
		// plain enumerator loop (shared body @0x180c001a0 machine code checked)
		public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (T item in sequence)
			{
				action(item);
			}
		}
	}
}
