using NemoLib.Mvp;
using UnityEngine;

public class GoodsTooltipView : View
{
	[SerializeField]
	private BagTooltipPresenter bag;

	[SerializeField]
	private EquipmentTooltipPresenter equipment;

	[SerializeField]
	private ItemTooltipPresenter item;

	[SerializeField]
	private GameObject keyLayout;

	[SerializeField]
	private GameObject keyLayout_KeyGuideMoveToCrafting;

	[SerializeField]
	private GameObject keyLayout_KeyGuideN;

	public BagTooltipPresenter Bag => null;

	public EquipmentTooltipPresenter Equipment => null;

	public ItemTooltipPresenter Item => null;

	public GameObject KeyLayout => null;

	public GameObject KeyLayout_KeyGuideMoveToCrafting => null;

	public GameObject KeyLayout_KeyGuideN => null;
}
