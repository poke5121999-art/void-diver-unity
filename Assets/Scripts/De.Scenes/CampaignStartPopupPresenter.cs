using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;

public class CampaignStartPopupPresenter : GamePopupPresenter
{
	[View]
	private CampaignStartPopupView _view;

	[InjectModel]
	private CampaignStartPopupModel _model;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "CampaignStart";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		TCampaign tCampaign = TCampaign.Table[_model.CampaignId];
		_view.CampaignName.text = tCampaign.Name;
		_view.CategoryText.text = tCampaign.GetChapterString();
		_view.DifficultyText.text = _model.Difficulty.ToColoredLocalText();
	}

	private void OnAnimationEnd()
	{
	}
}
