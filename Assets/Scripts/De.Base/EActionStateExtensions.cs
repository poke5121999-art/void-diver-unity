using System.Collections.Generic;
using GameCommon.Enum;

public static class EActionStateExtensions
{
	private static readonly HashSet<EActionState> _combatActionStates;

	public static bool IsCombatState(this EActionState state)
	{
		return false;
	}
}
