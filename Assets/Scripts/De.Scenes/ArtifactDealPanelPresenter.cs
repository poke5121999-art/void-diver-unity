using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Mvp;
using UniRx;

public class ArtifactDealPanelPresenter : BasePanelPresenter
{
	[InjectModel]
	private ArtifactDealPanelModel _model;

	[View]
	private ArtifactDealPanelView _view;

	private ArtifactDealScrollerModel _scrollerModel;

	private CompositeDisposable _disposable;

	protected override BasePanelModel PanelModel => _model;

	protected void OnInject(ArtifactDealPanelModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_scrollerModel = new ArtifactDealScrollerModel();
		_model.OnUpdateGoods.Subscribe(delegate(Goods goods)
		{
			_scrollerModel.OnUpdateGoods.OnNext(goods);
		}).AddTo(_disposable);
		_model.OnUpdatePreferCategories.Subscribe(delegate(List<EArtifactCategory> categories)
		{
			_scrollerModel.OnUpdatePreferCategories.OnNext(categories);
		}).AddTo(_disposable);
		_model.OnUpdatePreferPrefixIds.Subscribe(delegate(List<int> ids)
		{
			_scrollerModel.OnUpdatePreferPrefixIds.OnNext(ids);
		}).AddTo(_disposable);
		_model.OnUpdateDislikePrefixIds.Subscribe(delegate(List<int> ids)
		{
			_scrollerModel.OnUpdateDislikePrefixIds.OnNext(ids);
		}).AddTo(_disposable);
		_view.Scroller.GetComponent<ArtifactDealScrollerPresenter>().Inject(_scrollerModel);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable?.Dispose();
	}
}
