namespace SomeNet.Message
{
	public class Packet
	{
		public PacketType Type;

		public int RequestId;

		public int ReplyCode;

		public MessageBase Message;

		public MessageBase[] AdditionalMessages;

		public string MessageName => (Message != null) ? Message.GetType().Name : string.Empty;
	}
}
