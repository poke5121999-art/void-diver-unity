using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoodsQuantityPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TextMeshProUGUI message;

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
	private GameObject itemSlot;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private Image gold_Icon;

	[SerializeField]
	private TextMeshProUGUI gold_Text;

	[SerializeField]
	private TextMeshProUGUI goodsName;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject quantity;

	[SerializeField]
	private Button decreaseButton;

	[SerializeField]
	private Button increaseButton;

	[SerializeField]
	private TextMeshProUGUI quantityValue;

	[SerializeField]
	private Slider slider;

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public TextMeshProUGUI Message => message;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject ItemSlot => itemSlot;

	public GameObject Gold => gold;

	public Image Gold_Icon => gold_Icon;

	public TextMeshProUGUI Gold_Text => gold_Text;

	public TextMeshProUGUI GoodsName => goodsName;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject Quantity => quantity;

	public Button DecreaseButton => decreaseButton;

	public Button IncreaseButton => increaseButton;

	public TextMeshProUGUI QuantityValue => quantityValue;

	public Slider Slider => slider;
}
