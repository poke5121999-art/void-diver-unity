using De.InGame.Fsm;
using GameCommon.Enum;
using GameCommon.Table;
using Sirenix.Utilities;
using UnityEngine;

public class MonsterCuriousState : MonsterStateBase
{
	private float _stateEndTime;

	private bool _isComplete;

	public override EActionState ActionState => EActionState.Curious;

	public override bool IsAimEnable => false;

	public MonsterCuriousState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		float timeNow = TimeNow;
		_isComplete = false;
		_stateEndTime = timeNow + TConst.MonsterCuriousStateTime.FloatValue;
		int[] onCuriousBuffIds = MonsterData.OnCuriousBuffIds;
		if (onCuriousBuffIds != null)
		{
			onCuriousBuffIds.ForEach(delegate(int buffId)
			{
				MonsterController.HandleUnitEvent(new BuffEvent
				{
					BuffId = buffId,
					BuffStack = 1
				});
			});
		}
	}

	public override void OnUpdate()
	{
		if (_stateEndTime <= TimeNow)
		{
			_isComplete = true;
		}
		UnitView.SyncPosition();
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		if (unitEvent != null)
		{
			if (unitEvent is HpDamageEvent hpDamageEvent)
			{
				if (hpDamageEvent.SourceModel != null && hpDamageEvent.SourceModel.IsHitable && IsEnemy(hpDamageEvent.SourceModel))
				{
					SetTarget(hpDamageEvent.SourceModel);
					UnitModel.SetCalculatedDeltaAimPos(TargetPosition - OwnerPosition);
					return MonsterModel.IsInCombat ? EActionState.MoveToTarget : EActionState.Aggro;
				}
			}
			else if (unitEvent is CrowdControlEvent crowdControlEvent)
			{
				if (crowdControlEvent.CcInfo.ccType != ECrowdControlType.None)
				{
					return EActionState.CrowdControlled;
				}
			}
			else
			{
				if (unitEvent is DieEvent)
				{
					return EActionState.Dying;
				}
				if (unitEvent is CaptureEvent)
				{
					return EActionState.Captured;
				}
				if (unitEvent is UseSkillEvent)
				{
					return EActionState.Skill;
				}
				if (unitEvent is ReceiveAggroSpreadEvent receiveAggroSpreadEvent)
				{
					if (MonsterModel.AggroChainDepth == 0)
					{
						SetTarget(receiveAggroSpreadEvent.Target);
						MonsterModel.AggroChainDepth = receiveAggroSpreadEvent.ChainDepth + 1;
						return EActionState.ReceiveAggroSpread;
					}
				}
				else if (unitEvent is TryMonsterCuriousStateEvent tryMonsterCuriousStateEvent)
				{
					MonsterModel.WanderSpecificPosition = tryMonsterCuriousStateEvent.TargetPosition;
				}
			}
		}
		if (!_isComplete)
		{
			return EActionState.None;
		}
		MonsterModel.IsWanderSpecificPosition = true;
		return EActionState.Wander;
	}

	public override Vector3 GetForward()
	{
		return UnitModel.CalculatedDeltaAimPos;
	}
}
