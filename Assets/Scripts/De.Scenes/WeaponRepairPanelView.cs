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

	public GameObject Content => content;

	public GameObject Body => body;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public GameObject Buttons => buttons;

	public Button Confirm => confirm;

	public Image Confirm_Normal => confirm_Normal;

	public GameObject Confirm_Gold => confirm_Gold;

	public TextMeshProUGUI Confirm_Gold_Value => confirm_Gold_Value;

	public TextMeshProUGUI Confirm_Text => confirm_Text;

	public Button Locked => locked;

	public GameObject Top => top;

	public GoodsTooltipPresenter GoodsTooltip => goodsTooltip;
}
