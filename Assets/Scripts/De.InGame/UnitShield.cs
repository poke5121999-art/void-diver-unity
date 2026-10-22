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
		return null;
	}
}
