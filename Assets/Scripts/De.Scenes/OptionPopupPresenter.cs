using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;

public class OptionPopupPresenter : GamePopupPresenter
{
	[View]
	private OptionPopupView _view;

	private OptionPageModel _pageModel;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_pageModel = new OptionPageModel();
		_pageModel.OnEscape.Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.OptionPage.Inject(_pageModel);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		_pageModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_pageModel.RxIsInputEnabled.Value = false;
	}
}
