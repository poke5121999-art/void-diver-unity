using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class WaveExit : ExitObject
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CToggleActivatingToActivatedAsync_003Ed__26 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public WaveExit _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private void MoveNext()
		{
		}

		void IAsyncStateMachine.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			this.MoveNext();
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}

		void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
			this.SetStateMachine(stateMachine);
		}
	}

	[SerializeField]
	private NavMeshObstacle _navMeshObstacle;

	[SerializeField]
	private Collider _collider;

	private readonly NetworkVariable<EExitState> _syncExitState;

	private readonly NetworkVariable<float> _syncStateRemainTime;

	public override float HoldingTime => 0f;

	public bool DisplayOnMinimapAtStart => false;

	private int WaveId => 0;

	private int SpecialFieldId => 0;

	public ReactiveProperty<float> RxStateRemainTime { get; }

	public override EExitState ExitState => EExitState.Deactivated;

	public float StateRemainTime => 0f;

	public override string InteractionSfx => null;

	private void OnSyncExitStateChanged(EExitState oldValue, EExitState newValue)
	{
	}

	private void OnStateRemainTimeChanged(float oldValue, float newValue)
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void ApplyExitStateVisual(EExitState state)
	{
	}

	[AsyncStateMachine(typeof(_003CToggleActivatingToActivatedAsync_003Ed__26))]
	private UniTaskVoid ToggleActivatingToActivatedAsync()
	{
		return default;
	}

	private void Update()
	{
	}

	private void OnStageEnded()
	{
	}

	private float GetHoldingTime()
	{
		return 0f;
	}

	public override void OnInteraction(UnitController interactingUnit)
	{
	}

	public override bool CanInteract(UnitModel sourceModel)
	{
		return false;
	}

	public override void OnStartHolding(ulong sourceClientId)
	{
	}

	public override void OnStartInteraction(ulong sourceClientId)
	{
	}

	public override void OnEndInteraction(ulong sourceClientId)
	{
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
