using NemoLib.Mvp;
using UnityEngine;

public class TycoonMenuPopupView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private CanvasGroup body;

	[SerializeField]
	private EmployeeHirePagePresenter employeeHirePage;

	[SerializeField]
	private EmployeeInfoPagePresenter employeeInfoPage;

	[SerializeField]
	private TycoonMainPagePresenter tycoonMainPage;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private GameObject[] top_Tabs;

	public GameObject Content => content;

	public CanvasGroup Body => body;

	public EmployeeHirePagePresenter EmployeeHirePage => employeeHirePage;

	public EmployeeInfoPagePresenter EmployeeInfoPage => employeeInfoPage;

	public TycoonMainPagePresenter TycoonMainPage => tycoonMainPage;

	public GameObject Top => top;

	public GameObject[] Top_Tabs => top_Tabs;
}
