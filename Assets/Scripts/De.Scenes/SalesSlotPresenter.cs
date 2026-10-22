using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class SalesSlotPresenter : BaseSlotPresenter<SalesSlotModel>
{
	[View]
	private SalesSlotView _view;

	[InjectModel]
	private SalesSlotModel _model;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private PlayerManager _playerManager;

	protected override SalesSlotModel SlotModel => _model;

	private void OnInject(SalesSlotModel model)
	{
		InitSlot(model);
		_view.Text.text = TTycoonSalesSlot.Table[model.Id].Name;
		Refresh();
		_tycoonManager.OnSalesSlotsChanged.Subscribe(delegate
		{
			Refresh();
		}).AddTo(Disposable);
		_tycoonManager.OnEmployeesChanged.Subscribe(delegate
		{
			Refresh();
		}).AddTo(Disposable);
	}

	private void Refresh()
	{
		if (!_playerManager.SalesSlots.TryGetValue(_model.Id, out var slot))
		{
			return;
		}
		Employee employee = null;
		if (slot.EmployeeId > 0 && _playerManager.Employees.ContainsKey(slot.EmployeeId))
		{
			employee = _playerManager.Employees[slot.EmployeeId];
		}
		_view.Locked.SetActive(slot.IsDisabledForTurn);
		_view.Finished.SetActive(slot.IsFinished);
		SetupEmployee(slot, employee);
		SetupArtifact(slot);
		SetupSellSlider(slot);
	}

	private void SetupEmployee(SalesSlot slot, Employee employee)
	{
		int employeeId = slot.EmployeeId;
		_view.EmployeeSimpleSlot.gameObject.SetActive(employeeId > 0);
		_view.EmployeeSlotEmpty.gameObject.SetActive(employeeId < 1);
		if (employeeId < 1)
		{
			_view.Employee_TextEmpty.gameObject.SetActive(value: true);
			_view.Employee_TextName.gameObject.SetActive(value: false);
			_view.Employee_TextNamePanic.gameObject.SetActive(value: false);
			_view.Employee_TextLv.gameObject.SetActive(value: false);
			return;
		}
		bool isPanic = employee != null && employee.IsPanic;
		TEmployee employeeData = TEmployee.Table[slot.EmployeeId];
		_view.Employee_TextEmpty.gameObject.SetActive(value: false);
		_view.Employee_TextName.gameObject.SetActive(!isPanic);
		_view.Employee_TextNamePanic.gameObject.SetActive(isPanic);
		(isPanic ? _view.Employee_TextNamePanic : _view.Employee_TextName).text = employeeData.Name;
		_view.Employee_TextLv.gameObject.SetActive(value: true);
		_view.Employee_TextLv.text = TLocalizedText.LevelFormat.Format(employee?.Level ?? 0);
		_view.EmployeeSimpleSlot.GetComponent<EmployeeSimpleSlotPresenter>().Inject(new EmployeeSimpleSlotModel
		{
			EmployeeId = slot.EmployeeId,
			Level = employee?.Level ?? 0,
			Exp = employee?.Exp ?? 0,
			Stress = employee?.Stress ?? 0,
			IsPanic = isPanic
		});
	}

	private void SetupArtifact(SalesSlot slot)
	{
		var goods = slot.Goods;
		_view.Artifact.SetActive(value: true);
		_view.GoodsSlotEmpty.gameObject.SetActive(goods == null);
		if (goods == null)
		{
			_view.Artifact_TextEmpty.gameObject.SetActive(value: true);
			_view.Artifact_TextGrade.gameObject.SetActive(value: false);
			_view.Artifact_TextName.gameObject.SetActive(value: false);
			_view.GoodsSlot.gameObject.SetActive(value: false);
			return;
		}
		_view.Artifact_TextEmpty.gameObject.SetActive(value: false);
		_view.Artifact_TextName.gameObject.SetActive(value: true);
		_view.Artifact_TextName.text = TEquipment.Table[goods.Id].Name;
		EGoodsGradeType grade = TEquipment.Table[goods.Id].Grade;
		_view.Artifact_TextGrade.gameObject.SetActive(grade != EGoodsGradeType.None);
		_view.Artifact_TextGrade.text = grade.ToLocalText();
		_view.Artifact_TextGrade.color = grade.ToColor(_view.Artifact_TextGrade.color.a);
		_view.GoodsSlot.gameObject.SetActive(value: true);
		_view.GoodsSlot.Inject(new InventoryGoodsSlotModel(goods));
	}

	private void SetupSellSlider(SalesSlot slot)
	{
		if (slot.MaxWorkProgress <= 0f)
		{
			_view.SellSlider.gameObject.SetActive(value: false);
			return;
		}
		_view.SellSlider.gameObject.SetActive(value: true);
		_view.SellSlider.value = 1f - slot.WorkProgress / slot.MaxWorkProgress;
	}
}
