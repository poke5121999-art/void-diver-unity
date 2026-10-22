using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class EmployeeScrollerPresenter : VerticalScrollerPresenter<EmployeeCardModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private float _cellSize;

	[InjectModel]
	private EmployeeScrollerModel _model;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private TycoonManager _tycoonManager;

	private readonly List<EmployeeCardModel> _cardModels;

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<EmployeeCardModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	protected override List<EmployeeCardModel> BuildSlotModels()
	{
		return null;
	}

	protected override int GetItemCount()
	{
		return 0;
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(EmployeeScrollerModel model)
	{
	}

	private void BuildCardModels()
	{
	}

	private List<EmployeeCardModel> BuildHireModels()
	{
		return null;
	}

	private List<EmployeeCardModel> BuildInfoModels()
	{
		return null;
	}

	private List<EmployeeCardModel> BuildSettingModels()
	{
		return null;
	}

	private void OnCardClicked(EmployeeCardModel cardModel)
	{
	}

	private void OnCardEnter(EmployeeCardModel cardModel)
	{
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return 0;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	protected override void OnSelect(int index)
	{
	}

	protected override void OnDeselect(int index)
	{
	}

	public EmployeeCardView GetCardView(int employeeId)
	{
		return null;
	}
}
