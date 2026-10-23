using Cysharp.Threading.Tasks;
using NemoLib.Mvp;

public class EscapeFailPopupPresenter : GamePopupPresenter
{
	[View]
	private EscapeFailPopupView _view;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => "EscapeFail";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
	}

	private void OnAnimationEnd()
	{
		Close();
	}
}
