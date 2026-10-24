using System;
using System.Collections.Generic;
using System.Linq;
using De.InGame.Fsm;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Popup;
using Sirenix.Utilities;
using SomeNet.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public class CharacterController : UnitController
{
	public class Args
	{
		public ulong PlayerClientId;

		public int HitLayer;
	}

	private Args _args;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameCutsceneManager _gameCutsceneManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameShopManager _shopManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameFloatingTextManager _floatingTextManager;

	private CharacterStateMachine _characterStateMachine;

	private OutlineObject _outlineObject;

	private CompositeDisposable _stressStateDisposable = new CompositeDisposable();

	private float _stressUpdateRemainTime;

	private float _corruptionStressUpdateRemainTime;

	private EStressState _currentStressDebuffState;

	private float _stressDebuffStartTime;

	private float _nextStressDebuffUpdateTime;

	private int _shadowSpawnRemainCount;

	private float _shadowSpawnRemainTime;

	private float _shadowSpawnRemainDelayTime;

	private bool _stressBuffToastDisplayed;

	private float _brightnessElapsedUpdateTime;

	private float _lastStaminaDecreasedTime;

	[Inject]
	private GameObjectManager _gameObjectManager;

	[Inject]
	private GameMissionManager _missionManager;

	private const int PointMarkerLimit = 9;

	private float _lastHpDamageTime;

	private int _accumulatedComboDamage;

	private float _recoveryOnCriticalCoolEndTime;

	protected override UnitStateMachine StateMachine => _characterStateMachine;

	private CharacterView CharacterView => UnitView as CharacterView;

	private CharacterModel CharacterModel => UnitModel as CharacterModel;

	private PlayerInventory PlayerInventory => Inventory as PlayerInventory;

	private GamePlayer Player
	{
		get
		{
			if (_gameContext != null && _gameContext.Players.TryGetValue(ClientId, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public override ulong ClientId => CharacterModel.SyncClientId.Value;

	private bool IsMine => ClientId == base.NetworkManager.LocalClientId;

	private int CharacterId => CharacterModel.CharacterId;

	private OutlineObject OutlineObject
	{
		get
		{
			if (_outlineObject == null)
			{
				_outlineObject = GetComponentInChildren<OutlineObject>();
			}
			return _outlineObject;
		}
	}

	private float TimeNow => GameManager.TimeNow;

	public bool IsGamePlaying => _gameManager.StageState == EStageState.Playing;

	public Subject<int> OnShowEmoji { get; } = new Subject<int>();

	public Subject<string> OnShowSelfReviveText { get; } = new Subject<string>();

	public void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	public override void OnHostSpawn()
	{
		AssertIfNotHost();
		CharacterModel.SyncClientId.Value = _args.PlayerClientId;
		CharacterModel.SyncHitLayer.Value = _args.HitLayer;
	}

	protected override void Start()
	{
		base.Start();
		float num = Mathf.Max(0f, (CharacterModel.StressIntervalPercent + 100f) / 100f);
		_stressUpdateRemainTime = RandomUtility.GetRandomFloat(TConst.StressIntervalTimeMin.FloatValue, TConst.StressIntervalTimeMax.FloatValue) * num;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_stressStateDisposable?.Dispose();
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		PlayerInventory.OnInventoryChanged.Subscribe(OnInventoryChanged).AddTo(this);
		PlayerInventory.OnSafeInventoryChanged.Subscribe(OnSafeInventoryChanged).AddTo(this);
		CharacterModel.RxState.Where((EActionState _) => _ == EActionState.Dead).Subscribe(delegate
		{
			OnDead();
		}).AddTo(this);
		CharacterModel.RxState.Where((EActionState _) => _ == EActionState.Revive).Subscribe(delegate
		{
			OnRevive();
		}).AddTo(this);
		if (IsMine)
		{
			_playerManager.OnStashChanged.Subscribe(delegate
			{
				OnStashChanged();
			}).AddTo(this);
		}
		if (!_gameContext.Players.TryGetValue(ClientId, out var _))
		{
			_logger.Error($"Player with clientId {ClientId} not found.");
			return;
		}
		InitPlayer(_gameContext.Players[ClientId]);
	}

	private void InitPlayer(GamePlayer player)
	{
		CharacterModel.Init(player);
		CharacterView.Init(player);
		if (base.IsHost && GameContext.IsInStage)
		{
			TAnomaly.Table[GameContext.AnomalyType].PlayerBuffIds?.ForEach(delegate(int id)
			{
				CharacterModel.AddBuff(id, CharacterModel, 1, 0uL, Vector3.zero);
			});
		}
		base.name = string.Format("[Character] {0}:{1}", CharacterId, player.Name);
		CharacterModel.RxState.Pairwise().Subscribe(delegate(Pair<EActionState> p)
		{
			OnStateChanged(p.Current, p.Previous);
		}).AddTo(this);
		OnStateChanged(CharacterModel.State, EActionState.None);
		if (IsMine && CharacterModel.State == EActionState.Dying)
		{
			_cameraManager.DeactivateUiCamera();
		}
		if (base.IsHost)
		{
			_characterStateMachine = new CharacterStateMachine(this);
			UnitModel.PassiveSkillIds?.ForEach(delegate(int _)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = _
				});
			});
			CharacterModel.GetEquipmentPassiveSkillIds().ForEach(delegate(int _)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = _
				});
			});
			CharacterModel.OnEquipmentPassiveSkillAdded.Subscribe(delegate(int _)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = _
				});
			}).AddTo(this);
			List<GoodsSlot> slots = player.InventorySlots.Values.Select((GoodsSlot _) => _.Clone()).ToList();
			List<GoodsSlot> subSlots = player.SafeInventorySlots.Values.Select((GoodsSlot _) => _.Clone()).ToList();
			Inventory.Init(slots, subSlots);
			ApplyParadox();
			_gameContext.Players[ClientId].RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> _)
			{
				OnCharacterUpdated(_.Previous, _.Current);
			}).AddTo(this);
			CharacterModel.RxState.Where((EActionState _) => _ == EActionState.StageReady).Subscribe(delegate
			{
				GameManager.OnReadyStage();
			}).AddTo(this);
			CharacterModel.RxStressState.Subscribe(OnStressStateChanged).AddTo(this);
			CharacterModel.RxStress.Subscribe(OnStressChanged).AddTo(this);
			CharacterModel.RxCorruptionState.Pairwise().Subscribe(delegate(Pair<ECorruptionState> pair)
			{
				OnCorruptionStateChanged(pair.Previous, pair.Current);
			}).AddTo(this);
			CharacterModel.OnInventoryInitialized.Subscribe(delegate
			{
				UpdateCorruption();
			}).AddTo(this);
			PlayerInventory.OnInventoryChanged.Subscribe(OnInventoryChangedForCorruption).AddTo(this);
			PlayerInventory.OnSafeInventoryChanged.Subscribe(OnSafeInventoryChangedForCorruption).AddTo(this);
			CharacterModel.RxIsLightOn.Subscribe(OnLightOnChanged).AddTo(this);
			CharacterModel.RxLightFuel.Subscribe(OnLightFuelChanged).AddTo(this);
			CharacterModel.RxStamina.Pairwise().Subscribe(delegate(Pair<float> _)
			{
				OnStaminaChanged(_.Previous, _.Current);
			}).AddTo(this);
			UnitModel.RxBuffs.ObserveChange().Subscribe(delegate(int _)
			{
				OnBuffUpdate();
			}).AddTo(this);
			CharacterModel.RxTryRun.Subscribe(delegate(bool _)
			{
				OnTryRunChanged();
			}).AddTo(this);
			CharacterModel.RxIsMovingBackward.Subscribe(delegate(bool _)
			{
				OnIsMovingBackwardChanged();
			}).AddTo(this);
			CharacterModel.RxState.Pairwise().Where((Pair<EActionState> p) => p.Current == EActionState.Skill || p.Previous == EActionState.Skill).Subscribe(delegate
			{
				CharacterModel.UpdateStat(EStatType.MoveSpeed);
			}).AddTo(this);
		}
		GameManager.OnCharacterSpawned.OnNext(this);
		player.RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> _)
		{
			OnCharacterModified(_.Previous, _.Current);
		}).AddTo(this);
		player.RxTalent.Pairwise().Subscribe(delegate(Pair<Talent> _)
		{
			OnTalentChanged(_.Previous, _.Current);
		}).AddTo(this);
	}

	public override void OnNetworkDespawn()
	{
		_gameManager.OnCharacterDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (_gameManager.StageState != EStageState.Playing && _gameManager.StageState != EStageState.Ended)
		{
			return;
		}
		UpdateFogOfWar();
		if (base.IsHost)
		{
			UpdateLightFuel();
			UpdateStress();
			UpdateStamina();
			UpdateBasicAttackHold();
			SynchronizeGamePlayer();
			UpdateAim();
			UpdateRun();
		}
	}

	protected override void OnStageStarted()
	{
		base.OnStageStarted();
		if (base.IsHost)
		{
			PlayerInventory.SafeSlots.NormalizeCount();
			PlayerInventory.Slots.NormalizeCount();
			Player.Character = Player.Character.NormalizeCount();
		}
	}

	private void SynchronizeGamePlayer()
	{
		if (Player != null && Player.HasChange)
		{
			GamePlayerData changeData = Player.Flush();
			GameHost.SendToSnapshotApplied(new NtfUpdateGamePlayer
			{
				ChangeData = changeData
			});
		}
	}

	private void UpdateStress()
	{
		AssertIfNotHost();
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		EActionState state = UnitModel.State;
		if (state == EActionState.Dead || state == EActionState.Dying || state == EActionState.Escaped)
		{
			return;
		}
		UpdateNearHighStressAllyEffect();
		UpdateIncreaseStress();
		UpdateCorruptionStress();
		UpdateIntervalDebuff();
		UpdateShadowMonster();
		void UpdateCorruptionStress()
		{
			_corruptionStressUpdateRemainTime -= Time.fixedDeltaTime;
			if (!(_corruptionStressUpdateRemainTime > 0f))
			{
				TCorruption tCorruption = TCorruption.Table[CharacterModel.CorruptionState];
				_corruptionStressUpdateRemainTime = tCorruption.StressDamageIntervalTime;
				if (!(tCorruption.StressDamageAmount <= 0f))
				{
					HandleUnitEvent(new StressDamageEvent
					{
						SourceModel = CharacterModel,
						Amount = tCorruption.StressDamageAmount,
						HitPosition = UnitModel.Position,
						ReasonType = EStressReasonType.Corruption
					});
				}
			}
		}
		void ResetShadowSpawnState()
		{
			_shadowSpawnRemainCount = 0;
			_shadowSpawnRemainTime = 0f;
			_shadowSpawnRemainDelayTime = 0f;
		}
		void ScheduleShadowMonster()
		{
			TShadowMonsterSpawn tShadowMonsterSpawn = TShadowMonsterSpawn.SubTable[_gameContext.Difficulty, UnitModel.StressState];
			float floatValue = TConst.ShadowMonsterSpawnStartDelay.FloatValue;
			if (_shadowSpawnRemainTime == 0f)
			{
				_shadowSpawnRemainTime = floatValue;
			}
			_shadowSpawnRemainTime -= Time.fixedDeltaTime;
			if (_shadowSpawnRemainTime < 0f)
			{
				_shadowSpawnRemainTime = tShadowMonsterSpawn.ShadowSpawnInterval;
				if (RandomUtility.GetRandomBoolFromPercentage(tShadowMonsterSpawn.ShadowSpawnPercent))
				{
					_shadowSpawnRemainCount += RandomUtility.GetRandomInt(tShadowMonsterSpawn.ShadowMonsterCountMin, tShadowMonsterSpawn.ShadowMonsterCountMax + 1);
				}
			}
		}
		void TrySpawnShadowMonster()
		{
			if (_shadowSpawnRemainCount <= 0)
			{
				return;
			}
			_shadowSpawnRemainDelayTime -= Time.fixedDeltaTime;
			if (!(_shadowSpawnRemainDelayTime < 0f))
			{
				return;
			}
			float floatValue = TConst.ShadowMonsterSpawnDelay.FloatValue;
			float floatValue2 = TConst.ShadowMonsterSpawnRange.FloatValue;
			float floatValue3 = TConst.ShadowMonsterLifeTime.FloatValue;
			TShadowMonsterSpawn tShadowMonsterSpawn = TShadowMonsterSpawn.SubTable[_gameContext.Difficulty, UnitModel.StressState];
			Vector3 position = UnitModel.Position;
			float randomFloat = RandomUtility.GetRandomFloat(0f - floatValue2, floatValue2);
			Vector3 position2 = UnitModel.Position;
			Vector3 position3 = UnitModel.Position;
			float randomFloat2 = RandomUtility.GetRandomFloat(0f - floatValue2, floatValue2);
			int num = tShadowMonsterSpawn.ShadowMonsterGroupSpawnId;
			TAnomaly tAnomaly = TAnomaly.Table[_gameContext.AnomalyType];
			if (tAnomaly.EffectTypes != null && tAnomaly.EffectTypes.Contains(EAnomalyEffectType.SpawnShadowAsDeepOne) && tShadowMonsterSpawn.AnomalyGroupSpawnId > 0 && RandomUtility.GetRandomBoolFromPercentage(TConst.AnomalyDeepOneSpawnChancePercent.FloatValue))
			{
				num = tShadowMonsterSpawn.AnomalyGroupSpawnId;
			}
			if (NavMesh.SamplePosition(new Vector3(randomFloat + position.x, position2.y, randomFloat2 + position3.z), out var hit, 1f, 1))
			{
				MonsterSpawnData spawnData = new MonsterSpawnData
				{
					IsRespawn = false,
					UseMonsterSpawnTable = true,
					MonsterSpawnGroupId = num,
					SpawnPosition = hit.position,
					LifeTime = floatValue3,
					SpawnType = ESpawnType.ShadowMonster,
					TargetNetworkObjectId = base.NetworkObjectId
				};
				ObjectManager.TrySpawnMonster(spawnData);
				_shadowSpawnRemainCount--;
				_shadowSpawnRemainDelayTime = floatValue;
			}
		}
		void UpdateIncreaseStress()
		{
			_stressUpdateRemainTime -= Time.fixedDeltaTime;
			if (!(_stressUpdateRemainTime > 0f))
			{
				float num = Mathf.Max(0f, (CharacterModel.StressIntervalPercent + 100f) / 100f);
				_stressUpdateRemainTime = RandomUtility.GetRandomFloat(TConst.StressIntervalTimeMin.FloatValue, TConst.StressIntervalTimeMax.FloatValue) * num;
				int randomInt = RandomUtility.GetRandomInt(TConst.StressIntervalDamageMin.IntValue, TConst.StressIntervalDamageMax.IntValue + 1);
				HandleUnitEvent(new StressDamageEvent
				{
					SourceModel = CharacterModel,
					Amount = randomInt,
					HitPosition = UnitModel.Position,
					ReasonType = EStressReasonType.Natural
				});
			}
		}
		void UpdateIntervalDebuff()
		{
			if (_gameContext.IsTutorial)
			{
				return;
			}
			TStress tStress = TStress.Table[UnitModel.StressState];
			float timeNow = _gameManager.TimeNow;
			EStressState currentStressDebuffState = _currentStressDebuffState;
			if (currentStressDebuffState != UnitModel.StressState)
			{
				_stressDebuffStartTime = timeNow + tStress.IntervalDebuffStartDelay;
				_nextStressDebuffUpdateTime = Mathf.Min(_nextStressDebuffUpdateTime, tStress.DebuffInterval + timeNow);
				_currentStressDebuffState = UnitModel.StressState;
				_stressBuffToastDisplayed = false;
			}
			if (tStress.IntervalDebuffId < 1)
			{
				_nextStressDebuffUpdateTime = 0f;
				return;
			}
			float num = Mathf.Max(_stressDebuffStartTime, _nextStressDebuffUpdateTime);
			if (!UnitModel.HasBuffEffect(EBuffEffectType.Madness) && !_stressBuffToastDisplayed && num - timeNow < 3f)
			{
				_stressBuffToastDisplayed = true;
				GameHost.SendToAll(new NtfUnitBubbleText
				{
					NetworkObjectId = base.NetworkObjectId,
					TextKey = TLocalizedText.MadnessText.Id
				});
				GameHost.SendToAll(new NtfUserTextKeyChat
				{
					ClientId = ClientId,
					PlayerName = CharacterModel.PlayerName,
					TextKey = TLocalizedText.MadnessText.Id
				});
			}
			if (num <= timeNow)
			{
				_nextStressDebuffUpdateTime = timeNow + tStress.DebuffInterval;
				UnitModel.AddBuff(tStress.IntervalDebuffId, UnitModel, 1, 0uL, Vector3.zero);
				_stressBuffToastDisplayed = false;
			}
		}
		void UpdateShadowMonster()
		{
			if (!_gameContext.IsTutorial && UnitModel.StressState > EStressState.Alert)
			{
				ScheduleShadowMonster();
				TrySpawnShadowMonster();
			}
			else
			{
				ResetShadowSpawnState();
			}
		}
		void UpdateNearHighStressAllyEffect()
		{
			float range = TConst.StressDebuffTriggerAllyRange.FloatValue;
			int num = _gameManager.CharacterModels.Where((CharacterModel _) => _.HitLayer != (long)ClientId).Where((CharacterModel _) => _.State != EActionState.Dying && _.State != EActionState.Dead && _.State != EActionState.Revive && _.State != EActionState.PhaseUp && _.State != EActionState.Cutscene && _.State != EActionState.ReservedDespawn && _.State != EActionState.Escaped && _.State != EActionState.Captured && _.State != EActionState.StageReady && _.State != EActionState.Spawn).Where((CharacterModel _) => _.StressState == EStressState.Fear || _.StressState == EStressState.Despair).Count((CharacterModel _) => Vector3.Distance(_.Position, UnitModel.Position) <= range);
			int num2 = (UnitModel.Buffs.TryGetValue(2000001, out var value) ? value.StackCount : 0);
			num -= num2;
			if (num >= 1)
			{
				UnitModel.AddBuff(2000001, UnitModel, num, 0uL, Vector3.zero);
			}
			else if (num < 0)
			{
				UnitModel.RemoveBuff(2000001, Math.Abs(num), false);
			}
		}
	}

	private void UpdateLightFuel()
	{
		AssertIfNotHost();
		if (!UnitModel.IsNonCombat)
		{
			EActionState state = CharacterModel.State;
			if (state != EActionState.Dying && state != EActionState.Dead && state != EActionState.Escaped || state == EActionState.Interact)
			{
				CharacterModel.IsLightOn = CharacterModel.LightFuel > 0;
				if (CharacterModel.LightFuel < 1)
				{
					return;
				}
				_brightnessElapsedUpdateTime += Time.fixedDeltaTime;
				float num = TConst.LightFuelInterval.FloatValue;
				GamePlayer player = Player;
				int lightFuelEfficiency = player.LightFuelEfficiency;
				if (lightFuelEfficiency > 0)
				{
					num += (float)lightFuelEfficiency * num / 100f;
				}
				if (num <= _brightnessElapsedUpdateTime)
				{
					HandleUnitEvent(new LightFuelDamageEvent
					{
						Amount = TConst.LightFuelTickDamage.IntValue
					});
					_brightnessElapsedUpdateTime -= num;
				}
				return;
			}
		}
		CharacterModel.IsLightOn = false;
	}

	private void UpdateStamina()
	{
		AssertIfNotHost();
		if (CharacterModel.Stamina < CharacterModel.StaminaMax && TimeNow - _lastStaminaDecreasedTime >= TConst.StaminaRegenDelay.FloatValue)
		{
			HandleUnitEvent(new StaminaRecoveryEvent
			{
				Amount = CharacterModel.StaminaRegen * Time.fixedDeltaTime
			});
		}
	}

	private void OnLightOnChanged(bool isLightOn)
	{
		AssertIfNotHost();
		if (isLightOn)
		{
			_brightnessElapsedUpdateTime = 0f;
		}
	}

	private void OnLightFuelChanged(int lightFuel)
	{
		AssertIfNotHost();
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (lightFuel < 1)
		{
			GameHost.SendToAll(new NtfUnitMonologueText
			{
				NetworkObjectId = base.NetworkObjectId,
				TextKey = TLocalizedText.CannotSurviveWithoutLight.Id
			});
			CharacterModel.AddBuff(2003001, UnitModel, 1, 0uL, Vector3.zero);
		}
		else
		{
			CharacterModel.RemoveBuff(2003001, 1, false);
		}
	}

	private void OnStaminaChanged(float oldStamina, float newStamina)
	{
		AssertIfNotHost();
		if (newStamina < oldStamina)
		{
			_lastStaminaDecreasedTime = _gameManager.TimeNow;
		}
		if (!(UnitModel.RunStaminaCost * Time.fixedDeltaTime < newStamina))
		{
			CharacterModel.TryRun = false;
			GameHost.SendToAll(new NtfUnitMonologueText
			{
				NetworkObjectId = base.NetworkObjectId,
				TextKey = TLocalizedText.TooTiredToRun.Id
			});
		}
	}

	private void UpdateFogOfWar()
	{
		Vector3 direction = default(Vector3);
		direction = CharacterModel.Forward;
		PlaneSightUnit.SetDirection(direction);
		PlaneSightUnit.SetOnOffToggle(CharacterModel.IsLightOn);
		PlaneSightUnit.SetLightFuel(CharacterModel.LightFuel);
		PlaneSightUnit.SetIsMine(_gameManager.RxViewTargetClientId.Value == ClientId);
		PlaneSightUnit.SetIsShared(true);
	}

	private IList<int> GetOwnedEventItemIds()
	{
		HashSet<int> eventItemIds = new HashSet<int>();
		_campaignManager.CampaignTasks.Values.Select((CampaignTask _) => TCampaignTask.Table[_.Id]).Where((TCampaignTask _) => _.EventCondition.Type == EEventConditionType.MyItemCountInInventory || _.EventCondition.Type == EEventConditionType.TeamItemCountInInventory || _.EventCondition.Type == EEventConditionType.MyItemCountTotal).Select((TCampaignTask _) => (int)_.EventCondition.Value1)
			.ForEach(delegate(int id)
			{
				eventItemIds.Add(id);
			});
		if (_missionManager.HasMission)
		{
			_missionManager.MissionIds.Select((int _) => TMission.Table[_]).Where((TMission _) => _.EventCondition.Type == EEventConditionType.MyItemCountInInventory || _.EventCondition.Type == EEventConditionType.TeamItemCountInInventory || _.EventCondition.Type == EEventConditionType.MyItemCountTotal).Select((TMission _) => (int)_.EventCondition.Value1)
				.ForEach(delegate(int id)
				{
					eventItemIds.Add(id);
				});
		}
		_playerManager.GetLoungeQuestIds().Where((int _) => TLoungeQuestTask.GroupTable.ContainsKey(_)).SelectMany((int _) => TLoungeQuestTask.GroupTable[_]).Where((TLoungeQuestTask _) => _.EventCondition.Type == EEventConditionType.MyItemCountInInventory || _.EventCondition.Type == EEventConditionType.TeamItemCountInInventory || _.EventCondition.Type == EEventConditionType.MyItemCountTotal)
			.Select((TLoungeQuestTask _) => (int)_.EventCondition.Value1)
			.ForEach(delegate(int _)
			{
				eventItemIds.Add(_);
			});
		return eventItemIds.ToList();
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		IList<int> ownedEventItemIds = GetOwnedEventItemIds();
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			int num = changeData.OldSlot?.Id ?? 0;
			int num2 = changeData.NewSlot?.Id ?? 0;
			if (num > 0 && ownedEventItemIds.Contains(num))
			{
				TriggerOwnedItemEvent(num);
			}
			if (num2 > 0 && ownedEventItemIds.Contains(num2))
			{
				TriggerOwnedItemEvent(num2);
			}
		}
		TriggerGainWorthEvent();
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
		IList<int> ownedEventItemIds = GetOwnedEventItemIds();
		foreach (GoodsSlotChangeData changeData in changeDatas)
		{
			int num = changeData.OldSlot?.Id ?? 0;
			int num2 = changeData.NewSlot?.Id ?? 0;
			if (num > 0 && ownedEventItemIds.Contains(num))
			{
				TriggerOwnedItemEvent(num);
			}
			if (num2 > 0 && ownedEventItemIds.Contains(num2))
			{
				TriggerOwnedItemEvent(num2);
			}
		}
		TriggerGainWorthEvent();
	}

	private void OnDead()
	{
		GetOwnedEventItemIds().Where((int _) => CharacterModel.Inventory.GetItemCount(_) > 0).ForEach(TriggerOwnedItemEvent);
		TriggerGainWorthEvent();
	}

	private void OnRevive()
	{
		GetOwnedEventItemIds().Where((int _) => CharacterModel.Inventory.GetItemCount(_) > 0).ForEach(TriggerOwnedItemEvent);
		TriggerGainWorthEvent();
	}

	private void OnStashChanged()
	{
		GetOwnedEventItemIds().ForEach(TriggerOwnedItemEvent);
	}

	private void TriggerOwnedItemEvent(int itemId)
	{
		if (_gameManager.MyCharacterModel == null)
		{
			return;
		}
		long itemCountInInventory = _gameManager.MyCharacterModel.GetOwnedItemCount(itemId);
		long itemCountInStash = _playerManager.StashSlots.GetItemCount(itemId);
		long teamItemCountInInventory = _gameManager.CharacterModels.Sum((CharacterModel _) => (_.State == EActionState.Dead) ? 0 : _.Inventory.GetItemCount(itemId));
		EventSystem.Trigger(new OwnedItemEvent(itemId, itemCountInInventory, itemCountInStash, teamItemCountInInventory));
	}

	private void TriggerGainWorthEvent()
	{
		if (_gameManager.MyCharacterModel == null)
		{
			return;
		}
		int gainWorth = _gameManager.MyCharacterModel.CalculateGainWorth();
		int teamTotalWorth = _gameManager.CharacterModels.Sum((CharacterModel _) => _.CalculateGainWorth());
		EventSystem.Trigger(new GainWorthEvent(gainWorth, teamTotalWorth));
	}

	private void ApplyParadox()
	{
		Player.GetParadoxIds().Select((int _) => TParadox.Table[_].DebuffId).ForEach(delegate(int debuffId)
		{
			CharacterModel.AddBuff(debuffId, CharacterModel, 1, 0uL, Vector3.zero);
		});
	}

	private void UpdateParadox(int[] oldParadoxIds, int[] newParadoxIds)
	{
		AssertIfNotHost();
		if (_gameManager.StageState == EStageState.Ended)
		{
			return;
		}
		HashSet<int> oldSet = new HashSet<int>(oldParadoxIds ?? Array.Empty<int>());
		HashSet<int> newSet = new HashSet<int>(newParadoxIds ?? Array.Empty<int>());
		foreach (int id in oldSet.Except(newSet))
		{
			CharacterModel.RemoveBuff(TParadox.Table[id].DebuffId, 1, false);
		}
		foreach (int id2 in newSet.Except(oldSet))
		{
			CharacterModel.AddBuff(TParadox.Table[id2].DebuffId, CharacterModel, 1, 0uL, Vector3.zero);
		}
	}

	private void OnStressChanged(float value)
	{
		AssertIfNotHost();
		if (value <= 0f)
		{
			TBuff.DespairBuffIds.Where((int _) => CharacterModel.Buffs.ContainsKey(_)).ForEach(delegate(int _)
			{
				CharacterModel.RemoveBuff(_, 1, isExpired: false);
			});
		}
	}

	private void OnStressStateChanged(EStressState state)
	{
		AssertIfNotHost();
		_stressStateDisposable?.Dispose();
		_stressStateDisposable = new CompositeDisposable();
		TStress.Table.Values.ForEach(delegate(TStress _)
		{
			if (_.DebuffIds != null && !_.DebuffIds.Any())
			{
				return;
			}
			_.DebuffIds.ForEach(delegate(int debuffId)
			{
				if (state == _.State)
				{
					if (!UnitModel.HasBuff(debuffId))
					{
						UnitModel.AddBuff(debuffId, UnitModel, 1, 0uL, Vector3.zero);
					}
				}
				if (state != _.State && UnitModel.HasBuff(debuffId))
				{
					UnitModel.RemoveBuff(debuffId, 1, isExpired: false);
				}
			});
		});
		CharacterModel.RefreshStressConditionalStats();
		if (state != EStressState.Despair)
		{
			return;
		}
		SendDespairVfx();
		Observable.Interval(TimeSpan.FromSeconds(TConst.StressDespairVfxInterval.FloatValue)).Where(delegate(long _)
		{
			EActionState current = UnitModel.State;
			return current != EActionState.Dying && current != EActionState.Dead && current != EActionState.Revive && current != EActionState.PhaseUp && current != EActionState.Cutscene && current != EActionState.ReservedDespawn && current != EActionState.Escaped && current != EActionState.Captured && current != EActionState.StageReady && current != EActionState.Spawn;
		}).Subscribe(delegate
		{
			SendDespairVfx();
		}).AddTo(_stressStateDisposable);
		IReadOnlyList<int> despairBuffs = TBuff.DespairBuffIds;
		if (despairBuffs.Count >= 1 && !CharacterModel.Buffs.Keys.Any((int _) => despairBuffs.Contains(_)))
		{
			int randomInt = RandomUtility.GetRandomInt(0, despairBuffs.Count);
			HandleUnitEvent(new BuffEvent
			{
				BuffId = despairBuffs[randomInt],
				BuffStack = 1,
				SourceTarget = CharacterModel
			});
		}
		void SendDespairVfx()
		{
			GameHost.SendToAll(new NtfDisplayFx
			{
				Vfx = "Status/Despair",
				VfxDuration = 1f,
				Position = UnitModel.Position.ToProtoVector3(),
				TargetNetworkObjectId = base.NetworkObjectId,
				BoneType = EUnitBoneType.Head
			});
		}
	}

	private void OnCorruptionStateChanged(ECorruptionState before, ECorruptionState after)
	{
		AssertIfNotHost();
		int[] debuffs = GetDebuffs(before);
		int[] debuffs2 = GetDebuffs(after);
		int[] array = debuffs;
		foreach (int num in array)
		{
			if (!debuffs2.Contains(num) && UnitModel.HasBuff(num))
			{
				UnitModel.RemoveBuff(num, 1, false);
			}
		}
		array = debuffs2;
		foreach (int num2 in array)
		{
			if (!debuffs.Contains(num2) && !UnitModel.HasBuff(num2))
			{
				UnitModel.AddBuff(num2, UnitModel, 1, 0uL, Vector3.zero);
			}
		}
		int[] GetDebuffs(ECorruptionState state)
		{
			if (TCorruption.Table.TryGetValue(state, out var value) && value.DebuffIds != null)
			{
				return value.DebuffIds;
			}
			return Array.Empty<int>();
		}
	}

	private void OnCharacterUpdated(Character oldCharacter, Character newCharacter)
	{
		if (oldCharacter != null && newCharacter != null && oldCharacter.Id == newCharacter.Id)
		{
			int[] paradoxIds = oldCharacter.GetParadoxIds();
			int[] paradoxIds2 = newCharacter.GetParadoxIds();
			UpdateParadox(paradoxIds, paradoxIds2);
			UpdateCorruption();
		}
	}

	private void UpdateCorruption()
	{
		AssertIfNotHost();
		int num = CharacterModel.CalculateTotalCorruption();
		if (CharacterModel.Corruption != num)
		{
			CharacterModel.Corruption = num;
		}
	}

	private void OnInventoryChangedForCorruption(IList<GoodsSlotChangeData> changeDatas)
	{
		if (changeDatas.Any((GoodsSlotChangeData _) => _.OldSlot?.Type == EGoodsType.Artifact || _.NewSlot?.Type == EGoodsType.Artifact))
		{
			UpdateCorruption();
		}
	}

	private void OnSafeInventoryChangedForCorruption(IList<GoodsSlotChangeData> changeDatas)
	{
		if (changeDatas.Any((GoodsSlotChangeData _) => _.OldSlot?.Type == EGoodsType.Artifact || _.NewSlot?.Type == EGoodsType.Artifact))
		{
			UpdateCorruption();
		}
	}

	private void OnBuffUpdate()
	{
		PlaneSightUnit.SetIsBlinded(UnitModel.HasBuffEffect(EBuffEffectType.Blind));
	}

	private void OnTryRunChanged()
	{
		CharacterModel.UpdateStat(EStatType.MoveSpeed);
	}

	private void OnIsMovingBackwardChanged()
	{
		CharacterModel.UpdateStat(EStatType.MoveSpeed);
	}

	private void OnStateChanged(EActionState current, EActionState previous)
	{
		if (IsMine)
		{
			if (current == EActionState.Dying)
			{
				if (previous != EActionState.Dying)
				{
					_cameraManager.DeactivateUiCamera();
				}
			}
			else if (previous == EActionState.Dying)
			{
				_cameraManager.ActivateUiCamera();
			}
		}
		OutlineObject.SetEnable(current == EActionState.Dead);
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
		if (oldCharacter == null || newCharacter == null || oldCharacter.Id != newCharacter.Id)
		{
			return;
		}
		UnitModel.ApplyActiveSkills(newCharacter.GetInputActiveSkillIds());
		List<int> list = UnitModel.ApplyPassiveSkillsDelta(Player.GetPassiveSkillIds());
		if (base.IsHost)
		{
			list.ForEach(delegate(int _)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = _
				});
			});
		}
		if (GetEquipmentWorth(oldCharacter) != GetEquipmentWorth(newCharacter))
		{
			TriggerGainWorthEvent();
		}
	}

	private bool IsEquipmentWorthChanged(Character oldCharacter, Character newCharacter)
	{
		return GetEquipmentWorth(oldCharacter) != GetEquipmentWorth(newCharacter);
	}

	private int GetEquipmentWorth(Character character)
	{
		int num = character.WeaponSlots.Sum((GoodsSlot _) => _.GainWorth);
		int num2 = character.AccessorySlots.Sum((GoodsSlot _) => _.GainWorth);
		int num3 = character.ArtifactSlots.Sum((GoodsSlot _) => _.GainWorth);
		return num3 + num2 + num;
	}

	private void OnTalentChanged(Talent oldTalent, Talent newTalent)
	{
		if (newTalent == null)
		{
			return;
		}
		BitFlags bitFlags = oldTalent?.BitFlags ?? new BitFlags();
		int[] addedBitFlags = bitFlags.GetAddedBitFlags(newTalent.BitFlags);
		int[] array = addedBitFlags;
		foreach (int bitFlagIndex in array)
		{
			UpdateTalentUnlock(TTalent.SubTable[bitFlagIndex].Id);
		}
	}

	private void UpdateTalentUnlock(int talentId)
	{
		TalentEffect[] effects = TTalent.Table[talentId].Effects;
		if (effects == null)
		{
			return;
		}
		TalentEffect[] array = effects;
		for (int i = 0; i < array.Length; i++)
		{
			TalentEffect talentEffect = array[i];
			switch (talentEffect.Type)
			{
			case ETalentEffectType.AddSkill:
				if (TSkill.Table[talentEffect.Value1].SkillType == ESkillType.Passive && CharacterModel.PassiveSkillIds != null)
				{
					CharacterModel.PassiveSkillIds.Add(talentEffect.Value1);
					if (base.IsHost)
					{
						HandleUnitEvent(new AddPassiveEvent
						{
							SkillId = talentEffect.Value1
						});
					}
				}
				break;
			case ETalentEffectType.SkillSlotCount:
			case ETalentEffectType.UnlockSkillMode:
			case ETalentEffectType.ChangeSkill:
				CharacterModel.ApplyActiveSkills(Player.Character.GetInputActiveSkillIds());
				break;
			}
		}
	}

	private void UpdateAim()
	{
		AssertIfNotHost();
		if (StateMachine.IsAimEnable)
		{
			UnitModel.UpdateAim();
		}
	}

	private void UpdateRun()
	{
		AssertIfNotHost();
		if (!CharacterModel.TryRun)
		{
			return;
		}
		EActionState state = CharacterModel.State;
		if (state == EActionState.Skill || state == EActionState.CrowdControlled || state == EActionState.Interact)
		{
			CharacterModel.TryRunExpireTime = TimeNow + TConst.ToggleRunExpireDelay.FloatValue;
		}
		else if (TimeNow > CharacterModel.TryRunExpireTime)
		{
			CharacterModel.TryRun = false;
		}
	}

	protected override void RegisterClientHandlers()
	{
		base.RegisterClientHandlers();
		RegisterClientHandler<NtfStartInteraction>(OnStartInteraction);
		RegisterClientHandler<NtfEndInteraction>(OnEndInteraction);
		RegisterClientHandler<NtfChangeCharacterSkin>(OnChangeCharacterSkin);
		RegisterClientHandler<NtfChangeActiveSkill>(OnChangeActiveSkill);
		RegisterClientHandler<NtfEmoji>(OnEmoji);
		RegisterClientHandler<NtfSelfReviveText>(OnSelfReviveText);
		RegisterClientHandler<NtfMonsterKill>(OnMonsterKill);
		RegisterClientHandler<NtfItemUsed>(OnItemUsed);
		RegisterClientInventoryHandlers();
		RegisterClientPlayerActionHandlers();
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
		if (message.SourceClientId != CharacterModel.ClientId)
		{
			return;
		}
		IInteractiveObject interactiveObject = ObjectManager.FindInteractiveObject(message.TargetNetworkObjectId);
		if (interactiveObject == null || !base.IsHost)
		{
			return;
		}
		if (interactiveObject is DropGoods dropGoods)
		{
			Goods originalGoods = dropGoods.OriginalGoods;
			if (Inventory.TryPushGoods(originalGoods, out var remainGoods))
			{
				dropGoods.SetGoodsCount(0L, 0L);
				return;
			}
			if (originalGoods.IsBag())
			{
				TLocalizedText tLocalizedText = (Inventory.IsSlotFull() ? TLocalizedText.CannotCarryMoreSlot : TLocalizedText.CannotCarrySameBagType);
				GameHost.SendToClient(message.SourceClientId, new NtfUnitMonologueText
				{
					NetworkObjectId = base.NetworkObjectId,
					TextKey = tLocalizedText.Id
				});
			}
			else if (originalGoods.Count != remainGoods.Count)
			{
				GameHost.SendToClient(message.SourceClientId, new NtfUnitMonologueText
				{
					NetworkObjectId = base.NetworkObjectId,
					TextKey = TLocalizedText.PickUpItemMaxCountMonologue.Id
				});
			}
			if (originalGoods.IsItem() && originalGoods.Count != remainGoods.Count)
			{
				dropGoods.SetGoodsCount(remainGoods.BaseCount, remainGoods.GainCount);
			}
		}
		else if (interactiveObject is WaveExit || interactiveObject is SafeExit)
		{
			((ExitObject)interactiveObject).OnInteraction(this);
		}
		else if (interactiveObject is Portal portal)
		{
			CharacterView.MoveTo(portal.ExitPosition);
		}
		else if (interactiveObject is CharacterBody characterBody)
		{
			characterBody.Execute(UnitModel);
		}
		else if (interactiveObject is MonsterBody || (interactiveObject is RewardBox rewardBox && rewardBox.CheckStressCondition(CharacterModel.StressState)))
		{
			CharacterModel.SyncIsWaitingInteractionFinish.Value = true;
		}
		else if (interactiveObject is WaveExecutor waveExecutor)
		{
			waveExecutor.ExecuteWave();
		}
		else if (interactiveObject is SkillExecutor skillExecutor)
		{
			if (skillExecutor.CheckStressCondition(CharacterModel.StressState))
			{
				skillExecutor.Execute(CharacterModel);
			}
		}
		else if (interactiveObject is PhoneBooth phoneBooth && phoneBooth.CheckCanEnter(CharacterModel.ClientId) == ECanEnterPhoneBoothResult.Success)
		{
			HandleUnitEvent(new StageReadyEvent());
		}
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
		// TODO(decomp): kept; machine code @0x18061a680 is only: null-check message (NullReferenceException), null-check CharacterModel, tail-call CharacterModel.get_ClientId with the result unused. The read of message.<field> and any compare/branch against ClientId were dead code and are gone from the binary, so the original source line cannot be recovered
		_ = CharacterModel.ClientId;
	}

	private void OnChangeCharacterSkin(NtfChangeCharacterSkin message)
	{
		if (message.NetworkObjectId == base.NetworkObjectId)
		{
			CharacterView.UpdateSkins(message.IsSuper);
		}
	}

	private void OnChangeActiveSkill(NtfChangeActiveSkill message)
	{
		if (message.NetworkObjectId == base.NetworkObjectId)
		{
			CharacterModel.ApplyActiveSkills(message.SkillIds);
		}
	}

	private void OnEmoji(NtfEmoji message)
	{
		if (message.NetworkObjectId == CharacterModel.NetworkObjectId)
		{
			SoundManager.PlaySfx(string.Format("Emoji_{0:D2}", message.EmojiId));
			OnShowEmoji.OnNext(message.EmojiId);
		}
	}

	private void OnSelfReviveText(NtfSelfReviveText message)
	{
		if (message.ClientId != CharacterModel.ClientId)
		{
			return;
		}
		string text = ((message.EquipmentId >= 1) ? string.Format(TLocalizedText.SelfReviveWithEquipmentFloatingTextFormat.LocalText, TEquipment.Table[message.EquipmentId].Name) : TLocalizedText.DoorOfDeath.LocalText);
		if (IsMine)
		{
			string text2 = ((message.EquipmentId >= 1) ? string.Format(TLocalizedText.SelfReviveWithEquipmentChatFormat.LocalText, TEquipment.Table[message.EquipmentId].Name) : TLocalizedText.SelfReviveGuideChat.LocalText);
			_chatManager.AddChat(new ChatInfo
			{
				Type = EChatType.System,
				Text = text2,
				UtcTime = DateTime.UtcNow
			});
		}
		OnShowSelfReviveText.OnNext(text);
	}

	private void OnMonsterKill(NtfMonsterKill message)
	{
		if (message.SourceClientId == CharacterModel.ClientId)
		{
			EventSystem.Trigger(new MonsterKillEvent(message.SourceClientId, message.MonsterId));
		}
	}

	private void OnItemUsed(NtfItemUsed message)
	{
		_playerManager.SetItemUsed(message.ItemId);
		EventSystem.Trigger(new ItemUseEvent(message.ItemId));
	}

	private void RegisterClientInventoryHandlers()
	{
		RegisterClientHandler<ResInventorySlotToStashSlot>(OnInventorySlotToStashSlot);
		RegisterClientHandler<ResSafeSlotToStashSlot>(OnSafeSlotToStashSlot);
		RegisterClientHandler<ResMoveAllInventoryToStash>(OnMoveAllInventoryToStash);
		RegisterClientHandler<ResEquipStashSlot>(OnEquipStashSlot);
		RegisterClientHandler<ResEquipLootingSlot>(OnEquipLootingSlot);
		RegisterClientHandler<ResUnequipToStashSlot>(OnUnequipToStashSlot);
		RegisterClientHandler<ResInventorySlotToStashBagSlot>(OnInventorySlotToStashBagSlot);
		RegisterClientHandler<ResBagSlotToStash>(OnBagSlotToStash);
	}

	private void OnBagSlotToStash(ResBagSlotToStash message)
	{
		if (message.IsSuccess && !_playerManager.StashSlots.TryPushGoods(message.PulledGoods, message.StashSlotIndex))
		{
			Logger.Error($"Failed to push goods to stash at index {message.StashSlotIndex}: {message.PulledGoods}");
			GameChannel.SendToHost(new ReqGainGoods
			{
				Goods = new Goods[1] { message.PulledGoods }
			});
		}
	}

	private void OnInventorySlotToStashBagSlot(ResInventorySlotToStashBagSlot message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		long itemMaxStackCount = ((message.PulledGoods.Category == EGoodsCategory.Item) ? _gameManager.MyPlayer.GetInventoryItemCountMax(message.PulledGoods.Id) : long.MaxValue);
		if (_playerManager.StashSlots.TryPushIntoBag(message.StashBagHolderSlotIndex, message.PulledGoods, itemMaxStackCount, message.InnerSlotIndex, out var swappedOutGoods))
		{
			if (swappedOutGoods != null)
			{
				GameChannel.SendToHost(new ReqGainGoods
				{
					Goods = new Goods[1] { swappedOutGoods }
				});
			}
		}
		else
		{
			Logger.Warning($"Stash bag push lost a race at holder {message.StashBagHolderSlotIndex}: {message.PulledGoods}");
			GameChannel.SendToHost(new ReqGainGoods
			{
				Goods = new Goods[1] { message.PulledGoods }
			});
		}
	}

	private void OnMoveAllInventoryToStash(ResMoveAllInventoryToStash message)
	{
		if (message.PulledGoods == null)
		{
			return;
		}
		Goods[] pulledGoods = message.PulledGoods;
		foreach (Goods goods in pulledGoods)
		{
			if (!_playerManager.StashSlots.TryPushGoods(goods, -1))
			{
				Logger.Error($"Failed to push goods to stash: {goods}");
			}
		}
	}

	private void OnInventorySlotToStashSlot(ResInventorySlotToStashSlot message)
	{
		if (message.IsSuccess && !_playerManager.StashSlots.TryPushGoods(message.PulledGoods, message.StashSlotIndex))
		{
			Logger.Error($"Failed to push pulled goods to stash slot {message.StashSlotIndex}: {message.PulledGoods}");
		}
	}

	private void OnSafeSlotToStashSlot(ResSafeSlotToStashSlot message)
	{
		if (message.IsSuccess && !_playerManager.StashSlots.TryPushGoods(message.PulledGoods, message.StashSlotIndex))
		{
			base.Logger.Error(string.Format("Failed to push pulled goods from safe to stash slot {0}: {1}", message.StashSlotIndex, message.PulledGoods));
		}
	}

	private void OnEquipStashSlot(ResEquipStashSlot message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (!_playerManager.StashSlots.TryPullSlot(message.StashSlotIndex, out var _))
		{
			base.Logger.Error(string.Format("Failed to remove equipment from stash index {0} during equip.", message.StashSlotIndex));
		}
		if (message.UnequippedGoods != null && !_playerManager.StashSlots.TryPushGoods(message.UnequippedGoods, message.StashSlotIndex))
		{
			base.Logger.Error(string.Format("Failed to push released equipment {0}: {1}", message.StashSlotIndex, message.UnequippedGoods));
		}
	}

	private void OnEquipLootingSlot(ResEquipLootingSlot message)
	{
		if (message.EquippedGoods != null && message.EquippedGoods.Category == EGoodsCategory.Equipment)
		{
			_playerManager.SetEquipmentCollection(message.EquippedGoods.Id);
		}
	}

	private void OnUnequipToStashSlot(ResUnequipToStashSlot message)
	{
		if (message.UnequippedGoods != null && !_playerManager.StashSlots.TryPushGoods(message.UnequippedGoods, message.StashSlotIndex))
		{
			base.Logger.Error(string.Format("Failed to push unequipped equipment {0}: {1}", message.StashSlotIndex, message.UnequippedGoods));
		}
	}

	private void RegisterClientPlayerActionHandlers()
	{
		if (IsMine)
		{
			RegisterClientHandler<ResLevelUp>(OnLevelUp);
			RegisterClientHandler<ResUnlockTalent>(OnUnlockTalent);
			RegisterClientHandler<ResShopBuyGoods>(OnShopBuyGoods);
			RegisterClientHandler<ResShopSellGoods>(OnShopSellGoods);
			RegisterClientHandler<ResCraftingGoods>(OnCraftingGoods);
			RegisterClientHandler<ResScrapGoods>(OnScrapGoods);
			RegisterClientHandler<ResRegisterItemUsed>(OnRegisterItemUsed);
			RegisterClientHandler<ResSellArtifact>(OnSellArtifact);
			RegisterClientHandler<ResDealArtifact>(OnDealArtifact);
			RegisterClientHandler<ResRepairGoods>(OnRepairGoods);
			RegisterClientHandler<ResRepairAllGoods>(OnRepairAllGoods);
			RegisterClientHandler<ResTycoonRegisterArtifact>(OnTycoonRegisterArtifact);
			RegisterClientHandler<ResTycoonFinish>(OnTycoonFinish);
			RegisterClientHandler<ResTycoonHire>(OnTycoonHire);
			RegisterClientHandler<ResTycoonCancel>(OnTycoonCancel);
			RegisterClientHandler<ResTycoonPurify>(OnTycoonPurify);
			RegisterClientHandler<ResRest>(OnRest);
			RegisterClientHandler<ResParadoxPurify>(OnParadoxPurify);
			RegisterClientHandler<ResParadoxLock>(OnParadoxLock);
			RegisterClientHandler<ResTakeLostItem>(OnTakeLostItem);
			RegisterClientHandler<ResRerollLostItems>(OnRerollLostItems);
			RegisterClientHandler<ResExchangeCoin>(OnExchangeCoin);
			RegisterClientHandler<ResBuyInterior>(OnBuyInterior);
			RegisterClientHandler<ResSetPet>(OnSetPet);
		}
	}

	private void OnLevelUp(ResLevelUp message)
	{
		if (message.IsSuccess && message.ConsumeGold > 0)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.LevelUp, $"{message.Level}", message.ConsumeGold));
		}
	}

	private void OnUnlockTalent(ResUnlockTalent message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.RemainMaterials != null && message.RemainMaterials.Any())
		{
			_playerManager.ConsumeStashGoods(message.RemainMaterials);
		}
		EventSystem.Trigger(new TalentEvent(message.TalentId));
		EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.UnlockTalent, $"{message.TalentId}", message.ConsumeGold));
	}

	private void OnShopBuyGoods(ResShopBuyGoods message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.RemainMaterials != null)
		{
			_playerManager.ConsumeStashGoods(message.RemainMaterials);
		}
		if (message.DestinationSlotCategory == EGoodsSlotCategory.StashSlot && message.StashGainGoods != null)
		{
			_playerManager.AddGoodsToStash(message.StashGainGoods);
		}
		EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.ShopBuy, $"{message.ProductId}:{message.BuyCount}", message.ConsumeGold));
	}

	private void OnShopSellGoods(ResShopSellGoods message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.SlotIndex, message.SellGoods);
		}
		EventSystem.Trigger(new GainGoldEvent(EGoldGainType.ShopSell, message.SellGoods.ToSimpleString(), message.GainGold));
	}

	private void OnCraftingGoods(ResCraftingGoods message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.RemainMaterials != null && message.RemainMaterials.Any())
		{
			_playerManager.ConsumeStashGoods(message.RemainMaterials);
		}
		if (message.RemainGoods != null && message.RemainGoods.Any())
		{
			Goods[] remainGoods = message.RemainGoods;
			foreach (Goods goods in remainGoods)
			{
				_playerManager.AddGoodsToStash(goods);
			}
		}
		EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.Crafting, $"{message.CraftingId}", message.ConsumeGold));
	}

	private void OnScrapGoods(ResScrapGoods message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.SlotIndex, message.ConsumeGoods);
		}
		if (message.RemainGoods != null && message.RemainGoods.Any())
		{
			Goods[] remainGoods = message.RemainGoods;
			foreach (Goods goods in remainGoods)
			{
				_playerManager.AddGoodsToStash(goods);
			}
		}
	}

	private void OnRegisterItemUsed(ResRegisterItemUsed message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.SlotIndex, message.ConsumeGoods);
		}
		_playerManager.SetItemUsed(message.ItemId);
		EventSystem.Trigger(new ItemUseEvent(message.ItemId));
	}

	private void OnSellArtifact(ResSellArtifact message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.SlotIndex, message.SellGoods);
		}
		EventSystem.Trigger(new GainGoldEvent(EGoldGainType.DealArtifact, message.SellGoods.ToSimpleString(), message.GainGold));
	}

	private void OnDealArtifact(ResDealArtifact message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.SlotIndex, message.DealGoods);
		}
		EventSystem.Trigger(new GainCoinEvent(ECoinGainType.DealArtifact, message.GainCoin));
	}

	private void OnTycoonRegisterArtifact(ResTycoonRegisterArtifact message)
	{
		if (message.IsSuccess && message.ArtifactSlotCategory == EGoodsSlotCategory.StashSlot)
		{
			_playerManager.ConsumeStashSlot(message.ArtifactSlotIndex, message.Artifact);
		}
	}

	private void OnTycoonFinish(ResTycoonFinish message)
	{
		if (message.IsSuccess && message.GainCoin > 0)
		{
			EventSystem.Trigger(new GainCoinEvent(ECoinGainType.TycoonSales, message.GainCoin));
		}
	}

	private void OnTycoonHire(ResTycoonHire message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.TycoonHire, string.Empty, message.ConsumeGold));
		}
	}

	private void OnTycoonCancel(ResTycoonCancel message)
	{
	}

	private void OnTycoonPurify(ResTycoonPurify message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.TycoonPurify, string.Empty, message.ConsumeGold));
		}
	}

	private void OnRest(ResRest message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.Rest, $"{message.RemovedParadoxId}", message.ConsumeGold));
		}
	}

	private void OnParadoxPurify(ResParadoxPurify message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.ParadoxPurify, $"{message.ParadoxId}", message.ConsumeGold));
		}
	}

	private void OnParadoxLock(ResParadoxLock message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.ParadoxLock, $"{message.ParadoxId}", message.ConsumeGold));
		}
	}

	private void OnRepairGoods(ResRepairGoods message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.RepairGoods, message.RepairedGoods.ToSimpleString(), message.ConsumeGold));
		}
	}

	private void OnRepairAllGoods(ResRepairAllGoods message)
	{
		if (message.IsSuccess)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.RepairGoods, string.Empty, message.TotalConsumeGold));
		}
	}

	private void OnTakeLostItem(ResTakeLostItem message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		if (_playerManager.TryRemoveLostGoods(message.SlotIndex, out var goods))
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.TakeLostItem, goods.ToSimpleString(), message.ConsumeGold));
		}
	}

	private void OnRerollLostItems(ResRerollLostItems message)
	{
		if (!message.IsSuccess)
		{
			return;
		}
		_playerManager.RebuildSelectInfos();
		EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.RerollLostItems, string.Empty, message.ConsumeGold));
	}

	private void OnExchangeCoin(ResExchangeCoin message)
	{
		if (!message.IsSuccess || message.RemainGoods == null)
		{
			return;
		}
		if (message.Direction != ECoinExchangeDirection.CoinToItem)
		{
			_playerManager.ConsumeStashGoods(message.RemainGoods);
		}
		else
		{
			_playerManager.AddGoodsToStash(message.RemainGoods);
		}
	}

	private void OnBuyInterior(ResBuyInterior message)
	{
		if (message.IsSuccess && message.ConsumeGold > 0)
		{
			EventSystem.Trigger(new ConsumeGoldEvent(EGoldConsumeType.InteriorBuy, $"{message.InteriorShopId}", message.ConsumeGold));
		}
	}

	private void OnSetPet(ResSetPet message)
	{
	}

	protected override void RegisterHostHandlers()
	{
		base.RegisterHostHandlers();
		RegisterHostHandler<ReqInputMoveAim>(OnInputMoveAim);
		RegisterHostHandler<ReqInputRun>(OnInputRun);
		RegisterHostHandler<ReqInputSkill>(OnInputSkill);
		RegisterHostHandler<ReqInputUseItem>(OnInputUseItem);
		RegisterHostHandler<ReqInputTryInteract>(OnInputTryInteract);
		RegisterHostHandler<ReqInputCancelInteract>(OnInputCancelInteract);
		RegisterHostHandler<ReqTryKeyInteraction>(OnTryKeyInteraction);
		RegisterHostHandler<ReqFinishInteractionWaiting>(OnFinishInteractionWaiting);
		RegisterHostHandler<ReqKillCharacter>(OnKillCharacter);
		RegisterHostHandler<ReqSpawnDropGoods>(OnSpawnDropGoods);
		RegisterHostHandler<ReqSpawnMonster>(OnSpawnMonster);
		RegisterHostHandler<ReqSpawnWaveExecutor>(OnSpawnWaveExecutor);
		RegisterHostHandler<ReqSpawnExit>(OnSpawnExit);
		RegisterHostHandler<ReqRewardBox>(OnRewardBox);
		RegisterHostHandler<ReqForceEscape>(OnForceEscape);
		RegisterHostHandler<ReqPlayCutscene>(OnPlayCutscene);
		RegisterHostHandler<ReqFinishCutscene>(OnFinishCutscene);
		RegisterHostCheatHandlers();
		RegisterHostInventoryHandlers();
		RegisterHostPlayerActionHandlers();
		RegisterHostEtcHandlers();
	}

	private void OnTryKeyInteraction(ReqTryKeyInteraction message)
	{
		if (message.KeyItemIds.Any((int _) => Inventory.GetItemCount(_) < 1))
		{
			return;
		}
		IInteractiveObject interactiveObject = _gameObjectManager.InteractiveObjects[message.InteractiveObjectId];
		if (interactiveObject is RewardBox rewardBox)
		{
			if (rewardBox.LockState != ELockState.Locked)
			{
				return;
			}
			CancelInteraction();
			TRewardBox tRewardBox = TRewardBox.Table[rewardBox.RewardBoxId];
			if (tRewardBox.KeyItemIds.Any((int _) => !message.KeyItemIds.Contains(_)))
			{
				if (0 < tRewardBox.KeyFailHitboxId)
				{
					SpawnRewardBoxHitbox(rewardBox, isSuccess: false);
				}
				if (!tRewardBox.KeyFailRetryAble)
				{
					rewardBox.SetLockState(ELockState.PermanentlyLocked);
					return;
				}
				return;
			}
			ConsumeKeyItems();
			rewardBox.SetLockState(ELockState.Closed);
			if (0 < tRewardBox.KeySuccessHitBoxId)
			{
				SpawnRewardBoxHitbox(rewardBox, isSuccess: true);
				return;
			}
		}
		else
		{
			if (!(interactiveObject is Entrance entrance))
			{
				return;
			}
			if (entrance.LockState != ELockState.Locked)
			{
				return;
			}
			CancelInteraction();
			TEntrance tEntrance = TEntrance.Table[entrance.EntranceId];
			if (tEntrance.KeyItemIds.Any((int _) => !message.KeyItemIds.Contains(_)))
			{
				if (0 < tEntrance.KeyFailHitboxId)
				{
					SpawnEntranceHitbox(entrance, isSuccess: false);
				}
				if (!tEntrance.KeyFailRetryAble)
				{
					entrance.SetLockState(ELockState.PermanentlyLocked);
					return;
				}
				return;
			}
			ConsumeKeyItems();
			entrance.SetLockState(ELockState.Closed);
			if (0 < tEntrance.KeySuccessHitBoxId)
			{
				SpawnEntranceHitbox(entrance, isSuccess: true);
			}
		}
		StartInteractionWithDelay();
		void CancelInteraction()
		{
			HandleUnitEvent(new CancelInteractEvent
			{
				NetworkObjectId = interactiveObject.NetworkObjectId
			});
		}
		void ConsumeKeyItems()
		{
			int[] keyItemIds = message.KeyItemIds;
			foreach (int num in keyItemIds)
			{
				PlayerInventory.ConsumeGoods(new GoodsData
				{
					Id = num,
					Type = TItem.Table[num].GoodsType,
					Count = 1L
				});
			}
		}
		void SpawnEntranceHitbox(Entrance entrance, bool isSuccess)
		{
			int hitBoxId = (isSuccess ? TEntrance.Table[entrance.EntranceId].KeySuccessHitBoxId : TEntrance.Table[entrance.EntranceId].KeyFailHitboxId);
			Cysharp.Threading.Tasks.UniTaskExtensions.Forget(_gameObjectManager.SpawnHitBoxAsync(hitBoxId, null, entrance.ToStatsData(), new HitLayerInfo
			{
				HitLayer = 4000
			}, entrance.transform.forward, entrance.transform.position));
		}
		void SpawnRewardBoxHitbox(RewardBox rewardBox, bool isSuccess)
		{
			int hitBoxId = (isSuccess ? TRewardBox.Table[rewardBox.RewardBoxId].KeySuccessHitBoxId : TRewardBox.Table[rewardBox.RewardBoxId].KeyFailHitboxId);
			Cysharp.Threading.Tasks.UniTaskExtensions.Forget(_gameObjectManager.SpawnHitBoxAsync(hitBoxId, null, rewardBox.ToStatsData(), new HitLayerInfo
			{
				HitLayer = 4000
			}, rewardBox.transform.forward, rewardBox.transform.position));
		}
		void StartInteractionWithDelay()
		{
			Cysharp.Threading.Tasks.UniTaskExtensions.Forget(Cysharp.Threading.Tasks.UniTaskExtensions.ContinueWith(Cysharp.Threading.Tasks.UniTask.Delay(TimeSpan.FromSeconds(0.15)), delegate
			{
				HandleUnitEvent(new TryInteractEvent
				{
					NetworkObjectId = interactiveObject.NetworkObjectId
				});
			}));
		}
	}

	private void OnInputMoveAim(ReqInputMoveAim message)
	{
		Vector3 worldAim = Vector3.zero;
		if (GameManager.StageState == EStageState.Intro)
		{
			UnitModel.MoveDir = Vector3.zero;
		}
		else
		{
			Vector3 moveDir = MathUtility.TopDownToIsometric(InputPacking.UnpackForward(message.PackedForward));
			if (UnitModel.HasBuffEffect(EBuffEffectType.Confusion))
			{
				moveDir = new Vector3(0f - moveDir.x, moveDir.y, 0f - moveDir.z);
			}
			UnitModel.MoveDir = moveDir;
		}
		if (InputPacking.TryUnpackAim(message.PackedAim, out worldAim))
		{
			Vector3 position = UnitModel.transform.position;
			UnitModel.DeltaAimPos = new Vector3(worldAim.x - position.x, 0f, worldAim.z - position.z);
		}
	}

	private void OnInputRun(ReqInputRun message)
	{
		if (!message.Run)
		{
			CharacterModel.TryRun = false;
			return;
		}
		float runStaminaCost = UnitModel.RunStaminaCost;
		float deltaTime = Time.deltaTime;
		if (!(UnitModel.Stamina < deltaTime * runStaminaCost))
		{
			CharacterModel.TryRunExpireTime = GameManager.TimeNow + TConst.ToggleRunExpireDelay.FloatValue;
			CharacterModel.TryRun = true;
		}
	}

	private void OnInputSkill(ReqInputSkill message)
	{
		if (GameManager.StageState == EStageState.Intro)
		{
			return;
		}
		if (message.InputEvent == ESkillInputEvent.AttackDown)
		{
			if (!message.ContinuousAttack)
			{
				Cysharp.Threading.Tasks.UniTaskExtensions.Forget(Cysharp.Threading.Tasks.UniTaskExtensions.ContinueWith(Cysharp.Threading.Tasks.UniTask.DelayFrame(1), delegate
				{
					HandleUnitEvent(new SkillInputEvent
					{
						SkillInputType = ESkillInputEvent.AttackHold
					});
				}));
			}
			else
			{
				UnitModel.IsBasicAttackHolding = true;
			}
		}
		else if (message.InputEvent == ESkillInputEvent.AttackUp)
		{
			UnitModel.IsBasicAttackHolding = false;
		}
		HandleUnitEvent(new SkillInputEvent
		{
			SkillInputType = message.InputEvent
		});
	}

	private void UpdateBasicAttackHold()
	{
		if (UnitModel.IsBasicAttackHolding)
		{
			HandleUnitEvent(new SkillInputEvent
			{
				SkillInputType = ESkillInputEvent.AttackHold
			});
		}
	}

	private void OnInputUseItem(ReqInputUseItem message)
	{
		TItem tItem = TItem.Table[message.ItemId];
		if (!((tItem.GoodsType == EGoodsType.Consumable) ? (!GameManager.IsInLounge) : (tItem.GoodsType == EGoodsType.Note)))
		{
			return;
		}
		if (Inventory.GetItemCount(message.ItemId) < 1)
		{
			return;
		}
		if (!(UnitModel.GetRemainConsumableTypeCoolTime(TItem.Table[message.ItemId].ConsumableType) > 0f))
		{
			HandleUnitEvent(new UseItemEvent
			{
				ItemId = message.ItemId,
				SkillInputType = message.InputEvent
			});
		}
	}

	private void OnInputTryInteract(ReqInputTryInteract message)
	{
		HandleUnitEvent(new TryInteractEvent
		{
			NetworkObjectId = message.NetworkObjectId
		});
	}

	private void OnInputCancelInteract(ReqInputCancelInteract message)
	{
		HandleUnitEvent(new CancelInteractEvent
		{
			NetworkObjectId = message.NetworkObjectId
		});
	}

	private void OnFinishInteractionWaiting(ReqFinishInteractionWaiting message)
	{
		CharacterModel.SyncIsWaitingInteractionFinish.Value = false;
	}

	private void OnKillCharacter(ReqKillCharacter message)
	{
		if (CharacterModel.State != EActionState.Dying && CharacterModel.State != EActionState.Dead)
		{
			HandleUnitEvent(new DieEvent
			{
				IsForced = true
			});
		}
	}

	private void OnSpawnDropGoods(ReqSpawnDropGoods message)
	{
		AssertIfNotHost();
		if (message.Goods != null && message.Goods.Any())
		{
			ObjectManager.SpawnDropGoods(message.Goods, message.Position.ToVector3());
		}
	}

	private void OnSpawnMonster(ReqSpawnMonster message)
	{
		AssertIfNotHost();
		MonsterSpawnData spawnData = new MonsterSpawnData
		{
			IsRespawn = false,
			MonsterId = message.MonsterId,
			SpawnPosition = message.Position.ToVector3()
		};
		ObjectManager.TrySpawnMonster(spawnData);
	}

	private void OnSpawnWaveExecutor(ReqSpawnWaveExecutor message)
	{
		AssertIfNotHost();
		ObjectManager.SpawnWaveExecutor(message.WaveExecutorId, message.Position.ToVector3(), message.ExecuteImmediately);
	}

	private void OnSpawnExit(ReqSpawnExit message)
	{
		AssertIfNotHost();
		ObjectManager.SpawnExit(message.ExitId, message.Position.ToVector3());
	}

	private void OnRewardBox(ReqRewardBox message)
	{
		AssertIfNotHost();
		ObjectManager.SpawnRewardBox(message.RewardBoxId, message.Position.ToVector3(), message.Forward.ToVector3());
	}

	private void OnPlayCutscene(ReqPlayCutscene message)
	{
		AssertIfNotHost();
		HandleUnitEvent(new CutsceneStartEvent());
		GameHost.SendToClient(message.SenderId, new ResPlayCutscene
		{
			CutscenePrefab = message.CutscenePrefab,
			Position = message.Position
		});
	}

	private void OnFinishCutscene(ReqFinishCutscene message)
	{
		AssertIfNotHost();
		HandleUnitEvent(new CutsceneFinishEvent());
	}

	private void OnForceEscape(ReqForceEscape message)
	{
		if (CharacterModel.State != EActionState.Dying && CharacterModel.State != EActionState.Dead)
		{
			CharacterModel.State = EActionState.Escaped;
			HandleUnitEvent(new EscapeEvent());
		}
	}

	private void RegisterHostCheatHandlers()
	{
		RegisterHostHandler<ReqCheatApplySoul>(OnCheatApplySoul);
		RegisterHostHandler<ReqCheatReviveCharacter>(OnCheatReviveCharacter);
		RegisterHostHandler<ReqCheatMovePosition>(OnCheatMovePosition);
		RegisterHostHandler<ReqCheatAddGoods>(OnCheatAddGoods);
		RegisterHostHandler<ReqCheatSetGold>(OnCheatSetGold);
		RegisterHostHandler<ReqCheatSetCoin>(OnCheatSetCoin);
		RegisterHostHandler<ReqCheatSetExp>(OnCheatSetExp);
		RegisterHostHandler<ReqCheatApplyCharacterOptions>(OnCheatApplyCharacterOptions);
		RegisterHostHandler<ReqCheatResetSkillCooldown>(OnCheatResetSkillCooldown);
		RegisterHostHandler<ReqCheatSpawnMonster>(OnCheatSpawnMonster);
		RegisterHostHandler<ReqCheatSpawnTrap>(OnCheatSpawnTrap);
		RegisterHostHandler<ReqCheatSpawnWaveExecutor>(OnCheatSpawnWaveExecutor);
		RegisterHostHandler<ReqCheatSkillMonsterAll>(OnCheatSkillMonsterAll);
		RegisterHostHandler<ReqCheatAddParadox>(OnCheatAddParadox);
		RegisterHostHandler<ReqCheatAddRandomParadox>(OnCheatAddRandomParadox);
		RegisterHostHandler<ReqCheatSkillCharge>(OnCheatSkillCharge);
		RegisterHostHandler<ReqCheatLevelUp>(OnCheatLevelUp);
		RegisterHostHandler<ReqCheatUnlockAllTalent>(OnCheatUnlockAllTalent);
	}

	private void OnCheatApplySoul(ReqCheatApplySoul message)
	{
		if (CharacterModel.Hp <= 0)
		{
			return;
		}
		if (message.Soul < 0)
		{
			HandleUnitEvent(new SoulDamageEvent
			{
				Amount = Math.Abs(message.Soul)
			});
		}
		else if (message.Soul > 0)
		{
			HandleUnitEvent(new SoulRecoveryEvent
			{
				Amount = message.Soul
			});
		}
		else
		{
			_logger.Debug("soul is zero, no changes made.");
		}
	}

	private void OnCheatReviveCharacter(ReqCheatReviveCharacter message)
	{
		if (CharacterModel.State == EActionState.Dead)
		{
			CharacterModel.State = EActionState.Idle;
			CharacterModel.Hp = (int)CharacterModel.HpMax;
		}
	}

	private void OnCheatMovePosition(ReqCheatMovePosition message)
	{
		CharacterView.MoveTo(message.Position.ToVector3());
	}

	private void OnCheatAddGoods(ReqCheatAddGoods message)
	{
		if (!Inventory.IsSlotFull())
		{
			Inventory.TryGainGoods(message.Goods);
		}
	}

	private void OnCheatSetGold(ReqCheatSetGold message)
	{
		if (message.Gold >= 0)
		{
			Profile profile = Player.Profile.Clone();
			profile.Gold = message.Gold;
			Player.Profile = profile;
		}
	}

	private void OnCheatSetCoin(ReqCheatSetCoin message)
	{
		if (message.Coin < 0)
		{
			return;
		}
		Profile profile = Player?.Profile?.Clone();
		if (profile != null)
		{
			profile.Coin = message.Coin;
			Player.Profile = profile;
		}
	}

	private void OnCheatSetExp(ReqCheatSetExp message)
	{
		if (message.Exp < 0)
		{
			return;
		}
		Profile profile = Player?.Profile?.Clone();
		if (profile != null)
		{
			profile.Exp = message.Exp;
			Player.Profile = profile;
		}
	}

	private void OnCheatApplyCharacterOptions(ReqCheatApplyCharacterOptions message)
	{
		if (message.Invincibility && !CharacterModel.HasBuff(1000001))
		{
			CharacterModel.AddBuff(1000001, CharacterModel, 1, 0uL, Vector3.zero);
		}
		if (!message.Invincibility && CharacterModel.HasBuff(1000001))
		{
			CharacterModel.RemoveBuff(1000001, 1, false);
		}
		if (message.Invisible && !CharacterModel.HasBuff(1000002))
		{
			CharacterModel.AddBuff(1000002, CharacterModel, 1, 0uL, Vector3.zero);
		}
		if (!message.Invisible && CharacterModel.HasBuff(1000002))
		{
			CharacterModel.RemoveBuff(1000002, 1, false);
		}
		if (message.SpeedUp && !CharacterModel.HasBuff(1000003))
		{
			CharacterModel.AddBuff(1000003, CharacterModel, 1, 0uL, Vector3.zero);
		}
		if (!message.SpeedUp && CharacterModel.HasBuff(1000003))
		{
			CharacterModel.RemoveBuff(1000003, 1, false);
		}
	}

	private void OnCheatResetSkillCooldown(ReqCheatResetSkillCooldown message)
	{
		if (UnitModel.Skills == null || !UnitModel.Skills.Any())
		{
			return;
		}
		UnitModel.Skills.Values.Select((UnitSkill _) => _.Clone()).ToList().ForEach(delegate(UnitSkill _)
		{
			_.CoolEndTime = 0f;
			_.Stack = TSkill.Table[_.Id].StackCount;
			UnitModel.Skills[_.Id] = _;
		});
		UnitModel.ClearConsumableTypeCoolTimes();
	}

	private void OnCheatSkillCharge(ReqCheatSkillCharge message)
	{
		if (UnitModel.Skills == null || UnitModel.Skills.Count == 0)
		{
			return;
		}
		foreach (int skillId in UnitModel.Skills.Keys.ToList())
		{
			if (TSkill.Table.TryGetValue(skillId, out var value) && value.ChargeCost > 0)
			{
				float skillChargeAmount = UnitModel.GetSkillChargeAmount(skillId);
				if (!(skillChargeAmount < 0f) && (float)value.ChargeCost > skillChargeAmount)
				{
					UnitModel.AddSkillCharge(skillId, (float)value.ChargeCost - skillChargeAmount);
				}
			}
		}
	}

	private void OnCheatLevelUp(ReqCheatLevelUp message)
	{
		Profile profile = Player.Profile.Clone();
		int num = profile.Level + 1;
		if (num <= TConst.LevelMax.IntValue)
		{
			profile.Level = num;
			Player.Profile = profile;
		}
	}

	private void OnCheatUnlockAllTalent(ReqCheatUnlockAllTalent message)
	{
		TTalent.Table.Keys.ForEach(delegate(int _)
		{
			Player.UnlockTalent(_);
		});
	}

	private void OnCheatSpawnMonster(ReqCheatSpawnMonster message)
	{
		if (!TMonster.Table.ContainsKey(message.MonsterId))
		{
			return;
		}
		Vector3 position = CharacterModel.Position;
		Vector3 normalized = CharacterModel.Forward.normalized;
		for (int i = 0; i < message.MonsterCount; i++)
		{
			float randomFloat = RandomUtility.GetRandomFloat(-1f, 1f);
			float randomFloat2 = RandomUtility.GetRandomFloat(-1f, 1f);
			if (NavMesh.SamplePosition(new Vector3(randomFloat + position.x + normalized.x * 5f, position.y + normalized.y * 5f, randomFloat2 + position.z + normalized.z * 5f), out var hit, 3f, 1))
			{
				MonsterSpawnData spawnData = new MonsterSpawnData
				{
					MonsterId = message.MonsterId,
					SpawnPosition = hit.position
				};
				ObjectManager.TrySpawnMonster(spawnData);
			}
		}
	}

	private void OnCheatSpawnTrap(ReqCheatSpawnTrap message)
	{
		if (!TTrap.Table.ContainsKey(message.TrapId))
		{
			return;
		}
		Vector3 position = CharacterModel.Position;
		Vector3 normalized = CharacterModel.Forward.normalized;
		ObjectManager.SpawnTrap(message.TrapId, position + normalized * 1.5f, message.Forward.ToVector3());
	}

	private void OnCheatSpawnWaveExecutor(ReqCheatSpawnWaveExecutor message)
	{
		if (!TWave.Table.ContainsKey(message.WaveExecutorId))
		{
			return;
		}
		Vector3 position = CharacterModel.Position;
		Vector3 normalized = CharacterModel.Forward.normalized;
		WaveExecutor waveExecutor = ObjectManager.SpawnWaveExecutor(message.WaveExecutorId, position + normalized * 1.5f);
		if (waveExecutor != null)
		{
			waveExecutor.SetTriggerable(canTriggered: true);
		}
	}

	private void OnCheatSkillMonsterAll(ReqCheatSkillMonsterAll message)
	{
		ObjectManager.UnitModels.Where((UnitModel _) => _ is MonsterModel).Where((UnitModel _) => _.State != EActionState.Dying && _.State != EActionState.Dead).ToList().ForEach(delegate(UnitModel _)
		{
			_.GetComponent<UnitController>().HandleUnitEvent(new DieEvent());
		});
	}

	private void OnCheatAddParadox(ReqCheatAddParadox message)
	{
		if (TParadox.Table.ContainsKey(message.ParadoxId))
		{
			_gameContext.Players[message.SenderId].AcquireParadox(message.ParadoxId);
		}
	}

	private void OnCheatAddRandomParadox(ReqCheatAddRandomParadox message)
	{
		_gameContext.Players[message.SenderId].AcquireRandomParadox(includePositive: true, includeNegative: false);
		_gameContext.Players[message.SenderId].AcquireRandomParadox(includePositive: false, includeNegative: true);
	}

	private void RegisterHostEtcHandlers()
	{
		RegisterHostHandler<ReqVoteCampaign>(OnVoteCampaign);
		RegisterHostHandler<ReqAcquireParadox>(OnAcquireParadox);
		RegisterHostHandler<ReqRemoveParadox>(OnRemoveParadox);
		RegisterHostHandler<ReqUpdateCampaignState>(OnUpdateCampaignState);
		RegisterHostHandler<ReqTriggerLuaEvent>(OnTriggerLuaEvent);
		RegisterHostHandler<ReqAddLuaBuff>(OnAddLuaBuff);
		RegisterHostHandler<ReqPing>(OnPing);
		RegisterHostHandler<ReqEmoji>(OnEmoji);
		RegisterHostHandler<ReqSetCharacterHp>(OnSetCharacterHp);
		RegisterHostHandler<ReqSetCharacterStress>(OnSetCharacterStress);
		RegisterHostHandler<ReqSetCharacterLightFuel>(OnSetCharacterLightFuel);
		RegisterHostHandler<ReqSetCharacterCorruption>(OnSetCharacterCorruption);
		RegisterHostHandler<ReqSetCharacterPosition>(OnSetCharacterPosition);
		RegisterHostHandler<ReqSetCharacterForward>(OnSetCharacterForward);
		RegisterHostHandler<ReqVoteGiveUpCampaign>(OnVoteGiveUpCampaign);
		RegisterHostHandler<ReqAddPointMarker>(OnAddPointMarker);
		RegisterHostHandler<ReqRemovePointMarker>(OnRemovePointMarker);
	}

	private void OnVoteCampaign(ReqVoteCampaign message)
	{
		if (!_campaignManager.IsVotingCampaign)
		{
			return;
		}
		if (!_campaignManager.HasVoted(message.SenderId))
		{
			GameHost.SendToAll(new NtfVoteCampaign
			{
				ClientId = message.SenderId,
				IsAgreed = message.IsAgreed
			});
		}
	}

	private void OnAcquireParadox(ReqAcquireParadox message)
	{
		GamePlayer gamePlayer = GameContext.Players[message.SenderId];
		if (message.ParadoxId > 0)
		{
			gamePlayer.AcquireParadox(message.ParadoxId);
		}
		else
		{
			gamePlayer.AcquireRandomParadox(message.IncludePositive, message.IncludeNegative);
		}
	}

	private void OnRemoveParadox(ReqRemoveParadox message)
	{
		GamePlayer gamePlayer = GameContext.Players[message.SenderId];
		if (message.ParadoxId > 0)
		{
			gamePlayer.RemoveParadox(message.ParadoxId);
		}
		else
		{
			gamePlayer.RemoveRandomParadox(message.IncludePositive, message.IncludeNegative);
		}
	}

	private void OnUpdateCampaignState(ReqUpdateCampaignState message)
	{
		GameHost.SendToAll(new NtfUpdateCampaignState
		{
			ClientId = message.SenderId,
			State = message.State
		});
	}

	private void OnTriggerLuaEvent(ReqTriggerLuaEvent message)
	{
		GameHost.SendToAll(new NtfTriggerLuaEvent
		{
			Value = message.Value
		});
	}

	private void OnAddLuaBuff(ReqAddLuaBuff message)
	{
		UnitModel.AddBuff(message.BuffId, UnitModel, 1, 0uL, Vector3.zero);
	}

	private void OnPing(ReqPing message)
	{
		GameHost.SendToAll(new NtfPing
		{
			ClientId = message.SenderId,
			PingType = message.PingType,
			Position = message.Position,
			TargetType = message.TargetType,
			Value1 = message.Value1,
			Value2 = message.Value2
		});
	}

	private void OnEmoji(ReqEmoji message)
	{
		GameHost.SendToAll(new NtfEmoji
		{
			NetworkObjectId = base.NetworkObjectId,
			EmojiId = message.EmojiId
		});
	}

	private void OnSetCharacterHp(ReqSetCharacterHp message)
	{
		float num = message.Hp;
		float hpMax = UnitModel.HpMax;
		if (num < 0f)
		{
			num = 0f;
		}
		else if (hpMax < num)
		{
			num = hpMax;
		}
		int hp = UnitModel.Hp;
		if (Mathf.Approximately(hp, num))
		{
			return;
		}
		if ((float)hp < num)
		{
			HandleUnitEvent(new HpRecoveryEvent
			{
				Amount = (int)Math.Ceiling(num - (float)hp),
				ReasonType = EHpRecoveryReasonType.None,
				SourceModel = UnitModel
			});
		}
		else
		{
			HandleUnitEvent(new HpDamageEvent
			{
				Amount = (int)Math.Ceiling((float)hp - num),
				SourceModel = UnitModel,
				SourceIdentity = UnitIdentity.From(UnitModel),
				HitPosition = UnitModel.Position,
				ReasonType = EHpDamageReasonType.None,
				IsFixedDamage = false
			});
		}
	}

	private void OnSetCharacterStress(ReqSetCharacterStress message)
	{
		int stressMax = StressMax;
		float num = message.Stress;
		if (num < 0f)
		{
			num = 0f;
		}
		else if ((float)stressMax < num)
		{
			num = stressMax;
		}
		float stress = UnitModel.Stress;
		if (num != stress)
		{
			if (num > stress)
			{
				HandleUnitEvent(new StressDamageEvent
				{
					Amount = num - stress,
					SourceModel = UnitModel,
					HitPosition = UnitModel.Position,
					ReasonType = EStressReasonType.None
				});
			}
			else
			{
				HandleUnitEvent(new StressRecoveryEvent
				{
					IsFixed = false,
					Amount = stress - num
				});
			}
		}
	}

	private void OnSetCharacterCorruption(ReqSetCharacterCorruption message)
	{
		CharacterModel.Corruption = message.Corruption;
	}

	private void OnSetCharacterLightFuel(ReqSetCharacterLightFuel message)
	{
		int num = message.LightFuel;
		if (num < 0)
		{
			num = 0;
		}
		else if (num > TConst.LightFuelMax.IntValue)
		{
			num = TConst.LightFuelMax.IntValue;
		}
		int lightFuel = CharacterModel.LightFuel;
		if (num != lightFuel)
		{
			if (num < lightFuel)
			{
				HandleUnitEvent(new LightFuelDamageEvent
				{
					Amount = lightFuel - num,
					HitPosition = UnitModel.Position
				});
			}
			else
			{
				HandleUnitEvent(new LightFuelRecoveryEvent
				{
					Amount = num - lightFuel
				});
			}
		}
	}

	private void OnSetCharacterPosition(ReqSetCharacterPosition message)
	{
		CharacterView.MoveTo(message.Position.ToVector3());
	}

	private void OnSetCharacterForward(ReqSetCharacterForward message)
	{
		UnitModel.Forward = message.Forward.ToVector3();
	}

	private void OnVoteGiveUpCampaign(ReqVoteGiveUpCampaign message)
	{
		if (!_campaignManager.IsVotingCampaignGiveUp)
		{
			return;
		}
		if (!_campaignManager.HasVotedGiveUp(message.SenderId))
		{
			GameHost.SendToAll(new NtfVoteGiveUpCampaign
			{
				ClientId = message.SenderId,
				IsAgreed = message.IsAgreed
			});
		}
	}

	private void OnAddPointMarker(ReqAddPointMarker message)
	{
		PointMarkers pointMarkers = Player.PointMarkers;
		List<ProtoVector3> list = pointMarkers?.Positions?.ToList() ?? new List<ProtoVector3>();
		if (list.Count >= 9)
		{
			list.RemoveAt(0);
		}
		list.Add(message.Position);
		Player.PointMarkers = new PointMarkers
		{
			Positions = list.ToArray()
		};
	}

	private void OnRemovePointMarker(ReqRemovePointMarker message)
	{
		PointMarkers pointMarkers = Player.PointMarkers;
		List<ProtoVector3> list = pointMarkers?.Positions?.ToList() ?? new List<ProtoVector3>();
		if (message.MarkerIndex < 0 || message.MarkerIndex >= list.Count)
		{
			return;
		}
		list.RemoveAt(message.MarkerIndex);
		Player.PointMarkers = new PointMarkers
		{
			Positions = list.ToArray()
		};
	}

	private void RegisterHostInventoryHandlers()
	{
		RegisterHostHandler<ReqInventorySlotToSafeSlot>(OnInventorySlotToSafeSlot);
		RegisterHostHandler<ReqInventorySlotToLootingSlot>(OnOnInventorySlotToLootingSlot);
		RegisterHostHandler<ReqInventorySlotToStashSlot>(OnInventorySlotToStashSlot);
		RegisterHostHandler<ReqDropInventorySlot>(OnDropInventorySlot);
		RegisterHostHandler<ReqSwapInventorySlots>(OnSwapInventorySlots);
		RegisterHostHandler<ReqSafeSlotToInventorySlot>(OnSafeSlotToInventorySlot);
		RegisterHostHandler<ReqSafeSlotToLootingSlot>(OnSafeSlotToLootingSlot);
		RegisterHostHandler<ReqSafeSlotToStashSlot>(OnSafeSlotToStashSlot);
		RegisterHostHandler<ReqDropSafeSlot>(OnDropSafeSlot);
		RegisterHostHandler<ReqSwapSafeSlots>(OnSwapSafeSlots);
		RegisterHostHandler<ReqLootingSlotToSafeSlot>(OnLootingSlotToSafeSlot);
		RegisterHostHandler<ReqLootingSlotToInventorySlot>(OnLootingSlotToInventorySlot);
		RegisterHostHandler<ReqSwapLootingSlots>(OnSwapLootingSlots);
		RegisterHostHandler<ReqStashSlotToSafeSlot>(OnStashSlotToSafeSlot);
		RegisterHostHandler<ReqStashSlotToInventorySlot>(OnStashSlotToInventorySlot);
		RegisterHostHandler<ReqMoveAllInventoryToStash>(OnMoveAllInventoryToStash);
		RegisterHostHandler<ReqAnyToBagSlot>(OnAnyToBagSlot);
		RegisterHostHandler<ReqBagSlotToAny>(OnBagSlotToAny);
		RegisterHostHandler<ReqEquipInventorySlot>(OnEquipInventorySlot);
		RegisterHostHandler<ReqEquipSafeSlot>(OnEquipSafeSlot);
		RegisterHostHandler<ReqEquipLootingSlot>(OnEquipLootingSlot);
		RegisterHostHandler<ReqEquipStashSlot>(OnEquipStashSlot);
		RegisterHostHandler<ReqSwapEquipmentSlots>(OnSwapEquipmentSlots);
		RegisterHostHandler<ReqSwapWeapon>(OnSwapWeapon);
		RegisterHostHandler<ReqUnequipToInventorySlot>(OnUnequipToInventorySlot);
		RegisterHostHandler<ReqUnequipToSafeSlot>(OnUnequipToSafeSlot);
		RegisterHostHandler<ReqUnequipToLootingSlot>(OnUnequipToLootingSlot);
		RegisterHostHandler<ReqUnequipToStashSlot>(OnUnequipToStashSlot);
		RegisterHostHandler<ReqGainGoods>(OnGainGoods);
		RegisterHostHandler<ReqConsumeGoods>(OnConsumeGoods);
		RegisterHostHandler<ReqOrganizeInventorySlots>(OnOrganizeInventorySlots);
		RegisterHostHandler<ReqOrganizeInventorySafeSlots>(OnOrganizeInventorySafeSlots);
		RegisterHostHandler<ReqRemoveInventorySlot>(OnRemoveInventorySlot);
		RegisterHostHandler<ReqRemoveSafeSlot>(OnRemoveSafeSlot);
	}

	private void Equip(Goods equipmentGoods, EGoodsSlotCategory slotCategory, int slotIndex, out Goods unequippedGoods)
	{
		Character character = Player.Character.Clone();
		GoodsSlot[] array;
		switch (slotCategory)
		{
			case EGoodsSlotCategory.WeaponSlot:
				if (slotIndex < 0)
				{
					slotIndex = character.ActiveWeaponIndex;
				}
				unequippedGoods = character.WeaponSlots[slotIndex].Goods;
				array = character.WeaponSlots;
				break;
			case EGoodsSlotCategory.AccessorySlot:
				if (slotIndex < 0)
				{
					slotIndex = character.AccessorySlots.Select((GoodsSlot slot, int i) => (slot, i)).FirstOrDefault(((GoodsSlot, int) t) => t.Item1.Goods == null).Item2;
				}
				unequippedGoods = character.AccessorySlots[slotIndex].Goods;
				array = character.AccessorySlots;
				break;
			case EGoodsSlotCategory.ArtifactSlot:
				if (slotIndex < 0)
				{
					slotIndex = character.ArtifactSlots.Select((GoodsSlot slot, int i) => (slot, i)).FirstOrDefault(((GoodsSlot, int) t) => t.Item1.Goods == null).Item2;
				}
				unequippedGoods = character.ArtifactSlots[slotIndex].Goods;
				array = character.ArtifactSlots;
				break;
			default:
				throw new ArgumentOutOfRangeException("slotCategory", slotCategory, null);
		}
		array[slotIndex].Goods = equipmentGoods;
		Player.Character = character;
	}

	public void UnequipByEquipmentId(int equipmentId)
	{
		EGoodsType goodsType = TEquipment.Table[equipmentId].GoodsType;
		Character character = Player.Character.Clone();
		GoodsSlot[] array;
		switch (goodsType)
		{
			case EGoodsType.Weapon:
				array = character.WeaponSlots;
				break;
			case EGoodsType.Accessory:
				array = character.AccessorySlots;
				break;
			case EGoodsType.Artifact:
				array = character.ArtifactSlots;
				break;
			default:
				return;
		}
		GoodsSlot[] array2 = array;
		foreach (GoodsSlot goodsSlot in array2)
		{
			if (goodsSlot.Goods != null && goodsSlot.Goods.Id == equipmentId)
			{
				goodsSlot.Goods = null;
				break;
			}
		}
		Player.Character = character;
	}

	private Goods Unequip(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		Character character = Player.Character.Clone();
		Goods goods;
		if (slotCategory == EGoodsSlotCategory.WeaponSlot)
		{
			if (character.WeaponSlots[slotIndex].Goods == null)
			{
				return null;
			}
			goods = character.WeaponSlots[slotIndex].Goods;
			character.WeaponSlots[slotIndex].Goods = null;
		}
		else
		{
			GoodsSlot[] array;
			switch (slotCategory)
			{
				case EGoodsSlotCategory.AccessorySlot:
					array = character.AccessorySlots;
					break;
				case EGoodsSlotCategory.ArtifactSlot:
					array = character.ArtifactSlots;
					break;
				default:
					throw new ArgumentOutOfRangeException("slotCategory", slotCategory, null);
			}
			if (array[slotIndex].Goods == null)
			{
				return null;
			}
			goods = array[slotIndex].Goods;
			array[slotIndex].Goods = null;
		}
		Player.Character = character;
		return goods;
	}

	private void OnInventorySlotToSafeSlot(ReqInventorySlotToSafeSlot message)
	{
		if (!PlayerInventory.Slots.TryPullSlot(message.InventorySlotIndex, out var pullGoods))
		{
			return;
		}
		if (PlayerInventory.SafeSlots.TryPullSlot(message.SafeSlotIndex, out var pullGoods2))
		{
			PlayerInventory.Slots.TryPushGoods(pullGoods2, message.InventorySlotIndex);
		}
		PlayerInventory.SafeSlots.TryPushGoods(pullGoods, message.SafeSlotIndex);
	}

	private void OnOnInventorySlotToLootingSlot(ReqInventorySlotToLootingSlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (PlayerInventory.Slots.TryGetValue(message.InventorySlotIndex, out var value) && !value.IsEmpty && inventory.CanMoveGoods(value.Goods) && PlayerInventory.Slots.TryPullSlotGoods(message.InventorySlotIndex, message.Count, out var pullGoods))
		{
			inventory.PushGoods(pullGoods, message.LootingSlotIndex);
		}
	}

	private void OnInventorySlotToStashSlot(ReqInventorySlotToStashSlot message)
	{
		if (PlayerInventory.Slots.TryPullSlotGoods(message.InventorySlotIndex, message.Count, out var pullGoods))
		{
			GameHost.SendToClient(message.SenderId, new ResInventorySlotToStashSlot
			{
				IsSuccess = true,
				StashSlotIndex = message.StashSlotIndex,
				PulledGoods = pullGoods
			});
		}
		else
		{
			GameHost.SendToClient(message.SenderId, new ResInventorySlotToStashSlot
			{
				IsSuccess = false
			});
		}
	}

	private void OnMoveAllInventoryToStash(ReqMoveAllInventoryToStash message)
	{
		if (message.InventorySlotIndices == null || message.InventorySlotIndices.Length == 0)
		{
			return;
		}
		List<Goods> list = new List<Goods>();
		int[] inventorySlotIndices = message.InventorySlotIndices;
		foreach (int slotIndex in inventorySlotIndices)
		{
			if (PlayerInventory.Slots.TryPullSlot(slotIndex, out var pullGoods))
			{
				list.Add(pullGoods);
			}
		}
		if (list.Count != 0)
		{
			GameHost.SendToClient(message.SenderId, new ResMoveAllInventoryToStash
			{
				PulledGoods = list.ToArray()
			});
		}
	}

	private void OnDropInventorySlot(ReqDropInventorySlot message)
	{
		if (PlayerInventory.Slots.TryPullSlotGoods(message.InventorySlotIndex, message.Count, out var pullGoods))
		{
			base.ObjectManager.SpawnDropGoods(pullGoods, base.transform.position);
		}
	}

	private void OnSwapInventorySlots(ReqSwapInventorySlots message)
	{
		PlayerInventory.Slots.SwapIndex(message.SourceSlotIndex, message.TargetSlotIndex);
	}

	private void OnSafeSlotToInventorySlot(ReqSafeSlotToInventorySlot message)
	{
		if (!PlayerInventory.SafeSlots.TryPullSlot(message.SafeSlotIndex, out var pullGoods))
		{
			return;
		}
		if (PlayerInventory.Slots.TryPullSlot(message.InventorySlotIndex, out var pullGoods2))
		{
			PlayerInventory.SafeSlots.TryPushGoods(pullGoods2, message.SafeSlotIndex);
		}
		PlayerInventory.Slots.TryPushGoods(pullGoods, message.InventorySlotIndex);
	}

	private void OnSafeSlotToLootingSlot(ReqSafeSlotToLootingSlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (PlayerInventory.SafeSlots.TryGetValue(message.SafeSlotIndex, out var value) && !value.IsEmpty && inventory.CanMoveGoods(value.Goods) && PlayerInventory.SafeSlots.TryPullSlotGoods(message.SafeSlotIndex, message.Count, out var pullGoods))
		{
			inventory.PushGoods(pullGoods, message.LootingSlotIndex);
		}
	}

	private void OnSafeSlotToStashSlot(ReqSafeSlotToStashSlot message)
	{
		if (PlayerInventory.SafeSlots.TryPullSlotGoods(message.SafeSlotIndex, message.Count, out var pullGoods))
		{
			GameHost.SendToClient(message.SenderId, new ResSafeSlotToStashSlot
			{
				IsSuccess = true,
				StashSlotIndex = message.StashSlotIndex,
				PulledGoods = pullGoods
			});
		}
		else
		{
			GameHost.SendToClient(message.SenderId, new ResSafeSlotToStashSlot
			{
				IsSuccess = false
			});
		}
	}

	private void OnDropSafeSlot(ReqDropSafeSlot message)
	{
		if (PlayerInventory.SafeSlots.TryPullSlotGoods(message.SafeSlotIndex, message.Count, out var pullGoods))
		{
			base.ObjectManager.SpawnDropGoods(pullGoods, base.transform.position);
		}
	}

	private void OnSwapSafeSlots(ReqSwapSafeSlots message)
	{
		PlayerInventory.SafeSlots.SwapIndex(message.SourceSlotIndex, message.TargetSlotIndex);
	}

	private void OnLootingSlotToSafeSlot(ReqLootingSlotToSafeSlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (!inventory.Slots.TryGetValue(message.LootingSlotIndex, out var value) || !PlayerInventory.CanAddGoodsToSafeSlot(value.Goods))
		{
			return;
		}
		if (value.Category == EGoodsCategory.Equipment)
		{
			if (inventory.Slots.TryPullSlot(message.LootingSlotIndex, out var pullGoods))
			{
				PlayerInventory.PushGoodsToSafeSlot(pullGoods, message.SafeSlotIndex);
			}
		}
		else if (value.Category == EGoodsCategory.Bag)
		{
			if (inventory.Slots.TryPullSlot(message.LootingSlotIndex, out var pullGoods2))
			{
				PlayerInventory.PushGoodsToSafeSlot(pullGoods2, message.SafeSlotIndex);
			}
		}
		else if (value.Category == EGoodsCategory.Item)
		{
			long num = ((message.SafeSlotIndex < 0) ? PlayerInventory.GetAddableItemCountWithBags(value.Id) : PlayerInventory.GetAddableItemCount(value.Id));
			long num2 = Math.Min(message.Count, num);
			if (num2 <= 0)
			{
				PlayerInventory.PushGoodsToSafeSlot(GoodsCreator.MakeItemGoods(value.Id), message.SafeSlotIndex);
			}
			else if (inventory.Slots.TryPullSlotGoods(message.LootingSlotIndex, num2, out var pullGoods3))
			{
				PlayerInventory.PushGoodsToSafeSlot(pullGoods3, message.SafeSlotIndex);
			}
		}
	}

	private void OnLootingSlotToInventorySlot(ReqLootingSlotToInventorySlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (!inventory.Slots.TryGetValue(message.LootingSlotIndex, out var value) || !PlayerInventory.CanMoveGoods(value.Goods))
		{
			return;
		}
		if (value.Category == EGoodsCategory.Equipment)
		{
			if (inventory.Slots.TryPullSlot(message.LootingSlotIndex, out var pullGoods))
			{
				PlayerInventory.PushGoods(pullGoods, message.InventorySlotIndex);
			}
		}
		else if (value.Category == EGoodsCategory.Bag)
		{
			if (inventory.Slots.TryPullSlot(message.LootingSlotIndex, out var pullGoods2))
			{
				PlayerInventory.PushGoods(pullGoods2, message.InventorySlotIndex);
			}
		}
		else if (value.Category == EGoodsCategory.Item)
		{
			long num = ((message.InventorySlotIndex < 0) ? PlayerInventory.GetAddableItemCountWithBags(value.Id) : PlayerInventory.GetAddableItemCount(value.Id));
			long num2 = Math.Min(message.Count, num);
			if (num2 <= 0)
			{
				PlayerInventory.PushGoods(GoodsCreator.MakeItemGoods(value.Id), message.InventorySlotIndex);
			}
			else if (inventory.Slots.TryPullSlotGoods(message.LootingSlotIndex, num2, out var pullGoods3))
			{
				PlayerInventory.PushGoods(pullGoods3, message.InventorySlotIndex);
			}
		}
	}

	private void OnSwapLootingSlots(ReqSwapLootingSlots message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (!(inventory == null))
		{
			inventory.Slots.SwapIndex(message.SourceSlotIndex, message.TargetSlotIndex);
		}
	}

	private void OnStashSlotToSafeSlot(ReqStashSlotToSafeSlot message)
	{
		PlayerInventory.PushGoodsToSafeSlot(message.Goods, message.SafeSlotIndex);
	}

	private void OnStashSlotToInventorySlot(ReqStashSlotToInventorySlot message)
	{
		PlayerInventory.PushGoods(message.Goods, message.InventorySlotIndex);
	}

	private void OnEquipInventorySlot(ReqEquipInventorySlot message)
	{
		if (PlayerInventory.Slots.TryGetValue(message.InventorySlotIndex, out var value) && message.EquipmentSlotCategory == value.Type.GetSlotCategory() && PlayerInventory.Slots.TryPullSlot(message.InventorySlotIndex, out var pullGoods))
		{
			Equip(pullGoods, message.EquipmentSlotCategory, message.EquipmentSlotIndex, out var unequippedGoods);
			if (unequippedGoods != null)
			{
				PlayerInventory.PushGoods(unequippedGoods, message.InventorySlotIndex);
			}
		}
	}

	private void OnEquipSafeSlot(ReqEquipSafeSlot message)
	{
		if (PlayerInventory.SafeSlots.TryGetValue(message.SafeSlotIndex, out var value) && message.EquipmentSlotCategory == value.Type.GetSlotCategory() && PlayerInventory.SafeSlots.TryPullSlot(message.SafeSlotIndex, out var pullGoods))
		{
			Equip(pullGoods, message.EquipmentSlotCategory, message.EquipmentSlotIndex, out var unequippedGoods);
			if (unequippedGoods != null)
			{
				PlayerInventory.PushGoodsToSafeSlot(unequippedGoods, message.SafeSlotIndex);
			}
		}
	}

	private void OnEquipLootingSlot(ReqEquipLootingSlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (inventory.Slots.TryGetValue(message.LootingSlotIndex, out var value) && message.EquipmentSlotCategory == value.Type.GetSlotCategory() && inventory.Slots.TryPullSlot(message.LootingSlotIndex, out var pullGoods))
		{
			Equip(pullGoods, message.EquipmentSlotCategory, message.EquipmentSlotIndex, out var unequippedGoods);
			if (unequippedGoods != null)
			{
				inventory.PushGoods(unequippedGoods, message.LootingSlotIndex);
			}
			GameHost.SendToClient(message.SenderId, new ResEquipLootingSlot
			{
				EquippedGoods = pullGoods
			});
		}
	}

	private void OnEquipStashSlot(ReqEquipStashSlot message)
	{
		if (message.EquipmentSlotCategory == message.EquipmentGoods.Type.GetSlotCategory())
		{
			Equip(message.EquipmentGoods, message.EquipmentSlotCategory, message.EquipmentSlotIndex, out var unequippedGoods);
			GameHost.SendToClient(message.SenderId, new ResEquipStashSlot
			{
				IsSuccess = true,
				StashSlotIndex = message.StashSlotIndex,
				UnequippedGoods = unequippedGoods
			});
		}
		else
		{
			GameChannel.SendToHost(new ResEquipStashSlot
			{
				IsSuccess = false
			});
		}
	}

	private void OnUnequipToInventorySlot(ReqUnequipToInventorySlot message)
	{
		if (!PlayerInventory.IsSlotFull())
		{
			Goods goods = Unequip(message.EquipmentSlotCategory, message.EquipmentSlotIndex);
			if (goods != null)
			{
				PlayerInventory.PushGoods(goods, message.InventorySlotIndex);
			}
		}
	}

	private void OnUnequipToSafeSlot(ReqUnequipToSafeSlot message)
	{
		if (!PlayerInventory.IsSafeSlotFull())
		{
			Goods goods = Unequip(message.EquipmentSlotCategory, message.EquipmentSlotIndex);
			if (goods != null)
			{
				PlayerInventory.PushGoodsToSafeSlot(goods, message.SafeSlotIndex);
			}
		}
	}

	private void OnUnequipToLootingSlot(ReqUnequipToLootingSlot message)
	{
		Inventory inventory = _gameObjectManager.FindInventory(message.LootingNetworkObjectId, message.SenderId);
		if (inventory == null)
		{
			return;
		}
		if (!inventory.IsSlotFull())
		{
			Goods goods = Unequip(message.EquipmentSlotCategory, message.EquipmentSlotIndex);
			if (goods != null)
			{
				inventory.PushGoods(goods, message.LootingSlotIndex);
			}
		}
	}

	private void OnUnequipToStashSlot(ReqUnequipToStashSlot message)
	{
		Goods unequippedGoods = Unequip(message.EquipmentSlotCategory, message.EquipmentSlotIndex);
		GameHost.SendToClient(message.SenderId, new ResUnequipToStashSlot
		{
			StashSlotIndex = message.StashSlotIndex,
			UnequippedGoods = unequippedGoods
		});
	}

	private void OnSwapEquipmentSlots(ReqSwapEquipmentSlots message)
	{
		Character character = Player.Character.Clone();
		GoodsSlot[] array;
		Goods goods;
		if (message.EquipmentSlotCategory == EGoodsSlotCategory.AccessorySlot)
		{
			goods = character.AccessorySlots[message.SourceSlotIndex].Goods;
			character.AccessorySlots[message.SourceSlotIndex].Goods = character.AccessorySlots[message.TargetSlotIndex].Goods;
			array = character.AccessorySlots;
		}
		else
		{
			if (message.EquipmentSlotCategory != EGoodsSlotCategory.ArtifactSlot)
			{
				return;
			}
			goods = character.ArtifactSlots[message.SourceSlotIndex].Goods;
			character.ArtifactSlots[message.SourceSlotIndex].Goods = character.ArtifactSlots[message.TargetSlotIndex].Goods;
			array = character.ArtifactSlots;
		}
		array[message.TargetSlotIndex].Goods = goods;
		Player.Character = character;
	}

	private void OnSwapWeapon(ReqSwapWeapon message)
	{
		Character character = Player.Character.Clone();
		int num = ((character.ActiveWeaponIndex == 0) ? 1 : 0);
		if (!message.AllowEmpty && character.WeaponSlots[num].Goods == null)
		{
			return;
		}
		character.ActiveWeaponIndex = num;
		Player.Character = character;
	}

	private void OnOrganizeInventorySlots(ReqOrganizeInventorySlots message)
	{
		PlayerInventory.Slots.Organize();
	}

	private void OnOrganizeInventorySafeSlots(ReqOrganizeInventorySafeSlots message)
	{
		PlayerInventory.SafeSlots.Organize();
	}

	private void OnGainGoods(ReqGainGoods message)
	{
		foreach (Goods item in message.Goods.Where((Goods _) => _.IsCurrency()))
		{
			if (item is GoldGoods)
			{
				GainGold(item.Count);
			}
			else if (item is CoinGoods)
			{
				GainCoin(item.Count);
			}
			else if (item is ExpGoods)
			{
				GainExp(item.Count);
			}
		}
		Goods[] array = message.Goods.Where((Goods _) => !_.IsCurrency()).ToArray();
		if (array.Any())
		{
			if (!PlayerInventory.TryGainGoods(array))
			{
				GameHost.SendToClient(message.SenderId, new NtfUnitMonologueText
				{
					NetworkObjectId = base.NetworkObjectId,
					TextKey = TLocalizedText.InventoryFullDrop.Id
				});
			}
		}
	}

	private void OnConsumeGoods(ReqConsumeGoods message)
	{
		foreach (Goods item in message.Goods.Where((Goods _) => _.IsCurrency() && _.Count > 0))
		{
			TryConsumeGoods(item, allowStashFallback: false, out var consumeGold, out var consumeCoin, out var remainGoods);
		}
		IEnumerable<Goods> consumeGoods = message.Goods.Where((Goods _) => !_.IsCurrency()).ToArray();
		if (!consumeGoods.Any())
		{
			return;
		}
		Func<int, long> emptyCountByBagId = PlayerInventory.GetEmptyBagCount;
		Func<int, long> totalCountByBagId = PlayerInventory.GetBagCount;
		EBagConsumeFailReason eBagConsumeFailReason = BagConsumeRule.Check(consumeGoods, emptyCountByBagId, totalCountByBagId);
		if (eBagConsumeFailReason == EBagConsumeFailReason.None)
		{
			GoodsData[] goodsDatas = consumeGoods.Select((Goods _) => _.ToData()).ToArray();
			PlayerInventory.ConsumeGoods(goodsDatas);
		}
		else
		{
			string failMessage = BagConsumeRule.GetFailMessage(eBagConsumeFailReason);
			GameHost.SendNoticeToast(failMessage, message.SenderId, 24L);
		}
	}

	private void OnRemoveInventorySlot(ReqRemoveInventorySlot message)
	{
		if (!PlayerInventory.Slots.TryPullSlot(message.SlotIndex, out var _))
		{
			_logger.Error($"Failed to remove inventory slot {message.SlotIndex} for sender {message.SenderId}.");
		}
	}

	private void OnRemoveSafeSlot(ReqRemoveSafeSlot message)
	{
		if (!PlayerInventory.SafeSlots.TryPullSlot(message.SlotIndex, out var _))
		{
			_logger.Error($"Failed to remove safe slot {message.SlotIndex} for sender {message.SenderId}.");
		}
	}

	private void OnAnyToBagSlot(ReqAnyToBagSlot message)
	{
		if (message.SourceCategory.IsEquipmentCategory())
		{
			HandleEquipmentToBag(message);
			return;
		}
		if (message.SourceCategory == EGoodsSlotCategory.StashSlot)
		{
			HandleStashSourceToBag(message);
			return;
		}
		IDictionary<int, GoodsSlot> sourceSlots = GetSourceSlots(message.SourceCategory, message.SourceNetworkObjectId, message.SenderId);
		if (sourceSlots == null || !sourceSlots.TryGetValue(message.SourceSlotIndex, out var value) || value.IsEmpty || value.Category == EGoodsCategory.Bag)
		{
			return;
		}
		if (message.BagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			if (sourceSlots.TryPullSlotGoods(message.SourceSlotIndex, message.Count, out var pullGoods))
			{
				GameHost.SendToClient(message.SenderId, new ResInventorySlotToStashBagSlot
				{
					IsSuccess = true,
					StashBagHolderSlotIndex = message.BagHolderSlotIndex,
					InnerSlotIndex = message.InnerSlotIndex,
					PulledGoods = pullGoods
				});
			}
			return;
		}
		IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
		if (bagHolderSlots != null && sourceSlots.TryPullSlotGoods(message.SourceSlotIndex, message.Count, out var pullGoods2))
		{
			long itemMaxStackCount = ((pullGoods2.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(pullGoods2.Id) : long.MaxValue);
			if (bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, pullGoods2, itemMaxStackCount, message.InnerSlotIndex, out var swappedOutGoods))
			{
				if (swappedOutGoods != null && !sourceSlots.TryPushGoods(swappedOutGoods, message.SourceSlotIndex))
				{
					PlayerInventory.TryGainGoods(swappedOutGoods);
				}
			}
			else
			{
				sourceSlots.TryPushGoods(pullGoods2, message.SourceSlotIndex);
			}
		}
	}

	private void HandleEquipmentToBag(ReqAnyToBagSlot message)
	{
		EGoodsSlotCategory sourceCategory = message.SourceCategory;
		int sourceSlotIndex = message.SourceSlotIndex;
		Character character = Player.Character;
		GoodsSlot[] array;
		switch (sourceCategory)
		{
			case EGoodsSlotCategory.WeaponSlot:
				if (sourceSlotIndex < 0)
				{
					return;
				}
				array = character.WeaponSlots;
				break;
			case EGoodsSlotCategory.AccessorySlot:
				if (sourceSlotIndex < 0)
				{
					return;
				}
				array = character.AccessorySlots;
				break;
			case EGoodsSlotCategory.ArtifactSlot:
				if (sourceSlotIndex < 0)
				{
					return;
				}
				array = character.ArtifactSlots;
				break;
			default:
				return;
		}
		if (sourceSlotIndex >= array.Length || array[sourceSlotIndex]?.Goods == null)
		{
			return;
		}
		if (message.BagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			Goods goods = Unequip(message.SourceCategory, message.SourceSlotIndex);
			if (goods != null)
			{
				GameHost.SendToClient(message.SenderId, new ResInventorySlotToStashBagSlot
				{
					IsSuccess = true,
					StashBagHolderSlotIndex = message.BagHolderSlotIndex,
					InnerSlotIndex = message.InnerSlotIndex,
					PulledGoods = goods
				});
			}
			return;
		}
		IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
		if (bagHolderSlots == null)
		{
			return;
		}
		Goods goods2 = Unequip(message.SourceCategory, message.SourceSlotIndex);
		if (goods2 == null)
		{
			return;
		}
		long itemMaxStackCount = ((goods2.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(goods2.Id) : long.MaxValue);
		if (bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, goods2, itemMaxStackCount, message.InnerSlotIndex, out var swappedOutGoods))
		{
			if (swappedOutGoods == null)
			{
				return;
			}
			if (swappedOutGoods.Type.GetSlotCategory() != message.SourceCategory)
			{
				if (!PlayerInventory.Slots.TryPushGoods(swappedOutGoods, -1))
				{
					PlayerInventory.TryGainGoods(swappedOutGoods);
				}
				return;
			}
			goods2 = swappedOutGoods;
		}
		Equip(goods2, message.SourceCategory, message.SourceSlotIndex, out var unequippedGoods);
	}

	private void HandleStashSourceToBag(ReqAnyToBagSlot message)
	{
		Goods sourceGoods = message.SourceGoods;
		if (sourceGoods == null || sourceGoods.Count < 1 || sourceGoods.Category == EGoodsCategory.Bag)
		{
			return;
		}
		if (message.BagHolderCategory != EGoodsSlotCategory.StashSlot)
		{
			IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
			if (bagHolderSlots != null)
			{
				long itemMaxStackCount = ((sourceGoods.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(sourceGoods.Id) : long.MaxValue);
				if (bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, sourceGoods, itemMaxStackCount, message.InnerSlotIndex, out var swappedOutGoods))
				{
					if (swappedOutGoods == null)
					{
						return;
					}
					sourceGoods = swappedOutGoods;
				}
			}
		}
		PlayerInventory.TryGainGoods(sourceGoods);
	}

	private Goods GetEquippedGoods(EGoodsSlotCategory category, int slotIndex)
	{
		Character character = Player.Character;
		GoodsSlot[] array;
		switch (category)
		{
			case EGoodsSlotCategory.WeaponSlot:
				if (slotIndex < 0)
				{
					return null;
				}
				array = character.WeaponSlots;
				break;
			case EGoodsSlotCategory.AccessorySlot:
				if (slotIndex < 0)
				{
					return null;
				}
				array = character.AccessorySlots;
				break;
			case EGoodsSlotCategory.ArtifactSlot:
				if (slotIndex < 0)
				{
					return null;
				}
				array = character.ArtifactSlots;
				break;
			default:
				return null;
		}
		if (slotIndex >= array.Length)
		{
			return null;
		}
		return array[slotIndex].Goods;
	}

	private IDictionary<int, GoodsSlot> GetSourceSlots(EGoodsSlotCategory category, ulong networkObjectId, ulong senderId)
	{
		switch (category)
		{
			case EGoodsSlotCategory.InventorySlot:
				return PlayerInventory.Slots;
			case EGoodsSlotCategory.SafeSlot:
				return PlayerInventory.SafeSlots;
			case EGoodsSlotCategory.LootingSlot:
			{
				Inventory inventory = _gameObjectManager.FindInventory(networkObjectId, senderId);
				if (inventory != null)
				{
					return inventory.Slots;
				}
				return null;
			}
			default:
				return null;
		}
	}

	private void OnBagSlotToAny(ReqBagSlotToAny message)
	{
		if (message.BagHolderCategory == EGoodsSlotCategory.StashSlot)
		{
			return;
		}
		IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
		if (bagHolderSlots == null)
		{
			return;
		}
		if (message.TargetCategory == EGoodsSlotCategory.BagSlot)
		{
			long itemMaxStackCount = ResolveBagInnerItemMaxStack(bagHolderSlots, message.BagHolderSlotIndex, message.InnerSlotIndex);
			bagHolderSlots.TrySwapInnerSlots(message.BagHolderSlotIndex, message.InnerSlotIndex, message.TargetSlotIndex, itemMaxStackCount);
			return;
		}
		if (!bagHolderSlots.TryPullFromBag(message.BagHolderSlotIndex, message.InnerSlotIndex, message.Count, out var pullGoods))
		{
			return;
		}
		if (message.TargetCategory == EGoodsSlotCategory.StashSlot)
		{
			GameHost.SendToClient(message.SenderId, new ResBagSlotToStash
			{
				IsSuccess = true,
				StashSlotIndex = message.TargetSlotIndex,
				PulledGoods = pullGoods
			});
			return;
		}
		if (!TryPushToTarget(message, pullGoods))
		{
			long itemMaxStackCount2 = ((pullGoods.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(pullGoods.Id) : long.MaxValue);
			bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, pullGoods, itemMaxStackCount2, message.InnerSlotIndex);
		}
	}

	private bool TryPushToTarget(ReqBagSlotToAny message, Goods pulledGoods)
	{
		IDictionary<int, GoodsSlot> targetSlots;
		switch (message.TargetCategory)
		{
			case EGoodsSlotCategory.InventorySlot:
				targetSlots = PlayerInventory.Slots;
				break;
			case EGoodsSlotCategory.SafeSlot:
				targetSlots = PlayerInventory.SafeSlots;
				break;
			case EGoodsSlotCategory.LootingSlot:
			{
				Inventory inventory = _gameObjectManager.FindInventory(message.TargetNetworkObjectId, message.SenderId);
				if (inventory == null)
				{
					return false;
				}
				targetSlots = inventory.Slots;
				break;
			}
			case EGoodsSlotCategory.WeaponSlot:
			case EGoodsSlotCategory.AccessorySlot:
			case EGoodsSlotCategory.ArtifactSlot:
				return TryEquipFromBag(message, pulledGoods);
			default:
				return false;
		}
		return TryPushAndSwap(targetSlots, message, pulledGoods);
	}

	private bool TryPushAndSwap(IDictionary<int, GoodsSlot> targetSlots, ReqBagSlotToAny message, Goods pulledGoods)
	{
		if (message.TargetSlotIndex < 0)
		{
			return targetSlots.TryPushGoods(pulledGoods, -1);
		}
		if (!targetSlots.TryGetValue(message.TargetSlotIndex, out var value) || value.Goods == null)
		{
			return targetSlots.TryPushGoods(pulledGoods, message.TargetSlotIndex);
		}
		Goods goods = value.Goods;
		if (pulledGoods.Category == EGoodsCategory.Item && goods.Category == EGoodsCategory.Item && pulledGoods.Id == goods.Id)
		{
			long num = Player.GetInventoryItemCountMax(pulledGoods.Id);
			long num2 = Math.Max(0L, num - goods.Count);
			if (num2 > 0)
			{
				long num3 = Math.Min(pulledGoods.Count, num2);
				long num4 = Math.Min(num3, pulledGoods.BaseCount);
				long num5 = num3 - num4;
				long baseCount = pulledGoods.BaseCount - num4;
				long gainCount = pulledGoods.GainCount - num5;
				Goods goods2 = GoodsCreator.MakeGoods(pulledGoods.Type, pulledGoods.Id, goods.BaseCount + num4, goods.GainCount + num5);
				targetSlots.Remove(message.TargetSlotIndex);
				targetSlots.TryPushGoods(goods2, message.TargetSlotIndex);
				if (gainCount + baseCount > 0)
				{
					Goods goods3 = GoodsCreator.MakeGoods(pulledGoods.Type, pulledGoods.Id, baseCount, gainCount);
					IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
					if (bagHolderSlots != null)
					{
						bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, goods3, num, message.InnerSlotIndex);
					}
				}
				return true;
			}
		}
		IDictionary<int, GoodsSlot> bagHolderSlots2 = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
		if (bagHolderSlots2 == null)
		{
			return false;
		}
		long itemMaxStackCount = ((goods.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(goods.Id) : long.MaxValue);
		if (!bagHolderSlots2.TryPushIntoBag(message.BagHolderSlotIndex, goods, itemMaxStackCount, message.InnerSlotIndex))
		{
			return false;
		}
		targetSlots.Remove(message.TargetSlotIndex);
		return targetSlots.TryPushGoods(pulledGoods, message.TargetSlotIndex);
	}

	private bool TryEquipFromBag(ReqBagSlotToAny message, Goods pulledGoods)
	{
		Equip(pulledGoods, message.TargetCategory, message.TargetSlotIndex, out var unequippedGoods);
		if (unequippedGoods == null)
		{
			return true;
		}
		IDictionary<int, GoodsSlot> bagHolderSlots = GetBagHolderSlots(message.BagHolderCategory, message.BagHolderNetworkObjectId, message.SenderId);
		long itemMaxStackCount = ((unequippedGoods.Category == EGoodsCategory.Item) ? Player.GetInventoryItemCountMax(unequippedGoods.Id) : long.MaxValue);
		if (bagHolderSlots != null && bagHolderSlots.TryPushIntoBag(message.BagHolderSlotIndex, unequippedGoods, itemMaxStackCount, message.InnerSlotIndex))
		{
			return true;
		}
		if (!PlayerInventory.Slots.TryPushGoods(unequippedGoods, -1))
		{
			_logger.Error($"Failed to handle unequipped goods after bag-equip: {unequippedGoods}");
		}
		return true;
	}

	private long ResolveBagInnerItemMaxStack(IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int innerSlotIndex)
	{
		if (bagHolderSlots.TryGetValue(bagHolderSlotIndex, out var value) && value.Goods is BagGoods bagGoods && bagGoods.GoodsSlots != null && innerSlotIndex >= 0 && innerSlotIndex < bagGoods.GoodsSlots.Length)
		{
			Goods goods = bagGoods.GoodsSlots[innerSlotIndex]?.Goods;
			if (goods != null && goods.Category == EGoodsCategory.Item)
			{
				return Player.GetInventoryItemCountMax(goods.Id);
			}
		}
		return long.MaxValue;
	}

	private IDictionary<int, GoodsSlot> GetBagHolderSlots(EGoodsSlotCategory category, ulong networkObjectId, ulong senderId)
	{
		switch (category)
		{
			case EGoodsSlotCategory.InventorySlot:
				return PlayerInventory.Slots;
			case EGoodsSlotCategory.SafeSlot:
				return PlayerInventory.SafeSlots;
			case EGoodsSlotCategory.LootingSlot:
			{
				Inventory inventory = _gameObjectManager.FindInventory(networkObjectId, senderId);
				if (inventory != null)
				{
					return inventory.Slots;
				}
				return null;
			}
			default:
				return null;
		}
	}

	private void RegisterHostPlayerActionHandlers()
	{
		RegisterHostHandler<ReqLevelUp>(OnLevelUp);
		RegisterHostHandler<ReqGainAccumulatedExp>(OnGainAccumulatedExp);
		RegisterHostHandler<ReqUnlockTalent>(OnUnlockTalent);
		RegisterHostHandler<ReqChangeCharacter>(OnChangeCharacter);
		RegisterHostHandler<ReqSetPet>(OnSetPet);
		RegisterHostHandler<ReqChangeCharacterSkin>(OnChangeCharacterSkin);
		RegisterHostHandler<ReqChangeCharacterSkills>(OnChangeCharacterSkills);
		RegisterHostHandler<ReqShopBuyGoods>(OnShopBuyGoods);
		RegisterHostHandler<ReqShopSellGoods>(OnShopSellGoods);
		RegisterHostHandler<ReqCraftingGoods>(OnCraftingGoods);
		RegisterHostHandler<ReqScrapGoods>(OnScrapGoods);
		RegisterHostHandler<ReqRegisterItemUsed>(OnRegisterItemUsed);
		RegisterHostHandler<ReqSellArtifact>(OnSellArtifact);
		RegisterHostHandler<ReqDealArtifact>(OnDealArtifact);
		RegisterHostHandler<ReqRepairGoods>(OnRepairGoods);
		RegisterHostHandler<ReqRepairAllGoods>(OnRepairAllGoods);
		RegisterHostHandler<ReqRest>(OnRest);
		RegisterHostHandler<ReqParadoxPurify>(OnParadoxPurify);
		RegisterHostHandler<ReqParadoxLock>(OnParadoxLock);
		RegisterHostHandler<ReqParadoxUnlock>(OnParadoxUnlock);
		RegisterHostHandler<ReqTakeLostItem>(OnTakeLostItem);
		RegisterHostHandler<ReqRerollLostItems>(OnRerollLostItems);
		RegisterHostHandler<ReqTycoonFinish>(OnTycoonFinish);
		RegisterHostHandler<ReqTycoonHire>(OnTycoonHire);
		RegisterHostHandler<ReqTycoonRegisterArtifact>(OnTycoonRegisterArtifact);
		RegisterHostHandler<ReqTycoonCancel>(OnTycoonCancel);
		RegisterHostHandler<ReqTycoonPurify>(OnTycoonPurify);
		RegisterHostHandler<ReqExchangeCoin>(OnExchangeCoin);
		RegisterHostHandler<ReqBuyInterior>(OnBuyInterior);
	}

	private void GainGold(long amount)
	{
		AssertIfNotHost();
		if (amount < 1)
		{
			return;
		}
		Profile profile = Player?.Profile?.Clone();
		if (profile != null)
		{
			profile.Gold += amount;
			Player.Profile = profile;
		}
	}

	private bool TryConsumeGold(long amount)
	{
		AssertIfNotHost();
		Profile profile = Player.Profile.Clone();
		if (profile.Gold < amount)
		{
			_logger.Warning($"TryConsumeGold rejected: required={amount}, available={profile.Gold}");
			return false;
		}
		profile.Gold -= amount;
		Player.Profile = profile;
		return true;
	}

	private void GainCoin(long amount)
	{
		AssertIfNotHost();
		if (amount < 1)
		{
			return;
		}
		Profile profile = Player?.Profile?.Clone();
		if (profile != null)
		{
			profile.Coin += amount;
			Player.Profile = profile;
		}
	}

	private bool TryConsumeCoin(long amount)
	{
		AssertIfNotHost();
		Profile profile = Player.Profile.Clone();
		if (profile.Coin < amount)
		{
			_logger.Warning($"TryConsumeCoin rejected: required={amount}, available={profile.Coin}");
			return false;
		}
		profile.Coin -= amount;
		Player.Profile = profile;
		return true;
	}

	private bool TryConsumeExp(long amount)
	{
		AssertIfNotHost();
		Profile profile = Player.Profile.Clone();
		if (profile.Exp < amount)
		{
			_logger.Warning($"TryConsumeExp rejected: required={amount}, available={profile.Exp}");
			return false;
		}
		profile.Exp -= amount;
		Player.Profile = profile;
		return true;
	}

	private bool TryConsumeGoods(Goods goods, bool allowStashFallback, out long consumeGold, out long consumeCoin, out Goods remainGoods)
	{
		consumeGold = 0L;
		consumeCoin = 0L;
		remainGoods = null;
		GoodsData remainGoodsData = default(GoodsData);
		GoodsData remainGoodsData2 = default(GoodsData);
		if (goods is GoldGoods)
		{
			if (TryConsumeGold(goods.Count))
			{
				consumeGold = goods.Count;
				return true;
			}
			return false;
		}
		if (goods is CoinGoods)
		{
			if (TryConsumeCoin(goods.Count))
			{
				consumeCoin = goods.Count;
				return true;
			}
			return false;
		}
		if (goods is ExpGoods)
		{
			return TryConsumeExp(goods.Count);
		}
		if (allowStashFallback)
		{
			PlayerInventory.TryConsumeGoods(goods.ToData(), out remainGoodsData2);
			if (remainGoodsData2.Count > 0)
			{
				remainGoods = GoodsCreator.MakeFromGoodsData(remainGoodsData2, remainGoodsData2.Count, 0L);
			}
			return true;
		}
		if (goods.Type == EGoodsType.Bag)
		{
			long count = goods.Count;
			long emptyBagCount = PlayerInventory.GetEmptyBagCount(goods.Id);
			long bagCount = PlayerInventory.GetBagCount(goods.Id);
			EBagConsumeFailReason eBagConsumeFailReason = BagConsumeRule.Check(count, emptyBagCount, bagCount);
			if (eBagConsumeFailReason != EBagConsumeFailReason.None)
			{
				GameHost.SendNoticeToast(BagConsumeRule.GetFailMessage(eBagConsumeFailReason), ClientId);
				return false;
			}
		}
		return PlayerInventory.TryConsumeGoods(goods.ToData(), out remainGoodsData);
	}

	private bool TryConsumeGoods(Goods[] goodsArray, out long consumeGold, out long consumeCoin, out Goods[] remainMaterials)
	{
		consumeGold = 0L;
		consumeCoin = 0L;
		remainMaterials = null;
		if (goodsArray == null || goodsArray.Length == 0)
		{
			return true;
		}
		List<Goods> list = new List<Goods>();
		for (int i = 0; i < goodsArray.Length; i++)
		{
			if (!TryConsumeGoods(goodsArray[i], true, out var consumeGold2, out var consumeCoin2, out var remainGoods))
			{
				return false;
			}
			consumeGold += consumeGold2;
			consumeCoin += consumeCoin2;
			if (remainGoods != null)
			{
				list.Add(remainGoods);
			}
		}
		if (list.Count > 0)
		{
			remainMaterials = list.ToArray();
		}
		return true;
	}

	private void GainExp(long amount)
	{
		AssertIfNotHost();
		if (amount < 1)
		{
			return;
		}
		Profile profile = Player?.Profile?.Clone();
		if (profile != null)
		{
			profile.Exp += amount;
			Player.Profile = profile;
		}
	}

	private void GainCurrency(Goods goods)
	{
		if (goods is GoldGoods)
		{
			GainGold(goods.Count);
		}
		else if (goods is CoinGoods)
		{
			GainCoin(goods.Count);
		}
		else if (goods is ExpGoods)
		{
			GainExp(goods.Count);
		}
	}

	private void OnLevelUp(ReqLevelUp message)
	{
		AssertIfNotHost();
		ResLevelUp resLevelUp = new ResLevelUp
		{
			Sequence = message.Sequence
		};
		if (Player.Profile.CanLevelUp())
		{
			int level = Player.Profile.Level;
			long consumeGold = 0L;
			long consumeCoin = 0L;
			Goods[] goodsArray = TLevel.Table[level].ConsumeGoodsData?.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
			if (goodsArray != null && !TryConsumeGoods(goodsArray, out consumeGold, out consumeCoin, out var _))
			{
				GameHost.SendToClient(message.SenderId, resLevelUp);
				return;
			}
			Profile profile = Player.Profile.Clone();
			profile.Level = level + 1;
			Player.Profile = profile;
			resLevelUp.IsSuccess = true;
			resLevelUp.Level = level + 1;
			resLevelUp.ConsumeGold = consumeGold;
			resLevelUp.ConsumeCoin = consumeCoin;
		}
		GameHost.SendToClient(message.SenderId, resLevelUp);
	}

	private void OnGainAccumulatedExp(ReqGainAccumulatedExp message)
	{
		GainExp(message.AccumulatedExp);
	}

	private void OnUnlockTalent(ReqUnlockTalent message)
	{
		AssertIfNotHost();
		ResUnlockTalent resUnlockTalent = new ResUnlockTalent
		{
			Sequence = message.Sequence,
			TalentId = message.TalentId
		};
		TTalent tTalent = TTalent.Table[message.TalentId];
		if (!Player.HasTalent(message.TalentId))
		{
			Goods[] remainMaterials = Array.Empty<Goods>();
			long consumeGold = 0L;
			long consumeCoin = 0L;
			Goods[] goodsArray = tTalent.MaterialGoodsDatas?.Select((GoodsData _) => _.ToGoodsAsBase()).ToArray();
			if (goodsArray != null && !TryConsumeGoods(goodsArray, out consumeGold, out consumeCoin, out remainMaterials))
			{
				GameHost.SendToClient(message.SenderId, resUnlockTalent);
				return;
			}
			Player.UnlockTalent(message.TalentId);
			resUnlockTalent.IsSuccess = true;
			resUnlockTalent.ConsumeGold = consumeGold;
			resUnlockTalent.RemainMaterials = remainMaterials;
		}
		GameHost.SendToClient(message.SenderId, resUnlockTalent);
	}

	private void OnChangeCharacter(ReqChangeCharacter message)
	{
		AssertIfNotHost();
		Player.Character = message.Character;
		Profile profile = Player.Profile.Clone();
		profile.SelectedCharacterId = Player.Character.Id;
		Player.Profile = profile;
		SynchronizeGamePlayer();
		ObjectManager.RespawnCharacter(Player, CharacterModel.Position);
	}

	private void OnSetPet(ReqSetPet message)
	{
		AssertIfNotHost();
		ResSetPet resSetPet = new ResSetPet
		{
			Sequence = message.Sequence,
			PetId = message.PetId
		};
		Profile profile = Player.Profile.Clone();
		profile.SelectedPetId = message.PetId;
		Player.Profile = profile;
		SynchronizeGamePlayer();
		ObjectManager.SpawnPet(Player, CharacterModel.Position);
		resSetPet.IsSuccess = true;
		GameHost.SendToClient(message.SenderId, resSetPet);
	}

	private void OnChangeCharacterSkin(ReqChangeCharacterSkin message)
	{
		AssertIfNotHost();
		Character character = Player.Character.Clone();
		character.SkinPresetId = message.SkinPresetId;
		Player.Character = character;
	}

	private void OnChangeCharacterSkills(ReqChangeCharacterSkills message)
	{
		AssertIfNotHost();
		Character character = Player.Character.Clone();
		character.SelectedActiveSkillIds = message.ActiveSkillIds;
		Player.Character = character;
	}

	private void OnShopBuyGoods(ReqShopBuyGoods message)
	{
		AssertIfNotHost();
		ResShopBuyGoods resShopBuyGoods = new ResShopBuyGoods
		{
			Sequence = message.Sequence,
			ProductId = message.ProductId,
			BuyCount = message.BuyCount,
			DestinationSlotCategory = message.DestinationSlotCategory
		};
		TShopProduct tShopProduct = TShopProduct.Table[message.ProductId];
		GoodsData gainGoodsData = tShopProduct.GainGoodsData;
		Goods goods = ((gainGoodsData.Category != EGoodsCategory.Equipment) ? GoodsCreator.MakeFromGoodsData(tShopProduct.GainGoodsData, message.BuyCount * gainGoodsData.Count, 0L) : CreateShopEquipment(tShopProduct.GainGoodsData.Id, message.BuyCount));
		Goods goods2 = GoodsCreator.MakeFromGoodsData(tShopProduct.ConsumeGoodsData, (long)message.BuyCount * tShopProduct.ConsumeGoodsData.Count, 0L);
		EGoodsSlotCategory destinationSlotCategory = message.DestinationSlotCategory;
		if (destinationSlotCategory == EGoodsSlotCategory.StashSlot || PlayerInventory.CanAddGoods(goods))
		{
			if (_shopManager.TryConsumeShopProductStock(message.ProductId, message.BuyCount) && TryConsumeGoods(goods2, true, out var consumeGold, out var _, out var remainGoods))
			{
				resShopBuyGoods.ConsumeGold = consumeGold;
				if (remainGoods != null)
				{
					resShopBuyGoods.RemainMaterials = remainGoods;
				}
				if (destinationSlotCategory == EGoodsSlotCategory.StashSlot)
				{
					resShopBuyGoods.StashGainGoods = goods;
				}
				else if (!PlayerInventory.TryGainGoods(goods))
				{
					_logger.Error($"Failed to add bought goods to inventory. ProductId={message.ProductId}, BuyCount={message.BuyCount}");
				}
				resShopBuyGoods.IsSuccess = true;
			}
		}
		GameHost.SendToClient(message.SenderId, resShopBuyGoods);
		Goods CreateShopEquipment(int equipmentId, int count)
		{
			if (count > 1)
			{
				_logger.Error($"Cannot buy multiple equipments: ProductId={message.ProductId}, BuyCount={count}");
				return null;
			}
			TEquipment tEquipment = TEquipment.Table[equipmentId];
			int[] prefixes;
			int? maxDurability;
			float? currentDurability;
			byte campaignLevel;
			if (tEquipment.IsArtifact())
			{
				prefixes = tEquipment.PickPrefixes();
				campaignLevel = (byte)_gameContext.VariantDifficulty;
				maxDurability = null;
				currentDurability = null;
			}
			else
			{
				int num = tEquipment.GetMaxDurability(1f);
				maxDurability = num;
				currentDurability = num;
				prefixes = null;
				campaignLevel = 1;
			}
			return GoodsCreator.MakeEquipmentGoods(equipmentId, 1L, 0L, prefixes, maxDurability, currentDurability, campaignLevel);
		}
	}

	private void OnShopSellGoods(ReqShopSellGoods message)
	{
		AssertIfNotHost();
		ResShopSellGoods resShopSellGoods = new ResShopSellGoods
		{
			Sequence = message.Sequence,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex,
			SellGoods = message.SellGoods
		};
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot || PlayerInventory.TryConsumeSlotGoods(message.SlotCategory, message.SlotIndex, message.SellGoods))
		{
			int sellPriceTotal = message.SellGoods.SellPriceTotal;
			GainGold(sellPriceTotal);
			resShopSellGoods.IsSuccess = true;
			resShopSellGoods.GainGold = sellPriceTotal;
		}
		GameHost.SendToClient(message.SenderId, resShopSellGoods);
	}

	private void OnCraftingGoods(ReqCraftingGoods message)
	{
		AssertIfNotHost();
		ResCraftingGoods resCraftingGoods = new ResCraftingGoods
		{
			Sequence = message.Sequence,
			CraftingId = message.CraftingId
		};
		TCrafting tCrafting = TCrafting.Table[message.CraftingId];
		GoodsData resultGoodsData = tCrafting.ResultGoodsData;
		EGoodsCategory category = tCrafting.ResultGoodsData.Category;
		if ((category == EGoodsCategory.Equipment || category == EGoodsCategory.Bag) && message.Count > 1)
		{
			_logger.Error($"Cannot craft multiple equipment or bags: CraftingId={message.CraftingId}, Count={message.Count}");
			GameHost.SendToClient(message.SenderId, resCraftingGoods);
			return;
		}
		Goods goods = ((resultGoodsData.Category != EGoodsCategory.Equipment) ? GoodsCreator.MakeFromGoodsData(tCrafting.ResultGoodsData, tCrafting.ResultGoodsData.Count * message.Count, 0L) : CreateCraftedEquipment(tCrafting.ResultGoodsData.Id));
		GoodsData[] materialGoodsDatas = tCrafting.MaterialGoodsDatas;
		Goods[] remainMaterials = Array.Empty<Goods>();
		long consumeGold = 0L;
		long consumeCoin = 0L;
		if (materialGoodsDatas != null)
		{
			Goods[] goodsArray = materialGoodsDatas.Select((GoodsData _) => GoodsCreator.MakeFromGoodsData(_, message.Count * _.Count, 0L)).ToArray();
			if (goodsArray != null && !TryConsumeGoods(goodsArray, out consumeGold, out consumeCoin, out remainMaterials))
			{
				GameHost.SendToClient(message.SenderId, resCraftingGoods);
				return;
			}
		}
		PlayerInventory.TryPushGoods(goods, out var remainGoods);
		resCraftingGoods.IsSuccess = true;
		resCraftingGoods.RemainMaterials = remainMaterials;
		resCraftingGoods.ConsumeGold = consumeGold;
		if (remainGoods != null)
		{
			resCraftingGoods.RemainGoods = new Goods[1] { remainGoods };
		}
		GameHost.SendToClient(message.SenderId, resCraftingGoods);
		Goods CreateCraftedEquipment(int equipmentId)
		{
			TEquipment tEquipment = TEquipment.Table[equipmentId];
			int[] prefixes;
			int? maxDurability;
			float? currentDurability;
			byte campaignLevel;
			if (tEquipment.IsArtifact())
			{
				prefixes = tEquipment.PickPrefixes();
				campaignLevel = (byte)_gameContext.VariantDifficulty;
				maxDurability = null;
				currentDurability = null;
			}
			else
			{
				int num = tEquipment.GetMaxDurability(1f);
				maxDurability = num;
				currentDurability = num;
				prefixes = null;
				campaignLevel = 1;
			}
			return GoodsCreator.MakeEquipmentGoods(equipmentId, 1L, 0L, prefixes, maxDurability, currentDurability, campaignLevel);
		}
	}

	private void OnScrapGoods(ReqScrapGoods message)
	{
		ResScrapGoods resScrapGoods = new ResScrapGoods
		{
			Sequence = message.Sequence,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex
		};
		TScrap tScrap = TScrap.Table[message.ScrapId];
		Goods goods = GoodsCreator.MakeFromGoodsData(tScrap.InsertGoodsData, (long)message.Count * tScrap.InsertGoodsData.Count, 0L);
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot || PlayerInventory.TryConsumeSlotGoods(message.SlotCategory, message.SlotIndex, goods))
		{
			List<Goods> list = new List<Goods>();
			GoodsData[] resultGoodsData = tScrap.ResultGoodsData;
			for (int i = 0; i < resultGoodsData.Length; i++)
			{
				Goods goods2 = GoodsCreator.MakeFromGoodsData(resultGoodsData[i], (long)message.Count * resultGoodsData[i].Count, 0L);
				(Inventory as PlayerInventory).TryPushGoods(goods2, out var remainGoods);
				if (remainGoods != null)
				{
					list.Add(remainGoods);
				}
			}
			resScrapGoods.IsSuccess = true;
			resScrapGoods.ConsumeGoods = goods;
			resScrapGoods.RemainGoods = ((list.Count > 0) ? list.ToArray() : null);
		}
		GameHost.SendToClient(message.SenderId, resScrapGoods);
	}

	private void OnRegisterItemUsed(ReqRegisterItemUsed message)
	{
		AssertIfNotHost();
		ResRegisterItemUsed resRegisterItemUsed = new ResRegisterItemUsed
		{
			Sequence = message.Sequence,
			ItemId = message.ItemId,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex
		};
		TItem tItem = TItem.Table[message.ItemId];
		if (tItem != null && tItem.GoodsType == message.ExpectedType && tItem.GoodsType.GoodsCategory() == EGoodsCategory.Item)
		{
			Goods goods = GoodsCreator.MakeItemGoods(message.ItemId, 1L, 0L);
			if (message.SlotCategory != EGoodsSlotCategory.StashSlot && !PlayerInventory.TryConsumeSlotGoods(message.SlotCategory, message.SlotIndex, goods))
			{
				GameHost.SendToClient(message.SenderId, resRegisterItemUsed);
				return;
			}
			resRegisterItemUsed.IsSuccess = true;
			resRegisterItemUsed.ConsumeGoods = goods;
		}
		GameHost.SendToClient(message.SenderId, resRegisterItemUsed);
	}

	private void OnSellArtifact(ReqSellArtifact message)
	{
		ResSellArtifact resSellArtifact = new ResSellArtifact
		{
			Sequence = message.Sequence,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex,
			GainGold = message.GainGold,
			SellGoods = message.SellGoods
		};
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot || PlayerInventory.TryConsumeSlotGoods(message.SlotCategory, message.SlotIndex, message.SellGoods))
		{
			GainGold(message.GainGold);
			GainExp(message.GainExp);
			resSellArtifact.IsSuccess = true;
		}
		GameHost.SendToClient(message.SenderId, resSellArtifact);
	}

	private void OnDealArtifact(ReqDealArtifact message)
	{
		ResDealArtifact resDealArtifact = new ResDealArtifact
		{
			Sequence = message.Sequence,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex,
			DealGoods = message.DealGoods,
			GainCoin = message.GainCoin,
			GainExp = message.GainExp
		};
		if (message.SlotCategory == EGoodsSlotCategory.StashSlot || PlayerInventory.TryConsumeSlotGoods(message.SlotCategory, message.SlotIndex, message.DealGoods))
		{
			GainCoin(message.GainCoin);
			GainExp(message.GainExp);
			resDealArtifact.IsSuccess = true;
		}
		GameHost.SendToClient(message.SenderId, resDealArtifact);
	}

	private void OnRest(ReqRest message)
	{
		ResRest resRest = new ResRest
		{
			Sequence = message.Sequence
		};
		long num = Player.CalculateRestCost();
		if (TryConsumeGold(num))
		{
			Profile profile = Player.Profile.Clone();
			profile.RestCount++;
			Player.Profile = profile;
			resRest.RemovedParadoxId = Player.RemoveRandomParadox();
			resRest.IsSuccess = true;
			resRest.ConsumeGold = num;
		}
		GameHost.SendToClient(message.SenderId, resRest);
	}

	private void OnParadoxPurify(ReqParadoxPurify message)
	{
		ResParadoxPurify resParadoxPurify = new ResParadoxPurify
		{
			Sequence = message.Sequence,
			ParadoxId = message.ParadoxId
		};
		GoodsData removalGoodsData = TParadox.Table[message.ParadoxId].GetRemovalGoodsData();
		if (removalGoodsData.Count > 0)
		{
			Goods goods = removalGoodsData.ToGoodsAsBase();
			if (TryConsumeGoods(goods, false, out var consumeGold, out var _, out var _))
			{
				resParadoxPurify.ConsumeGold = consumeGold;
				Player.RemoveParadox(message.ParadoxId);
				resParadoxPurify.IsSuccess = true;
			}
		}
		GameHost.SendToClient(message.SenderId, resParadoxPurify);
	}

	private void OnParadoxLock(ReqParadoxLock message)
	{
		ResParadoxLock resParadoxLock = new ResParadoxLock
		{
			Sequence = message.Sequence,
			ParadoxId = message.ParadoxId
		};
		Goods goods = TParadox.Table[message.ParadoxId].GetLockGoodsData().ToGoodsAsBase();
		if (goods != null && goods.Count > 0 && TryConsumeGoods(goods, false, out var consumeGold, out var _, out var _))
		{
			resParadoxLock.ConsumeGold = consumeGold;
			Player.LockParadox(message.ParadoxId);
			resParadoxLock.IsSuccess = true;
		}
		GameHost.SendToClient(message.SenderId, resParadoxLock);
	}

	private void OnParadoxUnlock(ReqParadoxUnlock message)
	{
		ResParadoxUnlock resParadoxUnlock = new ResParadoxUnlock
		{
			Sequence = message.Sequence
		};
		if (Player.Character.LockedParadoxId == message.ParadoxId)
		{
			Goods goods = TParadox.Table[message.ParadoxId].GetUnlockGoodsData().ToGoodsAsBase();
			if (goods == null)
			{
				return;
			}
			if (goods.Count > 0 && TryConsumeGoods(goods, false, out var consumeGold, out var _, out var _))
			{
				resParadoxUnlock.ConsumeGold = consumeGold;
				Player.UnlockParadox();
				resParadoxUnlock.IsSuccess = true;
			}
		}
		GameHost.SendToClient(message.SenderId, resParadoxUnlock);
	}

	private void OnRepairGoods(ReqRepairGoods message)
	{
		ResRepairGoods resRepairGoods = new ResRepairGoods
		{
			Sequence = message.Sequence,
			SlotCategory = message.SlotCategory,
			SlotIndex = message.SlotIndex
		};
		Goods repairTargetGoods = GetRepairTargetGoods(message.SlotCategory, message.SlotIndex);
		if (repairTargetGoods != null && repairTargetGoods.CanRepair())
		{
			int repairCost = repairTargetGoods.GetRepairCost();
			if (repairCost > 0 && TryConsumeGold(repairCost))
			{
				Goods goods = repairTargetGoods.Repair(message.PreventMaxDurabilityLoss);
				SetRepairTargetGoods(message.SlotCategory, message.SlotIndex, goods);
				if (message.SlotCategory == EGoodsSlotCategory.WeaponSlot || message.SlotCategory == EGoodsSlotCategory.AccessorySlot)
				{
					CharacterModel.RefreshEquipmentEffects(true);
				}
				resRepairGoods.IsSuccess = true;
				resRepairGoods.RepairedGoods = goods;
				resRepairGoods.ConsumeGold = repairCost;
			}
		}
		GameHost.SendToClient(message.SenderId, resRepairGoods);
	}

	private Goods GetRepairTargetGoods(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		GoodsSlot goodsSlot;
		switch (slotCategory)
		{
			case EGoodsSlotCategory.WeaponSlot:
				goodsSlot = Player.Character.WeaponSlots[slotIndex];
				break;
			case EGoodsSlotCategory.AccessorySlot:
				goodsSlot = Player.Character.AccessorySlots[slotIndex];
				break;
			default:
				goodsSlot = PlayerInventory.GetGoodsSlot(slotCategory, slotIndex);
				if (goodsSlot == null)
				{
					return null;
				}
				break;
		}
		return goodsSlot.Goods;
	}

	private void SetRepairTargetGoods(EGoodsSlotCategory slotCategory, int slotIndex, Goods repairedGoods)
	{
		Goods unequippedGoods;
		switch (slotCategory)
		{
			case EGoodsSlotCategory.InventorySlot:
			{
				GoodsSlot goodsSlot2 = PlayerInventory.GetGoodsSlot(slotCategory, slotIndex);
				if (goodsSlot2 == null)
				{
					return;
				}
				goodsSlot2.Goods = repairedGoods;
				PlayerInventory.Slots[slotIndex] = goodsSlot2;
				break;
			}
			case EGoodsSlotCategory.SafeSlot:
			{
				GoodsSlot goodsSlot = PlayerInventory.GetGoodsSlot(slotCategory, slotIndex);
				if (goodsSlot == null)
				{
					return;
				}
				goodsSlot.Goods = repairedGoods;
				PlayerInventory.SafeSlots[slotIndex] = goodsSlot;
				break;
			}
			case EGoodsSlotCategory.WeaponSlot:
			case EGoodsSlotCategory.AccessorySlot:
				Equip(repairedGoods, slotCategory, slotIndex, out unequippedGoods);
				break;
		}
	}

	private void OnRepairAllGoods(ReqRepairAllGoods message)
	{
		ResRepairAllGoods resRepairAllGoods = new ResRepairAllGoods
		{
			Sequence = message.Sequence
		};
		List<(EGoodsSlotCategory, int)> list = new List<(EGoodsSlotCategory, int)>();
		long num = 0L;
		GoodsSlot[] weaponSlots = Player.Character.WeaponSlots;
		for (int i = 0; i < weaponSlots.Length; i++)
		{
			GoodsSlot goodsSlot = weaponSlots[i];
			if (goodsSlot.Goods is WeaponGoods weaponGoods && weaponGoods.CanRepair())
			{
				list.Add((EGoodsSlotCategory.WeaponSlot, goodsSlot.SlotIndex));
				num += weaponGoods.GetRepairCost();
			}
		}
		GoodsSlot[] accessorySlots = Player.Character.AccessorySlots;
		for (int j = 0; j < accessorySlots.Length; j++)
		{
			GoodsSlot goodsSlot2 = accessorySlots[j];
			if (goodsSlot2.Goods is AccessoryGoods accessoryGoods && accessoryGoods.CanRepair())
			{
				list.Add((EGoodsSlotCategory.AccessorySlot, goodsSlot2.SlotIndex));
				num += accessoryGoods.GetRepairCost();
			}
		}
		foreach (GoodsSlot value in PlayerInventory.Slots.Values)
		{
			if (value != null && !value.IsEmpty && value.Goods.IsEquipment() && value.Goods.CanRepair())
			{
				list.Add((EGoodsSlotCategory.InventorySlot, value.SlotIndex));
				num += value.Goods.GetRepairCost();
			}
		}
		foreach (GoodsSlot value2 in PlayerInventory.SafeSlots.Values)
		{
			if (value2 != null && !value2.IsEmpty && value2.Goods.IsEquipment() && value2.Goods.CanRepair())
			{
				list.Add((EGoodsSlotCategory.SafeSlot, value2.SlotIndex));
				num += value2.Goods.GetRepairCost();
			}
		}
		if (num < 1 || !TryConsumeGold(num))
		{
			GameHost.SendToClient(message.SenderId, resRepairAllGoods);
			return;
		}
		bool flag = false;
		foreach (var item in list)
		{
			EGoodsSlotCategory item2 = item.Item1;
			int item3 = item.Item2;
			Goods goods = GetRepairTargetGoods(item2, item3).Repair(message.PreventMaxDurabilityLoss);
			SetRepairTargetGoods(item2, item3, goods);
			if (item2 == EGoodsSlotCategory.WeaponSlot || item2 == EGoodsSlotCategory.AccessorySlot)
			{
				flag = true;
			}
		}
		if (flag)
		{
			(UnitModel as CharacterModel).RefreshEquipmentEffects(true);
		}
		resRepairAllGoods.IsSuccess = true;
		resRepairAllGoods.TotalConsumeGold = num;
		GameHost.SendToClient(message.SenderId, resRepairAllGoods);
	}

	private ETycoonFailType RollTycoonFailType()
	{
		return ETycoonFailType.ArtifactDamage;
	}

	private void FlushPlayerStateBeforeResponse()
	{
		if (PlayerInventory == null)
		{
			return;
		}
		PlayerInventory.SynchronizeImmediate();
		if (Player == null)
		{
			return;
		}
		if (Player != null && Player.HasChange)
		{
			GamePlayerData changeData = Player.Flush();
			GameHost.SendToSnapshotApplied(new NtfUpdateGamePlayer
			{
				ChangeData = changeData
			});
		}
	}

	private void OnTycoonHire(ReqTycoonHire message)
	{
		AssertIfNotHost();
		ResTycoonHire resTycoonHire = new ResTycoonHire
		{
			Sequence = message.Sequence
		};
		long num = _tycoonManager.CalculateHireCost(message.EmployeeId, message.Level);
		if (TryConsumeGold(num))
		{
			PlayerInventory.SynchronizeImmediate();
			SynchronizeGamePlayer();
			resTycoonHire.IsSuccess = true;
			resTycoonHire.ConsumeGold = num;
		}
		GameHost.SendToClient(message.SenderId, resTycoonHire);
	}

	private void OnTycoonRegisterArtifact(ReqTycoonRegisterArtifact message)
	{
		AssertIfNotHost();
		ResTycoonRegisterArtifact resTycoonRegisterArtifact = new ResTycoonRegisterArtifact
		{
			Sequence = message.Sequence,
			ArtifactSlotCategory = message.ArtifactSlotCategory,
			ArtifactSlotIndex = message.ArtifactSlotIndex
		};
		if (message.ArtifactSlotCategory == EGoodsSlotCategory.StashSlot)
		{
			if (message.Artifact != null)
			{
				resTycoonRegisterArtifact.Artifact = message.Artifact;
				PlayerInventory.SynchronizeImmediate();
				SynchronizeGamePlayer();
				resTycoonRegisterArtifact.IsSuccess = true;
			}
		}
		else
		{
			GoodsSlot goodsSlot = PlayerInventory.GetGoodsSlot(message.ArtifactSlotCategory, message.ArtifactSlotIndex);
			if (goodsSlot != null && !goodsSlot.IsEmpty)
			{
				resTycoonRegisterArtifact.Artifact = goodsSlot.Goods.Clone();
				if (PlayerInventory.TryConsumeSlotGoods(message.ArtifactSlotCategory, message.ArtifactSlotIndex, goodsSlot.Goods))
				{
					PlayerInventory.SynchronizeImmediate();
					SynchronizeGamePlayer();
					resTycoonRegisterArtifact.IsSuccess = true;
				}
			}
		}
		GameHost.SendToClient(message.SenderId, resTycoonRegisterArtifact);
	}

	private void OnTycoonFinish(ReqTycoonFinish message)
	{
		AssertIfNotHost();
		ResTycoonFinish resTycoonFinish = new ResTycoonFinish
		{
			Sequence = message.Sequence
		};
		long num = 0L;
		if (!message.IsSuccess)
		{
			float randomFloat = RandomUtility.GetRandomFloat();
			if (randomFloat < 0.1f)
			{
				resTycoonFinish.FailType = ETycoonFailType.ArtifactDamage;
				num = (long)((float)message.Artifact.SellPriceTotal * TConst.TycoonArtifactDamageGoldRatio.FloatValue);
			}
			else if (randomFloat >= 0.3f)
			{
				resTycoonFinish.FailType = ((randomFloat < 0.7f) ? ETycoonFailType.SlotDamage : ETycoonFailType.Loafing);
				resTycoonFinish.ReturnArtifact = message.Artifact;
			}
			else
			{
				resTycoonFinish.FailType = ETycoonFailType.CheapSale;
				num = (long)((float)message.Artifact.SellPriceTotal * TConst.TycoonCheapSaleGoldRatio.FloatValue);
			}
		}
		else
		{
			num = (long)(((float)message.PriceBonusPercent / 100f + 1f) * (float)message.Artifact.SellPriceTotal);
		}
		long num2 = num / TConst.CoinToGoldRate.IntValue;
		if (num2 > 0)
		{
			GainCoin(num2);
		}
		if (message.GainExp > 0)
		{
			GainExp(message.GainExp);
		}
		PlayerInventory.SynchronizeImmediate();
		SynchronizeGamePlayer();
		resTycoonFinish.IsSuccess = true;
		resTycoonFinish.GainCoin = num2;
		GameHost.SendToClient(message.SenderId, resTycoonFinish);
	}

	private void OnTycoonCancel(ReqTycoonCancel message)
	{
		AssertIfNotHost();
		ResTycoonCancel resTycoonCancel = new ResTycoonCancel
		{
			Sequence = message.Sequence
		};
		if (message.Artifact != null && PlayerInventory.CanAddGoods(message.Artifact))
		{
			PlayerInventory.TryGainGoods(message.Artifact);
			PlayerInventory.SynchronizeImmediate();
			SynchronizeGamePlayer();
			resTycoonCancel.IsSuccess = true;
		}
		GameHost.SendToClient(message.SenderId, resTycoonCancel);
	}

	private void OnTycoonPurify(ReqTycoonPurify message)
	{
		AssertIfNotHost();
		ResTycoonPurify resTycoonPurify = new ResTycoonPurify
		{
			Sequence = message.Sequence
		};
		long num = _tycoonManager.CalculatePurifyCost(message.Level);
		if (TryConsumeGold(num))
		{
			resTycoonPurify.IsSuccess = true;
			resTycoonPurify.ConsumeGold = num;
		}
		GameHost.SendToClient(message.SenderId, resTycoonPurify);
	}

	private void OnTakeLostItem(ReqTakeLostItem message)
	{
		ResTakeLostItem resTakeLostItem = new ResTakeLostItem
		{
			Sequence = message.Sequence,
			SlotIndex = message.SlotIndex
		};
		if (PlayerInventory.CanAddGoods(message.Goods) && ((message.ConsumeCoin < 1) ? TryConsumeGold(message.ConsumeGold) : TryConsumeCoin(message.ConsumeCoin)))
		{
			PlayerInventory.PushGoods(message.Goods, -1);
			resTakeLostItem.IsSuccess = true;
			resTakeLostItem.ConsumeGold = message.ConsumeGold;
			resTakeLostItem.ConsumeCoin = message.ConsumeCoin;
		}
		GameHost.SendToClient(message.SenderId, resTakeLostItem);
	}

	private void OnRerollLostItems(ReqRerollLostItems message)
	{
		ResRerollLostItems resRerollLostItems = new ResRerollLostItems
		{
			Sequence = message.Sequence
		};
		long num = Player.CalculateLostItemRerollCost();
		if (TryConsumeGold(num))
		{
			Profile profile = Player.Profile.Clone();
			profile.LostItemRerollCount++;
			Player.Profile = profile;
			resRerollLostItems.IsSuccess = true;
			resRerollLostItems.ConsumeGold = num;
		}
		GameHost.SendToClient(message.SenderId, resRerollLostItems);
	}

	private void OnExchangeCoin(ReqExchangeCoin message)
	{
		AssertIfNotHost();
		ResExchangeCoin resExchangeCoin = new ResExchangeCoin
		{
			Sequence = message.Sequence,
			Direction = message.Direction
		};
		if (message.Quantity > 0)
		{
			Goods goods = GoodsCreator.MakeItemGoods(10001, message.Quantity);
			if (message.Direction == ECoinExchangeDirection.CoinToItem)
			{
				if (!TryConsumeCoin(message.Quantity))
				{
					GameHost.SendToClient(message.SenderId, resExchangeCoin);
					return;
				}
				if (!PlayerInventory.TryPushGoods(goods, out var remainGoods))
				{
					resExchangeCoin.RemainGoods = remainGoods;
				}
			}
			else
			{
				if (!PlayerInventory.TryConsumeGoods(goods.ToData(), out var remainGoodsData))
				{
					resExchangeCoin.RemainGoods = GoodsCreator.MakeFromGoodsData(remainGoodsData, remainGoodsData.Count, 0L);
				}
				GainCoin(message.Quantity);
			}
			resExchangeCoin.IsSuccess = true;
		}
		GameHost.SendToClient(message.SenderId, resExchangeCoin);
	}

	private void OnBuyInterior(ReqBuyInterior message)
	{
		AssertIfNotHost();
		ResBuyInterior resBuyInterior = new ResBuyInterior
		{
			Sequence = message.Sequence,
			InteriorShopId = message.InteriorShopId
		};
		if (TInteriorShop.Table.TryGetValue(message.InteriorShopId, out var value))
		{
			Interior interior = value.Interior;
			if (!IsInteriorUnlocked(interior) && _playerManager.CheckStateConditions(value.UnlockConditions) && (value.ConsumeGold < 1 || TryConsumeGold(value.ConsumeGold)) && (value.ConsumeCoin < 1 || TryConsumeCoin(value.ConsumeCoin)))
			{
				if (interior.Type == EInteriorType.Area)
				{
					_gameLoungeManager.UnlockArea(interior.Id);
				}
				else if (interior.Type == EInteriorType.AreaDecoration)
				{
					_gameLoungeManager.UnlockAreaDecoration(interior.Id);
				}
				resBuyInterior.IsSuccess = true;
				resBuyInterior.ConsumeGold = value.ConsumeGold;
				resBuyInterior.ConsumeCoin = value.ConsumeCoin;
			}
		}
		GameHost.SendToClient(message.SenderId, resBuyInterior);
	}

	private bool IsInteriorUnlocked(Interior interior)
	{
		switch (interior.Type)
		{
			case EInteriorType.Area:
				return _gameLoungeManager.IsAreaUnlocked(interior.Id);
			case EInteriorType.NpcLevel:
				return _gameLoungeManager.GetNpcLevel(interior.Id) >= interior.Value;
			case EInteriorType.AreaDecoration:
				return _gameLoungeManager.IsAreaDecorationUnlocked(interior.Id);
			default:
				throw new ArgumentOutOfRangeException("Type", interior.Type, null);
		}
	}

	public override bool IsActionEventExcutable(ActionEvent actionEvent, ulong ownerId, ulong targetId)
	{
		if (!base.IsActionEventExcutable(actionEvent, ownerId, targetId))
		{
			return false;
		}
		return CheckTalentCondition(actionEvent.TalentConditionList);
	}

	protected override void RegisterUnitEventHandlers()
	{
		base.RegisterUnitEventHandlers();
		EventReceiver.Receive<StressDamageEvent>().Subscribe(OnStressDamageEvent).AddTo(this);
		EventReceiver.Receive<StressRecoveryEvent>().Subscribe(OnStressRecoveryEvent).AddTo(this);
		EventReceiver.Receive<StaminaDamageEvent>().Subscribe(OnStaminaDamageEvent).AddTo(this);
		EventReceiver.Receive<StaminaRecoveryEvent>().Subscribe(OnStaminaRecoveryEvent).AddTo(this);
		EventReceiver.Receive<SkillInputEvent>().Subscribe(OnSkillInputEvent).AddTo(this);
		EventReceiver.Receive<UseItemEvent>().Subscribe(OnItemUseEvent).AddTo(this);
		EventReceiver.Receive<TryInteractEvent>().Subscribe(OnTryInteractEvent).AddTo(this);
		EventReceiver.Receive<CancelInteractEvent>().Subscribe(OnCancelInteractEvent).AddTo(this);
		EventReceiver.Receive<EscapeEvent>().Subscribe(OnEscapeEvent).AddTo(this);
		EventReceiver.Receive<LightFuelDamageEvent>().Subscribe(OnLightFuelDamageEvent).AddTo(this);
		EventReceiver.Receive<LightFuelRecoveryEvent>().Subscribe(OnLightFuelRecoveryEvent).AddTo(this);
		EventReceiver.Receive<StageReadyEvent>().Subscribe(OnStageReadyEvent).AddTo(this);
		EventReceiver.Receive<SoulDamageEvent>().Subscribe(OnSoulDamageEvent).AddTo(this);
		EventReceiver.Receive<SoulRecoveryEvent>().Subscribe(OnSoulRecoveryEvent).AddTo(this);
		EventReceiver.Receive<CutsceneStartEvent>().Subscribe(OnCutsceneStartEvent).AddTo(this);
		EventReceiver.Receive<CutsceneFinishEvent>().Subscribe(OnCutsceneFinishEvent).AddTo(this);
	}

	protected override void OnDieEvent(DieEvent unitEvent)
	{
		base.OnDieEvent(unitEvent);
		StressDamageEvent unitEvent2 = new StressDamageEvent
		{
			SourceModel = unitEvent.SourceModel,
			Amount = TConst.StressDamageOnDead.IntValue,
			HitPosition = UnitModel.Position,
			ReasonType = EStressReasonType.Dead
		};
		HandleUnitEvent(unitEvent2);
		UnitIdentity sourceIdentity = unitEvent.SourceIdentity;
		if (!sourceIdentity.IsValid)
		{
			return;
		}
		NtfPlayerKilledToast ntfPlayerKilledToast = new NtfPlayerKilledToast
		{
			PlayerName = CharacterModel.PlayerName
		};
		if (sourceIdentity.UnitType == EUnitType.Character)
		{
			if (unitEvent.SourceModel is CharacterModel characterModel)
			{
				ntfPlayerKilledToast.KillerName = characterModel.PlayerName;
			}
		}
		else if (sourceIdentity.UnitType == EUnitType.Monster)
		{
			ntfPlayerKilledToast.KillerMonsterTableId = sourceIdentity.TableId;
		}
		GameHost.SendPlayerKilledToastToAll(ntfPlayerKilledToast, new HashSet<ulong> { CharacterModel.ClientId });
	}

	protected override void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
		base.OnHpDamageEvent(unitEvent);
		if (unitEvent.Damage < 1)
		{
			return;
		}
		TryShowAllyDamageBubble(unitEvent);
		float timeNow = _gameManager.TimeNow;
		if (timeNow - _lastHpDamageTime > TConst.ComboThresholdTime.FloatValue)
		{
			_accumulatedComboDamage = 0;
		}
		_lastHpDamageTime = timeNow;
		_accumulatedComboDamage += unitEvent.Damage;
		if ((float)_accumulatedComboDamage / UnitModel.HpMax * 100f >= TConst.StressDamageTriggerHpPercent.FloatValue)
		{
			StressDamageEvent unitEvent2 = new StressDamageEvent
			{
				SourceModel = unitEvent.SourceModel,
				Amount = TConst.StressDamageOnHpDamaged.IntValue,
				HitPosition = UnitModel.Position,
				ReasonType = EStressReasonType.HpDamaged
			};
			HandleUnitEvent(unitEvent2);
			_accumulatedComboDamage = 0;
		}
	}

	protected override void OnHpDamageApplied(HpDamageEvent unitEvent, int hpDamage)
	{
		base.OnHpDamageApplied(unitEvent, hpDamage);
		if (base.IsHost && unitEvent.ReasonType == EHpDamageReasonType.Hit && hpDamage > 0)
		{
			DecreaseAccessoryDurability();
		}
	}

	private void OnSkillInputEvent(SkillInputEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnItemUseEvent(UseItemEvent unitEvent)
	{
		TriggerEvent(unitEvent);
		TItem tItem = TItem.Table[unitEvent.ItemId];
		if (tItem.GoodsType == EGoodsType.Note)
		{
			if (unitEvent.SkillInputType == ESkillInputEvent.ItemSkillDown)
			{
				GameHost.SendToClient(ClientId, new NtfItemUsed
				{
					ClientId = ClientId,
					ItemId = unitEvent.ItemId
				});
			}
		}
		else if (tItem.GoodsType == EGoodsType.Blueprint && unitEvent.SkillInputType == ESkillInputEvent.ItemSkillDown)
		{
			HandleUnitEvent(new ConsumeItemEvent
			{
				SourceModel = UnitModel,
				ItemId = unitEvent.ItemId
			});
		}
	}

	private void OnTryInteractEvent(TryInteractEvent unitEvent)
	{
		StateMachine.TriggerEvent(unitEvent);
	}

	private void OnCancelInteractEvent(CancelInteractEvent unitEvent)
	{
		StateMachine.TriggerEvent(unitEvent);
	}

	private void OnEscapeEvent(EscapeEvent unitEvent)
	{
		StateMachine.TriggerEvent(unitEvent);
	}

	private void OnLightFuelDamageEvent(LightFuelDamageEvent unitEvent)
	{
		if (_gameManager.IsInLounge || UnitModel.IsNonCombat || !UnitModel.IsHitable || CharacterModel.LightFuel < 1)
		{
			return;
		}
		IList<BuffEffectInfo<BrightnessDamageTakenAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<BrightnessDamageTakenAmplifierEffect>();
		int num = unitEvent.Amount;
		if (buffEffects.Any())
		{
			float num2 = 1f;
			if (unitEvent.SkillId >= 1 && TSkill.Table.TryGetValue(unitEvent.SkillId, out var value))
			{
				ESkillTagType[] skillTags = value.SkillTags;
				float num3 = 0f;
				float num4 = 0f;
				foreach (BuffEffectInfo<BrightnessDamageTakenAmplifierEffect> item in buffEffects)
				{
					ESkillTagType[] effTags = item.BuffEffect.SkillTags;
					float num5 = (float)item.Stack * item.BuffEffect.Percent;
					if (effTags == null || effTags.Length == 0 || effTags.Contains(ESkillTagType.None))
					{
						num3 += num5;
					}
					else if (skillTags != null && skillTags.Any((ESkillTagType t) => effTags.Contains(t)))
					{
						num4 += num5;
					}
				}
				if (Mathf.Abs(num4 + num3) > 0.0001f)
				{
					num2 = (num4 + num3) / 100f + 1f;
				}
			}
			else
			{
				float num6 = buffEffects.Where((BuffEffectInfo<BrightnessDamageTakenAmplifierEffect> e) => e.BuffEffect.SkillTags == null || e.BuffEffect.SkillTags.Length == 0 || e.BuffEffect.SkillTags.Contains(ESkillTagType.None)).Sum((BuffEffectInfo<BrightnessDamageTakenAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent);
				if (Mathf.Abs(num6) > 0.0001f)
				{
					num2 = num6 / 100f + 1f;
				}
			}
			num2 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			num = (int)((float)unitEvent.Amount * num2);
		}
		int num7 = Math.Max(num, 1);
		if (num7 > 1)
		{
			_logger.Debug($"[LightFuelDamageEvent] Amplified Light Fuel Damage: {num7}");
		}
		CharacterModel.LightFuel = (int)Mathf.Max(0f, CharacterModel.LightFuel - num7);
		GameHost.SendToAll(new NtfLightFuelDamage
		{
			NetworkObjectId = base.NetworkObjectId,
			Amount = num7,
			HitPosition = unitEvent.HitPosition.ToProtoVector3()
		});
		TriggerEvent(unitEvent);
	}

	private void OnLightFuelRecoveryEvent(LightFuelRecoveryEvent unitEvent)
	{
		if (UnitModel.IsNonCombat || !UnitModel.IsHitable || CharacterModel.LightFuel >= TConst.LightFuelMax.IntValue)
		{
			return;
		}
		IList<BuffEffectInfo<BrightnessRecoveryAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<BrightnessRecoveryAmplifierEffect>();
		float num = 1f;
		if (buffEffects.Any())
		{
			num = buffEffects.Sum((BuffEffectInfo<BrightnessRecoveryAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
		}
		if (unitEvent.SkillId > 0 && TSkill.Table[unitEvent.SkillId].SkillTags.Contains(ESkillTagType.Item))
		{
			IList<BuffEffectInfo<ItemEffectAmplifierEffect>> buffEffects2 = UnitModel.GetBuffEffects<ItemEffectAmplifierEffect>();
			if (buffEffects2.Any())
			{
				num = (buffEffects2.Sum((BuffEffectInfo<ItemEffectAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f) * num;
			}
		}
		num = Mathf.Clamp(num, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
		int num2 = Math.Max((int)((float)unitEvent.Amount * num), 1);
		CharacterModel.LightFuel = Math.Min(CharacterModel.LightFuel + num2, TConst.LightFuelMax.IntValue);
		GameHost.SendToAll(new NtfLightFuelRecovery
		{
			NetworkObjectId = base.NetworkObjectId,
			Amount = num2
		});
		TriggerEvent(unitEvent);
	}

	private void OnStageReadyEvent(StageReadyEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected override void OnNoticeDamage(NoticeDamageEvent unitEvent)
	{
		base.OnNoticeDamage(unitEvent);
		if (UnitModel.IsHitable && unitEvent.SourceModel == UnitModel)
		{
			TryRecoveryStressOnCritical();
		}
		void TryRecoveryStressOnCritical()
		{
			float timeNow = GameManager.TimeNow;
			TConst stressRecoveryPercentOnCritical = TConst.StressRecoveryPercentOnCritical;
			if (unitEvent.IsCritical && !(timeNow < _recoveryOnCriticalCoolEndTime) && RandomUtility.GetRandomBoolFromPercentage(stressRecoveryPercentOnCritical.FloatValue))
			{
				int intValue = TConst.StressRecoveryOnCritical.IntValue;
				_recoveryOnCriticalCoolEndTime = timeNow + TConst.StressRecoveryCoolTimeOnCritical.FloatValue;
				HandleUnitEvent(new StressRecoveryEvent
				{
					Amount = intValue
				});
			}
		}
	}

	protected override void OnNoticeDie(NoticeDieEvent unitEvent)
	{
		base.OnNoticeDie(unitEvent);
		if (UnitModel.IsHitable)
		{
			DamageStressNearDeadAlly();
			RecoveryStressNearDeadEnemy();
		}
		void DamageStressNearDeadAlly()
		{
			UnitModel targetModel = unitEvent.TargetModel;
			if (targetModel == null || targetModel == UnitModel || targetModel.UnitType != EUnitType.Character || (UnitModel.Position - targetModel.Position).magnitude > TConst.StressDamageTriggerAllyDiedRange.FloatValue)
			{
				return;
			}
			HandleUnitEvent(new StressDamageEvent
			{
				SourceModel = targetModel,
				Amount = TConst.StressDamageOnAllyDied.IntValue,
				HitPosition = UnitModel.Position,
				ReasonType = EStressReasonType.AllyDead
			});
		}
		void RecoveryStressNearDeadEnemy()
		{
			UnitModel sourceModel = unitEvent.SourceModel;
			UnitModel targetModel2 = unitEvent.TargetModel;
			if (targetModel2 == null || targetModel2.UnitType != EUnitType.Monster)
			{
				return;
			}
			TMonster tMonster = TMonster.Table[targetModel2.TableId];
			int recoveryStressAmountOnDead = tMonster.RecoveryStressAmountOnDead;
			float recoveryStressRangeOnDead = tMonster.RecoveryStressRangeOnDead;
			if (recoveryStressAmountOnDead < 1)
			{
				return;
			}
			GameHost.SendToAll(new NtfUnitMonologueText
			{
				NetworkObjectId = base.NetworkObjectId,
				TextKey = TLocalizedText.MindHealing.Id
			});
			if (recoveryStressRangeOnDead < 0f && sourceModel == UnitModel)
			{
				HandleUnitEvent(new StressRecoveryEvent
				{
					Amount = recoveryStressAmountOnDead
				});
			}
			if (!(recoveryStressRangeOnDead < 0f) && !(recoveryStressRangeOnDead < Vector3.Distance(UnitModel.Position, targetModel2.Position)))
			{
				HandleUnitEvent(new StressRecoveryEvent
				{
					Amount = recoveryStressAmountOnDead
				});
			}
		}
	}

	protected override void OnNoticeRevive(NoticeReviveEvent unitEvent)
	{
		base.OnNoticeRevive(unitEvent);
		if (UnitModel.IsHitable)
		{
			RecoveryStressNearRevivedAlly(unitEvent.TargetModel);
		}
		void RecoveryStressNearRevivedAlly(UnitModel targetModel)
		{
			if (targetModel == null || targetModel == UnitModel || targetModel.UnitType != EUnitType.Character)
			{
				return;
			}
			if (!((targetModel.Position - UnitModel.Position).magnitude > TConst.StressRecoveryTriggerAllyReviveRange.FloatValue))
			{
				HandleUnitEvent(new StressRecoveryEvent
				{
					Amount = (float)TConst.StressRecoveryOnAllyRevive.IntValue
				});
			}
		}
	}

	private void OnStressDamageEvent(StressDamageEvent unitEvent)
	{
		if (_gameManager.IsInLounge)
		{
			return;
		}
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (unitEvent.ReasonType != EStressReasonType.Dead && !UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Stress >= (float)TConst.StressMax.IntValue)
		{
			return;
		}
		float num = unitEvent.Amount;
		if (!unitEvent.IsFixed)
		{
			float num2 = 1f;
			IList<BuffEffectInfo<StressDamageTakenAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<StressDamageTakenAmplifierEffect>(true);
			if (buffEffects.Any())
			{
				if (unitEvent.SkillId > 0 && TSkill.Table.TryGetValue(unitEvent.SkillId, out var value))
				{
					ESkillTagType[] skillTags = value.SkillTags;
					float num3 = 0f;
					float num4 = 0f;
					foreach (BuffEffectInfo<StressDamageTakenAmplifierEffect> item in buffEffects)
					{
						ESkillTagType[] effTags = item.BuffEffect.SkillTags;
						float num5 = (float)item.Stack * item.BuffEffect.Percent;
						if (effTags == null || effTags.Length == 0 || effTags.Contains(ESkillTagType.None))
						{
							num3 += num5;
						}
						else if (skillTags != null && skillTags.Any((ESkillTagType t) => effTags.Contains(t)))
						{
							num4 += num5;
						}
					}
					if (Mathf.Abs(num4 + num3) > 0.0001f)
					{
						num2 = (num4 + num3) / 100f + 1f;
					}
				}
				else
				{
					float num6 = buffEffects.Where((BuffEffectInfo<StressDamageTakenAmplifierEffect> e) => e.BuffEffect.SkillTags == null || e.BuffEffect.SkillTags.Length == 0 || e.BuffEffect.SkillTags.Contains(ESkillTagType.None)).Sum((BuffEffectInfo<StressDamageTakenAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent);
					if (Mathf.Abs(num6) > 0.0001f)
					{
						num2 = num6 / 100f + 1f;
					}
				}
			}
			num2 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			num = Mathf.Max(num2 * num, 1f);
			if (unitEvent.ReasonType != EStressReasonType.None && unitEvent.ReasonType != EStressReasonType.Natural)
			{
				num = Mathf.Max(num * TDifficulty.Table[_gameContext.Difficulty].StressDamagePercent / 100f, 1f);
			}
		}
		UnitModel.Stress = Mathf.Min(UnitModel.Stress + num, (float)TConst.StressMax.IntValue);
		GameHost.SendToAll(new NtfStressDamage
		{
			NetworkObjectId = base.NetworkObjectId,
			Amount = num,
			HitPosition = unitEvent.HitPosition.ToProtoVector3(),
			StressReasonType = unitEvent.ReasonType
		});
		if (unitEvent.ShowDamageEffect)
		{
			GameHost.SendToAll(new NtfDisplayFx
			{
				Vfx = "Common/StressDamage",
				VfxDuration = 3f,
				Sfx = "San_Down",
				Position = UnitModel.Position.ToProtoVector3(),
				TargetNetworkObjectId = base.NetworkObjectId,
				TargetPositionOffset = new ProtoVector3(),
				BoneType = EUnitBoneType.Head
			});
		}
		TriggerEvent(unitEvent);
	}

	private void OnStressRecoveryEvent(StressRecoveryEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (!UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Stress < 0f)
		{
			return;
		}
		if (CharacterModel.HasBuffEffect(EBuffEffectType.BlockStressRecovery))
		{
			GameHost.SendToAll(new NtfUnitMonologueText
			{
				NetworkObjectId = base.NetworkObjectId,
				TextKey = TLocalizedText.NoPointInDoingThis.Id
			});
			return;
		}
		float num = unitEvent.Amount;
		if (!unitEvent.IsFixed)
		{
			float num2;
			IList<BuffEffectInfo<StressRecoveryAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<StressRecoveryAmplifierEffect>(true);
			if (!buffEffects.Any())
			{
				num2 = 1f;
			}
			else
			{
				num2 = buffEffects.Sum((BuffEffectInfo<StressRecoveryAmplifierEffect> _) => (float)_.Stack * _.BuffEffect.Percent) / 100f + 1f;
			}
			if (unitEvent.SkillId > 0 && TSkill.Table[unitEvent.SkillId].SkillTags.Contains(ESkillTagType.Item))
			{
				IList<BuffEffectInfo<ItemEffectAmplifierEffect>> buffEffects2 = UnitModel.GetBuffEffects<ItemEffectAmplifierEffect>(true);
				if (buffEffects2.Any())
				{
					num2 = (buffEffects2.Sum((BuffEffectInfo<ItemEffectAmplifierEffect> _) => (float)_.Stack * _.BuffEffect.Percent) / 100f + 1f) * num2;
				}
			}
			num2 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			num = Mathf.Max(num2 * unitEvent.Amount, 1f);
		}
		UnitModel.Stress = Mathf.Max(UnitModel.Stress - num, 0f);
		GameHost.SendToAll(new NtfStressRecovery
		{
			NetworkObjectId = base.NetworkObjectId,
			Amount = num
		});
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = "Common/StressRecovery",
			VfxDuration = 3f,
			Sfx = "San_Up",
			Position = UnitModel.Position.ToProtoVector3(),
			TargetNetworkObjectId = base.NetworkObjectId,
			TargetPositionOffset = new ProtoVector3(),
			BoneType = EUnitBoneType.Head
		});
		TriggerEvent(unitEvent);
	}

	private void OnStaminaDamageEvent(StaminaDamageEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (!UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Stamina <= 0f)
		{
			return;
		}
		UnitModel.Stamina = Mathf.Max(UnitModel.Stamina - unitEvent.Amount, 0f);
		TriggerEvent(unitEvent);
	}

	private void OnStaminaRecoveryEvent(StaminaRecoveryEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (!UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Stamina >= UnitModel.StaminaMax)
		{
			return;
		}
		UnitModel.Stamina = Math.Min(unitEvent.Amount + UnitModel.Stamina, UnitModel.StaminaMax);
		TriggerEvent(unitEvent);
	}

	private void OnSoulDamageEvent(SoulDamageEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (!UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Soul < 1)
		{
			return;
		}
		UnitModel.Soul = (int)Mathf.Max((float)(UnitModel.Soul - unitEvent.Amount), 0f);
		TriggerEvent(unitEvent);
	}

	private void OnSoulRecoveryEvent(SoulRecoveryEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return;
		}
		if (!UnitModel.IsHitable)
		{
			return;
		}
		if (UnitModel.Soul >= TConst.SoulMax.IntValue)
		{
			return;
		}
		UnitModel.Soul = Math.Min(UnitModel.Soul + unitEvent.Amount, TConst.SoulMax.IntValue);
		if (UnitModel.Soul != TConst.SoulMax.IntValue)
		{
			TriggerEvent(unitEvent);
			return;
		}
		Observable.Interval(TimeSpan.FromSeconds(0.1f)).Where((long _) => UnitModel.State != EActionState.Skill).Take(1)
			.Subscribe(delegate(long _)
			{
				if (UnitModel.IsHitable)
				{
					TriggerEvent(unitEvent);
				}
				else
				{
					UnitModel.Soul = TConst.SoulMax.IntValue - 1;
				}
			})
			.AddTo(this);
	}

	private void OnCutsceneStartEvent(CutsceneStartEvent unitEvent)
	{
		StateMachine.TriggerEvent(unitEvent);
	}

	private void OnCutsceneFinishEvent(CutsceneFinishEvent unitEvent)
	{
		StateMachine.TriggerEvent(unitEvent);
	}

	private void DecreaseAccessoryDurability()
	{
		if (_gameManager.IsInLounge)
		{
			return;
		}
		Character character = Player.Character.Clone();
		float floatValue = TConst.DurabilityDecreaseValueOnHit.FloatValue;
		bool flag = false;
		List<int> list = new List<int>();
		GoodsSlot[] accessorySlots = character.AccessorySlots;
		foreach (GoodsSlot goodsSlot in accessorySlots)
		{
			if (goodsSlot.Goods is AccessoryGoods accessory && accessory.HasDurability() && !accessory.IsBrokenEquipment())
			{
				float newCurrentDurability = accessory.CurrentDurability - floatValue;
				if (newCurrentDurability < 0f)
				{
					newCurrentDurability = 0f;
				}
				goodsSlot.Goods = accessory.CloneWithDurability(newCurrentDurability);
				flag = true;
				_logger.Debug(string.Format("[Durability] Accessory decreased: ID={0}, Amount=-{1:F2}, Current={2:F2}/{3}", accessory.Id, floatValue, newCurrentDurability, accessory.MaxDurability));
				if (goodsSlot.Goods.IsBrokenEquipment())
				{
					list.Add(accessory.Id);
					_logger.Debug(string.Format("[Durability] Accessory broken: ID={0}", accessory.Id));
				}
			}
		}
		if (!flag)
		{
			return;
		}
		Player.Character = character;
		if (list.Count < 1)
		{
			return;
		}
		CharacterModel.RefreshEquipmentEffects(base.IsHost);
		foreach (int item in list)
		{
			OnEquipmentBroken(item);
		}
	}

	public void DecreaseWeaponDurability(float decreaseAmount)
	{
		if (_gameManager.IsInLounge)
		{
			return;
		}
		Character character = Player.Character.Clone();
		if (character.ActiveWeaponSlot.Goods is WeaponGoods weapon && weapon.HasDurability() && !weapon.IsBrokenEquipment())
		{
			float newCurrentDurability = Mathf.Max(0f, weapon.CurrentDurability - decreaseAmount);
			WeaponGoods weaponGoods = weapon.CloneWithDurability(newCurrentDurability);
			character.WeaponSlots[character.ActiveWeaponIndex].Goods = weaponGoods;
			bool flag = weaponGoods.IsBrokenEquipment();
			Player.Character = character;
			if (flag)
			{
				CharacterModel.RefreshEquipmentEffects(base.IsHost);
				OnEquipmentBroken(weapon.Id);
			}
		}
	}

	private void TryShowAllyDamageBubble(HpDamageEvent unitEvent)
	{
		if (!(unitEvent.SourceModel == null) && unitEvent.SourceModel is CharacterModel characterModel && !(characterModel == (UnitModel as CharacterModel)))
		{
			if (characterModel.TargetHitLayer != CharacterModel.TargetHitLayer)
			{
				return;
			}
			if (characterModel.HasBuffEffect(EBuffEffectType.Madness))
			{
				GameHost.SendToAll(new NtfUnitBubbleText
				{
					NetworkObjectId = CharacterModel.NetworkObjectId,
					TextKey = TLocalizedText.MadnessDamageProvideText.Id
				});
				GameHost.SendToAll(new NtfUnitBubbleText
				{
					NetworkObjectId = characterModel.NetworkObjectId,
					TextKey = TLocalizedText.MadnessDamageText.Id
				});
			}
		}
	}

	private void OnEquipmentBroken(int equipmentId)
	{
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = "Status/WeaponBroken",
			VfxDuration = 3f,
			Sfx = "WeaponBroken",
			Position = UnitModel.Position.ToProtoVector3()
		});
		GameHost.SendToAll(new NtfEquipmentBroken
		{
			NetworkObjectId = base.NetworkObjectId,
			EquipmentId = equipmentId
		});
	}



}
