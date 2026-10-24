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
using SomeNet.Collections;
using SomeNet.Utility;
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
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		OnTargetSpawned.Subscribe(delegate(ITarget t)
		{
			_targets.Add(t.NetworkObjectId, t);
		}).AddTo(this);
		OnTargetDespawned.Subscribe(delegate(ITarget t)
		{
			_targets.Remove(t.NetworkObjectId);
		}).AddTo(this);
		OnUnitSpawned.Subscribe(OnUnitObjectSpawned).AddTo(this);
		OnUnitDespawned.Subscribe(OnUnitObjectDespawned).AddTo(this);
		OnInteractiveObjectSpawned.Subscribe(OnObjectSpawned).AddTo(this);
		OnInteractiveObjectDespawned.Subscribe(OnObjectDespawned).AddTo(this);
		OnCollisionObjectSpawned.Subscribe(delegate(ICollisionObject o)
		{
			_collisionObjects.Add(o.NetworkObjectId, o);
		}).AddTo(this);
		OnCollisionObjectDespawned.Subscribe(delegate(ICollisionObject o)
		{
			_collisionObjects.Remove(o.NetworkObjectId);
		}).AddTo(this);
		OnTrapObjectSpawned.Subscribe(delegate(TrapObject o)
		{
			_traps.Add(o.NetworkObjectId, o);
		}).AddTo(this);
		OnTrapObjectDespawned.Subscribe(delegate(TrapObject o)
		{
			_traps.Remove(o.NetworkObjectId);
		}).AddTo(this);
		OnSpecialFieldSpawned.Subscribe(delegate(SpecialField o)
		{
			_specialFields.Add(o.NetworkObjectId, o);
		}).AddTo(this);
		OnSpecialFieldDespawned.Subscribe(delegate(SpecialField o)
		{
			_specialFields.Remove(o.NetworkObjectId);
		}).AddTo(this);
		OnPetSpawned.Subscribe(delegate(PetController pet)
		{
			_petControllers[pet.PetModel.SyncOwnerClientId.Value] = pet;
		}).AddTo(this);
		OnPetDespawned.Subscribe(delegate(PetController pet)
		{
			_petControllers.Remove(pet.PetModel.SyncOwnerClientId.Value);
		}).AddTo(this);
		_gameChannel.Receive<NtfStartInteractionHolding>().Subscribe(OnStartHolding).AddTo(this);
		_gameChannel.Receive<NtfStartInteraction>().Subscribe(OnStartInteraction).AddTo(this);
		_gameChannel.Receive<NtfEndInteraction>().Subscribe(OnEndInteraction).AddTo(this);
		_gameChannel.Receive<NtfCollisionTriggerEnter>().Subscribe(OnCollisionTriggerEnter).AddTo(this);
		_gameChannel.Receive<NtfAssignedZones>().Subscribe(OnAssignedZones).AddTo(this);
		_gameChannel.Receive<NtfPlayNpcAnimation>().Subscribe(OnPlayNpcAnimation).AddTo(this);
		_gameChannel.Receive<NtfSpawnSequentialTimeline>().Subscribe(OnSpawnSequentialTimeline).AddTo(this);
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.SceneLoaded).Subscribe(delegate
		{
			OnStageSceneLoadedAsync().Forget();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
		_gameManager.OnPlayerEntered.Subscribe(OnPlayerEntered).AddTo(this);
		_gameManager.OnPlayerLeft.Subscribe(OnPlayerLeft).AddTo(this);
		_gameLoungeManager.OnAreaUnlocked.Subscribe(DespawnLockedAreaObjects).AddTo(this);
	}

	private async UniTask OnStageSceneLoadedAsync()
	{
		_disposable = new CompositeDisposable();
		await SpawnSectorsAsync();
		if (_gameManager.IsHost)
		{
			InitMonsterSpawns();
			InitDropRewardSpawns();
			InitRewardBoxSpawns();
			InitExitSpawns();
			InitTrapSpawns();
			InitMimicSpawns();
			InitSkillExecutorSpawns();
			InitWaveExecutorSpawns();
			InitPortalSpawns();
			InitDisposableObjectSpawns();
			InitBreakablePropSpawns();
			InitEntranceSpawns();
			InitLockedAreaObjectSpawns();
			InitCollisionTriggerSpawns();
			InitNpcSpawns();
			InitZoneObjectSpawns();
			InitPatrolMonsterSpawn();
			Dictionary<ulong, GameObject> characterGameObjects = SpawnCharacters(_characterSpawnPoint);
			CharacterModel[] characters = characterGameObjects.Values.Select((GameObject go) => go.GetComponent<CharacterModel>()).ToArray();
			List<Vector3> characterPositions = characters.Select((CharacterModel c) => c.transform.position).ToList();
			_gameContext.Players.Values.ForEach(delegate(GamePlayer player)
			{
				SpawnPet(player, characterGameObjects[player.ClientId].transform.position);
			});
			UpdateMonsterSpawns(characters);
			UpdateDropRewardSpawns(characterPositions);
			UpdateRewardBoxesSpawns(characterPositions);
			UpdateTrapSpawns(characterPositions);
			UpdateMimicSpawns(characterPositions);
			UpdateSkillExecutorSpawns(characterPositions);
			UpdateDisposableObjectSpawns(characterPositions);
			UpdateBreakablePropSpawns(characterPositions);
			UpdateEntranceSpawns(characterPositions);
			SpawnWaveExecutors();
			SpawnPortals();
			SpawnExitSpawn();
			SpawnPhoneBooths();
			SpawnNpcs();
			_eventSystem.OnEventOccured.Where((IBaseEvent e) => e is NpcUnlockEvent).Select((IBaseEvent e) => e as NpcUnlockEvent).Subscribe(OnSpawnNpcsOnEventOccured)
				.AddTo(_disposable);
			if (_gameManager.IsInLounge)
			{
				RefreshCustomerNpcs();
				if (_gameContext.CampaignId == 0)
				{
					RefreshTycoonNpcs();
				}
			}
		}
		SpawnLocalZoneObjectSpawns();
		SpawnBaseMiniMapMarkers();
		InitAreaDecorationObjectSpawns();
		Observable.Interval(TimeSpan.FromSeconds(1.0)).Subscribe(delegate
		{
			OnUpdateSec();
		}).AddTo(_disposable);
	}

	private int GetBalanceSpawnCount(int originCount, float percent)
	{
		// 0x184a51a20 is ceil(double) (machine code checked)
		return System.Math.Max(1, (int)System.Math.Ceiling((double)((float)originCount * percent / 100f)));
	}

	private void OnStageClosed()
	{
		DespawnAllTycoonNpcs();
		_disposable?.Dispose();
		_disposable = null;
		_monsterSpawnDatas = null;
		_dropRewardSpawnDatas = null;
		_dropGoodsSpawnDatas = null;
		_questItemIds = null;
		_rewardBoxSpawnDatas = null;
		_questRewardBoxIds = null;
		_trapSpawnDatas = null;
		_mimicSpawnDatas = null;
		_skillExecutorSpawnDatas = null;
		_disposableObjectSpawnDatas = null;
		_breakablePropSpawnDatas = null;
		_entranceSpawnDatas = null;
		_collisionTriggerSpawnDatas = null;
		_targets.Clear();
		_unitControllers.Clear();
		_monstersInLocalSight.Clear();
		_monstersInSightBySightUnit.Clear();
		_interactiveObjects.Clear();
		_traps.Clear();
		_pointerArrows.Clear();
		_hitLayerIndex = 0;
	}

	private void OnUpdateSec()
	{
		if (!_gameManager.IsHost)
		{
			return;
		}
		IReadOnlyList<CharacterModel> characterModels = _gameManager.CharacterModels;
		List<Vector3> characterPositions = characterModels.Select((CharacterModel c) => c.transform.position).ToList();
		UpdateMonsterSpawns(characterModels);
		UpdateDropRewardSpawns(characterPositions);
		UpdateDropItemSpawns(characterPositions);
		UpdateRewardBoxesSpawns(characterPositions);
		UpdateTrapSpawns(characterPositions);
		UpdateMimicSpawns(characterPositions);
		UpdateSkillExecutorSpawns(characterPositions);
		UpdateEntranceSpawns(characterPositions);
		UpdateDisposableObjectSpawns(characterPositions);
		UpdateBreakablePropSpawns(characterPositions);
		UpdateCollisionTriggerSpawns(characterPositions);
	}

	private void OnPlayerEntered(GamePlayer player)
	{
		if (_gameManager.IsHost)
		{
			SpawnCharacter(player, _characterSpawnPoint);
			SpawnPet(player, _characterSpawnPoint);
		}
	}

	private void OnPlayerLeft(GamePlayer player)
	{
		if (!_gameManager.IsHost)
		{
			return;
		}
		DespawnCharacter(player);
		if (_petControllers.TryGetValue(player.ClientId, out var pet) && pet != null)
		{
			pet.GetComponent<Unity.Netcode.NetworkObject>().Despawn(destroy: true);
		}
	}

	public void BroadcastUnitEvent<T>(T unitEvent) where T : IUnitEvent
	{
		// TODO(decomp): generic body not in binary (@0x0); broadcast to every unit controller assumed from callers (NoticeDieEvent, NoticeCrowdControlEvent)
		foreach (UnitController unitController in _unitControllers.Values)
		{
			unitController.HandleUnitEvent(unitEvent);
		}
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
		_unitControllers.Add(unitController.NetworkObjectId, unitController);
		if (!(unitController is MonsterController monsterController))
		{
			return;
		}
		if (monsterController.IsBoss)
		{
			_bosseMonsterControllers.Add(unitController.NetworkObjectId, monsterController);
		}
		MonsterModel monsterModel = unitController.GetComponent<MonsterModel>();
		PlaneSightDetectableObject detectable = unitController.GetComponentInChildren<PlaneSightDetectableObject>();
		if (!(monsterModel != null) || !(detectable != null))
		{
			return;
		}
		detectable.RxIsDetectedByMine.Subscribe(delegate(bool isDetected)
		{
			if (isDetected)
			{
				if (monsterModel.IsHitable)
				{
					_monstersInLocalSight.Add(monsterModel);
				}
			}
			else
			{
				_monstersInLocalSight.Remove(monsterModel);
			}
		}).AddTo(unitController);
		monsterModel.RxState.Where((EActionState state) => state == EActionState.Dead).Subscribe(delegate
		{
			RemoveMonsterFromSightTracking(monsterModel);
		}).AddTo(unitController);
		if (!_gameManager.IsHost)
		{
			return;
		}
		HashSet<PlaneSightUnit> prevDetecting = new HashSet<PlaneSightUnit>();
		detectable.OnDetectingUnitsChanged.Subscribe(delegate
		{
			List<PlaneSightUnit> current = detectable.DetectingPlaneSightUnits;
			foreach (PlaneSightUnit unit in current.Where((PlaneSightUnit u) => !prevDetecting.Contains(u)))
			{
				if (!_monstersInSightBySightUnit.TryGetValue(unit, out var monsters))
				{
					monsters = new HashSet<MonsterModel>();
					_monstersInSightBySightUnit[unit] = monsters;
				}
				if (monsterModel.State != EActionState.Dead)
				{
					monsters.Add(monsterModel);
				}
			}
			foreach (PlaneSightUnit unit2 in prevDetecting.Where((PlaneSightUnit u) => !current.Contains(u)))
			{
				if (_monstersInSightBySightUnit.TryGetValue(unit2, out var monsters2))
				{
					monsters2.Remove(monsterModel);
				}
			}
			prevDetecting.Clear();
			prevDetecting.UnionWith(current);
		}).AddTo(unitController);
	}

	private void OnUnitObjectDespawned(UnitController unitController)
	{
		_unitControllers.Remove(unitController.NetworkObjectId);
		if (unitController is MonsterController)
		{
			_bosseMonsterControllers.Remove(unitController.NetworkObjectId);
			MonsterModel monsterModel = unitController.GetComponent<MonsterModel>();
			if (monsterModel != null)
			{
				RemoveMonsterFromSightTracking(monsterModel);
			}
		}
	}

	private void RemoveMonsterFromSightTracking(MonsterModel monsterModel)
	{
		_monstersInLocalSight.Remove(monsterModel);
		foreach (HashSet<MonsterModel> monsters in _monstersInSightBySightUnit.Values)
		{
			monsters.Remove(monsterModel);
		}
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
		TCharacter tCharacter = TCharacter.Table[player.Character.Id];
		int hitLayerIndex = _hitLayerIndex++;
		CharacterController characterController = _assetManager.InstantiateCharacter(tCharacter.Id);
		characterController.transform.position = spawnPosition;
		characterController.transform.rotation = Quaternion.identity;
		characterController.Init(new CharacterController.Args
		{
			PlayerClientId = player.ClientId,
			HitLayer = hitLayerIndex + 1001
		});
		characterController.Spawn(destroyWithScene: true);
	}

	private Dictionary<ulong, GameObject> SpawnCharacters(Vector3 spawnPoint)
	{
		Vector3[] offsets = new Vector3[3]
		{
			new Vector3(0.25f, 0f, -1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1.25f, 0f, -1f)
		};
		Dictionary<ulong, GameObject> spawned = new Dictionary<ulong, GameObject>();
		if (_gameContext.IsInStage)
		{
			IntroTimelineObject intro = _assetManager.InstantiateIntroTimelineObject();
			intro.transform.position = spawnPoint;
			intro.transform.rotation = Quaternion.identity;
			intro.Spawn(destroyWithScene: true);
		}
		int index = 0;
		foreach (KeyValuePair<ulong, GamePlayer> pair in _gameContext.Players)
		{
			TCharacter tCharacter = TCharacter.Table[pair.Value.Character.Id];
			int hitLayerIndex = _hitLayerIndex++;
			CharacterController characterController = _assetManager.InstantiateCharacter(tCharacter.Id);
			characterController.transform.position = spawnPoint + offsets[index];
			characterController.transform.rotation = Quaternion.identity;
			characterController.Init(new CharacterController.Args
			{
				PlayerClientId = pair.Key,
				HitLayer = hitLayerIndex + 1001
			});
			characterController.Spawn(destroyWithScene: true);
			spawned[pair.Key] = characterController.gameObject;
			index++;
		}
		return spawned;
	}

	private void DespawnCharacter(GamePlayer player)
	{
		CharacterController characterController = _gameManager.FindCharacterController(player.ClientId);
		if (!(characterController == null))
		{
			characterController.GetComponent<Unity.Netcode.NetworkObject>()?.Despawn(destroy: true);
		}
	}

	public void RespawnCharacter(GamePlayer player, Vector3 spawnPosition)
	{
		DespawnCharacter(player);
		SpawnCharacter(player, spawnPosition);
	}

	public void SpawnPet(GamePlayer player, Vector3 playerPosition)
	{
		if (player == null || player.Profile == null || player.Profile.SelectedPetId < 0)
		{
			return;
		}
		Vector3 position = playerPosition + new Vector3(0.3f, 0f, 0.3f);
		TPet tPet = TPet.Table[player.Profile.SelectedPetId];
		PetController petController = _assetManager.InstantiatePet(tPet.Id);
		petController.transform.position = position;
		petController.transform.rotation = Quaternion.identity;
		petController.Init(new PetController.Args
		{
			PetId = tPet.Id,
			OwnerClientId = player.ClientId
		});
		petController.Spawn(destroyWithScene: true);
	}

	private void DespawnPet(GamePlayer player)
	{
		if (_petControllers.TryGetValue(player.ClientId, out var pet) && pet != null)
		{
			pet.GetComponent<Unity.Netcode.NetworkObject>().Despawn(destroy: true);
		}
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
		_interactiveObjects.Add(interactiveObject.NetworkObjectId, interactiveObject);
		if (interactiveObject is RewardBox rewardBox)
		{
			_rewardBoxes.Add(rewardBox.NetworkObjectId, rewardBox);
		}
		else if (interactiveObject is DropGoods dropGoods)
		{
			_dropGoods.Add(dropGoods.NetworkObjectId, dropGoods);
		}
	}

	private void OnObjectDespawned(IInteractiveObject interactiveObject)
	{
		_interactiveObjects.Remove(interactiveObject.NetworkObjectId);
		if (interactiveObject is RewardBox rewardBox)
		{
			_rewardBoxes.Remove(rewardBox.NetworkObjectId);
		}
		else if (interactiveObject is DropGoods dropGoods)
		{
			_dropGoods.Remove(dropGoods.NetworkObjectId);
		}
	}

	private void OnStartHolding(NtfStartInteractionHolding message)
	{
		IInteractiveObject interactiveObject = _interactiveObjects.GetValueOrDefault(message.TargetNetworkObjectId);
		interactiveObject?.OnStartHolding(message.SourceClientId);
		if (message.SourceClientId == _gameManager.RxViewTargetClientId.Value && !string.IsNullOrEmpty(interactiveObject?.HoldingSfx) && interactiveObject != null)
		{
			_soundManager.PlaySfx(interactiveObject.HoldingSfx);
		}
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
		IInteractiveObject interactiveObject = _interactiveObjects.GetValueOrDefault(message.TargetNetworkObjectId);
		interactiveObject?.OnStartInteraction(message.SourceClientId);
		if (message.SourceClientId == _gameManager.RxViewTargetClientId.Value && !string.IsNullOrEmpty(interactiveObject?.InteractionSfx) && interactiveObject != null)
		{
			_soundManager.PlaySfx(interactiveObject.InteractionSfx);
		}
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
		_interactiveObjects.GetValueOrDefault(message.TargetNetworkObjectId)?.OnEndInteraction(message.SourceClientId);
	}

	public ICollisionObject FindCollisionObject(ulong networkObjectId)
	{
		return _collisionObjects.GetValueOrDefault(networkObjectId);
	}

	private void OnCollisionTriggerEnter(NtfCollisionTriggerEnter message)
	{
		_collisionObjects.GetValueOrDefault(message.TargetNetworkObjectId)?.OnTriggerEnter(message.SourceClientId);
	}

	public SpecialField FindSpecialField(ulong networkObjectId)
	{
		return _specialFields.GetValueOrDefault(networkObjectId);
	}

	public void SpawnSpecialField(ulong? sourceNetworkObjectId, int specialFieldId, Vector3 position, Vector3 scale)
	{
		SpecialField specialField = _assetManager.InstantiateSpecialField(specialFieldId);
		TSpecialField tSpecialField = TSpecialField.Table[specialFieldId];
		if (tSpecialField.FixedYPos >= 0f)
		{
			position.y = tSpecialField.FixedYPos;
		}
		specialField.transform.position = position;
		specialField.transform.localScale = scale;
		specialField.Init(new SpecialField.Args
		{
			SpecialFieldId = specialFieldId,
			SourceNetworkObjectId = (sourceNetworkObjectId ?? ulong.MaxValue)
		});
		specialField.Spawn(destroyWithScene: true);
	}

	public void DespawnSpecialField(int specialFieldId)
	{
		SpecialField specialField = _specialFields.Values.FirstOrDefault((SpecialField f) => f.SpecialFieldId == specialFieldId);
		if (!(specialField == null))
		{
			specialField.Despawn(0f, destroy: true);
		}
	}

	public async UniTask<HitBox> SpawnHitBoxAsync(int hitBoxId, UnitModel ownerUnitModel, ActionStatsData ownerStats, HitLayerInfo ownerHitLayer, Vector3 deltaAimPos, Vector3 executionPosition, bool isFlipped = false, int skillId = -1)
	{
		_gameManager.AssertIfNotHost();
		HitBoxInfo hitBoxInfo = THitBox.Table[hitBoxId].HitBoxInfo;
		if (hitBoxInfo.moveSpeed > 0f && deltaAimPos != Vector3.zero && ownerUnitModel is CharacterModel)
		{
			deltaAimPos = CorrectAimDirection(deltaAimPos, executionPosition, ownerHitLayer, hitBoxInfo.duration * hitBoxInfo.moveSpeed, hitBoxInfo.collisionEvents);
		}
		deltaAimPos = Quaternion.Euler(0f, hitBoxInfo.angleOffset, 0f) * deltaAimPos;
		Quaternion qDirection = ((!hitBoxInfo.SpawnWithIdentityRotation) ? MathUtility.LookRotationOrIdentity(deltaAimPos.normalized) : Quaternion.identity);
		Vector3 startOffset = qDirection * hitBoxInfo.startOffset;
		bool hadOwner = ownerUnitModel != null;
		if (!string.IsNullOrEmpty(hitBoxInfo.FireVfx))
		{
			Vector3 vfxOffset = qDirection * new Vector3(0f, 0f, hitBoxInfo.VfxZOffset);
			_gameHost.SendToAll(new NtfDisplayFx
			{
				Vfx = string.Format("Skill/{0}", hitBoxInfo.FireVfx),
				VfxDuration = hitBoxInfo.FireVfxDuration,
				Sfx = string.Empty,
				Position = (startOffset + executionPosition + vfxOffset).ToProtoVector3()
			});
			await UniTask.Delay(TimeSpan.FromSeconds(hitBoxInfo.SpawnDelayAfterFireVfx));
		}
		return Spawn();
		HitBox Spawn()
		{
			if (hadOwner && ownerUnitModel == null)
			{
				return null;
			}
			GameObject gameObject = _assetManager.InstantiateHitBox(hitBoxInfo.collisionType);
			float x = startOffset.x + executionPosition.x;
			float z = startOffset.z + executionPosition.z;
			float y = startOffset.y + executionPosition.y;
			if (hitBoxInfo.FixedYPos >= 0f)
			{
				y = hitBoxInfo.FixedYPos;
			}
			if (hitBoxInfo.SnapToGroundOnSpawn && UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, y, z), out var hit, 2f, -1))
			{
				x = hit.position.x;
				y = hit.position.y;
				z = hit.position.z;
			}
			gameObject.transform.position = new Vector3(x, y, z);
			gameObject.transform.rotation = qDirection;
			gameObject.transform.localScale = hitBoxInfo.collisionScale;
			HitBox hitBox = gameObject.GetComponent<HitBox>();
			hitBox.Init(hitBoxId, ownerUnitModel, ownerStats, hitBoxInfo, ownerHitLayer, deltaAimPos, executionPosition, isFlipped, skillId, null);
			hitBox.Spawn(destroyWithScene: true);
			return hitBox;
		}
	}

	public async UniTaskVoid SpawnHitBoxByIterator(UnitController ownerUnit, ActionStatsData ownerStats, ITarget target, HitBoxIterator hitBoxIterator, HitLayerInfo ownerHitLayerInfo, Vector3 deltaAimPos, bool isFlipped = false, int skillId = -1, float skillTimeScale = 1f, ESpawnPositionType spawnPositionType = ESpawnPositionType.None, float targetPlayerMaxDistance = -1f, CancellationTokenSource cancellationTokenSource = null)
	{
		if (hitBoxIterator.TotalCount == 0)
		{
			return;
		}
		UnitModel ownerModel = ownerUnit.GetComponent<UnitModel>();
		List<Vector3> spawnPositions = ownerUnit.GetTargetPosition(spawnPositionType, target, deltaAimPos, targetPlayerMaxDistance);
		int angleGap = ((hitBoxIterator.TotalAngle != 0) ? (hitBoxIterator.TotalAngle / hitBoxIterator.TotalCount) : 0);
		float defaultOffset = ((hitBoxIterator.TotalAngle != 0) ? (-(hitBoxIterator.TotalAngle / 2)) : 0);
		int totalCount = ((hitBoxIterator.UseCustomOrder && hitBoxIterator.CustomOrderIdx != null) ? hitBoxIterator.CustomOrderIdx.Count : hitBoxIterator.TotalCount);
		if (deltaAimPos == Vector3.zero)
		{
			deltaAimPos = Vector3.forward;
		}
		for (int i = 0; i < totalCount; i++)
		{
			if (hitBoxIterator.UseCustomOrder && (hitBoxIterator.CustomOrderIdx == null || hitBoxIterator.CustomOrderIdx.Count <= i))
			{
				break;
			}
			int index = (hitBoxIterator.UseCustomOrder ? hitBoxIterator.CustomOrderIdx[i] : i);
			if (cancellationTokenSource != null && cancellationTokenSource.IsCancellationRequested)
			{
				break;
			}
			float startAngleOffset = RandomUtility.GetRandomFloat(hitBoxIterator.StartAngleOffsetMin, hitBoxIterator.StartAngleOffsetMax);
			float hitBoxDelay = hitBoxIterator.HitBoxDelay;
			float angle = 0f;
			Vector3 aimDirection = Vector3.zero;
			Vector3 positionOffset = Vector3.zero;
			if (hitBoxIterator.IteratorType == EHitBoxIteratorType.Arc)
			{
				angle = (defaultOffset + startAngleOffset + (float)((index + 1) * angleGap)) % 360f;
				aimDirection = Quaternion.Euler(0f, angle, 0f) * deltaAimPos;
				float startPositionOffset = RandomUtility.GetRandomFloat(hitBoxIterator.StartPositionOffsetMin, hitBoxIterator.StartPositionOffsetMax);
				positionOffset = startPositionOffset * (Quaternion.Euler(0f, angle, 0f) * deltaAimPos.normalized);
			}
			else if (hitBoxIterator.IteratorType == EHitBoxIteratorType.Custom)
			{
				if (hitBoxIterator.CustomData == null || index >= hitBoxIterator.CustomData.Length)
				{
					_logger.Error(string.Format("CustomData is null or index {0} is out of range.", index));
					continue;
				}
				HitBoxIteratorCustomData customData = hitBoxIterator.CustomData[index];
				angle = customData.CustomAngle;
				aimDirection = Quaternion.Euler(0f, customData.CustomAngle, 0f) * deltaAimPos;
				positionOffset = Quaternion.LookRotation(deltaAimPos.normalized, Vector3.up) * Quaternion.Euler(0f, customData.CustomAngle, 0f) * customData.CustomPositionOffset;
				hitBoxDelay += customData.CustomAdditionalDelay;
			}
			if (hitBoxIterator.UpdateAimEveryFrame)
			{
				Vector3 calculatedDeltaAimPos = ownerModel.CalculatedDeltaAimPos;
				if (calculatedDeltaAimPos != Vector3.zero)
				{
					deltaAimPos = calculatedDeltaAimPos;
				}
			}
			if (hitBoxIterator.UpdateBasePositionEveryFrame)
			{
				spawnPositions = ownerUnit.GetTargetPosition(spawnPositionType, target, deltaAimPos, targetPlayerMaxDistance);
			}
			if (hitBoxIterator.UpdateAimEveryFrame)
			{
				Vector3 calculatedDeltaAimPos2 = ownerUnit.GetComponent<UnitModel>().CalculatedDeltaAimPos;
				if (calculatedDeltaAimPos2 != Vector3.zero)
				{
					deltaAimPos = calculatedDeltaAimPos2;
				}
			}
			foreach (Vector3 spawnPosition in spawnPositions)
			{
				Vector3 position = spawnPosition + positionOffset;
				if (hitBoxIterator.IndicatorInfo != null && hitBoxIterator.IndicatorInfo.IndicatorVfxType != EIndicatorVfxType.None && ownerModel != null)
				{
					SpawnIndicatorVfx(hitBoxIterator.IndicatorInfo, ownerModel, position, angle, isFlipped, skillId, skillTimeScale);
				}
				if (hitBoxIterator.HitBoxId > 0)
				{
					SpawnHitBoxAsync(hitBoxIterator.HitBoxId, ownerModel, ownerStats, ownerHitLayerInfo, aimDirection, position, isFlipped, skillId).Forget();
				}
				if (hitBoxIterator.MonsterInfos != null && hitBoxIterator.MonsterInfos.Count > 0)
				{
					TrySpawnMonster(hitBoxIterator.MonsterInfos, position, ownerUnit.GetComponent<Unity.Netcode.NetworkObject>().NetworkObjectId);
				}
			}
			if (hitBoxDelay > 0f)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(skillTimeScale * hitBoxDelay));
			}
		}
	}

	private Vector3 CorrectAimDirection(Vector3 deltaAimPos, Vector3 executionPosition, HitLayerInfo ownerHitLayer, float maxRange, CollisionEvent[] collisionEvents)
	{
		float maxAngle = TConst.HitboxAimCorrectionAngle.FloatValue;
		if (maxAngle > 0f && collisionEvents != null && collisionEvents.Length != 0)
		{
			HashSet<EHitTarget> targetTypes = new HashSet<EHitTarget>();
			for (int i = 0; i < collisionEvents.Length; i++)
			{
				if (collisionEvents[i].targetType != EHitTarget.None)
				{
					targetTypes.Add(collisionEvents[i].targetType);
				}
			}
			if (targetTypes.Count != 0)
			{
				Vector3 aimDirection = new Vector3(deltaAimPos.x, 0f, deltaAimPos.z).normalized;
				if (aimDirection != Vector3.zero)
				{
					if (Physics.Raycast(executionPosition, aimDirection, out var hitInfo, maxRange, Const.LayerMask.UNIT))
					{
						UnitModel hitModel = hitInfo.collider.GetComponent<UnitModel>();
						if (hitModel != null && hitModel.IsHitable && IsValidAimTarget(hitModel, ownerHitLayer, targetTypes))
						{
							return deltaAimPos;
						}
					}
					UnitModel bestTarget = null;
					float bestDistance = float.MaxValue;
					foreach (UnitModel candidate in UnitModels)
					{
						if (candidate == null || !candidate.IsHitable || !IsValidAimTarget(candidate, ownerHitLayer, targetTypes))
						{
							continue;
						}
						Vector3 toCandidate = new Vector3(candidate.Position.x - executionPosition.x, 0f, candidate.Position.z - executionPosition.z);
						float distance = toCandidate.magnitude;
						if (distance > 0f && distance <= maxRange)
						{
							float angle = Vector3.Angle(aimDirection, toCandidate.normalized);
							float tolerance = (float)System.Math.Asin(candidate.HitRadius / distance) * 57.29578f;
							if (angle - tolerance <= maxAngle && distance < bestDistance)
							{
								bestTarget = candidate;
								bestDistance = distance;
							}
						}
					}
					if (bestTarget == null)
					{
						return deltaAimPos;
					}
					Vector3 toBest = new Vector3(bestTarget.Position.x - executionPosition.x, 0f, bestTarget.Position.z - executionPosition.z);
					return toBest.normalized * deltaAimPos.magnitude;
				}
			}
		}
		return deltaAimPos;
	}

	private static bool IsValidAimTarget(UnitModel unitModel, HitLayerInfo ownerHitLayer, HashSet<EHitTarget> targetTypes)
	{
		HitLayerInfo hitInfo = unitModel.GetHitLayerInfo();
		foreach (EHitTarget targetType in targetTypes)
		{
			if (targetType.IsHitable(ownerHitLayer, hitInfo, 0))
			{
				return true;
			}
		}
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
		NpcObject npcObject = _assetManager.InstantiateNpc(npcId);
		npcObject.transform.position = position;
		npcObject.transform.localScale = Vector3.one;
		npcObject.Init(new NpcObject.Args
		{
			ShowSpawnVfx = showSpawnVfx
		});
		npcObject.Spawn(destroyWithScene: true);
	}

	public void DespawnNpc(int npcId)
	{
		NpcObject npcObject = FindNpc(npcId);
		if (npcObject == null)
		{
			_logger.Warning(string.Format("Npc ID ({0}) not found.", npcId));
		}
		npcObject.NetworkObject.Despawn(destroy: true);
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
		if (!TExtraUnit.Table.TryGetValue(extraUnitId, out var tExtraUnit))
		{
			_logger.Error(string.Format("ExtraUnitId {0} not found in table.", extraUnitId));
			return false;
		}
		if (!UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out var hit, 2f, -1))
		{
			return false;
		}
		controller = _assetManager.InstantiateExtraUnit(tExtraUnit.Id);
		if (controller == null)
		{
			return false;
		}
		controller.transform.position = hit.position;
		controller.transform.rotation = Quaternion.identity;
		controller.Init(new ExtraUnitController.Args
		{
			ExtraUnitId = extraUnitId
		});
		controller.Spawn(destroyWithScene: true);
		return true;
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
		WaveExecutor waveExecutor = _assetManager.InstantiateWaveExecutor(waveExecutorId);
		waveExecutor.transform.position = spawnPosition;
		waveExecutor.transform.rotation = Quaternion.identity;
		waveExecutor.Init(new WaveExecutor.Args
		{
			WaveExecutorId = waveExecutorId,
			ExecuteImmediately = executeImmediately
		});
		waveExecutor.Spawn(destroyWithScene: true);
		return waveExecutor;
	}

	public InteractiveTrigger SpawnInteractiveTrigger(string prefabName, int triggerId, Vector3 position)
	{
		InteractiveTrigger interactiveTrigger = _assetManager.InstantiateInteractiveTrigger(prefabName);
		interactiveTrigger.transform.position = position;
		interactiveTrigger.transform.rotation = Quaternion.identity;
		interactiveTrigger.transform.localScale = Vector3.one;
		interactiveTrigger.Init(new InteractiveTrigger.Args
		{
			TriggerId = triggerId
		});
		interactiveTrigger.Spawn(destroyWithScene: true);
		return interactiveTrigger;
	}

	public void SpawnEnvironmentProp(string prefabName, Vector3 position, Vector3 forward)
	{
		EnvironmentProp environmentProp = _assetManager.InstantiateEnvironmentProp(prefabName);
		environmentProp.transform.position = position;
		environmentProp.transform.rotation = Quaternion.identity;
		environmentProp.transform.localScale = Vector3.one;
		environmentProp.Init(forward);
	}

	public void SpawnIndicatorVfx(IndicatorInfo indicatorInfo, UnitModel casterUnitModel, Vector3 position, float angleOffset = 0f, bool isFlipped = false, int skillId = -1, float skillTimeScale = 1f)
	{
		GameObject gameObject = _assetManager.InstantiateIndicatorVfx(indicatorInfo.IndicatorVfxType.ToString());
		IndicatorVfx indicatorVfx = gameObject.GetComponent<IndicatorVfx>();
		indicatorVfx.Init(indicatorInfo, casterUnitModel, position, angleOffset, isFlipped, skillId, skillTimeScale);
		indicatorVfx.Spawn(destroyWithScene: true);
	}

	public void SpawnPhoneBooth(Vector3 position)
	{
		PhoneBooth phoneBooth = _assetManager.InstantiatePhoneBooth();
		phoneBooth.transform.position = position;
		phoneBooth.transform.localScale = Vector3.one;
		phoneBooth.Spawn(destroyWithScene: true);
	}

	public void SpawnPingObject(EPingType pingType, Vector3 position)
	{
		GameObject gameObject = _assetManager.SpawnPing(pingType);
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.one;
		gameObject.DespawnOrDestroy(2f);
	}

	public Portal SpawnPortal(string prefabName, Vector3 spawnPosition, Vector3 exitPosition)
	{
		Portal portal = _assetManager.InstantiatePortal(prefabName);
		portal.transform.position = spawnPosition;
		portal.transform.rotation = Quaternion.identity;
		portal.Init(new Portal.Args
		{
			ExitPosition = exitPosition
		});
		portal.Spawn(destroyWithScene: true);
		return portal;
	}

	private bool IsInRange(IEnumerable<Vector3> sourcePositions, Vector3 targetPosition, float range)
	{
		return sourcePositions.Any((Vector3 p) => Vector3.Distance(p, targetPosition) < range);
	}

	public void SpawnPointerArrow(int id, Vector3 position)
	{
		GameObject gameObject = _assetManager.SpawnPointerArrow();
		gameObject.transform.position = position;
		gameObject.transform.localScale = Vector3.one;
		_pointerArrows.Add(id, gameObject);
	}

	public void DespawnPointerArrow(int id)
	{
		if (_pointerArrows.Remove(id, out var value))
		{
			value.DespawnOrDestroy();
		}
	}

	private async UniTask SpawnSectorsAsync()
	{
		GameObject mapRoot = new GameObject("Map");
		List<UniTask> tasks = new List<UniTask>();
		tasks.Add(SpawnPlayableSectorsAsync());
		if (_gameManager.IsInStage)
		{
			SpawnBoundarySpecialFields();
			tasks.Add(SpawnBoundarySectorsAsync());
		}
		SpawnSpecialFieldsFromSector();
		await UniTask.WhenAll(tasks);
		async UniTask SpawnPlayableSectorsAsync()
		{
			List<UniTask<GameObject>> instantiateTasks = new List<UniTask<GameObject>>();
			for (int i = 0; i < _mapManager.SectorIds.Length; i++)
			{
				int sectorId = _mapManager.SectorIds[i];
				if (sectorId >= 0)
				{
					Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
					instantiateTasks.Add(_assetManager.InstantiateSectorAsync(sectorId, mapRoot.transform, sectorOffset));
				}
			}
			GameObject[] sectorObjects = await UniTask.WhenAll(instantiateTasks);
			_spawnedSectors.Clear();
			for (int j = 0; j < _mapManager.SectorIds.Length; j++)
			{
				_spawnedSectors.Add((_mapManager.SectorIds[j], sectorObjects[j]));
			}
			if (_gameManager.IsHost)
			{
				Unity.AI.Navigation.NavMeshSurface navMeshSurface = mapRoot.GetOrAddComponent<Unity.AI.Navigation.NavMeshSurface>();
				navMeshSurface.useGeometry = UnityEngine.AI.NavMeshCollectGeometry.PhysicsColliders;
				navMeshSurface.layerMask = (LayerMask)((1 << Const.Layer.PLANE) | (1 << Const.Layer.HIGH_OBSTACLE) | (1 << Const.Layer.LOW_OBSTACLE));
				navMeshSurface.BuildNavMesh();
			}
		}
		void SpawnBoundarySpecialFields()
		{
			if (!_gameManager.IsHost)
			{
				return;
			}
			int boundarySpecialFieldId = TCampaign.Table[_gameContext.CampaignId].BoundarySpecialFieldId;
			if (boundarySpecialFieldId >= 1)
			{
				float height = (float)_mapManager.MapSize.Height * 30f;
				float width = (float)_mapManager.MapSize.Width * 30f;
				SpawnSpecialField(null, boundarySpecialFieldId, new Vector3(1f, 0f, height * 0.5f), new Vector3(2f, 1f, height));
				SpawnSpecialField(null, boundarySpecialFieldId, new Vector3(width - 1f, 1f, height * 0.5f), new Vector3(2f, 1f, height));
				SpawnSpecialField(null, boundarySpecialFieldId, new Vector3(width * 0.5f, 1f, height - 1f), new Vector3(width - 4f, 1f, 2f));
				SpawnSpecialField(null, boundarySpecialFieldId, new Vector3(width * 0.5f, 1f, 1f), new Vector3(width - 4f, 1f, 2f));
			}
		}
		void SpawnSpecialFieldsFromSector()
		{
			if (!_gameManager.IsHost)
			{
				return;
			}
			int[] sectorIds = _gameContext.SectorIds;
			for (int k = 0; k < sectorIds.Length; k++)
			{
				int sectorId2 = sectorIds[k];
				if (sectorId2 < 0)
				{
					continue;
				}
				SpecialFieldSpawnGroupData[] groupDatas = TSector.Table[sectorId2].SpecialFieldSpawnGroupDatas;
				if (groupDatas == null)
				{
					continue;
				}
				for (int l = 0; l < groupDatas.Length; l++)
				{
					SpecialFieldSpawnGroupData groupData = groupDatas[l];
					if (groupData == null || groupData.SpawnDatas == null)
					{
						continue;
					}
					for (int m = 0; m < groupData.SpawnDatas.Length; m++)
					{
						SpecialFieldSpawnData spawnData = groupData.SpawnDatas[m];
						SpawnSpecialField(null, spawnData.SpecialFieldId, spawnData.SpawnPosition, spawnData.Scale);
					}
				}
			}
		}
		async UniTask SpawnBoundarySectorsAsync()
		{
			EThemeType themeType = TCampaign.Table[_gameContext.CampaignId].ThemeType;
			int height2 = _mapManager.MapSize.Height;
			int width2 = _mapManager.MapSize.Width;
			List<UniTask> boundaryTasks = new List<UniTask>();
			int[] topSectorIds = _mapManager.PickStageSectorIds(themeType, ESectorAreaType.TopBoundary, height2 + 3 + width2);
			int idIndex = 0;
			for (int n = 0; n < height2 + 2; n++)
			{
				boundaryTasks.Add(SpawnBoundarySectorAsync(topSectorIds[idIndex], -1, n - 1));
				idIndex++;
			}
			for (int x = 0; x < width2 + 1; x++)
			{
				boundaryTasks.Add(SpawnBoundarySectorAsync(topSectorIds[idIndex], x, height2));
				idIndex++;
			}
			int[] bottomSectorIds = _mapManager.PickStageSectorIds(themeType, ESectorAreaType.BottomBoundary, height2 + 1 + width2);
			idIndex = 0;
			for (int z = 0; z < height2 + 1; z++)
			{
				boundaryTasks.Add(SpawnBoundarySectorAsync(bottomSectorIds[idIndex], width2, z - 1));
				idIndex++;
			}
			for (int x2 = 0; x2 < width2; x2++)
			{
				boundaryTasks.Add(SpawnBoundarySectorAsync(bottomSectorIds[idIndex], x2, -1));
				idIndex++;
			}
			await UniTask.WhenAll(boundaryTasks);
		}
		async UniTask SpawnBoundarySectorAsync(int sectorId, int indexX, int indexZ)
		{
			await _assetManager.InstantiateSectorAsync(sectorId, mapRoot.transform, new Vector3((float)indexX * 30f, 0f, (float)indexZ * 30f));
		}
	}

	private void InitAreaDecorationObjectSpawns()
	{
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			AreaDecorationObjectSpawnData[] spawnDatas = TSector.Table[sectorId].AreaDecorationObjectSpawnDatas;
			if (spawnDatas == null)
			{
				continue;
			}
			GameObject sectorObject = _spawnedSectors.FirstOrDefault(((int sectorId, GameObject go) s) => s.sectorId == sectorId).go;
			Transform parent = ((sectorObject != null) ? sectorObject.transform : null);
			foreach (AreaDecorationObjectSpawnData spawnData in spawnDatas)
			{
				GameObject gameObject = new GameObject(string.Format("AreaDecorationObject_{0}", spawnData.GroupId));
				if (parent != null)
				{
					gameObject.transform.SetParent(parent, worldPositionStays: false);
					gameObject.transform.localPosition = spawnData.SpawnPosition;
				}
				else
				{
					gameObject.transform.position = sectorOffset + spawnData.SpawnPosition;
				}
				gameObject.GetOrAddComponent<AreaDecorationObject>().Init(spawnData.GroupId, spawnData.PrefabNames, spawnData.IsFlipped);
			}
		}
	}

	private void InitNpcSpawns()
	{
		NpcSpawnDatas.Clear();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			NpcSpawnData[] spawnDatas = TSector.Table[sectorId].NpcSpawnDatas;
			if (spawnDatas == null)
			{
				continue;
			}
			foreach (NpcSpawnData spawnData in spawnDatas)
			{
				NpcSpawnData npcSpawnData = spawnData.Clone();
				npcSpawnData.SpawnPosition = sectorOffset + spawnData.SpawnPosition;
				NpcSpawnDatas.Add(npcSpawnData);
			}
		}
	}

	private void SpawnNpcs()
	{
		if (!NpcSpawnDatas.Any())
		{
			return;
		}
		NpcSpawnDatas.Where((NpcSpawnData d) => _playerManager.IsNpcUnlocked(d.NpcId)).ToList().ForEach(delegate(NpcSpawnData d)
		{
			SpawnNpc(d.NpcId, d.SpawnPosition, showSpawnVfx: false);
			NpcSpawnDatas.Remove(d);
		});
	}

	private void OnSpawnNpcsOnEventOccured(NpcUnlockEvent npcUnlockEvent)
	{
		if (!NpcSpawnDatas.Any())
		{
			return;
		}
		HashSet<int> npcIdHashSet = npcUnlockEvent.NpcIds.ToHashSet();
		NpcSpawnDatas.Where((NpcSpawnData d) => npcIdHashSet.Contains(d.NpcId)).ToList().ForEach(delegate(NpcSpawnData d)
		{
			SpawnNpc(d.NpcId, d.SpawnPosition, showSpawnVfx: true);
			NpcSpawnDatas.Remove(d);
		});
	}

	public void ForceSpawnNpcs()
	{
		if (!NpcSpawnDatas.Any())
		{
			return;
		}
		NpcSpawnDatas.ToList().ForEach(delegate(NpcSpawnData d)
		{
			SpawnNpc(d.NpcId, d.SpawnPosition, showSpawnVfx: false);
			NpcSpawnDatas.Remove(d);
		});
	}

	private void SpawnPhoneBooths()
	{
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			PhoneBoothSpawnData[] spawnDatas = TSector.Table[sectorId].PhoneBoothSpawnDatas;
			if (spawnDatas == null)
			{
				continue;
			}
			foreach (PhoneBoothSpawnData spawnData in spawnDatas)
			{
				SpawnPhoneBooth(sectorOffset + spawnData.SpawnPosition);
			}
		}
	}

	private void SpawnBaseMiniMapMarkers()
	{
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			TextMarkerSpawnData[] spawnDatas = TSector.Table[sectorId].TextMarkerSpawnDatas;
			if (spawnDatas == null)
			{
				continue;
			}
			foreach (TextMarkerSpawnData spawnData in spawnDatas)
			{
				_mapManager.RxTextMarkerDatas.Add(new TextMarkerData
				{
					TextKey = spawnData.TextKey,
					Position = sectorOffset + spawnData.SpawnPosition
				});
			}
		}
	}

	private void InitBreakablePropSpawns()
	{
		_breakablePropSpawnDatas = PickBreakablePropSpawnData();
	}

	private void ClearBreakablePropSpawns()
	{
		_breakablePropSpawnDatas = null;
	}

	private List<BreakablePropSpawnData> PickBreakablePropSpawnData()
	{
		List<BreakablePropSpawnData> result = new List<BreakablePropSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			BreakablePropSpawnGroupData[] groupDatas = TSector.Table[sectorId].BreakablePropSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (BreakablePropSpawnGroupData groupData in groupDatas)
			{
				List<BreakablePropSpawnData> list = groupData.SpawnDatas.Select((BreakablePropSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateBreakablePropSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_breakablePropSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_breakablePropSpawnDatas.Where((BreakablePropSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(BreakablePropSpawnData d)
			{
				SpawnBreakableProp(d.BreakablePropId, d.SpawnPosition, d.Forward);
				_breakablePropSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnBreakableProp(int breakablePropId, Vector3 position, Vector3 forward)
	{
		BreakableProp breakableProp = _assetManager.InstantiateBreakableProp(breakablePropId);
		breakableProp.transform.position = position;
		breakableProp.Init(new BreakableProp.Args
		{
			Forward = forward,
			BreakablePropId = breakablePropId
		});
		breakableProp.Spawn(destroyWithScene: true);
	}

	private void InitCollisionTriggerSpawns()
	{
		_collisionTriggerSpawnDatas = PickCollisionTriggerSpawnData();
	}

	private void ClearCollisionTriggerSpawns()
	{
		_collisionTriggerSpawnDatas = null;
	}

	private List<CollisionTriggerSpawnData> PickCollisionTriggerSpawnData()
	{
		List<CollisionTriggerSpawnData> result = new List<CollisionTriggerSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId >= 0 && TSector.Table.ContainsKey(sectorId))
			{
				Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
				CollisionTriggerSpawnData[] spawnDatas = TSector.Table[sectorId].CollisionTriggerSpawnDatas;
				if (spawnDatas != null)
				{
					result.AddRange(spawnDatas.Select((CollisionTriggerSpawnData d) => d.Clone(sectorOffset)));
				}
			}
		}
		return result;
	}

	private void UpdateCollisionTriggerSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions == null || characterPositions.Count == 0 || _collisionTriggerSpawnDatas == null || _collisionTriggerSpawnDatas.Count == 0)
		{
			return;
		}
		foreach (CollisionTriggerSpawnData item in _collisionTriggerSpawnDatas.Where((CollisionTriggerSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList())
		{
			SpawnCollisionTrigger(item);
			_collisionTriggerSpawnDatas.Remove(item);
		}
	}

	public void SpawnCollisionTrigger(CollisionTriggerSpawnData data)
	{
		CollisionTrigger collisionTrigger = _assetManager.InstantiateCollisionTrigger();
		collisionTrigger.transform.SetPositionAndRotation(data.SpawnPosition, Quaternion.identity);
		collisionTrigger.transform.localScale = data.Scale;
		collisionTrigger.Set(data.TriggerId, data.CoolTime, data.IsIndividual, data.CanTriggered);
		collisionTrigger.Spawn(destroyWithScene: true);
	}

	private void InitDisposableObjectSpawns()
	{
		_disposableObjectSpawnDatas = PickDisposableObjectSpawnData();
	}

	private void ClearDisposableObjectSpawns()
	{
		_disposableObjectSpawnDatas = null;
	}

	private List<DisposableObjectSpawnData> PickDisposableObjectSpawnData()
	{
		List<DisposableObjectSpawnData> result = new List<DisposableObjectSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			DisposableObjectSpawnGroupData[] groupDatas = TSector.Table[sectorId].DisposableObjectSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (DisposableObjectSpawnGroupData groupData in groupDatas)
			{
				List<DisposableObjectSpawnData> list = groupData.SpawnDatas.Select((DisposableObjectSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateDisposableObjectSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_disposableObjectSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_disposableObjectSpawnDatas.Where((DisposableObjectSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(DisposableObjectSpawnData d)
			{
				SpawnDisposableObject(d.DisposableObjectId, d.SpawnPosition, d.Forward);
				_disposableObjectSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnDisposableObject(int disposableObjectId, Vector3 position, Vector3 forward)
	{
		DisposableObject disposableObject = _assetManager.InstantiateDisposableObject(disposableObjectId);
		disposableObject.transform.position = position;
		disposableObject.Init(new DisposableObject.Args
		{
			Forward = forward,
			DisposableObjectId = disposableObjectId
		});
		disposableObject.Spawn(destroyWithScene: true);
	}

	private void InitDropRewardSpawns()
	{
		_dropRewardSpawnDatas = PickDropRewardSpawnData();
		_dropGoodsSpawnDatas = new List<DropGoodsSpawnData>();
		// The lounge loads with no campaign (CampaignId 0) and TCampaignTask has no group 0: the binary branches on
		// CampaignId > 0 (0x18074c563) and takes an empty list otherwise.
		if (_gameContext.CampaignId > 0)
		{
			_questItemIds = (from t in TCampaignTask.GroupTable[_gameContext.CampaignId]
				where t.QuestItemIds != null
				select t).SelectMany((TCampaignTask t) => t.QuestItemIds).ToList();
		}
		else
		{
			_questItemIds = new List<int>();
		}
	}

	private void ClearDropRewardSpawns()
	{
		_dropRewardSpawnDatas = null;
		_dropGoodsSpawnDatas = null;
		_questItemIds = null;
	}

	private List<DropRewardSpawnData> PickDropRewardSpawnData()
	{
		List<DropRewardSpawnData> result = new List<DropRewardSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			DropRewardSpawnGroupData[] groupDatas = TSector.Table[sectorId].DropRewardSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (DropRewardSpawnGroupData groupData in groupDatas)
			{
				List<DropRewardSpawnData> list = groupData.SpawnDatas.Select((DropRewardSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = GetBalanceSpawnCount(RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount), TDifficulty.Table[_gameContext.Difficulty].DropRewardSpawnPercent);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	public void SpawnDropGoods(Goods goods, Vector3 position)
	{
		Vector3 spawnPos = FindDropGoodsPosition(position, null);
		SpawnDropGoodsAt(goods, spawnPos);
	}

	public void SpawnDropGoods(Goods goods, Vector3 position, int count)
	{
		if (count < 2)
		{
			Vector3 spawnPos = FindDropGoodsPosition(position, null);
			SpawnDropGoodsAt(goods, spawnPos);
			return;
		}
		List<Vector3> reserved = new List<Vector3>(count);
		for (int i = 0; i < count; i++)
		{
			Vector3 spawnPos2 = FindDropGoodsPosition(position, reserved);
			reserved.Add(spawnPos2);
			DropGoods dropGoods = _assetManager.InstantiateDropGoods();
			dropGoods.transform.position = spawnPos2;
			dropGoods.transform.rotation = Quaternion.identity;
			dropGoods.Init(goods);
			dropGoods.Spawn(destroyWithScene: true);
		}
	}

	public void SpawnDropGoods(IEnumerable<Goods> goods, Vector3 position)
	{
		goods.ForEach(delegate(Goods g)
		{
			SpawnDropGoods(g, position);
		});
	}

	private Vector3 FindDropGoodsPosition(Vector3 position, IReadOnlyList<Vector3> reserved)
	{
		int areaMask = 1 << UnityEngine.AI.NavMesh.GetAreaFromName("Walkable");
		for (int grid = 1; grid <= 7; grid += 2)
		{
			int half = grid / 2;
			IEnumerable<(int, int)> ring = ((grid != 1) ? Enumerable.Range(0, grid * 4 - 4).Select(delegate(int i)
			{
				int x;
				int z;
				if (i < grid - 1)
				{
					x = i - half;
					z = half;
				}
				else if (i < grid * 2 - 2)
				{
					x = half;
					z = grid + (half - i) - 1;
				}
				else if (i < (grid - 1) * 3)
				{
					z = -half;
					x = half + grid * 2 - i - 2;
				}
				else
				{
					z = -grid * 3 - half + 3 + i;
					x = -half;
				}
				return (x, z);
			}) : new (int, int)[1] { (0, 0) });
			foreach (var (x2, z2) in ring)
			{
				if (grid >= 2 && System.Math.Abs(x2) < half && System.Math.Abs(z2) < half)
				{
					continue;
				}
				Vector3 candidate = new Vector3(position.x + (float)x2 * 0.85f, position.y + 0f, position.z + (float)z2 * 0.85f);
				if (!UnityEngine.AI.NavMesh.SamplePosition(candidate, out var hit, 0.1f, areaMask))
				{
					continue;
				}
				if (reserved != null && reserved.Any((Vector3 r) => (r - hit.position).sqrMagnitude < 0.180625f))
				{
					continue;
				}
				if (!Physics.OverlapSphere(hit.position, 0.1f).Any((Collider c) => c.GetComponent<DropGoods>() != null) && IsPathExist(position, hit.position, areaMask))
				{
					return hit.position;
				}
			}
		}
		if (!UnityEngine.AI.NavMesh.SamplePosition(position, out var fallbackHit, 0.85f, areaMask))
		{
			return position;
		}
		return fallbackHit.position;
	}

	private bool IsPathExist(Vector3 from, Vector3 to, int areaMask)
	{
		UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
		if (!UnityEngine.AI.NavMesh.CalculatePath(from, to, areaMask, navMeshPath))
		{
			return false;
		}
		if (navMeshPath.status != UnityEngine.AI.NavMeshPathStatus.PathComplete)
		{
			return false;
		}
		Vector3[] corners = navMeshPath.corners;
		float num = 0f;
		for (int i = 1; i < corners.Length; i++)
		{
			num += Vector3.Distance(corners[i - 1], corners[i]);
		}
		return num <= 6f;
	}

	private void SpawnDropGoodsAt(Goods goods, Vector3 spawnPos)
	{
		DropGoods dropGoods = _assetManager.InstantiateDropGoods();
		dropGoods.transform.position = spawnPos;
		dropGoods.transform.rotation = Quaternion.identity;
		dropGoods.Init(goods);
		dropGoods.Spawn(destroyWithScene: true);
	}

	private void UpdateDropRewardSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_dropRewardSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_dropRewardSpawnDatas.Where((DropRewardSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(DropRewardSpawnData d)
			{
				List<Goods> list = TDropReward.Table.PickRewards(d.DropRewardGroupId, _gameContext.CampaignId, _missionManager.MissionIds.ToArray(), _gameContext.Difficulty, _gameContext.GetPartyWeaponTypes());
				list.ForEach(delegate(Goods goods)
				{
					SpawnDropGoods(goods, d.SpawnPosition);
				});
				_dropRewardSpawnDatas.Remove(d);
			});
		}
	}

	private void UpdateDropItemSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
			DespawnFarFromCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_dropGoodsSpawnDatas != null && _dropGoodsSpawnDatas.Count > 0)
			{
				_dropGoodsSpawnDatas.Where((DropGoodsSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(DropGoodsSpawnData d)
				{
					SpawnDropGoods(d.Goods, d.SpawnPosition);
					_dropGoodsSpawnDatas.Remove(d);
				});
			}
		}
		void DespawnFarFromCharacters()
		{
			List<DropGoods> list = _dropGoods.Values.Where((DropGoods d) => !IsQuestItem(d.DisplayGoods)).Where((DropGoods d) => !IsInRange(characterPositions, d.Position, 12f)).ToList();
			if (_dropGoodsSpawnDatas == null)
			{
				return;
			}
			int num = 0;
			foreach (DropGoods item in list)
			{
				_dropGoodsSpawnDatas.Add(new DropGoodsSpawnData
				{
					Goods = item.OriginalGoods,
					SpawnPosition = item.Position
				});
				item.Despawn(0f, destroy: true);
				num++;
				if (num >= 5)
				{
					break;
				}
			}
		}
	}

	public bool IsQuestItem(Goods goods)
	{
		return _questItemIds != null && goods.IsItem() && _questItemIds.Contains(goods.Id);
	}

	private void InitEntranceSpawns()
	{
		_entranceSpawnDatas = PickEntranceSpawnData();
	}

	private void ClearEntranceSpawns()
	{
		_entranceSpawnDatas = null;
	}

	private List<EntranceSpawnData> PickEntranceSpawnData()
	{
		List<EntranceSpawnData> result = new List<EntranceSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			EntranceSpawnGroupData[] groupDatas = TSector.Table[sectorId].EntranceSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (EntranceSpawnGroupData groupData in groupDatas)
			{
				List<EntranceSpawnData> list = groupData.SpawnDatas.Select((EntranceSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateEntranceSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_entranceSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_entranceSpawnDatas.Where((EntranceSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 20f)).ToList().ForEach(delegate(EntranceSpawnData d)
			{
				SpawnEntrance(d.EntranceId, d.SpawnPosition, d.Forward);
				_entranceSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnEntrance(int entranceId, Vector3 position, Vector3 forward)
	{
		TEntrance tEntrance = TEntrance.Table[entranceId];
		Entrance entrance = _assetManager.InstantiateEntrance(tEntrance.PrefabName);
		entrance.transform.position = position;
		entrance.transform.rotation = Quaternion.identity;
		entrance.Init(new Entrance.Args
		{
			Forward = forward,
			EntranceId = entranceId
		});
		entrance.Spawn(destroyWithScene: true);
	}

	private void InitExitSpawns()
	{
		_exitSpawnDatas = PickExitSpawnData();
	}

	private List<ExitSpawnData> PickExitSpawnData()
	{
		List<ExitSpawnData> result = new List<ExitSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			ExitSpawnGroupData[] groupDatas = TSector.Table[sectorId].ExitSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (ExitSpawnGroupData groupData in groupDatas)
			{
				List<ExitSpawnData> list = groupData.SpawnDatas.Select((ExitSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void SpawnExitSpawn()
	{
		if (_exitSpawnDatas != null && _exitSpawnDatas.Count > 0)
		{
			_exitSpawnDatas.ForEach(delegate(ExitSpawnData d)
			{
				SpawnExit(d.ExitId, d.SpawnPosition);
			});
			_exitSpawnDatas = null;
		}
	}

	public void SpawnExit(int exitId, Vector3 position)
	{
		TExit tExit = TExit.Table[exitId];
		ExitObject exitObject = _assetManager.InstantiateExit(tExit.Type);
		exitObject.transform.position = position;
		exitObject.Init(new ExitObject.Args
		{
			ExitId = exitId
		});
		exitObject.Spawn(destroyWithScene: true);
	}

	public void RefreshCustomerNpcs()
	{
		_gameManager.AssertIfNotHost();
		DespawnCustomerNpcs();
		SpawnRandomCustomerNpcs();
		SpawnFixedCustomerNpcs();
		void DespawnCustomerNpcs()
		{
			_interactiveObjects.Values.OfType<NpcObject>().Where((NpcObject n) => n != null).Where((NpcObject n) => TNpc.Table[n.NpcId].Type == ENpcType.Customer).ToList()
				.ForEach(delegate(NpcObject n)
				{
					n.NetworkObject.Despawn(destroy: true);
				});
		}
		void SpawnFixedCustomerNpcs()
		{
			if (_gameContext.IsTutorial)
			{
				return;
			}
			for (int i = 0; i < _mapManager.SectorIds.Length; i++)
			{
				int sectorId = _mapManager.SectorIds[i];
				if (sectorId >= 0)
				{
					Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
					foreach (GuestSpawnData g in TSector.Table[sectorId].GuestSpawnDatas.Where((GuestSpawnData g) => g.Id > 0).ToList())
					{
						SpawnNpc(g.Id, sectorOffset + g.SpawnPosition, showSpawnVfx: false);
					}
				}
			}
		}
		void SpawnRandomCustomerNpcs()
		{
			if (_gameContext.IsTutorial || !_playerManager.IsFeatureUnlocked(EFeatureUnlock.ArtifactDeal) || _gameContext.CampaignId <= 0)
			{
				return;
			}
			for (int i = 0; i < _mapManager.SectorIds.Length; i++)
			{
				int sectorId = _mapManager.SectorIds[i];
				if (sectorId < 0)
				{
					continue;
				}
				Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
				float baseChance = TConst.CustomerSpawnPercentBase.FloatValue;
				float perUserChance = TConst.CustomerSpawnPercentUserCount.FloatValue;
				int userCount = _gameContext.RxPlayers.Count;
				int maxCount = TConst.CustomerMaxCount.IntValue;
				GamePlayer me = _gameContext.Players[Unity.Netcode.NetworkManager.Singleton.LocalClientId];
				int limit = ((me.GuestCount > 0) ? (maxCount + me.GuestCount) : maxCount);
				List<GuestSpawnData> points = TSector.Table[sectorId].GuestSpawnDatas.Where((GuestSpawnData g) => g.Id < 1).OrderBy((GuestSpawnData g) => RandomUtility.GetRandomInt(int.MaxValue)).Take(limit).ToList();
				List<TNpc> npcs = TNpc.Table.Values.Where((TNpc n) => n.Type == ENpcType.Customer).OrderBy((TNpc n) => RandomUtility.GetRandomInt(int.MaxValue)).Take(limit).ToList();
				for (int j = 0; j < limit && points.Count > 0 && npcs.Count > 0; j++)
				{
					if (RandomUtility.GetRandomFloat(0f, 100f) <= (float)userCount * perUserChance + baseChance)
					{
						GuestSpawnData spawnPoint = points.First();
						TNpc npc = npcs.First();
						points.RemoveAt(0);
						npcs.RemoveAt(0);
						SpawnNpc(npc.Id, sectorOffset + spawnPoint.SpawnPosition, showSpawnVfx: false);
					}
				}
			}
		}
	}

	private void InitLockedAreaObjectSpawns()
	{
		_lockedAreaObjects.Clear();
		if (!_gameContext.IsInLounge)
		{
			return;
		}
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			LockedAreaObjectSpawnData[] spawnDatas = TSector.Table[sectorId].LockedAreaObjectSpawnDatas;
			if (spawnDatas == null)
			{
				continue;
			}
			foreach (LockedAreaObjectSpawnData spawnData in spawnDatas)
			{
				TArea tArea = TArea.Table[spawnData.AreaId];
				if (_gameContext.AreaUnlockedFlags == null || !_gameContext.AreaUnlockedFlags.IsSet(tArea.BitFlagIndex))
				{
					SpawnLockedAreaObject(spawnData.Clone(sectorOffset), spawnData.AreaId);
				}
			}
		}
	}

	private void SpawnLockedAreaObject(LockedAreaObjectSpawnData spawnData, int areaUnlockId)
	{
		LockedAreaObject lockedAreaObject = _assetManager.InstantiateLockedAreaObject(spawnData.PrefabName);
		lockedAreaObject.transform.position = spawnData.SpawnPosition;
		lockedAreaObject.Spawn(destroyWithScene: true);
		if (!_lockedAreaObjects.TryGetValue(areaUnlockId, out var value))
		{
			value = new List<LockedAreaObject>();
			_lockedAreaObjects[areaUnlockId] = value;
		}
		value.Add(lockedAreaObject);
	}

	public void DespawnLockedAreaObjects(int areaUnlockId)
	{
		_lockedAreaObjects.TryGetValue(areaUnlockId, out var value);
		if (value != null)
		{
			value.ForEach(delegate(LockedAreaObject o)
			{
				o.Despawn(0f, destroy: true);
			});
			_lockedAreaObjects.Remove(areaUnlockId);
		}
	}

	private void InitMimicSpawns()
	{
		_mimicSpawnDatas = PickMimicSpawnData();
	}

	private void ClearMimicSpawns()
	{
		_mimicSpawnDatas = null;
	}

	private List<MimicSpawnData> PickMimicSpawnData()
	{
		List<MimicSpawnData> result = new List<MimicSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			MimicSpawnGroupData[] groupDatas = TSector.Table[sectorId].MimicSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (MimicSpawnGroupData groupData in groupDatas)
			{
				List<MimicSpawnData> list = groupData.SpawnDatas.Select((MimicSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = GetBalanceSpawnCount(RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount), TDifficulty.Table[_gameContext.Difficulty].MimicSpawnPercent);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateMimicSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_mimicSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_mimicSpawnDatas.Where((MimicSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(MimicSpawnData d)
			{
				SpawnMimic(d.MimicSpawnId, d.SpawnPosition, d.Forward);
				_mimicSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnMimic(int mimicSpawnId, Vector3 position, Vector3 forward)
	{
		TMimicSpawn tMimicSpawn = TMimicSpawn.Table[mimicSpawnId];
		float num = RandomUtility.GetRandomFloat(0f, 100f);
		if (tMimicSpawn.MimicPercent < num)
		{
			if (tMimicSpawn.RewardBoxId > 0 && num <= tMimicSpawn.RewardBoxPercent + tMimicSpawn.MimicPercent)
			{
				SpawnRewardBox(tMimicSpawn.RewardBoxId, position, forward);
			}
			return;
		}
		MimicObject mimicObject = _assetManager.InstantiateMimic(TMimic.Table[tMimicSpawn.MimicId].PrefabName);
		mimicObject.transform.position = position;
		mimicObject.transform.rotation = Quaternion.identity;
		mimicObject.Init(new MimicObject.Args
		{
			MimicId = tMimicSpawn.MimicId,
			Forward = forward
		});
		mimicObject.Spawn(destroyWithScene: true);
	}

	private void InitMonsterSpawns()
	{
		_monsterSpawnDatas = new List<MonsterSpawnData>();
		_monsterSpawnDatas.AddRange(PickSectorMonsterSpawnDatas());
	}

	private void InitPatrolMonsterSpawn()
	{
		if (!_gameContext.IsInLounge)
		{
			_monsterSpawnDatas.AddRange(PickPatrolMonsterSpawnDatas());
		}
	}

	private void ClearMonsterSpawns()
	{
		_monsterSpawnDatas = null;
	}

	private List<MonsterSpawnData> PickSectorMonsterSpawnDatas()
	{
		List<MonsterSpawnData> result = new List<MonsterSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			MonsterSpawnGroupData[] groupDatas = TSector.Table[sectorId].MonsterSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (MonsterSpawnGroupData groupData in groupDatas)
			{
				List<MonsterSpawnData> list = groupData.SpawnDatas.Select((MonsterSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = GetBalanceSpawnCount(RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount), TDifficulty.Table[_gameContext.Difficulty].MonsterSpawnPercent);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private List<MonsterSpawnData> PickPatrolMonsterSpawnDatas()
	{
		TCampaign campaignData = TCampaign.Table[_gameContext.CampaignId];
		if (campaignData.PatrolMonsterCount < 1)
		{
			return new List<MonsterSpawnData>();
		}
		List<(Vector3, Vector3, Vector3)> patrolPoints = new List<(Vector3, Vector3, Vector3)>();
		(int Width, int Height) mapSize = _mapManager.MapSize;
		if (mapSize.Width >= 2 && mapSize.Height >= 3)
		{
			for (int c = 1; c < mapSize.Width; c++)
			{
				Vector3 start = _mapManager.GetSectorOffset(c, 1);
				Vector3 end = _mapManager.GetSectorOffset(c, mapSize.Height - 1);
				Vector3 point;
				if (System.Math.Abs(_characterSpawnPoint.x - start.x) < 20f)
				{
					point = ((!(Vector3.Distance(_characterSpawnPoint, start) <= Vector3.Distance(_characterSpawnPoint, end))) ? start : end);
				}
				else
				{
					point = new Vector3(start.x, 0f, RandomUtility.GetRandomFloat(start.z, end.z));
				}
				patrolPoints.Add((point, start, end));
			}
		}
		if (mapSize.Width >= 3 && mapSize.Height >= 2)
		{
			for (int r = 1; r < mapSize.Height; r++)
			{
				Vector3 start2 = _mapManager.GetSectorOffset(1, r);
				Vector3 end2 = _mapManager.GetSectorOffset(mapSize.Width - 1, r);
				Vector3 point2;
				if (System.Math.Abs(_characterSpawnPoint.z - start2.z) < 20f)
				{
					point2 = ((!(Vector3.Distance(_characterSpawnPoint, start2) <= Vector3.Distance(_characterSpawnPoint, end2))) ? start2 : end2);
				}
				else
				{
					point2 = new Vector3(RandomUtility.GetRandomFloat(start2.x, end2.x), 0f, start2.z);
				}
				patrolPoints.Add((point2, start2, end2));
			}
		}
		return patrolPoints.Shuffle().Take(campaignData.PatrolMonsterCount).Select(((Vector3, Vector3, Vector3) p) => new MonsterSpawnData
		{
			MonsterId = campaignData.PatrolMonsterId,
			SpawnType = ESpawnType.QuestPatrol,
			SpawnPosition = p.Item1,
			IsPatrolMonster = true,
			PatrolWanderPositions = new Vector3[2] { p.Item2, p.Item3 }
		}).ToList();
	}

	public void AddMonsterSpawnData(MonsterSpawnData spawnData)
	{
		_monsterSpawnDatas.Add(spawnData);
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
		List<Vector3> characterPositions = characters.Select((CharacterModel c) => c.transform.position).ToList();
		SpawnNearByCharacters();
		DespawnFarFromCharacters();
		void SpawnNearByCharacters()
		{
			if (_monsterSpawnDatas == null || _monsterSpawnDatas.Count < 1 || !characterPositions.Any())
			{
				return;
			}
			HashSet<MonsterSpawnData> spawnTargets = new HashSet<MonsterSpawnData>();
			foreach (MonsterSpawnData spawnData in _monsterSpawnDatas)
			{
				Vector3 spawnPosition = spawnData.SpawnPosition;
				if (spawnData.SpawnType != ESpawnType.QuestPatrol)
				{
					if (characterPositions.All((Vector3 p) => Vector3.Distance(p, spawnPosition) > TConst.MonsterSpawnRangeMax.FloatValue))
					{
						continue;
					}
					if (spawnData.IsRespawn)
					{
						if (characterPositions.Any((Vector3 p) => Vector3.Distance(p, spawnPosition) < TConst.MonsterSpawnRangeMin.FloatValue))
						{
							continue;
						}
						if (_gameManager.TimeNow < spawnData.SpawnableTime)
						{
							continue;
						}
					}
				}
				spawnTargets.Add(spawnData);
			}
			List<MonsterSpawnData> spawned = new List<MonsterSpawnData>();
			foreach (MonsterSpawnData spawnData2 in spawnTargets)
			{
				Vector3 spawnPosition2 = spawnData2.SpawnPosition;
				bool isLightOn = false;
				if (spawnData2.UseMonsterSpawnTable)
				{
					isLightOn = characters.MinBy((CharacterModel c) => Vector3.Distance(c.transform.position, spawnPosition2)).IsLightOn;
				}
				if (TrySpawnMonster(spawnData2, isLightOn))
				{
					spawned.Add(spawnData2);
				}
			}
			spawned.ForEach(delegate(MonsterSpawnData d)
			{
				_monsterSpawnDatas.Remove(d);
			});
		}
		void DespawnFarFromCharacters()
		{
			float despawnRange = TConst.MonsterSpawnRangeMax.FloatValue + 5f;
			if (_monsterSpawnDatas == null)
			{
				return;
			}
			List<MonsterController> list = _unitControllers.Values.Select((UnitController u) => u as MonsterController).Where((MonsterController c) => c != null).Where((MonsterController c) => !c.HasLifeTime)
				.Where((MonsterController c) => c.GetComponent<MonsterModel>() != null && !c.GetComponent<MonsterModel>().IsInactiveInStage)
				.Where((MonsterController c) => c.SpawnType == ESpawnType.None)
				.Where((MonsterController c) => c.GetComponent<MonsterModel>() != null && (c.GetComponent<MonsterModel>().BasePhaseMonsterId < 1 || c.GetComponent<MonsterModel>().BasePhaseMonsterId == c.GetComponent<MonsterModel>().TableId))
				.Where((MonsterController c) => !IsInRange(characterPositions, c.transform.position, despawnRange))
				.Take(5)
				.ToList();
			foreach (MonsterController item in list)
			{
				MonsterModel monsterModel = item.GetComponent<MonsterModel>();
				if (!(monsterModel.Summoner != null))
				{
					_monsterSpawnDatas.Add(new MonsterSpawnData
					{
						MonsterId = (monsterModel.IsPhaseMonster ? monsterModel.BasePhaseMonsterId : item.MonsterId),
						MonsterSpawnId = item.MonsterSpawnId,
						SpawnPosition = item.transform.position,
						IsRespawn = false,
						SpawnType = item.SpawnType,
						UseFixedAimPos = item.UseFixedAimPos,
						FixedAimPos = item.FixedAimPos
					});
				}
				else if (monsterModel.Summoner.TryGetComponent<MonsterController>(out var component))
				{
					component.RemoveSummonMonsterModel(monsterModel);
				}
				item.Despawn(0f, destroy: true);
			}
		}
	}

	public void TrySpawnMonster(List<SummonMonsterInfo> summonMonsterInfos, Vector3 position, ulong summonerNetworkObjectId)
	{
		if (!UnityEngine.AI.NavMesh.SamplePosition(position, out var hit, 5f, 1))
		{
			return;
		}
		SummonMonsterInfo randomInfo = summonMonsterInfos.GetRandomInfo();
		if (randomInfo != null)
		{
			MonsterController monsterController = FindMonsterController(summonerNetworkObjectId);
			MonsterModel monsterModel = ((monsterController == null) ? null : monsterController.GetComponent<MonsterModel>());
			MonsterSpawnData spawnData = new MonsterSpawnData
			{
				MonsterId = randomInfo.Id,
				SpawnPosition = hit.position,
				IsRespawn = false,
				SpawnType = ESpawnType.SummonActionEvent,
				LifeTime = randomInfo.LifeTime,
				SummonerNetworkObjectId = summonerNetworkObjectId,
				DieOnSummonerDeath = randomInfo.DieOnSummonerDeath,
				TargetNetworkObjectId = ((monsterModel == null) ? 0 : monsterModel.RxTargetNetworkObjectId.Value)
			};
			TrySpawnMonster(spawnData);
		}
	}

	public bool TrySpawnMonster(MonsterSpawnData spawnData, bool isLightOn = false, int[] additionalBuffIds = null)
	{
		MonsterController summonerController = FindMonsterController(spawnData.SummonerNetworkObjectId);
		if (summonerController != null && !summonerController.CanSummonMonster())
		{
			return false;
		}
		int monsterId = 0;
		int monsterSpawnId = spawnData.MonsterSpawnId;
		int[] buffIds = null;
		if (!spawnData.UseMonsterSpawnTable)
		{
			if (monsterSpawnId > 0)
			{
				TMonsterSpawn tMonsterSpawn = TMonsterSpawn.Table[monsterSpawnId];
				monsterId = tMonsterSpawn.MonsterId;
				buffIds = tMonsterSpawn.BuffIds;
			}
			else
			{
				monsterId = spawnData.MonsterId;
			}
		}
		else
		{
			TMonsterSpawn tMonsterSpawn2 = TMonsterSpawn.GroupTable[spawnData.MonsterSpawnGroupId].Pick(isLightOn);
			if (tMonsterSpawn2 == null)
			{
				return false;
			}
			monsterId = tMonsterSpawn2.MonsterId;
			monsterSpawnId = tMonsterSpawn2.Id;
			buffIds = tMonsterSpawn2.BuffIds;
		}
		TMonster tMonster = TMonster.Table[monsterId];
		Vector3 spawnPoint = GetSpawnablePosition(spawnData.SpawnPosition);
		if (!_gameManager.CharacterModels.All((CharacterModel c) => IsBlockingSpawn(c)))
		{
			SpawnMonster();
			return true;
		}
		_gameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = "Spawn/" + tMonster.SpawnVfx,
			VfxDuration = 3f,
			Sfx = tMonster.SpawnSfx,
			Position = (spawnPoint + new Vector3(0f, tMonster.SpawnVfxOffsetY, 0f)).ToProtoVector3()
		});
		UniTask.Delay(TimeSpan.FromSeconds(tMonster.SpawnDelay)).ContinueWith(SpawnMonster).Forget();
		return true;
		bool IsBlockingSpawn(CharacterModel c)
		{
			if (c == null)
			{
				return false;
			}
			if (c.State == EActionState.Dead || c.State == EActionState.Escaped)
			{
				return true;
			}
			return Vector3.Distance(c.transform.position, spawnPoint) < TConst.MonsterSpawnRangeMin.FloatValue;
		}
		Vector3 GetSpawnablePosition(Vector3 position)
		{
			if (UnityEngine.AI.NavMesh.SamplePosition(position, out var hit, 2f, 1))
			{
				return hit.position;
			}
			_logger.Warning(string.Format("Monster Spawn Position Invalid! MonsterId:{0}, Position:{1}", monsterId, position));
			return position;
		}
		void SpawnMonster()
		{
			MonsterController monsterController = _assetManager.InstantiateMonster(monsterId);
			if (monsterController == null)
			{
				_logger.Error(string.Format("Failed to instantiate monster. MonsterId:{0}", monsterId));
			}
			monsterController.transform.position = spawnPoint;
			monsterController.transform.rotation = Quaternion.identity;
			monsterController.Init(new MonsterController.Args
			{
				MonsterId = monsterId,
				MonsterSpawnId = monsterSpawnId,
				SpawnType = spawnData.SpawnType,
				Forward = ((spawnData.Forward == Vector3.zero) ? Vector3.back : spawnData.Forward),
				LifeTime = spawnData.LifeTime,
				HitLayer = _hitLayerIndex++ + 2001,
				TargetNetworkObjectId = spawnData.TargetNetworkObjectId,
				SummonerNetworkObjectId = spawnData.SummonerNetworkObjectId,
				DieOnSummonerDeath = spawnData.DieOnSummonerDeath,
				WaveStartPosition = spawnData.WaveStartPosition,
				UseFixedAimPos = spawnData.UseFixedAimPos,
				FixedAimPos = spawnData.FixedAimPos,
				IsPatrolMonster = spawnData.IsPatrolMonster,
				PatrolWanderPositions = spawnData.PatrolWanderPositions
			});
			monsterController.Spawn(destroyWithScene: true);
			MonsterModel component = monsterController.GetComponent<MonsterModel>();
			if (buffIds != null)
			{
				int[] array = buffIds;
				foreach (int buffId in array)
				{
					component.AddBuff(buffId, component, 1, 0uL, default(Vector3));
				}
			}
			if (additionalBuffIds != null)
			{
				int[] array2 = additionalBuffIds;
				foreach (int buffId2 in array2)
				{
					component.AddBuff(buffId2, component, 1, 0uL, default(Vector3));
				}
			}
			if (summonerController != null)
			{
				summonerController.AddSummonMonsterModel(component);
			}
		}
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
		_portalSpawnDatas = PickPortalSpawnData();
		_portalExitDatas = PickPortalExitDatas();
	}

	private List<PortalSpawnData> PickPortalSpawnData()
	{
		List<PortalSpawnData> result = new List<PortalSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			PortalSpawnGroupData[] groupDatas = TSector.Table[sectorId].PortalSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (PortalSpawnGroupData groupData in groupDatas)
			{
				List<PortalSpawnData> list = groupData.SpawnDatas.Select((PortalSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private Dictionary<int, PortalExitData> PickPortalExitDatas()
	{
		Dictionary<int, PortalExitData> spawnDatas = new Dictionary<int, PortalExitData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			PortalExitData[] exitDatas = TSector.Table[sectorId].PortalExitDatas;
			if (exitDatas != null)
			{
				exitDatas.ForEach(delegate(PortalExitData d)
				{
					spawnDatas.Add(d.Id, d.Clone(sectorOffset));
				});
			}
		}
		return spawnDatas;
	}

	private void SpawnPortals()
	{
		if (_portalSpawnDatas == null || _portalSpawnDatas.Count < 1 || _portalExitDatas == null)
		{
			return;
		}
		foreach (PortalSpawnData portalSpawnData in _portalSpawnDatas)
		{
			if (!_portalExitDatas.TryGetValue(portalSpawnData.PortalExitId, out var value))
			{
				_logger.Error(string.Format("Portal exit not found for PortalExitId {0}.", portalSpawnData.PortalExitId));
			}
			else
			{
				SpawnPortal(portalSpawnData.PrefabName, portalSpawnData.SpawnPosition, value.Position);
			}
		}
		_portalSpawnDatas = null;
		_portalExitDatas = null;
	}

	private void InitRewardBoxSpawns()
	{
		_rewardBoxSpawnDatas = PickRewardBoxSpawnData();
		_questRewardBoxIds = (from t in TCampaignTask.GroupTable[_gameContext.CampaignId]
			where t.QuestRewardBoxIds != null
			select t).SelectMany((TCampaignTask t) => t.QuestRewardBoxIds).ToList();
	}

	private void ClearRewardBoxSpawns()
	{
		_rewardBoxSpawnDatas = null;
		_questRewardBoxIds = null;
	}

	private List<RewardBoxSpawnData> PickRewardBoxSpawnData()
	{
		List<RewardBoxSpawnData> result = new List<RewardBoxSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			RewardBoxSpawnGroupData[] groupDatas = TSector.Table[sectorId].RewardBoxSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (RewardBoxSpawnGroupData groupData in groupDatas)
			{
				List<RewardBoxSpawnData> list = groupData.SpawnDatas.Select((RewardBoxSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = GetBalanceSpawnCount(RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount), TDifficulty.Table[_gameContext.Difficulty].RewardBoxSpawnPercent);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateRewardBoxesSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
			DespawnFarFromCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_rewardBoxSpawnDatas != null && _rewardBoxSpawnDatas.Count > 0)
			{
				_rewardBoxSpawnDatas.Where((RewardBoxSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(RewardBoxSpawnData d)
				{
					int rewardBoxId = ((d.RewardBoxId < 1) ? PickRewardBoxId(d.RewardBoxGroupId) : d.RewardBoxId);
					SpawnRewardBox(rewardBoxId, d.SpawnPosition, d.Forward, d.SlotCount, d.RespawnItemSlots, d.LockState, d.IsReplace);
					_rewardBoxSpawnDatas.Remove(d);
				});
			}
		}
		void DespawnFarFromCharacters()
		{
			if (_rewardBoxSpawnDatas == null)
			{
				return;
			}
			List<RewardBox> list = _rewardBoxes.Values.Where((RewardBox r) => !IsQuestRewardBox(r.RewardBoxId)).Where((RewardBox r) => !IsInRange(characterPositions, r.Position, 12f)).ToList();
			int num = 0;
			foreach (RewardBox item in list)
			{
				_rewardBoxSpawnDatas.Add(new RewardBoxSpawnData
				{
					RewardBoxId = item.RewardBoxId,
					SlotCount = item.GetSlotCount(),
					RespawnItemSlots = item.GetSlots(),
					LockState = item.LockState,
					SpawnPosition = item.Position,
					Forward = item.Forward,
					IsReplace = false
				});
				item.Despawn(0f, destroy: true);
				num++;
				if (num >= 5)
				{
					break;
				}
			}
		}
		int PickRewardBoxId(int groupId)
		{
			TRewardBox tRewardBox = TRewardBox.GroupTable[groupId].Pick();
			if (tRewardBox == null)
			{
				_logger.Error(string.Format("RewardBoxSpawnGroupId ({0}): reward box not found", groupId));
				return 0;
			}
			return tRewardBox.Id;
		}
	}

	public void SpawnRewardBox(int rewardBoxId, Vector3 position, Vector3 forward, int respawnItemSlotCount = -1, IEnumerable<GoodsSlot> respawnItemSlots = null, ELockState lockState = ELockState.None, bool isReplace = false)
	{
		RewardBox rewardBox = _assetManager.InstantiateRewardBox(TRewardBox.Table[rewardBoxId].PrefabName);
		rewardBox.transform.position = position;
		rewardBox.transform.rotation = Quaternion.identity;
		rewardBox.Init(new RewardBox.Args
		{
			Forward = forward,
			SlotCount = respawnItemSlotCount,
			RewardBoxId = rewardBoxId,
			RespawnItemSlots = respawnItemSlots,
			LockState = lockState,
			IsReplace = isReplace
		});
		rewardBox.Spawn(destroyWithScene: true);
	}

	public bool IsQuestRewardBox(int rewardBoxId)
	{
		return _questRewardBoxIds != null && _questRewardBoxIds.Count > 0 && _questRewardBoxIds.Contains(rewardBoxId);
	}

	private void OnSpawnSequentialTimeline(NtfSpawnSequentialTimeline message)
	{
		Vector3 position = ((message.Position != null) ? message.Position.ToVector3() : Vector3.zero);
		GameObject gameObject = _assetManager.InstantiateSequentialTimeline(message.PrefabName);
		if (!(gameObject == null))
		{
			gameObject.transform.position = position;
		}
	}

	private void InitSkillExecutorSpawns()
	{
		_skillExecutorSpawnDatas = PickSkillExecutorSpawnData();
	}

	private void ClearSkillExecutorSpawns()
	{
		_skillExecutorSpawnDatas = null;
	}

	private List<SkillExecutorSpawnData> PickSkillExecutorSpawnData()
	{
		List<SkillExecutorSpawnData> result = new List<SkillExecutorSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			SkillExecutorSpawnGroupData[] groupDatas = TSector.Table[sectorId].SkillExecutorSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (SkillExecutorSpawnGroupData groupData in groupDatas)
			{
				List<SkillExecutorSpawnData> list = groupData.SpawnDatas.Select((SkillExecutorSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateSkillExecutorSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_skillExecutorSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_skillExecutorSpawnDatas.Where((SkillExecutorSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(SkillExecutorSpawnData d)
			{
				SpawnSkillExecutor(d.SkillExecutorId, d.SpawnPosition);
				_skillExecutorSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnSkillExecutor(int skillExecutorId, Vector3 position)
	{
		SkillExecutor skillExecutor = _assetManager.InstantiateSkillExecutor(skillExecutorId);
		skillExecutor.transform.position = position;
		skillExecutor.transform.rotation = Quaternion.identity;
		skillExecutor.Init(new SkillExecutor.Args
		{
			SkillExecutorId = skillExecutorId
		});
		skillExecutor.Spawn(destroyWithScene: true);
	}

	private void InitTrapSpawns()
	{
		_trapSpawnDatas = PickTrapSpawnData();
	}

	private void ClearTrapSpawns()
	{
		_trapSpawnDatas = null;
	}

	private List<TrapSpawnData> PickTrapSpawnData()
	{
		List<TrapSpawnData> result = new List<TrapSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			TrapSpawnGroupData[] groupDatas = TSector.Table[sectorId].TrapSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (TrapSpawnGroupData groupData in groupDatas)
			{
				List<TrapSpawnData> list = groupData.SpawnDatas.Select((TrapSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					float spawnCount = groupData.SpawnCount;
					if (TAnomaly.Table[_gameContext.AnomalyType].EffectTypes != null && TAnomaly.Table[_gameContext.AnomalyType].EffectTypes.Contains(EAnomalyEffectType.TrapSpawnChange))
					{
						spawnCount = System.Math.Min(TConst.AnomalyTrapSpawnMultiplier.FloatValue * spawnCount, (float)list.Count);
					}
					int count = RandomUtility.GetRandomIntFromFloat(spawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void UpdateTrapSpawns(IList<Vector3> characterPositions)
	{
		if (characterPositions != null && characterPositions.Count > 0)
		{
			SpawnNearByCharacters();
		}
		void SpawnNearByCharacters()
		{
			if (_trapSpawnDatas == null || characterPositions == null || characterPositions.Count < 1)
			{
				return;
			}
			_trapSpawnDatas.Where((TrapSpawnData d) => IsInRange(characterPositions, d.SpawnPosition, 10f)).ToList().ForEach(delegate(TrapSpawnData d)
			{
				SpawnTrap(d.TrapId, d.SpawnPosition, d.Forward);
				_trapSpawnDatas.Remove(d);
			});
		}
	}

	public void SpawnTrap(int trapId, Vector3 position, Vector3 forward)
	{
		TrapObject trapObject = _assetManager.InstantiateTrap(trapId);
		trapObject.transform.position = position;
		trapObject.Init(new TrapObject.Args
		{
			Forward = forward,
			TrapId = trapId
		});
		trapObject.Spawn(destroyWithScene: true);
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
		if (_spawnedEmployeeNpcs.ContainsKey(employeeId))
		{
			return;
		}
		int npcId = TEmployee.Table[employeeId].NpcId;
		if (npcId == 0)
		{
			return;
		}
		NpcObject npcObject = FindNpcByFunction(ENpcFunctionType.TycoonSalesSlot, slotIndex);
		if (npcObject == null)
		{
			return;
		}
		Vector3 position = npcObject.transform.position + EmployeeNpcOffset;
		NpcObject npcObject2 = _assetManager.InstantiateNpc(npcId);
		npcObject2.transform.position = position;
		npcObject2.transform.localScale = Vector3.one;
		npcObject2.Init(new NpcObject.Args
		{
			ShowSpawnVfx = true
		});
		npcObject2.DisableInteraction();
		if (npcObject2 is EmployeeNpcObject employeeNpcObject)
		{
			employeeNpcObject.SetSalesSlotIndex(slotIndex);
		}
		npcObject2.Spawn(destroyWithScene: true);
		if (!(npcObject2 == null))
		{
			npcObject2.SetEmployeeId(employeeId);
			_spawnedEmployeeNpcs[employeeId] = (npcObject2, slotIndex);
		}
	}

	private NpcObject SpawnEmployeeNpcInternal(int npcId, Vector3 position, int slotIndex)
	{
		NpcObject npcObject = _assetManager.InstantiateNpc(npcId);
		npcObject.transform.position = position;
		npcObject.transform.localScale = Vector3.one;
		npcObject.Init(new NpcObject.Args
		{
			ShowSpawnVfx = true
		});
		npcObject.DisableInteraction();
		if (npcObject is EmployeeNpcObject employeeNpcObject)
		{
			employeeNpcObject.SetSalesSlotIndex(slotIndex);
		}
		npcObject.Spawn(destroyWithScene: true);
		return npcObject;
	}

	public void DespawnEmployeeNpc(int employeeId)
	{
		if (_spawnedEmployeeNpcs.TryGetValue(employeeId, out var value))
		{
			_spawnedEmployeeNpcs.Remove(employeeId);
			if (value.Npc.IsSpawned)
			{
				value.Npc.Despawn(0f, destroy: true);
			}
		}
	}

	private void DespawnAllEmployeeNpcs()
	{
		foreach (int item in _spawnedEmployeeNpcs.Keys.ToList())
		{
			DespawnEmployeeNpc(item);
		}
	}

	public void SpawnTycoonCustomerNpc(int slotIndex, int customerNpcId)
	{
		if (customerNpcId == 0 || _spawnedTycoonCustomerNpcs.ContainsKey(slotIndex))
		{
			return;
		}
		NpcObject npcObject = FindNpcByFunction(ENpcFunctionType.TycoonSalesSlot, slotIndex);
		if (npcObject == null)
		{
			return;
		}
		Vector3 position = npcObject.transform.position + CustomerNpcOffset;
		NpcObject npcObject2 = _assetManager.InstantiateNpc(customerNpcId);
		npcObject2.transform.position = position;
		npcObject2.transform.localScale = Vector3.one;
		npcObject2.Init(new NpcObject.Args
		{
			ShowSpawnVfx = true
		});
		npcObject2.DisableInteraction();
		npcObject2.Spawn(destroyWithScene: true);
		if (npcObject2 != null)
		{
			_spawnedTycoonCustomerNpcs[slotIndex] = npcObject2;
		}
	}

	public void DespawnTycoonCustomerNpc(int slotIndex)
	{
		if (_spawnedTycoonCustomerNpcs.TryGetValue(slotIndex, out var value))
		{
			_spawnedTycoonCustomerNpcs.Remove(slotIndex);
			if (value.IsSpawned)
			{
				value.Despawn(0f, destroy: true);
			}
		}
	}

	public void RefreshSlotNpcs(int slotIndex)
	{
		if (!_gameManager.IsHost)
		{
			return;
		}
		KeyValuePair<int, (NpcObject Npc, int SlotIndex)> keyValuePair = _spawnedEmployeeNpcs.FirstOrDefault((KeyValuePair<int, (NpcObject Npc, int SlotIndex)> kv) => kv.Value.SlotIndex == slotIndex);
		if (keyValuePair.Value.Npc != null)
		{
			DespawnEmployeeNpc(keyValuePair.Key);
		}
		DespawnTycoonCustomerNpc(slotIndex);
		if (_gameManager.IsInLounge && _playerManager.SalesSlots.TryGetValue(slotIndex, out var value) && value.IsStarted)
		{
			if (value.EmployeeId != 0)
			{
				SpawnEmployeeNpc(value.EmployeeId, slotIndex);
			}
			if (value.TycoonCustomerNpcId != 0)
			{
				SpawnTycoonCustomerNpc(slotIndex, value.TycoonCustomerNpcId);
			}
		}
	}

	public void RefreshTycoonNpcs()
	{
		if (!_gameManager.IsHost)
		{
			return;
		}
		DespawnAllTycoonNpcs();
		if (!_gameManager.IsInLounge)
		{
			return;
		}
		foreach (SalesSlot value in _playerManager.SalesSlots.Values)
		{
			if (value.IsStarted)
			{
				if (value.EmployeeId != 0)
				{
					SpawnEmployeeNpc(value.EmployeeId, value.Id);
				}
				if (value.TycoonCustomerNpcId != 0)
				{
					SpawnTycoonCustomerNpc(value.Id, value.TycoonCustomerNpcId);
				}
			}
		}
	}

	public void DespawnAllTycoonNpcs()
	{
		DespawnAllEmployeeNpcs();
		foreach (int item in _spawnedTycoonCustomerNpcs.Keys.ToList())
		{
			DespawnTycoonCustomerNpc(item);
		}
	}

	private void OnPlayNpcAnimation(NtfPlayNpcAnimation message)
	{
		if (_interactiveObjects.TryGetValue(message.NetworkObjectId, out var value) && value is NpcObject npcObject)
		{
			npcObject.PlayAnimation(message.AnimationName, message.IsLoop);
		}
	}

	private NpcObject SpawnTycoonNpc(int npcId, Vector3 position)
	{
		NpcObject npcObject = _assetManager.InstantiateNpc(npcId);
		npcObject.transform.position = position;
		npcObject.transform.localScale = Vector3.one;
		npcObject.Init(new NpcObject.Args
		{
			ShowSpawnVfx = true
		});
		npcObject.DisableInteraction();
		npcObject.Spawn(destroyWithScene: true);
		return npcObject;
	}

	private void InitWaveExecutorSpawns()
	{
		_waveExecutorSpawnDatas = PickWaveExecutorSpawnData();
	}

	private List<WaveExecutorSpawnData> PickWaveExecutorSpawnData()
	{
		List<WaveExecutorSpawnData> result = new List<WaveExecutorSpawnData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId < 0)
			{
				continue;
			}
			Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
			WaveExecutorSpawnGroupData[] groupDatas = TSector.Table[sectorId].WaveExecutorSpawnGroupDatas;
			if (groupDatas == null)
			{
				continue;
			}
			foreach (WaveExecutorSpawnGroupData groupData in groupDatas)
			{
				List<WaveExecutorSpawnData> list = groupData.SpawnDatas.Select((WaveExecutorSpawnData d) => d.Clone(sectorOffset)).ToList();
				if (!groupData.SpawnAll)
				{
					int count = RandomUtility.GetRandomIntFromFloat(groupData.SpawnCount);
					list = list.Shuffle().Take(count).ToList();
				}
				result.AddRange(list);
			}
		}
		return result;
	}

	private void SpawnWaveExecutors()
	{
		if (_waveExecutorSpawnDatas != null && _waveExecutorSpawnDatas.Count > 0)
		{
			_waveExecutorSpawnDatas.ForEach(delegate(WaveExecutorSpawnData d)
			{
				SpawnWaveExecutor(d.WaveExecutorId, d.SpawnPosition, executeImmediately: false);
			});
			_waveExecutorSpawnDatas = null;
		}
	}

	private void InitZoneObjectSpawns()
	{
		_gameManager.AssertIfNotHost();
		IList<ZoneData> zoneDatas = GetMapZoneDatas();
		List<ZoneData> assignedZoneDatas = new List<ZoneData>();
		AddAssignedCharacterSpawnPoint();
		if (_gameContext.IsInStage)
		{
			AddAssignedQuestZoneObjects();
			AddAssignedMissionZoneObjects();
		}
		if (CheckDuplicateZoneSpawnIds(assignedZoneDatas))
		{
			return;
		}
		RemoveBaseObjects(assignedZoneDatas);
		SpawnZoneObjects(assignedZoneDatas);
		_logger.Debug("\n" + string.Join("\n", assignedZoneDatas.Select((ZoneData z) => string.Format("ZoneSpawnId: {0}, Position: {1}, ZoneFlags: {2}, SpawnType: {3}", z.ZoneSpawnId, z.Position.ToVector3(), z.ZoneFlags, z.SpawnType))) + "\n", new Color(0f, 1f, 1f, 1f));
		_gameHost.SendToAll(new NtfAssignedZones
		{
			ZoneDatas = assignedZoneDatas.ToArray()
		});
		void AddAssignedCharacterSpawnPoint()
		{
			ZoneData zoneData = zoneDatas.FirstOrDefault((ZoneData z) => (z.ZoneFlags & EZoneFlags.PlayerSpawn) != 0);
			if (zoneData == null)
			{
				zoneData = zoneDatas.First((ZoneData z) => (z.ZoneFlags & EZoneFlags.Outside) != 0);
			}
			_characterSpawnPoint = zoneData.Position.ToVector3();
			zoneDatas.Remove(zoneData);
			zoneData.ZoneSpawnId = -1;
			zoneData.ExcludedSpawnRange = TConst.CharacterSpawnSafeRange.FloatValue;
			assignedZoneDatas.Add(zoneData);
		}
		void AddAssignedQuestZoneObjects()
		{
			int[] zoneSpawnIds = TCampaign.Table[_gameContext.CampaignId]?.ZoneSpawnIds;
			if (zoneSpawnIds == null)
			{
				return;
			}
			foreach (int zoneSpawnId in zoneSpawnIds)
			{
				ZoneData zoneData = AssignZone(zoneSpawnId, null, ESpawnType.Quest);
				if (zoneData != null)
				{
					assignedZoneDatas.Add(zoneData);
				}
			}
		}
		void AddAssignedMissionZoneObjects()
		{
			if (_missionManager == null || !_missionManager.HasMission)
			{
				return;
			}
			_missionManager.MissionIds.Select((int id) => TMission.Table[id]).ForEach(delegate(TMission mission)
			{
				if (mission.ZoneSpawnIds == null)
				{
					return;
				}
				foreach (int zoneSpawnId in mission.ZoneSpawnIds)
				{
					ZoneData zoneData = AssignZone(zoneSpawnId, mission.ThemeTypes, ESpawnType.Mission);
					if (zoneData != null)
					{
						assignedZoneDatas.Add(zoneData);
					}
				}
			});
		}
		ZoneData AssignZone(int zoneSpawnId, EThemeType[] themeTypes, ESpawnType spawnType)
		{
			TZoneSpawn zoneSpawnData = TZoneSpawn.Table[zoneSpawnId];
			List<(Vector3, float, float)> checkRanges = new List<(Vector3, float, float)>();
			foreach (ZoneData assigned in assignedZoneDatas)
			{
				ZoneSpawnDistanceData distanceData = zoneSpawnData.SpawnDistanceDatas.FirstOrDefault((ZoneSpawnDistanceData d) => d.ZoneSpawnId == assigned.ZoneSpawnId);
				if (distanceData != null)
				{
					checkRanges.Add((assigned.Position.ToVector3(), distanceData.Min, distanceData.Max));
				}
			}
			ZoneData zone;
			if (checkRanges.Any())
			{
				zone = zoneDatas.Where((ZoneData z) => checkRanges.All(((Vector3, float, float) v) =>
				{
					float distance = Vector3.Distance(z.Position.ToVector3(), v.Item1);
					return distance > v.Item2 && distance < v.Item3;
				})).FirstOrDefault(IsValidZone);
				if (zone == null)
				{
					_logger.Warning(string.Format("No valid zone found for ZoneSpawnId {0} with distance constraints.", zoneSpawnId));
					zone = zoneDatas.FirstOrDefault(IsValidZone);
					if (zone == null)
					{
						_logger.Error(string.Format("ZoneData not found. (ZoneSpawnId:{0})", zoneSpawnId));
						return null;
					}
				}
			}
			else
			{
				zone = zoneDatas.FirstOrDefault(IsValidZone);
				if (zone == null)
				{
					_logger.Error(string.Format("ZoneData not found. (ZoneSpawnId:{0})", zoneSpawnId));
					return null;
				}
			}
			zone.ZoneSpawnId = zoneSpawnId;
			zone.SpawnType = spawnType;
			zoneDatas.Remove(zone);
			return zone;
			bool IsValidZone(ZoneData z)
			{
				if ((zoneSpawnData.RequiredZoneFlags & z.ZoneFlags) != zoneSpawnData.RequiredZoneFlags || (z.ZoneFlags & zoneSpawnData.ExcludedZoneFlags) != 0)
				{
					return false;
				}
				if (themeTypes == null || themeTypes.Length == 0)
				{
					return true;
				}
				return themeTypes.Any((EThemeType t) => t == _gameContext.ThemeType);
			}
		}
	}

	private IList<ZoneData> GetMapZoneDatas()
	{
		List<ZoneData> zoneDatas = new List<ZoneData>();
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int sectorId = _mapManager.SectorIds[i];
			if (sectorId >= 0)
			{
				Vector3 sectorOffset = _mapManager.GetSectorOffset(i);
				zoneDatas.AddRange(TSector.Table[sectorId].ZonePointDatas.Select((ZonePointData p) => new ZoneData
				{
					ZoneFlags = p.ZoneFlags,
					Position = (sectorOffset + p.Position).ToProtoVector3()
				}));
			}
		}
		return zoneDatas.Shuffle();
	}

	private bool CheckDuplicateZoneSpawnIds(IList<ZoneData> zoneDatas)
	{
		List<int> duplicateIds = (from z in zoneDatas
			group z by z.ZoneSpawnId into g
			where g.Count() > 1
			select g.Key).ToList();
		if (!duplicateIds.Any())
		{
			return false;
		}
		_logger.Error("Duplicate ZoneSpawnId detected: " + string.Join(", ", duplicateIds));
		return true;
	}

	private void RemoveBaseObjects(IList<ZoneData> zoneDatas)
	{
		if (zoneDatas.Any())
		{
			zoneDatas.Where((ZoneData z) => z.ExcludedSpawnRange > 0f).ForEach(delegate(ZoneData z)
			{
				RemoveMonsterInRange(z.Position.ToVector3(), z.ExcludedSpawnRange);
			});
		}
		void RemoveMonsterInRange(Vector3 position, float range)
		{
			_monsterSpawnDatas.Where((MonsterSpawnData m) => m.SpawnType == ESpawnType.None).Where((MonsterSpawnData m) => Vector3.Distance(m.SpawnPosition, position) <= range).ToList()
				.ForEach(delegate(MonsterSpawnData m)
				{
					_monsterSpawnDatas.Remove(m);
				});
		}
	}

	private void SpawnZoneObjects(IList<ZoneData> zoneDatas)
	{
		_gameManager.AssertIfNotHost();
		if (zoneDatas.Any())
		{
			zoneDatas.Where((ZoneData z) => 0 < z.ZoneSpawnId).ForEach(SpawnZoneObject);
		}
		void SpawnZoneObject(ZoneData zoneData)
		{
			ZoneObjectSpawnData[] spawnDatas = TZoneSpawn.Table[zoneData.ZoneSpawnId].SpawnDatas;
			if (spawnDatas == null || spawnDatas.Length == 0)
			{
				_logger.Error(string.Format("ZoneSpawnId {0} has no spawn data.", zoneData.ZoneSpawnId));
				return;
			}
			spawnDatas.ForEach(delegate(ZoneObjectSpawnData spawnData)
			{
				Vector3 position = zoneData.Position.ToVector3() + spawnData.Offset;
				if (spawnData is ZoneInteractiveTriggerSpawnData zoneInteractiveTriggerSpawnData)
				{
					SpawnInteractiveTrigger(zoneInteractiveTriggerSpawnData.PrefabName, zoneInteractiveTriggerSpawnData.TriggerId, position);
				}
				else if (spawnData is ZoneMonsterSpawnData zoneMonsterSpawnData)
				{
					AddMonsterSpawnData(new MonsterSpawnData
					{
						UseMonsterSpawnTable = zoneMonsterSpawnData.UseSpawnGroup,
						MonsterId = (zoneMonsterSpawnData.UseSpawnGroup ? 0 : zoneMonsterSpawnData.MonsterId),
						MonsterSpawnGroupId = (zoneMonsterSpawnData.UseSpawnGroup ? zoneMonsterSpawnData.MonsterSpawnGroupId : 0),
						SpawnPosition = position,
						IsRespawn = false,
						SpawnType = zoneData.SpawnType
					});
				}
				else if (spawnData is ZoneNpcSpawnData zoneNpcSpawnData)
				{
					SpawnNpc(zoneNpcSpawnData.NpcId, position, showSpawnVfx: false);
				}
				else if (spawnData is ZoneRewardBoxSpawnData zoneRewardBoxSpawnData)
				{
					SpawnRewardBox(zoneRewardBoxSpawnData.RewardBoxId, position, zoneRewardBoxSpawnData.Forward);
				}
				else if (spawnData is ZoneSkillExecutorSpawnData zoneSkillExecutorSpawnData)
				{
					SpawnSkillExecutor(zoneSkillExecutorSpawnData.SkillExecutorId, position);
				}
				else if (spawnData is ZoneWaveExecutorSpawnData zoneWaveExecutorSpawnData)
				{
					SpawnWaveExecutor(zoneWaveExecutorSpawnData.WaveExecutorId, position, executeImmediately: false);
				}
				else if (spawnData is ZoneExitSpawnData zoneExitSpawnData)
				{
					SpawnExit(zoneExitSpawnData.ExitId, position);
				}
			});
		}
	}

	private void SpawnLocalZoneObjectSpawns()
	{
		if (_zoneDatas.Any())
		{
			_zoneDatas.Values.Where((ZoneData z) => 0 < z.ZoneSpawnId).ForEach(SpawnLocalZoneObject);
		}
		void SpawnLocalZoneObject(ZoneData zoneData)
		{
			ZoneObjectSpawnData[] spawnDatas = TZoneSpawn.Table[zoneData.ZoneSpawnId].SpawnDatas;
			if (spawnDatas == null || spawnDatas.Length == 0)
			{
				_logger.Error(string.Format("ZoneSpawnId {0} has no spawn data.", zoneData.ZoneSpawnId));
				return;
			}
			spawnDatas.ForEach(delegate(ZoneObjectSpawnData spawnData)
			{
				Vector3 vector = zoneData.Position.ToVector3() + spawnData.Offset;
				if (spawnData is ZoneQuestMarkerSpawnData zoneQuestMarkerSpawnData)
				{
					_mapManager.SpawnQuestMarker(new CampaignMarkerData
					{
						TaskId = zoneQuestMarkerSpawnData.CampaignTaskId,
						Position = vector
					});
				}
				else if (spawnData is ZoneCustomMarkerSpawnData zoneCustomMarkerSpawnData)
				{
					_mapManager.SpawnCustomMarker(new CustomMarkerData
					{
						CustomId = zoneCustomMarkerSpawnData.CustomId,
						MarkerName = zoneCustomMarkerSpawnData.MarkerName,
						Position = vector
					});
				}
				else if (spawnData is EnvironmentPropSpawnData environmentPropSpawnData)
				{
					SpawnEnvironmentProp(environmentPropSpawnData.Prefab, vector, environmentPropSpawnData.Forward);
				}
			});
		}
	}

	public Vector3 GetZoneSpawnPosition(int zoneSpawnId)
	{
		if (_zoneDatas.TryGetValue(zoneSpawnId, out var value))
		{
			return value.Position.ToVector3();
		}
		_logger.Error(string.Format("ZoneSpawnId {0} not found in assigned zones.", zoneSpawnId));
		return Vector3.zero;
	}

	private void OnAssignedZones(NtfAssignedZones message)
	{
		_zoneDatas.Clear();
		if (message.ZoneDatas != null && message.ZoneDatas.Length != 0)
		{
			message.ZoneDatas.ForEach(delegate(ZoneData z)
			{
				_zoneDatas.Add(z.ZoneSpawnId, z);
			});
		}
	}
}
