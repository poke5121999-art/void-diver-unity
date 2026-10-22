using GameCommon.Data;
using NemoLib.Mvp;

public class RepairConfirmPopupModel : Model
{
	public Goods Goods;

	public bool IsRepairAll;

	public int OverrideRepairCost;
}
