using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class InteriorPopupPresenter : GamePopupPresenter
{
	private enum EFocusPanel
	{
		Area = 0,
		Interior = 1
	}

	[View]
	private InteriorPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	private TextSlotScrollerPresenter _areaScroller;

	private InteriorSlotScrollerPresenter _interiorScroller;

	private TextSlotScrollerModel _areaScrollerModel;

	private InteriorSlotScrollerModel _interiorScrollerModel;

	private int _selectedAreaId;

	private int[] _areaIds;

	private TextSlotModel _selectedAreaTextSlot;

	private TextSlotModel _focusedAreaTextSlot;

	private EFocusPanel _focusPanel;

	public override bool IsInputActionPopup => true;

	public override string OpenSfx => "InventoryPopupOpen";

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	protected override async UniTask OnOpenAsync()
	{
		_gameManager.AssertIfNotHost();
		await base.OnOpenAsync();
		_areaScroller = _view.Area_Scroller.GetComponent<TextSlotScrollerPresenter>();
		_interiorScroller = _view.Interior_Scroller.GetComponent<InteriorSlotScrollerPresenter>();
		InitAreaScroller();
		InitInteriorScroller();
	}

	private void FocusPanel(EFocusPanel panel)
	{
		_focusPanel = panel;
		_areaScrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Area;
		_interiorScrollerModel.RxIsInputEnabled.Value = panel == EFocusPanel.Interior;
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		GameInputManager.UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			OnBack();
		}).AddTo(InputDisposable);
		FocusPanel(_focusPanel);
	}

	protected override void OnInputDisabled()
	{
		_areaScrollerModel.RxIsInputEnabled.Value = false;
		_interiorScrollerModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void OnBack()
	{
		if (_focusPanel == EFocusPanel.Interior)
		{
			if (_selectedAreaTextSlot != null)
			{
				_selectedAreaTextSlot.RxIsSelected.Value = false;
				_selectedAreaTextSlot = null;
			}
			FocusPanel(EFocusPanel.Area);
			return;
		}
		if (_selectedAreaTextSlot == null)
		{
			Close();
			return;
		}
		_selectedAreaTextSlot.RxIsSelected.Value = false;
		_selectedAreaTextSlot = null;
		if (_focusedAreaTextSlot == null)
		{
			ShowInteriorList(false);
			return;
		}
		_selectedAreaId = _areaIds[_focusedAreaTextSlot.Index];
		RefreshInteriorList();
	}

	private void InitAreaScroller()
	{
		_areaIds = TInteriorShop.GroupTable.Keys.ToArray();
		string[] texts = _areaIds.Select((int _) => TArea.Table[_].Name).ToArray();
		_areaScrollerModel = new TextSlotScrollerModel
		{
			Texts = texts
		};
		_areaScrollerModel.OnSlotClick.Subscribe(OnAreaSlotClick).AddTo(this);
		_areaScrollerModel.OnSlotEnter.Subscribe(OnAreaSlotEnter).AddTo(this);
		_areaScrollerModel.OnSlotExit.Subscribe(OnAreaSlotExit).AddTo(this);
		_areaScroller.Inject(_areaScrollerModel);
	}

	private void OnAreaSlotClick(TextSlotModel slot)
	{
		if (_selectedAreaTextSlot != null)
		{
			_selectedAreaTextSlot.RxIsSelected.Value = false;
		}
		_selectedAreaTextSlot = slot;
		_selectedAreaTextSlot.RxIsSelected.Value = true;
		_selectedAreaId = _areaIds[slot.Index];
		RefreshInteriorList();
		if (IsGamePad)
		{
			FocusPanel(EFocusPanel.Interior);
		}
	}

	private void OnAreaSlotEnter(TextSlotModel slot)
	{
		_focusedAreaTextSlot = slot;
		if (_selectedAreaTextSlot == null)
		{
			_selectedAreaId = _areaIds[slot.Index];
			RefreshInteriorList();
		}
	}

	private void OnAreaSlotExit(TextSlotModel slot)
	{
		_focusedAreaTextSlot = null;
		if (_selectedAreaTextSlot == null)
		{
			ShowInteriorList(false);
		}
	}

	private void InitInteriorScroller()
	{
		_interiorScrollerModel = new InteriorSlotScrollerModel();
		_interiorScrollerModel.OnSlotClick.Subscribe(OnInteriorSlotClick).AddTo(this);
		_interiorScroller.Inject(_interiorScrollerModel);
		ShowInteriorList(false);
	}

	private void ShowInteriorList(bool show)
	{
		_view.Interior_Scroller.SetActive(show);
		_view.Empty.SetActive(!show);
	}

	private void RefreshInteriorList()
	{
		if (!TInteriorShop.GroupTable.TryGetValue(_selectedAreaId, out var value))
		{
			ShowInteriorList(false);
			return;
		}
		ShowInteriorList(true);
		InteriorSlotModel[] items = SortInteriorItems(value);
		_interiorScroller.RefreshItems(items);
	}

	private InteriorSlotModel[] SortInteriorItems(TInteriorShop[] interiors)
	{
		return interiors.OrderBy((TInteriorShop _) => (int)_.Interior.Type).ThenBy(GetGroupOrder).ThenBy((TInteriorShop _) => _.Id)
			.Select((TInteriorShop _) => new InteriorSlotModel
			{
				InteriorShopId = _.Id,
				Interior = _.Interior
			})
			.ToArray();
	}

	private static int GetGroupOrder(TInteriorShop shop)
	{
		if (shop.Interior.Type != EInteriorType.AreaDecoration)
		{
			return 0;
		}
		return TAreaDecoration.Table[shop.Interior.Id].GroupId;
	}

	private bool IsInteriorUnlocked(TInteriorShop shop)
	{
		switch (shop.Interior.Type)
		{
		case EInteriorType.Area:
			return _gameLoungeManager.IsAreaUnlocked(shop.Interior.Id);
		case EInteriorType.NpcLevel:
			return shop.Interior.Value <= _gameLoungeManager.GetNpcLevel(shop.Interior.Id);
		case EInteriorType.AreaDecoration:
			return _gameLoungeManager.IsAreaDecorationUnlocked(shop.Interior.Id);
		default:
			throw new ArgumentOutOfRangeException("Type", shop.Interior.Type, null);
		}
	}

	private bool HasEnoughCost(int consumeGold, int consumeCoin)
	{
		if (consumeGold > 0 && !_playerManager.HasEnoughGold(consumeGold))
		{
			return false;
		}
		if (consumeCoin > 0 && !_playerManager.HasEnoughCoin(consumeCoin))
		{
			return false;
		}
		return true;
	}

	private void OnInteriorSlotClick(InteriorSlotModel slotModel)
	{
		TInteriorShop tInteriorShop = TInteriorShop.Table[slotModel.InteriorShopId];
		if (IsInteriorUnlocked(tInteriorShop))
		{
			OnUnlockedInteriorClick(tInteriorShop);
		}
		else
		{
			OnBuyInterior(tInteriorShop);
		}
	}

	private async UniTaskVoid OnUnlockedInteriorClick(TInteriorShop shop)
	{
		if (shop.Interior.Type != EInteriorType.AreaDecoration)
		{
			_toastManager.NoticeFail(TLocalizedText.AlreadyUnlocked.LocalText);
			return;
		}
		TAreaDecoration tAreaDecoration = TAreaDecoration.Table[shop.Interior.Id];
		if (_gameContext.ActiveAreaDecorations.TryGetValue(tAreaDecoration.GroupId, out var value) && value == shop.Interior.Id)
		{
			_toastManager.NoticeFail(TLocalizedText.AlreadySelectedInterior.LocalText);
			return;
		}
		await PlayInteriorTransition(delegate
		{
			_gameLoungeManager.SetActiveAreaDecoration(shop.Interior.Id);
		});
	}

	private async UniTaskVoid OnBuyInterior(TInteriorShop shop)
	{
		if (!(await ConfirmPurchase(shop)) || !(await SendBuyInteriorRequest(shop.Id)))
		{
			return;
		}
		Action onApply = null;
		switch (shop.Interior.Type)
		{
		case EInteriorType.NpcLevel:
			onApply = delegate
			{
				_gameLoungeManager.SetNpcLevel(shop.Interior.Id, shop.Interior.Value);
			};
			break;
		case EInteriorType.AreaDecoration:
			onApply = delegate
			{
				_gameLoungeManager.SetActiveAreaDecoration(shop.Interior.Id);
			};
			break;
		}
		await PlayInteriorTransition(onApply);
	}

	private async UniTask<bool> ConfirmPurchase(TInteriorShop shop)
	{
		if (!_playerManager.CheckStateConditions(shop.UnlockConditions))
		{
			_toastManager.NoticeFail(TLocalizedText.UnlockConditionNotMatch.LocalText);
			return false;
		}
		if (!HasEnoughCost(shop.ConsumeGold, shop.ConsumeCoin))
		{
			_toastManager.NoticeFail(TLocalizedText.NotEnoughGoldOrCoin.LocalText);
			return false;
		}
		ConsumeConfirmPopupPresenter consumeConfirmPopupPresenter = await _popupManager.OpenAsync<ConsumeConfirmPopupPresenter>(new ConsumeConfirmPopupModel
		{
			TitleText = GetInteriorName(shop),
			DescText = TLocalizedText.NeededCost.LocalText,
			GoldAmount = shop.ConsumeGold,
			CoinAmount = shop.ConsumeCoin
		});
		return await consumeConfirmPopupPresenter.Task is bool flag && flag;
	}

	private async UniTask<bool> SendBuyInteriorRequest(int interiorShopId)
	{
		ResBuyInterior resBuyInterior = await _gameChannel.RequestAsync<ResBuyInterior>(new ReqBuyInterior
		{
			InteriorShopId = interiorShopId
		});
		if (resBuyInterior != null && resBuyInterior.IsSuccess)
		{
			return true;
		}
		_toastManager.NoticeFail(TLocalizedText.FailedBuyInterior.LocalText);
		return false;
	}

	private static string GetInteriorName(TInteriorShop shop)
	{
		switch (shop.Interior.Type)
		{
		case EInteriorType.Area:
			return TArea.Table[shop.Interior.Id].Name;
		case EInteriorType.NpcLevel:
			return TNpc.Table[shop.Interior.Id].Name;
		case EInteriorType.AreaDecoration:
			return TAreaDecoration.Table[shop.Interior.Id].Name;
		default:
			throw new ArgumentOutOfRangeException("Type", shop.Interior.Type, null);
		}
	}

	private async UniTask PlayInteriorTransition(Action onApply)
	{
		using BlockScope blockScope = new BlockScope(false, false);
		_areaScrollerModel.RxIsInputEnabled.Value = false;
		_interiorScrollerModel.RxIsInputEnabled.Value = false;
		IsBlurEffect = false;
		_popupManager.RefreshBlurEffect();
		CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
		await canvasGroup.DOFade(0f, 0.25f).AsyncWaitForCompletion();
		_soundManager.PlaySfx("Interior_Change");
		TArea tArea = TArea.Table[_selectedAreaId];
		_cameraManager.FocusOnPosition(tArea.CameraPos, 1f);
		DecorationVisualManager.Instance.ClearTargets();
		await DecorationVisualManager.Instance.PlayTransitionOut();
		DecorationVisualManager.Instance.ClearTargets();
		onApply?.Invoke();
		await UniTask.Delay(500);
		DecorationVisualManager.Instance.StopTransitionOut();
		await DecorationVisualManager.Instance.PlayTransitionIn();
		_cameraManager.ReleaseFocus(1.5f);
		await canvasGroup.DOFade(1f, 0.25f).AsyncWaitForCompletion();
		DecorationVisualManager.Instance.StopTransitionIn();
		DecorationVisualManager.Instance.ResetTransitions();
		RefreshInteriorList();
		IsBlurEffect = true;
		_popupManager.RefreshBlurEffect();
		await UniTask.NextFrame();
		_interiorScrollerModel.EntryDirection = ENavDir.Down;
		FocusPanel(EFocusPanel.Interior);
		_interiorScrollerModel.EntryDirection = ENavDir.None;
	}
}
