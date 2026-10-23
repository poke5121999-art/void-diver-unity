using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class InteriorPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject area;

	[SerializeField]
	private GameObject area_Scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private GameObject interior;

	[SerializeField]
	private GameObject interior_Scroller;

	[SerializeField]
	private GameObject top;

	public Button Background => background;

	public GameObject Left => left;

	public GameObject Area => area;

	public GameObject Area_Scroller => area_Scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public GameObject Interior => interior;

	public GameObject Interior_Scroller => interior_Scroller;

	public GameObject Top => top;
}
