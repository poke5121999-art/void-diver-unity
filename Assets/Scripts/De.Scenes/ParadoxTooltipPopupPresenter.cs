using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;

public class ParadoxTooltipPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private ParadoxTooltipPopupModel _model;

	[View]
	private ParadoxTooltipPopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.ParadoxTooltip.Inject(new ParadoxTooltipModel
		{
			ParadoxId = _model.ParadoxId,
			IsLocked = _model.IsLocked
		});
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
	}
}
