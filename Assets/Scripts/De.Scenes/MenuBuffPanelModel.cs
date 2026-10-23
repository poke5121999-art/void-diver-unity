using UniRx;

public class MenuBuffPanelModel : BasePanelModel
{
	public readonly Subject<MenuBuffSlotModel> OnSlotEnter = new Subject<MenuBuffSlotModel>();

	public readonly Subject<MenuBuffSlotModel> OnSlotExit = new Subject<MenuBuffSlotModel>();
}
