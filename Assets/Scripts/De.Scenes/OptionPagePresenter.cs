using NemoLib.Mvp;
using UniRx;
using VContainer;

public class OptionPagePresenter : BasePanelPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Graphic = 1,
		Sound = 2,
		GamePlay = 3,
		Key = 4,
		Pad = 5
	}

	[InjectModel]
	private OptionPageModel _model;

	[View]
	private OptionPageView _view;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private InputService _inputService;

	private CompositeDisposable _disposable;

	private OptionGraphicPanelModel _graphicModel;

	private OptionSoundPanelModel _soundModel;

	private OptionGamePlayPanelModel _gamePlayModel;

	private OptionKeyPanelModel _keyModel;

	private OptionPadPanelModel _padModel;

	private EFocusPanel _selectedPanel;

	private EFocusPanel _displayedPanel;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	protected override void OnDestroy()
	{
	}

	private void OnDisable()
	{
	}

	private void OnInject(OptionPageModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void InitTabButtons()
	{
	}

	private void OnTabButtonClick(EFocusPanel panel)
	{
	}

	private void OnTabButtonEnter(EFocusPanel panel)
	{
	}

	private void InitTabPanels()
	{
	}

	private void SetTab(EFocusPanel panel)
	{
	}

	private void SetPage(EFocusPanel panel)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel)
	{
	}

	private void Close()
	{
	}
}
