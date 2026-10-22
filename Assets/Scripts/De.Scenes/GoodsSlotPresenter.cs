using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine.UI;
using VContainer;

public class GoodsSlotPresenter : BaseSlotPresenter<GoodsSlotModel>
{
	[InjectModel]
	private GoodsSlotModel _model;

	[View]
	private GoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private PopupManager _popupManager;

	protected override GoodsSlotModel SlotModel => null;

	protected override Button Button => null;

	private void OnInject(GoodsSlotModel model)
	{
	}

	protected override void OnClick()
	{
	}

	private bool TryShowGoodsTooltipPopup()
	{
		return false;
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

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
	}
}
