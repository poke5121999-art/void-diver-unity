using UniRx;
using System;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class StressVisualEffectPropPoint : MonoBehaviour
{
	private IDisposable _instantiatedSubscription;

	[FormerlySerializedAs("_isUsingPlayerSanity")]
	[SerializeField]
	private bool _isUsingPlayerStress;

	[SerializeField]
	private float _radius = 10f;

	[SerializeField]
	private float _intensity;

	private StressVisualEffectManager VisualEffectManager => StressVisualEffectManager.Instance;

	public float Radius => _radius;

	public float Intensity => ((bool)StressVisualEffectManager.Instance && _isUsingPlayerStress) ? StressVisualEffectManager.Instance.StressIntensity : _intensity;

	private void OnEnable()
	{
		RegisterToManager();
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = StressVisualEffectManager.OnInstantiated.Subscribe(delegate(StressVisualEffectManager _)
		{
			RegisterToManager();
		});
	}

	private void OnDisable()
	{
		_instantiatedSubscription?.Dispose();
		_instantiatedSubscription = null;
		if ((bool)StressVisualEffectManager.Instance)
		{
			StressVisualEffectManager.Instance.UnRegisterPoint(this);
		}
	}

	private void RegisterToManager()
	{
		if ((bool)StressVisualEffectManager.Instance)
		{
			StressVisualEffectManager.Instance.RegisterPoint(this);
		}
	}

	public void SetIsUsingPlayerStress(bool target)
	{
		_isUsingPlayerStress = target;
	}

	public void SetRadius(float radius)
	{
		_radius = radius;
	}
}
