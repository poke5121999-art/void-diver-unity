using NemoLib.Mvp;
using UniRx;
using VContainer;

public class StashPanelPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Filter = 1,
		Scroller = 2,
		Empty = 3
	}

	[InjectModel]
	private StashPanelModel _model;

	[View]
	private StashPanelView _view;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameManager _gameManager;

	private InventoryGoodsSlotModel _cachedSelectedSlot;

	private StashScrollerModel _scrollerModel;

	private EmptyPanelModel _emptyPanelModel;

	private StashFilterPanelModel _filterPanelModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => null;

	private GamePlayer Player => null;

	private EFocusPanel FocusedPanel => EFocusPanel.None;

	protected override void OnDestroy()
	{
	}

	private void Start()
	{
	}

	protected void OnInject(StashPanelModel model)
	{
	}

	private void OnPageChanged(int pageIndex)
	{
	}

	private void OnPageCountChanged(int pageCount)
	{
	}

	private void UpdatePagination()
	{
	}

	private void OnPrevPageButtonClick()
	{
	}

	private void OnNextPageButtonClick()
	{
	}

	private void OnFocusedSlotChanged(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnSlotsEmpty(bool isEmpty)
	{
	}

	private void FocusScrollerIfEmptyFocused()
	{
	}

	private void OnStashChanged()
	{
	}

	private void RefreshSelectedSlot()
	{
	}

	private void UpdateSlotCounts()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir dir)
	{
	}

	private void OnFilterPanelOutOfBottom(Unit _)
	{
	}

	private void OnScrollerOutOfTop(Unit _)
	{
	}

	private void OnEmptyPanelOutOfTop(Unit _)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}
}
