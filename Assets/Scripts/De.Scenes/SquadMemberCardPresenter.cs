using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using Steamworks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SquadMemberCardPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Base = 1,
		Empty = 2,
		PositiveParadox = 3,
		NegativeParadox = 4
	}

	[InjectModel]
	private SquadMemberCardModel _model;

	[View]
	private SquadMemberCardView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private CompositeDisposable _disposable;

	private ulong _steamId;

	private EmptyPanelModel _emptyPanelModel;

	private ParadoxPanelModel _positiveParadoxPanelModel;

	private ParadoxPanelModel _negativeParadoxPanelModel;

	protected override BasePanelModel PanelModel => _model;

	private EFocusPanel FocusedPanel
	{
		get
		{
			if (_model == null)
			{
				return EFocusPanel.None;
			}
			if (_model.IsEmpty)
			{
				return EFocusPanel.Empty;
			}
			if (_model.FocusPanel == EFocusPanel.None)
			{
				return EFocusPanel.Base;
			}
			return _model.FocusPanel;
		}
	}

	private bool IsSteamConnected
	{
		get
		{
			if (_environmentSettings.Platform != EPlatform.Steam)
			{
				return false;
			}
			return SteamManager.Initialized;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}

	private void OnInject(SquadMemberCardModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.PlayerLayout.SetActive(!model.IsEmpty);
		_view.ParadoxLayout.SetActive(!model.IsEmpty);
		_view.EmptyPanel.gameObject.SetActive(model.IsEmpty);
		if (!model.IsEmpty)
		{
			InitProfile();
			InitParadoxPanel();
			InitSkillPanel();
		}
		else
		{
			_emptyPanelModel = new EmptyPanelModel
			{
				Text = TLocalizedText.Empty.LocalText
			};
			_view.EmptyPanel.Inject(_emptyPanelModel);
		}
		InitGamePadNav();
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		Logger.Debug(string.Format("Card Index : {0}. {1}", _model.Index, FocusedPanel), new Color(0f, 0.54509807f, 0.54509807f, 1f));
		FocusPanel(FocusedPanel, _model.EntryDirection);
	}

	protected override void OnInputDisabled()
	{
		Logger.Debug(string.Format("Card Index : {0}", _model.Index), new Color(0f, 0.54509807f, 0.54509807f, 1f));
		if (!_model.IsEmpty)
		{
			_positiveParadoxPanelModel.RxIsInputEnabled.Value = false;
			_negativeParadoxPanelModel.RxIsInputEnabled.Value = false;
		}
		else
		{
			_emptyPanelModel.RxIsInputEnabled.Value = false;
		}
		base.OnInputDisabled();
	}

	private void InitProfile()
	{
		GamePlayer gamePlayer = _gameContext.Players[_model.ClientId];
		if (gamePlayer == null)
		{
			return;
		}
		_steamId = gamePlayer.SteamId;
		_view.HostIcon.gameObject.SetActive(gamePlayer.IsHostPlayer);
		_view.Level.text = string.Format("Lv. {0}", gamePlayer.Profile.Level);
		_view.Name.text = gamePlayer.Name;
		gamePlayer.RxCharacter.Where((Character c) => c != null).Subscribe(delegate(Character c)
		{
			_view.Face_Image.sprite = _assetManager.GetIconUnitCharacter(c.Id);
		}).AddTo(_disposable);
		if (_environmentSettings.Platform == EPlatform.Steam && SteamManager.Initialized)
		{
			string text = ((_steamId == 0L) ? SteamFriends.GetPersonaName() : SteamFriends.GetFriendPersonaName(new CSteamID(_steamId)));
			_view.SteamButton_TextUserId.text = (string.IsNullOrEmpty(text) ? _steamId.ToString() : text);
			_view.SteamButton.OnClickAsObservable().Subscribe(delegate
			{
				OnSteamButtonClick();
			}).AddTo(_disposable);
		}
		else
		{
			_view.SteamButton_TextUserId.text = TLocalizedText.Notconnected.LocalText;
		}
	}

	private void InitParadoxPanel()
	{
		GamePlayer player = _gameContext.Players[_model.ClientId];
		_positiveParadoxPanelModel = new ParadoxPanelModel
		{
			Player = player,
			IsPositive = true
		};
		_positiveParadoxPanelModel.OnParadoxSlotEnter.Subscribe(OnParadoxSlotEnter).AddTo(this);
		_positiveParadoxPanelModel.OnParadoxSlotExit.Subscribe(OnParadoxSlotExit).AddTo(this);
		_view.PositiveParadoxPanel.Inject(_positiveParadoxPanelModel);
		_negativeParadoxPanelModel = new ParadoxPanelModel
		{
			Player = player,
			IsPositive = false
		};
		_negativeParadoxPanelModel.OnParadoxSlotEnter.Subscribe(OnParadoxSlotEnter).AddTo(this);
		_negativeParadoxPanelModel.OnParadoxSlotExit.Subscribe(OnParadoxSlotExit).AddTo(this);
		_view.NegativeParadoxPanel.Inject(_negativeParadoxPanelModel);
	}

	private void InitSkillPanel()
	{
		MenuSkillPanelModel menuSkillPanelModel = new MenuSkillPanelModel();
		menuSkillPanelModel.ClientId = _model.ClientId;
		menuSkillPanelModel.OnSlotEnter.Subscribe(OnSkillSlotEnter).AddTo(this);
		menuSkillPanelModel.OnSlotExit.Subscribe(OnSkillSlotExit).AddTo(this);
		_view.MenuSkillPanel.Inject(menuSkillPanelModel);
	}

	private void InitGamePadNav()
	{
		ClearNav();
		if (!_model.IsEmpty)
		{
			SetNavHorizontal(new List<Selectable> { _view.SteamButton });
			_model.OutOfRight.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.PositiveParadox, ENavDir.Right);
			}).AddTo(_disposable);
			_positiveParadoxPanelModel.OutOfLeft.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.Base, ENavDir.Left);
			}).AddTo(_disposable);
			_positiveParadoxPanelModel.OutOfRight.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.NegativeParadox, ENavDir.Right);
			}).AddTo(_disposable);
			_positiveParadoxPanelModel.OutOfTop.Subscribe(delegate
			{
				TriggerOutOfTop();
			}).AddTo(_disposable);
			_positiveParadoxPanelModel.OutOfBottom.Subscribe(delegate
			{
				TriggerOutOfBottom();
			}).AddTo(_disposable);
			_negativeParadoxPanelModel.OutOfLeft.Subscribe(delegate
			{
				FocusPanel(EFocusPanel.PositiveParadox, ENavDir.Left);
			}).AddTo(_disposable);
			_negativeParadoxPanelModel.OutOfTop.Subscribe(delegate
			{
				TriggerOutOfTop();
			}).AddTo(_disposable);
			_negativeParadoxPanelModel.OutOfBottom.Subscribe(delegate
			{
				TriggerOutOfBottom();
			}).AddTo(_disposable);
		}
		else
		{
			_emptyPanelModel.OutOfTop.Subscribe(delegate
			{
				TriggerOutOfTop();
			}).AddTo(_disposable);
			_emptyPanelModel.OutOfBottom.Subscribe(delegate
			{
				TriggerOutOfBottom();
			}).AddTo(_disposable);
		}
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
		Logger.Debug(string.Format("Card Index : {0}. {1}", _model.Index, FocusedPanel), new Color(0f, 0.54509807f, 0.54509807f, 1f));
		_model.FocusPanel = panel;
		BasePanelModel panelModel = GetPanelModel();
		if (panelModel != null)
		{
			panelModel.EntryDirection = entryDir;
		}
		if (panel == EFocusPanel.Base)
		{
			ResumeNav();
			if (IsSteamConnected)
			{
				_view.SteamButton.Select();
			}
		}
		else
		{
			SuspendNav();
		}
		if (_emptyPanelModel != null)
		{
			_emptyPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.Empty;
		}
		if (_positiveParadoxPanelModel != null)
		{
			_positiveParadoxPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.PositiveParadox;
		}
		if (_negativeParadoxPanelModel != null)
		{
			_negativeParadoxPanelModel.RxIsInputEnabled.Value = panel == EFocusPanel.NegativeParadox;
		}
		BasePanelModel GetPanelModel()
		{
			return panel switch
			{
				EFocusPanel.Base => _model, 
				EFocusPanel.Empty => _emptyPanelModel, 
				EFocusPanel.PositiveParadox => _positiveParadoxPanelModel, 
				EFocusPanel.NegativeParadox => _negativeParadoxPanelModel, 
				_ => null, 
			};
		}
	}

	private void OnSteamButtonClick()
	{
		CSteamID steamID = ((_steamId == 0L) ? SteamUser.GetSteamID() : new CSteamID(_steamId));
		SteamFriends.ActivateGameOverlayToUser("steamid", steamID);
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
		_model.OnParadoxSlotEnter.OnNext(slotModel);
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
		_model.OnParadoxSlotExit.OnNext(slotModel);
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
		_model.OnSkillSlotEnter.OnNext(slotModel);
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
		_model.OnSkillSlotExit.OnNext(slotModel);
	}
}
