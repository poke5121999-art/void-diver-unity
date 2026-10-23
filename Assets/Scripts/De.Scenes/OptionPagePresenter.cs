using System.Linq;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;
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

	private EFocusPanel _displayedPanel = EFocusPanel.Graphic;

	protected override BasePanelModel PanelModel => _model;

	private InputActions.UiActions UiActions => _inputManager.UiActions;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnDisable()
	{
		_inputService.Flush();
	}

	private void OnInject(OptionPageModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_selectedPanel = EFocusPanel.None;
		_displayedPanel = EFocusPanel.Graphic;
		SetTab(EFocusPanel.None);
		InitTabPanels();
		InitTabButtons();
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		_graphicModel.RxIsInputEnabled.Value = _selectedPanel == EFocusPanel.Graphic;
		_soundModel.RxIsInputEnabled.Value = _selectedPanel == EFocusPanel.Sound;
		_gamePlayModel.RxIsInputEnabled.Value = _selectedPanel == EFocusPanel.GamePlay;
		_keyModel.RxIsInputEnabled.Value = _selectedPanel == EFocusPanel.Key;
		_padModel.RxIsInputEnabled.Value = _selectedPanel == EFocusPanel.Pad;
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}

	protected override void OnInputDisabled()
	{
		_graphicModel.RxIsInputEnabled.Value = false;
		_soundModel.RxIsInputEnabled.Value = false;
		_gamePlayModel.RxIsInputEnabled.Value = false;
		_keyModel.RxIsInputEnabled.Value = false;
		_padModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void InitTabButtons()
	{
		for (int i = 0; i < _view.Tabs.Length; i++)
		{
			EFocusPanel tab = (EFocusPanel)(i + 1);
			Button component = _view.Tabs[i].GetComponent<Button>();
			if (!(component == null))
			{
				component.OnClickWithSfx("ButtonClick").Subscribe(delegate
				{
					OnTabButtonClick(tab);
				}).AddTo(_disposable);
				_view.Tabs[i].GetComponent<ButtonController>().OnEnter.Subscribe(delegate
				{
					OnTabButtonEnter(tab);
				}).AddTo(_disposable);
			}
		}
	}

	private void OnTabButtonClick(EFocusPanel panel)
	{
		_selectedPanel = panel;
		SetTab(panel);
		SetPage(_selectedPanel);
		FocusPanel(panel);
	}

	private void OnTabButtonEnter(EFocusPanel panel)
	{
		_displayedPanel = panel;
		if (_selectedPanel == EFocusPanel.None)
		{
			SetPage(panel);
		}
	}

	private void InitTabPanels()
	{
		_graphicModel = new OptionGraphicPanelModel();
		_soundModel = new OptionSoundPanelModel();
		_gamePlayModel = new OptionGamePlayPanelModel();
		_keyModel = new OptionKeyPanelModel();
		_padModel = new OptionPadPanelModel();
		_view.Panel[0].GetComponent<OptionGraphicPanelPresenter>().Inject(_graphicModel);
		_view.Panel[1].GetComponent<OptionSoundPanelPresenter>().Inject(_soundModel);
		_view.Panel[2].GetComponent<OptionGamePlayPanelPresenter>().Inject(_gamePlayModel);
		_view.Panel[3].GetComponent<OptionKeyPanelPresenter>().Inject(_keyModel);
		_view.Panel[4].GetComponent<OptionPadPanelPresenter>().Inject(_padModel);
		SetPage(_displayedPanel);
	}

	private void SetTab(EFocusPanel panel)
	{
		for (int i = 0; i < _view.Panel.Length; i++)
		{
			_view.Tabs[i].GetComponent<ButtonController>().SetSelected(i == (int)(panel - 1));
		}
	}

	private void SetPage(EFocusPanel panel)
	{
		for (int i = 0; i < _view.Panel.Length; i++)
		{
			_view.Panel[i].SetActive(i == (int)(panel - 1));
		}
	}

	private void InitGamePadNav()
	{
		ClearNav();
		SetNavVertical(_view.Tabs.Select((GameObject go) => go.GetComponent<Selectable>()).ToList());
	}

	private void FocusPanel(EFocusPanel panel)
	{
		if (panel == EFocusPanel.None)
		{
			ResumeNav();
		}
		else
		{
			SuspendNav();
		}
		_graphicModel.RxIsInputEnabled.Value = panel == EFocusPanel.Graphic;
		_soundModel.RxIsInputEnabled.Value = panel == EFocusPanel.Sound;
		_gamePlayModel.RxIsInputEnabled.Value = panel == EFocusPanel.GamePlay;
		_keyModel.RxIsInputEnabled.Value = panel == EFocusPanel.Key;
		_padModel.RxIsInputEnabled.Value = panel == EFocusPanel.Pad;
	}

	private void Close()
	{
		if (_selectedPanel == EFocusPanel.None)
		{
			_model.OnEscape.OnNext(Unit.Default);
			return;
		}
		_selectedPanel = EFocusPanel.None;
		SetTab(EFocusPanel.None);
		SetPage(_displayedPanel);
		ResumeNav();
		_graphicModel.RxIsInputEnabled.Value = false;
		_soundModel.RxIsInputEnabled.Value = false;
		_gamePlayModel.RxIsInputEnabled.Value = false;
		_keyModel.RxIsInputEnabled.Value = false;
		_padModel.RxIsInputEnabled.Value = false;
	}
}
