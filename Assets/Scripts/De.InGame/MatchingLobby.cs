using GameCommon.Enum;

public class MatchingLobby
{
	public ulong LobbyId;

	public ulong HostId;

	public string AccountName;

	public string PlayerName;

	public string Title;

	public int PlayerCountMax;

	public string PingLocationString;

	public string Version;

	public int HostLevel;

	public ERoomDifficulty RoomDifficulty;

	public ERoomPublishType PublishType;

	public int MemberCount;

	public int PingTime;

	public bool IsGameStarted;
}
