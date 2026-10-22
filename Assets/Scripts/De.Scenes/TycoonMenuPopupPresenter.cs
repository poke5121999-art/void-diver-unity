using System;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class TycoonMenuPopupPresenter : GamePopupPresenter
{
	[View]
	private TycoonMenuPopupView _view;

	[InjectModel]
	private TycoonMenuPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SoundManager _soundManager;

	private TycoonTabModel[] _tabModels;

	private ETycoonMenuTab _currentTab;

	private BasePanelModel _selectedPageModel;

	private CompositeDisposable _pageDisposable;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiPadActions UiPadActions => GameInputManager.UiPadActions;

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_soundManager.PlaySfx("ArtifactUI");
		InitTabs();
		SwitchTab(_model.InitialTab);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
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

	private void InitTabs()
	{
		ETycoonMenuTab[] array = new ETycoonMenuTab[3]
		{
			ETycoonMenuTab.Main,
			ETycoonMenuTab.EmployeeInfo,
			ETycoonMenuTab.EmployeeHire
		};
		_tabModels = new TycoonTabModel[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			TycoonTabPresenter component = _view.Top_Tabs[i].GetComponent<TycoonTabPresenter>();
			TycoonTabModel tycoonTabModel = new TycoonTabModel
			{
				Tab = array[i]
			};
			tycoonTabModel.OnClick.Subscribe(OnTabClicked).AddTo(this);
			component.Inject(tycoonTabModel);
			_tabModels[i] = tycoonTabModel;
		}
	}

	private void OnTabClicked(TycoonTabModel tabModel)
	{
		SwitchTab(tabModel.Tab);
	}

	private void SwitchTab(ETycoonMenuTab tab)
	{
		if (_selectedPageModel != null)
		{
			_selectedPageModel.RxIsInputEnabled.Value = false;
		}
		InputManager.ClearSelectedPosition();
		_currentTab = tab;
		_pageDisposable?.Dispose();
		_pageDisposable = new CompositeDisposable();
		for (int i = 0; i < _tabModels.Length; i++)
		{
			_tabModels[i].RxIsSelected.Value = _tabModels[i].Tab == tab;
		}
		_view.TycoonMainPage.gameObject.SetActive(tab == ETycoonMenuTab.Main);
		_view.EmployeeInfoPage.gameObject.SetActive(tab == ETycoonMenuTab.EmployeeInfo);
		_view.EmployeeHirePage.gameObject.SetActive(tab == ETycoonMenuTab.EmployeeHire);
		BasePanelModel basePanelModel;
		Presenter presenter;
		switch (tab)
		{
		case ETycoonMenuTab.Main:
			basePanelModel = new TycoonMainPageModel
			{
				InitialSalesSlotId = _model.InitialSalesSlotId
			};
			presenter = _view.TycoonMainPage;
			break;
		case ETycoonMenuTab.EmployeeInfo:
			basePanelModel = new EmployeeInfoPageModel();
			presenter = _view.EmployeeInfoPage;
			break;
		case ETycoonMenuTab.EmployeeHire:
			basePanelModel = new EmployeeHirePageModel();
			presenter = _view.EmployeeHirePage;
			break;
		default:
			return;
		}
		_selectedPageModel = basePanelModel;
		basePanelModel.OnEscape.Subscribe(delegate
		{
			Close();
		}).AddTo(_pageDisposable);
		basePanelModel.RxIsInputEnabled.Value = base.IsInputEnable;
		presenter.Inject(basePanelModel);
	}

	private void OnTabLeft()
	{
		if (_currentTab >= ETycoonMenuTab.EmployeeInfo)
		{
			SwitchTab(_currentTab - 1);
		}
	}

	private void OnTabRight()
	{
		if ((int)_currentTab < Enum.GetValues(typeof(ETycoonMenuTab)).Length - 1)
		{
			SwitchTab(_currentTab + 1);
		}
	}
}
