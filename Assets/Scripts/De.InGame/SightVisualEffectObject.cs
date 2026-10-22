using UniRx;
using UnityEngine;
using VContainer;

public class SightVisualEffectObject : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	[Header("In Game Effect Setting: Brightness Effect")]
	public string PropertyName;

	private int _propertyNameID;

	public bool IsGlobalProperty;

	public string Description;

	private float _currentIntensity;

	private float _targetIntensity;

	private float _targetIntensityWhenReleavingMap;

	private MeshRenderer _meshRenderer;

	private PlaneSightCore PSCore => null;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void LateUpdate()
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void UpdateBrightnessVisualEffectIntensity()
	{
	}
}
