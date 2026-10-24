using System;
using System.Globalization;
using System.Threading;

namespace SomeNet.Utility
{
	public static class UniqueIdFactory
	{
		public static readonly int MaxBitCount = 62;

		public static readonly int TimestampBitCount = 41;

		public static readonly int ServerIdBitCount = 12;

		public static readonly int SequenceBitCount = 10;

		public static readonly int TimestampBitShiftCount = MaxBitCount - TimestampBitCount;

		public static readonly int ServerIdBitShiftCount = MaxBitCount - ServerIdBitCount - TimestampBitCount;

		public static readonly int MaxServiceId = (1 << ServerIdBitCount) - 1;

		public static readonly int MaxSequence = (1 << SequenceBitCount) - 1;

		private static long _fromTime = 0L;

		private static long _lastTime = 0L;

		private static long _sequence = 0L;

		private static uint _serviceId = 0u;

		private static object _lock = new object();

		public static void Init(int serverId)
		{
			if (serverId > MaxServiceId)
			{
				throw new Exception(string.Format("serverId overflow serverId:{0}, maxServerId:{1}", serverId, MaxServiceId));
			}
			_serviceId = (uint)serverId;
			// DateTime(2020, 1, 1) three-int constructor, kind Unspecified (machine code checked @0x182f7c2ea)
			_fromTime = new DateTimeOffset(new DateTime(2020, 1, 1), TimeSpan.Zero).ToUnixTimeMilliseconds();
		}

		public static long CreateId()
		{
			lock (_lock)
			{
				long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _fromTime;
				if (_sequence == MaxSequence)
				{
					_sequence = 0L;
					if (now == _lastTime)
					{
						Thread.Sleep(TimeSpan.FromMilliseconds(1.0));
						now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _fromTime;
					}
				}
				long id = (long)(_serviceId << ServerIdBitShiftCount) | (now << TimestampBitShiftCount) | _sequence;
				_lastTime = now;
				_sequence++;
				return id;
			}
		}

		public static string CreateToken()
		{
			return ConvertToToken(CreateId());
		}

		public static long ConvertToId(string token)
		{
			int length = token.Length;
			string tail = token.Substring(length - 5);
			string head = token.Substring(0, 3);
			string middle = token.Substring(3, length - 8);
			return long.Parse(tail + head + middle, NumberStyles.HexNumber);
		}

		public static string ConvertToToken(long id)
		{
			string hex = id.ToString("X");
			return hex.Substring(5, 3) + hex.Substring(8) + hex.Substring(0, 5);
		}

		public static bool TryConvertToId(string token, out long id)
		{
			id = 0L;
			try
			{
				id = ConvertToId(token);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool TryConvertToToken(long id, out string token)
		{
			token = null;
			try
			{
				token = ConvertToToken(id);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
