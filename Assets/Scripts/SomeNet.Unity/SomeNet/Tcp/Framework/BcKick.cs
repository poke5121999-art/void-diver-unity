using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BcKick : MessageBase
	{
		[ProtoMember(1)]
		public KickedReason KickedReason { get; set; }
	}
}
