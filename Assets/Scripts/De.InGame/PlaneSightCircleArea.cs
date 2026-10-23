using System;
using UniRx;
using UnityEngine;

public class PlaneSightCircleArea : MonoBehaviour, IPlaneSightDetector
{
	private IDisposable _instantiatedSubscription;

	[Header("Plane Sight Circle Area Setting")]
	[SerializeField]
	private bool _useLinecast = true;

	[SerializeField]
	private float _areaRadius = 4f;

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
		Vector3 position = detectableObject.transform.position;
		Vector3 position2 = base.gameObject.transform.position;
		float num = position.x - position2.x;
		float num2 = position.z - position2.z;
		return num * num + 0f + num2 * num2 < _areaRadius * _areaRadius;
	}

	public Transform GetTransform()
	{
		return base.transform;
	}
}
