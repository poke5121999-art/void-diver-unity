using System;
using System.Linq;
using GameCommon.Table;
using SomeNet.Utility;
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

	public int Charm => (int)((float)(Level - 1) * TEmployee.Table[Id].CharmGrowth + (float)TEmployee.Table[Id].Charm);

	public int Persuasion => (int)((float)(Level - 1) * TEmployee.Table[Id].PersuasionGrowth + (float)TEmployee.Table[Id].Persuasion);

	public int Resistance => (int)((float)(Level - 1) * TEmployee.Table[Id].ResistanceGrowth + (float)TEmployee.Table[Id].Resistance);

	public static Employee Create(int id, int level, bool withRandomSkills = false)
	{
		TEmployee employeeData = TEmployee.Table[id];
		Employee employee = new Employee
		{
			Id = id,
			Level = level,
			LearnedSkillIds = GetForcedSkillIds()
		};
		if (!withRandomSkills)
		{
			return employee;
		}
		if (employeeData.SkillIds == null)
		{
			return employee;
		}
		float learnPercent = TConst.TycoonEmployeeSkillLearnPercent.FloatValue;
		float learnPercentPerLevel = TConst.TycoonEmployeeSkillLearnPercentPerLevel.FloatValue;
		int slotsToFill = 2 - employee.LearnedSkillIds.Length;
		for (int i = 0; i < slotsToFill; i++)
		{
			if (employee.LearnedSkillIds.Length > 1)
			{
				break;
			}
			int candidate = GetRandomSkillCandidate();
			if (candidate == 0)
			{
				break;
			}
			if (RandomUtility.GetRandomBoolFromPercentage((float)(level - 1) * learnPercentPerLevel + learnPercent))
			{
				int[] grown = new int[employee.LearnedSkillIds.Length + 1];
				employee.LearnedSkillIds.CopyTo(grown, 0);
				grown[employee.LearnedSkillIds.Length] = candidate;
				employee.LearnedSkillIds = grown;
			}
		}
		return employee;

		int[] GetForcedSkillIds()
		{
			if (employeeData.SkillIds == null || employeeData.ForcedSkillLearnLevels == null)
			{
				return Array.Empty<int>();
			}
			return employeeData.SkillIds
				.Select((int skillId, int i) => new { SkillId = skillId, ForcedLevel = (i < employeeData.ForcedSkillLearnLevels.Length) ? employeeData.ForcedSkillLearnLevels[i] : 0 })
				.Where(x => x.ForcedLevel > 0 && x.ForcedLevel <= level)
				.Select(x => x.SkillId)
				.ToArray();
		}

		int GetRandomSkillCandidate()
		{
			return employeeData.SkillIds
				.Select((int skillId, int idx) => new { SkillId = skillId, ForcedLevel = (employeeData.ForcedSkillLearnLevels != null && idx < employeeData.ForcedSkillLearnLevels.Length) ? employeeData.ForcedSkillLearnLevels[idx] : int.MaxValue })
				.Where(x => !Array.Exists(employee.LearnedSkillIds, (int learnedId) => learnedId == x.SkillId))
				.OrderBy(x => x.ForcedLevel)
				.Select(x => x.SkillId)
				.FirstOrDefault();
		}
	}

	public Employee Clone()
	{
		return new Employee
		{
			Id = Id,
			Level = Level,
			Stress = Stress,
			IsPanic = IsPanic,
			Exp = Exp,
			LearnedSkillIds = (int[])LearnedSkillIds?.Clone()
		};
	}
}
