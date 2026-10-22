using ProtoBuf;
using SomeNet.Message;

namespace SomeNet.Tcp.Framework
{
	[ProtoContract]
	public class BcFirst : MessageBase
	{
		[ProtoMember(1)]
		public int WrapKey1;

		[ProtoMember(2)]
		public int WrapKey2;
	}
}
