using ProtoBuf;

[ProtoContract]
public class FiredEmployeeRecord
{
	[ProtoMember(1)]
	public int EmployeeId { get; set; }

	[ProtoMember(2)]
	public int RemainingCooldown { get; set; }

	public FiredEmployeeRecord Clone()
	{
		FiredEmployeeRecord result = new FiredEmployeeRecord();
		result.EmployeeId = EmployeeId;
		result.RemainingCooldown = RemainingCooldown;
		return result;
	}
}
