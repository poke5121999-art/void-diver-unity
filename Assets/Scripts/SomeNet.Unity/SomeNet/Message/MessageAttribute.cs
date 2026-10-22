using System;

namespace SomeNet.Message
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public abstract class MessageAttribute : Attribute
	{
		public MessageType Type { get; private set; }

		public string Route { get; private set; }

		public MessageAttribute(MessageType type, string route)
		{
			Type = type;
			Route = route;
		}
	}
}
