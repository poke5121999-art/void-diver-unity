using NemoLib.Mvp;
using UnityEngine;

public class CampaignListPanelView : View
{
	[SerializeField]
	private GameObject campaingListBackground;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject list;

	[SerializeField]
	private GameObject baseScroll;

	[SerializeField]
	private GameObject scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject cursor;

	public GameObject CampaingListBackground => campaingListBackground;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Left => left;

	public GameObject List => list;

	public GameObject BaseScroll => baseScroll;

	public GameObject Scroller => scroller;

	public GameObject Right => right;

	public GameObject Cursor => cursor;
}
