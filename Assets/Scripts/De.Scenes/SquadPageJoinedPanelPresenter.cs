using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using UnityEngine.UI;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class SquadPageJoinedPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Base = 1,
		Cards = 2
	}

	[InjectModel]
	private SquadPageJoinedPanelModel _model;

	[View]
	private SquadPageJoinedPanelView _view;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	private CompositeDisposable _disposable;

	private CompositeDisposable _cardDisposable;

	private readonly List<SquadMemberCardModel> _cardModels = new List<SquadMemberCardModel>();

	private SquadMemberCardPresenter.EFocusPanel _currentCardFocusPanel;

	private bool _isCodeVisible;

	protected override BasePanelModel PanelModel => _model;

	private EFocusPanel FocusedPanel => _model.FocusPanel;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
		_cardDisposable?.Dispose();
	}

	private void OnInject(SquadPageJoinedPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_view.Participating.SetActive(!model.IsHost);
		_view.Setting.SetActive(model.IsHost);
		UpdateLobby();
		UpdateLoungeCode();
		UpdateMemberCards();
		InitGamePadNav();
		_gameContext.RxPlayers.ObserveAdd().Subscribe(delegate
		{
			UpdateMemberCards();
		}).AddTo(_disposable);
		_gameContext.RxPlayers.ObserveRemove().Subscribe(delegate
		{
			UpdateMemberCards();
		}).AddTo(_disposable);
		_roomMatchmaker.OnJoinLobby.Subscribe(delegate
		{
			OnJoinLobby();
		}).AddTo(_disposable);
		_roomMatchmaker.OnCurrentLobbyDataUpdated.Subscribe(OnLobbyDataUpdated).AddTo(_disposable);
		_view.Setting_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSettingButtonClickAsync().Forget();
		}).AddTo(_disposable);
		_view.LoungeCode_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnLoungeCodeClick();
		}).AddTo(_disposable);
		_view.LoungeCode_Hide_OnOffButton.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnOnOffButtonClick();
		}).AddTo(_disposable);
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
		FocusPanel(_model.FocusPanel, _model.EntryDirection);
	}

	protected override void OnInputDisabled()
	{
		FocusPanel(EFocusPanel.None, ENavDir.None);
		base.OnInputDisabled();
	}

	private void OnJoinLobby()
	{
		UpdateLobby();
		UpdateLoungeCode();
	}

	private async UniTask OnSettingButtonClickAsync()
	{
		RoomSettingPopupPresenter popup = await _popupManager.OpenAsync<RoomSettingPopupPresenter>(new RoomSettingPopupModel
		{
			RoomTitle = (_roomMatchmaker.JoinedLobby?.Title ?? TLocalizedText.DefaultLobbyName.LocalText),
			PublishType = (_roomMatchmaker.IsPrivateLobby ? ERoomPublishType.Private : ERoomPublishType.Public),
			RoomDifficulty = _playerManager.Difficulty
		});
		if (popup != null && await popup.Task is RoomSettingPopupResult result)
		{
			_playerManager.SetDifficulty(result.RoomDifficulty);
			_playerManager.SetLastPlayedRoomDifficulty(result.RoomDifficulty);
			_roomMatchmaker.UpdateLobbySettings(result.RoomTitle, result.PublishType, result.RoomDifficulty);
		}
	}

	private void OnLoungeCodeClick()
	{
		string loungeCode = _roomMatchmaker.LoungeCode;
		_toastManager.Notice(TLocalizedText.CopyLoungeCode.Format(loungeCode));
		GUIUtility.systemCopyBuffer = loungeCode;
	}

	private void OnOnOffButtonClick()
	{
		_isCodeVisible = !_isCodeVisible;
		UpdateLoungeCode();
	}

	private void UpdateLoungeCode()
	{
		if (!_roomContext.IsSingleMode)
		{
			string loungeCode = _roomMatchmaker.LoungeCode;
			_view.LoungeCode_Hide_OnOffButton_On.gameObject.SetActive(_isCodeVisible);
			_view.LoungeCode_Hide_OnOffButton_Off.gameObject.SetActive(!_isCodeVisible);
			_view.LoungeCode_Text.text = (_isCodeVisible ? loungeCode : new string('*', loungeCode.Length));
		}
	}

	private void UpdateLobby()
	{
		MatchingLobby joinedLobby = _roomMatchmaker.JoinedLobby;
		ERoomDifficulty difficulty = ((joinedLobby != null) ? joinedLobby.RoomDifficulty : _playerManager.Difficulty);
		_view.LoungeCode_Lock.gameObject.SetActive(_roomMatchmaker.IsPrivateLobby);
		string title = _roomMatchmaker.JoinedLobby?.Title;
		_view.Title.text = (string.IsNullOrEmpty(title) ? TLocalizedText.DefaultLobbyName.LocalText : title);
		_view.Publish.text = (_roomMatchmaker.IsPrivateLobby ? ERoomPublishType.Private : ERoomPublishType.Public).ToLocalText();
		_view.Difficulty.color = difficulty.GetColor();
		_view.Difficulty.text = difficulty.ToLocalText();
	}

	private void OnLobbyDataUpdated(MatchingLobby lobby)
	{
		_view.LoungeCode_Lock.gameObject.SetActive(lobby.PublishType == ERoomPublishType.Private);
		_view.Title.text = lobby.Title;
		_view.Publish.text = lobby.PublishType.ToLocalText();
		_view.Difficulty.color = lobby.RoomDifficulty.GetColor();
		_view.Difficulty.text = lobby.RoomDifficulty.ToLocalText();
	}

	private void UpdateMemberCards()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		_cardModels.Clear();
		_cardDisposable?.Dispose();
		_cardDisposable = new CompositeDisposable();
		int playerCount = _gameContext.RxPlayers.Count;
		for (int i = 0; i < _view.Cards.Length; i++)
		{
			GameObject cardObject = _view.Cards[i];
			SquadMemberCardModel cardModel = new SquadMemberCardModel();
			if (i < playerCount)
			{
				cardModel.Index = i;
				cardModel.ClientId = _gameContext.RxPlayers.ElementAt(i).Key;
			}
			else
			{
				cardModel.Index = i;
				cardModel.IsEmpty = true;
			}
			_view.Cards[i].name = string.Format("Card_{0}", i);
			cardModel.OnParadoxSlotEnter.Subscribe(OnParadoxSlotEnter).AddTo(_cardDisposable);
			cardModel.OnParadoxSlotExit.Subscribe(OnParadoxSlotExit).AddTo(_cardDisposable);
			cardModel.OnSkillSlotEnter.Subscribe(OnSkillSlotEnter).AddTo(_cardDisposable);
			cardModel.OnSkillSlotExit.Subscribe(OnSkillSlotExit).AddTo(_cardDisposable);
			_cardModels.Add(cardModel);
			cardObject.GetComponent<SquadMemberCardPresenter>().Inject(cardModel);
		}
		InitCardGamePadNav();
		_view.ParadoxTooltip.gameObject.SetActive(value: false);
		_view.SkillTooltip.gameObject.SetActive(value: false);

		void InitCardGamePadNav()
		{
			foreach (SquadMemberCardModel card in _cardModels)
			{
				int index = card.Index;
				card.OutOfTop.Subscribe(delegate
				{
					OnCardOutOfTop(index);
				}).AddTo(_cardDisposable);
				card.OutOfBottom.Subscribe(delegate
				{
					OnCardOutOfBottom(index);
				}).AddTo(_cardDisposable);
			}
		}
	}

	private void InitGamePadNav()
	{
		ClearNav();
		List<Selectable> items = new List<Selectable>();
		items.Add(_view.LoungeCode_Hide_OnOffButton);
		items.Add(_view.LoungeCode_Button);
		if (_model.IsHost)
		{
			items.Add(_view.Setting_Button);
		}
		if (items.Count > 0)
		{
			SetNavHorizontal(items);
		}
		_model.OutOfBottom.Subscribe(delegate
		{
			OnBaseOutOfBottom();
		}).AddTo(_disposable);
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
		_model.FocusPanel = panel;
		switch (panel)
		{
		case EFocusPanel.Base:
			ResumeNav();
			foreach (SquadMemberCardModel cardModel in _cardModels)
			{
				cardModel.RxIsInputEnabled.Value = false;
			}
			break;
		case EFocusPanel.Cards:
			SuspendNav();
			FocusCard(0, entryDir);
			break;
		default:
			foreach (SquadMemberCardModel cardModel2 in _cardModels)
			{
				cardModel2.RxIsInputEnabled.Value = false;
			}
			break;
		}
	}

	private void FocusCard(int index, ENavDir entryDir)
	{
		if (index < 0 || index >= _cardModels.Count)
		{
			return;
		}
		foreach (SquadMemberCardModel cardModel in _cardModels)
		{
			cardModel.RxIsInputEnabled.Value = false;
		}
		_cardModels[index].EntryDirection = entryDir;
		_cardModels[index].FocusPanel = _currentCardFocusPanel;
		_cardModels[index].RxIsInputEnabled.Value = true;
	}

	private void OnBaseOutOfBottom()
	{
		if (_cardModels.Count != 0)
		{
			FocusPanel(EFocusPanel.Cards, ENavDir.Down);
		}
	}

	private void OnCardOutOfTop(int index)
	{
		if (!_cardModels[index].IsEmpty)
		{
			_currentCardFocusPanel = _cardModels[index].FocusPanel;
		}
		if (index != 0)
		{
			FocusCard(index - 1, ENavDir.Up);
		}
		else
		{
			FocusPanel(EFocusPanel.Base, ENavDir.Up);
		}
	}

	private void OnCardOutOfBottom(int index)
	{
		if (!_cardModels[index].IsEmpty)
		{
			_currentCardFocusPanel = _cardModels[index].FocusPanel;
		}
		if (index != _cardModels.Count - 1)
		{
			FocusCard(index + 1, ENavDir.Down);
		}
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
		_view.ParadoxTooltip.gameObject.SetActive(value: true);
		_view.ParadoxTooltip.GetComponent<ParadoxTooltipPresenter>().Inject(new ParadoxTooltipModel
		{
			ParadoxId = slotModel.ParadoxId,
			IsLocked = slotModel.IsLocked
		});
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
		_view.ParadoxTooltip.gameObject.SetActive(value: false);
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
		if (slotModel.SkillId > 0)
		{
			_view.SkillTooltip.gameObject.SetActive(value: true);
			_view.SkillTooltip.Inject(new InGameSkillTooltipModel
			{
				SkillId = slotModel.SkillId
			});
		}
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
		_view.SkillTooltip.gameObject.SetActive(value: false);
	}
}
