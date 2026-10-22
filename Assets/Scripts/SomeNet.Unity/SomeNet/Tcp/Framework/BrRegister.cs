using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BrRegister : MessageBase
	{
		[ProtoMember(1)]
		public long Token { get; set; }
	}
}
