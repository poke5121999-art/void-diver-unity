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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter InventoryPanel => null;

	public GameObject PopupKeyGuide => null;

	public GameObject PopupKeyGuide_CtrlLMB => null;

	public GameObject PopupKeyGuide_CtrlRMB => null;

	public GameObject PopupKeyGuide_Esc => null;

	public GameObject PopupKeyGuide_LMB => null;

	public GameObject PopupKeyGuide_LShift => null;

	public GameObject PopupKeyGuide_RMB => null;

	public StashPanelPresenter StashPanel => null;

	public GameObject Right => null;

	public ShopProductPanelPresenter ShopProductPanel => null;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => null;

	public GameObject Top => null;

	public GameObject CurrencyLayout => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_Text => null;
}
