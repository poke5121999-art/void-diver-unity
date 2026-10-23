using NemoLib.Mvp;

public class FadePopupPresenter : GamePopupPresenter
{
	[View]
	private FadePopupView _view;

	[InjectModel]
	private FadePopupModel _model;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => _model.OpenSfx;

	private void OnAnimationEnd()
	{
		Close();
	}
}
