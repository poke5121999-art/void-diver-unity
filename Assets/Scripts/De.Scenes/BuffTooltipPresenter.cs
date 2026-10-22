using NemoLib.Mvp;

public class BuffTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private BuffTooltipModel _model;

	[View]
	private BuffTooltipView _view;

	protected override BasePanelModel PanelModel => null;

	public int BuffId => 0;

	private void OnInject(BuffTooltipModel model)
	{
	}
}
