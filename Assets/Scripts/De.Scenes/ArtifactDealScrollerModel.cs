using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class ArtifactDealScrollerModel : BaseScrollerModel<ArtifactDealSlotModel>
{
	public readonly Subject<Goods> OnUpdateGoods = new Subject<Goods>();

	public readonly Subject<List<EArtifactCategory>> OnUpdatePreferCategories = new Subject<List<EArtifactCategory>>();

	public readonly Subject<List<int>> OnUpdatePreferPrefixIds = new Subject<List<int>>();

	public readonly Subject<List<int>> OnUpdateDislikePrefixIds = new Subject<List<int>>();
}
