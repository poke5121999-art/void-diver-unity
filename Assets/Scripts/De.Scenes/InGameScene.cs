using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Popup;
using NemoLib.Scene;
using UniRx;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class InGameScene : BaseScenePresenter
{
	[Inject]
	protected AppManager AppManager;

	[Inject]
	protected RoomContext RoomContext;

	[Inject]
	protected NetworkManager NetworkManager;

	[Inject]
	protected GameHost GameHost;

	[Inject]
	protected GameChannel GameChannel;

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameCampaignManager CampaignManager;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected ToastManager ToastManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected GameLuaController LuaController;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameMapManager MapManager;

	[Inject]
	protected IRoomMatchmaker RoomMatchmaker;

	[Inject]
	protected EnvironmentSettings Settings;

	[Inject]
	protected GameEventSystem EventSystem;

	[Inject]
	protected PlayerManager PlayerManager;

	[Inject]
	protected TycoonManager TycoonManager;

	[Inject]
	protected ArtifactDealManager ArtifactDealManager;

	[Inject]
	protected GameChatManager GameChatManager;

	[Inject]
	protected GameCutsceneManager GameCutsceneManager;

	[Inject]
	protected GameNpcDialogManager GameNpcDialogManager;

	[Inject]
	protected GameNpcTalkManager GameNpcTalkManager;

	[Inject]
	protected GameUiEventSystem UiEventSystem;

	[Inject]
	protected GraphicSettingService GraphicSettings;

	[Inject]
	protected StorageService StorageService;

	private Selectable _selectedObject;

	private bool _stagePlaying;

	private bool _isLooting;

	private ulong MyClientId => (MyCharacterModel != null) ? MyCharacterModel.ClientId : ulong.MaxValue;

	private GamePlayer MyPlayer => GameManager.MyPlayer;

	private CharacterModel MyCharacterModel => GameManager.MyCharacterModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.InGameActions InGameActions => GameInputManager.InGameActions;

	private InputActions.InGameKeyboardActions InGameKeyboardActions => GameInputManager.InGameKeyboardActions;

	protected override Selectable SelectedObject => _selectedObject;

	protected override void Start()
	{
		base.Start();
		GraphicSettings.SetGameplayVSyncActive(active: true);
		GameManager.OnGameClosed.Subscribe(delegate
		{
			OnGameClosed();
		}).AddTo(this);
		NetworkManager.SceneManager.OnSceneEvent += OnSceneEvent;
		GameChannel.Receive<NtfDisplayNoticeToast>().Subscribe(OnDisplayNoticeToast).AddTo(this);
		GameChannel.Receive<NtfPlayerKilledToast>().Subscribe(OnPlayerKilledToast).AddTo(this);
		GameChannel.Receive<NtfStartInteraction>().Where((NtfStartInteraction _) => _.SourceClientId == MyClientId).Subscribe(OnStartInteraction)
			.AddTo(this);
		GameChannel.Receive<NtfEndInteraction>().Where((NtfEndInteraction _) => _.SourceClientId == MyClientId).Subscribe(OnEndInteraction)
			.AddTo(this);
		GameChannel.Receive<NtfItemUsed>().Subscribe(OnItemUsed).AddTo(this);
		CampaignManager.OnCampaignVoteStarted.Subscribe(OnCampaignVoteStarted).AddTo(this);
		CampaignManager.OnGiveUpCampaignVoteStarted.Subscribe(OnGiveUpCampaignVoteStarted).AddTo(this);
		RoomMatchmaker.OnCreateLobby.Subscribe(OnCreateLobby).AddTo(this);
		RoomMatchmaker.OnJoinLobby.Subscribe(OnJoinLobby).AddTo(this);
		GameManager.RxStageState.Where((EStageState _) => _ == EStageState.Playing).Subscribe(delegate
		{
			OnStagePlaying();
		}).AddTo(this);
		GameManager.RxStageState.Where((EStageState _) => _ != EStageState.Playing).Subscribe(delegate
		{
			OnStageNotPlaying();
		}).AddTo(this);
		GameManager.RxStageState.Where((EStageState _) => _ == EStageState.Ended).Subscribe(delegate
		{
			OnStageEnded();
		}).AddTo(this);
		GameManager.OnPlayerEntered.Subscribe(OnPlayerEntered).AddTo(this);
		GameManager.OnPlayerLeft.Subscribe(OnPlayerLeft).AddTo(this);
		EventSystem.OnEventOccured.Subscribe(OnEventOccured).AddTo(this);
		StorageService.OnSaveFailed.Subscribe(OnSaveFailed).AddTo(this);
	}

	private void OnSaveFailed(ESaveResult result)
	{
		PopupManager.ShowTextPopup(result.ToLocalText());
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		if (_stagePlaying)
		{
			EnableInGameInput();
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		if (_stagePlaying)
		{
			DisableInGameInput();
		}
	}

	private void EnableInGameInput()
	{
		GameInputManager.UnblockInput(EInputActionType.InGame);
		GameInputManager.UnblockInput(EInputActionType.PlayerFunc);
		GameInputManager.UnblockInput(EInputActionType.Player);
		GameInputManager.InGameActions.Menu.OnClickWithSfx().Subscribe(delegate
		{
			OnMenuClick();
		}).AddTo(InputDisposable);
		GameInputManager.InGameActions.Inventory.OnClickWithSfx().Subscribe(delegate
		{
			OnInventoryClick();
		}).AddTo(InputDisposable);
		GameInputManager.InGameActions.Minimap.OnClickWithSfx().Subscribe(delegate
		{
			OnMinimapClick();
		}).AddTo(InputDisposable);
		GameInputManager.InGameActions.MinimapEscape.OnClickWithSfx().Subscribe(delegate
		{
			OnMinimapEscapeClick();
		}).AddTo(InputDisposable);
	}

	private void DisableInGameInput()
	{
		GameInputManager.BlockInput(EInputActionType.InGame);
		GameInputManager.BlockInput(EInputActionType.PlayerFunc);
		GameInputManager.BlockInput(EInputActionType.Player);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (NetworkManager.SceneManager != null)
		{
			NetworkManager.SceneManager.OnSceneEvent -= OnSceneEvent;
		}
	}

	private async UniTask OnSceneLoading()
	{
		using (new BlockScope(false, true))
		{
			ELoadingPopupType loadingPopupType = ELoadingPopupType.Default;
			if (GameManager.IsInLounge)
			{
				StageResultPlayerInfo stageResultPlayerInfo = GameManager.StageResultInfo?.PlayerInfos.FirstOrDefault((StageResultPlayerInfo _) => _.ClientId == GameManager.LocalClientId);
				if (stageResultPlayerInfo != null)
				{
					loadingPopupType = ((stageResultPlayerInfo.State == EActionState.Escaped) ? ELoadingPopupType.EscapeSuccess : ELoadingPopupType.EscapeFail);
				}
			}
			else if (GameManager.IsInStage)
			{
				loadingPopupType = ELoadingPopupType.EnterStage;
			}
			await PopupManager.OpenAsync<GameLoadingPopupPresenter>(new GameLoadingPopupModel
			{
				LoadingPopupType = loadingPopupType,
				CharacterId = PlayerManager.SelectedCharacterId
			});
			PopupManager.CloseAll(typeof(GameLoadingPopupPresenter));
		}
	}

	protected virtual void OnSceneLoadComplete()
	{
	}

	private void OnGameClosed()
	{
		PopupManager.CloseAllWithoutSfx();
		if (GameChannel.RxConnectState.Value != ERoomConnectState.ConnectingToHost && GameChannel.RxConnectState.Value != ERoomConnectState.BackToLocalRoom)
		{
			if (GameChannel.RxConnectState.Value == ERoomConnectState.BackToTitle)
			{
				GameChannel.RxConnectState.Value = ERoomConnectState.None;
				SceneManager.LoadScene("Scene/StartScene.unity", LoadSceneMode.Single);
			}
			else
			{
				GamePlayerData playerData = PlayerManager.CreateRoomPlayerData(isHostPlayer: true);
				GameManager.BackToSinglePlay(playerData, PlayerManager.Difficulty, isUnexpectedBackToSinglePlay: true).Forget();
			}
		}
	}

	private void OnStagePlaying()
	{
		_stagePlaying = true;
		if (IsInputEnable)
		{
			EnableInGameInput();
		}
		MyCharacterModel.RxState.Where((EActionState _) => _ == EActionState.Escaped).Subscribe(delegate
		{
			OnMyCharacterEscaped();
		}).AddTo(this);
	}

	private void OnStageNotPlaying()
	{
		if (_stagePlaying)
		{
			_stagePlaying = false;
			if (IsInputEnable)
			{
				DisableInGameInput();
			}
		}
	}

	private void OnMyCharacterEscaped()
	{
		PopupManager.FindPopup<MenuPopupPresenter>()?.Close();
		ToastManager.PlayerNotice(TLocalizedText.BoardingCompleted.LocalText);
	}

	private void OnStageEnded()
	{
		if (GameManager.IsEscaped())
		{
			Observable.Timer(TimeSpan.FromSeconds(1.0)).TakeUntilDestroy(this).Subscribe(delegate
			{
				PopupManager.Open<EscapeSuccessPopupPresenter>();
			})
				.AddTo(this);
		}
		else
		{
			Observable.Timer(TimeSpan.FromSeconds(1.0)).TakeUntilDestroy(this).Subscribe(delegate
			{
				PopupManager.Open<EscapeFailPopupPresenter>();
			})
				.AddTo(this);
		}
	}

	private void OnMenuClick()
	{
		if (MyCharacterModel == null || MyCharacterModel.State == EActionState.StageReady)
		{
			ToastManager.NoticeFail(TLocalizedText.StageEnteringUnavailable.LocalText);
		}
		else
		{
			if (PopupManager.ExistInputActionPopup() || MapManager.RxIsFullScreen.Value)
			{
				return;
			}
			PopupManager.Open<MenuPopupPresenter>(new MenuPopupModel
			{
				InventoryManagementLayout = (GameManager.IsInLounge ? InventoryManagementPagePresenter.ELayout.InventoryStash : InventoryManagementPagePresenter.ELayout.InventoryOnly)
			});
		}
	}

	private void OnMinimapClick()
	{
		MapManager.RxIsFullScreen.Value = !MapManager.RxIsFullScreen.Value;
	}

	private void OnMinimapEscapeClick()
	{
		if (MapManager.RxIsFullScreen.Value)
		{
			MapManager.RxIsFullScreen.Value = false;
		}
	}

	private void OnInventoryClick()
	{
		if (MyCharacterModel == null || MyCharacterModel.State == EActionState.StageReady)
		{
			ToastManager.NoticeFail(TLocalizedText.StageEnteringUnavailable.LocalText);
		}
		else
		{
			if (PopupManager.ExistInputActionPopup())
			{
				return;
			}
			PopupManager.Open<MenuPopupPresenter>(new MenuPopupModel
			{
				PageCategory = MenuPopupPresenter.EPageCategory.InventoryManagement,
				InventoryManagementLayout = (GameManager.IsInLounge ? InventoryManagementPagePresenter.ELayout.InventoryStash : InventoryManagementPagePresenter.ELayout.InventoryOnly)
			});
		}
	}

	private void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent.SceneEventType == SceneEventType.Load)
		{
			OnSceneLoading().Forget();
		}
		else if (sceneEvent.SceneEventType == SceneEventType.LoadComplete)
		{
			OnSceneLoadComplete();
		}
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
		if (_isLooting)
		{
			PopupManager.FindPopup<MenuPopupPresenter>()?.Close();
		}
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
		IInteractiveObject interactiveObject = ObjectManager.FindInteractiveObject(message.TargetNetworkObjectId);
		if (interactiveObject == null)
		{
			return;
		}
		if (interactiveObject is NpcObject npcObject)
		{
			OnNpcInteractionAsync(npcObject).Forget();
			return;
		}
		if (!(interactiveObject is MonsterBody))
		{
			if (!(interactiveObject is RewardBox rewardBox))
			{
				if (interactiveObject is Entrance entrance)
				{
					if (entrance.LockState == ELockState.Locked)
					{
						OnKeyInteractionEntranceAsync(entrance).Forget();
					}
				}
				else if (interactiveObject is DropGoods dropGoods)
				{
					Goods displayGoods = dropGoods.DisplayGoods;
					EGoodsCategory category = displayGoods.Category;
					string localText;
					if (category == EGoodsCategory.Item)
					{
						if (dropGoods.GoodsCount < 1)
						{
							return;
						}
						TItem tItem = TItem.Table[dropGoods.GoodsId];
						GoodsSlot goodsSlot = GameManager.MyInventory.Slots.FindItemSlot(tItem.Id);
						if (GameManager.MyInventory.GetAddableItemCountWithBags(tItem.Id) < 1)
						{
							GameManager.MyCharacterController.OnShowMonologueText.OnNext(TLocalizedText.CannotCarryMoreStack.LocalText);
							return;
						}
						if (goodsSlot != null || !GameManager.MyInventory.IsSlotFull() || GameManager.MyInventory.GetAddableCountInBagStacks(tItem.Id) > 0)
						{
							return;
						}
						localText = TLocalizedText.CannotCarryMoreSlot.LocalText;
					}
					else
					{
						if ((category != EGoodsCategory.Equipment && category != EGoodsCategory.Bag) || !GameManager.MyInventory.IsSlotFull())
						{
							return;
						}
						localText = TLocalizedText.CannotCarryMoreSlot.LocalText;
					}
					GameManager.MyCharacterController.OnShowMonologueText.OnNext(localText);
				}
				else
				{
					if (!(interactiveObject is PhoneBooth phoneBooth))
					{
						return;
					}
					ECanEnterPhoneBoothResult eCanEnterPhoneBoothResult = phoneBooth.CheckCanEnter(MyClientId);
					if (eCanEnterPhoneBoothResult == ECanEnterPhoneBoothResult.Success)
					{
						Character character = GameManager.MyPlayer?.Character;
						if (character == null)
						{
							return;
						}
						float floatValue = TConst.LowWeaponDurabilityValue.FloatValue;
						GoodsSlot[] weaponSlots = character.WeaponSlots;
						for (int i = 0; i < weaponSlots.Length; i++)
						{
							if (weaponSlots[i].Goods is WeaponGoods weaponGoods && weaponGoods.HasDurability() && weaponGoods.CurrentDurability < floatValue)
							{
								ToastManager.PlayerNotice(TLocalizedText.LowWeaponDurabilityToast.LocalText, "PetAlert");
								return;
							}
						}
					}
					else
					{
						TLocalizedText tLocalizedText = ((eCanEnterPhoneBoothResult == ECanEnterPhoneBoothResult.NoWeapon) ? TLocalizedText.CannotEnterStageWithoutWeapon : TLocalizedText.CannotEnterStage);
						ToastManager.NoticeFail(tLocalizedText.LocalText);
					}
				}
				return;
			}
			if (!rewardBox.CheckStressCondition(MyCharacterModel.StressState))
			{
				GameManager.MyCharacterController.OnShowMonologueText.OnNext(TLocalizedText.NothingHappened.LocalText);
				return;
			}
			if (rewardBox.LockState == ELockState.Locked)
			{
				OnKeyInteractionRewardBoxAsync(rewardBox).Forget();
				return;
			}
			if (rewardBox.HitBoxTriggerOnOpen)
			{
				GameManager.MyCharacterController.OnShowMonologueText.OnNext(TLocalizedText.RewardBoxHitBoxTriggerd.LocalText);
				return;
			}
			if (rewardBox.LockState != ELockState.Closed && rewardBox.LockState != ELockState.Opened)
			{
				return;
			}
		}
		OnInventoryInteractionAsync().Forget();

		async UniTask OnInventoryInteractionAsync()
		{
			MyCharacterModel.CanCancelInputInteraction = false;
			LootingInventory lootingInventory = ObjectManager.FindInventory(message.TargetNetworkObjectId, MyClientId) as LootingInventory;
			if (lootingInventory != null)
			{
				_isLooting = true;
				MenuPopupPresenter menuPopupPresenter = await PopupManager.OpenAsync<MenuPopupPresenter>(new MenuPopupModel
				{
					PageCategory = MenuPopupPresenter.EPageCategory.InventoryManagement,
					InventoryManagementLayout = InventoryManagementPagePresenter.ELayout.InventoryLooting,
					LootingInventory = lootingInventory
				});
				if (menuPopupPresenter != null)
				{
					await menuPopupPresenter.Task;
				}
				_isLooting = false;
				if (!(MyCharacterModel == null))
				{
					MyCharacterModel.CanCancelInputInteraction = true;
					GameChannel.SendToHost(new ReqFinishInteractionWaiting());
				}
			}
		}

		async UniTask OnKeyInteractionEntranceAsync(Entrance targetEntrance)
		{
			KeyInteractionPopupPresenter keyInteractionPopupPresenter = await PopupManager.OpenAsync<KeyInteractionPopupPresenter>(new KeyInteractionPopupModel
			{
				InteractiveObject = targetEntrance,
				KeyItemIds = targetEntrance.TableData.KeyItemIds,
				KeyItemTypes = targetEntrance.TableData.KeyItemTypes
			});
			if (keyInteractionPopupPresenter != null)
			{
				object obj = await keyInteractionPopupPresenter.Task;
				if (obj == null)
				{
					GameChannel.SendToHost(new ReqInputCancelInteract
					{
						NetworkObjectId = targetEntrance.NetworkObjectId
					});
				}
				else if (obj is bool && (bool)obj)
				{
					GameChannel.SendToHost(new ReqFinishInteractionWaiting());
				}
			}
		}

		async UniTask OnKeyInteractionRewardBoxAsync(RewardBox targetRewardBox)
		{
			KeyInteractionPopupPresenter keyInteractionPopupPresenter = await PopupManager.OpenAsync<KeyInteractionPopupPresenter>(new KeyInteractionPopupModel
			{
				InteractiveObject = targetRewardBox,
				KeyItemIds = targetRewardBox.TableData.KeyItemIds,
				KeyItemTypes = targetRewardBox.TableData.KeyItemTypes
			});
			if (keyInteractionPopupPresenter != null)
			{
				object obj = await keyInteractionPopupPresenter.Task;
				if (obj == null)
				{
					GameChannel.SendToHost(new ReqInputCancelInteract
					{
						NetworkObjectId = targetRewardBox.NetworkObjectId
					});
				}
				else if (obj is bool && (bool)obj)
				{
					GameChannel.SendToHost(new ReqFinishInteractionWaiting());
				}
			}
		}
	}

	private async UniTask OnNpcInteractionAsync(NpcObject npcObject)
	{
		List<DialogInfo> dialogInfos = GameNpcDialogManager.GetDialogInfos(npcObject.NpcId);
		int[] array = ((!GameManager.IsHost) ? (npcObject.NpcFunctionIds?.Where((int id) => TNpcFunction.Table[id].GuestInteractable).ToArray() ?? Array.Empty<int>()) : (npcObject.NpcFunctionIds ?? Array.Empty<int>()));
		if (array.Length == 0 && dialogInfos.Count == 0)
		{
			return;
		}
		DialogInfo dialogInfo = dialogInfos.Where(ShouldSkipSelectionPopup).OrderByDescending((DialogInfo _) => _.Purpose).ThenBy((DialogInfo _) => _.Priority)
			.FirstOrDefault();
		if (dialogInfo != null)
		{
			dialogInfo.OnClicked?.Invoke();
			return;
		}
		if (array.Length == 0)
		{
			return;
		}
		int num = ((array.Length >= 2) ? (await ShowSelectionPopupAsync(array)) : array[0]);
		if (num >= 1)
		{
			ExecuteNpcFunction(num);
		}

		static bool ShouldSkipSelectionPopup(DialogInfo dialog)
		{
			if ((dialog.Type == EDialogType.Talk || dialog.Purpose != EDialogPurpose.Complete) && dialog.Type != EDialogType.Quest)
			{
				return dialog.Type == EDialogType.Mission;
			}
			return true;
		}

		async UniTask<int> ShowSelectionPopupAsync(int[] npcFunctionIds)
		{
			npcObject.DisableInteraction();
			SelectionPopupPresenter selectionPopupPresenter = await PopupManager.OpenAsync<SelectionPopupPresenter>(new SelectionPopupModel
			{
				Position = npcObject.Position,
				SelectionDatas = npcFunctionIds.Select(BuildSelectionData).ToList()
			});
			object obj = await selectionPopupPresenter.Task;
			npcObject.EnableInteraction();
			// MoveNext @0x18087c5a0: type-check of the result against Int32 (klass compare), then npcFunctionIds[index]; anything else leaves the default -1; no extra range check
			if (!(obj is int index))
			{
				return -1;
			}
			return npcFunctionIds[index];
		}

		void ExecuteNpcFunction(int npcFunctionId)
		{
			TNpcFunction tNpcFunction = TNpcFunction.Table[npcFunctionId];
			switch (tNpcFunction.Type)
			{
			case ENpcFunctionType.CharacterSelect:
				OnCharacterSetting();
				break;
			case ENpcFunctionType.CharacterSkill:
				OnCharacterSkill();
				break;
			case ENpcFunctionType.Campaign:
				OnCampaignNpcAsync().Forget();
				break;
			case ENpcFunctionType.Stash:
				OnStash();
				break;
			case ENpcFunctionType.Storage:
				PopupManager.Open<StoragePopupPresenter>();
				break;
			case ENpcFunctionType.LostGoodsStorage:
				PopupManager.Open<LostItemPopupPresenter>();
				break;
			case ENpcFunctionType.Crafting:
				PopupManager.Open<CraftingPopupPresenter>(new CraftingPopupModel
				{
					CraftingGroupId = tNpcFunction.Value1
				});
				break;
			case ENpcFunctionType.Shop:
				PopupManager.Open<ShopPopupPresenter>(new ShopPopupModel
				{
					Title = tNpcFunction.ToLocalString(),
					ShopProductGroupId = tNpcFunction.Value1
				});
				break;
			case ENpcFunctionType.ArtifactShop:
				PopupManager.Open<ArtifactShopPopupPresenter>();
				break;
			case ENpcFunctionType.ArtifactDeal:
				if (ArtifactDealManager.IsDealDone(tNpcFunction.Value1))
				{
					ToastManager.NoticeFail(TLocalizedText.ArtifactDealAlreadyDoneNpc.LocalText);
				}
				else
				{
					PopupManager.Open<ArtifactDealPopupPresenter>(new ArtifactDealPopupModel
					{
						ArtifactDealId = tNpcFunction.Value1,
						NpcObject = npcObject
					});
				}
				break;
			case ENpcFunctionType.EquipmentRepair:
				PopupManager.Open<WeaponRepairPopupPresenter>();
				break;
			case ENpcFunctionType.Scrap:
				PopupManager.Open<ScrapPopupPresenter>(new ScrapPopupModel());
				break;
			case ENpcFunctionType.RegisterItemUsed:
				PopupManager.Open<RegisterItemUsedPopupPresenter>(new RegisterItemUsedPopupModel
				{
					RegisterableType = tNpcFunction.GoodsType
				});
				break;
			case ENpcFunctionType.CoinExchange:
				PopupManager.Open<CoinExchangePopupPresenter>(new CoinExchangePopupModel());
				break;
			case ENpcFunctionType.Talent:
				PopupManager.Open<TalentPopupPresenter>(new TalentPopupModel
				{
					Title = tNpcFunction.ToLocalString(),
					TalentGroupId = tNpcFunction.Value1
				});
				break;
			case ENpcFunctionType.LevelUp:
				OnLevelUpAsync().Forget();
				break;
			case ENpcFunctionType.Rest:
				OnRestNpcAsync().Forget();
				break;
			case ENpcFunctionType.ParadoxPurify:
				PopupManager.Open<ParadoxPurifyPopupPresenter>();
				break;
			case ENpcFunctionType.ParadoxLock:
				PopupManager.Open<ParadoxLockPopupPresenter>();
				break;
			case ENpcFunctionType.DangerLevel:
				OnDifficultyNpcAsync().Forget();
				break;
			case ENpcFunctionType.MultiplayCreate:
				OnMultiplayCreateAsync().Forget();
				break;
			case ENpcFunctionType.MultiplayJoin:
				OnMultiplayJoin();
				break;
			case ENpcFunctionType.MultiplayLeave:
				OnMultiplayLeaveAsync().Forget();
				break;
			case ENpcFunctionType.Dialog:
				OnDialogAsync().Forget();
				break;
			case ENpcFunctionType.Tycoon:
				OpenTycoonMenuOrNotify();
				break;
			case ENpcFunctionType.TycoonSalesSlot:
				OpenTycoonMenuOrNotify(tNpcFunction.Value1);
				break;
			case ENpcFunctionType.Interior:
				OnInterior();
				break;
			case ENpcFunctionType.LoungeBgm:
				OnLoungeBgm();
				break;
			}
		}

		void OnCharacterSetting()
		{
			PopupManager.Open<CharacterSettingPopupPresenter>();
		}

		void OnCharacterSkill()
		{
			PopupManager.Open<SkillSelectPopupPresenter>();
		}

		async UniTask OnCampaignNpcAsync()
		{
			if (!GameManager.IsHost)
			{
				npcObject.OnShowMonologueText.OnNext(TLocalizedText.CampaignHostOnly.LocalText);
			}
			else if (GameContext.CampaignId < 1)
			{
				PopupManager.Open<CampaignPopupPresenter>();
			}
			else
			{
				if (GameManager.PlayerCount != 1 && CampaignManager.ClientStates.Where((KeyValuePair<ulong, ECampaignState> _) => _.Key != GameManager.LocalClientId).Any((KeyValuePair<ulong, ECampaignState> _) => _.Value == ECampaignState.NotCompleted))
				{
					TCampaign tCampaign = TCampaign.Table[GameContext.CampaignId];
					if (await PopupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmAbandonCampaignOtherPlayerNotCompleted.Format(tCampaign.Name), TLocalizedText.AbandonCampaignWarning.LocalText))
					{
						GameManager.StartVoteGiveUpCampaign();
					}
					return;
				}
				TCampaign tCampaign2 = TCampaign.Table[GameContext.CampaignId];
				if (await PopupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ConfirmAbandonCampaign.Format(tCampaign2.Name), TLocalizedText.AbandonCampaignWarning.LocalText))
				{
					GameManager.CloseCampaign(ECampaignCloseReason.GiveUp);
					PopupManager.Open<CampaignPopupPresenter>();
				}
			}
		}

		async UniTask OnLevelUpAsync()
		{
			DialogPopupPresenter popup = await PopupManager.OpenAsync<DialogPopupPresenter>(new DialogPopupModel
			{
				DisplayPlayerInfo = true
			});
			int level = MyPlayer.Profile.Level;
			int intValue = TConst.LevelMax.IntValue;
			if (level >= intValue)
			{
				await AppendDialogTextAsync(TLocalizedText.DialogLevelUpMaxLevel.LocalText, null);
			}
			else
			{
				TLevel tLevel = TLevel.Table[level];
				if (MyPlayer.Profile.Exp < tLevel.NeedExp)
				{
					await AppendDialogTextAsync(TLocalizedText.DialogLevelUpExpNotEnough.Format(tLevel.NeedExp), null);
				}
				else if (!PlayerManager.HasEnoughGold(tLevel.NeedGold))
				{
					await AppendDialogTextAsync(TLocalizedText.DialogLevelUpCoinNotEnough.Format(tLevel.NeedGold), null);
				}
				else if (tLevel.NeedCoin > 0 && !PlayerManager.HasEnoughCoin(tLevel.NeedCoin))
				{
					await AppendDialogTextAsync(TLocalizedText.DialogLevelUpCoinNotEnough.Format(tLevel.NeedCoin), null);
				}
				// MoveNext @0x180878060 checked: Exp, then HasEnoughGold(NeedGold) (message arg NeedGold), then NeedCoin > 0 && !HasEnoughCoin (arg NeedCoin), then DialogLevelUpEnoughAllConditon with NeedCoin
				else if (await AppendDialogTextAsync(TLocalizedText.DialogLevelUpEnoughAllConditon.Format(tLevel.NeedCoin), new string[2]
				{
					TLocalizedText.Yes.LocalText,
					TLocalizedText.No.LocalText
				}) == 0)
				{
					ResLevelUp resLevelUp = await GameChannel.RequestAsync<ResLevelUp>(new ReqLevelUp());
					if (resLevelUp == null || !resLevelUp.IsSuccess)
					{
						return;
					}
					LevelUpPopupPresenter levelUpPopupPresenter = await PopupManager.OpenAsync<LevelUpPopupPresenter>(new LevelUpPopupModel
					{
						Level = MyPlayer.Profile.Level
					});
					if (levelUpPopupPresenter != null)
					{
						await levelUpPopupPresenter.Task;
					}
					await AppendDialogTextAsync(TLocalizedText.DialogLevelUpAccept.LocalText, null);
					UiEventSystem.FlushLevelAndExp();
				}
				else
				{
					await AppendDialogTextAsync(TLocalizedText.DialogLevelUpDeny.LocalText, null);
				}
			}
			popup.SetClosable();

			async UniTask<int> AppendDialogTextAsync(string dialogMessage, string[] selectableMessages)
			{
				int? result = null;
				DialogEntry entry = CreateDialogEntryFromMessages(dialogMessage, selectableMessages);
				entry.OnCompleted.AsObservable().Subscribe(delegate(int _)
				{
					result = _;
				}).AddTo(this);
				popup.AppendDialogText(entry);
				await UniTask.WaitUntil(() => result.HasValue);
				return result.Value;
			}
		}

		void OnInterior()
		{
			if (GameManager.IsHost)
			{
				PopupManager.Open<InteriorPopupPresenter>();
			}
			else
			{
				ToastManager.NoticeFail(TLocalizedText.OnlyHostAllowed.LocalText);
			}
		}

		void OnLoungeBgm()
		{
			if (GameManager.IsHost)
			{
				PopupManager.Open<BgmPopupPresenter>();
			}
			else
			{
				ToastManager.NoticeFail(TLocalizedText.OnlyHostAllowed.LocalText);
			}
		}

		void OnStash()
		{
			PopupManager.Open<MenuPopupPresenter>(new MenuPopupModel
			{
				PageCategory = MenuPopupPresenter.EPageCategory.InventoryManagement,
				InventoryManagementLayout = InventoryManagementPagePresenter.ELayout.InventoryStash
			});
		}

		async UniTask OnRestNpcAsync()
		{
			int[] changeableParadoxIds = MyPlayer.GetChangeableParadoxIds(includePositive: true, includeNegative: true);
			if (changeableParadoxIds.Length == 0)
			{
				ToastManager.NoticeFail(TLocalizedText.NoActiveParadox.LocalText);
				return;
			}
			long num = MyPlayer.CalculateRestCost();
			if (MyPlayer.Profile.Gold < num)
			{
				ToastManager.NoticeFail(TLocalizedText.NotEnoughCurrency.LocalText);
				return;
			}
			ConsumeConfirmPopupPresenter consumeConfirmPopupPresenter = await PopupManager.OpenAsync<ConsumeConfirmPopupPresenter>(new ConsumeConfirmPopupModel
			{
				TitleText = TLocalizedText.RestToPurifyParadox.LocalText,
				DescText = TLocalizedText.RestToPurifyParadoxDesc.LocalText,
				GoldAmount = num
			});
			object obj = await consumeConfirmPopupPresenter.Task;
			if (!(obj is bool) || !(bool)obj)
			{
				return;
			}
			ResRest resRest = await GameChannel.RequestAsync<ResRest>(new ReqRest());
			if (resRest != null && resRest.IsSuccess)
			{
				ToastManager.Notice(TLocalizedText.Rested.LocalText);
				PopupManager.Open<FadePopupPresenter>(new FadePopupModel
				{
					OpenSfx = "Npc_Rest"
				});
				TParadox tParadox = TParadox.Table[resRest.RemovedParadoxId];
				GameChatManager.AddChat(new ChatInfo
				{
					Type = EChatType.System,
					Text = string.Format(TLocalizedText.RestChatFormat.LocalText, tParadox.Name),
					UtcTime = DateTime.UtcNow
				});
			}
		}

		async UniTask OnDifficultyNpcAsync()
		{
			if (RoomContext.IsSingleMode)
			{
				RoomDifficultyPopupPresenter roomDifficultyPopupPresenter = await PopupManager.OpenAsync<RoomDifficultyPopupPresenter>(new RoomDifficultyPopupModel
				{
					Difficulty = PlayerManager.Difficulty,
					PreventMaxDurabilityLoss = PlayerManager.PreventRepairMaxDurabilityLoss
				});
				if (await roomDifficultyPopupPresenter.Task is RoomDifficultyPopupResult roomDifficultyPopupResult)
				{
					PlayerManager.SetDifficulty(roomDifficultyPopupResult.Difficulty);
					PlayerManager.SetLastPlayedRoomDifficulty(roomDifficultyPopupResult.Difficulty);
					PlayerManager.SetPreventRepairMaxDurabilityLoss(roomDifficultyPopupResult.PreventMaxDurabilityLoss);
					GameContext.RxRoomDifficulty.Value = roomDifficultyPopupResult.Difficulty;
				}
			}
			else
			{
				ToastManager.Notice(TLocalizedText.DangerLevelAlreadyInLoungeMessage.LocalText);
			}
		}

		async UniTask OnMultiplayCreateAsync()
		{
			if (!AppManager.IsOnline())
			{
				ToastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
			}
			else if (GameManager.IsTutorial || !GameManager.IsHost || !RoomContext.IsSingleMode)
			{
				ToastManager.Notice(TLocalizedText.AlreadyInLoungeMessage.LocalText);
			}
			else if (GameContext.CampaignId >= 1)
			{
				ToastManager.Notice(TLocalizedText.TryPublishRoomOnPlaying.LocalText);
			}
			else if (Settings.Platform != EPlatform.Local && Settings.Platform != EPlatform.Steam)
			{
				PopupManager.ShowTextPopup(string.Format("Unsupported platform type. {0}", Settings.Platform));
			}
			else
			{
				RoomSettingPopupPresenter roomSettingPopupPresenter = await PopupManager.OpenAsync<RoomSettingPopupPresenter>(new RoomSettingPopupModel
				{
					IsCreate = true,
					RoomTitle = TLocalizedText.DefaultLobbyName.LocalText,
					PublishType = ERoomPublishType.Public,
					RoomDifficulty = PlayerManager.Difficulty
				});
				if (await roomSettingPopupPresenter.Task is RoomSettingPopupResult roomSettingPopupResult)
				{
					PlayerManager.SetDifficulty(roomSettingPopupResult.RoomDifficulty);
					PlayerManager.SetLastPlayedRoomDifficulty(roomSettingPopupResult.RoomDifficulty);
					ECreateLobbyResult eCreateLobbyResult = await GameHost.PublishRoomAsync(roomSettingPopupResult.RoomTitle, roomSettingPopupResult.PlayerCountMax, PlayerManager.Name, PlayerManager.Level, roomSettingPopupResult.PublishType, roomSettingPopupResult.RoomDifficulty);
					if (eCreateLobbyResult != ECreateLobbyResult.Success)
					{
						ToastManager.NoticeFail(eCreateLobbyResult.ToLocalText());
					}
				}
			}
		}

		void OnMultiplayJoin()
		{
			if (!AppManager.IsOnline())
			{
				ToastManager.Notice(TLocalizedText.TryPublishRoomOnOffline.LocalText);
				return;
			}
			if (!GameManager.IsTutorial && GameManager.IsHost && RoomContext.IsSingleMode)
			{
				if (GameContext.CampaignId >= 1)
				{
					ToastManager.Notice(TLocalizedText.TryOpenRoomListOnPlaying.LocalText);
					return;
				}
				switch (Settings.Platform)
				{
				case EPlatform.Local:
					PopupManager.OpenAsync<LocalRoomConnectionPopupPresenter>().Forget();
					break;
				case EPlatform.Steam:
					PopupManager.OpenAsync<RoomListPopupPresenter>().Forget();
					break;
				default:
					PopupManager.ShowTextPopup(string.Format("Unsupported platform type. {0}", Settings.Platform));
					break;
				}
				return;
			}
			ToastManager.Notice(TLocalizedText.AlreadyInLoungeMessage.LocalText);
		}

		async UniTask OnMultiplayLeaveAsync()
		{
			if (!RoomContext.IsSingleMode)
			{
				if (!GameManager.IsTutorial && await PopupManager.ShowConfirmationTextPopupAsync(TLocalizedText.ExitTheLoungeDescription.LocalText))
				{
					GamePlayerData playerData = PlayerManager.CreateRoomPlayerData(isHostPlayer: true);
					GameManager.BackToSinglePlay(playerData, PlayerManager.Difficulty, isUnexpectedBackToSinglePlay: true).Forget();
				}
			}
			else
			{
				ToastManager.NoticeFail(TLocalizedText.NotInLoungeMessage.LocalText);
			}
		}

		async UniTask OnDialogAsync()
		{
			while (await ShowDialogSelectionAsync())
			{
			}
		}

		async UniTask<bool> ShowDialogSelectionAsync()
		{
			List<DialogInfo> sortedDialogInfos = BuildSortedDialogInfos();
			if (sortedDialogInfos.Count == 0)
			{
				return false;
			}
			DialogPopupPresenter dialogPopupPresenter = await PopupManager.OpenAsync<DialogPopupPresenter>(new DialogPopupModel());
			UniTaskCompletionSource<DialogInfo> resultSource = new UniTaskCompletionSource<DialogInfo>();
			string defaultDialog = TNpc.Table[npcObject.NpcId].DefaultDialog;
			SelectableMessageData[] selectableMessageDatas = BuildSelectableMessageDatas(sortedDialogInfos);
			DialogEntry dialogEntry = CreateDialogEntry(defaultDialog, selectableMessageDatas);
			dialogEntry.OnCompleted.AsObservable().Subscribe(delegate(int index)
			{
				resultSource.TrySetResult(sortedDialogInfos.ElementAtOrDefault(index));
			}).AddTo(this);
			dialogPopupPresenter.AppendDialogText(dialogEntry);
			using (GameNpcDialogManager.OnDialogChanged.Where((int changedNpcId) => changedNpcId == npcObject.NpcId).Subscribe(delegate
			{
				sortedDialogInfos = BuildSortedDialogInfos();
				if (sortedDialogInfos.Count == 0)
				{
					resultSource.TrySetResult(null);
				}
				else if (dialogPopupPresenter != null)
				{
					dialogPopupPresenter.RefreshSelections(BuildSelectableMessageDatas(sortedDialogInfos));
				}
			}))
			{
				DialogInfo dialogInfo2 = await resultSource.Task;
				dialogPopupPresenter.Close();
				if (dialogInfo2 == null)
				{
					return false;
				}
				GameNpcTalkManager.MarkDialogViewed(dialogInfo2.Key);
				dialogInfo2.OnClicked?.Invoke();
				await WaitForLuaDialogAsync(dialogPopupPresenter);
				return true;
			}
		}

		async UniTask WaitForLuaDialogAsync(DialogPopupPresenter previousPopup)
		{
			DialogPopupPresenter luaPopup = null;
			await UniTask.WaitUntil(delegate
			{
				luaPopup = PopupManager.FindPopup<DialogPopupPresenter>();
				return luaPopup != null && luaPopup != previousPopup;
			});
			await luaPopup.Task;
		}

		SelectionData BuildSelectionData(int npcFunctionId)
		{
			TNpcFunction tNpcFunction = TNpcFunction.Table[npcFunctionId];
			return new SelectionData
			{
				InteractionName = tNpcFunction.ToLocalString(),
				ShowQuestMark = HasQuestStartDialog(tNpcFunction)
			};
		}

		bool HasQuestStartDialog(TNpcFunction npcFunctionData)
		{
			if (npcFunctionData.Type != ENpcFunctionType.Dialog)
			{
				return false;
			}
			return GameNpcDialogManager.GetDialogInfos(npcFunctionData.NpcId).Any((DialogInfo dialog) => dialog.Type != EDialogType.Talk && dialog.Purpose == EDialogPurpose.Start);
		}

		List<DialogInfo> BuildSortedDialogInfos()
		{
			return dialogInfos.OrderBy((DialogInfo _) => (_.Type == EDialogType.None) ? int.MaxValue : ((int)_.Type)).ThenBy((DialogInfo _) => _.Purpose).ThenBy((DialogInfo _) => _.Priority)
				.ToList();
		}

		SelectableMessageData[] BuildSelectableMessageDatas(List<DialogInfo> sorted)
		{
			return sorted.Select((DialogInfo _) => new SelectableMessageData
			{
				Message = _.Title,
				DialogType = _.Type,
				HasRead = (_.Type == EDialogType.Talk && !GameNpcTalkManager.IsNewDialog(_.Key))
			}).Append(new SelectableMessageData
			{
				Message = TLocalizedText.DialogBack.LocalText
			}).ToArray();
		}

		DialogEntry CreateDialogEntry(string dialogMessage, SelectableMessageData[] selectableMessageDatas)
		{
			DialogEntry dialogEntry = new DialogEntry();
			DialogSpeakerEntry[] array2 = new DialogSpeakerEntry[3];
			int npcId = npcObject.NpcId;
			string imageName = string.Format("{0}_{1}", npcObject.NpcId, TNpc.Table[npcObject.NpcId].DefaultImageName);
			array2[1] = new DialogSpeakerEntry(ESpeaker.Npc, npcId, imageName);
			dialogEntry.Speakers = array2;
			dialogEntry.SpeakerIndex = 1;
			dialogEntry.Message = dialogMessage;
			dialogEntry.SelectableMessageDatas = selectableMessageDatas;
			dialogEntry.AllowEscapeClose = true;
			return dialogEntry;
		}

		DialogEntry CreateDialogEntryFromMessages(string dialogMessage, string[] selectableMessages)
		{
			DialogEntry dialogEntry = new DialogEntry();
			DialogSpeakerEntry[] array2 = new DialogSpeakerEntry[3];
			int npcId = npcObject.NpcId;
			string imageName = string.Format("{0}_Default", npcObject.NpcId);
			array2[1] = new DialogSpeakerEntry(ESpeaker.Npc, npcId, imageName);
			dialogEntry.Speakers = array2;
			dialogEntry.SpeakerIndex = 1;
			dialogEntry.Message = dialogMessage;
			dialogEntry.SelectableMessageDatas = selectableMessages?.Select((string message) => new SelectableMessageData
			{
				Message = message
			}).ToArray();
			return dialogEntry;
		}
	}

	private void OpenTycoonMenuOrNotify(int initialSalesSlotId = 0)
	{
		TycoonManager.CheckAndUnlockSalesSlots();
		if (PlayerManager.GetUnlockedSalesSlotIndices().Count == 0)
		{
			ToastManager.NoticeFail(TLocalizedText.FeatureLocked.LocalText);
			return;
		}
		PopupManager.Open<TycoonMenuPopupPresenter>(new TycoonMenuPopupModel
		{
			InitialSalesSlotId = initialSalesSlotId
		});
	}

	private void OnCampaignVoteStarted((int campaignId, EDifficulty difficulty, EAnomalyType anomalyType) voteData)
	{
		PopupManager.Open<CampaignVotePopupPresenter>(new CampaignVotePopupModel
		{
			CampaignId = voteData.campaignId,
			Difficulty = voteData.difficulty,
			AnomalyType = voteData.anomalyType
		});
	}

	private void OnDisplayNoticeToast(NtfDisplayNoticeToast message)
	{
		ToastManager.Notice(LocalizationUtility.GetLocalizedText(message.TextKey));
	}

	private void OnPlayerKilledToast(NtfPlayerKilledToast message)
	{
		string text;
		if (message.KillerMonsterTableId < 1)
		{
			text = ((!string.IsNullOrEmpty(message.KillerName)) ? message.KillerName : TLocalizedText.Unknown.LocalText);
		}
		else
		{
			text = TMonster.Table[message.KillerMonsterTableId].Name;
		}
		ToastManager.Notice(TLocalizedText.PlayerKilledByFormat.Format(message.PlayerName, text));
	}

	private void OnGiveUpCampaignVoteStarted(Unit unit)
	{
		PopupManager.Open<CampaignGiveUpVotePopupPresenter>(new CampaignGiveUpVotePopupModel());
	}

	private void OnItemUsed(NtfItemUsed message)
	{
		TItem tItem = TItem.Table[message.ItemId];
		if (tItem.GoodsType == EGoodsType.Note)
		{
			PopupManager.Open<NoteSystemPopupPresenter>(new NoteSystemPopupModel
			{
				Texts = new string[1] { tItem.Note },
				NoteType = tItem.NoteType,
				BackGroundAlpha = 0.5f
			});
		}
	}

	private void OnCreateLobby(Unit _)
	{
		ToastManager.Notice(TLocalizedText.CreateLobbyMessage.LocalText);
	}

	private void OnJoinLobby(bool isHost)
	{
		if (!isHost)
		{
			ToastManager.Notice(TLocalizedText.JoinLobbyMessage.LocalText);
		}
	}

	private void OnPlayerEntered(GamePlayer player)
	{
		if (player.ClientId != GameManager.LocalClientId)
		{
			ToastManager.Notice(string.Format(TLocalizedText.ChatPlayerEntered.LocalText, player.Name), "GuestEnter");
		}
	}

	private void OnPlayerLeft(GamePlayer player)
	{
		if (player.ClientId != GameManager.LocalClientId)
		{
			ToastManager.Notice(string.Format(TLocalizedText.ChatPlayerLeft.LocalText, player.Name), "GuestLeave");
		}
	}

	private void OnEventOccured(IBaseEvent baseEvent)
	{
		if (baseEvent is NpcUnlockEvent npcUnlockEvent)
		{
			OnNpcUnlock(npcUnlockEvent).Forget();
		}

		async UniTask OnNpcUnlock(NpcUnlockEvent npcUnlockEvent2)
		{
			int[] npcIds = npcUnlockEvent2.NpcIds;
			foreach (int npcId in npcIds)
			{
				if (TNpc.Table[npcId].ShowNewContentPopup)
				{
					NewContentPopupPresenter newContentPopupPresenter = await PopupManager.OpenAsync<NewContentPopupPresenter>(new NewContentPopupModel
					{
						NpcId = npcId
					});
					if (newContentPopupPresenter != null)
					{
						await newContentPopupPresenter.Task;
					}
				}
			}
		}
	}
}
