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

	public Button Background => background;

	public GameObject Content => content;

	public GameObject Body => body;

	public GoodsSlotPresenter DraggingSlot => draggingSlot;

	public GameObject Left => left;

	public PlayerInventoryPanelPresenter MyInventory => myInventory;

	public GameObject Right => right;

	public Button BtnAllFRepair => btnAllFRepair;

	public GameObject BtnAllFRepair_Disabled => btnAllFRepair_Disabled;

	public Image BtnAllFRepair_Normal => btnAllFRepair_Normal;

	public GameObject BtnAllFRepair_Gold => btnAllFRepair_Gold;

	public TextMeshProUGUI BtnAllFRepair_Gold_Value => btnAllFRepair_Gold_Value;

	public Image BtnAllFRepair_Line => btnAllFRepair_Line;

	public TextMeshProUGUI BtnAllFRepair_TextTitle => btnAllFRepair_TextTitle;

	public WeaponRepairPanelPresenter WeaponRepairPanel => weaponRepairPanel;
}
