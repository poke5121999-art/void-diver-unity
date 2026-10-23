using System;
using UniRx;
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

	private PlaneSightCore PSCore => PlaneSightCore.Instance;

	private void OnEnable()
	{
		if (PlaneSightCore.Instance != null)
		{
			ApplyTargetLayer();
			return;
		}
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = PlaneSightCore.OnInstantiated.AsObservable().Subscribe(delegate
		{
			ApplyTargetLayer();
		});
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		PlaneSightCore instance = PlaneSightCore.Instance;
		if (!(instance == null) && instance.CullingLayer == _targetCullingLayer)
		{
			instance.ChangeCullingLayer(_defaultCullingLayer);
		}
	}

	private void ApplyTargetLayer()
	{
		if (base.enabled)
		{
			PlaneSightCore instance = PlaneSightCore.Instance;
			if (instance != null)
			{
				instance.ChangeCullingLayer(_targetCullingLayer);
			}
		}
	}
}
