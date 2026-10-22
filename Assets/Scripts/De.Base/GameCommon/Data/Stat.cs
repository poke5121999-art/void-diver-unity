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

		public static bool TryParse(string str, out Stat stat)
		{
			stat = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (parts.Length != 2 || !System.Enum.TryParse(parts[0], out EStatType type) || !float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
			{
				return false;
			}
			stat = new Stat(type, value);
			return true;
		}
	}
}
