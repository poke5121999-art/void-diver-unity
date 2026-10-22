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

	public Button Background => null;

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public Button NextPageButton => null;

	public GameObject[] Pages => null;

	public GameObject PageSlotAnchor => null;

	public GameObject[] PageSlots => null;

	public Button PrevPageButton => null;

	public GameObject Top => null;

	public CanvasGroup Top_TitleGroup => null;
}
