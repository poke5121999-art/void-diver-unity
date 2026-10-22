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

	public Button Background => null;

	public GameObject Left => null;

	public GameObject Area => null;

	public GameObject Area_Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public GameObject Interior => null;

	public GameObject Interior_Scroller => null;

	public GameObject Top => null;
}
