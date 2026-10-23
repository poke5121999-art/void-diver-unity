using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class HelpPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private Button nextPageButton;

	[SerializeField]
	private GameObject[] pages;

	[SerializeField]
	private GameObject pageSlotAnchor;

	[SerializeField]
	private GameObject[] pageSlots;

	[SerializeField]
	private Button prevPageButton;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private CanvasGroup top_TitleGroup;

	public Button Background => background;

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public Button NextPageButton => nextPageButton;

	public GameObject[] Pages => pages;

	public GameObject PageSlotAnchor => pageSlotAnchor;

	public GameObject[] PageSlots => pageSlots;

	public Button PrevPageButton => prevPageButton;

	public GameObject Top => top;

	public CanvasGroup Top_TitleGroup => top_TitleGroup;
}
