using GameCommon.Enum;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class ArchivePagePresenter : BasePanelPresenter
{
	[InjectModel]
	private ArchivePageModel _model;

	[View]
	private ArchivePageView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameInputManager _inputManager;

	private ArchiveScrollerModel _archiveScrollerModel;

	private TextSlotScrollerModel _textSlotScrollerModel;

	private CompositeDisposable _disposable;

	private EQuestType[] _questTypes;

	private EThemeType[] _themeTypes;

	private EGoodsType[] _itemSubTypes;

	private EWeaponType[] _weaponTypes;

	private EGoodsGradeType[] _accessoryGradeTypes;

	private EGoodsGradeType[] _artifactGradeTypes;

	private EArchiveRecordCategory[] _archiveRecordCategories;

	private EGlossaryCategory[] _glossaryCategories;

	private ArchiveSlotModel _selectedArchiveSlot;

	private ArchiveSlotModel _focusedArchiveSlot;

	protected override BasePanelModel PanelModel => null;

	private InputActions.UiActions UiActions => default;

	private bool HasSelectedSlot => false;

	protected override void Awake()
	{
	}

	private void OnInject(ArchivePageModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void OnDisable()
	{
	}

	private void Close()
	{
	}

	private void OnBack()
	{
	}

	private void OnArchiveSlotClick(ArchiveSlotModel slotModel)
	{
	}

	private void OnArchiveSlotEnter(ArchiveSlotModel slotModel)
	{
	}

	private void OnArchiveSlotExit(ArchiveSlotModel slotModel)
	{
	}

	private void UpdateSubLists(EArchiveCategory category)
	{
	}

	private void OnTextSlotClick(TextSlotModel slotModel)
	{
	}
}
