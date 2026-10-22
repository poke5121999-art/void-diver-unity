namespace GameCommon.Enum
{
	public enum EActionState : byte
	{
		None = 0,
		Idle = 1,
		Move = 2,
		Skill = 3,
		Dying = 4,
		Dead = 5,
		Revive = 6,
		CrowdControlled = 7,
		Escaped = 8,
		Interact = 9,
		ReservedDespawn = 10,
		Spawn = 11,
		StageReady = 12,
		Cutscene = 13,
		MoveToTarget = 101,
		Wander = 102,
		Flee = 103,
		Avoid = 104,
		Pause = 105,
		KiteFromTarget = 106,
		Alert = 107,
		Aggro = 108,
		LostTarget = 109,
		ReceiveAggroSpread = 110,
		Curious = 111,
		PhaseUp = 112,
		Groggy = 113,
		Captured = 114
	}
}
