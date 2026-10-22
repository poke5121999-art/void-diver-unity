using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeInfoPageView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject employeeSpine;

	[SerializeField]
	private Image employeeSpine_ImgFloor;

	[SerializeField]
	private Image employeeSpine_Shadow;

	[SerializeField]
	private SkeletonGraphic employeeSpine_Skeleton;

	[SerializeField]
	private GameObject employeeSpine_TextBubble;

	[SerializeField]
	private EmployeeScrollerPresenter scrollView;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI caption;

	[SerializeField]
	private TextMeshProUGUI countText;

	public GameObject Content => null;

	public GameObject EmployeeSpine => null;

	public Image EmployeeSpine_ImgFloor => null;

	public Image EmployeeSpine_Shadow => null;

	public SkeletonGraphic EmployeeSpine_Skeleton => null;

	public GameObject EmployeeSpine_TextBubble => null;

	public EmployeeScrollerPresenter ScrollView => null;

	public GameObject Top => null;

	public Image BG => null;

	public TextMeshProUGUI Caption => null;

	public TextMeshProUGUI CountText => null;
}
