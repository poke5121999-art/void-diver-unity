using NemoLib.Mvp;

public class ParadoxTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private ParadoxTooltipModel _model;

	[View]
	private ParadoxTooltipView _view;

	protected override BasePanelModel PanelModel => null;

	private void OnInject(ParadoxTooltipModel model)
	{
	}
}
