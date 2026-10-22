using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRepairPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private Button confirm;

	[SerializeField]
	private Image confirm_Normal;

	[SerializeField]
	private GameObject confirm_Gold;

	[SerializeField]
	private TextMeshProUGUI confirm_Gold_Value;

	[SerializeField]
	private TextMeshProUGUI confirm_Text;

	[SerializeField]
	private Button locked;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GoodsTooltipPresenter goodsTooltip;

	public GameObject Content => null;

	public GameObject Body => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public GameObject Buttons => null;

	public Button Confirm => null;

	public Image Confirm_Normal => null;

	public GameObject Confirm_Gold => null;

	public TextMeshProUGUI Confirm_Gold_Value => null;

	public TextMeshProUGUI Confirm_Text => null;

	public Button Locked => null;

	public GameObject Top => null;

	public GoodsTooltipPresenter GoodsTooltip => null;
}
