using NemoLib.Mvp;
using UnityEngine;

public class ArchivePageView : View
{
	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject category;

	[SerializeField]
	private GameObject category_Scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private GameObject list;

	[SerializeField]
	private GameObject list_Scroller;

	public GameObject Left => left;

	public GameObject Category => category;

	public GameObject Category_Scroller => category_Scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public GameObject List => list;

	public GameObject List_Scroller => list_Scroller;
}
