using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;

public class NewContentPopupPresenter : GamePopupPresenter
{
	[View]
	private NewContentPopupView _view;

	[InjectModel]
	private NewContentPopupModel _model;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "NPC_Open";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Text.text = TNpc.Table[_model.NpcId].ToastText;
		_view.Background.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
		}
	}

	private void OnAnimationEnd()
	{
		Close();
	}
}
