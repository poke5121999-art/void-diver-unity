using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Mvp;
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

	private readonly List<EArtifactCategory> _preferCategories;

	private readonly List<int> _preferPrefixIds;

	private readonly List<int> _dislikePrefixIds;

	private int _preferStartIndex;

	private int _dislikeStartIndex;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<ArtifactDealSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	private void OnInject(ArtifactDealScrollerModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	private void UpdateScroller()
	{
	}

	protected override List<ArtifactDealSlotModel> BuildSlotModels()
	{
		return null;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
