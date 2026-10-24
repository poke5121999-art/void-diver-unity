using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class PlayerManager : MonoBase
{
	[Inject]
	private SeriLogger _logger;

	[Inject]
	private EnvironmentSettings _settings;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private StorageService _saveStorage;

	private static readonly TimeSpan _saveInterval = TimeSpan.FromSeconds(60.0);

	private DateTime _lastSaveTime = DateTime.MinValue;

	private bool _isQuitting;

	private bool _isSaving;

	private bool _saveRequested;

	private CancellationTokenSource _saveCts = new CancellationTokenSource();

	private PlayerContext _playerContext;

	public const int MaxSlotCount = 6;

	private AccountContext _accountContext;

	public ReactiveProperty<int> RxPlayerSlotIndex = new ReactiveProperty<int>();

	public readonly Subject<int> OnPlayerSlotUpdated = new Subject<int>();

	private CompositeDisposable _playerDisposable = new CompositeDisposable();

	private DateTime? _sessionStart;

	private HashSet<int> _lockedNpcIds;

	private HashSet<int> _lockedBgmIds;

	private HashSet<int> _lockedNpcFunctionIds;

	private HashSet<EFeatureUnlock> _lockedFeatures;

	private GamePlayer Player => _gameManager.MyPlayer;

	public bool IsPlayerLoaded
	{
		get
		{
			if (_playerContext == null)
			{
				return false;
			}
			return _playerContext.Initialized;
		}
	}

	private ulong MyClientId => _gameManager.LocalClientId;

	private CharacterModel MyCharacterModel => _gameManager.MyCharacterModel;

	public int PlayerSlotIndex => RxPlayerSlotIndex.Value;

	public PlayerSlot[] PlayerSlots => _accountContext.PlayerSlots;

	public PlayerSlot SelectedPlayerSlot => _accountContext.PlayerSlots[RxPlayerSlotIndex.Value];

	public IReadOnlyReactiveDictionary<int, Character> RxCharacters => _playerContext.RxCharacters;

	public IDictionary<int, Character> Characters => _playerContext.RxCharacters;

	public Character SelectedCharacter => _playerContext.RxCharacters[SelectedCharacterId];

	public IReadOnlyDictionary<long, DialogReplayRecord> DialogReplayRecords => _playerContext.DialogReplayRecords;

	public Subject<(EGoodsType goodsType, int id, bool isMarked)> OnGoodsMarkChanged { get; } = new Subject<(EGoodsType, int, bool)>();

	public Subject<int> OnEquipmentCollected { get; } = new Subject<int>();

	public Subject<int> OnItemCollected { get; } = new Subject<int>();

	public Subject<int> OnItemFirstUsed { get; } = new Subject<int>();

	public int BestStage
	{
		get
		{
			if (_playerContext == null || _playerContext.ClearedCampaignFlags == null)
			{
				return 0;
			}
			int num = 0;
			foreach (KeyValuePair<int, TCampaign> item in TCampaign.Table)
			{
				if (item.Value.Category == ECampaignCategory.Story && num < item.Key && _playerContext.ClearedCampaignFlags.IsSet(item.Value.BitFlagIndex))
				{
					num = item.Key;
				}
			}
			return num;
		}
	}

	public double AccountPowerScore
	{
		get
		{
			ulong[] array = _playerContext?.RxTalent?.Value?.BitFlags?.Flags;
			if (array == null)
			{
				return 0.0;
			}
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num += PopCount(array[i]);
			}
			return num;
		}
	}

	public BitFlags AreaFlags => _playerContext.AreaFlags;

	public BitFlags AreaDecorationUnlockedFlags => _playerContext.AreaDecorationUnlockedFlags;

	public IDictionary<int, int> ActiveAreaDecorations => _playerContext.ActiveAreaDecorations;

	public IDictionary<int, int> NpcLevels => _playerContext.NpcLevels;

	public IDictionary<int, GoodsSlot> InventorySlots => _playerContext.RxInventorySlots;

	public IDictionary<int, GoodsSlot> SafeInventorySlots => _playerContext.RxSafeInventorySlots;

	public IDictionary<int, GoodsSlot> StashSlots => _playerContext.RxStashSlots;

	public IDictionary<int, GoodsSlot> StorageSlots => _playerContext.RxStorageSlots;

	public ISubject<Unit> OnInventoryChanged => _playerContext.OnInventoryChanged;

	public ISubject<Unit> OnStashChanged => _playerContext.OnStashChanged;

	public ISubject<Unit> OnSafeInventoryChanged => _playerContext.OnSafeInventoryChanged;

	public ISubject<Unit> OnOwnedGoodsChanged => _playerContext.OnOwnedGoodsChanged;

	public ISubject<Unit> OnStorageChanged => _playerContext.OnStorageChanged;

	public IReadOnlyReactiveCollection<LostItemSelectInfo> RxSelectLostGoodsInfos => _playerContext.RxSelectLostGoodsInfos;

	public IDictionary<int, LostGoods> LostGoods => _playerContext.RxLostGoods;

	public ISubject<Unit> OnLostItemChanged => _playerContext.OnLostItemChanged;

	public IDictionary<int, LoungeQuest> LoungeQuests => _playerContext.LoungeQuests;

	public Subject<int> OnMonsterRecordUpdated { get; } = new Subject<int>();

	public ERoomDifficulty Difficulty => _playerContext.Difficulty;

	public ERoomDifficulty LastPlayedRoomDifficulty => _playerContext.LastPlayedRoomDifficulty;

	public Subject<Unit> OnPlayerInitialized { get; } = new Subject<Unit>();

	private string PlayerFileName => MakePlayerFileName(RxPlayerSlotIndex.Value);

	private string BackupPlayerFileName => "PlayerContext";

	public IReadOnlyReactiveCollection<int> RxQuickSlotItemIds => _playerContext.RxQuickSlotItemIds;

	public IList<int> QuickSlotItemIds => _playerContext.RxQuickSlotItemIds;

	public IReadOnlyReactiveProperty<Talent> RxTalent => _playerContext.RxTalent;

	public Talent Talent => _playerContext.RxTalent.Value;

	public IDictionary<int, Employee> Employees => _playerContext.Employees;

	public IDictionary<int, SalesSlot> SalesSlots => _playerContext.SalesSlots;

	public IDictionary<int, Employee> RecruitableEmployees => _playerContext.RecruitableEmployees;

	public BitFlags TycoonUnlockFlags => _playerContext.TycoonUnlockFlags;

	public IDictionary<int, int> FiredEmployeeCooldowns => _playerContext.FiredEmployeeCooldowns;

	public IReadOnlyReactiveProperty<Profile> RxProfile => _playerContext?.RxProfile;

	public Profile Profile => _playerContext.RxProfile.Value;

	public string Name => _playerContext.RxProfile.Value.Name;

	public long Gold => _playerContext?.RxProfile?.Value?.Gold ?? 0;

	public long Exp => _playerContext?.RxProfile?.Value?.Exp ?? 0;

	public long Coin => _playerContext?.RxProfile?.Value?.Coin ?? 0;

	public int Level => _playerContext?.RxProfile?.Value?.Level ?? 0;

	public int SelectedCharacterId => _playerContext?.RxProfile?.Value?.SelectedCharacterId ?? 0;

	public int SelectedPetId => _playerContext?.RxProfile?.Value?.SelectedPetId ?? 0;

	public long CreateTimeTicks
	{
		get
		{
			if (_playerContext == null)
			{
				return 0L;
			}
			return _playerContext.CreateTimeTicks;
		}
	}

	public AppVersion InstallVersion
	{
		get
		{
			if (_playerContext == null)
			{
				return null;
			}
			return _playerContext.Version;
		}
	}

	public double PlayTimeSeconds
	{
		get
		{
			if (_playerContext == null)
			{
				return 0.0;
			}
			return _playerContext.PlayTime.TotalSeconds;
		}
	}

	public int BgmId => _playerContext.RxProfile.Value.BgmId;

	public bool PreventRepairMaxDurabilityLoss
	{
		get
		{
			if (_playerContext == null)
			{
				return false;
			}
			return _playerContext.PreventRepairMaxDurabilityLoss;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameChannel.Receive<ResJoinGame>().Subscribe(OnJoinGame).AddTo(this);
		_gameChannel.Receive<NtfUpdateGamePlayer>().Where((NtfUpdateGamePlayer message) => message.ChangeData.ClientId == _gameManager.LocalClientId).Subscribe(OnUpdateGamePlayer)
			.AddTo(this);
		_gameChannel.Receive<NtfStageResultPlayerInfo>().Where((NtfStageResultPlayerInfo message) => message.PlayerInfo.ClientId == _gameManager.LocalClientId).Subscribe(OnStageResultPlayerInfo)
			.AddTo(this);
		_gameManager.RxStageState.Subscribe(OnStageStateChanged).AddTo(this);
		_eventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
	}

	private void OnDestroy()
	{
		_playerDisposable?.Dispose();
		_saveCts.Dispose();
	}

	private void OnJoinGame(ResJoinGame message)
	{
		_playerContext.LastPlayedRoomDifficulty = message.RoomDifficulty;
	}

	private void OnUpdateGamePlayer(NtfUpdateGamePlayer message)
	{
		if (message.ChangeData.Profile != null)
		{
			_playerContext.RxProfile.Value = message.ChangeData.Profile;
		}
		if (message.ChangeData.Character != null)
		{
			_playerContext.RxCharacters[message.ChangeData.Character.Id] = message.ChangeData.Character;
		}
		if (message.ChangeData.Talent != null)
		{
			_playerContext.RxTalent.Value = message.ChangeData.Talent;
		}
		if (message.ChangeData.InventorySlots != null)
		{
			_playerContext.ApplyInventorySlots(message.ChangeData.InventorySlots);
		}
		if (message.ChangeData.SafeInventorySlots != null)
		{
			_playerContext.ApplySafeInventorySlots(message.ChangeData.SafeInventorySlots);
		}
	}

	private void OnStageResultPlayerInfo(NtfStageResultPlayerInfo message)
	{
		RegisterLostGoods(message.PlayerInfo.LostGoods);
	}

	private void OnStageStateChanged(EStageState state)
	{
		switch (state)
		{
		case EStageState.SceneLoad:
			OnStageSceneLoad();
			OnStopSession();
			break;
		case EStageState.Playing:
			OnStartSession();
			break;
		default:
			OnStopSession();
			break;
		}
		void OnStageSceneLoad()
		{
			_playerContext.RxProfile.Value.IsCurrentlyInStage = _gameManager.IsInStage;
		}
		void OnStartSession()
		{
			_sessionStart = DateTime.UtcNow;
			if (_gameManager.IsInStage)
			{
				SaveAsync().Forget();
			}
		}
		void OnStopSession()
		{
			CommitPlayTime();
			_sessionStart = null;
		}
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
		if (_playerContext != null && _playerContext.Initialized)
		{
			UnlockNpcIfConditionsMet(baseEvent);
			UnlockNpcFunctionIfConditionsMet(baseEvent);
			UnlockLoungeBgmIfConditionsMet(baseEvent);
			UnlockFeatureIfConditionsMet(baseEvent);
			HandleMonsterRecordFlag(baseEvent);
		}
	}

	private void FixedUpdate()
	{
		if (!_isQuitting && _playerContext != null && _playerContext.Initialized)
		{
			_playerContext.Flush();
			if (_playerContext != null && _playerContext.HasDirty && _gameManager.IsInLounge && _gameManager.StageState == EStageState.Playing && !(DateTime.UtcNow - _lastSaveTime < _saveInterval))
			{
				SaveAsync().Forget();
			}
		}
	}

	private void AutoSave()
	{
		if (_playerContext != null && _playerContext.HasDirty && _gameManager.IsInLounge && _gameManager.StageState == EStageState.Playing && !(DateTime.UtcNow - _lastSaveTime < _saveInterval))
		{
			SaveAsync().Forget();
		}
	}

	public void Save(bool force = false)
	{
		if (_isQuitting && !force)
		{
			return;
		}
		_lastSaveTime = DateTime.UtcNow;
		if (!_isQuitting && _playerContext != null)
		{
			CommitPlayTime();
			_playerContext.ClearDirty();
			_playerContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			_saveStorage.Save(MakePlayerFileName(_playerContext.SlotIndex), _playerContext.ToData());
		}
		UpdateSlot();
		if (_accountContext != null)
		{
			_accountContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			_saveStorage.Save("AccountContext", _accountContext.ToData());
		}
	}

	public async UniTask SaveAsync()
	{
		if (_isQuitting)
		{
			return;
		}
		if (_isSaving)
		{
			_saveRequested = true;
			return;
		}
		_isSaving = true;
		try
		{
			do
			{
				_saveRequested = false;
				_lastSaveTime = DateTime.UtcNow;
				await SavePlayerAsync(_saveCts.Token);
				UpdateSlot();
				await SaveAccountAsync(_saveCts.Token);
			}
			while (_saveRequested && !_isQuitting);
		}
		finally
		{
			_isSaving = false;
		}
	}

	public void OnApplicationQuit()
	{
		if (!_isQuitting)
		{
			_isQuitting = true;
			_saveCts.Cancel();
			if (_settings.Platform == EPlatform.Local)
			{
				long ticks = DateTime.UtcNow.Ticks;
				if (_playerContext != null && _playerContext.HasDirty)
				{
					_saveStorage.Backup("PlayerContext", _playerContext.ToData(), ticks);
				}
			}
		}
	}

	private string MakeAccountFileName()
	{
		return "AccountContext";
	}

	public async UniTask<bool> LoadAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		string accountId = _appManager.AccountId;
		AccountContextData data = (await _saveStorage.LoadAsync<AccountContextData>(MakeAccountFileName(), accountId, cancellationToken)).Data;
		if (data == null)
		{
			_accountContext = CreateNewAccountContext();
		}
		else
		{
			data.AccountId = accountId;
			_accountContext = new AccountContext(data);
		}
		RxPlayerSlotIndex.Value = _accountContext.PlayerSlotIndex;
		return true;
	}

	private void SaveAccount()
	{
		if (_accountContext != null)
		{
			_accountContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			_saveStorage.Save(MakeAccountFileName(), _accountContext.ToData());
		}
	}

	public async UniTask SaveAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_accountContext != null)
		{
			_accountContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			await _saveStorage.SaveAsync(MakeAccountFileName(), _accountContext.ToData(), cancellationToken);
		}
	}

	public void SelectPlayerSlot(int slotIndex)
	{
		_accountContext.PlayerSlotIndex = slotIndex;
		RxPlayerSlotIndex.Value = slotIndex;
	}

	public async UniTask DeletePlayer(int slotIndex)
	{
		long ticks = DateTime.UtcNow.Ticks;
		await _saveStorage.DeleteAsync<PlayerContextData>(MakePlayerFileName(slotIndex), ticks, BackupPlayerFileName);
		_accountContext.PlayerSlots[slotIndex] = new PlayerSlot
		{
			SlotIndex = slotIndex
		};
		if (_accountContext.PlayerSlotIndex == slotIndex)
		{
			_playerDisposable?.Dispose();
			_playerDisposable = null;
			_playerContext?.Dispose();
			_playerContext = null;
		}
		OnPlayerSlotUpdated.OnNext(slotIndex);
	}

	private void UpdateSlot()
	{
		if (_playerContext != null)
		{
			int slotIndex = _playerContext.SlotIndex;
			_accountContext.PlayerSlots[slotIndex] = new PlayerSlot
			{
				SlotIndex = slotIndex,
				PlayerName = _playerContext.RxProfile.Value.Name,
				Level = Level,
				SaveTimeTicks = _playerContext.SaveTimeTicks,
				PlayTimeTicks = _playerContext.PlayTimeTicks,
				Version = _playerContext.Version
			};
			_accountContext.PlayTimeTicks = _accountContext.PlayerSlots.Sum((PlayerSlot s) => s.PlayTimeTicks);
		}
	}

	public async UniTask<bool> RestoreAsync(int targetSlotIndex, long saveTimeTicks)
	{
		PlayerContextData data = await _saveStorage.LoadBackupAsync<PlayerContextData>(BackupPlayerFileName, saveTimeTicks, _saveCts.Token);
		if (data == null)
		{
			return false;
		}
		if (data.AccountId != _appManager.AccountId)
		{
			return false;
		}
		data.SlotIndex = targetSlotIndex;
		await _saveStorage.SaveAsync(MakePlayerFileName(targetSlotIndex), data, _saveCts.Token);
		_accountContext.PlayerSlots[targetSlotIndex] = new PlayerSlot
		{
			SlotIndex = targetSlotIndex,
			PlayerName = data.Profile?.Name,
			Level = ((data.Profile != null) ? data.Profile.Level : 0),
			SaveTimeTicks = data.SaveTimeTicks,
			PlayTimeTicks = data.PlayTimeTicks,
			Version = data.Version
		};
		_accountContext.PlayTimeTicks = _accountContext.PlayerSlots.Sum((PlayerSlot s) => s.PlayTimeTicks);
		await SaveAccountAsync(_saveCts.Token);
		OnPlayerSlotUpdated.OnNext(targetSlotIndex);
		return true;
	}

	private AccountContext CreateNewAccountContext()
	{
		AccountContextData accountContextData = new AccountContextData
		{
			AccountId = _appManager.AccountId,
			SaveTimeTicks = DateTime.UtcNow.Ticks,
			CreateTimeTicks = DateTime.UtcNow.Ticks,
			PlayerSlots = CreateEmptySlots(),
			LastPlayerSlotIndex = 0
		};
		return new AccountContext(accountContextData);
		static PlayerSlot[] CreateEmptySlots()
		{
			return Enumerable.Range(0, 6).Select((int i) => new PlayerSlot
			{
				SlotIndex = i
			}).ToArray();
		}
	}

	public void SetDialogReplayRecord(string messageKey, int selectedIndex)
	{
		long num = TDialogReplay.MakeKey(messageKey);
		if (num < 0)
		{
			_logger.Error("Invalid dialogReplayId for messageKey: " + messageKey);
		}
		else
		{
			_playerContext.DialogReplayRecords[num] = new DialogReplayRecord
			{
				Id = num,
				SelectedIndex = (byte)selectedIndex
			};
		}
	}

	public bool CheckStateConditions(EventCondition[] eventConditions)
	{
		if (eventConditions == null || eventConditions.Length == 0)
		{
			return true;
		}
		return eventConditions.All(CheckStateCondition);
	}

	public EventCondition GetFailedCondition(EventCondition[] eventConditions)
	{
		if (eventConditions != null && eventConditions.Length != 0)
		{
			for (int i = 0; i < eventConditions.Length; i++)
			{
				if (!CheckStateCondition(eventConditions[i]))
				{
					return eventConditions[i];
				}
			}
		}
		return default(EventCondition);
	}

	public bool CheckStateCondition(EventCondition eventCondition)
	{
		if (eventCondition.Type == EEventConditionType.None)
		{
			return true;
		}
		if (eventCondition.Type.GetValueType() != EEventConditionValueType.State)
		{
			return false;
		}
		switch (eventCondition.Type)
		{
		case EEventConditionType.UserLevel:
			return (int)eventCondition.Value1 <= Level;
		case EEventConditionType.CampaignCleared:
			return IsClearedCampaign((int)eventCondition.Value1);
		case EEventConditionType.MissionCleared:
			return IsClearedMission((int)eventCondition.Value1);
		case EEventConditionType.LoungeQuestCleared:
			return IsClearedLoungeQuest((int)eventCondition.Value1);
		case EEventConditionType.ItemCollected:
			return IsCollectedItem((int)eventCondition.Value1);
		case EEventConditionType.Talent:
			return HasTalent((int)eventCondition.Value1);
		case EEventConditionType.ItemUsed:
			return IsItemUsed((int)eventCondition.Value1);
		case EEventConditionType.AreaUnlocked:
			return IsAreaUnlocked((int)eventCondition.Value1);
		case EEventConditionType.AreaDecorationUnlocked:
			return IsAreaDecorationUnlocked((int)eventCondition.Value1);
		case EEventConditionType.NpcLevel:
		{
			int value;
			if (!_playerContext.NpcLevels.TryGetValue((int)eventCondition.Value1, out value))
			{
				value = 0;
			}
			return (int)eventCondition.Value2 <= value;
		}
		default:
			return false;
		}
	}

	public bool IsQuestCleared(EQuestType questType, int tableId)
	{
		switch (questType)
		{
		case EQuestType.Campaign:
			return IsClearedCampaign(tableId);
		case EQuestType.Mission:
			return IsClearedMission(tableId);
		case EQuestType.Lounge:
			return IsClearedLoungeQuest(tableId);
		default:
			return false;
		}
	}

	public bool SetClearedCampaign(int campaignId)
	{
		int bitFlagIndex = TCampaign.Table[campaignId].BitFlagIndex;
		if (_playerContext.ClearedCampaignFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.ClearedCampaignFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsClearedCampaign(int campaignId)
	{
		int bitFlagIndex = TCampaign.Table[campaignId].BitFlagIndex;
		if (_playerContext.ClearedCampaignFlags == null)
		{
			return false;
		}
		return _playerContext.ClearedCampaignFlags.IsSet(bitFlagIndex);
	}

	private static int PopCount(ulong v)
	{
		int num = 0;
		while (v != 0)
		{
			num += (int)(v & 1);
			v >>= 1;
		}
		return num;
	}

	public bool SetItemCollection(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		if (tItem.BitFlagIndex < 0)
		{
			return false;
		}
		if (_playerContext.ItemCollectionFlags.IsSet(tItem.BitFlagIndex))
		{
			return false;
		}
		_playerContext.ItemCollectionFlags.Set(tItem.BitFlagIndex);
		OnItemCollected.OnNext(itemId);
		return true;
	}

	public bool IsCollectedItem(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		if (tItem.BitFlagIndex < 0)
		{
			return true;
		}
		return _playerContext.ItemCollectionFlags.IsSet(tItem.BitFlagIndex);
	}

	public bool SetEquipmentCollection(int equipmentId)
	{
		TEquipment tEquipment = TEquipment.Table[equipmentId];
		if (tEquipment.BitFlagIndex < 0)
		{
			return false;
		}
		if (_playerContext.EquipmentCollectionFlags.IsSet(tEquipment.BitFlagIndex))
		{
			return false;
		}
		_playerContext.EquipmentCollectionFlags.Set(tEquipment.BitFlagIndex);
		OnEquipmentCollected.OnNext(equipmentId);
		return true;
	}

	public bool IsCollectedEquipment(int equipmentId)
	{
		TEquipment tEquipment = TEquipment.Table[equipmentId];
		if (tEquipment.BitFlagIndex < 0)
		{
			return true;
		}
		return _playerContext.EquipmentCollectionFlags.IsSet(tEquipment.BitFlagIndex);
	}

	public void ToggleMarkGoods(Goods goods)
	{
		if (goods == null)
		{
			return;
		}
		BitFlags bitFlags;
		int bitFlagIndex;
		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			bitFlags = _playerContext.ItemMarkFlags;
			bitFlagIndex = TItem.Table[goods.Id].BitFlagIndex;
			break;
		case EGoodsCategory.Equipment:
			bitFlags = _playerContext.EquipmentMarkFlags;
			bitFlagIndex = TEquipment.Table[goods.Id].BitFlagIndex;
			break;
		case EGoodsCategory.Bag:
			bitFlags = _playerContext.BagMarkFlags;
			bitFlagIndex = TBag.Table[goods.Id].BitFlagIndex;
			break;
		default:
			return;
		}
		bool item = bitFlags.Toggle(bitFlagIndex);
		OnGoodsMarkChanged.OnNext((goods.Type, goods.Id, item));
	}

	public bool IsMarked(Goods goods)
	{
		if (goods == null)
		{
			return false;
		}
		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			return _playerContext.ItemMarkFlags.IsSet(TItem.Table[goods.Id].BitFlagIndex);
		case EGoodsCategory.Equipment:
			return _playerContext.EquipmentMarkFlags.IsSet(TEquipment.Table[goods.Id].BitFlagIndex);
		case EGoodsCategory.Bag:
			return _playerContext.BagMarkFlags.IsSet(TBag.Table[goods.Id].BitFlagIndex);
		default:
			return false;
		}
	}

	public bool SetItemUsed(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		if (tItem.BitFlagIndex < 0)
		{
			return false;
		}
		if (_playerContext.ItemUsedFlags.IsSet(tItem.BitFlagIndex))
		{
			return false;
		}
		_playerContext.ItemUsedFlags.Set(tItem.BitFlagIndex);
		OnItemFirstUsed.OnNext(itemId);
		return true;
	}

	public bool IsItemUsed(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		if (tItem.BitFlagIndex < 0)
		{
			return false;
		}
		return _playerContext.ItemUsedFlags.IsSet(tItem.BitFlagIndex);
	}

	public void ClearItemUsed(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		if (_playerContext.ItemUsedFlags.IsSet(tItem.BitFlagIndex))
		{
			_playerContext.ItemUsedFlags.ClearIndex(tItem.BitFlagIndex);
		}
	}

	public bool SetClearedMission(int missionId)
	{
		int bitFlagIndex = TMission.Table[missionId].BitFlagIndex;
		if (_playerContext.ClearedMissionFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.ClearedMissionFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsClearedMission(int missionId)
	{
		return _playerContext.ClearedMissionFlags.IsSet(TMission.Table[missionId].BitFlagIndex);
	}

	public bool SetClearedLoungeQuest(int loungeQuestId)
	{
		int bitFlagIndex = TLoungeQuest.Table[loungeQuestId].BitFlagIndex;
		if (_playerContext.ClearedLoungeQuestFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.ClearedLoungeQuestFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsClearedLoungeQuest(int loungeQuestId)
	{
		return _playerContext.ClearedLoungeQuestFlags.IsSet(TLoungeQuest.Table[loungeQuestId].BitFlagIndex);
	}

	public bool SetNpcUnlocked(int npcId)
	{
		int bitFlagIndex = TNpc.Table[npcId].BitFlagIndex;
		if (_playerContext.NpcUnlockedFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.NpcUnlockedFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsNpcUnlocked(int npcId)
	{
		return _playerContext.NpcUnlockedFlags.IsSet(TNpc.Table[npcId].BitFlagIndex);
	}

	public bool IsAreaUnlocked(int areaId)
	{
		int bitFlagIndex = TArea.Table[areaId].BitFlagIndex;
		if (_playerContext.AreaFlags == null)
		{
			return false;
		}
		return _playerContext.AreaFlags.IsSet(bitFlagIndex);
	}

	public void SetAreaUnlocked(int areaId)
	{
		_playerContext.AreaFlags.Set(TArea.Table[areaId].BitFlagIndex);
	}

	public bool IsAreaDecorationUnlocked(int areaDecorationSettingId)
	{
		int bitFlagIndex = TAreaDecoration.Table[areaDecorationSettingId].BitFlagIndex;
		if (_playerContext.AreaDecorationUnlockedFlags == null)
		{
			return false;
		}
		return _playerContext.AreaDecorationUnlockedFlags.IsSet(bitFlagIndex);
	}

	public void SetAreaDecorationUnlocked(int areaDecorationSettingId)
	{
		_playerContext.AreaDecorationUnlockedFlags.Set(TAreaDecoration.Table[areaDecorationSettingId].BitFlagIndex);
	}

	public void SetActiveAreaDecoration(int groupId, int areaDecorationSettingId)
	{
		_playerContext.ActiveAreaDecorations[groupId] = areaDecorationSettingId;
	}

	public int GetNpcLevel(int npcId)
	{
		int value;
		if (!_playerContext.NpcLevels.TryGetValue(npcId, out value))
		{
			return 0;
		}
		return value;
	}

	public void SetNpcLevel(int npcId, int level)
	{
		_playerContext.NpcLevels[npcId] = level;
		_eventSystem.Trigger(new NpcLevelChangedEvent(npcId, level));
	}

	public bool SetNpcTalkViewed(int npcTalkId)
	{
		int bitFlagIndex = TNpcTalk.Table[npcTalkId].BitFlagIndex;
		if (_playerContext.ViewedNpcTalkFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.ViewedNpcTalkFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsNpcTalkViewed(int npcTalkId)
	{
		return _playerContext.ViewedNpcTalkFlags.IsSet(TNpcTalk.Table[npcTalkId].BitFlagIndex);
	}

	public bool IsLoungeBgmUnlocked(int bgmId)
	{
		TLoungeBgm tLoungeBgm = TLoungeBgm.Table[bgmId];
		if (tLoungeBgm.BitFlagIndex < 0)
		{
			return true;
		}
		if (_playerContext.LoungeBgmUnlockedFlags == null)
		{
			return false;
		}
		return _playerContext.LoungeBgmUnlockedFlags.IsSet(tLoungeBgm.BitFlagIndex);
	}

	public bool SetLoungeBgmUnlocked(int bgmId)
	{
		int bitFlagIndex = TLoungeBgm.Table[bgmId].BitFlagIndex;
		if (_playerContext.LoungeBgmUnlockedFlags.IsSet(bitFlagIndex))
		{
			return false;
		}
		_playerContext.LoungeBgmUnlockedFlags.Set(bitFlagIndex);
		return true;
	}

	public bool IsNpcFunctionUnlocked(int npcFunctionId)
	{
		TNpcFunction tNpcFunction = TNpcFunction.Table[npcFunctionId];
		if (tNpcFunction.BitFlagIndex < 0)
		{
			return true;
		}
		if (_playerContext.NpcFunctionUnlockedFlags == null)
		{
			return false;
		}
		return _playerContext.NpcFunctionUnlockedFlags.IsSet(tNpcFunction.BitFlagIndex);
	}

	public bool SetNpcFunctionUnlocked(int npcFunctionId)
	{
		TNpcFunction tNpcFunction = TNpcFunction.Table[npcFunctionId];
		if (tNpcFunction.BitFlagIndex < 0)
		{
			return false;
		}
		if (_playerContext.NpcFunctionUnlockedFlags.IsSet(tNpcFunction.BitFlagIndex))
		{
			return false;
		}
		_playerContext.NpcFunctionUnlockedFlags.Set(tNpcFunction.BitFlagIndex);
		return true;
	}

	public bool IsFeatureUnlocked(EFeatureUnlock feature)
	{
		if (!TFeatureUnlock.Table.ContainsKey(feature))
		{
			return true;
		}
		if (TFeatureUnlock.Table[feature].UnlockConditions == null)
		{
			return true;
		}
		if (_playerContext.FeatureUnlockedFlags == null)
		{
			return false;
		}
		return _playerContext.FeatureUnlockedFlags.IsSet((int)feature);
	}

	public bool SetFeatureUnlocked(EFeatureUnlock feature)
	{
		if (_playerContext.FeatureUnlockedFlags.IsSet((int)feature))
		{
			return false;
		}
		_playerContext.FeatureUnlockedFlags.Set((int)feature);
		return true;
	}

	public bool IsKnownGoods(Goods goods)
	{
		switch (goods.Type)
		{
		case EGoodsType.Blueprint:
		case EGoodsType.MusicDisc:
			return IsItemUsed(goods.Id);
		case EGoodsType.Weapon:
		case EGoodsType.Accessory:
		case EGoodsType.Artifact:
			return IsCollectedEquipment(goods.Id);
		case EGoodsType.Bag:
			return true;
		default:
			return IsCollectedItem(goods.Id);
		}
	}

	public bool IsUnknownGoods(Goods goods)
	{
		switch (goods.Type)
		{
		case EGoodsType.Blueprint:
		case EGoodsType.MusicDisc:
			return !IsItemUsed(goods.Id);
		case EGoodsType.Weapon:
		case EGoodsType.Accessory:
		case EGoodsType.Artifact:
			return !IsCollectedEquipment(goods.Id);
		case EGoodsType.Bag:
			return false;
		default:
			return !IsCollectedItem(goods.Id);
		}
	}

	public void AddGoodsToStash(Goods goods)
	{
		if (CanAddGoodsToStash(goods) && _playerContext.RxStashSlots.TryPushGoods(goods, -1))
		{
			return;
		}
		int num = _playerContext.RxStorageSlots.AllocateIndex();
		_playerContext.RxStorageSlots.Add(num, new GoodsSlot
		{
			SlotIndex = num,
			Goods = goods
		});
	}

	public bool TryGoodsToStash(Goods goods, int slotIndex = -1)
	{
		if (!CanAddGoodsToStash(goods))
		{
			return false;
		}
		return _playerContext.RxStashSlots.TryPushGoods(goods, slotIndex);
	}

	public int GetStashEmptySlotCount()
	{
		return _gameManager.MyPlayer.StashSlotCount - _playerContext.RxStashSlots.Count;
	}

	public bool CanAddGoodsToStash(Goods goods)
	{
		if (_playerContext.RxStashSlots.Count >= _gameManager.MyPlayer.StashSlotCount)
		{
			switch (goods.Category)
			{
			case EGoodsCategory.Item:
				return _playerContext.RxStashSlots.Values.Any((GoodsSlot slot) => slot.Id == goods.Id);
			case EGoodsCategory.Equipment:
			case EGoodsCategory.Bag:
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			case EGoodsCategory.Currency:
				break;
			}
		}
		return true;
	}

	public int[] GetMovableInventorySlotIndicesForStash()
	{
		int stashSlotCount = _gameManager.MyPlayer.StashSlotCount;
		int num = _playerContext.RxStashSlots.Count;
		HashSet<int> hashSet = new HashSet<int>(_playerContext.RxStashSlots.Values.Where((GoodsSlot s) => s.Category == EGoodsCategory.Item).Select((GoodsSlot s) => s.Id));
		List<int> list = new List<int>();
		foreach (GoodsSlot item in _playerContext.RxInventorySlots.Values.Where((GoodsSlot s) => !s.IsEmpty).OrderBy((GoodsSlot s) => s.SlotIndex))
		{
			bool flag;
			bool flag2;
			switch (item.Goods.Category)
			{
			case EGoodsCategory.Item:
			{
				bool flag3 = hashSet.Contains(item.Id);
				flag = flag3 || num < stashSlotCount;
				flag2 = !flag3;
				break;
			}
			case EGoodsCategory.Equipment:
			case EGoodsCategory.Bag:
				flag = num < stashSlotCount;
				flag2 = true;
				break;
			default:
				continue;
			}
			if (flag)
			{
				list.Add(item.SlotIndex);
				if (flag2)
				{
					num++;
				}
			}
		}
		return list.ToArray();
	}

	public long GetTotalBagCount(int bagId)
	{
		return _playerContext.RxInventorySlots.GetBagCount(bagId) + _playerContext.RxSafeInventorySlots.GetBagCount(bagId) + _playerContext.RxStashSlots.GetBagCount(bagId);
	}

	public long GetTotalEmptyBagCount(int bagId)
	{
		return _playerContext.RxInventorySlots.GetEmptyBagCount(bagId) + _playerContext.RxSafeInventorySlots.GetEmptyBagCount(bagId) + _playerContext.RxStashSlots.GetEmptyBagCount(bagId);
	}

	public string GetBagConsumeFailToast(IEnumerable<Goods> consumeGoods)
	{
		EBagConsumeFailReason reason = BagConsumeRule.Check(consumeGoods, GetTotalEmptyBagCount, GetTotalBagCount);
		return BagConsumeRule.GetFailMessage(reason);
	}

	public void ConsumeStashGoods(Goods consumeGoods)
	{
		Goods[] pullGoods;
		if (!_playerContext.RxStashSlots.TryPullGoods(consumeGoods, out pullGoods))
		{
			_logger.Error(string.Format("Failed to consume stash goods. {0}.", consumeGoods.Type));
		}
	}

	public void ConsumeStashGoods(Goods[] consumeGoodsArray)
	{
		if (consumeGoodsArray == null || !consumeGoodsArray.Any())
		{
			return;
		}
		for (int i = 0; i < consumeGoodsArray.Length; i++)
		{
			Goods goods = consumeGoodsArray[i];
			Goods[] pullGoods;
			if (!_playerContext.RxStashSlots.TryPullGoods(goods, out pullGoods))
			{
				_logger.Error(string.Format("Failed to consume stash goods. {0}.", goods.Type));
			}
		}
	}

	public void ConsumeStashSlot(int slotIndex, Goods consumeGoods)
	{
		if (consumeGoods.IsItem())
		{
			long itemCount = _playerContext.RxStashSlots.GetItemCount(consumeGoods.Id);
			if (itemCount < consumeGoods.Count)
			{
				_logger.Error(string.Format("Failed to consume stash item {0}: Not enough goods. Owned: {1}.", consumeGoods, itemCount));
				return;
			}
			Goods[] pullGoods;
			_playerContext.RxStashSlots.TryPullGoods(consumeGoods, out pullGoods);
			return;
		}
		GoodsSlot value;
		if (!_playerContext.RxStashSlots.TryGetValue(slotIndex, out value))
		{
			_logger.Error(string.Format("Failed to consume stash slot {0}: Slot is empty.", slotIndex));
			return;
		}
		if (value.Type == consumeGoods.Type && value.Id == consumeGoods.Id)
		{
			Goods pullGoods2;
			_playerContext.RxStashSlots.TryPullSlot(slotIndex, out pullGoods2);
			return;
		}
		_logger.Error(string.Format("Failed to consume stash slot {0}: Slot goods {1}, consume goods {2}.", slotIndex, value, consumeGoods));
	}

	public void OrganizeStash()
	{
		_playerContext.RxStashSlots.Organize();
	}

	public void AddGoodsToStorage(Goods[] goods)
	{
		goods?.ForEach(AddGoodsToStorage);
	}

	public void AddGoodsToStorage(Goods goods)
	{
		int num = _playerContext.RxStorageSlots.AllocateIndex();
		_playerContext.RxStorageSlots.Add(num, new GoodsSlot
		{
			SlotIndex = num,
			Goods = goods
		});
	}

	public int GetStorageSlotCount()
	{
		return _playerContext.RxStorageSlots.Count;
	}

	public bool HasStorageSlots()
	{
		return _playerContext.RxStorageSlots.Any();
	}

	public void RegisterLostGoods(IList<Goods> lostGoods)
	{
		DecreaseExpireAndCleanup(_playerContext);
		if (lostGoods != null && lostGoods.Count > 0)
		{
			AppendLostGoods(_playerContext, lostGoods);
		}
		List<(int, float, float)> selected = BuildWeightedCandidates(_playerContext).OrderByDescending(((int, float, float) x) => x.Item2).Take(TConst.LostGoodsStorageDefaultSlotCount.IntValue).ToList();
		AddSelectInfo(_playerContext, selected);
	}

	private static void DecreaseExpireAndCleanup(PlayerContext ctx)
	{
		if (ctx.RxLostGoods == null || ctx.RxLostGoods.Count == 0)
		{
			return;
		}
		foreach (int item in ctx.RxLostGoods.Keys.ToList())
		{
			LostGoods lostGoods = ctx.RxLostGoods[item];
			lostGoods.ExpireTurn--;
			ctx.RxLostGoods[item] = lostGoods;
		}
		foreach (int item2 in (from kv in ctx.RxLostGoods
			where kv.Value.ExpireTurn < 1
			select kv.Key).ToList())
		{
			ctx.RxLostGoods.Remove(item2);
		}
	}

	private static void AppendLostGoods(PlayerContext ctx, IList<Goods> goodsList)
	{
		int num = ((ctx.RxLostGoods != null && ctx.RxLostGoods.Keys.Any()) ? (ctx.RxLostGoods.Keys.Max() + 1) : 0);
		foreach (Goods goods in goodsList)
		{
			BagGoods bagGoods = goods as BagGoods;
			if (bagGoods != null)
			{
				GoodsSlot[] goodsSlots = bagGoods.GoodsSlots;
				for (int i = 0; i < goodsSlots.Length; i++)
				{
					GoodsSlot goodsSlot = goodsSlots[i];
					if (!goodsSlot.IsEmpty && !goodsSlot.Goods.IsEventItem())
					{
						AppendLostGoodsEntry(ctx, num, goodsSlot.Goods.CloneWithoutGain());
						num++;
					}
				}
				BagGoods goods2 = GoodsCreator.MakeBagGoods(bagGoods.Id, bagGoods.Count);
				AppendLostGoodsEntry(ctx, num, goods2);
				num++;
			}
			else if (!goods.IsEventItem())
			{
				AppendLostGoodsEntry(ctx, num, goods.CloneWithoutGain());
				num++;
			}
		}
	}

	private static void AppendLostGoodsEntry(PlayerContext ctx, int idx, Goods goods)
	{
		ctx.RxLostGoods[idx] = new LostGoods
		{
			SlotIndex = idx,
			Goods = goods,
			ExpireTurn = TConst.LostGoodsRetentionExitCount.IntValue
		};
	}

	private static IEnumerable<(int, float, float)> BuildWeightedCandidates(PlayerContext playerContext)
	{
		return playerContext.RxLostGoods.Select(delegate(KeyValuePair<int, LostGoods> kv)
		{
			Goods goods = kv.Value.Goods;
			EGoodsGradeType grade = goods.GetGrade();
			float num = 1f;
			float num2 = 1f;
			bool flag = false;
			if (goods != null)
			{
				if (goods is ArtifactGoods)
				{
					num2 = TConst.LostGoodsArtifactWeightMultiplier.FloatValue;
					flag = true;
				}
				else if (goods is AccessoryGoods || goods is WeaponGoods)
				{
					num2 = TConst.LostGoodsEquipmentWeightMultiplier.FloatValue;
				}
				else if (goods is ConsumableGoods)
				{
					num2 = TConst.LostGoodsConsumableWeightMultiplier.FloatValue;
				}
				else if (goods is BagGoods)
				{
					num2 = TConst.LostGoodsBagWeightMultiplier.FloatValue;
				}
			}
			switch (grade)
			{
			case EGoodsGradeType.Normal:
				num = TConst.LostGoodsGradeNormalWeightMultiplier.FloatValue;
				break;
			case EGoodsGradeType.Rare:
				num = TConst.LostGoodsGradeRareWeightMultiplier.FloatValue;
				break;
			case EGoodsGradeType.Elite:
				num = TConst.LostGoodsGradeEliteWeightMultiplier.FloatValue;
				break;
			case EGoodsGradeType.Epic:
				num = TConst.LostGoodsGradeEpicWeightMultiplier.FloatValue;
				break;
			case EGoodsGradeType.Legend:
				num = TConst.LostGoodsGradeLegendWeightMultiplier.FloatValue;
				break;
			case EGoodsGradeType.Unique:
				num = TConst.LostGoodsGradeUniqueWeightMultiplier.FloatValue;
				break;
			}
			float item = SomeNet.Utility.RandomUtility.GetRandomFloat(0f, num + num2);
			float item2 = ((!flag) ? UnityEngine.Random.Range(TConst.LostGoodsPriceMultiplierMin.FloatValue, TConst.LostGoodsPriceMultiplierMax.FloatValue) : UnityEngine.Random.Range(TConst.LostArtifactPriceMultiplierMin.FloatValue, TConst.LostArtifactPriceMultiplierMax.FloatValue));
			return (kv.Key, item, item2);
		});
	}

	private static void AddSelectInfo(PlayerContext playerContext, List<(int SlotIndex, float Score, float PriceMultiplier)> selected)
	{
		playerContext.RxSelectLostGoodsInfos.Clear();
		foreach (var item in selected)
		{
			playerContext.RxSelectLostGoodsInfos.Add(new LostItemSelectInfo
			{
				SlotIndex = item.SlotIndex,
				PriceMultiplier = item.PriceMultiplier
			});
		}
	}

	public void RebuildSelectInfos()
	{
		List<(int, float, float)> selected = BuildWeightedCandidates(_playerContext).OrderByDescending(((int, float, float) x) => x.Item2).Take(TConst.LostGoodsStorageDefaultSlotCount.IntValue).ToList();
		AddSelectInfo(_playerContext, selected);
	}

	public bool TryRemoveLostGoods(int slotIndex, out Goods goods)
	{
		goods = null;
		LostGoods value;
		if (!((IDictionary<int, LostGoods>)_playerContext.RxLostGoods).Remove(slotIndex, out value))
		{
			_logger.Error(string.Format("Failed to remove lost goods for SlotIndex={0}", slotIndex));
			return false;
		}
		goods = value.Goods;
		LostItemSelectInfo lostItemSelectInfo = _playerContext.RxSelectLostGoodsInfos.FirstOrDefault((LostItemSelectInfo info) => info.SlotIndex == slotIndex);
		if (lostItemSelectInfo == null)
		{
			_logger.Error(string.Format("Failed to find selected lost goods info for SlotIndex={0}", slotIndex));
			return false;
		}
		_playerContext.RxSelectLostGoodsInfos.Remove(lostItemSelectInfo);
		return true;
	}

	public int GetLoungeQuestCount()
	{
		return _playerContext.LoungeQuests.Count;
	}

	public IList<int> GetLoungeQuestIds()
	{
		return _playerContext.LoungeQuests.Keys.ToList();
	}

	public IList<LoungeQuest> GetLoungeQuests()
	{
		return _playerContext.LoungeQuests.Values.ToList();
	}

	public long GetLoungeConditionProgress(EventCondition eventCondition, long currentProgress)
	{
		EEventConditionValueType valueType = eventCondition.Type.GetValueType();
		if (valueType == EEventConditionValueType.State)
		{
			return CheckStateCondition(eventCondition) ? 1 : 0;
		}
		if (valueType == EEventConditionValueType.Increment)
		{
			return currentProgress;
		}
		if (valueType != EEventConditionValueType.Assign)
		{
			return 0L;
		}
		switch (eventCondition.Type)
		{
		case EEventConditionType.LuaProgress:
			return currentProgress;
		case EEventConditionType.MyItemCountInInventory:
			return _playerContext.RxInventorySlots.GetItemCount((int)eventCondition.Value1) + _playerContext.RxSafeInventorySlots.GetItemCount((int)eventCondition.Value1);
		case EEventConditionType.MyItemCountTotal:
			return _playerContext.RxInventorySlots.GetItemCount((int)eventCondition.Value1) + _playerContext.RxSafeInventorySlots.GetItemCount((int)eventCondition.Value1) + _playerContext.RxStashSlots.GetItemCount((int)eventCondition.Value1);
		default:
			// paramName literal 'Type' read from the metadata-usage cell 0x1861013b8 at 0x18076d71e (cold block after the method's int3), actualValue is the boxed EEventConditionType, message null
			throw new ArgumentOutOfRangeException("Type", eventCondition.Type, null);
		}
	}

	private void HandleMonsterRecordFlag(IBaseEvent baseEvent)
	{
		MonsterKillEvent monsterKillEvent = baseEvent as MonsterKillEvent;
		if (monsterKillEvent != null)
		{
			HandleMonsterKill(monsterKillEvent);
			return;
		}
		MonsterAggroedEvent monsterAggroedEvent = baseEvent as MonsterAggroedEvent;
		if (monsterAggroedEvent != null)
		{
			HandleMonsterAggroed(monsterAggroedEvent);
			return;
		}
		LocalPlayerAttackEvent localPlayerAttackEvent = baseEvent as LocalPlayerAttackEvent;
		if (localPlayerAttackEvent != null)
		{
			HandleLocalPlayerAttack(localPlayerAttackEvent);
		}
		void HandleLocalPlayerAttack(LocalPlayerAttackEvent localPlayerAttackEvent2)
		{
			if (localPlayerAttackEvent2.IsPenalty)
			{
				AddMonsterRecordFlags(localPlayerAttackEvent2.MonsterId, EMonsterRecordFlags.Weakness);
			}
		}
		void HandleMonsterAggroed(MonsterAggroedEvent monsterAggroedEvent2)
		{
			AddMonsterRecordFlags(monsterAggroedEvent2.MonsterId, EMonsterRecordFlags.BaseInfo);
		}
		void HandleMonsterKill(MonsterKillEvent monsterKillEvent2)
		{
			if (MyCharacterModel.ClientId != monsterKillEvent2.SourceClientId)
			{
				return;
			}
			TMonster monsterData = TMonster.Table[monsterKillEvent2.MonsterId];
			int groupId = monsterData.GroupId;
			MonsterRecord monsterRecord;
			if (!_playerContext.MonsterRecords.TryGetValue(groupId, out monsterRecord))
			{
				monsterRecord = new MonsterRecord
				{
					MonsterGroupId = groupId
				};
			}
			monsterRecord.KillCount = (byte)Math.Min(monsterRecord.KillCount + 1, 100);
			_playerContext.MonsterRecords[groupId] = monsterRecord;
			TryRegisterDescriptionRecord();
			TryRegisterWeaknessRecord();
			void TryRegisterDescriptionRecord()
			{
				if ((monsterRecord.Flags & EMonsterRecordFlags.Description) != 0)
				{
					return;
				}
				byte descriptionRevealKillCount = monsterData.DescriptionRevealKillCount;
				if (descriptionRevealKillCount == 0 || monsterRecord.KillCount < descriptionRevealKillCount)
				{
					float descriptionRevealPercent = monsterData.DescriptionRevealPercent;
					if (descriptionRevealPercent > 0f && SomeNet.Utility.RandomUtility.GetRandomBoolFromPercentage(descriptionRevealPercent))
					{
						monsterRecord.Flags |= EMonsterRecordFlags.Description;
					}
				}
				else
				{
					monsterRecord.Flags |= EMonsterRecordFlags.Description;
				}
			}
			void TryRegisterWeaknessRecord()
			{
				if ((monsterRecord.Flags & EMonsterRecordFlags.Weakness) != 0)
				{
					return;
				}
				float weaknessRevealPercent = monsterData.WeaknessRevealPercent;
				if (weaknessRevealPercent > 0f && SomeNet.Utility.RandomUtility.GetRandomBoolFromPercentage(weaknessRevealPercent))
				{
					monsterRecord.Flags |= EMonsterRecordFlags.Weakness;
				}
			}
		}
	}

	public bool IsUnknownMonster(int monsterId)
	{
		return GetMonsterRecordFlags(monsterId) == EMonsterRecordFlags.None;
	}

	public void SetMonsterCollection(int monsterId)
	{
		AddMonsterRecordFlags(monsterId, EMonsterRecordFlags.BaseInfo | EMonsterRecordFlags.Weakness | EMonsterRecordFlags.Description);
	}

	public bool HasMonsterRecordFlag(int monsterId, EMonsterRecordFlags flag)
	{
		return (GetMonsterRecordFlags(monsterId) & flag) == flag;
	}

	public EMonsterRecordFlags GetMonsterRecordFlags(int monsterId)
	{
		TMonster tMonster = TMonster.Table[monsterId];
		MonsterRecord value;
		if (!_playerContext.MonsterRecords.TryGetValue(tMonster.GroupId, out value))
		{
			return EMonsterRecordFlags.None;
		}
		return value.Flags;
	}

	private void AddMonsterRecordFlags(int monsterId, EMonsterRecordFlags flag)
	{
		int groupId = TMonster.Table[monsterId].GroupId;
		MonsterRecord value;
		if (!_playerContext.MonsterRecords.TryGetValue(groupId, out value))
		{
			value = new MonsterRecord
			{
				MonsterGroupId = groupId
			};
		}
		if ((value.Flags & flag) != flag)
		{
			value.Flags |= flag;
			_playerContext.MonsterRecords[groupId] = value;
			OnMonsterRecordUpdated.OnNext(groupId);
		}
	}

	public void SetDifficulty(ERoomDifficulty roomDifficulty)
	{
		if (_playerContext != null)
		{
			_playerContext.Difficulty = roomDifficulty;
		}
	}

	public void SetLastPlayedRoomDifficulty(ERoomDifficulty roomDifficulty)
	{
		if (_playerContext != null)
		{
			_playerContext.LastPlayedRoomDifficulty = roomDifficulty;
		}
	}

	private void CommitPlayTime()
	{
		if (_playerContext != null && _sessionStart.HasValue)
		{
			DateTime utcNow = DateTime.UtcNow;
			_playerContext.PlayTimeTicks += (utcNow - _sessionStart.Value).Ticks;
			_sessionStart = utcNow;
		}
	}

	private string MakePlayerFileName(int playerSlotIndex)
	{
		return string.Format("{0}_{1}", "PlayerContext", playerSlotIndex);
	}

	public void CreatePlayer(string userName, ERoomDifficulty difficulty)
	{
		int value = RxPlayerSlotIndex.Value;
		PlayerContextData playerContextData = CreatePlayerContextData(value);
		playerContextData.Version = _appManager.Version;
		InitPlayerContext(new PlayerContext(playerContextData));
		Profile profile = _playerContext.RxProfile.Value.Clone();
		profile.Name = userName;
		_playerContext.RxProfile.Value = profile;
		_playerContext.ClearDirty();
		_playerContext.Difficulty = difficulty;
		_playerContext.LastPlayedRoomDifficulty = difficulty;
	}

	public async UniTask<ELoadResult> LoadPlayerAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		using (new BlockScope(true, false))
		{
			if (_playerContext == null || _playerContext.SlotIndex != PlayerSlotIndex)
			{
				string accountId = _appManager.AccountId;
				StorageLoadResult<PlayerContextData> storageLoadResult = await _saveStorage.LoadAsync<PlayerContextData>(PlayerFileName, accountId, cancellationToken);
				if (storageLoadResult.LoadResult != ELoadResult.Success)
				{
					return storageLoadResult.LoadResult;
				}
				PlayerContextData data = storageLoadResult.Data;
				data.SlotIndex = PlayerSlotIndex;
				if (data.Version > _appManager.Version)
				{
					return ELoadResult.VersionMismatch;
				}
				Patch(data.Version, data);
				data.Version = _appManager.Version;
				RepairPlayerData(data);
				PlayerContext context = new PlayerContext(data);
				InitPlayerContext(context);
				UnlockNpcsFromCurrentState();
				UnlockNpcFunctionsFromCurrentState();
				UnlockLoungeBgmsFromCurrentState();
				UnlockFeaturesFromCurrentState();
			}
			if (Profile.IsCurrentlyInStage)
			{
				ApplyStageDead();
			}
			return ELoadResult.Success;
		}
	}

	private void InitPlayerContext(PlayerContext context)
	{
		_playerDisposable?.Dispose();
		_playerDisposable = new CompositeDisposable();
		_playerContext?.Dispose();
		_playerContext = context;
		_playerContext.OnUpdateAcquisition.Subscribe(OnUpdateAcquisition).AddTo(_playerDisposable);
		_lockedNpcIds = null;
		_lockedBgmIds = null;
		_lockedNpcFunctionIds = null;
		_lockedFeatures = null;
		OnPlayerInitialized.OnNext(Unit.Default);
	}

	private void ApplyStageDead()
	{
		List<Goods> list = _playerContext.RxInventorySlots.ApplyStageDead(_playerContext.LastPlayedRoomDifficulty);
		(Character, Goods[]) tuple = _playerContext.RxCharacters[SelectedCharacterId].ApplyStageDead(_playerContext.LastPlayedRoomDifficulty);
		Character item = tuple.Item1;
		if (tuple.Item2.Length != 0)
		{
			list.AddRange(tuple.Item2);
		}
		RegisterLostGoods(list);
		_playerContext.RxInventorySlots.NormalizeCount();
		_playerContext.RxSafeInventorySlots.NormalizeCount();
		_playerContext.RxCharacters[_playerContext.RxCharacters[SelectedCharacterId].Id] = item;
		_playerContext.RxCharacters[SelectedCharacterId].NormalizeCount();
		_playerContext.RxProfile.Value.IsCurrentlyInStage = false;
	}

	private void SavePlayer()
	{
		if (!_isQuitting && _playerContext != null)
		{
			CommitPlayTime();
			_playerContext.ClearDirty();
			_playerContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			_saveStorage.Save(MakePlayerFileName(_playerContext.SlotIndex), _playerContext.ToData());
		}
	}

	private async UniTask SavePlayerAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!_isQuitting && _playerContext != null)
		{
			CommitPlayTime();
			_playerContext.ClearDirty();
			_playerContext.SaveTimeTicks = DateTime.UtcNow.Ticks;
			await _saveStorage.SaveAsync(MakePlayerFileName(_playerContext.SlotIndex), _playerContext.ToData(), cancellationToken);
		}
	}

	public void BackupPlayer(long ticks)
	{
		if (_playerContext != null && _playerContext.HasDirty)
		{
			_saveStorage.Backup("PlayerContext", _playerContext.ToData(), ticks);
		}
	}

	private void OnUpdateAcquisition(Goods goods)
	{
		if (goods == null)
		{
			return;
		}
		EGoodsCategory category = goods.Category;
		if (category == EGoodsCategory.Item)
		{
			int id = goods.Id;
			TItem tItem = TItem.Table[id];
			if (tItem.BitFlagIndex >= 0 && !_playerContext.ItemCollectionFlags.IsSet(tItem.BitFlagIndex))
			{
				_playerContext.ItemCollectionFlags.Set(tItem.BitFlagIndex);
				OnItemCollected.OnNext(id);
				_eventSystem.Trigger(new ItemCollectedEvent(goods.Id));
			}
		}
		else if (category == EGoodsCategory.Equipment)
		{
			int id2 = goods.Id;
			TEquipment tEquipment = TEquipment.Table[id2];
			if (tEquipment.BitFlagIndex >= 0 && !_playerContext.EquipmentCollectionFlags.IsSet(tEquipment.BitFlagIndex))
			{
				_playerContext.EquipmentCollectionFlags.Set(tEquipment.BitFlagIndex);
				OnEquipmentCollected.OnNext(id2);
			}
		}
	}

	private PlayerContext CreateNewPlayerContext(int playerSlotIndex)
	{
		PlayerContextData playerContextData = CreatePlayerContextData(playerSlotIndex);
		playerContextData.Version = _appManager.Version;
		return new PlayerContext(playerContextData);
	}

	public GamePlayerData CreateRoomPlayerData(bool isHostPlayer)
	{
		EPlatform platform = _settings.Platform;
		Character character = _playerContext.RxCharacters[SelectedCharacterId];
		GoodsSlot[] inventorySlots = _playerContext.RxInventorySlots.Values.Select((GoodsSlot s) => s.Clone()).ToArray();
		GoodsSlot[] safeInventorySlots = _playerContext.RxSafeInventorySlots.Values.Select((GoodsSlot s) => s.Clone()).ToArray();
		return new GamePlayerData
		{
			Name = _playerContext.RxProfile.Value.Name,
			IsHostPlayer = isHostPlayer,
			Character = character.Clone(),
			InventorySlots = inventorySlots,
			SafeInventorySlots = safeInventorySlots,
			Talent = Talent.Clone(),
			Profile = _playerContext.RxProfile.Value.Clone(),
			SteamId = ((platform == EPlatform.Steam && SteamManager.Initialized) ? Steamworks.SteamUser.GetSteamID().m_SteamID : 0)
		};
	}

	private PlayerContextData CreatePlayerContextData(int playerSlotIndex)
	{
		List<GoodsSlot> list = new List<GoodsSlot>();
		int slotIndex = 0;
		list.AddRange(TItem.Table.Values.Where((TItem i) => 0 < i.DefaultCount).Select((TItem i) => GoodsCreator.MakeItemGoods(i.Id, i.DefaultCount)).ToList()
			.Select((Goods goods) => new GoodsSlot
			{
				SlotIndex = slotIndex++,
				Goods = goods
			}));
		list.AddRange(TEquipment.Table.Values.Where((TEquipment e) => e.IsDefault).Select((TEquipment e) => GoodsCreator.MakeEquipmentGoods(e.Id, 1L, 0L, e.PickPrefixes())).ToList()
			.Select((Goods goods) => new GoodsSlot
			{
				SlotIndex = slotIndex++,
				Goods = goods
			}));
		return new PlayerContextData
		{
			AccountId = _appManager.AccountId,
			SlotIndex = playerSlotIndex,
			Version = new AppVersion(),
			CreateTimeTicks = DateTime.UtcNow.Ticks,
			Profile = new Profile
			{
				Name = string.Empty,
				SelectedCharacterId = TCharacter.Table.Values.First().Id,
				SelectedPetId = -1,
				Gold = TConst.DefaultGold.IntValue,
				Level = 1,
				Exp = 0L,
				BgmId = TLoungeBgm.Table.Values.Min((TLoungeBgm b) => b.Id)
			},
			Talent = new Talent
			{
				BitFlags = new BitFlags(TTalent.Table.Values.Max((TTalent t) => t.BitFlagIndex))
			},
			Characters = TCharacter.Table.Values.Where((TCharacter c) => c.IsUsable).Select(delegate(TCharacter c)
			{
				return new Character
				{
					Id = c.Id,
					SelectedActiveSkillIds = c.ActiveSkillIds.Take(2).ToArray(),
					AttackSkillId = c.AttackSkillId,
					DashSkillId = c.DashSkillId,
					WeaponSlots = new GoodsSlot[2]
					{
						new GoodsSlot
						{
							SlotIndex = 0,
							Goods = GoodsCreator.MakeEquipmentGoods(c.DefaultWeaponId, 1L, 0L, null, null, null, 1)
						},
						new GoodsSlot
						{
							SlotIndex = 1,
							Goods = null
						}
					},
					ActiveWeaponIndex = 0,
					AccessorySlots = Enumerable.Range(0, TConst.CharacterAccessorySlotCount.IntValue).Select((int i) => new GoodsSlot
					{
						SlotIndex = i,
						Goods = null
					}).ToArray(),
					ArtifactSlots = Enumerable.Range(0, TConst.CharacterArtifactSlotCount.IntValue).Select((int i) => new GoodsSlot
					{
						SlotIndex = i,
						Goods = null
					}).ToArray(),
					SkinPresetId = TCharacterSkinPreset.GetDefaultSkinPresetId(c.Id)
				};
			}).ToArray(),
			StashSlots = list.ToArray(),
			InventorySlots = Array.Empty<GoodsSlot>(),
			ItemCollectionFlags = new BitFlags(TItem.Table.Values.Max((TItem i) => i.BitFlagIndex)),
			EquipmentCollectionFlags = new BitFlags(TEquipment.Table.Values.Max((TEquipment e) => e.BitFlagIndex)),
			ItemMarkFlags = new BitFlags(TItem.Table.Values.Max((TItem i) => i.BitFlagIndex)),
			EquipmentMarkFlags = new BitFlags(TEquipment.Table.Values.Max((TEquipment e) => e.BitFlagIndex)),
			BagMarkFlags = new BitFlags(TBag.Table.Values.Max((TBag b) => b.BitFlagIndex)),
			ClearedCampaignFlags = new BitFlags(TCampaign.Table.Values.Max((TCampaign c) => c.BitFlagIndex)),
			ClearedMissionFlags = new BitFlags(TMission.Table.Values.Max((TMission m) => m.BitFlagIndex)),
			ItemUsedFlags = new BitFlags(TItem.Table.Values.Max((TItem i) => i.BitFlagIndex)),
			CompletedLoungeQuestFlags = new BitFlags(TLoungeQuest.Table.Values.Max((TLoungeQuest q) => q.BitFlagIndex)),
			NpcUnlockedFlags = new BitFlags(TNpc.Table.Values.Max((TNpc n) => n.BitFlagIndex)),
			NpcFunctionUnlockedFlags = new BitFlags(TNpcFunction.Table.Values.Max((TNpcFunction f) => f.BitFlagIndex)),
			FeatureUnlockedFlags = new BitFlags(TFeatureUnlock.Table.Values.Max((TFeatureUnlock f) => (int)f.Feature)),
			NpcLevels = null,
			MonsterRecords = Array.Empty<MonsterRecord>(),
			DialogReplayRecords = Array.Empty<DialogReplayRecord>(),
			UnlockedSkinFlags = new BitFlags(TCharacterSkin.Table.Values.Max((TCharacterSkin s) => s.BitFlagIndex)),
			AreaFlags = CreateDefaultAreaFlags(),
			AreaDecorationUnlockedFlags = CreateDefaultAreaDecorationFlags(),
			ActiveAreaDecorations = TAreaDecoration.Table.Values.Where((TAreaDecoration d) => d.Index == 0).Select((TAreaDecoration d) => new ActiveAreaDecoration
			{
				GroupId = d.GroupId,
				AreaDecorationSettingId = d.Id
			}).ToArray(),
			TycoonUnlockFlags = new BitFlags(TTycoonSalesSlot.Table.Values.Max((TTycoonSalesSlot s) => s.BitFlagIndex)),
			ViewedNpcTalkFlags = new BitFlags(TNpcTalk.Table.Values.Max((TNpcTalk t) => t.BitFlagIndex)),
			LoungeBgmUnlockedFlags = new BitFlags(TLoungeBgm.Table.Values.Max((TLoungeBgm b) => b.BitFlagIndex)),
			QuickSlotItemIds = Enumerable.Repeat(0, 5).ToArray(),
			PlayerStores = null,
			Difficulty = ERoomDifficulty.Junior,
			SalesSlots = Array.Empty<SalesSlot>(),
			Employees = new Employee[1] { Employee.Create(1000001, 1, false) }
		};
	}

	private static BitFlags CreateDefaultAreaFlags()
	{
		BitFlags bitFlags = new BitFlags(TArea.Table.Values.Max((TArea a) => a.BitFlagIndex));
		foreach (TArea item in TArea.Table.Values.Where((TArea a) => a.DefaultUnlocked))
		{
			bitFlags.Set(item.BitFlagIndex);
		}
		return bitFlags;
	}

	private static BitFlags CreateDefaultAreaDecorationFlags()
	{
		BitFlags bitFlags = new BitFlags(TAreaDecoration.Table.Values.Max((TAreaDecoration d) => d.BitFlagIndex));
		foreach (TAreaDecoration item in TAreaDecoration.Table.Values.Where((TAreaDecoration d) => d.Index == 0))
		{
			bitFlags.Set(item.BitFlagIndex);
		}
		return bitFlags;
	}

	private void Patch(AppVersion dataVersion, PlayerContextData data)
	{
		if (dataVersion < new AppVersion(0, 0, 0, 120))
		{
			Patch_0_0_0_120(data);
		}
	}

	private void Patch_0_0_0_120(PlayerContextData data)
	{
		(int, int)[] indexMap = TEquipment.Table.Values.Select((TEquipment e) => (e.Id, e.BitFlagIndex)).ToArray();
		int newSize = TEquipment.Table.Values.Max((TEquipment e) => e.BitFlagIndex);
		data.EquipmentCollectionFlags = RemapBitFlags(data.EquipmentCollectionFlags, indexMap, newSize);
		data.EquipmentMarkFlags = RemapBitFlags(data.EquipmentMarkFlags, indexMap, newSize);
		(int, int)[] indexMap2 = TBag.Table.Values.Select((TBag b) => (b.Id, b.BitFlagIndex)).ToArray();
		int newSize2 = TBag.Table.Values.Max((TBag b) => b.BitFlagIndex);
		data.BagMarkFlags = RemapBitFlags(data.BagMarkFlags, indexMap2, newSize2);
	}

	private static BitFlags RemapBitFlags(BitFlags oldFlags, IEnumerable<(int oldIndex, int newIndex)> indexMap, int newSize)
	{
		BitFlags bitFlags = new BitFlags(newSize);
		if (oldFlags == null)
		{
			return bitFlags;
		}
		foreach (var item in indexMap)
		{
			if (item.oldIndex >= 0 && item.newIndex >= 0 && oldFlags.IsSet(item.oldIndex))
			{
				bitFlags.Set(item.newIndex);
			}
		}
		return bitFlags;
	}

	private bool RepairPlayerData(PlayerContextData data)
	{
		if (data == null)
		{
			return false;
		}
		return RepairGoods(data);
	}

	private bool RepairGoods(PlayerContextData data)
	{
		bool flag = false;
		bool flag2 = false;
		if (data.StashSlots != null)
		{
			GoodsSlot[] stashSlots = data.StashSlots;
			foreach (GoodsSlot goodsSlot in stashSlots)
			{
				if (goodsSlot != null && RepairGoodsItem(goodsSlot.Goods))
				{
					flag = true;
				}
			}
		}
		bool flag3 = false;
		if (data.InventorySlots != null)
		{
			GoodsSlot[] inventorySlots = data.InventorySlots;
			foreach (GoodsSlot goodsSlot2 in inventorySlots)
			{
				if (goodsSlot2 != null && RepairGoodsItem(goodsSlot2.Goods))
				{
					flag3 = true;
				}
			}
		}
		bool flag4 = false;
		if (data.StorageSlots != null)
		{
			GoodsSlot[] storageSlots = data.StorageSlots;
			foreach (GoodsSlot goodsSlot3 in storageSlots)
			{
				if (goodsSlot3 != null && RepairGoodsItem(goodsSlot3.Goods))
				{
					flag4 = true;
				}
			}
		}
		if (data.SafeInventorySlots != null)
		{
			GoodsSlot[] safeInventorySlots = data.SafeInventorySlots;
			foreach (GoodsSlot goodsSlot4 in safeInventorySlots)
			{
				if (goodsSlot4 != null && RepairGoodsItem(goodsSlot4.Goods))
				{
					flag2 = true;
				}
			}
		}
		bool flag5 = flag2 || flag4 || flag3 || flag;
		if (data.Characters != null)
		{
			Character[] characters = data.Characters;
			foreach (Character character in characters)
			{
				if (character != null)
				{
					bool flag6 = RepairGoodsSlots(character.WeaponSlots);
					bool flag7 = RepairGoodsSlots(character.AccessorySlots);
					bool flag8 = RepairGoodsSlots(character.ArtifactSlots);
					flag5 = flag8 || flag7 || flag6 || flag5;
				}
			}
		}
		if (data.SalesSlots != null)
		{
			SalesSlot[] salesSlots = data.SalesSlots;
			foreach (SalesSlot salesSlot in salesSlots)
			{
				if (salesSlot != null && RepairGoodsItem(salesSlot.Goods))
				{
					flag5 = true;
				}
			}
		}
		if (data.LostGoods != null)
		{
			LostGoods[] lostGoods = data.LostGoods;
			foreach (LostGoods lostGoods2 in lostGoods)
			{
				if (lostGoods2 != null && RepairGoodsItem(lostGoods2.Goods))
				{
					flag5 = true;
				}
			}
		}
		return flag5;
	}

	private bool RepairGoodsSlots(GoodsSlot[] slots)
	{
		if (slots == null)
		{
			return false;
		}
		bool result = false;
		foreach (GoodsSlot goodsSlot in slots)
		{
			if (goodsSlot != null && RepairGoodsItem(goodsSlot.Goods))
			{
				result = true;
			}
		}
		return result;
	}

	private bool RepairGoodsItem(Goods goods)
	{
		if (goods != null)
		{
			BagGoods bagGoods = goods as BagGoods;
			if (bagGoods != null)
			{
				return RepairGoodsSlots(bagGoods.GoodsSlots);
			}
			if (IsPrefixOverflow(goods))
			{
				(goods as ArtifactGoods)?.NormalizeHackPrefixes();
				return true;
			}
		}
		return false;
	}

	private static bool IsPrefixOverflow(Goods goods)
	{
		if (goods.Category != EGoodsCategory.Equipment)
		{
			return false;
		}
		if (!TEquipment.Table.ContainsKey(goods.Id))
		{
			return false;
		}
		int num = Math.Max(TEquipment.Table[goods.Id].GetMaxLegitPrefixCount(), 1);
		int num2 = 0;
		ArtifactGoods artifactGoods = goods as ArtifactGoods;
		if (artifactGoods != null && artifactGoods.Prefixes != null)
		{
			num2 = artifactGoods.Prefixes.Length;
		}
		return num < num2;
	}

	private static void NormalizeHackPrefixes(Goods goods)
	{
		(goods as ArtifactGoods)?.NormalizeHackPrefixes();
	}

	private static int GetEquipmentPrefixesCount(Goods goods)
	{
		ArtifactGoods artifactGoods = goods as ArtifactGoods;
		if (artifactGoods != null && artifactGoods.Prefixes != null)
		{
			return artifactGoods.Prefixes.Length;
		}
		return 0;
	}

	public string GetPlayerStoreString(int id, string key)
	{
		PlayerStore value;
		if (!_playerContext.PlayerStores.TryGetValue(id, out value))
		{
			return null;
		}
		string value2 = value.GetValue(key);
		if (string.IsNullOrEmpty(value2))
		{
			return null;
		}
		return value2;
	}

	public void SetPlayerStoreString(int id, string key, string value)
	{
		PlayerStore value2;
		if (!_playerContext.PlayerStores.TryGetValue(id, out value2))
		{
			value2 = new PlayerStore
			{
				Id = id
			};
			_playerContext.PlayerStores.Add(id, value2);
		}
		value2.SetValue(key, value);
	}

	public void SetPlayerStoreTempString(int id, string key, string value)
	{
		PlayerStore value2;
		if (!_playerContext.PlayerStores.TryGetValue(id, out value2))
		{
			value2 = new PlayerStore
			{
				Id = id
			};
			_playerContext.PlayerStores.Add(id, value2);
		}
		value2.SetTempValue(key, value);
	}

	public int GetPlayerStoreInt(int id, string key)
	{
		string playerStoreString = GetPlayerStoreString(id, key);
		int result;
		if (!string.IsNullOrEmpty(playerStoreString) && int.TryParse(playerStoreString, out result))
		{
			return result;
		}
		return -1;
	}

	public void SetPlayerStoreInt(int id, string key, int value)
	{
		SetPlayerStoreString(id, key, value.ToString());
	}

	public void SetPlayerStoreTempInt(int id, string key, int value)
	{
		SetPlayerStoreTempString(id, key, value.ToString());
	}

	public float GetPlayerStoreFloat(int id, string key)
	{
		string playerStoreString = GetPlayerStoreString(id, key);
		float result;
		if (!string.IsNullOrEmpty(playerStoreString) && float.TryParse(playerStoreString, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out result))
		{
			return result;
		}
		return -1f;
	}

	public void SetPlayerStoreFloat(int id, string key, float value)
	{
		SetPlayerStoreString(id, key, value.ToString());
	}

	public void SetPlayerStoreTempFloat(int id, string key, float value)
	{
		SetPlayerStoreTempString(id, key, value.ToString());
	}

	public bool GetPlayerStoreBool(int id, string key)
	{
		string playerStoreString = GetPlayerStoreString(id, key);
		if (string.IsNullOrEmpty(playerStoreString))
		{
			return false;
		}
		bool result;
		return bool.TryParse(playerStoreString, out result) && result;
	}

	public void SetPlayerStoreBool(int id, string key, bool value)
	{
		SetPlayerStoreString(id, key, value.ToString());
	}

	public void SetPlayerStoreTempBool(int id, string key, bool value)
	{
		SetPlayerStoreTempString(id, key, value.ToString());
	}

	public void RemovePlayerStore(int id)
	{
		_playerContext.PlayerStores.Remove(id);
	}

	public void RemovePlayerStoreValue(int id, string key)
	{
		PlayerStore value;
		if (_playerContext.PlayerStores.TryGetValue(id, out value))
		{
			value.Remove(key);
		}
	}

	public bool TryGetQuickSlotItemId(int slotIndex, out int itemId)
	{
		itemId = _playerContext.RxQuickSlotItemIds[slotIndex];
		return 0 < itemId;
	}

	public void SetQuickSlot(int settingIndex, int settingItemId)
	{
		_playerContext.RxQuickSlotItemIds[settingIndex] = settingItemId;
		if (settingItemId >= 1)
		{
			(from pair in _playerContext.RxQuickSlotItemIds.Select((int id, int index) => (id, index))
				where pair.Item1 == settingItemId && pair.Item2 != settingIndex
				select pair.Item2).ToList().ForEach(delegate(int i)
			{
				_playerContext.RxQuickSlotItemIds[i] = 0;
			});
		}
	}

	public void SwapQuickSlot(int sourceSlotIndex, int targetSlotIndex)
	{
		int value = _playerContext.RxQuickSlotItemIds[sourceSlotIndex];
		int value2 = _playerContext.RxQuickSlotItemIds[targetSlotIndex];
		_playerContext.RxQuickSlotItemIds[sourceSlotIndex] = value2;
		_playerContext.RxQuickSlotItemIds[targetSlotIndex] = value;
	}

	public void UnlockPlayerCharacterSkin(int characterSkinId)
	{
		_playerContext.UnlockedSkinFlags.Set(TCharacterSkin.Table[characterSkinId].BitFlagIndex);
	}

	public bool IsCharacterSkinUnlocked(int characterSkinId)
	{
		TCharacterSkin tCharacterSkin = TCharacterSkin.Table[characterSkinId];
		if (tCharacterSkin.BitFlagIndex < 0)
		{
			return true;
		}
		return _playerContext.UnlockedSkinFlags.IsSet(tCharacterSkin.BitFlagIndex);
	}

	public int[] GetAvailableSkinIds(int characterId)
	{
		return TCharacterSkin.GroupTable[characterId].Where((TCharacterSkin s) => IsCharacterSkinUnlocked(s.Id)).Select((TCharacterSkin s) => s.Id).ToArray();
	}

	public bool HasTalent(int talentId)
	{
		return Talent.HasTalent(talentId);
	}

	public EUnlockState GetTalentState(int talentId)
	{
		if (Talent.HasTalent(talentId))
		{
			return EUnlockState.Unlocked;
		}
		TTalent tTalent = TTalent.Table[talentId];
		if (!CheckStateConditions(tTalent.GetUnlockConditions()))
		{
			return EUnlockState.Locked;
		}
		if (tTalent.RootTalentIds != null && tTalent.RootTalentIds.Any((int rootId) => !Talent.HasTalent(rootId)))
		{
			return EUnlockState.Locked;
		}
		return EUnlockState.Unlockable;
	}

	public bool IsSalesSlotUnlocked(int tycoonUnlockId)
	{
		BitFlags tycoonUnlockFlags = _playerContext.TycoonUnlockFlags;
		if (tycoonUnlockFlags == null)
		{
			return false;
		}
		TTycoonSalesSlot value;
		if (!TTycoonSalesSlot.Table.TryGetValue(tycoonUnlockId, out value))
		{
			return false;
		}
		return tycoonUnlockFlags.IsSet(value.BitFlagIndex);
	}

	public List<int> GetUnlockedSalesSlotIndices()
	{
		return TTycoonSalesSlot.Table.Values.Where((TTycoonSalesSlot u) => IsSalesSlotUnlocked(u.Id)).OrderBy((TTycoonSalesSlot u) => u.Id).Select((TTycoonSalesSlot u) => u.Id)
			.ToList();
	}

	public void SetPreventRepairMaxDurabilityLoss(bool value)
	{
		if (_playerContext != null)
		{
			_playerContext.PreventRepairMaxDurabilityLoss = value;
		}
	}

	public void ChangeName(string newName)
	{
		Profile profile = _playerContext.RxProfile.Value.Clone();
		profile.Name = newName;
		_playerContext.RxProfile.Value = profile;
		_playerContext.ClearDirty();
	}

	public bool HasEnoughGold(long amount)
	{
		return amount <= _playerContext.RxProfile.Value.Gold;
	}

	public bool HasEnoughCoin(long amount)
	{
		return amount <= _playerContext.RxProfile.Value.Coin;
	}

	private void UnlockNpcIfConditionsMet(IBaseEvent baseEvent)
	{
		if (_lockedNpcIds == null)
		{
			_lockedNpcIds = TNpc.Table.Values.Where((TNpc n) => n.UnlockConditions != null && !IsNpcUnlocked(n.Id)).Select((TNpc n) => n.Id).ToHashSet();
		}
		if (!_lockedNpcIds.Any())
		{
			return;
		}
		int[] array = _lockedNpcIds.Where((int id) => TNpc.Table[id].UnlockConditions.All((EventCondition cond) => baseEvent.CalculateProgress(cond, 0L, 1L) > 0)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(int id)
			{
				SetNpcUnlocked(id);
				_lockedNpcIds.Remove(id);
			});
			_eventSystem.Trigger(new NpcUnlockEvent(array));
		}
	}

	private void UnlockNpcFunctionIfConditionsMet(IBaseEvent baseEvent)
	{
		if (_lockedNpcFunctionIds == null)
		{
			_lockedNpcFunctionIds = TNpcFunction.Table.Values.Where((TNpcFunction f) => f.UnlockConditions != null && !IsNpcFunctionUnlocked(f.Id)).Select((TNpcFunction f) => f.Id).ToHashSet();
		}
		if (!_lockedNpcFunctionIds.Any())
		{
			return;
		}
		int[] array = _lockedNpcFunctionIds.Where((int id) => TNpcFunction.Table[id].UnlockConditions.All((EventCondition cond) => baseEvent.CalculateProgress(cond, 0L, 1L) > 0)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(int id)
			{
				SetNpcFunctionUnlocked(id);
				_lockedNpcFunctionIds.Remove(id);
			});
			_eventSystem.Trigger(new NpcFunctionUnlockEvent(array));
		}
	}

	private void UnlockFeatureIfConditionsMet(IBaseEvent baseEvent)
	{
		if (_lockedFeatures == null)
		{
			_lockedFeatures = TFeatureUnlock.Table.Values.Where((TFeatureUnlock f) => f.UnlockConditions != null && !IsFeatureUnlocked(f.Feature)).Select((TFeatureUnlock f) => f.Feature).ToHashSet();
		}
		if (!_lockedFeatures.Any())
		{
			return;
		}
		EFeatureUnlock[] array = _lockedFeatures.Where((EFeatureUnlock feature) => TFeatureUnlock.Table[feature].UnlockConditions.All((EventCondition cond) => baseEvent.CalculateProgress(cond, 0L, 1L) > 0)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(EFeatureUnlock feature)
			{
				SetFeatureUnlocked(feature);
				_lockedFeatures.Remove(feature);
				if (feature == EFeatureUnlock.SafeInventory)
				{
					_gameChannel.SendToHost(new ReqSetPet
					{
						PetId = TPet.Table.Keys.Min()
					});
				}
			});
			_eventSystem.Trigger(new FeatureUnlockEvent(array));
		}
	}

	private void UnlockNpcsFromCurrentState()
	{
		if (_lockedNpcIds == null)
		{
			_lockedNpcIds = TNpc.Table.Values.Where((TNpc n) => n.UnlockConditions != null && !IsNpcUnlocked(n.Id)).Select((TNpc n) => n.Id).ToHashSet();
		}
		if (!_lockedNpcIds.Any())
		{
			return;
		}
		int[] array = _lockedNpcIds.Where((int id) => CheckStateConditions(TNpc.Table[id].UnlockConditions)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(int id)
			{
				SetNpcUnlocked(id);
				_lockedNpcIds.Remove(id);
			});
			_eventSystem.Trigger(new NpcUnlockEvent(array));
		}
	}

	private void UnlockNpcFunctionsFromCurrentState()
	{
		if (_lockedNpcFunctionIds == null)
		{
			_lockedNpcFunctionIds = TNpcFunction.Table.Values.Where((TNpcFunction f) => f.UnlockConditions != null && !IsNpcFunctionUnlocked(f.Id)).Select((TNpcFunction f) => f.Id).ToHashSet();
		}
		if (!_lockedNpcFunctionIds.Any())
		{
			return;
		}
		int[] array = _lockedNpcFunctionIds.Where((int id) => CheckStateConditions(TNpcFunction.Table[id].UnlockConditions)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(int id)
			{
				SetNpcFunctionUnlocked(id);
				_lockedNpcFunctionIds.Remove(id);
			});
			_eventSystem.Trigger(new NpcFunctionUnlockEvent(array));
		}
	}

	private void UnlockLoungeBgmsFromCurrentState()
	{
		if (_lockedBgmIds == null)
		{
			_lockedBgmIds = TLoungeBgm.Table.Values.Where((TLoungeBgm b) => b.UnlockConditions != null && !IsLoungeBgmUnlocked(b.Id)).Select((TLoungeBgm b) => b.Id).ToHashSet();
		}
		if (_lockedBgmIds.Any())
		{
			int[] array = _lockedBgmIds.Where((int id) => CheckStateConditions(TLoungeBgm.Table[id].UnlockConditions)).ToArray();
			if (array.Any())
			{
				array.ForEach(delegate(int id)
				{
					SetLoungeBgmUnlocked(id);
					_lockedBgmIds.Remove(id);
				});
			}
		}
	}

	private void UnlockFeaturesFromCurrentState()
	{
		if (_lockedFeatures == null)
		{
			_lockedFeatures = TFeatureUnlock.Table.Values.Where((TFeatureUnlock f) => f.UnlockConditions != null && !IsFeatureUnlocked(f.Feature)).Select((TFeatureUnlock f) => f.Feature).ToHashSet();
		}
		if (!_lockedFeatures.Any())
		{
			return;
		}
		EFeatureUnlock[] array = _lockedFeatures.Where((EFeatureUnlock feature) => CheckStateConditions(TFeatureUnlock.Table[feature].UnlockConditions)).ToArray();
		if (array.Any())
		{
			array.ForEach(delegate(EFeatureUnlock feature)
			{
				SetFeatureUnlocked(feature);
				_lockedFeatures.Remove(feature);
			});
			_eventSystem.Trigger(new FeatureUnlockEvent(array));
		}
	}

	private void UnlockLoungeBgmIfConditionsMet(IBaseEvent baseEvent)
	{
		if (_lockedBgmIds == null)
		{
			_lockedBgmIds = TLoungeBgm.Table.Values.Where((TLoungeBgm b) => b.UnlockConditions != null && !IsLoungeBgmUnlocked(b.Id)).Select((TLoungeBgm b) => b.Id).ToHashSet();
		}
		if (_lockedBgmIds.Any())
		{
			int[] array = _lockedBgmIds.Where((int id) => TLoungeBgm.Table[id].UnlockConditions.All((EventCondition cond) => baseEvent.CalculateProgress(cond, 0L, 1L) > 0)).ToArray();
			if (array.Any())
			{
				array.ForEach(delegate(int id)
				{
					SetLoungeBgmUnlocked(id);
					_lockedBgmIds.Remove(id);
				});
			}
		}
	}

	public bool HasEnoughGoods(Goods goods)
	{
		return GetTotalGoodsCount(goods) >= goods.Count;
	}

	public bool HasEnoughGoods(Goods[] goodsArray)
	{
		if (goodsArray != null && goodsArray.Any())
		{
			return goodsArray.All(HasEnoughGoods);
		}
		return true;
	}

	public bool HasEnoughGoods(GoodsData data)
	{
		Goods goods = data.ToGoodsAsBase();
		return GetTotalGoodsCount(goods) >= goods.Count;
	}

	public bool HasEnoughGoods(GoodsData[] dataArray)
	{
		if (dataArray != null && dataArray.Any())
		{
			return dataArray.All(HasEnoughGoods);
		}
		return true;
	}

	public long GetInventoryGoodsCount(Goods goods)
	{
		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			return _playerContext.RxInventorySlots.GetItemCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetItemCount(goods.Id);
		case EGoodsCategory.Equipment:
			return _playerContext.RxInventorySlots.GetEquipmentCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetEquipmentCount(goods.Id);
		case EGoodsCategory.Bag:
			return _playerContext.RxInventorySlots.GetBagCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetBagCount(goods.Id);
		default:
			return 0L;
		}
	}

	public long GetStashGoodsCount(Goods goods)
	{
		switch (goods.Category)
		{
		case EGoodsCategory.Item:
			return _playerContext.RxStashSlots.GetItemCount(goods.Id);
		case EGoodsCategory.Equipment:
			return _playerContext.RxStashSlots.GetEquipmentCount(goods.Id);
		case EGoodsCategory.Bag:
			return _playerContext.RxStashSlots.GetBagCount(goods.Id);
		default:
			return 0L;
		}
	}

	public long GetTotalGoodsCount(GoodsData data, bool includeEquippedGoods = false)
	{
		return GetTotalGoodsCount(data.ToGoodsAsBase(), includeEquippedGoods);
	}

	public long GetTotalGoodsCount(Goods goods, bool includeEquippedGoods = false)
	{
		switch (goods.Type)
		{
		case EGoodsType.Gold:
			return Gold;
		case EGoodsType.Coin:
			return Coin;
		case EGoodsType.Exp:
			return Exp;
		case EGoodsType.Consumable:
		case EGoodsType.Valuable:
		case EGoodsType.Misc:
		case EGoodsType.Note:
		case EGoodsType.Blueprint:
		case EGoodsType.MusicDisc:
		{
			long num = _playerContext.RxInventorySlots.GetItemCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetItemCount(goods.Id);
			if (_gameManager.IsInLounge)
			{
				num += _playerContext.RxStashSlots.GetItemCount(goods.Id);
			}
			return num;
		}
		case EGoodsType.Weapon:
		case EGoodsType.Accessory:
		case EGoodsType.Artifact:
		{
			long num2 = _playerContext.RxInventorySlots.GetEquipmentCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetEquipmentCount(goods.Id);
			if (_gameManager.IsInLounge)
			{
				num2 += _playerContext.RxStashSlots.GetEquipmentCount(goods.Id);
			}
			if (!includeEquippedGoods)
			{
				return num2;
			}
			if (goods.Type == EGoodsType.Weapon)
			{
				num2 += _playerContext.RxCharacters.Values.SelectMany((Character c) => c.WeaponSlots).Count((GoodsSlot slot) => slot.Goods != null && slot.Goods.Id == goods.Id);
			}
			if (goods.Type == EGoodsType.Accessory)
			{
				num2 += _playerContext.RxCharacters.Values.SelectMany((Character c) => c.AccessorySlots).Count((GoodsSlot slot) => slot.Goods != null && slot.Goods.Id == goods.Id);
			}
			if (goods.Type == EGoodsType.Artifact)
			{
				num2 += _playerContext.RxCharacters.Values.SelectMany((Character c) => c.ArtifactSlots).Count((GoodsSlot slot) => slot.Goods != null && slot.Goods.Id == goods.Id);
			}
			return num2;
		}
		case EGoodsType.Bag:
		{
			long num3 = _playerContext.RxInventorySlots.GetBagCount(goods.Id) + _playerContext.RxSafeInventorySlots.GetBagCount(goods.Id);
			if (!_gameManager.IsInLounge)
			{
				return num3;
			}
			return num3 + _playerContext.RxStashSlots.GetBagCount(goods.Id);
		}
		default:
			return 0L;
		}
	}

	public long CalculateEnoughCraftingCount(Goods[] materialItems)
	{
		if (materialItems == null || materialItems.Length == 0)
		{
			return 0L;
		}
		long num = long.MaxValue;
		foreach (Goods goods in materialItems)
		{
			long totalGoodsCount = GetTotalGoodsCount(goods);
			if (totalGoodsCount < goods.Count)
			{
				return 0L;
			}
			num = Math.Min(num, totalGoodsCount / goods.Count);
		}
		return num;
	}
}

