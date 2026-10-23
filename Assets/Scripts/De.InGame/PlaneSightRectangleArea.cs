using System;
using UniRx;
using UnityEngine;

public class PlaneSightRectangleArea : MonoBehaviour, IPlaneSightDetector
{
	private IDisposable _instantiatedSubscription;

	[Header("Plane Sight Circle Area Setting")]
	[SerializeField]
	private bool _useLinecast = true;

	[SerializeField]
	private float _areaWidth = 3f;

	[SerializeField]
	private float _areaHeight = 4f;

	private PlaneSightCore PSCore => PlaneSightCore.Instance;

	private void OnEnable()
	{
		((IPlaneSightDetector)this).RegisterToManagerAsDetector();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = PlaneSightCore.OnInstantiated.Subscribe(OnPlaneSightInstantiated);
	}

	private void OnPlaneSightInstantiated(PlaneSightCore fow)
	{
		if (base.enabled)
		{
			((IPlaneSightDetector)this).RegisterToManagerAsDetector();
		}
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		((IPlaneSightDetector)this).UnRegisterFromManagerAsDetector();
	}

	void IPlaneSightDetector.RegisterToManagerAsDetector()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightCore.Instance.RegisterDetector(this);
		}
	}

	void IPlaneSightDetector.UnRegisterFromManagerAsDetector()
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightCore.Instance.UnRegisterDetector(this);
		}
	}

	public bool GetIsThisDetected(GameObject detectableObject, bool ignoreToggle = false)
	{
		if (PlaneSightCore.Instance == null)
		{
			return false;
		}
		if (detectableObject == null)
		{
			return false;
		}
		if (_useLinecast && Physics.Linecast(base.gameObject.transform.position, detectableObject.transform.position, out var _, PSCore.WallLayerMask))
		{
			return false;
		}
		float num = _areaWidth * 0.5f;
		float num2 = _areaHeight * 0.5f;
		Vector3 vector = base.transform.InverseTransformPoint(detectableObject.transform.position);
		if (0f - num <= vector.x && vector.x <= num && 0f - num2 <= vector.z && vector.z <= num2)
		{
			return true;
		}
		return false;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}
}
