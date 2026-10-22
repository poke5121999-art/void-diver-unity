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

	public GameObject Contents => contents;

	public GameObject BtnGroup => btnGroup;

	public GameObject Confirm => confirm;

	public Button Confirm_Btn => confirm_Btn;

	public EmployeeCardPresenter EmployeeCard => employeeCard;

	public GameObject EmployeeSpine => employeeSpine;

	public SkeletonGraphic Skeleton => skeleton;

	public GameObject TextBubble => textBubble;

	public Image TextBubble_BG => textBubble_BG;

	public Image TextBubble_Tail => textBubble_Tail;

	public TextMeshProUGUI TextBubble_Text => textBubble_Text;

	public GameObject Title => title;

	public TextMeshProUGUI Title_TextMain => title_TextMain;
}
