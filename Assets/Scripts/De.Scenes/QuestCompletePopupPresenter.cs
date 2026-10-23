using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;

public class QuestCompletePopupPresenter : GamePopupPresenter
{
	[View]
	private QuestCompletePopupView _view;

	[InjectModel]
	private QuestCompletePopupModel _model;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => "QuestSuccess";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Title.text = TCampaign.Table[_model.CampaignId].Name;
	}

	private void OnAnimationEnd()
	{
		Close();
	}
}
