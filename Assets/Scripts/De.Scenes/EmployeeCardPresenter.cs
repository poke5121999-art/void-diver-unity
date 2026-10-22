using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class EmployeeCardPresenter : BaseSlotPresenter<EmployeeCardModel>
{
	[InjectModel]
	private EmployeeCardModel _model;

	[View]
	private EmployeeCardView _view;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private AssetManager _assetManager;

	private CompositeDisposable _disposable;

	protected override EmployeeCardModel SlotModel => null;

	protected override Button Button => null;

	protected override void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnInject(EmployeeCardModel model)
	{
	}

	private void SetupName(EmployeeCardModel model, TEmployee employeeData)
	{
	}

	private void SetupPanicGauge(EmployeeCardModel model)
	{
	}

	private void SetupExp(EmployeeCardModel model)
	{
	}

	private void SetupHireInfo(EmployeeCardModel model)
	{
	}

	private void SetupCondition(EmployeeCardModel model)
	{
	}

	private void OnSelectedChanged(bool isSelected)
	{
	}

	private void SetupCleansingCost(EmployeeCardModel model)
	{
	}

	private void SetupSkill(EmployeeCardModel model, TEmployee employeeData, int index, GameObject root, GameObject explain, GameObject get, TextMeshProUGUI nameText, TextMeshProUGUI descText, TextMeshProUGUI getNameText, TextMeshProUGUI getDescText, Image lockIcon)
	{
	}

	private void SetupStatDelta(EmployeeCardModel model, TEmployee employeeData)
	{
	}
}
