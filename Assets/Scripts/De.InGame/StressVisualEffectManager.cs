using System.Collections.Generic;
using DG.Tweening;
using GameCommon.Enum;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

[ExecuteInEditMode]
public class StressVisualEffectManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	public static StressVisualEffectManager Instance;

	public static Subject<StressVisualEffectManager> OnInstantiated = new Subject<StressVisualEffectManager>();

	[SerializeField]
	private StressVisualEffectPropGroupScriptable _scriptable;

	[Header("Stress Prop Manager Setting: On Start")]
	[SerializeField]
	private float _targetFollowerPointRadius = 20f;

	[FormerlySerializedAs("_stressIntensity")]
	[Header("Debug")]
	[SerializeField]
	private float _stressIntensity;

	[SerializeField]
	private List<StressVisualEffectPropPoint> _points = new List<StressVisualEffectPropPoint>();

	private Vector4[] _pointPositions;

	private float[] _pointRadiuses;

	private float[] _pointIntensities;

	private EStressState _prevStressState;

	private Sequence _stressEffectSequence;

	private InGameVisualEffectSequence _visualEffectSequence;

	private bool _isThisTimeActive;

	private float _loopDelay = 1f;

	private float _loopDuration = 1f;

	private StressVisualEffectPropPoint _targetFollowerPoint;

	private GameObject _targetGameObject;

	public float StressIntensity => _stressIntensity;

	protected override void Awake()
	{
		if (Application.isPlaying)
		{
			base.Awake();
		}
		_pointPositions = new Vector4[StressVisualEffectPropConsts.NUMBER_POINT_COUNT_MAX];
		_pointRadiuses = new float[StressVisualEffectPropConsts.NUMBER_POINT_COUNT_MAX];
		_pointIntensities = new float[StressVisualEffectPropConsts.NUMBER_POINT_COUNT_MAX];
		if ((bool)Instance && Instance != this)
		{
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			return;
		}
		Instance = this;
		OnInstantiated.OnNext(this);
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
			_targetFollowerPoint = CreateTargetFollowerPoint();
			if ((bool)_scriptable)
			{
				_scriptable.ApplyGlobalStressProperties();
			}
		}
	}

	private void LateUpdate()
	{
		UpdatePoints();
		UpdateTargetFollowerPoint();
	}

	private void OnDestroy()
	{
		if (_stressEffectSequence != null)
		{
			_stressEffectSequence.Kill();
			_stressEffectSequence = null;
		}
		_stressIntensity = 0f;
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private StressVisualEffectPropPoint CreateTargetFollowerPoint()
	{
		GameObject gameObject = new GameObject("Stress Visual Effect Prop Camera");
		gameObject.transform.parent = base.gameObject.transform;
		StressVisualEffectPropPoint stressVisualEffectPropPoint = gameObject.AddComponent<StressVisualEffectPropPoint>();
		stressVisualEffectPropPoint.SetIsUsingPlayerStress(target: true);
		stressVisualEffectPropPoint.SetRadius(_targetFollowerPointRadius);
		return stressVisualEffectPropPoint;
	}

	public void RegisterPoint(StressVisualEffectPropPoint point)
	{
		if (!_points.Contains(point))
		{
			_points.Add(point);
		}
	}

	public void UnRegisterPoint(StressVisualEffectPropPoint point)
	{
		if (_points.Contains(point))
		{
			_points.Remove(point);
		}
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
		_targetGameObject = null;
		CharacterController characterController = _gameManager.FindCharacterController(clientId);
		if ((bool)characterController)
		{
			_targetGameObject = characterController.transform.gameObject;
		}
	}

	private void OnStressChanged(EStressState stressState)
	{
		StartStressEffect(stressState);
	}

	private void StartStressEffect(EStressState stressState)
	{
		if (!_scriptable || _prevStressState == stressState)
		{
			return;
		}
		_prevStressState = stressState;
		_visualEffectSequence = _scriptable.Sequences.Find((StressVisualEffectSequence sequence) => sequence.State == stressState)?.Sequence;
		if (_visualEffectSequence != null && _visualEffectSequence.IntensityMax > 0f)
		{
			StartLoopSequence();
		}
		else
		{
			StartEscapeSequence();
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
		sequence.PrependInterval(_scriptable.EscapeDelay);
		sequence.Append(DOTween.To(() => _stressIntensity, delegate(float x)
		{
			_stressIntensity = x;
		}, 0f, _scriptable.EscapeDuration)).SetEase(_scriptable.EscapeEase);
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
		sequence.AppendInterval(_visualEffectSequence.Delay);
		sequence.Append(DOTween.To(() => _stressIntensity, delegate(float x)
		{
			_stressIntensity = x;
		}, _isThisTimeActive ? _visualEffectSequence.IntensityMax : 0f, _visualEffectSequence.TransitionDuration)).SetEase(_visualEffectSequence.EaseToMax);
		sequence.AppendInterval(_loopDuration);
		sequence.Append(DOTween.To(() => _stressIntensity, delegate(float x)
		{
			_stressIntensity = x;
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
		_stressIntensity = 0f;
	}

	private void UpdatePoints()
	{
		for (int i = 0; i < StressVisualEffectPropConsts.NUMBER_POINT_COUNT_MAX; i++)
		{
			if (i < _points.Count && _points[i] != null)
			{
				Vector3 position = _points[i].gameObject.transform.position;
				_pointPositions[i] = new Vector4(position.x, position.y, position.z, 0f);
				_pointRadiuses[i] = _points[i].Radius;
				_pointIntensities[i] = _points[i].Intensity;
			}
			else
			{
				_pointPositions[i] = Vector4.zero;
				_pointRadiuses[i] = 0f;
				_pointIntensities[i] = 0f;
			}
		}
		Shader.SetGlobalVectorArray(StressVisualEffectPropConsts.SHADER_PROPERTY_POINT_POSITIONS, _pointPositions);
		Shader.SetGlobalFloatArray(StressVisualEffectPropConsts.SHADER_PROPERTY_POINT_RADIUSES, _pointRadiuses);
		Shader.SetGlobalFloatArray(StressVisualEffectPropConsts.SHADER_PROPERTY_POINT_INTENSITIES, _pointIntensities);
	}

	private void UpdateTargetFollowerPoint()
	{
		if (!(_targetFollowerPoint == null) && !(_targetGameObject == null))
		{
			_targetFollowerPoint.gameObject.transform.position = _targetGameObject.transform.position;
		}
	}
}
