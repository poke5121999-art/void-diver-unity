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

	public BagTooltipPresenter Bag => bag;

	public EquipmentTooltipPresenter Equipment => equipment;

	public ItemTooltipPresenter Item => item;

	public GameObject KeyLayout => keyLayout;

	public GameObject KeyLayout_KeyGuideMoveToCrafting => keyLayout_KeyGuideMoveToCrafting;

	public GameObject KeyLayout_KeyGuideN => keyLayout_KeyGuideN;
}
