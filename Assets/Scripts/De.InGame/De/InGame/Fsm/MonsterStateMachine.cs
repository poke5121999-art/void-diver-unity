using GameCommon.Enum;
using GameCommon.Table;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace De.InGame.Fsm
{
	public class MonsterStateMachine : UnitStateMachine
	{
		[Inject]
		protected NetworkManager NetworkManager;

		public MonsterController MonsterController { get; }

		public MonsterStateMachine(UnitController controller, bool isPatrolMonster)
			: base(controller)
		{
			MonsterController = controller as MonsterController;
			AddState(new MonsterSpawnState(this));
			AddState(new MonsterReservedDespawn(this));
			AddState(new MonsterIdleState(this));
			AddState(isPatrolMonster ? new MonsterPatrolWanderState(this) : new MonsterPointWanderState(this));
			AddState(new UnitSkillState(this));
			AddState(new MonsterMoveState(this));
			AddState(new MonsterAvoidState(this));
			AddState(new MonsterPauseState(this));
			AddState(new MonsterMoveToTargetState(this));
			AddState(new MonsterKiteFromTargetState(this));
			AddState(new MonsterFleeState(this));
			AddState(new MonsterLostTargetState(this));
			AddState(new MonsterAlertState(this));
			AddState(new MonsterAggroState(this));
			AddState(new MonsterReceiveAggroSpreadState(this));
			AddState(new UnitDyingState(this));
			AddState(new MonsterDeadState(this));
			AddState(new UnitCrowdControlledState(this));
			AddState(new MonsterCuriousState(this));
			AddState(new MonsterPhaseUpState(this));
			AddState(new MonsterGroggyState(this));
			AddState(new MonsterCapturedState(this));
		}

		public override void TriggerEvent(IUnitEvent stateEvent)
		{
			if (stateEvent is GroggyEvent)
			{
				if (UnitModel.State != EActionState.Dying && UnitModel.State != EActionState.Dead && UnitModel.State != EActionState.Groggy)
				{
					ChangeState(EActionState.Groggy, stateEvent);
					return;
				}
			}
			else if (stateEvent is CaptureEvent && UnitModel.State != EActionState.Dying && UnitModel.State != EActionState.Dead && UnitModel.State != EActionState.Captured)
			{
				ChangeState(EActionState.Captured, stateEvent);
				return;
			}
			base.TriggerEvent(stateEvent);
		}

		protected override void ChangeState(EActionState actionState, IUnitEvent unitEvent)
		{
			base.ChangeState(actionState, unitEvent);
			if (UnitModel.CalculatedDeltaAimPos == Vector3.zero)
			{
				TMonster tMonster = TMonster.Table[UnitModel.TableId];
				Logger.Warning(string.Format("{0}/{1} CalculatedDeltaAimPos Zero, Prev {2}, now {3}\n", tMonster.Name, tMonster.Id, PreviousState, UnitModel.State) + string.Format("Position {0}", UnitModel.transform.position));
			}
		}
	}
}
