using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SomeNet.Utility
{
	public static class EnumerableExtensions
	{
		public static TSource FindMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.FindMinBy(selector, Comparer<TKey>.Default);
		}

		public static TSource FindMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			// TODO(decomp): no instantiation of FindMinBy/FindMaxBy exists in GameAssembly (never called), so the bodies are written from the names; empty source returns default.
			TSource best = default;
			TKey bestKey = default;
			bool first = true;
			foreach (TSource item in source)
			{
				TKey key = selector(item);
				if (first || comparer.Compare(key, bestKey) < 0)
				{
					best = item;
					bestKey = key;
					first = false;
				}
			}
			return best;
		}

		public static TSource FindMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.FindMaxBy(selector, Comparer<TKey>.Default);
		}

		public static TSource FindMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
		{
			TSource best = default;
			TKey bestKey = default;
			bool first = true;
			foreach (TSource item in source)
			{
				TKey key = selector(item);
				if (first || comparer.Compare(key, bestKey) > 0)
				{
					best = item;
					bestKey = key;
					first = false;
				}
			}
			return best;
		}

		public static IEnumerable<T> Yield<T>(this T item)
		{
			yield return item;
		}
	}
}
