using NemoLib.Mvp;
using UnityEngine;

public class SafeInventoryPanelView : View
{
	[SerializeField]
	private GameObject[] safeSlots;

	public GameObject[] SafeSlots => safeSlots;
}
