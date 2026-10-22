using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;

public class OptionListPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private OptionListPopupModel _model;

	[View]
	private OptionListPopupView _view;

	private TextSlotScrollerModel _scrollerModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "PopUpOpen";

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Text.text = _model.Title;
		_scrollerModel = new TextSlotScrollerModel
		{
			Texts = _model.Options
		};
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		TextSlotScrollerPresenter component = _view.Scroller.GetComponent<TextSlotScrollerPresenter>();
		component.Inject(_scrollerModel);
		component.SetSelected(_model.SelectedIndex);
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

	private void OnSlotClick(TextSlotModel slotModel)
	{
		Close(slotModel.Index);
	}
}
