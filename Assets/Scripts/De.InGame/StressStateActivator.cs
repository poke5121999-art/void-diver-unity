using GameCommon.Enum;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public class StressStateActivator : MonoBase
{
	[SerializeField]
	private GameObject[] _targetObjects;

	[SerializeField]
	private EStressState[] _stressConditions;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _viewTargetDisposable;

	private void Start()
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

	private bool ValidateField()
	{
		return false;
	}
}
