using GameCommon.Data;
using ProtoBuf;

[ProtoContract]
public class SalesSlot
{
	[ProtoMember(1)]
	public int Id { get; set; }

	[ProtoMember(2)]
	public int EmployeeId { get; set; }

	[ProtoMember(3)]
	public Goods Goods { get; set; }

	[ProtoMember(4)]
	public float WorkProgress { get; set; }

	[ProtoMember(5)]
	public float MaxWorkProgress { get; set; }

	[ProtoMember(6)]
	public bool IsDisabledForTurn { get; set; }

	[ProtoMember(7)]
	public bool IsFinished { get; set; }

	[ProtoMember(8)]
	public int TycoonCustomerNpcId { get; set; }

	[ProtoMember(9)]
	public bool IsStarted { get; set; }

	public SalesSlot Clone()
	{
		return new SalesSlot
		{
			Id = Id,
			EmployeeId = EmployeeId,
			Goods = Goods?.Clone(),
			WorkProgress = WorkProgress,
			MaxWorkProgress = MaxWorkProgress,
			IsDisabledForTurn = IsDisabledForTurn,
			IsFinished = IsFinished,
			TycoonCustomerNpcId = TycoonCustomerNpcId,
			IsStarted = IsStarted
		};
	}
}
