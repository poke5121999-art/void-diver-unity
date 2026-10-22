using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerUnit : MonoBehaviour
{
	private bool _triggerEnable;

	private readonly HashSet<Collider> _activeTriggers;

	public bool TriggerEnable => false;

	private void OnTriggerEnter(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}

	private void OnDisable()
	{
	}
}
