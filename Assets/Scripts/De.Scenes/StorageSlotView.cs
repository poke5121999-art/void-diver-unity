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

	public Button Button => null;

	public Image Normal => null;

	public GoodsSlotPresenter GoodsSlot => null;

	public Image Line => null;

	public TextMeshProUGUI OwnedQuantity => null;

	public TextMeshProUGUI Text => null;
}
