using System;
using System.Linq;
using De.InGame.Fsm;
using GameCommon.Table;
using SomeNet.Collections;
using GameCommon.Enum;
using UnityEngine;

public class MonsterAggroState : MonsterStateBase
{
	private float _stateStartTime;

	private float _lastAggroSfxTime;

	private bool _shouldPlayAggroSfx;

	public override EActionState ActionState => EActionState.Aggro;

	public override bool IsAimEnable => true;

	private float StateTime => TConst.MonsterAggroStateTime.FloatValue;

	private float StateElapsedTime => TimeNow - _stateStartTime;

	public MonsterAggroState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_stateStartTime = TimeNow;
		if (!(Target is IHitable hitable) || !hitable.IsHitable || MonsterModel.IsShadow || MonsterModel.AggroChainDepth >= TConst.MonsterAggroChainMax.IntValue)
		{
			return;
		}
		ObjectManager.UnitModels.Where((UnitModel u) => u is MonsterModel monsterModel && TMonster.Table[monsterModel.MonsterId].FactionId == TMonster.Table[MonsterController.MonsterId].FactionId).Where((UnitModel u) => Distance(u) <= TConst.TargetSpreadRange.FloatValue).Where((UnitModel u) => u != UnitModel).ForEach(delegate(UnitModel u)
		{
			MonsterController component = u.GetComponent<MonsterController>();
			ReceiveAggroSpreadEvent receiveAggroSpreadEvent = new ReceiveAggroSpreadEvent();
			receiveAggroSpreadEvent.Target = Target;
			receiveAggroSpreadEvent.ChainDepth = MonsterModel.AggroChainDepth;
			component.HandleUnitEvent(receiveAggroSpreadEvent);
		});
	}

	public override void OnUpdate()
	{
		UnitView.SyncPosition(-1f);
	}

	public override void OnExit()
	{
		if (!IsActivated)
		{
			return;
		}
		if (_shouldPlayAggroSfx)
		{
			_shouldPlayAggroSfx = false;
			_lastAggroSfxTime = TimeNow;
			GameHost.SendToAll(new NtfPlaySfx
			{
				Sfx = MonsterData.AggroSfx,
				Position = OwnerPosition.ToProtoVector3()
			});
		}
		MonsterModel.AggroChainDepth = 0;
		base.OnExit();
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		if (unitEvent != null)
		{
			if (unitEvent is CrowdControlEvent crowdControlEvent)
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
			}
		}
		if (Target == null)
		{
			SetTarget(FindEnemyInSight());
		}
		else if (Target is IHitable hitable && !hitable.IsHitable)
		{
			SetTarget(FindEnemyInSight());
		}
		if (Target != null)
		{
			UnitModel.SetCalculatedDeltaAimPos(TargetPosition - OwnerPosition);
		}
		if (StateElapsedTime > StateTime)
		{
			if (Target == null)
			{
				return EActionState.Idle;
			}
			if (MonsterData.AggroSfx.HasValue())
			{
				_shouldPlayAggroSfx = TimeNow - _lastAggroSfxTime >= TConst.MonsterAggroSfxCoolTime.FloatValue;
				return EActionState.MoveToTarget;
			}
			_shouldPlayAggroSfx = false;
			return EActionState.MoveToTarget;
		}
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return MonsterModel.CalculatedDeltaAimPos;
	}
}
