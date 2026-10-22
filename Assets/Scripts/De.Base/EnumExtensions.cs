using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using Steamworks;
using UnityEngine;

public static class EnumExtensions
{
	private static readonly Color _goodsColorNormal = new Color(137f / 255f, 137f / 255f, 137f / 255f, 1f);

	private static readonly Color _goodsColorRare = new Color(46f / 255f, 155f / 255f, 143f / 255f, 1f);

	private static readonly Color _goodsColorElite = new Color(62f / 255f, 127f / 255f, 224f / 255f, 1f);

	private static readonly Color _goodsColorEpic = new Color(193f / 255f, 65f / 255f, 204f / 255f, 1f);

	private static readonly Color _goodsColorLegend = new Color(1f, 143f / 255f, 43f / 255f, 1f);

	private static readonly Color _goodsColorUnique = new Color(1f, 246f / 255f, 145f / 255f, 1f);

	public static string ToLocalText(this Enum enumValue)
	{
		string text = string.Format("{0}_{1}", enumValue.GetType().Name, enumValue);
		if (!LocalizationUtility.TryGetLocalizedText(text, out var text2))
		{
			UnityEngine.Debug.LogWarning("Localized Text not found. Key: " + text);
		}
		return text2;
	}

	public static string ToLocalText(this Enum enumValue, string suffix)
	{
		string text = string.Format("{0}_{1}_{2}", enumValue.GetType().Name, enumValue, suffix);
		if (!LocalizationUtility.TryGetLocalizedText(text, out var text2))
		{
			UnityEngine.Debug.LogWarning("Localized Text not found. Key: " + text);
		}
		return text2;
	}

	public static string ToExpressionText(this Enum enumValue)
	{
		return string.Format("{0}:{1}", enumValue.GetType().Name, enumValue);
	}

