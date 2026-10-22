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

	public Button Button => button;

	public Image Normal => normal;

	public GameObject Coin => coin;

	public Image Coin_Icon => coin_Icon;

	public TextMeshProUGUI Coin_Value => coin_Value;

	public GameObject Gold => gold;

	public Image Gold_Icon => gold_Icon;

	public TextMeshProUGUI Gold_Value => gold_Value;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject Item => item;

	public GameObject Item_Element => item_Element;

	public Image Item_Icon => item_Icon;

	public TextMeshProUGUI Item_Value => item_Value;

	public Image Line => line;

	public Image NewTag => newTag;

	public TextMeshProUGUI Text => text;

	public TextMeshProUGUI TextStock => textStock;
}
