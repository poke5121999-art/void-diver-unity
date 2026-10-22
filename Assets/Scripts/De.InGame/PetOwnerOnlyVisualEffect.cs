using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

public class PetOwnerOnlyVisualEffect : MonoBase
{
	[SerializeField]
	private PetModel _petModel;

	[SerializeField]
	private List<GameObject> _deactivateTargets;

	[SerializeField]
	private List<GameObject> _hideLayerTargets;

	[SerializeField]
	private List<Behaviour> _disableComponents;

	[SerializeField]
	private int _hideLayer;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private CompositeDisposable _ownerDisposable;

	private CharacterModel _ownerCharacterModel;

	private ulong _currentOwnerClientId;

	private readonly List<int> _originalLayers;

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	private void OnOwnerClientIdChanged(ulong clientId)
	{
	}

	private void TryBindOwner()
	{
	}

	private void Refresh()
	{
	}

	private bool GetShouldShow()
	{
		return false;
	}
}
