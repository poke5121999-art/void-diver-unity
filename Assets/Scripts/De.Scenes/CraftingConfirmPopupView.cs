using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingConfirmPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GameObject crafting;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private GameObject material;

	[SerializeField]
	private CraftingMaterialPanelPresenter craftingMaterialPanel;

	[SerializeField]
	private GameObject quantity;

	[SerializeField]
	private Button quantity_DecreaseButton;

	[SerializeField]
	private Button quantity_IncreaseButton;

	[SerializeField]
	private Slider quantity_Slider;

	[SerializeField]
	private TextMeshProUGUI quantity_Value;

	[SerializeField]
	private Image quantity_ValueBg;

	[SerializeField]
	private GameObject craftingItem;

	[SerializeField]
	private InventoryGoodsSlotPresenter craftingItem_GoodsSlot;

	[SerializeField]
	private GameObject craftingItem_VerticalLayout;

	[SerializeField]
	private TextMeshProUGUI craftingItem_Name;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Crafting => null;

	public Image BG => null;

	public GameObject Material => null;

	public CraftingMaterialPanelPresenter CraftingMaterialPanel => null;

	public GameObject Quantity => null;

	public Button Quantity_DecreaseButton => null;

	public Button Quantity_IncreaseButton => null;

	public Slider Quantity_Slider => null;

	public TextMeshProUGUI Quantity_Value => null;

	public Image Quantity_ValueBg => null;

	public GameObject CraftingItem => null;

	public InventoryGoodsSlotPresenter CraftingItem_GoodsSlot => null;

	public GameObject CraftingItem_VerticalLayout => null;

	public TextMeshProUGUI CraftingItem_Name => null;
}
