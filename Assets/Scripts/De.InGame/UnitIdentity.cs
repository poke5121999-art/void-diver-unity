using GameCommon.Enum;

public readonly struct UnitIdentity
{
	public readonly EUnitType UnitType;

	public readonly int TableId;

	public bool IsValid => UnitType != EUnitType.None;

	public UnitIdentity(EUnitType unitType, int tableId)
	{
		UnitType = unitType;
		TableId = tableId;
	}

	public static UnitIdentity From(UnitModel unitModel)
	{
		if (unitModel == null)
		{
			return default;
		}
		return new UnitIdentity(unitModel.UnitType, unitModel.TableId);
	}
}
