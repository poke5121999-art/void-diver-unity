using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

public class LostItemPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		None = 0,
		Scroller = 1,
		Btns = 2
	}

	[View]
	private LostItemPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private PopupManager _popupManager;

	private LostItemScrollerModel _scrollerModel;

	private EFocusPanel _focusPanel = EFocusPanel.Scroller;

	private CompositeDisposable _btnsNavDisposable;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private InputAction MoveScrollAction => GameInputManager.UiPadActions.MoveScroll;

	public override string OpenSfx => "PopUpOpen";

	public override bool IsInputActionPopup => true;

	private PlayerInventory MyInventory => _gameManager.MyInventory;

	private long RerollCost
	{
		get
		{
			float num = TConst.LostItemRerollCost.IntValue;
			for (int i = 0; i < _playerManager.Profile.LostItemRerollCount; i++)
			{
				num *= TConst.LostItemRerollCostMultiplier.FloatValue;
			}
			return (long)num;
		}
	}

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.TextEmpty.gameObject.SetActive(!_playerManager.RxSelectLostGoodsInfos.Any());
		_playerManager.RxSelectLostGoodsInfos.ObserveChange().Subscribe(delegate
		{
			SetScroller();
			RefreshBtnsInteractable();
		}).AddTo(this);
		_view.Btns_BtnRefresh.OnClickWithSfx().Subscribe(delegate
		{
			OnRerollClick();
		}).AddTo(this);
		_view.Btns_BtnList.OnClickWithSfx().Subscribe(delegate
		{
			OnListClick();
		}).AddTo(this);
		SetScroller();
		SetRerollCostView();
		RefreshBtnsInteractable();
		InitGamePadNav();
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		FocusPanel(_focusPanel);
	}

	protected override void OnInputDisabled()
	{
		_btnsNavDisposable?.Dispose();
		_btnsNavDisposable = null;
		_scrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void InitGamePadNav()
	{
		SetNavVertical(new Selectable[2] { _view.Btns_BtnList, _view.Btns_BtnRefresh });
		SetDefaultNav(_view.Btns_BtnList);
	}

	private void FocusPanel(EFocusPanel panel)
	{
		if (panel != EFocusPanel.None)
		{
			_focusPanel = panel;
		}
		_btnsNavDisposable?.Dispose();
		_btnsNavDisposable = null;
		_scrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Scroller;
		if (panel == EFocusPanel.Btns)
		{
			InputManager.SelectObject(_view.Btns_BtnList.gameObject);
			_btnsNavDisposable = new CompositeDisposable();
			Observable.Merge(MoveScrollAction.OnPressStartAsObservable(), MoveScrollAction.OnClickAsObservable()).Where((InputAction.CallbackContext ctx) => ctx.ReadValue<Vector2>().x < -0.5f).Subscribe(delegate
			{
				FocusPanel(EFocusPanel.Scroller);
			})
				.AddTo(_btnsNavDisposable);
		}
	}

	private void SetScroller()
	{
		_view.TextEmpty.gameObject.SetActive(!_playerManager.RxSelectLostGoodsInfos.Any());
		_scrollerModel = new LostItemScrollerModel();
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(this);
		_scrollerModel.OutOfRight.Subscribe(delegate
		{
			FocusPanel(EFocusPanel.Btns);
		}).AddTo(this);
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = _focusPanel == EFocusPanel.Scroller;
		}
		_view.Scroller.Inject(_scrollerModel);
	}

	private void OnSlotEnter(LostItemSlotModel slotModel)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.LostGoods.Goods;
		_view.GoodsTooltip.Inject(goodsTooltipModel);
	}

	private void OnSlotExit(LostItemSlotModel slotModel)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: false);
	}

	private void OnSlotClick(LostItemSlotModel slotModel)
	{
		RecoverAsync(slotModel).Forget();
	}

	private void OnRerollClick()
	{
		RerollAsync().Forget();
	}

	private void OnListClick()
	{
		_popupManager.Open<LostItemListPopupPresenter>();
	}

	private async UniTask RecoverAsync(LostItemSlotModel slotModel)
	{
		LostGoods lostGoods = slotModel.LostGoods;
		LostItemSelectInfo selectInfo = _playerManager.RxSelectLostGoodsInfos.FirstOrDefault((LostItemSelectInfo info) => info.SlotIndex == lostGoods.SlotIndex);
		int price = slotModel.Price;
		bool isArtifact = slotModel.IsArtifact;
		if (!isArtifact)
		{
			if (_playerManager.Gold < price)
			{
				_soundManager.PlaySfx("Fail");
				_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
				return;
			}
		}
		else if (_playerManager.Coin < price)
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.Notice(TLocalizedText.NotEnoughGoldOrCoin.LocalText);
			return;
		}
		if (!MyInventory.CanAddGoods(lostGoods.Goods, out var reason))
		{
			_toastManager.NoticeFail(reason);
			return;
		}
		if (await _popupManager.ShowConfirmationTextPopupAsync(TLocalizedText.LostItemRecovery.LocalText) && _playerManager.RxSelectLostGoodsInfos.Contains(selectInfo))
		{
			ResTakeLostItem resTakeLostItem = await _gameChannel.RequestAsync<ResTakeLostItem>(new ReqTakeLostItem
			{
				SlotIndex = selectInfo.SlotIndex,
				ConsumeGold = (isArtifact ? 0 : price),
				ConsumeCoin = (isArtifact ? price : 0),
				Goods = lostGoods.Goods
			});
			if (resTakeLostItem != null && resTakeLostItem.IsSuccess)
			{
				_soundManager.PlaySfx("Casher_Coin_Online");
			}
		}
	}

	private void RefreshBtnsInteractable()
	{
		bool flag = _playerManager.LostGoods.Count > 0;
		_view.Btns_BtnRefresh.interactable = flag;
		_view.Btns_BtnRefresh.GetComponent<ButtonController>().SetLocked(!flag);
		_view.Btns_BtnRefresh_Gold.SetActive(flag);
		_view.Btns_BtnList.interactable = flag;
		_view.Btns_BtnList.GetComponent<ButtonController>().SetLocked(!flag);
	}

	private void SetRerollCostView()
	{
		_view.Btns_BtnRefresh_Gold_Value.text = RerollCost.ToString("N0");
	}

	private async UniTask RerollAsync()
	{
		long rerollCost = RerollCost;
		if (_playerManager.Gold >= rerollCost)
		{
			ResRerollLostItems resRerollLostItems = await _gameChannel.RequestAsync<ResRerollLostItems>(new ReqRerollLostItems
			{
				ConsumeGold = rerollCost
			});
			if (resRerollLostItems != null && resRerollLostItems.IsSuccess)
			{
				_soundManager.PlaySfx("Casher");
				SetRerollCostView();
			}
		}
		else
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.Notice(TLocalizedText.NotEnoughGold.LocalText);
		}
	}
}
