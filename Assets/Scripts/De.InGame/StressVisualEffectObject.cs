using DG.Tweening;
using GameCommon.Enum;
using UniRx;
using UnityEngine;
using VContainer;

public class StressVisualEffectObject : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private Material _material;

	private EStressState _prevStressState;

	private Sequence _stressEffectSequence;

	[Header("Stress Visual Effect Object Setting: Data")]
	[SerializeField]
	private StressVisualEffectObjectScriptable _scriptableData;

	private InGameVisualEffectSequence _visualEffectSequence;

	private bool _isThisTimeActive;

	private float _loopDelay = 1f;

	private float _loopDuration = 1f;

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
			_stressEffectSequence = null;
		}
		SetPropertyNumber(0f);
		MeshRenderer component = GetComponent<MeshRenderer>();
		if ((bool)component)
		{
			_material = component.material;
		}
	}

	private void OnDisable()
	{
		ResetStressEffect();
	}

	private void OnDestroy()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
			_stressEffectSequence = null;
		}
		SetPropertyNumber(0f);
		_disposable?.Dispose();
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_disposable?.Dispose();
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel != null)
		{
			_disposable = new CompositeDisposable();
			characterModel.RxStressState.Subscribe(OnStressChanged).AddTo(_disposable);
		}
	}

	private void OnStressChanged(EStressState stressState)
	{
		StartStressEffect(stressState);
	}

	private void SetPropertyNumber(float number)
	{
		if (_scriptableData == null)
		{
			return;
		}
		if (!_scriptableData.IsGlobalProperty)
		{
			if (_material != null)
			{
				_material.SetFloat(_scriptableData.PropertyName, number);
			}
		}
		else
		{
			Shader.SetGlobalFloat(_scriptableData.PropertyName, number);
		}
	}

	private float GetPropertyNumber()
	{
		if (_scriptableData == null)
		{
			return 0f;
		}
		if (_scriptableData.IsGlobalProperty)
		{
			return Shader.GetGlobalFloat(_scriptableData.PropertyName);
		}
		if (!_material)
		{
			return 0f;
		}
		return _material.GetFloat(_scriptableData.PropertyName);
	}

	private void StartStressEffect(EStressState stressState)
	{
		if (_scriptableData == null)
		{
			return;
		}
		if (!_scriptableData.IsGlobalProperty)
		{
			if (_material == null || !_material.HasProperty(_scriptableData.PropertyName))
			{
				return;
			}
		}
		if (_prevStressState != stressState)
		{
			_prevStressState = stressState;
			_visualEffectSequence = _scriptableData.Sequences.Find((StressVisualEffectSequence sequence) => sequence.State == stressState)?.Sequence;
			if (_visualEffectSequence != null && _visualEffectSequence.IntensityMax > 0f)
			{
				StartLoopSequence();
			}
			else
			{
				StartEscapeSequence();
			}
		}
	}

	private void StartEscapeSequence()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
		}
		_stressEffectSequence = CreateEscapeSequence();
		_stressEffectSequence.Play();
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

	private Sequence CreateEscapeSequence()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.PrependInterval(_scriptableData.EscapeDelay);
		sequence.Append(DOTween.To(() => GetPropertyNumber(), delegate(float x)
		{
			SetPropertyNumber(x);
		}, 0f, _scriptableData.EscapeDuration)).SetEase(_scriptableData.EscapeEase);
		return sequence;
	}

	private Sequence CreateLoopSequence()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.AppendCallback(delegate
		{
			_loopDelay = _visualEffectSequence.Delay;
			_loopDuration = _visualEffectSequence.Duration;
			_isThisTimeActive = UnityEngine.Random.Range(0f, 1f) < _visualEffectSequence.ActiveRatio;
		});
		sequence.AppendInterval(_loopDelay);
		sequence.Append(DOTween.To(() => GetPropertyNumber(), delegate(float x)
		{
			SetPropertyNumber(x);
		}, _isThisTimeActive ? _visualEffectSequence.IntensityMax : 0f, _visualEffectSequence.TransitionDuration)).SetEase(_visualEffectSequence.EaseToMax);
		sequence.AppendInterval(_loopDuration);
		sequence.Append(DOTween.To(() => GetPropertyNumber(), delegate(float x)
		{
			SetPropertyNumber(x);
		}, _isThisTimeActive ? _visualEffectSequence.IntensityMin : 0f, _visualEffectSequence.TransitionAfterDuration)).SetEase(_visualEffectSequence.EaseToMin);
		sequence.OnComplete(delegate
		{
			StartLoopSequence();
		});
		return sequence;
	}

	private void ResetStressEffect()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
			_stressEffectSequence = null;
		}
		SetPropertyNumber(0f);
	}
}
