using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class ShopPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerInventoryPanelPresenter inventoryPanel;

	[SerializeField]
	private GameObject popupKeyGuide;

	[SerializeField]
	private GameObject popupKeyGuide_CtrlLMB;

	[SerializeField]
	private GameObject popupKeyGuide_CtrlRMB;

	[SerializeField]
	private GameObject popupKeyGuide_Esc;

	[SerializeField]
	private GameObject popupKeyGuide_LMB;

	[SerializeField]
	private GameObject popupKeyGuide_LShift;

	[SerializeField]
	private GameObject popupKeyGuide_RMB;

	[SerializeField]
	private StashPanelPresenter stashPanel;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private ShopProductPanelPresenter shopProductPanel;

	[SerializeField]
	private DraggingGoodsSlotPresenter draggingGoodsSlot;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject currencyLayout;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter InventoryPanel => inventoryPanel;

	public GameObject PopupKeyGuide => popupKeyGuide;

	public GameObject PopupKeyGuide_CtrlLMB => popupKeyGuide_CtrlLMB;

	public GameObject PopupKeyGuide_CtrlRMB => popupKeyGuide_CtrlRMB;

	public GameObject PopupKeyGuide_Esc => popupKeyGuide_Esc;

	public GameObject PopupKeyGuide_LMB => popupKeyGuide_LMB;

	public GameObject PopupKeyGuide_LShift => popupKeyGuide_LShift;

	public GameObject PopupKeyGuide_RMB => popupKeyGuide_RMB;

	public StashPanelPresenter StashPanel => stashPanel;

	public GameObject Right => right;

	public ShopProductPanelPresenter ShopProductPanel => shopProductPanel;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => draggingGoodsSlot;

	public GameObject Top => top;

	public GameObject CurrencyLayout => currencyLayout;

	public GameObject Title => title;

	public TextMeshProUGUI Title_Text => title_Text;
}
