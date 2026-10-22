using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public class ProtoVector3
	{
		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }
	}
}
