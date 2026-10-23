using NemoLib.Mvp;
using UnityEngine;

public class KeySettingSlotRowView : View
{
	[SerializeField]
	private GameObject[] slots;

	public GameObject[] Slots => slots;
}
