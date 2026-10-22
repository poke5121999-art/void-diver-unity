using System;

namespace SomeNet.Utility
{
	public struct SimpleStopWatch
	{
		private DateTime _startTime;

		public static SimpleStopWatch Start()
		{
			SimpleStopWatch watch = default(SimpleStopWatch);
			watch._startTime = DateTime.UtcNow;
			return watch;
		}

		public TimeSpan Stop()
		{
			return DateTime.UtcNow - _startTime;
		}

		public int StopMs()
		{
			return (int)((DateTime.UtcNow - _startTime).Ticks / 10000);
		}
	}
}
