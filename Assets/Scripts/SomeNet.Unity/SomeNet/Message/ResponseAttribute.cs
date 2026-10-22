namespace SomeNet.Message
{
	public sealed class ResponseAttribute : MessageAttribute
	{
		public ResponseAttribute(string route)
			: base(MessageType.Response, route)
		{
		}
	}
}
