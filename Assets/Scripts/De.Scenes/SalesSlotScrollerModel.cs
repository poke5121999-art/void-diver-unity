using System.Collections.Generic;

public class SalesSlotScrollerModel : BaseScrollerModel<SalesSlotModel>
{
	public List<SalesSlotModel> SlotModels = new List<SalesSlotModel>();

	public int InitialSelectedId = -1;
}
