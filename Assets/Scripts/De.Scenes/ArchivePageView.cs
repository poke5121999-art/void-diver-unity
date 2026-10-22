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

	public GameObject Left => null;

	public GameObject Category => null;

	public GameObject Category_Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public GameObject List => null;

	public GameObject List_Scroller => null;
}
