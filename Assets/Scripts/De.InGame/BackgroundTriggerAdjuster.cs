using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BackgroundTriggerAdjuster : MonoBehaviour
{
	[SerializeField]
	protected BackgroundTriggerSettings BackgroundTriggerSettings;

	[SerializeField]
	[FormerlySerializedAs("_boxColliders")]
	protected List<BoxCollider> BoxColliders = new List<BoxCollider>();

	[SerializeField]
	protected float BoxColliderMaxY;

	protected virtual void Start()
	{
		if (BoxColliders != null && BoxColliders.Count != 0)
		{
			BoxColliders.ForEach(delegate(BoxCollider _)
			{
				Vector3 triggerOffset = GetTriggerOffset();
				float triggerBump = GetTriggerBump();
				Vector2 triggerYRange = GetTriggerYRange();
				_.center += triggerOffset;
				_.size += new Vector3(triggerBump, 0f, triggerBump);
				if (float.IsNaN(triggerYRange.x) || float.IsNaN(triggerYRange.y) || triggerYRange.y <= triggerYRange.x)
				{
					_.size += Vector3.up * 5f;
				}
				else
				{
					_.size = new Vector3(_.size.x, triggerYRange.y - triggerYRange.x, _.size.z);
					_.center = new Vector3(_.center.x, (triggerYRange.y + triggerYRange.x) * 0.5f, _.center.z);
				}
			});
		}
	}

	public void SetBackgroundTriggerSettings(BackgroundTriggerSettings settings)
	{
		BackgroundTriggerSettings = settings;
	}

	public void CacheBoxColliders()
	{
		BoxColliders.Clear();
		BoxColliders = GetComponents<BoxCollider>().ToList();
		if (BoxColliders.Count == 0)
		{
			BoxColliderMaxY = 1f;
			return;
		}
		BoxCollider boxCollider = BoxColliders[0];
		BoxColliderMaxY = boxCollider.transform.TransformPoint(boxCollider.center).y + boxCollider.size.y * 0.5f;
	}

	protected virtual Vector3 GetTriggerOffset()
	{
		return Vector3.zero;
	}

	protected virtual float GetTriggerBump()
	{
		return 0f;
	}

	protected virtual Vector2 GetTriggerYRange()
	{
		return new Vector2(float.NaN, float.NaN);
	}
}
