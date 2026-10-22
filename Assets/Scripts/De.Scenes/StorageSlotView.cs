using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private GoodsSlotPresenter goodsSlot;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI ownedQuantity;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => button;

	public Image Normal => normal;

	public GoodsSlotPresenter GoodsSlot => goodsSlot;

	public Image Line => line;

	public TextMeshProUGUI OwnedQuantity => ownedQuantity;

	public TextMeshProUGUI Text => text;
}
