using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static GamePlayer ToGamePlayer(this GamePlayerData gamePlayerData)
	{
		return null;
	}

	public static GamePlayerData ToGamePlayerData(this GamePlayer gamePlayer)
	{
		return null;
	}

	public static int[] GetParadoxIds(this GamePlayer gamePlayer)
	{
		return null;
	}

	public static int[] GetChangeableParadoxIds(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
		return null;
	}

	public static bool HasParadox(this GamePlayer gamePlayer)
	{
		return false;
	}

	public static long CalculateRestCost(this GamePlayer gamePlayer)
	{
		return 0L;
	}

	public static long CalculateLostItemRerollCost(this GamePlayer gamePlayer)
	{
		return 0L;
	}

	public static bool HasPositiveParadox(this GamePlayer gamePlayer)
	{
		return false;
	}

	public static bool HasNegativeParadox(this GamePlayer gamePlayer)
	{
		return false;
	}

	public static void AcquireRandomParadox(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
	}

	public static void RemoveRandomParadox(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
	}

	public static int RemoveRandomParadox(this GamePlayer gamePlayer)
	{
		return 0;
	}

	public static void AcquireParadox(this GamePlayer gamePlayer, int paradoxId)
	{
	}

	public static void RemoveParadox(this GamePlayer gamePlayer, int paradoxId)
	{
	}

	public static void LockParadox(this GamePlayer gamePlayer, int paradoxId)
	{
	}

	public static void UnlockParadox(this GamePlayer gamePlayer)
	{
	}

	public static void ResetRestCount(this GamePlayer gamePlayer)
	{
	}

	public static void ResetLostItemRerollCount(this GamePlayer gamePlayer)
	{
	}

	public static void SetBgmId(this GamePlayer gamePlayer, int bgmId)
	{
	}

	public static Color GetPlayerColor(this GamePlayer gamePlayer)
	{
		return default;
	}

	public static List<int> GetInputActiveSkillIds(this Character character)
	{
		return null;
	}

	public static Dictionary<int, int> GetSkillLevels(this Character character)
	{
		return null;
	}

	public static int[] GetParadoxIds(this Character character)
	{
		return null;
	}

	public static bool HasSameSkills(this Character a, Character b)
	{
		return false;
	}

	public static Character RevertPolymorphSkills(this Character character)
	{
		return null;
	}
}
