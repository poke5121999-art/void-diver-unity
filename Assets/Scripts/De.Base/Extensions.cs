using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Table;
using SomeNet.Utility;
using UnityEngine;

public static class Extensions
{
	public static GamePlayer ToGamePlayer(this GamePlayerData gamePlayerData)
	{
		return new GamePlayer(gamePlayerData);
	}

	public static GamePlayerData ToGamePlayerData(this GamePlayer gamePlayer)
	{
		GamePlayerData gamePlayerData = new GamePlayerData();
		gamePlayerData.ClientId = gamePlayer.ClientId;
		gamePlayerData.Name = gamePlayer.Name;
		gamePlayerData.IsHostPlayer = gamePlayer.IsHostPlayer;
		gamePlayerData.Character = gamePlayer.Character;
		gamePlayerData.InventorySlots = gamePlayer.InventorySlots.Select((KeyValuePair<int, GoodsSlot> _) => _.Value).ToArray();
		gamePlayerData.SafeInventorySlots = gamePlayer.SafeInventorySlots.Select((KeyValuePair<int, GoodsSlot> _) => _.Value).ToArray();
		gamePlayerData.Talent = gamePlayer.Talent;
		gamePlayerData.Profile = gamePlayer.Profile;
		gamePlayerData.SteamId = gamePlayer.SteamId;
		gamePlayerData.PlayerIndex = gamePlayer.PlayerIndex;
		gamePlayerData.PointMarkers = gamePlayer.PointMarkers;
		return gamePlayerData;
	}

	public static int[] GetParadoxIds(this GamePlayer gamePlayer)
	{
		int[] array = ((gamePlayer != null) ? gamePlayer.Character.GetParadoxIds() : null);
		return array ?? Array.Empty<int>();
	}

	public static int[] GetChangeableParadoxIds(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
		List<int> list = new List<int>();
		if (includePositive && (gamePlayer.Character.PositiveParadoxIds ?? Array.Empty<int>()).Length != 0)
		{
			list.AddRange(gamePlayer.Character.PositiveParadoxIds);
		}
		if (includeNegative && (gamePlayer.Character.NegativeParadoxIds ?? Array.Empty<int>()).Length != 0)
		{
			list.AddRange(gamePlayer.Character.NegativeParadoxIds);
		}
		return list.Where((int _) => _ != gamePlayer.Character.LockedParadoxId).ToArray();
	}

	public static bool HasParadox(this GamePlayer gamePlayer)
	{
		if (gamePlayer == null)
		{
			return false;
		}
		return (gamePlayer.Character.GetParadoxIds() ?? Array.Empty<int>()).Any();
	}

	public static long CalculateRestCost(this GamePlayer gamePlayer)
	{
		float num = TConst.RestCost.IntValue;
		for (int i = 0; i < gamePlayer.Profile.RestCount; i++)
		{
			num *= TConst.RestCostMultiplier.FloatValue;
		}
		return (long)num;
	}

	public static long CalculateLostItemRerollCost(this GamePlayer gamePlayer)
	{
		float num = TConst.LostItemRerollCost.IntValue;
		for (int i = 0; i < gamePlayer.Profile.LostItemRerollCount; i++)
		{
			num *= TConst.LostItemRerollCostMultiplier.FloatValue;
		}
		return (long)num;
	}

	public static bool HasPositiveParadox(this GamePlayer gamePlayer)
	{
		return (((gamePlayer != null) ? gamePlayer.Character.PositiveParadoxIds : null) ?? Array.Empty<int>()).Length != 0;
	}

	public static bool HasNegativeParadox(this GamePlayer gamePlayer)
	{
		return (((gamePlayer != null) ? gamePlayer.Character.NegativeParadoxIds : null) ?? Array.Empty<int>()).Length != 0;
	}

