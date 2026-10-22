using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerActivator : MonoBehaviour
{
	protected List<BackgroundTriggerUnit> TriggerUnits = new List<BackgroundTriggerUnit>();

	protected virtual void Start()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		BackgroundTriggerUnit component = other.GetComponent<BackgroundTriggerUnit>();
		if (!component)
		{
			return;
		}
		TriggerUnits.Add(component);
		UpdateSomething();
	}

	private void OnTriggerExit(Collider other)
	{
		BackgroundTriggerUnit component = other.GetComponent<BackgroundTriggerUnit>();
		if ((bool)component)
		{
			TriggerUnits.Remove(component);
			UpdateSomething();
		}
	}

	protected virtual void UpdateSomething()
	{
	}
}
