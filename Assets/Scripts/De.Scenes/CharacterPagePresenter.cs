using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class CharacterPagePresenter : BasePanelPresenter
{
	private enum ETab
	{
		ParadoxBuff = 0,
		Skill = 1
	}

	private enum EFocusPanel
	{
		None = 0,
		PositiveParadox = 1,
		NegativeParadox = 2,
		Buff = 3,
		Skill = 4
	}

	[InjectModel]
	private CharacterPageModel _model;

	[View]
	private CharacterPageView _view;

	[Inject]
	private GameManager _gameManager;

	private CompositeDisposable _disposable;

	private CharacterInfoPanelModel _characterInfoPanelModel;

	private ParadoxPanelModel _positiveParadoxModel;

	private ParadoxPanelModel _negativeParadoxModel;

	private MenuBuffPanelModel _buffPanelModel;

	private MenuSkillPanelModel _skillPanelModel;

	private EFocusPanel _focusedPanel;

	private ETab _currentTab;

	private InputActions.UiActions UiActions => default;

	protected override BasePanelModel PanelModel => null;

	private bool IsTabFocused => false;

	private Button SelectedTabButton => null;

	protected override void OnDestroy()
	{
	}

	private void OnInject(CharacterPageModel model)
	{
	}

	private void InitGamePadNav()
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

	private void InitCharacterInfoPanel()
	{
	}

	private void InitPositiveParadoxPanel()
	{
	}

	private void InitNegativeParadoxPanel()
	{
	}

	private void InitBuffPanel()
	{
	}

	private void InitSkillPanel()
	{
	}

	private void OutOfTop()
	{
	}

	private void OutOfBottom()
	{
	}

	private EFocusPanel GetNextPanelUp(EFocusPanel current)
	{
		return EFocusPanel.None;
	}

	private EFocusPanel GetNextPanelDown(EFocusPanel current)
	{
		return EFocusPanel.None;
	}

	private void Close()
	{
	}

	private void SelectTab(ETab tab)
	{
	}

	private void SetPanelInputEnabled(EFocusPanel panel, ENavDir direction = ENavDir.Down)
	{
	}

	private BasePanelModel GetPanelModel(EFocusPanel panel)
	{
		return null;
	}

	private void OnGoodsSlotEntered(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnGoodsSlotExit(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnBuffSlotEnter(MenuBuffSlotModel slotModel)
	{
	}

	private void OnBuffSlotExit(MenuBuffSlotModel slotModel)
	{
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
	}
}
