using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ParadoxLockPopupPresenter : GamePopupPresenter
{
	[View]
	private ParadoxLockPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private int _selectedSlotIndex = -1;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "ParadoxUI";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private GamePlayer MyPlayer => _gameManager.MyPlayer;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		RefreshSlots();
		await RefreshGamePadNav();
		MyPlayer.RxCharacter.Skip(1).Subscribe(OnCharacterChanged).AddTo(this);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
	}

	private void OnCharacterChanged(Character character)
	{
		RefreshSlots();
		RefreshGamePadNav().Forget();
	}

	private void RefreshSlots()
	{
		Character character = _gameManager.MyPlayer.RxCharacter.Value;
		int[] positiveParadoxIds = character.PositiveParadoxIds;
		int num = ((positiveParadoxIds != null) ? positiveParadoxIds.Length : 0);
		_view.Empty.gameObject.SetActive(num < 1);
		_view.LockedSlot.gameObject.SetActive(character.LockedParadoxId > 0);
		if (character.LockedParadoxId > 0)
		{
			TParadox data = TParadox.Table[character.LockedParadoxId];
			ParadoxSlotModel paradoxSlotModel = new ParadoxSlotModel
			{
				ParadoxId = character.LockedParadoxId,
				Cost = data.GetUnlockGoodsData()
			};
			_view.LockedSlot.GetComponent<ParadoxSlotPresenter>().Inject(paradoxSlotModel);
			paradoxSlotModel.OnClick.Subscribe(OnLockedSlotClick).AddTo(this);
		}
		int[] array = ((character.PositiveParadoxIds == null) ? System.Array.Empty<int>() : character.PositiveParadoxIds.Where((int id) => id != character.LockedParadoxId).ToArray());
		for (int i = 0; i < _view.Slots.Length; i++)
		{
			GameObject gameObject = _view.Slots[i];
			if (i < array.Length && array[i] != 0)
			{
				int num2 = array[i];
				TParadox data2 = TParadox.Table[num2];
				ParadoxSlotModel paradoxSlotModel2 = new ParadoxSlotModel
				{
					SlotIndex = i,
					ParadoxId = num2,
					Cost = data2.GetLockGoodsData()
				};
				gameObject.SetActive(true);
				gameObject.GetComponent<ParadoxSlotPresenter>().Inject(paradoxSlotModel2);
				paradoxSlotModel2.OnClick.Subscribe(OnSlotClick).AddTo(this);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	private async UniTask RefreshGamePadNav()
	{
		await UniTask.Yield();
		ClearNav();
		List<Button> list = _view.Slots.Where((GameObject s) => s.activeSelf).Select((GameObject s) => s.GetComponent<Button>()).ToList();
		if (list.Any())
		{
			SetNavVertical(list);
			int index = ((_selectedSlotIndex >= 0) ? _selectedSlotIndex : 0);
			if (index > list.Count - 1)
			{
				index = list.Count - 1;
			}
			SetDefaultNav(list[index]);
		}
	}

	private void OnSlotClick(ParadoxSlotModel slotModel)
	{
		_selectedSlotIndex = slotModel.SlotIndex;
		TryLockParadoxAsync(slotModel.ParadoxId).Forget();
	}

	private void OnLockedSlotClick(ParadoxSlotModel slotModel)
	{
		TryUnlockParadoxAsync(slotModel.ParadoxId).Forget();
	}

	private async UniTask<bool> TryLockParadoxAsync(int paradoxId)
	{
		TParadox data = TParadox.Table[paradoxId];
		if (paradoxId == _gameManager.MyPlayer.Character.LockedParadoxId)
		{
			_toastManager.NoticeFail(TLocalizedText.ParadoxLockAlreadyFixed.LocalText);
			return false;
		}
		GoodsData consumeGoods = data.GetLockGoodsData();
		if (consumeGoods.Count < 1)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotDeleteParadoxRank.LocalText);
			return false;
		}
		if (!_playerManager.HasEnoughGoods(consumeGoods))
		{
			_toastManager.NoticeFail(TLocalizedText.InsufficientFunds.LocalText);
			return false;
		}
		ParadoxConfirmPopupPresenter popup = await _popupManager.OpenAsync<ParadoxConfirmPopupPresenter>(new ParadoxConfirmPopupModel
		{
			ParadoxId = paradoxId,
			Text = TLocalizedText.ParadoxLockConfirm.LocalText,
			SubText = TLocalizedText.ParadoxLockPreviousReleased.LocalText,
			CostCaptionText = TLocalizedText.UParadoxConfirmPopupLockCostCaption.LocalText,
			Cost = consumeGoods
		});
		if (!(bool)await popup.Task)
		{
			return false;
		}
		if (_playerManager.HasEnoughGoods(consumeGoods))
		{
			ResParadoxLock resParadoxLock = await _gameChannel.RequestAsync<ResParadoxLock>(new ReqParadoxLock { ParadoxId = paradoxId });
			if (resParadoxLock == null || !resParadoxLock.IsSuccess)
			{
				return false;
			}
			_toastManager.Notice(TLocalizedText.ParadoxLockCompleted.LocalText);
			_soundManager.PlaySfx("ParadoxHeal");
			return true;
		}
		Logger.Error(string.Format("ConsumeGoods={0}, CurrentGold={1}", consumeGoods, _playerManager.Gold));
		return false;
	}

	private async UniTask<bool> TryUnlockParadoxAsync(int paradoxId)
	{
		TParadox data = TParadox.Table[paradoxId];
		GoodsData consumeGoods = data.GetUnlockGoodsData();
		if (consumeGoods.Count < 1)
		{
			_toastManager.NoticeFail(TLocalizedText.CannotDeleteParadoxRank.LocalText);
			return false;
		}
		if (!_playerManager.HasEnoughGoods(consumeGoods))
		{
			_toastManager.NoticeFail(TLocalizedText.InsufficientFunds.LocalText);
			return false;
		}
		ParadoxConfirmPopupPresenter popup = await _popupManager.OpenAsync<ParadoxConfirmPopupPresenter>(new ParadoxConfirmPopupModel
		{
			ParadoxId = paradoxId,
			Text = TLocalizedText.ParadoxUnlockConfirm.LocalText,
			CostCaptionText = TLocalizedText.ParadoxUnlockCost.LocalText,
			Cost = consumeGoods
		});
		if (!(bool)await popup.Task)
		{
			return false;
		}
		if (_playerManager.HasEnoughGoods(consumeGoods))
		{
			ResParadoxUnlock resParadoxUnlock = await _gameChannel.RequestAsync<ResParadoxUnlock>(new ReqParadoxUnlock { ParadoxId = paradoxId });
			if (resParadoxUnlock == null || !resParadoxUnlock.IsSuccess)
			{
				return false;
			}
			_toastManager.Notice(TLocalizedText.ParadoxUnlockCompleted.LocalText);
			_soundManager.PlaySfx("ParadoxHeal");
			return true;
		}
		Logger.Error(string.Format("ConsumeGoods={0}, CurrentGold={1}", consumeGoods, _playerManager.Gold));
		return false;
	}
}