	public static string ToStatText(this EStatType statType, float value)
	{
		string localizedText = LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}", statType.GetType().Name, statType));
		string text = ((value >= 0f) ? "+" : "");
		string text2 = string.Format("{0:0.#}", value);
		if (statType.ToString().EndsWith("Percent"))
		{
			return localizedText + " " + text + text2 + "%";
		}
		return localizedText + " " + text + text2;
	}

	public static string GetColorCode(this EElementalType elementalType)
	{
		switch (elementalType)
		{
		case EElementalType.Fire:
			return "#a74200";
		case EElementalType.Water:
			return "#0071c0";
		case EElementalType.Wind:
			return "#00a786";
		default:
			return "#FFFFFF";
		}
	}

	public static EElementalType GetElementalType(this EStatusEffectTag buffEffectType)
	{
		if (buffEffectType == EStatusEffectTag.Burn)
		{
			return EElementalType.Fire;
		}
		if (buffEffectType == EStatusEffectTag.FirePower)
		{
			return EElementalType.Fire;
		}
		return EElementalType.None;
	}

	public static string GetSpeakerName(this ESpeaker speaker, int tableId, string playerName, int playerCharacterId)
	{
		switch (speaker)
		{
		case ESpeaker.Player:
			return playerName;
		case ESpeaker.Character:
			return TCharacter.Table[tableId].Name;
		case ESpeaker.Npc:
			return TNpc.Table[tableId].Name;
		case ESpeaker.Monster:
			return TMonster.Table[tableId].Name;
		case ESpeaker.PlayerCharacter:
			return TCharacter.Table[playerCharacterId].Name;
		default:
			throw new ArgumentOutOfRangeException("speaker", speaker, null);
		}
	}

	public static EStatusEffectTag GetEffectTag(this ECrowdControlType type)
	{
		switch (type)
		{
		case ECrowdControlType.Stun:
			return EStatusEffectTag.Stun;
		case ECrowdControlType.Knockback:
			return EStatusEffectTag.Knockback;
		case ECrowdControlType.Pull:
			return EStatusEffectTag.Pull;
		case ECrowdControlType.Freeze:
			return EStatusEffectTag.Freeze;
		default:
			return EStatusEffectTag.None;
		}
	}

	public static ECrowdControlType GetCcType(this EStatusEffectTag tag)
	{
		switch (tag)
		{
		case EStatusEffectTag.Stun:
			return ECrowdControlType.Stun;
		case EStatusEffectTag.Knockback:
			return ECrowdControlType.Knockback;
		case EStatusEffectTag.Pull:
			return ECrowdControlType.Pull;
		case EStatusEffectTag.Freeze:
			return ECrowdControlType.Freeze;
		default:
			return ECrowdControlType.None;
		}
	}

	public static Color ToColor(this EOutlineType outlineType)
	{
		switch (outlineType)
		{
		case EOutlineType.None:
			return new Color(0f, 0f, 0f, 1f);
		case EOutlineType.Quest:
			return new Color(1f, 0f, 0f, 1f);
		case EOutlineType.Interaction:
			return new Color(0f, 1f, 0f, 1f);
		default:
			return new Color(1f, 1f, 1f, 1f);
		}
	}

	public static float ToStroke(this EOutlineType outlineType)
	{
		switch (outlineType)
		{
		case EOutlineType.None:
			return 0f;
		case EOutlineType.Quest:
			return 4f;
		case EOutlineType.Interaction:
			return 3.5f;
		default:
			return 1f;
		}
	}

	public static bool ToStartActive(this EOutlineType outlineType)
	{
		if (outlineType == EOutlineType.None || outlineType == EOutlineType.Default || (outlineType != EOutlineType.Quest && outlineType != EOutlineType.Interaction))
		{
			return false;
		}
		return true;
	}

	public static Color ToColor(this EGoodsGradeType goodsGradeType)
	{
		switch (goodsGradeType)
		{
		case EGoodsGradeType.Rare:
			return _goodsColorRare;
		case EGoodsGradeType.Elite:
			return _goodsColorElite;
		case EGoodsGradeType.Epic:
			return _goodsColorEpic;
		case EGoodsGradeType.Legend:
			return _goodsColorLegend;
		case EGoodsGradeType.Unique:
			return _goodsColorUnique;
		default:
			return _goodsColorNormal;
		}
	}

	public static Color ToColor(this EGoodsGradeType grade, float alpha)
	{
		Color result = grade.ToColor();
		result.a = alpha;
		return result;
	}

	public static int ToFrameRate(this EFrameRate frameRate)
	{
		switch (frameRate)
		{
		case EFrameRate.Frame15:
			return 15;
		case EFrameRate.Frame30:
			return 30;
		case EFrameRate.Frame60:
			return 60;
		case EFrameRate.Frame144:
			return 144;
		default:
			throw new ArgumentOutOfRangeException("frameRate", frameRate, null);
		}
	}

	public static int ToWidth(this EResolution resolution)
	{
		switch (resolution)
		{
		case EResolution.Native:
			return Display.main.systemWidth;
		case EResolution._1280x720:
			return 1280;
		case EResolution._1360x768:
			return 1360;
		case EResolution._1366x768:
			return 1366;
		case EResolution._1600x900:
			return 1600;
		case EResolution._1920x1080:
			return 1920;
		case EResolution._2560x1440:
			return 2560;
		default:
			throw new ArgumentOutOfRangeException("resolution", resolution, null);
		}
	}

	public static int ToHeight(this EResolution resolution)
	{
		switch (resolution)
		{
		case EResolution.Native:
			return Display.main.systemHeight;
		case EResolution._1280x720:
			return 720;
		case EResolution._1360x768:
		case EResolution._1366x768:
			return 768;
		case EResolution._1600x900:
			return 900;
		case EResolution._1920x1080:
			return 1080;
		case EResolution._2560x1440:
			return 1440;
		default:
			throw new ArgumentOutOfRangeException("resolution", resolution, null);
		}
	}

	public static string ToColoredLocalText(this EDifficulty difficulty)
	{
		string text;
		switch (difficulty)
		{
		case EDifficulty.Easy:
			text = "<color=#898989>";
			break;
		case EDifficulty.Normal:
			text = "<color=#007700>";
			break;
		case EDifficulty.Hard:
			text = "<color=#a5a500>";
			break;
		case EDifficulty.Insane:
			text = "<color=#a91414>";
			break;
		default:
			text = "<color=#FFFFFF>";
			break;
		}
		string localizedText = LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}", difficulty.GetType().Name, difficulty));
		return text + localizedText + "</color>";
	}

	public static Color ToBackgroundColor(this EDifficulty difficulty, float alpha = 1f)
	{
		Color color = default(Color);
		string htmlString;
		switch (difficulty)
		{
		case EDifficulty.Easy:
			htmlString = "#007700";
			break;
		case EDifficulty.Normal:
			htmlString = "#AAA400";
			break;
		case EDifficulty.Hard:
			htmlString = "#C37D37";
			break;
		case EDifficulty.Insane:
			htmlString = "#B21E1E";
			break;
		default:
			htmlString = "#FFFFFF";
			break;
		}
		ColorUtility.TryParseHtmlString(htmlString, out color);
		color.a = alpha;
		return color;
	}

	public static bool IsPointPing(this EPingType pintType)
	{
		return (int)pintType < 41;
	}

	public static bool IsEnemyTargetType(this EHitTarget targetType)
	{
		if ((uint)(targetType - 4) < 3u)
		{
			return true;
		}
		return targetType == EHitTarget.TargetOnly;
	}

	public static int GetRandomArtifactId(this EGoodsGradeType grade)
	{
		List<TEquipment> list = TEquipment.Table.Values.Where((TEquipment _) => _.IsArtifact() && _.Grade == grade).ToList();
		if (list.Count == 0)
		{
			return -1;
		}
		return list[RandomUtility.GetRandomInt(0, list.Count)].Id;
	}

	public static bool IsPenalty(this EElementalDamagedType type)
	{
		if ((uint)(type - 1) < 2u)
		{
			return true;
		}
		return type == EElementalDamagedType.WindPenalty;
	}

	public static bool IsResistance(this EElementalDamagedType type)
	{
		if ((uint)(type - 4) < 2u)
		{
			return true;
		}
		return type == EElementalDamagedType.WindResistance;
	}

	public static string GetRevealSfx(this EGoodsGradeType type)
	{
		switch (type)
		{
		case EGoodsGradeType.None:
		case EGoodsGradeType.Normal:
			return "Looting_low";
		case EGoodsGradeType.Rare:
			return "Looting_middle";
		case EGoodsGradeType.Elite:
		case EGoodsGradeType.Epic:
			return "Looting_high";
		case EGoodsGradeType.Legend:
		case EGoodsGradeType.Unique:
			return "Looting_veryhigh";
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public static float GetRevealTime(this EGoodsGradeType type)
	{
		switch (type)
		{
		case EGoodsGradeType.None:
		case EGoodsGradeType.Normal:
			return 0.8f;
		case EGoodsGradeType.Rare:
			return 1.1f;
		case EGoodsGradeType.Elite:
			return 2.2f;
		case EGoodsGradeType.Epic:
			return 3.1f;
		case EGoodsGradeType.Legend:
		case EGoodsGradeType.Unique:
			return 4f;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public static Color GetColor(this ERoomDifficulty difficulty)
	{
		uint num;
		switch (difficulty)
		{
		case ERoomDifficulty.Story:
			num = 0xFFB5B5B5u;
			break;
		case ERoomDifficulty.Junior:
			num = 0xFF007700u;
			break;
		case ERoomDifficulty.Classic:
			num = 0xFF009999u;
			break;
		case ERoomDifficulty.Veteran:
			num = 0xFF000099u;
			break;
		default:
			throw new ArgumentOutOfRangeException("difficulty", difficulty, null);
		}
		return new Color((float)(num & 0xFF) / 255f, (float)((num >> 8) & 0xFF) / 255f, (float)((num >> 16) & 0xFF) / 255f, (float)(num >> 24) / 255f);
	}

	public static ERoomDifficulty GetNextDifficulty(this ERoomDifficulty current)
	{
		ERoomDifficulty[] array = (ERoomDifficulty[])Enum.GetValues(typeof(ERoomDifficulty));
		int num = Array.IndexOf(array, current);
		return array[(num + 1) % array.Length];
	}

	public static ERoomDifficulty GetPrevDifficulty(this ERoomDifficulty current)
	{
		ERoomDifficulty[] array = (ERoomDifficulty[])Enum.GetValues(typeof(ERoomDifficulty));
		int num = Array.IndexOf(array, current);
		return array[(num - 1 + array.Length) % array.Length];
	}

	public static ELobbyType ToSteamLobbyType(this ERoomPublishType publishType)
	{
		if (publishType != ERoomPublishType.Private)
		{
			return ELobbyType.k_ELobbyTypePublic;
		}
		return ELobbyType.k_ELobbyTypeInvisible;
	}

	public static ERoomPublishType ToRoomPublishType(this ELobbyType lobbyType)
	{
		if (lobbyType == ELobbyType.k_ELobbyTypePublic)
		{
			return ERoomPublishType.Public;
		}
		return (lobbyType != ELobbyType.k_ELobbyTypeInvisible) ? ERoomPublishType.Public : ERoomPublishType.Private;
	}

	public static ERoomPublishType GetNextPublishType(this ERoomPublishType current)
	{
		ERoomPublishType[] array = (ERoomPublishType[])Enum.GetValues(typeof(ERoomPublishType));
		int num = Array.IndexOf(array, current);
		return array[(num + 1) % array.Length];
	}

	public static ERoomPublishType GetPrevPublishType(this ERoomPublishType current)
	{
		ERoomPublishType[] array = (ERoomPublishType[])Enum.GetValues(typeof(ERoomPublishType));
		int num = Array.IndexOf(array, current);
		return array[(num - 1 + array.Length) % array.Length];
	}

	public static string GetRoomDifficultyDescription(this ERoomDifficulty difficulty)
	{
		switch (difficulty)
		{
		case ERoomDifficulty.Story:
			return TLocalizedText.ERoomDifficultyStoryDesc.LocalText;
		case ERoomDifficulty.Junior:
			return TLocalizedText.ERoomDifficultyJuniorDesc.LocalText;
		case ERoomDifficulty.Classic:
			return TLocalizedText.ERoomDifficultyClassicDesc.LocalText;
		case ERoomDifficulty.Veteran:
			return TLocalizedText.ERoomDifficultyVeteranDesc.LocalText;
		default:
			return string.Empty;
		}
	}

	public static float GetGoodsRemovalPercent(this ERoomDifficulty difficulty)
	{
		switch (difficulty)
		{
		case ERoomDifficulty.Story:
			return TConst.GoodsRemovalPercentStory.FloatValue;
		case ERoomDifficulty.Junior:
			return TConst.GoodsRemovalPercentJunior.FloatValue;
		case ERoomDifficulty.Classic:
			return TConst.GoodsRemovalPercentClassic.FloatValue;
		case ERoomDifficulty.Veteran:
			return TConst.GoodsRemovalPercentVeteran.FloatValue;
		default:
			return 0f;
		}
	}

	public static EGoodsSlotCategory GetSlotCategory(this EGoodsType goodsSubType)
	{
		switch (goodsSubType)
		{
		case EGoodsType.Weapon:
			return EGoodsSlotCategory.WeaponSlot;
		case EGoodsType.Accessory:
			return EGoodsSlotCategory.AccessorySlot;
		case EGoodsType.Artifact:
			return EGoodsSlotCategory.ArtifactSlot;
		default:
			return EGoodsSlotCategory.None;
		}
	}

	public static bool IsInventoryCategory(this EGoodsSlotCategory category)
	{
		if ((uint)(category - 1) < 3u)
		{
			return true;
		}
		return category == EGoodsSlotCategory.StashSlot;
	}

	public static bool IsEquipmentCategory(this EGoodsSlotCategory category)
	{
		if ((uint)(category - 5) < 2u)
		{
			return true;
		}
		return category == EGoodsSlotCategory.ArtifactSlot;
	}

	public static bool IsQuickSlotCategory(this EGoodsSlotCategory category)
	{
		return category == EGoodsSlotCategory.QuickSlot;
	}

	public static float GetArtifactDealExpMultiplier(this EGoodsGradeType grade)
	{
		switch (grade)
		{
		case EGoodsGradeType.Rare:
			return TConst.ArtifactDealRareExpMultiplier.FloatValue;
		case EGoodsGradeType.Elite:
			return TConst.ArtifactDealEliteExpMultiplier.FloatValue;
		case EGoodsGradeType.Epic:
			return TConst.ArtifactDealEpicExpMultiplier.FloatValue;
		case EGoodsGradeType.Legend:
			return TConst.ArtifactDealLegendExpMultiplier.FloatValue;
		case EGoodsGradeType.Unique:
			return TConst.ArtifactDealUniqueExpMultiplier.FloatValue;
		default:
			return TConst.ArtifactDealNormalExpMultiplier.FloatValue;
		}
	}

	public static string GetQuestName(this EQuestType questType, int tableId)
	{
		switch (questType)
		{
		case EQuestType.Campaign:
			return TCampaign.Table[tableId].Name;
		case EQuestType.Mission:
			return TMission.Table[tableId].Name;
		case EQuestType.Lounge:
			return TLoungeQuest.Table[tableId].Name;
		default:
			return string.Empty;
		}
	}

	public static bool IsBoss(this EMonsterType monsterType)
	{
		if (monsterType == EMonsterType.Boss)
		{
			return true;
		}
		return monsterType == EMonsterType.MiniBoss;
	}

	public static string GetFailTypeName(this ETycoonFailType failType)
	{
		return LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}", failType.GetType().Name, failType));
	}

	public static string GetFailTypeDesc(this ETycoonFailType failType)
	{
		return LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}_{2}", failType.GetType().Name, failType, "Desc"));
	}

	public static string ToDescription(this EArtifactDealCardEffectType effectType, long value1)
	{
		string text = "";
		switch (effectType)
		{
		case EArtifactDealCardEffectType.IncreasePricePercent:
		case EArtifactDealCardEffectType.DecreasePricePercent:
		case EArtifactDealCardEffectType.AddPreferTags:
		case EArtifactDealCardEffectType.RerollPreferTags:
		case EArtifactDealCardEffectType.RerollDislikeTags:
		case EArtifactDealCardEffectType.RemoveDislikeTags:
		case EArtifactDealCardEffectType.IncreaseBonusSuccessPercent:
			text = string.Format("{0}", value1);
			break;
		case EArtifactDealCardEffectType.AddSpecificPreferTag:
			text = TArtifactPrefix.Table[(int)value1].Name;
			break;
		}
		string localizedText = LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}", effectType.GetType().Name, effectType));
		return string.Format(localizedText, text);
	}

	public static string ToDescription(this EEventConditionType conditionType, long conditionValue1, long conditionValue2)
	{
		string text = "";
		string text2 = "";
		switch (conditionType)
		{
		case EEventConditionType.MonsterKill:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyMonster.LocalText : TMonster.Table[(int)conditionValue1].Name);
			break;
		case EEventConditionType.WaveClear:
			text = TWave.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.OpenRewardBox:
			text = string.Format("{0}", conditionValue1);
			break;
		case EEventConditionType.StageEscape:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyCampaign.LocalText : TCampaign.Table[(int)conditionValue1].Name);
			text2 = ((conditionValue2 < 0) ? TLocalizedText.AnyDifficulty.LocalText : ((EDifficulty)conditionValue2).ToLocalText());
			break;
		case EEventConditionType.CampaignClear:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyCampaign.LocalText : TCampaign.Table[(int)conditionValue1].Name);
			break;
		case EEventConditionType.MissionClear:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyMission.LocalText : TMission.Table[(int)conditionValue1].Name);
			break;
		case EEventConditionType.ItemUse:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyItem.LocalText : TItem.Table[(int)conditionValue1].Name);
			break;
		case EEventConditionType.ArtifactDealFinish:
			text = ((conditionValue1 < 1) ? TLocalizedText.AnyArtifactDeal.LocalText : string.Format("{0}", conditionValue1));
			text2 = ((conditionValue2 < 1) ? TLocalizedText.AnyArtifact.LocalText : TEquipment.Table[(int)conditionValue2].Name);
			break;
		case EEventConditionType.EmployeeHire:
		case EEventConditionType.EmployeeFire:
			text = ((conditionValue1 > 0) ? TEmployee.Table[(int)conditionValue1].Name : TLocalizedText.AnyEmployee.LocalText);
			break;
		case EEventConditionType.AccountRecord:
		case EEventConditionType.UserLevel:
			text = string.Format("{0}", conditionValue1);
			break;
		case EEventConditionType.MyItemCountInInventory:
		case EEventConditionType.TeamItemCountInInventory:
		case EEventConditionType.MyItemCountTotal:
		case EEventConditionType.ItemCollected:
		case EEventConditionType.ItemUsed:
			text = TItem.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.CampaignCleared:
			text = TCampaign.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.MissionCleared:
			text = TMission.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.LoungeQuestCleared:
			text = TLoungeQuest.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.Talent:
			text = TTalent.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.AreaUnlocked:
			text = TArea.Table[(int)conditionValue1].Name;
			break;
		case EEventConditionType.AreaDecorationUnlocked:
			text = TAreaDecoration.Table[(int)conditionValue1].Name;
			break;
		}
		string localizedText = LocalizationUtility.GetLocalizedText(string.Format("{0}_{1}", conditionType.GetType().Name, conditionType));
		return string.Format(localizedText, text, text2);
	}

	public static EEventConditionValueType GetValueType(this EEventConditionType conditionType)
	{
		switch (conditionType)
		{
		case EEventConditionType.None:
			return EEventConditionValueType.None;
		case EEventConditionType.MonsterKill:
		case EEventConditionType.WaveClear:
		case EEventConditionType.OpenRewardBox:
		case EEventConditionType.StageEscape:
		case EEventConditionType.CampaignClear:
		case EEventConditionType.MissionClear:
		case EEventConditionType.ItemUse:
		case EEventConditionType.ArtifactDealFinish:
		case EEventConditionType.EmployeeHire:
		case EEventConditionType.EmployeeFire:
		case EEventConditionType.GainedCoin:
			return EEventConditionValueType.Increment;
		case EEventConditionType.AccountRecord:
		case EEventConditionType.LuaProgress:
		case EEventConditionType.MyItemCountInInventory:
		case EEventConditionType.TeamItemCountInInventory:
		case EEventConditionType.GainedWorth:
		case EEventConditionType.TeamTotalWorth:
		case EEventConditionType.MyItemCountTotal:
			return EEventConditionValueType.Assign;
		case EEventConditionType.UserLevel:
		case EEventConditionType.CampaignCleared:
		case EEventConditionType.MissionCleared:
		case EEventConditionType.LoungeQuestCleared:
		case EEventConditionType.ItemCollected:
		case EEventConditionType.Talent:
		case EEventConditionType.ItemUsed:
		case EEventConditionType.AreaUnlocked:
		case EEventConditionType.AreaDecorationUnlocked:
		case EEventConditionType.NpcLevel:
			return EEventConditionValueType.State;
		default:
			throw new ArgumentOutOfRangeException("conditionType", conditionType, null);
		}
	}

	public static bool IsAssignItemCountCondition(this EEventConditionType type)
	{
		if ((uint)(type - 2002) < 2u)
		{
			return true;
		}
		return type == EEventConditionType.MyItemCountTotal;
	}

	public static bool IsAccumulatedType(this ETalentEffectType type)
	{
		if ((uint)(type - 1) < 6u)
		{
			return true;
		}
		return type == ETalentEffectType.GuestCount;
	}
}
