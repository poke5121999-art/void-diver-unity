using GameCommon.Table;
using NemoLib.Mvp;

public class BuffTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private BuffTooltipModel _model;

	[View]
	private BuffTooltipView _view;

	protected override BasePanelModel PanelModel => _model;

	public int BuffId => _model.BuffId;

	private void OnInject(BuffTooltipModel model)
	{
		TBuff tBuff = TBuff.Table[model.BuffId];
		_view.Title_Name.text = tBuff.Name;
		_view.Description_Text.text = tBuff.Desc;
	}
}
