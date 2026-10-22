namespace SomeNet.Message
{
	public sealed class RequestAttribute : MessageAttribute
	{
		public RequestAttribute(string route)
			: base(MessageType.Request, route)
		{
		}
	}
}
