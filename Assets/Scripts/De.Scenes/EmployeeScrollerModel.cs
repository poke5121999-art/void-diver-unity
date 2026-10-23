using GameCommon.Enum;
using UniRx;

public class EmployeeScrollerModel : BaseScrollerModel<EmployeeCardModel>
{
	public EEmployeePopupMode Mode;

	public readonly Subject<EmployeeCardModel> OnCleansingClick = new Subject<EmployeeCardModel>();

	public readonly Subject<EmployeeCardModel> OnFireClick = new Subject<EmployeeCardModel>();
}
