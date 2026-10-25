using System.Globalization;
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
			StatType = type;
			Value = value;
		}

		public static Stat Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return $"{StatType}:{Value.ToString(CultureInfo.InvariantCulture)}";
		}

		// TryParse @0x18059e730: a null string throws, Enum.TryParse picks the type, the value is Single.Parse(NumberStyles.Any, InvariantCulture)
		// (throws on bad text), and a missing second part throws IndexOutOfRange; there is no part-count check
		public static bool TryParse(string str, out Stat stat)
		{
			stat = default;
			string[] parts = str.Split(':');
			if (!System.Enum.TryParse(parts[0], out EStatType type))
			{
				return false;
			}
			float value = float.Parse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture);
			stat = new Stat(type, value);
			return true;
		}
	}
}