	public static void AcquireRandomParadox(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
		if (!includePositive && !includeNegative)
		{
			return;
		}
		if (includeNegative && includePositive)
		{
			includePositive = RandomUtility.GetRandomBool(0.5f);
		}
		Character character = gamePlayer.Character.Clone();
		if (!includePositive)
		{
			character.NegativeParadoxIds = AcquireRandomParadoxId(character.NegativeParadoxIds);
		}
		else
		{
			character.PositiveParadoxIds = AcquireRandomParadoxId(character.PositiveParadoxIds);
		}
		gamePlayer.Character = character;
		int[] AcquireRandomParadoxId(int[] currentParadoxIds)
		{
			List<int> list = currentParadoxIds?.ToList() ?? new List<int>();
			if (list.Count == TConst.ParadoxCountMax.IntValue)
			{
				var list2 = list.Select((int id, int index) => new { id, index }).Where(_ => _.id != character.LockedParadoxId).ToList();
				var anon = list2[RandomUtility.GetRandomInt(list2.Count)];
				int level = TParadox.Table[list[anon.index]].Level;
				int num = TParadox.Table.PickRandomParadoxId(level, currentParadoxIds, includePositive);
				if (num > 0)
				{
					list[anon.index] = num;
				}
			}
			else
			{
				int item = TParadox.Table.PickRandomParadoxId(0, currentParadoxIds, includePositive);
				list.Add(item);
			}
			return list.ToArray();
		}
	}

	public static void RemoveRandomParadox(this GamePlayer gamePlayer, bool includePositive, bool includeNegative)
	{
		int[] changeableParadoxIds = gamePlayer.GetChangeableParadoxIds(includePositive, includeNegative);
		if (changeableParadoxIds.Any())
		{
			int num = changeableParadoxIds[RandomUtility.GetRandomInt(changeableParadoxIds.Length)];
			gamePlayer.RemoveParadox(num);
		}
	}

	public static int RemoveRandomParadox(this GamePlayer gamePlayer)
	{
		int lockedParadoxId = gamePlayer.Character.LockedParadoxId;
		int[] array = (gamePlayer.Character.GetParadoxIds() ?? Array.Empty<int>()).Where((int _) => _ != lockedParadoxId).ToArray();
		if (!array.Any())
		{
			return -1;
		}
		int num = array[RandomUtility.GetRandomInt(array.Length)];
		gamePlayer.RemoveParadox(num);
		return num;
	}

	public static void AcquireParadox(this GamePlayer gamePlayer, int paradoxId)
	{
		Character character = gamePlayer.Character.Clone();
		if (!TParadox.Table[paradoxId].IsPositive)
		{
			character.NegativeParadoxIds = AcquireParadoxId(character.NegativeParadoxIds);
		}
		else
		{
			character.PositiveParadoxIds = AcquireParadoxId(character.PositiveParadoxIds);
		}
		gamePlayer.Character = character;
		int[] AcquireParadoxId(int[] currentParadoxIds)
		{
			List<int> list = currentParadoxIds?.ToList() ?? new List<int>();
			if (list.Count < TConst.ParadoxCountMax.IntValue)
			{
				list.Add(paradoxId);
			}
			else
			{
				var list2 = list.Select((int id, int index) => new { id, index }).Where(_ => _.id != character.LockedParadoxId).ToList();
				var anon = list2[RandomUtility.GetRandomInt(list2.Count)];
				list[anon.index] = paradoxId;
			}
			return list.ToArray();
		}
	}

	public static void RemoveParadox(this GamePlayer gamePlayer, int paradoxId)
	{
		if (gamePlayer == null || gamePlayer.Character == null)
		{
			return;
		}
		Character character = gamePlayer.Character.Clone();
		if (!TParadox.Table[paradoxId].IsPositive)
		{
			character.NegativeParadoxIds = RemoveParadoxId(character.NegativeParadoxIds);
		}
		else
		{
			character.PositiveParadoxIds = RemoveParadoxId(character.PositiveParadoxIds);
		}
		gamePlayer.Character = character;
		int[] RemoveParadoxId(int[] currentParadoxIds)
		{
			List<int> list = currentParadoxIds?.ToList() ?? new List<int>();
			list.Remove(paradoxId);
			return list.ToArray();
		}
	}

	public static void LockParadox(this GamePlayer gamePlayer, int paradoxId)
	{
		Character character = gamePlayer.Character.Clone();
		character.LockedParadoxId = paradoxId;
		character.PositiveParadoxIds = character.PositiveParadoxIds.OrderByDescending((int _) => _ == paradoxId).ToArray();
		gamePlayer.Character = character;
	}

	public static void UnlockParadox(this GamePlayer gamePlayer)
	{
		Character character = gamePlayer.Character.Clone();
		character.LockedParadoxId = 0;
		gamePlayer.Character = character;
	}

	public static void ResetRestCount(this GamePlayer gamePlayer)
	{
		Profile profile = gamePlayer.Profile.Clone();
		profile.RestCount = 0;
		gamePlayer.Profile = profile;
	}

