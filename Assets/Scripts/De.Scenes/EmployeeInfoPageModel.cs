using UniRx;

public class EmployeeInfoPageModel : BasePanelModel
{
	public readonly Subject<int> OnEmployeeClick = new Subject<int>();
}
