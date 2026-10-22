using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameObjectManager : MonoBase
{
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

	private readonly Dictionary<ulong, ITarget> _targets = new Dictionary<ulong, ITarget>();

	private readonly Dictionary<ulong, UnitController> _unitControllers = new Dictionary<ulong, UnitController>();

	private readonly HashSet<MonsterModel> _monstersInLocalSight = new HashSet<MonsterModel>();

	private readonly Dictionary<PlaneSightUnit, HashSet<MonsterModel>> _monstersInSightBySightUnit = new Dictionary<PlaneSightUnit, HashSet<MonsterModel>>();

	private readonly Dictionary<ulong, IInteractiveObject> _interactiveObjects = new Dictionary<ulong, IInteractiveObject>();

	private readonly Dictionary<ulong, ICollisionObject> _collisionObjects = new Dictionary<ulong, ICollisionObject>();

	private readonly Dictionary<ulong, TrapObject> _traps = new Dictionary<ulong, TrapObject>();

	private readonly Dictionary<int, GameObject> _pointerArrows = new Dictionary<int, GameObject>();

	private readonly Dictionary<ulong, SpecialField> _specialFields = new Dictionary<ulong, SpecialField>();

	private readonly Dictionary<ulong, PetController> _petControllers = new Dictionary<ulong, PetController>();

	private int _hitLayerIndex;

	private readonly List<(int sectorId, GameObject go)> _spawnedSectors = new List<(int sectorId, GameObject go)>();

	public List<NpcSpawnData> NpcSpawnDatas = new List<NpcSpawnData>();

	private List<BreakablePropSpawnData> _breakablePropSpawnDatas;

	private List<CollisionTriggerSpawnData> _collisionTriggerSpawnDatas;

	private List<DisposableObjectSpawnData> _disposableObjectSpawnDatas;

	private List<DropRewardSpawnData> _dropRewardSpawnDatas;

	private List<DropGoodsSpawnData> _dropGoodsSpawnDatas;

	private List<int> _questItemIds;

	private readonly Dictionary<ulong, DropGoods> _dropGoods = new Dictionary<ulong, DropGoods>();

	private List<EntranceSpawnData> _entranceSpawnDatas;

	private List<ExitSpawnData> _exitSpawnDatas;

	private readonly Dictionary<int, List<LockedAreaObject>> _lockedAreaObjects = new Dictionary<int, List<LockedAreaObject>>();

	private List<MimicSpawnData> _mimicSpawnDatas;

	private List<MonsterSpawnData> _monsterSpawnDatas;

	private readonly Dictionary<ulong, MonsterController> _bosseMonsterControllers = new Dictionary<ulong, MonsterController>();

	private List<PortalSpawnData> _portalSpawnDatas;

	private Dictionary<int, PortalExitData> _portalExitDatas;

	private List<RewardBoxSpawnData> _rewardBoxSpawnDatas;

	private List<int> _questRewardBoxIds;

	private readonly Dictionary<ulong, RewardBox> _rewardBoxes = new Dictionary<ulong, RewardBox>();

	private List<SkillExecutorSpawnData> _skillExecutorSpawnDatas;

	private List<TrapSpawnData> _trapSpawnDatas;

	private static readonly Vector3 EmployeeNpcOffset = new Vector3(-0.6f, 0f, -0.4f);

	private static readonly Vector3 CustomerNpcOffset = new Vector3(0.4f, 0f, 0.6f);

	private readonly Dictionary<int, (NpcObject Npc, int SlotIndex)> _spawnedEmployeeNpcs = new Dictionary<int, (NpcObject Npc, int SlotIndex)>();

	private readonly Dictionary<int, NpcObject> _spawnedTycoonCustomerNpcs = new Dictionary<int, NpcObject>();

	private List<WaveExecutorSpawnData> _waveExecutorSpawnDatas;

	private Vector3 _characterSpawnPoint;

	private Dictionary<int, ZoneData> _zoneDatas = new Dictionary<int, ZoneData>();

	public Subject<ITarget> OnTargetSpawned { get; } = new Subject<ITarget>();

	public Subject<ITarget> OnTargetDespawned { get; } = new Subject<ITarget>();

	public IEnumerable<UnitModel> UnitModels => _unitControllers.Values.Select((UnitController c) => c.GetComponent<UnitModel>());

	public Subject<UnitController> OnUnitSpawned { get; } = new Subject<UnitController>();

	public Subject<UnitController> OnUnitDespawned { get; } = new Subject<UnitController>();

	public IReadOnlyCollection<MonsterModel> MonstersInLocalSight => _monstersInLocalSight;

	public Dictionary<ulong, IInteractiveObject> InteractiveObjects => _interactiveObjects;

	public Subject<IInteractiveObject> OnInteractiveObjectSpawned { get; } = new Subject<IInteractiveObject>();

	public Subject<IInteractiveObject> OnInteractiveObjectDespawned { get; } = new Subject<IInteractiveObject>();

	public Subject<NpcObject> OnNpcSpawned { get; } = new Subject<NpcObject>();

	public Subject<NpcObject> OnNpcDespawned { get; } = new Subject<NpcObject>();

	public Subject<ICollisionObject> OnCollisionObjectSpawned { get; } = new Subject<ICollisionObject>();

	public Subject<ICollisionObject> OnCollisionObjectDespawned { get; } = new Subject<ICollisionObject>();

	public Subject<TrapObject> OnTrapObjectSpawned { get; } = new Subject<TrapObject>();

	public Subject<TrapObject> OnTrapObjectDespawned { get; } = new Subject<TrapObject>();

	public Subject<PingObject> OnPingObjectSpawned { get; } = new Subject<PingObject>();

	public Subject<PingObject> OnPingObjectDespawned { get; } = new Subject<PingObject>();

	public IEnumerable<SpecialField> SpecialFields => _specialFields.Values;

	public Subject<SpecialField> OnSpecialFieldSpawned { get; } = new Subject<SpecialField>();

	public Subject<SpecialField> OnSpecialFieldDespawned { get; } = new Subject<SpecialField>();

	public Subject<PetController> OnPetSpawned { get; } = new Subject<PetController>();

	public Subject<PetController> OnPetDespawned { get; } = new Subject<PetController>();

	private ulong ViewTargetClientId => _gameManager.RxViewTargetClientId.Value;

	public IReadOnlyCollection<MonsterModel> GetMonstersInSight(PlaneSightUnit sightUnit)
	{
		if (sightUnit != null && _monstersInSightBySightUnit.TryGetValue(sightUnit, out var monsters))
		{
			return monsters;
		}
		return System.Array.Empty<MonsterModel>();
	}

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

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
		return _targets.Values.OfType<HitBox>();
	}

	public ITarget FindTarget(ulong networkObjectId)
	{
		return _targets.GetValueOrDefault(networkObjectId);
	}

	public ITarget FindTarget(UnitController unitController, ForceUpdateTargetActionEvent actionEvent)
	{
		return FindTargets(unitController, actionEvent).FirstOrDefault();
	}

	public List<ITarget> FindTargets(UnitController unitController, ForceUpdateTargetActionEvent actionEvent)
	{
		if (unitController == null)
		{
			_logger.Error("UnitController is null");
			return null;
		}
		Vector3 origin = unitController.transform.position;
		IEnumerable<ITarget> candidates = _targets.Values.Where((ITarget t) =>
		{
			if (t == null)
			{
				return false;
			}
			if (Vector3.Distance(t.Position, origin) > actionEvent.MaxDistance)
			{
				return false;
			}
			if (t is CharacterModel && actionEvent.AllowCharacterTarget)
			{
				return true;
			}
			if (t is MonsterModel monsterModel && actionEvent.AllowMonsterTarget && actionEvent.MonsterIds.Contains(monsterModel.MonsterId))
			{
				return true;
			}
			if (t is HitBox hitBox)
			{
				return actionEvent.AllowHitBox && actionEvent.HitBoxIds.Contains(hitBox.HitBoxId);
			}
			return false;
		});
		if (!candidates.Any())
		{
			return null;
		}
		List<ITarget> list = candidates.ToList();
		switch (actionEvent.PriorityType)
		{
		case EForceUpdateTargetPriorityType.Random:
			return new List<ITarget> { list.OrderBy((ITarget _) => System.Guid.NewGuid()).FirstOrDefault() };
		case EForceUpdateTargetPriorityType.Closest:
			return new List<ITarget> { list.MinBy((ITarget t) => Vector3.Distance(t.Position, origin)) };
		case EForceUpdateTargetPriorityType.All:
			return list;
		default:
			return null;
		}
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
		return _unitControllers.GetValueOrDefault(networkObjectId);
	}

	public UnitModel FindUnitModel(ulong networkObjectId)
	{
		UnitController controller = _unitControllers.GetValueOrDefault(networkObjectId);
		return controller != null ? controller.GetComponent<UnitModel>() : null;
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
		return _interactiveObjects.ContainsKey(networkObjectId);
	}

	public IInteractiveObject FindInteractiveObject(ulong networkObjectId)
	{
		return _interactiveObjects.GetValueOrDefault(networkObjectId);
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
		return _collisionObjects.GetValueOrDefault(networkObjectId);
	}

	private void OnCollisionTriggerEnter(NtfCollisionTriggerEnter message)
	{
	}

	public SpecialField FindSpecialField(ulong networkObjectId)
	{
		return _specialFields.GetValueOrDefault(networkObjectId);
	}

	public void SpawnSpecialField(ulong? sourceNetworkObjectId, int specialFieldId, Vector3 position, Vector3 scale)
	{
	}

	public void DespawnSpecialField(int specialFieldId)
	{
	}

	public UniTask<HitBox> SpawnHitBoxAsync(int hitBoxId, UnitModel ownerUnitModel, ActionStatsData ownerStats, HitLayerInfo ownerHitLayer, Vector3 deltaAimPos, Vector3 executionPosition, bool isFlipped = false, int skillId = -1)
	{
		return default;
	}

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
		return _interactiveObjects.Values.Select((IInteractiveObject o) => o as NpcObject).Where((NpcObject n) => n != null).FirstOrDefault((NpcObject n) => n.NpcId == npcId);
	}

	public NpcObject FindNpcByFunction(ENpcFunctionType functionType, int value1)
	{
		return _interactiveObjects.Values.Select((IInteractiveObject o) => o as NpcObject).Where((NpcObject n) => n != null).FirstOrDefault((NpcObject n) => n.NpcFunctionIds != null && n.NpcFunctionIds.Select((int id) => TNpcFunction.Table[id]).Any((TNpcFunction f) => f.Type == functionType && f.Value1 == value1));
	}

	public void SpawnNpc(int npcId, Vector3 position, bool showSpawnVfx)
	{
	}

	public void DespawnNpc(int npcId)
	{
	}

	public ITriggerable FindTriggerableObject(int triggerId)
	{
		ITriggerable triggerable = _interactiveObjects.Values.Select((IInteractiveObject o) => o as ITriggerable).Where((ITriggerable t) => t != null).FirstOrDefault((ITriggerable t) => t.TriggerId == triggerId);
		if (triggerable != null)
		{
			return triggerable;
		}
		return _collisionObjects.Values.Select((ICollisionObject o) => o as ITriggerable).Where((ITriggerable t) => t != null).FirstOrDefault((ITriggerable t) => t.TriggerId == triggerId);
	}

	public WaveExecutor FindWaveExecutor(int waveId)
	{
		return _interactiveObjects.Values.Select((IInteractiveObject o) => o as WaveExecutor).Where((WaveExecutor w) => w != null).FirstOrDefault((WaveExecutor w) => w.WaveId == waveId);
	}

	public bool TrySpawnExtraUnit(int extraUnitId, Vector3 spawnPosition, out ExtraUnitController controller)
	{
		controller = null;
		return false;
	}

	public Inventory FindInventory(ulong networkObjectId, ulong requestClientId)
	{
		IInteractiveObject interactiveObject = _interactiveObjects.GetValueOrDefault(networkObjectId);
		if (interactiveObject != null)
		{
			if (interactiveObject is MonsterBody monsterBody)
			{
				return monsterBody.FindInventory(requestClientId);
			}
			if (interactiveObject is RewardBox rewardBox)
			{
				return rewardBox.LootingInventory;
			}
		}
		UnitModel unitModel = FindUnitModel(networkObjectId);
		if (unitModel != null && unitModel is CharacterModel characterModel)
		{
			return characterModel.Inventory;
		}
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
		return _unitControllers.GetValueOrDefault(networkObjectId) as MonsterController;
	}

	public MonsterController FindMonsterControllerByMonsterId(int monsterId)
	{
		return _unitControllers.Values.OfType<MonsterController>().FirstOrDefault((MonsterController c) => c.MonsterId == monsterId);
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
		_gameManager.AssertIfNotHost();
		int spawned = _unitControllers.Values.OfType<MonsterController>().Count((MonsterController c) => c.MonsterId == monsterId && c.IsActivateInStage);
		int pending = _monsterSpawnDatas.Count((MonsterSpawnData d) => d.MonsterId == monsterId);
		return pending + spawned;
	}

	public IReadOnlyList<MonsterModel> FindBossesInRange()
	{
		if (_bosseMonsterControllers.Count == 0)
		{
			return System.Array.Empty<MonsterModel>();
		}
		CharacterController viewTarget = _gameManager.FindCharacterController(_gameManager.RxViewTargetClientId.Value);
		if (viewTarget == null)
		{
			return System.Array.Empty<MonsterModel>();
		}
		Vector3 targetPos = viewTarget.transform.position;
		return _bosseMonsterControllers.Values
			.Select((MonsterController c) => c != null ? c.GetComponent<MonsterModel>() : null)
			.Where((MonsterModel m) =>
			{
				if (m == null)
				{
					return false;
				}
				if (m.State == EActionState.Dead || m.State == EActionState.Revive || m.State == EActionState.Dying)
				{
					return false;
				}
				if (!m.HasTarget)
				{
					return false;
				}
				float distance = Vector3.Distance(m.Position, targetPos);
				return distance <= TMonster.Table[m.MonsterId].BossDetectionRange;
			})
			.ToList();
	}

	public MonsterModel FindBoss(ulong networkObjectId)
	{
		if (_bosseMonsterControllers.TryGetValue(networkObjectId, out var controller) && controller != null)
		{
			return controller.GetComponent<MonsterModel>();
		}
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
		if (_spawnedEmployeeNpcs.TryGetValue(employeeId, out var entry))
		{
			return entry.Npc;
		}
		return null;
	}

	private NpcObject FindSalesSlotNpc(int slotIndex)
	{
		return FindNpcByFunction(ENpcFunctionType.TycoonSalesSlot, slotIndex);
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
