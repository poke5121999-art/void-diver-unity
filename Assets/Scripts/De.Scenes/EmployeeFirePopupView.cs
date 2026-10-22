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

	public GameObject Contents => null;

	public GameObject BtnGroup => null;

	public GameObject Cancel => null;

	public Button Cancel_Btn => null;

	public GameObject Confirm => null;

	public Button Confirm_Btn => null;

	public EmployeeCardPresenter EmployeeInfoCard => null;

	public GameObject EmployeeSpine => null;

	public SkeletonGraphic EmployeeSpine_Skeleton => null;

	public GameObject EmployeeSpine_TextBubble => null;

	public Image EmployeeSpine_TextBubble_BG => null;

	public Image EmployeeSpine_TextBubble_Tail => null;

	public TextMeshProUGUI EmployeeSpine_TextBubble_Text => null;

	public TextMeshProUGUI EmployeeSpine_TextBubble_TextPanic => null;
}
