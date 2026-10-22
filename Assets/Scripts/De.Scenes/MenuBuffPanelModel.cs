using UniRx;

public class MenuBuffPanelModel : BasePanelModel
{
	public readonly Subject<MenuBuffSlotModel> OnSlotEnter;

	public readonly Subject<MenuBuffSlotModel> OnSlotExit;
}
