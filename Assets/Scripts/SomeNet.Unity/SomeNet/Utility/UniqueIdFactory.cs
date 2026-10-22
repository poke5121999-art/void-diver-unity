namespace SomeNet.Utility
{
	public static class UniqueIdFactory
	{
		public static readonly int MaxBitCount;

		public static readonly int TimestampBitCount;

		public static readonly int ServerIdBitCount;

		public static readonly int SequenceBitCount;

		public static readonly int TimestampBitShiftCount;

		public static readonly int ServerIdBitShiftCount;

		public static readonly int MaxServiceId;

		public static readonly int MaxSequence;

		private static long _fromTime;

		private static long _lastTime;

		private static long _sequence;

		private static uint _serviceId;

		private static object _lock;

		public static void Init(int serverId)
		{
		}

		public static long CreateId()
		{
			return 0L;
		}

		public static string CreateToken()
		{
			return null;
		}

		public static long ConvertToId(string token)
		{
			return 0L;
		}

		public static string ConvertToToken(long id)
		{
			return null;
		}

		public static bool TryConvertToId(string token, out long id)
		{
			id = default;
			return false;
		}

		public static bool TryConvertToToken(long id, out string token)
		{
			token = null;
			return false;
		}
	}
}
