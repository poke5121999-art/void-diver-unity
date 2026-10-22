using NemoLib.Mvp;
using UnityEngine;

public class MenuSkillPanelView : View
{
	[SerializeField]
	private GameObject[] slots;

	public GameObject[] Slots => slots;
}
