using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BqBindSession : MessageBase
	{
		[ProtoMember(1)]
		public long SessionToken { get; set; }
	}
}
