using System;
using ProtoBuf;

namespace GameCommon.Data
{
	[Serializable]
	[ProtoContract]
	public class AnimationSpeed
	{
		[ProtoMember(1)]
		public float endTime;

		[ProtoMember(2)]
		public float speed;
	}
}
