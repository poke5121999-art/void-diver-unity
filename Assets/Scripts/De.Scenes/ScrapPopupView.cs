using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrapPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private DraggingGoodsSlotPresenter draggingGoodsSlot;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerInventoryPanelPresenter myInventory;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private ScrapPanelPresenter scrapPanel;

	[SerializeField]
	private GameObject keyGuide;

	[SerializeField]
	private GameObject keyGuide_Escape;

	[SerializeField]
	private GameObject keyGuide_LShift;

	[SerializeField]
	private GameObject keyGuide_QuickSubmit;

	[SerializeField]
	private GameObject keyGuide_Select;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject goldGroup;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private TextMeshProUGUI gold_Text;

	public Button Background => null;

	public GameObject Content => null;

	public GameObject Body => null;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter MyInventory => null;

	public StashPanelPresenter StashPanel => null;

	public GameObject Right => null;

	public ScrapPanelPresenter ScrapPanel => null;

	public GameObject KeyGuide => null;

	public GameObject KeyGuide_Escape => null;

	public GameObject KeyGuide_LShift => null;

	public GameObject KeyGuide_QuickSubmit => null;

	public GameObject KeyGuide_Select => null;

	public GameObject Top => null;

	public GameObject GoldGroup => null;

	public GameObject Gold => null;

	public TextMeshProUGUI Gold_Text => null;
}
