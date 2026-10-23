using System;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(0)]
[ExecuteAlways]
public class PlanePointHelper : MonoBehaviour
{
	[Header("Light Source")]
	[SerializeField]
	private Light _light;

	[Header("Toggle Settings")]
	[FormerlySerializedAs("_lightBufferEnable")]
	[SerializeField]
	private bool _planePointBufferEnable;

	[FormerlySerializedAs("_rimLightEnable")]
	[SerializeField]
	private bool _rimEffectEnable = true;

	[Header("Fake Settings")]
	[Tooltip("Light 컴포넌트 없이 가짜 점 데이터로 동작. _fakeRange/_fakeIntensity 사용.")]
	[SerializeField]
	private bool _isFake;

	[FormerlySerializedAs("_fakeLightRange")]
	[SerializeField]
	private float _fakeRange = 2f;

	[FormerlySerializedAs("_fakeLightIntensity")]
	[SerializeField]
	private float _fakeIntensity = 1f;

	private IDisposable _instantiatedSubscription;

	public bool PlanePointBufferEnable => _planePointBufferEnable;

	public bool RimEffectEnable => _rimEffectEnable;

	public bool IsFake => _isFake;

	private void Awake()
	{
		if (_light == null && !_isFake)
		{
			_light = GetComponent<Light>();
		}
	}

	private void OnEnable()
	{
		RegisterToManager();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = PlanePointManager.OnInstantiated.Subscribe(delegate
		{
			RegisterToManager();
		});
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		UnRegisterFromManager();
	}

	private void OnDestroy()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		UnRegisterFromManager();
	}

	private void RegisterToManager()
	{
		if (PlanePointManager.Instance != null)
		{
			PlanePointManager.Instance.RegisterPoint(this);
		}
	}

	private void UnRegisterFromManager()
	{
		if (PlanePointManager.Instance != null)
		{
			PlanePointManager.Instance.UnRegisterPoint(this);
		}
	}

	public virtual Color GetLightColor()
	{
		if (!_isFake && !(_light == null))
		{
			return _light.color;
		}
		return new Color(0f, 0f, 0f, 1f);
	}

	public virtual float GetLightRange()
	{
		if (_isFake)
		{
			return _fakeRange;
		}
		if (_light == null)
		{
			return 0f;
		}
		return _light.range;
	}

	public virtual float GetLightInetnsity()
	{
		if (_isFake)
		{
			return _fakeIntensity;
		}
		if (_light == null)
		{
			return 0f;
		}
		return _light.intensity;
	}

	public virtual Vector3 GetLightPosition()
	{
		return base.transform.position;
	}

	[ContextMenu("Find Light Component")]
	private void FindLightComponent()
	{
		_light = GetComponent<Light>();
	}
}
