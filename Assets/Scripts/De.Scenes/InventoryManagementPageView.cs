using NemoLib.Mvp;
using UnityEngine;

public class InventoryManagementPageView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject center;

	[SerializeField]
	private BagPanelPresenter bagPanel;

	[SerializeField]
	private GameObject inventoryKeyGuide;

	[SerializeField]
	private GameObject dropHalf;

	[SerializeField]
	private GameObject insertHalf;

	[SerializeField]
	private GameObject moveToStashAllKeyboard;

	[SerializeField]
	private GameObject moveToStashAllPad;

	[SerializeField]
	private QuickSlotSettingPanelPresenter quickSlotSettingPanel;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerInventoryPanelPresenter myInventory;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private StashPanelPresenter itemStashPanel;

	[SerializeField]
	private LootingInventoryPanelPresenter lootingInventory;

	[SerializeField]
	private GameObject currencyLayout;

	[SerializeField]
	private DraggingGoodsSlotPresenter draggingGoodsSlot;

	public GameObject Content => null;

	public GameObject Body => null;

	public GameObject Center => null;

	public BagPanelPresenter BagPanel => null;

	public GameObject InventoryKeyGuide => null;

	public GameObject DropHalf => null;

	public GameObject InsertHalf => null;

	public GameObject MoveToStashAllKeyboard => null;

	public GameObject MoveToStashAllPad => null;

	public QuickSlotSettingPanelPresenter QuickSlotSettingPanel => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter MyInventory => null;

	public GameObject Right => null;

	public StashPanelPresenter ItemStashPanel => null;

	public LootingInventoryPanelPresenter LootingInventory => null;

	public GameObject CurrencyLayout => null;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => null;
}
