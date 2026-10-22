using System;

namespace SomeNet.Utility
{
	public static class RandomUtility
	{
		private static readonly Random _random = new Random();

		public static int GetRandomInt(int max)
		{
			return _random.Next(max);
		}

		public static int GetRandomInt(int min, int max)
		{
			return _random.Next(min, max);
		}

		public static int GetRandomIntFromFloat(float value)
		{
			int whole = (int)Math.Floor(value);
			float fraction = value - (float)whole;
			return (fraction > GetRandomFloat()) ? (whole + 1) : whole;
		}

		public static float GetRandomFloat(float min, float max)
		{
			return (float)(_random.NextDouble() * (double)(max - min) + (double)min);
		}

		public static float GetRandomFloat()
		{
			return (float)_random.NextDouble();
		}

		public static bool GetRandomBool(float successRate)
		{
			return successRate >= GetRandomFloat();
		}

		public static bool GetRandomBoolFromPercentage(float successPercent)
		{
			return successPercent / 100f >= GetRandomFloat();
		}
	}
}
