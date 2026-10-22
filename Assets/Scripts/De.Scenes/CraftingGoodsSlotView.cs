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

	public Button Button => null;

	public Image Normal => null;

	public GameObject Available => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public Image Line => null;

	public TextMeshProUGUI OwnedQuantity => null;

	public TextMeshProUGUI Text => null;
}
