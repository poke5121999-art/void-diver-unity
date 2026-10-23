using System;
using UniRx;
using UnityEngine;

public class MinimapSightUnit : MonoBehaviour
{
	private IDisposable _instantiatedSubscription;

	private void OnEnable()
	{
		MinimapSightCore.RegisterSightUnit(this);
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = MinimapSightCore.OnInstantiated.Subscribe(delegate
		{
			MinimapSightCore.RegisterSightUnit(this);
		});
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		MinimapSightCore.UnregisterSightUnit(this);
	}
}
