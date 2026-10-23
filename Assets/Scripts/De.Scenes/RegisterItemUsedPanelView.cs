using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterItemUsedPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	[SerializeField]
	private TextMeshProUGUI guideText;

	[SerializeField]
	private TextMeshProUGUI invalidText;

	public GameObject Content => content;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject Top => top;

	public TextMeshProUGUI TitleText => titleText;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;

	public TextMeshProUGUI GuideText => guideText;

	public TextMeshProUGUI InvalidText => invalidText;
}
