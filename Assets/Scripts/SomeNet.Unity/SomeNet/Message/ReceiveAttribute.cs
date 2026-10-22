namespace SomeNet.Message
{
	public sealed class ReceiveAttribute : MessageAttribute
	{
		public ReceiveAttribute(string route)
			: base(MessageType.Receive, route)
		{
		}
	}
}
