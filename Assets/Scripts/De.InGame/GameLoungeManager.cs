using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameLoungeManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SeriLogger _logger;

	public Subject<int> OnAreaUnlocked { get; } = new Subject<int>();

	public Subject<(EAreaDecorationType areaDecorationType, int oldAreaDecorationSettingId, int newAreaDecorationSettingId)> OnAreaDecorationChanged { get; } = new Subject<(EAreaDecorationType areaDecorationType, int oldAreaDecorationSettingId, int newAreaDecorationSettingId)>();

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.PrepareLoad).Subscribe(delegate
		{
			OnPrepareLoad();
		}).AddTo(this);
		_gameManager.OnPlayerEntered.Subscribe(OnPlayerEntered).AddTo(this);
		_gameChannel.Receive<NtfUpdateArea>().Subscribe(OnNtfUpdateArea).AddTo(this);
		_gameChannel.Receive<NtfUpdateAreaDecoration>().Subscribe(OnNtfUpdateAreaDecoration).AddTo(this);
		_gameChannel.Receive<NtfUpdateNpcLevel>().Subscribe(OnNtfUpdateNpcLevel).AddTo(this);
	}

	private void OnPrepareLoad()
	{
		if (!_gameManager.IsHost || !_gameContext.IsInLounge)
		{
			return;
		}
		_gameContext.AreaUnlockedFlags = _playerManager.AreaFlags?.Clone() ?? new BitFlags();
		SendAreaFlags();
		_gameContext.AreaDecorationUnlockedFlags = _playerManager.AreaDecorationUnlockedFlags?.Clone() ?? new BitFlags();
		_gameContext.ActiveAreaDecorations = ((_playerManager.ActiveAreaDecorations.Count > 0) ? new Dictionary<int, int>(_playerManager.ActiveAreaDecorations) : new Dictionary<int, int>());
		SendAreaDecorationState();
		InitNpcLevelState();
		SendNpcLevelState();
	}

	public void CheatResetInterior()
	{
		_playerManager.AreaFlags.Clear();
		_gameContext.AreaUnlockedFlags.Clear();
		foreach (TArea item in TArea.Table.Values.Where((TArea _) => _.DefaultUnlocked))
		{
			_playerManager.AreaFlags.Set(item.BitFlagIndex);
			_gameContext.AreaUnlockedFlags.Set(item.BitFlagIndex);
		}
		_playerManager.AreaDecorationUnlockedFlags.Clear();
		_gameContext.AreaDecorationUnlockedFlags.Clear();
		_playerManager.ActiveAreaDecorations.Clear();
		_gameContext.ActiveAreaDecorations.Clear();
		foreach (TAreaDecoration item2 in TAreaDecoration.Table.Values.Where((TAreaDecoration _) => _.Index == 0))
		{
			_playerManager.AreaDecorationUnlockedFlags.Set(item2.BitFlagIndex);
			_gameContext.AreaDecorationUnlockedFlags.Set(item2.BitFlagIndex);
			_playerManager.ActiveAreaDecorations[item2.GroupId] = item2.Id;
			_gameContext.ActiveAreaDecorations[item2.GroupId] = item2.Id;
		}
	}

	private void OnPlayerEntered(GamePlayer player)
	{
		if (!_gameManager.IsHost || !_gameContext.IsInLounge || _gameContext.AreaUnlockedFlags == null)
		{
			return;
		}
		_gameHost.SendToClient(player.ClientId, new NtfUpdateArea
		{
			AreaFlags = _gameContext.AreaUnlockedFlags?.Clone()
		});
		if (_gameContext.AreaDecorationUnlockedFlags != null)
		{
			_gameHost.SendToClient(player.ClientId, new NtfUpdateAreaDecoration
			{
				AreaDecorationUnlockedFlags = _gameContext.AreaDecorationUnlockedFlags?.Clone(),
				ActiveAreaDecorations = _gameContext.ActiveAreaDecorations.Select((KeyValuePair<int, int> kv) => new ActiveAreaDecoration
				{
					GroupId = kv.Key,
					AreaDecorationSettingId = kv.Value
				}).ToArray()
			});
		}
		if (_gameContext.NpcLevels != null)
		{
			if (_gameContext.NpcLevels.Count < 1)
			{
				return;
			}
			_gameHost.SendToClient(player.ClientId, new NtfUpdateNpcLevel
			{
				NpcLevels = _gameContext.NpcLevels.Select((KeyValuePair<int, int> kv) => new NpcLevelRecord
				{
					NpcId = kv.Key,
					Level = kv.Value
				}).ToArray()
			});
		}
	}

	public bool IsAreaUnlocked(int areaId)
	{
		TArea tArea = TArea.Table[areaId];
		return _gameContext.AreaUnlockedFlags.IsSet(tArea.BitFlagIndex);
	}

	private void InitAreaFlags()
	{
		_gameContext.AreaUnlockedFlags = _playerManager.AreaFlags?.Clone() ?? new BitFlags();
	}

	public void UnlockArea(int areaId)
	{
		_gameManager.AssertIfNotHost();
		TArea tArea = TArea.Table[areaId];
		_gameContext.AreaUnlockedFlags.Set(tArea.BitFlagIndex);
		_playerManager.SetAreaUnlocked(areaId);
		OnAreaUnlocked.OnNext(areaId);
		SendAreaFlags();
	}

	private void SendAreaFlags()
	{
		_gameHost.SendToAllExceptHost(new NtfUpdateArea
		{
			AreaFlags = _gameContext.AreaUnlockedFlags?.Clone()
		});
	}

	private void OnNtfUpdateArea(NtfUpdateArea message)
	{
		if (!_gameManager.IsHost)
		{
			_gameContext.AreaUnlockedFlags = message.AreaFlags;
		}
	}

	public bool IsAreaDecorationUnlocked(int areaDecorationSettingId)
	{
		TAreaDecoration tAreaDecoration = TAreaDecoration.Table[areaDecorationSettingId];
		return _gameContext.AreaDecorationUnlockedFlags.IsSet(tAreaDecoration.BitFlagIndex);
	}

	public int GetActiveAreaDecoration(int groupId)
	{
		if (!_gameContext.ActiveAreaDecorations.TryGetValue(groupId, out var value))
		{
			return 0;
		}
		return value;
	}

	private void InitAreaDecorationState()
	{
		_gameContext.AreaDecorationUnlockedFlags = _playerManager.AreaDecorationUnlockedFlags?.Clone() ?? new BitFlags();
		// Dropped by Ghidra after the write barrier; disassembly @0x18073a4ef.
		_gameContext.ActiveAreaDecorations = ((_playerManager.ActiveAreaDecorations.Count > 0) ? new Dictionary<int, int>(_playerManager.ActiveAreaDecorations) : new Dictionary<int, int>());
	}

	public void UnlockAreaDecoration(int areaDecorationSettingId)
	{
		_gameManager.AssertIfNotHost();
		TAreaDecoration tAreaDecoration = TAreaDecoration.Table[areaDecorationSettingId];
		_gameContext.AreaDecorationUnlockedFlags.Set(tAreaDecoration.BitFlagIndex);
		_playerManager.SetAreaDecorationUnlocked(areaDecorationSettingId);
	}

	public void SetActiveAreaDecoration(int areaDecorationSettingId)
	{
		_gameManager.AssertIfNotHost();
		TAreaDecoration tAreaDecoration = TAreaDecoration.Table[areaDecorationSettingId];
		if (!_gameContext.ActiveAreaDecorations.TryGetValue(tAreaDecoration.GroupId, out var value))
		{
			value = 0;
		}
		_gameContext.ActiveAreaDecorations[tAreaDecoration.GroupId] = areaDecorationSettingId;
		_playerManager.SetActiveAreaDecoration(tAreaDecoration.GroupId, areaDecorationSettingId);
		OnAreaDecorationChanged.OnNext((tAreaDecoration.AreaDecorationType, value, areaDecorationSettingId));
		SendAreaDecorationState();
	}

	private void SendAreaDecorationState()
	{
		_gameHost.SendToAllExceptHost(new NtfUpdateAreaDecoration
		{
			AreaDecorationUnlockedFlags = _gameContext.AreaDecorationUnlockedFlags?.Clone(),
			ActiveAreaDecorations = _gameContext.ActiveAreaDecorations?.Select((KeyValuePair<int, int> _) => new ActiveAreaDecoration
			{
				GroupId = _.Key,
				AreaDecorationSettingId = _.Value
			}).ToArray()
		});
	}

	private void OnNtfUpdateAreaDecoration(NtfUpdateAreaDecoration message)
	{
		if (_gameManager.IsHost)
		{
			return;
		}
		_gameContext.AreaDecorationUnlockedFlags = message.AreaDecorationUnlockedFlags;
		Dictionary<int, int> dictionary = _gameContext.ActiveAreaDecorations ?? new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = message.ActiveAreaDecorations?.ToDictionary((ActiveAreaDecoration a) => a.GroupId, (ActiveAreaDecoration a) => a.AreaDecorationSettingId) ?? new Dictionary<int, int>();
		_gameContext.ActiveAreaDecorations = dictionary2;
		foreach (KeyValuePair<int, int> item in dictionary2)
		{
			item.Deconstruct(out var key, out var value);
			if (!dictionary.TryGetValue(key, out var value2))
			{
				value2 = 0;
			}
			if (value2 != value)
			{
				TAreaDecoration tAreaDecoration = TAreaDecoration.Table[value];
				OnAreaDecorationChanged.OnNext((tAreaDecoration.AreaDecorationType, value2, value));
			}
		}
	}

	public int GetNpcLevel(int npcId)
	{
		if (!_gameContext.NpcLevels.TryGetValue(npcId, out var value))
		{
			return 0;
		}
		return value;
	}

	public void InitNpcLevelState()
	{
		_gameContext.NpcLevels = ((_playerManager.NpcLevels.Count > 0) ? _playerManager.NpcLevels.ToDictionary((KeyValuePair<int, int> kv) => kv.Key, (KeyValuePair<int, int> kv) => kv.Value) : new Dictionary<int, int>());
	}

	public void SetNpcLevel(int npcId, int level)
	{
		_gameManager.AssertIfNotHost();
		_gameContext.NpcLevels[npcId] = level;
		_playerManager.SetNpcLevel(npcId, level);
		_gameHost.SendToAllExceptHost(new NtfUpdateNpcLevel
		{
			NpcLevels = _gameContext.NpcLevels.Select((KeyValuePair<int, int> kv) => new NpcLevelRecord
			{
				NpcId = kv.Key,
				Level = kv.Value
			}).ToArray()
		});
	}

	private void SendNpcLevelState()
	{
		_gameHost.SendToAllExceptHost(new NtfUpdateNpcLevel
		{
			NpcLevels = _gameContext.NpcLevels.Select((KeyValuePair<int, int> kv) => new NpcLevelRecord
			{
				NpcId = kv.Key,
				Level = kv.Value
			}).ToArray()
		});
	}

	private void OnNtfUpdateNpcLevel(NtfUpdateNpcLevel message)
	{
		if (!_gameManager.IsHost)
		{
			_gameContext.NpcLevels = message.NpcLevels?.ToDictionary((NpcLevelRecord n) => n.NpcId, (NpcLevelRecord n) => n.Level) ?? new Dictionary<int, int>();
		}
	}
}
