using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class RadioToastPresenter : BaseToastPresenter
{
	[InjectModel]
	private RadioToastModel _model;

	[View]
	private RadioToastView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override void OnShow()
	{
	}

	private void OnAnimationCompleted()
	{
	}
}
