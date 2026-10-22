using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Mvp;
using System.Linq;
using UniRx;
using UnityEngine;

public class ArtifactDealScrollerPresenter : VerticalScrollerPresenter<ArtifactDealSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private float _lineSize;

	[InjectModel]
	private ArtifactDealScrollerModel _model;

	private readonly List<EArtifactCategory> _preferCategories = new List<EArtifactCategory>();

	private readonly List<int> _preferPrefixIds = new List<int>();

	private readonly List<int> _dislikePrefixIds = new List<int>();

	private int _preferStartIndex = -1;

	private int _dislikeStartIndex = -1;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<ArtifactDealSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	private void OnInject(ArtifactDealScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		model.OnUpdateGoods.Subscribe(delegate
		{
			UpdateScroller();
		}).AddTo(_disposable);
		model.OnUpdatePreferCategories.Subscribe(delegate(List<EArtifactCategory> categories)
		{
			_preferCategories.Clear();
			_preferCategories.AddRange(categories);
			UpdateScroller();
		}).AddTo(_disposable);
		model.OnUpdatePreferPrefixIds.Subscribe(delegate(List<int> ids)
		{
			_preferPrefixIds.Clear();
			_preferPrefixIds.AddRange(ids);
			UpdateScroller();
		}).AddTo(_disposable);
		model.OnUpdateDislikePrefixIds.Subscribe(delegate(List<int> ids)
		{
			_dislikePrefixIds.Clear();
			_dislikePrefixIds.AddRange(ids);
			UpdateScroller();
		}).AddTo(_disposable);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void UpdateScroller()
	{
		_preferStartIndex = ((_preferPrefixIds.Count > 0) ? 1 : (-1));
		_dislikeStartIndex = ((_dislikePrefixIds.Count > 0) ? (_preferPrefixIds.Count + 1) : (-1));
		RefreshScroller();
	}

	protected override List<ArtifactDealSlotModel> BuildSlotModels()
	{
		List<ArtifactDealSlotModel> list = new List<ArtifactDealSlotModel>();
		list.AddRange(_preferCategories.Select((EArtifactCategory c) => new ArtifactDealSlotModel
		{
			ArtifactCategory = c
		}));
		list.AddRange(_preferPrefixIds.Select((int id) => new ArtifactDealSlotModel
		{
			ArtifactPrefixId = id
		}));
		list.AddRange(_dislikePrefixIds.Select((int id) => new ArtifactDealSlotModel
		{
			ArtifactPrefixId = id,
			IsPreferPrefix = true
		}));
		return list;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		float num = _cellSize;
		if (dataIndex == _dislikeStartIndex || dataIndex == _preferStartIndex)
		{
			num += _lineSize;
		}
		return num;
	}
}
