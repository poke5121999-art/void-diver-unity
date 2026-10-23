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

		public static TConst MaxWeight => Table["MaxWeight"];

		public static TConst ItemCooltime => Table["ItemCooltime"];

		public static TConst LevelMax => Table["LevelMax"];

		public static TConst DefaultGold => Table["DefaultGold"];

		public static TConst StressIntervalTimeMin => Table["StressIntervalTimeMin"];

		public static TConst StressIntervalTimeMax => Table["StressIntervalTimeMax"];

		public static TConst StressIntervalDamageMin => Table["StressIntervalDamageMin"];

		public static TConst StressIntervalDamageMax => Table["StressIntervalDamageMax"];

		public static TConst AimIgnoreSqrRadius => Table["AimIgnoreSqrRadius"];

		public static TConst HitShakeAmplitude => Table["HitShakeAmplitude"];

		public static TConst HitShakeFrequency => Table["HitShakeFrequency"];

		public static TConst HitShakeDuration => Table["HitShakeDuration"];

		public static TConst WhirlStopTime => Table["WhirlStopTime"];

		public static TConst VignetteHpRatio => Table["VignetteHPRatio"];

		public static TConst LootingInteractionTime => Table["LootingInteractionTime"];

		public static TConst EntranceOpenInteractionTime => Table["EntranceOpenInteractionTime"];

		public static TConst EntranceCloseInteractionTime => Table["EntranceCloseInteractionTime"];

		public static TConst ExitActivateInteractionTime => Table["ExitActivateInteractionTime"];

		public static TConst ExitActivatingTime => Table["ExitActivatingTime"];

		public static TConst ExitInteractionTime => Table["ExitInteractionTime"];

		public static TConst ExitActivatedTime => Table["ExitActivatedTime"];

		public static TConst SafeExitActivatedTime => Table["SafeExitActivatedTime"];

		public static TConst SafeExitInteractionTime => Table["SafeExitInteractionTime"];

		public static TConst PortalInteractionTime => Table["PortalInteractionTime"];

		public static TConst ObstacleInteractionTime => Table["ObstacleInteractionTime"];

		public static TConst WaveExecutorInteractionTime => Table["WaveExecutorInteractionTime"];

		public static TConst DungeonEntranceInteractionTime => Table["DungeonEntranceInteractionTime"];

		public static TConst AutoToastMessageInterval => Table["AutoToastMessageInterval"];

		public static TConst InventoryOverloadRatio1 => Table["InventoryOverloadRatio1"];

		public static TConst InventoryOverloadRatio2 => Table["InventoryOverloadRatio2"];

		public static TConst InventoryOverloadToastMessage1 => Table["InventoryOverloadToastMessage1"];

		public static TConst InventoryOverloadToastMessage2 => Table["InventoryOverloadToastMessage2"];

		public static TConst InventoryOverloadBuff1 => Table["InventoryOverloadBuff1"];

		public static TConst InventoryOverloadBuff2 => Table["InventoryOverloadBuff2"];

		public static TConst InteractSightRange => Table["InteractSightRange"];

		public static TConst InteractSightBackRange => Table["InteractSightBackRange"];

		public static TConst InteractSightAngle => Table["InteractSightAngle"];

		public static TConst DefaultCriticalChancePercent => Table["DefaultCriticalChancePercent"];

		public static TConst DefaultCriticalDamagePercent => Table["DefaultCriticalDamagePercent"];

		public static TConst DefaultWeaknessDamagePercent => Table["DefaultWeaknessDamagePercent"];

		public static TConst MaxCooldownReductionPercent => Table["MaxCooldownReductionPercent"];

		public static TConst MinCoolTime => Table["MinCoolTime"];

		public static TConst TargetFollowRange => Table["TargetFollowRange"];

		public static TConst TargetFollowRangeGiveUpTime => Table["TargetFollowRangeGiveUpTime"];

		public static TConst TargetLostInSightTime => Table["TargetLostInSightTime"];

		public static TConst TargetSpreadRange => Table["TargetSpreadRange"];

		public static TConst MonsterSpawnRangeMin => Table["MonsterSpawnRangeMin"];

		public static TConst MonsterSpawnRangeMax => Table["MonsterSpawnRangeMax"];

		public static TConst WanderWaitTimeMin => Table["WanderWaitTimeMin"];

		public static TConst PatrolWanderWaitTime => Table["PatrolWanderWaitTime"];

		public static TConst WanderMinTime => Table["WanderMinTime"];

		public static TConst WanderMaxTime => Table["WanderMaxTime"];

		public static TConst WanderSpecificMaxTime => Table["WanderSpecificMaxTime"];

		public static TConst WanderWaitTimeMax => Table["WanderWaitTimeMax"];

		public static TConst StashPageSlotCount => Table["StashPageSlotCount"];

		public static TConst StashSlotCount => Table["StashSlotCount"];

		public static TConst InventoryPageSlotCount => Table["InventoryPageSlotCount"];

		public static TConst SafeInventorySlotBaseCount => Table["SafeInventorySlotBaseCount"];

		public static TConst CharacterInventorySlotCount => Table["CharacterInventorySlotCount"];

		public static TConst CharacterAccessorySlotCount => Table["CharacterAccessorySlotCount"];

		public static TConst CharacterArtifactSlotCount => Table["CharacterArtifactSlotCount"];

		public static TConst MonsterReactionDelay => Table["MonsterReactionDelay"];

		public static TConst ReviveDebuffId => Table["ReviveDebuffId"];

		public static TConst ReviveHpPercent => Table["ReviveHpPercent"];

		public static TConst ReviveStress => Table["ReviveStress"];

		public static TConst ReviveBrightness => Table["ReviveBrightness"];

		public static TConst ReviveInteractionTime => Table["ReviveInteractionTime"];

		public static TConst MonsterSpawnTime => Table["MonsterSpawnTime"];

		public static TConst ParadoxCountMax => Table["ParadoxCountMax"];

		public static TConst ParadoxUniqueLevel => Table["ParadoxUniqueLevel"];

		public static TConst ParadoxUnluckyPercent => Table["ParadoxUnluckyPercent"];

		public static TConst StressMax => Table["StressMax"];

		public static TConst CorruptionMax => Table["CorruptionMax"];

		public static TConst ComboThresholdTime => Table["ComboThresholdTime"];

		public static TConst AbsorbStressInteractionTime => Table["AbsorbStressInteractionTime"];

		public static TConst AbsorbStressAmount => Table["AbsorbStressAmount"];

		public static TConst AbsorbStressLimit => Table["AbsorbStressLimit"];

		public static TConst AbsorbStressCoolTime => Table["AbsorbStressCoolTime"];

		public static TConst StressDamageTriggerHpPercent => Table["StressDamageTriggerHpPercent"];

		public static TConst StressDamageOnHpDamaged => Table["StressDamageOnHpDamaged"];

		public static TConst StressDebuffTriggerAllyRange => Table["StressDebuffTriggerAllyRange"];

		public static TConst StressDamageTriggerAllyDiedRange => Table["StressDamageTriggerAllyDiedRange"];

		public static TConst StressDamageOnDead => Table["StressDamageOnDead"];

		public static TConst StressDamageOnAllyDied => Table["StressDamageOnAllyDied"];

		public static TConst StressDamageOnBackAttack => Table["StressDamageOnBackAttack"];

		public static TConst StressRecoveryOnCritical => Table["StressRecoveryOnCritical"];

		public static TConst StressRecoveryPercentOnCritical => Table["StressRecoveryPercentOnCritical"];

		public static TConst StressRecoveryCoolTimeOnCritical => Table["StressRecoveryCoolTimeOnCritical"];

		public static TConst StressRecoveryOnAllyRevive => Table["StressRecoveryOnAllyRevive"];

		public static TConst StressRecoveryTriggerAllyReviveRange => Table["StressRecoveryTriggerAllyReviveRange"];

		public static TConst LightFuelMax => Table["LightFuelMax"];

		public static TConst LightFuelDefault => Table["LightFuelDefault"];

		public static TConst LightFuelInterval => Table["LightFuelInterval"];

		public static TConst LightFuelTickDamage => Table["LightFuelTickDamage"];

		public static TConst BlindSightRange => Table["BlindSightRange"];

		public static TConst TargetHighStressPlayerTriggerStress => Table["TargetHighStressPlayerTriggerStress"];

		public static TConst AgentStoppingDistance => Table["AgentStoppingDistance"];

		public static TConst MoveSpeedThresholdForRun => Table["MoveSpeedThresholdForRun"];

		public static TConst MinWalkAnimationSpeed => Table["MinWalkAnimationSpeed"];

		public static TConst MaxWalkAnimationSpeed => Table["MaxWalkAnimationSpeed"];

		public static TConst LoungeStageId => Table["LoungeStageId"];

		public static TConst WaitTimeOnTargetAdded => Table["WaitTimeOnTargetAdded"];

		public static TConst ShadowMonsterSpawnStartDelay => Table["ShadowMonsterSpawnStartDelay"];

		public static TConst ShadowMonsterSpawnDelay => Table["ShadowMonsterSpawnDelay"];

		public static TConst ShadowMonsterSpawnRange => Table["ShadowMonsterSpawnRange"];

		public static TConst ShadowMonsterLifeTime => Table["ShadowMonsterLifeTime"];

		public static TConst ShadowMonsterDamagePercentOnHitByOther => Table["ShadowMonsterDamagePercentOnHitByOther"];

		public static TConst CharacterBackAttackAngle => Table["CharacterBackAttackAngle"];

		public static TConst CharacterBackAttackIncreaseCriticalChancePercent => Table["CharacterBackAttackIncreaseCriticalChancePercent"];

		public static TConst DefaultCharacterBackAttackDamagePercent => Table["DefaultCharacterBackAttackDamagePercent"];

		public static TConst ExtraUnitBackAttackIncreaseCriticalChancePercent => Table["ExtraUnitBackAttackIncreaseCriticalChancePercent"];

		public static TConst DefaultExtraUnitBackAttackDamagePercent => Table["DefaultExtraUnitBackAttackDamagePercent"];

		public static TConst MonsterBackAttackIncreaseCriticalChancePercent => Table["MonsterBackAttackIncreaseCriticalChancePercent"];

		public static TConst DefaultMonsterBackAttackDamagePercent => Table["DefaultMonsterBackAttackDamagePercent"];

		public static TConst EtcBackAttackDamagePercent => Table["EtcBackAttackDamagePercent"];

		public static TConst BackAttackMaxRange => Table["BackAttackMaxRange"];

		public static TConst MonsterAlertTrackTargetTime => Table["MonsterAlertTrackTargetTime"];

		public static TConst MonsterAlertTrackTargetTimeMultiplier => Table["MonsterAlertTrackTargetTimeMultiplier"];

		public static TConst MonsterAlertLostTargetRecentTime => Table["MonsterAlertLostTargetRecentTime"];

		public static TConst MonsterAlertLostTargetTime => Table["MonsterAlertLostTargetTime"];

		public static TConst MonsterAggroStateTime => Table["MonsterAggroStateTime"];

		public static TConst MonsterAggroSfxCoolTime => Table["MonsterAggroSfxCoolTime"];

		public static TConst MonsterCombatExitDelay => Table["MonsterCombatExitDelay"];

		public static TConst MonsterDirectAggroSightRangeMultiplier => Table["MonsterDirectAggroSightRangeMultiplier"];

		public static TConst MonsterDirectAggroSightBackRangeMultiplier => Table["MonsterDirectAggroSightBackRangeMultiplier"];

		public static TConst MonsterAggroChainMax => Table["MonsterAggroChainMax"];

		public static TConst TeleportPathTolerance => Table["TeleportPathTolerance"];

		public static TConst MonsterLostTargetStateTime => Table["MonsterLostTargetStateTime"];

		public static TConst MonsterReceiveAggroSpreadStateTime => Table["MonsterReceiveAggroSpreadStateTime"];

		public static TConst MonsterCuriousStateTime => Table["MonsterCuriousStateTime"];

		public static TConst SoulMax => Table["SoulMax"];

		public static TConst StressDespairVfxInterval => Table["StressDespairVfxInterval"];

		public static TConst HpBarMaxLimit => Table["HpBarMaxLimit"];

		public static TConst HpBarMinLimit => Table["HpBarMinLimit"];

		public static TConst BossHpBarMaxLimit => Table["BossHpBarMaxLimit"];

		public static TConst BossHpBarMinLimit => Table["BossHpBarMinLimit"];

		public static TConst ObstacleEdgeCheckMargin => Table["ObstacleEdgeCheckMargin"];

		public static TConst StageEnterDelay => Table["StageEnterDelay"];

		public static TConst GoodsRemovalPercentStory => Table["GoodsRemovalPercentStory"];

		public static TConst GoodsRemovalPercentJunior => Table["GoodsRemovalPercentJunior"];

		public static TConst GoodsRemovalPercentClassic => Table["GoodsRemovalPercentClassic"];

		public static TConst GoodsRemovalPercentVeteran => Table["GoodsRemovalPercentVeteran"];

		public static TConst RevolverScrollerThreshold => Table["RevolverScrollerThreshold"];

		public static TConst ReviveStressCost => Table["ReviveStressCost"];

		public static TConst CutsceneStateBuffId => Table["CutsceneStateBuffId"];

		public static TConst MissionCount => Table["MissionCount"];

		public static TConst ArtifactMinimumPricePercent => Table["ArtifactMinimumPricePercent"];

		public static TConst ArtifactPrefixMinNormal => Table["ArtifactPrefixMinNormal"];

		public static TConst ArtifactPrefixMinRare => Table["ArtifactPrefixMinRare"];

		public static TConst ArtifactPrefixMinElite => Table["ArtifactPrefixMinElite"];

		public static TConst ArtifactPrefixMinEpic => Table["ArtifactPrefixMinEpic"];

		public static TConst ArtifactPrefixMinLegend => Table["ArtifactPrefixMinLegend"];

		public static TConst ArtifactPrefixMinUnique => Table["ArtifactPrefixMinUnique"];

		public static TConst ArtifactPrefixMaxNormal => Table["ArtifactPrefixMaxNormal"];

		public static TConst ArtifactPrefixMaxRare => Table["ArtifactPrefixMaxRare"];

		public static TConst ArtifactPrefixMaxElite => Table["ArtifactPrefixMaxElite"];

		public static TConst ArtifactPrefixMaxEpic => Table["ArtifactPrefixMaxEpic"];

		public static TConst ArtifactPrefixMaxLegend => Table["ArtifactPrefixMaxLegend"];

		public static TConst ArtifactPrefixMaxUnique => Table["ArtifactPrefixMaxUnique"];

		public static TConst CustomerMaxCount => Table["CustomerMaxCount"];

		public static TConst CustomerSpawnPercentBase => Table["CustomerSpawnPercentBase"];

		public static TConst CustomerSpawnPercentUserCount => Table["CustomerSpawnPercentUserCount"];

		public static TConst StaminaRegenDelay => Table["StaminaRegenDelay"];

		public static TConst StaminaGaugeHideDelay => Table["StaminaGaugeHideDelay"];

		public static TConst CharacterSpawnSafeRange => Table["CharacterSpawnSafeRange"];

		public static TConst WeakPointGaugeCooldownDelay => Table["WeakPointGaugeCooldownDelay"];

		public static TConst WeakPointGaugeCooldownPercentPerSencod => Table["WeakPointGaugeCooldownPercentPerSencod"];

		public static TConst WeakPointFireFullStackDamagePercent => Table["WeakPointFireFullStackDamagePercent"];

		public static TConst WeakPointWaterFullStackDamagePercent => Table["WeakPointWaterFullStackDamagePercent"];

		public static TConst WeakPointWindFullStackDamagePercent => Table["WeakPointWindFullStackDamagePercent"];

		public static TConst WeakPointGaugeBlockTime => Table["WeakPointGaugeBlockTime"];

		public static TConst ArtifactPrefixNegativePercent => Table["ArtifactPrefixNegativePercent"];

		public static TConst MonsterRetargetToPlayerChance => Table["MonsterRetargetToPlayerChance"];

		public static TConst ExpFromRewardBox => Table["ExpFromRewardBox"];

		public static TConst ExpFromNormalMonster => Table["ExpFromNormalMonster"];

		public static TConst ExpFromBossMonster => Table["ExpFromBossMonster"];

		public static TConst ExpBonusFromNormalMonsterKill => Table["ExpBonusFromNormalMonsterKill"];

		public static TConst ExpBonusFromBossMonsterKill => Table["ExpBonusFromBossMonsterKill"];

		public static TConst ExpFromEscape => Table["ExpFromEscape"];

		public static TConst CharacterTalentTier1UserLevel => Table["CharacterTalentTier1UserLevel"];

		public static TConst CharacterTalentTier2UserLevel => Table["CharacterTalentTier2UserLevel"];

		public static TConst CharacterTalentTier3UserLevel => Table["CharacterTalentTier3UserLevel"];

		public static TConst CharacterTalentTier4UserLevel => Table["CharacterTalentTier4UserLevel"];

		public static TConst CharacterTalentTier5UserLevel => Table["CharacterTalentTier5UserLevel"];

		public static TConst CharacterTalentTier6UserLevel => Table["CharacterTalentTier6UserLevel"];

		public static TConst CharacterTalentResetCost => Table["CharacterTalentResetCost"];

		public static TConst BuffEffectAmplifierMin => Table["BuffEffectAmplifierMin"];

		public static TConst BuffEffectAmplifierMax => Table["BuffEffectAmplifierMax"];

		public static TConst MadnessAllyDamageFactor => Table["MadnessAllyDamageFactor"];

		public static TConst ToggleRunExpireDelay => Table["ToggleRunExpireDelay"];

		public static TConst SelfReviveMultiplayFactor => Table["SelfReviveMultiplayFactor"];

		public static TConst SelfReviveStressFactor => Table["SelfReviveStressFactor"];

		public static TConst SelfReviveStressExponent => Table["SelfReviveStressExponent"];

		public static TConst SelfReviveProbabilityReductionRate => Table["SelfReviveProbabilityReductionRate"];

		public static TConst RoomPlayerCountMax => Table["RoomPlayerCountMax"];

		public static TConst TalentTierStackCostMultiplier => Table["TalentTierStackCostMultiplier"];

		public static TConst LostGoodsRetentionExitCount => Table["LostGoodsRetentionExitCount"];

		public static TConst LostGoodsPriceMultiplierMin => Table["LostGoodsPriceMultiplierMin"];

		public static TConst LostGoodsPriceMultiplierMax => Table["LostGoodsPriceMultiplierMax"];

		public static TConst LostArtifactPriceMultiplierMin => Table["LostArtifactPriceMultiplierMin"];

		public static TConst LostArtifactPriceMultiplierMax => Table["LostArtifactPriceMultiplierMax"];

		public static TConst LostGoodsStorageDefaultSlotCount => Table["LostGoodsStorageDefaultSlotCount"];

		public static TConst LostGoodsGradeNormalWeightMultiplier => Table["LostGoodsGradeNormalWeightMultiplier"];

		public static TConst LostGoodsGradeRareWeightMultiplier => Table["LostGoodsGradeRareWeightMultiplier"];

		public static TConst LostGoodsGradeEliteWeightMultiplier => Table["LostGoodsGradeEliteWeightMultiplier"];

		public static TConst LostGoodsGradeEpicWeightMultiplier => Table["LostGoodsGradeEpicWeightMultiplier"];

		public static TConst LostGoodsGradeLegendWeightMultiplier => Table["LostGoodsGradeLegendWeightMultiplier"];

		public static TConst LostGoodsGradeUniqueWeightMultiplier => Table["LostGoodsGradeUniqueWeightMultiplier"];

		public static TConst LostGoodsArtifactWeightMultiplier => Table["LostGoodsArtifactWeightMultiplier"];

		public static TConst LostGoodsEquipmentWeightMultiplier => Table["LostGoodsEquipmentWeightMultiplier"];

		public static TConst LostGoodsConsumableWeightMultiplier => Table["LostGoodsConsumableWeightMultiplier"];

		public static TConst LostGoodsBagWeightMultiplier => Table["LostGoodsBagWeightMultiplier"];

		public static TConst CampaignMaxCount => Table["CampaignMaxCount"];

		public static TConst RecommendedLevelCampaignCount => Table["RecommendedLevelCampaignCount"];

		public static TConst CampaignLevelDifferenceWeight => Table["CampaignLevelDifferenceWeight"];

		public static TConst CampaignLevelMaxOffset => Table["CampaignLevelMaxOffset"];

		public static TConst RestCost => Table["RestCost"];

		public static TConst RestCostMultiplier => Table["RestCostMultiplier"];

		public static TConst BackwardMoveSpeedMultiplier => Table["BackwardMoveSpeedMultiplier"];

		public static TConst ArtifactShopCostMultiplier => Table["ArtifactShopCostMultiplier"];

		public static TConst ArtifactDealNormalExpMultiplier => Table["ArtifactDealNormalExpMultiplier"];

		public static TConst ArtifactDealRareExpMultiplier => Table["ArtifactDealRareExpMultiplier"];

		public static TConst ArtifactDealEliteExpMultiplier => Table["ArtifactDealEliteExpMultiplier"];

		public static TConst ArtifactDealEpicExpMultiplier => Table["ArtifactDealEpicExpMultiplier"];

		public static TConst ArtifactDealLegendExpMultiplier => Table["ArtifactDealLegendExpMultiplier"];

		public static TConst ArtifactDealUniqueExpMultiplier => Table["ArtifactDealUniqueExpMultiplier"];

		public static TConst PartyPlayExpMultiplyPercent => Table["PartyPlayExpMultiplyPercent"];

		public static TConst MiniMapPingDuration => Table["MiniMapPingDuration"];

		public static TConst DefenseFactor => Table["DefenseFactor"];

		public static TConst AimInteractPriorityRange => Table["AimInteractPriorityRange"];

		public static TConst EquipmentBreakThreshold => Table["EquipmentBreakThreshold"];

		public static TConst EquipmentBreakStatRate => Table["EquipmentBreakStatRate"];

		public static TConst DurabilityDecreaseFactorByDamageRate => Table["DurabilityDecreaseFactorByDamageRate"];

		public static TConst DurabilityDecreaseValueOnHit => Table["DurabilityDecreaseValueOnHit"];

		public static TConst RandomDurabilityMinimumRate => Table["RandomDurabilityMinimumRate"];

		public static TConst RepairMaxDurabilityLossRate => Table["RepairMaxDurabilityLossRate"];

		public static TConst ArtifactDealMinSuccessPercent => Table["ArtifactDealMinSuccessPercent"];

		public static TConst ArtifactDealMaxSuccessPercent => Table["ArtifactDealMaxSuccessPercent"];

		public static TConst ArtifactDealMatchBonusMultiplierOnTwo => Table["ArtifactDealMatchBonusMultiplierOnTwo"];

		public static TConst ArtifactDealMatchBonusMultiplierOnThree => Table["ArtifactDealMatchBonusMultiplierOnThree"];

		public static TConst ArtifactDealMatchBonusMultiplierOnFour => Table["ArtifactDealMatchBonusMultiplierOnFour"];

		public static TConst ArtifactDealMatchBonusMultiplierOnFive => Table["ArtifactDealMatchBonusMultiplierOnFive"];

		public static TConst ArtifactDealMatchBonusMultiplierOnSix => Table["ArtifactDealMatchBonusMultiplierOnSix"];

		public static TConst ArtifactDealMatchBonusMultiplierOnSeven => Table["ArtifactDealMatchBonusMultiplierOnSeven"];

		public static TConst ArtifactDealMatchBonusMultiplierOnAndAboveEight => Table["ArtifactDealMatchBonusMultiplierOnAndAboveEight"];

		public static TConst ArtifactDealPlayCardCount => Table["ArtifactDealPlayCardCount"];

		public static TConst ArtifactDealRerollCount => Table["ArtifactDealRerollCount"];

		public static TConst ArtifactDealPreferCategoryCount => Table["ArtifactDealPreferCategoryCount"];

		public static TConst ArtifactDealOwnedCategoryWeightPercent => Table["ArtifactDealOwnedCategoryWeightPercent"];

		public static TConst ArtifactDealHandsCount => Table["ArtifactDealHandsCount"];

		public static TConst ArtifactDealRerollDrawCards => Table["ArtifactDealRerollDrawCards"];

		public static TConst ArtifactDealMaxPreferTagCount => Table["ArtifactDealMaxPreferTagCount"];

		public static TConst ArtifactDealPreferMatchSlot1Percent => Table["ArtifactDealPreferMatchSlot1Percent"];

		public static TConst ArtifactDealPreferMatchSlot2Percent => Table["ArtifactDealPreferMatchSlot2Percent"];

		public static TConst ArtifactDealPreferMatchSlot3Percent => Table["ArtifactDealPreferMatchSlot3Percent"];

		public static TConst ArtifactDealDislikeMatchSlot1Percent => Table["ArtifactDealDislikeMatchSlot1Percent"];

		public static TConst ArtifactDealDislikeMatchSlot2Percent => Table["ArtifactDealDislikeMatchSlot2Percent"];

		public static TConst ArtifactDealDislikeMatchSlot3Percent => Table["ArtifactDealDislikeMatchSlot3Percent"];

		public static TConst ArtifactDealAddPreferMatchPercent => Table["ArtifactDealAddPreferMatchPercent"];

		public static TConst ArtifactDealRerollPreferAddMatchPercent => Table["ArtifactDealRerollPreferAddMatchPercent"];

		public static TConst ArtifactDealRerollDislikeRemoveMatchPercent => Table["ArtifactDealRerollDislikeRemoveMatchPercent"];

		public static TConst ArtifactDealCardRelevanceWeightPercent => Table["ArtifactDealCardRelevanceWeightPercent"];

		public static TConst ArtifactDealRerollPreferMatchedWeightPercent => Table["ArtifactDealRerollPreferMatchedWeightPercent"];

		public static TConst ArtifactDealRemoveDislikeMatchedWeightPercent => Table["ArtifactDealRemoveDislikeMatchedWeightPercent"];

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier1 => Table["ArtifactDealTagMatchedBonusValueVfxTier1"];

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier2 => Table["ArtifactDealTagMatchedBonusValueVfxTier2"];

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier3 => Table["ArtifactDealTagMatchedBonusValueVfxTier3"];

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier4 => Table["ArtifactDealTagMatchedBonusValueVfxTier4"];

		public static TConst ArtifactDealTagMatchedBonusValueVfxTier5 => Table["ArtifactDealTagMatchedBonusValueVfxTier5"];

		public static TConst ArtifactDealSuccessTierHigh => Table["ArtifactDealSuccessTierHigh"];

		public static TConst ArtifactDealSuccessTierMid => Table["ArtifactDealSuccessTierMid"];

		public static TConst SuppressionResistanceGrowthFactor => Table["SuppressionResistanceGrowthFactor"];

		public static TConst GroggyDamageMultiplier => Table["GroggyDamageMultiplier  "];

		public static TConst SuppressionGaugeFactorEasy => Table["SuppressionGaugeFactorEasy"];

		public static TConst SuppressionGaugeFactorNormal => Table["SuppressionGaugeFactorNormal"];

		public static TConst SuppressionGaugeFactorHard => Table["SuppressionGaugeFactorHard"];

		public static TConst SuppressionGaugeFactorInsane => Table["SuppressionGaugeFactorInsane"];

		public static TConst SuppressionImmunityDuration => Table["SuppressionImmunityDuration"];

		public static TConst GroggyDuration => Table["GroggyDuration"];

		public static TConst TycoonWorkProgressPerTurn => Table["TycoonWorkProgressPerTurn"];

		public static TConst TycoonMaxWorkProgressFactor => Table["TycoonMaxWorkProgressFactor"];

		public static TConst TycoonMaxWorkProgressMinRatio => Table["TycoonMaxWorkProgressMinRatio"];

		public static TConst TycoonMaxWorkProgressMaxRatio => Table["TycoonMaxWorkProgressMaxRatio"];

		public static TConst TycoonDefaultMaxWorkProgressNormal => Table["TycoonDefaultMaxWorkProgressNormal"];

		public static TConst TycoonDefaultMaxWorkProgressRare => Table["TycoonDefaultMaxWorkProgressRare"];

		public static TConst TycoonDefaultMaxWorkProgressElite => Table["TycoonDefaultMaxWorkProgressElite"];

		public static TConst TycoonDefaultMaxWorkProgressEpic => Table["TycoonDefaultMaxWorkProgressEpic"];

		public static TConst TycoonDefaultMaxWorkProgressLegendary => Table["TycoonDefaultMaxWorkProgressLegendary"];

		public static TConst TycoonDefaultMaxWorkProgressUnique => Table["TycoonDefaultMaxWorkProgressUnique"];

		public static TConst TycoonEmployeeStressRecoveryPerTurn => Table["TycoonEmployeeStressRecoveryPerTurn"];

		public static TConst TycoonEmployeeSwapPenaltyRatio => Table["TycoonEmployeeSwapPenaltyRatio"];

		public static TConst TycoonEmployeePurifyCostBase => Table["TycoonEmployeePurifyCostBase"];

		public static TConst TycoonEmployeePurifyCostFactor => Table["TycoonEmployeePurifyCostFactor"];

		public static TConst TycoonArtifactDamageGoldRatio => Table["TycoonArtifactDamageGoldRatio"];

		public static TConst TycoonCheapSaleGoldRatio => Table["TycoonCheapSaleGoldRatio"];

		public static TConst TycoonWorkProgressCharmFactor => Table["TycoonWorkProgressCharmFactor"];

		public static TConst TycoonWorkProgressPersuasionFactor => Table["TycoonWorkProgressPersuasionFactor"];

		public static TConst TycoonSalePricePersuasionFactor => Table["TycoonSalePricePersuasionFactor"];

		public static TConst TycoonEmployeeExpGainNormal => Table["TycoonEmployeeExpGainNormal"];

		public static TConst TycoonEmployeeExpGainRare => Table["TycoonEmployeeExpGainRare"];

		public static TConst TycoonEmployeeExpGainElite => Table["TycoonEmployeeExpGainElite"];

		public static TConst TycoonEmployeeExpGainEpic => Table["TycoonEmployeeExpGainEpic"];

		public static TConst TycoonEmployeeExpGainLegendary => Table["TycoonEmployeeExpGainLegendary"];

		public static TConst TycoonEmployeeExpGainUnique => Table["TycoonEmployeeExpGainUnique"];

		public static TConst TycoonEmployeeSkillLearnPercentPerLevel => Table["TycoonEmployeeSkillLearnPercentPerLevel"];

		public static TConst TycoonEmployeeSkillLearnPercent => Table["TycoonEmployeeSkillLearnPercent"];

		public static TConst TycoonHireCostPerLevelFactor => Table["TycoonHireCostPerLevelFactor"];

		public static TConst WeakPointShakeAmplitude => Table["WeakPointShakeAmplitude"];

		public static TConst WeakPointShakeFrequency => Table["WeakPointShakeFrequency"];

		public static TConst WeakPointShakeDuration => Table["WeakPointShakeDuration"];

		public static TConst ExitSpecialFieldRange => Table["ExitSpecialFieldRange"];

		public static TConst AnomalyDeepOneSpawnChancePercent => Table["AnomalyDeepOneSpawnChancePercent"];

		public static TConst AnomalyMonsterNoneCombatSightMultiplier => Table["AnomalyMonsterNoneCombatSightMultiplier"];

		public static TConst AnomalyTrapSpawnMultiplier => Table["AnomalyTrapSpawnMultiplier"];

		public static TConst AnomalyMinimapNoiseRatio => Table["AnomalyMinimapNoiseRatio"];

		public static TConst AnomalyMinimapNoiseChangeInterval => Table["AnomalyMinimapNoiseChangeInterval"];

		public static TConst AnomalyArtifactGradeUpStep => Table["AnomalyArtifactGradeUpStep"];

		public static TConst AnomalyArtifactGradeUpChancePercent => Table["AnomalyArtifactGradeUpChancePercent"];

		public static TConst CoinToGoldRate => Table["CoinToGoldRate"];

		public static TConst MaxEmployeeCountBase => Table["MaxEmployeeCountBase"];

		public static TConst ArtifactPriceMinPercent => Table["ArtifactPriceMinPercent"];

		public static TConst ArtifactPriceCampaignLevelGrowthRate => Table["ArtifactPriceCampaignLevelGrowthRate"];

		public static TConst CharacterWeaponTypeDropWeightBonusPercent => Table["CharacterWeaponTypeDropWeightBonusPercent"];

		public static TConst FireDamageFreezeDurationReductionPercent => Table["FireDamageFreezeDurationReductionPercent"];

		public static TConst FireDamageFreezeDurationReductionCooldown => Table["FireDamageFreezeDurationReductionCooldown"];

		public static TConst FireDamageFreezeDurationReductionSfx => Table["FireDamageFreezeDurationReductionSfx"];

		public static TConst FreezeEndSfx => Table["FreezeEndSfx"];

		public static TConst HitboxAimCorrectionAngle => Table["HitboxAimCorrectionAngle"];

		public static TConst LostItemRerollCost => Table["LostItemRerollCost"];

		public static TConst LostItemRerollCostMultiplier => Table["LostItemRerollCostMultiplier"];

		public static TConst CapturedMonsterScale => Table["CapturedMonsterScale"];

		public static TConst DespawnDelayOnCaptured => Table["DespawnDelayOnCaptured"];

		public static TConst PadNavFirstDelay => Table["PadNavFirstDelay"];

		public static TConst PadNavRepeatRate => Table["PadNavRepeatRate"];

		public static TConst VoteTimeLimit => Table["VoteTimeLimit"];

		public static TConst LowWeaponDurabilityValue => Table["LowWeaponDurabilityValue"];

		public string Pk
		{
			get
			{
				return Id;
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
				m_OnTableChanged += value;
			}
			[CompilerGenerated]
			remove
			{
				m_OnTableChanged -= value;
			}
		}

		public void Parse(string[] columns)
		{
			Id = TableParse.Str(columns[0]);
			IntValue = TableParse.Int(columns[1]);
			FloatValue = TableParse.Float(columns[2]);
			StringValue = TableParse.Str(columns[3]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, IntValue, FloatValue, StringValue);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,IntValue,FloatValue,StringValue";
		}

		public static string GetTypesCsv()
		{
			return "string,int,float,string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new ConstTable();
			table.Load(new CsvTableLoader<string, TConst>(text));
			Table = table;
			InvokeTableChangeEvent();
		}

		public static void InvokeTableChangeEvent()
		{
			m_OnTableChanged?.Invoke();
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
