using NemoLib.Mvp;

public class FadePopupPresenter : GamePopupPresenter
{
	[View]
	private FadePopupView _view;

	[InjectModel]
	private FadePopupModel _model;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	private void OnAnimationEnd()
	{
	}
}
