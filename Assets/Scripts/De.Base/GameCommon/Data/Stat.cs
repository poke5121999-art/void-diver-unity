using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public struct Stat
	{
		[ProtoMember(1)]
		public EStatType StatType { get; set; }

		[ProtoMember(2)]
		public float Value { get; set; }

		public Stat(EStatType type, float value)
		{
			StatType = EStatType.None;
			Value = 0f;
		}

		public static Stat Empty()
		{
			return default;
		}

		public static bool TryParse(string str, out Stat stat)
		{
			stat = default;
			return false;
		}
	}
}
