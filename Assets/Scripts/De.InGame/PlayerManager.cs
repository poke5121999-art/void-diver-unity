using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class PlayerManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CDeletePlayer_003Ed__50 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public int slotIndex;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CLoadAccountAsync_003Ed__46 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public CancellationToken cancellationToken;

		private string _003CaccountId_003E5__2;

		private UniTask<StorageLoadResult<AccountContextData>>.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CLoadPlayerAsync_003Ed__207 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<ELoadResult> _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public CancellationToken cancellationToken;

		private BlockScope _003CblockScope_003E5__2;

		private UniTask<StorageLoadResult<PlayerContextData>>.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRestoreAsync_003Ed__52 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public long saveTimeTicks;

		public int targetSlotIndex;

		private PlayerContextData _003Cdata_003E5__2;

		private UniTask<PlayerContextData>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSaveAccountAsync_003Ed__48 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSaveAsync_003Ed__33 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CSavePlayerAsync_003Ed__211 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public PlayerManager _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

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

	private static readonly TimeSpan _saveInterval;

	private DateTime _lastSaveTime;

	private bool _isQuitting;

	private bool _isSaving;

	private bool _saveRequested;

	private CancellationTokenSource _saveCts;

	private PlayerContext _playerContext;

	public const int MaxSlotCount = 6;

	private AccountContext _accountContext;

	public ReactiveProperty<int> RxPlayerSlotIndex;

	public readonly Subject<int> OnPlayerSlotUpdated;

	private CompositeDisposable _playerDisposable;

	private DateTime? _sessionStart;

	private HashSet<int> _lockedNpcIds;

	private HashSet<int> _lockedBgmIds;

	private HashSet<int> _lockedNpcFunctionIds;

	private HashSet<EFeatureUnlock> _lockedFeatures;

	private GamePlayer Player => null;

	public bool IsPlayerLoaded => false;

	private ulong MyClientId => 0uL;

	private CharacterModel MyCharacterModel => null;

	public int PlayerSlotIndex => 0;

	public PlayerSlot[] PlayerSlots => null;

	public PlayerSlot SelectedPlayerSlot => null;

	public IReadOnlyReactiveDictionary<int, Character> RxCharacters => null;

	public IDictionary<int, Character> Characters => null;

	public Character SelectedCharacter => null;

	public IReadOnlyDictionary<long, DialogReplayRecord> DialogReplayRecords => null;

	public Subject<(EGoodsType goodsType, int id, bool isMarked)> OnGoodsMarkChanged { get; }

	public Subject<int> OnEquipmentCollected { get; }

	public Subject<int> OnItemCollected { get; }

	public Subject<int> OnItemFirstUsed { get; }

	public int BestStage => 0;

	public double AccountPowerScore => 0.0;

	public BitFlags AreaFlags => null;

	public BitFlags AreaDecorationUnlockedFlags => null;

	public IDictionary<int, int> ActiveAreaDecorations => null;

	public IDictionary<int, int> NpcLevels => null;

	public IDictionary<int, GoodsSlot> InventorySlots => null;

	public IDictionary<int, GoodsSlot> SafeInventorySlots => null;

	public IDictionary<int, GoodsSlot> StashSlots => null;

	public IDictionary<int, GoodsSlot> StorageSlots => null;

	public ISubject<Unit> OnInventoryChanged => null;

	public ISubject<Unit> OnStashChanged => null;

	public ISubject<Unit> OnSafeInventoryChanged => null;

	public ISubject<Unit> OnOwnedGoodsChanged => null;

	public ISubject<Unit> OnStorageChanged => null;

	public IReadOnlyReactiveCollection<LostItemSelectInfo> RxSelectLostGoodsInfos => null;

	public IDictionary<int, LostGoods> LostGoods => null;

	public ISubject<Unit> OnLostItemChanged => null;

	public IDictionary<int, LoungeQuest> LoungeQuests => null;

	public Subject<int> OnMonsterRecordUpdated { get; }

	public ERoomDifficulty Difficulty => ERoomDifficulty.Story;

	public ERoomDifficulty LastPlayedRoomDifficulty => ERoomDifficulty.Story;

	public Subject<Unit> OnPlayerInitialized { get; }

	private string PlayerFileName => null;

	private string BackupPlayerFileName => null;

	public IReadOnlyReactiveCollection<int> RxQuickSlotItemIds => null;

	public IList<int> QuickSlotItemIds => null;

	public IReadOnlyReactiveProperty<Talent> RxTalent => null;

	public Talent Talent => null;

	public IDictionary<int, Employee> Employees => null;

	public IDictionary<int, SalesSlot> SalesSlots => null;

	public IDictionary<int, Employee> RecruitableEmployees => null;

	public BitFlags TycoonUnlockFlags => null;

	public IDictionary<int, int> FiredEmployeeCooldowns => null;

	public IReadOnlyReactiveProperty<Profile> RxProfile => null;

	public Profile Profile => null;

	public string Name => null;

	public long Gold => 0L;

	public long Exp => 0L;

	public long Coin => 0L;

	public int Level => 0;

	public int SelectedCharacterId => 0;

	public int SelectedPetId => 0;

	public long CreateTimeTicks => 0L;

	public AppVersion InstallVersion => null;

	public double PlayTimeSeconds => 0.0;

	public int BgmId => 0;

	public bool PreventRepairMaxDurabilityLoss => false;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnJoinGame(ResJoinGame message)
	{
	}

	private void OnUpdateGamePlayer(NtfUpdateGamePlayer message)
	{
	}

	private void OnStageResultPlayerInfo(NtfStageResultPlayerInfo message)
	{
	}

	private void OnStageStateChanged(EStageState state)
	{
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
	}

	private void FixedUpdate()
	{
	}

	private void AutoSave()
	{
	}

	public void Save(bool force = false)
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__33))]
	public UniTask SaveAsync()
	{
		return default;
	}

	public void OnApplicationQuit()
	{
	}

	private string MakeAccountFileName()
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CLoadAccountAsync_003Ed__46))]
	public UniTask<bool> LoadAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private void SaveAccount()
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAccountAsync_003Ed__48))]
	public UniTask SaveAccountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void SelectPlayerSlot(int slotIndex)
	{
	}

	[AsyncStateMachine(typeof(_003CDeletePlayer_003Ed__50))]
	public UniTask DeletePlayer(int slotIndex)
	{
		return default;
	}

	private void UpdateSlot()
	{
	}

	[AsyncStateMachine(typeof(_003CRestoreAsync_003Ed__52))]
	public UniTask<bool> RestoreAsync(int targetSlotIndex, long saveTimeTicks)
	{
		return default;
	}

	private AccountContext CreateNewAccountContext()
	{
		return null;
	}

	public void SetDialogReplayRecord(string messageKey, int selectedIndex)
	{
	}

	public bool CheckStateConditions(EventCondition[] eventConditions)
	{
		return false;
	}

	public EventCondition GetFailedCondition(EventCondition[] eventConditions)
	{
		return default;
	}

	public bool CheckStateCondition(EventCondition eventCondition)
	{
		return false;
	}

	public bool IsQuestCleared(EQuestType questType, int tableId)
	{
		return false;
	}

	public bool SetClearedCampaign(int campaignId)
	{
		return false;
	}

	public bool IsClearedCampaign(int campaignId)
	{
		return false;
	}

	private static int PopCount(ulong v)
	{
		return 0;
	}

	public bool SetItemCollection(int itemId)
	{
		return false;
	}

	public bool IsCollectedItem(int itemId)
	{
		return false;
	}

	public bool SetEquipmentCollection(int equipmentId)
	{
		return false;
	}

	public bool IsCollectedEquipment(int equipmentId)
	{
		return false;
	}

	public void ToggleMarkGoods(Goods goods)
	{
	}

	public bool IsMarked(Goods goods)
	{
		return false;
	}

	public bool SetItemUsed(int itemId)
	{
		return false;
	}

	public bool IsItemUsed(int itemId)
	{
		return false;
	}

	public void ClearItemUsed(int itemId)
	{
	}

	public bool SetClearedMission(int missionId)
	{
		return false;
	}

	public bool IsClearedMission(int missionId)
	{
		return false;
	}

	public bool SetClearedLoungeQuest(int loungeQuestId)
	{
		return false;
	}

	public bool IsClearedLoungeQuest(int loungeQuestId)
	{
		return false;
	}

	public bool SetNpcUnlocked(int npcId)
	{
		return false;
	}

	public bool IsNpcUnlocked(int npcId)
	{
		return false;
	}

	public bool IsAreaUnlocked(int areaId)
	{
		return false;
	}

	public void SetAreaUnlocked(int areaId)
	{
	}

	public bool IsAreaDecorationUnlocked(int areaDecorationSettingId)
	{
		return false;
	}

	public void SetAreaDecorationUnlocked(int areaDecorationSettingId)
	{
	}

	public void SetActiveAreaDecoration(int groupId, int areaDecorationSettingId)
	{
	}

	public int GetNpcLevel(int npcId)
	{
		return 0;
	}

	public void SetNpcLevel(int npcId, int level)
	{
	}

	public bool SetNpcTalkViewed(int npcTalkId)
	{
		return false;
	}

	public bool IsNpcTalkViewed(int npcTalkId)
	{
		return false;
	}

	public bool IsLoungeBgmUnlocked(int bgmId)
	{
		return false;
	}

	public bool SetLoungeBgmUnlocked(int bgmId)
	{
		return false;
	}

	public bool IsNpcFunctionUnlocked(int npcFunctionId)
	{
		return false;
	}

	public bool SetNpcFunctionUnlocked(int npcFunctionId)
	{
		return false;
	}

	public bool IsFeatureUnlocked(EFeatureUnlock feature)
	{
		return false;
	}

	public bool SetFeatureUnlocked(EFeatureUnlock feature)
	{
		return false;
	}

	public bool IsKnownGoods(Goods goods)
	{
		return false;
	}

	public bool IsUnknownGoods(Goods goods)
	{
		return false;
	}

	public void AddGoodsToStash(Goods goods)
	{
	}

	public bool TryGoodsToStash(Goods goods, int slotIndex = -1)
	{
		return false;
	}

	public int GetStashEmptySlotCount()
	{
		return 0;
	}

	public bool CanAddGoodsToStash(Goods goods)
	{
		return false;
	}

	public int[] GetMovableInventorySlotIndicesForStash()
	{
		return null;
	}

	public long GetTotalBagCount(int bagId)
	{
		return 0L;
	}

	public long GetTotalEmptyBagCount(int bagId)
	{
		return 0L;
	}

	public string GetBagConsumeFailToast(IEnumerable<Goods> consumeGoods)
	{
		return null;
	}

	public void ConsumeStashGoods(Goods consumeGoods)
	{
	}

	public void ConsumeStashGoods(Goods[] consumeGoodsArray)
	{
	}

	public void ConsumeStashSlot(int slotIndex, Goods consumeGoods)
	{
	}

	public void OrganizeStash()
	{
	}

	public void AddGoodsToStorage(Goods[] goods)
	{
	}

	public void AddGoodsToStorage(Goods goods)
	{
	}

	public int GetStorageSlotCount()
	{
		return 0;
	}

	public bool HasStorageSlots()
	{
		return false;
	}

	public void RegisterLostGoods(IList<Goods> lostGoods)
	{
	}

	private static void DecreaseExpireAndCleanup(PlayerContext ctx)
	{
	}

	private static void AppendLostGoods(PlayerContext ctx, IList<Goods> goodsList)
	{
	}

	private static void AppendLostGoodsEntry(PlayerContext ctx, int idx, Goods goods)
	{
	}

	private static IEnumerable<(int, float, float)> BuildWeightedCandidates(PlayerContext playerContext)
	{
		return null;
	}

	private static void AddSelectInfo(PlayerContext playerContext, List<(int SlotIndex, float Score, float PriceMultiplier)> selected)
	{
	}

	public void RebuildSelectInfos()
	{
	}

	public bool TryRemoveLostGoods(int slotIndex, out Goods goods)
	{
		goods = null;
		return false;
	}

	public int GetLoungeQuestCount()
	{
		return 0;
	}

	public IList<int> GetLoungeQuestIds()
	{
		return null;
	}

	public IList<LoungeQuest> GetLoungeQuests()
	{
		return null;
	}

	public long GetLoungeConditionProgress(EventCondition eventCondition, long currentProgress)
	{
		return 0L;
	}

	private void HandleMonsterRecordFlag(IBaseEvent baseEvent)
	{
	}

	public bool IsUnknownMonster(int monsterId)
	{
		return false;
	}

	public void SetMonsterCollection(int monsterId)
	{
	}

	public bool HasMonsterRecordFlag(int monsterId, EMonsterRecordFlags flag)
	{
		return false;
	}

	public EMonsterRecordFlags GetMonsterRecordFlags(int monsterId)
	{
		return EMonsterRecordFlags.None;
	}

	private void AddMonsterRecordFlags(int monsterId, EMonsterRecordFlags flag)
	{
	}

	public void SetDifficulty(ERoomDifficulty roomDifficulty)
	{
	}

	public void SetLastPlayedRoomDifficulty(ERoomDifficulty roomDifficulty)
	{
	}

	private void CommitPlayTime()
	{
	}

	private string MakePlayerFileName(int playerSlotIndex)
	{
		return null;
	}

	public void CreatePlayer(string userName, ERoomDifficulty difficulty)
	{
	}

	[AsyncStateMachine(typeof(_003CLoadPlayerAsync_003Ed__207))]
	public UniTask<ELoadResult> LoadPlayerAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	private void InitPlayerContext(PlayerContext context)
	{
	}

	private void ApplyStageDead()
	{
	}

	private void SavePlayer()
	{
	}

	[AsyncStateMachine(typeof(_003CSavePlayerAsync_003Ed__211))]
	private UniTask SavePlayerAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void BackupPlayer(long ticks)
	{
	}

	private void OnUpdateAcquisition(Goods goods)
	{
	}

	private PlayerContext CreateNewPlayerContext(int playerSlotIndex)
	{
		return null;
	}

	public GamePlayerData CreateRoomPlayerData(bool isHostPlayer)
	{
		return null;
	}

	private PlayerContextData CreatePlayerContextData(int playerSlotIndex)
	{
		return null;
	}

	private static BitFlags CreateDefaultAreaFlags()
	{
		return null;
	}

	private static BitFlags CreateDefaultAreaDecorationFlags()
	{
		return null;
	}

	private void Patch(AppVersion dataVersion, PlayerContextData data)
	{
	}

	private void Patch_0_0_0_120(PlayerContextData data)
	{
	}

	private static BitFlags RemapBitFlags(BitFlags oldFlags, IEnumerable<(int oldIndex, int newIndex)> indexMap, int newSize)
	{
		return null;
	}

	private bool RepairPlayerData(PlayerContextData data)
	{
		return false;
	}

	private bool RepairGoods(PlayerContextData data)
	{
		return false;
	}

	private bool RepairGoodsSlots(GoodsSlot[] slots)
	{
		return false;
	}

	private bool RepairGoodsItem(Goods goods)
	{
		return false;
	}

	private static bool IsPrefixOverflow(Goods goods)
	{
		return false;
	}

	private static void NormalizeHackPrefixes(Goods goods)
	{
	}

	private static int GetEquipmentPrefixesCount(Goods goods)
	{
		return 0;
	}

	public string GetPlayerStoreString(int id, string key)
	{
		return null;
	}

	public void SetPlayerStoreString(int id, string key, string value)
	{
	}

	public void SetPlayerStoreTempString(int id, string key, string value)
	{
	}

	public int GetPlayerStoreInt(int id, string key)
	{
		return 0;
	}

	public void SetPlayerStoreInt(int id, string key, int value)
	{
	}

	public void SetPlayerStoreTempInt(int id, string key, int value)
	{
	}

	public float GetPlayerStoreFloat(int id, string key)
	{
		return 0f;
	}

	public void SetPlayerStoreFloat(int id, string key, float value)
	{
	}

	public void SetPlayerStoreTempFloat(int id, string key, float value)
	{
	}

	public bool GetPlayerStoreBool(int id, string key)
	{
		return false;
	}

	public void SetPlayerStoreBool(int id, string key, bool value)
	{
	}

	public void SetPlayerStoreTempBool(int id, string key, bool value)
	{
	}

	public void RemovePlayerStore(int id)
	{
	}

	public void RemovePlayerStoreValue(int id, string key)
	{
	}

	public bool TryGetQuickSlotItemId(int slotIndex, out int itemId)
	{
		itemId = default;
		return false;
	}

	public void SetQuickSlot(int settingIndex, int settingItemId)
	{
	}

	public void SwapQuickSlot(int sourceSlotIndex, int targetSlotIndex)
	{
	}

	public void UnlockPlayerCharacterSkin(int characterSkinId)
	{
	}

	public bool IsCharacterSkinUnlocked(int characterSkinId)
	{
		return false;
	}

	public int[] GetAvailableSkinIds(int characterId)
	{
		return null;
	}

	public bool HasTalent(int talentId)
	{
		return false;
	}

	public EUnlockState GetTalentState(int talentId)
	{
		return EUnlockState.None;
	}

	public bool IsSalesSlotUnlocked(int tycoonUnlockId)
	{
		return false;
	}

	public List<int> GetUnlockedSalesSlotIndices()
	{
		return null;
	}

	public void SetPreventRepairMaxDurabilityLoss(bool value)
	{
	}

	public void ChangeName(string newName)
	{
	}

	public bool HasEnoughGold(long amount)
	{
		return false;
	}

	public bool HasEnoughCoin(long amount)
	{
		return false;
	}

	private void UnlockNpcIfConditionsMet(IBaseEvent baseEvent)
	{
	}

	private void UnlockNpcFunctionIfConditionsMet(IBaseEvent baseEvent)
	{
	}

	private void UnlockFeatureIfConditionsMet(IBaseEvent baseEvent)
	{
	}

	private void UnlockNpcsFromCurrentState()
	{
	}

	private void UnlockNpcFunctionsFromCurrentState()
	{
	}

	private void UnlockLoungeBgmsFromCurrentState()
	{
	}

	private void UnlockFeaturesFromCurrentState()
	{
	}

	private void UnlockLoungeBgmIfConditionsMet(IBaseEvent baseEvent)
	{
	}

	public bool HasEnoughGoods(Goods goods)
	{
		return false;
	}

	public bool HasEnoughGoods(Goods[] goodsArray)
	{
		return false;
	}

	public bool HasEnoughGoods(GoodsData data)
	{
		return false;
	}

	public bool HasEnoughGoods(GoodsData[] dataArray)
	{
		return false;
	}

	public long GetInventoryGoodsCount(Goods goods)
	{
		return 0L;
	}

	public long GetStashGoodsCount(Goods goods)
	{
		return 0L;
	}

	public long GetTotalGoodsCount(GoodsData data, bool includeEquippedGoods = false)
	{
		return 0L;
	}

	public long GetTotalGoodsCount(Goods goods, bool includeEquippedGoods = false)
	{
		return 0L;
	}

	public long CalculateEnoughCraftingCount(Goods[] materialItems)
	{
		return 0L;
	}
}
