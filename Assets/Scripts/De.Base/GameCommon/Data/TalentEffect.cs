using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public struct TalentEffect
	{
		[ProtoMember(1)]
		public ETalentEffectType Type { get; set; }

		[ProtoMember(2)]
		public int Value1 { get; set; }

		[ProtoMember(3)]
		public int Value2 { get; set; }

		public TalentEffect(ETalentEffectType type, int value1 = 0, int value2 = 0)
		{
			Type = ETalentEffectType.None;
			Value1 = 0;
			Value2 = 0;
		}

		public static TalentEffect Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryParse(string str, out TalentEffect talentEffect)
		{
			talentEffect = default;
			return false;
		}
	}
}
