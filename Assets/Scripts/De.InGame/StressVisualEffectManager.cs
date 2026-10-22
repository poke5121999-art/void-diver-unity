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

	public static Subject<StressVisualEffectManager> OnInstantiated;

	[SerializeField]
	private StressVisualEffectPropGroupScriptable _scriptable;

	[Header("Stress Prop Manager Setting: On Start")]
	[SerializeField]
	private float _targetFollowerPointRadius;

	[FormerlySerializedAs("_stressIntensity")]
	[Header("Debug")]
	[SerializeField]
	private float _stressIntensity;

	[SerializeField]
	private List<StressVisualEffectPropPoint> _points;

	private Vector4[] _pointPositions;

	private float[] _pointRadiuses;

	private float[] _pointIntensities;

	private EStressState _prevStressState;

	private Sequence _stressEffectSequence;

	private InGameVisualEffectSequence _visualEffectSequence;

	private bool _isThisTimeActive;

	private float _loopDelay;

	private float _loopDuration;

	private StressVisualEffectPropPoint _targetFollowerPoint;

	private GameObject _targetGameObject;

	public float StressIntensity => 0f;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void LateUpdate()
	{
	}

	private void OnDestroy()
	{
	}

	private StressVisualEffectPropPoint CreateTargetFollowerPoint()
	{
		return null;
	}

	public void RegisterPoint(StressVisualEffectPropPoint point)
	{
	}

	public void UnRegisterPoint(StressVisualEffectPropPoint point)
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnStressChanged(EStressState stressState)
	{
	}

	private void StartStressEffect(EStressState stressState)
	{
	}

	private void StartEscapeSequence()
	{
	}

	private void StartLoopSequence()
	{
	}

	private Sequence CreateEscapeSequence()
	{
		return null;
	}

	private Sequence CreateLoopSequence()
	{
		return null;
	}

	private void ResetStressEffect()
	{
	}

	private void UpdatePoints()
	{
	}

	private void UpdateTargetFollowerPoint()
	{
	}
}
