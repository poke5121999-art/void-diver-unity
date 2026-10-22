using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeFirePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject btnGroup;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Btn;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Btn;

	[SerializeField]
	private EmployeeCardPresenter employeeInfoCard;

	[SerializeField]
	private GameObject employeeSpine;

	[SerializeField]
	private SkeletonGraphic employeeSpine_Skeleton;

	[SerializeField]
	private GameObject employeeSpine_TextBubble;

	[SerializeField]
	private Image employeeSpine_TextBubble_BG;

	[SerializeField]
	private Image employeeSpine_TextBubble_Tail;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_TextBubble_Text;

	[SerializeField]
	private TextMeshProUGUI employeeSpine_TextBubble_TextPanic;

	public GameObject Contents => contents;

	public GameObject BtnGroup => btnGroup;

	public GameObject Cancel => cancel;

	public Button Cancel_Btn => cancel_Btn;

	public GameObject Confirm => confirm;

	public Button Confirm_Btn => confirm_Btn;

	public EmployeeCardPresenter EmployeeInfoCard => employeeInfoCard;

	public GameObject EmployeeSpine => employeeSpine;

	public SkeletonGraphic EmployeeSpine_Skeleton => employeeSpine_Skeleton;

	public GameObject EmployeeSpine_TextBubble => employeeSpine_TextBubble;

	public Image EmployeeSpine_TextBubble_BG => employeeSpine_TextBubble_BG;

	public Image EmployeeSpine_TextBubble_Tail => employeeSpine_TextBubble_Tail;

	public TextMeshProUGUI EmployeeSpine_TextBubble_Text => employeeSpine_TextBubble_Text;

	public TextMeshProUGUI EmployeeSpine_TextBubble_TextPanic => employeeSpine_TextBubble_TextPanic;
}
