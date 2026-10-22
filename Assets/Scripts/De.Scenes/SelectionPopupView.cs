using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject pivot;

	[SerializeField]
	private SelectionSlotScrollerPresenter scroller;

	public Button Background => null;

	public GameObject Pivot => null;

	public SelectionSlotScrollerPresenter Scroller => null;
}
