using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;

public class ArtifactDealPanelModel : BasePanelModel
{
	public Subject<Goods> OnUpdateGoods = new Subject<Goods>();

	public Subject<List<EArtifactCategory>> OnUpdatePreferCategories = new Subject<List<EArtifactCategory>>();

	public Subject<List<int>> OnUpdatePreferPrefixIds = new Subject<List<int>>();

	public Subject<List<int>> OnUpdateDislikePrefixIds = new Subject<List<int>>();
}
