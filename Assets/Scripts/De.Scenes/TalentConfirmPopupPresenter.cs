using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TalentConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private TalentConfirmPopupView _view;

	[InjectModel]
	private TalentConfirmPopupModel _model;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private ToastManager _toastManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "InventoryPopupOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		TTalent tTalent = TTalent.Table[_model.TalentId];
		_view.Name.text = tTalent.Name;
		SetCostRow(_view.NeedGold, _view.NeedGold_Text, tTalent.NeedGold, _playerManager.HasEnoughGold(tTalent.NeedGold));
		SetCostRow(_view.NeedCoin, _view.NeedCoin_Text, tTalent.NeedCoin, _playerManager.HasEnoughCoin(tTalent.NeedCoin));
		SetCostRow(_view.NeedRealCoin, _view.NeedRealCoin_Text, tTalent.NeedRealCoin, _playerManager.HasEnoughGoods(GoodsCreator.MakeItemGoods(10001, tTalent.NeedRealCoin)));
		TalentSlotModel talentSlotModel = new TalentSlotModel();
		talentSlotModel.TalentId = _model.TalentId;
		_view.TalentSlot.Inject(talentSlotModel);
		Goods[] array = tTalent.MaterialItemDatas?.Select((GoodsData d) => d.ToGoodsAsBase()).ToArray();
		bool flag = array != null && array.Any();
		_view.Material.SetActive(flag);
		_view.MaterialSlots.First().transform.parent.gameObject.SetActive(flag);
		if (flag)
		{
			for (int i = 0; i < _view.MaterialSlots.Length; i++)
			{
				GameObject gameObject = _view.MaterialSlots[i];
				gameObject.SetActive(i < array.Length);
				if (i < array.Length)
				{
					MaterialSlotPresenter component = gameObject.GetComponent<MaterialSlotPresenter>();
					MaterialSlotModel materialSlotModel = new MaterialSlotModel();
					materialSlotModel.Goods = array[i];
					component.Inject(materialSlotModel);
				}
			}
		}
		_view.Cancel_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		InitGamePadNav();
		static void SetCostRow(GameObject row, TextMeshProUGUI text, long amount, bool hasEnough)
		{
			row.SetActive(amount > 0);
			if (amount > 0)
			{
				string text2 = amount.ToNumberString();
				if (!hasEnough)
				{
					text2 = text2.WithColor("#a45646");
				}
				text.text = text2;
			}
		}
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		UiKeyboardActions.Confirm.OnClickWithSfx().Subscribe(delegate
		{
			Close(true);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
	}
}
