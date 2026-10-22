using System.Collections.Generic;
using System.Linq;
using De.InGame.Unit;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using GameCommon.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace De.InGame.Fsm
{
	public abstract class MonsterStateBase : UnitStateBase
	{
		protected class AiSkillDecision
		{
			public int SkillId;

			public UnitModel Target;
		}

		public override EActionState ActionState => EActionState.None;

		protected TMonster MonsterData => TMonster.Table[MonsterModel.MonsterId];

		protected MonsterModel MonsterModel => StateMachine.UnitModel as MonsterModel;

		protected MonsterView MonsterView => StateMachine.UnitView as MonsterView;

		protected Vector3 SpawnPosition => MonsterView.SpawnPosition;

		protected Vector3 OwnerPosition => StateMachine.UnitView.transform.position;

		protected MonsterStateMachine MonsterStateMachine => StateMachine as MonsterStateMachine;

		protected MonsterController MonsterController => StateMachine.UnitController as MonsterController;

		protected ITarget Target => MonsterModel.Target;

		protected Vector3 TargetPosition
		{
			get
			{
				ITarget target = MonsterModel.Target;
				return (target != null) ? target.Position : Vector3.zero;
			}
		}

		protected bool HasKiteRange
		{
			get
			{
				if (MonsterData.KiteStartRange <= 0f)
				{
					return false;
				}
				return MonsterData.KiteEndRange > 0f;
			}
		}

		protected NavMeshAgent NavMeshAgent => MonsterStateMachine.MonsterController.NavMeshAgent;

		protected MonsterStateBase(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		protected void SetTarget(ITarget target)
		{
			MonsterModel.SetTarget(target);
			if ((TargetPosition - StateMachine.UnitView.transform.position).sqrMagnitude >= 1E-10f)
			{
				return;
			}
			string message = string.Format("Owner = TargetPos : {0}\n", StateMachine.UnitModel) + string.Format("OwnerPosition: {0}, TargetPosition: {1}\n", StateMachine.UnitView.transform.position, TargetPosition) + string.Format("ActionState: {0}, PreviousState: {1}", ActionState, StateMachine.PreviousState);
			Logger.Warning(message, new Color(1f, 0f, 1f, 1f));
		}

		protected ITarget FindEnemyInSight()
		{
			if (GameManager.StageState != EStageState.Playing)
			{
				return null;
			}
			if (MonsterModel.IsShadow)
			{
				if (MonsterModel.Target is IHitable hitable && hitable.IsHitable)
				{
					if (!IsTargetInSight(MonsterModel.Target))
					{
						return null;
					}
					return MonsterModel.Target;
				}
				if (MonsterModel.OriginalTargetNetworkObjectId != 0)
				{
					ITarget target = ObjectManager.FindTarget(MonsterModel.OriginalTargetNetworkObjectId);
					if (target is IHitable hitable2 && hitable2.IsHitable && IsTargetInSight(target))
					{
						return target;
					}
				}
				return null;
			}
			if (GameManager.CharacterModels.All((CharacterModel c) => TConst.MonsterSpawnRangeMax.FloatValue < Vector3.Distance(c.Position, StateMachine.UnitView.transform.position)))
			{
				return null;
			}
			return (ITarget)ObjectManager.UnitModels.Where((UnitModel u) => IsAliveState(u)).Where(IsEnemy).Where((UnitModel u) => IsAliveState(u))
				.Where((UnitModel u) => !u.HasBuffEffect(EBuffEffectType.Invisible))
				.Where((UnitModel u) => IsTargetInSight(u))
				.OrderBy(HighStressFirstDistance)
				.FirstOrDefault(delegate(UnitModel unit)
				{
					if (MonsterData.IgnoreObstaclesForDetection)
					{
						return true;
					}
					Vector3 vector = unit.transform.position + Vector3.up * 0.5f;
					Vector3 vector2 = StateMachine.UnitModel.transform.position + Vector3.up * 0.5f;
					Vector3 normalized = (vector2 - vector).normalized;
					float distance = Vector3.Distance(vector, vector2);
					return !Physics.Raycast(vector, normalized, out var _, distance, Const.LayerMask.HIGH_OBSTACLE);
				});
		}

		private static bool IsAliveState(UnitModel unit)
		{
			EActionState value = unit.RxState.Value;
			switch (value)
			{
			case EActionState.Dying:
			case EActionState.Dead:
			case EActionState.Revive:
			case EActionState.PhaseUp:
			case EActionState.Cutscene:
			case EActionState.ReservedDespawn:
			case EActionState.Escaped:
			case EActionState.Captured:
			case EActionState.StageReady:
			case EActionState.Spawn:
				return false;
			default:
				return true;
			}
		}

		public bool IsEnemy(IHitable target)
		{
			HitLayerInfo hitLayerInfo = target.GetHitLayerInfo();
			HitLayerInfo hitLayerInfo2 = MonsterModel.GetHitLayerInfo();
			bool result = false;
			if (hitLayerInfo2.FactionId > 0 && hitLayerInfo.FactionId > 0)
			{
				TFaction tFaction = TFaction.Table[hitLayerInfo2.FactionId];
				result = tFaction.EnemyFactionIds != null && tFaction.EnemyFactionIds.Contains(hitLayerInfo.FactionId);
			}
			if (hitLayerInfo2.HitLayer / 1000 != hitLayerInfo.HitLayer / 1000)
			{
				result = true;
			}
			return result;
		}

		protected bool IsTargetInSight(ITarget target, float sightRangeMultiplier = 1f, float sightBackRangeMultiplier = 1f)
		{
			if (target == null)
			{
				return false;
			}
			float num = Distance(target);
			if (StateMachine.UnitModel.HasBuffEffect(EBuffEffectType.Blind))
			{
				float num2 = TConst.BlindSightRange.FloatValue;
				float sightBackRange = MonsterData.SightBackRange;
				if (sightBackRange <= num2)
				{
					num2 = sightBackRange;
				}
				return num <= num2;
			}
			float num3 = 1f;
			float num4 = 1f;
			IList<BuffEffectInfo<MonsterSightAmplifierEffect>> buffEffects = StateMachine.UnitModel.GetBuffEffects<MonsterSightAmplifierEffect>();
			if (buffEffects.Any())
			{
				num3 = buffEffects.Sum((BuffEffectInfo<MonsterSightAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
			}
			if (target is UnitModel unitModel)
			{
				IList<BuffEffectInfo<AggroRangeAmplifierEffect>> buffEffects2 = unitModel.GetBuffEffects<AggroRangeAmplifierEffect>();
				if (buffEffects2.Any())
				{
					num4 = buffEffects2.Sum((BuffEffectInfo<AggroRangeAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
				}
			}
			num4 *= num3;
			float num5 = Mathf.Clamp(num4, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
			if (MonsterModel.IsInCombat)
			{
				num5 = MonsterData.BattleSightRangePercent / 100f * num5;
			}
			else
			{
				TAnomaly tAnomaly = TAnomaly.Table[GameContext.AnomalyType];
				if (tAnomaly.EffectTypes != null && tAnomaly.EffectTypes.Contains(EAnomalyEffectType.MonsterNoneCombatSightChange))
				{
					num5 *= TConst.AnomalyMonsterNoneCombatSightMultiplier.FloatValue;
				}
			}
			if (num <= num5 * MonsterData.SightBackRange * sightBackRangeMultiplier)
			{
				return true;
			}
			if (num5 * MonsterData.SightRange * sightRangeMultiplier < num)
			{
				return false;
			}
			Vector3 normalized = new Vector3(target.Position.x - StateMachine.UnitModel.transform.position.x, 0f, target.Position.z - StateMachine.UnitModel.transform.position.z).normalized;
			Vector3 forward = GetForward();
			Vector3 normalized2 = new Vector3(forward.x, 0f, forward.z).normalized;
			if (normalized2 == Vector3.zero)
			{
				string message = string.Format("{0} GetForward() is 0 at state {1}, prev {2}\n", StateMachine.UnitModel, ActionState, StateMachine.PreviousState) + string.Format("Calculated DeltaAimPos: {0}\n", StateMachine.UnitModel.CalculatedDeltaAimPos) + string.Format("MoveDir: {0}", StateMachine.UnitModel.MoveDir);
				Logger.Warning(message);
				return false;
			}
			float num6 = (MonsterModel.IsInCombat ? 360f : MonsterData.SightAngle);
			return Mathf.Cos(num6 * 0.5f * 0.017453292f) <= Vector3.Dot(normalized, normalized2);
		}

		protected bool IsTargetInDirectAggroSight(ITarget target)
		{
			return IsTargetInSight(target, TConst.MonsterDirectAggroSightRangeMultiplier.FloatValue, TConst.MonsterDirectAggroSightBackRangeMultiplier.FloatValue);
		}

		protected float Distance(ITarget target)
		{
			return Vector3.Distance(StateMachine.UnitView.transform.position, target.Position);
		}

		private float HighStressFirstDistance(UnitModel target)
		{
			if (MonsterData.TargetHighStressPlayer && target.Stress > TConst.TargetHighStressPlayerTriggerStress.FloatValue)
			{
				return Vector3.Distance(SpawnPosition, target.transform.position) - float.MaxValue;
			}
			return Vector3.Distance(MonsterView.SpawnPosition, target.transform.position);
		}

		protected bool IsTargetInRange(float range)
		{
			if (MonsterModel.Target == null)
			{
				return false;
			}
			return Distance(MonsterModel.Target) <= range;
		}

		protected bool IsTargetOutRange(float range)
		{
			return !IsTargetInRange(range);
		}

		protected bool CheckSkill(out AiSkillDecision aiSkillDecision)
		{
			aiSkillDecision = null;
			UnitModel unitModel = StateMachine.UnitModel;
			IEnumerable<((int, UnitModel), int)> source = unitModel.GetActiveSkillIds().Where(unitModel.CheckSkill).Where(CheckCasterConditions)
				.Select(delegate(int skillId)
				{
					UnitModel target;
					bool flag = CheckTargetCondition(skillId, out target);
					return (skillId, flag ? target : null);
				})
				.Where(((int, UnitModel) candidate) => candidate.Item2 != null)
				.Select(((int, UnitModel) candidate) => (candidate, TSkill.Table[candidate.Item1].AiSkillCondition.Weight));
			if (!source.Any())
			{
				return false;
			}
			(int, UnitModel) tuple = WeightedRandomSelector.SelectWeightedRandom(source, default((int, UnitModel)));
			if (tuple.Item1 == 0)
			{
				return false;
			}
			aiSkillDecision = new AiSkillDecision
			{
				SkillId = tuple.Item1,
				Target = tuple.Item2
			};
			return true;
		}

		protected void HandleUseSkillEvent(AiSkillDecision aiSkillDecision)
		{
			Vector3 vector = aiSkillDecision.Target.Position - StateMachine.UnitModel.Position;
			if (vector == Vector3.zero)
			{
				vector = GetForward() * 0.01f;
			}
			StateMachine.UnitModel.DeltaAimPos = vector;
			StateMachine.UnitModel.SetCalculatedDeltaAimPos(vector);
			UnitController unitController = StateMachine.UnitController;
			UseSkillEvent useSkillEvent = new UseSkillEvent();
			useSkillEvent.TriggerTarget = aiSkillDecision.Target;
			useSkillEvent.SkillId = aiSkillDecision.SkillId;
			unitController.HandleUnitEvent(useSkillEvent);
		}

		protected bool CheckHighStressTarget(out ITarget target)
		{
			target = null;
			if (MonsterData.TargetHighStressPlayer && FindEnemyInSight() is UnitModel unitModel && unitModel != MonsterModel.Target && unitModel.Stress > TConst.TargetHighStressPlayerTriggerStress.FloatValue)
			{
				target = unitModel;
				return true;
			}
			return false;
		}

		protected bool CheckTargetOutsideBlindRange()
		{
			float num = Mathf.Min(TConst.BlindSightRange.FloatValue, MonsterData.SightBackRange);
			if (!StateMachine.UnitModel.HasBuffEffect(EBuffEffectType.Blind))
			{
				return false;
			}
			return Distance(MonsterModel.Target) > num;
		}

		protected bool CheckTargetOutsideFollowRange()
		{
			float num = Mathf.Max(MonsterData.SightRange, TConst.TargetFollowRange.FloatValue);
			return Distance(MonsterModel.Target) > num;
		}

		protected bool CheckTargetInKiteRange()
		{
			if (MonsterData.KiteStartRange <= 0f)
			{
				return false;
			}
			if (MonsterData.KiteEndRange <= 0f)
			{
				return false;
			}
			float kiteStartRange = MonsterData.KiteStartRange;
			if (MonsterModel.Target == null)
			{
				return false;
			}
			return Distance(MonsterModel.Target) <= kiteStartRange;
		}

		protected bool CheckMoveToTargetStartRange()
		{
			return !IsTargetInRange(MonsterData.StartMoveToTargetRange);
		}

		private bool CheckCasterConditions(int skillId)
		{
			AiSkillCondition aiSkillCondition = TSkill.Table[skillId].AiSkillCondition;
			if (aiSkillCondition == null)
			{
				Logger.Error(string.Format("SkillId {0} has no AiSkillCondition", skillId));
				return false;
			}
			if (aiSkillCondition.Weight < 1)
			{
				return false;
			}
			if (aiSkillCondition.CasterConditions != null)
			{
				return aiSkillCondition.CasterConditions.All(CheckCasterCondition);
			}
			Logger.Error(string.Format("SkillId {0} has no CasterConditions", skillId));
			return false;
		}

		private bool CheckTargetCondition(int skillId, out UnitModel target)
		{
			TSkill skill = TSkill.Table[skillId];
			AiSkillCondition aiSkillCondition = skill.AiSkillCondition;
			if (aiSkillCondition == null)
			{
				Logger.Error(string.Format("SkillId {0} has no AiSkillCondition", skillId));
			}
			else
			{
				if (aiSkillCondition.Weight < 1)
				{
					target = null;
					return false;
				}
				if (aiSkillCondition.TargetConditions != null)
				{
					List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel u) => StateMachine.UnitModel.IsSkillTargetable(skill.Id, u)).Where((UnitModel u) => IsAliveState(u)).ToList();
					for (int i = 0; i < skill.AiSkillCondition.TargetConditions.Length; i++)
					{
						FilterTargetCondition(skill.AiSkillCondition.TargetConditions[i], list);
					}
					if (!list.Any())
					{
						target = null;
						return false;
					}
					if (MonsterData.TargetHighStressPlayer)
					{
						UnitModel unitModel = list.Where((UnitModel u) => u.Stress > TConst.TargetHighStressPlayerTriggerStress.FloatValue).OrderBy((UnitModel _) => Random.value).FirstOrDefault();
						if (unitModel != null)
						{
							target = unitModel;
							return true;
						}
					}
					target = list[Random.Range(0, list.Count)];
					return true;
				}
				Logger.Error(string.Format("SkillId {0} has no TargetConditions", skillId));
			}
			target = null;
			return false;
		}

		private bool CheckCasterCondition(AiCasterCondition condition)
		{
			if (condition is AiCasterHpPercentCondition condition2)
			{
				return CheckCasterHpPercent(condition2);
			}
			if (condition is AiCasterHasBuffIdCondition condition3)
			{
				return CheckCasterHasBuff(condition3);
			}
			if (condition is AiCasterHasBuffEffectTypeCondition condition4)
			{
				return CheckCasterHasBuffEffectType(condition4);
			}
			if (condition is AiCasterHasCrowdControlCondition condition5)
			{
				return CheckCasterHasCrowdControl(condition5);
			}
			if (condition is AiCasterNearbyPlayerCountCondition condition6)
			{
				return CheckCasterNearbyPlayerCount(condition6);
			}
			if (condition is AiCasterNearbyMonsterCountCondition condition7)
			{
				return CheckCasterNearByMonsterCount(condition7);
			}
			if (condition is AiCasterNearbyAllMonsterCountCondition condition8)
			{
				return CheckCasterNearbyAllMonsterCount(condition8);
			}
			if (condition is AiCasterNearbyTargetableHitBoxCountCondition condition9)
			{
				return CheckCasterNearbyHitBoxCount(condition9);
			}
			if (condition is AiCasterNearbyAllTargetableHitBoxCountCondition condition10)
			{
				return CheckCasterNearbyAllHitBoxCount(condition10);
			}
			if (condition is AiCasterDifficultyCondition aiCasterDifficultyCondition)
			{
				return CheckCasterDifficulty(aiCasterDifficultyCondition);
			}
			Logger.Error(string.Format("check caster condition {0} is not implemented", condition));
			return false;
		}

		private bool CheckCasterHpPercent(AiCasterHpPercentCondition condition)
		{
			return ValueComparer.Compare((float)StateMachine.UnitModel.Hp / StateMachine.UnitModel.HpMax * 100f, condition.CompareType, condition.HpPercentValue);
		}

		private bool CheckCasterHasBuff(AiCasterHasBuffIdCondition condition)
		{
			return StateMachine.UnitModel.HasBuff(condition.BuffId);
		}

		private bool CheckCasterHasBuffEffectType(AiCasterHasBuffEffectTypeCondition condition)
		{
			return StateMachine.UnitModel.HasBuffEffect(condition.BuffEffectType);
		}

		private bool CheckCasterHasCrowdControl(AiCasterHasCrowdControlCondition condition)
		{
			return StateMachine.UnitModel.IsCrowdControlled(condition.CrowdControlType);
		}

		private bool CheckCasterNearbyPlayerCount(AiCasterNearbyPlayerCountCondition condition)
		{
			List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel _) => _ is CharacterModel).Where((UnitModel _) => IsAliveState(_)).Where((UnitModel _) => ValueComparer.Compare(Distance(_), condition.DistanceCompareType, condition.DistanceValue))
				.ToList();
			return ValueComparer.Compare(list.Count, condition.CountCompareType, condition.CountValue);
		}

		private bool CheckCasterNearByMonsterCount(AiCasterNearbyMonsterCountCondition condition)
		{
			List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel _) => _ is MonsterModel monsterModel && monsterModel.MonsterId == condition.MonsterId).Where((UnitModel _) => _ != StateMachine.UnitModel).Where((UnitModel _) => IsAliveState(_))
				.Where((UnitModel _) => ValueComparer.Compare(Distance(_), condition.DistanceCompareType, condition.DistanceValue))
				.ToList();
			return ValueComparer.Compare(list.Count, condition.CountCompareType, condition.CountValue);
		}

		private bool CheckCasterNearbyAllMonsterCount(AiCasterNearbyAllMonsterCountCondition condition)
		{
			List<UnitModel> list = ObjectManager.UnitModels.Where((UnitModel _) => _ is MonsterModel).Where((UnitModel _) => _ != StateMachine.UnitModel).Where((UnitModel _) => IsAliveState(_))
				.Where((UnitModel _) => ValueComparer.Compare(Distance(_), condition.DistanceCompareType, condition.DistanceValue))
				.ToList();
			return ValueComparer.Compare(list.Count, condition.CountCompareType, condition.CountValue);
		}

		private bool CheckCasterNearbyHitBoxCount(AiCasterNearbyTargetableHitBoxCountCondition condition)
		{
			int value = ObjectManager.GetHitBoxes().Where((HitBox _) => _.HitBoxId == condition.TargetableHitBoxId).Where((HitBox _) => ValueComparer.Compare(Distance(_), condition.DistanceCompareType, condition.DistanceValue))
				.Count();
			return ValueComparer.Compare(value, condition.CountCompareType, condition.CountValue);
		}

		private bool CheckCasterNearbyAllHitBoxCount(AiCasterNearbyAllTargetableHitBoxCountCondition condition)
		{
			int value = ObjectManager.GetHitBoxes().Where((HitBox _) => ValueComparer.Compare(Distance(_), condition.DistanceCompareType, condition.DistanceValue)).Count();
			return ValueComparer.Compare(value, condition.CountCompareType, condition.CountValue);
		}

		private bool CheckCasterDifficulty(AiCasterDifficultyCondition aiCasterDifficultyCondition)
		{
			return (int)aiCasterDifficultyCondition.Difficulty <= (int)GameContext.Difficulty;
		}

		private void FilterTargetCondition(AiTargetCondition condition, List<UnitModel> targets)
		{
			if (condition is AiTargetDistanceCondition condition2)
			{
				FilterTargetDistance(condition2, targets);
			}
			else if (condition is AiTargetHpPercentCondition condition3)
			{
				FilterTargetHpPercent(condition3, targets);
			}
			else if (condition is AiTargetHasBuffIdCondition condition4)
			{
				FilterTargetHasBuffId(condition4, targets);
			}
			else if (condition is AiTargetHasBuffEffectTypeCondition condition5)
			{
				FilterTargetHasBuffEffectType(condition5, targets);
			}
			else if (condition is AiTargetHasCrowdControlCondition condition6)
			{
				FilterTargetHasCrowdControl(condition6, targets);
			}
			else if (condition is AiNoHighObstacleToTarget condition7)
			{
				FilterNoHighObstacleToTarget(condition7, targets);
			}
			else
			{
				Logger.Error(string.Format("Unknown AiTargetCondition {0}", condition));
			}
		}

		private void FilterTargetDistance(AiTargetDistanceCondition condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => !ValueComparer.Compare(Distance(target), condition.compareType, condition.compareValue));
		}

		private void FilterTargetHpPercent(AiTargetHpPercentCondition condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => !ValueComparer.Compare((float)target.Hp / target.HpMax * 100f, condition.CompareType, condition.HpPercentValue));
		}

		private void FilterTargetHasBuffId(AiTargetHasBuffIdCondition condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => !target.HasBuff(condition.BuffId));
		}

		private void FilterTargetHasBuffEffectType(AiTargetHasBuffEffectTypeCondition condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => !target.HasBuffEffect(condition.BuffEffectType));
		}

		private void FilterTargetHasCrowdControl(AiTargetHasCrowdControlCondition condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => !target.IsCrowdControlled(condition.CrowdControlType));
		}

		private void FilterNoHighObstacleToTarget(AiNoHighObstacleToTarget condition, List<UnitModel> targets)
		{
			targets.RemoveAll((UnitModel target) => PhysicsUtility.IsRayCastHitBetween(StateMachine.UnitModel.transform.position, target.transform.position, Const.LayerMask.HIGH_OBSTACLE));
		}
	}
}
