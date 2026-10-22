using System;
using UnityEngine;

[DefaultExecutionOrder(0)]
[ExecuteAlways]
public class FakeShadowPoint : MonoBehaviour
{
	[Header("Toggle Setting")]
	[SerializeField]
	private bool _screenSpaceEnable;

	[SerializeField]
	private bool _objectEnable;

	[Header("Offset Setting")]
	[SerializeField]
	private Vector3 _offset;

	[Header("PlaneSystem Setting")]
	[Tooltip("전장의 안개 활성 상태에서만 그림자 동작 여부.")]
	[SerializeField]
	private bool _usingPlaneSystemEnable;

	private static readonly int _planeSystemEnableProperty;

	private IDisposable _instantiatedSubscription;

	public Vector3 Position => default;

	public float Range => 0f;

	public float CasterRange => 0f;

	public bool ScreenSpaceEnable => false;

	public bool ObjectEnable => false;

	private static bool IsPlaneSystemEnabled()
	{
		return false;
	}

	private void OnEnable()
	{
	}

	private void RegisterSelf()
	{
	}

	private void OnDisable()
	{
	}
}
