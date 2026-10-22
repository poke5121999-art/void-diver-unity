using Cysharp.Threading.Tasks;
using NemoLib.Mvp;
using UniRx;

public class LostItemListPopupPresenter : GamePopupPresenter
{
	[View]
	private LostItemListPopupView _view;

	private LostItemScrollerModel _scrollerModel;

	private GameInputManager GameInputManager => InputManager as GameInputManager;

	private InputActions.UiActions UiActions => GameInputManager.UiActions;

	public override string OpenSfx => "PopUpOpen";

	public override bool IsInputActionPopup => true;

	protected override async UniTask OnOpenAsync()
	{
		await base.OnOpenAsync();
		SetScroller();
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
			_scrollerModel.RxIsInputEnabled.Value = true;
		}
	}

	protected override void OnInputDisabled()
	{
		base.OnInputDisabled();
		_scrollerModel.RxIsInputEnabled.Value = false;
	}

	private void SetScroller()
	{
		_scrollerModel = new LostItemScrollerModel
		{
			Mode = ELostItemScrollerMode.All
		};
		_scrollerModel.OnSlotEnter.Subscribe(OnSlotEnter).AddTo(this);
		_scrollerModel.OnSlotExit.Subscribe(OnSlotExit).AddTo(this);
		if (IsInputEnable)
		{
			_scrollerModel.RxIsInputEnabled.Value = true;
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
}
