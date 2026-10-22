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

	private float _loopDelay;

	private float _loopDuration;

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnStressChanged(EStressState stressState)
	{
	}

	private void SetPropertyNumber(float number)
	{
	}

	private float GetPropertyNumber()
	{
		return 0f;
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
}
