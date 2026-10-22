using System;
using GameCommon.Enum;

public class ChatInfo
{
	public EChatType Type { get; set; }

	public DateTime UtcTime { get; set; }

	public string Text { get; set; }

	public ulong ClientId { get; set; }
}
