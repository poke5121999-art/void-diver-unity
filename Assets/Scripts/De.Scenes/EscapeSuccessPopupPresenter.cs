using Cysharp.Threading.Tasks;
using NemoLib.Mvp;

public class EscapeSuccessPopupPresenter : GamePopupPresenter
{
	[View]
	private EscapeSuccessPopupView _view;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => "EscapeSuccess";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
	}

	private void OnAnimationEnd()
	{
		Close();
	}
}
