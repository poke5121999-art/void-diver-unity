using System;
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
	private bool _rimEffectEnable;

	[Header("Fake Settings")]
	[Tooltip("Light 컴포넌트 없이 가짜 점 데이터로 동작. _fakeRange/_fakeIntensity 사용.")]
	[SerializeField]
	private bool _isFake;

	[FormerlySerializedAs("_fakeLightRange")]
	[SerializeField]
	private float _fakeRange;

	[FormerlySerializedAs("_fakeLightIntensity")]
	[SerializeField]
	private float _fakeIntensity;

	private IDisposable _instantiatedSubscription;

	public bool PlanePointBufferEnable => false;

	public bool RimEffectEnable => false;

	public bool IsFake => false;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void RegisterToManager()
	{
	}

	private void UnRegisterFromManager()
	{
	}

	public virtual Color GetLightColor()
	{
		return default;
	}

	public virtual float GetLightRange()
	{
		return 0f;
	}

	public virtual float GetLightInetnsity()
	{
		return 0f;
	}

	public virtual Vector3 GetLightPosition()
	{
		return default;
	}

	[ContextMenu("Find Light Component")]
	private void FindLightComponent()
	{
	}
}
