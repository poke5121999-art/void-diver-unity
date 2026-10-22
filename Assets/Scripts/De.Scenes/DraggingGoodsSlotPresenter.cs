using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.UI;
using VContainer;

public class DraggingGoodsSlotPresenter : BaseSlotPresenter<DraggingGoodsSlotModel>
{
	[InjectModel]
	private DraggingGoodsSlotModel _model;

	[View]
	private DraggingGoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	protected override DraggingGoodsSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(DraggingGoodsSlotModel model)
	{
	}

	private void UpdateLayout()
	{
	}

	private void SetNormalLayout()
	{
	}

	private void OnGoodsSlotChanged(GoodsSlot goodsSlot)
	{
	}
}
