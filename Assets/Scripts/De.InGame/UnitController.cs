using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using De.InGame.Fsm;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using Sirenix.Utilities;
using SomeNet.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

public abstract class UnitController : NetworkBase
{
	private class PassiveExecutionContext
	{
		public int SkillId;

		public SkillActionNode Node;

		public IUnitEvent TriggeredEvent;

		public List<ITarget> TriggerTargets;

		public bool ChildExecuted;
	}

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected RoomContext RoomContext;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameEventSystem EventSystem;

	[Inject]
	protected CameraManager CameraManager;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameMissionManager MissionManager;

	[SerializeField]
	protected UnitModel UnitModel;

	[SerializeField]
	protected UnitView UnitView;

	[SerializeField]
	protected Inventory Inventory;

	private readonly MessageBroker _eventMessageBroker = new MessageBroker();

	private PlaneSightUnit _planeSightUnit;

	private float _hpFraction;

	private readonly Stack<PassiveExecutionContext> _passiveExecutionStack = new Stack<PassiveExecutionContext>();

	private float _freezeDurationReductionCooldownEndTime;

	protected GameHost GameHost => base.NetworkHost as GameHost;

	protected GameChannel GameChannel => base.NetworkChannel as GameChannel;

	protected IMessageReceiver EventReceiver => _eventMessageBroker;

	protected abstract UnitStateMachine StateMachine { get; }

	public abstract ulong ClientId { get; }

	protected PlaneSightUnit PlaneSightUnit
	{
		get
		{
			if (!(_planeSightUnit == null))
			{
				return _planeSightUnit;
			}
			_planeSightUnit = GetComponentInChildren<PlaneSightUnit>();
			return _planeSightUnit;
		}
	}

	protected int StressMax => TConst.StressMax.IntValue;

	public Subject<string> OnShowBubbleText { get; } = new Subject<string>();

	public Subject<string> OnShowMonologueText { get; } = new Subject<string>();

	protected virtual void Start()
	{
		GameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Take(1).Subscribe(delegate
		{
			OnStageStarted();
		})
			.AddTo(this);
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		RegisterCommonHandlers();
		if (base.IsHost)
		{
			RegisterHostHandlers();
			RegisterUnitEventHandlers();
		}
		RegisterClientHandlers();
		ObjectManager.OnUnitSpawned.OnNext(this);
		ObjectManager.OnTargetSpawned.OnNext(UnitModel);
	}

	public override void OnNetworkDespawn()
	{
		if (base.IsHost)
		{
			StateMachine.Clear();
		}
		ObjectManager.OnUnitDespawned.OnNext(this);
		ObjectManager.OnTargetDespawned.OnNext(UnitModel);
		base.OnNetworkDespawn();
	}

	protected virtual void FixedUpdate()
	{
		if (base.IsHost)
		{
			StateMachine?.OnUpdate();
			UnitModel.Forward = ((StateMachine != null) ? StateMachine.GetForward() : Vector3.zero);
		}
	}

	protected override void OnNetworkUpdate()
	{
		base.OnNetworkUpdate();
		if (!(UnitModel == null) && UnitModel.IsInitialized && base.IsHost)
		{
			UnitModel.UpdateCrowdControl();
			UnitModel.UpdateBuffs();
			UnitModel.UpdateSkillStacks();
			UnitModel.UpdateShields();
			UnitModel.UpdateStackBuffTimers();
			UnitModel.Synchronize();
		}
	}

	protected virtual void OnStageStarted()
	{
		if (base.NetworkManager.IsHost)
		{
			float randomFloat = RandomUtility.GetRandomFloat(0f, 1f);
			Observable.Timer(TimeSpan.FromSeconds(randomFloat), TimeSpan.FromSeconds(1.0)).Where((long _) => RoomContext.RoomState == ERoomState.Ready || RoomContext.RoomState == ERoomState.Playing).Subscribe(delegate
			{
				OnUpdateSec();
			})
				.AddTo(this);
			UnitModel.RxState.DistinctUntilChanged().Pairwise().Subscribe(delegate(Pair<EActionState> pair)
			{
				HandleUnitEvent(new StateChangeEvent
				{
					State = pair.Current,
					PreviousState = pair.Previous
				});
			})
				.AddTo(this);
		}
	}

	protected void RegisterHostHandler<T>(Action<T> handler) where T : BaseMessage
	{
		GameHost.Receive<T>().Where((T message) => message.SenderId == ClientId).Subscribe(handler).AddTo(this);
	}

	protected void RegisterClientHandler<T>(Action<T> handler) where T : BaseMessage
	{
		GameChannel.Receive<T>().Subscribe(handler).AddTo(this);
	}

	protected virtual void OnUpdateSec()
	{
		AssertIfNotHost();
		UpdateTickDamage();
		UpdateRegenHp();
		UpdateSpecialFields();
		UpdateSkillCharge();
		HandleUnitEvent(new PerSecondEvent());
	}

	private void UpdateSkillCharge()
	{
		AssertIfNotHost();
		if (UnitModel.IsNonCombat || !UnitModel.IsHitable)
		{
			return;
		}
		foreach (int activeSkillId in UnitModel.ActiveSkillIds)
		{
			if (activeSkillId > 0 && TSkill.Table.TryGetValue(activeSkillId, out var value) && value.ChargePerSec > 0f)
			{
				HandleUnitEvent(new SkillChargeEvent
				{
					SourceModel = UnitModel,
					Amount = value.ChargePerSec
				});
			}
		}
	}

	private void UpdateRegenHp()
	{
		AssertIfNotHost();
		if (!UnitModel.IsNonCombat && UnitModel.IsHitable)
		{
			float num = _hpFraction + UnitModel.HpRegen;
			_hpFraction = num;
			if (1f <= num)
			{
				_hpFraction = num - (float)(int)num;
				HandleUnitEvent(new HpRecoveryEvent
				{
					Amount = (int)num,
					ReasonType = EHpRecoveryReasonType.Regen
				});
			}
		}
	}

	private void UpdateSpecialFields()
	{
		UnitModel.UpdateSpecialFields();
	}

	private void GenerateConsumeResultItems(int consumeItemId)
	{
		AssertIfNotHost();
		TItem tItem = TItem.Table[consumeItemId];
		if (tItem.AddAfterConsumeIds == null)
		{
			return;
		}
		for (int i = 0; i < tItem.AddAfterConsumeIds.Length; i++)
		{
			int num = tItem.AddAfterConsumeIds[i];
			if (num > 0)
			{
				Goods goods = GoodsCreator.MakeItemGoods(TItem.Table[num].Id, 1L);
				Inventory.TryGainGoods(goods);
			}
		}
	}

	private bool CheckCcInfoCondition(CcInfo ccInfo)
	{
		if (ccInfo.FactionCondition != EMonsterCategory.None && !UnitModel.MonsterCategories.Contains(ccInfo.FactionCondition))
		{
			return false;
		}
		if (ccInfo.MonsterCondition == EMonsterType.None)
		{
			return true;
		}
		if (UnitModel is MonsterModel monsterModel && monsterModel.MonsterType == ccInfo.MonsterCondition)
		{
			return true;
		}
		return false;
	}

