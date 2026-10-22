using System;
using UnityEngine;

public class PlaneSightCircleArea : MonoBehaviour, IPlaneSightDetector
{
	private IDisposable _instantiatedSubscription;

	[Header("Plane Sight Circle Area Setting")]
	[SerializeField]
	private bool _useLinecast;

	[SerializeField]
	private float _areaRadius;

	private PlaneSightCore PSCore => null;

	private void OnEnable()
	{
	}

	private void OnPlaneSightInstantiated(PlaneSightCore fow)
	{
	}

	private void OnDisable()
	{
	}

	void IPlaneSightDetector.RegisterToManagerAsDetector()
	{
	}

	void IPlaneSightDetector.UnRegisterFromManagerAsDetector()
	{
	}

	public bool GetIsThisDetected(GameObject detectableObject, bool ignoreToggle = false)
	{
		return false;
	}

	public Transform GetTransform()
	{
		return null;
	}
}
