using System;

public class MessageAttribute : Attribute
{
	public EMessageType Type { get; private set; }

	public MessageAttribute(EMessageType type)
	{
		Type = type;
	}
}
