using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuffPanelView : View
{
	[SerializeField]
	private EmptyPanelPresenter emptyPanel;

	[SerializeField]
	private Image list;

	[SerializeField]
	private Image baseScroll;

	[SerializeField]
	private MenuBuffScrollerPresenter scroller;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image bG;

	public EmptyPanelPresenter EmptyPanel => null;

	public Image List => null;

	public Image BaseScroll => null;

	public MenuBuffScrollerPresenter Scroller => null;

	public GameObject Top => null;

	public Image BG => null;
}
