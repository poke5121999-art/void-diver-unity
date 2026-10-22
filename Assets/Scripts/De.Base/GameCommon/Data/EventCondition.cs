using System;
using System.Globalization;
using GameCommon.Enum;
using ProtoBuf;

namespace GameCommon.Data
{
	[Serializable]
	[ProtoContract]
	public struct EventCondition
	{
		[ProtoMember(1)]
		public EEventConditionType Type;

		[ProtoMember(2)]
		public long Value1;

		[ProtoMember(3)]
		public long Value2;

		public EventCondition(EEventConditionType type, long value1, long value2 = 0L)
		{
			Type = type;
			Value1 = value1;
			Value2 = value2;
		}

		public static EventCondition Empty()
		{
			return default;
		}

		public static bool operator ==(EventCondition left, EventCondition right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(EventCondition left, EventCondition right)
		{
			return !left.Equals(right);
		}

		public override bool Equals(object obj)
		{
			return obj is EventCondition other && Type == other.Type && Value1 == other.Value1 && Value2 == other.Value2;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Value1, Value2);
		}

		public override string ToString()
		{
			return Value2 == 0 ? $"{Type}:{Value1}" : $"{Type}:{Value1}:{Value2}";
		}

		public static bool TryParse(string str, out EventCondition unlockCondition)
		{
			unlockCondition = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (!System.Enum.TryParse(parts[0], out EEventConditionType type))
			{
				return false;
			}
			long value1 = 0L;
			long value2 = 0L;
			if (parts.Length >= 2 && !long.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out value1))
			{
				return false;
			}
			if (parts.Length >= 3 && !long.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out value2))
			{
				return false;
			}
			unlockCondition = new EventCondition(type, value1, value2);
			return true;
		}
	}
}
