using System.Globalization;
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
			Type = type;
			Value1 = value1;
			Value2 = value2;
		}

		public static TalentEffect Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return Value2 == 0 ? $"{Type}:{Value1}" : $"{Type}:{Value1}:{Value2}";
		}

		public static bool TryParse(string str, out TalentEffect talentEffect)
		{
			talentEffect = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (!System.Enum.TryParse(parts[0], out ETalentEffectType type))
			{
				return false;
			}
			int value1 = 0;
			int value2 = 0;
			if (parts.Length >= 2 && !int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out value1))
			{
				return false;
			}
			if (parts.Length >= 3 && !int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out value2))
			{
				return false;
			}
			talentEffect = new TalentEffect(type, value1, value2);
			return true;
		}
	}
}
