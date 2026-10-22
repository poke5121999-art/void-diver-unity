using System;
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
			Type = EEventConditionType.None;
			Value1 = 0L;
			Value2 = 0L;
		}

		public static EventCondition Empty()
		{
			return default;
		}

		public static bool operator ==(EventCondition left, EventCondition right)
		{
			return false;
		}

		public static bool operator !=(EventCondition left, EventCondition right)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryParse(string str, out EventCondition unlockCondition)
		{
			unlockCondition = default;
			return false;
		}
	}
}
