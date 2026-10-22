using GameCommon.Enum;

public readonly struct UnitIdentity
{
	public readonly EUnitType UnitType;

	public readonly int TableId;

	public bool IsValid => false;

	public UnitIdentity(EUnitType unitType, int tableId)
	{
		UnitType = EUnitType.None;
		TableId = 0;
	}

	public static UnitIdentity From(UnitModel unitModel)
	{
		return default;
	}
}
