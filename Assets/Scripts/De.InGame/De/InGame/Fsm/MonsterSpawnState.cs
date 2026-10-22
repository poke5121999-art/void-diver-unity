using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterSpawnState : MonsterStateBase
	{
		private float _stateStartTime;

		private bool _isMoveToWaveStart;

		private bool _isCompleted;

		public override EActionState ActionState => EActionState.Spawn;

		public override bool IsAimEnable => false;

		public MonsterSpawnState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_isCompleted = false;
			_stateStartTime = GameManager.TimeNow;
			if (MonsterController.SpawnType == ESpawnType.Wave)
			{
				_isMoveToWaveStart = true;
				MonsterModel.IsWanderSpecificPosition = true;
				MonsterModel.WanderSpecificPosition = MonsterController.WaveStartPosition;
			}
		}

		public override void OnUpdate()
		{
			if (_stateStartTime + TConst.MonsterSpawnTime.FloatValue < GameManager.TimeNow)
			{
				_isCompleted = true;
			}
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (!_isCompleted)
			{
				return EActionState.None;
			}
			if (!_isMoveToWaveStart)
			{
				return EActionState.Idle;
			}
			return EActionState.Wander;
		}

		public override Vector3 GetForward()
		{
			return StateMachine.UnitModel.CalculatedDeltaAimPos;
		}
	}
}
