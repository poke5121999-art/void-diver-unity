using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeLevelUpPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject btnGroup;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Btn;

	[SerializeField]
	private EmployeeCardPresenter employeeCard;

	[SerializeField]
	private GameObject employeeSpine;

	[SerializeField]
	private SkeletonGraphic skeleton;

	[SerializeField]
	private GameObject textBubble;

	[SerializeField]
	private Image textBubble_BG;

	[SerializeField]
	private Image textBubble_Tail;

	[SerializeField]
	private TextMeshProUGUI textBubble_Text;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_TextMain;

	public GameObject Contents => null;

	public GameObject BtnGroup => null;

	public GameObject Confirm => null;

	public Button Confirm_Btn => null;

	public EmployeeCardPresenter EmployeeCard => null;

	public GameObject EmployeeSpine => null;

	public SkeletonGraphic Skeleton => null;

	public GameObject TextBubble => null;

	public Image TextBubble_BG => null;

	public Image TextBubble_Tail => null;

	public TextMeshProUGUI TextBubble_Text => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_TextMain => null;
}
