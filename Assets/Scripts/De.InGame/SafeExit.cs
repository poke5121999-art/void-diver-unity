using System;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class SafeExit : ExitObject
{
	private readonly NetworkVariable<EExitState> _syncExitState = new NetworkVariable<EExitState>();

	private readonly NetworkVariable<float> _syncStateRemainTime = new NetworkVariable<float>();

	private CompositeDisposable _disposable = new CompositeDisposable();

	private bool _isGraceTimerRunning;

	public override float HoldingTime => TConst.SafeExitInteractionTime.FloatValue;

	public override EExitState ExitState => _syncExitState.Value;

	public bool IsActivated => ExitState == EExitState.Activated;

	public ReactiveProperty<float> RxStateRemainTime { get; } = new ReactiveProperty<float>();

	public override string InteractionSfx => string.Empty;

	public bool DisplayOnMinimapAtStart => TExit.Table[ExitId].DisplayOnMinimapAtStart;

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		_syncExitState.Value = EExitState.Activating;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		ObjectManager.OnInteractiveObjectSpawned.OnNext(this);
		GameManager.RxStageState.Where((EStageState s) => s == EStageState.Ended).Subscribe(delegate
		{
			OnStageEnded();
		}).AddTo(this);
		RxExitState.Value = EExitState.Activating;
		ApplyState(EExitState.Activating);
		RxStateRemainTime.Value = _syncStateRemainTime.Value;
		_syncExitState.OnValueChanged = (NetworkVariable<EExitState>.OnValueChangedDelegate)Delegate.Combine(_syncExitState.OnValueChanged, new NetworkVariable<EExitState>.OnValueChangedDelegate(OnSyncExitStateChanged));
		_syncStateRemainTime.OnValueChanged = (NetworkVariable<float>.OnValueChangedDelegate)Delegate.Combine(_syncStateRemainTime.OnValueChanged, new NetworkVariable<float>.OnValueChangedDelegate(OnSyncStateRemainTimeChanged));
	}

	public override void OnNetworkDespawn()
	{
		_syncExitState.OnValueChanged = (NetworkVariable<EExitState>.OnValueChangedDelegate)Delegate.Remove(_syncExitState.OnValueChanged, new NetworkVariable<EExitState>.OnValueChangedDelegate(OnSyncExitStateChanged));
		_syncStateRemainTime.OnValueChanged = (NetworkVariable<float>.OnValueChangedDelegate)Delegate.Remove(_syncStateRemainTime.OnValueChanged, new NetworkVariable<float>.OnValueChangedDelegate(OnSyncStateRemainTimeChanged));
		ObjectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	private void OnStageEnded()
	{
		if (base.IsHost && ExitState != EExitState.Finished)
		{
			_isGraceTimerRunning = false;
			_syncExitState.Value = EExitState.Finished;
		}
	}

	private void OnSyncExitStateChanged(EExitState oldValue, EExitState newValue)
	{
		RxExitState.Value = newValue;
		ApplyState(newValue);
	}

	private void OnSyncStateRemainTimeChanged(float oldValue, float newValue)
	{
		RxStateRemainTime.Value = newValue;
	}

	private void ApplyState(EExitState exitState)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		GoActivating.SetActive(exitState == EExitState.Activating);
		GoActivated.SetActive(exitState == EExitState.Activated);
		GoFinished.SetActive(exitState == EExitState.Finished);
		NavMeshObstacle component = GetComponent<NavMeshObstacle>();
		if (component != null)
		{
			component.enabled = exitState == EExitState.Activated;
		}
		switch (exitState)
		{
		case EExitState.Activating:
		{
			if (!base.IsHost)
			{
				break;
			}
			PlayableDirector director = GoActivating.GetComponent<PlayableDirector>();
			if (director != null)
			{
				Observable.Timer(TimeSpan.FromSeconds(director.duration)).Subscribe(delegate
				{
					_syncExitState.Value = EExitState.Activated;
				}).AddTo(_disposable);
			}
			break;
		}
		case EExitState.Finished:
		{
			SoundManager.PlaySfx3D("BoothEscape_End", base.transform);
			if (!base.IsHost)
			{
				break;
			}
			PlayableDirector director2 = GoFinished.GetComponent<PlayableDirector>();
			if (director2 != null)
			{
				Despawn((float)director2.duration);
			}
			break;
		}
		}
	}

	private void SetState(EExitState state)
	{
		_syncExitState.Value = state;
	}

	private void Update()
	{
		if (!base.IsHost || !_isGraceTimerRunning)
		{
			return;
		}
		_syncStateRemainTime.Value -= Time.deltaTime;
		if (!(_syncStateRemainTime.Value > 0f))
		{
			_isGraceTimerRunning = false;
			_syncExitState.Value = EExitState.Finished;
			SoundManager.PlaySfx3D("BoothEscape_End", base.transform);
		}
	}

	public override bool CanInteract(UnitModel sourceModel)
	{
		return ExitState == EExitState.Activated;
	}

	public override void OnInteraction(UnitController interactingUnit)
	{
		if (!base.IsHost)
		{
			return;
		}
		if (TryConsumeUnlockGoods(interactingUnit))
		{
			GameHost.SendToClient(interactingUnit.ClientId, new NtfPlaySfx
			{
				Sfx = TExit.Table[ExitId].EscapeSfx
			});
			if (GameManager.PlayerCount < 2)
			{
				_syncExitState.Value = EExitState.Finished;
			}
			else if (!_isGraceTimerRunning)
			{
				_syncStateRemainTime.Value = TConst.SafeExitActivatedTime.FloatValue;
				_isGraceTimerRunning = true;
			}
			interactingUnit.HandleUnitEvent(new EscapeEvent());
		}
		else
		{
			GameHost.SendToClient(interactingUnit.ClientId, new NtfPlaySfx
			{
				Sfx = "Fail"
			});
		}
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
