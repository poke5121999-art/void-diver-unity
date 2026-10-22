using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class MenuPopupPresenter : GamePopupPresenter
{
	public enum EPageCategory
	{
		Quest = 0,
		InventoryManagement = 1,
		Character = 2,
		Archive = 3,
		Squad = 4,
		Option = 5,
		System = 6
	}

	[View]
	private MenuPopupView _view;

	[InjectModel]
	private MenuPopupModel _model;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	private List<MenuTabModel> _tabModels;

	private CompositeDisposable _pageDisposable;

	private BasePanelModel _selectedPageModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	public override string OpenSfx => "InventoryPopupOpen";

	public override string CloseSfx => "InventoryPopupOpen";

	private bool IsSquadTabLocked
	{
		get
		{
			TNpc tNpc = TNpc.Table[700012];
			if (!_playerManager.CheckStateConditions(tNpc.UnlockConditions))
			{
				return true;
			}
			return _gameManager.IsTutorial;
		}
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_tabModels = new List<MenuTabModel>(_view.Tabs.Length);
		for (int i = 0; i < _view.Tabs.Length; i++)
		{
			MenuTabPresenter componentInChildren = _view.Tabs[i].GetComponentInChildren<MenuTabPresenter>(true);
			if (componentInChildren != null)
			{
				MenuTabModel menuTabModel = new MenuTabModel();
				menuTabModel.PageCategory = (EPageCategory)i;
				menuTabModel.RxIsLocked.Value = i == 4 && IsSquadTabLocked;
				menuTabModel.OnClick.Subscribe(OnTabButtonClick).AddTo(OpenDisposable);
				_tabModels.Add(menuTabModel);
				componentInChildren.Inject(menuTabModel);
			}
		}
		SetTab(_model.PageCategory);
		SetPage(_model.PageCategory);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.CloseMenu.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiPadActions.TabLeft.OnClickWithSfx().Subscribe(delegate
		{
			OnTabLeft();
		}).AddTo(InputDisposable);
		UiPadActions.TabRight.OnClickWithSfx().Subscribe(delegate
		{
			OnTabRight();
		}).AddTo(InputDisposable);
		if (_selectedPageModel != null)
		{
			_selectedPageModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		if (_selectedPageModel != null)
		{
			_selectedPageModel.RxIsInputEnabled.Value = false;
		}
	}

	protected override void OnDestroy()
	{
		_pageDisposable?.Dispose();
		base.OnDestroy();
	}

	private void UpdatePage(EPageCategory category)
	{
		if (_model.PageCategory != category)
		{
			_model.PageCategory = category;
			SetTab(category);
			SetPage(category);
		}
	}

	private void SetTab(EPageCategory category)
	{
		foreach (MenuTabModel tabModel in _tabModels)
		{
			tabModel.RxIsSelected.Value = tabModel.PageCategory == category;
		}
	}

	private void SetPage(EPageCategory category)
	{
		if (_selectedPageModel != null)
		{
			_selectedPageModel.RxIsInputEnabled.Value = false;
		}
		InputManager.ClearSelectedPosition();
		_pageDisposable?.Dispose();
		_pageDisposable = new CompositeDisposable();
		for (EPageCategory ePageCategory = EPageCategory.Quest; (int)ePageCategory < _view.Pages.Length; ePageCategory++)
		{
			_view.Pages[(int)ePageCategory].SetActive(ePageCategory == category);
		}
		GameObject selectedPage = _view.Pages[(int)category];
		BasePanelModel basePanelModel;
		switch (category)
		{
		case EPageCategory.Quest:
			basePanelModel = new QuestPageModel();
			selectedPage.GetComponent<QuestPagePresenter>().Inject(basePanelModel);
			break;
		case EPageCategory.InventoryManagement:
		{
			InventoryManagementPageModel inventoryManagementPageModel = new InventoryManagementPageModel();
			inventoryManagementPageModel.Layout = _model.InventoryManagementLayout;
			inventoryManagementPageModel.LootingInventory = _model.LootingInventory;
			basePanelModel = inventoryManagementPageModel;
			selectedPage.GetComponent<InventoryManagementPagePresenter>().Inject(basePanelModel);
			break;
		}
		case EPageCategory.Character:
			basePanelModel = new CharacterPageModel();
			selectedPage.GetComponent<CharacterPagePresenter>().Inject(basePanelModel);
			break;
		case EPageCategory.Archive:
			basePanelModel = new ArchivePageModel();
			selectedPage.GetComponent<ArchivePagePresenter>().Inject(basePanelModel);
			break;
		case EPageCategory.Squad:
			basePanelModel = new SquadPageModel();
			selectedPage.GetComponent<SquadPagePresenter>().Inject(basePanelModel);
			break;
		case EPageCategory.Option:
			basePanelModel = new OptionPageModel();
			selectedPage.GetComponent<OptionPagePresenter>().Inject(basePanelModel);
			break;
		case EPageCategory.System:
			basePanelModel = new SystemPageModel();
			selectedPage.GetComponent<SystemPagePresenter>().Inject(basePanelModel);
			break;
		default:
			throw new ArgumentOutOfRangeException("category", category, null);
		}
		_selectedPageModel = basePanelModel;
		_selectedPageModel.OnEscape.Subscribe(delegate
		{
			Close();
		}).AddTo(_pageDisposable);
		_selectedPageModel.RxIsInputEnabled.Value = base.IsInputEnable;
	}

	private void OnTabLeft()
	{
		if (_model.PageCategory > EPageCategory.Quest)
		{
			EPageCategory ePageCategory = _model.PageCategory - 1;
			if (ePageCategory == EPageCategory.Squad && IsSquadTabLocked)
			{
				ePageCategory = EPageCategory.Archive;
			}
			UpdatePage(ePageCategory);
		}
	}

	private void OnTabRight()
	{
		int length = Enum.GetValues(typeof(EPageCategory)).Length;
		EPageCategory ePageCategory = _model.PageCategory + 1;
		if ((int)ePageCategory < length)
		{
			if (ePageCategory == EPageCategory.Squad && IsSquadTabLocked)
			{
				ePageCategory++;
				if ((int)ePageCategory >= length)
				{
					return;
				}
			}
			UpdatePage(ePageCategory);
		}
	}

	private void OnTabButtonClick(MenuTabModel tabModel)
	{
		if (!tabModel.RxIsLocked.Value)
		{
			if (_model.PageCategory != tabModel.PageCategory)
			{
				_model.PageCategory = tabModel.PageCategory;
				SetTab(tabModel.PageCategory);
				SetPage(tabModel.PageCategory);
			}
		}
		else
		{
			_toastManager.Notice(TLocalizedText.SquadTabLockedMessage.LocalText);
			_soundManager.PlaySfx("Fail");
		}
	}
}
