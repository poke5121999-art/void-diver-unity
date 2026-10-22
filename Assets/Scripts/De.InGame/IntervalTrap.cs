using UnityEngine;

public class IntervalTrap : TrapObject
{
	[SerializeField]
	private Animator _animator;

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
