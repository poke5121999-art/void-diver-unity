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
	private Vector3 _pointLightWorldOffset = Vector3.zero;

	[SerializeField]
	private PetModel _petModel;

	private Transform _cachedTransform;

	private bool _isRevealing;

	private bool _ownerLightOn = true;

	private CharacterModel _ownerCharacterModel;

	private ulong _currentOwnerClientId = ulong.MaxValue;

	private CompositeDisposable _ownerDisposable;

	private IDisposable _ownerIdSubscription;

	private bool IsVisualValid
	{
		get
		{
			if (!(_gameManager != null))
			{
				return false;
			}
			if (_gameManager.StageState == GameCommon.Enum.EStageState.Intro)
			{
				return false;
			}
			if (!(_gameCutsceneManager == null) && _gameCutsceneManager.RxIsPlaying.Value)
			{
				return false;
			}
			if (_ownerCharacterModel == null)
			{
				return true;
			}
			return _ownerCharacterModel.RxState.Value != GameCommon.Enum.EActionState.Escaped;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_cachedTransform = base.transform;
		if (_petModel == null)
		{
			_petModel = GetComponentInParent<PetModel>();
		}
	}

	private void OnEnable()
	{
		PlaneSightCore.OnRevealingMapChanged += OnRevealingMapChanged;
		PlaneSightCore instance = PlaneSightCore.Instance;
		_isRevealing = instance != null && instance.IsRevealingMap;
		_ownerLightOn = true;
		if (_petModel != null)
		{
			_ownerIdSubscription = _petModel.RxOwnerClientId.Subscribe(OnOwnerClientIdChanged);
		}
	}

	private void OnDisable()
	{
		PlaneSightCore.OnRevealingMapChanged -= OnRevealingMapChanged;
		_ownerIdSubscription?.Dispose();
		_ownerIdSubscription = null;
		_ownerDisposable?.Dispose();
		_ownerDisposable = null;
		_ownerCharacterModel = null;
		_currentOwnerClientId = ulong.MaxValue;
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
	}

	private void Update()
	{
		if (_ownerCharacterModel == null && _currentOwnerClientId != ulong.MaxValue)
		{
			TryBindOwner();
		}
	}

	private void TryBindOwner()
	{
		if (PlaneSightCore.Instance == null)
		{
			return;
		}
		CharacterModel characterModel = PlaneSightCore.Instance.FindCharacterModel(_currentOwnerClientId);
		if (!(characterModel == null))
		{
			_ownerCharacterModel = characterModel;
			_ownerDisposable = new CompositeDisposable();
			characterModel.RxIsLightOn.Subscribe(delegate(bool v)
			{
				_ownerLightOn = v;
			}).AddTo(_ownerDisposable);
		}
		else
		{
			_ownerLightOn = true;
		}
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
		_isRevealing = isRevealing;
	}

	private void LateUpdate()
	{
		UpdateUnityLights();
	}

	private void UpdateUnityLights()
	{
		if (_pointLight == null)
		{
			return;
		}
		if (IsVisualValid)
		{
			if (!_pointLight.gameObject.activeSelf)
			{
				_pointLight.gameObject.SetActive(value: true);
			}
			bool flag = !_isRevealing && _ownerLightOn;
			_pointLight.enabled = flag;
			if (flag)
			{
				_pointLight.gameObject.transform.position = _cachedTransform.position + _pointLightWorldOffset;
			}
		}
		else if (_pointLight.gameObject.activeSelf)
		{
			_pointLight.gameObject.SetActive(value: false);
		}
	}
}
