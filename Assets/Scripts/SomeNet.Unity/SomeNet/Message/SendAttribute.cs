namespace SomeNet.Message
{
	public sealed class SendAttribute : MessageAttribute
	{
		public SendAttribute(string route)
			: base(MessageType.Request, null)
		{
		}
	}
}
