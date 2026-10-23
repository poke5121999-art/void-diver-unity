using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
	public static (List<int>, List<int>) GetExclusiveElements(this List<int> a, List<int> b)
	{
		Dictionary<int, int> countsA = a.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
		Dictionary<int, int> countsB = b.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
		var onlyA = new List<int>();
		var onlyB = new List<int>();
		foreach (KeyValuePair<int, int> pair in countsA)
		{
			if (!countsB.ContainsKey(pair.Key))
			{
				onlyA.AddRange(Enumerable.Repeat(pair.Key, pair.Value));
			}
			else if (countsB[pair.Key] < pair.Value)
			{
				onlyA.AddRange(Enumerable.Repeat(pair.Key, pair.Value - countsB[pair.Key]));
			}
		}
		foreach (KeyValuePair<int, int> pair in countsB)
		{
			if (!countsA.ContainsKey(pair.Key))
			{
				onlyB.AddRange(Enumerable.Repeat(pair.Key, pair.Value));
			}
			else if (countsA[pair.Key] < pair.Value)
			{
				onlyB.AddRange(Enumerable.Repeat(pair.Key, pair.Value - countsA[pair.Key]));
			}
		}
		return (onlyA, onlyB);
	}
}
