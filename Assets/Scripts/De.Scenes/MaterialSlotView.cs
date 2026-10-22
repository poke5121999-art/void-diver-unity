using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class MaterialSlotView : View
{
	[SerializeField]
	private GoodsSlotPresenter goodsSlot;

	[SerializeField]
	private TextMeshProUGUI textCount;

	public GoodsSlotPresenter GoodsSlot => goodsSlot;

	public TextMeshProUGUI TextCount => textCount;
}
