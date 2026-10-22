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
	private float _radius;

	[SerializeField]
	private float _intensity;

	private StressVisualEffectManager VisualEffectManager => null;

	public float Radius => 0f;

	public float Intensity => 0f;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void RegisterToManager()
	{
	}

	public void SetIsUsingPlayerStress(bool target)
	{
	}

	public void SetRadius(float radius)
	{
	}
}
