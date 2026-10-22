using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerUnit : MonoBehaviour
{
	private bool _triggerEnable = true;

	private readonly HashSet<Collider> _activeTriggers = new HashSet<Collider>();

	public bool TriggerEnable => _triggerEnable;

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<BackgroundTriggerActivator>() != null)
		{
			_activeTriggers.Add(other);
			if (BackgroundTriggerManager.Instance != null)
			{
				BackgroundTriggerManager.Instance.NotifyEnter(other, this);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (_activeTriggers.Remove(other) && BackgroundTriggerManager.Instance != null)
		{
			BackgroundTriggerManager.Instance.NotifyExit(other, this);
		}
	}

	private void OnDisable()
	{
		foreach (Collider activeTrigger in _activeTriggers)
		{
			if (BackgroundTriggerManager.Instance != null)
			{
				BackgroundTriggerManager.Instance.NotifyExit(activeTrigger, this);
			}
		}
		_activeTriggers.Clear();
	}
}
