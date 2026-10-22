using System;
using UniRx;
using UnityEngine;

[DefaultExecutionOrder(0)]
[ExecuteAlways]
public class FakeShadowPoint : MonoBehaviour
{
	[Header("Toggle Setting")]
	[SerializeField]
	private bool _screenSpaceEnable = true;

	[SerializeField]
	private bool _objectEnable = true;

	[Header("Offset Setting")]
	[SerializeField]
	private Vector3 _offset;

	[Header("PlaneSystem Setting")]
	[Tooltip("전장의 안개 활성 상태에서만 그림자 동작 여부.")]
	[SerializeField]
	private bool _usingPlaneSystemEnable;

	private static readonly int _planeSystemEnableProperty = Shader.PropertyToID("_GlobalPlaneSystemEnable");

	private IDisposable _instantiatedSubscription;

	public Vector3 Position => base.transform.position + _offset;

	public float Range => FakeShadowConsts.NUMBER_POINT_RANGE;

	public float CasterRange => FakeShadowConsts.NUMBER_POINT_CATSTER_RANGE;

	public bool ScreenSpaceEnable
	{
		get
		{
			if (!_usingPlaneSystemEnable)
			{
				return _screenSpaceEnable;
			}
			if (!_screenSpaceEnable)
			{
				return false;
			}
			return IsPlaneSystemEnabled();
		}
	}

	public bool ObjectEnable
	{
		get
		{
			if (!_usingPlaneSystemEnable)
			{
				return _objectEnable;
			}
			if (!_objectEnable)
			{
				return false;
			}
			return IsPlaneSystemEnabled();
		}
	}

	private static bool IsPlaneSystemEnabled()
	{
		return Shader.GetGlobalFloat(_planeSystemEnableProperty) > 0.5f;
	}

	private void OnEnable()
	{
		RegisterSelf();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = FakeShadowManager.OnInstantiated.Subscribe(delegate
		{
			RegisterSelf();
		});
	}

	private void RegisterSelf()
	{
		if (FakeShadowManager.Instance != null)
		{
			FakeShadowManager.Instance.RegisterPointToManager(this);
		}
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		if (FakeShadowManager.Instance != null)
		{
			FakeShadowManager.Instance.UnregisterPointFromManager(this);
		}
	}
}
