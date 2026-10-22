using System;
using UnityEngine;

public class PlaneSightLayerSwapper : MonoBehaviour
{
	[PlaneSightTargetCameraLayer]
	[SerializeField]
	private int _defaultCullingLayer;

	[PlaneSightTargetCameraLayer]
	[SerializeField]
	private int _targetCullingLayer;

	private IDisposable _instantiatedSubscription;

	private PlaneSightCore PSCore => null;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void ApplyTargetLayer()
	{
	}
}
