using NemoLib.Mvp;
using Spine.Unity;
using UnityEngine;

public class EmployeeHirePageView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject employeeSpine;

	[SerializeField]
	private SkeletonGraphic employeeSpine_Skeleton;

	[SerializeField]
	private GameObject employeeSpine_TextBubble;

	[SerializeField]
	private EmployeeScrollerPresenter scrollView;

	public GameObject Content => null;

	public GameObject EmployeeSpine => null;

	public SkeletonGraphic EmployeeSpine_Skeleton => null;

	public GameObject EmployeeSpine_TextBubble => null;

	public EmployeeScrollerPresenter ScrollView => null;
}
