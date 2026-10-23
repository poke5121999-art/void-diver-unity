using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LostItemSlotView : View
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
	private TextMeshProUGUI dayCount;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private Image gold_Icon;

	[SerializeField]
	private TextMeshProUGUI gold_Value;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => button;

	public Image Normal => normal;

	public GameObject Coin => coin;

	public Image Coin_Icon => coin_Icon;

	public TextMeshProUGUI Coin_Value => coin_Value;

	public TextMeshProUGUI DayCount => dayCount;

	public GameObject Gold => gold;

	public Image Gold_Icon => gold_Icon;

	public TextMeshProUGUI Gold_Value => gold_Value;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public Image Line => line;

	public TextMeshProUGUI Text => text;
}
