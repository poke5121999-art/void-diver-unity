using System;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public static class LocalizationUtility
{
	private static ELanguage _language;

	public static void SetLanguage(ELanguage language)
	{
		_language = language;
	}

	public static bool TryGetLocalizedText(string key, out string text)
	{
		if (!TLocalizedText.Table.TryGetValue(key, out var row))
		{
			text = key;
			return false;
		}
		switch (_language)
		{
		case ELanguage.Ko:
			text = row.Ko;
			break;
		case ELanguage.Jp:
			text = row.Jp;
			break;
		case ELanguage.Tw:
			text = row.Tw;
			break;
		case ELanguage.Cn:
			text = row.Cn;
			break;
		case ELanguage.De:
			text = row.De;
			break;
		case ELanguage.Fr:
			text = row.Fr;
			break;
		case ELanguage.Es:
			text = row.Es;
			break;
		case ELanguage.It:
			text = row.It;
			break;
		case ELanguage.Ru:
			text = row.Ru;
			break;
		case ELanguage.Pt:
			text = row.Pt;
			break;
		case ELanguage.Br:
			text = row.Br;
			break;
		case ELanguage.Pl:
			text = row.Pl;
			break;
		case ELanguage.Vi:
			text = row.Vi;
			break;
		case ELanguage.Tr:
			text = row.Tr;
			break;
		default:
			text = row.En;
			break;
		}
		// no null check on the column: Replace on a null column throws NullReferenceException (`je 0x1801f8b22` is the NRE helper, machine code checked @0x1801f8840)
		text = text.Replace("\\r\\n", "\n").Replace("\\n", "\n");
		return true;
	}

	public static string GetLocalizedText(string key)
	{
		if (!TryGetLocalizedText(key, out var text))
		{
			Debug.LogWarning("Localized Text not found. Key: " + key);
		}
		return text;
	}

	public static Type[] GetLocalizableEnums()
	{
		return new Type[]
		{
			typeof(EAnomalyType),
			typeof(EArchiveCategory),
			typeof(EArchiveRecordCategory),
			typeof(EArtifactCategory),
			typeof(EArtifactDealCardEffectType),
			typeof(EBagType),
			typeof(EDifficulty),
			typeof(EEventConditionType),
			typeof(EFrameRate),
			typeof(EGlossaryCategory),
			typeof(EGoodsCategory),
			typeof(EGoodsDataType),
			typeof(EGoodsGradeType),
			typeof(EGoodsType),
			typeof(EInteriorType),
			typeof(EItemCategory),
			typeof(ELanguage),
			typeof(ELoadResult),
			typeof(EMonsterCategory),
			typeof(ENpcFunctionType),
			typeof(EPingResult),
			typeof(EPingType),
			typeof(EQualityLevel),
			typeof(EQuestType),
			typeof(EResolution),
			typeof(ERoomDifficulty),
			typeof(ERoomPingFilterType),
			typeof(ERoomPublishType),
			typeof(ESaveResult),
			typeof(EScreenMode),
			typeof(ESightTag),
			typeof(ESpeedTag),
			typeof(EStatType),
			typeof(EStatusEffectTag),
			typeof(ETalentEffectType),
			typeof(EThemeType),
			typeof(ETycoonFailType),
			typeof(EWeaponType),
		};
	}
}
