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

	public Button Button => null;

	public Image Normal => null;

	public GameObject Coin => null;

	public Image Coin_Icon => null;

	public TextMeshProUGUI Coin_Value => null;

	public TextMeshProUGUI DayCount => null;

	public GameObject Gold => null;

	public Image Gold_Icon => null;

	public TextMeshProUGUI Gold_Value => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public Image Line => null;

	public TextMeshProUGUI Text => null;
}
