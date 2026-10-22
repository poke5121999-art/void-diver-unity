using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using NemoLib.Scene;
using Unity.Netcode;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StartScene : BaseScenePresenter
{
	[View]
	private StartSceneView _view;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private ILuaRunner _luaRunner;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GraphicSettingService _graphicSettings;

	[Inject]
	private SoundSettingService _soundSettings;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private InputService _inputService;

	[Inject]
	private AccountRecordManager _accountRecordManager;

	[Inject]
	private AchievementManager _achievementManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private NewTagManager _newTagManager;

	[Inject]
	private GamePlaySettingService _playSettingService;

	[Inject]
	private FontPrewarmService _fontPrewarmService;

	[Inject]
	private StorageService _storageService;

	private Selectable _selectedObject;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	protected override Selectable SelectedObject => _selectedObject ?? _view.Play_Button;

	protected override void Start()
	{
		base.Start();
		_graphicSettings.SetGameplayVSyncActive(active: false);
		GameInputManager.Clear();
		_newTagManager.Initialize();
		_storageService.OnSaveFailed.Subscribe(OnSaveFailed).AddTo(this);
		StartAsync().Forget();
	}

	private void OnSaveFailed(ESaveResult result)
	{
		PopupManager.ShowTextPopup(((Enum)result).ToLocalText());
	}

	private async UniTask StartAsync()
	{
		_view.Version.text = Application.version;
		bool isFirstLoad = !_playerManager.IsPlayerLoaded;
		await EnsureAccountAsync();
		if (isFirstLoad)
		{
			FireLoginStep();
		}
		await _inputService.InitAsync();
		_accountRecordManager.Initialize();
		InitButtons();
		_playSettingService.RxLanguage.Subscribe(delegate
		{
			UpdateButtons();
			_fontPrewarmService.PrewarmAsync().Forget();
		}).AddTo(this);
		GetComponent<Animator>().enabled = true;
		await PreloadResourcesAsync();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		SetMenuActive(active: true);
	}

	protected override void OnInputDisabled()
	{
		SetMenuActive(active: false);
		InputDisposable?.Dispose();
	}

	private void SetMenuActive(bool active)
	{
		_view.Buttons.SetActive(active);
		_view.Social.SetActive(active);
		_view.Steam.SetActive(active);
	}

	private async UniTask PreloadResourcesAsync()
	{
		await UniTask.WhenAll(
			PopupManager.PreloadAsync<CampaignPopupPresenter>(),
			PopupManager.PreloadAsync<MenuPopupPresenter>(),
			PopupManager.PreloadAsync<KeyInteractionPopupPresenter>(),
			PopupManager.PreloadAsync<SelectionPopupPresenter>());
	}

	private void InitButtons()
	{
		string wishlistUrl = string.Format("steam://store/{0}", 3438920u);
		UpdateButtons();
		_view.Play_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnPlayButtonClickAsync().Forget();
		}).AddTo(this);
		_view.SelectSlot_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnSelectSlotButtonClickAsync().Forget();
		}).AddTo(this);
		_view.Option_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnOptionButtonClick();
		}).AddTo(this);
		_view.Quit_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnQuitButtonClick().Forget();
		}).AddTo(this);
		_view.Wishlist_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Application.OpenURL(wishlistUrl);
		}).AddTo(this);
		_view.Discord_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Application.OpenURL("https://discord.gg/kC3aBdsWc5");
		}).AddTo(this);
		_view.X_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Application.OpenURL("https://x.com/studionemo_de");
		}).AddTo(this);
		_view.Feedback_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnFeedbackButtonClick();
		}).AddTo(this);
		GameInputManager.UnblockInput(EInputActionType.Ui);
	}

	private async UniTask EnsureAccountAsync()
	{
		if (_playerManager.IsPlayerLoaded)
		{
			await _playerManager.SaveAsync();
		}
		else
		{
			await _playerManager.LoadAccountAsync();
		}
		await _accountRecordManager.SaveAsync();
	}

	private void FireLoginStep()
	{
		_eventSystem.Trigger(new LoginStepEvent(ELoginStep.LOGIN, ELoginDtl.SUCCESS, ELoginRsn.AUTO, (_environmentSettings.Platform != EPlatform.Steam) ? ELoginType.NL : ELoginType.LC, (long)(Time.realtimeSinceStartup * 1000f)));
	}

	private async UniTask<string> InputUserNameAsync()
	{
		InputNamePopupPresenter presenter = await PopupManager.OpenAsync<InputNamePopupPresenter>();
		return await presenter.Task as string;
	}

	private async UniTask<RoomDifficultyPopupResult> SelectRoomDifficultyAsync()
	{
		RoomDifficultyPopupPresenter presenter = await PopupManager.OpenAsync<RoomDifficultyPopupPresenter>(new RoomDifficultyPopupModel
		{
			Difficulty = ERoomDifficulty.Junior
		});
		return await presenter.Task as RoomDifficultyPopupResult;
	}

	private void OnLanguageApplied()
	{
		UpdateButtons();
		_fontPrewarmService.PrewarmAsync().Forget();
	}

	private void UpdateButtons()
	{
		PlayerSlot selectedPlayerSlot = _playerManager.SelectedPlayerSlot;
		bool isEmpty = selectedPlayerSlot.IsEmpty;
		_view.Play_Button_Text.text = (isEmpty ? TLocalizedText.NewGame : TLocalizedText.PlayGame).LocalText;
		_view.SelectSlot_Player_SlotNumber.text = TLocalizedText.PlayerSlotNumberFormat.Format(selectedPlayerSlot.SlotIndex + 1);
		_view.SelectSlot_Player_Name.text = (isEmpty ? TLocalizedText.EmptySlot.LocalText : selectedPlayerSlot.PlayerName);
	}

	private async UniTask OnPlayButtonClickAsync()
	{
		_selectedObject = _view.Play_Button;
		if (_playerManager.SelectedPlayerSlot.IsEmpty)
		{
			if (!await CreatePlayerAsync())
			{
				return;
			}
		}
		else
		{
			if (!await LoadPlayerAsync())
			{
				return;
			}
			if (!UIInputUtility.ValidateName(_playerManager.Name))
			{
				await PopupManager.ShowTextPopupAsync(TLocalizedText.NameContainsForbiddenWord.LocalText);
				while (true)
				{
					string name = await InputUserNameAsync();
					if (name.HasValue())
					{
						_playerManager.ChangeName(name);
						break;
					}
					await UniTask.Yield();
				}
			}
		}
		ApplyTutorial();
		await _playerManager.SaveAsync();
		await _accountRecordManager.SaveAsync();
		await PlayAsync();

		async UniTask<bool> CreatePlayerAsync()
		{
			string userName = await InputUserNameAsync();
			if (!userName.HasValue())
			{
				return false;
			}
			RoomDifficultyPopupResult result = await SelectRoomDifficultyAsync();
			if (result == null)
			{
				return false;
			}
			_playerManager.CreatePlayer(userName, result.Difficulty);
			_playerManager.SetPreventRepairMaxDurabilityLoss(result.PreventMaxDurabilityLoss);
			return true;
		}

		async UniTask<bool> LoadPlayerAsync()
		{
			ELoadResult loadResult = await _playerManager.LoadPlayerAsync();
			if (loadResult == ELoadResult.Success)
			{
				return true;
			}
			PopupManager.ShowTextPopup(loadResult.ToLocalText());
			return false;
		}

		void ApplyTutorial()
		{
			int tutorialId = TCampaign.Table.GetTutorialId();
			bool isTutorial = !_playerManager.IsClearedCampaign(tutorialId);
			if (isTutorial && _gameManager.IgnoreTutorial)
			{
				_playerManager.SetClearedCampaign(tutorialId);
				_eventSystem.Trigger(new CampaignClearEvent(tutorialId, isTutorial));
				isTutorial = false;
				if (_playerManager.Profile.SelectedPetId < 1)
				{
					_playerManager.Profile.SelectedPetId = TPet.Table.Keys.Min();
				}
			}
			_gameContext.IsTutorial = isTutorial;
		}

		async UniTask PlayAsync()
		{
			EPlatform platform = _environmentSettings.Platform;
			if (platform == EPlatform.Local)
			{
				LocalRoomCreationPopupPresenter presenter = await PopupManager.OpenAsync<LocalRoomCreationPopupPresenter>();
				await presenter.Task;
			}
			else if (platform == EPlatform.Steam)
			{
				_gameHost.CreateRoom(_playerManager.Difficulty);
				await _gameChannel.TryJoinGame(_playerManager.CreateRoomPlayerData(isHostPlayer: true));
				_gameManager.LoadGame();
			}
		}
	}

	private async UniTask OnSelectSlotButtonClickAsync()
	{
		_selectedObject = _view.SelectSlot_Button;
		SelectPlayerPopupPresenter presenter = await PopupManager.OpenAsync<SelectPlayerPopupPresenter>();
		await presenter.Task;
		UpdateButtons();
	}

	private void OnOptionButtonClick()
	{
		_selectedObject = _view.Option_Button;
		PopupManager.OpenAsync<OptionPopupPresenter>().Forget();
	}

	private async UniTask OnQuitButtonClick()
	{
		if (await PopupManager.ShowConfirmationTextPopupAsync(TLocalizedText.CloseGameDescription.LocalText))
		{
			_selectedObject = _view.Quit_Button;
			AppManager.Quit();
		}
	}

	private void OnStartAnimationEnded()
	{
	}

	private void OnFeedbackButtonClick()
	{
		Application.OpenURL(Const.Link.GetFeedbackLink(string.Format("{0}", _gamePlaySettings.Language)));
	}
}
