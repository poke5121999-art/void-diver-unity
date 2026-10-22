using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameObjectManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass128_0
	{
		public bool hadOwner;

		public UnitModel ownerUnitModel;

		public GameObjectManager _003C_003E4__this;

		public HitBoxInfo hitBoxInfo;

		public Vector3 executionPosition;

		public Vector3 startOffset;

		public Quaternion qDirection;

		public int hitBoxId;

		public ActionStatsData ownerStats;

		public HitLayerInfo ownerHitLayer;

		public Vector3 deltaAimPos;

		public bool isFlipped;

		public int skillId;
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnStageSceneLoadedAsync_003Ed__94 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameObjectManager _003C_003E4__this;

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
	private struct _003CSpawnHitBoxAsync_003Ed__128 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<HitBox> _003C_003Et__builder;

		public UnitModel ownerUnitModel;

		public GameObjectManager _003C_003E4__this;

		public Vector3 executionPosition;

		public int hitBoxId;

		public ActionStatsData ownerStats;

		public HitLayerInfo ownerHitLayer;

		public Vector3 deltaAimPos;

		public bool isFlipped;

		public int skillId;

		private _003C_003Ec__DisplayClass128_0 _003C_003E8__1;

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
	private struct _003CSpawnHitBoxByIterator_003Ed__129 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public HitBoxIterator hitBoxIterator;

		public UnitController ownerUnit;

		public ESpawnPositionType spawnPositionType;

		public ITarget target;

		public Vector3 deltaAimPos;

		public float targetPlayerMaxDistance;

		public CancellationTokenSource cancellationTokenSource;

		public GameObjectManager _003C_003E4__this;

		public bool isFlipped;

		public int skillId;

		public float skillTimeScale;

		public ActionStatsData ownerStats;

		public HitLayerInfo ownerHitLayerInfo;

		private UnitModel _003CownerModel_003E5__2;

		private List<Vector3> _003CspawnPositions_003E5__3;

		private int _003CangleGap_003E5__4;

		private float _003CdefaultOffset_003E5__5;

		private int _003CtotalCount_003E5__6;

		private int _003Ci_003E5__7;

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
	private struct _003CSpawnSectorsAsync_003Ed__150 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public GameObjectManager _003C_003E4__this;

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
	private GameManager _gameManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	private CompositeDisposable _disposable;

	private readonly Dictionary<ulong, ITarget> _targets;

	private readonly Dictionary<ulong, UnitController> _unitControllers;

	private readonly HashSet<MonsterModel> _monstersInLocalSight;

	private readonly Dictionary<PlaneSightUnit, HashSet<MonsterModel>> _monstersInSightBySightUnit;

	private readonly Dictionary<ulong, IInteractiveObject> _interactiveObjects;

	private readonly Dictionary<ulong, ICollisionObject> _collisionObjects;

	private readonly Dictionary<ulong, TrapObject> _traps;

	private readonly Dictionary<int, GameObject> _pointerArrows;

	private readonly Dictionary<ulong, SpecialField> _specialFields;

	private readonly Dictionary<ulong, PetController> _petControllers;

	private int _hitLayerIndex;

	private readonly List<(int sectorId, GameObject go)> _spawnedSectors;

	public List<NpcSpawnData> NpcSpawnDatas;

	private List<BreakablePropSpawnData> _breakablePropSpawnDatas;

	private List<CollisionTriggerSpawnData> _collisionTriggerSpawnDatas;

	private List<DisposableObjectSpawnData> _disposableObjectSpawnDatas;

	private List<DropRewardSpawnData> _dropRewardSpawnDatas;

	private List<DropGoodsSpawnData> _dropGoodsSpawnDatas;

	private List<int> _questItemIds;

	private readonly Dictionary<ulong, DropGoods> _dropGoods;

	private List<EntranceSpawnData> _entranceSpawnDatas;

	private List<ExitSpawnData> _exitSpawnDatas;

	private readonly Dictionary<int, List<LockedAreaObject>> _lockedAreaObjects;

	private List<MimicSpawnData> _mimicSpawnDatas;

	private List<MonsterSpawnData> _monsterSpawnDatas;

	private readonly Dictionary<ulong, MonsterController> _bosseMonsterControllers;

	private List<PortalSpawnData> _portalSpawnDatas;

	private Dictionary<int, PortalExitData> _portalExitDatas;

	private List<RewardBoxSpawnData> _rewardBoxSpawnDatas;

	private List<int> _questRewardBoxIds;

	private readonly Dictionary<ulong, RewardBox> _rewardBoxes;

	private List<SkillExecutorSpawnData> _skillExecutorSpawnDatas;

	private List<TrapSpawnData> _trapSpawnDatas;

	private static readonly Vector3 EmployeeNpcOffset;

	private static readonly Vector3 CustomerNpcOffset;

	private readonly Dictionary<int, (NpcObject Npc, int SlotIndex)> _spawnedEmployeeNpcs;

	private readonly Dictionary<int, NpcObject> _spawnedTycoonCustomerNpcs;

	private List<WaveExecutorSpawnData> _waveExecutorSpawnDatas;

	private Vector3 _characterSpawnPoint;

	private Dictionary<int, ZoneData> _zoneDatas;

	public Subject<ITarget> OnTargetSpawned { get; }

	public Subject<ITarget> OnTargetDespawned { get; }

	public IEnumerable<UnitModel> UnitModels => null;

	public Subject<UnitController> OnUnitSpawned { get; }

	public Subject<UnitController> OnUnitDespawned { get; }

	public IReadOnlyCollection<MonsterModel> MonstersInLocalSight => null;

	public Dictionary<ulong, IInteractiveObject> InteractiveObjects => null;

	public Subject<IInteractiveObject> OnInteractiveObjectSpawned { get; }

	public Subject<IInteractiveObject> OnInteractiveObjectDespawned { get; }

	public Subject<NpcObject> OnNpcSpawned { get; }

	public Subject<NpcObject> OnNpcDespawned { get; }

	public Subject<ICollisionObject> OnCollisionObjectSpawned { get; }

	public Subject<ICollisionObject> OnCollisionObjectDespawned { get; }

	public Subject<TrapObject> OnTrapObjectSpawned { get; }

	public Subject<TrapObject> OnTrapObjectDespawned { get; }

	public Subject<PingObject> OnPingObjectSpawned { get; }

	public Subject<PingObject> OnPingObjectDespawned { get; }

	public IEnumerable<SpecialField> SpecialFields => null;

	public Subject<SpecialField> OnSpecialFieldSpawned { get; }

	public Subject<SpecialField> OnSpecialFieldDespawned { get; }

	public Subject<PetController> OnPetSpawned { get; }

	public Subject<PetController> OnPetDespawned { get; }

	private ulong ViewTargetClientId => 0uL;

	public IReadOnlyCollection<MonsterModel> GetMonstersInSight(PlaneSightUnit sightUnit)
	{
		return null;
	}

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	[AsyncStateMachine(typeof(_003COnStageSceneLoadedAsync_003Ed__94))]
	private UniTask OnStageSceneLoadedAsync()
	{
		return default;
	}

	private int GetBalanceSpawnCount(int originCount, float percent)
	{
		return 0;
	}

	private void OnStageClosed()
	{
	}

	private void OnUpdateSec()
	{
	}

	private void OnPlayerEntered(GamePlayer player)
	{
	}

	private void OnPlayerLeft(GamePlayer player)
	{
	}

	public void BroadcastUnitEvent<T>(T unitEvent) where T : IUnitEvent
	{
	}

	public IEnumerable<HitBox> GetHitBoxes()
	{
		return null;
	}

	public ITarget FindTarget(ulong networkObjectId)
	{
		return null;
	}

	public ITarget FindTarget(UnitController unitController, ForceUpdateTargetActionEvent actionEvent)
	{
		return null;
	}

	public List<ITarget> FindTargets(UnitController unitController, ForceUpdateTargetActionEvent actionEvent)
	{
		return null;
	}

	private void OnUnitObjectSpawned(UnitController unitController)
	{
	}

	private void OnUnitObjectDespawned(UnitController unitController)
	{
	}

	private void RemoveMonsterFromSightTracking(MonsterModel monsterModel)
	{
	}

	public UnitController FindUnitController(ulong networkObjectId)
	{
		return null;
	}

	public UnitModel FindUnitModel(ulong networkObjectId)
	{
		return null;
	}

	private void SpawnCharacter(GamePlayer player, Vector3 spawnPosition)
	{
	}

	private Dictionary<ulong, GameObject> SpawnCharacters(Vector3 spawnPoint)
	{
		return null;
	}

	private void DespawnCharacter(GamePlayer player)
	{
	}

	public void RespawnCharacter(GamePlayer player, Vector3 spawnPosition)
	{
	}

	public void SpawnPet(GamePlayer player, Vector3 playerPosition)
	{
	}

	private void DespawnPet(GamePlayer player)
	{
	}

	public bool IsInteractiveObjectExist(ulong networkObjectId)
	{
		return false;
	}

	public IInteractiveObject FindInteractiveObject(ulong networkObjectId)
	{
		return null;
	}

	private void OnObjectSpawned(IInteractiveObject interactiveObject)
	{
	}

	private void OnObjectDespawned(IInteractiveObject interactiveObject)
	{
	}

	private void OnStartHolding(NtfStartInteractionHolding message)
	{
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
	}

	public ICollisionObject FindCollisionObject(ulong networkObjectId)
	{
		return null;
	}

	private void OnCollisionTriggerEnter(NtfCollisionTriggerEnter message)
	{
	}

	public SpecialField FindSpecialField(ulong networkObjectId)
	{
		return null;
	}

	public void SpawnSpecialField(ulong? sourceNetworkObjectId, int specialFieldId, Vector3 position, Vector3 scale)
	{
	}

	public void DespawnSpecialField(int specialFieldId)
	{
	}

	[AsyncStateMachine(typeof(_003CSpawnHitBoxAsync_003Ed__128))]
	public UniTask<HitBox> SpawnHitBoxAsync(int hitBoxId, UnitModel ownerUnitModel, ActionStatsData ownerStats, HitLayerInfo ownerHitLayer, Vector3 deltaAimPos, Vector3 executionPosition, bool isFlipped = false, int skillId = -1)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSpawnHitBoxByIterator_003Ed__129))]
	public UniTaskVoid SpawnHitBoxByIterator(UnitController ownerUnit, ActionStatsData ownerStats, ITarget target, HitBoxIterator hitBoxIterator, HitLayerInfo ownerHitLayerInfo, Vector3 deltaAimPos, bool isFlipped = false, int skillId = -1, float skillTimeScale = 1f, ESpawnPositionType spawnPositionType = ESpawnPositionType.None, float targetPlayerMaxDistance = -1f, CancellationTokenSource cancellationTokenSource = null)
	{
		return default;
	}

	private Vector3 CorrectAimDirection(Vector3 deltaAimPos, Vector3 executionPosition, HitLayerInfo ownerHitLayer, float maxRange, CollisionEvent[] collisionEvents)
	{
		return default;
	}

	private static bool IsValidAimTarget(UnitModel unitModel, HitLayerInfo ownerHitLayer, HashSet<EHitTarget> targetTypes)
	{
		return false;
	}

	public NpcObject FindNpc(int npcId)
	{
		return null;
	}

	public NpcObject FindNpcByFunction(ENpcFunctionType functionType, int value1)
	{
		return null;
	}

	public void SpawnNpc(int npcId, Vector3 position, bool showSpawnVfx)
	{
	}

	public void DespawnNpc(int npcId)
	{
	}

	public ITriggerable FindTriggerableObject(int triggerId)
	{
		return null;
	}

	public WaveExecutor FindWaveExecutor(int waveId)
	{
		return null;
	}

	public bool TrySpawnExtraUnit(int extraUnitId, Vector3 spawnPosition, out ExtraUnitController controller)
	{
		controller = null;
		return false;
	}

	public Inventory FindInventory(ulong networkObjectId, ulong requestClientId)
	{
		return null;
	}

	public WaveExecutor SpawnWaveExecutor(int waveExecutorId, Vector3 spawnPosition, bool executeImmediately = false)
	{
		return null;
	}

	public InteractiveTrigger SpawnInteractiveTrigger(string prefabName, int triggerId, Vector3 position)
	{
		return null;
	}

	public void SpawnEnvironmentProp(string prefabName, Vector3 position, Vector3 forward)
	{
	}

	public void SpawnIndicatorVfx(IndicatorInfo indicatorInfo, UnitModel casterUnitModel, Vector3 position, float angleOffset = 0f, bool isFlipped = false, int skillId = -1, float skillTimeScale = 1f)
	{
	}

	public void SpawnPhoneBooth(Vector3 position)
	{
	}

	public void SpawnPingObject(EPingType pingType, Vector3 position)
	{
	}

	public Portal SpawnPortal(string prefabName, Vector3 spawnPosition, Vector3 exitPosition)
	{
		return null;
	}

	private bool IsInRange(IEnumerable<Vector3> sourcePositions, Vector3 targetPosition, float range)
	{
		return false;
	}

	public void SpawnPointerArrow(int id, Vector3 position)
	{
	}

	public void DespawnPointerArrow(int id)
	{
	}

	[AsyncStateMachine(typeof(_003CSpawnSectorsAsync_003Ed__150))]
	private UniTask SpawnSectorsAsync()
	{
		return default;
	}

	private void InitAreaDecorationObjectSpawns()
	{
	}

	private void InitNpcSpawns()
	{
	}

	private void SpawnNpcs()
	{
	}

	private void OnSpawnNpcsOnEventOccured(NpcUnlockEvent npcUnlockEvent)
	{
	}

	public void ForceSpawnNpcs()
	{
	}

	private void SpawnPhoneBooths()
	{
	}

	private void SpawnBaseMiniMapMarkers()
	{
	}

	private void InitBreakablePropSpawns()
	{
	}

	private void ClearBreakablePropSpawns()
	{
	}

	private List<BreakablePropSpawnData> PickBreakablePropSpawnData()
	{
		return null;
	}

	private void UpdateBreakablePropSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnBreakableProp(int breakablePropId, Vector3 position, Vector3 forward)
	{
	}

	private void InitCollisionTriggerSpawns()
	{
	}

	private void ClearCollisionTriggerSpawns()
	{
	}

	private List<CollisionTriggerSpawnData> PickCollisionTriggerSpawnData()
	{
		return null;
	}

	private void UpdateCollisionTriggerSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnCollisionTrigger(CollisionTriggerSpawnData data)
	{
	}

	private void InitDisposableObjectSpawns()
	{
	}

	private void ClearDisposableObjectSpawns()
	{
	}

	private List<DisposableObjectSpawnData> PickDisposableObjectSpawnData()
	{
		return null;
	}

	private void UpdateDisposableObjectSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnDisposableObject(int disposableObjectId, Vector3 position, Vector3 forward)
	{
	}

	private void InitDropRewardSpawns()
	{
	}

	private void ClearDropRewardSpawns()
	{
	}

	private List<DropRewardSpawnData> PickDropRewardSpawnData()
	{
		return null;
	}

	public void SpawnDropGoods(Goods goods, Vector3 position)
	{
	}

	public void SpawnDropGoods(Goods goods, Vector3 position, int count)
	{
	}

	public void SpawnDropGoods(IEnumerable<Goods> goods, Vector3 position)
	{
	}

	private Vector3 FindDropGoodsPosition(Vector3 position, IReadOnlyList<Vector3> reserved)
	{
		return default;
	}

	private bool IsPathExist(Vector3 from, Vector3 to, int areaMask)
	{
		return false;
	}

	private void SpawnDropGoodsAt(Goods goods, Vector3 spawnPos)
	{
	}

	private void UpdateDropRewardSpawns(IList<Vector3> characterPositions)
	{
	}

	private void UpdateDropItemSpawns(IList<Vector3> characterPositions)
	{
	}

	public bool IsQuestItem(Goods goods)
	{
		return false;
	}

	private void InitEntranceSpawns()
	{
	}

	private void ClearEntranceSpawns()
	{
	}

	private List<EntranceSpawnData> PickEntranceSpawnData()
	{
		return null;
	}

	private void UpdateEntranceSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnEntrance(int entranceId, Vector3 position, Vector3 forward)
	{
	}

	private void InitExitSpawns()
	{
	}

	private List<ExitSpawnData> PickExitSpawnData()
	{
		return null;
	}

	private void SpawnExitSpawn()
	{
	}

	public void SpawnExit(int exitId, Vector3 position)
	{
	}

	public void RefreshCustomerNpcs()
	{
	}

	private void InitLockedAreaObjectSpawns()
	{
	}

	private void SpawnLockedAreaObject(LockedAreaObjectSpawnData spawnData, int areaUnlockId)
	{
	}

	public void DespawnLockedAreaObjects(int areaUnlockId)
	{
	}

	private void InitMimicSpawns()
	{
	}

	private void ClearMimicSpawns()
	{
	}

	private List<MimicSpawnData> PickMimicSpawnData()
	{
		return null;
	}

	private void UpdateMimicSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnMimic(int mimicSpawnId, Vector3 position, Vector3 forward)
	{
	}

	private void InitMonsterSpawns()
	{
	}

	private void InitPatrolMonsterSpawn()
	{
	}

	private void ClearMonsterSpawns()
	{
	}

	private List<MonsterSpawnData> PickSectorMonsterSpawnDatas()
	{
		return null;
	}

	private List<MonsterSpawnData> PickPatrolMonsterSpawnDatas()
	{
		return null;
	}

	public void AddMonsterSpawnData(MonsterSpawnData spawnData)
	{
	}

	public MonsterController FindMonsterController(ulong networkObjectId)
	{
		return null;
	}

	public MonsterController FindMonsterControllerByMonsterId(int monsterId)
	{
		return null;
	}

	private void UpdateMonsterSpawns(IEnumerable<CharacterModel> characters)
	{
	}

	public void TrySpawnMonster(List<SummonMonsterInfo> summonMonsterInfos, Vector3 position, ulong summonerNetworkObjectId)
	{
	}

	public bool TrySpawnMonster(MonsterSpawnData spawnData, bool isLightOn = false, int[] additionalBuffIds = null)
	{
		return false;
	}

	public int GetMonsterCount(int monsterId)
	{
		return 0;
	}

	public IReadOnlyList<MonsterModel> FindBossesInRange()
	{
		return null;
	}

	public MonsterModel FindBoss(ulong networkObjectId)
	{
		return null;
	}

	private void InitPortalSpawns()
	{
	}

	private List<PortalSpawnData> PickPortalSpawnData()
	{
		return null;
	}

	private Dictionary<int, PortalExitData> PickPortalExitDatas()
	{
		return null;
	}

	private void SpawnPortals()
	{
	}

	private void InitRewardBoxSpawns()
	{
	}

	private void ClearRewardBoxSpawns()
	{
	}

	private List<RewardBoxSpawnData> PickRewardBoxSpawnData()
	{
		return null;
	}

	private void UpdateRewardBoxesSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnRewardBox(int rewardBoxId, Vector3 position, Vector3 forward, int respawnItemSlotCount = -1, IEnumerable<GoodsSlot> respawnItemSlots = null, ELockState lockState = ELockState.None, bool isReplace = false)
	{
	}

	public bool IsQuestRewardBox(int rewardBoxId)
	{
		return false;
	}

	private void OnSpawnSequentialTimeline(NtfSpawnSequentialTimeline message)
	{
	}

	private void InitSkillExecutorSpawns()
	{
	}

	private void ClearSkillExecutorSpawns()
	{
	}

	private List<SkillExecutorSpawnData> PickSkillExecutorSpawnData()
	{
		return null;
	}

	private void UpdateSkillExecutorSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnSkillExecutor(int skillExecutorId, Vector3 position)
	{
	}

	private void InitTrapSpawns()
	{
	}

	private void ClearTrapSpawns()
	{
	}

	private List<TrapSpawnData> PickTrapSpawnData()
	{
		return null;
	}

	private void UpdateTrapSpawns(IList<Vector3> characterPositions)
	{
	}

	public void SpawnTrap(int trapId, Vector3 position, Vector3 forward)
	{
	}

	public NpcObject FindEmployeeNpc(int employeeId)
	{
		return null;
	}

	private NpcObject FindSalesSlotNpc(int slotIndex)
	{
		return null;
	}

	public void SpawnEmployeeNpc(int employeeId, int slotIndex)
	{
	}

	private NpcObject SpawnEmployeeNpcInternal(int npcId, Vector3 position, int slotIndex)
	{
		return null;
	}

	public void DespawnEmployeeNpc(int employeeId)
	{
	}

	private void DespawnAllEmployeeNpcs()
	{
	}

	public void SpawnTycoonCustomerNpc(int slotIndex, int customerNpcId)
	{
	}

	public void DespawnTycoonCustomerNpc(int slotIndex)
	{
	}

	public void RefreshSlotNpcs(int slotIndex)
	{
	}

	public void RefreshTycoonNpcs()
	{
	}

	public void DespawnAllTycoonNpcs()
	{
	}

	private void OnPlayNpcAnimation(NtfPlayNpcAnimation message)
	{
	}

	private NpcObject SpawnTycoonNpc(int npcId, Vector3 position)
	{
		return null;
	}

	private void InitWaveExecutorSpawns()
	{
	}

	private List<WaveExecutorSpawnData> PickWaveExecutorSpawnData()
	{
		return null;
	}

	private void SpawnWaveExecutors()
	{
	}

	private void InitZoneObjectSpawns()
	{
	}

	private IList<ZoneData> GetMapZoneDatas()
	{
		return null;
	}

	private bool CheckDuplicateZoneSpawnIds(IList<ZoneData> zoneDatas)
	{
		return false;
	}

	private void RemoveBaseObjects(IList<ZoneData> zoneDatas)
	{
	}

	private void SpawnZoneObjects(IList<ZoneData> zoneDatas)
	{
	}

	private void SpawnLocalZoneObjectSpawns()
	{
	}

	public Vector3 GetZoneSpawnPosition(int zoneSpawnId)
	{
		return default;
	}

	private void OnAssignedZones(NtfAssignedZones message)
	{
	}
}
