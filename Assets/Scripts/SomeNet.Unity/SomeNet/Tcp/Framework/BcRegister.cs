using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BcRegister : MessageBase
	{
		[ProtoMember(1)]
		public long SessionId { get; set; }
	}
}
