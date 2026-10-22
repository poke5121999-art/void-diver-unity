using Febucci.UI;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class EmployeeHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private NpcObject _npcObject;

	[View]
	private EmployeeHudView _view;

	private EmployeeNpcObject _employeeNpc;

	protected override GameObject Bubble => null;

	protected override TypewriterByCharacter BubbleTextWriter => null;

	protected override TextMeshProUGUI Monologue => null;

	private void Start()
	{
	}

	protected override void OnDestroy()
	{
	}

	private void OnEmployeeIdSet(int employeeId)
	{
	}

	private void OnEmployeeStateChanged(int prev, int curr)
	{
	}

	private void UpdateTycoonHud(ETycoonEmployeeState state)
	{
	}
}
