using System;
using GameCommon.Enum;
using Steamworks;
using UnityEngine;

public static class EnumExtensions
{
	private static readonly Color _goodsColorNormal;

	private static readonly Color _goodsColorRare;

	private static readonly Color _goodsColorElite;

	private static readonly Color _goodsColorEpic;

	private static readonly Color _goodsColorLegend;

	private static readonly Color _goodsColorUnique;

	public static string ToLocalText(this Enum enumValue)
	{
		return null;
	}

	public static string ToLocalText(this Enum enumValue, string suffix)
	{
		return null;
	}

	public static string ToExpressionText(this Enum enumValue)
	{
		return null;
	}

	public static string ToStatText(this EStatType statType, float value)
	{
		return null;
	}

	public static string GetColorCode(this EElementalType elementalType)
	{
		return null;
	}

	public static EElementalType GetElementalType(this EStatusEffectTag buffEffectType)
	{
		return EElementalType.None;
	}

	public static string GetSpeakerName(this ESpeaker speaker, int tableId, string playerName, int playerCharacterId)
	{
		return null;
	}

	public static EStatusEffectTag GetEffectTag(this ECrowdControlType type)
	{
		return EStatusEffectTag.None;
	}

	public static ECrowdControlType GetCcType(this EStatusEffectTag tag)
	{
		return ECrowdControlType.None;
	}

	public static Color ToColor(this EOutlineType outlineType)
	{
		return default;
	}

	public static float ToStroke(this EOutlineType outlineType)
	{
		return 0f;
	}

	public static bool ToStartActive(this EOutlineType outlineType)
	{
		return false;
	}

	public static Color ToColor(this EGoodsGradeType goodsGradeType)
	{
		return default;
	}

	public static Color ToColor(this EGoodsGradeType grade, float alpha)
	{
		return default;
	}

	public static int ToFrameRate(this EFrameRate frameRate)
	{
		return 0;
	}

	public static int ToWidth(this EResolution resolution)
	{
		return 0;
	}

	public static int ToHeight(this EResolution resolution)
	{
		return 0;
	}

	public static string ToColoredLocalText(this EDifficulty difficulty)
	{
		return null;
	}

	public static Color ToBackgroundColor(this EDifficulty difficulty, float alpha = 1f)
	{
		return default;
	}

	public static bool IsPointPing(this EPingType pintType)
	{
		return false;
	}

	public static bool IsEnemyTargetType(this EHitTarget targetType)
	{
		return false;
	}

	public static int GetRandomArtifactId(this EGoodsGradeType grade)
	{
		return 0;
	}

	public static bool IsPenalty(this EElementalDamagedType type)
	{
		return false;
	}

	public static bool IsResistance(this EElementalDamagedType type)
	{
		return false;
	}

	public static string GetRevealSfx(this EGoodsGradeType type)
	{
		return null;
	}

	public static float GetRevealTime(this EGoodsGradeType type)
	{
		return 0f;
	}

	public static Color GetColor(this ERoomDifficulty difficulty)
	{
		return default;
	}

	public static ERoomDifficulty GetNextDifficulty(this ERoomDifficulty current)
	{
		return ERoomDifficulty.Story;
	}

	public static ERoomDifficulty GetPrevDifficulty(this ERoomDifficulty current)
	{
		return ERoomDifficulty.Story;
	}

	public static ELobbyType ToSteamLobbyType(this ERoomPublishType publishType)
	{
		return ELobbyType.k_ELobbyTypePrivate;
	}

	public static ERoomPublishType ToRoomPublishType(this ELobbyType lobbyType)
	{
		return ERoomPublishType.Private;
	}

	public static ERoomPublishType GetNextPublishType(this ERoomPublishType current)
	{
		return ERoomPublishType.Private;
	}

	public static ERoomPublishType GetPrevPublishType(this ERoomPublishType current)
	{
		return ERoomPublishType.Private;
	}

	public static string GetRoomDifficultyDescription(this ERoomDifficulty difficulty)
	{
		return null;
	}

	public static float GetGoodsRemovalPercent(this ERoomDifficulty difficulty)
	{
		return 0f;
	}

	public static EGoodsSlotCategory GetSlotCategory(this EGoodsType goodsSubType)
	{
		return EGoodsSlotCategory.None;
	}

	public static bool IsInventoryCategory(this EGoodsSlotCategory category)
	{
		return false;
	}

	public static bool IsEquipmentCategory(this EGoodsSlotCategory category)
	{
		return false;
	}

	public static bool IsQuickSlotCategory(this EGoodsSlotCategory category)
	{
		return false;
	}

	public static float GetArtifactDealExpMultiplier(this EGoodsGradeType grade)
	{
		return 0f;
	}

	public static string GetQuestName(this EQuestType questType, int tableId)
	{
		return null;
	}

	public static bool IsBoss(this EMonsterType monsterType)
	{
		return false;
	}

	public static string GetFailTypeName(this ETycoonFailType failType)
	{
		return null;
	}

	public static string GetFailTypeDesc(this ETycoonFailType failType)
	{
		return null;
	}

	public static string ToDescription(this EArtifactDealCardEffectType effectType, long value1)
	{
		return null;
	}

	public static string ToDescription(this EEventConditionType conditionType, long conditionValue1, long conditionValue2)
	{
		return null;
	}

	public static EEventConditionValueType GetValueType(this EEventConditionType conditionType)
	{
		return EEventConditionValueType.None;
	}

	public static bool IsAssignItemCountCondition(this EEventConditionType type)
	{
		return false;
	}

	public static bool IsAccumulatedType(this ETalentEffectType type)
	{
		return false;
	}
}
