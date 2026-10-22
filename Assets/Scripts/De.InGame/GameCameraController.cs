using System.Linq;
using DG.Tweening;
using GameCommon.Enum;
using GameCommon.Table;
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

	private ulong ClientId => _gameManager.RxViewTargetClientId.Value;

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		LoadStressSettings();
		_postProcessingController = _cameraManager.GetComponent<CameraPostProcessingController>();
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.LoadCompleted).Subscribe(delegate
		{
			OnStageLoadCompleted();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Ended).Subscribe(delegate
		{
			OnStageEnded();
		}).AddTo(this);
		_gameChannel.Receive<NtfHpDamage>().Subscribe(OnDamage).AddTo(this);
		_gameChannel.Receive<NtfHpRecovery>().Subscribe(OnRecovery).AddTo(this);
		_gameChannel.Receive<NtfCameraShake>().Subscribe(OnNtfCameraShake).AddTo(this);
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		ResetStressStateEffect();
		_disposable?.Dispose();
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel == null)
		{
			return;
		}
		_cameraManager.ActivatePlayerCamera(characterModel.transform);
		_disposable = new CompositeDisposable();
		characterModel.RxHpMax.Subscribe(delegate
		{
			OnHpChanged(characterModel.Hp, characterModel.HpMax);
		}).AddTo(_disposable);
		characterModel.RxHp.Subscribe(delegate
		{
			OnHpChanged(characterModel.Hp, characterModel.HpMax);
		}).AddTo(_disposable);
		characterModel.RxStressState.Subscribe(OnStressStateChanged).AddTo(_disposable);
		characterModel.RxPosition.Subscribe(OnPositionChanged).AddTo(_disposable);
	}

	private void OnHpChanged(int hp, float hpMax)
	{
		_postProcessingController.LowHpVisualIntensity = 1f - (float)hp / hpMax;
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnNtfCameraShake(NtfCameraShake message)
	{
		if (message.TargetClientId == ClientId || message.IsGlobal)
		{
			_cameraManager.Shake(message.ShakeAmplitude, message.ShakeFrequency, message.ShakeDuration);
		}
	}

	private void OnStageClosed()
	{
		_cameraManager.DeactivatePlayerCamera();
		Clear();
	}

	private void OnStageEnded()
	{
		ResetStressStateEffect();
	}

	private void Clear()
	{
		if (_postProcessingController != null)
		{
			ResetStressStateEffect();
			_postProcessingController.Clear();
		}
		if (_cameraManager != null)
		{
			_cameraManager.Clear();
		}
	}

	private void OnDamage(NtfHpDamage message)
	{
		UnitModel unitModel = _objectManager.FindUnitModel(message.NetworkObjectId);
		if (unitModel == null)
		{
			return;
		}
		CharacterModel characterModel = unitModel as CharacterModel;
		if (characterModel == null || characterModel.ClientId != ClientId || message.Amount < 1)
		{
			return;
		}
		if (TConst.HitShakeDuration.FloatValue > 0f)
		{
			_cameraManager.Shake(TConst.HitShakeAmplitude.FloatValue, TConst.HitShakeFrequency.FloatValue, TConst.HitShakeDuration.FloatValue);
		}
		_postProcessingController.PlayDamageHpPostProcessing();
	}

	private void OnRecovery(NtfHpRecovery message)
	{
		UnitModel unitModel = _objectManager.FindUnitModel(message.NetworkObjectId);
		if (unitModel == null)
		{
			return;
		}
		CharacterModel characterModel = unitModel as CharacterModel;
		if (!(characterModel == null) && characterModel.ClientId == ClientId && message.Amount >= 1)
		{
			_postProcessingController.PlayHealingHpPostProcessing();
		}
	}

	private void OnPositionChanged(Vector3 position)
	{
		if (Vector3.Distance(_currentPosition, position) > 5f)
		{
			_cameraManager.WarpToFollow();
		}
		_currentPosition = position;
	}

	private void LoadStressSettings()
	{
		_stressCameraEffectSettings = Resources.Load<StressCameraEffectSettings>("Scriptable/Art/CameraEffect/StressCameraEffectSettings");
	}

	private void OnStressStateChanged(EStressState stressState)
	{
		StartStressStateEffect(stressState);
	}

	private void StartStressStateEffect(EStressState stressState)
	{
		if (_stressCameraEffectSettings == null || _prevStressState == stressState)
		{
			return;
		}
		_stressCameraEffectSequence = _stressCameraEffectSettings.EffectSequences.FirstOrDefault((StressCameraEffectSequence s) => s.State == stressState);
		if (_stressCameraEffectSequence != null)
		{
			if (_prevStressState < stressState)
			{
				StartOneShotEffect();
			}
			_prevStressState = stressState;
			if (_stressCameraEffectSequence.Sequence.IntensityMax > 0f)
			{
				StartLoopSequence();
			}
			else
			{
				EscapeSequence();
			}
		}
	}

	private void ResetStressStateEffect()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
		}
		_stressEffectSequence = null;
		if (_oneShotStressEffectTween != null)
		{
			_oneShotStressEffectTween.Kill();
		}
		_oneShotStressEffectTween = null;
		_postProcessingController.StressVisualIntensity = 0f;
		_postProcessingController.OneShotStressVisualIntensity = 0f;
	}

	private void StartLoopSequence()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
		}
		_stressEffectSequence = CreateLoopSequence();
		_stressEffectSequence.Play();
	}

	private Sequence CreateLoopSequence()
	{
		CameraEffectSequence cameraSequence = _stressCameraEffectSequence.Sequence;
		Sequence sequence = DOTween.Sequence();
		sequence.AppendCallback(delegate
		{
			_isThisTimeActive = Random.Range(0f, 1f) < cameraSequence.ActiveRatio;
		});
		sequence.AppendInterval(cameraSequence.StartDelay);
		sequence.Append(DOTween.To(() => _postProcessingController.StressVisualIntensity, delegate(float x)
		{
			_postProcessingController.StressVisualIntensity = x;
		}, _isThisTimeActive ? cameraSequence.IntensityMax : 0f, cameraSequence.DurationToMax)).SetEase(cameraSequence.EaseToMax);
		sequence.AppendInterval(cameraSequence.DelayBetween);
		sequence.Append(DOTween.To(() => _postProcessingController.StressVisualIntensity, delegate(float x)
		{
			_postProcessingController.StressVisualIntensity = x;
		}, _isThisTimeActive ? cameraSequence.IntensityMin : 0f, cameraSequence.DurationToMin)).SetEase(cameraSequence.EaseToMin);
		sequence.OnComplete(delegate
		{
			if (_stressEffectSequence != null)
			{
				_stressEffectSequence.Kill();
			}
			_stressEffectSequence = CreateLoopSequence();
			_stressEffectSequence.Play();
		});
		return sequence;
	}

	private void EscapeSequence()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
		}
		_stressEffectSequence = CreateEscapeSequence();
		_stressEffectSequence.Play();
	}

	private Sequence CreateEscapeSequence()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.PrependInterval(_stressCameraEffectSettings.EscapeDelay);
		sequence.Append(DOTween.To(() => _postProcessingController.StressVisualIntensity, delegate(float x)
		{
			_postProcessingController.StressVisualIntensity = x;
		}, 0f, _stressCameraEffectSettings.EscapeDuration)).SetEase(_stressCameraEffectSettings.EscapeEase);
		return sequence;
	}

	private bool IsValidOneShotEffect()
	{
		if (_stressCameraEffectSequence == null || _stressCameraEffectSequence.OneShotCurve == null)
		{
			return false;
		}
		return _stressCameraEffectSequence.OneShotCurve.keys.Length > 1;
	}

	private void StartOneShotEffect()
	{
		if (_oneShotStressEffectTween != null && _oneShotStressEffectTween.IsActive())
		{
			_oneShotStressEffectTween.Kill();
		}
		if (IsValidOneShotEffect())
		{
			Keyframe[] keys = _stressCameraEffectSequence.OneShotCurve.keys;
			float time = keys[keys.Length - 1].time;
			_oneShotStressEffectTween = DOVirtual.Float(0f, 1f, time, delegate(float x)
			{
				_postProcessingController.OneShotStressVisualIntensity = x;
			}).SetEase(_stressCameraEffectSequence.OneShotCurve).Play();
		}
	}
}
