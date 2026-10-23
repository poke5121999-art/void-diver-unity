using System;
using System.Linq;
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
		if (ValidateField())
		{
			_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		}
	}

	private void OnDestroy()
	{
		if (_viewTargetDisposable != null)
		{
			_viewTargetDisposable.Dispose();
		}
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		if (_viewTargetDisposable != null)
		{
			_viewTargetDisposable.Dispose();
		}
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel != null)
		{
			_viewTargetDisposable = new CompositeDisposable();
			characterModel.RxStressState.Subscribe(OnStressChanged).AddTo(_viewTargetDisposable);
		}
	}

	private void OnStressChanged(EStressState stressState)
	{
		bool isActive = _stressConditions.Contains(stressState);
		if (_targetObjects != null)
		{
			_targetObjects.ForEach(delegate(GameObject go)
			{
				go.SetActive(isActive);
			});
		}
	}

	private bool ValidateField()
	{
		bool result = true;
		if (_targetObjects == null)
		{
			_logger.Error(string.Format("{0} TargetObjects is null in StressStateActivator", base.gameObject));
			result = false;
		}
		else if (_targetObjects.Contains(base.gameObject))
		{
			_logger.Error(string.Format("{0} TargetObjects contains itself in StressStateActivator", base.gameObject));
			result = false;
		}
		else if (_targetObjects.Any((GameObject go) => base.transform.IsChildOf(go.transform)))
		{
			_logger.Error(string.Format("{0} TargetObjects contains its parent in StressStateActivator", base.gameObject));
			result = false;
		}
		if (_stressConditions == null)
		{
			_logger.Error(string.Format("{0} EnableStressStates is null in StressStateActivator", base.gameObject));
			result = false;
		}
		return result;
	}
}
