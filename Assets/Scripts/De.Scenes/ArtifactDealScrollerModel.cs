using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class ArtifactDealScrollerModel : BaseScrollerModel<ArtifactDealSlotModel>
{
	public readonly Subject<Goods> OnUpdateGoods;

	public readonly Subject<List<EArtifactCategory>> OnUpdatePreferCategories;

	public readonly Subject<List<int>> OnUpdatePreferPrefixIds;

	public readonly Subject<List<int>> OnUpdateDislikePrefixIds;
}
