using ProtoBuf;

[ProtoContract]
public class Employee
{
	[ProtoMember(1)]
	public int Id { get; set; }

	[ProtoMember(2)]
	public int Level { get; set; }

	[ProtoMember(3)]
	public int Stress { get; set; }

	[ProtoMember(4)]
	public bool IsPanic { get; set; }

	[ProtoMember(5)]
	public long Exp { get; set; }

	[ProtoMember(6)]
	public int[] LearnedSkillIds { get; set; }

	public int Charm => 0;

	public int Persuasion => 0;

	public int Resistance => 0;

	public static Employee Create(int id, int level, bool withRandomSkills = false)
	{
		return null;
	}

	public Employee Clone()
	{
		return null;
	}
}
