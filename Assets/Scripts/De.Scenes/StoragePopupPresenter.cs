using Cysharp.Threading.Tasks;
using GameCommon.Table;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class StoragePopupPresenter : GamePopupPresenter
{
	[View]
	private StoragePopupView _view;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private PlayerManager _playerManager;

	private StorageSlotScrollerModel _scrollerModel;

	private readonly ReactiveProperty<int> _rxSelectedSlot = new ReactiveProperty<int>();

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	public override string OpenSfx => "PopUpOpen";

	public override bool IsInputActionPopup => true;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	private GamePlayer Player => _gameManager.MyPlayer;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		UpdateStashSlotCountUi();
		_scrollerModel = new StorageSlotScrollerModel();
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(base.OpenDisposable);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(base.OpenDisposable);
		_scrollerModel.OnSlotClick.Subscribe(OnSlotClick).AddTo(base.OpenDisposable);
		_view.Scroller.Inject(_scrollerModel);
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			OnStashChanged();
		}).AddTo(base.OpenDisposable);
	}

	protected override void OnInputEnabled()
	{
		base.OnInputEnabled();
		UiActions.Escape.OnClickWithSfx().Subscribe(delegate
		{
			Close();
		}).AddTo(InputDisposable);
		_scrollerModel.RxIsInputEnabled.Value = true;
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_scrollerModel.RxIsInputEnabled.Value = false;
	}

	private void UpdateStashSlotCountUi()
	{
		_view.StashSlotCountText.text = string.Format("{0} {1} / {2}", TLocalizedText.Stash.LocalText, _playerManager.StashSlots.Count, _gameManager.MyPlayer.StashSlotCount);
	}

	private void OnStashChanged()
	{
		UpdateStashSlotCountUi();
	}

	private void OnSlotEnter(StorageSlotModel slotModel)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: true);
		GoodsTooltipPresenter component = _view.GoodsTooltip.GetComponent<GoodsTooltipPresenter>();
		GoodsTooltipModel goodsTooltipModel = new GoodsTooltipModel();
		goodsTooltipModel.Goods = slotModel.Goods;
		component.Inject(goodsTooltipModel);
	}

	private void OnSlotExit(StorageSlotModel slotModel)
	{
		_view.GoodsTooltip.gameObject.SetActive(value: false);
	}

	private void OnSlotClick(StorageSlotModel slotModel)
	{
		int index = slotModel.Index;
		if (_playerManager.StorageSlots.ContainsKey(index))
		{
			if (_playerManager.TryGoodsToStash(slotModel.Goods))
			{
				_soundManager.PlaySfx("ItemRelease");
				_playerManager.StorageSlots.Remove(index);
				_view.GoodsTooltip.gameObject.SetActive(value: false);
			}
			else
			{
				_soundManager.PlaySfx("Fail");
				_toastManager.Notice(TLocalizedText.NotEnoughStashSlots.LocalText);
			}
		}
	}
}