	private void UpdateTickDamage()
	{
		AssertIfNotHost();
		if (UnitModel.IsHitable)
		{
			ApplyDotEffect<DotHpRecoveryEffect>();
			ApplyDotEffect<DotHpDamageEffect>();
			ApplyDotEffect<DotStressRecoveryEffect>();
			ApplyDotEffect<DotStressDamageEffect>();
			ApplyDotEffect<DotBrightnessRecoveryEffect>();
			ApplyDotEffect<DotBrightnessDamageEffect>();
			ApplyDotEffect<DotSoulDamageEffect>();
			ApplyDotShieldEffect();
			ApplyDotRangeEffect<DotRangeHpRecoveryEffect>();
			ApplyDotRangeEffect<DotRangeHpDamageEffect>();
			ApplyDotRangeEffect<DotRangeStressRecoveryEffect>();
			ApplyDotRangeEffect<DotRangeStressDamageEffect>();
			ApplyDotRangeEffect<DotRangeBrightnessRecoveryEffect>();
			ApplyDotRangeEffect<DotRangeBrightnessDamageEffect>();
			ApplyDotPercentHpDamageEffect();
			ApplyDotRangePercentHpDamageEffect();
		}
		void ApplyDotEffect<T>() where T : DotBuffEffect
		{
			if (!UnitModel.IsHitable)
			{
				return;
			}
			IList<BuffEffectInfo<T>> buffEffects = UnitModel.GetBuffEffects<T>();
			if (!buffEffects.Any())
			{
				return;
			}
			EBuffEffectType effectType = buffEffects.First().BuffEffect.EffectType;
			List<(EStatusEffectTag, int, bool)> list = buffEffects.GroupBy((BuffEffectInfo<T> b) => b.EffectTag).Select((IGrouping<EStatusEffectTag, BuffEffectInfo<T>> g) => (g.Key, g.Sum(GetDotAmount), g.Any((BuffEffectInfo<T> info) => !(info.BuffEffect is DotStressDamageEffect dotStressDamageEffect) || dotStressDamageEffect.ShowDamageEffect))).ToList();
			List<List<ulong>> list2 = buffEffects.GroupBy((BuffEffectInfo<T> b) => b.EffectTag).Select((IGrouping<EStatusEffectTag, BuffEffectInfo<T>> g) => g.Where((BuffEffectInfo<T> x) => x.CasterIds != null).SelectMany((BuffEffectInfo<T> x) => x.CasterIds).Distinct().ToList()).ToList();
			foreach (EStatusEffectTag item in (from _ in buffEffects
				where _.EffectTag != EStatusEffectTag.None
				select _.EffectTag).Distinct().ToList())
			{
				SendDotEffectTagFx(item, UnitModel.Position, base.NetworkObjectId);
			}
			foreach (int item2 in buffEffects.Select((BuffEffectInfo<T> info) => info.BuffId).Distinct().ToList())
			{
				SendDotBuffFx(item2, UnitModel.Position, base.NetworkObjectId);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!UnitModel.IsHitable)
				{
					break;
				}
				(EStatusEffectTag, int, bool) tuple = list[i];
				List<ulong> list3 = list2[i];
				ulong casterId = ((list3.Count < 1) ? ulong.MaxValue : list3[RandomUtility.GetRandomInt(list3.Count)]);
				float amplifierPercent = CalculateAmplifierPercent(tuple.Item1, effectType, list3);
				HandleDotEffectEvent(this, effectType, tuple.Item1, tuple.Item2, UnitModel.Position, casterId, amplifierPercent, list[i].Item3);
			}
		}
		int GetDotAmount<T>(BuffEffectInfo<T> info) where T : DotBuffEffect
		{
			int num = info.BuffEffect.TickAmount * info.Stack;
			if (!info.BuffEffect.UseStatValue)
			{
				return num;
			}
			float statPercent = info.BuffEffect.StatPercent;
			EStatType statType = info.BuffEffect.StatType;
			UnitBuff unitBuff = UnitModel.Buffs[info.BuffId];
			float? num2 = ((unitBuff.StackEntries != null) ? new float?(unitBuff.StackEntries.GetCasterStatSum(statType)) : ((float?)null));
			float? num3 = num2 * (statPercent / 100f);
			return num + (int)num3.Value;
		}
		void ApplyDotPercentHpDamageEffect()
		{
			if (!UnitModel.IsHitable)
			{
				return;
			}
			IList<BuffEffectInfo<DotPercentHpDamageEffect>> buffEffects = UnitModel.GetBuffEffects<DotPercentHpDamageEffect>();
			if (!buffEffects.Any())
			{
				return;
			}
			EBuffEffectType effectType = EBuffEffectType.DotPercentHpDamage;
			buffEffects.GroupBy((BuffEffectInfo<DotPercentHpDamageEffect> b) => b.EffectTag).Select((IGrouping<EStatusEffectTag, BuffEffectInfo<DotPercentHpDamageEffect>> g) => (g.Key, g.Sum((BuffEffectInfo<DotPercentHpDamageEffect> buff) => (float)buff.Stack * buff.BuffEffect.Percent), g.Sum((BuffEffectInfo<DotPercentHpDamageEffect> buff) => buff.BuffEffect.TickAmount * buff.Stack), g.Where((BuffEffectInfo<DotPercentHpDamageEffect> x) => x.CasterIds != null).SelectMany((BuffEffectInfo<DotPercentHpDamageEffect> x) => x.CasterIds).Distinct().ToList())).ToList().ForEach(delegate((EStatusEffectTag, float, int, List<ulong>) _)
			{
				float amplifierPercent = CalculateAmplifierPercent(_.Item1, effectType, _.Item4);
				EStatusEffectTag item = _.Item1;
				SendDotEffectTagFx(item, UnitModel.Position, base.NetworkObjectId);
				// Trap 59 (docs/DECODING.md): float32 product before the integer conversion, as IL2CPP; Mono needs the (float).
				int amount = ApplyAmplifier(amplifierPercent, (int)(float)(UnitModel.HpMax * (_.Item2 / 100f)) + _.Item3);
				HpDamageEvent hpDamageEvent = new HpDamageEvent
				{
					Amount = amount,
					HitPosition = UnitModel.Position,
					IsFixedDamage = true,
					ElementalType = item.GetElementalType(),
					ReasonType = EHpDamageReasonType.Dot
				};
				UnitController component = UnitModel.GetComponent<UnitController>();
				if (!(component == null))
				{
					component.HandleUnitEvent(hpDamageEvent);
				}
			});
			foreach (int item2 in buffEffects.Select((BuffEffectInfo<DotPercentHpDamageEffect> info) => info.BuffId).Distinct().ToList())
			{
				SendDotBuffFx(item2, UnitModel.Position, base.NetworkObjectId);
			}
		}
		void ApplyDotRangeEffect<T>() where T : DotRangeBuffEffect
		{
			if (!UnitModel.IsHitable)
			{
				return;
			}
			IList<BuffEffectInfo<T>> buffEffects = UnitModel.GetBuffEffects<T>();
			if (!buffEffects.Any())
			{
				return;
			}
			Dictionary<(ulong, EBuffEffectType, EStatusEffectTag), (int, List<ulong>)> dictionary = new Dictionary<(ulong, EBuffEffectType, EStatusEffectTag), (int, List<ulong>)>();
			Dictionary<ulong, (UnitModel, HashSet<int>)> dictionary2 = new Dictionary<ulong, (UnitModel, HashSet<int>)>();
			foreach (BuffEffectInfo<T> item in buffEffects)
			{
				EHitTarget targetType = item.BuffEffect.TargetType;
				float range = item.BuffEffect.Range;
				EBuffEffectType effectType = item.BuffEffect.EffectType;
				EStatusEffectTag effectTag = item.EffectTag;
				int tickAmount = item.BuffEffect.TickAmount;
				int num = 0;
				if (item.BuffEffect.UseStatValue)
				{
					float statPercent = item.BuffEffect.StatPercent;
					EStatType statType = item.BuffEffect.StatType;
					UnitBuff unitBuff = UnitModel.Buffs[item.BuffId];
					float? num2 = ((unitBuff.StackEntries != null) ? new float?(unitBuff.StackEntries.GetCasterStatSum(statType)) : ((float?)null));
					float? num3 = num2 * (statPercent / 100f);
					num = (int)num3.Value;
				}
				List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel _) => targetType.IsHitable(UnitModel.GetHitLayerInfo(), _.GetHitLayerInfo(), _.HitLayer)).Where((UnitModel _) => _.IsHitable).Where((UnitModel _) => Vector3.Distance(_.Position, UnitModel.Position) < range)
					.ToList();
				foreach (UnitModel item2 in list)
				{
					ulong networkObjectId = item2.NetworkObjectId;
					(ulong, EBuffEffectType, EStatusEffectTag) key = (networkObjectId, effectType, effectTag);
					int num4 = item.Stack * tickAmount;
					int num5 = num4 + num;
					if (num < 1)
					{
						num5 = num4;
					}
					if (!dictionary.TryGetValue(key, out var value))
					{
						List<ulong> list2 = new List<ulong>();
						if (item.CasterIds != null)
						{
							list2.AddRange(item.CasterIds);
						}
						dictionary[key] = (num5, list2);
					}
					else
					{
						int item3 = value.Item1 + num5;
						List<ulong> item4 = value.Item2;
						if (item.CasterIds != null)
						{
							if (item4 == null)
							{
								item4 = new List<ulong>();
							}
							item4.AddRange(item.CasterIds);
						}
						dictionary[key] = (item3, item4);
					}
					if (!dictionary2.TryGetValue(networkObjectId, out var value2))
					{
						value2 = (item2, new HashSet<int>());
						dictionary2.Add(networkObjectId, value2);
					}
					value2.Item2.Add(item.BuffId);
				}
			}
			foreach (KeyValuePair<(ulong, EBuffEffectType, EStatusEffectTag), (int, List<ulong>)> item5 in dictionary)
			{
				ulong item6 = item5.Key.Item1;
				EBuffEffectType item7 = item5.Key.Item2;
				EStatusEffectTag item8 = item5.Key.Item3;
				int item9 = item5.Value.Item1;
				List<ulong> list3 = item5.Value.Item2;
				if (list3 == null)
				{
					list3 = new List<ulong>();
				}
				UnitModel unitModel = ObjectManager.FindUnitModel(item6);
				if (unitModel == null)
				{
					continue;
				}
				UnitController component = unitModel.GetComponent<UnitController>();
				if (!(component == null))
				{
					ulong casterId = ((list3.Count < 1) ? ulong.MaxValue : list3[RandomUtility.GetRandomInt(list3.Count)]);
					float amplifierPercent = CalculateAmplifierPercent(item8, item7, list3.Distinct());
					HandleDotEffectEvent(component, item7, item8, item9, unitModel.Position, casterId, amplifierPercent, showDamageEffect: true);
					SendDotEffectTagFx(item8, unitModel.Position, item6);
				}
			}
			foreach ((UnitModel, HashSet<int>) targetBuff in dictionary2.Values)
			{
				targetBuff.Item2.ForEach(delegate(int _)
				{
					SendDotBuffFx(_, targetBuff.Item1.Position, targetBuff.Item1.NetworkObjectId);
				});
			}
		}
		void ApplyDotRangePercentHpDamageEffect()
		{
			if (!UnitModel.IsHitable)
			{
				return;
			}
			IList<BuffEffectInfo<DotRangePercentHpDamageEffect>> buffEffects = UnitModel.GetBuffEffects<DotRangePercentHpDamageEffect>();
			if (!buffEffects.Any())
			{
				return;
			}
			Dictionary<ulong, (UnitModel, HashSet<int>)> dictionary = new Dictionary<ulong, (UnitModel, HashSet<int>)>();
			foreach (BuffEffectInfo<DotRangePercentHpDamageEffect> item in buffEffects)
			{
				EStatusEffectTag effectTag = item.EffectTag;
				int stack = item.Stack;
				float percent = item.BuffEffect.Percent;
				int stack2 = item.Stack;
				int tickAmount = item.BuffEffect.TickAmount;
				EHitTarget targetType = item.BuffEffect.TargetType;
				float range = item.BuffEffect.Range;
				IEnumerable<ulong> source = item.CasterIds ?? Array.Empty<ulong>();
				List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel _) => targetType.IsHitable(UnitModel.GetHitLayerInfo(), _.GetHitLayerInfo(), _.HitLayer)).Where((UnitModel _) => _.IsHitable).Where((UnitModel _) => Vector3.Distance(_.Position, UnitModel.Position) < range)
					.ToList();
				float percent2 = CalculateAmplifierPercent(effectTag, EBuffEffectType.DotRangePercentHpDamage, source.Distinct());
				foreach (UnitModel item2 in list)
				{
					int amount = ApplyAmplifier(percent2, (int)(float)(item2.HpMax * ((float)stack * percent / 100f)) + stack2 * tickAmount);
					HpDamageEvent hpDamageEvent = new HpDamageEvent
					{
						Amount = amount,
						HitPosition = item2.Position,
						IsFixedDamage = true,
						ElementalType = effectTag.GetElementalType(),
						ReasonType = EHpDamageReasonType.DotRange
					};
					UnitController component = item2.GetComponent<UnitController>();
					if (!(component == null))
					{
						component.HandleUnitEvent(hpDamageEvent);
						SendDotEffectTagFx(effectTag, item2.Position, item2.NetworkObjectId);
						if (!dictionary.TryGetValue(item2.NetworkObjectId, out var value))
						{
							value = (item2, new HashSet<int>());
							dictionary.Add(item2.NetworkObjectId, value);
						}
						value.Item2.Add(item.BuffId);
					}
				}
			}
			foreach ((UnitModel, HashSet<int>) targetBuff in dictionary.Values)
			{
				targetBuff.Item2.ForEach(delegate(int _)
				{
					SendDotBuffFx(_, targetBuff.Item1.Position, targetBuff.Item1.NetworkObjectId);
				});
			}
		}
		void ApplyDotShieldEffect()
		{
			if (!UnitModel.IsHitable)
			{
				return;
			}
			IList<BuffEffectInfo<DotShieldBuffEffect>> buffEffects = UnitModel.GetBuffEffects<DotShieldBuffEffect>();
			if (!buffEffects.Any())
			{
				return;
			}
			var list = buffEffects.GroupBy((BuffEffectInfo<DotShieldBuffEffect> info) => new
			{
				EffectTag = info.EffectTag,
				ShieldId = info.BuffEffect.ShieldId
			}).Select((g) => new
			{
				EffectTag = g.Key.EffectTag,
				ShieldId = g.Key.ShieldId,
				Amount = g.Sum(GetDotShieldAmount),
				Casters = g.Where((BuffEffectInfo<DotShieldBuffEffect> x) => x.CasterIds != null).SelectMany((BuffEffectInfo<DotShieldBuffEffect> x) => x.CasterIds).Distinct().ToList()
			}).ToList();
			foreach (EStatusEffectTag item in (from _ in buffEffects
				where _.EffectTag != EStatusEffectTag.None
				select _.EffectTag).Distinct().ToList())
			{
				SendDotEffectTagFx(item, UnitModel.Position, base.NetworkObjectId);
			}
			foreach (int item2 in buffEffects.Select((BuffEffectInfo<DotShieldBuffEffect> info) => info.BuffId).Distinct().ToList())
			{
				SendDotBuffFx(item2, UnitModel.Position, base.NetworkObjectId);
			}
			foreach (var item3 in list)
			{
				if (!UnitModel.IsHitable)
				{
					break;
				}
				EStatusEffectTag effectTag = item3.EffectTag;
				float percent = CalculateAmplifierPercent(effectTag, EBuffEffectType.DotShield, item3.Casters.Distinct());
				int amount = ApplyAmplifier(percent, item3.Amount);
				HandleUnitEvent(new ShieldEvent
				{
					SourceModel = UnitModel,
					ShieldId = item3.ShieldId,
					Amount = amount,
					CasterId = UnitModel.NetworkObjectId
				});
			}
		}
		int GetDotShieldAmount(BuffEffectInfo<DotShieldBuffEffect> info)
		{
			return GetDotAmount(info);
		}
		void HandleDotEffectEvent(UnitController unitController, EBuffEffectType effectType, EStatusEffectTag effectTag, int amount, Vector3 position, ulong casterId, float amplifierPercent, bool showDamageEffect)
		{
			UnitModel unitModel = ObjectManager.FindUnitModel(casterId);
			int num = ApplyAmplifier(amplifierPercent, amount);
			switch (effectType)
			{
			case EBuffEffectType.DotHpDamage:
				unitController.HandleUnitEvent(new HpDamageEvent
				{
					Amount = num,
					SourceModel = unitModel,
					HitPosition = position,
					SourceIdentity = UnitIdentity.From(unitModel),
					ElementalType = effectTag.GetElementalType(),
					ReasonType = EHpDamageReasonType.Dot
				});
				break;
			case EBuffEffectType.DotHpRecovery:
			case EBuffEffectType.DotRangeHpRecovery:
				unitController.HandleUnitEvent(new HpRecoveryEvent
				{
					Amount = num,
					SourceModel = unitModel
				});
				break;
			case EBuffEffectType.DotStressDamage:
				unitController.HandleUnitEvent(new StressDamageEvent
				{
					SourceModel = unitModel,
					ReasonType = EStressReasonType.Dot,
					Amount = num,
					HitPosition = position,
					ShowDamageEffect = showDamageEffect
				});
				break;
			case EBuffEffectType.DotStressRecovery:
			case EBuffEffectType.DotRangeStressRecovery:
				unitController.HandleUnitEvent(new StressRecoveryEvent
				{
					Amount = num
				});
				break;
			case EBuffEffectType.DotBrightnessDamage:
			case EBuffEffectType.DotRangeBrightnessDamage:
				unitController.HandleUnitEvent(new LightFuelDamageEvent
				{
					Amount = num,
					HitPosition = position
				});
				break;
			case EBuffEffectType.DotBrightnessRecovery:
			case EBuffEffectType.DotRangeBrightnessRecovery:
				unitController.HandleUnitEvent(new LightFuelRecoveryEvent
				{
					Amount = num
				});
				break;
			case EBuffEffectType.DotSoulDamage:
				unitController.HandleUnitEvent(new SoulDamageEvent
				{
					Amount = num
				});
				break;
			case EBuffEffectType.DotRangeHpDamage:
				unitController.HandleUnitEvent(new HpDamageEvent
				{
					SourceModel = unitModel,
					Amount = num,
					HitPosition = position,
					SourceIdentity = UnitIdentity.From(unitModel),
					ElementalType = effectTag.GetElementalType(),
					ReasonType = EHpDamageReasonType.DotRange
				});
				break;
			case EBuffEffectType.DotRangeStressDamage:
				unitController.HandleUnitEvent(new StressDamageEvent
				{
					SourceModel = unitModel,
					ReasonType = EStressReasonType.DotRange,
					Amount = num,
					HitPosition = position,
					ShowDamageEffect = showDamageEffect
				});
				break;
			}
		}
	}

	private static int ApplyAmplifier(float percent, int baseAmount)
	{
		if (percent > 0f)
		{
			baseAmount = (int)Mathf.Round((percent / 100f + 1f) * (float)baseAmount);
		}
		return Math.Max(baseAmount, 0);
	}

	private float CalculateAmplifierPercent(EStatusEffectTag effectTag, EBuffEffectType effectType, IEnumerable<ulong> casterIds)
	{
		float num = 0f;
		if (casterIds == null)
		{
			return 0f;
		}
		foreach (ulong casterId in casterIds)
		{
			UnitModel unitModel = ObjectManager.FindUnitModel(casterId);
			if (unitModel == null)
			{
				continue;
			}
			IList<BuffEffectInfo<DotEffectAmplifierEffect>> buffEffects = unitModel.GetBuffEffects<DotEffectAmplifierEffect>();
			if (buffEffects != null && buffEffects.Count != 0)
			{
				float num2 = buffEffects.Where((BuffEffectInfo<DotEffectAmplifierEffect> info) => IsMatchingAmplifier(info.BuffEffect, effectTag, effectType)).Select((BuffEffectInfo<DotEffectAmplifierEffect> info) => (float)info.Stack * info.BuffEffect.Percent).DefaultIfEmpty(0f)
					.Max();
				if (num < num2)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	private static bool IsMatchingAmplifier(DotEffectAmplifierEffect buffEffect, EStatusEffectTag effectTag, EBuffEffectType effectType)
	{
		if (buffEffect.TargetDotType != effectTag && buffEffect.TargetDotType != EStatusEffectTag.None)
		{
			return false;
		}
		if (buffEffect.TargetEffectTypes == null || buffEffect.TargetEffectTypes.Length == 0)
		{
			return true;
		}
		return buffEffect.TargetEffectTypes.Contains(effectType);
	}

	private void SendDotEffectTagFx(EStatusEffectTag effectTag, Vector3 position, ulong targetNetworkObjectId)
	{
		if (effectTag == EStatusEffectTag.None)
		{
			return;
		}
		TStatusEffectTag tStatusEffectTag = TStatusEffectTag.Table[effectTag];
		float vfxScale = ((tStatusEffectTag.DotVfxBoneType == EUnitBoneType.Symbol) ? 1f : UnitView.StatusVfxScale);
		if (!tStatusEffectTag.DotVfx.HasValue() && !tStatusEffectTag.DotSfx.HasValue())
		{
			return;
		}
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = (string.IsNullOrEmpty(tStatusEffectTag.DotVfx) ? null : string.Format("Status/{0}", tStatusEffectTag.DotVfx)),
			VfxScale = vfxScale,
			VfxDuration = 1f,
			Sfx = tStatusEffectTag.DotSfx,
			Position = position.ToProtoVector3(),
			TargetNetworkObjectId = targetNetworkObjectId,
			TargetPositionOffset = tStatusEffectTag.DotVfxOffset.ToProtoVector3(),
			BoneType = tStatusEffectTag.DotVfxBoneType
		});
	}

	private void SendDotBuffFx(int buffId, Vector3 position, ulong targetNetworkObjectId)
	{
		if (!TBuffVfx.Table.TryGetValue(buffId, out var value))
		{
			return;
		}
		float vfxScale = ((value.DotVfxBoneType == EUnitBoneType.Symbol) ? 1f : UnitView.StatusVfxScale);
		if (!value.DotVfx.HasValue() && !value.DotSfx.HasValue())
		{
			return;
		}
		GameHost.SendToAll(new NtfDisplayFx
		{
			Vfx = (string.IsNullOrEmpty(value.DotVfx) ? null : string.Format("Status/{0}", value.DotVfx)),
			VfxScale = vfxScale,
			VfxDuration = 1f,
			Sfx = value.DotSfx,
			Position = position.ToProtoVector3(),
			TargetNetworkObjectId = targetNetworkObjectId,
			TargetPositionOffset = value.DotVfxOffset.ToProtoVector3(),
			BoneType = value.DotVfxBoneType
		});
	}

	protected virtual void RegisterCommonHandlers()
	{
		RegisterClientHandler<NtfPlayAnimation>(OnPlayAnimation);
		RegisterClientHandler<NtfUpdateAnimation>(OnUpdateAnimation);
		RegisterClientHandler<NtfChangeSkill>(OnChangeSkill);
	}

	protected virtual void RegisterHostHandlers()
	{
	}

	protected virtual void RegisterClientHandlers()
	{
	}

	private void OnPlayAnimation(NtfPlayAnimation message)
	{
		if (message.NetworkObjectId == UnitModel.NetworkObjectId)
		{
			UnitView.PlayAnimationAsync(message.AnimationName, message.IsLoop, message.AnimStartOffsetTime, message.AnimationSpeeds, 1f, message.MultiTrackAnimationName).Forget();
		}
	}

	private void OnUpdateAnimation(NtfUpdateAnimation message)
	{
		if (message.NetworkObjectId == UnitModel.NetworkObjectId)
		{
			UnitView.UpdateAnimation(message.AnimationName, message.PrevAnimationName);
		}
	}

	private void OnChangeSkill(NtfChangeSkill message)
	{
		if (message.NetworkObjectId == UnitModel.NetworkObjectId && !UnitModel.IsHost)
		{
			if (!message.IsRemove)
			{
				UnitModel.ApplyChangeSkillOverride(message.OriginalSkillId, message.ReplacementSkillId);
			}
			else
			{
				UnitModel.RemoveChangeSkillOverride(message.OriginalSkillId, message.ReplacementSkillId);
			}
		}
	}

	public void ExecutePassiveSkill(int skillId, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
		if (TSkill.Table.TryGetValue(skillId, out var value) && UnitModel.CheckSkill(skillId))
		{
			ITarget target = triggerTargets.FirstOrDefault();
			ulong targetNetworkObjectId = ((target != null) ? target.NetworkObjectId : 0);
			if (CheckBuffConditions(value.RootActionNode.skillAction.BuffConditionList, base.NetworkObjectId, targetNetworkObjectId) && CheckTalentCondition(value.RootActionNode.skillAction.TalentConditionList))
			{
				ExecutePassiveSkillAction(skillId, value.RootActionNode, triggeredEvent, triggerTargets);
			}
		}
	}

	private void ExecutePassiveSkillAction(int skillId, SkillActionNode node, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
		if (!TSkill.Table.TryGetValue(skillId, out var value))
		{
			return;
		}
		if (node.skillAction.IsGivingCoolDown)
		{
			UnitModel.StartSkillCoolDown(skillId);
		}
		if (GameManager.IsInStage && node.skillAction.IsUsingCost)
		{
			int hpCost = value.HpCost;
			int stressCost = value.StressCost;
			int staminaCost = value.StaminaCost;
			UnitModel.AdjustSkillCosts(value, ref hpCost, ref stressCost, ref staminaCost);
			ApplySkillCost(skillId, hpCost, stressCost, staminaCost);
		}
		PassiveExecutionContext context = new PassiveExecutionContext
		{
			SkillId = skillId,
			Node = node,
			TriggeredEvent = triggeredEvent,
			TriggerTargets = triggerTargets
		};
		_passiveExecutionStack.Push(context);
		float actionDuration = node.skillAction.actionDuration;
		node.skillAction.actionEvents.ForEach(delegate(ActionEvent actionEvent)
		{
			if (actionDuration > 0f && actionDuration <= actionEvent.startTime && actionEvent.startTime != actionDuration)
			{
				return;
			}
			if (actionEvent.startTime > 0f)
			{
				Observable.Timer(TimeSpan.FromSeconds(actionEvent.startTime)).Subscribe(delegate
				{
					_passiveExecutionStack.Push(context);
					OnPassiveActionEvent(skillId, actionEvent, triggeredEvent, triggerTargets);
					_passiveExecutionStack.Pop();
				}).AddTo(this);
			}
			else
			{
				OnPassiveActionEvent(skillId, actionEvent, triggeredEvent, triggerTargets);
			}
		});
		if (node.HasChild && actionDuration >= 0f)
		{
			if (actionDuration <= 0f)
			{
				TryExecuteFallbackChildNode(context, skillId, triggeredEvent, triggerTargets);
			}
			else
			{
				Observable.Timer(TimeSpan.FromSeconds(actionDuration)).Subscribe(delegate
				{
					TryExecuteFallbackChildNode(context, skillId, triggeredEvent, triggerTargets);
				}).AddTo(this);
			}
		}
		_passiveExecutionStack.Pop();
	}

	private void TryExecuteFallbackChildNode(PassiveExecutionContext context, int skillId, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
		if (context.ChildExecuted || context.Node.childNodes == null)
		{
			return;
		}
		SkillActionNode[] childNodes = context.Node.childNodes;
		for (int i = 0; i < childNodes.Length; i++)
		{
			SkillActionNode skillActionNode = childNodes[i];
			if (skillActionNode.SkillTrigger == null)
			{
				IList<BuffCondition> buffConditionList = skillActionNode.skillAction.BuffConditionList;
				ulong networkObjectId = base.NetworkObjectId;
				ITarget target = triggerTargets.FirstOrDefault();
				ulong targetNetworkObjectId = ((target != null) ? target.NetworkObjectId : 0);
				if (CheckBuffConditions(buffConditionList, networkObjectId, targetNetworkObjectId) && CheckTalentCondition(skillActionNode.skillAction.TalentConditionList))
				{
					context.ChildExecuted = true;
					ExecutePassiveSkillAction(skillId, skillActionNode, triggeredEvent, triggerTargets);
					break;
				}
			}
		}
	}

	private void TryProceedPassiveChildNode(IUnitEvent unitEvent)
	{
		if (_passiveExecutionStack.Count == 0)
		{
			return;
		}
		PassiveExecutionContext[] array = _passiveExecutionStack.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			PassiveExecutionContext passiveExecutionContext = array[i];
			SkillActionNode node = passiveExecutionContext.Node;
			if (!node.HasChild || passiveExecutionContext.ChildExecuted)
			{
				continue;
			}
			SkillActionNode[] childNodes = node.childNodes;
			for (int j = 0; j < childNodes.Length; j++)
			{
				SkillActionNode skillActionNode = childNodes[j];
				if (skillActionNode.SkillTrigger == null || !unitEvent.CheckSkillTriggered(UnitModel, skillActionNode.SkillTrigger, out var triggerTargets))
				{
					continue;
				}
				IList<BuffCondition> buffConditionList = skillActionNode.skillAction.BuffConditionList;
				ulong networkObjectId = base.NetworkObjectId;
				ITarget target = triggerTargets.FirstOrDefault();
				ulong targetNetworkObjectId = ((target != null) ? target.NetworkObjectId : 0);
				if (CheckBuffConditions(buffConditionList, networkObjectId, targetNetworkObjectId) && CheckTalentCondition(skillActionNode.skillAction.TalentConditionList))
				{
					passiveExecutionContext.ChildExecuted = true;
					ExecutePassiveSkillAction(passiveExecutionContext.SkillId, skillActionNode, unitEvent, triggerTargets);
					break;
				}
			}
		}
	}

	private void OnPassiveActionEvent(int skillId, ActionEvent actionEvent, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
		List<ITarget> list = ((actionEvent.ActionTarget != ESkillActionTarget.TriggerTarget) ? new List<ITarget> { UnitModel } : triggerTargets);
		foreach (ITarget item in list)
		{
			if (item != null && IsActionEventExcutable(actionEvent, base.NetworkObjectId, item.NetworkObjectId))
			{
				ITarget triggerTarget = triggerTargets.FirstOrDefault();
				OnPassiveActionEventOnTarget(skillId, actionEvent, item, triggerTarget);
			}
		}
	}

	private void OnPassiveActionEventOnTarget(int skillId, ActionEvent actionEvent, ITarget actionTarget, ITarget triggerTarget)
	{
		SkillAction skillAction = TSkill.Table.TryGetValue(skillId).RootActionNode.skillAction;
		if (actionEvent is BuffActionEvent buffEvent)
		{
			OnBuffActionEvent(buffEvent, actionTarget);
		}
		else if (actionEvent is RemoveStatusEffectActionEvent actionEvent2)
		{
			OnRemoveBuffActionEvent(actionEvent2, actionTarget);
		}
		else if (actionEvent is CrowdControlActionEvent actionEvent3)
		{
			OnCrowdControlActionEvent(actionEvent3, actionTarget);
		}
		else if (actionEvent is VfxEvent vfxEvent)
		{
			OnVfxEvent(vfxEvent, actionTarget);
		}
		else if (actionEvent is SfxEvent sfxEvent)
		{
			OnSfxEvent(sfxEvent, actionTarget);
		}
		else if (actionEvent is HitBoxIteratorEvent hitBoxIteratorEvent)
		{
			OnHitBoxIteratorEvent(hitBoxIteratorEvent, actionTarget, skillId, UnitModel.CalculatedDeltaAimPos, skillAction);
		}
		else if (actionEvent is SummonActionEvent summonActionEvent)
		{
			OnSummonActionEvent(summonActionEvent, actionTarget);
		}
		else if (actionEvent is SpecialFieldActionEvent specialFieldActionEvent)
		{
			OnSpecialFieldActionEvent(specialFieldActionEvent, actionTarget);
		}
		else if (actionEvent is DamageActionEvent actionEvent4)
		{
			OnDamageActionEvent(actionEvent4, actionTarget, skillId);
		}
		else if (actionEvent is RecoveryActionEvent actionEvent5)
		{
			OnRecoveryActionEvent(actionEvent5, actionTarget, skillId);
		}
		else if (actionEvent is StressDamageActionEvent actionEvent6)
		{
			OnStressDamageEvent(actionEvent6, actionTarget, skillId);
		}
		else if (actionEvent is StressRecoveryActionEvent actionEvent7)
		{
			OnStressRecoveryEvent(actionEvent7, actionTarget, skillId);
		}
		else if (actionEvent is SoulRecoveryActionEvent actionEvent8)
		{
			OnSoulRecoveryEvent(actionEvent8, actionTarget);
		}
		else if (actionEvent is BrightnessDamageActionEvent actionEvent9)
		{
			OnBrightnessDamageEvent(actionEvent9, actionTarget, skillId);
		}
		else if (actionEvent is BrightnessRecoveryActionEvent actionEvent10)
		{
			OnBrightnessRecoveryEvent(actionEvent10, actionTarget, skillId);
		}
		else if (actionEvent is CameraShakeActionEvent cameraShakeActionEvent)
		{
			OnCameraShakeActionEvent(cameraShakeActionEvent);
		}
		else if (actionEvent is KillActionEvent)
		{
			OnKillActionEvent(actionTarget);
		}
		else if (actionEvent is TeleportActionEvent teleportActionEvent)
		{
			OnTeleportActionEvent(teleportActionEvent, actionTarget, triggerTarget);
		}
		else if (actionEvent is ForceUpdateTargetActionEvent forceUpdateTargetActionEvent)
		{
			OnForceUpdateTargetActionEvent(forceUpdateTargetActionEvent, actionTarget);
		}
		else if (actionEvent is HitBoxEvent hitBoxEvent)
		{
			OnTargetableHitBoxEvent(hitBoxEvent, actionTarget, skillId, UnitModel.CalculatedDeltaAimPos, skillAction);
		}
		else if (actionEvent is ApplyPolymorphSkillsActionEvent applyPolymorphSkillsActionEvent)
		{
			OnApplyPolymorphSkillsEvent(applyPolymorphSkillsActionEvent, actionTarget);
		}
		else if (actionEvent is RevertPolymorphSkillsActionEvent revertPolymorphSkillsActionEvent)
		{
			OnRevertPolymorphSkillsEvent(revertPolymorphSkillsActionEvent, actionTarget);
		}
		else if (actionEvent is ChangeSkinActionEvent changeSkinActionEvent)
		{
			OnChangeSkinActionEvent(changeSkinActionEvent, actionTarget);
		}
		else if (actionEvent is SpreadAggroActionEvent spreadAggroActionEvent)
		{
			OnSpreadAggroActionEvent(spreadAggroActionEvent, actionTarget);
		}
		else if (actionEvent is MonologueActionEvent monologueActionEvent)
		{
			OnMonologueActionEvent(monologueActionEvent, actionTarget);
		}
		else if (actionEvent is ShieldActionEvent actionEvent11)
		{
			OnShieldActionEvent(actionEvent11, actionTarget);
		}
		else if (actionEvent is AcquireParadoxActionEvent actionEvent12)
		{
			OnAcquireParadoxActionEvent(actionEvent12, actionTarget);
		}
		else if (actionEvent is RemoveParadoxActionEvent actionEvent13)
		{
			OnRemoveParadoxActionEvent(actionEvent13, actionTarget);
		}
	}

	public virtual bool IsActionEventExcutable(ActionEvent actionEvent, ulong ownerNetworkObjectId, ulong targetNetworkObjectId)
	{
		return CheckBuffConditions(actionEvent.BuffConditionList, ownerNetworkObjectId, targetNetworkObjectId);
	}

	public bool CheckBuffCondition(BuffCondition buffCondition, ulong skillOwnerId, ulong targetId)
	{
		int num = (UnitModel.Buffs.TryGetValue(buffCondition.BuffId, out var value) ? value.StackCount : 0);
		bool flag;
		switch (buffCondition.BuffConditionType)
		{
		case EBuffConditionType.Equal:
			flag = num == buffCondition.BuffStack;
			break;
		case EBuffConditionType.GreaterThan:
			flag = buffCondition.BuffStack < num;
			break;
		case EBuffConditionType.LessThan:
			flag = num < buffCondition.BuffStack;
			break;
		case EBuffConditionType.GreaterThanOrEqual:
			flag = buffCondition.BuffStack <= num;
			break;
		case EBuffConditionType.LessThanOrEqual:
			flag = num <= buffCondition.BuffStack;
			break;
		default:
			flag = true;
			break;
		}
		bool flag2 = !UnitModel.Buffs.ContainsKey(buffCondition.BuffId);
		if (!flag2)
		{
			switch (buffCondition.BuffConditionCasterType)
			{
			case EBuffConditionCasterType.SkillOwner:
				if (!UnitModel.Buffs.TryGetValue(buffCondition.BuffId, out var value2))
				{
					flag2 = false;
				}
				else
				{
					flag2 = value2.CasterIds.Contains(skillOwnerId) != buffCondition.CheckNotContainsCasterType;
				}
				break;
			case EBuffConditionCasterType.SkillTarget:
				if (!UnitModel.Buffs.TryGetValue(buffCondition.BuffId, out var value3))
				{
					flag2 = false;
				}
				else
				{
					flag2 = value3.CasterIds.Contains(targetId) != buffCondition.CheckNotContainsCasterType;
				}
				break;
			default:
				flag2 = true;
				break;
			}
		}
		return flag2 & flag;
	}

	public bool CheckBuffConditions(IList<BuffCondition> buffConditionList, ulong ownerNetworkObjectId, ulong targetNetworkObjectId)
	{
		if (buffConditionList == null || buffConditionList.Count == 0)
		{
			return true;
		}
		UnitModel unitModel = ObjectManager.FindTarget(targetNetworkObjectId) as UnitModel;
		UnitController triggerTargetController = (unitModel != null) ? unitModel.GetComponent<UnitController>() : null;
		return buffConditionList.All(delegate(BuffCondition condition)
		{
			switch (condition.ConditionOwnerType)
			{
			case EConditionTargetType.SkillOwner:
				return CheckBuffCondition(condition, NetworkObjectId, (triggerTargetController != null) ? triggerTargetController.NetworkObjectId : 0);
			case EConditionTargetType.TriggerTarget:
				if (triggerTargetController == null)
				{
					return false;
				}
				return triggerTargetController.CheckBuffCondition(condition, NetworkObjectId, triggerTargetController.NetworkObjectId);
			default:
				return true;
			}
		});
	}

	public bool CheckTalentCondition(IList<TalentCondition> talentConditionList)
	{
		if (talentConditionList != null && talentConditionList.Count != 0)
		{
			return talentConditionList.All(delegate(TalentCondition condition)
			{
				switch (condition.TalentConditionType)
				{
				case ETalentConditionType.Owned:
					return UnitModel.HasTalent(condition.TalentId);
				case ETalentConditionType.NotOwned:
					return !UnitModel.HasTalent(condition.TalentId);
				default:
					return true;
				}
			});
		}
		return true;
	}

	public void ApplySkillCost(int skillId, int hpCost, int sanityCost, int stamina)
	{
		if (skillId > 0 && TSkill.Table.TryGetValue(skillId, out var value) && value.ChargeCost > 0)
		{
			UnitModel.ConsumeSkillCharge(skillId);
		}
		if (hpCost > 0)
		{
			HandleUnitEvent(new HpDamageEvent
			{
				Amount = hpCost,
				HitPosition = UnitModel.Position,
				IsFixedDamage = true,
				ReasonType = EHpDamageReasonType.SkillUsed
			});
		}
		if (sanityCost > 0)
		{
			HandleUnitEvent(new StressDamageEvent
			{
				SourceModel = UnitModel,
				Amount = sanityCost,
				HitPosition = UnitModel.Position,
				IsFixed = true,
				ReasonType = EStressReasonType.SkillUsed
			});
		}
		if (stamina > 0)
		{
			HandleUnitEvent(new StaminaDamageEvent
			{
				Amount = stamina
			});
		}
	}

	public void AdjustSkillCosts(TSkill skill, ref int hpCost, ref int stressCost, ref int staminaCost)
	{
		UnitModel.AdjustSkillCosts(skill, ref hpCost, ref stressCost, ref staminaCost);
	}

	public void OnBuffActionEvent(BuffActionEvent buffEvent, ITarget target)
	{
		if (!(target is UnitModel unitModel))
		{
			return;
		}
		if (buffEvent.FactionCondition != EMonsterCategory.None && !unitModel.MonsterCategories.Contains(buffEvent.FactionCondition))
		{
			return;
		}
		if (buffEvent.MonsterCondition != EMonsterType.None)
		{
			if (!(target is MonsterModel monsterModel))
			{
				return;
			}
			if (monsterModel.MonsterType != buffEvent.MonsterCondition)
			{
				return;
			}
		}
		if (buffEvent.IsRemove)
		{
			unitModel.RemoveBuff(buffEvent.Id, buffEvent.stackAmount, isExpired: false);
			return;
		}
		unitModel.GetComponent<UnitController>().HandleUnitEvent(new BuffEvent
		{
			SourceTarget = UnitModel,
			BuffId = buffEvent.Id,
			BuffStack = buffEvent.stackAmount
		});
	}

	public void OnRemoveBuffActionEvent(RemoveStatusEffectActionEvent actionEvent, ITarget target)
	{
		if (target is UnitModel unitModel)
		{
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new RemoveStatusEffectEvent
			{
				SourceTarget = UnitModel,
				EffectTags = actionEvent.EffectTags
			});
		}
	}

	public void OnCrowdControlActionEvent(CrowdControlActionEvent actionEvent, ITarget target)
	{
		if (!(target is UnitModel unitModel))
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		if (actionEvent.CrowdControlInfo.UseForcedMoveDirection)
		{
			switch (actionEvent.CrowdControlInfo.ForcedMoveDirectionType)
			{
			case ECCforcedMoveDirectionType.OwnerAim:
				vector = new Vector3(UnitModel.CalculatedDeltaAimPos.x, 0f, UnitModel.CalculatedDeltaAimPos.z);
				if (vector == Vector3.zero)
				{
					vector = new Vector3(UnitModel.Forward.x, 0f, UnitModel.Forward.z);
				}
				break;
			case ECCforcedMoveDirectionType.OwnerMove:
				vector = new Vector3(UnitModel.MoveDir.x, 0f, UnitModel.MoveDir.z);
				if (vector == Vector3.zero)
				{
					vector = new Vector3(UnitModel.CalculatedDeltaAimPos.x, 0f, UnitModel.CalculatedDeltaAimPos.z);
				}
				break;
			default:
				vector = Vector3.zero;
				break;
			}
			if (vector != Vector3.zero)
			{
				vector = vector.normalized;
			}
		}
		unitModel.GetComponent<UnitController>().HandleUnitEvent(new CrowdControlEvent
		{
			SourceModel = UnitModel,
			CcInfo = actionEvent.CrowdControlInfo,
			KnockbackHitPosition = UnitModel.Position,
			ForcedMoveDirection = vector
		});
	}

	public long OnVfxEvent(VfxEvent vfxEvent, ITarget target)
	{
		Vector3 vector = Vector3.forward;
		if (vfxEvent.InheritAimDir)
		{
			vector = UnitModel.CalculatedDeltaAimPos;
		}
		else if (vfxEvent.InheritMoveDir)
		{
			vector = UnitModel.MoveDir;
		}
		Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
		if (vector2 == Vector3.zero)
		{
			vector2 = Vector3.forward;
		}
		long num = UniqueIdFactory.CreateId();
		NtfDisplayFx ntfDisplayFx = new NtfDisplayFx();
		ntfDisplayFx.UniqueId = num;
		ntfDisplayFx.SkillOwnerNetworkObjectId = base.NetworkObjectId;
		ntfDisplayFx.Vfx = string.Format("Skill/{0}", vfxEvent.prefab);
		ntfDisplayFx.VfxDuration = vfxEvent.duration;
		if (target != null)
		{
			ntfDisplayFx.Position = target.Position.ToProtoVector3();
			ntfDisplayFx.Direction = vector2.ToProtoVector3();
			ntfDisplayFx.AnimSpeeds = vfxEvent.VfxSpeeds;
			ntfDisplayFx.IsLoopVfx = vfxEvent.IsLoop;
			ntfDisplayFx.LoopDuration = vfxEvent.LoopDuration;
			ntfDisplayFx.DestroyOnUnitFollowerTargetDeath = vfxEvent.DestroyOnUnitFollowerTargetDeath;
			if (!vfxEvent.IsIndependent)
			{
				ntfDisplayFx.TargetNetworkObjectId = target.NetworkObjectId;
				ntfDisplayFx.TargetPositionOffset = vfxEvent.offset.ToProtoVector3();
				ntfDisplayFx.BoneType = vfxEvent.boneType;
				ntfDisplayFx.InheritMoveDir = vfxEvent.InheritMoveDir;
				ntfDisplayFx.UpdateByAimDir = vfxEvent.UpdateByAimDir;
			}
			else
			{
				Vector3 normalized = vector2.normalized;
				Vector3 normalized2 = Vector3.Cross(normalized, Vector3.up).normalized;
				Vector3 normalized3 = Vector3.Cross(normalized2, normalized).normalized;
				Vector3 offset = vfxEvent.offset;
				ntfDisplayFx.Position = (target.Position + normalized * offset.z + normalized2 * offset.x + normalized3 * offset.y).ToProtoVector3();
			}
			GameHost?.SendToAll(ntfDisplayFx);
		}
		return num;
	}

	public void OnSfxEvent(SfxEvent sfxEvent, ITarget target)
	{
		GameHost.SendToAll(new NtfPlaySfx
		{
			Sfx = sfxEvent.Sfx,
			Position = target.Position.ToProtoVector3(),
			TargetNetworkObjectId = (sfxEvent.IsIndependent ? 0 : target.NetworkObjectId)
		});
	}

	public void OnIndicatorVfxEvent(IndicatorVfxEvent indicatorVfxEvent, UnitModel casterUnitModel, Vector3 deltaAimPos, ITarget triggerTarget)
	{
		foreach (Vector3 item in GetTargetPosition(indicatorVfxEvent.SpawnPositionType, triggerTarget, deltaAimPos, indicatorVfxEvent.TargetPlayerMaxDistance))
		{
			ObjectManager.SpawnIndicatorVfx(indicatorVfxEvent.IndicatorInfo, casterUnitModel, item);
		}
	}

	public void OnHitBoxIteratorEvent(HitBoxIteratorEvent hitBoxIteratorEvent, ITarget target, int skillId, Vector3 deltaAimPos, SkillAction currentAction, float timeScale = 1f, CancellationTokenSource cancellationTokenSource = null)
	{
		if (hitBoxIteratorEvent.HitBoxIterator == null)
		{
			return;
		}
		if (hitBoxIteratorEvent.CancelOnOwnerDying)
		{
			CancellationTokenSource cts = cancellationTokenSource ?? new CancellationTokenSource();
			cancellationTokenSource = cts;
			UnitModel.RxState.Where((EActionState state) => state == EActionState.Dying || state == EActionState.Dead).Take(1).Subscribe(delegate
			{
				if (cts != null && !cts.IsCancellationRequested)
				{
					cts.Cancel();
				}
			})
				.AddTo(this);
		}
		ActionStatsData ownerStats = UnitModel.ToStatsData();
		HitBoxIterator hitBoxIterator = hitBoxIteratorEvent.HitBoxIterator;
		HitLayerInfo hitLayerInfo = UnitModel.GetHitLayerInfo();
		bool isFlipped = UnitView.IsFlipped;
		ObjectManager.SpawnHitBoxByIterator(this, ownerStats, target, hitBoxIterator, hitLayerInfo, deltaAimPos, isFlipped, skillId, timeScale, hitBoxIteratorEvent.SpawnPositionType, hitBoxIteratorEvent.TargetPlayerMaxDistance, cancellationTokenSource);
	}

	public void OnSummonActionEvent(SummonActionEvent summonActionEvent, ITarget target)
	{
		Vector3 vector = ((UnitModel.Forward.magnitude != 0f) ? UnitModel.Forward.normalized : Vector3.forward);
		Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
		Vector3 vector2 = quaternion * summonActionEvent.SpawnOffset;
		Vector3 position = target.Position + vector2;
		ObjectManager.TrySpawnMonster(summonActionEvent.MonsterInfos, position, base.NetworkObjectId);
	}

	public void OnSpecialFieldActionEvent(SpecialFieldActionEvent specialFieldActionEvent, ITarget target)
	{
		Vector3 vector = ((UnitModel.Forward.magnitude != 0f) ? UnitModel.Forward.normalized : Vector3.forward);
		Quaternion quaternion = Quaternion.LookRotation(vector, Vector3.up);
		Vector3 vector2 = quaternion * specialFieldActionEvent.SpawnOffset;
		Vector3 position = target.Position + vector2;
		ObjectManager.SpawnSpecialField(base.NetworkObjectId, specialFieldActionEvent.SpecialFieldId, position, specialFieldActionEvent.Scale);
	}

	public void OnDamageActionEvent(DamageActionEvent actionEvent, ITarget target, int skillId)
	{
		if (target is UnitModel unitModel)
		{
			InGameUtility.DamageResult damageResult = InGameUtility.CalculateDamage(actionEvent.DamageInfo, UnitModel, unitModel, isBackAttack: false, UnitModel.GetSkillLevel(skillId), null, allowCritical: false);
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new HpDamageEvent
			{
				SourceModel = UnitModel,
				SourceIdentity = UnitIdentity.From(UnitModel),
				Amount = damageResult.Damage,
				IsCritical = damageResult.IsCritical,
				HitPosition = target.Position,
				ElementalDamagedType = InGameUtility.GetElementalDamagedType(actionEvent.DamageInfo, UnitModel, unitModel),
				SkillId = skillId,
				ElementalType = actionEvent.DamageInfo.ElementalType,
				ReasonType = EHpDamageReasonType.Hit,
				IgnoreShield = actionEvent.DamageInfo.IgnoreShield
			});
		}
	}

	public void OnRecoveryActionEvent(RecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
		if (!(target is UnitModel unitModel))
		{
			return;
		}
		UnitController component = unitModel.GetComponent<UnitController>();
		switch (actionEvent.EffectType)
		{
		case EHitEffectType.HpRecovery:
			component.HandleUnitEvent(new HpRecoveryEvent
			{
				Amount = actionEvent.Amount,
				SkillId = skillId
			});
			break;
		case EHitEffectType.StressRecovery:
			component.HandleUnitEvent(new StressRecoveryEvent
			{
				Amount = actionEvent.Amount,
				SkillId = skillId
			});
			break;
		case EHitEffectType.BrightnessRecovery:
			component.HandleUnitEvent(new LightFuelRecoveryEvent
			{
				Amount = actionEvent.Amount,
				SkillId = skillId
			});
			break;
		case EHitEffectType.StaminaRecovery:
			component.HandleUnitEvent(new StaminaRecoveryEvent
			{
				Amount = actionEvent.Amount
			});
			break;
		default:
			Logger.Error(string.Format("Unsupported RecoveryActionEvent EffectType: {0}", actionEvent.EffectType));
			break;
		}
	}

	public void OnStressDamageEvent(StressDamageActionEvent actionEvent, ITarget target, int skillId)
	{
		if (target is UnitModel unitModel && !unitModel.GetBuffEffects<InvincibilityEffect>().Any((BuffEffectInfo<InvincibilityEffect> _) => _.BuffEffect.BlockStressDamage))
		{
			float randomFloat = RandomUtility.GetRandomFloat(actionEvent.AmountMin, actionEvent.AmountMax);
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new StressDamageEvent
			{
				SourceModel = UnitModel,
				Amount = randomFloat,
				HitPosition = unitModel.Position,
				ReasonType = EStressReasonType.StressDamaged,
				SkillId = skillId
			});
		}
	}

	public void OnStressRecoveryEvent(StressRecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
		if (target is UnitModel unitModel)
		{
			float randomFloat = RandomUtility.GetRandomFloat(actionEvent.AmountMin, actionEvent.AmountMax);
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new StressRecoveryEvent
			{
				Amount = randomFloat,
				SkillId = skillId
			});
		}
	}

	public void OnSoulRecoveryEvent(SoulRecoveryActionEvent actionEvent, ITarget target)
	{
		if (target is UnitModel unitModel)
		{
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new SoulRecoveryEvent
			{
				Amount = actionEvent.Amount
			});
		}
	}

	public void OnBrightnessDamageEvent(BrightnessDamageActionEvent actionEvent, ITarget target, int skillId)
	{
		if (target is UnitModel unitModel)
		{
			int randomInt = RandomUtility.GetRandomInt(actionEvent.AmountMin, actionEvent.AmountMax + 1);
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new LightFuelDamageEvent
			{
				Amount = randomInt,
				SkillId = skillId
			});
		}
	}

	public void OnBrightnessRecoveryEvent(BrightnessRecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
		if (target is UnitModel unitModel)
		{
			int randomInt = RandomUtility.GetRandomInt(actionEvent.AmountMin, actionEvent.AmountMax + 1);
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new LightFuelRecoveryEvent
			{
				Amount = randomInt,
				SkillId = skillId
			});
		}
	}

	public void OnConsumeItemActionEvent(ConsumeItemActionEvent consumeItemActionEvent, ITarget target)
	{
		TItem tItem = TItem.Table[consumeItemActionEvent.ItemId];
		if (Inventory.GetItemCount(tItem.Id) >= 1)
		{
			UnitModel.ApplyConsumableTypeCoolTime(tItem.Id);
			HandleUnitEvent(new ConsumeItemEvent
			{
				SourceModel = UnitModel,
				ItemId = consumeItemActionEvent.ItemId
			});
		}
	}

	public void OnCameraShakeActionEvent(CameraShakeActionEvent cameraShakeActionEvent)
	{
		GameHost.SendToAll(new NtfCameraShake
		{
			TargetClientId = ClientId,
			IsGlobal = !cameraShakeActionEvent.IsLocalCharacterOnly,
			ShakeAmplitude = cameraShakeActionEvent.ShakeAmplitude,
			ShakeFrequency = cameraShakeActionEvent.ShakeFrequency,
			ShakeDuration = cameraShakeActionEvent.ShakeDuration
		});
	}

	public void OnKillActionEvent(ITarget target)
	{
		if (target is UnitModel unitModel && !(unitModel == null))
		{
			EActionState state = unitModel.State;
			if (state != EActionState.Dying && state != EActionState.Dead && state != EActionState.ReservedDespawn)
			{
				unitModel.GetComponent<UnitController>().HandleUnitEvent(new DieEvent
				{
					SourceModel = UnitModel,
					SourceIdentity = UnitIdentity.From(UnitModel)
				});
			}
		}
	}

	public void OnTeleportActionEvent(TeleportActionEvent teleportActionEvent, ITarget teleportActor, ITarget triggerTarget = null)
	{
		if (!(teleportActor is UnitModel unitModel))
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		switch (teleportActionEvent.DirectionType)
		{
		case ETeleportDirectionType.MoveDir:
			vector = UnitModel.MoveDir * RandomUtility.GetRandomFloat(teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
			break;
		case ETeleportDirectionType.CasterAimDir:
		case ETeleportDirectionType.MoveToCasterAimPos:
		{
			Vector3 vector2 = new Vector3(UnitModel.CalculatedDeltaAimPos.x, 0f, UnitModel.CalculatedDeltaAimPos.z);
			float num = Mathf.Clamp(vector2.magnitude, teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
			vector = vector2.normalized * num;
			break;
		}
		case ETeleportDirectionType.MoveToTriggerTarget:
			if (triggerTarget != null)
			{
				Vector3 vector3 = triggerTarget.Position - teleportActor.Position;
				float num2 = Mathf.Clamp(vector3.magnitude, teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
				vector = vector3.normalized * num2;
			}
			break;
		case ETeleportDirectionType.MoveToCaster:
		{
			Vector3 vector4 = UnitModel.Position - teleportActor.Position;
			float num3 = Mathf.Clamp(vector4.magnitude, teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
			vector = vector4.normalized * num3;
			break;
		}
		case ETeleportDirectionType.MoveDirFirstAimSecond:
			if (UnitModel.MoveDir.magnitude > float.Epsilon)
			{
				vector = UnitModel.MoveDir * RandomUtility.GetRandomFloat(teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
			}
			else
			{
				vector = new Vector3(UnitModel.CalculatedDeltaAimPos.x, 0f, UnitModel.CalculatedDeltaAimPos.z).normalized * RandomUtility.GetRandomFloat(teleportActionEvent.MinDistance, teleportActionEvent.MaxDistance);
			}
			break;
		}
		float num4 = vector.magnitude + teleportActionEvent.TeleportPositionOffset;
		Vector3 vector5 = vector.normalized * num4;
		Quaternion quaternion = Quaternion.Euler(0f, teleportActionEvent.TeleportAngleOffset, 0f);
		Vector3 deltaMovePos = quaternion * vector5;
		unitModel.GetComponent<UnitController>().Teleport(deltaMovePos, teleportActionEvent);
	}

	public void OnShieldActionEvent(ShieldActionEvent actionEvent, ITarget target)
	{
		if (target is UnitModel unitModel)
		{
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new ShieldEvent
			{
				SourceModel = UnitModel,
				ShieldId = actionEvent.ShieldId,
				Amount = (int)(float)(actionEvent.StatFactor * UnitModel.GetStat(actionEvent.CoefficientStat)) + actionEvent.Value,
				CasterId = base.NetworkObjectId
			});
		}
	}

	public void OnAddSuppressionActionEvent(AddSuppressionActionEvent actionEvent, ITarget target)
	{
		if (target is UnitModel unitModel)
		{
			unitModel.GetComponent<UnitController>().HandleUnitEvent(new SuppressionDamageEvent
			{
				Amount = actionEvent.Amount
			});
		}
	}

	public void OnAcquireParadoxActionEvent(AcquireParadoxActionEvent actionEvent, ITarget target)
	{
		if (target is CharacterModel characterModel && RandomUtility.GetRandomBoolFromPercentage(actionEvent.Probability))
		{
			GamePlayer gamePlayer = GameContext.Players[characterModel.ClientId];
			if (actionEvent.ParadoxId < 1)
			{
				gamePlayer.AcquireRandomParadox(actionEvent.IncludePositive, actionEvent.IncludeNegative);
			}
			else
			{
				gamePlayer.AcquireParadox(actionEvent.ParadoxId);
			}
		}
	}

	public void OnRemoveParadoxActionEvent(RemoveParadoxActionEvent actionEvent, ITarget target)
	{
		if (target is CharacterModel characterModel && RandomUtility.GetRandomBoolFromPercentage(actionEvent.Probability))
		{
			GamePlayer gamePlayer = GameContext.Players[characterModel.ClientId];
			if (actionEvent.ParadoxId < 1)
			{
				gamePlayer.RemoveRandomParadox(actionEvent.IncludePositive, actionEvent.IncludeNegative);
			}
			else
			{
				gamePlayer.RemoveParadox(actionEvent.ParadoxId);
			}
		}
	}

	public void Teleport(Vector3 deltaMovePos, TeleportActionEvent teleportActionEvent)
	{
		NavMeshHit hit = default(NavMeshHit);
		Vector3 position = base.transform.position;
		Vector3 normalized = deltaMovePos.normalized;
		for (float num = deltaMovePos.magnitude; num >= 0f; num -= 1f)
		{
			Vector3 sourcePosition = position + normalized * num;
			if (!NavMesh.SamplePosition(sourcePosition, out hit, 1f, -1))
			{
				continue;
			}
			if (teleportActionEvent.CanTeleportWithoutPath)
			{
				UnitView.MoveTo(hit.position);
				return;
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			if (NavMesh.CalculatePath(position, hit.position, -1, navMeshPath) && navMeshPath.GetTotalLength(float.MaxValue) < teleportActionEvent.MaxDistance * TConst.TeleportPathTolerance.FloatValue)
			{
				UnitView.MoveTo(hit.position);
				return;
			}
		}
	}

	public void OnForceUpdateTargetActionEvent(ForceUpdateTargetActionEvent forceUpdateTargetActionEvent, ITarget target)
	{
		if (target is UnitModel unitModel && !(unitModel != UnitModel))
		{
			List<ITarget> list = ObjectManager.FindTargets(this, forceUpdateTargetActionEvent);
			if (list != null && list.Count != 0)
			{
				HandleUnitEvent(new ForceSkillTargetUpdateEvent
				{
					NextTargets = list
				});
			}
		}
		else
		{
			Logger.Error("ForceUpdateTargetActionEvent can only be applied to the unit itself.");
		}
	}

	public void OnTargetableHitBoxEvent(HitBoxEvent hitBoxEvent, ITarget target, int skillId, Vector3 deltaAimPos, SkillAction currentAction)
	{
		if (!(target is UnitModel))
		{
			return;
		}
		foreach (Vector3 item in GetTargetPosition(hitBoxEvent.SpawnPositionType, target, deltaAimPos, hitBoxEvent.TargetPlayerMaxDistance))
		{
			ObjectManager.SpawnHitBoxAsync(hitBoxEvent.Id, UnitModel, UnitModel.ToStatsData(), UnitModel.GetHitLayerInfo(), deltaAimPos, item, UnitView.IsFlipped, skillId).Forget();
		}
	}

	public void OnApplyPolymorphSkillsEvent(ApplyPolymorphSkillsActionEvent applyPolymorphSkillsActionEvent, ITarget target)
	{
		if (!(target is CharacterModel characterModel))
		{
			return;
		}
		TCharacter characterData = TCharacter.Table[characterModel.CharacterId];
		List<int> list = characterModel.ActiveSkillIds.Select((int id) => ConvertToPolymorphSkillId(id, characterData)).ToList();
		Character character = GameContext.Players[characterModel.ClientId].Character?.Clone();
		if (character != null)
		{
			character.SelectedActiveSkillIds = character.SelectedActiveSkillIds.Select((int id) => ConvertToPolymorphSkillId(id, characterData)).ToArray();
			character.AttackSkillId = characterData.PolymorphAttackSkillId;
			character.DashSkillId = characterData.PolymorphDashSkillId;
			GameContext.Players[characterModel.ClientId].Character = character;
			GameHost.SendToAll(new NtfChangeActiveSkill
			{
				NetworkObjectId = characterModel.NetworkObjectId,
				SkillIds = list.ToArray()
			});
		}
	}

	public void OnRevertPolymorphSkillsEvent(RevertPolymorphSkillsActionEvent revertPolymorphSkillsActionEvent, ITarget target)
	{
		if (!(target is CharacterModel characterModel))
		{
			return;
		}
		TCharacter characterData = TCharacter.Table[characterModel.CharacterId];
		List<int> list = characterModel.ActiveSkillIds.Select((int id) => ConvertToOriginalSkillId(id, characterData)).ToList();
		Character character = GameContext.Players[characterModel.ClientId].Character?.Clone();
		if (character != null)
		{
			character.SelectedActiveSkillIds = character.SelectedActiveSkillIds.Select((int id) => ConvertToOriginalSkillId(id, characterData)).ToArray();
			character.AttackSkillId = characterData.AttackSkillId;
			character.DashSkillId = characterData.DashSkillId;
			GameContext.Players[characterModel.ClientId].Character = character;
			GameHost.SendToAll(new NtfChangeActiveSkill
			{
				NetworkObjectId = characterModel.NetworkObjectId,
				SkillIds = list.ToArray()
			});
		}
	}

	private int ConvertToPolymorphSkillId(int oldSkillId, TCharacter characterData)
	{
		if (oldSkillId == characterData.AttackSkillId)
		{
			return characterData.PolymorphAttackSkillId;
		}
		if (oldSkillId == characterData.DashSkillId)
		{
			return characterData.PolymorphDashSkillId;
		}
		int num = characterData.ActiveSkillIds.IndexOf(oldSkillId);
		if (num == -1)
		{
			return oldSkillId;
		}
		return characterData.PolymorphActiveSkillIds.ElementAt(num);
	}

	private int ConvertToOriginalSkillId(int polymorphSkillId, TCharacter characterData)
	{
		if (polymorphSkillId == characterData.PolymorphAttackSkillId)
		{
			return characterData.AttackSkillId;
		}
		if (polymorphSkillId == characterData.PolymorphDashSkillId)
		{
			return characterData.DashSkillId;
		}
		int num = characterData.PolymorphActiveSkillIds.IndexOf(polymorphSkillId);
		if (num == -1)
		{
			return polymorphSkillId;
		}
		return characterData.ActiveSkillIds.ElementAt(num);
	}

	public void OnChangeSkinActionEvent(ChangeSkinActionEvent changeSkinActionEvent, ITarget target)
	{
		if (target is CharacterModel)
		{
			GameHost.SendToAll(new NtfChangeCharacterSkin
			{
				NetworkObjectId = target.NetworkObjectId,
				IsSuper = changeSkinActionEvent.IsSuper
			});
		}
	}

	public void OnSpreadAggroActionEvent(SpreadAggroActionEvent spreadAggroActionEvent, ITarget target)
	{
		ObjectManager.UnitModels.Where((UnitModel _) => _ is MonsterModel monsterModel && !TMonster.Table[monsterModel.MonsterId].IgnoreReceiveAggroSpreadEvent).Where((UnitModel _) => Vector3.Distance(_.transform.position, UnitModel.transform.position) <= spreadAggroActionEvent.Distance).Where((UnitModel _) => _ != UnitModel)
			.ForEach(delegate(UnitModel _)
			{
				_.GetComponent<UnitController>().HandleUnitEvent(new ReceiveAggroSpreadEvent
				{
					Target = target
				});
			});
	}

	public void OnShowConsumeGaugeActionEvent(ShowConsumeGaugeActionEvent actionEvent)
	{
		if (UnitModel is CharacterModel characterModel)
		{
			GameHost.SendToAll(new NtfShowConsumeGauge
			{
				NetworkObjectId = characterModel.NetworkObjectId,
				ItemId = actionEvent.ItemId,
				Duration = actionEvent.Duration,
				OtherPlayerCanSee = actionEvent.OtherPlayerCanSee
			});
		}
		else
		{
			Logger.Error("ShowConsumeGaugeActionEvent can only be applied to CharacterModel.");
		}
	}

	public void OnHitBoxCollectActionEvent(HitBoxCollectActionEvent hitBoxCollectActionEvent, ITarget target)
	{
		foreach (HitBox hitBox in ObjectManager.GetHitBoxes())
		{
			if (hitBoxCollectActionEvent.HitBoxIds.Contains(hitBox.HitBoxId))
			{
				float collectingHitBoxSpeed = hitBoxCollectActionEvent.CollectingHitBoxSpeed;
				GameObject moveTarget = base.gameObject;
				hitBox.SetMoveData(EHitBoxMoveType.TraceMoveTarget, collectingHitBoxSpeed, null, default(Vector3), -1f, -1f, 0f, default(Vector3), moveTarget);
			}
		}
	}

	public void OnMonologueActionEvent(MonologueActionEvent monologueActionEvent, ITarget target)
	{
		UnitModel unitModel = target as UnitModel;
		if (unitModel == null)
		{
			return;
		}
		int num = Mathf.Clamp(monologueActionEvent.ActivationPercent, 0, 100);
		if (RandomUtility.GetRandomInt(0, 100) < num)
		{
			string randomMessage = monologueActionEvent.GetRandomMessage();
			if (!string.IsNullOrEmpty(randomMessage))
			{
				GameHost.SendToAll(new NtfUnitMonologueText
				{
					NetworkObjectId = unitModel.NetworkObjectId,
					TextKey = randomMessage
				});
			}
		}
	}

	public List<Vector3> GetTargetPosition(ESpawnPositionType spawnPositionType, ITarget target, Vector3 skillDeltaAimPos = default(Vector3), float playerMaxDistance = -1f)
	{
		List<Vector3> targetPositions = new List<Vector3>();
		if (UnitModel == null)
		{
			return targetPositions;
		}
		Vector3 ownerPosition = UnitModel.transform.position;
		Vector3 item = ownerPosition;
		switch (spawnPositionType)
		{
		case ESpawnPositionType.None:
		case ESpawnPositionType.Owner:
			break;
		case ESpawnPositionType.Aim:
			item = ownerPosition + skillDeltaAimPos;
			break;
		case ESpawnPositionType.MonsterTarget:
			if (UnitModel is MonsterModel monsterModel)
			{
				item = monsterModel.Target.Position;
			}
			break;
		case ESpawnPositionType.SkillTriggerTarget:
			item = target.Position;
			break;
		case ESpawnPositionType.AllPlayers:
			GameManager.CharacterModels.Where((CharacterModel _) => _ != null).Where((CharacterModel _) => _.State != EActionState.Dead).Where((CharacterModel _) => playerMaxDistance < 0f || Vector3.Distance(ownerPosition, _.transform.position) <= playerMaxDistance)
				.Select((CharacterModel _) => _.transform.position)
				.ForEach(delegate(Vector3 _)
				{
					targetPositions.Add(_);
				});
			return targetPositions;
		default:
			return targetPositions;
		}
		targetPositions.Add(item);
		return targetPositions;
	}

	protected virtual void RegisterUnitEventHandlers()
	{
		_eventMessageBroker.Receive<HpDamageEvent>().Subscribe(OnHpDamageEvent).AddTo(this);
		_eventMessageBroker.Receive<HpRecoveryEvent>().Subscribe(OnHpRecoveryEvent).AddTo(this);
		_eventMessageBroker.Receive<BuffEvent>().Subscribe(OnBuffEvent).AddTo(this);
		_eventMessageBroker.Receive<BuffRemovedEvent>().Subscribe(OnBuffRemovedEvent).AddTo(this);
		_eventMessageBroker.Receive<RemoveStatusEffectEvent>().Subscribe(OnRemoveStatusEffectEvent).AddTo(this);
		_eventMessageBroker.Receive<CrowdControlEvent>().Subscribe(OnCrowdControlEvent).AddTo(this);
		_eventMessageBroker.Receive<DieEvent>().Subscribe(OnDieEvent).AddTo(this);
		_eventMessageBroker.Receive<ReviveEvent>().Subscribe(OnReviveEvent).AddTo(this);
		_eventMessageBroker.Receive<CaptureEvent>().Subscribe(OnCaptureEvent).AddTo(this);
		_eventMessageBroker.Receive<UseSkillEvent>().Subscribe(OnUseSkillEvent).AddTo(this);
		_eventMessageBroker.Receive<AddPassiveEvent>().Subscribe(OnAddPassiveEvent).AddTo(this);
		_eventMessageBroker.Receive<ConsumeItemEvent>().Subscribe(OnConsumeItemUnitEvent).AddTo(this);
		_eventMessageBroker.Receive<MoveFinishEvent>().Subscribe(OnMoveFinishEvent).AddTo(this);
		_eventMessageBroker.Receive<ForceSkillTargetUpdateEvent>().Subscribe(OnForceSkillTargetUpdateEvent).AddTo(this);
		_eventMessageBroker.Receive<ShieldEvent>().Subscribe(OnShieldEvent).AddTo(this);
		_eventMessageBroker.Receive<EnterSpecialFieldEvent>().Subscribe(OnEnterSpecialFieldEvent).AddTo(this);
		_eventMessageBroker.Receive<ExitSpecialFieldEvent>().Subscribe(OnExitSpecialFieldEvent).AddTo(this);
		_eventMessageBroker.Receive<ParryEvent>().Subscribe(OnParryEvent).AddTo(this);
		_eventMessageBroker.Receive<SkillChargeEvent>().Subscribe(OnSkillChargeEvent).AddTo(this);
		_eventMessageBroker.Receive<PerSecondEvent>().Subscribe(OnPerSecondEvent).AddTo(this);
		_eventMessageBroker.Receive<StateChangeEvent>().Subscribe(OnStateChangeEvent).AddTo(this);
		_eventMessageBroker.Receive<NoticeDamageEvent>().Subscribe(OnNoticeDamage).AddTo(this);
		_eventMessageBroker.Receive<NoticeCrowdControlEvent>().Subscribe(OnNoticeCrowdControl).AddTo(this);
		_eventMessageBroker.Receive<NoticeDieEvent>().Subscribe(OnNoticeDie).AddTo(this);
		_eventMessageBroker.Receive<NoticeReviveEvent>().Subscribe(OnNoticeRevive).AddTo(this);
		_eventMessageBroker.Receive<NoticeRecoveryEvent>().Subscribe(OnNoticeRecovery).AddTo(this);
	}

	public void HandleUnitEvent<T>(T unitEvent) where T : IUnitEvent
	{
		if (!base.IsHost)
		{
			Logger.Error("HandleUnitEvent called on non-host: " + unitEvent.GetType().Name);
			return;
		}
		_eventMessageBroker.Publish(unitEvent);
		TryProceedPassiveChildNode(unitEvent);
	}

	protected void TriggerEvent(IUnitEvent unitEvent)
	{
		AssertIfNotHost();
		StateMachine.TriggerEvent(unitEvent);
		TriggerPassiveSkill(unitEvent);
	}

	private void TriggerPassiveSkill(IUnitEvent unitEvent)
	{
		List<ITarget> triggerTargets = null;
		foreach (int passiveSkillId in UnitModel.PassiveSkillIds)
		{
			SkillTrigger skillTrigger = TSkill.Table[passiveSkillId].RootActionNode.SkillTrigger;
			if (unitEvent.CheckSkillTriggered(UnitModel, skillTrigger, out triggerTargets))
			{
				ExecutePassiveSkill(passiveSkillId, unitEvent, triggerTargets);
			}
		}
		if (!(UnitModel is CharacterModel characterModel))
		{
			return;
		}
		List<ITarget> triggerTargets2 = null;
		foreach (int equipmentPassiveSkillId in characterModel.GetEquipmentPassiveSkillIds())
		{
			SkillTrigger skillTrigger2 = TSkill.Table[equipmentPassiveSkillId].RootActionNode.SkillTrigger;
			if (unitEvent.CheckSkillTriggered(UnitModel, skillTrigger2, out triggerTargets2))
			{
				ExecutePassiveSkill(equipmentPassiveSkillId, unitEvent, triggerTargets2);
			}
		}
	}

	protected virtual int CalculateDamage(HpDamageEvent unitEvent)
	{
		if (UnitModel.IsNonCombat)
		{
			return 0;
		}
		if (!UnitModel.IsHitable)
		{
			return 0;
		}
		if (UnitModel.Hp < 1)
		{
			return 0;
		}
		int num = unitEvent.Amount;
		if (num < 1)
		{
			return 0;
		}
		if (unitEvent.IsFixedDamage)
		{
			return num;
		}
		IList<BuffEffectInfo<HpDamageTakenAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<HpDamageTakenAmplifierEffect>();
		float num2 = 1f;
		if (buffEffects.Any())
		{
			ESkillTagType[] combinedSkillTags = InGameUtility.GetCombinedSkillTags(unitEvent.SkillId, unitEvent.HitBoxId);
			if (combinedSkillTags.Length == 0)
			{
				float num3 = buffEffects.Where((BuffEffectInfo<HpDamageTakenAmplifierEffect> e) => e.BuffEffect.SkillTags == null || e.BuffEffect.SkillTags.Length == 0 || e.BuffEffect.SkillTags.Contains(ESkillTagType.None)).Sum((BuffEffectInfo<HpDamageTakenAmplifierEffect> e) => (float)e.Stack * e.BuffEffect.Percent);
				if (Mathf.Abs(num3) > 0.0001f)
				{
					num2 = num3 / 100f + 1f;
				}
			}
			else
			{
				float num4 = 0f;
				float num5 = 0f;
				foreach (BuffEffectInfo<HpDamageTakenAmplifierEffect> item in buffEffects)
				{
					ESkillTagType[] effectSkillTags = item.BuffEffect.SkillTags;
					float num6 = (float)item.Stack * item.BuffEffect.Percent;
					if (effectSkillTags == null || effectSkillTags.Length == 0 || effectSkillTags.Contains(ESkillTagType.None))
					{
						num4 += num6;
					}
					else if (combinedSkillTags.Any((ESkillTagType tag) => effectSkillTags.Contains(tag)))
					{
						num5 += num6;
					}
				}
				if (Mathf.Abs(num4 + num5) > 0.0001f)
				{
					num2 = (num4 + num5) / 100f + 1f;
				}
			}
		}
		num2 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
		return Math.Max((int)(float)((float)num * num2), 1);
	}

	protected virtual void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
		if (unitEvent.ElementalType == EElementalType.Fire && UnitModel.IsCrowdControlled(ECrowdControlType.Freeze) && _freezeDurationReductionCooldownEndTime <= Time.time)
		{
			UnitModel.ReduceCrowdControlDuration(ECrowdControlType.Freeze, TConst.FireDamageFreezeDurationReductionPercent.FloatValue);
			_freezeDurationReductionCooldownEndTime = Time.time + TConst.FireDamageFreezeDurationReductionCooldown.FloatValue;
			GameHost.SendToAll(new NtfOnFreezeFireDamage
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
		if (unitEvent.ReasonType == EHpDamageReasonType.SkillUsed || !UnitModel.IsDamageBlockedByInvincibility(unitEvent.IsCritical, unitEvent.IsBackAttack))
		{
			int amount = CalculateDamage(unitEvent);
			List<BuffEffectInfo<DamageConversionEffect>> list = UnitModel.GetBuffEffects<DamageConversionEffect>().Where((BuffEffectInfo<DamageConversionEffect> _) => _.BuffEffect.ElementalType == unitEvent.ElementalType).ToList();
			if (!list.Any())
			{
				if (amount < 1)
				{
					if (UnitModel.Hp <= 0)
					{
						HandleUnitEvent(new DieEvent
						{
							SourceModel = unitEvent.SourceModel,
							SourceIdentity = unitEvent.SourceIdentity
						});
					}
					return;
				}
				unitEvent.Damage = amount;
				int num = (unitEvent.IgnoreShield ? amount : UnitModel.AbsorbDamageWithShields(amount));
				int num2 = 0;
				if (GameContext.IsTutorial && this is CharacterController)
				{
					num2 = Mathf.CeilToInt(UnitModel.HpMax * 0.2f);
				}
				IList<BuffEffectInfo<HpThresholdEffect>> buffEffects = UnitModel.GetBuffEffects<HpThresholdEffect>();
				int num3 = num2;
				if (buffEffects.Count > 0)
				{
					float num4 = buffEffects.Max((BuffEffectInfo<HpThresholdEffect> _) => _.BuffEffect.ThresholdPercent);
					num3 = Mathf.CeilToInt(UnitModel.HpMax * num4 / 100f);
					if (num3 < num2)
					{
						num3 = num2;
					}
				}
				int num5 = num3;
				if (unitEvent.ReasonType == EHpDamageReasonType.SkillUsed)
				{
					num5 = 1;
					if (1 < num3)
					{
						num5 = num3;
					}
				}
				int num6 = UnitModel.Hp - num;
				if (num6 <= num5)
				{
					num6 = num5;
				}
				UnitModel.Hp = num6;
				GameHost.SendToAll(new NtfHpDamage
				{
					SourceNetworkObjectId = ((unitEvent.SourceModel == null) ? ulong.MaxValue : unitEvent.SourceModel.NetworkObjectId),
					NetworkObjectId = base.NetworkObjectId,
					Amount = amount,
					HitPosition = unitEvent.HitPosition.ToProtoVector3(),
					IsCritical = unitEvent.IsCritical,
					ElementalDamagedType = unitEvent.ElementalDamagedType,
					IsBackAttack = unitEvent.IsBackAttack,
					ReasonType = unitEvent.ReasonType
				});
				ObjectManager.BroadcastUnitEvent(new NoticeDamageEvent
				{
					SourceModel = unitEvent.SourceModel,
					TargetModel = UnitModel,
					SkillId = unitEvent.SkillId,
					Damage = amount,
					IsCritical = unitEvent.IsCritical,
					IsBackAttack = unitEvent.IsBackAttack,
					IsFirstHit = unitEvent.IsFirstHit,
					ReasonType = unitEvent.ReasonType
				});
				TriggerEvent(unitEvent);
				if (UnitModel.Hp <= 0)
				{
					HandleUnitEvent(new DieEvent
					{
						SourceModel = unitEvent.SourceModel,
						SourceIdentity = unitEvent.SourceIdentity
					});
				}
				OnHpDamageApplied(unitEvent, num);
				return;
			}
			float num7 = list.Sum((BuffEffectInfo<DamageConversionEffect> e) => (float)e.Stack * e.BuffEffect.Percent);
			HandleUnitEvent(new HpRecoveryEvent
			{
				ReasonType = EHpRecoveryReasonType.DamageConversion,
				Amount = (int)(float)(num7 * (float)amount / 100f)
			});
		}
		else
		{
			GameHost.SendToAll(new NtfOnHitInvincibility
			{
				NetworkObjectId = base.NetworkObjectId
			});
		}
		unitEvent.Damage = 0;
	}

	protected virtual void OnHpDamageApplied(HpDamageEvent unitEvent, int hpDamage)
	{
	}

	private void OnHpRecoveryEvent(HpRecoveryEvent unitEvent)
	{
		if (UnitModel.IsNonCombat || !UnitModel.IsHitable)
		{
			return;
		}
		int num = CalculateRecoveryAmount();
		if (num < 1)
		{
			return;
		}
		GameHost.SendToAll(new NtfHpRecovery
		{
			NetworkObjectId = base.NetworkObjectId,
			Amount = num,
			ReasonType = unitEvent.ReasonType
		});
		UnitModel.Hp = Math.Min(num + UnitModel.Hp, (int)UnitModel.HpMax);
		TriggerEvent(unitEvent);
		ObjectManager.BroadcastUnitEvent(new NoticeRecoveryEvent
		{
			SourceModel = unitEvent.SourceModel,
			TargetModel = UnitModel,
			SkillId = unitEvent.SkillId,
			Amount = num,
			ReasonType = unitEvent.ReasonType
		});
		int CalculateRecoveryAmount()
		{
			if (UnitModel.Hp >= (int)UnitModel.HpMax)
			{
				return 0;
			}
			IList<BuffEffectInfo<HpRecoveryAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<HpRecoveryAmplifierEffect>();
			float num2 = 1f;
			if (buffEffects.Any())
			{
				num2 = buffEffects.Sum((BuffEffectInfo<HpRecoveryAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
			}
			if (unitEvent.SkillId > 0 && TSkill.Table[unitEvent.SkillId].SkillTags.Contains(ESkillTagType.Item))
			{
				IList<BuffEffectInfo<ItemEffectAmplifierEffect>> buffEffects2 = UnitModel.GetBuffEffects<ItemEffectAmplifierEffect>();
				if (buffEffects2.Any())
				{
					num2 *= buffEffects2.Sum((BuffEffectInfo<ItemEffectAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
				}
			}
			num2 = Mathf.Clamp(num2, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			return Math.Max((int)(float)((float)unitEvent.Amount * num2), 1);
		}
	}

	private void OnBuffEvent(BuffEvent unitEvent)
	{
		IActionStats actionStats = unitEvent.SourceTarget as IActionStats;
		int buffId = unitEvent.BuffId;
		int buffStack = unitEvent.BuffStack;
		ulong casterNetworkObjectId = ((unitEvent.SourceTarget != null) ? unitEvent.SourceTarget.NetworkObjectId : 0);
		UnitModel.AddBuff(buffId, actionStats, buffStack, casterNetworkObjectId, default(Vector3));
		TriggerEvent(unitEvent);
	}

	private void OnBuffRemovedEvent(BuffRemovedEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnRemoveStatusEffectEvent(RemoveStatusEffectEvent unitEvent)
	{
		UnitModel.RemoveStatusEffects(unitEvent.EffectTags);
		TriggerEvent(unitEvent);
	}

	protected virtual void OnCrowdControlEvent(CrowdControlEvent unitEvent)
	{
		if (!CheckCcInfoCondition(unitEvent.CcInfo))
		{
			unitEvent.CcInfo.ccType = ECrowdControlType.None;
		}
		if (unitEvent.CcInfo.ccType != ECrowdControlType.None)
		{
			ECrowdControlType eCrowdControlType = UnitModel.GetImmunizedCcType(unitEvent.CcInfo.ccType);
			if (eCrowdControlType == ECrowdControlType.None)
			{
				EStatusEffectTag statusEffectTag = unitEvent.CcInfo.ccType.GetEffectTag();
				GameHost.SendToAll(new NtfOnImmuneStatusEffect
				{
					NetworkObjectId = base.NetworkObjectId,
					StatusEffectTag = statusEffectTag,
					IsFullyImmune = UnitModel.HasFullyImmuneEffect()
				});
			}
			unitEvent.CcInfo.ccType = eCrowdControlType;
			if (eCrowdControlType != ECrowdControlType.None)
			{
				ObjectManager.BroadcastUnitEvent(new NoticeCrowdControlEvent
				{
					SourceModel = unitEvent.SourceModel,
					TargetModel = UnitModel,
					CcInfo = unitEvent.CcInfo
				});
			}
		}
		TriggerEvent(unitEvent);
	}

	protected virtual void OnDieEvent(DieEvent unitEvent)
	{
		AssertIfNotHost();
		UnitModel.Hp = 0;
		foreach (BuffEffectInfo<SpawnHitBoxOnEndEffect> buffEffect in UnitModel.GetBuffEffects<SpawnHitBoxOnEndEffect>())
		{
			SpawnHitBoxOnEndEffect effect = buffEffect.BuffEffect;
			if (effect == null || effect.HitBoxId <= 0 || !effect.SpawnOnDie || buffEffect.CasterIds == null)
			{
				continue;
			}
			for (int i = 0; i < buffEffect.CasterIds.Length; i++)
			{
				UnitModel unitModel = ObjectManager.FindUnitModel(buffEffect.CasterIds[i]);
				if (!(unitModel == null))
				{
					Vector3 position = UnitModel.Position;
					Vector3 forward = unitModel.Forward;
					if (forward == Vector3.zero)
					{
						forward = Vector3.forward;
					}
					ObjectManager.SpawnHitBoxAsync(effect.HitBoxId, unitModel, unitModel.ToStatsData(), unitModel.GetHitLayerInfo(), forward, position).Forget();
				}
			}
		}
		TriggerEvent(unitEvent);
		ObjectManager.BroadcastUnitEvent(new NoticeDieEvent
		{
			SourceModel = unitEvent.SourceModel,
			SourceIdentity = unitEvent.SourceIdentity,
			TargetModel = UnitModel
		});
	}

	private void OnUseSkillEvent(UseSkillEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnMoveFinishEvent(MoveFinishEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnAddPassiveEvent(AddPassiveEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected virtual void OnReviveEvent(ReviveEvent unitEvent)
	{
		TriggerEvent(unitEvent);
		ObjectManager.BroadcastUnitEvent(new NoticeReviveEvent
		{
			SourceModel = unitEvent.SourceModel,
			TargetModel = UnitModel
		});
		UnitModel.UpdateSpecialFieldsOnRevive();
	}

	private void OnCaptureEvent(CaptureEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnForceSkillTargetUpdateEvent(ForceSkillTargetUpdateEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnEnterSpecialFieldEvent(EnterSpecialFieldEvent unitEvent)
	{
		UnitModel.OnEnterSpecialField(unitEvent.NetworkObjectId);
		TriggerEvent(unitEvent);
	}

	private void OnExitSpecialFieldEvent(ExitSpecialFieldEvent unitEvent)
	{
		UnitModel.OnExitSpecialField(unitEvent.NetworkObjectId);
		TriggerEvent(unitEvent);
	}

	protected virtual void OnNoticeDamage(NoticeDamageEvent unitEvent)
	{
		TriggerEvent(unitEvent);
		if (unitEvent.SourceModel == UnitModel)
		{
			ApplyShieldOnDamageProvide();
			ApplySkillChargeOnDotDamageProvide();
			ApplyBuffOnAttackProvide();
		}
		void ApplyBuffOnAttackProvide()
		{
			if (unitEvent.ReasonType == EHpDamageReasonType.Dot || unitEvent.ReasonType == EHpDamageReasonType.DotRange || unitEvent.Damage <= 0 || unitEvent.TargetModel == null)
			{
				return;
			}
			IList<BuffEffectInfo<ApplyBuffOnAttackEffect>> buffEffects = UnitModel.GetBuffEffects<ApplyBuffOnAttackEffect>();
			if (!buffEffects.Any())
			{
				return;
			}
			foreach (BuffEffectInfo<ApplyBuffOnAttackEffect> item in buffEffects)
			{
				if (RandomUtility.GetRandomBoolFromPercentage(item.BuffEffect.Probability))
				{
					unitEvent.TargetModel.AddBuff(item.BuffEffect.TargetBuffId, UnitModel, 1, 0uL, default(Vector3));
				}
			}
		}
		void ApplyShieldOnDamageProvide()
		{
			int damage = unitEvent.Damage;
			if (damage < 1)
			{
				return;
			}
			IList<BuffEffectInfo<ShieldOnDamageProvideEffect>> buffEffects = UnitModel.GetBuffEffects<ShieldOnDamageProvideEffect>();
			if (!buffEffects.Any())
			{
				return;
			}
			foreach (IGrouping<int, BuffEffectInfo<ShieldOnDamageProvideEffect>> item in buffEffects.GroupBy((BuffEffectInfo<ShieldOnDamageProvideEffect> e) => e.BuffEffect.ShieldId).Where((IGrouping<int, BuffEffectInfo<ShieldOnDamageProvideEffect>> g) => g.Count() > 0))
			{
				float num = item.Sum((BuffEffectInfo<ShieldOnDamageProvideEffect> e) => (float)e.Stack * e.BuffEffect.Percent);
				int num2;
				if (num > 0f && (num2 = Mathf.FloorToInt((float)damage * num / 100f)) > 0)
				{
					HandleUnitEvent(new ShieldEvent
					{
						SourceModel = UnitModel,
						ShieldId = item.Key,
						Amount = num2,
						CasterId = UnitModel.NetworkObjectId
					});
				}
			}
		}
		void ApplySkillChargeOnDotDamageProvide()
		{
			if (unitEvent.ReasonType != EHpDamageReasonType.Dot && unitEvent.ReasonType != EHpDamageReasonType.DotRange)
			{
				return;
			}
			if (unitEvent.SkillId > 0 && TSkill.Table[unitEvent.SkillId].ChargeCost > 0)
			{
				return;
			}
			int damage = unitEvent.Damage;
			if (damage >= 1 && UnitModel.Atk > 0f)
			{
				HandleUnitEvent(new SkillChargeEvent
				{
					SourceModel = UnitModel,
					Amount = (float)damage / UnitModel.Atk
				});
			}
		}
	}

	protected virtual void OnNoticeCrowdControl(NoticeCrowdControlEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected virtual void OnNoticeDie(NoticeDieEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected virtual void OnNoticeRevive(NoticeReviveEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	protected virtual void OnNoticeRecovery(NoticeRecoveryEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnParryEvent(ParryEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnSkillChargeEvent(SkillChargeEvent unitEvent)
	{
		float addAmount = unitEvent.Amount;
		IList<BuffEffectInfo<SkillChargeAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<SkillChargeAmplifierEffect>();
		if (buffEffects.Any())
		{
			float num = 1f;
			float num2 = buffEffects.Sum((BuffEffectInfo<SkillChargeAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent);
			if (Mathf.Abs(num2) > 0.0001f)
			{
				num = num2 / 100f + 1f;
			}
			num = Mathf.Clamp(num, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			int num3 = (int)(float)(num * addAmount);
			if (num3 < 1)
			{
				num3 = 0;
			}
			addAmount = num3;
		}
		int[] array = UnitModel.Skills.Keys.Where(delegate(int key)
		{
			return TSkill.Table.TryGetValue(key, out var value) && value.ChargeCost > 0;
		}).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			int num4 = array[i];
			if (!(UnitModel is CharacterModel characterModel) || !characterModel.IsEquipmentActiveSkillChargeBlocked(num4))
			{
				UnitModel.AddSkillCharge(num4, addAmount);
			}
		}
		TriggerEvent(unitEvent);
	}

	private void OnConsumeItemUnitEvent(ConsumeItemEvent unitEvent)
	{
		if (unitEvent.ItemId > 0)
		{
			GenerateConsumeResultItems(unitEvent.ItemId);
			if (!GameManager.IsInLounge)
			{
				TItem tItem2 = TItem.Table[unitEvent.ItemId];
				GoodsData goodsData = default(GoodsData);
				goodsData.Id = unitEvent.ItemId;
				goodsData.Type = tItem2.GoodsType;
				goodsData.Count = 1L;
				Inventory.ConsumeGoods(goodsData);
			}
			ulong clientId = ClientId;
			GameHost.SendToClient(clientId, new NtfItemUsed
			{
				ClientId = ClientId,
				ItemId = unitEvent.ItemId
			});
		}
		TriggerEvent(unitEvent);
	}

	private void OnShieldEvent(ShieldEvent unitEvent)
	{
		UnitModel.AddShield(unitEvent.ShieldId, unitEvent.Amount, unitEvent.CasterId);
		TriggerEvent(unitEvent);
	}

	private void OnPerSecondEvent(PerSecondEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}

	private void OnStateChangeEvent(StateChangeEvent unitEvent)
	{
		TriggerEvent(unitEvent);
	}



}
