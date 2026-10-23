public class UnitShield
{
	public int Id { get; set; }

	public ulong CasterId { get; set; }

	public int Value { get; set; }

	public int OriginalValue { get; set; }

	public float StartTime { get; set; }

	public float EndTime { get; set; }

	public UnitShield Clone()
	{
		return new UnitShield
		{
			Id = Id,
			CasterId = CasterId,
			Value = Value,
			OriginalValue = OriginalValue,
			StartTime = StartTime,
			EndTime = EndTime
		};
	}
}
