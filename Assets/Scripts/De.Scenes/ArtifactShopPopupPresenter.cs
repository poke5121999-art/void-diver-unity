using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class ArtifactShopPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[View]
	private ArtifactShopPopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private EItemPanelType _itemPanelType;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "ArtifactUI";

	public override bool IsInputActionPopup => true;

	private Inventory MyInventory => _gameManager.MyInventory;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private BasePanelModel CurrentPanelModel => (_itemPanelType == EItemPanelType.Inventory) ? _inventoryPanelModel : _stashPanelModel;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		_view.InventoryPanel.gameObject.SetActive(true);
		_view.StashPanel.gameObject.SetActive(true);
		_inventoryPanelModel = new PlayerInventoryPanelModel
		{
			PlayerInventory = _gameManager.MyInventory,
			AllowedSubTypes = new EGoodsType[1] { EGoodsType.Artifact },
			ArtifactFilterTypes = new EArtifactCategory[1] { EArtifactCategory.All },
			IsArtifactShop = true
		};
		_inventoryPanelModel.OnSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slotModel)
		{
			OnSlotClickAsync(slotModel).Forget();
		}).AddTo(this);
		_inventoryPanelModel.OnEquipmentSlotClick.Subscribe(OnEquipmentSlotClick).AddTo(this);
		_view.InventoryPanel.Inject(_inventoryPanelModel);
		_stashPanelModel = new StashPanelModel
		{
			AllowedSubtypes = new EGoodsType[1] { EGoodsType.Artifact },
			ArtifactFilterTypes = new EArtifactCategory[1] { EArtifactCategory.All },
			EnableFilterButtons = false,
			IsArtifactShop = true
		};
		_stashPanelModel.OnSlotClick.Subscribe(delegate(InventoryGoodsSlotModel slotModel)
		{
			OnSlotClickAsync(slotModel).Forget();
		}).AddTo(this);
		_view.StashPanel.GetComponent<Presenter>().Inject(_stashPanelModel);
		_itemPanelType = EItemPanelType.Inventory;
		_view.InventoryPanel.gameObject.SetActive(true);
		_view.StashPanel.gameObject.SetActive(false);
		if (ShouldAutoSwitchToStash())
		{
			SwapItemPanel();
		}
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			Back();
		}).AddTo(InputDisposable);
		UiActions.Switch.OnClickWithSfx("ButtonClick").Subscribe(delegate
		{
			SwapItemPanel();
		}).AddTo(InputDisposable);
		if (base.IsInputEnable)
		{
			CurrentPanelModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		CurrentPanelModel.RxIsInputEnabled.Value = false;
		base.OnInputDisabled();
	}

	private void SetLayoutInputEnabled(bool value)
	{
		if (!value || base.IsInputEnable)
		{
			CurrentPanelModel.RxIsInputEnabled.Value = value;
		}
	}

	private bool ShouldAutoSwitchToStash()
	{
		int num = _gameManager.MyInventory.Slots.Count((KeyValuePair<int, GoodsSlot> slot) => slot.Value.Type == EGoodsType.Artifact);
		int num2 = _playerManager.StashSlots.Count((KeyValuePair<int, GoodsSlot> slot) => slot.Value.Type == EGoodsType.Artifact);
		return num == 0 && num2 > 0;
	}

	private void SwapItemPanel()
	{
		CurrentPanelModel.RxIsInputEnabled.Value = false;
		_itemPanelType = ((_itemPanelType == EItemPanelType.Inventory) ? EItemPanelType.Stash : EItemPanelType.Inventory);
		_view.InventoryPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Inventory);
		_view.StashPanel.gameObject.SetActive(_itemPanelType == EItemPanelType.Stash);
		if (base.IsInputEnable)
		{
			CurrentPanelModel.RxIsInputEnabled.Value = true;
		}
	}

	private void Back()
	{
		Close(false);
	}

	private async UniTaskVoid OnSlotClickAsync(InventoryGoodsSlotModel slotModel)
	{
		if (slotModel.GoodsSlot.IsEmpty || slotModel.GoodsSlot.Type != EGoodsType.Artifact)
		{
			return;
		}
		_soundManager.PlaySfx("ButtonClick2");
		Goods goods = slotModel.GoodsSlot.Goods;
		int price = GetDealPrice(goods);
		string text = TLocalizedText.ItemSoldFormat.Format(goods.GetName(), 1, TLocalizedText.EGoodsTypeGold.LocalText, price);
		if (!(await _popupManager.ShowConfirmationTextPopupAsync(text)))
		{
			return;
		}
		long exp = CalculateArtifactDealExp(goods, price);
		ResSellArtifact resSellArtifact = await _gameChannel.RequestAsync<ResSellArtifact>(new ReqSellArtifact
		{
			SlotCategory = slotModel.SlotCategory,
			SlotIndex = slotModel.GoodsSlot.SlotIndex,
			SellGoods = goods.Clone(),
			GainGold = price,
			GainExp = exp
		});
		if (resSellArtifact != null && resSellArtifact.IsSuccess)
		{
			_soundManager.PlaySfx("casher");
			if (exp >= 1)
			{
				await UniTask.NextFrame();
				_uiEventSystem.FlushLevelAndExp();
			}
		}
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
		if (!slotModel.GoodsSlot.IsEmpty && slotModel.GoodsSlot.Type == EGoodsType.Artifact)
		{
			_soundManager.PlaySfx("Fail");
			_toastManager.NoticeFail(TLocalizedText.CannotDealEquipedArtifact.LocalText);
		}
	}

	private int GetDealPrice(Goods goods)
	{
		int artifactPrice = goods.GetArtifactPrice();
		TConst artifactShopCostMultiplier = TConst.ArtifactShopCostMultiplier;
		// GetDealPrice @0x180950960: (int)((float)price * FloatValue), a float32 product (IL2CPP): 600 * 0.7f = 420.0f.
		// The editor's Mono keeps a wider intermediate (419.99999) and truncated to 419, so the product is rounded to float first.
		return (int)(float)((float)artifactPrice * artifactShopCostMultiplier.FloatValue);
	}

	private long CalculateArtifactDealExp(Goods goods, int sellPrice)
	{
		EGoodsGradeType grade = TEquipment.Table[goods.Id].Grade;
		float artifactDealExpMultiplier = grade.GetArtifactDealExpMultiplier();
		if (artifactDealExpMultiplier <= 0f)
		{
			return 0L;
		}
		long num = (long)((float)sellPrice * artifactDealExpMultiplier);
		return (num < 1) ? 0 : num;
	}
}
