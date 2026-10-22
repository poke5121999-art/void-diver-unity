using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class BagTooltipPresenter : BasePanelPresenter
{
	[View]
	private BagTooltipView _view;

	[InjectModel]
	private BagTooltipModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private AssetManager _assetManager;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable.Dispose();
	}

	private void OnInject(BagTooltipModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		UpdateBagUis();
		_playerManager.OnStashChanged.Subscribe(delegate
		{
			UpdateBagUis();
		}).AddTo(_disposable);
		_playerManager.OnGoodsMarkChanged.Subscribe(OnGoodsMarkChanged).AddTo(_disposable);
		_view.Title_Mark.gameObject.SetActive(_playerManager.IsMarked(model.Goods));
		_view.Title_TypeIcon.sprite = _assetManager.GetIconGoodsSubType(_model.Goods.Type);
	}

	private void UpdateBagUis()
	{
		if (_model.Goods is BagGoods bagGoods)
		{
			TBag tBag = TBag.Table[bagGoods.Id];
			_view.Title_Name.text = string.Format("{0} ({1}/{2})", tBag.Name, bagGoods.OccupiedSlotCount, bagGoods.MaxSlotCount);
			_view.Title_GradeText.gameObject.SetActive(tBag.Grade != EGoodsGradeType.None);
			_view.Title_GradeText.text = tBag.Grade.ToLocalText();
			_view.Title_GradeText.color = tBag.Grade.ToColor(_view.Title_GradeText.color.a);
			_view.Description_Text.text = tBag.Desc;
			_view.Price_GoldText_Value.text = string.Format("{0}", tBag.SellPrice);
			_view.StashAmount_Value.text = string.Format("{0}", _playerManager.GetStashGoodsCount(_model.Goods));
		}
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
		if (_model.Goods.Type == data.goodsType && _model.Goods.Id == data.id)
		{
			_view.Title_Mark.gameObject.SetActive(data.isMarked);
		}
	}
}
