using NemoLib.Mvp;
using UniRx;

public class WeaponRepairPopupModel : Model
{
	public readonly Subject<InventoryGoodsSlotModel> OnSlotSelected;
}
