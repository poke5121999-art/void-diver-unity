using System.Collections.Generic;
using GameCommon.Data;
using NemoLib.Mvp;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class BagPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private BagPanelModel _model;

	[View]
	private BagPanelView _view;

	[Inject]
	private SoundManager _soundManager;

	private CompositeDisposable _disposable;

	private readonly List<InventoryGoodsSlotModel> _slotModels;

	private Selectable _lastFocusedSlot;

	protected override BasePanelModel PanelModel => null;

	protected override void OnDestroy()
	{
	}

	public void Open()
	{
	}

	public void Close()
	{
	}

	private void OnInject(BagPanelModel model)
	{
	}

	private void OnDragAcceptableChanged(bool isAcceptable)
	{
	}

	private void UpdateTooltip(InventoryGoodsSlotModel slotModel)
	{
	}

	private void InitSlots()
	{
	}

	protected override void DoSelect(Selectable selectable)
	{
	}

	private void InitGamePadNav()
	{
	}

	private void UpdateTitle(BagGoods bag)
	{
	}

	private void OnBack()
	{
	}
}
