using Febucci.UI;
using GameCommon.Table;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;

public class EmployeeHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private NpcObject _npcObject;

	[View]
	private EmployeeHudView _view;

	private EmployeeNpcObject _employeeNpc;

	protected override GameObject Bubble => _view.Bubble;

	protected override TypewriterByCharacter BubbleTextWriter => _view.Bubble_Text.GetComponent<TypewriterByCharacter>();

	protected override TextMeshProUGUI Monologue => _view.Monologue;

	private void Start()
	{
		_npcObject.RxEmployeeId.Where((int id) => id != 0).Subscribe(OnEmployeeIdSet).AddTo(this);
		_employeeNpc = _npcObject as EmployeeNpcObject;
		if (_employeeNpc != null)
		{
			_employeeNpc.SyncEmployeeState.OnValueChanged += OnEmployeeStateChanged;
			UpdateTycoonHud((ETycoonEmployeeState)_employeeNpc.SyncEmployeeState.Value);
		}
	}

	protected override void OnDestroy()
	{
		if (_employeeNpc != null)
		{
			_employeeNpc.SyncEmployeeState.OnValueChanged -= OnEmployeeStateChanged;
		}
		base.OnDestroy();
	}

	private void OnEmployeeIdSet(int employeeId)
	{
		TEmployee tEmployee = TEmployee.Table[employeeId];
		_view.Name.text = tEmployee.Name;
		_view.SubName.text = tEmployee.SubName;
	}

	private void OnEmployeeStateChanged(int prev, int curr)
	{
		UpdateTycoonHud((ETycoonEmployeeState)curr);
	}

	private void UpdateTycoonHud(ETycoonEmployeeState state)
	{
		_view.TycoonState.SetActive(state != ETycoonEmployeeState.None);
		_view.TycoonState_Complete.SetActive(state == ETycoonEmployeeState.Complete);
		_view.TycoonState_Panic.SetActive(state == ETycoonEmployeeState.Panic);
		_view.TycoonState_WaitMark.SetActive(state == ETycoonEmployeeState.InProgress);
	}
}
