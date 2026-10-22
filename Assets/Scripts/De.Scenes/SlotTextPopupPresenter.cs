using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;

public class SlotTextPopupPresenter : GamePopupPresenter
{
	[InjectModel]
	private SlotTextPopupModel _model;

	[View]
	private SlotTextPopupView _view;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.Message.text = _model.Message;
		_view.AdditionalMessage.text = _model.AdditionalMessage;
		InventoryGoodsSlotModel inventoryGoodsSlotModel = new InventoryGoodsSlotModel(_model.Goods, InventoryGoodsSlotPresenter.ELayout.Normal);
		inventoryGoodsSlotModel.UseTooltip = true;
		inventoryGoodsSlotModel.ShowItemCount = false;
		_view.GoodsSlot.Inject(inventoryGoodsSlotModel);
		EGoodsCategory category = _model.Goods.Category;
		string text;
		if (category == EGoodsCategory.Item)
		{
			text = TItem.Table[_model.Goods.Id].Name;
		}
		else if (category == EGoodsCategory.Equipment)
		{
			text = TEquipment.Table[_model.Goods.Id].Name;
		}
		else
		{
			text = string.Empty;
		}
		_view.NameText.text = text;
		_view.Cancel_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close();
		}).AddTo(this);
		_view.Confirm_Button.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Close(true);
		}).AddTo(this);
		Selectable[] items = new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button };
		SetNavHorizontal(items);
		SetDefaultNav(_view.Confirm_Button);
	}

	private void InitGamePadNav()
	{
		Selectable[] items = new Selectable[2] { _view.Confirm_Button, _view.Cancel_Button };
		SetNavHorizontal(items);
		if (IsInputEnable && _view.Confirm_Button != null)
		{
			DoSelect(_view.Confirm_Button);
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager gameInputManager = GameInputManager;
		if (gameInputManager != null)
		{
			UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
			{
				Close();
			}).AddTo(InputDisposable);
		}
	}

	private void OnCancel()
	{
		Close();
	}

	private void OnConfirm()
	{
		Close(true);
	}
}
