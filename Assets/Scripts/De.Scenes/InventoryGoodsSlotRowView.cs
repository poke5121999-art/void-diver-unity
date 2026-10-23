using NemoLib.Mvp;
using UnityEngine;

public class InventoryGoodsSlotRowView : View
{
	[SerializeField]
	private GameObject[] slots;

	public GameObject[] Slots => slots;
}
