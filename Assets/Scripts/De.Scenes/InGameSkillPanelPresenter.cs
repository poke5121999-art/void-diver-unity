using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class InGameSkillPanelPresenter : Presenter
{
	[View]
	private InGameSkillPanelView _view;

	[Model]
	private InGameSkillPanelModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private PlayerManager _playerManager;

	private CompositeDisposable _disposable;

	private CharacterModel CharacterModel => null;

	private void OnDestroy()
	{
	}

	private void Start()
	{
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
	}

	private void RefreshSkillSlots()
	{
	}

	private void OnSkillSlotPointEnter(InGameSkillSlotModel model)
	{
	}

	private void OnSkillSlotPointExit(InGameSkillSlotModel model)
	{
	}

	private void OnSkillSlotPointDown(InGameSkillSlotModel model)
	{
	}
}
