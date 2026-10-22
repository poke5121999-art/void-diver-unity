using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitCrowdControlledState : UnitStateBase
	{
		private float _moveRemainTime;

		private float _moveSpeed;

		private Vector3 _moveDir;

		public override EActionState ActionState => EActionState.CrowdControlled;

		public override bool IsAimEnable => false;

		public UnitCrowdControlledState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent stateEvent)
		{
			base.OnEnter(stateEvent);
			StateMachine.UnitView.SetNoObstacleAvoidanceType(noObstacleAvoidance: true);
			CrowdControlEvent crowdControlEvent = stateEvent as CrowdControlEvent;
			if (crowdControlEvent != null)
			{
				UpdateCcInfo(crowdControlEvent);
			}
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
			}
			StateMachine.UnitView.SetNoObstacleAvoidanceType(noObstacleAvoidance: false);
		}

		private void UpdateCcInfo(CrowdControlEvent crowdControlEvent)
		{
			StateMachine.UnitModel.AddCrowdControl(crowdControlEvent.CcInfo.ccType, crowdControlEvent.CcInfo.ccDuration);
			CcInfo ccInfo = crowdControlEvent.CcInfo;
			if (ccInfo.UseForcedMoveDirection && crowdControlEvent.ForcedMoveDirection != Vector3.zero)
			{
				float num = ((ccInfo.ccMoveDuration > 0f) ? ccInfo.ccMoveDuration : ccInfo.ccDuration);
				_moveRemainTime = Mathf.Min(ccInfo.ccDuration, num);
				float num2 = crowdControlEvent.ForcedMoveDistance ?? ccInfo.ccMoveDistance;
				_moveSpeed = num2 / ((_moveRemainTime > 0f) ? _moveRemainTime : 0.0001f);
				_moveDir = crowdControlEvent.ForcedMoveDirection.normalized;
			}
			else if (ccInfo.ccType == ECrowdControlType.Knockback)
			{
				float num3 = ((ccInfo.ccMoveDuration > 0f) ? ccInfo.ccMoveDuration : ccInfo.ccDuration);
				_moveRemainTime = Mathf.Min(ccInfo.ccDuration, num3);
				_moveSpeed = ccInfo.ccMoveDistance / ((_moveRemainTime > 0f) ? _moveRemainTime : 0.0001f);
				Vector3 vector = StateMachine.UnitView.transform.position - crowdControlEvent.KnockbackHitPosition;
				_moveDir = new Vector3(vector.x, 0f, vector.z).normalized;
			}
			else
			{
				if (ccInfo.ccType != ECrowdControlType.Pull || crowdControlEvent.SourceModel == null)
				{
					return;
				}
				Vector3 vector2 = crowdControlEvent.SourceModel.transform.position - StateMachine.UnitView.transform.position;
				Vector3 vector3 = new Vector3(vector2.x, 0f, vector2.z);
				_moveDir = vector3.normalized;
				float num4 = ((ccInfo.ccMoveDuration > 0f) ? ccInfo.ccMoveDuration : ccInfo.ccDuration);
				_moveRemainTime = Mathf.Min(ccInfo.ccDuration, num4);
				_moveSpeed = (vector3.magnitude + ccInfo.ccMoveOffset) / ((_moveRemainTime > 0f) ? _moveRemainTime : 0.0001f);
			}
		}

		public override void OnUpdate()
		{
			if (_moveRemainTime > 0f)
			{
				float num = Time.fixedDeltaTime;
				UnitView unitView = StateMachine.UnitView;
				if (_moveRemainTime <= num)
				{
					num = _moveRemainTime;
				}
				unitView.MoveWithNavMeshCheck(_moveDir, _moveSpeed, num);
				_moveRemainTime -= Time.fixedDeltaTime;
			}
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent != null)
			{
				if (unitEvent is CutsceneStartEvent)
				{
					return EActionState.Cutscene;
				}
				if (unitEvent is CrowdControlEvent crowdControlEvent)
				{
					if (crowdControlEvent.CcInfo.ccType != ECrowdControlType.None)
					{
						UpdateCcInfo(crowdControlEvent);
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
					if (unitEvent is EscapeEvent)
					{
						return EActionState.Escaped;
					}
				}
			}
			if (StateMachine.UnitModel.CrowdControls.Count == 0)
			{
				return (StateMachine.UnitModel.MoveDir != Vector3.zero) ? EActionState.Move : EActionState.Idle;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.CalculatedDeltaAimPos;
		}
	}
}
