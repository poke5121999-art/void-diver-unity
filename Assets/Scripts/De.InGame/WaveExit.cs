using System;
using GameCommon.Table;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class WaveExit : ExitObject
{
	[SerializeField]
	private NavMeshObstacle _navMeshObstacle;

	[SerializeField]
	private Collider _collider;

	private readonly NetworkVariable<EExitState> _syncExitState = new NetworkVariable<EExitState>();

	private readonly NetworkVariable<float> _syncStateRemainTime = new NetworkVariable<float>();

	public override float HoldingTime => GetHoldingTime();

	public bool DisplayOnMinimapAtStart => TExit.Table[base.ExitId].DisplayOnMinimapAtStart;

	private int WaveId => TExit.Table[base.ExitId].WaveId;

	private int SpecialFieldId => TExit.Table[base.ExitId].SpecialFieldId;

	public ReactiveProperty<float> RxStateRemainTime { get; } = new ReactiveProperty<float>();

	public override EExitState ExitState => _syncExitState.Value;

	public float StateRemainTime => _syncStateRemainTime.Value;

	public override string InteractionSfx => string.Empty;

	private void OnSyncExitStateChanged(EExitState oldValue, EExitState newValue)
	{
		base.RxExitState.Value = newValue;
		ApplyExitStateVisual(newValue);
		if (newValue == EExitState.Activated)
		{
			SoundManager.PlaySfx("WayOutReady");
		}
		else if (newValue == EExitState.Finished)
		{
			SoundManager.PlaySfx3D("BoothEscape_End", base.transform);
		}
	}

	private void OnStateRemainTimeChanged(float oldValue, float newValue)
	{
		RxStateRemainTime.Value = newValue;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		ObjectManager.OnInteractiveObjectSpawned.OnNext(this);
		_syncExitState.OnValueChanged += OnSyncExitStateChanged;
		_syncStateRemainTime.OnValueChanged += OnStateRemainTimeChanged;
		base.RxExitState.Value = _syncExitState.Value;
		RxStateRemainTime.Value = _syncStateRemainTime.Value;
		GoActivating.SetActive(value: false);
		GoActivated.SetActive(value: false);
		ApplyExitStateVisual(_syncExitState.Value);
		GameManager.RxStageState.Where((EStageState state) => state == EStageState.Ended).Subscribe(delegate
		{
			OnStageEnded();
		}).AddTo(this);
	}

	public override void OnNetworkDespawn()
	{
		_syncExitState.OnValueChanged -= OnSyncExitStateChanged;
		_syncStateRemainTime.OnValueChanged -= OnStateRemainTimeChanged;
		ObjectManager.OnInteractiveObjectDespawned.OnNext(this);
		base.OnNetworkDespawn();
	}

	private void ApplyExitStateVisual(EExitState state)
	{
		GoDeactivated.SetActive(state != EExitState.Activated && state != EExitState.Finished);
		GoFinished.SetActive(state == EExitState.Finished);
		if (state == EExitState.Finished)
		{
			GoActivating.SetActive(value: false);
			GoActivated.SetActive(value: false);
		}
		_navMeshObstacle.enabled = state != EExitState.Finished;
		_collider.enabled = state != EExitState.Finished;
	}

	private async UniTaskVoid ToggleActivatingToActivatedAsync()
	{
		await UniTask.Delay(TimeSpan.FromSeconds(4.0), ignoreTimeScale: false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
		GoActivating.SetActive(value: false);
		GoActivated.SetActive(value: true);
	}

	private void Update()
	{
		if (!GoActivating.activeSelf && ExitState == EExitState.Activating && _syncStateRemainTime.Value < 2.5f)
		{
			GoActivating.SetActive(value: true);
			SoundManager.PlaySfx3D("BoothEscape_Start", base.transform);
			ToggleActivatingToActivatedAsync().Forget();
		}
		if (!base.IsHost)
		{
			return;
		}
		EExitState value = _syncExitState.Value;
		if (value == EExitState.Deactivated || value == EExitState.Finished)
		{
			return;
		}
		_syncStateRemainTime.Value -= Time.deltaTime;
		switch (value)
		{
		case EExitState.Activating:
			if (!(_syncStateRemainTime.Value > 0f))
			{
				_syncStateRemainTime.Value = TConst.ExitActivatedTime.FloatValue;
				_syncExitState.Value = EExitState.Activated;
				if (SpecialFieldId >= 1)
				{
					float floatValue = TConst.ExitSpecialFieldRange.FloatValue;
					ObjectManager.SpawnSpecialField(base.NetworkObjectId, SpecialFieldId, base.transform.position, new Vector3(floatValue, 1f, floatValue));
				}
			}
			break;
		case EExitState.Activated:
			if (!(_syncStateRemainTime.Value > 0f))
			{
				_syncExitState.Value = EExitState.Finished;
				WaveManager.FinishWave(base.NetworkObjectId, isClear: true);
			}
			break;
		}
	}

	private void OnStageEnded()
	{
		if (ExitState == EExitState.Activated && base.IsHost)
		{
			_syncExitState.Value = EExitState.Finished;
		}
	}

	private float GetHoldingTime()
	{
		if (_syncExitState.Value != EExitState.Deactivated)
		{
			return -1f;
		}
		return TConst.ExitActivateInteractionTime.FloatValue;
	}

	public override void OnInteraction(UnitController interactingUnit)
	{
		AssertIfNotHost();
		if (_syncExitState.Value == EExitState.Deactivated && TryConsumeUnlockGoods(interactingUnit))
		{
			_syncExitState.Value = EExitState.Activating;
			WaveManager.StartWave(base.NetworkObjectId, WaveId, base.transform.position);
			_syncStateRemainTime.Value = TConst.ExitActivatingTime.FloatValue;
		}
	}

	public override bool CanInteract(UnitModel sourceModel)
	{
		return _syncExitState.Value == EExitState.Deactivated;
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
