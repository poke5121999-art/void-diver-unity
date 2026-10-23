using System.Collections.Generic;
using GameCommon.Enum;
using Unity.Netcode;
using UniRx;
using UnityEngine;
using VContainer;

public class PetOwnerOnlyVisualEffect : MonoBase
{
	[SerializeField]
	private PetModel _petModel;

	[SerializeField]
	private List<GameObject> _deactivateTargets = new List<GameObject>();

	[SerializeField]
	private List<GameObject> _hideLayerTargets = new List<GameObject>();

	[SerializeField]
	private List<Behaviour> _disableComponents = new List<Behaviour>();

	[SerializeField]
	private int _hideLayer;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private CompositeDisposable _ownerDisposable;

	private CharacterModel _ownerCharacterModel;

	private ulong _currentOwnerClientId = ulong.MaxValue;

	private readonly List<int> _originalLayers = new List<int>();

	protected override void Awake()
	{
		base.Awake();
		if (_petModel == null)
		{
			_petModel = GetComponentInParent<PetModel>();
		}
		foreach (GameObject hideLayerTarget in _hideLayerTargets)
		{
			_originalLayers.Add((hideLayerTarget != null) ? hideLayerTarget.layer : 0);
		}
	}

	private void OnEnable()
	{
		_disposable = new CompositeDisposable();
		if (_petModel != null)
		{
			_petModel.RxOwnerClientId.Subscribe(OnOwnerClientIdChanged).AddTo(_disposable);
		}
		if (_gameManager != null)
		{
			_gameManager.RxStageState.Subscribe(delegate
			{
				Refresh();
			}).AddTo(_disposable);
		}
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
		_disposable = null;
		_ownerDisposable?.Dispose();
		_ownerDisposable = null;
		_ownerCharacterModel = null;
		_currentOwnerClientId = ulong.MaxValue;
	}

	private void Update()
	{
		if (_ownerCharacterModel == null && _currentOwnerClientId != ulong.MaxValue)
		{
			TryBindOwner();
			if (_ownerCharacterModel != null)
			{
				Refresh();
			}
		}
	}

	private void OnOwnerClientIdChanged(ulong clientId)
	{
		if (_currentOwnerClientId == clientId && _ownerCharacterModel != null)
		{
			return;
		}
		_currentOwnerClientId = clientId;
		_ownerDisposable?.Dispose();
		_ownerDisposable = null;
		_ownerCharacterModel = null;
		TryBindOwner();
		Refresh();
	}

	private void TryBindOwner()
	{
		if (!(_gameManager == null) && _currentOwnerClientId != ulong.MaxValue)
		{
			CharacterModel characterModel = _gameManager.FindCharacterModel(_currentOwnerClientId);
			if (!(characterModel == null))
			{
				_ownerCharacterModel = characterModel;
				_ownerDisposable = new CompositeDisposable();
				characterModel.RxState.Subscribe(delegate
				{
					Refresh();
				}).AddTo(_ownerDisposable);
			}
		}
	}

	private void Refresh()
	{
		if (_currentOwnerClientId == ulong.MaxValue)
		{
			return;
		}
		bool shouldShow = GetShouldShow();
		foreach (GameObject deactivateTarget in _deactivateTargets)
		{
			if (deactivateTarget != null)
			{
				deactivateTarget.SetActive(shouldShow);
			}
		}
		for (int i = 0; i < _hideLayerTargets.Count; i++)
		{
			if (_hideLayerTargets[i] != null)
			{
				_hideLayerTargets[i].layer = (shouldShow ? _originalLayers[i] : _hideLayer);
			}
		}
		foreach (Behaviour disableComponent in _disableComponents)
		{
			if (disableComponent != null)
			{
				disableComponent.enabled = shouldShow;
			}
		}
	}

	private bool GetShouldShow()
	{
		EActionState eActionState = (_ownerCharacterModel != null) ? _ownerCharacterModel.RxState.Value : ((EActionState)0);
		if (_gameManager == null || _gameManager.IsInLounge)
		{
			return eActionState != EActionState.StageReady;
		}
		if (NetworkManager.Singleton != null && _currentOwnerClientId == NetworkManager.Singleton.LocalClientId && _gameManager.StageState != EStageState.Intro && eActionState != EActionState.StageReady)
		{
			return eActionState != EActionState.Escaped;
		}
		return false;
	}
}
