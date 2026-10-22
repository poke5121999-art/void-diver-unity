using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BqRegister : MessageBase
	{
		[ProtoMember(1)]
		public long SessionId { get; set; }
	}
}
