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

	public GameObject Content => null;

	public CanvasGroup Body => null;

	public EmployeeHirePagePresenter EmployeeHirePage => null;

	public EmployeeInfoPagePresenter EmployeeInfoPage => null;

	public TycoonMainPagePresenter TycoonMainPage => null;

	public GameObject Top => null;

	public GameObject[] Top_Tabs => null;
}
