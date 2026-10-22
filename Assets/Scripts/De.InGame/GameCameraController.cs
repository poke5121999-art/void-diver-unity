using DG.Tweening;
using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class GameCameraController : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private SeriLogger _logger;

	private CameraPostProcessingController _postProcessingController;

	private CompositeDisposable _disposable;

	private Vector3 _currentPosition;

	private StressCameraEffectSettings _stressCameraEffectSettings;

	private EStressState _prevStressState;

	private StressCameraEffectSequence _stressCameraEffectSequence;

	private bool _isThisTimeActive;

	private Tween _oneShotStressEffectTween;

	private Sequence _stressEffectSequence;

	private ulong ClientId => 0uL;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnHpChanged(int hp, float hpMax)
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnNtfCameraShake(NtfCameraShake message)
	{
	}

	private void OnStageClosed()
	{
	}

	private void OnStageEnded()
	{
	}

	private void Clear()
	{
	}

	private void OnDamage(NtfHpDamage message)
	{
	}

	private void OnRecovery(NtfHpRecovery message)
	{
	}

	private void OnPositionChanged(Vector3 position)
	{
	}

	private void LoadStressSettings()
	{
	}

	private void OnStressStateChanged(EStressState stressState)
	{
	}

	private void StartStressStateEffect(EStressState stressState)
	{
	}

	private void ResetStressStateEffect()
	{
	}

	private void StartLoopSequence()
	{
	}

	private Sequence CreateLoopSequence()
	{
		return null;
	}

	private void EscapeSequence()
	{
	}

	private Sequence CreateEscapeSequence()
	{
		return null;
	}

	private bool IsValidOneShotEffect()
	{
		return false;
	}

	private void StartOneShotEffect()
	{
	}
}
