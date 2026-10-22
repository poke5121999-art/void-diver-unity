using UnityEngine;
using UnityEngine.AI;

namespace De.InGame.Fsm
{
	public class PetBackOffState : PetStateBase
	{
		private const float _sampleRadiusMultiplier = 1.2f;

		private Vector3 _backOffPosition;

		private bool _isComplete;

		private bool _sampleFailed;

		public override EPetState PetState => EPetState.BackOff;

		public PetBackOffState(PetStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IPetEvent stateEvent)
		{
			PetModel.Speed = WalkSpeed;
			_backOffPosition = Vector3.zero;
			_isComplete = false;
			_sampleFailed = false;
		}

		public override void OnUpdate()
		{
			if (!TryFinishBackOff())
			{
				if (_backOffPosition == Vector3.zero && !TrySampleBackOffPosition())
				{
					_sampleFailed = true;
				}
				else
				{
					PetController.MoveToTarget(_backOffPosition, WalkSpeed);
				}
			}
		}

		public override EPetState GetNextStateEvent(IPetEvent stateEvent)
		{
			if (stateEvent is PetTeleportEvent)
			{
				return EPetState.Teleport;
			}
			if (stateEvent is PetCombatEnterEvent)
			{
				return EPetState.Alert;
			}
			if (_isComplete)
			{
				return EPetState.Idle;
			}
			if (!_sampleFailed)
			{
				return EPetState.None;
			}
			return EPetState.Idle;
		}

		private bool TryFinishBackOff()
		{
			float distanceToOwner = DistanceToOwner;
			bool flag = _backOffPosition != Vector3.zero && Vector3.Distance(PetController.transform.position, _backOffPosition) < 0.1f;
			if (distanceToOwner < DesiredDistance && !flag)
			{
				return false;
			}
			Vector3 ownerPosition = OwnerPosition;
			Vector3 position = PetController.transform.position;
			Vector3 forward = new Vector3(ownerPosition.x - position.x, 0f, ownerPosition.z - position.z).normalized;
			PetController.PetModel.Forward = forward;
			_isComplete = true;
			return true;
		}

		private bool TrySampleBackOffPosition()
		{
			Vector3 position = PetController.transform.position;
			Vector3 ownerPosition = OwnerPosition;
			Vector3 normalized = new Vector3(position.x - ownerPosition.x, 0f, position.z - ownerPosition.z).normalized;
			Vector3 sourcePosition = OwnerPosition + normalized * (DesiredDistance * 1.2f);
			if (NavMesh.SamplePosition(sourcePosition, out var hit, DesiredDistance, -1))
			{
				_backOffPosition = hit.position;
				return true;
			}
			return false;
		}
	}
}
