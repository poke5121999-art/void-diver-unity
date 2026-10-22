using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundTriggerAdjuster : MonoBehaviour
{
	[SerializeField]
	protected BackgroundTriggerSettings BackgroundTriggerSettings;

	[SerializeField]
	[FormerlySerializedAs("_boxColliders")]
	protected List<BoxCollider> BoxColliders;

	[SerializeField]
	protected float BoxColliderMaxY;

	protected virtual void Start()
	{
	}

	public void SetBackgroundTriggerSettings(BackgroundTriggerSettings settings)
	{
	}

	public void CacheBoxColliders()
	{
	}

	protected virtual Vector3 GetTriggerOffset()
	{
		return default;
	}

	protected virtual float GetTriggerBump()
	{
		return 0f;
	}

	protected virtual Vector2 GetTriggerYRange()
	{
		return default;
	}
}
