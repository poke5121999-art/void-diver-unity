using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class MonsterSpawnState : MonsterStateBase
	{
		private float _stateStartTime;

		private bool _isMoveToWaveStart;

		private bool _isCompleted;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		public MonsterSpawnState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return default;
		}
	}
}
