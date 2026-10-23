using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
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

	private readonly List<EmployeeCardModel> _cardModels = new List<EmployeeCardModel>();

	private CompositeDisposable _disposable;

	protected override BaseScrollerModel<EmployeeCardModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	protected override List<EmployeeCardModel> BuildSlotModels()
	{
		throw new NotImplementedException();
	}

	protected override int GetItemCount()
	{
		return _cardModels.Count;
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		base.OnDestroy();
	}

	private void OnInject(EmployeeScrollerModel model)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		BuildCardModels();
		foreach (EmployeeCardModel cardModel in _cardModels)
		{
			cardModel.OnClick.Subscribe(OnCardClicked).AddTo(_disposable);
			cardModel.OnPointerEnter.Subscribe(OnCardEnter).AddTo(_disposable);
			cardModel.OnCleansingClick.Subscribe(delegate(EmployeeCardModel m)
			{
				_model.OnCleansingClick.OnNext(m);
			}).AddTo(_disposable);
			cardModel.OnFireClick.Subscribe(delegate(EmployeeCardModel m)
			{
				_model.OnFireClick.OnNext(m);
			}).AddTo(_disposable);
		}
		Scroller.Delegate = this;
		Scroller.ReloadData();
		if (_cardModels.Count > 0)
		{
			RefreshNav();
		}
		else
		{
			RefreshNavCount();
		}
		model.RxIsInputEnabled.Where((bool x) => x).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_disposable);
		model.RxIsInputEnabled.Where((bool x) => !x).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_disposable);
	}

	private void BuildCardModels()
	{
		_cardModels.Clear();
		switch (_model.Mode)
		{
		case EEmployeePopupMode.Info:
			_cardModels.AddRange(BuildInfoModels());
			break;
		case EEmployeePopupMode.Hire:
			_cardModels.AddRange(BuildHireModels());
			break;
		case EEmployeePopupMode.Setting:
			_cardModels.AddRange(BuildSettingModels());
			break;
		}
	}

	private List<EmployeeCardModel> BuildHireModels()
	{
		return _tycoonManager.RecruitableEmployees.Values.Select((Employee e) => new EmployeeCardModel
		{
			EmployeeId = e.Id,
			Level = e.Level,
			Exp = e.Exp,
			Stress = e.Stress,
			IsPanic = e.IsPanic,
			SlotType = EEmployeeSlotType.Hire,
			HireCost = _tycoonManager.CalculateHireCost(e.Id, e.Level),
			LearnedSkillIds = e.LearnedSkillIds
		}).ToList();
	}

	private List<EmployeeCardModel> BuildInfoModels()
	{
		HashSet<int> placedEmployeeIds = new HashSet<int>(_playerManager.SalesSlots.Values.Where((SalesSlot s) => s.EmployeeId > 0).Select((SalesSlot s) => s.EmployeeId));
		return _playerManager.Employees.Values.Select((Employee e) => new EmployeeCardModel
		{
			EmployeeId = e.Id,
			Level = e.Level,
			Exp = e.Exp,
			Stress = e.Stress,
			IsPanic = e.IsPanic,
			SlotType = EEmployeeSlotType.Info,
			IsPlaced = placedEmployeeIds.Contains(e.Id),
			LearnedSkillIds = e.LearnedSkillIds
		}).ToList();
	}

	private List<EmployeeCardModel> BuildSettingModels()
	{
		HashSet<int> placedEmployeeIds = new HashSet<int>(_playerManager.SalesSlots.Values.Where((SalesSlot s) => s.EmployeeId > 0).Select((SalesSlot s) => s.EmployeeId));
		return _playerManager.Employees.Values.Where((Employee e) => !e.IsPanic && !placedEmployeeIds.Contains(e.Id)).Select((Employee e) => new EmployeeCardModel
		{
			EmployeeId = e.Id,
			Level = e.Level,
			Exp = e.Exp,
			Stress = e.Stress,
			IsPanic = false,
			SlotType = EEmployeeSlotType.Setting,
			LearnedSkillIds = e.LearnedSkillIds
		}).ToList();
	}

	private void OnCardClicked(EmployeeCardModel cardModel)
	{
		if (_model.Mode == EEmployeePopupMode.Info)
		{
			_cardModels.ForEach(delegate(EmployeeCardModel m)
			{
				m.RxIsSelected.Value = m.EmployeeId == cardModel.EmployeeId && !m.RxIsSelected.Value;
			});
		}
		_model.OnSlotClick.OnNext(cardModel);
	}

	private void OnCardEnter(EmployeeCardModel cardModel)
	{
		_model.OnSlotEnter.OnNext(cardModel);
	}

	public override int GetNumberOfCells(EnhancedScroller scroller)
	{
		return _cardModels.Count;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return _cellSize;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		baseCell.SetData(_cardModels[dataIndex]);
		return baseCell;
	}

	protected override void OnSelect(int index)
	{
		EnhancedScrollerCellView cellViewAtDataIndex = Scroller.GetCellViewAtDataIndex(index);
		if (!(cellViewAtDataIndex == null))
		{
			EmployeeCardView component = cellViewAtDataIndex.GetComponent<EmployeeCardView>();
			if (component != null)
			{
				InputManager.SelectObject(component.Contents.gameObject);
			}
		}
	}

	protected override void OnDeselect(int index)
	{
		EnhancedScrollerCellView cellViewAtDataIndex = Scroller.GetCellViewAtDataIndex(index);
		if (!(cellViewAtDataIndex == null))
		{
			EmployeeCardView component = cellViewAtDataIndex.GetComponent<EmployeeCardView>();
			if (component != null)
			{
				InputManager.DeselectObject(component.Contents.gameObject);
			}
		}
	}

	public EmployeeCardView GetCardView(int employeeId)
	{
		int num = _cardModels.FindIndex((EmployeeCardModel m) => m.EmployeeId == employeeId);
		if (num >= 0)
		{
			EnhancedScrollerCellView cellViewAtDataIndex = Scroller.GetCellViewAtDataIndex(num);
			if (cellViewAtDataIndex != null)
			{
				return cellViewAtDataIndex.GetComponent<EmployeeCardView>();
			}
		}
		return null;
	}
}
