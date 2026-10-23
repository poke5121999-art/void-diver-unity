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

	public GameObject Content => content;

	public GameObject Body => body;

	public GameObject Center => center;

	public BagPanelPresenter BagPanel => bagPanel;

	public GameObject InventoryKeyGuide => inventoryKeyGuide;

	public GameObject DropHalf => dropHalf;

	public GameObject InsertHalf => insertHalf;

	public GameObject MoveToStashAllKeyboard => moveToStashAllKeyboard;

	public GameObject MoveToStashAllPad => moveToStashAllPad;

	public QuickSlotSettingPanelPresenter QuickSlotSettingPanel => quickSlotSettingPanel;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter MyInventory => myInventory;

	public GameObject Right => right;

	public StashPanelPresenter ItemStashPanel => itemStashPanel;

	public LootingInventoryPanelPresenter LootingInventory => lootingInventory;

	public GameObject CurrencyLayout => currencyLayout;

	public DraggingGoodsSlotPresenter DraggingGoodsSlot => draggingGoodsSlot;
}
