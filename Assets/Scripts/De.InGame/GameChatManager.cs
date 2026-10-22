using System.Collections.Generic;
using NemoLib.Log;
using GameCommon.Enum;
using GameCommon.Table;
using System;
using UniRx;
using UnityEngine;
using VContainer;

public class GameChatManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SeriLogger _logger;

	public ReactiveCollection<ChatInfo> RxChatRecords = new ReactiveCollection<ChatInfo>();

	private const int UserChatLimitCount = 5;

	private const float UserChatLimitSeconds = 5f;

	private readonly Queue<float> _userChatTimeStamps = new Queue<float>();

	private float _userChatBlockEndTime;

	private bool IsUserChatBlocked => Time.time < _userChatBlockEndTime;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<NtfUserChat>().Subscribe(OnNtfUserChat).AddTo(this);
		_gameChannel.Receive<NtfUserTextKeyChat>().Subscribe(OnNtfUserTextKeyChat).AddTo(this);
		_gameManager.OnGameClosed.Subscribe(OnGameClosed).AddTo(this);
		_gameManager.OnPlayerEntered.Subscribe(OnPlayerEntered).AddTo(this);
		_gameManager.OnPlayerLeft.Subscribe(OnPlayerLeft).AddTo(this);
	}

	private void OnNtfUserChat(NtfUserChat message)
	{
		AddChat(new ChatInfo
		{
			Type = EChatType.User,
			UtcTime = DateTime.UtcNow,
			Text = UIInputUtility.Mask(message.Text),
			ClientId = message.ClientId
		});
	}

	private void OnNtfUserTextKeyChat(NtfUserTextKeyChat message)
	{
		string text = LocalizationUtility.GetLocalizedText(message.TextKey);
		ChatInfo chatInfo = new ChatInfo
		{
			Type = EChatType.User,
			UtcTime = DateTime.UtcNow
		};
		if (!string.IsNullOrEmpty(message.PlayerName))
		{
			text = "[" + message.PlayerName + "] " + text;
		}
		chatInfo.Text = text;
		chatInfo.ClientId = message.ClientId;
		AddChat(chatInfo);
	}

	private void OnGameClosed(Unit _)
	{
		Clear();
	}

	private void OnPlayerEntered(GamePlayer player)
	{
		AddChat(new ChatInfo
		{
			Type = EChatType.System,
			Text = string.Format(TLocalizedText.ChatPlayerEntered.LocalText, player.Name),
			UtcTime = DateTime.UtcNow
		});
	}

	private void OnPlayerLeft(GamePlayer player)
	{
		AddChat(new ChatInfo
		{
			Type = EChatType.System,
			Text = string.Format(TLocalizedText.ChatPlayerLeft.LocalText, player.Name),
			UtcTime = DateTime.UtcNow
		});
	}

	public void AddChat(ChatInfo chatInfo)
	{
		RxChatRecords.Add(chatInfo);
		while (RxChatRecords.Count > 20)
		{
			RxChatRecords.RemoveAt(0);
		}
	}

	private void Clear()
	{
		RxChatRecords.Clear();
	}

	public bool TrySendUserChat(string text)
	{
		if (Time.time < _userChatBlockEndTime)
		{
			return false;
		}
		_gameChannel.SendToHost(new ReqUserChat
		{
			Text = text
		});
		float time = Time.time;
		while (_userChatTimeStamps.Count > 0 && time - _userChatTimeStamps.Peek() > 5f)
		{
			_userChatTimeStamps.Dequeue();
		}
		_userChatTimeStamps.Enqueue(time);
		if (_userChatTimeStamps.Count >= 5)
		{
			_userChatBlockEndTime = time + 5f;
			_userChatTimeStamps.Clear();
		}
		return true;
	}
}
