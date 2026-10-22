using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class BgmPopupPresenter : GamePopupPresenter
{
	[View]
	private BgmPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	private BgmSlotScrollerModel _scrollerModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_gameManager.MyPlayer.RxProfile.Subscribe(OnProfileChanged).AddTo(this);
		_scrollerModel = new BgmSlotScrollerModel();
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_view.Scroller.GetComponent<BgmSlotScrollerPresenter>().Inject(_scrollerModel);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnProfileChanged(Profile profile)
	{
		_view.BgmName.text = TLoungeBgm.Table[profile.BgmId].Name;
	}

	private void OnSlotClick(BgmSlotModel model)
	{
		if (model.IsSelected)
		{
			_toastManager.NoticeFail(TLocalizedText.BgmAlreadySelected.LocalText);
		}
		else if (model.IsLocked)
		{
			_toastManager.NoticeFail(TLocalizedText.BgmLocked.LocalText);
		}
		else
		{
			_gameManager.MyPlayer.SetBgmId(model.BgmId);
		}
	}
}
