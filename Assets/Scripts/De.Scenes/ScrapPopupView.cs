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

	public Button Background => background;

	public GameObject Content => content;

	public GameObject Body => body;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => draggingGoodsSlot;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter MyInventory => myInventory;

	public StashPanelPresenter StashPanel => stashPanel;

	public GameObject Right => right;

	public ScrapPanelPresenter ScrapPanel => scrapPanel;

	public GameObject KeyGuide => keyGuide;

	public GameObject KeyGuide_Escape => keyGuide_Escape;

	public GameObject KeyGuide_LShift => keyGuide_LShift;

	public GameObject KeyGuide_QuickSubmit => keyGuide_QuickSubmit;

	public GameObject KeyGuide_Select => keyGuide_Select;

	public GameObject Top => top;

	public GameObject GoldGroup => goldGroup;

	public GameObject Gold => gold;

	public TextMeshProUGUI Gold_Text => gold_Text;
}
