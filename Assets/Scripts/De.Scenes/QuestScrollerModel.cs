public class QuestScrollerModel : BaseScrollerModel<QuestSlotModel>
{
	public readonly QuestScrollerPresenter.EFilterType FilterType;

	public QuestScrollerModel(QuestScrollerPresenter.EFilterType filterType)
	{
		FilterType = filterType;
	}
}
