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

	public EmptyPanelPresenter EmptyPanel => emptyPanel;

	public Image List => list;

	public Image BaseScroll => baseScroll;

	public MenuBuffScrollerPresenter Scroller => scroller;

	public GameObject Top => top;

	public Image BG => bG;
}
