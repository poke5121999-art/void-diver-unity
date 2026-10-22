using NemoLib.Mvp;

public class CampaignFailPopupPresenter : GamePopupPresenter
{
	[View]
	private CampaignFailPopupView _view;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => "CampaignFail";

	private void OnAnimationEnd()
	{
		Close();
	}
}
