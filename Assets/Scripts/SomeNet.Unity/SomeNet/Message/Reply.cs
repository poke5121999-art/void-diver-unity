namespace SomeNet.Message
{
	public class Reply
	{
		public int Code;

		public MessageBase Message;

		public MessageBase[] AdditionalMessages;

		public string MessageName => (Message != null) ? Message.GetType().Name : string.Empty;

		public Reply(int code)
		{
			Code = code;
		}

		public Reply(MessageBase message)
		{
			Message = message;
		}

		public Reply(int code, MessageBase message)
		{
			Code = code;
			Message = message;
		}

		public Reply(int code, MessageBase message, MessageBase[] additionalMessages)
		{
			Code = code;
			Message = message;
			AdditionalMessages = additionalMessages;
		}
	}
}
