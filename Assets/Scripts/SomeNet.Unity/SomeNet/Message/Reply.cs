namespace SomeNet.Message
{
	public class Reply
	{
		public int Code;

		public MessageBase Message;

		public MessageBase[] AdditionalMessages;

		public string MessageName => null;

		public Reply(int code)
		{
		}

		public Reply(MessageBase message)
		{
		}

		public Reply(int code, MessageBase message)
		{
		}

		public Reply(int code, MessageBase message, MessageBase[] additionalMessages)
		{
		}
	}
}
