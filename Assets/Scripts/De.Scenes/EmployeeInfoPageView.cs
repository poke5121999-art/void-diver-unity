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

	public GameObject Content => content;

	public GameObject EmployeeSpine => employeeSpine;

	public Image EmployeeSpine_ImgFloor => employeeSpine_ImgFloor;

	public Image EmployeeSpine_Shadow => employeeSpine_Shadow;

	public SkeletonGraphic EmployeeSpine_Skeleton => employeeSpine_Skeleton;

	public GameObject EmployeeSpine_TextBubble => employeeSpine_TextBubble;

	public EmployeeScrollerPresenter ScrollView => scrollView;

	public GameObject Top => top;

	public Image BG => bG;

	public TextMeshProUGUI Caption => caption;

	public TextMeshProUGUI CountText => countText;
}
