using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class ArtifactDealPanelModel : BasePanelModel
{
	public Subject<Goods> OnUpdateGoods;

	public Subject<List<EArtifactCategory>> OnUpdatePreferCategories;

	public Subject<List<int>> OnUpdatePreferPrefixIds;

	public Subject<List<int>> OnUpdateDislikePrefixIds;
}
