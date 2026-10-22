using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TConst : ITableData<string>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static ConstTable Table { get; set; }

		public static TConst MaxWeight => null;

		public static TConst ItemCooltime => null;

		public static TConst LevelMax => null;

		public static TConst DefaultGold => null;

		public static TConst StressIntervalTimeMin => null;

		public static TConst StressIntervalTimeMax => null;

		public static TConst StressIntervalDamageMin => null;

		public static TConst StressIntervalDamageMax => null;

		public static TConst AimIgnoreSqrRadius => null;

		public static TConst HitShakeAmplitude => null;

		public static TConst HitShakeFrequency => null;

		public static TConst HitShakeDuration => null;

		public static TConst WhirlStopTime => null;

		public static TConst VignetteHpRatio => null;

		public static TConst LootingInteractionTime => null;

		public static TConst EntranceOpenInteractionTime => null;

		public static TConst EntranceCloseInteractionTime => null;

		public static TConst ExitActivateInteractionTime => null;

		public static TConst ExitActivatingTime => null;

		public static TConst ExitInteractionTime => null;

		public static TConst ExitActivatedTime => null;

		public static TConst SafeExitActivatedTime => null;

		public static TConst SafeExitInteractionTime => null;

		public static TConst PortalInteractionTime => null;

		public static TConst ObstacleInteractionTime => null;

		public static TConst WaveExecutorInteractionTime => null;

		public static TConst DungeonEntranceInteractionTime => null;

		public static TConst AutoToastMessageInterval => null;

		public static TConst InventoryOverloadRatio1 => null;

		public static TConst InventoryOverloadRatio2 => null;

		public static TConst InventoryOverloadToastMessage1 => null;

		public static TConst InventoryOverloadToastMessage2 => null;

		public static TConst InventoryOverloadBuff1 => null;

		public static TConst InventoryOverloadBuff2 => null;

		public static TConst InteractSightRange => null;

		public static TConst InteractSightBackRange => null;

		public static TConst InteractSightAngle => null;

		public static TConst DefaultCriticalChancePercent => null;

		public static TConst DefaultCriticalDamagePercent => null;

		public static TConst DefaultWeaknessDamagePercent => null;

		public static TConst MaxCooldownReductionPercent => null;

		public static TConst MinCoolTime => null;

		public static TConst TargetFollowRange => null;

		public static TConst TargetFollowRangeGiveUpTime => null;

		public static TConst TargetLostInSightTime => null;

		public static TConst TargetSpreadRange => null;

		public static TConst MonsterSpawnRangeMin => null;

		public static TConst MonsterSpawnRangeMax => null;

		public static TConst WanderWaitTimeMin => null;

		public static TConst PatrolWanderWaitTime => null;

		public static TConst WanderMinTime => null;

		public static TConst WanderMaxTime => null;

		public static TConst WanderSpecificMaxTime => null;

		public static TConst WanderWaitTimeMax => null;

		public static TConst StashPageSlotCount => null;

		public static TConst StashSlotCount => null;

		public static TConst InventoryPageSlotCount => null;

		public static TConst SafeInventorySlotBaseCount => null;

		public static TConst CharacterInventorySlotCount => null;

		public static TConst CharacterAccessorySlotCount => null;

		public static TConst CharacterArtifactSlotCount => null;

		public static TConst MonsterReactionDelay => null;

		public static TConst ReviveDebuffId => null;

		public static TConst ReviveHpPercent => null;

		public static TConst ReviveStress => null;

		public static TConst ReviveBrightness => null;

		public static TConst ReviveInteractionTime => null;

		public static TConst MonsterSpawnTime => null;

		public static TConst ParadoxCountMax => null;

		public static TConst ParadoxUniqueLevel => null;

		public static TConst ParadoxUnluckyPercent => null;

		public static TConst StressMax => null;

		public static TConst CorruptionMax => null;

		public static TConst ComboThresholdTime => null;

		public static TConst AbsorbStressInteractionTime => null;

		public static TConst AbsorbStressAmount => null;

		public static TConst AbsorbStressLimit => null;

		public static TConst AbsorbStressCoolTime => null;

		public static TConst StressDamageTriggerHpPercent => null;

		public static TConst StressDamageOnHpDamaged => null;

		public static TConst StressDebuffTriggerAllyRange => null;

		public static TConst StressDamageTriggerAllyDiedRange => null;

		public static TConst StressDamageOnDead => null;

		public static TConst StressDamageOnAllyDied => null;

		public static TConst StressDamageOnBackAttack => null;

		public static TConst StressRecoveryOnCritical => null;

		public static TConst StressRecoveryPercentOnCritical => null;

		public static TConst StressRecoveryCoolTimeOnCritical => null;

		public static TConst StressRecoveryOnAllyRevive => null;

		public static TConst StressRecoveryTriggerAllyReviveRange => null;

		public static TConst LightFuelMax => null;

		public static TConst LightFuelDefault => null;

		public static TConst LightFuelInterval => null;

		public static TConst LightFuelTickDamage => null;

		public static TConst BlindSightRange => null;

		public static TConst TargetHighStressPlayerTriggerStress => null;

		public static TConst AgentStoppingDistance => null;

		public static TConst MoveSpeedThresholdForRun => null;

		public static TConst MinWalkAnimationSpeed => null;

		public static TConst MaxWalkAnimationSpeed => null;

		public static TConst LoungeStageId => null;

		public static TConst WaitTimeOnTargetAdded => null;

		public static TConst ShadowMonsterSpawnStartDelay => null;

		public static TConst ShadowMonsterSpawnDelay => null;

		public static TConst ShadowMonsterSpawnRange => null;

		public static TConst ShadowMonsterLifeTime => null;

		public static TConst ShadowMonsterDamagePercentOnHitByOther => null;

		public static TConst CharacterBackAttackAngle => null;

		public static TConst CharacterBackAttackIncreaseCriticalChancePercent => null;

		public static TConst DefaultCharacterBackAttackDamagePercent => null;

		public static TConst ExtraUnitBackAttackIncreaseCriticalChancePercent => null;

		public static TConst DefaultExtraUnitBackAttackDamagePercent => null;

		public static TConst MonsterBackAttackIncreaseCriticalChancePercent => null;

		public static TConst DefaultMonsterBackAttackDamagePercent => null;

		public static TConst EtcBackAttackDamagePercent => null;

		public static TConst BackAttackMaxRange => null;

		public static TConst MonsterAlertTrackTargetTime => null;

		public static TConst MonsterAlertTrackTargetTimeMultiplier => null;

		public static TConst MonsterAlertLostTargetRecentTime => null;

		public static TConst MonsterAlertLostTargetTime => null;

		public static TConst MonsterAggroStateTime => null;

		public static TConst MonsterAggroSfxCoolTime => null;

		public static TConst MonsterCombatExitDelay => null;

		public static TConst MonsterDirectAggroSightRangeMultiplier => null;

		public static TConst MonsterDirectAggroSightBackRangeMultiplier => null;

		public static TConst MonsterAggroChainMax => null;

		public static TConst TeleportPathTolerance => null;

		public static TConst MonsterLostTargetStateTime => null;

		public static TConst MonsterReceiveAggroSpreadStateTime => null;

		public static TConst MonsterCuriousStateTime => null;

		public static TConst SoulMax => null;

		public static TConst StressDespairVfxInterval => null;

		public static TConst HpBarMaxLimit => null;

		public static TConst HpBarMinLimit => null;

		public static TConst BossHpBarMaxLimit => null;

		public static TConst BossHpBarMinLimit => null;

		public static TConst ObstacleEdgeCheckMargin => null;

		public static TConst StageEnterDelay => null;

		public static TConst GoodsRemovalPercentStory => null;

		public static TConst GoodsRemovalPercentJunior => null;

		public static TConst GoodsRemovalPercentClassic => null;

		public static TConst GoodsRemovalPercentVeteran => null;

		public static TConst RevolverScrollerThreshold => null;

		public static TConst ReviveStressCost => null;

		public static TConst CutsceneStateBuffId => null;

		public static TConst MissionCount => null;

		public static TConst ArtifactMinimumPricePercent => null;

		public static TConst ArtifactPrefixMinNormal => null;

		public static TConst ArtifactPrefixMinRare => null;

		public static TConst ArtifactPrefixMinElite => null;

		public static TConst ArtifactPrefixMinEpic => null;

		public static TConst ArtifactPrefixMinLegend => null;

		public static TConst ArtifactPrefixMinUnique => null;

		public static TConst ArtifactPrefixMaxNormal => null;

		public static TConst ArtifactPrefixMaxRare => null;

		public static TConst ArtifactPrefixMaxElite => null;

		public static TConst ArtifactPrefixMaxEpic => null;

		public static TConst ArtifactPrefixMaxLegend => null;

		public static TConst ArtifactPrefixMaxUnique => null;

		public static TConst CustomerMaxCount => null;

		public static TConst CustomerSpawnPercentBase => null;

		public static TConst CustomerSpawnPercentUserCount => null;

		public static TConst StaminaRegenDelay => null;

		public static TConst StaminaGaugeHideDelay => null;

		public static TConst CharacterSpawnSafeRange => null;

		public static TConst WeakPointGaugeCooldownDelay => null;

		public static TConst WeakPointGaugeCooldownPercentPerSencod => null;

		public static TConst WeakPointFireFullStackDamagePercent => null;

		public static TConst WeakPointWaterFullStackDamagePercent => null;

		public static TConst WeakPointWindFullStackDamagePercent => null;

		public static TConst WeakPointGaugeBlockTime => null;

		public static TConst ArtifactPrefixNegativePercent => null;

		public static TConst MonsterRetargetToPlayerChance => null;

		public static TConst ExpFromRewardBox => null;

		public static TConst ExpFromNormalMonster => null;

		public static TConst ExpFromBossMonster => null;

		public static TConst ExpBonusFromNormalMonsterKill => null;

		public static TConst ExpBonusFromBossMonsterKill => null;

		public static TConst ExpFromEscape => null;

		public static TConst CharacterTalentTier1UserLevel => null;

		public static TConst CharacterTalentTier2UserLevel => null;

		public static TConst CharacterTalentTier3UserLevel => null;

		public static TConst CharacterTalentTier4UserLevel => null;

		public static TConst CharacterTalentTier5UserLevel => null;

		public static TConst CharacterTalentTier6UserLevel => null;

		public static TConst CharacterTalentResetCost => null;

		public static TConst BuffEffectAmplifierMin => null;

		public static TConst BuffEffectAmplifierMax => null;

		public static TConst MadnessAllyDamageFactor => null;

		public static TConst ToggleRunExpireDelay => null;

		public static TConst SelfReviveMultiplayFactor => null;

		public static TConst SelfReviveStressFactor => null;

		public static TConst SelfReviveStressExponent => null;

		public static TConst SelfReviveProbabilityReductionRate => null;

		public static TConst RoomPlayerCountMax => null;

		public static TConst TalentTierStackCostMultiplier => null;

		public static TConst LostGoodsRetentionExitCount => null;

		public static TConst LostGoodsPriceMultiplierMin => null;

		public static TConst LostGoodsPriceMultiplierMax => null;

		public static TConst LostArtifactPriceMultiplierMin => null;

		public static TConst LostArtifactPriceMultiplierMax => null;

		public static TConst LostGoodsStorageDefaultSlotCount => null;

		public static TConst LostGoodsGradeNormalWeightMultiplier => null;

		public static TConst LostGoodsGradeRareWeightMultiplier => null;

		public static TConst LostGoodsGradeEliteWeightMultiplier => null;

		public static TConst LostGoodsGradeEpicWeightMultiplier => null;

		public static TConst LostGoodsGradeLegendWeightMultiplier => null;

		public static TConst LostGoodsGradeUniqueWeightMultiplier => null;

		public static TConst LostGoodsArtifactWeightMultiplier => null;

		public static TConst LostGoodsEquipmentWeightMultiplier => null;

		public static TConst LostGoodsConsumableWeightMultiplier => null;

		public static TConst LostGoodsBagWeightMultiplier => null;

		public static TConst CampaignMaxCount => null;

		public static TConst RecommendedLevelCampaignCount => null;

		public static TConst CampaignLevelDifferenceWeight => null;

		public static TConst CampaignLevelMaxOffset => null;

		public static TConst RestCost => null;

		public static TConst RestCostMultiplier => null;

		public static TConst BackwardMoveSpeedMultiplier => null;

		public static TConst ArtifactShopCostMultiplier => null;

		public static TConst ArtifactDealNormalExpMultiplier => null;

		public static TConst ArtifactDealRareExpMultiplier => null;

		public static TConst ArtifactDealEliteExpMultiplier => null;

		public static TConst ArtifactDealEpicExpMultiplier => null;

		public static TConst ArtifactDealLegendExpMultiplier => null;

		public static TConst ArtifactDealUniqueExpMultiplier => null;

		public static TConst PartyPlayExpMultiplyPercent => null;

		public static TConst MiniMapPingDuration => null;

		public static TConst DefenseFactor => null;

		public static TConst AimInteractPriorityRange => null;

		public static TConst EquipmentBreakThreshold => null;

		public static TConst EquipmentBreakStatRate => null;

		public static TConst DurabilityDecreaseFactorByDamageRate => null;

		public static TConst DurabilityDecreaseValueOnHit => null;

		public static TConst RandomDurabilityMinimumRate => null;

		public static TConst RepairMaxDurabilityLossRate => null;

		public static TConst ArtifactDealMinSuccessPercent => null;

		public static TConst ArtifactDealMaxSuccessPercent => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnTwo => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnThree => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnFour => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnFive => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnSix => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnSeven => null;

		public static TConst ArtifactDealMatchBonusMultiplierOnAndAboveEight => null;

		public static TConst ArtifactDealPlayCardCount => null;

		public static TConst ArtifactDealRerollCount => null;

		public static TConst ArtifactDealPreferCategoryCount => null;

		public static TConst ArtifactDealOwnedCategoryWeightPercent => null;

		public static TConst ArtifactDealHandsCount => null;

		public static TConst ArtifactDealRerollDrawCards => null;

		public static TConst ArtifactDealMaxPreferTagCount => null;

		public static TConst ArtifactDealPreferMatchSlot1Percent => null;

		public static TConst ArtifactDealPreferMatchSlot2Percent => null;

		public static TConst ArtifactDealPreferMatchSlot3Percent => null;

		public static TConst ArtifactDealDislikeMatchSlot1Percent => null;

		public static TConst ArtifactDealDislikeMatchSlot2Percent => null;

		public static TConst ArtifactDealDislikeMatchSlot3Percent => null;

		public static TConst ArtifactDealAddPreferMatchPercent => null;

		public static TConst ArtifactDealRerollPreferAddMatchPercent => null;

		public static TConst ArtifactDealRerollDislikeRemoveMatchPercent => null;

		public static TConst ArtifactDealCardRelevanceWeightPercent => null;

		public static TConst ArtifactDealRerollPreferMatchedWeightPercent => null;

		public static TConst ArtifactDealRemoveDislikeMatchedWeightPercent => null;

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier1 => null;

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier2 => null;

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier3 => null;

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier4 => null;

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier5 => null;

		public static TConst ArtifactDealSuccessTierHigh => null;

		public static TConst ArtifactDealSuccessTierMid => null;

		public static TConst SuppressionResistanceGrowthFactor => null;

		public static TConst GroggyDamageMultiplier => null;

		public static TConst SuppressionGaugeFactorEasy => null;

		public static TConst SuppressionGaugeFactorNormal => null;

		public static TConst SuppressionGaugeFactorHard => null;

		public static TConst SuppressionGaugeFactorInsane => null;

		public static TConst SuppressionImmunityDuration => null;

		public static TConst GroggyDuration => null;

		public static TConst TycoonWorkProgressPerTurn => null;

		public static TConst TycoonMaxWorkProgressFactor => null;

		public static TConst TycoonMaxWorkProgressMinRatio => null;

		public static TConst TycoonMaxWorkProgressMaxRatio => null;

		public static TConst TycoonDefaultMaxWorkProgressNormal => null;

		public static TConst TycoonDefaultMaxWorkProgressRare => null;

		public static TConst TycoonDefaultMaxWorkProgressElite => null;

		public static TConst TycoonDefaultMaxWorkProgressEpic => null;

		public static TConst TycoonDefaultMaxWorkProgressLegendary => null;

		public static TConst TycoonDefaultMaxWorkProgressUnique => null;

		public static TConst TycoonEmployeeStressRecoveryPerTurn => null;

		public static TConst TycoonEmployeeSwapPenaltyRatio => null;

		public static TConst TycoonEmployeePurifyCostBase => null;

		public static TConst TycoonEmployeePurifyCostFactor => null;

		public static TConst TycoonArtifactDamageGoldRatio => null;

		public static TConst TycoonCheapSaleGoldRatio => null;

		public static TConst TycoonWorkProgressCharmFactor => null;

		public static TConst TycoonWorkProgressPersuasionFactor => null;

		public static TConst TycoonSalePricePersuasionFactor => null;

		public static TConst TycoonEmployeeExpGainNormal => null;

		public static TConst TycoonEmployeeExpGainRare => null;

		public static TConst TycoonEmployeeExpGainElite => null;

		public static TConst TycoonEmployeeExpGainEpic => null;

		public static TConst TycoonEmployeeExpGainLegendary => null;

		public static TConst TycoonEmployeeExpGainUnique => null;

		public static TConst TycoonEmployeeSkillLearnPercentPerLevel => null;

		public static TConst TycoonEmployeeSkillLearnPercent => null;

		public static TConst TycoonHireCostPerLevelFactor => null;

		public static TConst WeakPointShakeAmplitude => null;

		public static TConst WeakPointShakeFrequency => null;

		public static TConst WeakPointShakeDuration => null;

		public static TConst ExitSpecialFieldRange => null;

		public static TConst AnomalyDeepOneSpawnChancePercent => null;

		public static TConst AnomalyMonsterNoneCombatSightMultiplier => null;

		public static TConst AnomalyTrapSpawnMultiplier => null;

		public static TConst AnomalyMinimapNoiseRatio => null;

		public static TConst AnomalyMinimapNoiseChangeInterval => null;

		public static TConst AnomalyArtifactGradeUpStep => null;

		public static TConst AnomalyArtifactGradeUpChancePercent => null;

		public static TConst CoinToGoldRate => null;

		public static TConst MaxEmployeeCountBase => null;

		public static TConst ArtifactPriceMinPercent => null;

		public static TConst ArtifactPriceCampaignLevelGrowthRate => null;

		public static TConst CharacterWeaponTypeDropWeightBonusPercent => null;

		public static TConst FireDamageFreezeDurationReductionPercent => null;

		public static TConst FireDamageFreezeDurationReductionCooldown => null;

		public static TConst FireDamageFreezeDurationReductionSfx => null;

		public static TConst FreezeEndSfx => null;

		public static TConst HitboxAimCorrectionAngle => null;

		public static TConst LostItemRerollCost => null;

		public static TConst LostItemRerollCostMultiplier => null;

		public static TConst CapturedMonsterScale => null;

		public static TConst DespawnDelayOnCaptured => null;

		public static TConst PadNavFirstDelay => null;

		public static TConst PadNavRepeatRate => null;

		public static TConst VoteTimeLimit => null;

		public static TConst LowWeaponDurabilityValue => null;

		public string Pk
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public string Id { get; set; }

		public int IntValue { get; set; }

		public float FloatValue { get; set; }

		public string StringValue { get; set; }

		public Action<ITableData<string>> OnDataLoaded { get; set; }

		public static event Action OnTableChanged
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public void Parse(string[] columns)
		{
		}

		public string ToCsv()
		{
			return null;
		}

		public static void SaveCsv(string filePath)
		{
		}

		public static string GetColumnsCsv()
		{
			return null;
		}

		public static string GetTypesCsv()
		{
			return null;
		}

		public static void LoadFromPath(string filePath)
		{
		}

		public static void LoadFromText(string text)
		{
		}

		public static void InvokeTableChangeEvent()
		{
		}

		public static bool ValidateTable()
		{
			return false;
		}

		public bool Validate()
		{
			return false;
		}
	}
}
