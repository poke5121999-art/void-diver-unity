namespace SomeNet.Message
{
	public enum ParseHeaderResult
	{
		Ok = 1,
		NotEnoughDataReceived = -1,
		ExceptionOccured = -2,
		InvalidPacketType = -3,
		FailedToGetMessageId = -4
	}
}
