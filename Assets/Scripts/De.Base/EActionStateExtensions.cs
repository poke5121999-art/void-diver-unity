using System.Collections.Generic;
using GameCommon.Enum;

public static class EActionStateExtensions
{
	private static readonly HashSet<EActionState> _combatActionStates = new HashSet<EActionState>
	{
		EActionState.Skill,
		EActionState.Spawn,
		EActionState.Aggro,
		EActionState.MoveToTarget,
		EActionState.Flee,
		EActionState.KiteFromTarget
	};

	public static bool IsCombatState(this EActionState state)
	{
		return _combatActionStates.Contains(state);
	}
}
