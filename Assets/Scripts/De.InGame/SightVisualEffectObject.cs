using UniRx;
using UnityEngine;
using VContainer;

public class SightVisualEffectObject : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	[Header("In Game Effect Setting: Brightness Effect")]
	public string PropertyName = "";

	private int _propertyNameID;

	public bool IsGlobalProperty;

	public string Description = "";

	private float _currentIntensity;

	private float _targetIntensity;

	private float _targetIntensityWhenReleavingMap;

	private MeshRenderer _meshRenderer;

	private PlaneSightCore PSCore => PlaneSightCore.Instance;

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_meshRenderer = GetComponent<MeshRenderer>();
		_propertyNameID = Shader.PropertyToID(PropertyName);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void LateUpdate()
	{
		UpdateBrightnessVisualEffectIntensity();
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_targetIntensity = 1f;
		_disposable?.Dispose();
		if (_gameManager.FindCharacterModel(clientId) != null)
		{
			_disposable = new CompositeDisposable();
		}
	}

	private void UpdateBrightnessVisualEffectIntensity()
	{
		if ((bool)PSCore)
		{
			_targetIntensityWhenReleavingMap = (PSCore.IsRevealingMap ? 1f : 0f);
		}
		float num = Mathf.Clamp01(Mathf.Max(_targetIntensity, _targetIntensityWhenReleavingMap));
		// TODO(decomp): an extra Mathf.Epsilon comparison (<= 10) around the lerp was inlined by Ghidra; the smoothing condition is read as |current - target| >= Epsilon @0x180710e10
		if (Mathf.Abs(_currentIntensity - num) >= Mathf.Epsilon)
		{
			num = Mathf.Lerp(_currentIntensity, num, Mathf.Clamp01(Time.deltaTime * 10f));
		}
		_currentIntensity = num;
		num = Mathf.Clamp01(num);
		if (!IsGlobalProperty)
		{
			if ((bool)_meshRenderer && (bool)_meshRenderer.material && _meshRenderer.material.HasProperty(_propertyNameID))
			{
				_meshRenderer.material.SetFloat(_propertyNameID, num);
			}
		}
		else
		{
			Shader.SetGlobalFloat(_propertyNameID, num);
		}
	}
}
