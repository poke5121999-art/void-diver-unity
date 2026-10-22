using System.Collections.Generic;
using De.InGame.Unit;
using GameCommon.Enum;
using GameCommon.Table;
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

		protected TMonster MonsterData => null;

		protected MonsterModel MonsterModel => null;

		protected MonsterView MonsterView => null;

		protected Vector3 SpawnPosition => default;

		protected Vector3 OwnerPosition => default;

		protected MonsterStateMachine MonsterStateMachine => null;

		protected MonsterController MonsterController => null;

		protected ITarget Target => null;

		protected Vector3 TargetPosition => default;

		protected bool HasKiteRange => false;

		protected NavMeshAgent NavMeshAgent => null;

		protected MonsterStateBase(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		protected void SetTarget(ITarget target)
		{
		}

		protected ITarget FindEnemyInSight()
		{
			return null;
		}

		public bool IsEnemy(IHitable target)
		{
			return false;
		}

		protected bool IsTargetInSight(ITarget target, float sightRangeMultiplier = 1f, float sightBackRangeMultiplier = 1f)
		{
			return false;
		}

		protected bool IsTargetInDirectAggroSight(ITarget target)
		{
			return false;
		}

		protected float Distance(ITarget target)
		{
			return 0f;
		}

		private float HighStressFirstDistance(UnitModel target)
		{
			return 0f;
		}

		protected bool IsTargetInRange(float range)
		{
			return false;
		}

		protected bool IsTargetOutRange(float range)
		{
			return false;
		}

		protected bool CheckSkill(out AiSkillDecision aiSkillDecision)
		{
			aiSkillDecision = null;
			return false;
		}

		protected void HandleUseSkillEvent(AiSkillDecision aiSkillDecision)
		{
		}

		protected bool CheckHighStressTarget(out ITarget target)
		{
			target = null;
			return false;
		}

		protected bool CheckTargetOutsideBlindRange()
		{
			return false;
		}

		protected bool CheckTargetOutsideFollowRange()
		{
			return false;
		}

		protected bool CheckTargetInKiteRange()
		{
			return false;
		}

		protected bool CheckMoveToTargetStartRange()
		{
			return false;
		}

		private bool CheckCasterConditions(int skillId)
		{
			return false;
		}

		private bool CheckTargetCondition(int skillId, out UnitModel target)
		{
			target = null;
			return false;
		}

		private bool CheckCasterCondition(AiCasterCondition condition)
		{
			return false;
		}

		private bool CheckCasterHpPercent(AiCasterHpPercentCondition condition)
		{
			return false;
		}

		private bool CheckCasterHasBuff(AiCasterHasBuffIdCondition condition)
		{
			return false;
		}

		private bool CheckCasterHasBuffEffectType(AiCasterHasBuffEffectTypeCondition condition)
		{
			return false;
		}

		private bool CheckCasterHasCrowdControl(AiCasterHasCrowdControlCondition condition)
		{
			return false;
		}

		private bool CheckCasterNearbyPlayerCount(AiCasterNearbyPlayerCountCondition condition)
		{
			return false;
		}

		private bool CheckCasterNearByMonsterCount(AiCasterNearbyMonsterCountCondition condition)
		{
			return false;
		}

		private bool CheckCasterNearbyAllMonsterCount(AiCasterNearbyAllMonsterCountCondition condition)
		{
			return false;
		}

		private bool CheckCasterNearbyHitBoxCount(AiCasterNearbyTargetableHitBoxCountCondition condition)
		{
			return false;
		}

		private bool CheckCasterNearbyAllHitBoxCount(AiCasterNearbyAllTargetableHitBoxCountCondition condition)
		{
			return false;
		}

		private bool CheckCasterDifficulty(AiCasterDifficultyCondition aiCasterDifficultyCondition)
		{
			return false;
		}

		private void FilterTargetCondition(AiTargetCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterTargetDistance(AiTargetDistanceCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterTargetHpPercent(AiTargetHpPercentCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterTargetHasBuffId(AiTargetHasBuffIdCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterTargetHasBuffEffectType(AiTargetHasBuffEffectTypeCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterTargetHasCrowdControl(AiTargetHasCrowdControlCondition condition, List<UnitModel> targets)
		{
		}

		private void FilterNoHighObstacleToTarget(AiNoHighObstacleToTarget condition, List<UnitModel> targets)
		{
		}
	}
}
