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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public TextMeshProUGUI Message => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject ItemSlot => null;

	public GameObject Gold => null;

	public Image Gold_Icon => null;

	public TextMeshProUGUI Gold_Text => null;

	public TextMeshProUGUI GoodsName => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public GameObject Quantity => null;

	public Button DecreaseButton => null;

	public Button IncreaseButton => null;

	public TextMeshProUGUI QuantityValue => null;

	public Slider Slider => null;
}
