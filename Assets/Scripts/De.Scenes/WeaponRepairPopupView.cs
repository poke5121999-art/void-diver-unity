using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponRepairPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GoodsSlotPresenter draggingSlot;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerInventoryPanelPresenter myInventory;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private Button btnAllFRepair;

	[SerializeField]
	private GameObject btnAllFRepair_Disabled;

	[SerializeField]
	private Image btnAllFRepair_Normal;

	[SerializeField]
	private GameObject btnAllFRepair_Gold;

	[SerializeField]
	private TextMeshProUGUI btnAllFRepair_Gold_Value;

	[SerializeField]
	private Image btnAllFRepair_Line;

	[SerializeField]
	private TextMeshProUGUI btnAllFRepair_TextTitle;

	[SerializeField]
	private WeaponRepairPanelPresenter weaponRepairPanel;

	public Button Background => null;

	public GameObject Content => null;

	public GameObject Body => null;

	public GoodsSlotPresenter DraggingSlot => null;

	public GameObject Left => null;

	public PlayerInventoryPanelPresenter MyInventory => null;

	public GameObject Right => null;

	public Button BtnAllFRepair => null;

	public GameObject BtnAllFRepair_Disabled => null;

	public Image BtnAllFRepair_Normal => null;

	public GameObject BtnAllFRepair_Gold => null;

	public TextMeshProUGUI BtnAllFRepair_Gold_Value => null;

	public Image BtnAllFRepair_Line => null;

	public TextMeshProUGUI BtnAllFRepair_TextTitle => null;

	public WeaponRepairPanelPresenter WeaponRepairPanel => null;
}
