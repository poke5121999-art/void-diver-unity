using System.Collections.Generic;
using System.Linq;

namespace GameCommon.Utility
{
	public static class WeightedRandomSelector
	{
		// TODO(decomp): generic method has no pseudo-C in the binary; roll source (UnityEngine.Random) and zero-weight handling are assumed
		public static T SelectWeightedRandom<T>(IEnumerable<(T item, int weight)> candidates, T defaultValue)
		{
			List<(T item, int weight)> list = candidates.ToList();
			int total = list.Sum(x => x.weight);
			if (total <= 0)
			{
				return defaultValue;
			}
			int roll = UnityEngine.Random.Range(0, total);
			foreach ((T item, int weight) in list)
			{
				roll -= weight;
				if (roll < 0)
				{
					return item;
				}
			}
			return defaultValue;
		}
	}
}
