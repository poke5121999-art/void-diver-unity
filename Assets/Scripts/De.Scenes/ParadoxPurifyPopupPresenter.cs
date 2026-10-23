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

public class ParadoxPurifyPopupPresenter : GamePopupPresenter
{
	[View]
	private ParadoxPurifyPopupView _view;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	private int _selectedSlotIndex = -1;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "ParadoxUI";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		RefreshSlots();
		await RefreshGamePadNav();
		_gameManager.MyPlayer.RxCharacter.Skip(1).Subscribe(OnCharacterChanged).AddTo(this);
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
		int[] negativeParadoxIds = character.NegativeParadoxIds;
		int num = ((negativeParadoxIds != null) ? negativeParadoxIds.Length : 0);
		_view.Empty.gameObject.SetActive(num < 1);
		for (int i = 0; i < _view.Slots.Length; i++)
		{
			GameObject gameObject = _view.Slots[i];
			if (i < num && character.NegativeParadoxIds[i] != 0)
			{
				int num2 = character.NegativeParadoxIds[i];
				TParadox data = TParadox.Table[num2];
				ParadoxSlotModel paradoxSlotModel = new ParadoxSlotModel
				{
					SlotIndex = i,
					ParadoxId = num2,
					Cost = data.GetRemovalGoodsData()
				};
				gameObject.SetActive(true);
				gameObject.GetComponent<ParadoxSlotPresenter>().Inject(paradoxSlotModel);
				paradoxSlotModel.OnClick.Subscribe(OnSlotClick).AddTo(this);
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
		TryPurifyParadoxAsync(slotModel.ParadoxId).Forget();
	}

	private async UniTask<bool> TryPurifyParadoxAsync(int paradoxId)
	{
		TParadox paradoxData = TParadox.Table[paradoxId];
		GoodsData consumeGoods = paradoxData.GetRemovalGoodsData();
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
			Text = TLocalizedText.ParadoxPurifyConfirm.LocalText,
			CostCaptionText = TLocalizedText.UParadoxConfirmPopupCostCaption.LocalText,
			Cost = consumeGoods
		});
		if (!(bool)await popup.Task)
		{
			return false;
		}
		ResParadoxPurify resParadoxPurify = await _gameChannel.RequestAsync<ResParadoxPurify>(new ReqParadoxPurify { ParadoxId = paradoxId });
		if (resParadoxPurify == null || !resParadoxPurify.IsSuccess)
		{
			return false;
		}
		_toastManager.Notice(TLocalizedText.ParadoxPurifyCompletedFormat.Format(paradoxData.Name));
		_soundManager.PlaySfx("ParadoxHeal");
		return true;
	}
}
