using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameNpcTalkManager : MonoBase
{
	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameNpcDialogManager _npcDialogManager;

	public readonly HashSet<int> ActiveTalkIds = new HashSet<int>();

	private readonly Dictionary<string, int> _talkIdByDialogKey = new Dictionary<string, int>();

	private readonly HashSet<int> _nextActiveTalkIds = new HashSet<int>();

	private readonly List<int> _addedTalkIds = new List<int>();

	private readonly List<int> _removedTalkIds = new List<int>();

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_eventSystem.OnEventOccured.Subscribe(delegate
		{
			EvaluateAll();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Subscribe(delegate
		{
			EvaluateAll();
		}).AddTo(this);
		_gameManager.OnGameClosed.Subscribe(delegate
		{
			Clear();
		}).AddTo(this);
	}

	private void Clear()
	{
		ActiveTalkIds.Clear();
		_talkIdByDialogKey.Clear();
	}

	public bool IsNewDialog(string dialogKey)
	{
		int value;
		if (!_talkIdByDialogKey.TryGetValue(dialogKey, out value))
		{
			return false;
		}
		return !_playerManager.IsNpcTalkViewed(value);
	}

	public void MarkDialogViewed(string dialogKey)
	{
		int value;
		if (_talkIdByDialogKey.TryGetValue(dialogKey, out value))
		{
			_playerManager.SetNpcTalkViewed(value);
		}
	}

	private void EvaluateAll()
	{
		string luaKey = null;
		if (!_playerManager.IsPlayerLoaded || _gameManager.IsInStage)
		{
			return;
		}
		_nextActiveTalkIds.Clear();
		foreach (TNpcTalk value in TNpcTalk.Table.Values)
		{
			bool flag = _playerManager.CheckStateConditions(value.AddConditions);
			bool flag2 = value.RemoveConditions != null && value.RemoveConditions.Length > 0 && _playerManager.CheckStateConditions(value.RemoveConditions);
			if (flag && !flag2)
			{
				_nextActiveTalkIds.Add(value.Id);
			}
		}
		if (_nextActiveTalkIds.SetEquals(ActiveTalkIds))
		{
			return;
		}
		_addedTalkIds.Clear();
		_removedTalkIds.Clear();
		foreach (int nextActiveTalkId in _nextActiveTalkIds)
		{
			if (!ActiveTalkIds.Contains(nextActiveTalkId))
			{
				_addedTalkIds.Add(nextActiveTalkId);
			}
		}
		foreach (int activeTalkId in ActiveTalkIds)
		{
			if (!_nextActiveTalkIds.Contains(activeTalkId))
			{
				_removedTalkIds.Add(activeTalkId);
			}
		}
		ActiveTalkIds.Clear();
		ActiveTalkIds.UnionWith(_nextActiveTalkIds);
		if (_addedTalkIds.Count > 0)
		{
			luaKey = "NpcTalk/NpcTalk";
			foreach (int addedTalkId in _addedTalkIds)
			{
				TNpcTalk talkData = TNpcTalk.Table[addedTalkId];
				DialogInfo dialogInfo = new DialogInfo
				{
					Key = "NpcTalk/NpcTalk/" + talkData.FunctionName,
					Type = EDialogType.Talk,
					Title = TLocalizedText.Table[talkData.FunctionName + "_TITLE"].LocalText,
					OnClicked = delegate
					{
						_eventSystem.OnEventOccured.OnNext(new LuaDialogEvent(luaKey, talkData.FunctionName));
					}
				};
				_talkIdByDialogKey[dialogInfo.Key] = talkData.Id;
				_npcDialogManager.AddDialog(talkData.NpcId, dialogInfo);
			}
		}
		if (_removedTalkIds.Count < 1)
		{
			return;
		}
		foreach (int removedTalkId in _removedTalkIds)
		{
			TNpcTalk tNpcTalk = TNpcTalk.Table[removedTalkId];
			string text = "NpcTalk/NpcTalk/" + tNpcTalk.FunctionName;
			_talkIdByDialogKey.Remove(text);
			_npcDialogManager.RemoveDialog(tNpcTalk.NpcId, text);
		}
	}
}
