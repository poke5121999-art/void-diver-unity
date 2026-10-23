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

	public GameObject Content => content;

	public GameObject EmployeeSpine => employeeSpine;

	public SkeletonGraphic EmployeeSpine_Skeleton => employeeSpine_Skeleton;

	public GameObject EmployeeSpine_TextBubble => employeeSpine_TextBubble;

	public EmployeeScrollerPresenter ScrollView => scrollView;
}
