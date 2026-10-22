namespace GameCommon.Enum
{
	[LocalizableEnum(new string[] { })]
	public enum EArtifactDealCardEffectType
	{
		None = 0,
		IncreasePricePercent = 1,
		DecreasePricePercent = 2,
		AddPreferTags = 3,
		RerollPreferTags = 4,
		RerollDislikeTags = 5,
		RemoveDislikeTags = 6,
		AddSpecificPreferTag = 7,
		IncreaseBonusSuccessPercent = 8
	}
}