	public static void ResetLostItemRerollCount(this GamePlayer gamePlayer)
	{
		Profile profile = gamePlayer.Profile.Clone();
		profile.LostItemRerollCount = 0;
		gamePlayer.Profile = profile;
	}

	public static void SetBgmId(this GamePlayer gamePlayer, int bgmId)
	{
		Profile profile = gamePlayer.Profile.Clone();
		profile.BgmId = bgmId;
		gamePlayer.Profile = profile;
	}

	public static Color GetPlayerColor(this GamePlayer gamePlayer)
	{
		string colorCode;
		switch (gamePlayer.PlayerIndex)
		{
		case 0:
			colorCode = "#26D0CC";
			break;
		case 1:
			colorCode = "#FFD200";
			break;
		case 2:
			colorCode = "#DC59EF";
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		return GameCommon.Utility.ColorUtility.ParseOrWhite(colorCode);
	}

	public static List<int> GetInputActiveSkillIds(this Character character)
	{
		List<int> list = new List<int>();
		list.Add(character.AttackSkillId);
		list.Add((character.SelectedActiveSkillIds.Length > 0) ? character.SelectedActiveSkillIds[0] : (-1));
		list.Add(character.DashSkillId);
		list.Add((character.SelectedActiveSkillIds.Length > 1) ? character.SelectedActiveSkillIds[1] : (-1));
		list.Add((character.SelectedActiveSkillIds.Length > 2) ? character.SelectedActiveSkillIds[2] : (-1));
		list.Add((character.SelectedActiveSkillIds.Length > 3) ? character.SelectedActiveSkillIds[3] : (-1));
		list.Add(character.EquipmentActiveSkillId);
		return list;
	}

	public static Dictionary<int, int> GetSkillLevels(this Character character)
	{
		return new Dictionary<int, int>();
	}

	public static int[] GetParadoxIds(this Character character)
	{
		List<int> list = new List<int>();
		if (character.PositiveParadoxIds != null)
		{
			list.AddRange(character.PositiveParadoxIds.Where((int _) => 0 < _));
		}
		if (character.NegativeParadoxIds != null)
		{
			list.AddRange(character.NegativeParadoxIds.Where((int _) => 0 < _));
		}
		return list.ToArray();
	}

	public static bool HasSameSkills(this Character a, Character b)
	{
		if (a.AttackSkillId == b.AttackSkillId && a.DashSkillId == b.DashSkillId && (a.SelectedActiveSkillIds ?? Array.Empty<int>()).SequenceEqual(b.SelectedActiveSkillIds ?? Array.Empty<int>()) && (a.LockedActiveSkillSlots ?? Array.Empty<bool>()).SequenceEqual(b.LockedActiveSkillSlots ?? Array.Empty<bool>()))
		{
			return a.EquipmentActiveSkillId == b.EquipmentActiveSkillId;
		}
		return false;
	}

	public static Character RevertPolymorphSkills(this Character character)
	{
		if (character == null)
		{
			return null;
		}
		TCharacter tCharacter = TCharacter.Table[character.Id];
		Character character2 = character.Clone();
		bool flag = false;
		if (character2.AttackSkillId == tCharacter.PolymorphAttackSkillId || character2.AttackSkillId < 1)
		{
			flag = true;
			character2.AttackSkillId = tCharacter.AttackSkillId;
		}
		if (character2.DashSkillId == tCharacter.PolymorphDashSkillId || character2.DashSkillId < 1)
		{
			flag = true;
			character2.DashSkillId = tCharacter.DashSkillId;
		}
		if (character2.SelectedActiveSkillIds == null || tCharacter.PolymorphActiveSkillIds == null)
		{
			return flag ? character2 : character;
		}
		for (int i = 0; i < character2.SelectedActiveSkillIds.Length; i++)
		{
			int num = character2.SelectedActiveSkillIds[i];
			if (num > 0 && !tCharacter.ActiveSkillIds.Contains(num))
			{
				int num2 = Array.IndexOf(tCharacter.PolymorphActiveSkillIds, num);
				if (num2 != -1 && num2 < tCharacter.ActiveSkillIds.Length)
				{
					character2.SelectedActiveSkillIds[i] = tCharacter.ActiveSkillIds[num2];
					flag = true;
				}
			}
		}
		return flag ? character2 : character;
	}
}
