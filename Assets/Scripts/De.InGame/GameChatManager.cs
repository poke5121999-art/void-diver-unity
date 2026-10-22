using System.Collections.Generic;
using NemoLib.Log;
using UniRx;
using VContainer;

public class GameChatManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SeriLogger _logger;

	public ReactiveCollection<ChatInfo> RxChatRecords;

	private const int UserChatLimitCount = 5;

	private const float UserChatLimitSeconds = 5f;

	private readonly Queue<float> _userChatTimeStamps;

	private float _userChatBlockEndTime;

	private bool IsUserChatBlocked => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnNtfUserChat(NtfUserChat message)
	{
	}

	private void OnNtfUserTextKeyChat(NtfUserTextKeyChat message)
	{
	}

	private void OnGameClosed(Unit _)
	{
	}

	private void OnPlayerEntered(GamePlayer player)
	{
	}

	private void OnPlayerLeft(GamePlayer player)
	{
	}

	public void AddChat(ChatInfo chatInfo)
	{
	}

	private void Clear()
	{
	}

	public bool TrySendUserChat(string text)
	{
		return false;
	}
}
