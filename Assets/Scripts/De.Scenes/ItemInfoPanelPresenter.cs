using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class ItemInfoPanelPresenter : BasePanelPresenter
{
	[View]
	private ItemInfoPanelView _view;

	[InjectModel]
	private ItemInfoPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	private const string ITEM_MAX_COLOR = "#a74200";

	protected override BasePanelModel PanelModel => null;

	private GamePlayer Player => null;

	private bool ShowNoteButtonActive => false;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(ItemInfoPanelModel model)
	{
	}

	private void UpdateItemUis()
	{
	}

	private void OnBackButtonClick()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void ShowNoteButtonClick()
	{
	}
}
