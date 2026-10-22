using System;
using UniRx;
using UnityEngine;
using VContainer;

[DefaultExecutionOrder(200)]
public class PlaneSightPetVisualEffect : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCutsceneManager _gameCutsceneManager;

	[SerializeField]
	private Light _pointLight;

	[SerializeField]
	private Vector3 _pointLightWorldOffset;

	[SerializeField]
	private PetModel _petModel;

	private Transform _cachedTransform;

	private bool _isRevealing;

	private bool _ownerLightOn;

	private CharacterModel _ownerCharacterModel;

	private ulong _currentOwnerClientId;

	private CompositeDisposable _ownerDisposable;

	private IDisposable _ownerIdSubscription;

	private bool IsVisualValid => false;

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnOwnerClientIdChanged(ulong clientId)
	{
	}

	private void Update()
	{
	}

	private void TryBindOwner()
	{
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateUnityLights()
	{
	}
}
