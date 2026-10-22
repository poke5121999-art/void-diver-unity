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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Crafting => crafting;

	public Image BG => bG;

	public GameObject Material => material;

	public CraftingMaterialPanelPresenter CraftingMaterialPanel => craftingMaterialPanel;

	public GameObject Quantity => quantity;

	public Button Quantity_DecreaseButton => quantity_DecreaseButton;

	public Button Quantity_IncreaseButton => quantity_IncreaseButton;

	public Slider Quantity_Slider => quantity_Slider;

	public TextMeshProUGUI Quantity_Value => quantity_Value;

	public Image Quantity_ValueBg => quantity_ValueBg;

	public GameObject CraftingItem => craftingItem;

	public InventoryGoodsSlotPresenter CraftingItem_GoodsSlot => craftingItem_GoodsSlot;

	public GameObject CraftingItem_VerticalLayout => craftingItem_VerticalLayout;

	public TextMeshProUGUI CraftingItem_Name => craftingItem_Name;
}
