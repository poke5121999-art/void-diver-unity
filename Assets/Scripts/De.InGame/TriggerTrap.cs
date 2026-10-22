using System.Collections.Generic;
using UnityEngine;

public class TriggerTrap : TrapObject
{
	[SerializeField]
	protected Collider[] TriggerColliders;

	[SerializeField]
	private Animator _animator;

	private readonly HashSet<UnitModel> _targetDatas;

	public override void OnNetworkSpawn()
	{
	}

	protected override bool CheckExecutable()
	{
		return false;
	}

	private void OnTriggerEnter(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
	}

	protected override void OnIsActivatedValueChanged(bool oldValue, bool newValue)
	{
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
