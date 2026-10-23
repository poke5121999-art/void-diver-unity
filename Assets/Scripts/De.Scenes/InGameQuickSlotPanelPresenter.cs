using UnityEngine;
using UniRx.Triggers;
using UniRx;
using NemoLib.Popup;
using GameCommon.Table;
using GameCommon.Data;
using System.Linq;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Scene;
using VContainer;

public class InGameQuickSlotPanelPresenter : BasePanelPresenter
{
	[View]
	private InGameQuickSlotPanelView _view;

	[InjectModel]
	private InGameQuickSlotPanelModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerInputController _inputController;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SceneManager _sceneManager;

	[Inject]
	private SoundManager _soundManager;

	private int _selectedSlotIndex;

	protected override BasePanelModel PanelModel => _model;

	private GamePlayer Player => _gameManager.MyPlayer;

	private InputActions.InGamePadActions InGamePadActions => InputManager.InGamePadActions;

	private int EquipmentSwapSlotIndex => -1;

	private void Start()
	{
		InputManager.RxControlDevice.Subscribe(OnControlDeviceChanged).AddTo(this);
		InitGamePadUiActions();
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
		{
			OnCharacterReady();
		}).AddTo(this);
		_view.GoodsTooltip.gameObject.SetActive(false);
	}

	private void OnCharacterReady()
	{
		BuildWeaponSwapSlot();
		BuildQuickSlots();
		MoveSelectionIndicator(_selectedSlotIndex);
		_gameManager.MyPlayer.RxCharacter.Pairwise().Where((Pair<Character> _) => _.Previous != null && _.Current != null && _.Previous.ActiveWeaponIndex != _.Current.ActiveWeaponIndex).Subscribe(delegate
		{
			OnActiveWeaponChanged();
		}).AddTo(this);
	}

	private void InitGamePadUiActions()
	{
		InGamePadActions.QuickSlotMoveLeft.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnQuickSlotMoveLeft();
		}).AddTo(this);
		InGamePadActions.QuickSlotMoveRight.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnQuickSlotMoveRight();
		}).AddTo(this);
		InGamePadActions.QuickSlotUse.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnQuickSlotUseDown();
		}).AddTo(this);
		InGamePadActions.QuickSlotUse.OnPressEndAsObservable().Subscribe(delegate
		{
			OnQuickSlotUseUp();
		}).AddTo(this);
	}

	private void OnControlDeviceChanged(EControlDevice device)
	{
		_view.SelectionIndicator.gameObject.SetActive(device == EControlDevice.GamePad);
		if (device == EControlDevice.GamePad)
		{
			MoveSelectionIndicator(_selectedSlotIndex);
		}
	}

	private void MoveSelectionIndicator(int slotIndex)
	{
		if (slotIndex < 0)
		{
			_view.SelectionIndicator.rectTransform.position = _view.WeaponSwapSlot.transform.position;
		}
		else
		{
			_view.SelectionIndicator.rectTransform.position = _view.Slots[slotIndex].transform.position;
		}
	}

	private void BuildWeaponSwapSlot()
	{
		InGameWeaponSwapSlotModel inGameWeaponSwapSlotModel = new InGameWeaponSwapSlotModel();
		inGameWeaponSwapSlotModel.IsActiveWeapon = true;
		inGameWeaponSwapSlotModel.OnLeftClick.Subscribe(OnWeaponSwapSlotClick).AddTo(this);
		inGameWeaponSwapSlotModel.OnEnter.Subscribe(delegate(InGameWeaponSwapSlotModel _)
		{
			OnWeaponSwapSlotEnter(_.IsActiveWeapon);
		}).AddTo(this);
		inGameWeaponSwapSlotModel.OnExit.Subscribe(delegate
		{
			OnWeaponSwapSlotExit();
		}).AddTo(this);
		_view.WeaponSwapSlot.Inject(inGameWeaponSwapSlotModel);
		InGameWeaponSwapSlotModel inGameWeaponSwapSlotModel2 = new InGameWeaponSwapSlotModel();
		inGameWeaponSwapSlotModel2.IsActiveWeapon = false;
		inGameWeaponSwapSlotModel2.OnEnter.Subscribe(delegate(InGameWeaponSwapSlotModel _)
		{
			OnWeaponSwapSlotEnter(_.IsActiveWeapon);
		}).AddTo(this);
		inGameWeaponSwapSlotModel2.OnExit.Subscribe(delegate
		{
			OnWeaponSwapSlotExit();
		}).AddTo(this);
		_view.SubWeapon_WeaponSwapSlot.Inject(inGameWeaponSwapSlotModel2);
	}

	private void OnWeaponSwapSlotEnter(bool isActiveWeapon)
	{
		Character value = _gameManager.MyPlayer.RxCharacter.Value;
		GoodsSlot goodsSlot = (isActiveWeapon ? value.ActiveWeaponSlot : value.SubWeaponSlot);
		if (goodsSlot != null)
		{
			Goods goods = goodsSlot.Goods;
			if (goods != null)
			{
				_view.GoodsTooltip.Inject(new GoodsTooltipModel
				{
					Goods = goods
				});
				_view.GoodsTooltip.gameObject.SetActive(true);
			}
		}
	}

	private void OnWeaponSwapSlotExit()
	{
		_view.GoodsTooltip.gameObject.SetActive(false);
	}

	private void BuildQuickSlots()
	{
		for (int i = 0; i < _view.Slots.Length; i++)
		{
			InGameQuickSlotModel inGameQuickSlotModel = new InGameQuickSlotModel();
			inGameQuickSlotModel.Index = i;
			inGameQuickSlotModel.OnEnter.Subscribe(delegate(InGameQuickSlotModel m)
			{
				OnQuickSlotEnter(m.Index);
			}).AddTo(this);
			inGameQuickSlotModel.OnExit.Subscribe(delegate(InGameQuickSlotModel m)
			{
				OnQuickSlotExit(m.Index);
			}).AddTo(this);
			inGameQuickSlotModel.OnLeftClick.Subscribe(OnQuickSlotClick).AddTo(this);
			_view.Slots[i].GetComponent<InGameQuickSlotPresenter>().Inject(inGameQuickSlotModel);
		}
	}

	private void OnWeaponSwapSlotClick(InGameWeaponSwapSlotModel model)
	{
		if (_gameManager.IsParticipant)
		{
			_gameChannel.SendToHost(new ReqSwapWeapon());
		}
	}

	private void OnQuickSlotEnter(int slotIndex)
	{
		if (_playerManager.TryGetQuickSlotItemId(slotIndex, out var itemId))
		{
			long inventoryGoodsCount = _playerManager.GetInventoryGoodsCount(GoodsCreator.MakeItemGoods(itemId, 0L, 0L));
			_view.GoodsTooltip.Inject(new GoodsTooltipModel
			{
				Goods = GoodsCreator.MakeItemGoods(itemId, inventoryGoodsCount, 0L)
			});
			_view.GoodsTooltip.gameObject.SetActive(true);
		}
	}

	private void OnQuickSlotExit(int slotIndex)
	{
		_view.GoodsTooltip.gameObject.SetActive(false);
	}

	private void OnQuickSlotClick(InGameQuickSlotModel model)
	{
		if (!_playerManager.TryGetQuickSlotItemId(model.Index, out var itemId))
		{
			return;
		}
		if (_inputController.IsPingPressed)
		{
			if (_gameManager.MyCharacterModel.IsHitable)
			{
				EPingResult ePingResult = _pingManager.TrySendPingByItem(itemId);
				if (ePingResult != EPingResult.Ok)
				{
					_toastManager.Notice(ePingResult.ToLocalText());
				}
			}
			else
			{
				_toastManager.NoticeFail(TLocalizedText.ActionNotAvailableWhileDead.LocalText);
			}
		}
		else if (_gameManager.IsParticipant)
		{
			TrySendUseItem(itemId, ESkillInputEvent.ItemSkillDown);
		}
	}

	private bool TrySendUseItem(int itemId, ESkillInputEvent inputEvent)
	{
		if (!_gameManager.IsParticipant)
		{
			return false;
		}
		if (_gameManager.MyCharacterModel.HasBuffEffect(EBuffEffectType.ItemBlocked))
		{
			return false;
		}
		if (itemId < 1)
		{
			return false;
		}
		_gameChannel.SendToHost(new ReqInputUseItem
		{
			ItemId = itemId,
			InputEvent = inputEvent
		});
		return true;
	}

	private void OnActiveWeaponChanged()
	{
		_soundManager.PlaySfx("Weapon_Swap");
		_view.SubWeapon.GetComponent<Animator>().SetTrigger("Swap");
	}

	private void OnQuickSlotMoveLeft()
	{
		_selectedSlotIndex--;
		if (_selectedSlotIndex < -1)
		{
			_selectedSlotIndex = _view.Slots.Length - 1;
		}
		MoveSelectionIndicator(_selectedSlotIndex);
	}

	private void OnQuickSlotMoveRight()
	{
		_selectedSlotIndex++;
		if (_selectedSlotIndex >= _view.Slots.Length)
		{
			_selectedSlotIndex = -1;
		}
		MoveSelectionIndicator(_selectedSlotIndex);
	}

	private void OnQuickSlotUseDown()
	{
		if (!_gameManager.IsParticipant)
		{
			return;
		}
		if (_selectedSlotIndex < 0)
		{
			_gameChannel.SendToHost(new ReqSwapWeapon());
		}
		else if (_playerManager.TryGetQuickSlotItemId(_selectedSlotIndex, out var itemId))
		{
			TrySendUseItem(itemId, ESkillInputEvent.ItemSkillDown);
		}
	}

	private void OnQuickSlotUseUp()
	{
		if (_gameManager.IsParticipant && _selectedSlotIndex >= 0 && _playerManager.TryGetQuickSlotItemId(_selectedSlotIndex, out var itemId))
		{
			TrySendUseItem(itemId, ESkillInputEvent.ItemSkillUp);
		}
	}
}
