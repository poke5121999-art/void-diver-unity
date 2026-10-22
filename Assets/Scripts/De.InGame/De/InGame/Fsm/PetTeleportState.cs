using UnityEngine;
using UnityEngine.AI;

namespace De.InGame.Fsm
{
	public class PetTeleportState : PetStateBase
	{
		private const float _sampleRadius = 1.8f;

		private static readonly Vector3 _followOffset = new Vector3(-1f, 0f, -1f).normalized;

		public override EPetState PetState => EPetState.Teleport;

		public PetTeleportState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = -1f;
			Vector3 ownerPosition = OwnerPosition;
			Vector3 sourcePosition = ownerPosition + _followOffset * 1.8f;
			Vector3 position = ownerPosition;
			if (NavMesh.SamplePosition(sourcePosition, out var hit, 1.8f, -1))
			{
				position = hit.position;
			}
			PetView.Teleport(position);
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			return EPetState.Idle;
		}
	}
}
