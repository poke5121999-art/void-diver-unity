using UnityEngine;

namespace De.InGame.Fsm
{
	public class PetBackOffState : PetStateBase
	{
		private const float _sampleRadiusMultiplier = 1.2f;

		private Vector3 _backOffPosition;

		private bool _isComplete;

		private bool _sampleFailed;

		public override EPetState PetState => EPetState.Idle;

		public PetBackOffState(PetStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
		}

		public override void OnUpdate()
		{
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			return EPetState.Idle;
		}

		private bool TryFinishBackOff()
		{
			return false;
		}

		private bool TrySampleBackOffPosition()
		{
			return false;
		}
	}
}
