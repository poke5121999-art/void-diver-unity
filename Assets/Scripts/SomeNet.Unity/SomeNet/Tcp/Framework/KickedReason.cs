namespace SomeNet.Tcp.Framework
{
	public enum KickedReason
	{
		None = 0,
		DoubleLogin = 1,
		ServerStop = 2,
		ServerException = 3,
		SerializeException = 4,
		UserKick = 5,
		Timeout = 6,
		HackingAttemptDetected = 7
	}
}
