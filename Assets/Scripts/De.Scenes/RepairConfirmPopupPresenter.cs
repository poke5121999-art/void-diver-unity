using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class RepairConfirmPopupPresenter : GamePopupPresenter
{
	[View]
	private RepairConfirmPopupView _view;

	[InjectModel]
	private RepairConfirmPopupModel _model;

	[Inject]
	private PlayerManager _playerManager;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputActions.UiKeyboardActions UiKeyboardActions => GameInputManager.UiKeyboardActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		UpdateUi();
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(false);
		}).AddTo(this);
		InitGamePadNav();
	}

	private void InitGamePadNav()
	{
		SetNavHorizontal(new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button });
		SetDefaultNav(_view.Confirm_Button);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Confirm.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(true);
		}).AddTo(InputDisposable);
		UiKeyboardActions.Cancel.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(false);
		}).AddTo(InputDisposable);
	}

	private void UpdateUi()
	{
		bool preventMaxDurabilityLoss = _playerManager.PreventRepairMaxDurabilityLoss;
		string text = TLocalizedText.URepairConfirmPopupCenterDescText.LocalText;
		if (!preventMaxDurabilityLoss)
		{
			text = text + "\n" + TLocalizedText.URepairConfirmPopupCenterDescDurabilityText.LocalText;
		}
		_view.Canter_DescText.text = text;
		if (_model.IsRepairAll)
		{
			_view.Title_Text.text = TLocalizedText.RepairAllConfirmTitle.LocalText;
			_view.Title_SubText.gameObject.SetActive(!preventMaxDurabilityLoss);
			_view.Canter_Durability.SetActive(value: false);
			_view.Canter_Price_Gold_Value.text = string.Format("{0}", _model.OverrideRepairCost);
			return;
		}
		Goods goods = _model.Goods;
		_view.Title_Text.text = TLocalizedText.RepairConfirmTitleFormat.Format(goods.GetName());
		_view.Title_SubText.gameObject.SetActive(value: false);
		_view.Canter_Price_Gold_Value.text = string.Format("{0}", goods.GetRepairCost());
		_view.Canter_Durability_Valued_Before.text = string.Format("{0}", goods.GetMaxDurability());
		Goods goods2 = goods.Repair(_playerManager.PreventRepairMaxDurabilityLoss);
		_view.Canter_Durability_Valued_After.text = string.Format("{0}", goods2.GetMaxDurability());
	}

	private void Confirm()
	{
		Close(true);
	}
}
