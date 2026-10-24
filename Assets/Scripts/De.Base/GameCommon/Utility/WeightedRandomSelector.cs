using System.Collections.Generic;
using System.Linq;

namespace GameCommon.Utility
{
	public static class WeightedRandomSelector
	{
		// SelectWeightedRandom<int> @0x180ef32e0 (machine code checked): float roll in [0, total) from RandomUtility, running float sum of the weights, first item whose sum exceeds the roll
		public static T SelectWeightedRandom<T>(IEnumerable<(T item, int weight)> candidates, T defaultValue)
		{
			List<(T item, int weight)> list = candidates.ToList();
			if (list == null || list.Count == 0)
			{
				return defaultValue;
			}
			int total = list.Sum(x => x.weight);
			if (total <= 0)
			{
				return defaultValue;
			}
			float roll = SomeNet.Utility.RandomUtility.GetRandomFloat(0f, total);
			float sum = 0f;
			foreach ((T item, int weight) in list)
			{
				sum += weight;
				if (sum > roll)
				{
					return item;
				}
			}
			return defaultValue;
		}
	}
}
