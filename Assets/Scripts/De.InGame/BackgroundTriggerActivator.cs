using System.Collections.Generic;
using UnityEngine;

public class BackgroundTriggerActivator : MonoBehaviour
{
	protected List<BackgroundTriggerUnit> TriggerUnits;

	protected virtual void Start()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}

	protected virtual void UpdateSomething()
	{
	}
}
