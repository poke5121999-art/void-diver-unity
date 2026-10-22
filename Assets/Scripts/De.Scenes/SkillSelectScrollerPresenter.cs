using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class SkillSelectScrollerPresenter : VerticalScrollerPresenter<SkillSelectInfoSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[SerializeField]
	private RectTransform _measureRoot;

	[InjectModel]
	private SkillSelectScrollerModel _model;

	[Inject]
	private GameManager _gameManager;

	private readonly List<float> _cellHeights;

	private bool _measured;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<SkillSelectInfoSlotModel> ScrollerModel => null;

	private GamePlayer Player => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(SkillSelectScrollerModel model)
	{
	}

	protected override void OnSelect(int index)
	{
	}

	private void OnClickSkillSelectInfoSlot(SkillSelectInfoSlotModel infoSlotModel)
	{
	}

	protected override List<SkillSelectInfoSlotModel> BuildSlotModels()
	{
		return null;
	}

	private void Measure(int initialIndex = 0)
	{
	}

	private void PrepareMeasureRoot()
	{
	}

	private void BuildHeights()
	{
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
