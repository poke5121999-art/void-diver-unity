using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopProductSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private Image coin_Icon;

	[SerializeField]
	private TextMeshProUGUI coin_Value;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private Image gold_Icon;

	[SerializeField]
	private TextMeshProUGUI gold_Value;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject item;

	[SerializeField]
	private GameObject item_Element;

	[SerializeField]
	private Image item_Icon;

	[SerializeField]
	private TextMeshProUGUI item_Value;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Image newTag;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private TextMeshProUGUI textStock;

	public Button Button => null;

	public Image Normal => null;

	public GameObject Coin => null;

	public Image Coin_Icon => null;

	public TextMeshProUGUI Coin_Value => null;

	public GameObject Gold => null;

	public Image Gold_Icon => null;

	public TextMeshProUGUI Gold_Value => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public GameObject Item => null;

	public GameObject Item_Element => null;

	public Image Item_Icon => null;

	public TextMeshProUGUI Item_Value => null;

	public Image Line => null;

	public Image NewTag => null;

	public TextMeshProUGUI Text => null;

	public TextMeshProUGUI TextStock => null;
}
