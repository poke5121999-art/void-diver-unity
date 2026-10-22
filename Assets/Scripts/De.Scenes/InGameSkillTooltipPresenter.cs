using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class InGameSkillTooltipPresenter : BasePanelPresenter
{
	[View]
	private InGameSkillTooltipView _view;

	[InjectModel]
	private InGameSkillTooltipModel _model;

	[Inject]
	private AssetManager _assetManager;

	protected override BasePanelModel PanelModel => null;

	protected void OnInject(InGameSkillTooltipModel model)
	{
	}
}
