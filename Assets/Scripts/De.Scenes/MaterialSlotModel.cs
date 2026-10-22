using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;

public class MaterialSlotModel : Model
{
	public Goods Goods;

	public bool IsUnknown;

	public bool IsCrafting;

	public Subject<Goods> OnCraftingNavigate;
}
