using GameCommon.Enum;
using UnityEngine;

public class CollectableObject : NetworkBase, IActionStats
{
	[SerializeField]
	private int _collectableObjectId;

	public EElementalType ElementalType => EElementalType.None;

	public float WeaponAtkMultiplier => 0f;

	public float GetStat(EStatType statType)
	{
		return 0f;
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
