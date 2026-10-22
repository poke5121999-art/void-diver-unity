using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingGoodsSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject available;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI ownedQuantity;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => button;

	public Image Normal => normal;

	public GameObject Available => available;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public Image Line => line;

	public TextMeshProUGUI OwnedQuantity => ownedQuantity;

	public TextMeshProUGUI Text => text;
}
