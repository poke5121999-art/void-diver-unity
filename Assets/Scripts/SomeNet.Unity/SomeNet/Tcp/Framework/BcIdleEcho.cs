using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BcIdleEcho : MessageBase
	{
		[ProtoMember(1)]
		public int Data;
	}
}
