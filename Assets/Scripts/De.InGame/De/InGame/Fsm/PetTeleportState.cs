using UnityEngine;

namespace De.InGame.Fsm
{
	public class PetTeleportState : PetStateBase
	{
		private const float _sampleRadius = 1.8f;

		private static readonly Vector3 _followOffset;

		public override EPetState PetState => EPetState.Idle;

		public PetTeleportState(PetStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			return EPetState.Idle;
		}
	}
}
