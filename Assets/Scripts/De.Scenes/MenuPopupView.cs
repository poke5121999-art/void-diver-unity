using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class MenuPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject[] pages;

	[SerializeField]
	private GameObject[] tabs;

	public GameObject Contents => contents;

	public Button Background => background;

	public GameObject[] Pages => pages;

	public GameObject[] Tabs => tabs;
}
