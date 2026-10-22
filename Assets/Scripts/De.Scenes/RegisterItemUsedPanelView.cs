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

	public GameObject Content => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GoodsSlotPresenter GoodsSlot => null;

	public GameObject Top => null;

	public TextMeshProUGUI TitleText => null;

	public GoodsTooltipPresenter GoodsTooltip => null;

	public TextMeshProUGUI GuideText => null;

	public TextMeshProUGUI InvalidText => null;
}
