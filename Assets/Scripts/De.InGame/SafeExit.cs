using GameCommon.Enum;
using UniRx;
using Unity.Netcode;

public class SafeExit : ExitObject
{
	private readonly NetworkVariable<EExitState> _syncExitState;

	private readonly NetworkVariable<float> _syncStateRemainTime;

	private CompositeDisposable _disposable;

	private bool _isGraceTimerRunning;

	public override float HoldingTime => 0f;

	public override EExitState ExitState => EExitState.Deactivated;

	public bool IsActivated => false;

	public ReactiveProperty<float> RxStateRemainTime { get; }

	public override string InteractionSfx => null;

	public bool DisplayOnMinimapAtStart => false;

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void OnStageEnded()
	{
	}

	private void OnSyncExitStateChanged(EExitState oldValue, EExitState newValue)
	{
	}

	private void OnSyncStateRemainTimeChanged(float oldValue, float newValue)
	{
	}

	private void ApplyState(EExitState exitState)
	{
	}

	private void SetState(EExitState state)
	{
	}

	private void Update()
	{
	}

	public override bool CanInteract(UnitModel sourceModel)
	{
		return false;
	}

	public override void OnInteraction(UnitController interactingUnit)
	{
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



}
