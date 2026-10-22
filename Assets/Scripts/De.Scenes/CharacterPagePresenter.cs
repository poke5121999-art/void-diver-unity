using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using UnityEngine;
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

	private InputActions.UiActions UiActions => InputManager.UiActions;

	protected override BasePanelModel PanelModel => _model;

	private bool IsTabFocused => _focusedPanel == EFocusPanel.None;

	private Button SelectedTabButton => (_currentTab == ETab.Skill) ? _view.SkillTalentTab_Button : _view.ParadoxBuffTab_Button;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(CharacterPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		InitCharacterInfoPanel();
		InitPositiveParadoxPanel();
		InitNegativeParadoxPanel();
		InitBuffPanel();
		InitSkillPanel();
		ClearNav();
		ConnectNav(_view.ParadoxBuffTab_Button, ENavDir.Right, _view.SkillTalentTab_Button);
		ConnectNav(_view.SkillTalentTab_Button, ENavDir.Left, _view.ParadoxBuffTab_Button);
		SetDefaultNav(_view.ParadoxBuffTab_Button);
		_model.OutOfBottom.Subscribe(delegate
		{
			OutOfBottom();
		}).AddTo(_disposable);
		_view.EquipmentTooltip.gameObject.SetActive(value: false);
		_view.ParadoxTooltip.gameObject.SetActive(value: false);
		_view.SkillTooltip.gameObject.SetActive(value: false);
		_view.BuffTooltip.gameObject.SetActive(value: false);
		_view.SkillTalentTab_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			SelectTab(ETab.Skill);
		}).AddTo(_disposable);
		_view.ParadoxBuffTab_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			SelectTab(ETab.ParadoxBuff);
		}).AddTo(_disposable);
		SelectTab(ETab.ParadoxBuff);
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		ConnectNav(_view.ParadoxBuffTab_Button, ENavDir.Right, _view.SkillTalentTab_Button);
		ConnectNav(_view.SkillTalentTab_Button, ENavDir.Left, _view.ParadoxBuffTab_Button);
		SetDefaultNav(_view.ParadoxBuffTab_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (InputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
			_view.EscapeKeyGuide.SetActive(value: true);
			_characterInfoPanelModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		_view.EscapeKeyGuide.SetActive(value: false);
		_characterInfoPanelModel.RxIsInputEnabled.Value = false;
		_positiveParadoxModel.RxIsInputEnabled.Value = false;
		_negativeParadoxModel.RxIsInputEnabled.Value = false;
		_buffPanelModel.RxIsInputEnabled.Value = false;
		_skillPanelModel.RxIsInputEnabled.Value = false;
	}

	private void OnDisable()
	{
		_disposable?.Dispose();
	}

	private void InitCharacterInfoPanel()
	{
		_characterInfoPanelModel = new CharacterInfoPanelModel();
		_characterInfoPanelModel.OnGoodsSlotEntered.Subscribe(OnGoodsSlotEntered).AddTo(_disposable);
		_characterInfoPanelModel.OnGoodsSlotExit.Subscribe(OnGoodsSlotExit).AddTo(_disposable);
		_view.CharacterInfoPanel.Inject(_characterInfoPanelModel);
	}

	private void InitPositiveParadoxPanel()
	{
		_positiveParadoxModel = new ParadoxPanelModel
		{
			Player = _gameManager.MyPlayer,
			IsPositive = true
		};
		_positiveParadoxModel.OnParadoxSlotEnter.Subscribe(OnParadoxSlotEnter).AddTo(_disposable);
		_positiveParadoxModel.OnParadoxSlotExit.Subscribe(OnParadoxSlotExit).AddTo(_disposable);
		_positiveParadoxModel.OutOfTop.Subscribe(delegate
		{
			OutOfTop();
		}).AddTo(_disposable);
		_positiveParadoxModel.OutOfBottom.Subscribe(delegate
		{
			OutOfBottom();
		}).AddTo(_disposable);
		_view.PositiveParadoxPanel.Inject(_positiveParadoxModel);
	}

	private void InitNegativeParadoxPanel()
	{
		_negativeParadoxModel = new ParadoxPanelModel
		{
			Player = _gameManager.MyPlayer,
			IsPositive = false
		};
		_negativeParadoxModel.OnParadoxSlotEnter.Subscribe(OnParadoxSlotEnter).AddTo(_disposable);
		_negativeParadoxModel.OnParadoxSlotExit.Subscribe(OnParadoxSlotExit).AddTo(_disposable);
		_negativeParadoxModel.OutOfTop.Subscribe(delegate
		{
			OutOfTop();
		}).AddTo(_disposable);
		_negativeParadoxModel.OutOfBottom.Subscribe(delegate
		{
			OutOfBottom();
		}).AddTo(_disposable);
		_view.NegativeParadoxPanel.Inject(_negativeParadoxModel);
	}

	private void InitBuffPanel()
	{
		_buffPanelModel = new MenuBuffPanelModel();
		_buffPanelModel.OnSlotEnter.Subscribe(OnBuffSlotEnter).AddTo(_disposable);
		_buffPanelModel.OnSlotExit.Subscribe(OnBuffSlotExit).AddTo(_disposable);
		_buffPanelModel.OutOfTop.Subscribe(delegate
		{
			OutOfTop();
		}).AddTo(_disposable);
		_buffPanelModel.OutOfBottom.Subscribe(delegate
		{
			OutOfBottom();
		}).AddTo(_disposable);
		_view.BuffPanel.Inject(_buffPanelModel);
	}

	private void InitSkillPanel()
	{
		_skillPanelModel = new MenuSkillPanelModel
		{
			ClientId = _gameManager.LocalClientId
		};
		_skillPanelModel.OnSlotEnter.Subscribe(OnSkillSlotEnter).AddTo(_disposable);
		_skillPanelModel.OnSlotExit.Subscribe(OnSkillSlotExit).AddTo(_disposable);
		_skillPanelModel.OutOfTop.Subscribe(delegate
		{
			OutOfTop();
		}).AddTo(_disposable);
		_view.Skill_MenuSkillPanel.Inject(_skillPanelModel);
	}

	private void OutOfTop()
	{
		switch (_focusedPanel)
		{
		case EFocusPanel.None:
			break;
		case EFocusPanel.PositiveParadox:
		case EFocusPanel.Skill:
			_focusedPanel = EFocusPanel.None;
			_positiveParadoxModel.RxIsInputEnabled.Value = false;
			_negativeParadoxModel.RxIsInputEnabled.Value = false;
			_buffPanelModel.RxIsInputEnabled.Value = false;
			_skillPanelModel.RxIsInputEnabled.Value = false;
			SetDefaultNav(SelectedTabButton);
			ResumeNav();
			break;
		case EFocusPanel.NegativeParadox:
			_focusedPanel = EFocusPanel.PositiveParadox;
			SetPanelInputEnabled(_focusedPanel, ENavDir.Up);
			SuspendNav();
			break;
		case EFocusPanel.Buff:
			_focusedPanel = EFocusPanel.NegativeParadox;
			SetPanelInputEnabled(_focusedPanel, ENavDir.Up);
			SuspendNav();
			break;
		}
	}

	private void OutOfBottom()
	{
		EFocusPanel eFocusPanel = GetNextPanelDown(_focusedPanel);
		Logger.Debug(string.Format("{0} >> {1}", _focusedPanel, eFocusPanel), new Color(0.49803922f, 1f, 0.83137256f, 1f));
		if (_focusedPanel != eFocusPanel)
		{
			_focusedPanel = eFocusPanel;
			SetPanelInputEnabled(eFocusPanel);
			SuspendNav();
		}
	}

	private EFocusPanel GetNextPanelUp(EFocusPanel current)
	{
		if (current != EFocusPanel.None && current != EFocusPanel.PositiveParadox)
		{
			switch (current)
			{
			case EFocusPanel.NegativeParadox:
				return EFocusPanel.PositiveParadox;
			case EFocusPanel.Buff:
				return EFocusPanel.NegativeParadox;
			default:
				return current;
			case EFocusPanel.Skill:
				break;
			}
		}
		return EFocusPanel.None;
	}

	private EFocusPanel GetNextPanelDown(EFocusPanel current)
	{
		switch (_currentTab)
		{
		case ETab.ParadoxBuff:
			switch (current)
			{
			case EFocusPanel.None:
				return EFocusPanel.PositiveParadox;
			case EFocusPanel.PositiveParadox:
				return EFocusPanel.NegativeParadox;
			case EFocusPanel.NegativeParadox:
			case EFocusPanel.Buff:
				return EFocusPanel.Buff;
			default:
				return EFocusPanel.None;
			}
		case ETab.Skill:
			return EFocusPanel.Skill;
		default:
			return current;
		}
	}

	private void Close()
	{
		_model.OnEscape.OnNext(Unit.Default);
	}

	private void SelectTab(ETab tab)
	{
		_currentTab = tab;
		_focusedPanel = EFocusPanel.None;
		_view.SkillTalentTab_Button.GetComponent<ButtonController>().SetSelected(tab == ETab.Skill);
		_view.ParadoxBuffTab_Button.GetComponent<ButtonController>().SetSelected(tab == ETab.ParadoxBuff);
		_view.SkillTalentPage.SetActive(tab == ETab.Skill);
		_view.ParadoxBuffPage.SetActive(tab == ETab.ParadoxBuff);
		_positiveParadoxModel.RxIsInputEnabled.Value = false;
		_negativeParadoxModel.RxIsInputEnabled.Value = false;
		_buffPanelModel.RxIsInputEnabled.Value = false;
		_skillPanelModel.RxIsInputEnabled.Value = false;
		SetDefaultNav(SelectedTabButton);
	}

	private void SetPanelInputEnabled(EFocusPanel panel, ENavDir direction = ENavDir.Down)
	{
		if (panel != EFocusPanel.None)
		{
			GetPanelModel(panel).EntryDirection = direction;
		}
		_positiveParadoxModel.RxIsInputEnabled.Value = panel == EFocusPanel.PositiveParadox;
		_negativeParadoxModel.RxIsInputEnabled.Value = panel == EFocusPanel.NegativeParadox;
		_buffPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Buff;
		_skillPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Skill;
	}

	private BasePanelModel GetPanelModel(EFocusPanel panel)
	{
		switch (panel)
		{
		case EFocusPanel.PositiveParadox:
			return _positiveParadoxModel;
		case EFocusPanel.NegativeParadox:
			return _negativeParadoxModel;
		case EFocusPanel.Buff:
			return _buffPanelModel;
		case EFocusPanel.Skill:
			return _skillPanelModel;
		default:
			return null;
		}
	}

	private void OnGoodsSlotEntered(InventoryGoodsSlotModel slotModel)
	{
		if (!slotModel.IsSlotEmpty)
		{
			EquipmentTooltipModel model = new EquipmentTooltipModel
			{
				Goods = slotModel.GoodsSlot.Goods
			};
			_view.EquipmentTooltip.Inject(model);
			_view.EquipmentTooltip.gameObject.SetActive(value: true);
		}
	}

	private void OnGoodsSlotExit(InventoryGoodsSlotModel slotModel)
	{
		_view.EquipmentTooltip.gameObject.SetActive(value: false);
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
		_view.ParadoxTooltip.gameObject.SetActive(value: true);
		_view.ParadoxTooltip.Inject(new ParadoxTooltipModel
		{
			ParadoxId = slotModel.ParadoxId,
			IsLocked = slotModel.IsLocked
		});
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
		_view.ParadoxTooltip.gameObject.SetActive(value: false);
	}

	private void OnBuffSlotEnter(MenuBuffSlotModel slotModel)
	{
		_view.BuffTooltip.gameObject.SetActive(value: true);
		_view.BuffTooltip.Inject(new BuffTooltipModel
		{
			BuffId = slotModel.UnitBuff.Id
		});
	}

	private void OnBuffSlotExit(MenuBuffSlotModel slotModel)
	{
		_view.BuffTooltip.gameObject.SetActive(value: false);
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
		if (slotModel.SkillId >= 1)
		{
			_view.SkillTooltip.gameObject.SetActive(value: true);
			_view.SkillTooltip.Inject(new InGameSkillTooltipModel
			{
				SkillId = slotModel.SkillId,
				ShowModifiedValues = true,
				UnitModel = _gameManager.MyCharacterModel
			});
		}
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
		_view.SkillTooltip.gameObject.SetActive(value: false);
	}
}
