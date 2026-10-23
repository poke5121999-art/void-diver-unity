using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using De.InGame.Fsm;
using De.InGame.Unit;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : UnitController
{
	public class Args
	{
		public int MonsterId;

		public int MonsterSpawnId;

		public ESpawnType SpawnType;

		public Vector3 Forward;

		public float LifeTime;

		public int HitLayer;

		public ulong TargetNetworkObjectId;

		public ulong SummonerNetworkObjectId;

		public bool DieOnSummonerDeath;

		public Vector3 WaveStartPosition;

		public bool IsPatrolMonster;

		public Vector3[] PatrolWanderPositions;

		public bool UseFixedAimPos;

		public Vector3 FixedAimPos;
	}

	private Args _args;

	private MonsterStateMachine _monsterStateMachine;

	private PlaneSightDetectableObject _planeSightDetectableObject;

	private OutlineObject _outlineObject;

	private readonly Subject<Unit> _combatEventSubject = new Subject<Unit>();

	private readonly NetworkVariable<Vector3> _syncInitForward = new NetworkVariable<Vector3>(Vector3.zero);

	private readonly NetworkVariable<ESpawnType> _syncInitSpawnType = new NetworkVariable<ESpawnType>();

	private readonly List<MonsterModel> _summonMonsterModels = new List<MonsterModel>();

	private List<ulong> _lightDetectingUnitIds = new List<ulong>();

	private float _nextFleeCheckTime;

	public NavMeshAgent NavMeshAgent => UnitView.Agent;

	protected override UnitStateMachine StateMachine => _monsterStateMachine;

	private TMonster MonsterData => TMonster.Table[MonsterModel.MonsterId];

	public override ulong ClientId => OwnerClientId;

	private MonsterView MonsterView => UnitView as MonsterView;

	private MonsterModel MonsterModel => UnitModel as MonsterModel;

	private PlaneSightDetectableObject PlaneSightDetectableObject
	{
		get
		{
			if (_planeSightDetectableObject == null)
			{
				_planeSightDetectableObject = GetComponentInChildren<PlaneSightDetectableObject>();
			}
			return _planeSightDetectableObject;
		}
	}

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

	public int MonsterId => MonsterModel.MonsterId;

	public int MonsterSpawnId => MonsterModel.MonsterSpawnId;

	public bool IsActivateInStage => MonsterModel.IsActivateInStage;

	public ESpawnType SpawnType => _args.SpawnType;

	public bool HasLifeTime => _args.LifeTime > 0f;

	public Vector3 WaveStartPosition => _args.WaveStartPosition;

	public bool IsPatrolMonster => _args.IsPatrolMonster;

	public bool UseFixedAimPos => _args.UseFixedAimPos;

	public Vector3 FixedAimPos => _args.FixedAimPos;

	private float TimeNow => GameManager.TimeNow;

	public bool IsBoss => MonsterModel.IsBoss;

	public bool IsBasePhase => MonsterData.IsBasePhaseMonster;

	public int SummonMonsterCount => _summonMonsterModels.Count;

	private void TriggerCombatEvent()
	{
		_combatEventSubject.OnNext(Unit.Default);
	}

	public void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	public override void OnNetworkSpawn()
	{
		if (base.IsHost)
		{
			_syncInitForward.Value = _args.Forward;
			_syncInitSpawnType.Value = _args.SpawnType;
			MonsterModel.SyncHitLayer.Value = _args.HitLayer;
			MonsterModel.SyncMonsterSpawnId.Value = _args.MonsterSpawnId;
			MonsterModel.MonsterId = _args.MonsterId;
		}
		base.OnNetworkSpawn();
		if (base.IsHost)
		{
			MonsterView.InitAgent();
			_monsterStateMachine = new MonsterStateMachine(this, _args.IsPatrolMonster);
			if (_args.SummonerNetworkObjectId != 0)
			{
				MonsterModel.Summoner = ObjectManager.FindUnitModel(_args.SummonerNetworkObjectId);
				if (_args.DieOnSummonerDeath && MonsterModel.Summoner != null)
				{
					MonsterModel.Summoner.RxState.Where((EActionState state) => state == EActionState.Dying || state == EActionState.Dead).Subscribe(delegate
					{
						HandleUnitEvent(new DieEvent());
					}).AddTo(this);
				}
			}
			TMonster monsterData = TMonster.Table[MonsterModel.MonsterId];
			int dropRewardGroupId = ((!TMonsterSpawn.Table.TryGetValue(MonsterModel.MonsterSpawnId, out var spawnData)) ? monsterData.DropRewardGroupId : spawnData.DropRewardGroupId);
			Inventory[] inventories = MonsterModel.BodyObject.GetComponents<Inventory>();
			for (int i = 0; i < inventories.Length; i++)
			{
				IList<GoodsSlot> slots = CreateInventorySlots(dropRewardGroupId);
				inventories[i].Init(slots, Math.Max(slots?.Count ?? 0, 1));
			}
			if (_args.LifeTime > 0f)
			{
				Observable.Timer(TimeSpan.FromSeconds(_args.LifeTime)).Subscribe(delegate
				{
					if (UnitModel.State != EActionState.Dying && UnitModel.State != EActionState.Dead)
					{
						MonsterModel.SyncIsLifeTimeFinished.Value = true;
						HandleUnitEvent(new DieEvent());
					}
				}).AddTo(this);
			}
			MonsterModel.RxIsInCombat.Subscribe(delegate(bool enterCombat)
			{
				if (enterCombat)
				{
					monsterData.OnCombatBuffIds.ForEach(delegate(int id)
					{
						HandleUnitEvent(new BuffEvent
						{
							BuffId = id,
							BuffStack = 1
						});
					});
					monsterData.OnCuriousBuffIds.ForEach(delegate(int id)
					{
						MonsterModel.RemoveBuff(id, 1);
					});
				}
				else
				{
					monsterData.OnCombatBuffIds.ForEach(delegate(int id)
					{
						MonsterModel.RemoveBuff(id, 1);
					});
				}
			}).AddTo(this);
			_combatEventSubject.Subscribe(delegate
			{
				OnCombatEvent();
			}).AddTo(this);
			MonsterModel.RxState.Where((EActionState state) => state.IsCombatState()).Subscribe(delegate
			{
				TriggerCombatEvent();
			}).AddTo(this);
			MonsterModel.RxWeakPointType.Subscribe(OnWeakPointTypeChanged).AddTo(this);
			MonsterModel.RxStats.ObserveChange().Subscribe(delegate
			{
				OnStatChanged();
			}).AddTo(this);
			UniTask.DelayFrame(1).ContinueWith(delegate
			{
				if (_args.SpawnType == ESpawnType.Wave || _args.SpawnType == ESpawnType.SummonActionEvent)
				{
					_monsterStateMachine.SetInitialState(EActionState.Spawn);
				}
			}).Forget();
		}
		base.name = string.Format("{0}", MonsterModel.MonsterId);
		MonsterModel.Init(_syncInitForward.Value, _syncInitSpawnType.Value);
		if (base.IsHost)
		{
			if (GameContext.IsInStage)
			{
				TAnomaly.Table[GameContext.AnomalyType].MonsterBuffIds?.ForEach(delegate(int id)
				{
					MonsterModel.AddBuff(id, MonsterModel);
				});
			}
			if (_args.UseFixedAimPos)
			{
				MonsterModel.UseFixedAimPos = true;
				MonsterModel.FixedAimPos = _args.FixedAimPos;
				MonsterModel.CalculatedDeltaAimPos = _args.FixedAimPos;
			}
		}
		MonsterView.Init();
		MonsterModel.RxState.Where((EActionState _) => _ == EActionState.Dead).Subscribe(delegate
		{
			OnDead();
		}).AddTo(this);
		MonsterModel.RxState.Where((EActionState _) => _ == EActionState.Captured).Subscribe(delegate
		{
			OnCaptured();
		}).AddTo(this);
		MonsterModel.RxState.Subscribe(delegate
		{
			OnStateChanged();
		}).AddTo(this);
		if (base.IsHost && _args.TargetNetworkObjectId != 0)
		{
			MonsterModel.SetTarget(ObjectManager.FindUnitModel(_args.TargetNetworkObjectId));
			if (_args.SpawnType == ESpawnType.SummonActionEvent)
			{
				MonsterModel.IsInCombat = true;
				MonsterModel.CombatExitTime = TimeNow + TConst.MonsterCombatExitDelay.FloatValue;
			}
			if (MonsterModel.IsShadow)
			{
				MonsterModel.OriginalTargetNetworkObjectId = _args.TargetNetworkObjectId;
			}
		}
		MonsterModel.RxTargetNetworkObjectId.Subscribe(OnTargetChanged).AddTo(this);
		GameManager.RxViewTargetClientId.Subscribe(delegate
		{
			if (MonsterModel.IsShadow)
			{
				UpdateShadowMonsterEffect();
			}
		}).AddTo(this);
		if (base.IsHost)
		{
			UnitModel.PassiveSkillIds.ForEach(delegate(int id)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = id
				});
			});
			if (_args.IsPatrolMonster)
			{
				MonsterModel.PatrolWanderPositions = _args.PatrolWanderPositions;
			}
		}
		base.gameObject.name = string.Format("[Monster] {0}", MonsterModel.MonsterId);
	}

	protected override void OnNetworkUpdate()
	{
		base.OnNetworkUpdate();
		if (base.IsHost)
		{
			OnUpdateLightCount();
			CooldownWeakPointDamage();
		}
	}

	protected override void OnUpdateSec()
	{
		base.OnUpdateSec();
		UpdateLightStackBuff();
		CheckCombatExitTime();
		UpdateSuppressionTick();
	}

	private IList<GoodsSlot> CreateInventorySlots(int dropRewardGroupId)
	{
		if (dropRewardGroupId < 1)
		{
			return null;
		}
		return TDropReward.Table.PickRewards(dropRewardGroupId, GameContext.CampaignId, MissionManager.MissionIds.ToArray(), GameContext.Difficulty, GameContext.GetPartyWeaponTypes()).Select((Goods goods, int index) => new GoodsSlot
		{
			SlotIndex = index,
			Goods = goods,
			IsUnrevealed = true
		}).ToList();
	}

	private void OnCaptured()
	{
		float scale = Math.Min(TConst.CapturedMonsterScale.FloatValue / UnitModel.HitRadius, 1f);
		base.transform.localScale = new Vector3(scale, scale, scale);
	}

	private void OnDead()
	{
		if (MonsterModel.BodyObject != null)
		{
			MonsterBody monsterBody = MonsterModel.BodyObject.GetComponent<MonsterBody>();
			LootingInventory lootingInventory = monsterBody.FindInventory(GameManager.LocalClientId);
			if (lootingInventory != null)
			{
				lootingInventory.OnInventoryChanged.Subscribe(delegate
				{
					OnStateChanged();
				}).AddTo(this);
			}
		}
	}

	private void OnStateChanged()
	{
		if (MonsterModel.State != EActionState.Dead)
		{
			OutlineObject.SetEnable(enable: false);
		}
		else if (MonsterModel.BodyObject != null)
		{
			MonsterBody monsterBody = MonsterModel.BodyObject.GetComponent<MonsterBody>();
			LootingInventory lootingInventory = monsterBody.FindInventory(GameManager.LocalClientId);
			if (lootingInventory != null)
			{
				OutlineObject.SetEnable(!lootingInventory.IsEmpty);
			}
		}
	}

	private void OnTargetChanged(ulong targetNetworkObjectId)
	{
		if (MonsterModel.IsShadow)
		{
			UpdateShadowMonsterEffect();
		}
		if (!base.IsHost)
		{
			return;
		}
		if (MonsterModel.IsShadow)
		{
			bool originalTargetAlive = false;
			if (MonsterModel.OriginalTargetNetworkObjectId != 0)
			{
				ITarget originalTarget = ObjectManager.FindTarget(MonsterModel.OriginalTargetNetworkObjectId);
				if (originalTarget is IHitable hitable && hitable.IsHitable)
				{
					if (!MonsterModel.HasTarget)
					{
						MonsterModel.SetTarget(originalTarget);
					}
					originalTargetAlive = true;
				}
			}
			if (!originalTargetAlive)
			{
				HandleUnitEvent(new DieEvent());
			}
		}
		if (MonsterModel.HasTarget && MonsterModel.Target is CharacterModel characterModel)
		{
			GameHost.SendToAll(new NtfMonsterTargetChanged
			{
				NetworkObjectId = base.NetworkObjectId,
				TargetClientId = characterModel.ClientId,
				IsBoss = MonsterModel.IsBoss
			});
		}
		if (MonsterModel.HasTarget && MonsterModel.Target is MonsterModel && MonsterData.FactionId > 0 && TFaction.Table.TryGetValue(MonsterData.FactionId, out var faction) && faction.EnemyFactionMonologue.HasValue())
		{
			GameHost.SendToAll(new NtfUnitMonologueText
			{
				NetworkObjectId = MonsterModel.NetworkObjectId,
				TextKey = faction.EnemyFactionMonologueKey
			});
		}
	}

	private void OnViewTargetChanged()
	{
		if (MonsterModel.IsShadow)
		{
			UpdateShadowMonsterEffect();
		}
	}

	private void UpdateShadowMonsterEffect()
	{
		ITarget target = MonsterModel.Target;
		if (target != null && target is CharacterModel characterModel)
		{
			MonsterView.SetShadowMonsterEffect(characterModel.ClientId == GameManager.RxViewTargetClientId.Value);
		}
	}

	private void OnCombatEvent()
	{
		AssertIfNotHost();
		MonsterModel.CombatExitTime = TimeNow + TConst.MonsterCombatExitDelay.FloatValue;
		MonsterModel.IsInCombat = true;
	}

	private void CheckCombatExitTime()
	{
		AssertIfNotHost();
		if (MonsterModel.CombatExitTime <= TimeNow)
		{
			MonsterModel.IsInCombat = false;
		}
	}

	private void CooldownWeakPointDamage()
	{
		if (!(MonsterModel.WeakPointStackDamage <= 0f) && !(TimeNow <= MonsterModel.LastWeakPointDamageTime + TConst.WeakPointGaugeCooldownDelay.FloatValue))
		{
			float decrease = TConst.WeakPointGaugeCooldownPercentPerSencod.FloatValue / 100f * MonsterModel.MaxWeakPointStackDamage * (1f / (float)base.NetworkManager.NetworkTickSystem.TickRate);
			MonsterModel.WeakPointStackDamage = Math.Max(0f, MonsterModel.WeakPointStackDamage - decrease);
		}
	}

	private void OnStatChanged()
	{
		Dictionary<EStatType, EElementalType> map = new Dictionary<EStatType, EElementalType>();
		map.Add(EStatType.FireResistancePercent, EElementalType.Fire);
		map.Add(EStatType.WaterResistancePercent, EElementalType.Water);
		map.Add(EStatType.WindResistancePercent, EElementalType.Wind);
		var weakest = map.Select((KeyValuePair<EStatType, EElementalType> kv) => new
		{
			StatType = kv.Key,
			ElementalType = kv.Value,
			Value = MonsterModel.GetStat(kv.Key)
		}).Where(x => x.Value < 0f).OrderBy(x => x.Value).ThenBy(x => x.StatType).FirstOrDefault();
		MonsterModel.WeakPointElementalType = ((weakest != null) ? weakest.ElementalType : EElementalType.None);
	}

	private void OnWeakPointTypeChanged(EElementalType type)
	{
		MonsterModel.WeakPointStackDamage = 0f;
		MonsterModel.LastWeakPointDamageTime = 0f;
		MonsterModel.LastWeakPointFullStackDamageTime = float.NegativeInfinity;
	}

	protected override void RegisterClientHandlers()
	{
	}

	protected override void RegisterHostHandlers()
	{
		RegisterCheatHandlers();
	}

	public void AddSummonMonsterModel(MonsterModel model)
	{
		_summonMonsterModels.Add(model);
	}

	public void RemoveSummonMonsterModel(MonsterModel model)
	{
		_summonMonsterModels.Remove(model);
	}

	public bool CanSummonMonster()
	{
		return _summonMonsterModels.Count < TMonster.Table[MonsterModel.MonsterId].SummonMonsterLimit;
	}

	private void OnUpdateLightCount()
	{
		TMonster monsterData = TMonster.Table[MonsterModel.MonsterId];
		if (monsterData.LightBuffId == null || monsterData.LightBuffId.Length == 0)
		{
			return;
		}
		_lightDetectingUnitIds = PlaneSightDetectableObject.DetectingPlaneSightUnits.Select((PlaneSightUnit unit) => unit.UnitModel.NetworkObjectId).ToList();
		int[] lightBuffIds = monsterData.LightBuffId;
		foreach (int buffId in lightBuffIds)
		{
			if (!TBuff.Table.TryGetValue(buffId, out var buffData))
			{
				continue;
			}
			int currentStack = (MonsterModel.Buffs.ContainsKey(buffId) ? MonsterModel.Buffs[buffId].StackCount : 0);
			int detectedCount = PlaneSightDetectableObject.RxDetectedCount.Value;
			if (currentStack < detectedCount)
			{
				int addStack = Math.Min(detectedCount, buffData.MaxStackCount) - currentStack;
				if (addStack < 1)
				{
					continue;
				}
				HandleUnitEvent(new BuffEvent
				{
					BuffId = buffId,
					BuffStack = addStack
				});
			}
			else if (detectedCount < currentStack)
			{
				int removeStack = currentStack - detectedCount;
				if (removeStack < 1)
				{
					continue;
				}
				MonsterModel.RemoveBuff(buffId, removeStack);
			}
			MonsterModel.UpdateBuffCasterId(buffId, _lightDetectingUnitIds);
		}
	}

	private void UpdateLightStackBuff()
	{
		if (MonsterData.LightStackBuffId == null || MonsterData.LightStackBuffId.Length == 0)
		{
			return;
		}
		int[] lightStackBuffIds = MonsterData.LightStackBuffId;
		foreach (int buffId in lightStackBuffIds)
		{
			if (_lightDetectingUnitIds.Count < 1)
			{
				MonsterModel.RemoveBuff(buffId, 1);
				continue;
			}
			HandleUnitEvent(new BuffEvent
			{
				BuffId = buffId,
				BuffStack = _lightDetectingUnitIds.Count
			});
			MonsterModel.UpdateBuffCasterId(buffId, _lightDetectingUnitIds);
		}
	}

	private void RegisterCheatHandlers()
	{
		RegisterHostHandler<ReqCheatApplyMonsterHp>(OnCheatApplyHp);
	}

	private void OnCheatApplyHp(ReqCheatApplyMonsterHp message)
	{
		if (MonsterModel.Hp > 0)
		{
			MonsterModel.Hp = message.Hp;
		}
	}

	private void OnBuffEventForSuppression(BuffEvent unitEvent)
	{
		if (MonsterModel.HasSuppressionSystem && !MonsterModel.IsSuppressionImmune && MonsterModel.State != EActionState.Groggy)
		{
			EStatusEffectTag effectTag = TBuff.Table[unitEvent.BuffId].EffectTag;
			if (!MonsterModel.HasImmunityEffect(effectTag) && MonsterModel.IsSuppressionBuffTag(effectTag) && TStatusEffectSuppression.Table.TryGetValue(effectTag, out var suppression) && suppression.Value > 0f)
			{
				HandleUnitEvent(new SuppressionDamageEvent
				{
					Amount = (float)unitEvent.BuffStack * suppression.Value
				});
			}
		}
	}

	private void AddSuppression(float amount)
	{
		if (MonsterModel.HasSuppressionSystem && !MonsterModel.IsSuppressionImmune && MonsterModel.State != EActionState.Groggy)
		{
			float remaining = MonsterModel.SuppressionGaugeValue - amount;
			if (remaining <= 0f)
			{
				MonsterModel.SuppressionGaugeValue = 0f;
				HandleUnitEvent(new GroggyEvent());
			}
			else
			{
				MonsterModel.SuppressionGaugeValue = remaining;
			}
		}
	}

	private void UpdateSuppressionTick()
	{
		if (!MonsterModel.HasSuppressionSystem || MonsterModel.IsSuppressionImmune || MonsterModel.State == EActionState.Groggy)
		{
			return;
		}
		float total = 0f;
		foreach (UnitBuff buff in MonsterModel.Buffs.Values)
		{
			if (buff.IsActive)
			{
				EStatusEffectTag effectTag = TBuff.Table[buff.Id].EffectTag;
				if (MonsterModel.IsSuppressionBuffTag(effectTag) && !MonsterModel.HasImmunityEffect(effectTag) && TStatusEffectSuppression.Table.TryGetValue(effectTag, out var suppression) && suppression.Value > 0f)
				{
					total += (float)buff.StackCount * suppression.Value;
				}
			}
		}
		if (!(total <= 0f))
		{
			HandleUnitEvent(new SuppressionDamageEvent
			{
				Amount = total
			});
		}
	}

	private void OnSuppressionDamageEvent(SuppressionDamageEvent unitEvent)
	{
		if (unitEvent.Amount > 0f)
		{
			AddSuppression(unitEvent.Amount);
		}
	}

	private void OnGroggyEvent(GroggyEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected override void RegisterUnitEventHandlers()
	{
		base.RegisterUnitEventHandlers();
		EventReceiver.Receive<ReceiveAggroSpreadEvent>().Subscribe(OnReceiveAggroSpreadEvent).AddTo(this);
		EventReceiver.Receive<ForceMonsterSetTargetEvent>().Subscribe(OnForceMonsterSetTargetEvent).AddTo(this);
		EventReceiver.Receive<TryMonsterCuriousStateEvent>().Subscribe(OnTryMonsterCuriousStateEvent).AddTo(this);
		EventReceiver.Receive<MonsterPhaseUpEvent>().Subscribe(OnMonsterPhaseUpEvent).AddTo(this);
		EventReceiver.Receive<BuffEvent>().Subscribe(OnBuffEventForSuppression).AddTo(this);
		EventReceiver.Receive<SuppressionDamageEvent>().Subscribe(OnSuppressionDamageEvent).AddTo(this);
		EventReceiver.Receive<GroggyEvent>().Subscribe(OnGroggyEvent).AddTo(this);
	}

	protected void OnReceiveAggroSpreadEvent(ReceiveAggroSpreadEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected override void OnDieEvent(DieEvent unitEvent)
	{
		base.OnDieEvent(unitEvent);
		if (!MonsterModel.IsLifeTimeFinished && unitEvent.SourceModel is CharacterModel characterModel)
		{
			GameHost.SendToAll(new NtfMonsterKill
			{
				SourceClientId = characterModel.ClientId,
				MonsterNetworkObjectId = base.NetworkObjectId,
				MonsterId = MonsterModel.MonsterId
			});
		}
	}

	protected override void OnNoticeDie(NoticeDieEvent unitEvent)
	{
		TriggerEvent(unitEvent);
		if (unitEvent.TargetModel is MonsterModel monsterModel && _summonMonsterModels.Contains(monsterModel))
		{
			_summonMonsterModels.Remove(monsterModel);
		}
	}

	protected override int CalculateDamage(HpDamageEvent unitEvent)
	{
		int damage = base.CalculateDamage(unitEvent);
		if (MonsterModel.IsShadow)
		{
			ulong sourceId = ((unitEvent.SourceModel != null) ? unitEvent.SourceModel.NetworkObjectId : 0);
			if (sourceId != 0 && sourceId != MonsterModel.OriginalTargetNetworkObjectId)
			{
				damage = (int)(TConst.ShadowMonsterDamagePercentOnHitByOther.FloatValue / 100f * (float)damage);
			}
		}
		if (unitEvent.SourceIdentity.UnitType == EUnitType.Monster)
		{
			if (TMonster.Table.TryGetValue(unitEvent.SourceIdentity.TableId, out var sourceMonster))
			{
				if (sourceMonster.FactionId != MonsterModel.FactionId)
				{
					damage = (int)((float)damage * 0.5f);
				}
			}
			else
			{
				Logger.Warning(string.Format("SourceMonster has invalid MonsterId={0}. NetObjId={1}", unitEvent.SourceIdentity.TableId, (unitEvent.SourceModel != null) ? unitEvent.SourceModel.NetworkObjectId : 0));
			}
		}
		if (MonsterModel.State == EActionState.Groggy)
		{
			damage = (int)((float)damage * TConst.GroggyDamageMultiplier.FloatValue);
		}
		return damage;
	}

	protected override void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
		base.OnHpDamageEvent(unitEvent);
		if (unitEvent.Damage < 1)
		{
			return;
		}
		if (MonsterModel.Target == null)
		{
			MonsterModel.SetTarget(unitEvent.SourceModel);
		}
		UpdateFlee();
		CharacterModel characterModel = unitEvent.SourceModel as CharacterModel;
		if (characterModel != null && MonsterModel.Target != null && RandomUtility.GetRandomFloat(0f, 100f) < TConst.MonsterRetargetToPlayerChance.FloatValue)
		{
			MonsterModel.SetTarget(characterModel);
		}
		bool isWeakPointDamage = unitEvent.ElementalDamagedType.IsPenalty();
		if (unitEvent.ReasonType == EHpDamageReasonType.WeakPointFullStack)
		{
			isWeakPointDamage = false;
		}
		if (!isWeakPointDamage || !(TimeNow > MonsterModel.LastWeakPointFullStackDamageTime + TConst.WeakPointGaugeBlockTime.FloatValue))
		{
			return;
		}
		MonsterModel.LastWeakPointDamageTime = TimeNow;
		int stackDamage = unitEvent.Damage;
		if (unitEvent.SourceModel != null)
		{
			IList<BuffEffectInfo<WeakPointStackAmplifierEffect>> amplifiers = unitEvent.SourceModel.GetBuffEffects<WeakPointStackAmplifierEffect>();
			if (amplifiers.Any())
			{
				float multiplier = amplifiers.Sum((BuffEffectInfo<WeakPointStackAmplifierEffect> e) => (float)e.Stack * e.BuffEffect.Percent) / 100f + 1f;
				multiplier = Math.Min(Math.Max(multiplier, TConst.BuffEffectAmplifierMin.FloatValue), TConst.BuffEffectAmplifierMax.FloatValue);
				stackDamage = Math.Max(1, (int)((float)unitEvent.Damage * multiplier));
			}
		}
		MonsterModel.WeakPointStackDamage += stackDamage;
		if (MonsterModel.WeakPointStackDamage < MonsterModel.MaxWeakPointStackDamage)
		{
			return;
		}
		MonsterModel.WeakPointStackDamage = 0f;
		if (MonsterModel.WeakPointElementalType == EElementalType.None)
		{
			return;
		}
		var (damagePercent, vfx, sfx, buffId) = GetWeakPointConfig(MonsterModel.WeakPointElementalType);
		float maxStack = MonsterModel.MaxWeakPointStackDamage;
		MonsterModel.LastWeakPointFullStackDamageTime = TimeNow;
		HpDamageEvent fullStackEvent = new HpDamageEvent
		{
			SourceModel = unitEvent.SourceModel,
			SourceIdentity = unitEvent.SourceIdentity,
			Amount = (int)(damagePercent / 100f * maxStack),
			HitPosition = unitEvent.HitPosition,
			ElementalType = EElementalType.None,
			ReasonType = EHpDamageReasonType.WeakPointFullStack
		};
		MonsterModel.AddBuff(buffId, unitEvent.SourceModel, 1, 0uL, Vector3.zero);
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = vfx,
			VfxDuration = 2f,
			Sfx = sfx,
			Position = UnitModel.Position.ToProtoVector3(),
			TargetNetworkObjectId = base.NetworkObjectId,
			TargetPositionOffset = new ProtoVector3(),
			BoneType = EUnitBoneType.Body
		});
		if (characterModel != null)
		{
			GameHost.SendToClient(characterModel.ClientId, new NtfCameraShake
			{
				TargetClientId = characterModel.ClientId,
				IsGlobal = false,
				ShakeAmplitude = TConst.WeakPointShakeAmplitude.FloatValue,
				ShakeDuration = TConst.WeakPointShakeDuration.FloatValue,
				ShakeFrequency = TConst.WeakPointShakeFrequency.FloatValue
			});
		}
		HandleUnitEvent(fullStackEvent);
		void UpdateFlee()
		{
			if ((float)UnitModel.Hp * 100f / UnitModel.HpMax <= MonsterData.FleeHpPercent && TimeNow >= _nextFleeCheckTime)
			{
				_nextFleeCheckTime = TimeNow + (float)MonsterData.FleeCooltime;
				if (RandomUtility.GetRandomBoolFromPercentage(MonsterData.FleePercent))
				{
					MonsterModel.IsPendingFleeState = true;
				}
			}
		}
		(float, string, string, int) GetWeakPointConfig(EElementalType type)
		{
			switch (type)
			{
			case EElementalType.Fire:
				return (TConst.WeakPointFireFullStackDamagePercent.FloatValue, "Common/Elemental_Fire_Hit", "Elemental_Fire_Hit", 1987617);
			case EElementalType.Water:
				return (TConst.WeakPointWaterFullStackDamagePercent.FloatValue, "Common/Elemental_Water_Hit", "Elemental_Water_Hit", 1987618);
			case EElementalType.Wind:
				return (TConst.WeakPointWindFullStackDamagePercent.FloatValue, "Common/Elemental_Wind_Hit", "Elemental_Wind_Hit", 1987619);
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
		}
	}

	private void OnForceMonsterSetTargetEvent(ForceMonsterSetTargetEvent unitEvent)
	{
		MonsterModel.SetTarget(ObjectManager.FindTarget(unitEvent.TargetNetworkId));
	}

	private void OnTryMonsterCuriousStateEvent(TryMonsterCuriousStateEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnMonsterPhaseUpEvent(MonsterPhaseUpEvent unitEvent)
	{
		MonsterModel.SetNextPhase(unitEvent.NextPhaseMonsterId);
		UnitModel.PassiveSkillIds.ForEach(delegate(int id)
		{
			HandleUnitEvent(new AddPassiveEvent
			{
				SkillId = id
			});
		});
	}

	protected override void OnCrowdControlEvent(CrowdControlEvent unitEvent)
	{
		if (MonsterModel.HasSuppressionSystem && unitEvent.CcInfo.ccType != ECrowdControlType.None)
		{
			EStatusEffectTag effectTag = unitEvent.CcInfo.ccType.GetEffectTag();
			if (!MonsterModel.HasImmunityEffect(effectTag) && MonsterModel.IsSuppressionBuffTag(effectTag))
			{
				if (!MonsterModel.IsSuppressionImmune && MonsterModel.State != EActionState.Groggy)
				{
					float amount = unitEvent.CcInfo.SuppressionValue;
					if (TStatusEffectSuppression.Table.TryGetValue(effectTag, out var suppression))
					{
						amount += suppression.Value;
					}
					if (amount > 0f)
					{
						AddSuppression(amount);
					}
				}
				unitEvent.CcInfo.ccType = ECrowdControlType.None;
			}
		}
		base.OnCrowdControlEvent(unitEvent);
	}
}
