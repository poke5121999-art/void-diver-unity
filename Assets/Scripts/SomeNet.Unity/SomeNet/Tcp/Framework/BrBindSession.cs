using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BrBindSession : MessageBase
	{
		[ProtoMember(1)]
		public int ResultCode { get; set; }
	}
}
