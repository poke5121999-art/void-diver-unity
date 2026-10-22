using System;
using System.Runtime.CompilerServices;
using SomeNet.Table;

namespace GameCommon.Table
{
	[Serializable]
	public class TLocalizedText : ITableData<string>
	{
		[CompilerGenerated]
		private static Action m_OnTableChanged;

		public static LocalizedTextTable Table { get; set; }

		public static TLocalizedText CharacterTalentNeedTalentCondition => null;

		public static TLocalizedText CharacterTalentTier => null;

		public static TLocalizedText ChatFailBlocked => null;

		public static TLocalizedText ChatFailEmpty => null;

		public static TLocalizedText ChatPlayerEntered => null;

		public static TLocalizedText ChatPlayerLeft => null;

		public static TLocalizedText ChatEquipmentBreak => null;

		public static TLocalizedText DialogLevelUpAccept => null;

		public static TLocalizedText DialogLevelUpDeny => null;

		public static TLocalizedText DialogLevelUpEnoughAllConditon => null;

		public static TLocalizedText DialogLevelUpExpNotEnough => null;

		public static TLocalizedText DialogLevelUpGoldNotEnough => null;

		public static TLocalizedText DialogLevelUpCoinNotEnough => null;

		public static TLocalizedText DialogLevelUpMaxLevel => null;

		public static TLocalizedText FeatureComingSoon => null;

		public static TLocalizedText FilterItemTypeAll => null;

		public static TLocalizedText TItemNote10001 => null;

		public static TLocalizedText ItemTypeArtifact => null;

		public static TLocalizedText ItemTypeConsumable => null;

		public static TLocalizedText ItemTypeKey => null;

		public static TLocalizedText ItemTypeMisc => null;

		public static TLocalizedText ItemTypeNote => null;

		public static TLocalizedText KeyInteractionOnEmptySlotClick => null;

		public static TLocalizedText KeyInteractionOnNomoreSlotsToClick => null;

		public static TLocalizedText KeyInteractionOnSameItemClick => null;

		public static TLocalizedText MadnessDamageProvideText => null;

		public static TLocalizedText MadnessDamageText => null;

		public static TLocalizedText MadnessText => null;

		public static TLocalizedText PingAlertFormat => null;

		public static TLocalizedText PingAlertObjectFormat => null;

		public static TLocalizedText PingBlocked => null;

		public static TLocalizedText PingItemFormat => null;

		public static TLocalizedText PingNoticeFormat => null;

		public static TLocalizedText PingNoticeObjectFormat => null;

		public static TLocalizedText PingSkillFormat => null;

		public static TLocalizedText PingStatusFormat => null;

		public static TLocalizedText PingTypeFormat => null;

		public static TLocalizedText ResetCharacterTalentDesc => null;

		public static TLocalizedText ResetCharacterTalentTitle => null;

		public static TLocalizedText UserLevel => null;

		public static TLocalizedText CharacterTalentAlreadyUnlocked => null;

		public static TLocalizedText ExclusiveCharacterTalentUnlocked => null;

		public static TLocalizedText TalentUnlocked => null;

		public static TLocalizedText EquipmentCraftPopupName => null;

		public static TLocalizedText TestRoom => null;

		public static TLocalizedText Base => null;

		public static TLocalizedText DepartingSoon => null;

		public static TLocalizedText ExceedingBossesNotDisplayed => null;

		public static TLocalizedText Cancel => null;

		public static TLocalizedText EmojiCancelled => null;

		public static TLocalizedText EmojiClickedWithoutDrag => null;

		public static TLocalizedText DestructibleObject => null;

		public static TLocalizedText DungeonEntrance => null;

		public static TLocalizedText Elara => null;

		public static TLocalizedText DivingBooth => null;

		public static TLocalizedText Portal => null;

		public static TLocalizedText WaveActivator => null;

		public static TLocalizedText CallDivingBooth => null;

		public static TLocalizedText Escape => null;

		public static TLocalizedText Pickup => null;

		public static TLocalizedText Inspect => null;

		public static TLocalizedText Rescue => null;

		public static TLocalizedText MindSync => null;

		public static TLocalizedText Open => null;

		public static TLocalizedText Close => null;

		public static TLocalizedText Break => null;

		public static TLocalizedText StopDeparture => null;

		public static TLocalizedText Board => null;

		public static TLocalizedText Interact => null;

		public static TLocalizedText PingCancelled => null;

		public static TLocalizedText ActionNotAvailableWhileDead => null;

		public static TLocalizedText NothingHappened => null;

		public static TLocalizedText CannotCarryMoreSlot => null;

		public static TLocalizedText CannotCarryMoreStack => null;

		public static TLocalizedText CannotCarrySameBagType => null;

		public static TLocalizedText CannotEnterStage => null;

		public static TLocalizedText InventoryManagement => null;

		public static TLocalizedText EquipmentManagement => null;

		public static TLocalizedText StorageManagement => null;

		public static TLocalizedText CampaignHostOnly => null;

		public static TLocalizedText ConfirmHostStartCampaign => null;

		public static TLocalizedText ConfirmAbandonCampaign => null;

		public static TLocalizedText AbandonCampaignWarning => null;

		public static TLocalizedText ConfirmAbandonCampaignOtherPlayerNotCompleted => null;

		public static TLocalizedText ItemCrafting => null;

		public static TLocalizedText WeaponCrafting => null;

		public static TLocalizedText AccessoryCrafting => null;

		public static TLocalizedText NoActiveCampaign => null;

		public static TLocalizedText NotEnoughActionPoints => null;

		public static TLocalizedText RestToPurifyParadox => null;

		public static TLocalizedText RestToPurifyParadoxDesc => null;

		public static TLocalizedText NoActiveParadox => null;

		public static TLocalizedText CharacterSelect => null;

		public static TLocalizedText SkillSelect => null;

		public static TLocalizedText CharacterTraits => null;

		public static TLocalizedText TechDevelopment => null;

		public static TLocalizedText Yes => null;

		public static TLocalizedText No => null;

		public static TLocalizedText Rested => null;

		public static TLocalizedText EnemiesIncoming => null;

		public static TLocalizedText DivingBoothCalled => null;

		public static TLocalizedText BoardDivingBooth => null;

		public static TLocalizedText LightRadiatesAll => null;

		public static TLocalizedText BrighterLightNearby => null;

		public static TLocalizedText PainMentalDamage => null;

		public static TLocalizedText AllyVoiceFades => null;

		public static TLocalizedText DangerousNearby => null;

		public static TLocalizedText FearApproaches => null;

		public static TLocalizedText DespairAllEnds => null;

		public static TLocalizedText ThisIsTheEnd => null;

		public static TLocalizedText NotOverYet => null;

		public static TLocalizedText DoorOfDeath => null;

		public static TLocalizedText SomeoneIsReady => null;

		public static TLocalizedText Preparing => null;

		public static TLocalizedText EscapeNow => null;

		public static TLocalizedText Dislike => null;

		public static TLocalizedText Like => null;

		public static TLocalizedText MinCountOne => null;

		public static TLocalizedText ConfirmExitGame => null;

		public static TLocalizedText AvailableForDevelopment => null;

		public static TLocalizedText DevelopmentComplete => null;

		public static TLocalizedText RequiresPrerequisiteTrait => null;

		public static TLocalizedText NotEnoughGold => null;

		public static TLocalizedText NotEnoughReputationLevel => null;

		public static TLocalizedText TraitsReset => null;

		public static TLocalizedText NotEnoughMaterialsForCraft => null;

		public static TLocalizedText NotEnoughFundsForCraft => null;

		public static TLocalizedText ConfirmWeaponCraft => null;

		public static TLocalizedText ConfirmAccessoryCraft => null;

		public static TLocalizedText NotEnoughMaterials => null;

		public static TLocalizedText NoEmptySlots => null;

		public static TLocalizedText CannotUnequipWeapon => null;

		public static TLocalizedText CannotDropInLootInventory => null;

		public static TLocalizedText CannotDropInQuickSlot => null;

		public static TLocalizedText SelectItemInInventory => null;

		public static TLocalizedText CannotUseInLounge => null;

		public static TLocalizedText CannotUseInInventory => null;

		public static TLocalizedText ItemUsePending => null;

		public static TLocalizedText CannotUseItemInCurrentState => null;

		public static TLocalizedText CannotExceedCarryLimit => null;

		public static TLocalizedText CannotRegisterToQuickSlot => null;

		public static TLocalizedText NotEnoughCurrency => null;

		public static TLocalizedText NotEnoughCraftMaterials => null;

		public static TLocalizedText CraftLocked => null;

		public static TLocalizedText SelectPurchaseQuantity => null;

		public static TLocalizedText NotEnoughFunds => null;

		public static TLocalizedText NotEnoughInventorySlots => null;

		public static TLocalizedText AlreadyAtMaxCount => null;

		public static TLocalizedText CraftInvenMax => null;

		public static TLocalizedText SelectItem => null;

		public static TLocalizedText CannotSellRelic => null;

		public static TLocalizedText NoItemsToSell => null;

		public static TLocalizedText SelectQuantityToSell => null;

		public static TLocalizedText CannotDropWarehouseItem => null;

		public static TLocalizedText SelectItemToDrop => null;

		public static TLocalizedText AlreadyDeveloped => null;

		public static TLocalizedText LowestDifficulty => null;

		public static TLocalizedText HighestDifficulty => null;

		public static TLocalizedText FirstCampaign => null;

		public static TLocalizedText LastCampaign => null;

		public static TLocalizedText VoteCancelledByOpposition => null;

		public static TLocalizedText EnterUserName => null;

		public static TLocalizedText UserNameNoSpecialChars => null;

		public static TLocalizedText GameConnectionFailed => null;

		public static TLocalizedText GameAlreadyStarted => null;

		public static TLocalizedText PlayerLimitExceeded => null;

		public static TLocalizedText FeatureInProgress => null;

		public static TLocalizedText CannotDeleteParadoxRank => null;

		public static TLocalizedText InsufficientFunds => null;

		public static TLocalizedText GraphicsQuality => null;

		public static TLocalizedText EffectToggle => null;

		public static TLocalizedText EffectOn => null;

		public static TLocalizedText EffectOff => null;

		public static TLocalizedText SlotUnlockRequired => null;

		public static TLocalizedText CannotSurviveWithoutLight => null;

		public static TLocalizedText TooTiredToRun => null;

		public static TLocalizedText Unknown => null;

		public static TLocalizedText MindHealing => null;

		public static TLocalizedText NoPointInDoingThis => null;

		public static TLocalizedText ElementAbsorption => null;

		public static TLocalizedText Invincible => null;

		public static TLocalizedText DeadMonsterFormat => null;

		public static TLocalizedText UnsupportedTechTreeFormat => null;

		public static TLocalizedText MaxItemCountFormat => null;

		public static TLocalizedText CraftableItemCountFormat => null;

		public static TLocalizedText SecondsFormat => null;

		public static TLocalizedText EquipmentEquippedFormat => null;

		public static TLocalizedText ConfirmEquipItemFormat => null;

		public static TLocalizedText GoodsCraftedFormat => null;

		public static TLocalizedText GoodsCraftedWithStashFormat => null;

		public static TLocalizedText GoodsCraftedCountFormat => null;

		public static TLocalizedText ItemPurchasedFormat => null;

		public static TLocalizedText ItemSoldFormat => null;

		public static TLocalizedText TechTreeCompletedFormat => null;

		public static TLocalizedText UsernameLengthLimitFormat => null;

		public static TLocalizedText NetworkErrorFormat => null;

		public static TLocalizedText ParadoxPurifyCompletedFormat => null;

		public static TLocalizedText ParadoxPurifyConfirm => null;

		public static TLocalizedText PlayerKilledByFormat => null;

		public static TLocalizedText StatusEffectImmuneFormat => null;

		public static TLocalizedText GoodsCountOwnedFormat => null;

		public static TLocalizedText ItemOverflowMovedToStash => null;

		public static TLocalizedText Owned => null;

		public static TLocalizedText Equipped => null;

		public static TLocalizedText NextPage => null;

		public static TLocalizedText PreviousPage => null;

		public static TLocalizedText Select => null;

		public static TLocalizedText ConfirmeTrade => null;

		public static TLocalizedText Run => null;

		public static TLocalizedText Continue => null;

		public static TLocalizedText GiveUpStage => null;

		public static TLocalizedText ExitTheLounge => null;

		public static TLocalizedText ReturnToTitle => null;

		public static TLocalizedText EndGame => null;

		public static TLocalizedText Inventory => null;

		public static TLocalizedText LootingInventory => null;

		public static TLocalizedText Stash => null;

		public static TLocalizedText SafeInventory => null;

		public static TLocalizedText Mission => null;

		public static TLocalizedText Stress => null;

		public static TLocalizedText HpCost => null;

		public static TLocalizedText StaminaCost => null;

		public static TLocalizedText CoolTime => null;

		public static TLocalizedText StackCoolTime => null;

		public static TLocalizedText Elemental => null;

		public static TLocalizedText Confirm => null;

		public static TLocalizedText Trade => null;

		public static TLocalizedText OwnedCurrency => null;

		public static TLocalizedText OwnedCoin => null;

		public static TLocalizedText OwnedRealCoin => null;

		public static TLocalizedText CoinExchangeOnline => null;

		public static TLocalizedText CoinExchangeReal => null;

		public static TLocalizedText SelectForSale => null;

		public static TLocalizedText ToggleInventoryStorage => null;

		public static TLocalizedText Reward => null;

		public static TLocalizedText Agree => null;

		public static TLocalizedText Disagree => null;

		public static TLocalizedText Voting => null;

		public static TLocalizedText CancelVoting => null;

		public static TLocalizedText Tutorial => null;

		public static TLocalizedText NormalCampaign => null;

		public static TLocalizedText StoryCampaign => null;

		public static TLocalizedText Cure => null;

		public static TLocalizedText Create => null;

		public static TLocalizedText NeededCost => null;

		public static TLocalizedText ConsumedMaterial => null;

		public static TLocalizedText Quantity => null;

		public static TLocalizedText EnterRoomByCode => null;

		public static TLocalizedText Enter => null;

		public static TLocalizedText Refresh => null;

		public static TLocalizedText Connect => null;

		public static TLocalizedText Back => null;

		public static TLocalizedText Complete => null;

		public static TLocalizedText CraftableQuantity => null;

		public static TLocalizedText Empty => null;

		public static TLocalizedText Difficulty => null;

		public static TLocalizedText TimeLimit => null;

		public static TLocalizedText FailCondition => null;

		public static TLocalizedText MainEnemy => null;

		public static TLocalizedText ConsensusSucceeded => null;

		public static TLocalizedText ConsensusFailed => null;

		public static TLocalizedText SkillSetting => null;

		public static TLocalizedText TalentSetting => null;

		public static TLocalizedText SlotInsertOne => null;

		public static TLocalizedText SlotInsertAll => null;

		public static TLocalizedText SlotInsertHalf => null;

		public static TLocalizedText SlotMove => null;

		public static TLocalizedText SlotSort => null;

		public static TLocalizedText SlotDiscardOne => null;

		public static TLocalizedText SlotDiscardAll => null;

		public static TLocalizedText SlotDiscardHalf => null;

		public static TLocalizedText SlotUse => null;

		public static TLocalizedText SlotMarking => null;

		public static TLocalizedText Crafting => null;

		public static TLocalizedText Equip => null;

		public static TLocalizedText Unequip => null;

		public static TLocalizedText QuickSlotGuide => null;

		public static TLocalizedText ShopSelectBuyOrSell => null;

		public static TLocalizedText ShopSelectBuyOrSellMax => null;

		public static TLocalizedText ShopInstantBuyOrSellOne => null;

		public static TLocalizedText ShopInstantBuyOrSellMax => null;

		public static TLocalizedText Character => null;

		public static TLocalizedText SkillAndTalent => null;

		public static TLocalizedText Squad => null;

		public static TLocalizedText Option => null;

		public static TLocalizedText System => null;

		public static TLocalizedText Paradox => null;

		public static TLocalizedText QuickView => null;

		public static TLocalizedText NextView => null;

		public static TLocalizedText CancelDialog => null;

		public static TLocalizedText Exp => null;

		public static TLocalizedText CTag1 => null;

		public static TLocalizedText CTag2 => null;

		public static TLocalizedText CTag3 => null;

		public static TLocalizedText CTag4 => null;

		public static TLocalizedText CTag5 => null;

		public static TLocalizedText CTag6 => null;

		public static TLocalizedText CTag7 => null;

		public static TLocalizedText CTag8 => null;

		public static TLocalizedText CTag9 => null;

		public static TLocalizedText CTag10 => null;

		public static TLocalizedText CTag11 => null;

		public static TLocalizedText CTag12 => null;

		public static TLocalizedText CTag13 => null;

		public static TLocalizedText CTag14 => null;

		public static TLocalizedText CTag15 => null;

		public static TLocalizedText CTag16 => null;

		public static TLocalizedText CTag17 => null;

		public static TLocalizedText CTag18 => null;

		public static TLocalizedText CTag19 => null;

		public static TLocalizedText WarningMaxVolume => null;

		public static TLocalizedText WarningMinVolume => null;

		public static TLocalizedText On => null;

		public static TLocalizedText Off => null;

		public static TLocalizedText CanBeChangedOnlyOnTitle => null;

		public static TLocalizedText NewGame => null;

		public static TLocalizedText SinglePlay => null;

		public static TLocalizedText CoopPlay => null;

		public static TLocalizedText MakeLounge => null;

		public static TLocalizedText JoinLounge => null;

		public static TLocalizedText Reset => null;

		public static TLocalizedText Quit => null;

		public static TLocalizedText ConfirmResetAndNewGame => null;

		public static TLocalizedText SkillMonologue10010000 => null;

		public static TLocalizedText SkillMonologue10010300 => null;

		public static TLocalizedText SkillMonologue10010400 => null;

		public static TLocalizedText SkillMonologue10010500 => null;

		public static TLocalizedText SkillMonologue100106001 => null;

		public static TLocalizedText SkillMonologue100106002 => null;

		public static TLocalizedText SkillMonologue10011101 => null;

		public static TLocalizedText SkillMonologue100113001 => null;

		public static TLocalizedText SkillMonologue100113002 => null;

		public static TLocalizedText SkillMonologue100113003 => null;

		public static TLocalizedText SkillMonologue100113011 => null;

		public static TLocalizedText SkillMonologue100113012 => null;

		public static TLocalizedText SkillMonologue100113013 => null;

		public static TLocalizedText SkillMonologue100114001 => null;

		public static TLocalizedText SkillMonologue100114002 => null;

		public static TLocalizedText SkillMonologue100114003 => null;

		public static TLocalizedText SkillMonologue100114011 => null;

		public static TLocalizedText SkillMonologue100114012 => null;

		public static TLocalizedText SkillMonologue100115001 => null;

		public static TLocalizedText SkillMonologue100115002 => null;

		public static TLocalizedText SkillMonologue100115011 => null;

		public static TLocalizedText SkillMonologue100115012 => null;

		public static TLocalizedText SkillMonologue10011600 => null;

		public static TLocalizedText SkillMonologue10011601 => null;

		public static TLocalizedText SkillMonologue10011701 => null;

		public static TLocalizedText SkillMonologue10011703 => null;

		public static TLocalizedText SkillMonologue10012100 => null;

		public static TLocalizedText SkillMonologue10012300 => null;

		public static TLocalizedText SkillMonologue100124001 => null;

		public static TLocalizedText SkillMonologue100124002 => null;

		public static TLocalizedText SkillMonologue100125001 => null;

		public static TLocalizedText SkillMonologue100125002 => null;

		public static TLocalizedText SkillMonologue10012600 => null;

		public static TLocalizedText SkillMonologue10012700 => null;

		public static TLocalizedText SkillMonologue10013200 => null;

		public static TLocalizedText SkillMonologue10013300 => null;

		public static TLocalizedText SkillMonologue20000100 => null;

		public static TLocalizedText SkillMonologue20000300 => null;

		public static TLocalizedText SkillMonologue200006001 => null;

		public static TLocalizedText SkillMonologue200006002 => null;

		public static TLocalizedText SkillMonologue200006003 => null;

		public static TLocalizedText SkillMonologue200010001 => null;

		public static TLocalizedText SkillMonologue200010002 => null;

		public static TLocalizedText SkillMonologue200010003 => null;

		public static TLocalizedText SkillMonologue20001001 => null;

		public static TLocalizedText SkillMonologue20001002 => null;

		public static TLocalizedText SkillMonologue20001031 => null;

		public static TLocalizedText SkillMonologue200010401 => null;

		public static TLocalizedText SkillMonologue200010402 => null;

		public static TLocalizedText SkillMonologue20001062 => null;

		public static TLocalizedText SkillMonologue20001064 => null;

		public static TLocalizedText SkillMonologue200021011 => null;

		public static TLocalizedText SkillMonologue200021012 => null;

		public static TLocalizedText SkillMonologue20002102 => null;

		public static TLocalizedText SkillMonologue21000051 => null;

		public static TLocalizedText SkillMonologue21000052 => null;

		public static TLocalizedText SkillMonologue21000056 => null;

		public static TLocalizedText SkillMonologue21000058 => null;

		public static TLocalizedText SkillMonologue21000073 => null;

		public static TLocalizedText SkillMonologue21000075 => null;

		public static TLocalizedText SkillMonologue21000076 => null;

		public static TLocalizedText SkillMonologue21000080 => null;

		public static TLocalizedText SkillMonologue21000100 => null;

		public static TLocalizedText SkillMonologue21000103 => null;

		public static TLocalizedText SkillMonologue21000104 => null;

		public static TLocalizedText SkillMonologue21000150 => null;

		public static TLocalizedText SkillMonologue21000153 => null;

		public static TLocalizedText SkillMonologue21000154 => null;

		public static TLocalizedText SkillMonologue21000350 => null;

		public static TLocalizedText SkillMonologue21000351 => null;

		public static TLocalizedText SkillMonologue21000352 => null;

		public static TLocalizedText SkillMonologue21000353 => null;

		public static TLocalizedText SkillMonologue21000355 => null;

		public static TLocalizedText SkillMonologue21000400 => null;

		public static TLocalizedText SkillMonologue21000401 => null;

		public static TLocalizedText SkillMonologue21000402 => null;

		public static TLocalizedText SkillMonologue21000403 => null;

		public static TLocalizedText SkillMonologue21000512 => null;

		public static TLocalizedText SkillMonologue21000521 => null;

		public static TLocalizedText SkillMonologue21000522 => null;

		public static TLocalizedText SkillMonologue21000536 => null;

		public static TLocalizedText SkillMonologue21000553 => null;

		public static TLocalizedText SkillMonologue21000573 => null;

		public static TLocalizedText SkillMonologue21000575 => null;

		public static TLocalizedText SkillMonologue21000580 => null;

		public static TLocalizedText SkillMonologue21000581 => null;

		public static TLocalizedText SkillMonologue21000583 => null;

		public static TLocalizedText SkillMonologue210005832 => null;

		public static TLocalizedText SkillMonologue210005901 => null;

		public static TLocalizedText SkillMonologue210005902 => null;

		public static TLocalizedText SkillMonologue210005903 => null;

		public static TLocalizedText SkillMonologue210005904 => null;

		public static TLocalizedText SkillMonologue210006011 => null;

		public static TLocalizedText SkillMonologue210006012 => null;

		public static TLocalizedText SkillMonologue210006013 => null;

		public static TLocalizedText SkillMonologue210006101 => null;

		public static TLocalizedText SkillMonologue210006102 => null;

		public static TLocalizedText SkillMonologue210006211 => null;

		public static TLocalizedText SkillMonologue210006212 => null;

		public static TLocalizedText SkillMonologue210006213 => null;

		public static TLocalizedText SkillMonologue210006214 => null;

		public static TLocalizedText SkillMonologue21004006 => null;

		public static TLocalizedText SkillMonologue21100004 => null;

		public static TLocalizedText SkillMonologue30010040 => null;

		public static TLocalizedText SkillMonologue30010041 => null;

		public static TLocalizedText SkillMonologue50002000 => null;

		public static TLocalizedText SkillMonologue50002006 => null;

		public static TLocalizedText SkillMonologue50002031 => null;

		public static TLocalizedText SkillMonologue50002041 => null;

		public static TLocalizedText SkillMonologue50002051 => null;

		public static TLocalizedText SkillMonologue50002061 => null;

		public static TLocalizedText SkillMonologue50002071 => null;

		public static TLocalizedText SkillMonologue50002094 => null;

		public static TLocalizedText SkillMonologue50002095 => null;

		public static TLocalizedText SkillMonologue50002106 => null;

		public static TLocalizedText SkillMonologue50002111 => null;

		public static TLocalizedText SkillMonologue50002141 => null;

		public static TLocalizedText SkillMonologue50002142 => null;

		public static TLocalizedText CopyLoungeCode => null;

		public static TLocalizedText GameLoomLobbyNotSelected => null;

		public static TLocalizedText SteamInvalidLobbyId => null;

		public static TLocalizedText WarningUnknownGoods => null;

		public static TLocalizedText MiniMapMarker10001 => null;

		public static TLocalizedText MiniMapMarker10002 => null;

		public static TLocalizedText MiniMapMarker10003 => null;

		public static TLocalizedText MiniMapMarker10004 => null;

		public static TLocalizedText MiniMapMarker10005 => null;

		public static TLocalizedText MiniMapMarker10006 => null;

		public static TLocalizedText MiniMapMarker10007 => null;

		public static TLocalizedText MiniMapMarker10008 => null;

		public static TLocalizedText MiniMapMarker10009 => null;

		public static TLocalizedText MiniMapMarker10010 => null;

		public static TLocalizedText MiniMapMarker10011 => null;

		public static TLocalizedText MiniMapMarker10012 => null;

		public static TLocalizedText MiniMapMarker10013 => null;

		public static TLocalizedText MiniMapMarker10014 => null;

		public static TLocalizedText MiniMapMarker10015 => null;

		public static TLocalizedText MiniMapMarker10016 => null;

		public static TLocalizedText MiniMapMarker10017 => null;

		public static TLocalizedText MiniMapMarker10018 => null;

		public static TLocalizedText FloatingTextBackAttack => null;

		public static TLocalizedText FloatingTextResistance => null;

		public static TLocalizedText FloatingTextWeakPoint => null;

		public static TLocalizedText Seleted => null;

		public static TLocalizedText NetworkOffline => null;

		public static TLocalizedText ParadoxUnique => null;

		public static TLocalizedText ParadoxLevelFormat => null;

		public static TLocalizedText NotEnoughStashSlots => null;

		public static TLocalizedText MultiLoungeName => null;

		public static TLocalizedText Revealed => null;

		public static TLocalizedText GoodsFilterTypeAll => null;

		public static TLocalizedText GoodsFilterTypeEtc => null;

		public static TLocalizedText ApplyCharacterTalentTitle => null;

		public static TLocalizedText ApplyCharacterTalentDesc => null;

		public static TLocalizedText FullCostReturn => null;

		public static TLocalizedText UnlockBlueprintFailLearned => null;

		public static TLocalizedText UnlockBluePrintSuccessToast => null;

		public static TLocalizedText RecordBlueprint => null;

		public static TLocalizedText BlueprintInvalidTypeToast => null;

		public static TLocalizedText BlueprintRecordFailToast => null;

		public static TLocalizedText Filter => null;

		public static TLocalizedText MiniMapMarker4001 => null;

		public static TLocalizedText MiniMapMarker4002 => null;

		public static TLocalizedText MiniMapMarker4003 => null;

		public static TLocalizedText MiniMapMarker4004 => null;

		public static TLocalizedText MiniMapMarker4005 => null;

		public static TLocalizedText MiniMapMarker4006 => null;

		public static TLocalizedText MiniMapMarker4007 => null;

		public static TLocalizedText MiniMapMarker4008 => null;

		public static TLocalizedText MiniMapMarker4009 => null;

		public static TLocalizedText MiniMapMarker4010 => null;

		public static TLocalizedText MiniMapMarker4011 => null;

		public static TLocalizedText MiniMapMarker4012 => null;

		public static TLocalizedText MiniMapMarker4013 => null;

		public static TLocalizedText MiniMapMarker4101 => null;

		public static TLocalizedText MiniMapMarker4102 => null;

		public static TLocalizedText MiniMapMarker4103 => null;

		public static TLocalizedText MiniMapMarker4104 => null;

		public static TLocalizedText MiniMapMarker4105 => null;

		public static TLocalizedText MiniMapMarker4106 => null;

		public static TLocalizedText MiniMapMarker4107 => null;

		public static TLocalizedText MiniMapMarker4108 => null;

		public static TLocalizedText MiniMapMarker4109 => null;

		public static TLocalizedText MiniMapMarker4110 => null;

		public static TLocalizedText MiniMapMarker4111 => null;

		public static TLocalizedText MiniMapMarker5001 => null;

		public static TLocalizedText MiniMapMarker5002 => null;

		public static TLocalizedText MiniMapMarker5003 => null;

		public static TLocalizedText MiniMapMarker5004 => null;

		public static TLocalizedText MiniMapMarker5005 => null;

		public static TLocalizedText MiniMapMarker5006 => null;

		public static TLocalizedText MiniMapMarker5007 => null;

		public static TLocalizedText MiniMapMarker5008 => null;

		public static TLocalizedText MiniMapMarker5009 => null;

		public static TLocalizedText MiniMapMarker5010 => null;

		public static TLocalizedText NotEnoughGoodsToUnlockExit => null;

		public static TLocalizedText CreateLobbyMessage => null;

		public static TLocalizedText JoinLobbyMessage => null;

		public static TLocalizedText TryPublishRoomOnPlaying => null;

		public static TLocalizedText TryOpenRoomListOnPlaying => null;

		public static TLocalizedText Sight => null;

		public static TLocalizedText MoveSpeed => null;

		public static TLocalizedText ElementalResistance => null;

		public static TLocalizedText ElementalResistanceTagWeak => null;

		public static TLocalizedText ElementalResistanceTagNormal => null;

		public static TLocalizedText ElementalResistanceTagStrong => null;

		public static TLocalizedText CannotEquipWeaponType => null;

		public static TLocalizedText DefaultLobbyName => null;

		public static TLocalizedText DefaultLobbyNamePlaceholder => null;

		public static TLocalizedText EquipmentTooltipExclusiveCharacter => null;

		public static TLocalizedText FilterTypeAll => null;

		public static TLocalizedText LobbyUpdated => null;

		public static TLocalizedText AvailableRewardFormat => null;

		public static TLocalizedText ParadoxLockAlreadyFixed => null;

		public static TLocalizedText ParadoxLockConfirm => null;

		public static TLocalizedText ParadoxLockPreviousReleased => null;

		public static TLocalizedText ParadoxLockCompleted => null;

		public static TLocalizedText CorruptionValue => null;

		public static TLocalizedText CorruptionValueTotal => null;

		public static TLocalizedText LostBuy => null;

		public static TLocalizedText LostBuyCancel => null;

		public static TLocalizedText LostGoodsActionPointLeft => null;

		public static TLocalizedText LoungeNameEmptyMessage => null;

		public static TLocalizedText AlreadyInLoungeMessage => null;

		public static TLocalizedText NotInLoungeMessage => null;

		public static TLocalizedText DangerLevelAlreadyInLoungeMessage => null;

		public static TLocalizedText EquipmentOverflowMovedToStash => null;

		public static TLocalizedText TitleTitleText => null;

		public static TLocalizedText TitleSubtitleText => null;

		public static TLocalizedText TitleSteamWishListText => null;

		public static TLocalizedText TitleFeedbackText => null;

		public static TLocalizedText DialogBack => null;

		public static TLocalizedText CannotUseSkillWithoutWeapon => null;

		public static TLocalizedText CannotEnterStageWithoutWeapon => null;

		public static TLocalizedText EquipementSlotTypeMismatch => null;

		public static TLocalizedText NotEquipableEquipmentType => null;

		public static TLocalizedText NotEquipableWeaponType => null;

		public static TLocalizedText ExitTheLoungeDescription => null;

		public static TLocalizedText GiveUpDescription => null;

		public static TLocalizedText ReturnToTitleDescription => null;

		public static TLocalizedText CloseGameDescription => null;

		public static TLocalizedText CannotSellArtifact => null;

		public static TLocalizedText WrongKeyMessage => null;

		public static TLocalizedText SquadTabLockedMessage => null;

		public static TLocalizedText RoomDifficultyUpdated => null;

		public static TLocalizedText CannotUseInStage => null;

		public static TLocalizedText NotOpenedCraftingNpc => null;

		public static TLocalizedText PickUpItemMaxCount => null;

		public static TLocalizedText PickUpItemMaxCountMonologue => null;

		public static TLocalizedText CannotDealEquipedArtifact => null;

		public static TLocalizedText LostItemRecovery => null;

		public static TLocalizedText PurificationCost => null;

		public static TLocalizedText ParadoxLockCost => null;

		public static TLocalizedText LevelFormat => null;

		public static TLocalizedText InventoryFullDrop => null;

		public static TLocalizedText UiCloseEsc => null;

		public static TLocalizedText RadioToastChatFormat => null;

		public static TLocalizedText RewardBoxHitBoxTriggerd => null;

		public static TLocalizedText RestChatFormat => null;

		public static TLocalizedText NetworkErrorBackToLounge => null;

		public static TLocalizedText SelfReviveGuideChat => null;

		public static TLocalizedText TryPublishRoomOnOffline => null;

		public static TLocalizedText CampaignCompletedText => null;

		public static TLocalizedText DisclaimerText => null;

		public static TLocalizedText DisclaimerTitleText => null;

		public static TLocalizedText SuccessChanceFormat => null;

		public static TLocalizedText ArtifactDealCardPlaySuccessHeader => null;

		public static TLocalizedText ArtifactDealCardPlayFailureHeader => null;

		public static TLocalizedText ArtifactDealRemainPlayCountFormat => null;

		public static TLocalizedText ArtifactDealRerollFormat => null;

		public static TLocalizedText ArtifactDealNoMorePlayCount => null;

		public static TLocalizedText ArtifactDealCardPlaySuccess => null;

		public static TLocalizedText ArtifactDealCardPlayFailure => null;

		public static TLocalizedText ArtifactDealNoMoreRerollCount => null;

		public static TLocalizedText ArtifactDealGiveUpAskMassage => null;

		public static TLocalizedText ArtifactDealGiveUpNoticeFail => null;

		public static TLocalizedText ArtifactDealCannotPlayAlreadyHavePreferTag => null;

		public static TLocalizedText ArtifactDealCannotPlayPreferTagsAtMaxCountFormat => null;

		public static TLocalizedText ArtifactDealCannotPlayNoDislikeTagsToRemove => null;

		public static TLocalizedText ArtifactDealMatchedPreferTagCountFormat => null;

		public static TLocalizedText ArtifactDealStartDealFormat => null;

		public static TLocalizedText StockCountFormat => null;

		public static TLocalizedText NotEnoughStockCount => null;

		public static TLocalizedText FailedBuyShopProudct => null;

		public static TLocalizedText NoteNotAcquired => null;

		public static TLocalizedText RepairConfirmTitleFormat => null;

		public static TLocalizedText RepairAllConfirmTitle => null;

		public static TLocalizedText RepairResultText => null;

		public static TLocalizedText PlayGame => null;

		public static TLocalizedText LoadSaveSlot => null;

		public static TLocalizedText PlayerSlotNumberFormat => null;

		public static TLocalizedText EmptySlot => null;

		public static TLocalizedText ConfirmDeletePlayerSlotFormat => null;

		public static TLocalizedText CannotDeleteEmptySlot => null;

		public static TLocalizedText ConfirmPlayerName => null;

		public static TLocalizedText NewTag => null;

		public static TLocalizedText UnlockConditionNotMatch => null;

		public static TLocalizedText NotEnoughGoldOrCoin => null;

		public static TLocalizedText AlreadyUnlocked => null;

		public static TLocalizedText AlreadySelectedInterior => null;

		public static TLocalizedText SelecScrapQuantity => null;

		public static TLocalizedText InvalidScrapType => null;

		public static TLocalizedText InvalidScrapSpace => null;

		public static TLocalizedText ScrapFail => null;

		public static TLocalizedText ScrapSuccess => null;

		public static TLocalizedText NotEnoughScrapGoods => null;

		public static TLocalizedText CancelSelect => null;

		public static TLocalizedText UnknownMonster => null;

		public static TLocalizedText UnknownGoodsFormat => null;

		public static TLocalizedText TycoonArtifactSellFormat => null;

		public static TLocalizedText TycoonEmployeePanicFormat => null;

		public static TLocalizedText TycoonSalesSlotNameForamt => null;

		public static TLocalizedText TycoonNotSetEmployee => null;

		public static TLocalizedText TycoonNotSetArtifact => null;

		public static TLocalizedText EmployeeLevelUpTitleFormat => null;

		public static TLocalizedText TycoonResultTitleFormat => null;

		public static TLocalizedText EmployeePurifyConsumeConfirmTitle => null;

		public static TLocalizedText EmployeePurifyConsumeConfirmDesc => null;

		public static TLocalizedText ClearArtifact => null;

		public static TLocalizedText ClearEmployee => null;

		public static TLocalizedText AlreadyMaxEmployeeCount => null;

		public static TLocalizedText CoinDepositDesc => null;

		public static TLocalizedText CoinWithdrawalDesc => null;

		public static TLocalizedText SkillHpCostFormat => null;

		public static TLocalizedText SkillStressCostFormat => null;

		public static TLocalizedText SkillModeBaseFormat => null;

		public static TLocalizedText SkillUnequippedFormat => null;

		public static TLocalizedText SkillOwnerFormat => null;

		public static TLocalizedText ConfirmArtifactDeal => null;

		public static TLocalizedText NoAvailableGoods => null;

		public static TLocalizedText Notconnected => null;

		public static TLocalizedText UseKeyboardInput => null;

		public static TLocalizedText ContainsProhibitedWords => null;

		public static TLocalizedText LoungeBgm => null;

		public static TLocalizedText BgmAlreadySelected => null;

		public static TLocalizedText BgmLocked => null;

		public static TLocalizedText OnlyHostAllowed => null;

		public static TLocalizedText ShopGroup1 => null;

		public static TLocalizedText ShopGroup2 => null;

		public static TLocalizedText ShopGroup1001 => null;

		public static TLocalizedText ShopGroup1002 => null;

		public static TLocalizedText ShopGroup1003 => null;

		public static TLocalizedText ShopGroup1004 => null;

		public static TLocalizedText ShopGroup1005 => null;

		public static TLocalizedText ShopGroup600001 => null;

		public static TLocalizedText ShopGroup600002 => null;

		public static TLocalizedText ShopGroup600003 => null;

		public static TLocalizedText ShopGroup600004 => null;

		public static TLocalizedText ShopGroup600005 => null;

		public static TLocalizedText ShopGroup600006 => null;

		public static TLocalizedText ShopGroup600007 => null;

		public static TLocalizedText ShopGroup600008 => null;

		public static TLocalizedText ShopGroup600009 => null;

		public static TLocalizedText ShopGroup600010 => null;

		public static TLocalizedText ShopGroup600011 => null;

		public static TLocalizedText TalentGroup1 => null;

		public static TLocalizedText TalentGroup2 => null;

		public static TLocalizedText TalentGroup3 => null;

		public static TLocalizedText TalentGroup4 => null;

		public static TLocalizedText FreezeFireDamageFloatingText => null;

		public static TLocalizedText FullyImmuneText => null;

		public static TLocalizedText SelfReviveWithEquipmentChatFormat => null;

		public static TLocalizedText SelfReviveWithEquipmentFloatingTextFormat => null;

		public static TLocalizedText PlayTimeFormat => null;

		public static TLocalizedText FailedBuyInterior => null;

		public static TLocalizedText GoldAmountFormat => null;

		public static TLocalizedText CoinAmountFormat => null;

		public static TLocalizedText SelectTargetSaveSlotFirst => null;

		public static TLocalizedText ConfirmRestoreSaveData => null;

		public static TLocalizedText RestoreSaveDataFailed => null;

		public static TLocalizedText RestoreSaveDataCompleted => null;

		public static TLocalizedText ParadoxUnlockCost => null;

		public static TLocalizedText ParadoxUnlockConfirm => null;

		public static TLocalizedText ParadoxUnlockCompleted => null;

		public static TLocalizedText PointMarkerRestrictedArea => null;

		public static TLocalizedText TycoonEmployeeSettingConfirm => null;

		public static TLocalizedText TycoonArtifactSettingConfirm => null;

		public static TLocalizedText TycoonEmployeeSetting => null;

		public static TLocalizedText TycoonArtifactSetting => null;

		public static TLocalizedText CannotFirePlacedEmployee => null;

		public static TLocalizedText AnyMonster => null;

		public static TLocalizedText AnyCampaign => null;

		public static TLocalizedText AnyMission => null;

		public static TLocalizedText AnyDifficulty => null;

		public static TLocalizedText AnyEmployee => null;

		public static TLocalizedText AnyArtifactDeal => null;

		public static TLocalizedText AnyArtifact => null;

		public static TLocalizedText AnyItem => null;

		public static TLocalizedText LostItemList => null;

		public static TLocalizedText LostItemRefresh => null;

		public static TLocalizedText MoveToStashAll => null;

		public static TLocalizedText LobbyAlreadyStarted => null;

		public static TLocalizedText ActiveCondition => null;

		public static TLocalizedText SelectArtifactDealGoods => null;

		public static TLocalizedText Manual => null;

		public static TLocalizedText Area => null;

		public static TLocalizedText EquipmentRepair => null;

		public static TLocalizedText RepairAll => null;

		public static TLocalizedText ArchiveRecordComplete => null;

		public static TLocalizedText SellResultSuccess => null;

		public static TLocalizedText SellResultFail => null;

		public static TLocalizedText SellResultCost => null;

		public static TLocalizedText SellResultExp => null;

		public static TLocalizedText SellResultNotSale => null;

		public static TLocalizedText ArtifactDealAlreadyDoneNpc => null;

		public static TLocalizedText ArtifactDealingNpc => null;

		public static TLocalizedText Resolution => null;

		public static TLocalizedText Language => null;

		public static TLocalizedText Toggle => null;

		public static TLocalizedText Hold => null;

		public static TLocalizedText PadOnly => null;

		public static TLocalizedText CannotChangeLanguageWhilePlaying => null;

		public static TLocalizedText WindowOnly => null;

		public static TLocalizedText EmployeeNoStress => null;

		public static TLocalizedText InventoryFull => null;

		public static TLocalizedText SameBagAlreadyOwned => null;

		public static TLocalizedText CannotPutBagInBag => null;

		public static TLocalizedText GoodsNotAllowedInBag => null;

		public static TLocalizedText BagStackFull => null;

		public static TLocalizedText BagConsumeRequireEmpty => null;

		public static TLocalizedText BagConsumeNotEnough => null;

		public static TLocalizedText MonsterBossTypeMiniBoss => null;

		public static TLocalizedText MonsterBossTypeBoss => null;

		public static TLocalizedText DealingArtifact => null;

		public static TLocalizedText RerollArtifactCard => null;

		public static TLocalizedText RepairAllConfirmDesc => null;

		public static TLocalizedText SelectedSkill => null;

		public static TLocalizedText PurifyPlacedEmployeeMessage => null;

		public static TLocalizedText RoomSetting => null;

		public static TLocalizedText ChatEquipmentBrokenFormat => null;

		public static TLocalizedText MonologueEquipmentBrokenFormat => null;

		public static TLocalizedText EquipmentSkillCannotBeReplaced => null;

		public static TLocalizedText NotEnoughStashSpace => null;

		public static TLocalizedText CutSceneSkip => null;

		public static TLocalizedText DuplicateEquipPartGroup => null;

		public static TLocalizedText EmployeeSkillEffect => null;

		public static TLocalizedText EmployeeSkillStressReduction => null;

		public static TLocalizedText TalentAlreadyLearned => null;

		public static TLocalizedText TycoonEmployeeHired => null;

		public static TLocalizedText TycoonEmployeeChanged => null;

		public static TLocalizedText TycoonEmployeeFired => null;

		public static TLocalizedText TycoonEmployeePurified => null;

		public static TLocalizedText RegisterFormat => null;

		public static TLocalizedText RegisterInvalidTypeToastFormat => null;

		public static TLocalizedText RegisterFailAlready => null;

		public static TLocalizedText RegisterFailToast => null;

		public static TLocalizedText RegisterSuccessToastFormat => null;

		public static TLocalizedText SkillSlotNotEquip => null;

		public static TLocalizedText FeatureLocked => null;

		public static TLocalizedText StageEnteringUnavailable => null;

		public static TLocalizedText DecreaseQuantityLarge => null;

		public static TLocalizedText IncreaseQuantityLarge => null;

		public static TLocalizedText LowWeaponDurabilityToast => null;

		public static TLocalizedText NewPlayerJoinCancel => null;

		public static TLocalizedText BoardingCompleted => null;

		public static TLocalizedText ArtifactDealStopByOther => null;

		public static TLocalizedText CoopVersionMismatch => null;

		public static TLocalizedText WeaponProtectPlay => null;

		public static TLocalizedText SecondsLeftFormat => null;

		public static TLocalizedText NameContainsForbiddenWord => null;

		public static TLocalizedText CannotScrapOccupiedBag => null;

		public static TLocalizedText CannotSellOccupiedBag => null;

		public static TLocalizedText EArtifactCategoryBooks => null;

		public static TLocalizedText EArtifactCategoryCoins => null;

		public static TLocalizedText EArtifactCategoryElectronics => null;

		public static TLocalizedText EArtifactCategoryJewels => null;

		public static TLocalizedText EArtifactCategoryManuscripts => null;

		public static TLocalizedText EArtifactCategoryMaps => null;

		public static TLocalizedText EArtifactCategoryMechanics => null;

		public static TLocalizedText EArtifactCategoryMedia => null;

		public static TLocalizedText EArtifactCategoryPaintings => null;

		public static TLocalizedText EArtifactCategoryRelics => null;

		public static TLocalizedText EArtifactCategoryStatues => null;

		public static TLocalizedText EArtifactCategoryNone => null;

		public static TLocalizedText EArtifactCategoryAll => null;

		public static TLocalizedText EEventConditionTypeCampaignCleared => null;

		public static TLocalizedText EEventConditionTypeCharacterCollected => null;

		public static TLocalizedText EEventConditionTypeItemCollected => null;

		public static TLocalizedText EEventConditionTypeMissionCleared => null;

		public static TLocalizedText EEventConditionTypeTechTree => null;

		public static TLocalizedText EEventConditionTypeUserLevel => null;

		public static TLocalizedText EEventConditionTypeArtifactDealFinish => null;

		public static TLocalizedText EEventConditionTypeEmployeeHire => null;

		public static TLocalizedText EEventConditionTypeEmployeeFire => null;

		public static TLocalizedText EEventConditionTypeGainedCoin => null;

		public static TLocalizedText EEventConditionTypeAccountRecord => null;

		public static TLocalizedText EEventConditionTypeAreaDecorationUnlocked => null;

		public static TLocalizedText EEquipmentTypeAccessory => null;

		public static TLocalizedText EEquipmentTypeWeapon => null;

		public static TLocalizedText EEquipmentTypeNone => null;

		public static TLocalizedText EEventConditionTypeMonsterKill => null;

		public static TLocalizedText EEventConditionTypeMyItemCountInInventory => null;

		public static TLocalizedText EEventConditionTypeMyItemCountTotal => null;

		public static TLocalizedText EEventConditionTypeTeamItemCountInInventory => null;

		public static TLocalizedText EEventConditionTypeWaveClear => null;

		public static TLocalizedText EEventConditionTypeItemUse => null;

		public static TLocalizedText EEventConditionTypeGainedWorth => null;

		public static TLocalizedText EEventConditionTypeTeamTotalWorth => null;

		public static TLocalizedText EEventConditionTypeItemUsed => null;

		public static TLocalizedText EEventConditionTypeNone => null;

		public static TLocalizedText EEventConditionTypeOpenRewardBox => null;

		public static TLocalizedText EEventConditionTypeStageEscape => null;

		public static TLocalizedText EEventConditionTypeCampaignClear => null;

		public static TLocalizedText EEventConditionTypeMissionClear => null;

		public static TLocalizedText EEventConditionTypeBlueprintUsed => null;

		public static TLocalizedText EEventConditionTypeLuaProgress => null;

		public static TLocalizedText EEventConditionTypeLoungeQuestCleared => null;

		public static TLocalizedText EEventConditionTypeTalent => null;

		public static TLocalizedText EEventConditionTypeAreaUnlocked => null;

		public static TLocalizedText EEventConditionTypeDecoUnlocked => null;

		public static TLocalizedText EEventConditionTypeFacilityUnlocked => null;

		public static TLocalizedText EPingTypeAlert => null;

		public static TLocalizedText EPingTypeAway => null;

		public static TLocalizedText EPingTypeCancel => null;

		public static TLocalizedText EPingTypeChangeRequest => null;

		public static TLocalizedText EPingTypeGather => null;

		public static TLocalizedText EPingTypeHelpRequest => null;

		public static TLocalizedText EPingTypeHpRecovery => null;

		public static TLocalizedText EPingTypeItem => null;

		public static TLocalizedText EPingTypeManagementContent => null;

		public static TLocalizedText EPingTypeMoveOrder => null;

		public static TLocalizedText EPingTypeNotice => null;

		public static TLocalizedText EPingTypePrepareBattle => null;

		public static TLocalizedText EPingTypeProceedQuest => null;

		public static TLocalizedText EPingTypeReady => null;

		public static TLocalizedText EPingTypeRestRequest => null;

		public static TLocalizedText EPingTypeReturn => null;

		public static TLocalizedText EPingTypeSkill => null;

		public static TLocalizedText EPingTypeStatus => null;

		public static TLocalizedText EPingTypeStressRecovery => null;

		public static TLocalizedText EPingTypeTargetFound => null;

		public static TLocalizedText EPingTypeWait => null;

		public static TLocalizedText EPingTypeWaitRequest => null;

		public static TLocalizedText EPingTypeNone => null;

		public static TLocalizedText EPingResultOk => null;

		public static TLocalizedText EPingResultBlocked => null;

		public static TLocalizedText EPingResultRestrictedArea => null;

		public static TLocalizedText EStatTypeNone => null;

		public static TLocalizedText EStatTypeAp => null;

		public static TLocalizedText EStatTypeApDef => null;

		public static TLocalizedText EStatTypeApDefPercent => null;

		public static TLocalizedText EStatTypeApPercent => null;

		public static TLocalizedText EStatTypeAtk => null;

		public static TLocalizedText EStatTypeAtkPercent => null;

		public static TLocalizedText EStatTypeAtkSpeed => null;

		public static TLocalizedText EStatTypeAtkSpeedPercent => null;

		public static TLocalizedText EStatTypeDef => null;

		public static TLocalizedText EStatTypeDefPercent => null;

		public static TLocalizedText EStatTypeHp => null;

		public static TLocalizedText EStatTypeHpPercent => null;

		public static TLocalizedText EStatTypeHpMax => null;

		public static TLocalizedText EStatTypeHpMaxPercent => null;

		public static TLocalizedText EStatTypeHpRegen => null;

		public static TLocalizedText EStatTypeHpRegenPercent => null;

		public static TLocalizedText EStatTypeMaxWeight => null;

		public static TLocalizedText EStatTypeMoveSpeed => null;

		public static TLocalizedText EStatTypeMoveSpeedPercent => null;

		public static TLocalizedText EStatTypeMp => null;

		public static TLocalizedText EStatTypeMpPercent => null;

		public static TLocalizedText EStatTypeMpRegen => null;

		public static TLocalizedText EStatTypeMpRegenPercent => null;

		public static TLocalizedText EStatTypeSp => null;

		public static TLocalizedText EStatTypeSpPercent => null;

		public static TLocalizedText EStatTypeSpRegen => null;

		public static TLocalizedText EStatTypeSpRegenPercent => null;

		public static TLocalizedText EStatTypeStaminaMax => null;

		public static TLocalizedText EStatTypeStaminaRegen => null;

		public static TLocalizedText EStatTypeRunStaminaCost => null;

		public static TLocalizedText EStatTypeRunStaminaCostPercent => null;

		public static TLocalizedText EStatTypeRunIncreaseSpeed => null;

		public static TLocalizedText EStatTypeRunIncreaseSpeedPercent => null;

		public static TLocalizedText EStatTypeStressIntervalPercent => null;

		public static TLocalizedText EStatTypePenetration => null;

		public static TLocalizedText EStatTypePenetrationPercent => null;

		public static TLocalizedText EStatTypeCriticalChancePercent => null;

		public static TLocalizedText EStatTypeCriticalTakenChancePercent => null;

		public static TLocalizedText EStatTypeCriticalDamagePercent => null;

		public static TLocalizedText EStatTypeWeaknessDamagePercent => null;

		public static TLocalizedText EStatTypeCooldownReductionPercent => null;

		public static TLocalizedText EStatTypeAdditionalAttackDamagePercent => null;

		public static TLocalizedText EStatTypeReductionDamagePercent => null;

		public static TLocalizedText EStatTypeBackAttackDamagePercent => null;

		public static TLocalizedText EStatTypeFireResistancePercent => null;

		public static TLocalizedText EStatTypeWaterResistancePercent => null;

		public static TLocalizedText EStatTypeWindResistancePercent => null;

		public static TLocalizedText EStatTypeCorruptionMax => null;

		public static TLocalizedText EStatTypeDashDistancePercent => null;

		public static TLocalizedText EStatTypeFireDamagePercent => null;

		public static TLocalizedText EStatTypeWaterDamagePercent => null;

		public static TLocalizedText EStatTypeWindDamagePercent => null;

		public static TLocalizedText EStatTypeMax => null;

		public static TLocalizedText ETechTreeTypeGuestCount => null;

		public static TLocalizedText ETechTreeTypeInventorySlotCount => null;

		public static TLocalizedText ETechTreeTypeItemCraftingBonus => null;

		public static TLocalizedText ETechTreeTypeItemCraftingUnlock => null;

		public static TLocalizedText ETechTreeTypeLightFuelEfficiency => null;

		public static TLocalizedText ETechTreeTypeRecoveryItemCount => null;

		public static TLocalizedText ETechTreeTypeShopProductUnlock => null;

		public static TLocalizedText ETechTreeTypeSkillSlotCount => null;

		public static TLocalizedText ETechTreeTypeStashSlotCount => null;

		public static TLocalizedText ETechTreeTypeSafeInventorySlotCount => null;

		public static TLocalizedText ETechTreeTypeCorruptionMaxBonus => null;

		public static TLocalizedText ETechTreeTypeNone => null;

		public static TLocalizedText EThemeTypeCity => null;

		public static TLocalizedText EThemeTypeCountry => null;

		public static TLocalizedText EThemeTypeTown => null;

		public static TLocalizedText EThemeTypeTown2 => null;

		public static TLocalizedText EThemeTypeStation => null;

		public static TLocalizedText EThemeTypeNone => null;

		public static TLocalizedText EThemeTypeLounge => null;

		public static TLocalizedText EThemeTypeEtc => null;

		public static TLocalizedText EFrameRateFrame15 => null;

		public static TLocalizedText EFrameRateFrame30 => null;

		public static TLocalizedText EFrameRateFrame60 => null;

		public static TLocalizedText EFrameRateFrame144 => null;

		public static TLocalizedText EFrameRateFrameNoLimit => null;

		public static TLocalizedText EQualityLevelLow => null;

		public static TLocalizedText EQualityLevelMedium => null;

		public static TLocalizedText EQualityLevelHigh => null;

		public static TLocalizedText EQualityLevelUltra => null;

		public static TLocalizedText EScreenModeWindowed1280X720 => null;

		public static TLocalizedText EScreenModeWindowed1920X1080 => null;

		public static TLocalizedText EScreenModeWindowedFullscreen => null;

		public static TLocalizedText EScreenModeExclusiveFullScreen => null;

		public static TLocalizedText ELanguageNone => null;

		public static TLocalizedText ELanguageKo => null;

		public static TLocalizedText ELanguageEn => null;

		public static TLocalizedText ELanguageJp => null;

		public static TLocalizedText ELanguageTw => null;

		public static TLocalizedText ELanguageCn => null;

		public static TLocalizedText ELanguageRu => null;

		public static TLocalizedText ELanguageBr => null;

		public static TLocalizedText ELanguageDe => null;

		public static TLocalizedText ELanguageFr => null;

		public static TLocalizedText ELanguageEs => null;

		public static TLocalizedText ELanguageIt => null;

		public static TLocalizedText ELanguagePt => null;

		public static TLocalizedText ELanguagePl => null;

		public static TLocalizedText ELanguageVi => null;

		public static TLocalizedText ELanguageTr => null;

		public static TLocalizedText EDifficultyEasy => null;

		public static TLocalizedText EDifficultyNormal => null;

		public static TLocalizedText EDifficultyHard => null;

		public static TLocalizedText EDifficultyInsane => null;

		public static TLocalizedText ERoomDifficultyTitle => null;

		public static TLocalizedText ERoomDifficultyStory => null;

		public static TLocalizedText ERoomDifficultyJunior => null;

		public static TLocalizedText ERoomDifficultyClassic => null;

		public static TLocalizedText ERoomDifficultyVeteran => null;

		public static TLocalizedText ERoomDifficultyStoryDesc => null;

		public static TLocalizedText ERoomDifficultyJuniorDesc => null;

		public static TLocalizedText ERoomDifficultyClassicDesc => null;

		public static TLocalizedText ERoomDifficultyVeteranDesc => null;

		public static TLocalizedText ERoomDifficultyNotice => null;

		public static TLocalizedText ERoomPingFilterTypeExcellentOnly => null;

		public static TLocalizedText ERoomPingFilterTypeNormal => null;

		public static TLocalizedText ERoomPingFilterTypeAll => null;

		public static TLocalizedText ERoomPublishTypePrivate => null;

		public static TLocalizedText ERoomPublishTypeFriendsOnly => null;

		public static TLocalizedText ERoomPublishTypePublic => null;

		public static TLocalizedText ESightTagNone => null;

		public static TLocalizedText ESightTagNarrow => null;

		public static TLocalizedText ESightTagNormal => null;

		public static TLocalizedText ESightTagWide => null;

		public static TLocalizedText ESpeedTagNone => null;

		public static TLocalizedText ESpeedTagSlow => null;

		public static TLocalizedText ESpeedTagNormal => null;

		public static TLocalizedText ESpeedTagFast => null;

		public static TLocalizedText EGoodsSubTypeNone => null;

		public static TLocalizedText EGoodsSubTypeConsumable => null;

		public static TLocalizedText EGoodsSubTypeMisc => null;

		public static TLocalizedText EGoodsSubTypeNote => null;

		public static TLocalizedText EGoodsSubTypeBlueprint => null;

		public static TLocalizedText EGoodsSubTypeWeapon => null;

		public static TLocalizedText EGoodsSubTypeAccessory => null;

		public static TLocalizedText EGoodsSubTypeArtifact => null;

		public static TLocalizedText EGoodsTypeNone => null;

		public static TLocalizedText EGoodsTypeGold => null;

		public static TLocalizedText EGoodsTypeExp => null;

		public static TLocalizedText EGoodsTypeItem => null;

		public static TLocalizedText EGoodsTypeEquipment => null;

		public static TLocalizedText EGoodsTypeCoin => null;

		public static TLocalizedText EItemCategoryNone => null;

		public static TLocalizedText EItemCategoryQuest => null;

		public static TLocalizedText EItemCategoryMission => null;

		public static TLocalizedText EItemCategoryCraft => null;

		public static TLocalizedText EMonsterCategoryNone => null;

		public static TLocalizedText ENpcFunctionTypeNone => null;

		public static TLocalizedText ENpcFunctionTypeCharacterSelect => null;

		public static TLocalizedText ENpcFunctionTypeCharacterSkill => null;

		public static TLocalizedText ENpcFunctionTypeCharacterTalent => null;

		public static TLocalizedText ENpcFunctionTypeCampaign => null;

		public static TLocalizedText ENpcFunctionTypeStash => null;

		public static TLocalizedText ENpcFunctionTypeStorage => null;

		public static TLocalizedText ENpcFunctionTypeLostGoodsStorage => null;

		public static TLocalizedText ENpcFunctionTypeCrafting => null;

		public static TLocalizedText ENpcFunctionTypeShop => null;

		public static TLocalizedText ENpcFunctionTypeArtifactShop => null;

		public static TLocalizedText ENpcFunctionTypeTechTree => null;

		public static TLocalizedText ENpcFunctionTypeLevelUp => null;

		public static TLocalizedText ENpcFunctionTypeRest => null;

		public static TLocalizedText ENpcFunctionTypeParadoxPurify => null;

		public static TLocalizedText ENpcFunctionTypeParadoxLock => null;

		public static TLocalizedText ENpcFunctionTypeDangerLevel => null;

		public static TLocalizedText ENpcFunctionTypeMultiplayCreate => null;

		public static TLocalizedText ENpcFunctionTypeMultiplayJoin => null;

		public static TLocalizedText ENpcFunctionTypeMultiplayLeave => null;

		public static TLocalizedText ENpcFunctionTypeDialog => null;

		public static TLocalizedText ENpcFunctionTypeArtifactDeal => null;

		public static TLocalizedText ENpcFunctionTypeTalent => null;

		public static TLocalizedText ENpcFunctionTypeEquipmentRepair => null;

		public static TLocalizedText ENpcFunctionTypeTycoon => null;

		public static TLocalizedText ENpcFunctionTypeTycoonCustomer => null;

		public static TLocalizedText ENpcFunctionTypeInterior => null;

		public static TLocalizedText ENpcFunctionTypeScrap => null;

		public static TLocalizedText ENpcFunctionTypeRegisterItemUsed => null;

		public static TLocalizedText ENpcFunctionTypeCoinExchange => null;

		public static TLocalizedText ENpcFunctionTypeTycoonSalesSlot => null;

		public static TLocalizedText ENpcFunctionTypeTycoonEmployee => null;

		public static TLocalizedText ENpcFunctionTypeLoungeBgm => null;

		public static TLocalizedText EArchiveCategoryNone => null;

		public static TLocalizedText EArchiveCategoryQuest => null;

		public static TLocalizedText EArchiveCategoryMonster => null;

		public static TLocalizedText EArchiveCategoryItem => null;

		public static TLocalizedText EArchiveCategoryWeapon => null;

		public static TLocalizedText EArchiveCategoryAccessory => null;

		public static TLocalizedText EArchiveCategoryArtifact => null;

		public static TLocalizedText EArchiveCategoryRecord => null;

		public static TLocalizedText EArchiveCategoryGlossary => null;

		public static TLocalizedText EArchiveRecordCategoryNone => null;

		public static TLocalizedText EArchiveRecordCategoryNote => null;

		public static TLocalizedText EGlossaryCategoryNone => null;

		public static TLocalizedText EGlossaryCategoryNpcAndFaction => null;

		public static TLocalizedText EGlossaryCategoryStat => null;

		public static TLocalizedText EGlossaryCategoryCombatAndEffect => null;

		public static TLocalizedText EGlossaryCategorySystem => null;

		public static TLocalizedText EQuestTypeNone => null;

		public static TLocalizedText EQuestTypeCampaign => null;

		public static TLocalizedText EQuestTypeMission => null;

		public static TLocalizedText EQuestTypeLounge => null;

		public static TLocalizedText EWeaponTypeNone => null;

		public static TLocalizedText EWeaponTypeSword => null;

		public static TLocalizedText EWeaponTypePhone => null;

		public static TLocalizedText EWeaponTypeShotgun => null;

		public static TLocalizedText EWeaponTypeRevolver => null;

		public static TLocalizedText EArchiveCategoryNoneDesc => null;

		public static TLocalizedText EArchiveCategoryQuestDesc => null;

		public static TLocalizedText EArchiveCategoryMonsterDesc => null;

		public static TLocalizedText EArchiveCategoryItemDesc => null;

		public static TLocalizedText EArchiveCategoryWeaponDesc => null;

		public static TLocalizedText EArchiveCategoryAccessoryDesc => null;

		public static TLocalizedText EArchiveCategoryArtifactDesc => null;

		public static TLocalizedText EArchiveCategoryRecordDesc => null;

		public static TLocalizedText EArchiveCategoryGlossaryDesc => null;

		public static TLocalizedText EGoodsGradeTypeNone => null;

		public static TLocalizedText EGoodsGradeTypeNormal => null;

		public static TLocalizedText EGoodsGradeTypeRare => null;

		public static TLocalizedText EGoodsGradeTypeElite => null;

		public static TLocalizedText EGoodsGradeTypeEpic => null;

		public static TLocalizedText EGoodsGradeTypeLegend => null;

		public static TLocalizedText EGoodsGradeTypeUnique => null;

		public static TLocalizedText EArtifactDealCardEffectTypeNone => null;

		public static TLocalizedText EArtifactDealCardEffectTypeIncreasePricePercent => null;

		public static TLocalizedText EArtifactDealCardEffectTypeDecreasePricePercent => null;

		public static TLocalizedText EArtifactDealCardEffectTypeAddPreferTags => null;

		public static TLocalizedText EArtifactDealCardEffectTypeRerollPreferTags => null;

		public static TLocalizedText EArtifactDealCardEffectTypeRerollDislikeTags => null;

		public static TLocalizedText EArtifactDealCardEffectTypeRemoveDislikeTags => null;

		public static TLocalizedText EArtifactDealCardEffectTypeAddSpecificPreferTag => null;

		public static TLocalizedText EArtifactDealCardEffectTypeIncreaseBonusSuccessPercent => null;

		public static TLocalizedText EInteriorTypeNone => null;

		public static TLocalizedText EInteriorTypeArea => null;

		public static TLocalizedText EInteriorTypeAreaDecoration => null;

		public static TLocalizedText EInteriorTypeNpcLevel => null;

		public static TLocalizedText ETalentEffectTypeNone => null;

		public static TLocalizedText ETalentEffectTypeInventorySlotCount => null;

		public static TLocalizedText ETalentEffectTypeSafeInventorySlotCount => null;

		public static TLocalizedText ETalentEffectTypeStashSlotCount => null;

		public static TLocalizedText ETalentEffectTypeSkillSlotCount => null;

		public static TLocalizedText ETalentEffectTypeRecoveryItemCount => null;

		public static TLocalizedText ETalentEffectTypeLightFuelEfficiency => null;

		public static TLocalizedText ETalentEffectTypeGuestCount => null;

		public static TLocalizedText ETalentEffectTypeCorruptionMaxBonus => null;

		public static TLocalizedText ETalentEffectTypeShopProductUnlock => null;

		public static TLocalizedText ETalentEffectTypeItemCraftingUnlock => null;

		public static TLocalizedText ETalentEffectTypeAddSkill => null;

		public static TLocalizedText ETalentEffectTypeUpgradeSkill => null;

		public static TLocalizedText ETalentEffectTypeUnlockSkillMode => null;

		public static TLocalizedText ETalentEffectTypeChangeSkill => null;

		public static TLocalizedText ETalentEffectTypeShopProductGradeAppearBonus => null;

		public static TLocalizedText EAnomalyTypeNone => null;

		public static TLocalizedText EAnomalyTypeNoneDesc => null;

		public static TLocalizedText EAnomalyTypeDeepSeaRain => null;

		public static TLocalizedText EAnomalyTypeDeepSeaRainDesc => null;

		public static TLocalizedText EAnomalyTypeYellowStorm => null;

		public static TLocalizedText EAnomalyTypeYellowStormDesc => null;

		public static TLocalizedText EAnomalyTypeBurningVoid => null;

		public static TLocalizedText EAnomalyTypeBurningVoidDesc => null;

		public static TLocalizedText EAnomalyTypeFrozenMist => null;

		public static TLocalizedText EAnomalyTypeFrozenMistDesc => null;

		public static TLocalizedText EAimCorrectionLevelNone => null;

		public static TLocalizedText EAimCorrectionLevelLow => null;

		public static TLocalizedText EAimCorrectionLevelMedium => null;

		public static TLocalizedText EAimCorrectionLevelHigh => null;

		public static TLocalizedText ELoadResultNotFound => null;

		public static TLocalizedText ELoadResultCorrupted => null;

		public static TLocalizedText ELoadResultSuccess => null;

		public static TLocalizedText ELoadResultFailed => null;

		public static TLocalizedText ELoadResultUserMismatch => null;

		public static TLocalizedText ELoadResultVersionMismatch => null;

		public static TLocalizedText EStatusEffectTagNone => null;

		public static TLocalizedText EStatusEffectTagBleed => null;

		public static TLocalizedText EStatusEffectTagBurn => null;

		public static TLocalizedText EStatusEffectTagBlind => null;

		public static TLocalizedText EStatusEffectTagRoot => null;

		public static TLocalizedText EStatusEffectTagLight => null;

		public static TLocalizedText EStatusEffectTagSilence => null;

		public static TLocalizedText EStatusEffectTagSlow => null;

		public static TLocalizedText EStatusEffectTagWeaken => null;

		public static TLocalizedText EStatusEffectTagDefReduction => null;

		public static TLocalizedText EStatusEffectTagConfuse => null;

		public static TLocalizedText EStatusEffectTagPoison => null;

		public static TLocalizedText EStatusEffectTagMadness => null;

		public static TLocalizedText EStatusEffectTagStun => null;

		public static TLocalizedText EStatusEffectTagKnockback => null;

		public static TLocalizedText EStatusEffectTagPull => null;

		public static TLocalizedText EStatusEffectTagFreeze => null;

		public static TLocalizedText EStatusEffectTagFirePower => null;

		public static TLocalizedText EStatusEffectTagAll => null;

		public static TLocalizedText ETycoonFailTypeArtifactDamage => null;

		public static TLocalizedText ETycoonFailTypeCheapSale => null;

		public static TLocalizedText ETycoonFailTypeSlotDamage => null;

		public static TLocalizedText ETycoonFailTypeLoafing => null;

		public static TLocalizedText ETycoonFailTypeArtifactDamageDesc => null;

		public static TLocalizedText ETycoonFailTypeCheapSaleDesc => null;

		public static TLocalizedText ETycoonFailTypeSlotDamageDesc => null;

		public static TLocalizedText ETycoonFailTypeLoafingDesc => null;

		public static TLocalizedText EGoodsCategoryNone => null;

		public static TLocalizedText EGoodsCategoryCurrency => null;

		public static TLocalizedText EGoodsCategoryItem => null;

		public static TLocalizedText EGoodsCategoryEquipment => null;

		public static TLocalizedText EGoodsDataTypeNone => null;

		public static TLocalizedText EGoodsDataTypeGold => null;

		public static TLocalizedText EGoodsDataTypeCoin => null;

		public static TLocalizedText EGoodsDataTypeExp => null;

		public static TLocalizedText EGoodsDataTypeItem => null;

		public static TLocalizedText EGoodsDataTypeEquipment => null;

		public static TLocalizedText EGoodsTypeConsumable => null;

		public static TLocalizedText EGoodsTypeValuable => null;

		public static TLocalizedText EGoodsTypeMisc => null;

		public static TLocalizedText EGoodsTypeNote => null;

		public static TLocalizedText EGoodsTypeBlueprint => null;

		public static TLocalizedText EGoodsTypeWeapon => null;

		public static TLocalizedText EGoodsTypeAccessory => null;

		public static TLocalizedText EGoodsTypeArtifact => null;

		public static TLocalizedText EResolutionNative => null;

		public static TLocalizedText EResolution1280X720 => null;

		public static TLocalizedText EResolution1360X768 => null;

		public static TLocalizedText EResolution1366X768 => null;

		public static TLocalizedText EResolution1600X900 => null;

		public static TLocalizedText EResolution1920X1080 => null;

		public static TLocalizedText EResolution2560X1440 => null;

		public static TLocalizedText EScreenModeWindowed => null;

		public static TLocalizedText EScreenModeBorderlessWindowed => null;

		public static TLocalizedText EScreenModeFullscreen => null;

		public static TLocalizedText EEventConditionTypeNpcLevel => null;

		public static TLocalizedText EBagTypeNone => null;

		public static TLocalizedText EBagTypeMaterial => null;

		public static TLocalizedText EBagTypeKey => null;

		public static TLocalizedText EBagTypeArtifact => null;

		public static TLocalizedText EGoodsCategoryBag => null;

		public static TLocalizedText EGoodsDataTypeBag => null;

		public static TLocalizedText EGoodsTypeBag => null;

		public static TLocalizedText EGoodsTypeMusicDisc => null;

		public static TLocalizedText ESaveResultSuccess => null;

		public static TLocalizedText ESaveResultIoFailure => null;

		public static TLocalizedText ESaveResultFailed => null;

		public static TLocalizedText ECreateLobbyResultSuccess => null;

		public static TLocalizedText ECreateLobbyResultNotInitialized => null;

		public static TLocalizedText ECreateLobbyResultAlreadyJoined => null;

		public static TLocalizedText ECreateLobbyResultTimeout => null;

		public static TLocalizedText ECreateLobbyResultFailed => null;

		public static TLocalizedText Lq1100101 => null;

		public static TLocalizedText Lq1100102 => null;

		public static TLocalizedText Lq1100103 => null;

		public static TLocalizedText Lq1100104 => null;

		public static TLocalizedText Lq1100105 => null;

		public static TLocalizedText Lq1100106 => null;

		public static TLocalizedText Lq1100107 => null;

		public static TLocalizedText Lq1100108 => null;

		public static TLocalizedText Lq1100109 => null;

		public static TLocalizedText Lq1100110 => null;

		public static TLocalizedText Lq1100111 => null;

		public static TLocalizedText Lq1100112 => null;

		public static TLocalizedText Lq1100113 => null;

		public static TLocalizedText Lq1100114 => null;

		public static TLocalizedText Lq1100115 => null;

		public static TLocalizedText Lq1100116 => null;

		public static TLocalizedText Lq1100117 => null;

		public static TLocalizedText Lq1100118 => null;

		public static TLocalizedText Lq1100119 => null;

		public static TLocalizedText Lq1100120 => null;

		public static TLocalizedText Lq1100121 => null;

		public static TLocalizedText Lq1100122 => null;

		public static TLocalizedText Lq1100123 => null;

		public static TLocalizedText Lq1100124 => null;

		public static TLocalizedText Lq1100125 => null;

		public static TLocalizedText Lq1100126 => null;

		public static TLocalizedText Lq1100127 => null;

		public static TLocalizedText Lq1100128 => null;

		public static TLocalizedText Lq1100129 => null;

		public static TLocalizedText Lq1100130 => null;

		public static TLocalizedText Lq1100131 => null;

		public static TLocalizedText Lq1100132 => null;

		public static TLocalizedText Lq1100133 => null;

		public static TLocalizedText Lq1100134 => null;

		public static TLocalizedText Lq1100135 => null;

		public static TLocalizedText Lq1100201 => null;

		public static TLocalizedText Lq1100202 => null;

		public static TLocalizedText Lq1100203 => null;

		public static TLocalizedText Lq1100204 => null;

		public static TLocalizedText Lq1100205 => null;

		public static TLocalizedText Lq1100206 => null;

		public static TLocalizedText Lq1100207 => null;

		public static TLocalizedText Lq1100208 => null;

		public static TLocalizedText Lq1100209 => null;

		public static TLocalizedText Lq1100210 => null;

		public static TLocalizedText Lq1100211 => null;

		public static TLocalizedText Lq1100212 => null;

		public static TLocalizedText Lq1100213 => null;

		public static TLocalizedText Lq1100214 => null;

		public static TLocalizedText Lq1100215 => null;

		public static TLocalizedText Lq1100216 => null;

		public static TLocalizedText Lq1100217 => null;

		public static TLocalizedText Lq1100218 => null;

		public static TLocalizedText Lq1100301 => null;

		public static TLocalizedText Lq1100302 => null;

		public static TLocalizedText Lq1100303 => null;

		public static TLocalizedText Lq1100304 => null;

		public static TLocalizedText Lq1100305 => null;

		public static TLocalizedText Lq1100306 => null;

		public static TLocalizedText Lq1100307 => null;

		public static TLocalizedText Lq1100308 => null;

		public static TLocalizedText Lq1100309 => null;

		public static TLocalizedText Lq1100310 => null;

		public static TLocalizedText Lq1100311 => null;

		public static TLocalizedText Lq1100312 => null;

		public static TLocalizedText Lq1100313 => null;

		public static TLocalizedText Lq1100314 => null;

		public static TLocalizedText Lq1100401 => null;

		public static TLocalizedText Lq1100402 => null;

		public static TLocalizedText Lq1100403 => null;

		public static TLocalizedText Lq1100404 => null;

		public static TLocalizedText Lq1100501 => null;

		public static TLocalizedText Lq1100502 => null;

		public static TLocalizedText Lq1100503 => null;

		public static TLocalizedText Lq1100504 => null;

		public static TLocalizedText Lq1100505 => null;

		public static TLocalizedText Lq1100506 => null;

		public static TLocalizedText Lq1100507 => null;

		public static TLocalizedText Lq1100508 => null;

		public static TLocalizedText Lq1100509 => null;

		public static TLocalizedText Lq1100510 => null;

		public static TLocalizedText Lq1100511 => null;

		public static TLocalizedText Lq1100512 => null;

		public static TLocalizedText Lq1100513 => null;

		public static TLocalizedText Lq1100514 => null;

		public static TLocalizedText Lq1100515 => null;

		public static TLocalizedText Lq1100516 => null;

		public static TLocalizedText Lq1100517 => null;

		public static TLocalizedText Lq1100518 => null;

		public static TLocalizedText Lq1100519 => null;

		public static TLocalizedText Lq1100520 => null;

		public static TLocalizedText Lq1100521 => null;

		public static TLocalizedText Lq1100522 => null;

		public static TLocalizedText Lq1100523 => null;

		public static TLocalizedText Lq1100524 => null;

		public static TLocalizedText Lq1100525 => null;

		public static TLocalizedText Lq1100526 => null;

		public static TLocalizedText Lq1100527 => null;

		public static TLocalizedText Lq1100528 => null;

		public static TLocalizedText Lq1100529 => null;

		public static TLocalizedText Lq1100530 => null;

		public static TLocalizedText Lq1100531 => null;

		public static TLocalizedText Lq11009101 => null;

		public static TLocalizedText Lq11009102 => null;

		public static TLocalizedText Lq11009103 => null;

		public static TLocalizedText Lq11009104 => null;

		public static TLocalizedText Lq11009105 => null;

		public static TLocalizedText Lq11009106 => null;

		public static TLocalizedText Lq11009107 => null;

		public static TLocalizedText LCommon999 => null;

		public static TLocalizedText Lq11009201 => null;

		public static TLocalizedText Lq11009301 => null;

		public static TLocalizedText Lq11009302 => null;

		public static TLocalizedText Lq11009303 => null;

		public static TLocalizedText Lq11009401 => null;

		public static TLocalizedText Lq11009501 => null;

		public static TLocalizedText Lq11009601 => null;

		public static TLocalizedText Lq11009701 => null;

		public static TLocalizedText Lq11009801 => null;

		public static TLocalizedText Lq11009802 => null;

		public static TLocalizedText Lq11009803 => null;

		public static TLocalizedText Lq11009901 => null;

		public static TLocalizedText Lq110010001 => null;

		public static TLocalizedText Lq1101101 => null;

		public static TLocalizedText Lq1101102 => null;

		public static TLocalizedText Lq1101103 => null;

		public static TLocalizedText Lq1101104 => null;

		public static TLocalizedText Lq1101105 => null;

		public static TLocalizedText Lq1101106 => null;

		public static TLocalizedText Lq1101107 => null;

		public static TLocalizedText Lq1101108 => null;

		public static TLocalizedText Lq1101109 => null;

		public static TLocalizedText Lq1101201 => null;

		public static TLocalizedText Lq1101202 => null;

		public static TLocalizedText Lq1101203 => null;

		public static TLocalizedText Lq11019101 => null;

		public static TLocalizedText Lq11019102 => null;

		public static TLocalizedText Lq11019103 => null;

		public static TLocalizedText Lq11019104 => null;

		public static TLocalizedText Lq11019201 => null;

		public static TLocalizedText Lq11019202 => null;

		public static TLocalizedText Lq11019301 => null;

		public static TLocalizedText Lq11019302 => null;

		public static TLocalizedText Lq11019401 => null;

		public static TLocalizedText Lq11019402 => null;

		public static TLocalizedText Lq11019403 => null;

		public static TLocalizedText Lq1102101 => null;

		public static TLocalizedText Lq1102102 => null;

		public static TLocalizedText Lq1102103 => null;

		public static TLocalizedText Lq1102201 => null;

		public static TLocalizedText Lq1102202 => null;

		public static TLocalizedText Lq1102203 => null;

		public static TLocalizedText Lq1102204 => null;

		public static TLocalizedText Lq1102205 => null;

		public static TLocalizedText Lq1102206 => null;

		public static TLocalizedText Lq1102207 => null;

		public static TLocalizedText Lq1102208 => null;

		public static TLocalizedText Lq1102209 => null;

		public static TLocalizedText Lq1102210 => null;

		public static TLocalizedText Lq1102211 => null;

		public static TLocalizedText Lq1102212 => null;

		public static TLocalizedText Lq1102213 => null;

		public static TLocalizedText Lq1102214 => null;

		public static TLocalizedText Lq1102215 => null;

		public static TLocalizedText Lq1102216 => null;

		public static TLocalizedText Lq1102217 => null;

		public static TLocalizedText Lq1102218 => null;

		public static TLocalizedText Lq1102219 => null;

		public static TLocalizedText Lq1102220 => null;

		public static TLocalizedText Lq1102221 => null;

		public static TLocalizedText Lq11029101 => null;

		public static TLocalizedText Lq11029201 => null;

		public static TLocalizedText Lq11029301 => null;

		public static TLocalizedText Lq11029302 => null;

		public static TLocalizedText Lq11029303 => null;

		public static TLocalizedText Lq11029304 => null;

		public static TLocalizedText Lq11029305 => null;

		public static TLocalizedText Lq1103101 => null;

		public static TLocalizedText Lq1103102 => null;

		public static TLocalizedText Lq1103103 => null;

		public static TLocalizedText Lq1103104 => null;

		public static TLocalizedText Lq1103105 => null;

		public static TLocalizedText Lq1103106 => null;

		public static TLocalizedText Lq1103201 => null;

		public static TLocalizedText Lq1103202 => null;

		public static TLocalizedText Lq1103203 => null;

		public static TLocalizedText Lq1103204 => null;

		public static TLocalizedText Lq1103205 => null;

		public static TLocalizedText Lq1103206 => null;

		public static TLocalizedText Lq1103207 => null;

		public static TLocalizedText Lq1103208 => null;

		public static TLocalizedText Lq1103209 => null;

		public static TLocalizedText Lq1103210 => null;

		public static TLocalizedText Lq1103211 => null;

		public static TLocalizedText Lq1103212 => null;

		public static TLocalizedText Lq1103213 => null;

		public static TLocalizedText Lq1103214 => null;

		public static TLocalizedText Lq1103215 => null;

		public static TLocalizedText Lq1103216 => null;

		public static TLocalizedText Lq1103217 => null;

		public static TLocalizedText Lq1103218 => null;

		public static TLocalizedText Lq1103219 => null;

		public static TLocalizedText Lq1103220 => null;

		public static TLocalizedText Lq1103221 => null;

		public static TLocalizedText Lq1103222 => null;

		public static TLocalizedText Lq1103223 => null;

		public static TLocalizedText Lq1103224 => null;

		public static TLocalizedText Lq1103225 => null;

		public static TLocalizedText Lq1103226 => null;

		public static TLocalizedText Lq11039101 => null;

		public static TLocalizedText Lq11039102 => null;

		public static TLocalizedText Lq11039103 => null;

		public static TLocalizedText Lq11039104 => null;

		public static TLocalizedText Lq11039105 => null;

		public static TLocalizedText Lq11039201 => null;

		public static TLocalizedText Lq11039301 => null;

		public static TLocalizedText Lq11039401 => null;

		public static TLocalizedText Lq1104101 => null;

		public static TLocalizedText Lq1104102 => null;

		public static TLocalizedText Lq1104103 => null;

		public static TLocalizedText Lq1104104 => null;

		public static TLocalizedText Lq1104201 => null;

		public static TLocalizedText Lq1104202 => null;

		public static TLocalizedText Lq1104203 => null;

		public static TLocalizedText Lq1104204 => null;

		public static TLocalizedText Lq1104205 => null;

		public static TLocalizedText Lq1104206 => null;

		public static TLocalizedText Lq1104207 => null;

		public static TLocalizedText Lq1104208 => null;

		public static TLocalizedText Lq1104209 => null;

		public static TLocalizedText Lq1104210 => null;

		public static TLocalizedText Lq1104211 => null;

		public static TLocalizedText Lq1104212 => null;

		public static TLocalizedText Lq1104213 => null;

		public static TLocalizedText Lq1104214 => null;

		public static TLocalizedText Lq1104215 => null;

		public static TLocalizedText Lq1104216 => null;

		public static TLocalizedText Lq1104217 => null;

		public static TLocalizedText Lq1104218 => null;

		public static TLocalizedText Lq1104219 => null;

		public static TLocalizedText Lq1104220 => null;

		public static TLocalizedText Lq1104221 => null;

		public static TLocalizedText Lq1104222 => null;

		public static TLocalizedText Lq1104223 => null;

		public static TLocalizedText Lq11049101 => null;

		public static TLocalizedText Lq11049201 => null;

		public static TLocalizedText Lq11049301 => null;

		public static TLocalizedText Lq11049401 => null;

		public static TLocalizedText Lq11049402 => null;

		public static TLocalizedText Lq11049403 => null;

		public static TLocalizedText Lq1105101 => null;

		public static TLocalizedText Lq1105102 => null;

		public static TLocalizedText Lq1105103 => null;

		public static TLocalizedText Lq1105104 => null;

		public static TLocalizedText Lq1105201 => null;

		public static TLocalizedText Lq1105301 => null;

		public static TLocalizedText Lq1105302 => null;

		public static TLocalizedText Lq1105303 => null;

		public static TLocalizedText Lq1105304 => null;

		public static TLocalizedText Lq1105305 => null;

		public static TLocalizedText Lq1105306 => null;

		public static TLocalizedText Lq1105307 => null;

		public static TLocalizedText Lq1105308 => null;

		public static TLocalizedText Lq1105309 => null;

		public static TLocalizedText Lq1105310 => null;

		public static TLocalizedText Lq1105311 => null;

		public static TLocalizedText Lq1105312 => null;

		public static TLocalizedText Lq11059101 => null;

		public static TLocalizedText Lq11059201 => null;

		public static TLocalizedText Lq11059202 => null;

		public static TLocalizedText Lq11059203 => null;

		public static TLocalizedText Lq1201101 => null;

		public static TLocalizedText Lq1201102 => null;

		public static TLocalizedText Lq1201201 => null;

		public static TLocalizedText Lq1201202 => null;

		public static TLocalizedText Lq1201203 => null;

		public static TLocalizedText Lq1201204 => null;

		public static TLocalizedText Lq1201205 => null;

		public static TLocalizedText Lq1201206 => null;

		public static TLocalizedText Lq1201207 => null;

		public static TLocalizedText Lq1201208 => null;

		public static TLocalizedText Lq1201209 => null;

		public static TLocalizedText Lq1201210 => null;

		public static TLocalizedText Lq1201211 => null;

		public static TLocalizedText Lq1201212 => null;

		public static TLocalizedText Lq12019101 => null;

		public static TLocalizedText Lq12019102 => null;

		public static TLocalizedText Lq12019201 => null;

		public static TLocalizedText Lq12019301 => null;

		public static TLocalizedText Lq12019401 => null;

		public static TLocalizedText Lq12019402 => null;

		public static TLocalizedText Lq12019501 => null;

		public static TLocalizedText Lq1202101 => null;

		public static TLocalizedText Lq1202102 => null;

		public static TLocalizedText Lq1202103 => null;

		public static TLocalizedText Lq1202201 => null;

		public static TLocalizedText Lq1202202 => null;

		public static TLocalizedText Lq1202203 => null;

		public static TLocalizedText Lq1202204 => null;

		public static TLocalizedText Lq1202205 => null;

		public static TLocalizedText Lq1202206 => null;

		public static TLocalizedText Lq1202207 => null;

		public static TLocalizedText Lq1202208 => null;

		public static TLocalizedText Lq1202301 => null;

		public static TLocalizedText Lq1202302 => null;

		public static TLocalizedText Lq1202303 => null;

		public static TLocalizedText Lq1202304 => null;

		public static TLocalizedText Lq1202305 => null;

		public static TLocalizedText Lq1202306 => null;

		public static TLocalizedText Lq1202307 => null;

		public static TLocalizedText Lq1202308 => null;

		public static TLocalizedText Lq1202309 => null;

		public static TLocalizedText Lq1202310 => null;

		public static TLocalizedText Lq1202311 => null;

		public static TLocalizedText Lq12029101 => null;

		public static TLocalizedText Lq12029401 => null;

		public static TLocalizedText Lq12029402 => null;

		public static TLocalizedText Lq12029201 => null;

		public static TLocalizedText Lq12029202 => null;

		public static TLocalizedText Lq12029203 => null;

		public static TLocalizedText Lq12029301 => null;

		public static TLocalizedText Lq12029302 => null;

		public static TLocalizedText Lq12029501 => null;

		public static TLocalizedText Lq12029502 => null;

		public static TLocalizedText Lq12029503 => null;

		public static TLocalizedText Lq12029504 => null;

		public static TLocalizedText Ll800019201 => null;

		public static TLocalizedText Ll80002101 => null;

		public static TLocalizedText Ll80002102 => null;

		public static TLocalizedText Ll80002103 => null;

		public static TLocalizedText Ll80002104 => null;

		public static TLocalizedText Ll80002105 => null;

		public static TLocalizedText Ll80002StartTitle => null;

		public static TLocalizedText Ll80003101 => null;

		public static TLocalizedText Ll80003102 => null;

		public static TLocalizedText Ll80003103 => null;

		public static TLocalizedText Ll80003104 => null;

		public static TLocalizedText Ll80003105 => null;

		public static TLocalizedText Ll80003106 => null;

		public static TLocalizedText Ll80003107 => null;

		public static TLocalizedText Ll80003108 => null;

		public static TLocalizedText Ll80003StartTitle => null;

		public static TLocalizedText Ll80004101 => null;

		public static TLocalizedText Ll80004102 => null;

		public static TLocalizedText Ll80004103 => null;

		public static TLocalizedText Ll80004104 => null;

		public static TLocalizedText Ll80004105 => null;

		public static TLocalizedText Ll80004StartTitle => null;

		public static TLocalizedText Ll80005101 => null;

		public static TLocalizedText Ll80005102 => null;

		public static TLocalizedText Ll80005103 => null;

		public static TLocalizedText Ll80005104 => null;

		public static TLocalizedText Ll80005StartTitle => null;

		public static TLocalizedText Ll80006101 => null;

		public static TLocalizedText Ll80006102 => null;

		public static TLocalizedText Ll80006103 => null;

		public static TLocalizedText Ll80006104 => null;

		public static TLocalizedText Ll80006105 => null;

		public static TLocalizedText Ll80006106 => null;

		public static TLocalizedText Ll80006107 => null;

		public static TLocalizedText Ll80006108 => null;

		public static TLocalizedText Ll80006109 => null;

		public static TLocalizedText Ll80006110 => null;

		public static TLocalizedText Ll80006111 => null;

		public static TLocalizedText Ll80006112 => null;

		public static TLocalizedText Ll80006113 => null;

		public static TLocalizedText Ll80006114 => null;

		public static TLocalizedText Ll80006115 => null;

		public static TLocalizedText Ll80006116 => null;

		public static TLocalizedText Ll80006117 => null;

		public static TLocalizedText Ll80006118 => null;

		public static TLocalizedText Ll80006119 => null;

		public static TLocalizedText Ll80006120 => null;

		public static TLocalizedText Ll80006121 => null;

		public static TLocalizedText Ll80006122 => null;

		public static TLocalizedText Ll80006123 => null;

		public static TLocalizedText Ll80006124 => null;

		public static TLocalizedText Ll80006125 => null;

		public static TLocalizedText Ll80006126 => null;

		public static TLocalizedText Ll80006127 => null;

		public static TLocalizedText Ll80006128 => null;

		public static TLocalizedText Ll80006129 => null;

		public static TLocalizedText Ll80006130 => null;

		public static TLocalizedText Ll80006131 => null;

		public static TLocalizedText Ll80006132 => null;

		public static TLocalizedText Ll80006133 => null;

		public static TLocalizedText Ll80006134 => null;

		public static TLocalizedText Ll80006StartTitle => null;

		public static TLocalizedText Ll80007101 => null;

		public static TLocalizedText Ll80007102 => null;

		public static TLocalizedText Ll80007103 => null;

		public static TLocalizedText Ll80007104 => null;

		public static TLocalizedText Ll80007StartTitle => null;

		public static TLocalizedText Ll80100101 => null;

		public static TLocalizedText Ll80100102 => null;

		public static TLocalizedText Ll80100103 => null;

		public static TLocalizedText Ll80100104 => null;

		public static TLocalizedText Ll80100105 => null;

		public static TLocalizedText Ll80100106 => null;

		public static TLocalizedText Ll80100107 => null;

		public static TLocalizedText Ll80100108 => null;

		public static TLocalizedText Ll80100109 => null;

		public static TLocalizedText Ll80100110 => null;

		public static TLocalizedText Ll80100201 => null;

		public static TLocalizedText Ll80100202 => null;

		public static TLocalizedText Ll80100StartTitle => null;

		public static TLocalizedText Ll80100601 => null;

		public static TLocalizedText Ll80101106 => null;

		public static TLocalizedText Ll8010130001 => null;

		public static TLocalizedText Ll8010140001 => null;

		public static TLocalizedText Ll8010150001 => null;

		public static TLocalizedText Ll80101201 => null;

		public static TLocalizedText Ll80101300 => null;

		public static TLocalizedText Ll80101301 => null;

		public static TLocalizedText Ll80101302 => null;

		public static TLocalizedText Ll80101303 => null;

		public static TLocalizedText Ll80101304 => null;

		public static TLocalizedText Ll80101305 => null;

		public static TLocalizedText Ll80101306 => null;

		public static TLocalizedText Ll80101307 => null;

		public static TLocalizedText Ll80101308 => null;

		public static TLocalizedText Ll80101309 => null;

		public static TLocalizedText Ll80101310 => null;

		public static TLocalizedText Ll80101311 => null;

		public static TLocalizedText Ll80101312 => null;

		public static TLocalizedText Ll80101313 => null;

		public static TLocalizedText Ll80101314 => null;

		public static TLocalizedText Ll80101315 => null;

		public static TLocalizedText Ll80101316 => null;

		public static TLocalizedText Ll80101317 => null;

		public static TLocalizedText Ll80101318 => null;

		public static TLocalizedText Ll80101400 => null;

		public static TLocalizedText Ll80101401 => null;

		public static TLocalizedText Ll80101402 => null;

		public static TLocalizedText Ll80101403 => null;

		public static TLocalizedText Ll80101404 => null;

		public static TLocalizedText Ll80101405 => null;

		public static TLocalizedText Ll80101406 => null;

		public static TLocalizedText Ll80101407 => null;

		public static TLocalizedText Ll80101408 => null;

		public static TLocalizedText Ll80101409 => null;

		public static TLocalizedText Ll80101410 => null;

		public static TLocalizedText Ll80101411 => null;

		public static TLocalizedText Ll80101412 => null;

		public static TLocalizedText Ll80101413 => null;

		public static TLocalizedText Ll80101414 => null;

		public static TLocalizedText Ll80101415 => null;

		public static TLocalizedText Ll80101416 => null;

		public static TLocalizedText Ll80101417 => null;

		public static TLocalizedText Ll80101418 => null;

		public static TLocalizedText Ll80101419 => null;

		public static TLocalizedText Ll80101420 => null;

		public static TLocalizedText Ll80101500 => null;

		public static TLocalizedText Ll80101501 => null;

		public static TLocalizedText Ll80101502 => null;

		public static TLocalizedText Ll80101503 => null;

		public static TLocalizedText Ll80101504 => null;

		public static TLocalizedText Ll80101505 => null;

		public static TLocalizedText Ll80101506 => null;

		public static TLocalizedText Ll80101507 => null;

		public static TLocalizedText Ll80101508 => null;

		public static TLocalizedText Ll80101509 => null;

		public static TLocalizedText Ll80101510 => null;

		public static TLocalizedText Ll80101511 => null;

		public static TLocalizedText Ll80101512 => null;

		public static TLocalizedText Ll80101513 => null;

		public static TLocalizedText Ll80101514 => null;

		public static TLocalizedText Ll80101515 => null;

		public static TLocalizedText Ll80101516 => null;

		public static TLocalizedText Ll80101517 => null;

		public static TLocalizedText Ll80101518 => null;

		public static TLocalizedText Ll80101519 => null;

		public static TLocalizedText Ll80101520 => null;

		public static TLocalizedText Ll80101521 => null;

		public static TLocalizedText Ll80101522 => null;

		public static TLocalizedText Ll80101StartTitle => null;

		public static TLocalizedText Ll8010199901 => null;

		public static TLocalizedText Ll80102106 => null;

		public static TLocalizedText Ll8010230001 => null;

		public static TLocalizedText Ll8010240001 => null;

		public static TLocalizedText Ll8010250001 => null;

		public static TLocalizedText Ll80102201 => null;

		public static TLocalizedText Ll80102300 => null;

		public static TLocalizedText Ll80102301 => null;

		public static TLocalizedText Ll80102302 => null;

		public static TLocalizedText Ll80102303 => null;

		public static TLocalizedText Ll80102304 => null;

		public static TLocalizedText Ll80102305 => null;

		public static TLocalizedText Ll80102306 => null;

		public static TLocalizedText Ll80102307 => null;

		public static TLocalizedText Ll80102308 => null;

		public static TLocalizedText Ll80102309 => null;

		public static TLocalizedText Ll80102310 => null;

		public static TLocalizedText Ll80102311 => null;

		public static TLocalizedText Ll80102312 => null;

		public static TLocalizedText Ll80102313 => null;

		public static TLocalizedText Ll80102314 => null;

		public static TLocalizedText Ll80102315 => null;

		public static TLocalizedText Ll80102316 => null;

		public static TLocalizedText Ll80102317 => null;

		public static TLocalizedText Ll80102318 => null;

		public static TLocalizedText Ll80102400 => null;

		public static TLocalizedText Ll80102401 => null;

		public static TLocalizedText Ll80102402 => null;

		public static TLocalizedText Ll80102403 => null;

		public static TLocalizedText Ll80102404 => null;

		public static TLocalizedText Ll80102405 => null;

		public static TLocalizedText Ll80102406 => null;

		public static TLocalizedText Ll80102407 => null;

		public static TLocalizedText Ll80102408 => null;

		public static TLocalizedText Ll80102409 => null;

		public static TLocalizedText Ll80102410 => null;

		public static TLocalizedText Ll80102411 => null;

		public static TLocalizedText Ll80102412 => null;

		public static TLocalizedText Ll80102413 => null;

		public static TLocalizedText Ll80102414 => null;

		public static TLocalizedText Ll80102415 => null;

		public static TLocalizedText Ll80102416 => null;

		public static TLocalizedText Ll80102417 => null;

		public static TLocalizedText Ll80102418 => null;

		public static TLocalizedText Ll80102419 => null;

		public static TLocalizedText Ll80102420 => null;

		public static TLocalizedText Ll80102500 => null;

		public static TLocalizedText Ll80102501 => null;

		public static TLocalizedText Ll80102502 => null;

		public static TLocalizedText Ll80102503 => null;

		public static TLocalizedText Ll80102504 => null;

		public static TLocalizedText Ll80102505 => null;

		public static TLocalizedText Ll80102506 => null;

		public static TLocalizedText Ll80102507 => null;

		public static TLocalizedText Ll80102508 => null;

		public static TLocalizedText Ll80102509 => null;

		public static TLocalizedText Ll80102510 => null;

		public static TLocalizedText Ll80102511 => null;

		public static TLocalizedText Ll80102512 => null;

		public static TLocalizedText Ll80102513 => null;

		public static TLocalizedText Ll80102514 => null;

		public static TLocalizedText Ll80102515 => null;

		public static TLocalizedText Ll80102516 => null;

		public static TLocalizedText Ll80102517 => null;

		public static TLocalizedText Ll80102518 => null;

		public static TLocalizedText Ll80102519 => null;

		public static TLocalizedText Ll80102520 => null;

		public static TLocalizedText Ll80102521 => null;

		public static TLocalizedText Ll80102522 => null;

		public static TLocalizedText Ll80102StartTitle => null;

		public static TLocalizedText Ll8010299901 => null;

		public static TLocalizedText Ll80103106 => null;

		public static TLocalizedText Ll8010330001 => null;

		public static TLocalizedText Ll8010340001 => null;

		public static TLocalizedText Ll8010350001 => null;

		public static TLocalizedText Ll80103201 => null;

		public static TLocalizedText Ll80103300 => null;

		public static TLocalizedText Ll80103301 => null;

		public static TLocalizedText Ll80103302 => null;

		public static TLocalizedText Ll80103303 => null;

		public static TLocalizedText Ll80103304 => null;

		public static TLocalizedText Ll80103305 => null;

		public static TLocalizedText Ll80103306 => null;

		public static TLocalizedText Ll80103307 => null;

		public static TLocalizedText Ll80103308 => null;

		public static TLocalizedText Ll80103309 => null;

		public static TLocalizedText Ll80103310 => null;

		public static TLocalizedText Ll80103311 => null;

		public static TLocalizedText Ll80103312 => null;

		public static TLocalizedText Ll80103313 => null;

		public static TLocalizedText Ll80103314 => null;

		public static TLocalizedText Ll80103315 => null;

		public static TLocalizedText Ll80103316 => null;

		public static TLocalizedText Ll80103317 => null;

		public static TLocalizedText Ll80103318 => null;

		public static TLocalizedText Ll80103400 => null;

		public static TLocalizedText Ll80103401 => null;

		public static TLocalizedText Ll80103402 => null;

		public static TLocalizedText Ll80103403 => null;

		public static TLocalizedText Ll80103404 => null;

		public static TLocalizedText Ll80103405 => null;

		public static TLocalizedText Ll80103406 => null;

		public static TLocalizedText Ll80103407 => null;

		public static TLocalizedText Ll80103408 => null;

		public static TLocalizedText Ll80103409 => null;

		public static TLocalizedText Ll80103410 => null;

		public static TLocalizedText Ll80103411 => null;

		public static TLocalizedText Ll80103412 => null;

		public static TLocalizedText Ll80103413 => null;

		public static TLocalizedText Ll80103414 => null;

		public static TLocalizedText Ll80103415 => null;

		public static TLocalizedText Ll80103416 => null;

		public static TLocalizedText Ll80103417 => null;

		public static TLocalizedText Ll80103418 => null;

		public static TLocalizedText Ll80103419 => null;

		public static TLocalizedText Ll80103420 => null;

		public static TLocalizedText Ll80103500 => null;

		public static TLocalizedText Ll80103501 => null;

		public static TLocalizedText Ll80103502 => null;

		public static TLocalizedText Ll80103503 => null;

		public static TLocalizedText Ll80103504 => null;

		public static TLocalizedText Ll80103505 => null;

		public static TLocalizedText Ll80103506 => null;

		public static TLocalizedText Ll80103507 => null;

		public static TLocalizedText Ll80103508 => null;

		public static TLocalizedText Ll80103509 => null;

		public static TLocalizedText Ll80103510 => null;

		public static TLocalizedText Ll80103511 => null;

		public static TLocalizedText Ll80103512 => null;

		public static TLocalizedText Ll80103513 => null;

		public static TLocalizedText Ll80103514 => null;

		public static TLocalizedText Ll80103515 => null;

		public static TLocalizedText Ll80103516 => null;

		public static TLocalizedText Ll80103517 => null;

		public static TLocalizedText Ll80103518 => null;

		public static TLocalizedText Ll80103519 => null;

		public static TLocalizedText Ll80103520 => null;

		public static TLocalizedText Ll80103521 => null;

		public static TLocalizedText Ll80103522 => null;

		public static TLocalizedText Ll80103StartTitle => null;

		public static TLocalizedText Ll8010399901 => null;

		public static TLocalizedText Ll80104106 => null;

		public static TLocalizedText Ll8010430001 => null;

		public static TLocalizedText Ll80104300 => null;

		public static TLocalizedText Ll80104301 => null;

		public static TLocalizedText Ll80104302 => null;

		public static TLocalizedText Ll80104303 => null;

		public static TLocalizedText Ll80104304 => null;

		public static TLocalizedText Ll80104305 => null;

		public static TLocalizedText Ll80104306 => null;

		public static TLocalizedText Ll80104307 => null;

		public static TLocalizedText Ll80104308 => null;

		public static TLocalizedText Ll80104309 => null;

		public static TLocalizedText Ll80104310 => null;

		public static TLocalizedText Ll80104311 => null;

		public static TLocalizedText Ll80104312 => null;

		public static TLocalizedText Ll80104313 => null;

		public static TLocalizedText Ll80104314 => null;

		public static TLocalizedText Ll80104315 => null;

		public static TLocalizedText Ll80104316 => null;

		public static TLocalizedText Ll80104317 => null;

		public static TLocalizedText Ll80104318 => null;

		public static TLocalizedText Ll80104319 => null;

		public static TLocalizedText Ll80104320 => null;

		public static TLocalizedText Ll80104321 => null;

		public static TLocalizedText Ll80104322 => null;

		public static TLocalizedText Ll80104323 => null;

		public static TLocalizedText Ll80104324 => null;

		public static TLocalizedText Ll80104325 => null;

		public static TLocalizedText Ll80104326 => null;

		public static TLocalizedText Ll80104327 => null;

		public static TLocalizedText Ll80104328 => null;

		public static TLocalizedText Ll80104329 => null;

		public static TLocalizedText Ll80104330 => null;

		public static TLocalizedText Ll80104331 => null;

		public static TLocalizedText Ll80104332 => null;

		public static TLocalizedText Ll80104333 => null;

		public static TLocalizedText Ll80104334 => null;

		public static TLocalizedText Ll80104StartTitle => null;

		public static TLocalizedText Ll8010429901 => null;

		public static TLocalizedText Ll8010499901 => null;

		public static TLocalizedText Ll80200101 => null;

		public static TLocalizedText Ll80200102 => null;

		public static TLocalizedText Ll80200103 => null;

		public static TLocalizedText Ll80200104 => null;

		public static TLocalizedText Ll80200105 => null;

		public static TLocalizedText Ll80200201 => null;

		public static TLocalizedText Ll80200202 => null;

		public static TLocalizedText Ll80200301 => null;

		public static TLocalizedText Ll80200302 => null;

		public static TLocalizedText Ll80200303 => null;

		public static TLocalizedText Ll80200304 => null;

		public static TLocalizedText Ll80200401 => null;

		public static TLocalizedText Ll80200402 => null;

		public static TLocalizedText Ll80200403 => null;

		public static TLocalizedText Ll80200404 => null;

		public static TLocalizedText Ll80200405 => null;

		public static TLocalizedText Ll80200501 => null;

		public static TLocalizedText Ll80200502 => null;

		public static TLocalizedText Ll80200503 => null;

		public static TLocalizedText Ll80200504 => null;

		public static TLocalizedText Ll80200505 => null;

		public static TLocalizedText Ll80200StartTitle => null;

		public static TLocalizedText Ll80200CompleteTitle => null;

		public static TLocalizedText Ll80200601 => null;

		public static TLocalizedText Ll81101StartTitle => null;

		public static TLocalizedText Ll81101CompleteTitle => null;

		public static TLocalizedText Ll81102StartTitle => null;

		public static TLocalizedText Ll81102CompleteTitle => null;

		public static TLocalizedText Ll81103StartTitle => null;

		public static TLocalizedText Ll81103CompleteTitle => null;

		public static TLocalizedText Ll81104StartTitle => null;

		public static TLocalizedText Ll81104CompleteTitle => null;

		public static TLocalizedText Ll81105StartTitle => null;

		public static TLocalizedText Ll81105CompleteTitle => null;

		public static TLocalizedText Ll82100101 => null;

		public static TLocalizedText Ll82100102 => null;

		public static TLocalizedText Ll82100103 => null;

		public static TLocalizedText Ll82100104 => null;

		public static TLocalizedText Ll82100105 => null;

		public static TLocalizedText Ll8210010601 => null;

		public static TLocalizedText Ll8210010702 => null;

		public static TLocalizedText Ll82100106 => null;

		public static TLocalizedText Ll82100107 => null;

		public static TLocalizedText Ll82100201 => null;

		public static TLocalizedText Ll82100202 => null;

		public static TLocalizedText Ll82100StartTitle => null;

		public static TLocalizedText LlComplete => null;

		public static TLocalizedText Ll82101201 => null;

		public static TLocalizedText Ll82101202 => null;

		public static TLocalizedText Ll82101203 => null;

		public static TLocalizedText Ll82101204 => null;

		public static TLocalizedText Ll8210120501 => null;

		public static TLocalizedText Ll8210120602 => null;

		public static TLocalizedText Ll82101205 => null;

		public static TLocalizedText Ll82101206 => null;

		public static TLocalizedText Ll82101301 => null;

		public static TLocalizedText Ll82101StartTitle => null;

		public static TLocalizedText Ll82101101 => null;

		public static TLocalizedText Ll82101102 => null;

		public static TLocalizedText Ll82101103 => null;

		public static TLocalizedText Ll82101401 => null;

		public static TLocalizedText Ll82102101 => null;

		public static TLocalizedText Ll82102102 => null;

		public static TLocalizedText Ll82102103 => null;

		public static TLocalizedText Ll82102104 => null;

		public static TLocalizedText Ll8210210501 => null;

		public static TLocalizedText Ll8210210602 => null;

		public static TLocalizedText Ll82102105 => null;

		public static TLocalizedText Ll82102106 => null;

		public static TLocalizedText Ll82102201 => null;

		public static TLocalizedText Ll82102202 => null;

		public static TLocalizedText Ll82102203 => null;

		public static TLocalizedText Ll82102204 => null;

		public static TLocalizedText Ll82102StartTitle => null;

		public static TLocalizedText Ll82102401 => null;

		public static TLocalizedText Ll82103201 => null;

		public static TLocalizedText Ll82103202 => null;

		public static TLocalizedText Ll82103203 => null;

		public static TLocalizedText Ll82103204 => null;

		public static TLocalizedText Ll8210320501 => null;

		public static TLocalizedText Ll8210320602 => null;

		public static TLocalizedText Ll82103205 => null;

		public static TLocalizedText Ll82103206 => null;

		public static TLocalizedText Ll82103301 => null;

		public static TLocalizedText Ll82103302 => null;

		public static TLocalizedText Ll82103StartTitle => null;

		public static TLocalizedText Ll82103101 => null;

		public static TLocalizedText Ll82103102 => null;

		public static TLocalizedText Ll82103103 => null;

		public static TLocalizedText Ll82103401 => null;

		public static TLocalizedText Ll82104101 => null;

		public static TLocalizedText Ll82104102 => null;

		public static TLocalizedText Ll82104103 => null;

		public static TLocalizedText Ll82104104 => null;

		public static TLocalizedText Ll8210410501 => null;

		public static TLocalizedText Ll8210410602 => null;

		public static TLocalizedText Ll82104105 => null;

		public static TLocalizedText Ll82104106 => null;

		public static TLocalizedText Ll82104201 => null;

		public static TLocalizedText Ll82104202 => null;

		public static TLocalizedText Ll82104203 => null;

		public static TLocalizedText Ll82104StartTitle => null;

		public static TLocalizedText Ll82104401 => null;

		public static TLocalizedText Ll82105101 => null;

		public static TLocalizedText Ll82105102 => null;

		public static TLocalizedText Ll82105103 => null;

		public static TLocalizedText Ll82105104 => null;

		public static TLocalizedText Ll82105105 => null;

		public static TLocalizedText Ll8210510601 => null;

		public static TLocalizedText Ll8210510702 => null;

		public static TLocalizedText Ll82105106 => null;

		public static TLocalizedText Ll82105107 => null;

		public static TLocalizedText Ll82105201 => null;

		public static TLocalizedText Ll82105202 => null;

		public static TLocalizedText Ll82105203 => null;

		public static TLocalizedText Ll82105StartTitle => null;

		public static TLocalizedText Ll82105401 => null;

		public static TLocalizedText Ll82106101 => null;

		public static TLocalizedText Ll82106102 => null;

		public static TLocalizedText Ll82106103 => null;

		public static TLocalizedText Ll82106104 => null;

		public static TLocalizedText Ll8210610501 => null;

		public static TLocalizedText Ll8210610602 => null;

		public static TLocalizedText Ll82106105 => null;

		public static TLocalizedText Ll82106106 => null;

		public static TLocalizedText Ll82106201 => null;

		public static TLocalizedText Ll82106202 => null;

		public static TLocalizedText Ll82106StartTitle => null;

		public static TLocalizedText Ll82106401 => null;

		public static TLocalizedText Ll82107101 => null;

		public static TLocalizedText Ll82107102 => null;

		public static TLocalizedText Ll82107103 => null;

		public static TLocalizedText Ll82107104 => null;

		public static TLocalizedText Ll8210710501 => null;

		public static TLocalizedText Ll8210710602 => null;

		public static TLocalizedText Ll82107105 => null;

		public static TLocalizedText Ll82107106 => null;

		public static TLocalizedText Ll82107201 => null;

		public static TLocalizedText Ll82107202 => null;

		public static TLocalizedText Ll82107StartTitle => null;

		public static TLocalizedText Ll82107401 => null;

		public static TLocalizedText Ll82108101 => null;

		public static TLocalizedText Ll82108102 => null;

		public static TLocalizedText Ll82108103 => null;

		public static TLocalizedText Ll82108104 => null;

		public static TLocalizedText Ll8210810501 => null;

		public static TLocalizedText Ll8210810602 => null;

		public static TLocalizedText Ll82108105 => null;

		public static TLocalizedText Ll82108106 => null;

		public static TLocalizedText Ll82108201 => null;

		public static TLocalizedText Ll82108202 => null;

		public static TLocalizedText Ll82108203 => null;

		public static TLocalizedText Ll82108StartTitle => null;

		public static TLocalizedText Ll82108401 => null;

		public static TLocalizedText Ll82109201 => null;

		public static TLocalizedText Ll82109202 => null;

		public static TLocalizedText Ll82109203 => null;

		public static TLocalizedText Ll82109204 => null;

		public static TLocalizedText Ll82109205 => null;

		public static TLocalizedText Ll8210920601 => null;

		public static TLocalizedText Ll8210920702 => null;

		public static TLocalizedText Ll82109206 => null;

		public static TLocalizedText Ll82109207 => null;

		public static TLocalizedText Ll82109301 => null;

		public static TLocalizedText Ll82109302 => null;

		public static TLocalizedText Ll82109303 => null;

		public static TLocalizedText Ll82109StartTitle => null;

		public static TLocalizedText Ll82109101 => null;

		public static TLocalizedText Ll82109102 => null;

		public static TLocalizedText Ll82109103 => null;

		public static TLocalizedText Ll82109401 => null;

		public static TLocalizedText Ll82111101 => null;

		public static TLocalizedText Ll82111102 => null;

		public static TLocalizedText Ll82111103 => null;

		public static TLocalizedText Ll82111104 => null;

		public static TLocalizedText Ll8211110501 => null;

		public static TLocalizedText Ll8211110602 => null;

		public static TLocalizedText Ll82111105 => null;

		public static TLocalizedText Ll82111106 => null;

		public static TLocalizedText Ll82111201 => null;

		public static TLocalizedText Ll82111202 => null;

		public static TLocalizedText Ll82111203 => null;

		public static TLocalizedText Ll82111StartTitle => null;

		public static TLocalizedText Ll82111401 => null;

		public static TLocalizedText Ll82112101 => null;

		public static TLocalizedText Ll82112102 => null;

		public static TLocalizedText Ll82112103 => null;

		public static TLocalizedText Ll8211210401 => null;

		public static TLocalizedText Ll8211210502 => null;

		public static TLocalizedText Ll82112104 => null;

		public static TLocalizedText Ll82112105 => null;

		public static TLocalizedText Ll82112201 => null;

		public static TLocalizedText Ll82112202 => null;

		public static TLocalizedText Ll82112StartTitle => null;

		public static TLocalizedText Ll82113101 => null;

		public static TLocalizedText Ll82113102 => null;

		public static TLocalizedText Ll82113103 => null;

		public static TLocalizedText Ll8211310401 => null;

		public static TLocalizedText Ll8211310502 => null;

		public static TLocalizedText Ll82113104 => null;

		public static TLocalizedText Ll82113105 => null;

		public static TLocalizedText Ll82113201 => null;

		public static TLocalizedText Ll82113202 => null;

		public static TLocalizedText Ll82113203 => null;

		public static TLocalizedText Ll82113204 => null;

		public static TLocalizedText Ll82113StartTitle => null;

		public static TLocalizedText Ll82113401 => null;

		public static TLocalizedText Ll82190101 => null;

		public static TLocalizedText Ll82190102 => null;

		public static TLocalizedText Ll82190103 => null;

		public static TLocalizedText Ll8219010401 => null;

		public static TLocalizedText Ll8219010502 => null;

		public static TLocalizedText Ll82190104 => null;

		public static TLocalizedText Ll82190105 => null;

		public static TLocalizedText Ll82190201 => null;

		public static TLocalizedText Ll82190StartTitle => null;

		public static TLocalizedText Ll82190401 => null;

		public static TLocalizedText Ll82201101 => null;

		public static TLocalizedText Ll82201102 => null;

		public static TLocalizedText Ll82201103 => null;

		public static TLocalizedText Ll8220110401 => null;

		public static TLocalizedText Ll8220110502 => null;

		public static TLocalizedText Ll82201104 => null;

		public static TLocalizedText Ll82201105 => null;

		public static TLocalizedText Ll82201201 => null;

		public static TLocalizedText Ll82201202 => null;

		public static TLocalizedText Ll82201StartTitle => null;

		public static TLocalizedText Ll82201401 => null;

		public static TLocalizedText Ll82202101 => null;

		public static TLocalizedText Ll82202102 => null;

		public static TLocalizedText Ll82202103 => null;

		public static TLocalizedText Ll82202104 => null;

		public static TLocalizedText Ll8220210501 => null;

		public static TLocalizedText Ll8220210602 => null;

		public static TLocalizedText Ll82202105 => null;

		public static TLocalizedText Ll82202106 => null;

		public static TLocalizedText Ll82202201 => null;

		public static TLocalizedText Ll82202202 => null;

		public static TLocalizedText Ll82202203 => null;

		public static TLocalizedText Ll82202204 => null;

		public static TLocalizedText Ll82202StartTitle => null;

		public static TLocalizedText Ll82202401 => null;

		public static TLocalizedText Ll82203201 => null;

		public static TLocalizedText Ll82203202 => null;

		public static TLocalizedText Ll82203203 => null;

		public static TLocalizedText Ll8220320401 => null;

		public static TLocalizedText Ll8220320502 => null;

		public static TLocalizedText Ll82203204 => null;

		public static TLocalizedText Ll82203205 => null;

		public static TLocalizedText Ll82203301 => null;

		public static TLocalizedText Ll82203302 => null;

		public static TLocalizedText Ll82203303 => null;

		public static TLocalizedText Ll82203304 => null;

		public static TLocalizedText Ll82203StartTitle => null;

		public static TLocalizedText Ll82203401 => null;

		public static TLocalizedText Ll82204101 => null;

		public static TLocalizedText Ll82204102 => null;

		public static TLocalizedText Ll82204103 => null;

		public static TLocalizedText Ll8220410401 => null;

		public static TLocalizedText Ll8220410502 => null;

		public static TLocalizedText Ll82204104 => null;

		public static TLocalizedText Ll82204105 => null;

		public static TLocalizedText Ll82204201 => null;

		public static TLocalizedText Ll82204202 => null;

		public static TLocalizedText Ll82204203 => null;

		public static TLocalizedText Ll82204StartTitle => null;

		public static TLocalizedText Ll82204401 => null;

		public static TLocalizedText Ll82205201 => null;

		public static TLocalizedText Ll82205202 => null;

		public static TLocalizedText Ll82205203 => null;

		public static TLocalizedText Ll82205204 => null;

		public static TLocalizedText Ll8220520501 => null;

		public static TLocalizedText Ll8220520602 => null;

		public static TLocalizedText Ll82205205 => null;

		public static TLocalizedText Ll82205206 => null;

		public static TLocalizedText Ll82205301 => null;

		public static TLocalizedText Ll82205302 => null;

		public static TLocalizedText Ll82205303 => null;

		public static TLocalizedText Ll82205StartTitle => null;

		public static TLocalizedText Ll82205401 => null;

		public static TLocalizedText Ll82206201 => null;

		public static TLocalizedText Ll82206202 => null;

		public static TLocalizedText Ll8220620301 => null;

		public static TLocalizedText Ll8220620402 => null;

		public static TLocalizedText Ll82206203 => null;

		public static TLocalizedText Ll82206204 => null;

		public static TLocalizedText Ll82206301 => null;

		public static TLocalizedText Ll82206302 => null;

		public static TLocalizedText Ll82206303 => null;

		public static TLocalizedText Ll82206StartTitle => null;

		public static TLocalizedText Ll82206401 => null;

		public static TLocalizedText Ll82207201 => null;

		public static TLocalizedText Ll82207202 => null;

		public static TLocalizedText Ll82207203 => null;

		public static TLocalizedText Ll8220720401 => null;

		public static TLocalizedText Ll8220720502 => null;

		public static TLocalizedText Ll82207204 => null;

		public static TLocalizedText Ll82207205 => null;

		public static TLocalizedText Ll82207301 => null;

		public static TLocalizedText Ll82207302 => null;

		public static TLocalizedText Ll82207303 => null;

		public static TLocalizedText Ll82207StartTitle => null;

		public static TLocalizedText Ll82207401 => null;

		public static TLocalizedText Ll82208101 => null;

		public static TLocalizedText Ll82208102 => null;

		public static TLocalizedText Ll82208103 => null;

		public static TLocalizedText Ll82208104 => null;

		public static TLocalizedText Ll8220810501 => null;

		public static TLocalizedText Ll8220810602 => null;

		public static TLocalizedText Ll82208105 => null;

		public static TLocalizedText Ll82208106 => null;

		public static TLocalizedText Ll82208201 => null;

		public static TLocalizedText Ll82208202 => null;

		public static TLocalizedText Ll82208203 => null;

		public static TLocalizedText Ll82208StartTitle => null;

		public static TLocalizedText Ll82208401 => null;

		public static TLocalizedText Ll82209101 => null;

		public static TLocalizedText Ll82209102 => null;

		public static TLocalizedText Ll82209103 => null;

		public static TLocalizedText Ll82209104 => null;

		public static TLocalizedText Ll82209105 => null;

		public static TLocalizedText Ll8220910601 => null;

		public static TLocalizedText Ll8220910702 => null;

		public static TLocalizedText Ll82209106 => null;

		public static TLocalizedText Ll82209107 => null;

		public static TLocalizedText Ll82209201 => null;

		public static TLocalizedText Ll82209202 => null;

		public static TLocalizedText Ll82209StartTitle => null;

		public static TLocalizedText Ll82209401 => null;

		public static TLocalizedText Ll82210101 => null;

		public static TLocalizedText Ll82210102 => null;

		public static TLocalizedText Ll82210103 => null;

		public static TLocalizedText Ll8221010401 => null;

		public static TLocalizedText Ll8221010502 => null;

		public static TLocalizedText Ll82210104 => null;

		public static TLocalizedText Ll82210105 => null;

		public static TLocalizedText Ll82210201 => null;

		public static TLocalizedText Ll82210202 => null;

		public static TLocalizedText Ll82210StartTitle => null;

		public static TLocalizedText Ll82210401 => null;

		public static TLocalizedText Ll82290101 => null;

		public static TLocalizedText Ll82290102 => null;

		public static TLocalizedText Ll82290103 => null;

		public static TLocalizedText Ll8229010401 => null;

		public static TLocalizedText Ll8229010502 => null;

		public static TLocalizedText Ll82290104 => null;

		public static TLocalizedText Ll82290105 => null;

		public static TLocalizedText Ll82290201 => null;

		public static TLocalizedText Ll82290StartTitle => null;

		public static TLocalizedText Ll82290401 => null;

		public static TLocalizedText Ll84101101 => null;

		public static TLocalizedText Ll84101102 => null;

		public static TLocalizedText Ll84101103 => null;

		public static TLocalizedText Ll84101104 => null;

		public static TLocalizedText Ll84101105 => null;

		public static TLocalizedText Ll84101106 => null;

		public static TLocalizedText Ll84101107 => null;

		public static TLocalizedText Ll84101108 => null;

		public static TLocalizedText Ll84101109 => null;

		public static TLocalizedText Ll84101110 => null;

		public static TLocalizedText Ll84101111 => null;

		public static TLocalizedText Ll84101112 => null;

		public static TLocalizedText Ll84101113 => null;

		public static TLocalizedText Ll84101114 => null;

		public static TLocalizedText Ll84101115 => null;

		public static TLocalizedText Ll84101116 => null;

		public static TLocalizedText Ll84101117 => null;

		public static TLocalizedText Ll84101118 => null;

		public static TLocalizedText Ll84101119 => null;

		public static TLocalizedText Ll84101120 => null;

		public static TLocalizedText Ll84101121 => null;

		public static TLocalizedText Ll84101122 => null;

		public static TLocalizedText Ll84101123 => null;

		public static TLocalizedText Ll84101124 => null;

		public static TLocalizedText Ll84101125 => null;

		public static TLocalizedText Ll84101126 => null;

		public static TLocalizedText Ll84101127 => null;

		public static TLocalizedText Ll84101128 => null;

		public static TLocalizedText Ll84101129 => null;

		public static TLocalizedText Ll84101130 => null;

		public static TLocalizedText Ll841019101 => null;

		public static TLocalizedText Ll841019201 => null;

		public static TLocalizedText Ll841019202 => null;

		public static TLocalizedText Ll84102101 => null;

		public static TLocalizedText Ll84102102 => null;

		public static TLocalizedText Ll84102103 => null;

		public static TLocalizedText Ll84102104 => null;

		public static TLocalizedText Ll84102105 => null;

		public static TLocalizedText Ll84102106 => null;

		public static TLocalizedText Ll84102107 => null;

		public static TLocalizedText Ll84102108 => null;

		public static TLocalizedText Ll84102109 => null;

		public static TLocalizedText Ll84102110 => null;

		public static TLocalizedText Ll84102111 => null;

		public static TLocalizedText Ll84102Title => null;

		public static TLocalizedText Ll84102Need => null;

		public static TLocalizedText Ll84102401 => null;

		public static TLocalizedText Ll84301101 => null;

		public static TLocalizedText Ll84301102 => null;

		public static TLocalizedText Ll84301103 => null;

		public static TLocalizedText Ll84301104 => null;

		public static TLocalizedText Ll84301105 => null;

		public static TLocalizedText Ll84301106 => null;

		public static TLocalizedText Ll84301107 => null;

		public static TLocalizedText Ll84301108 => null;

		public static TLocalizedText Ll84301109 => null;

		public static TLocalizedText Ll84301110 => null;

		public static TLocalizedText Ll84301111 => null;

		public static TLocalizedText Ll84301112 => null;

		public static TLocalizedText Ll84301113 => null;

		public static TLocalizedText Ll84301114 => null;

		public static TLocalizedText Ll84301115 => null;

		public static TLocalizedText Ll84301116 => null;

		public static TLocalizedText Ll84301117 => null;

		public static TLocalizedText Ll84301118 => null;

		public static TLocalizedText Ll84301119 => null;

		public static TLocalizedText Ll84301120 => null;

		public static TLocalizedText Ll84301121 => null;

		public static TLocalizedText Ll84301122 => null;

		public static TLocalizedText Ll84301123 => null;

		public static TLocalizedText Ll84301124 => null;

		public static TLocalizedText Ll84301125 => null;

		public static TLocalizedText Ll84301126 => null;

		public static TLocalizedText Ll84301127 => null;

		public static TLocalizedText Ll84301128 => null;

		public static TLocalizedText Ll84301Title => null;

		public static TLocalizedText Ll84301Need => null;

		public static TLocalizedText Ll84301401 => null;

		public static TLocalizedText Ll84302101 => null;

		public static TLocalizedText Ll84302102 => null;

		public static TLocalizedText Ll84302103 => null;

		public static TLocalizedText Ll84302104 => null;

		public static TLocalizedText Ll84302105 => null;

		public static TLocalizedText Ll84302106 => null;

		public static TLocalizedText Ll84302107 => null;

		public static TLocalizedText Ll84302108 => null;

		public static TLocalizedText Ll84302109 => null;

		public static TLocalizedText Ll84302110 => null;

		public static TLocalizedText Ll84302111 => null;

		public static TLocalizedText Ll84302112 => null;

		public static TLocalizedText Ll84302113 => null;

		public static TLocalizedText Ll84302114 => null;

		public static TLocalizedText Ll84302115 => null;

		public static TLocalizedText Ll84302116 => null;

		public static TLocalizedText Ll84302117 => null;

		public static TLocalizedText Ll84302118 => null;

		public static TLocalizedText Ll84302119 => null;

		public static TLocalizedText Ll84302120 => null;

		public static TLocalizedText Ll84302121 => null;

		public static TLocalizedText Ll84302122 => null;

		public static TLocalizedText Ll84302123 => null;

		public static TLocalizedText Ll84302124 => null;

		public static TLocalizedText Ll84302125 => null;

		public static TLocalizedText Ll84302Title => null;

		public static TLocalizedText Ll84302Need => null;

		public static TLocalizedText Ll84302401 => null;

		public static TLocalizedText Ll84401101 => null;

		public static TLocalizedText Ll84401102 => null;

		public static TLocalizedText Ll84401103 => null;

		public static TLocalizedText Ll84401104 => null;

		public static TLocalizedText Ll84401105 => null;

		public static TLocalizedText Ll84401106 => null;

		public static TLocalizedText Ll84401107 => null;

		public static TLocalizedText Ll84401108 => null;

		public static TLocalizedText Ll84401109 => null;

		public static TLocalizedText Ll84401110 => null;

		public static TLocalizedText Ll84401111 => null;

		public static TLocalizedText Ll84401112 => null;

		public static TLocalizedText Ll84401113 => null;

		public static TLocalizedText Ll84401114 => null;

		public static TLocalizedText Ll84401115 => null;

		public static TLocalizedText Ll84401116 => null;

		public static TLocalizedText Ll84401117 => null;

		public static TLocalizedText Ll84401118 => null;

		public static TLocalizedText Ll84401119 => null;

		public static TLocalizedText Ll84401120 => null;

		public static TLocalizedText Ll84401121 => null;

		public static TLocalizedText Ll84401122 => null;

		public static TLocalizedText Ll84401123 => null;

		public static TLocalizedText Ll84401124 => null;

		public static TLocalizedText Ll84401125 => null;

		public static TLocalizedText Ll84401126 => null;

		public static TLocalizedText Ll84401Title => null;

		public static TLocalizedText Ll84401Need => null;

		public static TLocalizedText Ll84401401 => null;

		public static TLocalizedText Ll84402101 => null;

		public static TLocalizedText Ll84402102 => null;

		public static TLocalizedText Ll84402103 => null;

		public static TLocalizedText Ll84402Title => null;

		public static TLocalizedText Ll84402Need => null;

		public static TLocalizedText Ll84402401 => null;

		public static TLocalizedText Ll84501101 => null;

		public static TLocalizedText Ll84501102 => null;

		public static TLocalizedText Ll84501103 => null;

		public static TLocalizedText Ll84501104 => null;

		public static TLocalizedText Ll84501105 => null;

		public static TLocalizedText Ll84501106 => null;

		public static TLocalizedText Ll84501107 => null;

		public static TLocalizedText Ll84501108 => null;

		public static TLocalizedText Ll84501109 => null;

		public static TLocalizedText Ll84501110 => null;

		public static TLocalizedText Ll84501111 => null;

		public static TLocalizedText Ll84501112 => null;

		public static TLocalizedText Ll84501113 => null;

		public static TLocalizedText Ll84501114 => null;

		public static TLocalizedText Ll84501115 => null;

		public static TLocalizedText Ll84501116 => null;

		public static TLocalizedText Ll84501117 => null;

		public static TLocalizedText Ll84501118 => null;

		public static TLocalizedText Ll84501119 => null;

		public static TLocalizedText Ll84501120 => null;

		public static TLocalizedText Ll84501121 => null;

		public static TLocalizedText Ll84501122 => null;

		public static TLocalizedText Ll84501123 => null;

		public static TLocalizedText Ll84501124 => null;

		public static TLocalizedText Ll84501125 => null;

		public static TLocalizedText Ll84501126 => null;

		public static TLocalizedText Ll84501127 => null;

		public static TLocalizedText Ll84501Title => null;

		public static TLocalizedText Ll84501Need => null;

		public static TLocalizedText Ll84501401 => null;

		public static TLocalizedText Ll84502101 => null;

		public static TLocalizedText Ll84502102 => null;

		public static TLocalizedText Ll84502103 => null;

		public static TLocalizedText Ll84502104 => null;

		public static TLocalizedText Ll84502105 => null;

		public static TLocalizedText Ll84502106 => null;

		public static TLocalizedText Ll84502107 => null;

		public static TLocalizedText Ll84502108 => null;

		public static TLocalizedText Ll84502109 => null;

		public static TLocalizedText Ll84502110 => null;

		public static TLocalizedText Ll84502111 => null;

		public static TLocalizedText Ll84502112 => null;

		public static TLocalizedText Ll84502113 => null;

		public static TLocalizedText Ll84502114 => null;

		public static TLocalizedText Ll84502115 => null;

		public static TLocalizedText Ll84502116 => null;

		public static TLocalizedText Ll84502117 => null;

		public static TLocalizedText Ll84502118 => null;

		public static TLocalizedText Ll84502119 => null;

		public static TLocalizedText Ll84502120 => null;

		public static TLocalizedText Ll84502Title => null;

		public static TLocalizedText Ll84502Need => null;

		public static TLocalizedText Ll84502401 => null;

		public static TLocalizedText Lm910120211 => null;

		public static TLocalizedText Lm910120221 => null;

		public static TLocalizedText Lm910120231 => null;

		public static TLocalizedText Lm910120241 => null;

		public static TLocalizedText Lm9101202 => null;

		public static TLocalizedText Lm910130211 => null;

		public static TLocalizedText Lm910130221 => null;

		public static TLocalizedText Lm910130231 => null;

		public static TLocalizedText Lm910130241 => null;

		public static TLocalizedText Lm9101302 => null;

		public static TLocalizedText Lm9101111 => null;

		public static TLocalizedText Lm910120201 => null;

		public static TLocalizedText Lm910130201 => null;

		public static TLocalizedText Lm9101121 => null;

		public static TLocalizedText Lm9101131 => null;

		public static TLocalizedText Lm9101141 => null;

		public static TLocalizedText Lm9101101 => null;

		public static TLocalizedText Lm910101 => null;

		public static TLocalizedText Lm910102 => null;

		public static TLocalizedText Lm910220211 => null;

		public static TLocalizedText Lm910220221 => null;

		public static TLocalizedText Lm910220231 => null;

		public static TLocalizedText Lm910220241 => null;

		public static TLocalizedText Lm9102202 => null;

		public static TLocalizedText Lm910230211 => null;

		public static TLocalizedText Lm910230221 => null;

		public static TLocalizedText Lm910230231 => null;

		public static TLocalizedText Lm910230241 => null;

		public static TLocalizedText Lm9102302 => null;

		public static TLocalizedText Lm9102111 => null;

		public static TLocalizedText Lm910220201 => null;

		public static TLocalizedText Lm910230201 => null;

		public static TLocalizedText Lm9102121 => null;

		public static TLocalizedText Lm9102131 => null;

		public static TLocalizedText Lm9102141 => null;

		public static TLocalizedText Lm9102101 => null;

		public static TLocalizedText Lm910201 => null;

		public static TLocalizedText Lm910202 => null;

		public static TLocalizedText Lm910320211 => null;

		public static TLocalizedText Lm910320221 => null;

		public static TLocalizedText Lm910320231 => null;

		public static TLocalizedText Lm910320241 => null;

		public static TLocalizedText Lm9103202 => null;

		public static TLocalizedText Lm910330211 => null;

		public static TLocalizedText Lm910330221 => null;

		public static TLocalizedText Lm910330231 => null;

		public static TLocalizedText Lm910330241 => null;

		public static TLocalizedText Lm9103302 => null;

		public static TLocalizedText Lm9103111 => null;

		public static TLocalizedText Lm910320201 => null;

		public static TLocalizedText Lm910330201 => null;

		public static TLocalizedText Lm9103121 => null;

		public static TLocalizedText Lm9103131 => null;

		public static TLocalizedText Lm9103141 => null;

		public static TLocalizedText Lm9103101 => null;

		public static TLocalizedText Lm910301 => null;

		public static TLocalizedText Lm910302 => null;

		public static TLocalizedText Lm910420211 => null;

		public static TLocalizedText Lm910420221 => null;

		public static TLocalizedText Lm910420231 => null;

		public static TLocalizedText Lm910420241 => null;

		public static TLocalizedText Lm9104202 => null;

		public static TLocalizedText Lm910430211 => null;

		public static TLocalizedText Lm910430221 => null;

		public static TLocalizedText Lm910430231 => null;

		public static TLocalizedText Lm910430241 => null;

		public static TLocalizedText Lm9104302 => null;

		public static TLocalizedText Lm9104111 => null;

		public static TLocalizedText Lm910420201 => null;

		public static TLocalizedText Lm910430201 => null;

		public static TLocalizedText Lm9104121 => null;

		public static TLocalizedText Lm9104131 => null;

		public static TLocalizedText Lm9104141 => null;

		public static TLocalizedText Lm9104101 => null;

		public static TLocalizedText Lm910401 => null;

		public static TLocalizedText Lm910402 => null;

		public static TLocalizedText Lm910520211 => null;

		public static TLocalizedText Lm910520221 => null;

		public static TLocalizedText Lm910520231 => null;

		public static TLocalizedText Lm910520241 => null;

		public static TLocalizedText Lm9105202 => null;

		public static TLocalizedText Lm910530211 => null;

		public static TLocalizedText Lm910530221 => null;

		public static TLocalizedText Lm910530231 => null;

		public static TLocalizedText Lm910530241 => null;

		public static TLocalizedText Lm9105302 => null;

		public static TLocalizedText Lm9105111 => null;

		public static TLocalizedText Lm910520201 => null;

		public static TLocalizedText Lm910530201 => null;

		public static TLocalizedText Lm9105121 => null;

		public static TLocalizedText Lm9105131 => null;

		public static TLocalizedText Lm9105141 => null;

		public static TLocalizedText Lm9105101 => null;

		public static TLocalizedText Lm910501 => null;

		public static TLocalizedText Lm910502 => null;

		public static TLocalizedText Lm911120211 => null;

		public static TLocalizedText Lm911120221 => null;

		public static TLocalizedText Lm911120231 => null;

		public static TLocalizedText Lm911120241 => null;

		public static TLocalizedText Lm9111202 => null;

		public static TLocalizedText Lm911130211 => null;

		public static TLocalizedText Lm911130221 => null;

		public static TLocalizedText Lm911130231 => null;

		public static TLocalizedText Lm911130241 => null;

		public static TLocalizedText Lm9111302 => null;

		public static TLocalizedText Lm9111111 => null;

		public static TLocalizedText Lm911120201 => null;

		public static TLocalizedText Lm911130201 => null;

		public static TLocalizedText Lm9111121 => null;

		public static TLocalizedText Lm9111131 => null;

		public static TLocalizedText Lm9111141 => null;

		public static TLocalizedText Lm9111101 => null;

		public static TLocalizedText Lm911101 => null;

		public static TLocalizedText Lm911102 => null;

		public static TLocalizedText Lm911103 => null;

		public static TLocalizedText Lm911104 => null;

		public static TLocalizedText Lm911105 => null;

		public static TLocalizedText Lm911106 => null;

		public static TLocalizedText Lm911220211 => null;

		public static TLocalizedText Lm911220221 => null;

		public static TLocalizedText Lm911220231 => null;

		public static TLocalizedText Lm911220241 => null;

		public static TLocalizedText Lm9112202 => null;

		public static TLocalizedText Lm911230211 => null;

		public static TLocalizedText Lm911230221 => null;

		public static TLocalizedText Lm911230231 => null;

		public static TLocalizedText Lm911230241 => null;

		public static TLocalizedText Lm9112302 => null;

		public static TLocalizedText Lm911240211 => null;

		public static TLocalizedText Lm911240221 => null;

		public static TLocalizedText Lm911240231 => null;

		public static TLocalizedText Lm911240241 => null;

		public static TLocalizedText Lm9112402 => null;

		public static TLocalizedText Lm9112111 => null;

		public static TLocalizedText Lm911220201 => null;

		public static TLocalizedText Lm911230201 => null;

		public static TLocalizedText Lm911240201 => null;

		public static TLocalizedText Lm9112121 => null;

		public static TLocalizedText Lm9112131 => null;

		public static TLocalizedText Lm9112141 => null;

		public static TLocalizedText Lm9112101 => null;

		public static TLocalizedText Lm911205 => null;

		public static TLocalizedText Lm911206 => null;

		public static TLocalizedText Lm911320211 => null;

		public static TLocalizedText Lm911320221 => null;

		public static TLocalizedText Lm911320231 => null;

		public static TLocalizedText Lm911320241 => null;

		public static TLocalizedText Lm9113202 => null;

		public static TLocalizedText Lm911330211 => null;

		public static TLocalizedText Lm911330221 => null;

		public static TLocalizedText Lm911330231 => null;

		public static TLocalizedText Lm911330241 => null;

		public static TLocalizedText Lm9113302 => null;

		public static TLocalizedText Lm911340211 => null;

		public static TLocalizedText Lm911340221 => null;

		public static TLocalizedText Lm911340231 => null;

		public static TLocalizedText Lm911340241 => null;

		public static TLocalizedText Lm9113402 => null;

		public static TLocalizedText Lm9113111 => null;

		public static TLocalizedText Lm911320201 => null;

		public static TLocalizedText Lm911330201 => null;

		public static TLocalizedText Lm911340201 => null;

		public static TLocalizedText Lm9113121 => null;

		public static TLocalizedText Lm9113131 => null;

		public static TLocalizedText Lm9113141 => null;

		public static TLocalizedText Lm9113101 => null;

		public static TLocalizedText Lm911305 => null;

		public static TLocalizedText Lm911306 => null;

		public static TLocalizedText Lm911420211 => null;

		public static TLocalizedText Lm911420221 => null;

		public static TLocalizedText Lm911420231 => null;

		public static TLocalizedText Lm911420241 => null;

		public static TLocalizedText Lm9114202 => null;

		public static TLocalizedText Lm911430211 => null;

		public static TLocalizedText Lm911430221 => null;

		public static TLocalizedText Lm911430231 => null;

		public static TLocalizedText Lm911430241 => null;

		public static TLocalizedText Lm9114302 => null;

		public static TLocalizedText Lm911440211 => null;

		public static TLocalizedText Lm911440221 => null;

		public static TLocalizedText Lm911440231 => null;

		public static TLocalizedText Lm911440241 => null;

		public static TLocalizedText Lm9114402 => null;

		public static TLocalizedText Lm9114111 => null;

		public static TLocalizedText Lm911420201 => null;

		public static TLocalizedText Lm911430201 => null;

		public static TLocalizedText Lm911440201 => null;

		public static TLocalizedText Lm9114121 => null;

		public static TLocalizedText Lm9114131 => null;

		public static TLocalizedText Lm9114141 => null;

		public static TLocalizedText Lm9114101 => null;

		public static TLocalizedText Lm911405 => null;

		public static TLocalizedText Lm911406 => null;

		public static TLocalizedText Lm911520211 => null;

		public static TLocalizedText Lm911520221 => null;

		public static TLocalizedText Lm911520231 => null;

		public static TLocalizedText Lm911520241 => null;

		public static TLocalizedText Lm9115202 => null;

		public static TLocalizedText Lm911530211 => null;

		public static TLocalizedText Lm911530221 => null;

		public static TLocalizedText Lm911530231 => null;

		public static TLocalizedText Lm911530241 => null;

		public static TLocalizedText Lm9115302 => null;

		public static TLocalizedText Lm911540211 => null;

		public static TLocalizedText Lm911540221 => null;

		public static TLocalizedText Lm911540231 => null;

		public static TLocalizedText Lm911540241 => null;

		public static TLocalizedText Lm9115402 => null;

		public static TLocalizedText Lm9115111 => null;

		public static TLocalizedText Lm911520201 => null;

		public static TLocalizedText Lm911530201 => null;

		public static TLocalizedText Lm911540201 => null;

		public static TLocalizedText Lm9115121 => null;

		public static TLocalizedText Lm9115131 => null;

		public static TLocalizedText Lm9115141 => null;

		public static TLocalizedText Lm9115101 => null;

		public static TLocalizedText Lm911505 => null;

		public static TLocalizedText Lm911506 => null;

		public static TLocalizedText Lm911620211 => null;

		public static TLocalizedText Lm911620221 => null;

		public static TLocalizedText Lm911620231 => null;

		public static TLocalizedText Lm911620241 => null;

		public static TLocalizedText Lm9116202 => null;

		public static TLocalizedText Lm911630211 => null;

		public static TLocalizedText Lm911630221 => null;

		public static TLocalizedText Lm911630231 => null;

		public static TLocalizedText Lm911630241 => null;

		public static TLocalizedText Lm9116302 => null;

		public static TLocalizedText Lm911640211 => null;

		public static TLocalizedText Lm911640221 => null;

		public static TLocalizedText Lm911640231 => null;

		public static TLocalizedText Lm911640241 => null;

		public static TLocalizedText Lm9116402 => null;

		public static TLocalizedText Lm9116111 => null;

		public static TLocalizedText Lm911620201 => null;

		public static TLocalizedText Lm911630201 => null;

		public static TLocalizedText Lm911640201 => null;

		public static TLocalizedText Lm9116121 => null;

		public static TLocalizedText Lm9116131 => null;

		public static TLocalizedText Lm9116141 => null;

		public static TLocalizedText Lm9116101 => null;

		public static TLocalizedText Lm911605 => null;

		public static TLocalizedText Lm911606 => null;

		public static TLocalizedText Lm9117101 => null;

		public static TLocalizedText Lm911720401 => null;

		public static TLocalizedText Lm911720501 => null;

		public static TLocalizedText Lm911720601 => null;

		public static TLocalizedText Lm911720201 => null;

		public static TLocalizedText Lm9117111 => null;

		public static TLocalizedText Lm9117121 => null;

		public static TLocalizedText Lm9117131 => null;

		public static TLocalizedText Lm9117141 => null;

		public static TLocalizedText Lm911720211 => null;

		public static TLocalizedText Lm911720221 => null;

		public static TLocalizedText Lm911720231 => null;

		public static TLocalizedText Lm911720241 => null;

		public static TLocalizedText Lm9117202 => null;

		public static TLocalizedText Lm911720411 => null;

		public static TLocalizedText Lm911720421 => null;

		public static TLocalizedText Lm911720431 => null;

		public static TLocalizedText Lm911720441 => null;

		public static TLocalizedText Lm9117204 => null;

		public static TLocalizedText Lm911720511 => null;

		public static TLocalizedText Lm911720521 => null;

		public static TLocalizedText Lm911720531 => null;

		public static TLocalizedText Lm911720541 => null;

		public static TLocalizedText Lm9117205 => null;

		public static TLocalizedText Lm911720611 => null;

		public static TLocalizedText Lm911720621 => null;

		public static TLocalizedText Lm911720631 => null;

		public static TLocalizedText Lm911720641 => null;

		public static TLocalizedText Lm9117206 => null;

		public static TLocalizedText Lm911705 => null;

		public static TLocalizedText Lm911706 => null;

		public static TLocalizedText Lm911707 => null;

		public static TLocalizedText Lm911820211 => null;

		public static TLocalizedText Lm911820221 => null;

		public static TLocalizedText Lm911820231 => null;

		public static TLocalizedText Lm911820241 => null;

		public static TLocalizedText Lm9118202 => null;

		public static TLocalizedText Lm911830211 => null;

		public static TLocalizedText Lm911830221 => null;

		public static TLocalizedText Lm911830231 => null;

		public static TLocalizedText Lm911830241 => null;

		public static TLocalizedText Lm9118302 => null;

		public static TLocalizedText Lm911840211 => null;

		public static TLocalizedText Lm911840221 => null;

		public static TLocalizedText Lm911840231 => null;

		public static TLocalizedText Lm911840241 => null;

		public static TLocalizedText Lm9118402 => null;

		public static TLocalizedText Lm9118111 => null;

		public static TLocalizedText Lm911820201 => null;

		public static TLocalizedText Lm911830201 => null;

		public static TLocalizedText Lm911840201 => null;

		public static TLocalizedText Lm9118121 => null;

		public static TLocalizedText Lm9118131 => null;

		public static TLocalizedText Lm9118141 => null;

		public static TLocalizedText Lm9118101 => null;

		public static TLocalizedText Lm911805 => null;

		public static TLocalizedText Lm911806 => null;

		public static TLocalizedText Lm9121003 => null;

		public static TLocalizedText Lm9121001 => null;

		public static TLocalizedText Lm912120211 => null;

		public static TLocalizedText Lm912120221 => null;

		public static TLocalizedText Lm912120231 => null;

		public static TLocalizedText Lm912120241 => null;

		public static TLocalizedText Lm9121202 => null;

		public static TLocalizedText Lm912130211 => null;

		public static TLocalizedText Lm912130221 => null;

		public static TLocalizedText Lm912130231 => null;

		public static TLocalizedText Lm912130241 => null;

		public static TLocalizedText Lm9121302 => null;

		public static TLocalizedText Lm912140511 => null;

		public static TLocalizedText Lm912140521 => null;

		public static TLocalizedText Lm912140531 => null;

		public static TLocalizedText Lm912140541 => null;

		public static TLocalizedText Lm9121405 => null;

		public static TLocalizedText Lm9121111 => null;

		public static TLocalizedText Lm912120201 => null;

		public static TLocalizedText Lm912130201 => null;

		public static TLocalizedText Lm9121121 => null;

		public static TLocalizedText Lm9121131 => null;

		public static TLocalizedText Lm9121141 => null;

		public static TLocalizedText Lm9121101 => null;

		public static TLocalizedText Lm9121102 => null;

		public static TLocalizedText Lm912140501 => null;

		public static TLocalizedText Lm912140502 => null;

		public static TLocalizedText Lm912140503 => null;

		public static TLocalizedText Lm912101 => null;

		public static TLocalizedText Lm912105 => null;

		public static TLocalizedText Lm912104 => null;

		public static TLocalizedText Lm912102 => null;

		public static TLocalizedText Lm9122003 => null;

		public static TLocalizedText Lm9122001 => null;

		public static TLocalizedText Lm912220211 => null;

		public static TLocalizedText Lm912220221 => null;

		public static TLocalizedText Lm912220231 => null;

		public static TLocalizedText Lm912220241 => null;

		public static TLocalizedText Lm9122202 => null;

		public static TLocalizedText Lm912230211 => null;

		public static TLocalizedText Lm912230221 => null;

		public static TLocalizedText Lm912230231 => null;

		public static TLocalizedText Lm912230241 => null;

		public static TLocalizedText Lm9122302 => null;

		public static TLocalizedText Lm912240511 => null;

		public static TLocalizedText Lm912240521 => null;

		public static TLocalizedText Lm912240531 => null;

		public static TLocalizedText Lm912240541 => null;

		public static TLocalizedText Lm9122405 => null;

		public static TLocalizedText Lm9122111 => null;

		public static TLocalizedText Lm912220201 => null;

		public static TLocalizedText Lm912230201 => null;

		public static TLocalizedText Lm9122121 => null;

		public static TLocalizedText Lm9122131 => null;

		public static TLocalizedText Lm9122141 => null;

		public static TLocalizedText Lm9122101 => null;

		public static TLocalizedText Lm9122102 => null;

		public static TLocalizedText Lm912240501 => null;

		public static TLocalizedText Lm912240502 => null;

		public static TLocalizedText Lm912240503 => null;

		public static TLocalizedText Lm912201 => null;

		public static TLocalizedText Lm912205 => null;

		public static TLocalizedText Lm912204 => null;

		public static TLocalizedText Lm912202 => null;

		public static TLocalizedText Lm9123001 => null;

		public static TLocalizedText Lm9123202 => null;

		public static TLocalizedText Lm9123302 => null;

		public static TLocalizedText Lm912340211 => null;

		public static TLocalizedText Lm912340221 => null;

		public static TLocalizedText Lm912340231 => null;

		public static TLocalizedText Lm912340241 => null;

		public static TLocalizedText Lm9123402 => null;

		public static TLocalizedText Lm9123111 => null;

		public static TLocalizedText Lm912320201 => null;

		public static TLocalizedText Lm912330201 => null;

		public static TLocalizedText Lm912340201 => null;

		public static TLocalizedText Lm9123121 => null;

		public static TLocalizedText Lm9123131 => null;

		public static TLocalizedText Lm9123141 => null;

		public static TLocalizedText Lm9123101 => null;

		public static TLocalizedText Lm912305 => null;

		public static TLocalizedText Lm912306 => null;

		public static TLocalizedText Lm91241102 => null;

		public static TLocalizedText Lm91242102 => null;

		public static TLocalizedText Lm91243102 => null;

		public static TLocalizedText Lm91241101 => null;

		public static TLocalizedText Lm9124110201 => null;

		public static TLocalizedText Lm9124130201 => null;

		public static TLocalizedText Lm91242101 => null;

		public static TLocalizedText Lm9124210201 => null;

		public static TLocalizedText Lm9124230201 => null;

		public static TLocalizedText Lm91243101 => null;

		public static TLocalizedText Lm9124310201 => null;

		public static TLocalizedText Lm9124330201 => null;

		public static TLocalizedText Lm9124130211 => null;

		public static TLocalizedText Lm9124130221 => null;

		public static TLocalizedText Lm9124130231 => null;

		public static TLocalizedText Lm9124130241 => null;

		public static TLocalizedText Lm91241302 => null;

		public static TLocalizedText Lm9124230211 => null;

		public static TLocalizedText Lm9124230221 => null;

		public static TLocalizedText Lm9124230231 => null;

		public static TLocalizedText Lm9124230241 => null;

		public static TLocalizedText Lm91242302 => null;

		public static TLocalizedText Lm9124330211 => null;

		public static TLocalizedText Lm9124330221 => null;

		public static TLocalizedText Lm9124330231 => null;

		public static TLocalizedText Lm9124330241 => null;

		public static TLocalizedText Lm91243302 => null;

		public static TLocalizedText Lm912401 => null;

		public static TLocalizedText Lm912402 => null;

		public static TLocalizedText Lm912403 => null;

		public static TLocalizedText Ll890000101 => null;

		public static TLocalizedText Ll890000102 => null;

		public static TLocalizedText Ll890001201 => null;

		public static TLocalizedText Ll890001202 => null;

		public static TLocalizedText Ll890001203 => null;

		public static TLocalizedText Ll890002301 => null;

		public static TLocalizedText Ll890002302 => null;

		public static TLocalizedText Ll890002303 => null;

		public static TLocalizedText Ll890003401 => null;

		public static TLocalizedText Ll890003402 => null;

		public static TLocalizedText Ll890003403 => null;

		public static TLocalizedText Ll890004501 => null;

		public static TLocalizedText Ll890004502 => null;

		public static TLocalizedText Ll890004503 => null;

		public static TLocalizedText Ll890005601 => null;

		public static TLocalizedText Ll890005602 => null;

		public static TLocalizedText Ll890005603 => null;

		public static TLocalizedText Ll890006701 => null;

		public static TLocalizedText Ll890006702 => null;

		public static TLocalizedText Ll890006703 => null;

		public static TLocalizedText Ll890100801 => null;

		public static TLocalizedText Ll890100802 => null;

		public static TLocalizedText Ll890100803 => null;

		public static TLocalizedText Ll890101901 => null;

		public static TLocalizedText Ll890101902 => null;

		public static TLocalizedText Ll890101903 => null;

		public static TLocalizedText Ll890101904 => null;

		public static TLocalizedText Ll8901021001 => null;

		public static TLocalizedText Ll8901021002 => null;

		public static TLocalizedText Ll8901031101 => null;

		public static TLocalizedText Ll8901031102 => null;

		public static TLocalizedText Ll8901031103 => null;

		public static TLocalizedText Ll8901041201 => null;

		public static TLocalizedText Ll8901041202 => null;

		public static TLocalizedText Ll8901041203 => null;

		public static TLocalizedText Ll8902001301 => null;

		public static TLocalizedText Ll8902001302 => null;

		public static TLocalizedText Ll8903001401 => null;

		public static TLocalizedText Ll8903001402 => null;

		public static TLocalizedText Ll8903001403 => null;

		public static TLocalizedText Ll8903011501 => null;

		public static TLocalizedText Ll8903011502 => null;

		public static TLocalizedText Ll8903011503 => null;

		public static TLocalizedText Ll8903021601 => null;

		public static TLocalizedText Ll8903021602 => null;

		public static TLocalizedText Ll8903021603 => null;

		public static TLocalizedText Ll8903031701 => null;

		public static TLocalizedText Ll8903031702 => null;

		public static TLocalizedText Ll8903031703 => null;

		public static TLocalizedText Ll8903041801 => null;

		public static TLocalizedText Ll8903041802 => null;

		public static TLocalizedText Ll8903041803 => null;

		public static TLocalizedText Ll8904001901 => null;

		public static TLocalizedText Ll8905002001 => null;

		public static TLocalizedText Ll8905002002 => null;

		public static TLocalizedText Ll8905002003 => null;

		public static TLocalizedText Ll8905012101 => null;

		public static TLocalizedText Ll8905012102 => null;

		public static TLocalizedText Ll8905012103 => null;

		public static TLocalizedText Ll8906002201 => null;

		public static TLocalizedText Ll8906002202 => null;

		public static TLocalizedText Ll8906002203 => null;

		public static TLocalizedText Ll8906012301 => null;

		public static TLocalizedText Ll8906012302 => null;

		public static TLocalizedText Ll8906012303 => null;

		public static TLocalizedText Ll8908002401 => null;

		public static TLocalizedText Ll8908002402 => null;

		public static TLocalizedText Ll8908002403 => null;

		public static TLocalizedText Ll8908012501 => null;

		public static TLocalizedText Ll8908012502 => null;

		public static TLocalizedText Ll8908012503 => null;

		public static TLocalizedText Ll8908022601 => null;

		public static TLocalizedText Ll8908022602 => null;

		public static TLocalizedText Ll8908032701 => null;

		public static TLocalizedText Ll8908032702 => null;

		public static TLocalizedText Ll8908032703 => null;

		public static TLocalizedText Ll8908042801 => null;

		public static TLocalizedText Ll8908042802 => null;

		public static TLocalizedText Ll8908042803 => null;

		public static TLocalizedText Ll8908052901 => null;

		public static TLocalizedText Ll8908052902 => null;

		public static TLocalizedText Ll8911003001 => null;

		public static TLocalizedText Ll8912003101 => null;

		public static TLocalizedText Ll8913003201 => null;

		public static TLocalizedText Ll8914003301 => null;

		public static TLocalizedText Ll8915003401 => null;

		public static TLocalizedText Ll8915003402 => null;

		public static TLocalizedText Ll8915003403 => null;

		public static TLocalizedText Ll8915013501 => null;

		public static TLocalizedText Ll8915013502 => null;

		public static TLocalizedText Ll8915023601 => null;

		public static TLocalizedText Ll8915023602 => null;

		public static TLocalizedText Ll8915023603 => null;

		public static TLocalizedText Ll8915033701 => null;

		public static TLocalizedText Ll8915033702 => null;

		public static TLocalizedText Ll8915033703 => null;

		public static TLocalizedText Ll8916003801 => null;

		public static TLocalizedText Ll8916003802 => null;

		public static TLocalizedText Ll8916003803 => null;

		public static TLocalizedText Ll8916013901 => null;

		public static TLocalizedText Ll8916013902 => null;

		public static TLocalizedText Ll8916013903 => null;

		public static TLocalizedText Ll8916013904 => null;

		public static TLocalizedText Ll8916024001 => null;

		public static TLocalizedText Ll8916024002 => null;

		public static TLocalizedText Ll8916024003 => null;

		public static TLocalizedText Ll8916034101 => null;

		public static TLocalizedText Ll8916034102 => null;

		public static TLocalizedText Ll8916034103 => null;

		public static TLocalizedText Ll8601004201 => null;

		public static TLocalizedText Ll8601004202 => null;

		public static TLocalizedText Ll8601014301 => null;

		public static TLocalizedText Ll8601014302 => null;

		public static TLocalizedText Ll8602004401 => null;

		public static TLocalizedText Ll8602004402 => null;

		public static TLocalizedText Ll8602014501 => null;

		public static TLocalizedText Ll8602014502 => null;

		public static TLocalizedText Ll8603004601 => null;

		public static TLocalizedText Ll8603004602 => null;

		public static TLocalizedText Ll8603014701 => null;

		public static TLocalizedText Ll8603014702 => null;

		public static TLocalizedText Ll8604004801 => null;

		public static TLocalizedText Ll8604004802 => null;

		public static TLocalizedText Ll8604014901 => null;

		public static TLocalizedText Ll8604014902 => null;

		public static TLocalizedText Ll8605005001 => null;

		public static TLocalizedText Ll8605005002 => null;

		public static TLocalizedText Ll8605015101 => null;

		public static TLocalizedText Ll8605015102 => null;

		public static TLocalizedText Ll8606005201 => null;

		public static TLocalizedText Ll8606005202 => null;

		public static TLocalizedText Ll8606015301 => null;

		public static TLocalizedText Ll8606015302 => null;

		public static TLocalizedText Ll8607005401 => null;

		public static TLocalizedText Ll8607005402 => null;

		public static TLocalizedText Ll8607015501 => null;

		public static TLocalizedText Ll8607015502 => null;

		public static TLocalizedText Ll8608005601 => null;

		public static TLocalizedText Ll8608005602 => null;

		public static TLocalizedText Ll8608015701 => null;

		public static TLocalizedText Ll8608015702 => null;

		public static TLocalizedText Ll8609005801 => null;

		public static TLocalizedText Ll8609005802 => null;

		public static TLocalizedText Ll8609015901 => null;

		public static TLocalizedText Ll8609015902 => null;

		public static TLocalizedText Ll8610006001 => null;

		public static TLocalizedText Ll8610006002 => null;

		public static TLocalizedText Ll8610016101 => null;

		public static TLocalizedText Ll8610016102 => null;

		public static TLocalizedText Ll8611006201 => null;

		public static TLocalizedText Ll8611006202 => null;

		public static TLocalizedText Ll8611016301 => null;

		public static TLocalizedText Ll8611016302 => null;

		public static TLocalizedText Ll8917006401 => null;

		public static TLocalizedText TalkDialog890000Title => null;

		public static TLocalizedText TalkDialog890001Title => null;

		public static TLocalizedText TalkDialog890002Title => null;

		public static TLocalizedText TalkDialog890003Title => null;

		public static TLocalizedText TalkDialog890004Title => null;

		public static TLocalizedText TalkDialog890005Title => null;

		public static TLocalizedText TalkDialog890006Title => null;

		public static TLocalizedText TalkDialog890100Title => null;

		public static TLocalizedText TalkDialog890101Title => null;

		public static TLocalizedText TalkDialog890102Title => null;

		public static TLocalizedText TalkDialog890103Title => null;

		public static TLocalizedText TalkDialog890104Title => null;

		public static TLocalizedText TalkDialog890200Title => null;

		public static TLocalizedText TalkDialog890300Title => null;

		public static TLocalizedText TalkDialog890301Title => null;

		public static TLocalizedText TalkDialog890302Title => null;

		public static TLocalizedText TalkDialog890303Title => null;

		public static TLocalizedText TalkDialog890304Title => null;

		public static TLocalizedText TalkDialog890400Title => null;

		public static TLocalizedText TalkDialog890500Title => null;

		public static TLocalizedText TalkDialog890501Title => null;

		public static TLocalizedText TalkDialog890600Title => null;

		public static TLocalizedText TalkDialog890601Title => null;

		public static TLocalizedText TalkDialog890800Title => null;

		public static TLocalizedText TalkDialog890801Title => null;

		public static TLocalizedText TalkDialog890802Title => null;

		public static TLocalizedText TalkDialog890803Title => null;

		public static TLocalizedText TalkDialog890804Title => null;

		public static TLocalizedText TalkDialog890805Title => null;

		public static TLocalizedText TalkDialog891100Title => null;

		public static TLocalizedText TalkDialog891200Title => null;

		public static TLocalizedText TalkDialog891300Title => null;

		public static TLocalizedText TalkDialog891400Title => null;

		public static TLocalizedText TalkDialog891500Title => null;

		public static TLocalizedText TalkDialog891501Title => null;

		public static TLocalizedText TalkDialog891502Title => null;

		public static TLocalizedText TalkDialog891503Title => null;

		public static TLocalizedText TalkDialog891600Title => null;

		public static TLocalizedText TalkDialog891601Title => null;

		public static TLocalizedText TalkDialog891602Title => null;

		public static TLocalizedText TalkDialog891603Title => null;

		public static TLocalizedText TalkDialog860100Title => null;

		public static TLocalizedText TalkDialog860101Title => null;

		public static TLocalizedText TalkDialog860200Title => null;

		public static TLocalizedText TalkDialog860201Title => null;

		public static TLocalizedText TalkDialog860300Title => null;

		public static TLocalizedText TalkDialog860301Title => null;

		public static TLocalizedText TalkDialog860400Title => null;

		public static TLocalizedText TalkDialog860401Title => null;

		public static TLocalizedText TalkDialog860500Title => null;

		public static TLocalizedText TalkDialog860501Title => null;

		public static TLocalizedText TalkDialog860600Title => null;

		public static TLocalizedText TalkDialog860601Title => null;

		public static TLocalizedText TalkDialog860700Title => null;

		public static TLocalizedText TalkDialog860701Title => null;

		public static TLocalizedText TalkDialog860800Title => null;

		public static TLocalizedText TalkDialog860801Title => null;

		public static TLocalizedText TalkDialog860900Title => null;

		public static TLocalizedText TalkDialog860901Title => null;

		public static TLocalizedText TalkDialog861000Title => null;

		public static TLocalizedText TalkDialog861001Title => null;

		public static TLocalizedText TalkDialog861100Title => null;

		public static TLocalizedText TalkDialog861101Title => null;

		public static TLocalizedText TalkDialog891700Title => null;

		public static TLocalizedText LlCaptureReport301 => null;

		public static TLocalizedText LlCaptureReport302 => null;

		public static TLocalizedText LlCaptureReport303 => null;

		public static TLocalizedText LCommon1001 => null;

		public static TLocalizedText LCommon1002 => null;

		public static TLocalizedText LCommon1003 => null;

		public static TLocalizedText LCommon1004 => null;

		public static TLocalizedText LCommon1005 => null;

		public static TLocalizedText LCommon1006 => null;

		public static TLocalizedText LCommon1007 => null;

		public static TLocalizedText LCommon1008 => null;

		public static TLocalizedText Ll82101T01 => null;

		public static TLocalizedText Ll82101T02 => null;

		public static TLocalizedText Ll82101T03 => null;

		public static TLocalizedText Ll82101T04 => null;

		public static TLocalizedText Ll82101T05 => null;

		public static TLocalizedText Ll82101T06 => null;

		public static TLocalizedText Ll82203101 => null;

		public static TLocalizedText Ll82203102 => null;

		public static TLocalizedText Ll82205101 => null;

		public static TLocalizedText Ll82205102 => null;

		public static TLocalizedText Ll82206101 => null;

		public static TLocalizedText Ll82206102 => null;

		public static TLocalizedText Ll82207101 => null;

		public static TLocalizedText Ll82207102 => null;

		public static TLocalizedText LqCommonNotReady => null;

		public static TLocalizedText LNoneStart => null;

		public static TLocalizedText LCommon4002 => null;

		public static TLocalizedText LCommon6001 => null;

		public static TLocalizedText LCommon2002 => null;

		public static TLocalizedText LCommon2003 => null;

		public static TLocalizedText LCommon2004 => null;

		public static TLocalizedText LCommon2001 => null;

		public static TLocalizedText LCommon2030 => null;

		public static TLocalizedText LCommon2031 => null;

		public static TLocalizedText LCommon2032 => null;

		public static TLocalizedText LCommon2033 => null;

		public static TLocalizedText LCommon2034 => null;

		public static TLocalizedText LCommon2035 => null;

		public static TLocalizedText LCommon2036 => null;

		public static TLocalizedText LCommon2040 => null;

		public static TLocalizedText LCommon2041 => null;

		public static TLocalizedText LCommon2042 => null;

		public static TLocalizedText LCommon2043 => null;

		public static TLocalizedText LCommon2044 => null;

		public static TLocalizedText LCommon2050 => null;

		public static TLocalizedText LCommon2051 => null;

		public static TLocalizedText LCommon3001 => null;

		public static TLocalizedText LCommon4001 => null;

		public static TLocalizedText LCommon4011 => null;

		public static TLocalizedText LCommon4012 => null;

		public static TLocalizedText LCommon4021 => null;

		public static TLocalizedText LCommon4022 => null;

		public static TLocalizedText LCommon4031 => null;

		public static TLocalizedText LCommon4032 => null;

		public static TLocalizedText LCommon4041 => null;

		public static TLocalizedText LCommon4042 => null;

		public static TLocalizedText LCommon4052 => null;

		public static TLocalizedText LCommon4061 => null;

		public static TLocalizedText LCommon4062 => null;

		public static TLocalizedText LCommon4063 => null;

		public static TLocalizedText LCommon4064 => null;

		public static TLocalizedText LCommon4065 => null;

		public static TLocalizedText LCommon4066 => null;

		public static TLocalizedText LCommon5001 => null;

		public static TLocalizedText LCommon5002 => null;

		public static TLocalizedText LCommon5003 => null;

		public static TLocalizedText LCommon5004 => null;

		public static TLocalizedText LCommon5005 => null;

		public static TLocalizedText LCommon5006 => null;

		public static TLocalizedText LCommon5013 => null;

		public static TLocalizedText LCommon5014 => null;

		public static TLocalizedText LCommon5017 => null;

		public static TLocalizedText LCommon5018 => null;

		public static TLocalizedText LCommon5019 => null;

		public static TLocalizedText LCommon5020 => null;

		public static TLocalizedText LlCaptureReport401 => null;

		public static TLocalizedText LqRescueR101 => null;

		public static TLocalizedText LqRescueNpc01 => null;

		public static TLocalizedText LqRescueR102 => null;

		public static TLocalizedText Ll82112Npc02 => null;

		public static TLocalizedText Ll82112Npc01 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000015 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000015 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000011 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000012 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000013 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000014 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000025 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000025 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000021 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000022 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000023 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000024 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000035 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000035 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000031 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000032 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000033 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000034 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000045 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000045 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000041 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000042 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000043 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000044 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000055 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000055 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000051 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000052 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000053 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000054 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000065 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000065 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000061 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000062 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000063 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000064 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000075 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000075 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000071 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000072 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000073 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000074 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000085 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000085 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000081 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000082 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000083 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000084 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000095 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000095 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000091 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000092 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000093 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000094 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000105 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000105 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000101 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000102 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000103 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000104 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000115 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000115 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000111 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000112 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000113 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000114 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000125 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000125 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000121 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000122 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000123 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000124 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardFail6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessDislikeTag6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPreferTag6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnCardSuccessPrice6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealAccepted6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealGiveUp6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnDealStart6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationLowChance6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnHesitationMidChance6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnPlayCountExhausted6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnReroll6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultFail6000131 => null;

		public static TLocalizedText ArtifactDealTextBubbleTextsOnResultSuccess6000131 => null;

		public static TLocalizedText EMonsterCategoryBeast => null;

		public static TLocalizedText EMonsterCategoryBlasphemy => null;

		public static TLocalizedText EMonsterCategoryEldritch => null;

		public static TLocalizedText EMonsterCategoryHuman => null;

		public static TLocalizedText EMonsterCategoryMachine => null;

		public static TLocalizedText EMonsterCategoryPhantasm => null;

		public static TLocalizedText EMonsterCategoryPlant => null;

		public static TLocalizedText EmployeeTextFinishHighStressA21 => null;

		public static TLocalizedText EmployeeTextFinishHighStressA22 => null;

		public static TLocalizedText EmployeeTextFinishHighStressA1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressA2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressB1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressB2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressC21 => null;

		public static TLocalizedText EmployeeTextFinishHighStressC22 => null;

		public static TLocalizedText EmployeeTextFinishHighStressC1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressC2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressD1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressD2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressE21 => null;

		public static TLocalizedText EmployeeTextFinishHighStressE22 => null;

		public static TLocalizedText EmployeeTextFinishHighStressE1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressE2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressF1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressF2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressG1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressG2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressH1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressH2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressI1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressI2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressJ1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressJ2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressK1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressK2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressL1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressL2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressM1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressM2 => null;

		public static TLocalizedText EmployeeTextFinishHighStressN1 => null;

		public static TLocalizedText EmployeeTextFinishHighStressN2 => null;

		public static TLocalizedText EmployeeTextFinishPanicA21 => null;

		public static TLocalizedText EmployeeTextFinishPanicA22 => null;

		public static TLocalizedText EmployeeTextFinishPanicA1 => null;

		public static TLocalizedText EmployeeTextFinishPanicA2 => null;

		public static TLocalizedText EmployeeTextFinishPanicB1 => null;

		public static TLocalizedText EmployeeTextFinishPanicB2 => null;

		public static TLocalizedText EmployeeTextFinishPanicC21 => null;

		public static TLocalizedText EmployeeTextFinishPanicC22 => null;

		public static TLocalizedText EmployeeTextFinishPanicC1 => null;

		public static TLocalizedText EmployeeTextFinishPanicC2 => null;

		public static TLocalizedText EmployeeTextFinishPanicD1 => null;

		public static TLocalizedText EmployeeTextFinishPanicD2 => null;

		public static TLocalizedText EmployeeTextFinishPanicE21 => null;

		public static TLocalizedText EmployeeTextFinishPanicE22 => null;

		public static TLocalizedText EmployeeTextFinishPanicE1 => null;

		public static TLocalizedText EmployeeTextFinishPanicE2 => null;

		public static TLocalizedText EmployeeTextFinishPanicF1 => null;

		public static TLocalizedText EmployeeTextFinishPanicF2 => null;

		public static TLocalizedText EmployeeTextFinishPanicG1 => null;

		public static TLocalizedText EmployeeTextFinishPanicG2 => null;

		public static TLocalizedText EmployeeTextFinishPanicH1 => null;

		public static TLocalizedText EmployeeTextFinishPanicH2 => null;

		public static TLocalizedText EmployeeTextFinishPanicI1 => null;

		public static TLocalizedText EmployeeTextFinishPanicI2 => null;

		public static TLocalizedText EmployeeTextFinishPanicJ1 => null;

		public static TLocalizedText EmployeeTextFinishPanicJ2 => null;

		public static TLocalizedText EmployeeTextFinishPanicK1 => null;

		public static TLocalizedText EmployeeTextFinishPanicK2 => null;

		public static TLocalizedText EmployeeTextFinishPanicL1 => null;

		public static TLocalizedText EmployeeTextFinishPanicL2 => null;

		public static TLocalizedText EmployeeTextFinishPanicM1 => null;

		public static TLocalizedText EmployeeTextFinishPanicM2 => null;

		public static TLocalizedText EmployeeTextFinishPanicN1 => null;

		public static TLocalizedText EmployeeTextFinishPanicN2 => null;

		public static TLocalizedText EmployeeTextFinishA21 => null;

		public static TLocalizedText EmployeeTextFinishA22 => null;

		public static TLocalizedText EmployeeTextFinishA23 => null;

		public static TLocalizedText EmployeeTextFinishA24 => null;

		public static TLocalizedText EmployeeTextFinishA1 => null;

		public static TLocalizedText EmployeeTextFinishA2 => null;

		public static TLocalizedText EmployeeTextFinishA3 => null;

		public static TLocalizedText EmployeeTextFinishA4 => null;

		public static TLocalizedText EmployeeTextFinishB1 => null;

		public static TLocalizedText EmployeeTextFinishB2 => null;

		public static TLocalizedText EmployeeTextFinishB3 => null;

		public static TLocalizedText EmployeeTextFinishB4 => null;

		public static TLocalizedText EmployeeTextFinishC21 => null;

		public static TLocalizedText EmployeeTextFinishC22 => null;

		public static TLocalizedText EmployeeTextFinishC23 => null;

		public static TLocalizedText EmployeeTextFinishC24 => null;

		public static TLocalizedText EmployeeTextFinishC1 => null;

		public static TLocalizedText EmployeeTextFinishC2 => null;

		public static TLocalizedText EmployeeTextFinishC3 => null;

		public static TLocalizedText EmployeeTextFinishC4 => null;

		public static TLocalizedText EmployeeTextFinishD1 => null;

		public static TLocalizedText EmployeeTextFinishD2 => null;

		public static TLocalizedText EmployeeTextFinishD3 => null;

		public static TLocalizedText EmployeeTextFinishD4 => null;

		public static TLocalizedText EmployeeTextFinishE21 => null;

		public static TLocalizedText EmployeeTextFinishE22 => null;

		public static TLocalizedText EmployeeTextFinishE23 => null;

		public static TLocalizedText EmployeeTextFinishE24 => null;

		public static TLocalizedText EmployeeTextFinishE1 => null;

		public static TLocalizedText EmployeeTextFinishE2 => null;

		public static TLocalizedText EmployeeTextFinishE3 => null;

		public static TLocalizedText EmployeeTextFinishE4 => null;

		public static TLocalizedText EmployeeTextFinishF1 => null;

		public static TLocalizedText EmployeeTextFinishF2 => null;

		public static TLocalizedText EmployeeTextFinishF3 => null;

		public static TLocalizedText EmployeeTextFinishF4 => null;

		public static TLocalizedText EmployeeTextFinishG1 => null;

		public static TLocalizedText EmployeeTextFinishG2 => null;

		public static TLocalizedText EmployeeTextFinishG3 => null;

		public static TLocalizedText EmployeeTextFinishG4 => null;

		public static TLocalizedText EmployeeTextFinishH1 => null;

		public static TLocalizedText EmployeeTextFinishH2 => null;

		public static TLocalizedText EmployeeTextFinishH3 => null;

		public static TLocalizedText EmployeeTextFinishH4 => null;

		public static TLocalizedText EmployeeTextFinishI1 => null;

		public static TLocalizedText EmployeeTextFinishI2 => null;

		public static TLocalizedText EmployeeTextFinishI3 => null;

		public static TLocalizedText EmployeeTextFinishI4 => null;

		public static TLocalizedText EmployeeTextFinishJ1 => null;

		public static TLocalizedText EmployeeTextFinishJ2 => null;

		public static TLocalizedText EmployeeTextFinishJ3 => null;

		public static TLocalizedText EmployeeTextFinishJ4 => null;

		public static TLocalizedText EmployeeTextFinishK1 => null;

		public static TLocalizedText EmployeeTextFinishK2 => null;

		public static TLocalizedText EmployeeTextFinishK3 => null;

		public static TLocalizedText EmployeeTextFinishK4 => null;

		public static TLocalizedText EmployeeTextFinishL1 => null;

		public static TLocalizedText EmployeeTextFinishL2 => null;

		public static TLocalizedText EmployeeTextFinishL3 => null;

		public static TLocalizedText EmployeeTextFinishL4 => null;

		public static TLocalizedText EmployeeTextFinishM1 => null;

		public static TLocalizedText EmployeeTextFinishM2 => null;

		public static TLocalizedText EmployeeTextFinishM3 => null;

		public static TLocalizedText EmployeeTextFinishM4 => null;

		public static TLocalizedText EmployeeTextFinishN1 => null;

		public static TLocalizedText EmployeeTextFinishN2 => null;

		public static TLocalizedText EmployeeTextFinishN3 => null;

		public static TLocalizedText EmployeeTextFinishN4 => null;

		public static TLocalizedText EmployeeTextFirePanicA21 => null;

		public static TLocalizedText EmployeeTextFirePanicA22 => null;

		public static TLocalizedText EmployeeTextFirePanicA1 => null;

		public static TLocalizedText EmployeeTextFirePanicA2 => null;

		public static TLocalizedText EmployeeTextFirePanicB1 => null;

		public static TLocalizedText EmployeeTextFirePanicB2 => null;

		public static TLocalizedText EmployeeTextFirePanicC21 => null;

		public static TLocalizedText EmployeeTextFirePanicC22 => null;

		public static TLocalizedText EmployeeTextFirePanicC1 => null;

		public static TLocalizedText EmployeeTextFirePanicC2 => null;

		public static TLocalizedText EmployeeTextFirePanicD1 => null;

		public static TLocalizedText EmployeeTextFirePanicD2 => null;

		public static TLocalizedText EmployeeTextFirePanicE21 => null;

		public static TLocalizedText EmployeeTextFirePanicE22 => null;

		public static TLocalizedText EmployeeTextFirePanicE1 => null;

		public static TLocalizedText EmployeeTextFirePanicE2 => null;

		public static TLocalizedText EmployeeTextFirePanicF1 => null;

		public static TLocalizedText EmployeeTextFirePanicF2 => null;

		public static TLocalizedText EmployeeTextFirePanicG1 => null;

		public static TLocalizedText EmployeeTextFirePanicG2 => null;

		public static TLocalizedText EmployeeTextFirePanicH1 => null;

		public static TLocalizedText EmployeeTextFirePanicH2 => null;

		public static TLocalizedText EmployeeTextFirePanicI1 => null;

		public static TLocalizedText EmployeeTextFirePanicI2 => null;

		public static TLocalizedText EmployeeTextFirePanicJ1 => null;

		public static TLocalizedText EmployeeTextFirePanicJ2 => null;

		public static TLocalizedText EmployeeTextFirePanicK1 => null;

		public static TLocalizedText EmployeeTextFirePanicK2 => null;

		public static TLocalizedText EmployeeTextFirePanicL1 => null;

		public static TLocalizedText EmployeeTextFirePanicL2 => null;

		public static TLocalizedText EmployeeTextFirePanicM1 => null;

		public static TLocalizedText EmployeeTextFirePanicM2 => null;

		public static TLocalizedText EmployeeTextFirePanicN1 => null;

		public static TLocalizedText EmployeeTextFirePanicN2 => null;

		public static TLocalizedText EmployeeTextFireA21 => null;

		public static TLocalizedText EmployeeTextFireA22 => null;

		public static TLocalizedText EmployeeTextFireA23 => null;

		public static TLocalizedText EmployeeTextFireA24 => null;

		public static TLocalizedText EmployeeTextFireA1 => null;

		public static TLocalizedText EmployeeTextFireA2 => null;

		public static TLocalizedText EmployeeTextFireA3 => null;

		public static TLocalizedText EmployeeTextFireA4 => null;

		public static TLocalizedText EmployeeTextFireB1 => null;

		public static TLocalizedText EmployeeTextFireB2 => null;

		public static TLocalizedText EmployeeTextFireB3 => null;

		public static TLocalizedText EmployeeTextFireB4 => null;

		public static TLocalizedText EmployeeTextFireC21 => null;

		public static TLocalizedText EmployeeTextFireC22 => null;

		public static TLocalizedText EmployeeTextFireC23 => null;

		public static TLocalizedText EmployeeTextFireC24 => null;

		public static TLocalizedText EmployeeTextFireC1 => null;

		public static TLocalizedText EmployeeTextFireC2 => null;

		public static TLocalizedText EmployeeTextFireC3 => null;

		public static TLocalizedText EmployeeTextFireC4 => null;

		public static TLocalizedText EmployeeTextFireD1 => null;

		public static TLocalizedText EmployeeTextFireD2 => null;

		public static TLocalizedText EmployeeTextFireD3 => null;

		public static TLocalizedText EmployeeTextFireD4 => null;

		public static TLocalizedText EmployeeTextFireE21 => null;

		public static TLocalizedText EmployeeTextFireE22 => null;

		public static TLocalizedText EmployeeTextFireE23 => null;

		public static TLocalizedText EmployeeTextFireE24 => null;

		public static TLocalizedText EmployeeTextFireE1 => null;

		public static TLocalizedText EmployeeTextFireE2 => null;

		public static TLocalizedText EmployeeTextFireE3 => null;

		public static TLocalizedText EmployeeTextFireE4 => null;

		public static TLocalizedText EmployeeTextFireF1 => null;

		public static TLocalizedText EmployeeTextFireF2 => null;

		public static TLocalizedText EmployeeTextFireF3 => null;

		public static TLocalizedText EmployeeTextFireF4 => null;

		public static TLocalizedText EmployeeTextFireG1 => null;

		public static TLocalizedText EmployeeTextFireG2 => null;

		public static TLocalizedText EmployeeTextFireG3 => null;

		public static TLocalizedText EmployeeTextFireG4 => null;

		public static TLocalizedText EmployeeTextFireH1 => null;

		public static TLocalizedText EmployeeTextFireH2 => null;

		public static TLocalizedText EmployeeTextFireH3 => null;

		public static TLocalizedText EmployeeTextFireH4 => null;

		public static TLocalizedText EmployeeTextFireI1 => null;

		public static TLocalizedText EmployeeTextFireI2 => null;

		public static TLocalizedText EmployeeTextFireI3 => null;

		public static TLocalizedText EmployeeTextFireI4 => null;

		public static TLocalizedText EmployeeTextFireJ1 => null;

		public static TLocalizedText EmployeeTextFireJ2 => null;

		public static TLocalizedText EmployeeTextFireJ3 => null;

		public static TLocalizedText EmployeeTextFireJ4 => null;

		public static TLocalizedText EmployeeTextFireK1 => null;

		public static TLocalizedText EmployeeTextFireK2 => null;

		public static TLocalizedText EmployeeTextFireK3 => null;

		public static TLocalizedText EmployeeTextFireK4 => null;

		public static TLocalizedText EmployeeTextFireL1 => null;

		public static TLocalizedText EmployeeTextFireL2 => null;

		public static TLocalizedText EmployeeTextFireL3 => null;

		public static TLocalizedText EmployeeTextFireL4 => null;

		public static TLocalizedText EmployeeTextFireM1 => null;

		public static TLocalizedText EmployeeTextFireM2 => null;

		public static TLocalizedText EmployeeTextFireM3 => null;

		public static TLocalizedText EmployeeTextFireM4 => null;

		public static TLocalizedText EmployeeTextFireN1 => null;

		public static TLocalizedText EmployeeTextFireN2 => null;

		public static TLocalizedText EmployeeTextFireN3 => null;

		public static TLocalizedText EmployeeTextFireN4 => null;

		public static TLocalizedText EmployeeTextHireA21 => null;

		public static TLocalizedText EmployeeTextHireA22 => null;

		public static TLocalizedText EmployeeTextHireA23 => null;

		public static TLocalizedText EmployeeTextHireA24 => null;

		public static TLocalizedText EmployeeTextHireA1 => null;

		public static TLocalizedText EmployeeTextHireA2 => null;

		public static TLocalizedText EmployeeTextHireA3 => null;

		public static TLocalizedText EmployeeTextHireA4 => null;

		public static TLocalizedText EmployeeTextHireB1 => null;

		public static TLocalizedText EmployeeTextHireB2 => null;

		public static TLocalizedText EmployeeTextHireB3 => null;

		public static TLocalizedText EmployeeTextHireB4 => null;

		public static TLocalizedText EmployeeTextHireC21 => null;

		public static TLocalizedText EmployeeTextHireC22 => null;

		public static TLocalizedText EmployeeTextHireC23 => null;

		public static TLocalizedText EmployeeTextHireC24 => null;

		public static TLocalizedText EmployeeTextHireC1 => null;

		public static TLocalizedText EmployeeTextHireC2 => null;

		public static TLocalizedText EmployeeTextHireC3 => null;

		public static TLocalizedText EmployeeTextHireC4 => null;

		public static TLocalizedText EmployeeTextHireD1 => null;

		public static TLocalizedText EmployeeTextHireD2 => null;

		public static TLocalizedText EmployeeTextHireD3 => null;

		public static TLocalizedText EmployeeTextHireD4 => null;

		public static TLocalizedText EmployeeTextHireE21 => null;

		public static TLocalizedText EmployeeTextHireE22 => null;

		public static TLocalizedText EmployeeTextHireE23 => null;

		public static TLocalizedText EmployeeTextHireE24 => null;

		public static TLocalizedText EmployeeTextHireE1 => null;

		public static TLocalizedText EmployeeTextHireE2 => null;

		public static TLocalizedText EmployeeTextHireE3 => null;

		public static TLocalizedText EmployeeTextHireE4 => null;

		public static TLocalizedText EmployeeTextHireF1 => null;

		public static TLocalizedText EmployeeTextHireF2 => null;

		public static TLocalizedText EmployeeTextHireF3 => null;

		public static TLocalizedText EmployeeTextHireF4 => null;

		public static TLocalizedText EmployeeTextHireG1 => null;

		public static TLocalizedText EmployeeTextHireG2 => null;

		public static TLocalizedText EmployeeTextHireG3 => null;

		public static TLocalizedText EmployeeTextHireG4 => null;

		public static TLocalizedText EmployeeTextHireH11 => null;

		public static TLocalizedText EmployeeTextHireH12 => null;

		public static TLocalizedText EmployeeTextHireH13 => null;

		public static TLocalizedText EmployeeTextHireH14 => null;

		public static TLocalizedText EmployeeTextHireH21 => null;

		public static TLocalizedText EmployeeTextHireH22 => null;

		public static TLocalizedText EmployeeTextHireH23 => null;

		public static TLocalizedText EmployeeTextHireH24 => null;

		public static TLocalizedText EmployeeTextHireH31 => null;

		public static TLocalizedText EmployeeTextHireH32 => null;

		public static TLocalizedText EmployeeTextHireH33 => null;

		public static TLocalizedText EmployeeTextHireH34 => null;

		public static TLocalizedText EmployeeTextHireH41 => null;

		public static TLocalizedText EmployeeTextHireH42 => null;

		public static TLocalizedText EmployeeTextHireH43 => null;

		public static TLocalizedText EmployeeTextHireH44 => null;

		public static TLocalizedText EmployeeTextHireH51 => null;

		public static TLocalizedText EmployeeTextHireH52 => null;

		public static TLocalizedText EmployeeTextHireH53 => null;

		public static TLocalizedText EmployeeTextHireH54 => null;

		public static TLocalizedText EmployeeTextHireH61 => null;

		public static TLocalizedText EmployeeTextHireH62 => null;

		public static TLocalizedText EmployeeTextHireH63 => null;

		public static TLocalizedText EmployeeTextHireH64 => null;

		public static TLocalizedText EmployeeTextHireI11 => null;

		public static TLocalizedText EmployeeTextHireI12 => null;

		public static TLocalizedText EmployeeTextHireI13 => null;

		public static TLocalizedText EmployeeTextHireI14 => null;

		public static TLocalizedText EmployeeTextHireI21 => null;

		public static TLocalizedText EmployeeTextHireI22 => null;

		public static TLocalizedText EmployeeTextHireI23 => null;

		public static TLocalizedText EmployeeTextHireI24 => null;

		public static TLocalizedText EmployeeTextHireI31 => null;

		public static TLocalizedText EmployeeTextHireI32 => null;

		public static TLocalizedText EmployeeTextHireI33 => null;

		public static TLocalizedText EmployeeTextHireI34 => null;

		public static TLocalizedText EmployeeTextHireI41 => null;

		public static TLocalizedText EmployeeTextHireI42 => null;

		public static TLocalizedText EmployeeTextHireI43 => null;

		public static TLocalizedText EmployeeTextHireI44 => null;

		public static TLocalizedText EmployeeTextHireI51 => null;

		public static TLocalizedText EmployeeTextHireI52 => null;

		public static TLocalizedText EmployeeTextHireI53 => null;

		public static TLocalizedText EmployeeTextHireI54 => null;

		public static TLocalizedText EmployeeTextHireI61 => null;

		public static TLocalizedText EmployeeTextHireI62 => null;

		public static TLocalizedText EmployeeTextHireI63 => null;

		public static TLocalizedText EmployeeTextHireI64 => null;

		public static TLocalizedText EmployeeTextHireI71 => null;

		public static TLocalizedText EmployeeTextHireI72 => null;

		public static TLocalizedText EmployeeTextHireI73 => null;

		public static TLocalizedText EmployeeTextHireI74 => null;

		public static TLocalizedText EmployeeTextHireJ11 => null;

		public static TLocalizedText EmployeeTextHireJ12 => null;

		public static TLocalizedText EmployeeTextHireJ13 => null;

		public static TLocalizedText EmployeeTextHireJ14 => null;

		public static TLocalizedText EmployeeTextHireJ21 => null;

		public static TLocalizedText EmployeeTextHireJ22 => null;

		public static TLocalizedText EmployeeTextHireJ23 => null;

		public static TLocalizedText EmployeeTextHireJ24 => null;

		public static TLocalizedText EmployeeTextHireJ31 => null;

		public static TLocalizedText EmployeeTextHireJ32 => null;

		public static TLocalizedText EmployeeTextHireJ33 => null;

		public static TLocalizedText EmployeeTextHireJ34 => null;

		public static TLocalizedText EmployeeTextHireJ41 => null;

		public static TLocalizedText EmployeeTextHireJ42 => null;

		public static TLocalizedText EmployeeTextHireJ43 => null;

		public static TLocalizedText EmployeeTextHireJ44 => null;

		public static TLocalizedText EmployeeTextHireJ51 => null;

		public static TLocalizedText EmployeeTextHireJ52 => null;

		public static TLocalizedText EmployeeTextHireJ53 => null;

		public static TLocalizedText EmployeeTextHireJ54 => null;

		public static TLocalizedText EmployeeTextHireJ61 => null;

		public static TLocalizedText EmployeeTextHireJ62 => null;

		public static TLocalizedText EmployeeTextHireJ63 => null;

		public static TLocalizedText EmployeeTextHireJ64 => null;

		public static TLocalizedText EmployeeTextHireK1 => null;

		public static TLocalizedText EmployeeTextHireK2 => null;

		public static TLocalizedText EmployeeTextHireK3 => null;

		public static TLocalizedText EmployeeTextHireK4 => null;

		public static TLocalizedText EmployeeTextHireL1 => null;

		public static TLocalizedText EmployeeTextHireL2 => null;

		public static TLocalizedText EmployeeTextHireL3 => null;

		public static TLocalizedText EmployeeTextHireL4 => null;

		public static TLocalizedText EmployeeTextHireM1 => null;

		public static TLocalizedText EmployeeTextHireM2 => null;

		public static TLocalizedText EmployeeTextHireM3 => null;

		public static TLocalizedText EmployeeTextHireM4 => null;

		public static TLocalizedText EmployeeTextHireN1 => null;

		public static TLocalizedText EmployeeTextHireN2 => null;

		public static TLocalizedText EmployeeTextHireN3 => null;

		public static TLocalizedText EmployeeTextHireN4 => null;

		public static TLocalizedText EmployeeTextInfoPanicA21 => null;

		public static TLocalizedText EmployeeTextInfoPanicA22 => null;

		public static TLocalizedText EmployeeTextInfoPanicA23 => null;

		public static TLocalizedText EmployeeTextInfoPanicA1 => null;

		public static TLocalizedText EmployeeTextInfoPanicA2 => null;

		public static TLocalizedText EmployeeTextInfoPanicA3 => null;

		public static TLocalizedText EmployeeTextInfoPanicB1 => null;

		public static TLocalizedText EmployeeTextInfoPanicB2 => null;

		public static TLocalizedText EmployeeTextInfoPanicB3 => null;

		public static TLocalizedText EmployeeTextInfoPanicC21 => null;

		public static TLocalizedText EmployeeTextInfoPanicC22 => null;

		public static TLocalizedText EmployeeTextInfoPanicC23 => null;

		public static TLocalizedText EmployeeTextInfoPanicC1 => null;

		public static TLocalizedText EmployeeTextInfoPanicC2 => null;

		public static TLocalizedText EmployeeTextInfoPanicC3 => null;

		public static TLocalizedText EmployeeTextInfoPanicD1 => null;

		public static TLocalizedText EmployeeTextInfoPanicD2 => null;

		public static TLocalizedText EmployeeTextInfoPanicD3 => null;

		public static TLocalizedText EmployeeTextInfoPanicE21 => null;

		public static TLocalizedText EmployeeTextInfoPanicE22 => null;

		public static TLocalizedText EmployeeTextInfoPanicE23 => null;

		public static TLocalizedText EmployeeTextInfoPanicE1 => null;

		public static TLocalizedText EmployeeTextInfoPanicE2 => null;

		public static TLocalizedText EmployeeTextInfoPanicE3 => null;

		public static TLocalizedText EmployeeTextInfoPanicF1 => null;

		public static TLocalizedText EmployeeTextInfoPanicF2 => null;

		public static TLocalizedText EmployeeTextInfoPanicF3 => null;

		public static TLocalizedText EmployeeTextInfoPanicG1 => null;

		public static TLocalizedText EmployeeTextInfoPanicG2 => null;

		public static TLocalizedText EmployeeTextInfoPanicG3 => null;

		public static TLocalizedText EmployeeTextInfoPanicH1 => null;

		public static TLocalizedText EmployeeTextInfoPanicH2 => null;

		public static TLocalizedText EmployeeTextInfoPanicH3 => null;

		public static TLocalizedText EmployeeTextInfoPanicI1 => null;

		public static TLocalizedText EmployeeTextInfoPanicI2 => null;

		public static TLocalizedText EmployeeTextInfoPanicI3 => null;

		public static TLocalizedText EmployeeTextInfoPanicJ1 => null;

		public static TLocalizedText EmployeeTextInfoPanicJ2 => null;

		public static TLocalizedText EmployeeTextInfoPanicJ3 => null;

		public static TLocalizedText EmployeeTextInfoPanicK1 => null;

		public static TLocalizedText EmployeeTextInfoPanicK2 => null;

		public static TLocalizedText EmployeeTextInfoPanicK3 => null;

		public static TLocalizedText EmployeeTextInfoPanicL1 => null;

		public static TLocalizedText EmployeeTextInfoPanicL2 => null;

		public static TLocalizedText EmployeeTextInfoPanicL3 => null;

		public static TLocalizedText EmployeeTextInfoPanicM1 => null;

		public static TLocalizedText EmployeeTextInfoPanicM2 => null;

		public static TLocalizedText EmployeeTextInfoPanicM3 => null;

		public static TLocalizedText EmployeeTextInfoPanicN1 => null;

		public static TLocalizedText EmployeeTextInfoPanicN2 => null;

		public static TLocalizedText EmployeeTextInfoPanicN3 => null;

		public static TLocalizedText EmployeeTextInfoA21 => null;

		public static TLocalizedText EmployeeTextInfoA22 => null;

		public static TLocalizedText EmployeeTextInfoA23 => null;

		public static TLocalizedText EmployeeTextInfoA24 => null;

		public static TLocalizedText EmployeeTextInfoA1 => null;

		public static TLocalizedText EmployeeTextInfoA2 => null;

		public static TLocalizedText EmployeeTextInfoA3 => null;

		public static TLocalizedText EmployeeTextInfoA4 => null;

		public static TLocalizedText EmployeeTextInfoB1 => null;

		public static TLocalizedText EmployeeTextInfoB2 => null;

		public static TLocalizedText EmployeeTextInfoB3 => null;

		public static TLocalizedText EmployeeTextInfoB4 => null;

		public static TLocalizedText EmployeeTextInfoC21 => null;

		public static TLocalizedText EmployeeTextInfoC22 => null;

		public static TLocalizedText EmployeeTextInfoC23 => null;

		public static TLocalizedText EmployeeTextInfoC24 => null;

		public static TLocalizedText EmployeeTextInfoC1 => null;

		public static TLocalizedText EmployeeTextInfoC2 => null;

		public static TLocalizedText EmployeeTextInfoC3 => null;

		public static TLocalizedText EmployeeTextInfoC4 => null;

		public static TLocalizedText EmployeeTextInfoD1 => null;

		public static TLocalizedText EmployeeTextInfoD2 => null;

		public static TLocalizedText EmployeeTextInfoD3 => null;

		public static TLocalizedText EmployeeTextInfoD4 => null;

		public static TLocalizedText EmployeeTextInfoE21 => null;

		public static TLocalizedText EmployeeTextInfoE22 => null;

		public static TLocalizedText EmployeeTextInfoE23 => null;

		public static TLocalizedText EmployeeTextInfoE24 => null;

		public static TLocalizedText EmployeeTextInfoE1 => null;

		public static TLocalizedText EmployeeTextInfoE2 => null;

		public static TLocalizedText EmployeeTextInfoE3 => null;

		public static TLocalizedText EmployeeTextInfoE4 => null;

		public static TLocalizedText EmployeeTextInfoF1 => null;

		public static TLocalizedText EmployeeTextInfoF2 => null;

		public static TLocalizedText EmployeeTextInfoF3 => null;

		public static TLocalizedText EmployeeTextInfoF4 => null;

		public static TLocalizedText EmployeeTextInfoG1 => null;

		public static TLocalizedText EmployeeTextInfoG2 => null;

		public static TLocalizedText EmployeeTextInfoG3 => null;

		public static TLocalizedText EmployeeTextInfoG4 => null;

		public static TLocalizedText EmployeeTextInfoH1 => null;

		public static TLocalizedText EmployeeTextInfoH2 => null;

		public static TLocalizedText EmployeeTextInfoH3 => null;

		public static TLocalizedText EmployeeTextInfoH4 => null;

		public static TLocalizedText EmployeeTextInfoI1 => null;

		public static TLocalizedText EmployeeTextInfoI2 => null;

		public static TLocalizedText EmployeeTextInfoI3 => null;

		public static TLocalizedText EmployeeTextInfoI4 => null;

		public static TLocalizedText EmployeeTextInfoJ1 => null;

		public static TLocalizedText EmployeeTextInfoJ2 => null;

		public static TLocalizedText EmployeeTextInfoJ3 => null;

		public static TLocalizedText EmployeeTextInfoJ4 => null;

		public static TLocalizedText EmployeeTextInfoK1 => null;

		public static TLocalizedText EmployeeTextInfoK2 => null;

		public static TLocalizedText EmployeeTextInfoK3 => null;

		public static TLocalizedText EmployeeTextInfoK4 => null;

		public static TLocalizedText EmployeeTextInfoL1 => null;

		public static TLocalizedText EmployeeTextInfoL2 => null;

		public static TLocalizedText EmployeeTextInfoL3 => null;

		public static TLocalizedText EmployeeTextInfoL4 => null;

		public static TLocalizedText EmployeeTextInfoM1 => null;

		public static TLocalizedText EmployeeTextInfoM2 => null;

		public static TLocalizedText EmployeeTextInfoM3 => null;

		public static TLocalizedText EmployeeTextInfoM4 => null;

		public static TLocalizedText EmployeeTextInfoN1 => null;

		public static TLocalizedText EmployeeTextInfoN2 => null;

		public static TLocalizedText EmployeeTextInfoN3 => null;

		public static TLocalizedText EmployeeTextInfoN4 => null;

		public static TLocalizedText EmployeeTextLevelUpA21 => null;

		public static TLocalizedText EmployeeTextLevelUpA22 => null;

		public static TLocalizedText EmployeeTextLevelUpA23 => null;

		public static TLocalizedText EmployeeTextLevelUpA24 => null;

		public static TLocalizedText EmployeeTextLevelUpA1 => null;

		public static TLocalizedText EmployeeTextLevelUpA2 => null;

		public static TLocalizedText EmployeeTextLevelUpA3 => null;

		public static TLocalizedText EmployeeTextLevelUpA4 => null;

		public static TLocalizedText EmployeeTextLevelUpB1 => null;

		public static TLocalizedText EmployeeTextLevelUpB2 => null;

		public static TLocalizedText EmployeeTextLevelUpB3 => null;

		public static TLocalizedText EmployeeTextLevelUpB4 => null;

		public static TLocalizedText EmployeeTextLevelUpC21 => null;

		public static TLocalizedText EmployeeTextLevelUpC22 => null;

		public static TLocalizedText EmployeeTextLevelUpC23 => null;

		public static TLocalizedText EmployeeTextLevelUpC24 => null;

		public static TLocalizedText EmployeeTextLevelUpC1 => null;

		public static TLocalizedText EmployeeTextLevelUpC2 => null;

		public static TLocalizedText EmployeeTextLevelUpC3 => null;

		public static TLocalizedText EmployeeTextLevelUpC4 => null;

		public static TLocalizedText EmployeeTextLevelUpD1 => null;

		public static TLocalizedText EmployeeTextLevelUpD2 => null;

		public static TLocalizedText EmployeeTextLevelUpD3 => null;

		public static TLocalizedText EmployeeTextLevelUpD4 => null;

		public static TLocalizedText EmployeeTextLevelUpE21 => null;

		public static TLocalizedText EmployeeTextLevelUpE22 => null;

		public static TLocalizedText EmployeeTextLevelUpE23 => null;

		public static TLocalizedText EmployeeTextLevelUpE24 => null;

		public static TLocalizedText EmployeeTextLevelUpE1 => null;

		public static TLocalizedText EmployeeTextLevelUpE2 => null;

		public static TLocalizedText EmployeeTextLevelUpE3 => null;

		public static TLocalizedText EmployeeTextLevelUpE4 => null;

		public static TLocalizedText EmployeeTextLevelUpF1 => null;

		public static TLocalizedText EmployeeTextLevelUpF2 => null;

		public static TLocalizedText EmployeeTextLevelUpF3 => null;

		public static TLocalizedText EmployeeTextLevelUpF4 => null;

		public static TLocalizedText EmployeeTextLevelUpG1 => null;

		public static TLocalizedText EmployeeTextLevelUpG2 => null;

		public static TLocalizedText EmployeeTextLevelUpG3 => null;

		public static TLocalizedText EmployeeTextLevelUpG4 => null;

		public static TLocalizedText EmployeeTextLevelUpH1 => null;

		public static TLocalizedText EmployeeTextLevelUpH2 => null;

		public static TLocalizedText EmployeeTextLevelUpH3 => null;

		public static TLocalizedText EmployeeTextLevelUpH4 => null;

		public static TLocalizedText EmployeeTextLevelUpI1 => null;

		public static TLocalizedText EmployeeTextLevelUpI2 => null;

		public static TLocalizedText EmployeeTextLevelUpI3 => null;

		public static TLocalizedText EmployeeTextLevelUpI4 => null;

		public static TLocalizedText EmployeeTextLevelUpJ1 => null;

		public static TLocalizedText EmployeeTextLevelUpJ2 => null;

		public static TLocalizedText EmployeeTextLevelUpJ3 => null;

		public static TLocalizedText EmployeeTextLevelUpJ4 => null;

		public static TLocalizedText EmployeeTextLevelUpK1 => null;

		public static TLocalizedText EmployeeTextLevelUpK2 => null;

		public static TLocalizedText EmployeeTextLevelUpK3 => null;

		public static TLocalizedText EmployeeTextLevelUpK4 => null;

		public static TLocalizedText EmployeeTextLevelUpL1 => null;

		public static TLocalizedText EmployeeTextLevelUpL2 => null;

		public static TLocalizedText EmployeeTextLevelUpL3 => null;

		public static TLocalizedText EmployeeTextLevelUpL4 => null;

		public static TLocalizedText EmployeeTextLevelUpM1 => null;

		public static TLocalizedText EmployeeTextLevelUpM2 => null;

		public static TLocalizedText EmployeeTextLevelUpM3 => null;

		public static TLocalizedText EmployeeTextLevelUpM4 => null;

		public static TLocalizedText EmployeeTextLevelUpN1 => null;

		public static TLocalizedText EmployeeTextLevelUpN2 => null;

		public static TLocalizedText EmployeeTextLevelUpN3 => null;

		public static TLocalizedText EmployeeTextLevelUpN4 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressA21 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressA22 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressA1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressA2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressB1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressB2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressC21 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressC22 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressC1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressC2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressD1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressD2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressE21 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressE22 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressE1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressE2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressF1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressF2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressG1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressG2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressH1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressH2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressI1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressI2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressJ1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressJ2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressK1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressK2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressL1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressL2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressM1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressM2 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressN1 => null;

		public static TLocalizedText EmployeeTextReadyToSellHighStressN2 => null;

		public static TLocalizedText EmployeeTextReadyToSellA21 => null;

		public static TLocalizedText EmployeeTextReadyToSellA22 => null;

		public static TLocalizedText EmployeeTextReadyToSellA23 => null;

		public static TLocalizedText EmployeeTextReadyToSellA24 => null;

		public static TLocalizedText EmployeeTextReadyToSellA1 => null;

		public static TLocalizedText EmployeeTextReadyToSellA2 => null;

		public static TLocalizedText EmployeeTextReadyToSellA3 => null;

		public static TLocalizedText EmployeeTextReadyToSellA4 => null;

		public static TLocalizedText EmployeeTextReadyToSellB1 => null;

		public static TLocalizedText EmployeeTextReadyToSellB2 => null;

		public static TLocalizedText EmployeeTextReadyToSellB3 => null;

		public static TLocalizedText EmployeeTextReadyToSellB4 => null;

		public static TLocalizedText EmployeeTextReadyToSellC21 => null;

		public static TLocalizedText EmployeeTextReadyToSellC22 => null;

		public static TLocalizedText EmployeeTextReadyToSellC23 => null;

		public static TLocalizedText EmployeeTextReadyToSellC24 => null;

		public static TLocalizedText EmployeeTextReadyToSellC1 => null;

		public static TLocalizedText EmployeeTextReadyToSellC2 => null;

		public static TLocalizedText EmployeeTextReadyToSellC3 => null;

		public static TLocalizedText EmployeeTextReadyToSellC4 => null;

		public static TLocalizedText EmployeeTextReadyToSellD1 => null;

		public static TLocalizedText EmployeeTextReadyToSellD2 => null;

		public static TLocalizedText EmployeeTextReadyToSellD3 => null;

		public static TLocalizedText EmployeeTextReadyToSellD4 => null;

		public static TLocalizedText EmployeeTextReadyToSellE21 => null;

		public static TLocalizedText EmployeeTextReadyToSellE22 => null;

		public static TLocalizedText EmployeeTextReadyToSellE23 => null;

		public static TLocalizedText EmployeeTextReadyToSellE24 => null;

		public static TLocalizedText EmployeeTextReadyToSellE1 => null;

		public static TLocalizedText EmployeeTextReadyToSellE2 => null;

		public static TLocalizedText EmployeeTextReadyToSellE3 => null;

		public static TLocalizedText EmployeeTextReadyToSellE4 => null;

		public static TLocalizedText EmployeeTextReadyToSellF1 => null;

		public static TLocalizedText EmployeeTextReadyToSellF2 => null;

		public static TLocalizedText EmployeeTextReadyToSellF3 => null;

		public static TLocalizedText EmployeeTextReadyToSellF4 => null;

		public static TLocalizedText EmployeeTextReadyToSellG1 => null;

		public static TLocalizedText EmployeeTextReadyToSellG2 => null;

		public static TLocalizedText EmployeeTextReadyToSellG3 => null;

		public static TLocalizedText EmployeeTextReadyToSellG4 => null;

		public static TLocalizedText EmployeeTextReadyToSellH1 => null;

		public static TLocalizedText EmployeeTextReadyToSellH2 => null;

		public static TLocalizedText EmployeeTextReadyToSellH3 => null;

		public static TLocalizedText EmployeeTextReadyToSellH4 => null;

		public static TLocalizedText EmployeeTextReadyToSellI1 => null;

		public static TLocalizedText EmployeeTextReadyToSellI2 => null;

		public static TLocalizedText EmployeeTextReadyToSellI3 => null;

		public static TLocalizedText EmployeeTextReadyToSellI4 => null;

		public static TLocalizedText EmployeeTextReadyToSellJ1 => null;

		public static TLocalizedText EmployeeTextReadyToSellJ2 => null;

		public static TLocalizedText EmployeeTextReadyToSellJ3 => null;

		public static TLocalizedText EmployeeTextReadyToSellJ4 => null;

		public static TLocalizedText EmployeeTextReadyToSellK1 => null;

		public static TLocalizedText EmployeeTextReadyToSellK2 => null;

		public static TLocalizedText EmployeeTextReadyToSellK3 => null;

		public static TLocalizedText EmployeeTextReadyToSellK4 => null;

		public static TLocalizedText EmployeeTextReadyToSellL1 => null;

		public static TLocalizedText EmployeeTextReadyToSellL2 => null;

		public static TLocalizedText EmployeeTextReadyToSellL3 => null;

		public static TLocalizedText EmployeeTextReadyToSellL4 => null;

		public static TLocalizedText EmployeeTextReadyToSellM1 => null;

		public static TLocalizedText EmployeeTextReadyToSellM2 => null;

		public static TLocalizedText EmployeeTextReadyToSellM3 => null;

		public static TLocalizedText EmployeeTextReadyToSellM4 => null;

		public static TLocalizedText EmployeeTextReadyToSellN1 => null;

		public static TLocalizedText EmployeeTextReadyToSellN2 => null;

		public static TLocalizedText EmployeeTextReadyToSellN3 => null;

		public static TLocalizedText EmployeeTextReadyToSellN4 => null;

		public static TLocalizedText EmployeeTextSellingPanicA21 => null;

		public static TLocalizedText EmployeeTextSellingPanicA22 => null;

		public static TLocalizedText EmployeeTextSellingPanicA1 => null;

		public static TLocalizedText EmployeeTextSellingPanicA2 => null;

		public static TLocalizedText EmployeeTextSellingPanicB1 => null;

		public static TLocalizedText EmployeeTextSellingPanicB2 => null;

		public static TLocalizedText EmployeeTextSellingPanicC21 => null;

		public static TLocalizedText EmployeeTextSellingPanicC22 => null;

		public static TLocalizedText EmployeeTextSellingPanicC1 => null;

		public static TLocalizedText EmployeeTextSellingPanicC2 => null;

		public static TLocalizedText EmployeeTextSellingPanicD1 => null;

		public static TLocalizedText EmployeeTextSellingPanicD2 => null;

		public static TLocalizedText EmployeeTextSellingPanicE21 => null;

		public static TLocalizedText EmployeeTextSellingPanicE22 => null;

		public static TLocalizedText EmployeeTextSellingPanicE1 => null;

		public static TLocalizedText EmployeeTextSellingPanicE2 => null;

		public static TLocalizedText EmployeeTextSellingPanicF1 => null;

		public static TLocalizedText EmployeeTextSellingPanicF2 => null;

		public static TLocalizedText EmployeeTextSellingPanicG1 => null;

		public static TLocalizedText EmployeeTextSellingPanicG2 => null;

		public static TLocalizedText EmployeeTextSellingPanicH1 => null;

		public static TLocalizedText EmployeeTextSellingPanicH2 => null;

		public static TLocalizedText EmployeeTextSellingPanicI1 => null;

		public static TLocalizedText EmployeeTextSellingPanicI2 => null;

		public static TLocalizedText EmployeeTextSellingPanicJ1 => null;

		public static TLocalizedText EmployeeTextSellingPanicJ2 => null;

		public static TLocalizedText EmployeeTextSellingPanicK1 => null;

		public static TLocalizedText EmployeeTextSellingPanicK2 => null;

		public static TLocalizedText EmployeeTextSellingPanicL1 => null;

		public static TLocalizedText EmployeeTextSellingPanicL2 => null;

		public static TLocalizedText EmployeeTextSellingPanicM1 => null;

		public static TLocalizedText EmployeeTextSellingPanicM2 => null;

		public static TLocalizedText EmployeeTextSellingPanicN1 => null;

		public static TLocalizedText EmployeeTextSellingPanicN2 => null;

		public static TLocalizedText EmployeeTextSellingA21 => null;

		public static TLocalizedText EmployeeTextSellingA22 => null;

		public static TLocalizedText EmployeeTextSellingA23 => null;

		public static TLocalizedText EmployeeTextSellingA24 => null;

		public static TLocalizedText EmployeeTextSellingA1 => null;

		public static TLocalizedText EmployeeTextSellingA2 => null;

		public static TLocalizedText EmployeeTextSellingA3 => null;

		public static TLocalizedText EmployeeTextSellingA4 => null;

		public static TLocalizedText EmployeeTextSellingB1 => null;

		public static TLocalizedText EmployeeTextSellingB2 => null;

		public static TLocalizedText EmployeeTextSellingB3 => null;

		public static TLocalizedText EmployeeTextSellingB4 => null;

		public static TLocalizedText EmployeeTextSellingC21 => null;

		public static TLocalizedText EmployeeTextSellingC22 => null;

		public static TLocalizedText EmployeeTextSellingC23 => null;

		public static TLocalizedText EmployeeTextSellingC24 => null;

		public static TLocalizedText EmployeeTextSellingC1 => null;

		public static TLocalizedText EmployeeTextSellingC2 => null;

		public static TLocalizedText EmployeeTextSellingC3 => null;

		public static TLocalizedText EmployeeTextSellingC4 => null;

		public static TLocalizedText EmployeeTextSellingD1 => null;

		public static TLocalizedText EmployeeTextSellingD2 => null;

		public static TLocalizedText EmployeeTextSellingD3 => null;

		public static TLocalizedText EmployeeTextSellingD4 => null;

		public static TLocalizedText EmployeeTextSellingE21 => null;

		public static TLocalizedText EmployeeTextSellingE22 => null;

		public static TLocalizedText EmployeeTextSellingE23 => null;

		public static TLocalizedText EmployeeTextSellingE24 => null;

		public static TLocalizedText EmployeeTextSellingE1 => null;

		public static TLocalizedText EmployeeTextSellingE2 => null;

		public static TLocalizedText EmployeeTextSellingE3 => null;

		public static TLocalizedText EmployeeTextSellingE4 => null;

		public static TLocalizedText EmployeeTextSellingF1 => null;

		public static TLocalizedText EmployeeTextSellingF2 => null;

		public static TLocalizedText EmployeeTextSellingF3 => null;

		public static TLocalizedText EmployeeTextSellingF4 => null;

		public static TLocalizedText EmployeeTextSellingG1 => null;

		public static TLocalizedText EmployeeTextSellingG2 => null;

		public static TLocalizedText EmployeeTextSellingG3 => null;

		public static TLocalizedText EmployeeTextSellingG4 => null;

		public static TLocalizedText EmployeeTextSellingH1 => null;

		public static TLocalizedText EmployeeTextSellingH2 => null;

		public static TLocalizedText EmployeeTextSellingH3 => null;

		public static TLocalizedText EmployeeTextSellingH4 => null;

		public static TLocalizedText EmployeeTextSellingI1 => null;

		public static TLocalizedText EmployeeTextSellingI2 => null;

		public static TLocalizedText EmployeeTextSellingI3 => null;

		public static TLocalizedText EmployeeTextSellingI4 => null;

		public static TLocalizedText EmployeeTextSellingJ1 => null;

		public static TLocalizedText EmployeeTextSellingJ2 => null;

		public static TLocalizedText EmployeeTextSellingJ3 => null;

		public static TLocalizedText EmployeeTextSellingJ4 => null;

		public static TLocalizedText EmployeeTextSellingK1 => null;

		public static TLocalizedText EmployeeTextSellingK2 => null;

		public static TLocalizedText EmployeeTextSellingK3 => null;

		public static TLocalizedText EmployeeTextSellingK4 => null;

		public static TLocalizedText EmployeeTextSellingL1 => null;

		public static TLocalizedText EmployeeTextSellingL2 => null;

		public static TLocalizedText EmployeeTextSellingL3 => null;

		public static TLocalizedText EmployeeTextSellingL4 => null;

		public static TLocalizedText EmployeeTextSellingM1 => null;

		public static TLocalizedText EmployeeTextSellingM2 => null;

		public static TLocalizedText EmployeeTextSellingM3 => null;

		public static TLocalizedText EmployeeTextSellingM4 => null;

		public static TLocalizedText EmployeeTextSellingN1 => null;

		public static TLocalizedText EmployeeTextSellingN2 => null;

		public static TLocalizedText EmployeeTextSellingN3 => null;

		public static TLocalizedText EmployeeTextSellingN4 => null;

		public static TLocalizedText TAchievementName1001 => null;

		public static TLocalizedText TAchievementDesc1001 => null;

		public static TLocalizedText TAchievementName1002 => null;

		public static TLocalizedText TAchievementDesc1002 => null;

		public static TLocalizedText TAreaName1 => null;

		public static TLocalizedText TAreaName2 => null;

		public static TLocalizedText TAreaName3 => null;

		public static TLocalizedText TAreaName4 => null;

		public static TLocalizedText TAreaName5 => null;

		public static TLocalizedText TAreaDecorationName101 => null;

		public static TLocalizedText TAreaDecorationName102 => null;

		public static TLocalizedText TAreaDecorationName103 => null;

		public static TLocalizedText TAreaDecorationName111 => null;

		public static TLocalizedText TAreaDecorationName112 => null;

		public static TLocalizedText TAreaDecorationName113 => null;

		public static TLocalizedText TAreaDecorationName201 => null;

		public static TLocalizedText TAreaDecorationName202 => null;

		public static TLocalizedText TAreaDecorationName203 => null;

		public static TLocalizedText TAreaDecorationName211 => null;

		public static TLocalizedText TAreaDecorationName222 => null;

		public static TLocalizedText TAreaDecorationName223 => null;

		public static TLocalizedText TAreaDecorationName311 => null;

		public static TLocalizedText TAreaDecorationName312 => null;

		public static TLocalizedText TAreaDecorationName313 => null;

		public static TLocalizedText TAreaDecorationName321 => null;

		public static TLocalizedText TAreaDecorationName322 => null;

		public static TLocalizedText TAreaDecorationName323 => null;

		public static TLocalizedText TAreaDecorationName401 => null;

		public static TLocalizedText TAreaDecorationName402 => null;

		public static TLocalizedText TAreaDecorationName403 => null;

		public static TLocalizedText TAreaDecorationName411 => null;

		public static TLocalizedText TAreaDecorationName412 => null;

		public static TLocalizedText TAreaDecorationName413 => null;

		public static TLocalizedText TAreaDecorationName501 => null;

		public static TLocalizedText TAreaDecorationName502 => null;

		public static TLocalizedText TAreaDecorationName503 => null;

		public static TLocalizedText TAreaDecorationName511 => null;

		public static TLocalizedText TAreaDecorationName512 => null;

		public static TLocalizedText TAreaDecorationName513 => null;

		public static TLocalizedText TAreaDecorationName1001 => null;

		public static TLocalizedText TAreaDecorationName1002 => null;

		public static TLocalizedText TAreaDecorationName1003 => null;

		public static TLocalizedText TAreaDecorationName1004 => null;

		public static TLocalizedText TAreaDecorationName1005 => null;

		public static TLocalizedText TAreaDecorationName1101 => null;

		public static TLocalizedText TAreaDecorationName1102 => null;

		public static TLocalizedText TAreaDecorationName1103 => null;

		public static TLocalizedText TAreaDecorationName1104 => null;

		public static TLocalizedText TAreaDecorationName1105 => null;

		public static TLocalizedText TAreaDecorationName1201 => null;

		public static TLocalizedText TAreaDecorationName1202 => null;

		public static TLocalizedText TAreaDecorationName1203 => null;

		public static TLocalizedText TAreaDecorationName1204 => null;

		public static TLocalizedText TAreaDecorationName1205 => null;

		public static TLocalizedText TAreaDecorationName1301 => null;

		public static TLocalizedText TAreaDecorationName1302 => null;

		public static TLocalizedText TAreaDecorationName1303 => null;

		public static TLocalizedText TAreaDecorationName1304 => null;

		public static TLocalizedText TAreaDecorationName1305 => null;

		public static TLocalizedText TAreaDecorationName2001 => null;

		public static TLocalizedText TAreaDecorationName2002 => null;

		public static TLocalizedText TAreaDecorationName2003 => null;

		public static TLocalizedText TAreaDecorationName2004 => null;

		public static TLocalizedText TAreaDecorationName2005 => null;

		public static TLocalizedText TAreaDecorationName2101 => null;

		public static TLocalizedText TAreaDecorationName2102 => null;

		public static TLocalizedText TAreaDecorationName2103 => null;

		public static TLocalizedText TAreaDecorationName2104 => null;

		public static TLocalizedText TAreaDecorationName2105 => null;

		public static TLocalizedText TAreaDecorationName3000 => null;

		public static TLocalizedText TAreaDecorationName3001 => null;

		public static TLocalizedText TAreaDecorationName3002 => null;

		public static TLocalizedText TAreaDecorationName3003 => null;

		public static TLocalizedText TAreaDecorationName3004 => null;

		public static TLocalizedText TAreaDecorationName3005 => null;

		public static TLocalizedText TAreaDecorationName3101 => null;

		public static TLocalizedText TAreaDecorationName3102 => null;

		public static TLocalizedText TAreaDecorationName3103 => null;

		public static TLocalizedText TAreaDecorationName3104 => null;

		public static TLocalizedText TAreaDecorationName3105 => null;

		public static TLocalizedText TAreaDecorationName4000 => null;

		public static TLocalizedText TAreaDecorationName4001 => null;

		public static TLocalizedText TAreaDecorationName4002 => null;

		public static TLocalizedText TAreaDecorationName4003 => null;

		public static TLocalizedText TAreaDecorationName4004 => null;

		public static TLocalizedText TAreaDecorationName4005 => null;

		public static TLocalizedText TAreaDecorationName4101 => null;

		public static TLocalizedText TAreaDecorationName4102 => null;

		public static TLocalizedText TAreaDecorationName4103 => null;

		public static TLocalizedText TAreaDecorationName4104 => null;

		public static TLocalizedText TAreaDecorationName4105 => null;

		public static TLocalizedText TAreaDecorationName5000 => null;

		public static TLocalizedText TAreaDecorationName5001 => null;

		public static TLocalizedText TAreaDecorationName5002 => null;

		public static TLocalizedText TAreaDecorationName5003 => null;

		public static TLocalizedText TAreaDecorationName5004 => null;

		public static TLocalizedText TAreaDecorationName5005 => null;

		public static TLocalizedText TAreaDecorationName5101 => null;

		public static TLocalizedText TAreaDecorationName5102 => null;

		public static TLocalizedText TAreaDecorationName5103 => null;

		public static TLocalizedText TAreaDecorationName5104 => null;

		public static TLocalizedText TAreaDecorationName5105 => null;

		public static TLocalizedText TAreaDecorationName5201 => null;

		public static TLocalizedText TAreaDecorationName5202 => null;

		public static TLocalizedText TAreaDecorationName5203 => null;

		public static TLocalizedText TAreaDecorationName5204 => null;

		public static TLocalizedText TAreaDecorationName5205 => null;

		public static TLocalizedText TAreaDecorationName10011 => null;

		public static TLocalizedText TAreaDecorationName10012 => null;

		public static TLocalizedText TAreaDecorationName10013 => null;

		public static TLocalizedText TAreaDecorationName10014 => null;

		public static TLocalizedText TAreaDecorationName10021 => null;

		public static TLocalizedText TAreaDecorationName10022 => null;

		public static TLocalizedText TAreaDecorationName10023 => null;

		public static TLocalizedText TAreaDecorationName10024 => null;

		public static TLocalizedText TAreaDecorationName10031 => null;

		public static TLocalizedText TAreaDecorationName10032 => null;

		public static TLocalizedText TAreaDecorationName10033 => null;

		public static TLocalizedText TAreaDecorationName10034 => null;

		public static TLocalizedText TAreaDecorationName10041 => null;

		public static TLocalizedText TAreaDecorationName10042 => null;

		public static TLocalizedText TAreaDecorationName10043 => null;

		public static TLocalizedText TAreaDecorationName10044 => null;

		public static TLocalizedText TAreaDecorationName10051 => null;

		public static TLocalizedText TAreaDecorationName10052 => null;

		public static TLocalizedText TAreaDecorationName10053 => null;

		public static TLocalizedText TAreaDecorationName10054 => null;

		public static TLocalizedText TAreaDecorationName10061 => null;

		public static TLocalizedText TAreaDecorationName10062 => null;

		public static TLocalizedText TAreaDecorationName10063 => null;

		public static TLocalizedText TAreaDecorationName10064 => null;

		public static TLocalizedText TAreaDecorationName10071 => null;

		public static TLocalizedText TAreaDecorationName10072 => null;

		public static TLocalizedText TAreaDecorationName10073 => null;

		public static TLocalizedText TAreaDecorationName10074 => null;

		public static TLocalizedText TAreaDecorationName10081 => null;

		public static TLocalizedText TAreaDecorationName10082 => null;

		public static TLocalizedText TAreaDecorationName10083 => null;

		public static TLocalizedText TAreaDecorationName10084 => null;

		public static TLocalizedText TAreaDecorationName10091 => null;

		public static TLocalizedText TAreaDecorationName10092 => null;

		public static TLocalizedText TAreaDecorationName10093 => null;

		public static TLocalizedText TAreaDecorationName10094 => null;

		public static TLocalizedText TAreaDecorationName20011 => null;

		public static TLocalizedText TAreaDecorationName20012 => null;

		public static TLocalizedText TAreaDecorationName20013 => null;

		public static TLocalizedText TAreaDecorationName20014 => null;

		public static TLocalizedText TAreaDecorationName30011 => null;

		public static TLocalizedText TAreaDecorationName30012 => null;

		public static TLocalizedText TAreaDecorationName30013 => null;

		public static TLocalizedText TAreaDecorationName30014 => null;

		public static TLocalizedText TAreaDecorationName30021 => null;

		public static TLocalizedText TAreaDecorationName30022 => null;

		public static TLocalizedText TAreaDecorationName30023 => null;

		public static TLocalizedText TAreaDecorationName30024 => null;

		public static TLocalizedText TAreaDecorationName30031 => null;

		public static TLocalizedText TAreaDecorationName30032 => null;

		public static TLocalizedText TAreaDecorationName30033 => null;

		public static TLocalizedText TAreaDecorationName30034 => null;

		public static TLocalizedText TAreaDecorationName40011 => null;

		public static TLocalizedText TAreaDecorationName40012 => null;

		public static TLocalizedText TAreaDecorationName40013 => null;

		public static TLocalizedText TAreaDecorationName40014 => null;

		public static TLocalizedText TArtifactDealCardName1 => null;

		public static TLocalizedText TArtifactDealCardName2 => null;

		public static TLocalizedText TArtifactDealCardName3 => null;

		public static TLocalizedText TArtifactDealCardName4 => null;

		public static TLocalizedText TArtifactDealCardName5 => null;

		public static TLocalizedText TArtifactDealCardName6 => null;

		public static TLocalizedText TArtifactDealCardName7 => null;

		public static TLocalizedText TArtifactDealCardName8 => null;

		public static TLocalizedText TArtifactDealCardName9 => null;

		public static TLocalizedText TArtifactDealCardName10 => null;

		public static TLocalizedText TArtifactDealCardName11 => null;

		public static TLocalizedText TArtifactDealCardName12 => null;

		public static TLocalizedText TArtifactDealCardName13 => null;

		public static TLocalizedText TArtifactDealCardName14 => null;

		public static TLocalizedText TArtifactDealCardName15 => null;

		public static TLocalizedText TArtifactDealCardName16 => null;

		public static TLocalizedText TArtifactDealCardName17 => null;

		public static TLocalizedText TArtifactDealCardName18 => null;

		public static TLocalizedText TArtifactDealCardName19 => null;

		public static TLocalizedText TArtifactDealCardName20 => null;

		public static TLocalizedText TArtifactDealCardName21 => null;

		public static TLocalizedText TArtifactDealCardName22 => null;

		public static TLocalizedText TArtifactDealCardName23 => null;

		public static TLocalizedText TArtifactDealCardName24 => null;

		public static TLocalizedText TArtifactDealCardName25 => null;

		public static TLocalizedText TArtifactDealCardName26 => null;

		public static TLocalizedText TArtifactDealCardName27 => null;

		public static TLocalizedText TArtifactDealCardName28 => null;

		public static TLocalizedText TArtifactDealCardName29 => null;

		public static TLocalizedText TArtifactDealCardName30 => null;

		public static TLocalizedText TArtifactDealCardName31 => null;

		public static TLocalizedText TArtifactDealCardName32 => null;

		public static TLocalizedText TArtifactDealCardName33 => null;

		public static TLocalizedText TArtifactDealCardName34 => null;

		public static TLocalizedText TArtifactDealCardName35 => null;

		public static TLocalizedText TArtifactDealCardName36 => null;

		public static TLocalizedText TArtifactDealCardName37 => null;

		public static TLocalizedText TArtifactDealCardName38 => null;

		public static TLocalizedText TArtifactDealCardName39 => null;

		public static TLocalizedText TArtifactDealCardName40 => null;

		public static TLocalizedText TArtifactDealCardName41 => null;

		public static TLocalizedText TArtifactDealCardName42 => null;

		public static TLocalizedText TArtifactDealCardName43 => null;

		public static TLocalizedText TArtifactDealCardName44 => null;

		public static TLocalizedText TArtifactDealCardName45 => null;

		public static TLocalizedText TArtifactDealCardName46 => null;

		public static TLocalizedText TArtifactDealCardName47 => null;

		public static TLocalizedText TArtifactDealCardName48 => null;

		public static TLocalizedText TArtifactDealCardName49 => null;

		public static TLocalizedText TArtifactDealCardName50 => null;

		public static TLocalizedText TArtifactDealCardName51 => null;

		public static TLocalizedText TArtifactDealCardName52 => null;

		public static TLocalizedText TArtifactDealCardName53 => null;

		public static TLocalizedText TArtifactDealCardName54 => null;

		public static TLocalizedText TArtifactDealCardName55 => null;

		public static TLocalizedText TArtifactDealCardName56 => null;

		public static TLocalizedText TArtifactDealCardName57 => null;

		public static TLocalizedText TArtifactDealCardName58 => null;

		public static TLocalizedText TArtifactDealCardName59 => null;

		public static TLocalizedText TArtifactDealCardName60 => null;

		public static TLocalizedText TArtifactDealCardName61 => null;

		public static TLocalizedText TArtifactDealCardName62 => null;

		public static TLocalizedText TArtifactDealCardName63 => null;

		public static TLocalizedText TArtifactDealCardName64 => null;

		public static TLocalizedText TArtifactDealCardName65 => null;

		public static TLocalizedText TArtifactDealCardName66 => null;

		public static TLocalizedText TArtifactDealCardName67 => null;

		public static TLocalizedText TArtifactDealCardName68 => null;

		public static TLocalizedText TArtifactDealCardName69 => null;

		public static TLocalizedText TArtifactDealCardName70 => null;

		public static TLocalizedText TArtifactDealCardName71 => null;

		public static TLocalizedText TArtifactDealCardName72 => null;

		public static TLocalizedText TArtifactDealCardName73 => null;

		public static TLocalizedText TArtifactPrefixName10001 => null;

		public static TLocalizedText TArtifactPrefixName10002 => null;

		public static TLocalizedText TArtifactPrefixName10003 => null;

		public static TLocalizedText TArtifactPrefixName10004 => null;

		public static TLocalizedText TArtifactPrefixName10005 => null;

		public static TLocalizedText TArtifactPrefixName10006 => null;

		public static TLocalizedText TArtifactPrefixName10007 => null;

		public static TLocalizedText TArtifactPrefixName10008 => null;

		public static TLocalizedText TArtifactPrefixName10009 => null;

		public static TLocalizedText TArtifactPrefixName10010 => null;

		public static TLocalizedText TArtifactPrefixName10011 => null;

		public static TLocalizedText TArtifactPrefixName10012 => null;

		public static TLocalizedText TArtifactPrefixName10013 => null;

		public static TLocalizedText TArtifactPrefixName10014 => null;

		public static TLocalizedText TArtifactPrefixName10015 => null;

		public static TLocalizedText TArtifactPrefixName10016 => null;

		public static TLocalizedText TArtifactPrefixName10017 => null;

		public static TLocalizedText TArtifactPrefixName10018 => null;

		public static TLocalizedText TArtifactPrefixName10019 => null;

		public static TLocalizedText TArtifactPrefixName10020 => null;

		public static TLocalizedText TArtifactPrefixName10021 => null;

		public static TLocalizedText TArtifactPrefixName10022 => null;

		public static TLocalizedText TArtifactPrefixName10023 => null;

		public static TLocalizedText TArtifactPrefixName10024 => null;

		public static TLocalizedText TArtifactPrefixName10025 => null;

		public static TLocalizedText TArtifactPrefixName10026 => null;

		public static TLocalizedText TArtifactPrefixName10027 => null;

		public static TLocalizedText TArtifactPrefixName10028 => null;

		public static TLocalizedText TArtifactPrefixName10029 => null;

		public static TLocalizedText TArtifactPrefixName10030 => null;

		public static TLocalizedText TArtifactPrefixName20001 => null;

		public static TLocalizedText TArtifactPrefixName20002 => null;

		public static TLocalizedText TArtifactPrefixName20003 => null;

		public static TLocalizedText TArtifactPrefixName20004 => null;

		public static TLocalizedText TArtifactPrefixName20005 => null;

		public static TLocalizedText TArtifactPrefixName20006 => null;

		public static TLocalizedText TArtifactPrefixName20007 => null;

		public static TLocalizedText TArtifactPrefixName20008 => null;

		public static TLocalizedText TArtifactPrefixName20009 => null;

		public static TLocalizedText TArtifactPrefixName20010 => null;

		public static TLocalizedText TArtifactPrefixName20011 => null;

		public static TLocalizedText TArtifactPrefixName20012 => null;

		public static TLocalizedText TArtifactPrefixName20013 => null;

		public static TLocalizedText TArtifactPrefixName20014 => null;

		public static TLocalizedText TArtifactPrefixName20015 => null;

		public static TLocalizedText TArtifactPrefixName20016 => null;

		public static TLocalizedText TArtifactPrefixName20017 => null;

		public static TLocalizedText TArtifactPrefixName20018 => null;

		public static TLocalizedText TArtifactPrefixName20019 => null;

		public static TLocalizedText TArtifactPrefixName20020 => null;

		public static TLocalizedText TArtifactPrefixName20021 => null;

		public static TLocalizedText TArtifactPrefixName20022 => null;

		public static TLocalizedText TArtifactPrefixName20023 => null;

		public static TLocalizedText TArtifactPrefixName20024 => null;

		public static TLocalizedText TArtifactPrefixName20025 => null;

		public static TLocalizedText TArtifactPrefixName20026 => null;

		public static TLocalizedText TArtifactPrefixName20027 => null;

		public static TLocalizedText TArtifactPrefixName20028 => null;

		public static TLocalizedText TArtifactPrefixName20029 => null;

		public static TLocalizedText TArtifactPrefixName20030 => null;

		public static TLocalizedText TBagName20001 => null;

		public static TLocalizedText TBagDesc20001 => null;

		public static TLocalizedText TBagName20002 => null;

		public static TLocalizedText TBagDesc20002 => null;

		public static TLocalizedText TBagName20011 => null;

		public static TLocalizedText TBagDesc20011 => null;

		public static TLocalizedText TBagName20012 => null;

		public static TLocalizedText TBagDesc20012 => null;

		public static TLocalizedText TBagName20021 => null;

		public static TLocalizedText TBagDesc20021 => null;

		public static TLocalizedText TBagName20022 => null;

		public static TLocalizedText TBagDesc20022 => null;

		public static TLocalizedText TBuffName1 => null;

		public static TLocalizedText TBuffDesc1 => null;

		public static TLocalizedText TBuffName2 => null;

		public static TLocalizedText TBuffDesc2 => null;

		public static TLocalizedText TBuffName5 => null;

		public static TLocalizedText TBuffDesc5 => null;

		public static TLocalizedText TBuffName101 => null;

		public static TLocalizedText TBuffDesc101 => null;

		public static TLocalizedText TBuffName102 => null;

		public static TLocalizedText TBuffDesc102 => null;

		public static TLocalizedText TBuffName103 => null;

		public static TLocalizedText TBuffDesc103 => null;

		public static TLocalizedText TBuffName104 => null;

		public static TLocalizedText TBuffDesc104 => null;

		public static TLocalizedText TBuffName1011 => null;

		public static TLocalizedText TBuffDesc1011 => null;

		public static TLocalizedText TBuffName1012 => null;

		public static TLocalizedText TBuffDesc1012 => null;

		public static TLocalizedText TBuffName1013 => null;

		public static TLocalizedText TBuffDesc1013 => null;

		public static TLocalizedText TBuffName1014 => null;

		public static TLocalizedText TBuffDesc1014 => null;

		public static TLocalizedText TBuffName1015 => null;

		public static TLocalizedText TBuffDesc1015 => null;

		public static TLocalizedText TBuffName1016 => null;

		public static TLocalizedText TBuffDesc1016 => null;

		public static TLocalizedText TBuffName1017 => null;

		public static TLocalizedText TBuffDesc1017 => null;

		public static TLocalizedText TBuffName1018 => null;

		public static TLocalizedText TBuffDesc1018 => null;

		public static TLocalizedText TBuffName1019 => null;

		public static TLocalizedText TBuffDesc1019 => null;

		public static TLocalizedText TBuffName1020 => null;

		public static TLocalizedText TBuffDesc1020 => null;

		public static TLocalizedText TBuffName1021 => null;

		public static TLocalizedText TBuffDesc1021 => null;

		public static TLocalizedText TBuffName1022 => null;

		public static TLocalizedText TBuffDesc1022 => null;

		public static TLocalizedText TBuffName1023 => null;

		public static TLocalizedText TBuffDesc1023 => null;

		public static TLocalizedText TBuffName1024 => null;

		public static TLocalizedText TBuffDesc1024 => null;

		public static TLocalizedText TBuffName1025 => null;

		public static TLocalizedText TBuffDesc1025 => null;

		public static TLocalizedText TBuffName1100 => null;

		public static TLocalizedText TBuffDesc1100 => null;

		public static TLocalizedText TBuffName1101 => null;

		public static TLocalizedText TBuffDesc1101 => null;

		public static TLocalizedText TBuffName1102 => null;

		public static TLocalizedText TBuffDesc1102 => null;

		public static TLocalizedText TBuffName1103 => null;

		public static TLocalizedText TBuffDesc1103 => null;

		public static TLocalizedText TBuffName1104 => null;

		public static TLocalizedText TBuffDesc1104 => null;

		public static TLocalizedText TBuffName1105 => null;

		public static TLocalizedText TBuffDesc1105 => null;

		public static TLocalizedText TBuffName1106 => null;

		public static TLocalizedText TBuffDesc1106 => null;

		public static TLocalizedText TBuffName1107 => null;

		public static TLocalizedText TBuffDesc1107 => null;

		public static TLocalizedText TBuffName1108 => null;

		public static TLocalizedText TBuffDesc1108 => null;

		public static TLocalizedText TBuffName1109 => null;

		public static TLocalizedText TBuffDesc1109 => null;

		public static TLocalizedText TBuffName1110 => null;

		public static TLocalizedText TBuffDesc1110 => null;

		public static TLocalizedText TBuffName1111 => null;

		public static TLocalizedText TBuffDesc1111 => null;

		public static TLocalizedText TBuffName1112 => null;

		public static TLocalizedText TBuffDesc1112 => null;

		public static TLocalizedText TBuffName1113 => null;

		public static TLocalizedText TBuffDesc1113 => null;

		public static TLocalizedText TBuffName1200 => null;

		public static TLocalizedText TBuffDesc1200 => null;

		public static TLocalizedText TBuffName1201 => null;

		public static TLocalizedText TBuffDesc1201 => null;

		public static TLocalizedText TBuffName1202 => null;

		public static TLocalizedText TBuffDesc1202 => null;

		public static TLocalizedText TBuffName1203 => null;

		public static TLocalizedText TBuffDesc1203 => null;

		public static TLocalizedText TBuffName1204 => null;

		public static TLocalizedText TBuffDesc1204 => null;

		public static TLocalizedText TBuffName1205 => null;

		public static TLocalizedText TBuffDesc1205 => null;

		public static TLocalizedText TBuffName1206 => null;

		public static TLocalizedText TBuffDesc1206 => null;

		public static TLocalizedText TBuffName1207 => null;

		public static TLocalizedText TBuffDesc1207 => null;

		public static TLocalizedText TBuffName1300 => null;

		public static TLocalizedText TBuffDesc1300 => null;

		public static TLocalizedText TBuffName1301 => null;

		public static TLocalizedText TBuffDesc1301 => null;

		public static TLocalizedText TBuffName1302 => null;

		public static TLocalizedText TBuffDesc1302 => null;

		public static TLocalizedText TBuffName1303 => null;

		public static TLocalizedText TBuffDesc1303 => null;

		public static TLocalizedText TBuffName1304 => null;

		public static TLocalizedText TBuffDesc1304 => null;

		public static TLocalizedText TBuffName1305 => null;

		public static TLocalizedText TBuffDesc1305 => null;

		public static TLocalizedText TBuffName1306 => null;

		public static TLocalizedText TBuffDesc1306 => null;

		public static TLocalizedText TBuffName1307 => null;

		public static TLocalizedText TBuffDesc1307 => null;

		public static TLocalizedText TBuffName1308 => null;

		public static TLocalizedText TBuffDesc1308 => null;

		public static TLocalizedText TBuffName1309 => null;

		public static TLocalizedText TBuffDesc1309 => null;

		public static TLocalizedText TBuffName1310 => null;

		public static TLocalizedText TBuffDesc1310 => null;

		public static TLocalizedText TBuffName1311 => null;

		public static TLocalizedText TBuffDesc1311 => null;

		public static TLocalizedText TBuffName1312 => null;

		public static TLocalizedText TBuffDesc1312 => null;

		public static TLocalizedText TBuffName1313 => null;

		public static TLocalizedText TBuffDesc1313 => null;

		public static TLocalizedText TBuffName2000 => null;

		public static TLocalizedText TBuffDesc2000 => null;

		public static TLocalizedText TBuffName2001 => null;

		public static TLocalizedText TBuffDesc2001 => null;

		public static TLocalizedText TBuffName2002 => null;

		public static TLocalizedText TBuffDesc2002 => null;

		public static TLocalizedText TBuffName2003 => null;

		public static TLocalizedText TBuffDesc2003 => null;

		public static TLocalizedText TBuffName2004 => null;

		public static TLocalizedText TBuffDesc2004 => null;

		public static TLocalizedText TBuffName2005 => null;

		public static TLocalizedText TBuffDesc2005 => null;

		public static TLocalizedText TBuffName2006 => null;

		public static TLocalizedText TBuffDesc2006 => null;

		public static TLocalizedText TBuffName2007 => null;

		public static TLocalizedText TBuffDesc2007 => null;

		public static TLocalizedText TBuffName2008 => null;

		public static TLocalizedText TBuffDesc2008 => null;

		public static TLocalizedText TBuffName2009 => null;

		public static TLocalizedText TBuffDesc2009 => null;

		public static TLocalizedText TBuffName2010 => null;

		public static TLocalizedText TBuffDesc2010 => null;

		public static TLocalizedText TBuffName2011 => null;

		public static TLocalizedText TBuffDesc2011 => null;

		public static TLocalizedText TBuffName2014 => null;

		public static TLocalizedText TBuffDesc2014 => null;

		public static TLocalizedText TBuffName2015 => null;

		public static TLocalizedText TBuffDesc2015 => null;

		public static TLocalizedText TBuffName2016 => null;

		public static TLocalizedText TBuffDesc2016 => null;

		public static TLocalizedText TBuffName2017 => null;

		public static TLocalizedText TBuffDesc2017 => null;

		public static TLocalizedText TBuffName2018 => null;

		public static TLocalizedText TBuffDesc2018 => null;

		public static TLocalizedText TBuffName2019 => null;

		public static TLocalizedText TBuffDesc2019 => null;

		public static TLocalizedText TBuffName2020 => null;

		public static TLocalizedText TBuffDesc2020 => null;

		public static TLocalizedText TBuffName2021 => null;

		public static TLocalizedText TBuffDesc2021 => null;

		public static TLocalizedText TBuffName2022 => null;

		public static TLocalizedText TBuffDesc2022 => null;

		public static TLocalizedText TBuffName2023 => null;

		public static TLocalizedText TBuffDesc2023 => null;

		public static TLocalizedText TBuffName2024 => null;

		public static TLocalizedText TBuffDesc2024 => null;

		public static TLocalizedText TBuffName2025 => null;

		public static TLocalizedText TBuffDesc2025 => null;

		public static TLocalizedText TBuffName2026 => null;

		public static TLocalizedText TBuffDesc2026 => null;

		public static TLocalizedText TBuffName2027 => null;

		public static TLocalizedText TBuffDesc2027 => null;

		public static TLocalizedText TBuffName2028 => null;

		public static TLocalizedText TBuffDesc2028 => null;

		public static TLocalizedText TBuffName2029 => null;

		public static TLocalizedText TBuffDesc2029 => null;

		public static TLocalizedText TBuffName2030 => null;

		public static TLocalizedText TBuffDesc2030 => null;

		public static TLocalizedText TBuffName2031 => null;

		public static TLocalizedText TBuffDesc2031 => null;

		public static TLocalizedText TBuffName2032 => null;

		public static TLocalizedText TBuffDesc2032 => null;

		public static TLocalizedText TBuffName2033 => null;

		public static TLocalizedText TBuffDesc2033 => null;

		public static TLocalizedText TBuffName2034 => null;

		public static TLocalizedText TBuffDesc2034 => null;

		public static TLocalizedText TBuffName2035 => null;

		public static TLocalizedText TBuffDesc2035 => null;

		public static TLocalizedText TBuffName2036 => null;

		public static TLocalizedText TBuffDesc2036 => null;

		public static TLocalizedText TBuffName2037 => null;

		public static TLocalizedText TBuffDesc2037 => null;

		public static TLocalizedText TBuffName2038 => null;

		public static TLocalizedText TBuffDesc2038 => null;

		public static TLocalizedText TBuffName2039 => null;

		public static TLocalizedText TBuffDesc2039 => null;

		public static TLocalizedText TBuffName2040 => null;

		public static TLocalizedText TBuffDesc2040 => null;

		public static TLocalizedText TBuffName2050 => null;

		public static TLocalizedText TBuffDesc2050 => null;

		public static TLocalizedText TBuffName2051 => null;

		public static TLocalizedText TBuffDesc2051 => null;

		public static TLocalizedText TBuffName2052 => null;

		public static TLocalizedText TBuffDesc2052 => null;

		public static TLocalizedText TBuffName2053 => null;

		public static TLocalizedText TBuffDesc2053 => null;

		public static TLocalizedText TBuffName2054 => null;

		public static TLocalizedText TBuffDesc2054 => null;

		public static TLocalizedText TBuffName2055 => null;

		public static TLocalizedText TBuffDesc2055 => null;

		public static TLocalizedText TBuffName2056 => null;

		public static TLocalizedText TBuffDesc2056 => null;

		public static TLocalizedText TBuffName2057 => null;

		public static TLocalizedText TBuffDesc2057 => null;

		public static TLocalizedText TBuffName2058 => null;

		public static TLocalizedText TBuffDesc2058 => null;

		public static TLocalizedText TBuffName2059 => null;

		public static TLocalizedText TBuffDesc2059 => null;

		public static TLocalizedText TBuffName2060 => null;

		public static TLocalizedText TBuffDesc2060 => null;

		public static TLocalizedText TBuffName2061 => null;

		public static TLocalizedText TBuffDesc2061 => null;

		public static TLocalizedText TBuffName2062 => null;

		public static TLocalizedText TBuffDesc2062 => null;

		public static TLocalizedText TBuffName2063 => null;

		public static TLocalizedText TBuffDesc2063 => null;

		public static TLocalizedText TBuffName2066 => null;

		public static TLocalizedText TBuffDesc2066 => null;

		public static TLocalizedText TBuffName2067 => null;

		public static TLocalizedText TBuffDesc2067 => null;

		public static TLocalizedText TBuffName2068 => null;

		public static TLocalizedText TBuffDesc2068 => null;

		public static TLocalizedText TBuffName2069 => null;

		public static TLocalizedText TBuffDesc2069 => null;

		public static TLocalizedText TBuffName2070 => null;

		public static TLocalizedText TBuffDesc2070 => null;

		public static TLocalizedText TBuffName2071 => null;

		public static TLocalizedText TBuffDesc2071 => null;

		public static TLocalizedText TBuffName2072 => null;

		public static TLocalizedText TBuffDesc2072 => null;

		public static TLocalizedText TBuffName2073 => null;

		public static TLocalizedText TBuffDesc2073 => null;

		public static TLocalizedText TBuffName2074 => null;

		public static TLocalizedText TBuffDesc2074 => null;

		public static TLocalizedText TBuffName2075 => null;

		public static TLocalizedText TBuffDesc2075 => null;

		public static TLocalizedText TBuffName2076 => null;

		public static TLocalizedText TBuffDesc2076 => null;

		public static TLocalizedText TBuffName2077 => null;

		public static TLocalizedText TBuffDesc2077 => null;

		public static TLocalizedText TBuffName2078 => null;

		public static TLocalizedText TBuffDesc2078 => null;

		public static TLocalizedText TBuffName2079 => null;

		public static TLocalizedText TBuffDesc2079 => null;

		public static TLocalizedText TBuffName2080 => null;

		public static TLocalizedText TBuffDesc2080 => null;

		public static TLocalizedText TBuffName2081 => null;

		public static TLocalizedText TBuffDesc2081 => null;

		public static TLocalizedText TBuffName2082 => null;

		public static TLocalizedText TBuffDesc2082 => null;

		public static TLocalizedText TBuffName2084 => null;

		public static TLocalizedText TBuffDesc2084 => null;

		public static TLocalizedText TBuffName2085 => null;

		public static TLocalizedText TBuffDesc2085 => null;

		public static TLocalizedText TBuffName2086 => null;

		public static TLocalizedText TBuffDesc2086 => null;

		public static TLocalizedText TBuffName2087 => null;

		public static TLocalizedText TBuffDesc2087 => null;

		public static TLocalizedText TBuffName2088 => null;

		public static TLocalizedText TBuffDesc2088 => null;

		public static TLocalizedText TBuffName2089 => null;

		public static TLocalizedText TBuffDesc2089 => null;

		public static TLocalizedText TBuffName2090 => null;

		public static TLocalizedText TBuffDesc2090 => null;

		public static TLocalizedText TBuffName2091 => null;

		public static TLocalizedText TBuffDesc2091 => null;

		public static TLocalizedText TBuffName3000 => null;

		public static TLocalizedText TBuffDesc3000 => null;

		public static TLocalizedText TBuffName3001 => null;

		public static TLocalizedText TBuffDesc3001 => null;

		public static TLocalizedText TBuffName3002 => null;

		public static TLocalizedText TBuffDesc3002 => null;

		public static TLocalizedText TBuffName3011 => null;

		public static TLocalizedText TBuffDesc3011 => null;

		public static TLocalizedText TBuffName3012 => null;

		public static TLocalizedText TBuffDesc3012 => null;

		public static TLocalizedText TBuffName3013 => null;

		public static TLocalizedText TBuffDesc3013 => null;

		public static TLocalizedText TBuffName3021 => null;

		public static TLocalizedText TBuffDesc3021 => null;

		public static TLocalizedText TBuffName3022 => null;

		public static TLocalizedText TBuffDesc3022 => null;

		public static TLocalizedText TBuffName3023 => null;

		public static TLocalizedText TBuffDesc3023 => null;

		public static TLocalizedText TBuffName3031 => null;

		public static TLocalizedText TBuffDesc3031 => null;

		public static TLocalizedText TBuffName3051 => null;

		public static TLocalizedText TBuffDesc3051 => null;

		public static TLocalizedText TBuffName3100 => null;

		public static TLocalizedText TBuffDesc3100 => null;

		public static TLocalizedText TBuffName3101 => null;

		public static TLocalizedText TBuffDesc3101 => null;

		public static TLocalizedText TBuffName3102 => null;

		public static TLocalizedText TBuffDesc3102 => null;

		public static TLocalizedText TBuffName3103 => null;

		public static TLocalizedText TBuffDesc3103 => null;

		public static TLocalizedText TBuffName3104 => null;

		public static TLocalizedText TBuffDesc3104 => null;

		public static TLocalizedText TBuffName3105 => null;

		public static TLocalizedText TBuffDesc3105 => null;

		public static TLocalizedText TBuffName3106 => null;

		public static TLocalizedText TBuffDesc3106 => null;

		public static TLocalizedText TBuffName3107 => null;

		public static TLocalizedText TBuffDesc3107 => null;

		public static TLocalizedText TBuffName3108 => null;

		public static TLocalizedText TBuffDesc3108 => null;

		public static TLocalizedText TBuffName3109 => null;

		public static TLocalizedText TBuffDesc3109 => null;

		public static TLocalizedText TBuffName3110 => null;

		public static TLocalizedText TBuffDesc3110 => null;

		public static TLocalizedText TBuffName3111 => null;

		public static TLocalizedText TBuffDesc3111 => null;

		public static TLocalizedText TBuffName3112 => null;

		public static TLocalizedText TBuffDesc3112 => null;

		public static TLocalizedText TBuffName3113 => null;

		public static TLocalizedText TBuffDesc3113 => null;

		public static TLocalizedText TBuffName3114 => null;

		public static TLocalizedText TBuffDesc3114 => null;

		public static TLocalizedText TBuffName3115 => null;

		public static TLocalizedText TBuffDesc3115 => null;

		public static TLocalizedText TBuffName3500 => null;

		public static TLocalizedText TBuffDesc3500 => null;

		public static TLocalizedText TBuffName3501 => null;

		public static TLocalizedText TBuffDesc3501 => null;

		public static TLocalizedText TBuffName3502 => null;

		public static TLocalizedText TBuffDesc3502 => null;

		public static TLocalizedText TBuffName3600 => null;

		public static TLocalizedText TBuffDesc3600 => null;

		public static TLocalizedText TBuffName3601 => null;

		public static TLocalizedText TBuffDesc3601 => null;

		public static TLocalizedText TBuffName3602 => null;

		public static TLocalizedText TBuffDesc3602 => null;

		public static TLocalizedText TBuffName5001 => null;

		public static TLocalizedText TBuffDesc5001 => null;

		public static TLocalizedText TBuffName5002 => null;

		public static TLocalizedText TBuffDesc5002 => null;

		public static TLocalizedText TBuffName5005 => null;

		public static TLocalizedText TBuffDesc5005 => null;

		public static TLocalizedText TBuffName5007 => null;

		public static TLocalizedText TBuffDesc5007 => null;

		public static TLocalizedText TBuffName5008 => null;

		public static TLocalizedText TBuffDesc5008 => null;

		public static TLocalizedText TBuffName5009 => null;

		public static TLocalizedText TBuffDesc5009 => null;

		public static TLocalizedText TBuffName6000 => null;

		public static TLocalizedText TBuffDesc6000 => null;

		public static TLocalizedText TBuffName6001 => null;

		public static TLocalizedText TBuffDesc6001 => null;

		public static TLocalizedText TBuffName6002 => null;

		public static TLocalizedText TBuffDesc6002 => null;

		public static TLocalizedText TBuffName6003 => null;

		public static TLocalizedText TBuffDesc6003 => null;

		public static TLocalizedText TBuffName6004 => null;

		public static TLocalizedText TBuffDesc6004 => null;

		public static TLocalizedText TBuffName6101 => null;

		public static TLocalizedText TBuffDesc6101 => null;

		public static TLocalizedText TBuffName6201 => null;

		public static TLocalizedText TBuffDesc6201 => null;

		public static TLocalizedText TBuffName7001 => null;

		public static TLocalizedText TBuffDesc7001 => null;

		public static TLocalizedText TBuffName7002 => null;

		public static TLocalizedText TBuffDesc7002 => null;

		public static TLocalizedText TBuffName7003 => null;

		public static TLocalizedText TBuffDesc7003 => null;

		public static TLocalizedText TBuffName7004 => null;

		public static TLocalizedText TBuffDesc7004 => null;

		public static TLocalizedText TBuffName7005 => null;

		public static TLocalizedText TBuffDesc7005 => null;

		public static TLocalizedText TBuffName7101 => null;

		public static TLocalizedText TBuffDesc7101 => null;

		public static TLocalizedText TBuffName7102 => null;

		public static TLocalizedText TBuffDesc7102 => null;

		public static TLocalizedText TBuffName7103 => null;

		public static TLocalizedText TBuffDesc7103 => null;

		public static TLocalizedText TBuffName7201 => null;

		public static TLocalizedText TBuffDesc7201 => null;

		public static TLocalizedText TBuffName7202 => null;

		public static TLocalizedText TBuffDesc7202 => null;

		public static TLocalizedText TBuffName7203 => null;

		public static TLocalizedText TBuffDesc7203 => null;

		public static TLocalizedText TBuffName7204 => null;

		public static TLocalizedText TBuffDesc7204 => null;

		public static TLocalizedText TBuffName7205 => null;

		public static TLocalizedText TBuffDesc7205 => null;

		public static TLocalizedText TBuffName7206 => null;

		public static TLocalizedText TBuffDesc7206 => null;

		public static TLocalizedText TBuffName7207 => null;

		public static TLocalizedText TBuffDesc7207 => null;

		public static TLocalizedText TBuffName7208 => null;

		public static TLocalizedText TBuffDesc7208 => null;

		public static TLocalizedText TBuffName10003 => null;

		public static TLocalizedText TBuffDesc10003 => null;

		public static TLocalizedText TBuffName10004 => null;

		public static TLocalizedText TBuffDesc10004 => null;

		public static TLocalizedText TBuffName10005 => null;

		public static TLocalizedText TBuffDesc10005 => null;

		public static TLocalizedText TBuffName20001 => null;

		public static TLocalizedText TBuffDesc20001 => null;

		public static TLocalizedText TBuffName20002 => null;

		public static TLocalizedText TBuffDesc20002 => null;

		public static TLocalizedText TBuffName20003 => null;

		public static TLocalizedText TBuffDesc20003 => null;

		public static TLocalizedText TBuffName20004 => null;

		public static TLocalizedText TBuffDesc20004 => null;

		public static TLocalizedText TBuffName20005 => null;

		public static TLocalizedText TBuffDesc20005 => null;

		public static TLocalizedText TBuffName20011 => null;

		public static TLocalizedText TBuffDesc20011 => null;

		public static TLocalizedText TBuffName20012 => null;

		public static TLocalizedText TBuffDesc20012 => null;

		public static TLocalizedText TBuffName20013 => null;

		public static TLocalizedText TBuffDesc20013 => null;

		public static TLocalizedText TBuffName20014 => null;

		public static TLocalizedText TBuffDesc20014 => null;

		public static TLocalizedText TBuffName20015 => null;

		public static TLocalizedText TBuffDesc20015 => null;

		public static TLocalizedText TBuffName20021 => null;

		public static TLocalizedText TBuffDesc20021 => null;

		public static TLocalizedText TBuffName20022 => null;

		public static TLocalizedText TBuffDesc20022 => null;

		public static TLocalizedText TBuffName20023 => null;

		public static TLocalizedText TBuffDesc20023 => null;

		public static TLocalizedText TBuffName20024 => null;

		public static TLocalizedText TBuffDesc20024 => null;

		public static TLocalizedText TBuffName20025 => null;

		public static TLocalizedText TBuffDesc20025 => null;

		public static TLocalizedText TBuffName20031 => null;

		public static TLocalizedText TBuffDesc20031 => null;

		public static TLocalizedText TBuffName20032 => null;

		public static TLocalizedText TBuffDesc20032 => null;

		public static TLocalizedText TBuffName20033 => null;

		public static TLocalizedText TBuffDesc20033 => null;

		public static TLocalizedText TBuffName20034 => null;

		public static TLocalizedText TBuffDesc20034 => null;

		public static TLocalizedText TBuffName20035 => null;

		public static TLocalizedText TBuffDesc20035 => null;

		public static TLocalizedText TBuffName20041 => null;

		public static TLocalizedText TBuffDesc20041 => null;

		public static TLocalizedText TBuffName20042 => null;

		public static TLocalizedText TBuffDesc20042 => null;

		public static TLocalizedText TBuffName20051 => null;

		public static TLocalizedText TBuffDesc20051 => null;

		public static TLocalizedText TBuffName20052 => null;

		public static TLocalizedText TBuffDesc20052 => null;

		public static TLocalizedText TBuffName20053 => null;

		public static TLocalizedText TBuffDesc20053 => null;

		public static TLocalizedText TBuffName20061 => null;

		public static TLocalizedText TBuffDesc20061 => null;

		public static TLocalizedText TBuffName20062 => null;

		public static TLocalizedText TBuffDesc20062 => null;

		public static TLocalizedText TBuffName20063 => null;

		public static TLocalizedText TBuffDesc20063 => null;

		public static TLocalizedText TBuffName20064 => null;

		public static TLocalizedText TBuffDesc20064 => null;

		public static TLocalizedText TBuffName20065 => null;

		public static TLocalizedText TBuffDesc20065 => null;

		public static TLocalizedText TBuffName20071 => null;

		public static TLocalizedText TBuffDesc20071 => null;

		public static TLocalizedText TBuffName20072 => null;

		public static TLocalizedText TBuffDesc20072 => null;

		public static TLocalizedText TBuffName20073 => null;

		public static TLocalizedText TBuffDesc20073 => null;

		public static TLocalizedText TBuffName20074 => null;

		public static TLocalizedText TBuffDesc20074 => null;

		public static TLocalizedText TBuffName20075 => null;

		public static TLocalizedText TBuffDesc20075 => null;

		public static TLocalizedText TBuffName20081 => null;

		public static TLocalizedText TBuffDesc20081 => null;

		public static TLocalizedText TBuffName20082 => null;

		public static TLocalizedText TBuffDesc20082 => null;

		public static TLocalizedText TBuffName20083 => null;

		public static TLocalizedText TBuffDesc20083 => null;

		public static TLocalizedText TBuffName20091 => null;

		public static TLocalizedText TBuffDesc20091 => null;

		public static TLocalizedText TBuffName20092 => null;

		public static TLocalizedText TBuffDesc20092 => null;

		public static TLocalizedText TBuffName20093 => null;

		public static TLocalizedText TBuffDesc20093 => null;

		public static TLocalizedText TBuffName20101 => null;

		public static TLocalizedText TBuffDesc20101 => null;

		public static TLocalizedText TBuffName20102 => null;

		public static TLocalizedText TBuffDesc20102 => null;

		public static TLocalizedText TBuffName20103 => null;

		public static TLocalizedText TBuffDesc20103 => null;

		public static TLocalizedText TBuffName20111 => null;

		public static TLocalizedText TBuffDesc20111 => null;

		public static TLocalizedText TBuffName20112 => null;

		public static TLocalizedText TBuffDesc20112 => null;

		public static TLocalizedText TBuffName20113 => null;

		public static TLocalizedText TBuffDesc20113 => null;

		public static TLocalizedText TBuffName20121 => null;

		public static TLocalizedText TBuffDesc20121 => null;

		public static TLocalizedText TBuffName20122 => null;

		public static TLocalizedText TBuffDesc20122 => null;

		public static TLocalizedText TBuffName20123 => null;

		public static TLocalizedText TBuffDesc20123 => null;

		public static TLocalizedText TBuffName20131 => null;

		public static TLocalizedText TBuffDesc20131 => null;

		public static TLocalizedText TBuffName20132 => null;

		public static TLocalizedText TBuffDesc20132 => null;

		public static TLocalizedText TBuffName20133 => null;

		public static TLocalizedText TBuffDesc20133 => null;

		public static TLocalizedText TBuffName20141 => null;

		public static TLocalizedText TBuffDesc20141 => null;

		public static TLocalizedText TBuffName20142 => null;

		public static TLocalizedText TBuffDesc20142 => null;

		public static TLocalizedText TBuffName20143 => null;

		public static TLocalizedText TBuffDesc20143 => null;

		public static TLocalizedText TBuffName20151 => null;

		public static TLocalizedText TBuffDesc20151 => null;

		public static TLocalizedText TBuffName20152 => null;

		public static TLocalizedText TBuffDesc20152 => null;

		public static TLocalizedText TBuffName20153 => null;

		public static TLocalizedText TBuffDesc20153 => null;

		public static TLocalizedText TBuffName20161 => null;

		public static TLocalizedText TBuffDesc20161 => null;

		public static TLocalizedText TBuffName20162 => null;

		public static TLocalizedText TBuffDesc20162 => null;

		public static TLocalizedText TBuffName20163 => null;

		public static TLocalizedText TBuffDesc20163 => null;

		public static TLocalizedText TBuffName20171 => null;

		public static TLocalizedText TBuffDesc20171 => null;

		public static TLocalizedText TBuffName20172 => null;

		public static TLocalizedText TBuffDesc20172 => null;

		public static TLocalizedText TBuffName20173 => null;

		public static TLocalizedText TBuffDesc20173 => null;

		public static TLocalizedText TBuffName20181 => null;

		public static TLocalizedText TBuffDesc20181 => null;

		public static TLocalizedText TBuffName20182 => null;

		public static TLocalizedText TBuffDesc20182 => null;

		public static TLocalizedText TBuffName20183 => null;

		public static TLocalizedText TBuffDesc20183 => null;

		public static TLocalizedText TBuffName20191 => null;

		public static TLocalizedText TBuffDesc20191 => null;

		public static TLocalizedText TBuffName20192 => null;

		public static TLocalizedText TBuffDesc20192 => null;

		public static TLocalizedText TBuffName20193 => null;

		public static TLocalizedText TBuffDesc20193 => null;

		public static TLocalizedText TBuffName20201 => null;

		public static TLocalizedText TBuffDesc20201 => null;

		public static TLocalizedText TBuffName20202 => null;

		public static TLocalizedText TBuffDesc20202 => null;

		public static TLocalizedText TBuffName20203 => null;

		public static TLocalizedText TBuffDesc20203 => null;

		public static TLocalizedText TBuffName20211 => null;

		public static TLocalizedText TBuffDesc20211 => null;

		public static TLocalizedText TBuffName20212 => null;

		public static TLocalizedText TBuffDesc20212 => null;

		public static TLocalizedText TBuffName20213 => null;

		public static TLocalizedText TBuffDesc20213 => null;

		public static TLocalizedText TBuffName20221 => null;

		public static TLocalizedText TBuffDesc20221 => null;

		public static TLocalizedText TBuffName20222 => null;

		public static TLocalizedText TBuffDesc20222 => null;

		public static TLocalizedText TBuffName20223 => null;

		public static TLocalizedText TBuffDesc20223 => null;

		public static TLocalizedText TBuffName20231 => null;

		public static TLocalizedText TBuffDesc20231 => null;

		public static TLocalizedText TBuffName20232 => null;

		public static TLocalizedText TBuffDesc20232 => null;

		public static TLocalizedText TBuffName20233 => null;

		public static TLocalizedText TBuffDesc20233 => null;

		public static TLocalizedText TBuffName20241 => null;

		public static TLocalizedText TBuffDesc20241 => null;

		public static TLocalizedText TBuffName20242 => null;

		public static TLocalizedText TBuffDesc20242 => null;

		public static TLocalizedText TBuffName20243 => null;

		public static TLocalizedText TBuffDesc20243 => null;

		public static TLocalizedText TBuffName20251 => null;

		public static TLocalizedText TBuffDesc20251 => null;

		public static TLocalizedText TBuffName20252 => null;

		public static TLocalizedText TBuffDesc20252 => null;

		public static TLocalizedText TBuffName20253 => null;

		public static TLocalizedText TBuffDesc20253 => null;

		public static TLocalizedText TBuffName20254 => null;

		public static TLocalizedText TBuffDesc20254 => null;

		public static TLocalizedText TBuffName20255 => null;

		public static TLocalizedText TBuffDesc20255 => null;

		public static TLocalizedText TBuffName20261 => null;

		public static TLocalizedText TBuffDesc20261 => null;

		public static TLocalizedText TBuffName20262 => null;

		public static TLocalizedText TBuffDesc20262 => null;

		public static TLocalizedText TBuffName20263 => null;

		public static TLocalizedText TBuffDesc20263 => null;

		public static TLocalizedText TBuffName20271 => null;

		public static TLocalizedText TBuffDesc20271 => null;

		public static TLocalizedText TBuffName20272 => null;

		public static TLocalizedText TBuffDesc20272 => null;

		public static TLocalizedText TBuffName20273 => null;

		public static TLocalizedText TBuffDesc20273 => null;

		public static TLocalizedText TBuffName20281 => null;

		public static TLocalizedText TBuffDesc20281 => null;

		public static TLocalizedText TBuffName20282 => null;

		public static TLocalizedText TBuffDesc20282 => null;

		public static TLocalizedText TBuffName20283 => null;

		public static TLocalizedText TBuffDesc20283 => null;

		public static TLocalizedText TBuffName20291 => null;

		public static TLocalizedText TBuffDesc20291 => null;

		public static TLocalizedText TBuffName20292 => null;

		public static TLocalizedText TBuffDesc20292 => null;

		public static TLocalizedText TBuffName20293 => null;

		public static TLocalizedText TBuffDesc20293 => null;

		public static TLocalizedText TBuffName20301 => null;

		public static TLocalizedText TBuffDesc20301 => null;

		public static TLocalizedText TBuffName20302 => null;

		public static TLocalizedText TBuffDesc20302 => null;

		public static TLocalizedText TBuffName20303 => null;

		public static TLocalizedText TBuffDesc20303 => null;

		public static TLocalizedText TBuffName20311 => null;

		public static TLocalizedText TBuffDesc20311 => null;

		public static TLocalizedText TBuffName20312 => null;

		public static TLocalizedText TBuffDesc20312 => null;

		public static TLocalizedText TBuffName20313 => null;

		public static TLocalizedText TBuffDesc20313 => null;

		public static TLocalizedText TBuffName20321 => null;

		public static TLocalizedText TBuffDesc20321 => null;

		public static TLocalizedText TBuffName20322 => null;

		public static TLocalizedText TBuffDesc20322 => null;

		public static TLocalizedText TBuffName20323 => null;

		public static TLocalizedText TBuffDesc20323 => null;

		public static TLocalizedText TBuffName20331 => null;

		public static TLocalizedText TBuffDesc20331 => null;

		public static TLocalizedText TBuffName20332 => null;

		public static TLocalizedText TBuffDesc20332 => null;

		public static TLocalizedText TBuffName20333 => null;

		public static TLocalizedText TBuffDesc20333 => null;

		public static TLocalizedText TBuffName20341 => null;

		public static TLocalizedText TBuffDesc20341 => null;

		public static TLocalizedText TBuffName20342 => null;

		public static TLocalizedText TBuffDesc20342 => null;

		public static TLocalizedText TBuffName30001 => null;

		public static TLocalizedText TBuffDesc30001 => null;

		public static TLocalizedText TBuffName30002 => null;

		public static TLocalizedText TBuffDesc30002 => null;

		public static TLocalizedText TBuffName40001 => null;

		public static TLocalizedText TBuffDesc40001 => null;

		public static TLocalizedText TBuffName40002 => null;

		public static TLocalizedText TBuffDesc40002 => null;

		public static TLocalizedText TBuffName40003 => null;

		public static TLocalizedText TBuffDesc40003 => null;

		public static TLocalizedText TBuffName40004 => null;

		public static TLocalizedText TBuffDesc40004 => null;

		public static TLocalizedText TBuffName40005 => null;

		public static TLocalizedText TBuffDesc40005 => null;

		public static TLocalizedText TBuffName40006 => null;

		public static TLocalizedText TBuffDesc40006 => null;

		public static TLocalizedText TBuffName40007 => null;

		public static TLocalizedText TBuffDesc40007 => null;

		public static TLocalizedText TBuffName40008 => null;

		public static TLocalizedText TBuffDesc40008 => null;

		public static TLocalizedText TBuffName40009 => null;

		public static TLocalizedText TBuffDesc40009 => null;

		public static TLocalizedText TBuffName40010 => null;

		public static TLocalizedText TBuffDesc40010 => null;

		public static TLocalizedText TBuffName40011 => null;

		public static TLocalizedText TBuffDesc40011 => null;

		public static TLocalizedText TBuffName40012 => null;

		public static TLocalizedText TBuffDesc40012 => null;

		public static TLocalizedText TBuffName40013 => null;

		public static TLocalizedText TBuffDesc40013 => null;

		public static TLocalizedText TBuffName40014 => null;

		public static TLocalizedText TBuffDesc40014 => null;

		public static TLocalizedText TBuffName40015 => null;

		public static TLocalizedText TBuffDesc40015 => null;

		public static TLocalizedText TBuffName40016 => null;

		public static TLocalizedText TBuffDesc40016 => null;

		public static TLocalizedText TBuffName40017 => null;

		public static TLocalizedText TBuffDesc40017 => null;

		public static TLocalizedText TBuffName40018 => null;

		public static TLocalizedText TBuffDesc40018 => null;

		public static TLocalizedText TBuffName40019 => null;

		public static TLocalizedText TBuffDesc40019 => null;

		public static TLocalizedText TBuffName40020 => null;

		public static TLocalizedText TBuffDesc40020 => null;

		public static TLocalizedText TBuffName40021 => null;

		public static TLocalizedText TBuffDesc40021 => null;

		public static TLocalizedText TBuffName40022 => null;

		public static TLocalizedText TBuffDesc40022 => null;

		public static TLocalizedText TBuffName40023 => null;

		public static TLocalizedText TBuffDesc40023 => null;

		public static TLocalizedText TBuffName40024 => null;

		public static TLocalizedText TBuffDesc40024 => null;

		public static TLocalizedText TBuffName40025 => null;

		public static TLocalizedText TBuffDesc40025 => null;

		public static TLocalizedText TBuffName40101 => null;

		public static TLocalizedText TBuffDesc40101 => null;

		public static TLocalizedText TBuffName40102 => null;

		public static TLocalizedText TBuffDesc40102 => null;

		public static TLocalizedText TBuffName40103 => null;

		public static TLocalizedText TBuffDesc40103 => null;

		public static TLocalizedText TBuffName40104 => null;

		public static TLocalizedText TBuffDesc40104 => null;

		public static TLocalizedText TBuffName40105 => null;

		public static TLocalizedText TBuffDesc40105 => null;

		public static TLocalizedText TBuffName40106 => null;

		public static TLocalizedText TBuffDesc40106 => null;

		public static TLocalizedText TBuffName40107 => null;

		public static TLocalizedText TBuffDesc40107 => null;

		public static TLocalizedText TBuffName40108 => null;

		public static TLocalizedText TBuffDesc40108 => null;

		public static TLocalizedText TBuffName40109 => null;

		public static TLocalizedText TBuffDesc40109 => null;

		public static TLocalizedText TBuffName40110 => null;

		public static TLocalizedText TBuffDesc40110 => null;

		public static TLocalizedText TBuffName40111 => null;

		public static TLocalizedText TBuffDesc40111 => null;

		public static TLocalizedText TBuffName40112 => null;

		public static TLocalizedText TBuffDesc40112 => null;

		public static TLocalizedText TBuffName40113 => null;

		public static TLocalizedText TBuffDesc40113 => null;

		public static TLocalizedText TBuffName40114 => null;

		public static TLocalizedText TBuffDesc40114 => null;

		public static TLocalizedText TBuffName40115 => null;

		public static TLocalizedText TBuffDesc40115 => null;

		public static TLocalizedText TBuffName40116 => null;

		public static TLocalizedText TBuffDesc40116 => null;

		public static TLocalizedText TBuffName40117 => null;

		public static TLocalizedText TBuffDesc40117 => null;

		public static TLocalizedText TBuffName40118 => null;

		public static TLocalizedText TBuffDesc40118 => null;

		public static TLocalizedText TBuffName40119 => null;

		public static TLocalizedText TBuffDesc40119 => null;

		public static TLocalizedText TBuffName40120 => null;

		public static TLocalizedText TBuffDesc40120 => null;

		public static TLocalizedText TBuffName40121 => null;

		public static TLocalizedText TBuffDesc40121 => null;

		public static TLocalizedText TBuffName40122 => null;

		public static TLocalizedText TBuffDesc40122 => null;

		public static TLocalizedText TBuffName40123 => null;

		public static TLocalizedText TBuffDesc40123 => null;

		public static TLocalizedText TBuffName40124 => null;

		public static TLocalizedText TBuffDesc40124 => null;

		public static TLocalizedText TBuffName40125 => null;

		public static TLocalizedText TBuffDesc40125 => null;

		public static TLocalizedText TBuffName41001 => null;

		public static TLocalizedText TBuffDesc41001 => null;

		public static TLocalizedText TBuffName41002 => null;

		public static TLocalizedText TBuffDesc41002 => null;

		public static TLocalizedText TBuffName41003 => null;

		public static TLocalizedText TBuffDesc41003 => null;

		public static TLocalizedText TBuffName41004 => null;

		public static TLocalizedText TBuffDesc41004 => null;

		public static TLocalizedText TBuffName41005 => null;

		public static TLocalizedText TBuffDesc41005 => null;

		public static TLocalizedText TBuffName41006 => null;

		public static TLocalizedText TBuffDesc41006 => null;

		public static TLocalizedText TBuffName41007 => null;

		public static TLocalizedText TBuffDesc41007 => null;

		public static TLocalizedText TBuffName41008 => null;

		public static TLocalizedText TBuffDesc41008 => null;

		public static TLocalizedText TBuffName41009 => null;

		public static TLocalizedText TBuffDesc41009 => null;

		public static TLocalizedText TBuffName41010 => null;

		public static TLocalizedText TBuffDesc41010 => null;

		public static TLocalizedText TBuffName41011 => null;

		public static TLocalizedText TBuffDesc41011 => null;

		public static TLocalizedText TBuffName41012 => null;

		public static TLocalizedText TBuffDesc41012 => null;

		public static TLocalizedText TBuffName41013 => null;

		public static TLocalizedText TBuffDesc41013 => null;

		public static TLocalizedText TBuffName41014 => null;

		public static TLocalizedText TBuffDesc41014 => null;

		public static TLocalizedText TBuffName41015 => null;

		public static TLocalizedText TBuffDesc41015 => null;

		public static TLocalizedText TBuffName41016 => null;

		public static TLocalizedText TBuffDesc41016 => null;

		public static TLocalizedText TBuffName41017 => null;

		public static TLocalizedText TBuffDesc41017 => null;

		public static TLocalizedText TBuffName41018 => null;

		public static TLocalizedText TBuffDesc41018 => null;

		public static TLocalizedText TBuffName41019 => null;

		public static TLocalizedText TBuffDesc41019 => null;

		public static TLocalizedText TBuffName41020 => null;

		public static TLocalizedText TBuffDesc41020 => null;

		public static TLocalizedText TBuffName41021 => null;

		public static TLocalizedText TBuffDesc41021 => null;

		public static TLocalizedText TBuffName41022 => null;

		public static TLocalizedText TBuffDesc41022 => null;

		public static TLocalizedText TBuffName41023 => null;

		public static TLocalizedText TBuffDesc41023 => null;

		public static TLocalizedText TBuffName41024 => null;

		public static TLocalizedText TBuffDesc41024 => null;

		public static TLocalizedText TBuffName41025 => null;

		public static TLocalizedText TBuffDesc41025 => null;

		public static TLocalizedText TBuffName41101 => null;

		public static TLocalizedText TBuffDesc41101 => null;

		public static TLocalizedText TBuffName41102 => null;

		public static TLocalizedText TBuffDesc41102 => null;

		public static TLocalizedText TBuffName41103 => null;

		public static TLocalizedText TBuffDesc41103 => null;

		public static TLocalizedText TBuffName41104 => null;

		public static TLocalizedText TBuffDesc41104 => null;

		public static TLocalizedText TBuffName41105 => null;

		public static TLocalizedText TBuffDesc41105 => null;

		public static TLocalizedText TBuffName41106 => null;

		public static TLocalizedText TBuffDesc41106 => null;

		public static TLocalizedText TBuffName41107 => null;

		public static TLocalizedText TBuffDesc41107 => null;

		public static TLocalizedText TBuffName41108 => null;

		public static TLocalizedText TBuffDesc41108 => null;

		public static TLocalizedText TBuffName41109 => null;

		public static TLocalizedText TBuffDesc41109 => null;

		public static TLocalizedText TBuffName41110 => null;

		public static TLocalizedText TBuffDesc41110 => null;

		public static TLocalizedText TBuffName41111 => null;

		public static TLocalizedText TBuffDesc41111 => null;

		public static TLocalizedText TBuffName41112 => null;

		public static TLocalizedText TBuffDesc41112 => null;

		public static TLocalizedText TBuffName41113 => null;

		public static TLocalizedText TBuffDesc41113 => null;

		public static TLocalizedText TBuffName41114 => null;

		public static TLocalizedText TBuffDesc41114 => null;

		public static TLocalizedText TBuffName41115 => null;

		public static TLocalizedText TBuffDesc41115 => null;

		public static TLocalizedText TBuffName41116 => null;

		public static TLocalizedText TBuffDesc41116 => null;

		public static TLocalizedText TBuffName41117 => null;

		public static TLocalizedText TBuffDesc41117 => null;

		public static TLocalizedText TBuffName41118 => null;

		public static TLocalizedText TBuffDesc41118 => null;

		public static TLocalizedText TBuffName41119 => null;

		public static TLocalizedText TBuffDesc41119 => null;

		public static TLocalizedText TBuffName41120 => null;

		public static TLocalizedText TBuffDesc41120 => null;

		public static TLocalizedText TBuffName41121 => null;

		public static TLocalizedText TBuffDesc41121 => null;

		public static TLocalizedText TBuffName41122 => null;

		public static TLocalizedText TBuffDesc41122 => null;

		public static TLocalizedText TBuffName41123 => null;

		public static TLocalizedText TBuffDesc41123 => null;

		public static TLocalizedText TBuffName41124 => null;

		public static TLocalizedText TBuffDesc41124 => null;

		public static TLocalizedText TBuffName41125 => null;

		public static TLocalizedText TBuffDesc41125 => null;

		public static TLocalizedText TBuffName42001 => null;

		public static TLocalizedText TBuffDesc42001 => null;

		public static TLocalizedText TBuffName42002 => null;

		public static TLocalizedText TBuffDesc42002 => null;

		public static TLocalizedText TBuffName42003 => null;

		public static TLocalizedText TBuffDesc42003 => null;

		public static TLocalizedText TBuffName42004 => null;

		public static TLocalizedText TBuffDesc42004 => null;

		public static TLocalizedText TBuffName42005 => null;

		public static TLocalizedText TBuffDesc42005 => null;

		public static TLocalizedText TBuffName42006 => null;

		public static TLocalizedText TBuffDesc42006 => null;

		public static TLocalizedText TBuffName42007 => null;

		public static TLocalizedText TBuffDesc42007 => null;

		public static TLocalizedText TBuffName42008 => null;

		public static TLocalizedText TBuffDesc42008 => null;

		public static TLocalizedText TBuffName42009 => null;

		public static TLocalizedText TBuffDesc42009 => null;

		public static TLocalizedText TBuffName42010 => null;

		public static TLocalizedText TBuffDesc42010 => null;

		public static TLocalizedText TBuffName42011 => null;

		public static TLocalizedText TBuffDesc42011 => null;

		public static TLocalizedText TBuffName42012 => null;

		public static TLocalizedText TBuffDesc42012 => null;

		public static TLocalizedText TBuffName42013 => null;

		public static TLocalizedText TBuffDesc42013 => null;

		public static TLocalizedText TBuffName42014 => null;

		public static TLocalizedText TBuffDesc42014 => null;

		public static TLocalizedText TBuffName42015 => null;

		public static TLocalizedText TBuffDesc42015 => null;

		public static TLocalizedText TBuffName42016 => null;

		public static TLocalizedText TBuffDesc42016 => null;

		public static TLocalizedText TBuffName42017 => null;

		public static TLocalizedText TBuffDesc42017 => null;

		public static TLocalizedText TBuffName42018 => null;

		public static TLocalizedText TBuffDesc42018 => null;

		public static TLocalizedText TBuffName42019 => null;

		public static TLocalizedText TBuffDesc42019 => null;

		public static TLocalizedText TBuffName42020 => null;

		public static TLocalizedText TBuffDesc42020 => null;

		public static TLocalizedText TBuffName42101 => null;

		public static TLocalizedText TBuffDesc42101 => null;

		public static TLocalizedText TBuffName42102 => null;

		public static TLocalizedText TBuffDesc42102 => null;

		public static TLocalizedText TBuffName42103 => null;

		public static TLocalizedText TBuffDesc42103 => null;

		public static TLocalizedText TBuffName42104 => null;

		public static TLocalizedText TBuffDesc42104 => null;

		public static TLocalizedText TBuffName42105 => null;

		public static TLocalizedText TBuffDesc42105 => null;

		public static TLocalizedText TBuffName42106 => null;

		public static TLocalizedText TBuffDesc42106 => null;

		public static TLocalizedText TBuffName42107 => null;

		public static TLocalizedText TBuffDesc42107 => null;

		public static TLocalizedText TBuffName42108 => null;

		public static TLocalizedText TBuffDesc42108 => null;

		public static TLocalizedText TBuffName42109 => null;

		public static TLocalizedText TBuffDesc42109 => null;

		public static TLocalizedText TBuffName42110 => null;

		public static TLocalizedText TBuffDesc42110 => null;

		public static TLocalizedText TBuffName42111 => null;

		public static TLocalizedText TBuffDesc42111 => null;

		public static TLocalizedText TBuffName42112 => null;

		public static TLocalizedText TBuffDesc42112 => null;

		public static TLocalizedText TBuffName42113 => null;

		public static TLocalizedText TBuffDesc42113 => null;

		public static TLocalizedText TBuffName42114 => null;

		public static TLocalizedText TBuffDesc42114 => null;

		public static TLocalizedText TBuffName42115 => null;

		public static TLocalizedText TBuffDesc42115 => null;

		public static TLocalizedText TBuffName42116 => null;

		public static TLocalizedText TBuffDesc42116 => null;

		public static TLocalizedText TBuffName42117 => null;

		public static TLocalizedText TBuffDesc42117 => null;

		public static TLocalizedText TBuffName42118 => null;

		public static TLocalizedText TBuffDesc42118 => null;

		public static TLocalizedText TBuffName42119 => null;

		public static TLocalizedText TBuffDesc42119 => null;

		public static TLocalizedText TBuffName42120 => null;

		public static TLocalizedText TBuffDesc42120 => null;

		public static TLocalizedText TBuffName43001 => null;

		public static TLocalizedText TBuffDesc43001 => null;

		public static TLocalizedText TBuffName43002 => null;

		public static TLocalizedText TBuffDesc43002 => null;

		public static TLocalizedText TBuffName43003 => null;

		public static TLocalizedText TBuffDesc43003 => null;

		public static TLocalizedText TBuffName43004 => null;

		public static TLocalizedText TBuffDesc43004 => null;

		public static TLocalizedText TBuffName43005 => null;

		public static TLocalizedText TBuffDesc43005 => null;

		public static TLocalizedText TBuffName43006 => null;

		public static TLocalizedText TBuffDesc43006 => null;

		public static TLocalizedText TBuffName43007 => null;

		public static TLocalizedText TBuffDesc43007 => null;

		public static TLocalizedText TBuffName43008 => null;

		public static TLocalizedText TBuffDesc43008 => null;

		public static TLocalizedText TBuffName43009 => null;

		public static TLocalizedText TBuffDesc43009 => null;

		public static TLocalizedText TBuffName50000 => null;

		public static TLocalizedText TBuffDesc50000 => null;

		public static TLocalizedText TBuffName50001 => null;

		public static TLocalizedText TBuffDesc50001 => null;

		public static TLocalizedText TBuffName55001 => null;

		public static TLocalizedText TBuffDesc55001 => null;

		public static TLocalizedText TBuffName55002 => null;

		public static TLocalizedText TBuffDesc55002 => null;

		public static TLocalizedText TBuffName55003 => null;

		public static TLocalizedText TBuffDesc55003 => null;

		public static TLocalizedText TBuffName55004 => null;

		public static TLocalizedText TBuffDesc55004 => null;

		public static TLocalizedText TBuffName60000 => null;

		public static TLocalizedText TBuffDesc60000 => null;

		public static TLocalizedText TBuffName60001 => null;

		public static TLocalizedText TBuffDesc60001 => null;

		public static TLocalizedText TBuffName60002 => null;

		public static TLocalizedText TBuffDesc60002 => null;

		public static TLocalizedText TBuffName60011 => null;

		public static TLocalizedText TBuffDesc60011 => null;

		public static TLocalizedText TBuffName60012 => null;

		public static TLocalizedText TBuffDesc60012 => null;

		public static TLocalizedText TBuffName60013 => null;

		public static TLocalizedText TBuffDesc60013 => null;

		public static TLocalizedText TBuffName60014 => null;

		public static TLocalizedText TBuffDesc60014 => null;

		public static TLocalizedText TBuffName60015 => null;

		public static TLocalizedText TBuffDesc60015 => null;

		public static TLocalizedText TBuffName60016 => null;

		public static TLocalizedText TBuffDesc60016 => null;

		public static TLocalizedText TBuffName60100 => null;

		public static TLocalizedText TBuffDesc60100 => null;

		public static TLocalizedText TBuffName60101 => null;

		public static TLocalizedText TBuffDesc60101 => null;

		public static TLocalizedText TBuffName70001 => null;

		public static TLocalizedText TBuffDesc70001 => null;

		public static TLocalizedText TBuffName70002 => null;

		public static TLocalizedText TBuffDesc70002 => null;

		public static TLocalizedText TBuffName70003 => null;

		public static TLocalizedText TBuffDesc70003 => null;

		public static TLocalizedText TBuffName70004 => null;

		public static TLocalizedText TBuffDesc70004 => null;

		public static TLocalizedText TBuffName70005 => null;

		public static TLocalizedText TBuffDesc70005 => null;

		public static TLocalizedText TBuffName70006 => null;

		public static TLocalizedText TBuffDesc70006 => null;

		public static TLocalizedText TBuffName70007 => null;

		public static TLocalizedText TBuffDesc70007 => null;

		public static TLocalizedText TBuffName70008 => null;

		public static TLocalizedText TBuffDesc70008 => null;

		public static TLocalizedText TBuffName70009 => null;

		public static TLocalizedText TBuffDesc70009 => null;

		public static TLocalizedText TBuffName70010 => null;

		public static TLocalizedText TBuffDesc70010 => null;

		public static TLocalizedText TBuffName70011 => null;

		public static TLocalizedText TBuffDesc70011 => null;

		public static TLocalizedText TBuffName70012 => null;

		public static TLocalizedText TBuffDesc70012 => null;

		public static TLocalizedText TBuffName70013 => null;

		public static TLocalizedText TBuffDesc70013 => null;

		public static TLocalizedText TBuffName70014 => null;

		public static TLocalizedText TBuffDesc70014 => null;

		public static TLocalizedText TBuffName70015 => null;

		public static TLocalizedText TBuffDesc70015 => null;

		public static TLocalizedText TBuffName70016 => null;

		public static TLocalizedText TBuffDesc70016 => null;

		public static TLocalizedText TBuffName70017 => null;

		public static TLocalizedText TBuffDesc70017 => null;

		public static TLocalizedText TBuffName70018 => null;

		public static TLocalizedText TBuffDesc70018 => null;

		public static TLocalizedText TBuffName70019 => null;

		public static TLocalizedText TBuffDesc70019 => null;

		public static TLocalizedText TBuffName70020 => null;

		public static TLocalizedText TBuffDesc70020 => null;

		public static TLocalizedText TBuffName70021 => null;

		public static TLocalizedText TBuffDesc70021 => null;

		public static TLocalizedText TBuffName70022 => null;

		public static TLocalizedText TBuffDesc70022 => null;

		public static TLocalizedText TBuffName71001 => null;

		public static TLocalizedText TBuffDesc71001 => null;

		public static TLocalizedText TBuffName71002 => null;

		public static TLocalizedText TBuffDesc71002 => null;

		public static TLocalizedText TBuffName71003 => null;

		public static TLocalizedText TBuffDesc71003 => null;

		public static TLocalizedText TBuffName71004 => null;

		public static TLocalizedText TBuffDesc71004 => null;

		public static TLocalizedText TBuffName71005 => null;

		public static TLocalizedText TBuffDesc71005 => null;

		public static TLocalizedText TBuffName71006 => null;

		public static TLocalizedText TBuffDesc71006 => null;

		public static TLocalizedText TBuffName71007 => null;

		public static TLocalizedText TBuffDesc71007 => null;

		public static TLocalizedText TBuffName71008 => null;

		public static TLocalizedText TBuffDesc71008 => null;

		public static TLocalizedText TBuffName71009 => null;

		public static TLocalizedText TBuffDesc71009 => null;

		public static TLocalizedText TBuffName71010 => null;

		public static TLocalizedText TBuffDesc71010 => null;

		public static TLocalizedText TBuffName80001 => null;

		public static TLocalizedText TBuffDesc80001 => null;

		public static TLocalizedText TBuffName80002 => null;

		public static TLocalizedText TBuffDesc80002 => null;

		public static TLocalizedText TBuffName80003 => null;

		public static TLocalizedText TBuffDesc80003 => null;

		public static TLocalizedText TBuffName80004 => null;

		public static TLocalizedText TBuffDesc80004 => null;

		public static TLocalizedText TBuffName80005 => null;

		public static TLocalizedText TBuffDesc80005 => null;

		public static TLocalizedText TBuffName80006 => null;

		public static TLocalizedText TBuffDesc80006 => null;

		public static TLocalizedText TBuffName80007 => null;

		public static TLocalizedText TBuffDesc80007 => null;

		public static TLocalizedText TBuffName80008 => null;

		public static TLocalizedText TBuffDesc80008 => null;

		public static TLocalizedText TBuffName80009 => null;

		public static TLocalizedText TBuffDesc80009 => null;

		public static TLocalizedText TBuffName80010 => null;

		public static TLocalizedText TBuffDesc80010 => null;

		public static TLocalizedText TBuffName80011 => null;

		public static TLocalizedText TBuffDesc80011 => null;

		public static TLocalizedText TBuffName80012 => null;

		public static TLocalizedText TBuffDesc80012 => null;

		public static TLocalizedText TBuffName80013 => null;

		public static TLocalizedText TBuffDesc80013 => null;

		public static TLocalizedText TBuffName80014 => null;

		public static TLocalizedText TBuffDesc80014 => null;

		public static TLocalizedText TBuffName80015 => null;

		public static TLocalizedText TBuffDesc80015 => null;

		public static TLocalizedText TBuffName80016 => null;

		public static TLocalizedText TBuffDesc80016 => null;

		public static TLocalizedText TBuffName80017 => null;

		public static TLocalizedText TBuffDesc80017 => null;

		public static TLocalizedText TBuffName80018 => null;

		public static TLocalizedText TBuffDesc80018 => null;

		public static TLocalizedText TBuffName80019 => null;

		public static TLocalizedText TBuffDesc80019 => null;

		public static TLocalizedText TBuffName1000001 => null;

		public static TLocalizedText TBuffDesc1000001 => null;

		public static TLocalizedText TBuffName1000002 => null;

		public static TLocalizedText TBuffDesc1000002 => null;

		public static TLocalizedText TBuffName1000003 => null;

		public static TLocalizedText TBuffDesc1000003 => null;

		public static TLocalizedText TBuffName1001001 => null;

		public static TLocalizedText TBuffDesc1001001 => null;

		public static TLocalizedText TBuffName1001002 => null;

		public static TLocalizedText TBuffDesc1001002 => null;

		public static TLocalizedText TBuffName1001003 => null;

		public static TLocalizedText TBuffDesc1001003 => null;

		public static TLocalizedText TBuffName1001004 => null;

		public static TLocalizedText TBuffDesc1001004 => null;

		public static TLocalizedText TBuffName1001005 => null;

		public static TLocalizedText TBuffDesc1001005 => null;

		public static TLocalizedText TBuffName1001006 => null;

		public static TLocalizedText TBuffDesc1001006 => null;

		public static TLocalizedText TBuffName1001007 => null;

		public static TLocalizedText TBuffDesc1001007 => null;

		public static TLocalizedText TBuffName1001008 => null;

		public static TLocalizedText TBuffDesc1001008 => null;

		public static TLocalizedText TBuffName1001009 => null;

		public static TLocalizedText TBuffDesc1001009 => null;

		public static TLocalizedText TBuffName1001010 => null;

		public static TLocalizedText TBuffDesc1001010 => null;

		public static TLocalizedText TBuffName1001011 => null;

		public static TLocalizedText TBuffDesc1001011 => null;

		public static TLocalizedText TBuffName1001012 => null;

		public static TLocalizedText TBuffDesc1001012 => null;

		public static TLocalizedText TBuffName2000001 => null;

		public static TLocalizedText TBuffDesc2000001 => null;

		public static TLocalizedText TBuffName2001002 => null;

		public static TLocalizedText TBuffDesc2001002 => null;

		public static TLocalizedText TBuffName2002001 => null;

		public static TLocalizedText TBuffDesc2002001 => null;

		public static TLocalizedText TBuffName2003001 => null;

		public static TLocalizedText TBuffDesc2003001 => null;

		public static TLocalizedText TBuffName2004001 => null;

		public static TLocalizedText TBuffDesc2004001 => null;

		public static TLocalizedText TBuffName2004002 => null;

		public static TLocalizedText TBuffDesc2004002 => null;

		public static TLocalizedText TBuffName2004003 => null;

		public static TLocalizedText TBuffDesc2004003 => null;

		public static TLocalizedText TBuffName3000000 => null;

		public static TLocalizedText TBuffDesc3000000 => null;

		public static TLocalizedText TBuffName3000001 => null;

		public static TLocalizedText TBuffDesc3000001 => null;

		public static TLocalizedText TBuffName3000002 => null;

		public static TLocalizedText TBuffDesc3000002 => null;

		public static TLocalizedText TBuffName3000003 => null;

		public static TLocalizedText TBuffDesc3000003 => null;

		public static TLocalizedText TBuffName3000004 => null;

		public static TLocalizedText TBuffDesc3000004 => null;

		public static TLocalizedText TBuffName3000005 => null;

		public static TLocalizedText TBuffDesc3000005 => null;

		public static TLocalizedText TBuffName3000006 => null;

		public static TLocalizedText TBuffDesc3000006 => null;

		public static TLocalizedText TBuffName3000007 => null;

		public static TLocalizedText TBuffDesc3000007 => null;

		public static TLocalizedText TBuffName3000009 => null;

		public static TLocalizedText TBuffDesc3000009 => null;

		public static TLocalizedText TBuffName3000010 => null;

		public static TLocalizedText TBuffDesc3000010 => null;

		public static TLocalizedText TBuffName3000012 => null;

		public static TLocalizedText TBuffDesc3000012 => null;

		public static TLocalizedText TBuffName3000013 => null;

		public static TLocalizedText TBuffDesc3000013 => null;

		public static TLocalizedText TBuffName3000014 => null;

		public static TLocalizedText TBuffDesc3000014 => null;

		public static TLocalizedText TBuffName3000015 => null;

		public static TLocalizedText TBuffDesc3000015 => null;

		public static TLocalizedText TBuffName3100001 => null;

		public static TLocalizedText TBuffDesc3100001 => null;

		public static TLocalizedText TBuffName3100002 => null;

		public static TLocalizedText TBuffDesc3100002 => null;

		public static TLocalizedText TBuffName8000000 => null;

		public static TLocalizedText TBuffDesc8000000 => null;

		public static TLocalizedText TBuffName9000000 => null;

		public static TLocalizedText TBuffDesc9000000 => null;

		public static TLocalizedText TBuffName62100001 => null;

		public static TLocalizedText TBuffDesc62100001 => null;

		public static TLocalizedText TBuffName62100002 => null;

		public static TLocalizedText TBuffDesc62100002 => null;

		public static TLocalizedText TBuffName62100003 => null;

		public static TLocalizedText TBuffDesc62100003 => null;

		public static TLocalizedText TBuffName62100004 => null;

		public static TLocalizedText TBuffDesc62100004 => null;

		public static TLocalizedText TBuffName62100005 => null;

		public static TLocalizedText TBuffDesc62100005 => null;

		public static TLocalizedText TBuffName62100006 => null;

		public static TLocalizedText TBuffDesc62100006 => null;

		public static TLocalizedText TBuffName62100007 => null;

		public static TLocalizedText TBuffDesc62100007 => null;

		public static TLocalizedText TBuffName62100008 => null;

		public static TLocalizedText TBuffDesc62100008 => null;

		public static TLocalizedText TBuffName62100009 => null;

		public static TLocalizedText TBuffDesc62100009 => null;

		public static TLocalizedText TBuffName62100010 => null;

		public static TLocalizedText TBuffDesc62100010 => null;

		public static TLocalizedText TBuffName62100011 => null;

		public static TLocalizedText TBuffDesc62100011 => null;

		public static TLocalizedText TBuffName62100012 => null;

		public static TLocalizedText TBuffDesc62100012 => null;

		public static TLocalizedText TBuffName62100013 => null;

		public static TLocalizedText TBuffDesc62100013 => null;

		public static TLocalizedText TBuffName62100014 => null;

		public static TLocalizedText TBuffDesc62100014 => null;

		public static TLocalizedText TBuffName65001001 => null;

		public static TLocalizedText TBuffDesc65001001 => null;

		public static TLocalizedText TBuffName65001002 => null;

		public static TLocalizedText TBuffDesc65001002 => null;

		public static TLocalizedText TBuffName65001003 => null;

		public static TLocalizedText TBuffDesc65001003 => null;

		public static TLocalizedText TBuffName65001004 => null;

		public static TLocalizedText TBuffDesc65001004 => null;

		public static TLocalizedText TBuffName65001005 => null;

		public static TLocalizedText TBuffDesc65001005 => null;

		public static TLocalizedText TBuffName65001006 => null;

		public static TLocalizedText TBuffDesc65001006 => null;

		public static TLocalizedText TBuffName65001007 => null;

		public static TLocalizedText TBuffDesc65001007 => null;

		public static TLocalizedText TBuffName65001008 => null;

		public static TLocalizedText TBuffDesc65001008 => null;

		public static TLocalizedText TBuffName65001009 => null;

		public static TLocalizedText TBuffDesc65001009 => null;

		public static TLocalizedText TBuffName65001010 => null;

		public static TLocalizedText TBuffDesc65001010 => null;

		public static TLocalizedText TBuffName65001011 => null;

		public static TLocalizedText TBuffDesc65001011 => null;

		public static TLocalizedText TBuffName65001012 => null;

		public static TLocalizedText TBuffDesc65001012 => null;

		public static TLocalizedText TBuffName65001013 => null;

		public static TLocalizedText TBuffDesc65001013 => null;

		public static TLocalizedText TBuffName65001014 => null;

		public static TLocalizedText TBuffDesc65001014 => null;

		public static TLocalizedText TBuffName65001015 => null;

		public static TLocalizedText TBuffDesc65001015 => null;

		public static TLocalizedText TBuffName65001016 => null;

		public static TLocalizedText TBuffDesc65001016 => null;

		public static TLocalizedText TBuffName65001017 => null;

		public static TLocalizedText TBuffDesc65001017 => null;

		public static TLocalizedText TBuffName65001018 => null;

		public static TLocalizedText TBuffDesc65001018 => null;

		public static TLocalizedText TBuffName65001019 => null;

		public static TLocalizedText TBuffDesc65001019 => null;

		public static TLocalizedText TBuffName65001020 => null;

		public static TLocalizedText TBuffDesc65001020 => null;

		public static TLocalizedText TBuffName65001021 => null;

		public static TLocalizedText TBuffDesc65001021 => null;

		public static TLocalizedText TBuffName65001022 => null;

		public static TLocalizedText TBuffDesc65001022 => null;

		public static TLocalizedText TBuffName65001023 => null;

		public static TLocalizedText TBuffDesc65001023 => null;

		public static TLocalizedText TBuffName65001024 => null;

		public static TLocalizedText TBuffDesc65001024 => null;

		public static TLocalizedText TBuffName65001025 => null;

		public static TLocalizedText TBuffDesc65001025 => null;

		public static TLocalizedText TBuffName65001026 => null;

		public static TLocalizedText TBuffDesc65001026 => null;

		public static TLocalizedText TBuffName65001027 => null;

		public static TLocalizedText TBuffDesc65001027 => null;

		public static TLocalizedText TBuffName65001028 => null;

		public static TLocalizedText TBuffDesc65001028 => null;

		public static TLocalizedText TBuffName65001029 => null;

		public static TLocalizedText TBuffDesc65001029 => null;

		public static TLocalizedText TBuffName65001030 => null;

		public static TLocalizedText TBuffDesc65001030 => null;

		public static TLocalizedText TBuffName65001031 => null;

		public static TLocalizedText TBuffDesc65001031 => null;

		public static TLocalizedText TBuffName65001032 => null;

		public static TLocalizedText TBuffDesc65001032 => null;

		public static TLocalizedText TBuffName65001033 => null;

		public static TLocalizedText TBuffDesc65001033 => null;

		public static TLocalizedText TBuffName65001034 => null;

		public static TLocalizedText TBuffDesc65001034 => null;

		public static TLocalizedText TBuffName65001035 => null;

		public static TLocalizedText TBuffDesc65001035 => null;

		public static TLocalizedText TBuffName65001036 => null;

		public static TLocalizedText TBuffDesc65001036 => null;

		public static TLocalizedText TBuffName65001037 => null;

		public static TLocalizedText TBuffDesc65001037 => null;

		public static TLocalizedText TBuffName65001038 => null;

		public static TLocalizedText TBuffDesc65001038 => null;

		public static TLocalizedText TBuffName65001039 => null;

		public static TLocalizedText TBuffDesc65001039 => null;

		public static TLocalizedText TBuffName65001040 => null;

		public static TLocalizedText TBuffDesc65001040 => null;

		public static TLocalizedText TBuffName65001041 => null;

		public static TLocalizedText TBuffDesc65001041 => null;

		public static TLocalizedText TBuffName65001042 => null;

		public static TLocalizedText TBuffDesc65001042 => null;

		public static TLocalizedText TBuffName65001043 => null;

		public static TLocalizedText TBuffDesc65001043 => null;

		public static TLocalizedText TBuffName65001044 => null;

		public static TLocalizedText TBuffDesc65001044 => null;

		public static TLocalizedText TBuffName65001045 => null;

		public static TLocalizedText TBuffDesc65001045 => null;

		public static TLocalizedText TBuffName65001046 => null;

		public static TLocalizedText TBuffDesc65001046 => null;

		public static TLocalizedText TBuffName65001047 => null;

		public static TLocalizedText TBuffDesc65001047 => null;

		public static TLocalizedText TBuffName65001048 => null;

		public static TLocalizedText TBuffDesc65001048 => null;

		public static TLocalizedText TBuffName65001049 => null;

		public static TLocalizedText TBuffDesc65001049 => null;

		public static TLocalizedText TBuffName65001050 => null;

		public static TLocalizedText TBuffDesc65001050 => null;

		public static TLocalizedText TBuffName65001051 => null;

		public static TLocalizedText TBuffDesc65001051 => null;

		public static TLocalizedText TBuffName65001052 => null;

		public static TLocalizedText TBuffDesc65001052 => null;

		public static TLocalizedText TBuffName65002001 => null;

		public static TLocalizedText TBuffDesc65002001 => null;

		public static TLocalizedText TBuffName65002002 => null;

		public static TLocalizedText TBuffDesc65002002 => null;

		public static TLocalizedText TBuffName65002003 => null;

		public static TLocalizedText TBuffDesc65002003 => null;

		public static TLocalizedText TBuffName65002004 => null;

		public static TLocalizedText TBuffDesc65002004 => null;

		public static TLocalizedText TBuffName65002005 => null;

		public static TLocalizedText TBuffDesc65002005 => null;

		public static TLocalizedText TBuffName65002006 => null;

		public static TLocalizedText TBuffDesc65002006 => null;

		public static TLocalizedText TBuffName65002007 => null;

		public static TLocalizedText TBuffDesc65002007 => null;

		public static TLocalizedText TBuffName65002008 => null;

		public static TLocalizedText TBuffDesc65002008 => null;

		public static TLocalizedText TBuffName65002009 => null;

		public static TLocalizedText TBuffDesc65002009 => null;

		public static TLocalizedText TBuffName65002010 => null;

		public static TLocalizedText TBuffDesc65002010 => null;

		public static TLocalizedText TBuffName65002011 => null;

		public static TLocalizedText TBuffDesc65002011 => null;

		public static TLocalizedText TBuffName65002012 => null;

		public static TLocalizedText TBuffDesc65002012 => null;

		public static TLocalizedText TBuffName65002013 => null;

		public static TLocalizedText TBuffDesc65002013 => null;

		public static TLocalizedText TBuffName65002014 => null;

		public static TLocalizedText TBuffDesc65002014 => null;

		public static TLocalizedText TBuffName65002015 => null;

		public static TLocalizedText TBuffDesc65002015 => null;

		public static TLocalizedText TBuffName65002016 => null;

		public static TLocalizedText TBuffDesc65002016 => null;

		public static TLocalizedText TBuffName65002017 => null;

		public static TLocalizedText TBuffDesc65002017 => null;

		public static TLocalizedText TBuffName65002018 => null;

		public static TLocalizedText TBuffDesc65002018 => null;

		public static TLocalizedText TBuffName65002019 => null;

		public static TLocalizedText TBuffDesc65002019 => null;

		public static TLocalizedText TBuffName65002020 => null;

		public static TLocalizedText TBuffDesc65002020 => null;

		public static TLocalizedText TBuffName65002021 => null;

		public static TLocalizedText TBuffDesc65002021 => null;

		public static TLocalizedText TBuffName65002022 => null;

		public static TLocalizedText TBuffDesc65002022 => null;

		public static TLocalizedText TBuffName65002023 => null;

		public static TLocalizedText TBuffDesc65002023 => null;

		public static TLocalizedText TBuffName65002024 => null;

		public static TLocalizedText TBuffDesc65002024 => null;

		public static TLocalizedText TBuffName65002025 => null;

		public static TLocalizedText TBuffDesc65002025 => null;

		public static TLocalizedText TBuffName65002026 => null;

		public static TLocalizedText TBuffDesc65002026 => null;

		public static TLocalizedText TBuffName65002027 => null;

		public static TLocalizedText TBuffDesc65002027 => null;

		public static TLocalizedText TBuffName65002028 => null;

		public static TLocalizedText TBuffDesc65002028 => null;

		public static TLocalizedText TBuffName65002029 => null;

		public static TLocalizedText TBuffDesc65002029 => null;

		public static TLocalizedText TBuffName65002030 => null;

		public static TLocalizedText TBuffDesc65002030 => null;

		public static TLocalizedText TBuffName65003001 => null;

		public static TLocalizedText TBuffDesc65003001 => null;

		public static TLocalizedText TBuffName65003002 => null;

		public static TLocalizedText TBuffDesc65003002 => null;

		public static TLocalizedText TBuffName65003003 => null;

		public static TLocalizedText TBuffDesc65003003 => null;

		public static TLocalizedText TBuffName65003004 => null;

		public static TLocalizedText TBuffDesc65003004 => null;

		public static TLocalizedText TBuffName65003005 => null;

		public static TLocalizedText TBuffDesc65003005 => null;

		public static TLocalizedText TBuffName65003006 => null;

		public static TLocalizedText TBuffDesc65003006 => null;

		public static TLocalizedText TBuffName65003007 => null;

		public static TLocalizedText TBuffDesc65003007 => null;

		public static TLocalizedText TBuffName65003008 => null;

		public static TLocalizedText TBuffDesc65003008 => null;

		public static TLocalizedText TBuffName65003009 => null;

		public static TLocalizedText TBuffDesc65003009 => null;

		public static TLocalizedText TBuffName65003010 => null;

		public static TLocalizedText TBuffDesc65003010 => null;

		public static TLocalizedText TBuffName65003011 => null;

		public static TLocalizedText TBuffDesc65003011 => null;

		public static TLocalizedText TBuffName65003012 => null;

		public static TLocalizedText TBuffDesc65003012 => null;

		public static TLocalizedText TBuffName65003013 => null;

		public static TLocalizedText TBuffDesc65003013 => null;

		public static TLocalizedText TBuffName65003014 => null;

		public static TLocalizedText TBuffDesc65003014 => null;

		public static TLocalizedText TBuffName65003015 => null;

		public static TLocalizedText TBuffDesc65003015 => null;

		public static TLocalizedText TBuffName65003016 => null;

		public static TLocalizedText TBuffDesc65003016 => null;

		public static TLocalizedText TBuffName65003017 => null;

		public static TLocalizedText TBuffDesc65003017 => null;

		public static TLocalizedText TBuffName65003018 => null;

		public static TLocalizedText TBuffDesc65003018 => null;

		public static TLocalizedText TBuffName65003019 => null;

		public static TLocalizedText TBuffDesc65003019 => null;

		public static TLocalizedText TBuffName65003020 => null;

		public static TLocalizedText TBuffDesc65003020 => null;

		public static TLocalizedText TBuffName65003021 => null;

		public static TLocalizedText TBuffDesc65003021 => null;

		public static TLocalizedText TBuffName65003022 => null;

		public static TLocalizedText TBuffDesc65003022 => null;

		public static TLocalizedText TBuffName65003023 => null;

		public static TLocalizedText TBuffDesc65003023 => null;

		public static TLocalizedText TBuffName65003024 => null;

		public static TLocalizedText TBuffDesc65003024 => null;

		public static TLocalizedText TBuffName65003025 => null;

		public static TLocalizedText TBuffDesc65003025 => null;

		public static TLocalizedText TBuffName65003026 => null;

		public static TLocalizedText TBuffDesc65003026 => null;

		public static TLocalizedText TBuffName65003027 => null;

		public static TLocalizedText TBuffDesc65003027 => null;

		public static TLocalizedText TBuffName65003028 => null;

		public static TLocalizedText TBuffDesc65003028 => null;

		public static TLocalizedText TBuffName65003029 => null;

		public static TLocalizedText TBuffDesc65003029 => null;

		public static TLocalizedText TBuffName65003030 => null;

		public static TLocalizedText TBuffDesc65003030 => null;

		public static TLocalizedText TBuffName65003031 => null;

		public static TLocalizedText TBuffDesc65003031 => null;

		public static TLocalizedText TBuffName65004001 => null;

		public static TLocalizedText TBuffDesc65004001 => null;

		public static TLocalizedText TBuffName65004002 => null;

		public static TLocalizedText TBuffDesc65004002 => null;

		public static TLocalizedText TBuffName65004003 => null;

		public static TLocalizedText TBuffDesc65004003 => null;

		public static TLocalizedText TBuffName65004004 => null;

		public static TLocalizedText TBuffDesc65004004 => null;

		public static TLocalizedText TBuffName65004005 => null;

		public static TLocalizedText TBuffDesc65004005 => null;

		public static TLocalizedText TBuffName65004006 => null;

		public static TLocalizedText TBuffDesc65004006 => null;

		public static TLocalizedText TBuffName65004007 => null;

		public static TLocalizedText TBuffDesc65004007 => null;

		public static TLocalizedText TBuffName65004008 => null;

		public static TLocalizedText TBuffDesc65004008 => null;

		public static TLocalizedText TBuffName65004009 => null;

		public static TLocalizedText TBuffDesc65004009 => null;

		public static TLocalizedText TBuffName65004010 => null;

		public static TLocalizedText TBuffDesc65004010 => null;

		public static TLocalizedText TBuffName65004011 => null;

		public static TLocalizedText TBuffDesc65004011 => null;

		public static TLocalizedText TBuffName65004012 => null;

		public static TLocalizedText TBuffDesc65004012 => null;

		public static TLocalizedText TBuffName65004013 => null;

		public static TLocalizedText TBuffDesc65004013 => null;

		public static TLocalizedText TBuffName65004014 => null;

		public static TLocalizedText TBuffDesc65004014 => null;

		public static TLocalizedText TBuffName65004015 => null;

		public static TLocalizedText TBuffDesc65004015 => null;

		public static TLocalizedText TCampaignName101 => null;

		public static TLocalizedText TCampaignDesc101 => null;

		public static TLocalizedText TCampaignFailConditionDesc101 => null;

		public static TLocalizedText TCampaignSectorName101 => null;

		public static TLocalizedText TCampaignName102 => null;

		public static TLocalizedText TCampaignDesc102 => null;

		public static TLocalizedText TCampaignFailConditionDesc102 => null;

		public static TLocalizedText TCampaignSectorName102 => null;

		public static TLocalizedText TCampaignName103 => null;

		public static TLocalizedText TCampaignDesc103 => null;

		public static TLocalizedText TCampaignFailConditionDesc103 => null;

		public static TLocalizedText TCampaignSectorName103 => null;

		public static TLocalizedText TCampaignName104 => null;

		public static TLocalizedText TCampaignDesc104 => null;

		public static TLocalizedText TCampaignFailConditionDesc104 => null;

		public static TLocalizedText TCampaignSectorName104 => null;

		public static TLocalizedText TCampaignName105 => null;

		public static TLocalizedText TCampaignDesc105 => null;

		public static TLocalizedText TCampaignFailConditionDesc105 => null;

		public static TLocalizedText TCampaignSectorName105 => null;

		public static TLocalizedText TCampaignName106 => null;

		public static TLocalizedText TCampaignDesc106 => null;

		public static TLocalizedText TCampaignFailConditionDesc106 => null;

		public static TLocalizedText TCampaignSectorName106 => null;

		public static TLocalizedText TCampaignName107 => null;

		public static TLocalizedText TCampaignDesc107 => null;

		public static TLocalizedText TCampaignFailConditionDesc107 => null;

		public static TLocalizedText TCampaignSectorName107 => null;

		public static TLocalizedText TCampaignName108 => null;

		public static TLocalizedText TCampaignDesc108 => null;

		public static TLocalizedText TCampaignFailConditionDesc108 => null;

		public static TLocalizedText TCampaignSectorName108 => null;

		public static TLocalizedText TCampaignName111 => null;

		public static TLocalizedText TCampaignDesc111 => null;

		public static TLocalizedText TCampaignFailConditionDesc111 => null;

		public static TLocalizedText TCampaignSectorName111 => null;

		public static TLocalizedText TCampaignName112 => null;

		public static TLocalizedText TCampaignDesc112 => null;

		public static TLocalizedText TCampaignFailConditionDesc112 => null;

		public static TLocalizedText TCampaignSectorName112 => null;

		public static TLocalizedText TCampaignName113 => null;

		public static TLocalizedText TCampaignDesc113 => null;

		public static TLocalizedText TCampaignFailConditionDesc113 => null;

		public static TLocalizedText TCampaignSectorName113 => null;

		public static TLocalizedText TCampaignName114 => null;

		public static TLocalizedText TCampaignDesc114 => null;

		public static TLocalizedText TCampaignFailConditionDesc114 => null;

		public static TLocalizedText TCampaignSectorName114 => null;

		public static TLocalizedText TCampaignName115 => null;

		public static TLocalizedText TCampaignDesc115 => null;

		public static TLocalizedText TCampaignFailConditionDesc115 => null;

		public static TLocalizedText TCampaignSectorName115 => null;

		public static TLocalizedText TCampaignName116 => null;

		public static TLocalizedText TCampaignDesc116 => null;

		public static TLocalizedText TCampaignFailConditionDesc116 => null;

		public static TLocalizedText TCampaignSectorName116 => null;

		public static TLocalizedText TCampaignName117 => null;

		public static TLocalizedText TCampaignDesc117 => null;

		public static TLocalizedText TCampaignFailConditionDesc117 => null;

		public static TLocalizedText TCampaignSectorName117 => null;

		public static TLocalizedText TCampaignName118 => null;

		public static TLocalizedText TCampaignDesc118 => null;

		public static TLocalizedText TCampaignFailConditionDesc118 => null;

		public static TLocalizedText TCampaignSectorName118 => null;

		public static TLocalizedText TCampaignName201 => null;

		public static TLocalizedText TCampaignDesc201 => null;

		public static TLocalizedText TCampaignFailConditionDesc201 => null;

		public static TLocalizedText TCampaignSectorName201 => null;

		public static TLocalizedText TCampaignName202 => null;

		public static TLocalizedText TCampaignDesc202 => null;

		public static TLocalizedText TCampaignFailConditionDesc202 => null;

		public static TLocalizedText TCampaignSectorName202 => null;

		public static TLocalizedText TCampaignName203 => null;

		public static TLocalizedText TCampaignDesc203 => null;

		public static TLocalizedText TCampaignFailConditionDesc203 => null;

		public static TLocalizedText TCampaignSectorName203 => null;

		public static TLocalizedText TCampaignName204 => null;

		public static TLocalizedText TCampaignDesc204 => null;

		public static TLocalizedText TCampaignFailConditionDesc204 => null;

		public static TLocalizedText TCampaignSectorName204 => null;

		public static TLocalizedText TCampaignName205 => null;

		public static TLocalizedText TCampaignDesc205 => null;

		public static TLocalizedText TCampaignFailConditionDesc205 => null;

		public static TLocalizedText TCampaignSectorName205 => null;

		public static TLocalizedText TCampaignName206 => null;

		public static TLocalizedText TCampaignDesc206 => null;

		public static TLocalizedText TCampaignFailConditionDesc206 => null;

		public static TLocalizedText TCampaignSectorName206 => null;

		public static TLocalizedText TCampaignName207 => null;

		public static TLocalizedText TCampaignDesc207 => null;

		public static TLocalizedText TCampaignFailConditionDesc207 => null;

		public static TLocalizedText TCampaignSectorName207 => null;

		public static TLocalizedText TCampaignName208 => null;

		public static TLocalizedText TCampaignDesc208 => null;

		public static TLocalizedText TCampaignFailConditionDesc208 => null;

		public static TLocalizedText TCampaignSectorName208 => null;

		public static TLocalizedText TCampaignName209 => null;

		public static TLocalizedText TCampaignDesc209 => null;

		public static TLocalizedText TCampaignFailConditionDesc209 => null;

		public static TLocalizedText TCampaignSectorName209 => null;

		public static TLocalizedText TCampaignName210 => null;

		public static TLocalizedText TCampaignDesc210 => null;

		public static TLocalizedText TCampaignFailConditionDesc210 => null;

		public static TLocalizedText TCampaignSectorName210 => null;

		public static TLocalizedText TCampaignName211 => null;

		public static TLocalizedText TCampaignDesc211 => null;

		public static TLocalizedText TCampaignFailConditionDesc211 => null;

		public static TLocalizedText TCampaignSectorName211 => null;

		public static TLocalizedText TCampaignName301 => null;

		public static TLocalizedText TCampaignDesc301 => null;

		public static TLocalizedText TCampaignFailConditionDesc301 => null;

		public static TLocalizedText TCampaignSectorName301 => null;

		public static TLocalizedText TCampaignName302 => null;

		public static TLocalizedText TCampaignDesc302 => null;

		public static TLocalizedText TCampaignFailConditionDesc302 => null;

		public static TLocalizedText TCampaignSectorName302 => null;

		public static TLocalizedText TCampaignName303 => null;

		public static TLocalizedText TCampaignDesc303 => null;

		public static TLocalizedText TCampaignFailConditionDesc303 => null;

		public static TLocalizedText TCampaignSectorName303 => null;

		public static TLocalizedText TCampaignName304 => null;

		public static TLocalizedText TCampaignDesc304 => null;

		public static TLocalizedText TCampaignFailConditionDesc304 => null;

		public static TLocalizedText TCampaignSectorName304 => null;

		public static TLocalizedText TCampaignName305 => null;

		public static TLocalizedText TCampaignDesc305 => null;

		public static TLocalizedText TCampaignFailConditionDesc305 => null;

		public static TLocalizedText TCampaignSectorName305 => null;

		public static TLocalizedText TCampaignName306 => null;

		public static TLocalizedText TCampaignDesc306 => null;

		public static TLocalizedText TCampaignFailConditionDesc306 => null;

		public static TLocalizedText TCampaignSectorName306 => null;

		public static TLocalizedText TCampaignName307 => null;

		public static TLocalizedText TCampaignDesc307 => null;

		public static TLocalizedText TCampaignFailConditionDesc307 => null;

		public static TLocalizedText TCampaignSectorName307 => null;

		public static TLocalizedText TCampaignName308 => null;

		public static TLocalizedText TCampaignDesc308 => null;

		public static TLocalizedText TCampaignFailConditionDesc308 => null;

		public static TLocalizedText TCampaignSectorName308 => null;

		public static TLocalizedText TCampaignName309 => null;

		public static TLocalizedText TCampaignDesc309 => null;

		public static TLocalizedText TCampaignFailConditionDesc309 => null;

		public static TLocalizedText TCampaignSectorName309 => null;

		public static TLocalizedText TCampaignName310 => null;

		public static TLocalizedText TCampaignDesc310 => null;

		public static TLocalizedText TCampaignFailConditionDesc310 => null;

		public static TLocalizedText TCampaignSectorName310 => null;

		public static TLocalizedText TCampaignName999 => null;

		public static TLocalizedText TCampaignDesc999 => null;

		public static TLocalizedText TCampaignFailConditionDesc999 => null;

		public static TLocalizedText TCampaignSectorName999 => null;

		public static TLocalizedText TCampaignName1100 => null;

		public static TLocalizedText TCampaignDesc1100 => null;

		public static TLocalizedText TCampaignFailConditionDesc1100 => null;

		public static TLocalizedText TCampaignSectorName1100 => null;

		public static TLocalizedText TCampaignName1101 => null;

		public static TLocalizedText TCampaignDesc1101 => null;

		public static TLocalizedText TCampaignFailConditionDesc1101 => null;

		public static TLocalizedText TCampaignSectorName1101 => null;

		public static TLocalizedText TCampaignName1102 => null;

		public static TLocalizedText TCampaignDesc1102 => null;

		public static TLocalizedText TCampaignFailConditionDesc1102 => null;

		public static TLocalizedText TCampaignSectorName1102 => null;

		public static TLocalizedText TCampaignName1103 => null;

		public static TLocalizedText TCampaignDesc1103 => null;

		public static TLocalizedText TCampaignFailConditionDesc1103 => null;

		public static TLocalizedText TCampaignSectorName1103 => null;

		public static TLocalizedText TCampaignName1104 => null;

		public static TLocalizedText TCampaignDesc1104 => null;

		public static TLocalizedText TCampaignFailConditionDesc1104 => null;

		public static TLocalizedText TCampaignSectorName1104 => null;

		public static TLocalizedText TCampaignName1105 => null;

		public static TLocalizedText TCampaignDesc1105 => null;

		public static TLocalizedText TCampaignFailConditionDesc1105 => null;

		public static TLocalizedText TCampaignSectorName1105 => null;

		public static TLocalizedText TCampaignName1201 => null;

		public static TLocalizedText TCampaignDesc1201 => null;

		public static TLocalizedText TCampaignFailConditionDesc1201 => null;

		public static TLocalizedText TCampaignSectorName1201 => null;

		public static TLocalizedText TCampaignName1202 => null;

		public static TLocalizedText TCampaignDesc1202 => null;

		public static TLocalizedText TCampaignFailConditionDesc1202 => null;

		public static TLocalizedText TCampaignSectorName1202 => null;

		public static TLocalizedText TCampaignName1203 => null;

		public static TLocalizedText TCampaignDesc1203 => null;

		public static TLocalizedText TCampaignFailConditionDesc1203 => null;

		public static TLocalizedText TCampaignSectorName1203 => null;

		public static TLocalizedText TCampaignName1204 => null;

		public static TLocalizedText TCampaignDesc1204 => null;

		public static TLocalizedText TCampaignFailConditionDesc1204 => null;

		public static TLocalizedText TCampaignSectorName1204 => null;

		public static TLocalizedText TCampaignQuestName10101 => null;

		public static TLocalizedText TCampaignQuestDesc10101 => null;

		public static TLocalizedText TCampaignQuestName10201 => null;

		public static TLocalizedText TCampaignQuestDesc10201 => null;

		public static TLocalizedText TCampaignQuestName10301 => null;

		public static TLocalizedText TCampaignQuestDesc10301 => null;

		public static TLocalizedText TCampaignQuestName10401 => null;

		public static TLocalizedText TCampaignQuestDesc10401 => null;

		public static TLocalizedText TCampaignQuestName10501 => null;

		public static TLocalizedText TCampaignQuestDesc10501 => null;

		public static TLocalizedText TCampaignQuestName10601 => null;

		public static TLocalizedText TCampaignQuestDesc10601 => null;

		public static TLocalizedText TCampaignQuestName10701 => null;

		public static TLocalizedText TCampaignQuestDesc10701 => null;

		public static TLocalizedText TCampaignQuestName10801 => null;

		public static TLocalizedText TCampaignQuestDesc10801 => null;

		public static TLocalizedText TCampaignQuestName10901 => null;

		public static TLocalizedText TCampaignQuestDesc10901 => null;

		public static TLocalizedText TCampaignQuestName11001 => null;

		public static TLocalizedText TCampaignQuestDesc11001 => null;

		public static TLocalizedText TCampaignQuestName11101 => null;

		public static TLocalizedText TCampaignQuestDesc11101 => null;

		public static TLocalizedText TCampaignQuestName11201 => null;

		public static TLocalizedText TCampaignQuestDesc11201 => null;

		public static TLocalizedText TCampaignQuestName11301 => null;

		public static TLocalizedText TCampaignQuestDesc11301 => null;

		public static TLocalizedText TCampaignQuestName20101 => null;

		public static TLocalizedText TCampaignQuestDesc20101 => null;

		public static TLocalizedText TCampaignQuestName20201 => null;

		public static TLocalizedText TCampaignQuestDesc20201 => null;

		public static TLocalizedText TCampaignQuestName20301 => null;

		public static TLocalizedText TCampaignQuestDesc20301 => null;

		public static TLocalizedText TCampaignQuestName20401 => null;

		public static TLocalizedText TCampaignQuestDesc20401 => null;

		public static TLocalizedText TCampaignQuestName20501 => null;

		public static TLocalizedText TCampaignQuestDesc20501 => null;

		public static TLocalizedText TCampaignQuestName20601 => null;

		public static TLocalizedText TCampaignQuestDesc20601 => null;

		public static TLocalizedText TCampaignQuestName20701 => null;

		public static TLocalizedText TCampaignQuestDesc20701 => null;

		public static TLocalizedText TCampaignQuestName20801 => null;

		public static TLocalizedText TCampaignQuestDesc20801 => null;

		public static TLocalizedText TCampaignQuestName20901 => null;

		public static TLocalizedText TCampaignQuestDesc20901 => null;

		public static TLocalizedText TCampaignQuestName21001 => null;

		public static TLocalizedText TCampaignQuestDesc21001 => null;

		public static TLocalizedText TCampaignQuestName21101 => null;

		public static TLocalizedText TCampaignQuestDesc21101 => null;

		public static TLocalizedText TCampaignQuestName21201 => null;

		public static TLocalizedText TCampaignQuestDesc21201 => null;

		public static TLocalizedText TCampaignQuestName110001 => null;

		public static TLocalizedText TCampaignQuestDesc110001 => null;

		public static TLocalizedText TCampaignQuestName110101 => null;

		public static TLocalizedText TCampaignQuestDesc110101 => null;

		public static TLocalizedText TCampaignQuestName110201 => null;

		public static TLocalizedText TCampaignQuestDesc110201 => null;

		public static TLocalizedText TCampaignQuestName110301 => null;

		public static TLocalizedText TCampaignQuestDesc110301 => null;

		public static TLocalizedText TCampaignQuestName110401 => null;

		public static TLocalizedText TCampaignQuestDesc110401 => null;

		public static TLocalizedText TCampaignQuestName110501 => null;

		public static TLocalizedText TCampaignQuestDesc110501 => null;

		public static TLocalizedText TCampaignTaskDesc1011 => null;

		public static TLocalizedText TCampaignTaskDesc1021 => null;

		public static TLocalizedText TCampaignTaskDesc1022 => null;

		public static TLocalizedText TCampaignTaskDesc1031 => null;

		public static TLocalizedText TCampaignTaskDesc1032 => null;

		public static TLocalizedText TCampaignTaskDesc1041 => null;

		public static TLocalizedText TCampaignTaskDesc1051 => null;

		public static TLocalizedText TCampaignTaskDesc1052 => null;

		public static TLocalizedText TCampaignTaskDesc1061 => null;

		public static TLocalizedText TCampaignTaskDesc1062 => null;

		public static TLocalizedText TCampaignTaskDesc1071 => null;

		public static TLocalizedText TCampaignTaskDesc1072 => null;

		public static TLocalizedText TCampaignTaskDesc1081 => null;

		public static TLocalizedText TCampaignTaskDesc1082 => null;

		public static TLocalizedText TCampaignTaskDesc1111 => null;

		public static TLocalizedText TCampaignTaskDesc1112 => null;

		public static TLocalizedText TCampaignTaskDesc1121 => null;

		public static TLocalizedText TCampaignTaskDesc1131 => null;

		public static TLocalizedText TCampaignTaskDesc1132 => null;

		public static TLocalizedText TCampaignTaskDesc1141 => null;

		public static TLocalizedText TCampaignTaskDesc1142 => null;

		public static TLocalizedText TCampaignTaskDesc1151 => null;

		public static TLocalizedText TCampaignTaskDesc1152 => null;

		public static TLocalizedText TCampaignTaskDesc1161 => null;

		public static TLocalizedText TCampaignTaskDesc1162 => null;

		public static TLocalizedText TCampaignTaskDesc1171 => null;

		public static TLocalizedText TCampaignTaskDesc1181 => null;

		public static TLocalizedText TCampaignTaskDesc1182 => null;

		public static TLocalizedText TCampaignTaskDesc2011 => null;

		public static TLocalizedText TCampaignTaskDesc2021 => null;

		public static TLocalizedText TCampaignTaskDesc2022 => null;

		public static TLocalizedText TCampaignTaskDesc2031 => null;

		public static TLocalizedText TCampaignTaskDesc2032 => null;

		public static TLocalizedText TCampaignTaskDesc2041 => null;

		public static TLocalizedText TCampaignTaskDesc2042 => null;

		public static TLocalizedText TCampaignTaskDesc2051 => null;

		public static TLocalizedText TCampaignTaskDesc2052 => null;

		public static TLocalizedText TCampaignTaskDesc2061 => null;

		public static TLocalizedText TCampaignTaskDesc2062 => null;

		public static TLocalizedText TCampaignTaskDesc2071 => null;

		public static TLocalizedText TCampaignTaskDesc2072 => null;

		public static TLocalizedText TCampaignTaskDesc2081 => null;

		public static TLocalizedText TCampaignTaskDesc2091 => null;

		public static TLocalizedText TCampaignTaskDesc2092 => null;

		public static TLocalizedText TCampaignTaskDesc2101 => null;

		public static TLocalizedText TCampaignTaskDesc2111 => null;

		public static TLocalizedText TCampaignTaskDesc2112 => null;

		public static TLocalizedText TCampaignTaskDesc3011 => null;

		public static TLocalizedText TCampaignTaskDesc3021 => null;

		public static TLocalizedText TCampaignTaskDesc3022 => null;

		public static TLocalizedText TCampaignTaskDesc3031 => null;

		public static TLocalizedText TCampaignTaskDesc3032 => null;

		public static TLocalizedText TCampaignTaskDesc3041 => null;

		public static TLocalizedText TCampaignTaskDesc3051 => null;

		public static TLocalizedText TCampaignTaskDesc3052 => null;

		public static TLocalizedText TCampaignTaskDesc3061 => null;

		public static TLocalizedText TCampaignTaskDesc3062 => null;

		public static TLocalizedText TCampaignTaskDesc3071 => null;

		public static TLocalizedText TCampaignTaskDesc3072 => null;

		public static TLocalizedText TCampaignTaskDesc3081 => null;

		public static TLocalizedText TCampaignTaskDesc3082 => null;

		public static TLocalizedText TCampaignTaskDesc3091 => null;

		public static TLocalizedText TCampaignTaskDesc3101 => null;

		public static TLocalizedText TCampaignTaskDesc3102 => null;

		public static TLocalizedText TCampaignTaskDesc9991 => null;

		public static TLocalizedText TCampaignTaskDesc11001 => null;

		public static TLocalizedText TCampaignTaskDesc11011 => null;

		public static TLocalizedText TCampaignTaskDesc11012 => null;

		public static TLocalizedText TCampaignTaskDesc11021 => null;

		public static TLocalizedText TCampaignTaskDesc11031 => null;

		public static TLocalizedText TCampaignTaskDesc11041 => null;

		public static TLocalizedText TCampaignTaskDesc11051 => null;

		public static TLocalizedText TCampaignTaskDesc12011 => null;

		public static TLocalizedText TCampaignTaskDesc12012 => null;

		public static TLocalizedText TCampaignTaskDesc12013 => null;

		public static TLocalizedText TCampaignTaskDesc12021 => null;

		public static TLocalizedText TCampaignTaskDesc12031 => null;

		public static TLocalizedText TCampaignTaskDesc12041 => null;

		public static TLocalizedText TCharacterName1001 => null;

		public static TLocalizedText TCharacterCombatText1001 => null;

		public static TLocalizedText TCharacterName2001 => null;

		public static TLocalizedText TCharacterName3001 => null;

		public static TLocalizedText TCharacterName4001 => null;

		public static TLocalizedText TCharacterName5001 => null;

		public static TLocalizedText TCharacterName100001 => null;

		public static TLocalizedText TCharacterDesc100001 => null;

		public static TLocalizedText TCharacterTitle100001 => null;

		public static TLocalizedText TCharacterName100002 => null;

		public static TLocalizedText TCharacterDesc100002 => null;

		public static TLocalizedText TCharacterTitle100002 => null;

		public static TLocalizedText TCharacterName100003 => null;

		public static TLocalizedText TCharacterDesc100003 => null;

		public static TLocalizedText TCharacterTitle100003 => null;

		public static TLocalizedText TCharacterName100004 => null;

		public static TLocalizedText TCharacterDesc100004 => null;

		public static TLocalizedText TCharacterTitle100004 => null;

		public static TLocalizedText TCharacterName100005 => null;

		public static TLocalizedText TCharacterDesc100005 => null;

		public static TLocalizedText TCharacterTitle100005 => null;

		public static TLocalizedText TCharacterSkinName1001 => null;

		public static TLocalizedText TCharacterSkinName1002 => null;

		public static TLocalizedText TCharacterSkinName1003 => null;

		public static TLocalizedText TCharacterSkinName2001 => null;

		public static TLocalizedText TCharacterSkinName2002 => null;

		public static TLocalizedText TCharacterSkinName3001 => null;

		public static TLocalizedText TCharacterSkinName3002 => null;

		public static TLocalizedText TCharacterSkinName3003 => null;

		public static TLocalizedText TCharacterSkinName4001 => null;

		public static TLocalizedText TCharacterSkinName4002 => null;

		public static TLocalizedText TCharacterSkinName4003 => null;

		public static TLocalizedText TCharacterSkinName5001 => null;

		public static TLocalizedText TCharacterTalentName1001011 => null;

		public static TLocalizedText TCharacterTalentDesc1001011 => null;

		public static TLocalizedText TCharacterTalentName1001012 => null;

		public static TLocalizedText TCharacterTalentDesc1001012 => null;

		public static TLocalizedText TCharacterTalentName1001013 => null;

		public static TLocalizedText TCharacterTalentDesc1001013 => null;

		public static TLocalizedText TCharacterTalentName1001014 => null;

		public static TLocalizedText TCharacterTalentDesc1001014 => null;

		public static TLocalizedText TCharacterTalentName1001021 => null;

		public static TLocalizedText TCharacterTalentDesc1001021 => null;

		public static TLocalizedText TCharacterTalentName1001022 => null;

		public static TLocalizedText TCharacterTalentDesc1001022 => null;

		public static TLocalizedText TCharacterTalentName1001023 => null;

		public static TLocalizedText TCharacterTalentDesc1001023 => null;

		public static TLocalizedText TCharacterTalentName1001024 => null;

		public static TLocalizedText TCharacterTalentDesc1001024 => null;

		public static TLocalizedText TCharacterTalentName1001031 => null;

		public static TLocalizedText TCharacterTalentDesc1001031 => null;

		public static TLocalizedText TCharacterTalentName1001032 => null;

		public static TLocalizedText TCharacterTalentDesc1001032 => null;

		public static TLocalizedText TCharacterTalentName1001033 => null;

		public static TLocalizedText TCharacterTalentDesc1001033 => null;

		public static TLocalizedText TCharacterTalentName1001034 => null;

		public static TLocalizedText TCharacterTalentDesc1001034 => null;

		public static TLocalizedText TCharacterTalentName1001041 => null;

		public static TLocalizedText TCharacterTalentDesc1001041 => null;

		public static TLocalizedText TCharacterTalentName1001042 => null;

		public static TLocalizedText TCharacterTalentDesc1001042 => null;

		public static TLocalizedText TCharacterTalentName1001043 => null;

		public static TLocalizedText TCharacterTalentDesc1001043 => null;

		public static TLocalizedText TCharacterTalentName1001051 => null;

		public static TLocalizedText TCharacterTalentDesc1001051 => null;

		public static TLocalizedText TCharacterTalentName1001052 => null;

		public static TLocalizedText TCharacterTalentDesc1001052 => null;

		public static TLocalizedText TCharacterTalentName1001061 => null;

		public static TLocalizedText TCharacterTalentDesc1001061 => null;

		public static TLocalizedText TCharacterTalentName1001062 => null;

		public static TLocalizedText TCharacterTalentDesc1001062 => null;

		public static TLocalizedText TCharacterTalentName1001063 => null;

		public static TLocalizedText TCharacterTalentDesc1001063 => null;

		public static TLocalizedText TCharacterTalentName1003011 => null;

		public static TLocalizedText TCharacterTalentDesc1003011 => null;

		public static TLocalizedText TCharacterTalentName1004011 => null;

		public static TLocalizedText TCharacterTalentDesc1004011 => null;

		public static TLocalizedText TCharacterTalentName2001011 => null;

		public static TLocalizedText TCharacterTalentDesc2001011 => null;

		public static TLocalizedText TCharacterTalentName2001012 => null;

		public static TLocalizedText TCharacterTalentDesc2001012 => null;

		public static TLocalizedText TCharacterTalentName2001013 => null;

		public static TLocalizedText TCharacterTalentDesc2001013 => null;

		public static TLocalizedText TCharacterTalentName2001014 => null;

		public static TLocalizedText TCharacterTalentDesc2001014 => null;

		public static TLocalizedText TCharacterTalentName2001021 => null;

		public static TLocalizedText TCharacterTalentDesc2001021 => null;

		public static TLocalizedText TCharacterTalentName2001022 => null;

		public static TLocalizedText TCharacterTalentDesc2001022 => null;

		public static TLocalizedText TCharacterTalentName2001023 => null;

		public static TLocalizedText TCharacterTalentDesc2001023 => null;

		public static TLocalizedText TCharacterTalentName2001024 => null;

		public static TLocalizedText TCharacterTalentDesc2001024 => null;

		public static TLocalizedText TCharacterTalentName2001031 => null;

		public static TLocalizedText TCharacterTalentDesc2001031 => null;

		public static TLocalizedText TCharacterTalentName2001032 => null;

		public static TLocalizedText TCharacterTalentDesc2001032 => null;

		public static TLocalizedText TCharacterTalentName2001033 => null;

		public static TLocalizedText TCharacterTalentDesc2001033 => null;

		public static TLocalizedText TCharacterTalentName2001034 => null;

		public static TLocalizedText TCharacterTalentDesc2001034 => null;

		public static TLocalizedText TCharacterTalentName2001041 => null;

		public static TLocalizedText TCharacterTalentDesc2001041 => null;

		public static TLocalizedText TCharacterTalentName2001042 => null;

		public static TLocalizedText TCharacterTalentDesc2001042 => null;

		public static TLocalizedText TCharacterTalentName2001043 => null;

		public static TLocalizedText TCharacterTalentDesc2001043 => null;

		public static TLocalizedText TCharacterTalentName2001051 => null;

		public static TLocalizedText TCharacterTalentDesc2001051 => null;

		public static TLocalizedText TCharacterTalentName2001052 => null;

		public static TLocalizedText TCharacterTalentDesc2001052 => null;

		public static TLocalizedText TCharacterTalentName2001053 => null;

		public static TLocalizedText TCharacterTalentDesc2001053 => null;

		public static TLocalizedText TCharacterTalentName2001061 => null;

		public static TLocalizedText TCharacterTalentDesc2001061 => null;

		public static TLocalizedText TCharacterTalentName2001062 => null;

		public static TLocalizedText TCharacterTalentDesc2001062 => null;

		public static TLocalizedText TCharacterTalentName2001063 => null;

		public static TLocalizedText TCharacterTalentDesc2001063 => null;

		public static TLocalizedText TCharacterTalentName2002011 => null;

		public static TLocalizedText TCharacterTalentDesc2002011 => null;

		public static TLocalizedText TCharacterTalentName2002012 => null;

		public static TLocalizedText TCharacterTalentDesc2002012 => null;

		public static TLocalizedText TCharacterTalentName2002013 => null;

		public static TLocalizedText TCharacterTalentDesc2002013 => null;

		public static TLocalizedText TCharacterTalentName2002014 => null;

		public static TLocalizedText TCharacterTalentDesc2002014 => null;

		public static TLocalizedText TCharacterTalentName2002021 => null;

		public static TLocalizedText TCharacterTalentDesc2002021 => null;

		public static TLocalizedText TCharacterTalentName2002022 => null;

		public static TLocalizedText TCharacterTalentDesc2002022 => null;

		public static TLocalizedText TCharacterTalentName2002023 => null;

		public static TLocalizedText TCharacterTalentDesc2002023 => null;

		public static TLocalizedText TCharacterTalentName2002024 => null;

		public static TLocalizedText TCharacterTalentDesc2002024 => null;

		public static TLocalizedText TCharacterTalentName2002031 => null;

		public static TLocalizedText TCharacterTalentDesc2002031 => null;

		public static TLocalizedText TCharacterTalentName2002032 => null;

		public static TLocalizedText TCharacterTalentDesc2002032 => null;

		public static TLocalizedText TCharacterTalentName2002033 => null;

		public static TLocalizedText TCharacterTalentDesc2002033 => null;

		public static TLocalizedText TCharacterTalentName2002034 => null;

		public static TLocalizedText TCharacterTalentDesc2002034 => null;

		public static TLocalizedText TCharacterTalentName2002041 => null;

		public static TLocalizedText TCharacterTalentDesc2002041 => null;

		public static TLocalizedText TCharacterTalentName2002042 => null;

		public static TLocalizedText TCharacterTalentDesc2002042 => null;

		public static TLocalizedText TCharacterTalentName2002043 => null;

		public static TLocalizedText TCharacterTalentDesc2002043 => null;

		public static TLocalizedText TCharacterTalentName2002051 => null;

		public static TLocalizedText TCharacterTalentDesc2002051 => null;

		public static TLocalizedText TCharacterTalentName2002052 => null;

		public static TLocalizedText TCharacterTalentDesc2002052 => null;

		public static TLocalizedText TCharacterTalentName2002053 => null;

		public static TLocalizedText TCharacterTalentDesc2002053 => null;

		public static TLocalizedText TCharacterTalentName2002061 => null;

		public static TLocalizedText TCharacterTalentDesc2002061 => null;

		public static TLocalizedText TCharacterTalentName2002062 => null;

		public static TLocalizedText TCharacterTalentDesc2002062 => null;

		public static TLocalizedText TCharacterTalentName2002063 => null;

		public static TLocalizedText TCharacterTalentDesc2002063 => null;

		public static TLocalizedText TCharacterTalentTreeName100001 => null;

		public static TLocalizedText TCharacterTalentTreeDesc100001 => null;

		public static TLocalizedText TCharacterTalentTreeName100003 => null;

		public static TLocalizedText TCharacterTalentTreeDesc100003 => null;

		public static TLocalizedText TCharacterTalentTreeName100004 => null;

		public static TLocalizedText TCharacterTalentTreeDesc100004 => null;

		public static TLocalizedText TCharacterTalentTreeName200001 => null;

		public static TLocalizedText TCharacterTalentTreeDesc200001 => null;

		public static TLocalizedText TCharacterTalentTreeName200002 => null;

		public static TLocalizedText TCharacterTalentTreeDesc200002 => null;

		public static TLocalizedText TEmployeeName1000001 => null;

		public static TLocalizedText TEmployeeSubName1000001 => null;

		public static TLocalizedText TEmployeeDesc1000001 => null;

		public static TLocalizedText TEmployeeName1000002 => null;

		public static TLocalizedText TEmployeeSubName1000002 => null;

		public static TLocalizedText TEmployeeDesc1000002 => null;

		public static TLocalizedText TEmployeeName1000003 => null;

		public static TLocalizedText TEmployeeSubName1000003 => null;

		public static TLocalizedText TEmployeeDesc1000003 => null;

		public static TLocalizedText TEmployeeName1000004 => null;

		public static TLocalizedText TEmployeeSubName1000004 => null;

		public static TLocalizedText TEmployeeDesc1000004 => null;

		public static TLocalizedText TEmployeeName1000005 => null;

		public static TLocalizedText TEmployeeSubName1000005 => null;

		public static TLocalizedText TEmployeeDesc1000005 => null;

		public static TLocalizedText TEmployeeName1000006 => null;

		public static TLocalizedText TEmployeeSubName1000006 => null;

		public static TLocalizedText TEmployeeDesc1000006 => null;

		public static TLocalizedText TEmployeeName1000007 => null;

		public static TLocalizedText TEmployeeSubName1000007 => null;

		public static TLocalizedText TEmployeeDesc1000007 => null;

		public static TLocalizedText TEmployeeName1000008 => null;

		public static TLocalizedText TEmployeeSubName1000008 => null;

		public static TLocalizedText TEmployeeDesc1000008 => null;

		public static TLocalizedText TEmployeeName1000009 => null;

		public static TLocalizedText TEmployeeSubName1000009 => null;

		public static TLocalizedText TEmployeeDesc1000009 => null;

		public static TLocalizedText TEmployeeName1000010 => null;

		public static TLocalizedText TEmployeeSubName1000010 => null;

		public static TLocalizedText TEmployeeDesc1000010 => null;

		public static TLocalizedText TEmployeeName1000011 => null;

		public static TLocalizedText TEmployeeSubName1000011 => null;

		public static TLocalizedText TEmployeeDesc1000011 => null;

		public static TLocalizedText TEmployeeName1000012 => null;

		public static TLocalizedText TEmployeeSubName1000012 => null;

		public static TLocalizedText TEmployeeDesc1000012 => null;

		public static TLocalizedText TEmployeeName1000013 => null;

		public static TLocalizedText TEmployeeSubName1000013 => null;

		public static TLocalizedText TEmployeeDesc1000013 => null;

		public static TLocalizedText TEmployeeName1000014 => null;

		public static TLocalizedText TEmployeeSubName1000014 => null;

		public static TLocalizedText TEmployeeDesc1000014 => null;

		public static TLocalizedText TEmployeeName1000015 => null;

		public static TLocalizedText TEmployeeSubName1000015 => null;

		public static TLocalizedText TEmployeeDesc1000015 => null;

		public static TLocalizedText TEmployeeName1000016 => null;

		public static TLocalizedText TEmployeeSubName1000016 => null;

		public static TLocalizedText TEmployeeDesc1000016 => null;

		public static TLocalizedText TEmployeeName1000017 => null;

		public static TLocalizedText TEmployeeSubName1000017 => null;

		public static TLocalizedText TEmployeeDesc1000017 => null;

		public static TLocalizedText TEmployeeName1000018 => null;

		public static TLocalizedText TEmployeeSubName1000018 => null;

		public static TLocalizedText TEmployeeDesc1000018 => null;

		public static TLocalizedText TEmployeeName1000019 => null;

		public static TLocalizedText TEmployeeSubName1000019 => null;

		public static TLocalizedText TEmployeeDesc1000019 => null;

		public static TLocalizedText TEmployeeName1000020 => null;

		public static TLocalizedText TEmployeeSubName1000020 => null;

		public static TLocalizedText TEmployeeDesc1000020 => null;

		public static TLocalizedText TEmployeeName1000021 => null;

		public static TLocalizedText TEmployeeSubName1000021 => null;

		public static TLocalizedText TEmployeeDesc1000021 => null;

		public static TLocalizedText TEmployeeName1000022 => null;

		public static TLocalizedText TEmployeeSubName1000022 => null;

		public static TLocalizedText TEmployeeDesc1000022 => null;

		public static TLocalizedText TEmployeeName1000023 => null;

		public static TLocalizedText TEmployeeSubName1000023 => null;

		public static TLocalizedText TEmployeeDesc1000023 => null;

		public static TLocalizedText TEmployeeName1000024 => null;

		public static TLocalizedText TEmployeeSubName1000024 => null;

		public static TLocalizedText TEmployeeDesc1000024 => null;

		public static TLocalizedText TEmployeeName1000025 => null;

		public static TLocalizedText TEmployeeSubName1000025 => null;

		public static TLocalizedText TEmployeeDesc1000025 => null;

		public static TLocalizedText TEmployeeName1000026 => null;

		public static TLocalizedText TEmployeeSubName1000026 => null;

		public static TLocalizedText TEmployeeDesc1000026 => null;

		public static TLocalizedText TEmployeeName1000027 => null;

		public static TLocalizedText TEmployeeSubName1000027 => null;

		public static TLocalizedText TEmployeeDesc1000027 => null;

		public static TLocalizedText TEmployeeName1000028 => null;

		public static TLocalizedText TEmployeeSubName1000028 => null;

		public static TLocalizedText TEmployeeDesc1000028 => null;

		public static TLocalizedText TEmployeeName1000029 => null;

		public static TLocalizedText TEmployeeSubName1000029 => null;

		public static TLocalizedText TEmployeeDesc1000029 => null;

		public static TLocalizedText TEmployeeName1000030 => null;

		public static TLocalizedText TEmployeeSubName1000030 => null;

		public static TLocalizedText TEmployeeDesc1000030 => null;

		public static TLocalizedText TEmployeeName1000031 => null;

		public static TLocalizedText TEmployeeSubName1000031 => null;

		public static TLocalizedText TEmployeeDesc1000031 => null;

		public static TLocalizedText TEmployeeName1000032 => null;

		public static TLocalizedText TEmployeeSubName1000032 => null;

		public static TLocalizedText TEmployeeDesc1000032 => null;

		public static TLocalizedText TEmployeeName1000033 => null;

		public static TLocalizedText TEmployeeSubName1000033 => null;

		public static TLocalizedText TEmployeeDesc1000033 => null;

		public static TLocalizedText TEmployeeName1000034 => null;

		public static TLocalizedText TEmployeeSubName1000034 => null;

		public static TLocalizedText TEmployeeDesc1000034 => null;

		public static TLocalizedText TEmployeeName1000035 => null;

		public static TLocalizedText TEmployeeSubName1000035 => null;

		public static TLocalizedText TEmployeeDesc1000035 => null;

		public static TLocalizedText TEmployeeName1000036 => null;

		public static TLocalizedText TEmployeeSubName1000036 => null;

		public static TLocalizedText TEmployeeDesc1000036 => null;

		public static TLocalizedText TEmployeeName1000037 => null;

		public static TLocalizedText TEmployeeSubName1000037 => null;

		public static TLocalizedText TEmployeeDesc1000037 => null;

		public static TLocalizedText TEmployeeName1000038 => null;

		public static TLocalizedText TEmployeeSubName1000038 => null;

		public static TLocalizedText TEmployeeDesc1000038 => null;

		public static TLocalizedText TEmployeeName1000039 => null;

		public static TLocalizedText TEmployeeSubName1000039 => null;

		public static TLocalizedText TEmployeeDesc1000039 => null;

		public static TLocalizedText TEmployeeName1000040 => null;

		public static TLocalizedText TEmployeeSubName1000040 => null;

		public static TLocalizedText TEmployeeDesc1000040 => null;

		public static TLocalizedText TEmployeeName1000041 => null;

		public static TLocalizedText TEmployeeSubName1000041 => null;

		public static TLocalizedText TEmployeeDesc1000041 => null;

		public static TLocalizedText TEmployeeName1000042 => null;

		public static TLocalizedText TEmployeeSubName1000042 => null;

		public static TLocalizedText TEmployeeDesc1000042 => null;

		public static TLocalizedText TEmployeeName1000043 => null;

		public static TLocalizedText TEmployeeSubName1000043 => null;

		public static TLocalizedText TEmployeeDesc1000043 => null;

		public static TLocalizedText TEmployeeName1000044 => null;

		public static TLocalizedText TEmployeeSubName1000044 => null;

		public static TLocalizedText TEmployeeDesc1000044 => null;

		public static TLocalizedText TEmployeeName1000045 => null;

		public static TLocalizedText TEmployeeSubName1000045 => null;

		public static TLocalizedText TEmployeeDesc1000045 => null;

		public static TLocalizedText TEmployeeName1000046 => null;

		public static TLocalizedText TEmployeeSubName1000046 => null;

		public static TLocalizedText TEmployeeDesc1000046 => null;

		public static TLocalizedText TEmployeeName1000047 => null;

		public static TLocalizedText TEmployeeSubName1000047 => null;

		public static TLocalizedText TEmployeeDesc1000047 => null;

		public static TLocalizedText TEmployeeName1000048 => null;

		public static TLocalizedText TEmployeeSubName1000048 => null;

		public static TLocalizedText TEmployeeDesc1000048 => null;

		public static TLocalizedText TEmployeeName1000049 => null;

		public static TLocalizedText TEmployeeSubName1000049 => null;

		public static TLocalizedText TEmployeeDesc1000049 => null;

		public static TLocalizedText TEmployeeName1000050 => null;

		public static TLocalizedText TEmployeeSubName1000050 => null;

		public static TLocalizedText TEmployeeDesc1000050 => null;

		public static TLocalizedText TEmployeeName1000051 => null;

		public static TLocalizedText TEmployeeSubName1000051 => null;

		public static TLocalizedText TEmployeeDesc1000051 => null;

		public static TLocalizedText TEmployeeName1000052 => null;

		public static TLocalizedText TEmployeeSubName1000052 => null;

		public static TLocalizedText TEmployeeDesc1000052 => null;

		public static TLocalizedText TEmployeeName1000053 => null;

		public static TLocalizedText TEmployeeSubName1000053 => null;

		public static TLocalizedText TEmployeeDesc1000053 => null;

		public static TLocalizedText TEmployeeName1000054 => null;

		public static TLocalizedText TEmployeeSubName1000054 => null;

		public static TLocalizedText TEmployeeDesc1000054 => null;

		public static TLocalizedText TEmployeeName1000055 => null;

		public static TLocalizedText TEmployeeSubName1000055 => null;

		public static TLocalizedText TEmployeeDesc1000055 => null;

		public static TLocalizedText TEmployeeName1000056 => null;

		public static TLocalizedText TEmployeeSubName1000056 => null;

		public static TLocalizedText TEmployeeDesc1000056 => null;

		public static TLocalizedText TEmployeeName1000057 => null;

		public static TLocalizedText TEmployeeSubName1000057 => null;

		public static TLocalizedText TEmployeeDesc1000057 => null;

		public static TLocalizedText TEmployeeName1000058 => null;

		public static TLocalizedText TEmployeeSubName1000058 => null;

		public static TLocalizedText TEmployeeDesc1000058 => null;

		public static TLocalizedText TEmployeeName1000059 => null;

		public static TLocalizedText TEmployeeSubName1000059 => null;

		public static TLocalizedText TEmployeeDesc1000059 => null;

		public static TLocalizedText TEmployeeName1000060 => null;

		public static TLocalizedText TEmployeeSubName1000060 => null;

		public static TLocalizedText TEmployeeDesc1000060 => null;

		public static TLocalizedText TEmployeeName1000061 => null;

		public static TLocalizedText TEmployeeSubName1000061 => null;

		public static TLocalizedText TEmployeeDesc1000061 => null;

		public static TLocalizedText TEmployeeName1000062 => null;

		public static TLocalizedText TEmployeeSubName1000062 => null;

		public static TLocalizedText TEmployeeDesc1000062 => null;

		public static TLocalizedText TEmployeeName1000063 => null;

		public static TLocalizedText TEmployeeSubName1000063 => null;

		public static TLocalizedText TEmployeeDesc1000063 => null;

		public static TLocalizedText TEmployeeName1000064 => null;

		public static TLocalizedText TEmployeeSubName1000064 => null;

		public static TLocalizedText TEmployeeDesc1000064 => null;

		public static TLocalizedText TEmployeeName1000065 => null;

		public static TLocalizedText TEmployeeSubName1000065 => null;

		public static TLocalizedText TEmployeeDesc1000065 => null;

		public static TLocalizedText TEmployeeName1000066 => null;

		public static TLocalizedText TEmployeeSubName1000066 => null;

		public static TLocalizedText TEmployeeDesc1000066 => null;

		public static TLocalizedText TEmployeeName1000067 => null;

		public static TLocalizedText TEmployeeSubName1000067 => null;

		public static TLocalizedText TEmployeeDesc1000067 => null;

		public static TLocalizedText TEmployeeName1000068 => null;

		public static TLocalizedText TEmployeeSubName1000068 => null;

		public static TLocalizedText TEmployeeDesc1000068 => null;

		public static TLocalizedText TEmployeeName1000069 => null;

		public static TLocalizedText TEmployeeSubName1000069 => null;

		public static TLocalizedText TEmployeeDesc1000069 => null;

		public static TLocalizedText TEmployeeName1000070 => null;

		public static TLocalizedText TEmployeeSubName1000070 => null;

		public static TLocalizedText TEmployeeDesc1000070 => null;

		public static TLocalizedText TEmployeeName1000071 => null;

		public static TLocalizedText TEmployeeSubName1000071 => null;

		public static TLocalizedText TEmployeeDesc1000071 => null;

		public static TLocalizedText TEmployeeName1000072 => null;

		public static TLocalizedText TEmployeeSubName1000072 => null;

		public static TLocalizedText TEmployeeDesc1000072 => null;

		public static TLocalizedText TEmployeeName1000073 => null;

		public static TLocalizedText TEmployeeSubName1000073 => null;

		public static TLocalizedText TEmployeeDesc1000073 => null;

		public static TLocalizedText TEmployeeName1000074 => null;

		public static TLocalizedText TEmployeeSubName1000074 => null;

		public static TLocalizedText TEmployeeDesc1000074 => null;

		public static TLocalizedText TEmployeeName1000075 => null;

		public static TLocalizedText TEmployeeSubName1000075 => null;

		public static TLocalizedText TEmployeeDesc1000075 => null;

		public static TLocalizedText TEmployeeName1000076 => null;

		public static TLocalizedText TEmployeeSubName1000076 => null;

		public static TLocalizedText TEmployeeDesc1000076 => null;

		public static TLocalizedText TEmployeeName1000077 => null;

		public static TLocalizedText TEmployeeSubName1000077 => null;

		public static TLocalizedText TEmployeeDesc1000077 => null;

		public static TLocalizedText TEmployeeName1000078 => null;

		public static TLocalizedText TEmployeeSubName1000078 => null;

		public static TLocalizedText TEmployeeDesc1000078 => null;

		public static TLocalizedText TEmployeeName1000079 => null;

		public static TLocalizedText TEmployeeSubName1000079 => null;

		public static TLocalizedText TEmployeeDesc1000079 => null;

		public static TLocalizedText TEmployeeName1000080 => null;

		public static TLocalizedText TEmployeeSubName1000080 => null;

		public static TLocalizedText TEmployeeDesc1000080 => null;

		public static TLocalizedText TEmployeeName1000081 => null;

		public static TLocalizedText TEmployeeSubName1000081 => null;

		public static TLocalizedText TEmployeeDesc1000081 => null;

		public static TLocalizedText TEmployeeName1000082 => null;

		public static TLocalizedText TEmployeeSubName1000082 => null;

		public static TLocalizedText TEmployeeDesc1000082 => null;

		public static TLocalizedText TEmployeeName1000083 => null;

		public static TLocalizedText TEmployeeSubName1000083 => null;

		public static TLocalizedText TEmployeeDesc1000083 => null;

		public static TLocalizedText TEmployeeName1000084 => null;

		public static TLocalizedText TEmployeeSubName1000084 => null;

		public static TLocalizedText TEmployeeDesc1000084 => null;

		public static TLocalizedText TEmployeeName1000085 => null;

		public static TLocalizedText TEmployeeSubName1000085 => null;

		public static TLocalizedText TEmployeeDesc1000085 => null;

		public static TLocalizedText TEmployeeName1000086 => null;

		public static TLocalizedText TEmployeeSubName1000086 => null;

		public static TLocalizedText TEmployeeDesc1000086 => null;

		public static TLocalizedText TEmployeeName1000087 => null;

		public static TLocalizedText TEmployeeSubName1000087 => null;

		public static TLocalizedText TEmployeeDesc1000087 => null;

		public static TLocalizedText TEmployeeName1000088 => null;

		public static TLocalizedText TEmployeeSubName1000088 => null;

		public static TLocalizedText TEmployeeDesc1000088 => null;

		public static TLocalizedText TEmployeeName1000089 => null;

		public static TLocalizedText TEmployeeSubName1000089 => null;

		public static TLocalizedText TEmployeeDesc1000089 => null;

		public static TLocalizedText TEmployeeName1000090 => null;

		public static TLocalizedText TEmployeeSubName1000090 => null;

		public static TLocalizedText TEmployeeDesc1000090 => null;

		public static TLocalizedText TEmployeeName1000091 => null;

		public static TLocalizedText TEmployeeSubName1000091 => null;

		public static TLocalizedText TEmployeeDesc1000091 => null;

		public static TLocalizedText TEmployeeName1000092 => null;

		public static TLocalizedText TEmployeeSubName1000092 => null;

		public static TLocalizedText TEmployeeDesc1000092 => null;

		public static TLocalizedText TEmployeeName1000093 => null;

		public static TLocalizedText TEmployeeSubName1000093 => null;

		public static TLocalizedText TEmployeeDesc1000093 => null;

		public static TLocalizedText TEmployeeName1000094 => null;

		public static TLocalizedText TEmployeeSubName1000094 => null;

		public static TLocalizedText TEmployeeDesc1000094 => null;

		public static TLocalizedText TEmployeeName1000095 => null;

		public static TLocalizedText TEmployeeSubName1000095 => null;

		public static TLocalizedText TEmployeeDesc1000095 => null;

		public static TLocalizedText TEmployeeName1000096 => null;

		public static TLocalizedText TEmployeeSubName1000096 => null;

		public static TLocalizedText TEmployeeDesc1000096 => null;

		public static TLocalizedText TEmployeeName1000097 => null;

		public static TLocalizedText TEmployeeSubName1000097 => null;

		public static TLocalizedText TEmployeeDesc1000097 => null;

		public static TLocalizedText TEmployeeName1000098 => null;

		public static TLocalizedText TEmployeeSubName1000098 => null;

		public static TLocalizedText TEmployeeDesc1000098 => null;

		public static TLocalizedText TEmployeeName1000099 => null;

		public static TLocalizedText TEmployeeSubName1000099 => null;

		public static TLocalizedText TEmployeeDesc1000099 => null;

		public static TLocalizedText TEmployeeName1000100 => null;

		public static TLocalizedText TEmployeeSubName1000100 => null;

		public static TLocalizedText TEmployeeDesc1000100 => null;

		public static TLocalizedText TEmployeeSkillName1000001 => null;

		public static TLocalizedText TEmployeeSkillDesc1000001 => null;

		public static TLocalizedText TEmployeeSkillName1000002 => null;

		public static TLocalizedText TEmployeeSkillDesc1000002 => null;

		public static TLocalizedText TEmployeeSkillName1000003 => null;

		public static TLocalizedText TEmployeeSkillDesc1000003 => null;

		public static TLocalizedText TEmployeeSkillName1000004 => null;

		public static TLocalizedText TEmployeeSkillDesc1000004 => null;

		public static TLocalizedText TEmployeeSkillName1000005 => null;

		public static TLocalizedText TEmployeeSkillDesc1000005 => null;

		public static TLocalizedText TEmployeeSkillName1000006 => null;

		public static TLocalizedText TEmployeeSkillDesc1000006 => null;

		public static TLocalizedText TEmployeeSkillName1000007 => null;

		public static TLocalizedText TEmployeeSkillDesc1000007 => null;

		public static TLocalizedText TEmployeeSkillName1000008 => null;

		public static TLocalizedText TEmployeeSkillDesc1000008 => null;

		public static TLocalizedText TEmployeeSkillName1000009 => null;

		public static TLocalizedText TEmployeeSkillDesc1000009 => null;

		public static TLocalizedText TEmployeeSkillName1000010 => null;

		public static TLocalizedText TEmployeeSkillDesc1000010 => null;

		public static TLocalizedText TEmployeeSkillName1000011 => null;

		public static TLocalizedText TEmployeeSkillDesc1000011 => null;

		public static TLocalizedText TEmployeeSkillName1000012 => null;

		public static TLocalizedText TEmployeeSkillDesc1000012 => null;

		public static TLocalizedText TEmployeeSkillName1000013 => null;

		public static TLocalizedText TEmployeeSkillDesc1000013 => null;

		public static TLocalizedText TEmployeeSkillName1000014 => null;

		public static TLocalizedText TEmployeeSkillDesc1000014 => null;

		public static TLocalizedText TEmployeeSkillName1000015 => null;

		public static TLocalizedText TEmployeeSkillDesc1000015 => null;

		public static TLocalizedText TEmployeeSkillName1000016 => null;

		public static TLocalizedText TEmployeeSkillDesc1000016 => null;

		public static TLocalizedText TEmployeeSkillName1000017 => null;

		public static TLocalizedText TEmployeeSkillDesc1000017 => null;

		public static TLocalizedText TEmployeeSkillName1000018 => null;

		public static TLocalizedText TEmployeeSkillDesc1000018 => null;

		public static TLocalizedText TEmployeeSkillName1000019 => null;

		public static TLocalizedText TEmployeeSkillDesc1000019 => null;

		public static TLocalizedText TEmployeeSkillName1000020 => null;

		public static TLocalizedText TEmployeeSkillDesc1000020 => null;

		public static TLocalizedText TEmployeeSkillName1000021 => null;

		public static TLocalizedText TEmployeeSkillDesc1000021 => null;

		public static TLocalizedText TEmployeeSkillName1000022 => null;

		public static TLocalizedText TEmployeeSkillDesc1000022 => null;

		public static TLocalizedText TEmployeeSkillName1000023 => null;

		public static TLocalizedText TEmployeeSkillDesc1000023 => null;

		public static TLocalizedText TEmployeeSkillName1000024 => null;

		public static TLocalizedText TEmployeeSkillDesc1000024 => null;

		public static TLocalizedText TEmployeeSkillName1000025 => null;

		public static TLocalizedText TEmployeeSkillDesc1000025 => null;

		public static TLocalizedText TEmployeeSkillName1000026 => null;

		public static TLocalizedText TEmployeeSkillDesc1000026 => null;

		public static TLocalizedText TEmployeeSkillName1000027 => null;

		public static TLocalizedText TEmployeeSkillDesc1000027 => null;

		public static TLocalizedText TEmployeeSkillName1000028 => null;

		public static TLocalizedText TEmployeeSkillDesc1000028 => null;

		public static TLocalizedText TEntranceName1000 => null;

		public static TLocalizedText TEntranceName1001 => null;

		public static TLocalizedText TEntranceName1002 => null;

		public static TLocalizedText TEntranceName1003 => null;

		public static TLocalizedText TEntranceName1004 => null;

		public static TLocalizedText TEntranceName1012 => null;

		public static TLocalizedText TEntranceName1013 => null;

		public static TLocalizedText TEntranceName1014 => null;

		public static TLocalizedText TEntranceName2000 => null;

		public static TLocalizedText TEntranceName2001 => null;

		public static TLocalizedText TEntranceName2002 => null;

		public static TLocalizedText TEntranceName2003 => null;

		public static TLocalizedText TEntranceName2004 => null;

		public static TLocalizedText TEntranceName2005 => null;

		public static TLocalizedText TEntranceName3001 => null;

		public static TLocalizedText TEntranceName3002 => null;

		public static TLocalizedText TEntranceName4001 => null;

		public static TLocalizedText TEntranceName4002 => null;

		public static TLocalizedText TEntranceName4003 => null;

		public static TLocalizedText TEntranceName4004 => null;

		public static TLocalizedText TEntranceName4005 => null;

		public static TLocalizedText TEquipmentName1001 => null;

		public static TLocalizedText TEquipmentDesc1001 => null;

		public static TLocalizedText TEquipmentName1002 => null;

		public static TLocalizedText TEquipmentDesc1002 => null;

		public static TLocalizedText TEquipmentName1003 => null;

		public static TLocalizedText TEquipmentDesc1003 => null;

		public static TLocalizedText TEquipmentName1004 => null;

		public static TLocalizedText TEquipmentDesc1004 => null;

		public static TLocalizedText TEquipmentName1005 => null;

		public static TLocalizedText TEquipmentDesc1005 => null;

		public static TLocalizedText TEquipmentName1006 => null;

		public static TLocalizedText TEquipmentDesc1006 => null;

		public static TLocalizedText TEquipmentName1007 => null;

		public static TLocalizedText TEquipmentDesc1007 => null;

		public static TLocalizedText TEquipmentName1008 => null;

		public static TLocalizedText TEquipmentDesc1008 => null;

		public static TLocalizedText TEquipmentName1009 => null;

		public static TLocalizedText TEquipmentDesc1009 => null;

		public static TLocalizedText TEquipmentName1010 => null;

		public static TLocalizedText TEquipmentDesc1010 => null;

		public static TLocalizedText TEquipmentName1011 => null;

		public static TLocalizedText TEquipmentDesc1011 => null;

		public static TLocalizedText TEquipmentName1012 => null;

		public static TLocalizedText TEquipmentDesc1012 => null;

		public static TLocalizedText TEquipmentName1013 => null;

		public static TLocalizedText TEquipmentDesc1013 => null;

		public static TLocalizedText TEquipmentName1014 => null;

		public static TLocalizedText TEquipmentDesc1014 => null;

		public static TLocalizedText TEquipmentName1015 => null;

		public static TLocalizedText TEquipmentDesc1015 => null;

		public static TLocalizedText TEquipmentName1016 => null;

		public static TLocalizedText TEquipmentDesc1016 => null;

		public static TLocalizedText TEquipmentName1017 => null;

		public static TLocalizedText TEquipmentDesc1017 => null;

		public static TLocalizedText TEquipmentName1018 => null;

		public static TLocalizedText TEquipmentDesc1018 => null;

		public static TLocalizedText TEquipmentName1019 => null;

		public static TLocalizedText TEquipmentDesc1019 => null;

		public static TLocalizedText TEquipmentName1020 => null;

		public static TLocalizedText TEquipmentDesc1020 => null;

		public static TLocalizedText TEquipmentName1021 => null;

		public static TLocalizedText TEquipmentDesc1021 => null;

		public static TLocalizedText TEquipmentName1022 => null;

		public static TLocalizedText TEquipmentDesc1022 => null;

		public static TLocalizedText TEquipmentName1101 => null;

		public static TLocalizedText TEquipmentDesc1101 => null;

		public static TLocalizedText TEquipmentName1102 => null;

		public static TLocalizedText TEquipmentDesc1102 => null;

		public static TLocalizedText TEquipmentName1103 => null;

		public static TLocalizedText TEquipmentDesc1103 => null;

		public static TLocalizedText TEquipmentName1104 => null;

		public static TLocalizedText TEquipmentDesc1104 => null;

		public static TLocalizedText TEquipmentName1105 => null;

		public static TLocalizedText TEquipmentDesc1105 => null;

		public static TLocalizedText TEquipmentName1106 => null;

		public static TLocalizedText TEquipmentDesc1106 => null;

		public static TLocalizedText TEquipmentName1107 => null;

		public static TLocalizedText TEquipmentDesc1107 => null;

		public static TLocalizedText TEquipmentName1108 => null;

		public static TLocalizedText TEquipmentDesc1108 => null;

		public static TLocalizedText TEquipmentName1109 => null;

		public static TLocalizedText TEquipmentDesc1109 => null;

		public static TLocalizedText TEquipmentName1110 => null;

		public static TLocalizedText TEquipmentDesc1110 => null;

		public static TLocalizedText TEquipmentName1111 => null;

		public static TLocalizedText TEquipmentDesc1111 => null;

		public static TLocalizedText TEquipmentName1112 => null;

		public static TLocalizedText TEquipmentDesc1112 => null;

		public static TLocalizedText TEquipmentName1113 => null;

		public static TLocalizedText TEquipmentDesc1113 => null;

		public static TLocalizedText TEquipmentName1114 => null;

		public static TLocalizedText TEquipmentDesc1114 => null;

		public static TLocalizedText TEquipmentName1115 => null;

		public static TLocalizedText TEquipmentDesc1115 => null;

		public static TLocalizedText TEquipmentName1116 => null;

		public static TLocalizedText TEquipmentDesc1116 => null;

		public static TLocalizedText TEquipmentName1117 => null;

		public static TLocalizedText TEquipmentDesc1117 => null;

		public static TLocalizedText TEquipmentName1118 => null;

		public static TLocalizedText TEquipmentDesc1118 => null;

		public static TLocalizedText TEquipmentName1119 => null;

		public static TLocalizedText TEquipmentDesc1119 => null;

		public static TLocalizedText TEquipmentName1120 => null;

		public static TLocalizedText TEquipmentDesc1120 => null;

		public static TLocalizedText TEquipmentName1121 => null;

		public static TLocalizedText TEquipmentDesc1121 => null;

		public static TLocalizedText TEquipmentName1122 => null;

		public static TLocalizedText TEquipmentDesc1122 => null;

		public static TLocalizedText TEquipmentName1201 => null;

		public static TLocalizedText TEquipmentDesc1201 => null;

		public static TLocalizedText TEquipmentName1202 => null;

		public static TLocalizedText TEquipmentDesc1202 => null;

		public static TLocalizedText TEquipmentName1203 => null;

		public static TLocalizedText TEquipmentDesc1203 => null;

		public static TLocalizedText TEquipmentName1204 => null;

		public static TLocalizedText TEquipmentDesc1204 => null;

		public static TLocalizedText TEquipmentName1205 => null;

		public static TLocalizedText TEquipmentDesc1205 => null;

		public static TLocalizedText TEquipmentName1206 => null;

		public static TLocalizedText TEquipmentDesc1206 => null;

		public static TLocalizedText TEquipmentName1207 => null;

		public static TLocalizedText TEquipmentDesc1207 => null;

		public static TLocalizedText TEquipmentName1208 => null;

		public static TLocalizedText TEquipmentDesc1208 => null;

		public static TLocalizedText TEquipmentName1209 => null;

		public static TLocalizedText TEquipmentDesc1209 => null;

		public static TLocalizedText TEquipmentName1210 => null;

		public static TLocalizedText TEquipmentDesc1210 => null;

		public static TLocalizedText TEquipmentName1211 => null;

		public static TLocalizedText TEquipmentDesc1211 => null;

		public static TLocalizedText TEquipmentName1212 => null;

		public static TLocalizedText TEquipmentDesc1212 => null;

		public static TLocalizedText TEquipmentName1213 => null;

		public static TLocalizedText TEquipmentDesc1213 => null;

		public static TLocalizedText TEquipmentName1214 => null;

		public static TLocalizedText TEquipmentDesc1214 => null;

		public static TLocalizedText TEquipmentName1215 => null;

		public static TLocalizedText TEquipmentDesc1215 => null;

		public static TLocalizedText TEquipmentName1216 => null;

		public static TLocalizedText TEquipmentDesc1216 => null;

		public static TLocalizedText TEquipmentName1217 => null;

		public static TLocalizedText TEquipmentDesc1217 => null;

		public static TLocalizedText TEquipmentName1218 => null;

		public static TLocalizedText TEquipmentDesc1218 => null;

		public static TLocalizedText TEquipmentName1219 => null;

		public static TLocalizedText TEquipmentDesc1219 => null;

		public static TLocalizedText TEquipmentName1220 => null;

		public static TLocalizedText TEquipmentDesc1220 => null;

		public static TLocalizedText TEquipmentName1221 => null;

		public static TLocalizedText TEquipmentDesc1221 => null;

		public static TLocalizedText TEquipmentName1222 => null;

		public static TLocalizedText TEquipmentDesc1222 => null;

		public static TLocalizedText TEquipmentName1301 => null;

		public static TLocalizedText TEquipmentDesc1301 => null;

		public static TLocalizedText TEquipmentName1302 => null;

		public static TLocalizedText TEquipmentDesc1302 => null;

		public static TLocalizedText TEquipmentName1303 => null;

		public static TLocalizedText TEquipmentDesc1303 => null;

		public static TLocalizedText TEquipmentName1304 => null;

		public static TLocalizedText TEquipmentDesc1304 => null;

		public static TLocalizedText TEquipmentName1305 => null;

		public static TLocalizedText TEquipmentDesc1305 => null;

		public static TLocalizedText TEquipmentName1306 => null;

		public static TLocalizedText TEquipmentDesc1306 => null;

		public static TLocalizedText TEquipmentName1307 => null;

		public static TLocalizedText TEquipmentDesc1307 => null;

		public static TLocalizedText TEquipmentName1308 => null;

		public static TLocalizedText TEquipmentDesc1308 => null;

		public static TLocalizedText TEquipmentName1309 => null;

		public static TLocalizedText TEquipmentDesc1309 => null;

		public static TLocalizedText TEquipmentName1310 => null;

		public static TLocalizedText TEquipmentDesc1310 => null;

		public static TLocalizedText TEquipmentName1311 => null;

		public static TLocalizedText TEquipmentDesc1311 => null;

		public static TLocalizedText TEquipmentName1312 => null;

		public static TLocalizedText TEquipmentDesc1312 => null;

		public static TLocalizedText TEquipmentName1313 => null;

		public static TLocalizedText TEquipmentDesc1313 => null;

		public static TLocalizedText TEquipmentName1314 => null;

		public static TLocalizedText TEquipmentDesc1314 => null;

		public static TLocalizedText TEquipmentName1315 => null;

		public static TLocalizedText TEquipmentDesc1315 => null;

		public static TLocalizedText TEquipmentName1316 => null;

		public static TLocalizedText TEquipmentDesc1316 => null;

		public static TLocalizedText TEquipmentName1317 => null;

		public static TLocalizedText TEquipmentDesc1317 => null;

		public static TLocalizedText TEquipmentName1318 => null;

		public static TLocalizedText TEquipmentDesc1318 => null;

		public static TLocalizedText TEquipmentName1319 => null;

		public static TLocalizedText TEquipmentDesc1319 => null;

		public static TLocalizedText TEquipmentName1320 => null;

		public static TLocalizedText TEquipmentDesc1320 => null;

		public static TLocalizedText TEquipmentName1321 => null;

		public static TLocalizedText TEquipmentDesc1321 => null;

		public static TLocalizedText TEquipmentName1322 => null;

		public static TLocalizedText TEquipmentDesc1322 => null;

		public static TLocalizedText TEquipmentName20011 => null;

		public static TLocalizedText TEquipmentDesc20011 => null;

		public static TLocalizedText TEquipmentName20012 => null;

		public static TLocalizedText TEquipmentDesc20012 => null;

		public static TLocalizedText TEquipmentName20013 => null;

		public static TLocalizedText TEquipmentDesc20013 => null;

		public static TLocalizedText TEquipmentName20014 => null;

		public static TLocalizedText TEquipmentDesc20014 => null;

		public static TLocalizedText TEquipmentName20015 => null;

		public static TLocalizedText TEquipmentDesc20015 => null;

		public static TLocalizedText TEquipmentName20016 => null;

		public static TLocalizedText TEquipmentDesc20016 => null;

		public static TLocalizedText TEquipmentName20021 => null;

		public static TLocalizedText TEquipmentDesc20021 => null;

		public static TLocalizedText TEquipmentName20022 => null;

		public static TLocalizedText TEquipmentDesc20022 => null;

		public static TLocalizedText TEquipmentName20023 => null;

		public static TLocalizedText TEquipmentDesc20023 => null;

		public static TLocalizedText TEquipmentName20024 => null;

		public static TLocalizedText TEquipmentDesc20024 => null;

		public static TLocalizedText TEquipmentName20025 => null;

		public static TLocalizedText TEquipmentDesc20025 => null;

		public static TLocalizedText TEquipmentName20026 => null;

		public static TLocalizedText TEquipmentDesc20026 => null;

		public static TLocalizedText TEquipmentName20031 => null;

		public static TLocalizedText TEquipmentDesc20031 => null;

		public static TLocalizedText TEquipmentName20032 => null;

		public static TLocalizedText TEquipmentDesc20032 => null;

		public static TLocalizedText TEquipmentName20033 => null;

		public static TLocalizedText TEquipmentDesc20033 => null;

		public static TLocalizedText TEquipmentName20034 => null;

		public static TLocalizedText TEquipmentDesc20034 => null;

		public static TLocalizedText TEquipmentName20035 => null;

		public static TLocalizedText TEquipmentDesc20035 => null;

		public static TLocalizedText TEquipmentName20036 => null;

		public static TLocalizedText TEquipmentDesc20036 => null;

		public static TLocalizedText TEquipmentName20041 => null;

		public static TLocalizedText TEquipmentDesc20041 => null;

		public static TLocalizedText TEquipmentName20042 => null;

		public static TLocalizedText TEquipmentDesc20042 => null;

		public static TLocalizedText TEquipmentName20043 => null;

		public static TLocalizedText TEquipmentDesc20043 => null;

		public static TLocalizedText TEquipmentName20044 => null;

		public static TLocalizedText TEquipmentDesc20044 => null;

		public static TLocalizedText TEquipmentName20051 => null;

		public static TLocalizedText TEquipmentDesc20051 => null;

		public static TLocalizedText TEquipmentName20052 => null;

		public static TLocalizedText TEquipmentDesc20052 => null;

		public static TLocalizedText TEquipmentName20053 => null;

		public static TLocalizedText TEquipmentDesc20053 => null;

		public static TLocalizedText TEquipmentName20061 => null;

		public static TLocalizedText TEquipmentDesc20061 => null;

		public static TLocalizedText TEquipmentName20062 => null;

		public static TLocalizedText TEquipmentDesc20062 => null;

		public static TLocalizedText TEquipmentName20063 => null;

		public static TLocalizedText TEquipmentDesc20063 => null;

		public static TLocalizedText TEquipmentName20071 => null;

		public static TLocalizedText TEquipmentDesc20071 => null;

		public static TLocalizedText TEquipmentName20072 => null;

		public static TLocalizedText TEquipmentDesc20072 => null;

		public static TLocalizedText TEquipmentName20073 => null;

		public static TLocalizedText TEquipmentDesc20073 => null;

		public static TLocalizedText TEquipmentName20081 => null;

		public static TLocalizedText TEquipmentDesc20081 => null;

		public static TLocalizedText TEquipmentName20082 => null;

		public static TLocalizedText TEquipmentDesc20082 => null;

		public static TLocalizedText TEquipmentName20083 => null;

		public static TLocalizedText TEquipmentDesc20083 => null;

		public static TLocalizedText TEquipmentName20091 => null;

		public static TLocalizedText TEquipmentDesc20091 => null;

		public static TLocalizedText TEquipmentName20092 => null;

		public static TLocalizedText TEquipmentDesc20092 => null;

		public static TLocalizedText TEquipmentName20093 => null;

		public static TLocalizedText TEquipmentDesc20093 => null;

		public static TLocalizedText TEquipmentName21011 => null;

		public static TLocalizedText TEquipmentDesc21011 => null;

		public static TLocalizedText TEquipmentName21012 => null;

		public static TLocalizedText TEquipmentDesc21012 => null;

		public static TLocalizedText TEquipmentName21013 => null;

		public static TLocalizedText TEquipmentDesc21013 => null;

		public static TLocalizedText TEquipmentName21014 => null;

		public static TLocalizedText TEquipmentDesc21014 => null;

		public static TLocalizedText TEquipmentName21015 => null;

		public static TLocalizedText TEquipmentDesc21015 => null;

		public static TLocalizedText TEquipmentName21016 => null;

		public static TLocalizedText TEquipmentDesc21016 => null;

		public static TLocalizedText TEquipmentName21021 => null;

		public static TLocalizedText TEquipmentDesc21021 => null;

		public static TLocalizedText TEquipmentName21022 => null;

		public static TLocalizedText TEquipmentDesc21022 => null;

		public static TLocalizedText TEquipmentName21023 => null;

		public static TLocalizedText TEquipmentDesc21023 => null;

		public static TLocalizedText TEquipmentName21024 => null;

		public static TLocalizedText TEquipmentDesc21024 => null;

		public static TLocalizedText TEquipmentName21025 => null;

		public static TLocalizedText TEquipmentDesc21025 => null;

		public static TLocalizedText TEquipmentName21026 => null;

		public static TLocalizedText TEquipmentDesc21026 => null;

		public static TLocalizedText TEquipmentName21031 => null;

		public static TLocalizedText TEquipmentDesc21031 => null;

		public static TLocalizedText TEquipmentName21032 => null;

		public static TLocalizedText TEquipmentDesc21032 => null;

		public static TLocalizedText TEquipmentName21033 => null;

		public static TLocalizedText TEquipmentDesc21033 => null;

		public static TLocalizedText TEquipmentName21034 => null;

		public static TLocalizedText TEquipmentDesc21034 => null;

		public static TLocalizedText TEquipmentName21041 => null;

		public static TLocalizedText TEquipmentDesc21041 => null;

		public static TLocalizedText TEquipmentName21042 => null;

		public static TLocalizedText TEquipmentDesc21042 => null;

		public static TLocalizedText TEquipmentName21043 => null;

		public static TLocalizedText TEquipmentDesc21043 => null;

		public static TLocalizedText TEquipmentName22011 => null;

		public static TLocalizedText TEquipmentDesc22011 => null;

		public static TLocalizedText TEquipmentName22012 => null;

		public static TLocalizedText TEquipmentDesc22012 => null;

		public static TLocalizedText TEquipmentName22013 => null;

		public static TLocalizedText TEquipmentDesc22013 => null;

		public static TLocalizedText TEquipmentName22014 => null;

		public static TLocalizedText TEquipmentDesc22014 => null;

		public static TLocalizedText TEquipmentName22015 => null;

		public static TLocalizedText TEquipmentDesc22015 => null;

		public static TLocalizedText TEquipmentName22016 => null;

		public static TLocalizedText TEquipmentDesc22016 => null;

		public static TLocalizedText TEquipmentName23011 => null;

		public static TLocalizedText TEquipmentDesc23011 => null;

		public static TLocalizedText TEquipmentName23012 => null;

		public static TLocalizedText TEquipmentDesc23012 => null;

		public static TLocalizedText TEquipmentName23013 => null;

		public static TLocalizedText TEquipmentDesc23013 => null;

		public static TLocalizedText TEquipmentName23014 => null;

		public static TLocalizedText TEquipmentDesc23014 => null;

		public static TLocalizedText TEquipmentName23021 => null;

		public static TLocalizedText TEquipmentDesc23021 => null;

		public static TLocalizedText TEquipmentName23022 => null;

		public static TLocalizedText TEquipmentDesc23022 => null;

		public static TLocalizedText TEquipmentName23023 => null;

		public static TLocalizedText TEquipmentDesc23023 => null;

		public static TLocalizedText TEquipmentName23031 => null;

		public static TLocalizedText TEquipmentDesc23031 => null;

		public static TLocalizedText TEquipmentName23032 => null;

		public static TLocalizedText TEquipmentDesc23032 => null;

		public static TLocalizedText TEquipmentName23033 => null;

		public static TLocalizedText TEquipmentDesc23033 => null;

		public static TLocalizedText TEquipmentName23041 => null;

		public static TLocalizedText TEquipmentDesc23041 => null;

		public static TLocalizedText TEquipmentName23042 => null;

		public static TLocalizedText TEquipmentDesc23042 => null;

		public static TLocalizedText TEquipmentName23051 => null;

		public static TLocalizedText TEquipmentDesc23051 => null;

		public static TLocalizedText TEquipmentName23052 => null;

		public static TLocalizedText TEquipmentDesc23052 => null;

		public static TLocalizedText TEquipmentName23053 => null;

		public static TLocalizedText TEquipmentDesc23053 => null;

		public static TLocalizedText TEquipmentName23061 => null;

		public static TLocalizedText TEquipmentDesc23061 => null;

		public static TLocalizedText TEquipmentName23062 => null;

		public static TLocalizedText TEquipmentDesc23062 => null;

		public static TLocalizedText TEquipmentName23063 => null;

		public static TLocalizedText TEquipmentDesc23063 => null;

		public static TLocalizedText TEquipmentName23071 => null;

		public static TLocalizedText TEquipmentDesc23071 => null;

		public static TLocalizedText TEquipmentName23072 => null;

		public static TLocalizedText TEquipmentDesc23072 => null;

		public static TLocalizedText TEquipmentName23073 => null;

		public static TLocalizedText TEquipmentDesc23073 => null;

		public static TLocalizedText TEquipmentName23081 => null;

		public static TLocalizedText TEquipmentDesc23081 => null;

		public static TLocalizedText TEquipmentName23082 => null;

		public static TLocalizedText TEquipmentDesc23082 => null;

		public static TLocalizedText TEquipmentName23083 => null;

		public static TLocalizedText TEquipmentDesc23083 => null;

		public static TLocalizedText TEquipmentName23091 => null;

		public static TLocalizedText TEquipmentDesc23091 => null;

		public static TLocalizedText TEquipmentName23092 => null;

		public static TLocalizedText TEquipmentDesc23092 => null;

		public static TLocalizedText TEquipmentName23093 => null;

		public static TLocalizedText TEquipmentDesc23093 => null;

		public static TLocalizedText TEquipmentName23101 => null;

		public static TLocalizedText TEquipmentDesc23101 => null;

		public static TLocalizedText TEquipmentName23102 => null;

		public static TLocalizedText TEquipmentDesc23102 => null;

		public static TLocalizedText TEquipmentName23103 => null;

		public static TLocalizedText TEquipmentDesc23103 => null;

		public static TLocalizedText TEquipmentName23111 => null;

		public static TLocalizedText TEquipmentDesc23111 => null;

		public static TLocalizedText TEquipmentName23112 => null;

		public static TLocalizedText TEquipmentDesc23112 => null;

		public static TLocalizedText TEquipmentName23113 => null;

		public static TLocalizedText TEquipmentDesc23113 => null;

		public static TLocalizedText TEquipmentName23121 => null;

		public static TLocalizedText TEquipmentDesc23121 => null;

		public static TLocalizedText TEquipmentName23122 => null;

		public static TLocalizedText TEquipmentDesc23122 => null;

		public static TLocalizedText TEquipmentName23123 => null;

		public static TLocalizedText TEquipmentDesc23123 => null;

		public static TLocalizedText TEquipmentName24011 => null;

		public static TLocalizedText TEquipmentDesc24011 => null;

		public static TLocalizedText TEquipmentName24012 => null;

		public static TLocalizedText TEquipmentDesc24012 => null;

		public static TLocalizedText TEquipmentName24013 => null;

		public static TLocalizedText TEquipmentDesc24013 => null;

		public static TLocalizedText TEquipmentName24021 => null;

		public static TLocalizedText TEquipmentDesc24021 => null;

		public static TLocalizedText TEquipmentName24022 => null;

		public static TLocalizedText TEquipmentDesc24022 => null;

		public static TLocalizedText TEquipmentName24023 => null;

		public static TLocalizedText TEquipmentDesc24023 => null;

		public static TLocalizedText TEquipmentName24031 => null;

		public static TLocalizedText TEquipmentDesc24031 => null;

		public static TLocalizedText TEquipmentName24032 => null;

		public static TLocalizedText TEquipmentDesc24032 => null;

		public static TLocalizedText TEquipmentName24033 => null;

		public static TLocalizedText TEquipmentDesc24033 => null;

		public static TLocalizedText TEquipmentName24041 => null;

		public static TLocalizedText TEquipmentDesc24041 => null;

		public static TLocalizedText TEquipmentName24042 => null;

		public static TLocalizedText TEquipmentDesc24042 => null;

		public static TLocalizedText TEquipmentName24043 => null;

		public static TLocalizedText TEquipmentDesc24043 => null;

		public static TLocalizedText TEquipmentName24044 => null;

		public static TLocalizedText TEquipmentDesc24044 => null;

		public static TLocalizedText TEquipmentName24045 => null;

		public static TLocalizedText TEquipmentDesc24045 => null;

		public static TLocalizedText TEquipmentName24051 => null;

		public static TLocalizedText TEquipmentDesc24051 => null;

		public static TLocalizedText TEquipmentName24052 => null;

		public static TLocalizedText TEquipmentDesc24052 => null;

		public static TLocalizedText TEquipmentName24053 => null;

		public static TLocalizedText TEquipmentDesc24053 => null;

		public static TLocalizedText TEquipmentName24054 => null;

		public static TLocalizedText TEquipmentDesc24054 => null;

		public static TLocalizedText TEquipmentName24055 => null;

		public static TLocalizedText TEquipmentDesc24055 => null;

		public static TLocalizedText TEquipmentName25011 => null;

		public static TLocalizedText TEquipmentDesc25011 => null;

		public static TLocalizedText TEquipmentName25012 => null;

		public static TLocalizedText TEquipmentDesc25012 => null;

		public static TLocalizedText TEquipmentName25013 => null;

		public static TLocalizedText TEquipmentDesc25013 => null;

		public static TLocalizedText TEquipmentName25014 => null;

		public static TLocalizedText TEquipmentDesc25014 => null;

		public static TLocalizedText TEquipmentName25015 => null;

		public static TLocalizedText TEquipmentDesc25015 => null;

		public static TLocalizedText TEquipmentName25021 => null;

		public static TLocalizedText TEquipmentDesc25021 => null;

		public static TLocalizedText TEquipmentName25022 => null;

		public static TLocalizedText TEquipmentDesc25022 => null;

		public static TLocalizedText TEquipmentName25023 => null;

		public static TLocalizedText TEquipmentDesc25023 => null;

		public static TLocalizedText TEquipmentName25024 => null;

		public static TLocalizedText TEquipmentDesc25024 => null;

		public static TLocalizedText TEquipmentName25025 => null;

		public static TLocalizedText TEquipmentDesc25025 => null;

		public static TLocalizedText TEquipmentName25031 => null;

		public static TLocalizedText TEquipmentDesc25031 => null;

		public static TLocalizedText TEquipmentName25032 => null;

		public static TLocalizedText TEquipmentDesc25032 => null;

		public static TLocalizedText TEquipmentName25033 => null;

		public static TLocalizedText TEquipmentDesc25033 => null;

		public static TLocalizedText TEquipmentName25034 => null;

		public static TLocalizedText TEquipmentDesc25034 => null;

		public static TLocalizedText TEquipmentName25035 => null;

		public static TLocalizedText TEquipmentDesc25035 => null;

		public static TLocalizedText TEquipmentName25041 => null;

		public static TLocalizedText TEquipmentDesc25041 => null;

		public static TLocalizedText TEquipmentName25042 => null;

		public static TLocalizedText TEquipmentDesc25042 => null;

		public static TLocalizedText TEquipmentName25043 => null;

		public static TLocalizedText TEquipmentDesc25043 => null;

		public static TLocalizedText TEquipmentName25044 => null;

		public static TLocalizedText TEquipmentDesc25044 => null;

		public static TLocalizedText TEquipmentName25051 => null;

		public static TLocalizedText TEquipmentDesc25051 => null;

		public static TLocalizedText TEquipmentName25052 => null;

		public static TLocalizedText TEquipmentDesc25052 => null;

		public static TLocalizedText TEquipmentName25053 => null;

		public static TLocalizedText TEquipmentDesc25053 => null;

		public static TLocalizedText TEquipmentName25054 => null;

		public static TLocalizedText TEquipmentDesc25054 => null;

		public static TLocalizedText TEquipmentName25061 => null;

		public static TLocalizedText TEquipmentDesc25061 => null;

		public static TLocalizedText TEquipmentName25062 => null;

		public static TLocalizedText TEquipmentDesc25062 => null;

		public static TLocalizedText TEquipmentName25063 => null;

		public static TLocalizedText TEquipmentDesc25063 => null;

		public static TLocalizedText TEquipmentName25071 => null;

		public static TLocalizedText TEquipmentDesc25071 => null;

		public static TLocalizedText TEquipmentName25072 => null;

		public static TLocalizedText TEquipmentDesc25072 => null;

		public static TLocalizedText TEquipmentName25073 => null;

		public static TLocalizedText TEquipmentDesc25073 => null;

		public static TLocalizedText TEquipmentName25074 => null;

		public static TLocalizedText TEquipmentDesc25074 => null;

		public static TLocalizedText TEquipmentName26011 => null;

		public static TLocalizedText TEquipmentDesc26011 => null;

		public static TLocalizedText TEquipmentName26012 => null;

		public static TLocalizedText TEquipmentDesc26012 => null;

		public static TLocalizedText TEquipmentName26022 => null;

		public static TLocalizedText TEquipmentDesc26022 => null;

		public static TLocalizedText TEquipmentName26031 => null;

		public static TLocalizedText TEquipmentDesc26031 => null;

		public static TLocalizedText TEquipmentName26032 => null;

		public static TLocalizedText TEquipmentDesc26032 => null;

		public static TLocalizedText TEquipmentName26042 => null;

		public static TLocalizedText TEquipmentDesc26042 => null;

		public static TLocalizedText TEquipmentName26051 => null;

		public static TLocalizedText TEquipmentDesc26051 => null;

		public static TLocalizedText TEquipmentName26052 => null;

		public static TLocalizedText TEquipmentDesc26052 => null;

		public static TLocalizedText TEquipmentName26062 => null;

		public static TLocalizedText TEquipmentDesc26062 => null;

		public static TLocalizedText TEquipmentName26071 => null;

		public static TLocalizedText TEquipmentDesc26071 => null;

		public static TLocalizedText TEquipmentName26072 => null;

		public static TLocalizedText TEquipmentDesc26072 => null;

		public static TLocalizedText TEquipmentName26073 => null;

		public static TLocalizedText TEquipmentDesc26073 => null;

		public static TLocalizedText TEquipmentName26081 => null;

		public static TLocalizedText TEquipmentDesc26081 => null;

		public static TLocalizedText TEquipmentName26082 => null;

		public static TLocalizedText TEquipmentDesc26082 => null;

		public static TLocalizedText TEquipmentName26083 => null;

		public static TLocalizedText TEquipmentDesc26083 => null;

		public static TLocalizedText TEquipmentName26091 => null;

		public static TLocalizedText TEquipmentDesc26091 => null;

		public static TLocalizedText TEquipmentName26101 => null;

		public static TLocalizedText TEquipmentDesc26101 => null;

		public static TLocalizedText TEquipmentName26102 => null;

		public static TLocalizedText TEquipmentDesc26102 => null;

		public static TLocalizedText TEquipmentName100011 => null;

		public static TLocalizedText TEquipmentDesc100011 => null;

		public static TLocalizedText TEquipmentName100013 => null;

		public static TLocalizedText TEquipmentDesc100013 => null;

		public static TLocalizedText TEquipmentName100014 => null;

		public static TLocalizedText TEquipmentDesc100014 => null;

		public static TLocalizedText TEquipmentName100015 => null;

		public static TLocalizedText TEquipmentDesc100015 => null;

		public static TLocalizedText TEquipmentName100021 => null;

		public static TLocalizedText TEquipmentDesc100021 => null;

		public static TLocalizedText TEquipmentName100023 => null;

		public static TLocalizedText TEquipmentDesc100023 => null;

		public static TLocalizedText TEquipmentName100024 => null;

		public static TLocalizedText TEquipmentDesc100024 => null;

		public static TLocalizedText TEquipmentName100025 => null;

		public static TLocalizedText TEquipmentDesc100025 => null;

		public static TLocalizedText TEquipmentName100031 => null;

		public static TLocalizedText TEquipmentDesc100031 => null;

		public static TLocalizedText TEquipmentName100033 => null;

		public static TLocalizedText TEquipmentDesc100033 => null;

		public static TLocalizedText TEquipmentName100034 => null;

		public static TLocalizedText TEquipmentDesc100034 => null;

		public static TLocalizedText TEquipmentName100035 => null;

		public static TLocalizedText TEquipmentDesc100035 => null;

		public static TLocalizedText TEquipmentName100041 => null;

		public static TLocalizedText TEquipmentDesc100041 => null;

		public static TLocalizedText TEquipmentName100043 => null;

		public static TLocalizedText TEquipmentDesc100043 => null;

		public static TLocalizedText TEquipmentName100044 => null;

		public static TLocalizedText TEquipmentDesc100044 => null;

		public static TLocalizedText TEquipmentName100045 => null;

		public static TLocalizedText TEquipmentDesc100045 => null;

		public static TLocalizedText TEquipmentName100051 => null;

		public static TLocalizedText TEquipmentDesc100051 => null;

		public static TLocalizedText TEquipmentName100053 => null;

		public static TLocalizedText TEquipmentDesc100053 => null;

		public static TLocalizedText TEquipmentName100054 => null;

		public static TLocalizedText TEquipmentDesc100054 => null;

		public static TLocalizedText TEquipmentName100055 => null;

		public static TLocalizedText TEquipmentDesc100055 => null;

		public static TLocalizedText TEquipmentName100061 => null;

		public static TLocalizedText TEquipmentDesc100061 => null;

		public static TLocalizedText TEquipmentName100063 => null;

		public static TLocalizedText TEquipmentDesc100063 => null;

		public static TLocalizedText TEquipmentName100064 => null;

		public static TLocalizedText TEquipmentDesc100064 => null;

		public static TLocalizedText TEquipmentName100065 => null;

		public static TLocalizedText TEquipmentDesc100065 => null;

		public static TLocalizedText TEquipmentName100071 => null;

		public static TLocalizedText TEquipmentDesc100071 => null;

		public static TLocalizedText TEquipmentName100073 => null;

		public static TLocalizedText TEquipmentDesc100073 => null;

		public static TLocalizedText TEquipmentName100074 => null;

		public static TLocalizedText TEquipmentDesc100074 => null;

		public static TLocalizedText TEquipmentName100075 => null;

		public static TLocalizedText TEquipmentDesc100075 => null;

		public static TLocalizedText TEquipmentName101011 => null;

		public static TLocalizedText TEquipmentDesc101011 => null;

		public static TLocalizedText TEquipmentName101013 => null;

		public static TLocalizedText TEquipmentDesc101013 => null;

		public static TLocalizedText TEquipmentName101014 => null;

		public static TLocalizedText TEquipmentDesc101014 => null;

		public static TLocalizedText TEquipmentName101015 => null;

		public static TLocalizedText TEquipmentDesc101015 => null;

		public static TLocalizedText TEquipmentName101021 => null;

		public static TLocalizedText TEquipmentDesc101021 => null;

		public static TLocalizedText TEquipmentName101023 => null;

		public static TLocalizedText TEquipmentDesc101023 => null;

		public static TLocalizedText TEquipmentName101024 => null;

		public static TLocalizedText TEquipmentDesc101024 => null;

		public static TLocalizedText TEquipmentName101025 => null;

		public static TLocalizedText TEquipmentDesc101025 => null;

		public static TLocalizedText TEquipmentName101031 => null;

		public static TLocalizedText TEquipmentDesc101031 => null;

		public static TLocalizedText TEquipmentName101033 => null;

		public static TLocalizedText TEquipmentDesc101033 => null;

		public static TLocalizedText TEquipmentName101034 => null;

		public static TLocalizedText TEquipmentDesc101034 => null;

		public static TLocalizedText TEquipmentName101035 => null;

		public static TLocalizedText TEquipmentDesc101035 => null;

		public static TLocalizedText TEquipmentName101041 => null;

		public static TLocalizedText TEquipmentDesc101041 => null;

		public static TLocalizedText TEquipmentName101043 => null;

		public static TLocalizedText TEquipmentDesc101043 => null;

		public static TLocalizedText TEquipmentName101044 => null;

		public static TLocalizedText TEquipmentDesc101044 => null;

		public static TLocalizedText TEquipmentName101045 => null;

		public static TLocalizedText TEquipmentDesc101045 => null;

		public static TLocalizedText TEquipmentName101051 => null;

		public static TLocalizedText TEquipmentDesc101051 => null;

		public static TLocalizedText TEquipmentName101053 => null;

		public static TLocalizedText TEquipmentDesc101053 => null;

		public static TLocalizedText TEquipmentName101061 => null;

		public static TLocalizedText TEquipmentDesc101061 => null;

		public static TLocalizedText TEquipmentName101063 => null;

		public static TLocalizedText TEquipmentDesc101063 => null;

		public static TLocalizedText TEquipmentName101064 => null;

		public static TLocalizedText TEquipmentDesc101064 => null;

		public static TLocalizedText TEquipmentName101065 => null;

		public static TLocalizedText TEquipmentDesc101065 => null;

		public static TLocalizedText TEquipmentName101074 => null;

		public static TLocalizedText TEquipmentDesc101074 => null;

		public static TLocalizedText TEquipmentName101075 => null;

		public static TLocalizedText TEquipmentDesc101075 => null;

		public static TLocalizedText TEquipmentName101081 => null;

		public static TLocalizedText TEquipmentDesc101081 => null;

		public static TLocalizedText TEquipmentName101083 => null;

		public static TLocalizedText TEquipmentDesc101083 => null;

		public static TLocalizedText TEquipmentName101084 => null;

		public static TLocalizedText TEquipmentDesc101084 => null;

		public static TLocalizedText TEquipmentName101085 => null;

		public static TLocalizedText TEquipmentDesc101085 => null;

		public static TLocalizedText TEquipmentName101091 => null;

		public static TLocalizedText TEquipmentDesc101091 => null;

		public static TLocalizedText TEquipmentName101093 => null;

		public static TLocalizedText TEquipmentDesc101093 => null;

		public static TLocalizedText TEquipmentName101094 => null;

		public static TLocalizedText TEquipmentDesc101094 => null;

		public static TLocalizedText TEquipmentName101095 => null;

		public static TLocalizedText TEquipmentDesc101095 => null;

		public static TLocalizedText TEquipmentName102011 => null;

		public static TLocalizedText TEquipmentDesc102011 => null;

		public static TLocalizedText TEquipmentName102013 => null;

		public static TLocalizedText TEquipmentDesc102013 => null;

		public static TLocalizedText TEquipmentName102014 => null;

		public static TLocalizedText TEquipmentDesc102014 => null;

		public static TLocalizedText TEquipmentName102015 => null;

		public static TLocalizedText TEquipmentDesc102015 => null;

		public static TLocalizedText TEquipmentName102021 => null;

		public static TLocalizedText TEquipmentDesc102021 => null;

		public static TLocalizedText TEquipmentName102023 => null;

		public static TLocalizedText TEquipmentDesc102023 => null;

		public static TLocalizedText TEquipmentName102024 => null;

		public static TLocalizedText TEquipmentDesc102024 => null;

		public static TLocalizedText TEquipmentName102025 => null;

		public static TLocalizedText TEquipmentDesc102025 => null;

		public static TLocalizedText TEquipmentName102034 => null;

		public static TLocalizedText TEquipmentDesc102034 => null;

		public static TLocalizedText TEquipmentName102035 => null;

		public static TLocalizedText TEquipmentDesc102035 => null;

		public static TLocalizedText TEquipmentName102041 => null;

		public static TLocalizedText TEquipmentDesc102041 => null;

		public static TLocalizedText TEquipmentName102043 => null;

		public static TLocalizedText TEquipmentDesc102043 => null;

		public static TLocalizedText TEquipmentName102044 => null;

		public static TLocalizedText TEquipmentDesc102044 => null;

		public static TLocalizedText TEquipmentName102045 => null;

		public static TLocalizedText TEquipmentDesc102045 => null;

		public static TLocalizedText TEquipmentName102051 => null;

		public static TLocalizedText TEquipmentDesc102051 => null;

		public static TLocalizedText TEquipmentName102053 => null;

		public static TLocalizedText TEquipmentDesc102053 => null;

		public static TLocalizedText TEquipmentName102054 => null;

		public static TLocalizedText TEquipmentDesc102054 => null;

		public static TLocalizedText TEquipmentName102055 => null;

		public static TLocalizedText TEquipmentDesc102055 => null;

		public static TLocalizedText TEquipmentName102064 => null;

		public static TLocalizedText TEquipmentDesc102064 => null;

		public static TLocalizedText TEquipmentName102065 => null;

		public static TLocalizedText TEquipmentDesc102065 => null;

		public static TLocalizedText TEquipmentName102071 => null;

		public static TLocalizedText TEquipmentDesc102071 => null;

		public static TLocalizedText TEquipmentName102073 => null;

		public static TLocalizedText TEquipmentDesc102073 => null;

		public static TLocalizedText TEquipmentName102074 => null;

		public static TLocalizedText TEquipmentDesc102074 => null;

		public static TLocalizedText TEquipmentName102075 => null;

		public static TLocalizedText TEquipmentDesc102075 => null;

		public static TLocalizedText TEquipmentName102081 => null;

		public static TLocalizedText TEquipmentDesc102081 => null;

		public static TLocalizedText TEquipmentName102083 => null;

		public static TLocalizedText TEquipmentDesc102083 => null;

		public static TLocalizedText TEquipmentName102084 => null;

		public static TLocalizedText TEquipmentDesc102084 => null;

		public static TLocalizedText TEquipmentName102085 => null;

		public static TLocalizedText TEquipmentDesc102085 => null;

		public static TLocalizedText TEquipmentName103011 => null;

		public static TLocalizedText TEquipmentDesc103011 => null;

		public static TLocalizedText TEquipmentName103013 => null;

		public static TLocalizedText TEquipmentDesc103013 => null;

		public static TLocalizedText TEquipmentName103014 => null;

		public static TLocalizedText TEquipmentDesc103014 => null;

		public static TLocalizedText TEquipmentName103015 => null;

		public static TLocalizedText TEquipmentDesc103015 => null;

		public static TLocalizedText TEquipmentName103021 => null;

		public static TLocalizedText TEquipmentDesc103021 => null;

		public static TLocalizedText TEquipmentName103023 => null;

		public static TLocalizedText TEquipmentDesc103023 => null;

		public static TLocalizedText TEquipmentName103024 => null;

		public static TLocalizedText TEquipmentDesc103024 => null;

		public static TLocalizedText TEquipmentName103025 => null;

		public static TLocalizedText TEquipmentDesc103025 => null;

		public static TLocalizedText TEquipmentName103031 => null;

		public static TLocalizedText TEquipmentDesc103031 => null;

		public static TLocalizedText TEquipmentName103033 => null;

		public static TLocalizedText TEquipmentDesc103033 => null;

		public static TLocalizedText TEquipmentName103034 => null;

		public static TLocalizedText TEquipmentDesc103034 => null;

		public static TLocalizedText TEquipmentName103035 => null;

		public static TLocalizedText TEquipmentDesc103035 => null;

		public static TLocalizedText TEquipmentName103044 => null;

		public static TLocalizedText TEquipmentDesc103044 => null;

		public static TLocalizedText TEquipmentName103045 => null;

		public static TLocalizedText TEquipmentDesc103045 => null;

		public static TLocalizedText TEquipmentName103051 => null;

		public static TLocalizedText TEquipmentDesc103051 => null;

		public static TLocalizedText TEquipmentName103053 => null;

		public static TLocalizedText TEquipmentDesc103053 => null;

		public static TLocalizedText TEquipmentName103054 => null;

		public static TLocalizedText TEquipmentDesc103054 => null;

		public static TLocalizedText TEquipmentName103055 => null;

		public static TLocalizedText TEquipmentDesc103055 => null;

		public static TLocalizedText TEquipmentName103061 => null;

		public static TLocalizedText TEquipmentDesc103061 => null;

		public static TLocalizedText TEquipmentName103063 => null;

		public static TLocalizedText TEquipmentDesc103063 => null;

		public static TLocalizedText TEquipmentName103064 => null;

		public static TLocalizedText TEquipmentDesc103064 => null;

		public static TLocalizedText TEquipmentName103065 => null;

		public static TLocalizedText TEquipmentDesc103065 => null;

		public static TLocalizedText TEquipmentName103071 => null;

		public static TLocalizedText TEquipmentDesc103071 => null;

		public static TLocalizedText TEquipmentName103073 => null;

		public static TLocalizedText TEquipmentDesc103073 => null;

		public static TLocalizedText TEquipmentName103074 => null;

		public static TLocalizedText TEquipmentDesc103074 => null;

		public static TLocalizedText TEquipmentName103075 => null;

		public static TLocalizedText TEquipmentDesc103075 => null;

		public static TLocalizedText TEquipmentName103081 => null;

		public static TLocalizedText TEquipmentDesc103081 => null;

		public static TLocalizedText TEquipmentName103083 => null;

		public static TLocalizedText TEquipmentDesc103083 => null;

		public static TLocalizedText TEquipmentName104011 => null;

		public static TLocalizedText TEquipmentDesc104011 => null;

		public static TLocalizedText TEquipmentName104013 => null;

		public static TLocalizedText TEquipmentDesc104013 => null;

		public static TLocalizedText TEquipmentName104014 => null;

		public static TLocalizedText TEquipmentDesc104014 => null;

		public static TLocalizedText TEquipmentName104015 => null;

		public static TLocalizedText TEquipmentDesc104015 => null;

		public static TLocalizedText TEquipmentName104021 => null;

		public static TLocalizedText TEquipmentDesc104021 => null;

		public static TLocalizedText TEquipmentName104023 => null;

		public static TLocalizedText TEquipmentDesc104023 => null;

		public static TLocalizedText TEquipmentName104024 => null;

		public static TLocalizedText TEquipmentDesc104024 => null;

		public static TLocalizedText TEquipmentName104025 => null;

		public static TLocalizedText TEquipmentDesc104025 => null;

		public static TLocalizedText TEquipmentName104034 => null;

		public static TLocalizedText TEquipmentDesc104034 => null;

		public static TLocalizedText TEquipmentName104035 => null;

		public static TLocalizedText TEquipmentDesc104035 => null;

		public static TLocalizedText TEquipmentName104041 => null;

		public static TLocalizedText TEquipmentDesc104041 => null;

		public static TLocalizedText TEquipmentName104043 => null;

		public static TLocalizedText TEquipmentDesc104043 => null;

		public static TLocalizedText TEquipmentName104044 => null;

		public static TLocalizedText TEquipmentDesc104044 => null;

		public static TLocalizedText TEquipmentName104045 => null;

		public static TLocalizedText TEquipmentDesc104045 => null;

		public static TLocalizedText TEquipmentName104051 => null;

		public static TLocalizedText TEquipmentDesc104051 => null;

		public static TLocalizedText TEquipmentName104053 => null;

		public static TLocalizedText TEquipmentDesc104053 => null;

		public static TLocalizedText TEquipmentName104054 => null;

		public static TLocalizedText TEquipmentDesc104054 => null;

		public static TLocalizedText TEquipmentName104055 => null;

		public static TLocalizedText TEquipmentDesc104055 => null;

		public static TLocalizedText TEquipmentName104064 => null;

		public static TLocalizedText TEquipmentDesc104064 => null;

		public static TLocalizedText TEquipmentName104065 => null;

		public static TLocalizedText TEquipmentDesc104065 => null;

		public static TLocalizedText TEquipmentName104071 => null;

		public static TLocalizedText TEquipmentDesc104071 => null;

		public static TLocalizedText TEquipmentName104073 => null;

		public static TLocalizedText TEquipmentDesc104073 => null;

		public static TLocalizedText TEquipmentName104074 => null;

		public static TLocalizedText TEquipmentDesc104074 => null;

		public static TLocalizedText TEquipmentName104075 => null;

		public static TLocalizedText TEquipmentDesc104075 => null;

		public static TLocalizedText TEquipmentName104081 => null;

		public static TLocalizedText TEquipmentDesc104081 => null;

		public static TLocalizedText TEquipmentName104083 => null;

		public static TLocalizedText TEquipmentDesc104083 => null;

		public static TLocalizedText TEquipmentName104084 => null;

		public static TLocalizedText TEquipmentDesc104084 => null;

		public static TLocalizedText TEquipmentName104085 => null;

		public static TLocalizedText TEquipmentDesc104085 => null;

		public static TLocalizedText TEquipmentName104091 => null;

		public static TLocalizedText TEquipmentDesc104091 => null;

		public static TLocalizedText TEquipmentName104093 => null;

		public static TLocalizedText TEquipmentDesc104093 => null;

		public static TLocalizedText TEquipmentName105011 => null;

		public static TLocalizedText TEquipmentDesc105011 => null;

		public static TLocalizedText TEquipmentName105013 => null;

		public static TLocalizedText TEquipmentDesc105013 => null;

		public static TLocalizedText TEquipmentName105014 => null;

		public static TLocalizedText TEquipmentDesc105014 => null;

		public static TLocalizedText TEquipmentName105015 => null;

		public static TLocalizedText TEquipmentDesc105015 => null;

		public static TLocalizedText TEquipmentName105024 => null;

		public static TLocalizedText TEquipmentDesc105024 => null;

		public static TLocalizedText TEquipmentName105025 => null;

		public static TLocalizedText TEquipmentDesc105025 => null;

		public static TLocalizedText TEquipmentName105031 => null;

		public static TLocalizedText TEquipmentDesc105031 => null;

		public static TLocalizedText TEquipmentName105033 => null;

		public static TLocalizedText TEquipmentDesc105033 => null;

		public static TLocalizedText TEquipmentName105034 => null;

		public static TLocalizedText TEquipmentDesc105034 => null;

		public static TLocalizedText TEquipmentName105035 => null;

		public static TLocalizedText TEquipmentDesc105035 => null;

		public static TLocalizedText TEquipmentName105041 => null;

		public static TLocalizedText TEquipmentDesc105041 => null;

		public static TLocalizedText TEquipmentName105043 => null;

		public static TLocalizedText TEquipmentDesc105043 => null;

		public static TLocalizedText TEquipmentName105044 => null;

		public static TLocalizedText TEquipmentDesc105044 => null;

		public static TLocalizedText TEquipmentName105045 => null;

		public static TLocalizedText TEquipmentDesc105045 => null;

		public static TLocalizedText TEquipmentName105054 => null;

		public static TLocalizedText TEquipmentDesc105054 => null;

		public static TLocalizedText TEquipmentName105055 => null;

		public static TLocalizedText TEquipmentDesc105055 => null;

		public static TLocalizedText TEquipmentName105061 => null;

		public static TLocalizedText TEquipmentDesc105061 => null;

		public static TLocalizedText TEquipmentName105063 => null;

		public static TLocalizedText TEquipmentDesc105063 => null;

		public static TLocalizedText TEquipmentName105064 => null;

		public static TLocalizedText TEquipmentDesc105064 => null;

		public static TLocalizedText TEquipmentName105065 => null;

		public static TLocalizedText TEquipmentDesc105065 => null;

		public static TLocalizedText TEquipmentName105071 => null;

		public static TLocalizedText TEquipmentDesc105071 => null;

		public static TLocalizedText TEquipmentName105073 => null;

		public static TLocalizedText TEquipmentDesc105073 => null;

		public static TLocalizedText TEquipmentName105074 => null;

		public static TLocalizedText TEquipmentDesc105074 => null;

		public static TLocalizedText TEquipmentName105075 => null;

		public static TLocalizedText TEquipmentDesc105075 => null;

		public static TLocalizedText TEquipmentName105081 => null;

		public static TLocalizedText TEquipmentDesc105081 => null;

		public static TLocalizedText TEquipmentName105083 => null;

		public static TLocalizedText TEquipmentDesc105083 => null;

		public static TLocalizedText TEquipmentName105091 => null;

		public static TLocalizedText TEquipmentDesc105091 => null;

		public static TLocalizedText TEquipmentName105093 => null;

		public static TLocalizedText TEquipmentDesc105093 => null;

		public static TLocalizedText TEquipmentName106011 => null;

		public static TLocalizedText TEquipmentDesc106011 => null;

		public static TLocalizedText TEquipmentName106013 => null;

		public static TLocalizedText TEquipmentDesc106013 => null;

		public static TLocalizedText TEquipmentName106014 => null;

		public static TLocalizedText TEquipmentDesc106014 => null;

		public static TLocalizedText TEquipmentName106015 => null;

		public static TLocalizedText TEquipmentDesc106015 => null;

		public static TLocalizedText TEquipmentName106024 => null;

		public static TLocalizedText TEquipmentDesc106024 => null;

		public static TLocalizedText TEquipmentName106025 => null;

		public static TLocalizedText TEquipmentDesc106025 => null;

		public static TLocalizedText TEquipmentName106031 => null;

		public static TLocalizedText TEquipmentDesc106031 => null;

		public static TLocalizedText TEquipmentName106033 => null;

		public static TLocalizedText TEquipmentDesc106033 => null;

		public static TLocalizedText TEquipmentName106034 => null;

		public static TLocalizedText TEquipmentDesc106034 => null;

		public static TLocalizedText TEquipmentName106035 => null;

		public static TLocalizedText TEquipmentDesc106035 => null;

		public static TLocalizedText TEquipmentName106044 => null;

		public static TLocalizedText TEquipmentDesc106044 => null;

		public static TLocalizedText TEquipmentName106045 => null;

		public static TLocalizedText TEquipmentDesc106045 => null;

		public static TLocalizedText TEquipmentName106051 => null;

		public static TLocalizedText TEquipmentDesc106051 => null;

		public static TLocalizedText TEquipmentName106053 => null;

		public static TLocalizedText TEquipmentDesc106053 => null;

		public static TLocalizedText TEquipmentName106061 => null;

		public static TLocalizedText TEquipmentDesc106061 => null;

		public static TLocalizedText TEquipmentName106063 => null;

		public static TLocalizedText TEquipmentDesc106063 => null;

		public static TLocalizedText TEquipmentName106064 => null;

		public static TLocalizedText TEquipmentDesc106064 => null;

		public static TLocalizedText TEquipmentName106065 => null;

		public static TLocalizedText TEquipmentDesc106065 => null;

		public static TLocalizedText TEquipmentName106071 => null;

		public static TLocalizedText TEquipmentDesc106071 => null;

		public static TLocalizedText TEquipmentName106073 => null;

		public static TLocalizedText TEquipmentDesc106073 => null;

		public static TLocalizedText TEquipmentName106074 => null;

		public static TLocalizedText TEquipmentDesc106074 => null;

		public static TLocalizedText TEquipmentName106075 => null;

		public static TLocalizedText TEquipmentDesc106075 => null;

		public static TLocalizedText TEquipmentName106081 => null;

		public static TLocalizedText TEquipmentDesc106081 => null;

		public static TLocalizedText TEquipmentName106083 => null;

		public static TLocalizedText TEquipmentDesc106083 => null;

		public static TLocalizedText TEquipmentName106091 => null;

		public static TLocalizedText TEquipmentDesc106091 => null;

		public static TLocalizedText TEquipmentName106093 => null;

		public static TLocalizedText TEquipmentDesc106093 => null;

		public static TLocalizedText TEquipmentName106094 => null;

		public static TLocalizedText TEquipmentDesc106094 => null;

		public static TLocalizedText TEquipmentName106095 => null;

		public static TLocalizedText TEquipmentDesc106095 => null;

		public static TLocalizedText TEquipmentName107011 => null;

		public static TLocalizedText TEquipmentDesc107011 => null;

		public static TLocalizedText TEquipmentName107013 => null;

		public static TLocalizedText TEquipmentDesc107013 => null;

		public static TLocalizedText TEquipmentName107014 => null;

		public static TLocalizedText TEquipmentDesc107014 => null;

		public static TLocalizedText TEquipmentName107015 => null;

		public static TLocalizedText TEquipmentDesc107015 => null;

		public static TLocalizedText TEquipmentName107024 => null;

		public static TLocalizedText TEquipmentDesc107024 => null;

		public static TLocalizedText TEquipmentName107025 => null;

		public static TLocalizedText TEquipmentDesc107025 => null;

		public static TLocalizedText TEquipmentName107031 => null;

		public static TLocalizedText TEquipmentDesc107031 => null;

		public static TLocalizedText TEquipmentName107033 => null;

		public static TLocalizedText TEquipmentDesc107033 => null;

		public static TLocalizedText TEquipmentName107034 => null;

		public static TLocalizedText TEquipmentDesc107034 => null;

		public static TLocalizedText TEquipmentName107035 => null;

		public static TLocalizedText TEquipmentDesc107035 => null;

		public static TLocalizedText TEquipmentName107041 => null;

		public static TLocalizedText TEquipmentDesc107041 => null;

		public static TLocalizedText TEquipmentName107043 => null;

		public static TLocalizedText TEquipmentDesc107043 => null;

		public static TLocalizedText TEquipmentName107044 => null;

		public static TLocalizedText TEquipmentDesc107044 => null;

		public static TLocalizedText TEquipmentName107045 => null;

		public static TLocalizedText TEquipmentDesc107045 => null;

		public static TLocalizedText TEquipmentName107051 => null;

		public static TLocalizedText TEquipmentDesc107051 => null;

		public static TLocalizedText TEquipmentName107053 => null;

		public static TLocalizedText TEquipmentDesc107053 => null;

		public static TLocalizedText TEquipmentName107054 => null;

		public static TLocalizedText TEquipmentDesc107054 => null;

		public static TLocalizedText TEquipmentName107055 => null;

		public static TLocalizedText TEquipmentDesc107055 => null;

		public static TLocalizedText TEquipmentName107061 => null;

		public static TLocalizedText TEquipmentDesc107061 => null;

		public static TLocalizedText TEquipmentName107063 => null;

		public static TLocalizedText TEquipmentDesc107063 => null;

		public static TLocalizedText TEquipmentName107064 => null;

		public static TLocalizedText TEquipmentDesc107064 => null;

		public static TLocalizedText TEquipmentName107065 => null;

		public static TLocalizedText TEquipmentDesc107065 => null;

		public static TLocalizedText TEquipmentName107071 => null;

		public static TLocalizedText TEquipmentDesc107071 => null;

		public static TLocalizedText TEquipmentName107073 => null;

		public static TLocalizedText TEquipmentDesc107073 => null;

		public static TLocalizedText TEquipmentName107074 => null;

		public static TLocalizedText TEquipmentDesc107074 => null;

		public static TLocalizedText TEquipmentName107075 => null;

		public static TLocalizedText TEquipmentDesc107075 => null;

		public static TLocalizedText TEquipmentName107081 => null;

		public static TLocalizedText TEquipmentDesc107081 => null;

		public static TLocalizedText TEquipmentName107083 => null;

		public static TLocalizedText TEquipmentDesc107083 => null;

		public static TLocalizedText TEquipmentName108014 => null;

		public static TLocalizedText TEquipmentDesc108014 => null;

		public static TLocalizedText TEquipmentName108015 => null;

		public static TLocalizedText TEquipmentDesc108015 => null;

		public static TLocalizedText TEquipmentName108021 => null;

		public static TLocalizedText TEquipmentDesc108021 => null;

		public static TLocalizedText TEquipmentName108023 => null;

		public static TLocalizedText TEquipmentDesc108023 => null;

		public static TLocalizedText TEquipmentName108024 => null;

		public static TLocalizedText TEquipmentDesc108024 => null;

		public static TLocalizedText TEquipmentName108025 => null;

		public static TLocalizedText TEquipmentDesc108025 => null;

		public static TLocalizedText TEquipmentName108031 => null;

		public static TLocalizedText TEquipmentDesc108031 => null;

		public static TLocalizedText TEquipmentName108033 => null;

		public static TLocalizedText TEquipmentDesc108033 => null;

		public static TLocalizedText TEquipmentName108034 => null;

		public static TLocalizedText TEquipmentDesc108034 => null;

		public static TLocalizedText TEquipmentName108035 => null;

		public static TLocalizedText TEquipmentDesc108035 => null;

		public static TLocalizedText TEquipmentName108041 => null;

		public static TLocalizedText TEquipmentDesc108041 => null;

		public static TLocalizedText TEquipmentName108043 => null;

		public static TLocalizedText TEquipmentDesc108043 => null;

		public static TLocalizedText TEquipmentName108044 => null;

		public static TLocalizedText TEquipmentDesc108044 => null;

		public static TLocalizedText TEquipmentName108045 => null;

		public static TLocalizedText TEquipmentDesc108045 => null;

		public static TLocalizedText TEquipmentName108051 => null;

		public static TLocalizedText TEquipmentDesc108051 => null;

		public static TLocalizedText TEquipmentName108053 => null;

		public static TLocalizedText TEquipmentDesc108053 => null;

		public static TLocalizedText TEquipmentName108054 => null;

		public static TLocalizedText TEquipmentDesc108054 => null;

		public static TLocalizedText TEquipmentName108055 => null;

		public static TLocalizedText TEquipmentDesc108055 => null;

		public static TLocalizedText TEquipmentName108061 => null;

		public static TLocalizedText TEquipmentDesc108061 => null;

		public static TLocalizedText TEquipmentName108063 => null;

		public static TLocalizedText TEquipmentDesc108063 => null;

		public static TLocalizedText TEquipmentName108064 => null;

		public static TLocalizedText TEquipmentDesc108064 => null;

		public static TLocalizedText TEquipmentName108065 => null;

		public static TLocalizedText TEquipmentDesc108065 => null;

		public static TLocalizedText TEquipmentName108071 => null;

		public static TLocalizedText TEquipmentDesc108071 => null;

		public static TLocalizedText TEquipmentName108073 => null;

		public static TLocalizedText TEquipmentDesc108073 => null;

		public static TLocalizedText TEquipmentName108074 => null;

		public static TLocalizedText TEquipmentDesc108074 => null;

		public static TLocalizedText TEquipmentName108075 => null;

		public static TLocalizedText TEquipmentDesc108075 => null;

		public static TLocalizedText TEquipmentName108081 => null;

		public static TLocalizedText TEquipmentDesc108081 => null;

		public static TLocalizedText TEquipmentName108083 => null;

		public static TLocalizedText TEquipmentDesc108083 => null;

		public static TLocalizedText TEquipmentName109011 => null;

		public static TLocalizedText TEquipmentDesc109011 => null;

		public static TLocalizedText TEquipmentName109013 => null;

		public static TLocalizedText TEquipmentDesc109013 => null;

		public static TLocalizedText TEquipmentName109014 => null;

		public static TLocalizedText TEquipmentDesc109014 => null;

		public static TLocalizedText TEquipmentName109015 => null;

		public static TLocalizedText TEquipmentDesc109015 => null;

		public static TLocalizedText TEquipmentName109024 => null;

		public static TLocalizedText TEquipmentDesc109024 => null;

		public static TLocalizedText TEquipmentName109025 => null;

		public static TLocalizedText TEquipmentDesc109025 => null;

		public static TLocalizedText TEquipmentName109031 => null;

		public static TLocalizedText TEquipmentDesc109031 => null;

		public static TLocalizedText TEquipmentName109033 => null;

		public static TLocalizedText TEquipmentDesc109033 => null;

		public static TLocalizedText TEquipmentName109041 => null;

		public static TLocalizedText TEquipmentDesc109041 => null;

		public static TLocalizedText TEquipmentName109043 => null;

		public static TLocalizedText TEquipmentDesc109043 => null;

		public static TLocalizedText TEquipmentName109044 => null;

		public static TLocalizedText TEquipmentDesc109044 => null;

		public static TLocalizedText TEquipmentName109045 => null;

		public static TLocalizedText TEquipmentDesc109045 => null;

		public static TLocalizedText TEquipmentName109051 => null;

		public static TLocalizedText TEquipmentDesc109051 => null;

		public static TLocalizedText TEquipmentName109053 => null;

		public static TLocalizedText TEquipmentDesc109053 => null;

		public static TLocalizedText TEquipmentName109054 => null;

		public static TLocalizedText TEquipmentDesc109054 => null;

		public static TLocalizedText TEquipmentName109055 => null;

		public static TLocalizedText TEquipmentDesc109055 => null;

		public static TLocalizedText TEquipmentName109061 => null;

		public static TLocalizedText TEquipmentDesc109061 => null;

		public static TLocalizedText TEquipmentName109063 => null;

		public static TLocalizedText TEquipmentDesc109063 => null;

		public static TLocalizedText TEquipmentName109064 => null;

		public static TLocalizedText TEquipmentDesc109064 => null;

		public static TLocalizedText TEquipmentName109065 => null;

		public static TLocalizedText TEquipmentDesc109065 => null;

		public static TLocalizedText TEquipmentName109071 => null;

		public static TLocalizedText TEquipmentDesc109071 => null;

		public static TLocalizedText TEquipmentName109073 => null;

		public static TLocalizedText TEquipmentDesc109073 => null;

		public static TLocalizedText TEquipmentName109074 => null;

		public static TLocalizedText TEquipmentDesc109074 => null;

		public static TLocalizedText TEquipmentName109075 => null;

		public static TLocalizedText TEquipmentDesc109075 => null;

		public static TLocalizedText TEquipmentName109091 => null;

		public static TLocalizedText TEquipmentDesc109091 => null;

		public static TLocalizedText TEquipmentName109093 => null;

		public static TLocalizedText TEquipmentDesc109093 => null;

		public static TLocalizedText TEquipmentName109094 => null;

		public static TLocalizedText TEquipmentDesc109094 => null;

		public static TLocalizedText TEquipmentName109095 => null;

		public static TLocalizedText TEquipmentDesc109095 => null;

		public static TLocalizedText TEquipmentName110014 => null;

		public static TLocalizedText TEquipmentDesc110014 => null;

		public static TLocalizedText TEquipmentName110015 => null;

		public static TLocalizedText TEquipmentDesc110015 => null;

		public static TLocalizedText TEquipmentName110024 => null;

		public static TLocalizedText TEquipmentDesc110024 => null;

		public static TLocalizedText TEquipmentName110025 => null;

		public static TLocalizedText TEquipmentDesc110025 => null;

		public static TLocalizedText TEquipmentName110031 => null;

		public static TLocalizedText TEquipmentDesc110031 => null;

		public static TLocalizedText TEquipmentName110033 => null;

		public static TLocalizedText TEquipmentDesc110033 => null;

		public static TLocalizedText TEquipmentName110034 => null;

		public static TLocalizedText TEquipmentDesc110034 => null;

		public static TLocalizedText TEquipmentName110035 => null;

		public static TLocalizedText TEquipmentDesc110035 => null;

		public static TLocalizedText TEquipmentName110041 => null;

		public static TLocalizedText TEquipmentDesc110041 => null;

		public static TLocalizedText TEquipmentName110043 => null;

		public static TLocalizedText TEquipmentDesc110043 => null;

		public static TLocalizedText TEquipmentName110044 => null;

		public static TLocalizedText TEquipmentDesc110044 => null;

		public static TLocalizedText TEquipmentName110045 => null;

		public static TLocalizedText TEquipmentDesc110045 => null;

		public static TLocalizedText TEquipmentName110051 => null;

		public static TLocalizedText TEquipmentDesc110051 => null;

		public static TLocalizedText TEquipmentName110053 => null;

		public static TLocalizedText TEquipmentDesc110053 => null;

		public static TLocalizedText TEquipmentName110054 => null;

		public static TLocalizedText TEquipmentDesc110054 => null;

		public static TLocalizedText TEquipmentName110055 => null;

		public static TLocalizedText TEquipmentDesc110055 => null;

		public static TLocalizedText TEquipmentName110064 => null;

		public static TLocalizedText TEquipmentDesc110064 => null;

		public static TLocalizedText TEquipmentName110065 => null;

		public static TLocalizedText TEquipmentDesc110065 => null;

		public static TLocalizedText TEquipmentName110071 => null;

		public static TLocalizedText TEquipmentDesc110071 => null;

		public static TLocalizedText TEquipmentName110073 => null;

		public static TLocalizedText TEquipmentDesc110073 => null;

		public static TLocalizedText TEquipmentName110081 => null;

		public static TLocalizedText TEquipmentDesc110081 => null;

		public static TLocalizedText TEquipmentName110083 => null;

		public static TLocalizedText TEquipmentDesc110083 => null;

		public static TLocalizedText TEquipmentName110084 => null;

		public static TLocalizedText TEquipmentDesc110084 => null;

		public static TLocalizedText TEquipmentName110085 => null;

		public static TLocalizedText TEquipmentDesc110085 => null;

		public static TLocalizedText TEquipmentName110091 => null;

		public static TLocalizedText TEquipmentDesc110091 => null;

		public static TLocalizedText TEquipmentName110093 => null;

		public static TLocalizedText TEquipmentDesc110093 => null;

		public static TLocalizedText TEquipmentName110094 => null;

		public static TLocalizedText TEquipmentDesc110094 => null;

		public static TLocalizedText TEquipmentName110095 => null;

		public static TLocalizedText TEquipmentDesc110095 => null;

		public static TLocalizedText TEquipmentName110101 => null;

		public static TLocalizedText TEquipmentDesc110101 => null;

		public static TLocalizedText TEquipmentName110103 => null;

		public static TLocalizedText TEquipmentDesc110103 => null;

		public static TLocalizedText TEquipmentCraftingCategory1001 => null;

		public static TLocalizedText TEquipmentCraftingCategory1002 => null;

		public static TLocalizedText TEquipmentCraftingCategory1005 => null;

		public static TLocalizedText TEquipmentCraftingCategory1006 => null;

		public static TLocalizedText TEquipmentCraftingCategory1007 => null;

		public static TLocalizedText TEquipmentCraftingCategory1008 => null;

		public static TLocalizedText TEquipmentCraftingCategory1009 => null;

		public static TLocalizedText TEquipmentCraftingCategory1010 => null;

		public static TLocalizedText TEquipmentCraftingCategory1011 => null;

		public static TLocalizedText TEquipmentCraftingCategory1012 => null;

		public static TLocalizedText TEquipmentCraftingCategory1013 => null;

		public static TLocalizedText TEquipmentCraftingCategory1014 => null;

		public static TLocalizedText TEquipmentCraftingCategory1015 => null;

		public static TLocalizedText TEquipmentCraftingCategory1016 => null;

		public static TLocalizedText TEquipmentCraftingCategory1101 => null;

		public static TLocalizedText TEquipmentCraftingCategory1102 => null;

		public static TLocalizedText TEquipmentCraftingCategory1105 => null;

		public static TLocalizedText TEquipmentCraftingCategory1106 => null;

		public static TLocalizedText TEquipmentCraftingCategory1107 => null;

		public static TLocalizedText TEquipmentCraftingCategory1108 => null;

		public static TLocalizedText TEquipmentCraftingCategory1109 => null;

		public static TLocalizedText TEquipmentCraftingCategory1110 => null;

		public static TLocalizedText TEquipmentCraftingCategory1111 => null;

		public static TLocalizedText TEquipmentCraftingCategory1112 => null;

		public static TLocalizedText TEquipmentCraftingCategory1113 => null;

		public static TLocalizedText TEquipmentCraftingCategory1114 => null;

		public static TLocalizedText TEquipmentCraftingCategory1115 => null;

		public static TLocalizedText TEquipmentCraftingCategory1116 => null;

		public static TLocalizedText TEquipmentCraftingCategory1201 => null;

		public static TLocalizedText TEquipmentCraftingCategory1202 => null;

		public static TLocalizedText TEquipmentCraftingCategory1205 => null;

		public static TLocalizedText TEquipmentCraftingCategory1206 => null;

		public static TLocalizedText TEquipmentCraftingCategory1207 => null;

		public static TLocalizedText TEquipmentCraftingCategory1208 => null;

		public static TLocalizedText TEquipmentCraftingCategory1209 => null;

		public static TLocalizedText TEquipmentCraftingCategory1210 => null;

		public static TLocalizedText TEquipmentCraftingCategory1211 => null;

		public static TLocalizedText TEquipmentCraftingCategory1212 => null;

		public static TLocalizedText TEquipmentCraftingCategory1213 => null;

		public static TLocalizedText TEquipmentCraftingCategory1214 => null;

		public static TLocalizedText TEquipmentCraftingCategory1215 => null;

		public static TLocalizedText TEquipmentCraftingCategory1216 => null;

		public static TLocalizedText TEquipmentCraftingCategory20011 => null;

		public static TLocalizedText TEquipmentCraftingCategory20012 => null;

		public static TLocalizedText TEquipmentCraftingCategory20013 => null;

		public static TLocalizedText TEquipmentCraftingCategory20014 => null;

		public static TLocalizedText TEquipmentCraftingCategory20015 => null;

		public static TLocalizedText TEquipmentCraftingCategory20021 => null;

		public static TLocalizedText TEquipmentCraftingCategory20022 => null;

		public static TLocalizedText TEquipmentCraftingCategory20023 => null;

		public static TLocalizedText TEquipmentCraftingCategory20024 => null;

		public static TLocalizedText TEquipmentCraftingCategory20025 => null;

		public static TLocalizedText TEquipmentCraftingCategory20031 => null;

		public static TLocalizedText TEquipmentCraftingCategory20032 => null;

		public static TLocalizedText TEquipmentCraftingCategory20033 => null;

		public static TLocalizedText TEquipmentCraftingCategory20034 => null;

		public static TLocalizedText TEquipmentCraftingCategory20035 => null;

		public static TLocalizedText TEquipmentCraftingCategory20041 => null;

		public static TLocalizedText TEquipmentCraftingCategory20042 => null;

		public static TLocalizedText TEquipmentCraftingCategory20043 => null;

		public static TLocalizedText TEquipmentCraftingCategory20051 => null;

		public static TLocalizedText TEquipmentCraftingCategory20052 => null;

		public static TLocalizedText TEquipmentCraftingCategory20053 => null;

		public static TLocalizedText TEquipmentCraftingCategory20061 => null;

		public static TLocalizedText TEquipmentCraftingCategory20062 => null;

		public static TLocalizedText TEquipmentCraftingCategory20063 => null;

		public static TLocalizedText TEquipmentCraftingCategory21011 => null;

		public static TLocalizedText TEquipmentCraftingCategory21012 => null;

		public static TLocalizedText TEquipmentCraftingCategory21013 => null;

		public static TLocalizedText TEquipmentCraftingCategory21014 => null;

		public static TLocalizedText TEquipmentCraftingCategory21015 => null;

		public static TLocalizedText TEquipmentCraftingCategory21021 => null;

		public static TLocalizedText TEquipmentCraftingCategory21022 => null;

		public static TLocalizedText TEquipmentCraftingCategory21023 => null;

		public static TLocalizedText TEquipmentCraftingCategory21024 => null;

		public static TLocalizedText TEquipmentCraftingCategory21025 => null;

		public static TLocalizedText TEquipmentCraftingCategory21031 => null;

		public static TLocalizedText TEquipmentCraftingCategory21032 => null;

		public static TLocalizedText TEquipmentCraftingCategory21041 => null;

		public static TLocalizedText TEquipmentCraftingCategory21042 => null;

		public static TLocalizedText TEquipmentCraftingCategory21043 => null;

		public static TLocalizedText TEquipmentCraftingCategory22011 => null;

		public static TLocalizedText TEquipmentCraftingCategory22012 => null;

		public static TLocalizedText TEquipmentCraftingCategory22013 => null;

		public static TLocalizedText TEquipmentCraftingCategory22014 => null;

		public static TLocalizedText TEquipmentCraftingCategory22015 => null;

		public static TLocalizedText TEquipmentCraftingCategory23011 => null;

		public static TLocalizedText TEquipmentCraftingCategory23012 => null;

		public static TLocalizedText TEquipmentCraftingCategory23013 => null;

		public static TLocalizedText TEquipmentCraftingCategory23071 => null;

		public static TLocalizedText TEquipmentCraftingCategory23072 => null;

		public static TLocalizedText TEquipmentCraftingCategory23073 => null;

		public static TLocalizedText TEquipmentCraftingCategory23081 => null;

		public static TLocalizedText TEquipmentCraftingCategory23082 => null;

		public static TLocalizedText TEquipmentCraftingCategory23083 => null;

		public static TLocalizedText TEquipmentCraftingCategory23091 => null;

		public static TLocalizedText TEquipmentCraftingCategory23092 => null;

		public static TLocalizedText TEquipmentCraftingCategory23093 => null;

		public static TLocalizedText TEquipmentCraftingCategory23101 => null;

		public static TLocalizedText TEquipmentCraftingCategory23102 => null;

		public static TLocalizedText TEquipmentCraftingCategory23103 => null;

		public static TLocalizedText TEquipmentCraftingCategory23111 => null;

		public static TLocalizedText TEquipmentCraftingCategory23112 => null;

		public static TLocalizedText TEquipmentCraftingCategory23113 => null;

		public static TLocalizedText TEquipmentCraftingCategory23121 => null;

		public static TLocalizedText TEquipmentCraftingCategory23122 => null;

		public static TLocalizedText TEquipmentCraftingCategory23123 => null;

		public static TLocalizedText TEquipmentCraftingCategory24051 => null;

		public static TLocalizedText TEquipmentCraftingCategory24052 => null;

		public static TLocalizedText TEquipmentCraftingCategory24053 => null;

		public static TLocalizedText TEquipmentCraftingCategory24054 => null;

		public static TLocalizedText TEquipmentCraftingCategory24055 => null;

		public static TLocalizedText TEquipmentCraftingCategory25011 => null;

		public static TLocalizedText TEquipmentCraftingCategory25012 => null;

		public static TLocalizedText TEquipmentCraftingCategory25013 => null;

		public static TLocalizedText TEquipmentCraftingCategory25021 => null;

		public static TLocalizedText TEquipmentCraftingCategory25022 => null;

		public static TLocalizedText TEquipmentCraftingCategory25023 => null;

		public static TLocalizedText TEquipmentCraftingCategory25031 => null;

		public static TLocalizedText TEquipmentCraftingCategory25032 => null;

		public static TLocalizedText TEquipmentCraftingCategory25033 => null;

		public static TLocalizedText TEquipmentCraftingCategory25041 => null;

		public static TLocalizedText TEquipmentCraftingCategory25042 => null;

		public static TLocalizedText TEquipmentCraftingCategory25043 => null;

		public static TLocalizedText TEquipmentCraftingCategory25051 => null;

		public static TLocalizedText TEquipmentCraftingCategory25052 => null;

		public static TLocalizedText TEquipmentCraftingCategory25053 => null;

		public static TLocalizedText TEquipmentCraftingCategory25071 => null;

		public static TLocalizedText TEquipmentCraftingCategory25072 => null;

		public static TLocalizedText TEquipmentCraftingCategory25073 => null;

		public static TLocalizedText TEquipmentCraftingCategory26011 => null;

		public static TLocalizedText TEquipmentCraftingCategory26022 => null;

		public static TLocalizedText TEquipmentCraftingCategory26031 => null;

		public static TLocalizedText TEquipmentCraftingCategory26042 => null;

		public static TLocalizedText TEquipmentCraftingCategory26051 => null;

		public static TLocalizedText TEquipmentCraftingCategory26062 => null;

		public static TLocalizedText TEquipmentCraftingCategory26071 => null;

		public static TLocalizedText TEquipmentCraftingCategory26081 => null;

		public static TLocalizedText TEquipmentCraftingCategory26082 => null;

		public static TLocalizedText TEquipmentEffectName1 => null;

		public static TLocalizedText TEquipmentEffectName2 => null;

		public static TLocalizedText TEquipmentEffectName3 => null;

		public static TLocalizedText TEquipmentEffectName4 => null;

		public static TLocalizedText TEquipmentEffectName5 => null;

		public static TLocalizedText TEquipmentEffectDesc101 => null;

		public static TLocalizedText TEquipmentEffectDesc102 => null;

		public static TLocalizedText TEquipmentEffectDesc103 => null;

		public static TLocalizedText TEquipmentEffectDesc104 => null;

		public static TLocalizedText TEquipmentEffectDesc201 => null;

		public static TLocalizedText TEquipmentEffectDesc202 => null;

		public static TLocalizedText TEquipmentEffectDesc203 => null;

		public static TLocalizedText TEquipmentEffectDesc301 => null;

		public static TLocalizedText TEquipmentEffectDesc302 => null;

		public static TLocalizedText TEquipmentEffectDesc303 => null;

		public static TLocalizedText TEquipmentEffectDesc401 => null;

		public static TLocalizedText TEquipmentEffectDesc402 => null;

		public static TLocalizedText TEquipmentEffectDesc403 => null;

		public static TLocalizedText TEquipmentEffectDesc501 => null;

		public static TLocalizedText TEquipmentEffectDesc502 => null;

		public static TLocalizedText TEquipmentEffectDesc503 => null;

		public static TLocalizedText TEquipmentEffectName20001 => null;

		public static TLocalizedText TEquipmentEffectDesc20001 => null;

		public static TLocalizedText TEquipmentEffectName20002 => null;

		public static TLocalizedText TEquipmentEffectDesc20002 => null;

		public static TLocalizedText TEquipmentEffectName20003 => null;

		public static TLocalizedText TEquipmentEffectDesc20003 => null;

		public static TLocalizedText TEquipmentEffectName20004 => null;

		public static TLocalizedText TEquipmentEffectDesc20004 => null;

		public static TLocalizedText TEquipmentEffectName20005 => null;

		public static TLocalizedText TEquipmentEffectDesc20005 => null;

		public static TLocalizedText TEquipmentEffectName20011 => null;

		public static TLocalizedText TEquipmentEffectDesc20011 => null;

		public static TLocalizedText TEquipmentEffectName20012 => null;

		public static TLocalizedText TEquipmentEffectDesc20012 => null;

		public static TLocalizedText TEquipmentEffectName20013 => null;

		public static TLocalizedText TEquipmentEffectDesc20013 => null;

		public static TLocalizedText TEquipmentEffectName20014 => null;

		public static TLocalizedText TEquipmentEffectDesc20014 => null;

		public static TLocalizedText TEquipmentEffectName20015 => null;

		public static TLocalizedText TEquipmentEffectDesc20015 => null;

		public static TLocalizedText TEquipmentEffectName20021 => null;

		public static TLocalizedText TEquipmentEffectDesc20021 => null;

		public static TLocalizedText TEquipmentEffectName20022 => null;

		public static TLocalizedText TEquipmentEffectDesc20022 => null;

		public static TLocalizedText TEquipmentEffectName20023 => null;

		public static TLocalizedText TEquipmentEffectDesc20023 => null;

		public static TLocalizedText TEquipmentEffectName20024 => null;

		public static TLocalizedText TEquipmentEffectDesc20024 => null;

		public static TLocalizedText TEquipmentEffectName20025 => null;

		public static TLocalizedText TEquipmentEffectDesc20025 => null;

		public static TLocalizedText TEquipmentEffectName20031 => null;

		public static TLocalizedText TEquipmentEffectDesc20031 => null;

		public static TLocalizedText TEquipmentEffectName20032 => null;

		public static TLocalizedText TEquipmentEffectDesc20032 => null;

		public static TLocalizedText TEquipmentEffectName20033 => null;

		public static TLocalizedText TEquipmentEffectDesc20033 => null;

		public static TLocalizedText TEquipmentEffectName20034 => null;

		public static TLocalizedText TEquipmentEffectDesc20034 => null;

		public static TLocalizedText TEquipmentEffectName20035 => null;

		public static TLocalizedText TEquipmentEffectDesc20035 => null;

		public static TLocalizedText TEquipmentEffectName20041 => null;

		public static TLocalizedText TEquipmentEffectDesc20041 => null;

		public static TLocalizedText TEquipmentEffectName20042 => null;

		public static TLocalizedText TEquipmentEffectDesc20042 => null;

		public static TLocalizedText TEquipmentEffectName20051 => null;

		public static TLocalizedText TEquipmentEffectDesc20051 => null;

		public static TLocalizedText TEquipmentEffectName20052 => null;

		public static TLocalizedText TEquipmentEffectDesc20052 => null;

		public static TLocalizedText TEquipmentEffectName20053 => null;

		public static TLocalizedText TEquipmentEffectDesc20053 => null;

		public static TLocalizedText TEquipmentEffectName20061 => null;

		public static TLocalizedText TEquipmentEffectDesc20061 => null;

		public static TLocalizedText TEquipmentEffectName20062 => null;

		public static TLocalizedText TEquipmentEffectDesc20062 => null;

		public static TLocalizedText TEquipmentEffectName20063 => null;

		public static TLocalizedText TEquipmentEffectDesc20063 => null;

		public static TLocalizedText TEquipmentEffectName20064 => null;

		public static TLocalizedText TEquipmentEffectDesc20064 => null;

		public static TLocalizedText TEquipmentEffectName20065 => null;

		public static TLocalizedText TEquipmentEffectDesc20065 => null;

		public static TLocalizedText TEquipmentEffectName20071 => null;

		public static TLocalizedText TEquipmentEffectDesc20071 => null;

		public static TLocalizedText TEquipmentEffectName20072 => null;

		public static TLocalizedText TEquipmentEffectDesc20072 => null;

		public static TLocalizedText TEquipmentEffectName20073 => null;

		public static TLocalizedText TEquipmentEffectDesc20073 => null;

		public static TLocalizedText TEquipmentEffectName20074 => null;

		public static TLocalizedText TEquipmentEffectDesc20074 => null;

		public static TLocalizedText TEquipmentEffectName20075 => null;

		public static TLocalizedText TEquipmentEffectDesc20075 => null;

		public static TLocalizedText TEquipmentEffectName20081 => null;

		public static TLocalizedText TEquipmentEffectDesc20081 => null;

		public static TLocalizedText TEquipmentEffectName20082 => null;

		public static TLocalizedText TEquipmentEffectDesc20082 => null;

		public static TLocalizedText TEquipmentEffectName20083 => null;

		public static TLocalizedText TEquipmentEffectDesc20083 => null;

		public static TLocalizedText TEquipmentEffectName20091 => null;

		public static TLocalizedText TEquipmentEffectDesc20091 => null;

		public static TLocalizedText TEquipmentEffectName20092 => null;

		public static TLocalizedText TEquipmentEffectDesc20092 => null;

		public static TLocalizedText TEquipmentEffectName20093 => null;

		public static TLocalizedText TEquipmentEffectDesc20093 => null;

		public static TLocalizedText TEquipmentEffectName20101 => null;

		public static TLocalizedText TEquipmentEffectDesc20101 => null;

		public static TLocalizedText TEquipmentEffectName20102 => null;

		public static TLocalizedText TEquipmentEffectDesc20102 => null;

		public static TLocalizedText TEquipmentEffectName20103 => null;

		public static TLocalizedText TEquipmentEffectDesc20103 => null;

		public static TLocalizedText TEquipmentEffectName20111 => null;

		public static TLocalizedText TEquipmentEffectDesc20111 => null;

		public static TLocalizedText TEquipmentEffectName20112 => null;

		public static TLocalizedText TEquipmentEffectDesc20112 => null;

		public static TLocalizedText TEquipmentEffectName20113 => null;

		public static TLocalizedText TEquipmentEffectDesc20113 => null;

		public static TLocalizedText TEquipmentEffectName20121 => null;

		public static TLocalizedText TEquipmentEffectDesc20121 => null;

		public static TLocalizedText TEquipmentEffectName20122 => null;

		public static TLocalizedText TEquipmentEffectDesc20122 => null;

		public static TLocalizedText TEquipmentEffectName20123 => null;

		public static TLocalizedText TEquipmentEffectDesc20123 => null;

		public static TLocalizedText TEquipmentEffectName20131 => null;

		public static TLocalizedText TEquipmentEffectDesc20131 => null;

		public static TLocalizedText TEquipmentEffectName20132 => null;

		public static TLocalizedText TEquipmentEffectDesc20132 => null;

		public static TLocalizedText TEquipmentEffectName20133 => null;

		public static TLocalizedText TEquipmentEffectDesc20133 => null;

		public static TLocalizedText TEquipmentEffectName20141 => null;

		public static TLocalizedText TEquipmentEffectDesc20141 => null;

		public static TLocalizedText TEquipmentEffectName20142 => null;

		public static TLocalizedText TEquipmentEffectDesc20142 => null;

		public static TLocalizedText TEquipmentEffectName20143 => null;

		public static TLocalizedText TEquipmentEffectDesc20143 => null;

		public static TLocalizedText TEquipmentEffectName20151 => null;

		public static TLocalizedText TEquipmentEffectDesc20151 => null;

		public static TLocalizedText TEquipmentEffectName20152 => null;

		public static TLocalizedText TEquipmentEffectDesc20152 => null;

		public static TLocalizedText TEquipmentEffectName20153 => null;

		public static TLocalizedText TEquipmentEffectDesc20153 => null;

		public static TLocalizedText TEquipmentEffectName20161 => null;

		public static TLocalizedText TEquipmentEffectDesc20161 => null;

		public static TLocalizedText TEquipmentEffectName20162 => null;

		public static TLocalizedText TEquipmentEffectDesc20162 => null;

		public static TLocalizedText TEquipmentEffectName20163 => null;

		public static TLocalizedText TEquipmentEffectDesc20163 => null;

		public static TLocalizedText TEquipmentEffectName20171 => null;

		public static TLocalizedText TEquipmentEffectDesc20171 => null;

		public static TLocalizedText TEquipmentEffectName20172 => null;

		public static TLocalizedText TEquipmentEffectDesc20172 => null;

		public static TLocalizedText TEquipmentEffectName20173 => null;

		public static TLocalizedText TEquipmentEffectDesc20173 => null;

		public static TLocalizedText TEquipmentEffectName20181 => null;

		public static TLocalizedText TEquipmentEffectDesc20181 => null;

		public static TLocalizedText TEquipmentEffectName20182 => null;

		public static TLocalizedText TEquipmentEffectDesc20182 => null;

		public static TLocalizedText TEquipmentEffectName20183 => null;

		public static TLocalizedText TEquipmentEffectDesc20183 => null;

		public static TLocalizedText TEquipmentEffectName20191 => null;

		public static TLocalizedText TEquipmentEffectDesc20191 => null;

		public static TLocalizedText TEquipmentEffectName20192 => null;

		public static TLocalizedText TEquipmentEffectDesc20192 => null;

		public static TLocalizedText TEquipmentEffectName20193 => null;

		public static TLocalizedText TEquipmentEffectDesc20193 => null;

		public static TLocalizedText TEquipmentEffectName20201 => null;

		public static TLocalizedText TEquipmentEffectDesc20201 => null;

		public static TLocalizedText TEquipmentEffectName20202 => null;

		public static TLocalizedText TEquipmentEffectDesc20202 => null;

		public static TLocalizedText TEquipmentEffectName20203 => null;

		public static TLocalizedText TEquipmentEffectDesc20203 => null;

		public static TLocalizedText TEquipmentEffectName20211 => null;

		public static TLocalizedText TEquipmentEffectDesc20211 => null;

		public static TLocalizedText TEquipmentEffectName20212 => null;

		public static TLocalizedText TEquipmentEffectDesc20212 => null;

		public static TLocalizedText TEquipmentEffectName20213 => null;

		public static TLocalizedText TEquipmentEffectDesc20213 => null;

		public static TLocalizedText TEquipmentEffectName20221 => null;

		public static TLocalizedText TEquipmentEffectDesc20221 => null;

		public static TLocalizedText TEquipmentEffectName20222 => null;

		public static TLocalizedText TEquipmentEffectDesc20222 => null;

		public static TLocalizedText TEquipmentEffectName20223 => null;

		public static TLocalizedText TEquipmentEffectDesc20223 => null;

		public static TLocalizedText TEquipmentEffectName20231 => null;

		public static TLocalizedText TEquipmentEffectDesc20231 => null;

		public static TLocalizedText TEquipmentEffectName20232 => null;

		public static TLocalizedText TEquipmentEffectDesc20232 => null;

		public static TLocalizedText TEquipmentEffectName20233 => null;

		public static TLocalizedText TEquipmentEffectDesc20233 => null;

		public static TLocalizedText TEquipmentEffectName20241 => null;

		public static TLocalizedText TEquipmentEffectDesc20241 => null;

		public static TLocalizedText TEquipmentEffectName20242 => null;

		public static TLocalizedText TEquipmentEffectDesc20242 => null;

		public static TLocalizedText TEquipmentEffectName20243 => null;

		public static TLocalizedText TEquipmentEffectDesc20243 => null;

		public static TLocalizedText TEquipmentEffectName20251 => null;

		public static TLocalizedText TEquipmentEffectDesc20251 => null;

		public static TLocalizedText TEquipmentEffectName20252 => null;

		public static TLocalizedText TEquipmentEffectDesc20252 => null;

		public static TLocalizedText TEquipmentEffectName20253 => null;

		public static TLocalizedText TEquipmentEffectDesc20253 => null;

		public static TLocalizedText TEquipmentEffectName20254 => null;

		public static TLocalizedText TEquipmentEffectDesc20254 => null;

		public static TLocalizedText TEquipmentEffectName20255 => null;

		public static TLocalizedText TEquipmentEffectDesc20255 => null;

		public static TLocalizedText TEquipmentEffectName20261 => null;

		public static TLocalizedText TEquipmentEffectDesc20261 => null;

		public static TLocalizedText TEquipmentEffectName20262 => null;

		public static TLocalizedText TEquipmentEffectDesc20262 => null;

		public static TLocalizedText TEquipmentEffectName20263 => null;

		public static TLocalizedText TEquipmentEffectDesc20263 => null;

		public static TLocalizedText TEquipmentEffectName20271 => null;

		public static TLocalizedText TEquipmentEffectDesc20271 => null;

		public static TLocalizedText TEquipmentEffectName20272 => null;

		public static TLocalizedText TEquipmentEffectDesc20272 => null;

		public static TLocalizedText TEquipmentEffectName20273 => null;

		public static TLocalizedText TEquipmentEffectDesc20273 => null;

		public static TLocalizedText TEquipmentEffectName20281 => null;

		public static TLocalizedText TEquipmentEffectDesc20281 => null;

		public static TLocalizedText TEquipmentEffectName20282 => null;

		public static TLocalizedText TEquipmentEffectDesc20282 => null;

		public static TLocalizedText TEquipmentEffectName20283 => null;

		public static TLocalizedText TEquipmentEffectDesc20283 => null;

		public static TLocalizedText TEquipmentEffectName20291 => null;

		public static TLocalizedText TEquipmentEffectDesc20291 => null;

		public static TLocalizedText TEquipmentEffectName20292 => null;

		public static TLocalizedText TEquipmentEffectDesc20292 => null;

		public static TLocalizedText TEquipmentEffectName20293 => null;

		public static TLocalizedText TEquipmentEffectDesc20293 => null;

		public static TLocalizedText TEquipmentEffectName20301 => null;

		public static TLocalizedText TEquipmentEffectDesc20301 => null;

		public static TLocalizedText TEquipmentEffectName20302 => null;

		public static TLocalizedText TEquipmentEffectDesc20302 => null;

		public static TLocalizedText TEquipmentEffectName20303 => null;

		public static TLocalizedText TEquipmentEffectDesc20303 => null;

		public static TLocalizedText TEquipmentEffectName20321 => null;

		public static TLocalizedText TEquipmentEffectDesc20321 => null;

		public static TLocalizedText TEquipmentEffectName20322 => null;

		public static TLocalizedText TEquipmentEffectDesc20322 => null;

		public static TLocalizedText TEquipmentEffectName20323 => null;

		public static TLocalizedText TEquipmentEffectDesc20323 => null;

		public static TLocalizedText TEquipmentEffectName20341 => null;

		public static TLocalizedText TEquipmentEffectDesc20341 => null;

		public static TLocalizedText TEquipmentEffectName20342 => null;

		public static TLocalizedText TEquipmentEffectDesc20342 => null;

		public static TLocalizedText TEquipmentEffectName30001 => null;

		public static TLocalizedText TEquipmentEffectDesc30001 => null;

		public static TLocalizedText TEquipmentEffectName30002 => null;

		public static TLocalizedText TEquipmentEffectDesc30002 => null;

		public static TLocalizedText TEquipmentEffectName30003 => null;

		public static TLocalizedText TEquipmentEffectDesc30003 => null;

		public static TLocalizedText TEquipmentEffectName30011 => null;

		public static TLocalizedText TEquipmentEffectDesc30011 => null;

		public static TLocalizedText TEquipmentEffectName30012 => null;

		public static TLocalizedText TEquipmentEffectDesc30012 => null;

		public static TLocalizedText TEquipmentEffectName30013 => null;

		public static TLocalizedText TEquipmentEffectDesc30013 => null;

		public static TLocalizedText TEquipmentEffectName30021 => null;

		public static TLocalizedText TEquipmentEffectDesc30021 => null;

		public static TLocalizedText TEquipmentEffectName30022 => null;

		public static TLocalizedText TEquipmentEffectDesc30022 => null;

		public static TLocalizedText TEquipmentEffectName30023 => null;

		public static TLocalizedText TEquipmentEffectDesc30023 => null;

		public static TLocalizedText TEquipmentEffectName30031 => null;

		public static TLocalizedText TEquipmentEffectDesc30031 => null;

		public static TLocalizedText TEquipmentEffectName30032 => null;

		public static TLocalizedText TEquipmentEffectDesc30032 => null;

		public static TLocalizedText TEquipmentEffectName30033 => null;

		public static TLocalizedText TEquipmentEffectDesc30033 => null;

		public static TLocalizedText TEquipmentEffectName30041 => null;

		public static TLocalizedText TEquipmentEffectDesc30041 => null;

		public static TLocalizedText TEquipmentEffectName30042 => null;

		public static TLocalizedText TEquipmentEffectDesc30042 => null;

		public static TLocalizedText TEquipmentEffectName30043 => null;

		public static TLocalizedText TEquipmentEffectDesc30043 => null;

		public static TLocalizedText TEquipmentEffectName30051 => null;

		public static TLocalizedText TEquipmentEffectDesc30051 => null;

		public static TLocalizedText TEquipmentEffectName30061 => null;

		public static TLocalizedText TEquipmentEffectDesc30061 => null;

		public static TLocalizedText TEquipmentEffectName30062 => null;

		public static TLocalizedText TEquipmentEffectDesc30062 => null;

		public static TLocalizedText TEquipmentSetName1 => null;

		public static TLocalizedText TEquipmentSetDesc1 => null;

		public static TLocalizedText TEquipmentSetName2 => null;

		public static TLocalizedText TEquipmentSetDesc2 => null;

		public static TLocalizedText TEquipmentSetName3 => null;

		public static TLocalizedText TEquipmentSetDesc3 => null;

		public static TLocalizedText TEquipmentSetDesc101 => null;

		public static TLocalizedText TEquipmentSetDesc102 => null;

		public static TLocalizedText TEquipmentSetDesc201 => null;

		public static TLocalizedText TExtraUnitName10001 => null;

		public static TLocalizedText TExtraUnitDesc10001 => null;

		public static TLocalizedText TExtraUnitName10002 => null;

		public static TLocalizedText TExtraUnitDesc10002 => null;

		public static TLocalizedText TExtraUnitName10003 => null;

		public static TLocalizedText TExtraUnitDesc10003 => null;

		public static TLocalizedText TFacilityName101 => null;

		public static TLocalizedText TFacilityName111 => null;

		public static TLocalizedText TFacilityName121 => null;

		public static TLocalizedText TFacilityName122 => null;

		public static TLocalizedText TFacilityName123 => null;

		public static TLocalizedText TFactionName1001 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue1001 => null;

		public static TLocalizedText TFactionName1002 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue1002 => null;

		public static TLocalizedText TFactionName1100 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue1100 => null;

		public static TLocalizedText TFactionName1101 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue1101 => null;

		public static TLocalizedText TFactionName2001 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue2001 => null;

		public static TLocalizedText TFactionName3001 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue3001 => null;

		public static TLocalizedText TFactionName9999 => null;

		public static TLocalizedText TFactionEnemyFactionMonologue9999 => null;

		public static TLocalizedText TGlossaryName10000 => null;

		public static TLocalizedText TGlossaryDesc10000 => null;

		public static TLocalizedText TGlossaryName10001 => null;

		public static TLocalizedText TGlossaryDesc10001 => null;

		public static TLocalizedText TGlossaryName10002 => null;

		public static TLocalizedText TGlossaryDesc10002 => null;

		public static TLocalizedText TGlossaryName10003 => null;

		public static TLocalizedText TGlossaryDesc10003 => null;

		public static TLocalizedText TGlossaryName10004 => null;

		public static TLocalizedText TGlossaryDesc10004 => null;

		public static TLocalizedText TGlossaryName10005 => null;

		public static TLocalizedText TGlossaryDesc10005 => null;

		public static TLocalizedText TGlossaryName10006 => null;

		public static TLocalizedText TGlossaryDesc10006 => null;

		public static TLocalizedText TGlossaryName10007 => null;

		public static TLocalizedText TGlossaryDesc10007 => null;

		public static TLocalizedText TGlossaryName10008 => null;

		public static TLocalizedText TGlossaryDesc10008 => null;

		public static TLocalizedText TGlossaryName10009 => null;

		public static TLocalizedText TGlossaryDesc10009 => null;

		public static TLocalizedText TGlossaryName10010 => null;

		public static TLocalizedText TGlossaryDesc10010 => null;

		public static TLocalizedText TGlossaryName10011 => null;

		public static TLocalizedText TGlossaryDesc10011 => null;

		public static TLocalizedText TGlossaryName10012 => null;

		public static TLocalizedText TGlossaryDesc10012 => null;

		public static TLocalizedText TGlossaryName10013 => null;

		public static TLocalizedText TGlossaryDesc10013 => null;

		public static TLocalizedText TGlossaryName10014 => null;

		public static TLocalizedText TGlossaryDesc10014 => null;

		public static TLocalizedText TGlossaryName10015 => null;

		public static TLocalizedText TGlossaryDesc10015 => null;

		public static TLocalizedText TGlossaryName10016 => null;

		public static TLocalizedText TGlossaryDesc10016 => null;

		public static TLocalizedText TGlossaryName10017 => null;

		public static TLocalizedText TGlossaryDesc10017 => null;

		public static TLocalizedText TGlossaryName10018 => null;

		public static TLocalizedText TGlossaryDesc10018 => null;

		public static TLocalizedText TGlossaryName10019 => null;

		public static TLocalizedText TGlossaryDesc10019 => null;

		public static TLocalizedText TGlossaryName10020 => null;

		public static TLocalizedText TGlossaryDesc10020 => null;

		public static TLocalizedText TGlossaryName10021 => null;

		public static TLocalizedText TGlossaryDesc10021 => null;

		public static TLocalizedText TGlossaryName10022 => null;

		public static TLocalizedText TGlossaryDesc10022 => null;

		public static TLocalizedText TGlossaryName10023 => null;

		public static TLocalizedText TGlossaryDesc10023 => null;

		public static TLocalizedText TGlossaryName10024 => null;

		public static TLocalizedText TGlossaryDesc10024 => null;

		public static TLocalizedText TGlossaryName10025 => null;

		public static TLocalizedText TGlossaryDesc10025 => null;

		public static TLocalizedText TGlossaryName10026 => null;

		public static TLocalizedText TGlossaryDesc10026 => null;

		public static TLocalizedText TGlossaryName10027 => null;

		public static TLocalizedText TGlossaryDesc10027 => null;

		public static TLocalizedText TGlossaryName10028 => null;

		public static TLocalizedText TGlossaryDesc10028 => null;

		public static TLocalizedText TGlossaryName10029 => null;

		public static TLocalizedText TGlossaryDesc10029 => null;

		public static TLocalizedText TGlossaryName10030 => null;

		public static TLocalizedText TGlossaryDesc10030 => null;

		public static TLocalizedText TGlossaryName10031 => null;

		public static TLocalizedText TGlossaryDesc10031 => null;

		public static TLocalizedText TGlossaryName10032 => null;

		public static TLocalizedText TGlossaryDesc10032 => null;

		public static TLocalizedText TGlossaryName10033 => null;

		public static TLocalizedText TGlossaryDesc10033 => null;

		public static TLocalizedText TGlossaryName10034 => null;

		public static TLocalizedText TGlossaryDesc10034 => null;

		public static TLocalizedText TGlossaryName10035 => null;

		public static TLocalizedText TGlossaryDesc10035 => null;

		public static TLocalizedText TGlossaryName10036 => null;

		public static TLocalizedText TGlossaryDesc10036 => null;

		public static TLocalizedText TGlossaryName10037 => null;

		public static TLocalizedText TGlossaryDesc10037 => null;

		public static TLocalizedText TGlossaryName10038 => null;

		public static TLocalizedText TGlossaryDesc10038 => null;

		public static TLocalizedText TGlossaryName10039 => null;

		public static TLocalizedText TGlossaryDesc10039 => null;

		public static TLocalizedText TGlossaryName10040 => null;

		public static TLocalizedText TGlossaryDesc10040 => null;

		public static TLocalizedText TGlossaryName10041 => null;

		public static TLocalizedText TGlossaryDesc10041 => null;

		public static TLocalizedText TGlossaryName10042 => null;

		public static TLocalizedText TGlossaryDesc10042 => null;

		public static TLocalizedText TGlossaryName10044 => null;

		public static TLocalizedText TGlossaryDesc10044 => null;

		public static TLocalizedText TGlossaryName10045 => null;

		public static TLocalizedText TGlossaryDesc10045 => null;

		public static TLocalizedText TGlossaryName10046 => null;

		public static TLocalizedText TGlossaryDesc10046 => null;

		public static TLocalizedText TGlossaryName10047 => null;

		public static TLocalizedText TGlossaryDesc10047 => null;

		public static TLocalizedText TGlossaryName10048 => null;

		public static TLocalizedText TGlossaryDesc10048 => null;

		public static TLocalizedText TGlossaryName10049 => null;

		public static TLocalizedText TGlossaryDesc10049 => null;

		public static TLocalizedText TGlossaryName20000 => null;

		public static TLocalizedText TGlossaryDesc20000 => null;

		public static TLocalizedText TGlossaryName20001 => null;

		public static TLocalizedText TGlossaryDesc20001 => null;

		public static TLocalizedText TGlossaryName20002 => null;

		public static TLocalizedText TGlossaryDesc20002 => null;

		public static TLocalizedText TGlossaryName20003 => null;

		public static TLocalizedText TGlossaryDesc20003 => null;

		public static TLocalizedText TGlossaryName20004 => null;

		public static TLocalizedText TGlossaryDesc20004 => null;

		public static TLocalizedText TGlossaryName20005 => null;

		public static TLocalizedText TGlossaryDesc20005 => null;

		public static TLocalizedText TGlossaryName20006 => null;

		public static TLocalizedText TGlossaryDesc20006 => null;

		public static TLocalizedText TGlossaryName20007 => null;

		public static TLocalizedText TGlossaryDesc20007 => null;

		public static TLocalizedText TGlossaryName20009 => null;

		public static TLocalizedText TGlossaryDesc20009 => null;

		public static TLocalizedText TGlossaryName20010 => null;

		public static TLocalizedText TGlossaryDesc20010 => null;

		public static TLocalizedText TGlossaryName20011 => null;

		public static TLocalizedText TGlossaryDesc20011 => null;

		public static TLocalizedText TGlossaryName20012 => null;

		public static TLocalizedText TGlossaryDesc20012 => null;

		public static TLocalizedText TGlossaryName20013 => null;

		public static TLocalizedText TGlossaryDesc20013 => null;

		public static TLocalizedText TGlossaryName20014 => null;

		public static TLocalizedText TGlossaryDesc20014 => null;

		public static TLocalizedText TGlossaryName20015 => null;

		public static TLocalizedText TGlossaryDesc20015 => null;

		public static TLocalizedText TGlossaryName20016 => null;

		public static TLocalizedText TGlossaryDesc20016 => null;

		public static TLocalizedText TGlossaryName20017 => null;

		public static TLocalizedText TGlossaryDesc20017 => null;

		public static TLocalizedText TGlossaryName20018 => null;

		public static TLocalizedText TGlossaryDesc20018 => null;

		public static TLocalizedText TGlossaryName20019 => null;

		public static TLocalizedText TGlossaryDesc20019 => null;

		public static TLocalizedText TGlossaryName20020 => null;

		public static TLocalizedText TGlossaryDesc20020 => null;

		public static TLocalizedText TGlossaryName30001 => null;

		public static TLocalizedText TGlossaryDesc30001 => null;

		public static TLocalizedText TGlossaryName30002 => null;

		public static TLocalizedText TGlossaryDesc30002 => null;

		public static TLocalizedText TGlossaryName30003 => null;

		public static TLocalizedText TGlossaryDesc30003 => null;

		public static TLocalizedText TGlossaryName30004 => null;

		public static TLocalizedText TGlossaryDesc30004 => null;

		public static TLocalizedText TGlossaryName30005 => null;

		public static TLocalizedText TGlossaryDesc30005 => null;

		public static TLocalizedText TGlossaryName30006 => null;

		public static TLocalizedText TGlossaryDesc30006 => null;

		public static TLocalizedText TGlossaryName30007 => null;

		public static TLocalizedText TGlossaryDesc30007 => null;

		public static TLocalizedText TGlossaryName30008 => null;

		public static TLocalizedText TGlossaryDesc30008 => null;

		public static TLocalizedText TGlossaryName30009 => null;

		public static TLocalizedText TGlossaryDesc30009 => null;

		public static TLocalizedText TGlossaryName30010 => null;

		public static TLocalizedText TGlossaryDesc30010 => null;

		public static TLocalizedText TGlossaryName30011 => null;

		public static TLocalizedText TGlossaryDesc30011 => null;

		public static TLocalizedText TGlossaryName30012 => null;

		public static TLocalizedText TGlossaryDesc30012 => null;

		public static TLocalizedText TGlossaryName30013 => null;

		public static TLocalizedText TGlossaryDesc30013 => null;

		public static TLocalizedText TGlossaryName30014 => null;

		public static TLocalizedText TGlossaryDesc30014 => null;

		public static TLocalizedText TGlossaryName30015 => null;

		public static TLocalizedText TGlossaryDesc30015 => null;

		public static TLocalizedText TGlossaryName30016 => null;

		public static TLocalizedText TGlossaryDesc30016 => null;

		public static TLocalizedText TGlossaryName30017 => null;

		public static TLocalizedText TGlossaryDesc30017 => null;

		public static TLocalizedText TGlossaryName30018 => null;

		public static TLocalizedText TGlossaryDesc30018 => null;

		public static TLocalizedText TGlossaryName30019 => null;

		public static TLocalizedText TGlossaryDesc30019 => null;

		public static TLocalizedText TGlossaryName30020 => null;

		public static TLocalizedText TGlossaryDesc30020 => null;

		public static TLocalizedText TGlossaryName30021 => null;

		public static TLocalizedText TGlossaryDesc30021 => null;

		public static TLocalizedText TGlossaryName30022 => null;

		public static TLocalizedText TGlossaryDesc30022 => null;

		public static TLocalizedText TGlossaryName30023 => null;

		public static TLocalizedText TGlossaryDesc30023 => null;

		public static TLocalizedText TGlossaryName30024 => null;

		public static TLocalizedText TGlossaryDesc30024 => null;

		public static TLocalizedText TGlossaryName30025 => null;

		public static TLocalizedText TGlossaryDesc30025 => null;

		public static TLocalizedText TGlossaryName30026 => null;

		public static TLocalizedText TGlossaryDesc30026 => null;

		public static TLocalizedText TGlossaryName30027 => null;

		public static TLocalizedText TGlossaryDesc30027 => null;

		public static TLocalizedText TGlossaryName30028 => null;

		public static TLocalizedText TGlossaryDesc30028 => null;

		public static TLocalizedText TGlossaryName30029 => null;

		public static TLocalizedText TGlossaryDesc30029 => null;

		public static TLocalizedText TGlossaryName30030 => null;

		public static TLocalizedText TGlossaryDesc30030 => null;

		public static TLocalizedText TGlossaryName30031 => null;

		public static TLocalizedText TGlossaryDesc30031 => null;

		public static TLocalizedText TGlossaryName30032 => null;

		public static TLocalizedText TGlossaryDesc30032 => null;

		public static TLocalizedText TGlossaryName30033 => null;

		public static TLocalizedText TGlossaryDesc30033 => null;

		public static TLocalizedText TGlossaryName30034 => null;

		public static TLocalizedText TGlossaryDesc30034 => null;

		public static TLocalizedText TGlossaryName30035 => null;

		public static TLocalizedText TGlossaryDesc30035 => null;

		public static TLocalizedText TGlossaryName40001 => null;

		public static TLocalizedText TGlossaryDesc40001 => null;

		public static TLocalizedText TGlossaryName40002 => null;

		public static TLocalizedText TGlossaryDesc40002 => null;

		public static TLocalizedText TGlossaryName40003 => null;

		public static TLocalizedText TGlossaryDesc40003 => null;

		public static TLocalizedText TGlossaryName40004 => null;

		public static TLocalizedText TGlossaryDesc40004 => null;

		public static TLocalizedText TGlossaryName40005 => null;

		public static TLocalizedText TGlossaryDesc40005 => null;

		public static TLocalizedText TGlossaryName40006 => null;

		public static TLocalizedText TGlossaryDesc40006 => null;

		public static TLocalizedText TGlossaryName40007 => null;

		public static TLocalizedText TGlossaryDesc40007 => null;

		public static TLocalizedText TGlossaryName40008 => null;

		public static TLocalizedText TGlossaryDesc40008 => null;

		public static TLocalizedText TGlossaryName40009 => null;

		public static TLocalizedText TGlossaryDesc40009 => null;

		public static TLocalizedText TGlossaryName40010 => null;

		public static TLocalizedText TGlossaryDesc40010 => null;

		public static TLocalizedText TGlossaryName40011 => null;

		public static TLocalizedText TGlossaryDesc40011 => null;

		public static TLocalizedText TGlossaryName40012 => null;

		public static TLocalizedText TGlossaryDesc40012 => null;

		public static TLocalizedText TGlossaryName40013 => null;

		public static TLocalizedText TGlossaryDesc40013 => null;

		public static TLocalizedText TGlossaryName40014 => null;

		public static TLocalizedText TGlossaryDesc40014 => null;

		public static TLocalizedText TGlossaryName40015 => null;

		public static TLocalizedText TGlossaryDesc40015 => null;

		public static TLocalizedText TGlossaryName40016 => null;

		public static TLocalizedText TGlossaryDesc40016 => null;

		public static TLocalizedText TGlossaryName40017 => null;

		public static TLocalizedText TGlossaryDesc40017 => null;

		public static TLocalizedText TGlossaryName40018 => null;

		public static TLocalizedText TGlossaryDesc40018 => null;

		public static TLocalizedText TGlossaryName40019 => null;

		public static TLocalizedText TGlossaryDesc40019 => null;

		public static TLocalizedText TGlossaryName40020 => null;

		public static TLocalizedText TGlossaryDesc40020 => null;

		public static TLocalizedText TGlossaryName40021 => null;

		public static TLocalizedText TGlossaryDesc40021 => null;

		public static TLocalizedText TGlossaryName40022 => null;

		public static TLocalizedText TGlossaryDesc40022 => null;

		public static TLocalizedText TGlossaryName40023 => null;

		public static TLocalizedText TGlossaryDesc40023 => null;

		public static TLocalizedText TGlossaryName40024 => null;

		public static TLocalizedText TGlossaryDesc40024 => null;

		public static TLocalizedText TGlossaryName40025 => null;

		public static TLocalizedText TGlossaryDesc40025 => null;

		public static TLocalizedText TGlossaryName40026 => null;

		public static TLocalizedText TGlossaryDesc40026 => null;

		public static TLocalizedText TGlossaryName40027 => null;

		public static TLocalizedText TGlossaryDesc40027 => null;

		public static TLocalizedText TGlossaryName40028 => null;

		public static TLocalizedText TGlossaryDesc40028 => null;

		public static TLocalizedText TGlossaryName40029 => null;

		public static TLocalizedText TGlossaryDesc40029 => null;

		public static TLocalizedText TInputActionName1 => null;

		public static TLocalizedText TInputActionName2 => null;

		public static TLocalizedText TInputActionName3 => null;

		public static TLocalizedText TInputActionName4 => null;

		public static TLocalizedText TInputActionName5 => null;

		public static TLocalizedText TInputActionName6 => null;

		public static TLocalizedText TInputActionName7 => null;

		public static TLocalizedText TInputActionName8 => null;

		public static TLocalizedText TInputActionName9 => null;

		public static TLocalizedText TInputActionName10 => null;

		public static TLocalizedText TInputActionName11 => null;

		public static TLocalizedText TInputActionName12 => null;

		public static TLocalizedText TInputActionName13 => null;

		public static TLocalizedText TInputActionName14 => null;

		public static TLocalizedText TInputActionName15 => null;

		public static TLocalizedText TInputActionName16 => null;

		public static TLocalizedText TInputActionName1001 => null;

		public static TLocalizedText TInputActionName1002 => null;

		public static TLocalizedText TInputActionName1003 => null;

		public static TLocalizedText TInputActionName1004 => null;

		public static TLocalizedText TInputActionName1005 => null;

		public static TLocalizedText TInputActionName1006 => null;

		public static TLocalizedText TInputActionName1007 => null;

		public static TLocalizedText TInputActionName1008 => null;

		public static TLocalizedText TInputActionName1009 => null;

		public static TLocalizedText TInputActionName1010 => null;

		public static TLocalizedText TInputActionName1011 => null;

		public static TLocalizedText TInputActionName1012 => null;

		public static TLocalizedText TInputActionName1013 => null;

		public static TLocalizedText TInputActionName1014 => null;

		public static TLocalizedText TInputActionName1015 => null;

		public static TLocalizedText TInputActionName1016 => null;

		public static TLocalizedText TInputActionName1017 => null;

		public static TLocalizedText TInputActionName1018 => null;

		public static TLocalizedText TInputActionName1019 => null;

		public static TLocalizedText TInputActionName1020 => null;

		public static TLocalizedText TInputActionName1021 => null;

		public static TLocalizedText TInputActionName1022 => null;

		public static TLocalizedText TItemName2000 => null;

		public static TLocalizedText TItemDesc2000 => null;

		public static TLocalizedText TItemName2001 => null;

		public static TLocalizedText TItemDesc2001 => null;

		public static TLocalizedText TItemName2002 => null;

		public static TLocalizedText TItemDesc2002 => null;

		public static TLocalizedText TItemName2003 => null;

		public static TLocalizedText TItemDesc2003 => null;

		public static TLocalizedText TItemName2004 => null;

		public static TLocalizedText TItemDesc2004 => null;

		public static TLocalizedText TItemName2005 => null;

		public static TLocalizedText TItemDesc2005 => null;

		public static TLocalizedText TItemName2006 => null;

		public static TLocalizedText TItemDesc2006 => null;

		public static TLocalizedText TItemName2007 => null;

		public static TLocalizedText TItemDesc2007 => null;

		public static TLocalizedText TItemName2008 => null;

		public static TLocalizedText TItemDesc2008 => null;

		public static TLocalizedText TItemName2009 => null;

		public static TLocalizedText TItemDesc2009 => null;

		public static TLocalizedText TItemName2010 => null;

		public static TLocalizedText TItemDesc2010 => null;

		public static TLocalizedText TItemName2011 => null;

		public static TLocalizedText TItemDesc2011 => null;

		public static TLocalizedText TItemName2012 => null;

		public static TLocalizedText TItemDesc2012 => null;

		public static TLocalizedText TItemName2013 => null;

		public static TLocalizedText TItemDesc2013 => null;

		public static TLocalizedText TItemName2014 => null;

		public static TLocalizedText TItemDesc2014 => null;

		public static TLocalizedText TItemName2015 => null;

		public static TLocalizedText TItemDesc2015 => null;

		public static TLocalizedText TItemName2016 => null;

		public static TLocalizedText TItemDesc2016 => null;

		public static TLocalizedText TItemName2017 => null;

		public static TLocalizedText TItemDesc2017 => null;

		public static TLocalizedText TItemName2018 => null;

		public static TLocalizedText TItemDesc2018 => null;

		public static TLocalizedText TItemName2019 => null;

		public static TLocalizedText TItemDesc2019 => null;

		public static TLocalizedText TItemName2020 => null;

		public static TLocalizedText TItemDesc2020 => null;

		public static TLocalizedText TItemName2021 => null;

		public static TLocalizedText TItemDesc2021 => null;

		public static TLocalizedText TItemName2022 => null;

		public static TLocalizedText TItemDesc2022 => null;

		public static TLocalizedText TItemName2023 => null;

		public static TLocalizedText TItemDesc2023 => null;

		public static TLocalizedText TItemName2024 => null;

		public static TLocalizedText TItemDesc2024 => null;

		public static TLocalizedText TItemName2025 => null;

		public static TLocalizedText TItemDesc2025 => null;

		public static TLocalizedText TItemName2026 => null;

		public static TLocalizedText TItemDesc2026 => null;

		public static TLocalizedText TItemName2027 => null;

		public static TLocalizedText TItemDesc2027 => null;

		public static TLocalizedText TItemName2028 => null;

		public static TLocalizedText TItemDesc2028 => null;

		public static TLocalizedText TItemName2029 => null;

		public static TLocalizedText TItemDesc2029 => null;

		public static TLocalizedText TItemName2030 => null;

		public static TLocalizedText TItemDesc2030 => null;

		public static TLocalizedText TItemName2031 => null;

		public static TLocalizedText TItemDesc2031 => null;

		public static TLocalizedText TItemName2032 => null;

		public static TLocalizedText TItemDesc2032 => null;

		public static TLocalizedText TItemName2033 => null;

		public static TLocalizedText TItemDesc2033 => null;

		public static TLocalizedText TItemName2034 => null;

		public static TLocalizedText TItemDesc2034 => null;

		public static TLocalizedText TItemName2035 => null;

		public static TLocalizedText TItemDesc2035 => null;

		public static TLocalizedText TItemName2036 => null;

		public static TLocalizedText TItemDesc2036 => null;

		public static TLocalizedText TItemName2037 => null;

		public static TLocalizedText TItemDesc2037 => null;

		public static TLocalizedText TItemName2038 => null;

		public static TLocalizedText TItemDesc2038 => null;

		public static TLocalizedText TItemName2039 => null;

		public static TLocalizedText TItemDesc2039 => null;

		public static TLocalizedText TItemName2040 => null;

		public static TLocalizedText TItemDesc2040 => null;

		public static TLocalizedText TItemName2041 => null;

		public static TLocalizedText TItemDesc2041 => null;

		public static TLocalizedText TItemName2501 => null;

		public static TLocalizedText TItemDesc2501 => null;

		public static TLocalizedText TItemName2502 => null;

		public static TLocalizedText TItemDesc2502 => null;

		public static TLocalizedText TItemName2503 => null;

		public static TLocalizedText TItemDesc2503 => null;

		public static TLocalizedText TItemName2504 => null;

		public static TLocalizedText TItemDesc2504 => null;

		public static TLocalizedText TItemName2505 => null;

		public static TLocalizedText TItemDesc2505 => null;

		public static TLocalizedText TItemName3000 => null;

		public static TLocalizedText TItemDesc3000 => null;

		public static TLocalizedText TItemName3001 => null;

		public static TLocalizedText TItemDesc3001 => null;

		public static TLocalizedText TItemName3002 => null;

		public static TLocalizedText TItemDesc3002 => null;

		public static TLocalizedText TItemName3003 => null;

		public static TLocalizedText TItemDesc3003 => null;

		public static TLocalizedText TItemName3004 => null;

		public static TLocalizedText TItemDesc3004 => null;

		public static TLocalizedText TItemName3005 => null;

		public static TLocalizedText TItemDesc3005 => null;

		public static TLocalizedText TItemName3006 => null;

		public static TLocalizedText TItemDesc3006 => null;

		public static TLocalizedText TItemName3007 => null;

		public static TLocalizedText TItemDesc3007 => null;

		public static TLocalizedText TItemName3008 => null;

		public static TLocalizedText TItemDesc3008 => null;

		public static TLocalizedText TItemName3009 => null;

		public static TLocalizedText TItemDesc3009 => null;

		public static TLocalizedText TItemName3010 => null;

		public static TLocalizedText TItemDesc3010 => null;

		public static TLocalizedText TItemName3011 => null;

		public static TLocalizedText TItemDesc3011 => null;

		public static TLocalizedText TItemName4001 => null;

		public static TLocalizedText TItemDesc4001 => null;

		public static TLocalizedText TItemName4002 => null;

		public static TLocalizedText TItemDesc4002 => null;

		public static TLocalizedText TItemName4003 => null;

		public static TLocalizedText TItemDesc4003 => null;

		public static TLocalizedText TItemName4004 => null;

		public static TLocalizedText TItemDesc4004 => null;

		public static TLocalizedText TItemName4005 => null;

		public static TLocalizedText TItemDesc4005 => null;

		public static TLocalizedText TItemName4006 => null;

		public static TLocalizedText TItemDesc4006 => null;

		public static TLocalizedText TItemName4007 => null;

		public static TLocalizedText TItemDesc4007 => null;

		public static TLocalizedText TItemName4008 => null;

		public static TLocalizedText TItemDesc4008 => null;

		public static TLocalizedText TItemName4009 => null;

		public static TLocalizedText TItemDesc4009 => null;

		public static TLocalizedText TItemName4010 => null;

		public static TLocalizedText TItemDesc4010 => null;

		public static TLocalizedText TItemName5004 => null;

		public static TLocalizedText TItemDesc5004 => null;

		public static TLocalizedText TItemName6001 => null;

		public static TLocalizedText TItemDesc6001 => null;

		public static TLocalizedText TItemName6002 => null;

		public static TLocalizedText TItemDesc6002 => null;

		public static TLocalizedText TItemName6003 => null;

		public static TLocalizedText TItemDesc6003 => null;

		public static TLocalizedText TItemName7000 => null;

		public static TLocalizedText TItemDesc7000 => null;

		public static TLocalizedText TItemName7001 => null;

		public static TLocalizedText TItemDesc7001 => null;

		public static TLocalizedText TItemName7002 => null;

		public static TLocalizedText TItemDesc7002 => null;

		public static TLocalizedText TItemName7003 => null;

		public static TLocalizedText TItemDesc7003 => null;

		public static TLocalizedText TItemName7004 => null;

		public static TLocalizedText TItemDesc7004 => null;

		public static TLocalizedText TItemName7005 => null;

		public static TLocalizedText TItemDesc7005 => null;

		public static TLocalizedText TItemName7006 => null;

		public static TLocalizedText TItemDesc7006 => null;

		public static TLocalizedText TItemName7007 => null;

		public static TLocalizedText TItemDesc7007 => null;

		public static TLocalizedText TItemName7008 => null;

		public static TLocalizedText TItemDesc7008 => null;

		public static TLocalizedText TItemName7009 => null;

		public static TLocalizedText TItemDesc7009 => null;

		public static TLocalizedText TItemName7010 => null;

		public static TLocalizedText TItemDesc7010 => null;

		public static TLocalizedText TItemName7011 => null;

		public static TLocalizedText TItemDesc7011 => null;

		public static TLocalizedText TItemName7012 => null;

		public static TLocalizedText TItemDesc7012 => null;

		public static TLocalizedText TItemName7013 => null;

		public static TLocalizedText TItemDesc7013 => null;

		public static TLocalizedText TItemName7014 => null;

		public static TLocalizedText TItemDesc7014 => null;

		public static TLocalizedText TItemName7015 => null;

		public static TLocalizedText TItemDesc7015 => null;

		public static TLocalizedText TItemName7016 => null;

		public static TLocalizedText TItemDesc7016 => null;

		public static TLocalizedText TItemName7017 => null;

		public static TLocalizedText TItemDesc7017 => null;

		public static TLocalizedText TItemName7018 => null;

		public static TLocalizedText TItemDesc7018 => null;

		public static TLocalizedText TItemName7019 => null;

		public static TLocalizedText TItemDesc7019 => null;

		public static TLocalizedText TItemName7020 => null;

		public static TLocalizedText TItemDesc7020 => null;

		public static TLocalizedText TItemName7021 => null;

		public static TLocalizedText TItemDesc7021 => null;

		public static TLocalizedText TItemName7022 => null;

		public static TLocalizedText TItemDesc7022 => null;

		public static TLocalizedText TItemName7023 => null;

		public static TLocalizedText TItemDesc7023 => null;

		public static TLocalizedText TItemName7024 => null;

		public static TLocalizedText TItemDesc7024 => null;

		public static TLocalizedText TItemName7025 => null;

		public static TLocalizedText TItemDesc7025 => null;

		public static TLocalizedText TItemName7026 => null;

		public static TLocalizedText TItemDesc7026 => null;

		public static TLocalizedText TItemName7027 => null;

		public static TLocalizedText TItemDesc7027 => null;

		public static TLocalizedText TItemName7028 => null;

		public static TLocalizedText TItemDesc7028 => null;

		public static TLocalizedText TItemName7029 => null;

		public static TLocalizedText TItemDesc7029 => null;

		public static TLocalizedText TItemName7030 => null;

		public static TLocalizedText TItemDesc7030 => null;

		public static TLocalizedText TItemName7031 => null;

		public static TLocalizedText TItemDesc7031 => null;

		public static TLocalizedText TItemName7032 => null;

		public static TLocalizedText TItemDesc7032 => null;

		public static TLocalizedText TItemName7033 => null;

		public static TLocalizedText TItemDesc7033 => null;

		public static TLocalizedText TItemName7034 => null;

		public static TLocalizedText TItemDesc7034 => null;

		public static TLocalizedText TItemName7035 => null;

		public static TLocalizedText TItemDesc7035 => null;

		public static TLocalizedText TItemName7101 => null;

		public static TLocalizedText TItemDesc7101 => null;

		public static TLocalizedText TItemName7102 => null;

		public static TLocalizedText TItemDesc7102 => null;

		public static TLocalizedText TItemName7211 => null;

		public static TLocalizedText TItemDesc7211 => null;

		public static TLocalizedText TItemName7212 => null;

		public static TLocalizedText TItemDesc7212 => null;

		public static TLocalizedText TItemName7213 => null;

		public static TLocalizedText TItemDesc7213 => null;

		public static TLocalizedText TItemName7214 => null;

		public static TLocalizedText TItemDesc7214 => null;

		public static TLocalizedText TItemName7215 => null;

		public static TLocalizedText TItemDesc7215 => null;

		public static TLocalizedText TItemName7221 => null;

		public static TLocalizedText TItemDesc7221 => null;

		public static TLocalizedText TItemName7222 => null;

		public static TLocalizedText TItemDesc7222 => null;

		public static TLocalizedText TItemName7223 => null;

		public static TLocalizedText TItemDesc7223 => null;

		public static TLocalizedText TItemName7224 => null;

		public static TLocalizedText TItemDesc7224 => null;

		public static TLocalizedText TItemName7225 => null;

		public static TLocalizedText TItemDesc7225 => null;

		public static TLocalizedText TItemName7231 => null;

		public static TLocalizedText TItemDesc7231 => null;

		public static TLocalizedText TItemName7232 => null;

		public static TLocalizedText TItemDesc7232 => null;

		public static TLocalizedText TItemName7233 => null;

		public static TLocalizedText TItemDesc7233 => null;

		public static TLocalizedText TItemName7234 => null;

		public static TLocalizedText TItemDesc7234 => null;

		public static TLocalizedText TItemName7235 => null;

		public static TLocalizedText TItemDesc7235 => null;

		public static TLocalizedText TItemName7241 => null;

		public static TLocalizedText TItemDesc7241 => null;

		public static TLocalizedText TItemName7242 => null;

		public static TLocalizedText TItemDesc7242 => null;

		public static TLocalizedText TItemName7311 => null;

		public static TLocalizedText TItemDesc7311 => null;

		public static TLocalizedText TItemName7312 => null;

		public static TLocalizedText TItemDesc7312 => null;

		public static TLocalizedText TItemName7321 => null;

		public static TLocalizedText TItemDesc7321 => null;

		public static TLocalizedText TItemName7322 => null;

		public static TLocalizedText TItemDesc7322 => null;

		public static TLocalizedText TItemName7331 => null;

		public static TLocalizedText TItemDesc7331 => null;

		public static TLocalizedText TItemName7332 => null;

		public static TLocalizedText TItemDesc7332 => null;

		public static TLocalizedText TItemName8000 => null;

		public static TLocalizedText TItemDesc8000 => null;

		public static TLocalizedText TItemName8001 => null;

		public static TLocalizedText TItemDesc8001 => null;

		public static TLocalizedText TItemName8002 => null;

		public static TLocalizedText TItemDesc8002 => null;

		public static TLocalizedText TItemName8003 => null;

		public static TLocalizedText TItemDesc8003 => null;

		public static TLocalizedText TItemName8004 => null;

		public static TLocalizedText TItemDesc8004 => null;

		public static TLocalizedText TItemName8005 => null;

		public static TLocalizedText TItemDesc8005 => null;

		public static TLocalizedText TItemName8006 => null;

		public static TLocalizedText TItemDesc8006 => null;

		public static TLocalizedText TItemName8007 => null;

		public static TLocalizedText TItemDesc8007 => null;

		public static TLocalizedText TItemName8008 => null;

		public static TLocalizedText TItemDesc8008 => null;

		public static TLocalizedText TItemName8009 => null;

		public static TLocalizedText TItemDesc8009 => null;

		public static TLocalizedText TItemName8010 => null;

		public static TLocalizedText TItemDesc8010 => null;

		public static TLocalizedText TItemName8011 => null;

		public static TLocalizedText TItemDesc8011 => null;

		public static TLocalizedText TItemName8012 => null;

		public static TLocalizedText TItemDesc8012 => null;

		public static TLocalizedText TItemName8013 => null;

		public static TLocalizedText TItemDesc8013 => null;

		public static TLocalizedText TItemName8014 => null;

		public static TLocalizedText TItemDesc8014 => null;

		public static TLocalizedText TItemName8015 => null;

		public static TLocalizedText TItemDesc8015 => null;

		public static TLocalizedText TItemName8016 => null;

		public static TLocalizedText TItemDesc8016 => null;

		public static TLocalizedText TItemName8017 => null;

		public static TLocalizedText TItemDesc8017 => null;

		public static TLocalizedText TItemName8018 => null;

		public static TLocalizedText TItemDesc8018 => null;

		public static TLocalizedText TItemName8019 => null;

		public static TLocalizedText TItemDesc8019 => null;

		public static TLocalizedText TItemName8020 => null;

		public static TLocalizedText TItemDesc8020 => null;

		public static TLocalizedText TItemName8021 => null;

		public static TLocalizedText TItemDesc8021 => null;

		public static TLocalizedText TItemName8022 => null;

		public static TLocalizedText TItemDesc8022 => null;

		public static TLocalizedText TItemName8023 => null;

		public static TLocalizedText TItemDesc8023 => null;

		public static TLocalizedText TItemName8024 => null;

		public static TLocalizedText TItemDesc8024 => null;

		public static TLocalizedText TItemName8025 => null;

		public static TLocalizedText TItemDesc8025 => null;

		public static TLocalizedText TItemName8026 => null;

		public static TLocalizedText TItemDesc8026 => null;

		public static TLocalizedText TItemName8027 => null;

		public static TLocalizedText TItemDesc8027 => null;

		public static TLocalizedText TItemName8028 => null;

		public static TLocalizedText TItemDesc8028 => null;

		public static TLocalizedText TItemName8029 => null;

		public static TLocalizedText TItemDesc8029 => null;

		public static TLocalizedText TItemName8030 => null;

		public static TLocalizedText TItemDesc8030 => null;

		public static TLocalizedText TItemName8031 => null;

		public static TLocalizedText TItemDesc8031 => null;

		public static TLocalizedText TItemName8032 => null;

		public static TLocalizedText TItemDesc8032 => null;

		public static TLocalizedText TItemName8033 => null;

		public static TLocalizedText TItemDesc8033 => null;

		public static TLocalizedText TItemName8034 => null;

		public static TLocalizedText TItemDesc8034 => null;

		public static TLocalizedText TItemName8035 => null;

		public static TLocalizedText TItemDesc8035 => null;

		public static TLocalizedText TItemName8036 => null;

		public static TLocalizedText TItemDesc8036 => null;

		public static TLocalizedText TItemName8037 => null;

		public static TLocalizedText TItemDesc8037 => null;

		public static TLocalizedText TItemName8038 => null;

		public static TLocalizedText TItemDesc8038 => null;

		public static TLocalizedText TItemName8039 => null;

		public static TLocalizedText TItemDesc8039 => null;

		public static TLocalizedText TItemName8040 => null;

		public static TLocalizedText TItemDesc8040 => null;

		public static TLocalizedText TItemName8041 => null;

		public static TLocalizedText TItemDesc8041 => null;

		public static TLocalizedText TItemName8042 => null;

		public static TLocalizedText TItemDesc8042 => null;

		public static TLocalizedText TItemName8043 => null;

		public static TLocalizedText TItemDesc8043 => null;

		public static TLocalizedText TItemName8044 => null;

		public static TLocalizedText TItemDesc8044 => null;

		public static TLocalizedText TItemName8045 => null;

		public static TLocalizedText TItemDesc8045 => null;

		public static TLocalizedText TItemName8046 => null;

		public static TLocalizedText TItemDesc8046 => null;

		public static TLocalizedText TItemName8047 => null;

		public static TLocalizedText TItemDesc8047 => null;

		public static TLocalizedText TItemName8048 => null;

		public static TLocalizedText TItemDesc8048 => null;

		public static TLocalizedText TItemName8049 => null;

		public static TLocalizedText TItemDesc8049 => null;

		public static TLocalizedText TItemName8050 => null;

		public static TLocalizedText TItemDesc8050 => null;

		public static TLocalizedText TItemName8051 => null;

		public static TLocalizedText TItemDesc8051 => null;

		public static TLocalizedText TItemName8052 => null;

		public static TLocalizedText TItemDesc8052 => null;

		public static TLocalizedText TItemName8053 => null;

		public static TLocalizedText TItemDesc8053 => null;

		public static TLocalizedText TItemName8060 => null;

		public static TLocalizedText TItemDesc8060 => null;

		public static TLocalizedText TItemName8061 => null;

		public static TLocalizedText TItemDesc8061 => null;

		public static TLocalizedText TItemName8062 => null;

		public static TLocalizedText TItemDesc8062 => null;

		public static TLocalizedText TItemName8063 => null;

		public static TLocalizedText TItemDesc8063 => null;

		public static TLocalizedText TItemName8064 => null;

		public static TLocalizedText TItemDesc8064 => null;

		public static TLocalizedText TItemName8065 => null;

		public static TLocalizedText TItemDesc8065 => null;

		public static TLocalizedText TItemName8066 => null;

		public static TLocalizedText TItemDesc8066 => null;

		public static TLocalizedText TItemName8067 => null;

		public static TLocalizedText TItemDesc8067 => null;

		public static TLocalizedText TItemName8068 => null;

		public static TLocalizedText TItemDesc8068 => null;

		public static TLocalizedText TItemName8069 => null;

		public static TLocalizedText TItemDesc8069 => null;

		public static TLocalizedText TItemName8070 => null;

		public static TLocalizedText TItemDesc8070 => null;

		public static TLocalizedText TItemName8071 => null;

		public static TLocalizedText TItemDesc8071 => null;

		public static TLocalizedText TItemName8072 => null;

		public static TLocalizedText TItemDesc8072 => null;

		public static TLocalizedText TItemName8073 => null;

		public static TLocalizedText TItemDesc8073 => null;

		public static TLocalizedText TItemName8075 => null;

		public static TLocalizedText TItemDesc8075 => null;

		public static TLocalizedText TItemName8076 => null;

		public static TLocalizedText TItemDesc8076 => null;

		public static TLocalizedText TItemName8077 => null;

		public static TLocalizedText TItemDesc8077 => null;

		public static TLocalizedText TItemName8079 => null;

		public static TLocalizedText TItemDesc8079 => null;

		public static TLocalizedText TItemName8080 => null;

		public static TLocalizedText TItemDesc8080 => null;

		public static TLocalizedText TItemName8081 => null;

		public static TLocalizedText TItemDesc8081 => null;

		public static TLocalizedText TItemName8082 => null;

		public static TLocalizedText TItemDesc8082 => null;

		public static TLocalizedText TItemName8083 => null;

		public static TLocalizedText TItemDesc8083 => null;

		public static TLocalizedText TItemName8084 => null;

		public static TLocalizedText TItemDesc8084 => null;

		public static TLocalizedText TItemName8085 => null;

		public static TLocalizedText TItemDesc8085 => null;

		public static TLocalizedText TItemName8086 => null;

		public static TLocalizedText TItemDesc8086 => null;

		public static TLocalizedText TItemName8087 => null;

		public static TLocalizedText TItemDesc8087 => null;

		public static TLocalizedText TItemName8088 => null;

		public static TLocalizedText TItemDesc8088 => null;

		public static TLocalizedText TItemName8089 => null;

		public static TLocalizedText TItemDesc8089 => null;

		public static TLocalizedText TItemName8090 => null;

		public static TLocalizedText TItemDesc8090 => null;

		public static TLocalizedText TItemName8091 => null;

		public static TLocalizedText TItemDesc8091 => null;

		public static TLocalizedText TItemName8092 => null;

		public static TLocalizedText TItemDesc8092 => null;

		public static TLocalizedText TItemName8093 => null;

		public static TLocalizedText TItemDesc8093 => null;

		public static TLocalizedText TItemName8094 => null;

		public static TLocalizedText TItemDesc8094 => null;

		public static TLocalizedText TItemName8095 => null;

		public static TLocalizedText TItemDesc8095 => null;

		public static TLocalizedText TItemName8096 => null;

		public static TLocalizedText TItemDesc8096 => null;

		public static TLocalizedText TItemName8097 => null;

		public static TLocalizedText TItemDesc8097 => null;

		public static TLocalizedText TItemName8098 => null;

		public static TLocalizedText TItemDesc8098 => null;

		public static TLocalizedText TItemName8099 => null;

		public static TLocalizedText TItemDesc8099 => null;

		public static TLocalizedText TItemName8100 => null;

		public static TLocalizedText TItemDesc8100 => null;

		public static TLocalizedText TItemName8101 => null;

		public static TLocalizedText TItemDesc8101 => null;

		public static TLocalizedText TItemName8102 => null;

		public static TLocalizedText TItemDesc8102 => null;

		public static TLocalizedText TItemName8103 => null;

		public static TLocalizedText TItemDesc8103 => null;

		public static TLocalizedText TItemName8104 => null;

		public static TLocalizedText TItemDesc8104 => null;

		public static TLocalizedText TItemName8105 => null;

		public static TLocalizedText TItemDesc8105 => null;

		public static TLocalizedText TItemName8106 => null;

		public static TLocalizedText TItemDesc8106 => null;

		public static TLocalizedText TItemName8107 => null;

		public static TLocalizedText TItemDesc8107 => null;

		public static TLocalizedText TItemName8108 => null;

		public static TLocalizedText TItemDesc8108 => null;

		public static TLocalizedText TItemName8109 => null;

		public static TLocalizedText TItemDesc8109 => null;

		public static TLocalizedText TItemName8110 => null;

		public static TLocalizedText TItemDesc8110 => null;

		public static TLocalizedText TItemName8111 => null;

		public static TLocalizedText TItemDesc8111 => null;

		public static TLocalizedText TItemName8112 => null;

		public static TLocalizedText TItemDesc8112 => null;

		public static TLocalizedText TItemName8201 => null;

		public static TLocalizedText TItemDesc8201 => null;

		public static TLocalizedText TItemName8202 => null;

		public static TLocalizedText TItemDesc8202 => null;

		public static TLocalizedText TItemName8203 => null;

		public static TLocalizedText TItemDesc8203 => null;

		public static TLocalizedText TItemName8204 => null;

		public static TLocalizedText TItemDesc8204 => null;

		public static TLocalizedText TItemName8205 => null;

		public static TLocalizedText TItemDesc8205 => null;

		public static TLocalizedText TItemName8206 => null;

		public static TLocalizedText TItemDesc8206 => null;

		public static TLocalizedText TItemName8207 => null;

		public static TLocalizedText TItemDesc8207 => null;

		public static TLocalizedText TItemName8208 => null;

		public static TLocalizedText TItemDesc8208 => null;

		public static TLocalizedText TItemName8209 => null;

		public static TLocalizedText TItemDesc8209 => null;

		public static TLocalizedText TItemName8210 => null;

		public static TLocalizedText TItemDesc8210 => null;

		public static TLocalizedText TItemName8211 => null;

		public static TLocalizedText TItemDesc8211 => null;

		public static TLocalizedText TItemName8212 => null;

		public static TLocalizedText TItemDesc8212 => null;

		public static TLocalizedText TItemName9001 => null;

		public static TLocalizedText TItemDesc9001 => null;

		public static TLocalizedText TItemName9002 => null;

		public static TLocalizedText TItemDesc9002 => null;

		public static TLocalizedText TItemName9011 => null;

		public static TLocalizedText TItemDesc9011 => null;

		public static TLocalizedText TItemName9012 => null;

		public static TLocalizedText TItemDesc9012 => null;

		public static TLocalizedText TItemName9013 => null;

		public static TLocalizedText TItemDesc9013 => null;

		public static TLocalizedText TItemName9014 => null;

		public static TLocalizedText TItemDesc9014 => null;

		public static TLocalizedText TItemName9015 => null;

		public static TLocalizedText TItemDesc9015 => null;

		public static TLocalizedText TItemName9016 => null;

		public static TLocalizedText TItemDesc9016 => null;

		public static TLocalizedText TItemName9017 => null;

		public static TLocalizedText TItemDesc9017 => null;

		public static TLocalizedText TItemName9018 => null;

		public static TLocalizedText TItemDesc9018 => null;

		public static TLocalizedText TItemName9019 => null;

		public static TLocalizedText TItemDesc9019 => null;

		public static TLocalizedText TItemName9020 => null;

		public static TLocalizedText TItemDesc9020 => null;

		public static TLocalizedText TItemName9021 => null;

		public static TLocalizedText TItemDesc9021 => null;

		public static TLocalizedText TItemName9022 => null;

		public static TLocalizedText TItemDesc9022 => null;

		public static TLocalizedText TItemName9023 => null;

		public static TLocalizedText TItemDesc9023 => null;

		public static TLocalizedText TItemName9024 => null;

		public static TLocalizedText TItemDesc9024 => null;

		public static TLocalizedText TItemName9025 => null;

		public static TLocalizedText TItemDesc9025 => null;

		public static TLocalizedText TItemName9026 => null;

		public static TLocalizedText TItemDesc9026 => null;

		public static TLocalizedText TItemName10001 => null;

		public static TLocalizedText TItemDesc10001 => null;

		public static TLocalizedText TItemName10002 => null;

		public static TLocalizedText TItemDesc10002 => null;

		public static TLocalizedText TItemName10003 => null;

		public static TLocalizedText TItemDesc10003 => null;

		public static TLocalizedText TItemName10004 => null;

		public static TLocalizedText TItemDesc10004 => null;

		public static TLocalizedText TItemName10005 => null;

		public static TLocalizedText TItemDesc10005 => null;

		public static TLocalizedText TItemName11001 => null;

		public static TLocalizedText TItemDesc11001 => null;

		public static TLocalizedText TItemNote11001 => null;

		public static TLocalizedText TItemName11002 => null;

		public static TLocalizedText TItemDesc11002 => null;

		public static TLocalizedText TItemNote11002 => null;

		public static TLocalizedText TItemName11003 => null;

		public static TLocalizedText TItemDesc11003 => null;

		public static TLocalizedText TItemNote11003 => null;

		public static TLocalizedText TItemName11004 => null;

		public static TLocalizedText TItemDesc11004 => null;

		public static TLocalizedText TItemNote11004 => null;

		public static TLocalizedText TItemName11005 => null;

		public static TLocalizedText TItemDesc11005 => null;

		public static TLocalizedText TItemNote11005 => null;

		public static TLocalizedText TItemName11006 => null;

		public static TLocalizedText TItemDesc11006 => null;

		public static TLocalizedText TItemNote11006 => null;

		public static TLocalizedText TItemName11007 => null;

		public static TLocalizedText TItemDesc11007 => null;

		public static TLocalizedText TItemNote11007 => null;

		public static TLocalizedText TItemName11008 => null;

		public static TLocalizedText TItemDesc11008 => null;

		public static TLocalizedText TItemNote11008 => null;

		public static TLocalizedText TItemName11101 => null;

		public static TLocalizedText TItemDesc11101 => null;

		public static TLocalizedText TItemNote11101 => null;

		public static TLocalizedText TItemName11102 => null;

		public static TLocalizedText TItemDesc11102 => null;

		public static TLocalizedText TItemNote11102 => null;

		public static TLocalizedText TItemName11103 => null;

		public static TLocalizedText TItemDesc11103 => null;

		public static TLocalizedText TItemNote11103 => null;

		public static TLocalizedText TItemName11104 => null;

		public static TLocalizedText TItemDesc11104 => null;

		public static TLocalizedText TItemNote11104 => null;

		public static TLocalizedText TItemName11105 => null;

		public static TLocalizedText TItemDesc11105 => null;

		public static TLocalizedText TItemNote11105 => null;

		public static TLocalizedText TItemName11106 => null;

		public static TLocalizedText TItemDesc11106 => null;

		public static TLocalizedText TItemNote11106 => null;

		public static TLocalizedText TItemName11107 => null;

		public static TLocalizedText TItemDesc11107 => null;

		public static TLocalizedText TItemNote11107 => null;

		public static TLocalizedText TItemName11108 => null;

		public static TLocalizedText TItemDesc11108 => null;

		public static TLocalizedText TItemNote11108 => null;

		public static TLocalizedText TItemName11109 => null;

		public static TLocalizedText TItemDesc11109 => null;

		public static TLocalizedText TItemNote11109 => null;

		public static TLocalizedText TItemName11110 => null;

		public static TLocalizedText TItemDesc11110 => null;

		public static TLocalizedText TItemNote11110 => null;

		public static TLocalizedText TItemName11111 => null;

		public static TLocalizedText TItemDesc11111 => null;

		public static TLocalizedText TItemNote11111 => null;

		public static TLocalizedText TItemName11112 => null;

		public static TLocalizedText TItemDesc11112 => null;

		public static TLocalizedText TItemNote11112 => null;

		public static TLocalizedText TItemName11113 => null;

		public static TLocalizedText TItemDesc11113 => null;

		public static TLocalizedText TItemNote11113 => null;

		public static TLocalizedText TItemName11114 => null;

		public static TLocalizedText TItemDesc11114 => null;

		public static TLocalizedText TItemNote11114 => null;

		public static TLocalizedText TItemName11115 => null;

		public static TLocalizedText TItemDesc11115 => null;

		public static TLocalizedText TItemNote11115 => null;

		public static TLocalizedText TItemName11116 => null;

		public static TLocalizedText TItemDesc11116 => null;

		public static TLocalizedText TItemNote11116 => null;

		public static TLocalizedText TItemName12001 => null;

		public static TLocalizedText TItemDesc12001 => null;

		public static TLocalizedText TItemName12002 => null;

		public static TLocalizedText TItemDesc12002 => null;

		public static TLocalizedText TItemName12003 => null;

		public static TLocalizedText TItemDesc12003 => null;

		public static TLocalizedText TItemName12004 => null;

		public static TLocalizedText TItemDesc12004 => null;

		public static TLocalizedText TItemName12005 => null;

		public static TLocalizedText TItemDesc12005 => null;

		public static TLocalizedText TItemName12006 => null;

		public static TLocalizedText TItemDesc12006 => null;

		public static TLocalizedText TItemName12007 => null;

		public static TLocalizedText TItemDesc12007 => null;

		public static TLocalizedText TItemName12008 => null;

		public static TLocalizedText TItemDesc12008 => null;

		public static TLocalizedText TItemName12009 => null;

		public static TLocalizedText TItemDesc12009 => null;

		public static TLocalizedText TItemName12010 => null;

		public static TLocalizedText TItemDesc12010 => null;

		public static TLocalizedText TItemName82101 => null;

		public static TLocalizedText TItemDesc82101 => null;

		public static TLocalizedText TItemName82102 => null;

		public static TLocalizedText TItemDesc82102 => null;

		public static TLocalizedText TItemName82103 => null;

		public static TLocalizedText TItemDesc82103 => null;

		public static TLocalizedText TItemName82104 => null;

		public static TLocalizedText TItemDesc82104 => null;

		public static TLocalizedText TItemName82105 => null;

		public static TLocalizedText TItemDesc82105 => null;

		public static TLocalizedText TItemName82106 => null;

		public static TLocalizedText TItemDesc82106 => null;

		public static TLocalizedText TItemName82107 => null;

		public static TLocalizedText TItemDesc82107 => null;

		public static TLocalizedText TItemName82108 => null;

		public static TLocalizedText TItemDesc82108 => null;

		public static TLocalizedText TItemName82109 => null;

		public static TLocalizedText TItemDesc82109 => null;

		public static TLocalizedText TItemName82111 => null;

		public static TLocalizedText TItemDesc82111 => null;

		public static TLocalizedText TItemName82113 => null;

		public static TLocalizedText TItemDesc82113 => null;

		public static TLocalizedText TItemName82201 => null;

		public static TLocalizedText TItemDesc82201 => null;

		public static TLocalizedText TItemName82202 => null;

		public static TLocalizedText TItemDesc82202 => null;

		public static TLocalizedText TItemName82203 => null;

		public static TLocalizedText TItemDesc82203 => null;

		public static TLocalizedText TItemName82204 => null;

		public static TLocalizedText TItemDesc82204 => null;

		public static TLocalizedText TItemName82205 => null;

		public static TLocalizedText TItemDesc82205 => null;

		public static TLocalizedText TItemName82206 => null;

		public static TLocalizedText TItemDesc82206 => null;

		public static TLocalizedText TItemName82207 => null;

		public static TLocalizedText TItemDesc82207 => null;

		public static TLocalizedText TItemName82208 => null;

		public static TLocalizedText TItemDesc82208 => null;

		public static TLocalizedText TItemName82209 => null;

		public static TLocalizedText TItemDesc82209 => null;

		public static TLocalizedText TItemName82210 => null;

		public static TLocalizedText TItemDesc82210 => null;

		public static TLocalizedText TItemName91011 => null;

		public static TLocalizedText TItemDesc91011 => null;

		public static TLocalizedText TItemName91012 => null;

		public static TLocalizedText TItemDesc91012 => null;

		public static TLocalizedText TItemName102001 => null;

		public static TLocalizedText TItemDesc102001 => null;

		public static TLocalizedText TItemName102002 => null;

		public static TLocalizedText TItemDesc102002 => null;

		public static TLocalizedText TItemName102003 => null;

		public static TLocalizedText TItemDesc102003 => null;

		public static TLocalizedText TItemName102004 => null;

		public static TLocalizedText TItemDesc102004 => null;

		public static TLocalizedText TItemName102006 => null;

		public static TLocalizedText TItemDesc102006 => null;

		public static TLocalizedText TItemName102007 => null;

		public static TLocalizedText TItemDesc102007 => null;

		public static TLocalizedText TItemName102008 => null;

		public static TLocalizedText TItemDesc102008 => null;

		public static TLocalizedText TItemName102010 => null;

		public static TLocalizedText TItemDesc102010 => null;

		public static TLocalizedText TItemName102012 => null;

		public static TLocalizedText TItemDesc102012 => null;

		public static TLocalizedText TItemName102013 => null;

		public static TLocalizedText TItemDesc102013 => null;

		public static TLocalizedText TItemName102014 => null;

		public static TLocalizedText TItemDesc102014 => null;

		public static TLocalizedText TItemName102015 => null;

		public static TLocalizedText TItemDesc102015 => null;

		public static TLocalizedText TItemName102016 => null;

		public static TLocalizedText TItemDesc102016 => null;

		public static TLocalizedText TItemName102017 => null;

		public static TLocalizedText TItemDesc102017 => null;

		public static TLocalizedText TItemName102018 => null;

		public static TLocalizedText TItemDesc102018 => null;

		public static TLocalizedText TItemName102019 => null;

		public static TLocalizedText TItemDesc102019 => null;

		public static TLocalizedText TItemName102025 => null;

		public static TLocalizedText TItemDesc102025 => null;

		public static TLocalizedText TItemName102026 => null;

		public static TLocalizedText TItemDesc102026 => null;

		public static TLocalizedText TItemName102027 => null;

		public static TLocalizedText TItemDesc102027 => null;

		public static TLocalizedText TItemName102029 => null;

		public static TLocalizedText TItemDesc102029 => null;

		public static TLocalizedText TItemName102030 => null;

		public static TLocalizedText TItemDesc102030 => null;

		public static TLocalizedText TItemName102031 => null;

		public static TLocalizedText TItemDesc102031 => null;

		public static TLocalizedText TItemName102032 => null;

		public static TLocalizedText TItemDesc102032 => null;

		public static TLocalizedText TItemName102033 => null;

		public static TLocalizedText TItemDesc102033 => null;

		public static TLocalizedText TItemName102034 => null;

		public static TLocalizedText TItemDesc102034 => null;

		public static TLocalizedText TItemName102035 => null;

		public static TLocalizedText TItemDesc102035 => null;

		public static TLocalizedText TItemName102036 => null;

		public static TLocalizedText TItemDesc102036 => null;

		public static TLocalizedText TItemName102038 => null;

		public static TLocalizedText TItemDesc102038 => null;

		public static TLocalizedText TItemName102040 => null;

		public static TLocalizedText TItemDesc102040 => null;

		public static TLocalizedText TItemName201001 => null;

		public static TLocalizedText TItemDesc201001 => null;

		public static TLocalizedText TItemName201003 => null;

		public static TLocalizedText TItemDesc201003 => null;

		public static TLocalizedText TItemName201005 => null;

		public static TLocalizedText TItemDesc201005 => null;

		public static TLocalizedText TItemName201006 => null;

		public static TLocalizedText TItemDesc201006 => null;

		public static TLocalizedText TItemName201007 => null;

		public static TLocalizedText TItemDesc201007 => null;

		public static TLocalizedText TItemName201008 => null;

		public static TLocalizedText TItemDesc201008 => null;

		public static TLocalizedText TItemName201009 => null;

		public static TLocalizedText TItemDesc201009 => null;

		public static TLocalizedText TItemName201010 => null;

		public static TLocalizedText TItemDesc201010 => null;

		public static TLocalizedText TItemName201011 => null;

		public static TLocalizedText TItemDesc201011 => null;

		public static TLocalizedText TItemName201012 => null;

		public static TLocalizedText TItemDesc201012 => null;

		public static TLocalizedText TItemName201013 => null;

		public static TLocalizedText TItemDesc201013 => null;

		public static TLocalizedText TItemName201014 => null;

		public static TLocalizedText TItemDesc201014 => null;

		public static TLocalizedText TItemName201015 => null;

		public static TLocalizedText TItemDesc201015 => null;

		public static TLocalizedText TItemName201016 => null;

		public static TLocalizedText TItemDesc201016 => null;

		public static TLocalizedText TItemName201017 => null;

		public static TLocalizedText TItemDesc201017 => null;

		public static TLocalizedText TItemName201018 => null;

		public static TLocalizedText TItemDesc201018 => null;

		public static TLocalizedText TItemName201019 => null;

		public static TLocalizedText TItemDesc201019 => null;

		public static TLocalizedText TItemName201020 => null;

		public static TLocalizedText TItemDesc201020 => null;

		public static TLocalizedText TItemName201021 => null;

		public static TLocalizedText TItemDesc201021 => null;

		public static TLocalizedText TItemName201022 => null;

		public static TLocalizedText TItemDesc201022 => null;

		public static TLocalizedText TItemName201101 => null;

		public static TLocalizedText TItemDesc201101 => null;

		public static TLocalizedText TItemName201103 => null;

		public static TLocalizedText TItemDesc201103 => null;

		public static TLocalizedText TItemName201105 => null;

		public static TLocalizedText TItemDesc201105 => null;

		public static TLocalizedText TItemName201106 => null;

		public static TLocalizedText TItemDesc201106 => null;

		public static TLocalizedText TItemName201107 => null;

		public static TLocalizedText TItemDesc201107 => null;

		public static TLocalizedText TItemName201108 => null;

		public static TLocalizedText TItemDesc201108 => null;

		public static TLocalizedText TItemName201109 => null;

		public static TLocalizedText TItemDesc201109 => null;

		public static TLocalizedText TItemName201110 => null;

		public static TLocalizedText TItemDesc201110 => null;

		public static TLocalizedText TItemName201111 => null;

		public static TLocalizedText TItemDesc201111 => null;

		public static TLocalizedText TItemName201112 => null;

		public static TLocalizedText TItemDesc201112 => null;

		public static TLocalizedText TItemName201113 => null;

		public static TLocalizedText TItemDesc201113 => null;

		public static TLocalizedText TItemName201114 => null;

		public static TLocalizedText TItemDesc201114 => null;

		public static TLocalizedText TItemName201115 => null;

		public static TLocalizedText TItemDesc201115 => null;

		public static TLocalizedText TItemName201116 => null;

		public static TLocalizedText TItemDesc201116 => null;

		public static TLocalizedText TItemName201117 => null;

		public static TLocalizedText TItemDesc201117 => null;

		public static TLocalizedText TItemName201118 => null;

		public static TLocalizedText TItemDesc201118 => null;

		public static TLocalizedText TItemName201119 => null;

		public static TLocalizedText TItemDesc201119 => null;

		public static TLocalizedText TItemName201120 => null;

		public static TLocalizedText TItemDesc201120 => null;

		public static TLocalizedText TItemName201121 => null;

		public static TLocalizedText TItemDesc201121 => null;

		public static TLocalizedText TItemName201122 => null;

		public static TLocalizedText TItemDesc201122 => null;

		public static TLocalizedText TItemName201201 => null;

		public static TLocalizedText TItemDesc201201 => null;

		public static TLocalizedText TItemName201203 => null;

		public static TLocalizedText TItemDesc201203 => null;

		public static TLocalizedText TItemName201205 => null;

		public static TLocalizedText TItemDesc201205 => null;

		public static TLocalizedText TItemName201206 => null;

		public static TLocalizedText TItemDesc201206 => null;

		public static TLocalizedText TItemName201207 => null;

		public static TLocalizedText TItemDesc201207 => null;

		public static TLocalizedText TItemName201208 => null;

		public static TLocalizedText TItemDesc201208 => null;

		public static TLocalizedText TItemName201209 => null;

		public static TLocalizedText TItemDesc201209 => null;

		public static TLocalizedText TItemName201210 => null;

		public static TLocalizedText TItemDesc201210 => null;

		public static TLocalizedText TItemName201211 => null;

		public static TLocalizedText TItemDesc201211 => null;

		public static TLocalizedText TItemName201212 => null;

		public static TLocalizedText TItemDesc201212 => null;

		public static TLocalizedText TItemName201213 => null;

		public static TLocalizedText TItemDesc201213 => null;

		public static TLocalizedText TItemName201214 => null;

		public static TLocalizedText TItemDesc201214 => null;

		public static TLocalizedText TItemName201215 => null;

		public static TLocalizedText TItemDesc201215 => null;

		public static TLocalizedText TItemName201216 => null;

		public static TLocalizedText TItemDesc201216 => null;

		public static TLocalizedText TItemName201217 => null;

		public static TLocalizedText TItemDesc201217 => null;

		public static TLocalizedText TItemName201218 => null;

		public static TLocalizedText TItemDesc201218 => null;

		public static TLocalizedText TItemName201219 => null;

		public static TLocalizedText TItemDesc201219 => null;

		public static TLocalizedText TItemName201220 => null;

		public static TLocalizedText TItemDesc201220 => null;

		public static TLocalizedText TItemName201221 => null;

		public static TLocalizedText TItemDesc201221 => null;

		public static TLocalizedText TItemName201222 => null;

		public static TLocalizedText TItemDesc201222 => null;

		public static TLocalizedText TItemName201305 => null;

		public static TLocalizedText TItemDesc201305 => null;

		public static TLocalizedText TItemName201306 => null;

		public static TLocalizedText TItemDesc201306 => null;

		public static TLocalizedText TItemName201307 => null;

		public static TLocalizedText TItemDesc201307 => null;

		public static TLocalizedText TItemName201308 => null;

		public static TLocalizedText TItemDesc201308 => null;

		public static TLocalizedText TItemName201309 => null;

		public static TLocalizedText TItemDesc201309 => null;

		public static TLocalizedText TItemName201310 => null;

		public static TLocalizedText TItemDesc201310 => null;

		public static TLocalizedText TItemName201311 => null;

		public static TLocalizedText TItemDesc201311 => null;

		public static TLocalizedText TItemName201312 => null;

		public static TLocalizedText TItemDesc201312 => null;

		public static TLocalizedText TItemName201313 => null;

		public static TLocalizedText TItemDesc201313 => null;

		public static TLocalizedText TItemName201314 => null;

		public static TLocalizedText TItemDesc201314 => null;

		public static TLocalizedText TItemName201315 => null;

		public static TLocalizedText TItemDesc201315 => null;

		public static TLocalizedText TItemName201316 => null;

		public static TLocalizedText TItemDesc201316 => null;

		public static TLocalizedText TItemName201317 => null;

		public static TLocalizedText TItemDesc201317 => null;

		public static TLocalizedText TItemName201318 => null;

		public static TLocalizedText TItemDesc201318 => null;

		public static TLocalizedText TItemName201319 => null;

		public static TLocalizedText TItemDesc201319 => null;

		public static TLocalizedText TItemName201320 => null;

		public static TLocalizedText TItemDesc201320 => null;

		public static TLocalizedText TItemName201321 => null;

		public static TLocalizedText TItemDesc201321 => null;

		public static TLocalizedText TItemName201322 => null;

		public static TLocalizedText TItemDesc201322 => null;

		public static TLocalizedText TItemName220011 => null;

		public static TLocalizedText TItemDesc220011 => null;

		public static TLocalizedText TItemName220012 => null;

		public static TLocalizedText TItemDesc220012 => null;

		public static TLocalizedText TItemName220013 => null;

		public static TLocalizedText TItemDesc220013 => null;

		public static TLocalizedText TItemName220014 => null;

		public static TLocalizedText TItemDesc220014 => null;

		public static TLocalizedText TItemName220015 => null;

		public static TLocalizedText TItemDesc220015 => null;

		public static TLocalizedText TItemName220016 => null;

		public static TLocalizedText TItemDesc220016 => null;

		public static TLocalizedText TItemName220021 => null;

		public static TLocalizedText TItemDesc220021 => null;

		public static TLocalizedText TItemName220022 => null;

		public static TLocalizedText TItemDesc220022 => null;

		public static TLocalizedText TItemName220023 => null;

		public static TLocalizedText TItemDesc220023 => null;

		public static TLocalizedText TItemName220024 => null;

		public static TLocalizedText TItemDesc220024 => null;

		public static TLocalizedText TItemName220025 => null;

		public static TLocalizedText TItemDesc220025 => null;

		public static TLocalizedText TItemName220026 => null;

		public static TLocalizedText TItemDesc220026 => null;

		public static TLocalizedText TItemName220031 => null;

		public static TLocalizedText TItemDesc220031 => null;

		public static TLocalizedText TItemName220032 => null;

		public static TLocalizedText TItemDesc220032 => null;

		public static TLocalizedText TItemName220033 => null;

		public static TLocalizedText TItemDesc220033 => null;

		public static TLocalizedText TItemName220034 => null;

		public static TLocalizedText TItemDesc220034 => null;

		public static TLocalizedText TItemName220035 => null;

		public static TLocalizedText TItemDesc220035 => null;

		public static TLocalizedText TItemName220036 => null;

		public static TLocalizedText TItemDesc220036 => null;

		public static TLocalizedText TItemName220041 => null;

		public static TLocalizedText TItemDesc220041 => null;

		public static TLocalizedText TItemName220042 => null;

		public static TLocalizedText TItemDesc220042 => null;

		public static TLocalizedText TItemName220043 => null;

		public static TLocalizedText TItemDesc220043 => null;

		public static TLocalizedText TItemName220044 => null;

		public static TLocalizedText TItemDesc220044 => null;

		public static TLocalizedText TItemName220051 => null;

		public static TLocalizedText TItemDesc220051 => null;

		public static TLocalizedText TItemName220052 => null;

		public static TLocalizedText TItemDesc220052 => null;

		public static TLocalizedText TItemName220053 => null;

		public static TLocalizedText TItemDesc220053 => null;

		public static TLocalizedText TItemName220061 => null;

		public static TLocalizedText TItemDesc220061 => null;

		public static TLocalizedText TItemName220062 => null;

		public static TLocalizedText TItemDesc220062 => null;

		public static TLocalizedText TItemName220063 => null;

		public static TLocalizedText TItemDesc220063 => null;

		public static TLocalizedText TItemName220071 => null;

		public static TLocalizedText TItemDesc220071 => null;

		public static TLocalizedText TItemName220072 => null;

		public static TLocalizedText TItemDesc220072 => null;

		public static TLocalizedText TItemName220073 => null;

		public static TLocalizedText TItemDesc220073 => null;

		public static TLocalizedText TItemName220081 => null;

		public static TLocalizedText TItemDesc220081 => null;

		public static TLocalizedText TItemName220082 => null;

		public static TLocalizedText TItemDesc220082 => null;

		public static TLocalizedText TItemName220083 => null;

		public static TLocalizedText TItemDesc220083 => null;

		public static TLocalizedText TItemName220091 => null;

		public static TLocalizedText TItemDesc220091 => null;

		public static TLocalizedText TItemName220092 => null;

		public static TLocalizedText TItemDesc220092 => null;

		public static TLocalizedText TItemName220093 => null;

		public static TLocalizedText TItemDesc220093 => null;

		public static TLocalizedText TItemName221011 => null;

		public static TLocalizedText TItemDesc221011 => null;

		public static TLocalizedText TItemName221012 => null;

		public static TLocalizedText TItemDesc221012 => null;

		public static TLocalizedText TItemName221013 => null;

		public static TLocalizedText TItemDesc221013 => null;

		public static TLocalizedText TItemName221014 => null;

		public static TLocalizedText TItemDesc221014 => null;

		public static TLocalizedText TItemName221015 => null;

		public static TLocalizedText TItemDesc221015 => null;

		public static TLocalizedText TItemName221016 => null;

		public static TLocalizedText TItemDesc221016 => null;

		public static TLocalizedText TItemName221021 => null;

		public static TLocalizedText TItemDesc221021 => null;

		public static TLocalizedText TItemName221022 => null;

		public static TLocalizedText TItemDesc221022 => null;

		public static TLocalizedText TItemName221023 => null;

		public static TLocalizedText TItemDesc221023 => null;

		public static TLocalizedText TItemName221024 => null;

		public static TLocalizedText TItemDesc221024 => null;

		public static TLocalizedText TItemName221025 => null;

		public static TLocalizedText TItemDesc221025 => null;

		public static TLocalizedText TItemName221026 => null;

		public static TLocalizedText TItemDesc221026 => null;

		public static TLocalizedText TItemName221031 => null;

		public static TLocalizedText TItemDesc221031 => null;

		public static TLocalizedText TItemName221032 => null;

		public static TLocalizedText TItemDesc221032 => null;

		public static TLocalizedText TItemName221033 => null;

		public static TLocalizedText TItemDesc221033 => null;

		public static TLocalizedText TItemName221034 => null;

		public static TLocalizedText TItemDesc221034 => null;

		public static TLocalizedText TItemName221041 => null;

		public static TLocalizedText TItemDesc221041 => null;

		public static TLocalizedText TItemName221042 => null;

		public static TLocalizedText TItemDesc221042 => null;

		public static TLocalizedText TItemName221043 => null;

		public static TLocalizedText TItemDesc221043 => null;

		public static TLocalizedText TItemName222011 => null;

		public static TLocalizedText TItemDesc222011 => null;

		public static TLocalizedText TItemName222012 => null;

		public static TLocalizedText TItemDesc222012 => null;

		public static TLocalizedText TItemName222013 => null;

		public static TLocalizedText TItemDesc222013 => null;

		public static TLocalizedText TItemName222014 => null;

		public static TLocalizedText TItemDesc222014 => null;

		public static TLocalizedText TItemName222015 => null;

		public static TLocalizedText TItemDesc222015 => null;

		public static TLocalizedText TItemName222016 => null;

		public static TLocalizedText TItemDesc222016 => null;

		public static TLocalizedText TItemName223011 => null;

		public static TLocalizedText TItemDesc223011 => null;

		public static TLocalizedText TItemName223012 => null;

		public static TLocalizedText TItemDesc223012 => null;

		public static TLocalizedText TItemName223013 => null;

		public static TLocalizedText TItemDesc223013 => null;

		public static TLocalizedText TItemName223014 => null;

		public static TLocalizedText TItemDesc223014 => null;

		public static TLocalizedText TItemName223021 => null;

		public static TLocalizedText TItemDesc223021 => null;

		public static TLocalizedText TItemName223022 => null;

		public static TLocalizedText TItemDesc223022 => null;

		public static TLocalizedText TItemName223023 => null;

		public static TLocalizedText TItemDesc223023 => null;

		public static TLocalizedText TItemName223031 => null;

		public static TLocalizedText TItemDesc223031 => null;

		public static TLocalizedText TItemName223032 => null;

		public static TLocalizedText TItemDesc223032 => null;

		public static TLocalizedText TItemName223033 => null;

		public static TLocalizedText TItemDesc223033 => null;

		public static TLocalizedText TItemName223041 => null;

		public static TLocalizedText TItemDesc223041 => null;

		public static TLocalizedText TItemName223042 => null;

		public static TLocalizedText TItemDesc223042 => null;

		public static TLocalizedText TItemName223043 => null;

		public static TLocalizedText TItemDesc223043 => null;

		public static TLocalizedText TItemName223061 => null;

		public static TLocalizedText TItemDesc223061 => null;

		public static TLocalizedText TItemName223062 => null;

		public static TLocalizedText TItemDesc223062 => null;

		public static TLocalizedText TItemName223063 => null;

		public static TLocalizedText TItemDesc223063 => null;

		public static TLocalizedText TItemName223071 => null;

		public static TLocalizedText TItemDesc223071 => null;

		public static TLocalizedText TItemName223072 => null;

		public static TLocalizedText TItemDesc223072 => null;

		public static TLocalizedText TItemName223073 => null;

		public static TLocalizedText TItemDesc223073 => null;

		public static TLocalizedText TItemName223081 => null;

		public static TLocalizedText TItemDesc223081 => null;

		public static TLocalizedText TItemName223082 => null;

		public static TLocalizedText TItemDesc223082 => null;

		public static TLocalizedText TItemName223083 => null;

		public static TLocalizedText TItemDesc223083 => null;

		public static TLocalizedText TItemName223091 => null;

		public static TLocalizedText TItemDesc223091 => null;

		public static TLocalizedText TItemName223092 => null;

		public static TLocalizedText TItemDesc223092 => null;

		public static TLocalizedText TItemName223093 => null;

		public static TLocalizedText TItemDesc223093 => null;

		public static TLocalizedText TItemName223101 => null;

		public static TLocalizedText TItemDesc223101 => null;

		public static TLocalizedText TItemName223102 => null;

		public static TLocalizedText TItemDesc223102 => null;

		public static TLocalizedText TItemName223103 => null;

		public static TLocalizedText TItemDesc223103 => null;

		public static TLocalizedText TItemName223111 => null;

		public static TLocalizedText TItemDesc223111 => null;

		public static TLocalizedText TItemName223112 => null;

		public static TLocalizedText TItemDesc223112 => null;

		public static TLocalizedText TItemName223113 => null;

		public static TLocalizedText TItemDesc223113 => null;

		public static TLocalizedText TItemName223121 => null;

		public static TLocalizedText TItemDesc223121 => null;

		public static TLocalizedText TItemName223122 => null;

		public static TLocalizedText TItemDesc223122 => null;

		public static TLocalizedText TItemName223123 => null;

		public static TLocalizedText TItemDesc223123 => null;

		public static TLocalizedText TItemName224051 => null;

		public static TLocalizedText TItemDesc224051 => null;

		public static TLocalizedText TItemName224052 => null;

		public static TLocalizedText TItemDesc224052 => null;

		public static TLocalizedText TItemName224053 => null;

		public static TLocalizedText TItemDesc224053 => null;

		public static TLocalizedText TItemName225011 => null;

		public static TLocalizedText TItemDesc225011 => null;

		public static TLocalizedText TItemName225012 => null;

		public static TLocalizedText TItemDesc225012 => null;

		public static TLocalizedText TItemName225013 => null;

		public static TLocalizedText TItemDesc225013 => null;

		public static TLocalizedText TItemName225014 => null;

		public static TLocalizedText TItemDesc225014 => null;

		public static TLocalizedText TItemName225015 => null;

		public static TLocalizedText TItemDesc225015 => null;

		public static TLocalizedText TItemName225021 => null;

		public static TLocalizedText TItemDesc225021 => null;

		public static TLocalizedText TItemName225022 => null;

		public static TLocalizedText TItemDesc225022 => null;

		public static TLocalizedText TItemName225023 => null;

		public static TLocalizedText TItemDesc225023 => null;

		public static TLocalizedText TItemName225024 => null;

		public static TLocalizedText TItemDesc225024 => null;

		public static TLocalizedText TItemName225025 => null;

		public static TLocalizedText TItemDesc225025 => null;

		public static TLocalizedText TItemName225031 => null;

		public static TLocalizedText TItemDesc225031 => null;

		public static TLocalizedText TItemName225032 => null;

		public static TLocalizedText TItemDesc225032 => null;

		public static TLocalizedText TItemName225033 => null;

		public static TLocalizedText TItemDesc225033 => null;

		public static TLocalizedText TItemName225034 => null;

		public static TLocalizedText TItemDesc225034 => null;

		public static TLocalizedText TItemName225035 => null;

		public static TLocalizedText TItemDesc225035 => null;

		public static TLocalizedText TItemName225041 => null;

		public static TLocalizedText TItemDesc225041 => null;

		public static TLocalizedText TItemName225042 => null;

		public static TLocalizedText TItemDesc225042 => null;

		public static TLocalizedText TItemName225043 => null;

		public static TLocalizedText TItemDesc225043 => null;

		public static TLocalizedText TItemName225044 => null;

		public static TLocalizedText TItemDesc225044 => null;

		public static TLocalizedText TItemName225051 => null;

		public static TLocalizedText TItemDesc225051 => null;

		public static TLocalizedText TItemName225052 => null;

		public static TLocalizedText TItemDesc225052 => null;

		public static TLocalizedText TItemName225053 => null;

		public static TLocalizedText TItemDesc225053 => null;

		public static TLocalizedText TItemName225054 => null;

		public static TLocalizedText TItemDesc225054 => null;

		public static TLocalizedText TItemName225071 => null;

		public static TLocalizedText TItemDesc225071 => null;

		public static TLocalizedText TItemName225072 => null;

		public static TLocalizedText TItemDesc225072 => null;

		public static TLocalizedText TItemName225073 => null;

		public static TLocalizedText TItemDesc225073 => null;

		public static TLocalizedText TItemName225074 => null;

		public static TLocalizedText TItemDesc225074 => null;

		public static TLocalizedText TItemName226011 => null;

		public static TLocalizedText TItemDesc226011 => null;

		public static TLocalizedText TItemName226012 => null;

		public static TLocalizedText TItemDesc226012 => null;

		public static TLocalizedText TItemName226022 => null;

		public static TLocalizedText TItemDesc226022 => null;

		public static TLocalizedText TItemName226031 => null;

		public static TLocalizedText TItemDesc226031 => null;

		public static TLocalizedText TItemName226032 => null;

		public static TLocalizedText TItemDesc226032 => null;

		public static TLocalizedText TItemName226042 => null;

		public static TLocalizedText TItemDesc226042 => null;

		public static TLocalizedText TItemName226051 => null;

		public static TLocalizedText TItemDesc226051 => null;

		public static TLocalizedText TItemName226052 => null;

		public static TLocalizedText TItemDesc226052 => null;

		public static TLocalizedText TItemName226062 => null;

		public static TLocalizedText TItemDesc226062 => null;

		public static TLocalizedText TItemName226071 => null;

		public static TLocalizedText TItemDesc226071 => null;

		public static TLocalizedText TItemName226072 => null;

		public static TLocalizedText TItemDesc226072 => null;

		public static TLocalizedText TItemName226073 => null;

		public static TLocalizedText TItemDesc226073 => null;

		public static TLocalizedText TItemName226081 => null;

		public static TLocalizedText TItemDesc226081 => null;

		public static TLocalizedText TItemName226082 => null;

		public static TLocalizedText TItemDesc226082 => null;

		public static TLocalizedText TItemName226083 => null;

		public static TLocalizedText TItemDesc226083 => null;

		public static TLocalizedText TItemName226091 => null;

		public static TLocalizedText TItemDesc226091 => null;

		public static TLocalizedText TItemName226101 => null;

		public static TLocalizedText TItemDesc226101 => null;

		public static TLocalizedText TItemName226102 => null;

		public static TLocalizedText TItemDesc226102 => null;

		public static TLocalizedText TItemName300000 => null;

		public static TLocalizedText TItemDesc300000 => null;

		public static TLocalizedText TItemName300001 => null;

		public static TLocalizedText TItemDesc300001 => null;

		public static TLocalizedText TItemName300002 => null;

		public static TLocalizedText TItemDesc300002 => null;

		public static TLocalizedText TItemName300003 => null;

		public static TLocalizedText TItemDesc300003 => null;

		public static TLocalizedText TItemName300004 => null;

		public static TLocalizedText TItemDesc300004 => null;

		public static TLocalizedText TItemName300005 => null;

		public static TLocalizedText TItemDesc300005 => null;

		public static TLocalizedText TItemName300006 => null;

		public static TLocalizedText TItemDesc300006 => null;

		public static TLocalizedText TItemName300007 => null;

		public static TLocalizedText TItemDesc300007 => null;

		public static TLocalizedText TItemName300008 => null;

		public static TLocalizedText TItemDesc300008 => null;

		public static TLocalizedText TItemName300009 => null;

		public static TLocalizedText TItemDesc300009 => null;

		public static TLocalizedText TItemName300010 => null;

		public static TLocalizedText TItemDesc300010 => null;

		public static TLocalizedText TItemName300011 => null;

		public static TLocalizedText TItemDesc300011 => null;

		public static TLocalizedText TItemName300012 => null;

		public static TLocalizedText TItemDesc300012 => null;

		public static TLocalizedText TItemName300013 => null;

		public static TLocalizedText TItemDesc300013 => null;

		public static TLocalizedText TItemName300014 => null;

		public static TLocalizedText TItemDesc300014 => null;

		public static TLocalizedText TItemName300015 => null;

		public static TLocalizedText TItemDesc300015 => null;

		public static TLocalizedText TItemName300016 => null;

		public static TLocalizedText TItemDesc300016 => null;

		public static TLocalizedText TItemName301001 => null;

		public static TLocalizedText TItemDesc301001 => null;

		public static TLocalizedText TItemName301002 => null;

		public static TLocalizedText TItemDesc301002 => null;

		public static TLocalizedText TItemName301003 => null;

		public static TLocalizedText TItemDesc301003 => null;

		public static TLocalizedText TItemName301004 => null;

		public static TLocalizedText TItemDesc301004 => null;

		public static TLocalizedText TItemName301005 => null;

		public static TLocalizedText TItemDesc301005 => null;

		public static TLocalizedText TItemName301006 => null;

		public static TLocalizedText TItemDesc301006 => null;

		public static TLocalizedText TItemName301007 => null;

		public static TLocalizedText TItemDesc301007 => null;

		public static TLocalizedText TItemName301008 => null;

		public static TLocalizedText TItemDesc301008 => null;

		public static TLocalizedText TItemName301009 => null;

		public static TLocalizedText TItemDesc301009 => null;

		public static TLocalizedText TItemName301010 => null;

		public static TLocalizedText TItemDesc301010 => null;

		public static TLocalizedText TItemName301011 => null;

		public static TLocalizedText TItemDesc301011 => null;

		public static TLocalizedText TItemName302001 => null;

		public static TLocalizedText TItemDesc302001 => null;

		public static TLocalizedText TItemName302002 => null;

		public static TLocalizedText TItemDesc302002 => null;

		public static TLocalizedText TItemName302003 => null;

		public static TLocalizedText TItemDesc302003 => null;

		public static TLocalizedText TItemName302004 => null;

		public static TLocalizedText TItemDesc302004 => null;

		public static TLocalizedText TItemName302005 => null;

		public static TLocalizedText TItemDesc302005 => null;

		public static TLocalizedText TItemName302006 => null;

		public static TLocalizedText TItemDesc302006 => null;

		public static TLocalizedText TItemName302007 => null;

		public static TLocalizedText TItemDesc302007 => null;

		public static TLocalizedText TItemName302008 => null;

		public static TLocalizedText TItemDesc302008 => null;

		public static TLocalizedText TItemName302009 => null;

		public static TLocalizedText TItemDesc302009 => null;

		public static TLocalizedText TItemName303001 => null;

		public static TLocalizedText TItemDesc303001 => null;

		public static TLocalizedText TItemName303002 => null;

		public static TLocalizedText TItemDesc303002 => null;

		public static TLocalizedText TItemName303003 => null;

		public static TLocalizedText TItemDesc303003 => null;

		public static TLocalizedText TItemName310001 => null;

		public static TLocalizedText TItemDesc310001 => null;

		public static TLocalizedText TItemName310002 => null;

		public static TLocalizedText TItemDesc310002 => null;

		public static TLocalizedText TItemName310003 => null;

		public static TLocalizedText TItemDesc310003 => null;

		public static TLocalizedText TItemName310004 => null;

		public static TLocalizedText TItemDesc310004 => null;

		public static TLocalizedText TItemName310005 => null;

		public static TLocalizedText TItemDesc310005 => null;

		public static TLocalizedText TItemName310006 => null;

		public static TLocalizedText TItemDesc310006 => null;

		public static TLocalizedText TItemName310007 => null;

		public static TLocalizedText TItemDesc310007 => null;

		public static TLocalizedText TItemName310008 => null;

		public static TLocalizedText TItemDesc310008 => null;

		public static TLocalizedText TItemName310009 => null;

		public static TLocalizedText TItemDesc310009 => null;

		public static TLocalizedText TItemName310010 => null;

		public static TLocalizedText TItemDesc310010 => null;

		public static TLocalizedText TItemName310011 => null;

		public static TLocalizedText TItemDesc310011 => null;

		public static TLocalizedText TItemName310012 => null;

		public static TLocalizedText TItemDesc310012 => null;

		public static TLocalizedText TItemName310013 => null;

		public static TLocalizedText TItemDesc310013 => null;

		public static TLocalizedText TItemName310014 => null;

		public static TLocalizedText TItemDesc310014 => null;

		public static TLocalizedText TItemName310015 => null;

		public static TLocalizedText TItemDesc310015 => null;

		public static TLocalizedText TItemName310051 => null;

		public static TLocalizedText TItemDesc310051 => null;

		public static TLocalizedText TItemName310052 => null;

		public static TLocalizedText TItemDesc310052 => null;

		public static TLocalizedText TItemName310053 => null;

		public static TLocalizedText TItemDesc310053 => null;

		public static TLocalizedText TItemName310054 => null;

		public static TLocalizedText TItemDesc310054 => null;

		public static TLocalizedText TItemName310055 => null;

		public static TLocalizedText TItemDesc310055 => null;

		public static TLocalizedText TItemName310056 => null;

		public static TLocalizedText TItemDesc310056 => null;

		public static TLocalizedText TItemName310057 => null;

		public static TLocalizedText TItemDesc310057 => null;

		public static TLocalizedText TItemName310058 => null;

		public static TLocalizedText TItemDesc310058 => null;

		public static TLocalizedText TItemName310059 => null;

		public static TLocalizedText TItemDesc310059 => null;

		public static TLocalizedText TItemName310060 => null;

		public static TLocalizedText TItemDesc310060 => null;

		public static TLocalizedText TItemName310061 => null;

		public static TLocalizedText TItemDesc310061 => null;

		public static TLocalizedText TItemName310062 => null;

		public static TLocalizedText TItemDesc310062 => null;

		public static TLocalizedText TItemName310101 => null;

		public static TLocalizedText TItemDesc310101 => null;

		public static TLocalizedText TItemNote310101 => null;

		public static TLocalizedText TItemName310102 => null;

		public static TLocalizedText TItemDesc310102 => null;

		public static TLocalizedText TItemNote310102 => null;

		public static TLocalizedText TItemName310103 => null;

		public static TLocalizedText TItemDesc310103 => null;

		public static TLocalizedText TItemNote310103 => null;

		public static TLocalizedText TItemName310104 => null;

		public static TLocalizedText TItemDesc310104 => null;

		public static TLocalizedText TItemNote310104 => null;

		public static TLocalizedText TItemName310105 => null;

		public static TLocalizedText TItemDesc310105 => null;

		public static TLocalizedText TItemNote310105 => null;

		public static TLocalizedText TItemName310106 => null;

		public static TLocalizedText TItemDesc310106 => null;

		public static TLocalizedText TItemNote310106 => null;

		public static TLocalizedText TItemName310107 => null;

		public static TLocalizedText TItemDesc310107 => null;

		public static TLocalizedText TItemNote310107 => null;

		public static TLocalizedText TItemName310108 => null;

		public static TLocalizedText TItemDesc310108 => null;

		public static TLocalizedText TItemNote310108 => null;

		public static TLocalizedText TItemName310109 => null;

		public static TLocalizedText TItemDesc310109 => null;

		public static TLocalizedText TItemNote310109 => null;

		public static TLocalizedText TItemName310110 => null;

		public static TLocalizedText TItemDesc310110 => null;

		public static TLocalizedText TItemNote310110 => null;

		public static TLocalizedText TItemName310111 => null;

		public static TLocalizedText TItemDesc310111 => null;

		public static TLocalizedText TItemNote310111 => null;

		public static TLocalizedText TItemName310112 => null;

		public static TLocalizedText TItemDesc310112 => null;

		public static TLocalizedText TItemNote310112 => null;

		public static TLocalizedText TItemName310113 => null;

		public static TLocalizedText TItemDesc310113 => null;

		public static TLocalizedText TItemNote310113 => null;

		public static TLocalizedText TItemName310114 => null;

		public static TLocalizedText TItemDesc310114 => null;

		public static TLocalizedText TItemNote310114 => null;

		public static TLocalizedText TItemName310115 => null;

		public static TLocalizedText TItemDesc310115 => null;

		public static TLocalizedText TItemNote310115 => null;

		public static TLocalizedText TItemName310151 => null;

		public static TLocalizedText TItemDesc310151 => null;

		public static TLocalizedText TItemNote310151 => null;

		public static TLocalizedText TItemName310152 => null;

		public static TLocalizedText TItemDesc310152 => null;

		public static TLocalizedText TItemNote310152 => null;

		public static TLocalizedText TItemName310153 => null;

		public static TLocalizedText TItemDesc310153 => null;

		public static TLocalizedText TItemNote310153 => null;

		public static TLocalizedText TItemName310154 => null;

		public static TLocalizedText TItemDesc310154 => null;

		public static TLocalizedText TItemNote310154 => null;

		public static TLocalizedText TItemName310155 => null;

		public static TLocalizedText TItemDesc310155 => null;

		public static TLocalizedText TItemNote310155 => null;

		public static TLocalizedText TItemName310156 => null;

		public static TLocalizedText TItemDesc310156 => null;

		public static TLocalizedText TItemNote310156 => null;

		public static TLocalizedText TItemName310157 => null;

		public static TLocalizedText TItemDesc310157 => null;

		public static TLocalizedText TItemNote310157 => null;

		public static TLocalizedText TItemName310158 => null;

		public static TLocalizedText TItemDesc310158 => null;

		public static TLocalizedText TItemNote310158 => null;

		public static TLocalizedText TItemName310159 => null;

		public static TLocalizedText TItemDesc310159 => null;

		public static TLocalizedText TItemNote310159 => null;

		public static TLocalizedText TItemName310160 => null;

		public static TLocalizedText TItemDesc310160 => null;

		public static TLocalizedText TItemNote310160 => null;

		public static TLocalizedText TItemName310161 => null;

		public static TLocalizedText TItemDesc310161 => null;

		public static TLocalizedText TItemNote310161 => null;

		public static TLocalizedText TItemName310162 => null;

		public static TLocalizedText TItemDesc310162 => null;

		public static TLocalizedText TItemNote310162 => null;

		public static TLocalizedText TItemName401102 => null;

		public static TLocalizedText TItemDesc401102 => null;

		public static TLocalizedText TItemNote401102 => null;

		public static TLocalizedText TItemName401103 => null;

		public static TLocalizedText TItemDesc401103 => null;

		public static TLocalizedText TItemNote401103 => null;

		public static TLocalizedText TItemName401104 => null;

		public static TLocalizedText TItemDesc401104 => null;

		public static TLocalizedText TItemNote401104 => null;

		public static TLocalizedText TItemName401105 => null;

		public static TLocalizedText TItemDesc401105 => null;

		public static TLocalizedText TItemNote401105 => null;

		public static TLocalizedText TItemName401109 => null;

		public static TLocalizedText TItemDesc401109 => null;

		public static TLocalizedText TItemNote401109 => null;

		public static TLocalizedText TItemName401110 => null;

		public static TLocalizedText TItemDesc401110 => null;

		public static TLocalizedText TItemNote401110 => null;

		public static TLocalizedText TItemName401111 => null;

		public static TLocalizedText TItemDesc401111 => null;

		public static TLocalizedText TItemNote401111 => null;

		public static TLocalizedText TItemName401112 => null;

		public static TLocalizedText TItemDesc401112 => null;

		public static TLocalizedText TItemNote401112 => null;

		public static TLocalizedText TItemName401113 => null;

		public static TLocalizedText TItemDesc401113 => null;

		public static TLocalizedText TItemNote401113 => null;

		public static TLocalizedText TItemName401114 => null;

		public static TLocalizedText TItemDesc401114 => null;

		public static TLocalizedText TItemNote401114 => null;

		public static TLocalizedText TItemName401116 => null;

		public static TLocalizedText TItemDesc401116 => null;

		public static TLocalizedText TItemNote401116 => null;

		public static TLocalizedText TItemName401117 => null;

		public static TLocalizedText TItemDesc401117 => null;

		public static TLocalizedText TItemNote401117 => null;

		public static TLocalizedText TItemName401118 => null;

		public static TLocalizedText TItemDesc401118 => null;

		public static TLocalizedText TItemNote401118 => null;

		public static TLocalizedText TItemName401119 => null;

		public static TLocalizedText TItemDesc401119 => null;

		public static TLocalizedText TItemNote401119 => null;

		public static TLocalizedText TItemName401120 => null;

		public static TLocalizedText TItemDesc401120 => null;

		public static TLocalizedText TItemNote401120 => null;

		public static TLocalizedText TItemName401121 => null;

		public static TLocalizedText TItemDesc401121 => null;

		public static TLocalizedText TItemNote401121 => null;

		public static TLocalizedText TItemName401122 => null;

		public static TLocalizedText TItemDesc401122 => null;

		public static TLocalizedText TItemNote401122 => null;

		public static TLocalizedText TItemName401201 => null;

		public static TLocalizedText TItemDesc401201 => null;

		public static TLocalizedText TItemNote401201 => null;

		public static TLocalizedText TItemName401202 => null;

		public static TLocalizedText TItemDesc401202 => null;

		public static TLocalizedText TItemNote401202 => null;

		public static TLocalizedText TItemName401203 => null;

		public static TLocalizedText TItemDesc401203 => null;

		public static TLocalizedText TItemNote401203 => null;

		public static TLocalizedText TItemName401204 => null;

		public static TLocalizedText TItemDesc401204 => null;

		public static TLocalizedText TItemNote401204 => null;

		public static TLocalizedText TItemName401205 => null;

		public static TLocalizedText TItemDesc401205 => null;

		public static TLocalizedText TItemNote401205 => null;

		public static TLocalizedText TItemName401209 => null;

		public static TLocalizedText TItemDesc401209 => null;

		public static TLocalizedText TItemNote401209 => null;

		public static TLocalizedText TItemName401211 => null;

		public static TLocalizedText TItemDesc401211 => null;

		public static TLocalizedText TItemNote401211 => null;

		public static TLocalizedText TItemName401212 => null;

		public static TLocalizedText TItemDesc401212 => null;

		public static TLocalizedText TItemNote401212 => null;

		public static TLocalizedText TItemName401213 => null;

		public static TLocalizedText TItemDesc401213 => null;

		public static TLocalizedText TItemNote401213 => null;

		public static TLocalizedText TItemName401214 => null;

		public static TLocalizedText TItemDesc401214 => null;

		public static TLocalizedText TItemNote401214 => null;

		public static TLocalizedText TItemName401215 => null;

		public static TLocalizedText TItemDesc401215 => null;

		public static TLocalizedText TItemNote401215 => null;

		public static TLocalizedText TItemName500001 => null;

		public static TLocalizedText TItemDesc500001 => null;

		public static TLocalizedText TItemName500002 => null;

		public static TLocalizedText TItemDesc500002 => null;

		public static TLocalizedText TItemName500003 => null;

		public static TLocalizedText TItemDesc500003 => null;

		public static TLocalizedText TItemName500004 => null;

		public static TLocalizedText TItemDesc500004 => null;

		public static TLocalizedText TItemName500005 => null;

		public static TLocalizedText TItemDesc500005 => null;

		public static TLocalizedText TItemName500006 => null;

		public static TLocalizedText TItemDesc500006 => null;

		public static TLocalizedText TItemName500007 => null;

		public static TLocalizedText TItemDesc500007 => null;

		public static TLocalizedText TItemName500008 => null;

		public static TLocalizedText TItemDesc500008 => null;

		public static TLocalizedText TItemName500009 => null;

		public static TLocalizedText TItemDesc500009 => null;

		public static TLocalizedText TItemName500010 => null;

		public static TLocalizedText TItemDesc500010 => null;

		public static TLocalizedText TItemName500011 => null;

		public static TLocalizedText TItemDesc500011 => null;

		public static TLocalizedText TItemName500012 => null;

		public static TLocalizedText TItemDesc500012 => null;

		public static TLocalizedText TItemName500013 => null;

		public static TLocalizedText TItemDesc500013 => null;

		public static TLocalizedText TItemName500014 => null;

		public static TLocalizedText TItemDesc500014 => null;

		public static TLocalizedText TItemName500015 => null;

		public static TLocalizedText TItemDesc500015 => null;

		public static TLocalizedText TItemName500016 => null;

		public static TLocalizedText TItemDesc500016 => null;

		public static TLocalizedText TItemName500017 => null;

		public static TLocalizedText TItemDesc500017 => null;

		public static TLocalizedText TItemName500018 => null;

		public static TLocalizedText TItemDesc500018 => null;

		public static TLocalizedText TItemName500019 => null;

		public static TLocalizedText TItemDesc500019 => null;

		public static TLocalizedText TItemName500020 => null;

		public static TLocalizedText TItemDesc500020 => null;

		public static TLocalizedText TItemName500021 => null;

		public static TLocalizedText TItemDesc500021 => null;

		public static TLocalizedText TItemName500022 => null;

		public static TLocalizedText TItemDesc500022 => null;

		public static TLocalizedText TItemName500023 => null;

		public static TLocalizedText TItemDesc500023 => null;

		public static TLocalizedText TLoadingTextTipText101 => null;

		public static TLocalizedText TLoadingTextTipText102 => null;

		public static TLocalizedText TLoadingTextTipText103 => null;

		public static TLocalizedText TLoadingTextTipText104 => null;

		public static TLocalizedText TLoadingTextTipText105 => null;

		public static TLocalizedText TLoadingTextTipText106 => null;

		public static TLocalizedText TLoadingTextTipText107 => null;

		public static TLocalizedText TLoadingTextTipText108 => null;

		public static TLocalizedText TLoadingTextTipText109 => null;

		public static TLocalizedText TLoadingTextTipText110 => null;

		public static TLocalizedText TLoadingTextTipText111 => null;

		public static TLocalizedText TLoadingTextTipText112 => null;

		public static TLocalizedText TLoadingTextTipText113 => null;

		public static TLocalizedText TLoadingTextTipText114 => null;

		public static TLocalizedText TLoadingTextTipText115 => null;

		public static TLocalizedText TLoadingTextTipText116 => null;

		public static TLocalizedText TLoadingTextTipText117 => null;

		public static TLocalizedText TLoadingTextTipText118 => null;

		public static TLocalizedText TLoadingTextTipText119 => null;

		public static TLocalizedText TLoadingTextTipText120 => null;

		public static TLocalizedText TLoadingTextTipText121 => null;

		public static TLocalizedText TLoadingTextTipText122 => null;

		public static TLocalizedText TLoadingTextTipText123 => null;

		public static TLocalizedText TLoadingTextTipText124 => null;

		public static TLocalizedText TLoadingTextTipText125 => null;

		public static TLocalizedText TLoadingTextTipText126 => null;

		public static TLocalizedText TLoadingTextTipText127 => null;

		public static TLocalizedText TLoadingTextTipText1001 => null;

		public static TLocalizedText TLoadingTextTipText1002 => null;

		public static TLocalizedText TLoadingTextTipText1003 => null;

		public static TLocalizedText TLoadingTextTipText1004 => null;

		public static TLocalizedText TLoadingTextTipText1005 => null;

		public static TLocalizedText TLoadingTextTipText1006 => null;

		public static TLocalizedText TLoadingTextTipText1007 => null;

		public static TLocalizedText TLoadingTextTipText1008 => null;

		public static TLocalizedText TLoadingTextTipText1009 => null;

		public static TLocalizedText TLoadingTextTipText1010 => null;

		public static TLocalizedText TLoadingTextTipText1011 => null;

		public static TLocalizedText TLoadingTextTipText1012 => null;

		public static TLocalizedText TLoadingTextTipText1013 => null;

		public static TLocalizedText TLoadingTextTipText1014 => null;

		public static TLocalizedText TLoadingTextTipText1015 => null;

		public static TLocalizedText TLoadingTextTipText1016 => null;

		public static TLocalizedText TLoadingTextTipText1017 => null;

		public static TLocalizedText TLoadingTextTipText1018 => null;

		public static TLocalizedText TLoadingTextTipText1019 => null;

		public static TLocalizedText TLoadingTextTipText1020 => null;

		public static TLocalizedText TLoadingTextTipText1021 => null;

		public static TLocalizedText TLoadingTextTipText1022 => null;

		public static TLocalizedText TLoadingTextTipText1023 => null;

		public static TLocalizedText TLoadingTextTipText1024 => null;

		public static TLocalizedText TLoadingTextTipText1025 => null;

		public static TLocalizedText TLoadingTextTipText1026 => null;

		public static TLocalizedText TLoadingTextTipText1027 => null;

		public static TLocalizedText TLoadingTextTipText2001 => null;

		public static TLocalizedText TLoadingTextTipText2002 => null;

		public static TLocalizedText TLoadingTextTipText2003 => null;

		public static TLocalizedText TLoadingTextTipText2004 => null;

		public static TLocalizedText TLoadingTextTipText2005 => null;

		public static TLocalizedText TLoadingTextTipText2006 => null;

		public static TLocalizedText TLoadingTextTipText2007 => null;

		public static TLocalizedText TLoadingTextTipText2008 => null;

		public static TLocalizedText TLoadingTextTipText2009 => null;

		public static TLocalizedText TLoadingTextTipText2010 => null;

		public static TLocalizedText TLoadingTextTipText2011 => null;

		public static TLocalizedText TLoadingTextTipText2012 => null;

		public static TLocalizedText TLoadingTextTipText2013 => null;

		public static TLocalizedText TLoadingTextTipText2014 => null;

		public static TLocalizedText TLoadingTextTipText2015 => null;

		public static TLocalizedText TLoadingTextTipText2016 => null;

		public static TLocalizedText TLoadingTextTipText2017 => null;

		public static TLocalizedText TLoadingTextTipText3001 => null;

		public static TLocalizedText TLoadingTextTipText3002 => null;

		public static TLocalizedText TLoadingTextTipText3003 => null;

		public static TLocalizedText TLoadingTextTipText3004 => null;

		public static TLocalizedText TLoadingTextTipText3005 => null;

		public static TLocalizedText TLoadingTextTipText3006 => null;

		public static TLocalizedText TLoadingTextTipText3007 => null;

		public static TLocalizedText TLoadingTextTipText3008 => null;

		public static TLocalizedText TLoadingTextTipText3009 => null;

		public static TLocalizedText TLoadingTextTipText3010 => null;

		public static TLocalizedText TLoadingTextTipText3011 => null;

		public static TLocalizedText TLoadingTextTipText3012 => null;

		public static TLocalizedText TLoadingTextTipText3013 => null;

		public static TLocalizedText TLoadingTextTipText3014 => null;

		public static TLocalizedText TLoadingTextTipText3015 => null;

		public static TLocalizedText TLoadingTextTipText3016 => null;

		public static TLocalizedText TLoadingTextTipText3017 => null;

		public static TLocalizedText TLoadingTextTipText3018 => null;

		public static TLocalizedText TLoadingTextTipText3019 => null;

		public static TLocalizedText TLoadingTextTipText3020 => null;

		public static TLocalizedText TLoadingTextTipText3021 => null;

		public static TLocalizedText TLoadingTextTipText3022 => null;

		public static TLocalizedText TLoungeBgmName1001 => null;

		public static TLocalizedText TLoungeBgmName1002 => null;

		public static TLocalizedText TLoungeBgmName1003 => null;

		public static TLocalizedText TLoungeBgmName1004 => null;

		public static TLocalizedText TLoungeBgmName2001 => null;

		public static TLocalizedText TLoungeBgmName2002 => null;

		public static TLocalizedText TLoungeBgmName2003 => null;

		public static TLocalizedText TLoungeBgmName2004 => null;

		public static TLocalizedText TLoungeBgmName3001 => null;

		public static TLocalizedText TLoungeBgmName3002 => null;

		public static TLocalizedText TLoungeBgmName3003 => null;

		public static TLocalizedText TLoungeBgmName4001 => null;

		public static TLocalizedText TLoungeBgmName4002 => null;

		public static TLocalizedText TLoungeBgmName4003 => null;

		public static TLocalizedText TLoungeBgmName4004 => null;

		public static TLocalizedText TLoungeBgmName4100 => null;

		public static TLocalizedText TLoungeBgmName5001 => null;

		public static TLocalizedText TLoungeBgmName5002 => null;

		public static TLocalizedText TLoungeBgmName5003 => null;

		public static TLocalizedText TLoungeBgmName5051 => null;

		public static TLocalizedText TLoungeBgmName5052 => null;

		public static TLocalizedText TLoungeBgmName5053 => null;

		public static TLocalizedText TLoungeBgmName5054 => null;

		public static TLocalizedText TLoungeBgmName5055 => null;

		public static TLocalizedText TLoungeBgmName5056 => null;

		public static TLocalizedText TLoungeBgmName5101 => null;

		public static TLocalizedText TLoungeBgmName5102 => null;

		public static TLocalizedText TLoungeBgmName5103 => null;

		public static TLocalizedText TLoungeBgmName5104 => null;

		public static TLocalizedText TLoungeBgmName5105 => null;

		public static TLocalizedText TLoungeBgmName5106 => null;

		public static TLocalizedText TLoungeQuestName80002 => null;

		public static TLocalizedText TLoungeQuestDesc80002 => null;

		public static TLocalizedText TLoungeQuestName80003 => null;

		public static TLocalizedText TLoungeQuestDesc80003 => null;

		public static TLocalizedText TLoungeQuestName80004 => null;

		public static TLocalizedText TLoungeQuestDesc80004 => null;

		public static TLocalizedText TLoungeQuestName80005 => null;

		public static TLocalizedText TLoungeQuestDesc80005 => null;

		public static TLocalizedText TLoungeQuestName80006 => null;

		public static TLocalizedText TLoungeQuestDesc80006 => null;

		public static TLocalizedText TLoungeQuestName80007 => null;

		public static TLocalizedText TLoungeQuestDesc80007 => null;

		public static TLocalizedText TLoungeQuestName80100 => null;

		public static TLocalizedText TLoungeQuestDesc80100 => null;

		public static TLocalizedText TLoungeQuestName80101 => null;

		public static TLocalizedText TLoungeQuestDesc80101 => null;

		public static TLocalizedText TLoungeQuestName80102 => null;

		public static TLocalizedText TLoungeQuestDesc80102 => null;

		public static TLocalizedText TLoungeQuestName80103 => null;

		public static TLocalizedText TLoungeQuestDesc80103 => null;

		public static TLocalizedText TLoungeQuestName80104 => null;

		public static TLocalizedText TLoungeQuestDesc80104 => null;

		public static TLocalizedText TLoungeQuestName80200 => null;

		public static TLocalizedText TLoungeQuestDesc80200 => null;

		public static TLocalizedText TLoungeQuestName80201 => null;

		public static TLocalizedText TLoungeQuestDesc80201 => null;

		public static TLocalizedText TLoungeQuestName80202 => null;

		public static TLocalizedText TLoungeQuestDesc80202 => null;

		public static TLocalizedText TLoungeQuestName80203 => null;

		public static TLocalizedText TLoungeQuestDesc80203 => null;

		public static TLocalizedText TLoungeQuestName80204 => null;

		public static TLocalizedText TLoungeQuestDesc80204 => null;

		public static TLocalizedText TLoungeQuestName80205 => null;

		public static TLocalizedText TLoungeQuestDesc80205 => null;

		public static TLocalizedText TLoungeQuestName81101 => null;

		public static TLocalizedText TLoungeQuestDesc81101 => null;

		public static TLocalizedText TLoungeQuestName81102 => null;

		public static TLocalizedText TLoungeQuestDesc81102 => null;

		public static TLocalizedText TLoungeQuestName81103 => null;

		public static TLocalizedText TLoungeQuestDesc81103 => null;

		public static TLocalizedText TLoungeQuestName81104 => null;

		public static TLocalizedText TLoungeQuestDesc81104 => null;

		public static TLocalizedText TLoungeQuestName81105 => null;

		public static TLocalizedText TLoungeQuestDesc81105 => null;

		public static TLocalizedText TLoungeQuestName82100 => null;

		public static TLocalizedText TLoungeQuestDesc82100 => null;

		public static TLocalizedText TLoungeQuestName82101 => null;

		public static TLocalizedText TLoungeQuestDesc82101 => null;

		public static TLocalizedText TLoungeQuestName82102 => null;

		public static TLocalizedText TLoungeQuestDesc82102 => null;

		public static TLocalizedText TLoungeQuestName82103 => null;

		public static TLocalizedText TLoungeQuestDesc82103 => null;

		public static TLocalizedText TLoungeQuestName82104 => null;

		public static TLocalizedText TLoungeQuestDesc82104 => null;

		public static TLocalizedText TLoungeQuestName82105 => null;

		public static TLocalizedText TLoungeQuestDesc82105 => null;

		public static TLocalizedText TLoungeQuestName82106 => null;

		public static TLocalizedText TLoungeQuestDesc82106 => null;

		public static TLocalizedText TLoungeQuestName82107 => null;

		public static TLocalizedText TLoungeQuestDesc82107 => null;

		public static TLocalizedText TLoungeQuestName82108 => null;

		public static TLocalizedText TLoungeQuestDesc82108 => null;

		public static TLocalizedText TLoungeQuestName82109 => null;

		public static TLocalizedText TLoungeQuestDesc82109 => null;

		public static TLocalizedText TLoungeQuestName82111 => null;

		public static TLocalizedText TLoungeQuestDesc82111 => null;

		public static TLocalizedText TLoungeQuestName82112 => null;

		public static TLocalizedText TLoungeQuestDesc82112 => null;

		public static TLocalizedText TLoungeQuestName82113 => null;

		public static TLocalizedText TLoungeQuestDesc82113 => null;

		public static TLocalizedText TLoungeQuestName82190 => null;

		public static TLocalizedText TLoungeQuestDesc82190 => null;

		public static TLocalizedText TLoungeQuestName82201 => null;

		public static TLocalizedText TLoungeQuestDesc82201 => null;

		public static TLocalizedText TLoungeQuestName82202 => null;

		public static TLocalizedText TLoungeQuestDesc82202 => null;

		public static TLocalizedText TLoungeQuestName82203 => null;

		public static TLocalizedText TLoungeQuestDesc82203 => null;

		public static TLocalizedText TLoungeQuestName82204 => null;

		public static TLocalizedText TLoungeQuestDesc82204 => null;

		public static TLocalizedText TLoungeQuestName82205 => null;

		public static TLocalizedText TLoungeQuestDesc82205 => null;

		public static TLocalizedText TLoungeQuestName82206 => null;

		public static TLocalizedText TLoungeQuestDesc82206 => null;

		public static TLocalizedText TLoungeQuestName82207 => null;

		public static TLocalizedText TLoungeQuestDesc82207 => null;

		public static TLocalizedText TLoungeQuestName82208 => null;

		public static TLocalizedText TLoungeQuestDesc82208 => null;

		public static TLocalizedText TLoungeQuestName82209 => null;

		public static TLocalizedText TLoungeQuestDesc82209 => null;

		public static TLocalizedText TLoungeQuestName82210 => null;

		public static TLocalizedText TLoungeQuestDesc82210 => null;

		public static TLocalizedText TLoungeQuestName82290 => null;

		public static TLocalizedText TLoungeQuestDesc82290 => null;

		public static TLocalizedText TLoungeQuestName83101 => null;

		public static TLocalizedText TLoungeQuestDesc83101 => null;

		public static TLocalizedText TLoungeQuestName83102 => null;

		public static TLocalizedText TLoungeQuestDesc83102 => null;

		public static TLocalizedText TLoungeQuestName83103 => null;

		public static TLocalizedText TLoungeQuestDesc83103 => null;

		public static TLocalizedText TLoungeQuestName83104 => null;

		public static TLocalizedText TLoungeQuestDesc83104 => null;

		public static TLocalizedText TLoungeQuestName83105 => null;

		public static TLocalizedText TLoungeQuestDesc83105 => null;

		public static TLocalizedText TLoungeQuestName83106 => null;

		public static TLocalizedText TLoungeQuestDesc83106 => null;

		public static TLocalizedText TLoungeQuestName83107 => null;

		public static TLocalizedText TLoungeQuestDesc83107 => null;

		public static TLocalizedText TLoungeQuestName83108 => null;

		public static TLocalizedText TLoungeQuestDesc83108 => null;

		public static TLocalizedText TLoungeQuestName83109 => null;

		public static TLocalizedText TLoungeQuestDesc83109 => null;

		public static TLocalizedText TLoungeQuestName83110 => null;

		public static TLocalizedText TLoungeQuestDesc83110 => null;

		public static TLocalizedText TLoungeQuestName83111 => null;

		public static TLocalizedText TLoungeQuestDesc83111 => null;

		public static TLocalizedText TLoungeQuestName83112 => null;

		public static TLocalizedText TLoungeQuestDesc83112 => null;

		public static TLocalizedText TLoungeQuestName83113 => null;

		public static TLocalizedText TLoungeQuestDesc83113 => null;

		public static TLocalizedText TLoungeQuestName83114 => null;

		public static TLocalizedText TLoungeQuestDesc83114 => null;

		public static TLocalizedText TLoungeQuestName83115 => null;

		public static TLocalizedText TLoungeQuestDesc83115 => null;

		public static TLocalizedText TLoungeQuestName83201 => null;

		public static TLocalizedText TLoungeQuestDesc83201 => null;

		public static TLocalizedText TLoungeQuestName83202 => null;

		public static TLocalizedText TLoungeQuestDesc83202 => null;

		public static TLocalizedText TLoungeQuestName83203 => null;

		public static TLocalizedText TLoungeQuestDesc83203 => null;

		public static TLocalizedText TLoungeQuestName83204 => null;

		public static TLocalizedText TLoungeQuestDesc83204 => null;

		public static TLocalizedText TLoungeQuestName83205 => null;

		public static TLocalizedText TLoungeQuestDesc83205 => null;

		public static TLocalizedText TLoungeQuestName83206 => null;

		public static TLocalizedText TLoungeQuestDesc83206 => null;

		public static TLocalizedText TLoungeQuestName83207 => null;

		public static TLocalizedText TLoungeQuestDesc83207 => null;

		public static TLocalizedText TLoungeQuestName83208 => null;

		public static TLocalizedText TLoungeQuestDesc83208 => null;

		public static TLocalizedText TLoungeQuestName83209 => null;

		public static TLocalizedText TLoungeQuestDesc83209 => null;

		public static TLocalizedText TLoungeQuestName83210 => null;

		public static TLocalizedText TLoungeQuestDesc83210 => null;

		public static TLocalizedText TLoungeQuestName83211 => null;

		public static TLocalizedText TLoungeQuestDesc83211 => null;

		public static TLocalizedText TLoungeQuestName83212 => null;

		public static TLocalizedText TLoungeQuestDesc83212 => null;

		public static TLocalizedText TLoungeQuestName84101 => null;

		public static TLocalizedText TLoungeQuestDesc84101 => null;

		public static TLocalizedText TLoungeQuestName84102 => null;

		public static TLocalizedText TLoungeQuestDesc84102 => null;

		public static TLocalizedText TLoungeQuestName84301 => null;

		public static TLocalizedText TLoungeQuestDesc84301 => null;

		public static TLocalizedText TLoungeQuestName84302 => null;

		public static TLocalizedText TLoungeQuestDesc84302 => null;

		public static TLocalizedText TLoungeQuestName84401 => null;

		public static TLocalizedText TLoungeQuestDesc84401 => null;

		public static TLocalizedText TLoungeQuestName84402 => null;

		public static TLocalizedText TLoungeQuestDesc84402 => null;

		public static TLocalizedText TLoungeQuestName84501 => null;

		public static TLocalizedText TLoungeQuestDesc84501 => null;

		public static TLocalizedText TLoungeQuestName84502 => null;

		public static TLocalizedText TLoungeQuestDesc84502 => null;

		public static TLocalizedText TLoungeQuestName810100 => null;

		public static TLocalizedText TLoungeQuestName810101 => null;

		public static TLocalizedText TLoungeQuestName810102 => null;

		public static TLocalizedText TLoungeQuestName810800 => null;

		public static TLocalizedText TLoungeQuestName810801 => null;

		public static TLocalizedText TLoungeQuestName811500 => null;

		public static TLocalizedText TLoungeQuestName811501 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800061 => null;

		public static TLocalizedText TLoungeQuestTaskDesc800071 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801001 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc801051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc802001 => null;

		public static TLocalizedText TLoungeQuestTaskDesc802002 => null;

		public static TLocalizedText TLoungeQuestTaskDesc802003 => null;

		public static TLocalizedText TLoungeQuestTaskDesc811011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc811021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc811031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc811041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc811051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821001 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821061 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821071 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821081 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821091 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821111 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821121 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821131 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821901 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821902 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821903 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821904 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821905 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821906 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821907 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821908 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821909 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821910 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821911 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821912 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821913 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821914 => null;

		public static TLocalizedText TLoungeQuestTaskDesc821915 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822061 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822071 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822081 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822091 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822101 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822901 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822902 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822903 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822904 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822905 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822906 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822907 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822908 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822909 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822910 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822911 => null;

		public static TLocalizedText TLoungeQuestTaskDesc822912 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831061 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831071 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831081 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831091 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831101 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831111 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831121 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831131 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831141 => null;

		public static TLocalizedText TLoungeQuestTaskDesc831151 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832031 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832041 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832051 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832061 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832071 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832081 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832091 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832101 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832111 => null;

		public static TLocalizedText TLoungeQuestTaskDesc832121 => null;

		public static TLocalizedText TLoungeQuestTaskDesc841011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc841021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc843011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc843021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc844011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc844021 => null;

		public static TLocalizedText TLoungeQuestTaskDesc845011 => null;

		public static TLocalizedText TLoungeQuestTaskDesc845021 => null;

		public static TLocalizedText TMimicName1001 => null;

		public static TLocalizedText TMimicName2001 => null;

		public static TLocalizedText TMissionName9001 => null;

		public static TLocalizedText TMissionName9002 => null;

		public static TLocalizedText TMissionName9003 => null;

		public static TLocalizedText TMissionName9004 => null;

		public static TLocalizedText TMissionName9005 => null;

		public static TLocalizedText TMissionName9006 => null;

		public static TLocalizedText TMissionName9007 => null;

		public static TLocalizedText TMissionName9008 => null;

		public static TLocalizedText TMissionName9009 => null;

		public static TLocalizedText TMissionName9010 => null;

		public static TLocalizedText TMissionName9011 => null;

		public static TLocalizedText TMissionName9012 => null;

		public static TLocalizedText TMissionName9013 => null;

		public static TLocalizedText TMissionName9101 => null;

		public static TLocalizedText TMissionDesc9101 => null;

		public static TLocalizedText TMissionName9102 => null;

		public static TLocalizedText TMissionDesc9102 => null;

		public static TLocalizedText TMissionName9103 => null;

		public static TLocalizedText TMissionDesc9103 => null;

		public static TLocalizedText TMissionName9104 => null;

		public static TLocalizedText TMissionDesc9104 => null;

		public static TLocalizedText TMissionName9105 => null;

		public static TLocalizedText TMissionDesc9105 => null;

		public static TLocalizedText TMissionName9111 => null;

		public static TLocalizedText TMissionDesc9111 => null;

		public static TLocalizedText TMissionName9112 => null;

		public static TLocalizedText TMissionDesc9112 => null;

		public static TLocalizedText TMissionName9113 => null;

		public static TLocalizedText TMissionDesc9113 => null;

		public static TLocalizedText TMissionName9114 => null;

		public static TLocalizedText TMissionDesc9114 => null;

		public static TLocalizedText TMissionName9115 => null;

		public static TLocalizedText TMissionDesc9115 => null;

		public static TLocalizedText TMissionName9116 => null;

		public static TLocalizedText TMissionDesc9116 => null;

		public static TLocalizedText TMissionName9117 => null;

		public static TLocalizedText TMissionDesc9117 => null;

		public static TLocalizedText TMissionName9118 => null;

		public static TLocalizedText TMissionDesc9118 => null;

		public static TLocalizedText TMissionName9121 => null;

		public static TLocalizedText TMissionDesc9121 => null;

		public static TLocalizedText TMissionName9122 => null;

		public static TLocalizedText TMissionDesc9122 => null;

		public static TLocalizedText TMissionName9123 => null;

		public static TLocalizedText TMissionDesc9123 => null;

		public static TLocalizedText TMissionName9124 => null;

		public static TLocalizedText TMissionDesc9124 => null;

		public static TLocalizedText TMissionName9125 => null;

		public static TLocalizedText TMissionDesc9125 => null;

		public static TLocalizedText TMonsterName200001 => null;

		public static TLocalizedText TMonsterDesc200001 => null;

		public static TLocalizedText TMonsterBossRadioText200001 => null;

		public static TLocalizedText TMonsterName200002 => null;

		public static TLocalizedText TMonsterDesc200002 => null;

		public static TLocalizedText TMonsterBossRadioText200002 => null;

		public static TLocalizedText TMonsterName200003 => null;

		public static TLocalizedText TMonsterDesc200003 => null;

		public static TLocalizedText TMonsterBossRadioText200003 => null;

		public static TLocalizedText TMonsterName200004 => null;

		public static TLocalizedText TMonsterDesc200004 => null;

		public static TLocalizedText TMonsterBossRadioText200004 => null;

		public static TLocalizedText TMonsterName200005 => null;

		public static TLocalizedText TMonsterDesc200005 => null;

		public static TLocalizedText TMonsterBossRadioText200005 => null;

		public static TLocalizedText TMonsterName200006 => null;

		public static TLocalizedText TMonsterDesc200006 => null;

		public static TLocalizedText TMonsterBossRadioText200006 => null;

		public static TLocalizedText TMonsterName200007 => null;

		public static TLocalizedText TMonsterDesc200007 => null;

		public static TLocalizedText TMonsterBossRadioText200007 => null;

		public static TLocalizedText TMonsterName200008 => null;

		public static TLocalizedText TMonsterDesc200008 => null;

		public static TLocalizedText TMonsterBossRadioText200008 => null;

		public static TLocalizedText TMonsterName200009 => null;

		public static TLocalizedText TMonsterDesc200009 => null;

		public static TLocalizedText TMonsterBossRadioText200009 => null;

		public static TLocalizedText TMonsterName200011 => null;

		public static TLocalizedText TMonsterDesc200011 => null;

		public static TLocalizedText TMonsterBossRadioText200011 => null;

		public static TLocalizedText TMonsterName200012 => null;

		public static TLocalizedText TMonsterDesc200012 => null;

		public static TLocalizedText TMonsterBossRadioText200012 => null;

		public static TLocalizedText TMonsterName200013 => null;

		public static TLocalizedText TMonsterDesc200013 => null;

		public static TLocalizedText TMonsterBossRadioText200013 => null;

		public static TLocalizedText TMonsterName200014 => null;

		public static TLocalizedText TMonsterDesc200014 => null;

		public static TLocalizedText TMonsterBossRadioText200014 => null;

		public static TLocalizedText TMonsterName200015 => null;

		public static TLocalizedText TMonsterDesc200015 => null;

		public static TLocalizedText TMonsterBossRadioText200015 => null;

		public static TLocalizedText TMonsterName200016 => null;

		public static TLocalizedText TMonsterDesc200016 => null;

		public static TLocalizedText TMonsterBossRadioText200016 => null;

		public static TLocalizedText TMonsterName200017 => null;

		public static TLocalizedText TMonsterDesc200017 => null;

		public static TLocalizedText TMonsterBossRadioText200017 => null;

		public static TLocalizedText TMonsterName200018 => null;

		public static TLocalizedText TMonsterDesc200018 => null;

		public static TLocalizedText TMonsterBossRadioText200018 => null;

		public static TLocalizedText TMonsterName200019 => null;

		public static TLocalizedText TMonsterDesc200019 => null;

		public static TLocalizedText TMonsterBossRadioText200019 => null;

		public static TLocalizedText TMonsterName200020 => null;

		public static TLocalizedText TMonsterDesc200020 => null;

		public static TLocalizedText TMonsterBossRadioText200020 => null;

		public static TLocalizedText TMonsterName200021 => null;

		public static TLocalizedText TMonsterDesc200021 => null;

		public static TLocalizedText TMonsterBossRadioText200021 => null;

		public static TLocalizedText TMonsterName200022 => null;

		public static TLocalizedText TMonsterDesc200022 => null;

		public static TLocalizedText TMonsterBossRadioText200022 => null;

		public static TLocalizedText TMonsterName200023 => null;

		public static TLocalizedText TMonsterDesc200023 => null;

		public static TLocalizedText TMonsterBossRadioText200023 => null;

		public static TLocalizedText TMonsterName200024 => null;

		public static TLocalizedText TMonsterDesc200024 => null;

		public static TLocalizedText TMonsterBossRadioText200024 => null;

		public static TLocalizedText TMonsterName200025 => null;

		public static TLocalizedText TMonsterDesc200025 => null;

		public static TLocalizedText TMonsterBossRadioText200025 => null;

		public static TLocalizedText TMonsterName200026 => null;

		public static TLocalizedText TMonsterDesc200026 => null;

		public static TLocalizedText TMonsterBossRadioText200026 => null;

		public static TLocalizedText TMonsterName200027 => null;

		public static TLocalizedText TMonsterDesc200027 => null;

		public static TLocalizedText TMonsterBossRadioText200027 => null;

		public static TLocalizedText TMonsterName200029 => null;

		public static TLocalizedText TMonsterDesc200029 => null;

		public static TLocalizedText TMonsterBossRadioText200029 => null;

		public static TLocalizedText TMonsterName200030 => null;

		public static TLocalizedText TMonsterDesc200030 => null;

		public static TLocalizedText TMonsterBossRadioText200030 => null;

		public static TLocalizedText TMonsterName200031 => null;

		public static TLocalizedText TMonsterDesc200031 => null;

		public static TLocalizedText TMonsterBossRadioText200031 => null;

		public static TLocalizedText TMonsterName200032 => null;

		public static TLocalizedText TMonsterDesc200032 => null;

		public static TLocalizedText TMonsterBossRadioText200032 => null;

		public static TLocalizedText TMonsterName200033 => null;

		public static TLocalizedText TMonsterDesc200033 => null;

		public static TLocalizedText TMonsterBossRadioText200033 => null;

		public static TLocalizedText TMonsterName200040 => null;

		public static TLocalizedText TMonsterDesc200040 => null;

		public static TLocalizedText TMonsterBossRadioText200040 => null;

		public static TLocalizedText TMonsterName200041 => null;

		public static TLocalizedText TMonsterDesc200041 => null;

		public static TLocalizedText TMonsterBossRadioText200041 => null;

		public static TLocalizedText TMonsterName200042 => null;

		public static TLocalizedText TMonsterDesc200042 => null;

		public static TLocalizedText TMonsterBossRadioText200042 => null;

		public static TLocalizedText TMonsterName200043 => null;

		public static TLocalizedText TMonsterDesc200043 => null;

		public static TLocalizedText TMonsterBossRadioText200043 => null;

		public static TLocalizedText TMonsterName200044 => null;

		public static TLocalizedText TMonsterDesc200044 => null;

		public static TLocalizedText TMonsterBossRadioText200044 => null;

		public static TLocalizedText TMonsterName200045 => null;

		public static TLocalizedText TMonsterDesc200045 => null;

		public static TLocalizedText TMonsterBossRadioText200045 => null;

		public static TLocalizedText TMonsterName200046 => null;

		public static TLocalizedText TMonsterDesc200046 => null;

		public static TLocalizedText TMonsterBossRadioText200046 => null;

		public static TLocalizedText TMonsterName200047 => null;

		public static TLocalizedText TMonsterDesc200047 => null;

		public static TLocalizedText TMonsterBossRadioText200047 => null;

		public static TLocalizedText TMonsterName200048 => null;

		public static TLocalizedText TMonsterDesc200048 => null;

		public static TLocalizedText TMonsterBossRadioText200048 => null;

		public static TLocalizedText TMonsterName200049 => null;

		public static TLocalizedText TMonsterDesc200049 => null;

		public static TLocalizedText TMonsterBossRadioText200049 => null;

		public static TLocalizedText TMonsterName200050 => null;

		public static TLocalizedText TMonsterDesc200050 => null;

		public static TLocalizedText TMonsterBossRadioText200050 => null;

		public static TLocalizedText TMonsterName210001 => null;

		public static TLocalizedText TMonsterDesc210001 => null;

		public static TLocalizedText TMonsterBossRadioText210001 => null;

		public static TLocalizedText TMonsterName210002 => null;

		public static TLocalizedText TMonsterDesc210002 => null;

		public static TLocalizedText TMonsterBossRadioText210002 => null;

		public static TLocalizedText TMonsterName210003 => null;

		public static TLocalizedText TMonsterDesc210003 => null;

		public static TLocalizedText TMonsterBossRadioText210003 => null;

		public static TLocalizedText TMonsterName210004 => null;

		public static TLocalizedText TMonsterDesc210004 => null;

		public static TLocalizedText TMonsterBossRadioText210004 => null;

		public static TLocalizedText TMonsterName210005 => null;

		public static TLocalizedText TMonsterDesc210005 => null;

		public static TLocalizedText TMonsterBossRadioText210005 => null;

		public static TLocalizedText TMonsterName210006 => null;

		public static TLocalizedText TMonsterDesc210006 => null;

		public static TLocalizedText TMonsterBossRadioText210006 => null;

		public static TLocalizedText TMonsterName210007 => null;

		public static TLocalizedText TMonsterDesc210007 => null;

		public static TLocalizedText TMonsterBossRadioText210007 => null;

		public static TLocalizedText TMonsterName210008 => null;

		public static TLocalizedText TMonsterDesc210008 => null;

		public static TLocalizedText TMonsterBossRadioText210008 => null;

		public static TLocalizedText TMonsterName210011 => null;

		public static TLocalizedText TMonsterDesc210011 => null;

		public static TLocalizedText TMonsterBossRadioText210011 => null;

		public static TLocalizedText TMonsterName210012 => null;

		public static TLocalizedText TMonsterDesc210012 => null;

		public static TLocalizedText TMonsterBossRadioText210012 => null;

		public static TLocalizedText TMonsterName210013 => null;

		public static TLocalizedText TMonsterDesc210013 => null;

		public static TLocalizedText TMonsterBossRadioText210013 => null;

		public static TLocalizedText TMonsterName210014 => null;

		public static TLocalizedText TMonsterDesc210014 => null;

		public static TLocalizedText TMonsterBossRadioText210014 => null;

		public static TLocalizedText TMonsterName210015 => null;

		public static TLocalizedText TMonsterDesc210015 => null;

		public static TLocalizedText TMonsterBossRadioText210015 => null;

		public static TLocalizedText TMonsterName210016 => null;

		public static TLocalizedText TMonsterDesc210016 => null;

		public static TLocalizedText TMonsterBossRadioText210016 => null;

		public static TLocalizedText TMonsterName210017 => null;

		public static TLocalizedText TMonsterDesc210017 => null;

		public static TLocalizedText TMonsterBossRadioText210017 => null;

		public static TLocalizedText TMonsterName210018 => null;

		public static TLocalizedText TMonsterDesc210018 => null;

		public static TLocalizedText TMonsterBossRadioText210018 => null;

		public static TLocalizedText TMonsterName210019 => null;

		public static TLocalizedText TMonsterDesc210019 => null;

		public static TLocalizedText TMonsterBossRadioText210019 => null;

		public static TLocalizedText TMonsterName210020 => null;

		public static TLocalizedText TMonsterDesc210020 => null;

		public static TLocalizedText TMonsterBossRadioText210020 => null;

		public static TLocalizedText TMonsterName210021 => null;

		public static TLocalizedText TMonsterDesc210021 => null;

		public static TLocalizedText TMonsterBossRadioText210021 => null;

		public static TLocalizedText TMonsterName210023 => null;

		public static TLocalizedText TMonsterDesc210023 => null;

		public static TLocalizedText TMonsterBossRadioText210023 => null;

		public static TLocalizedText TMonsterName210024 => null;

		public static TLocalizedText TMonsterDesc210024 => null;

		public static TLocalizedText TMonsterBossRadioText210024 => null;

		public static TLocalizedText TMonsterName210025 => null;

		public static TLocalizedText TMonsterDesc210025 => null;

		public static TLocalizedText TMonsterBossRadioText210025 => null;

		public static TLocalizedText TMonsterName210026 => null;

		public static TLocalizedText TMonsterDesc210026 => null;

		public static TLocalizedText TMonsterBossRadioText210026 => null;

		public static TLocalizedText TMonsterName210030 => null;

		public static TLocalizedText TMonsterDesc210030 => null;

		public static TLocalizedText TMonsterBossRadioText210030 => null;

		public static TLocalizedText TMonsterName220001 => null;

		public static TLocalizedText TMonsterDesc220001 => null;

		public static TLocalizedText TMonsterBossRadioText220001 => null;

		public static TLocalizedText TMonsterName220002 => null;

		public static TLocalizedText TMonsterDesc220002 => null;

		public static TLocalizedText TMonsterBossRadioText220002 => null;

		public static TLocalizedText TMonsterName220003 => null;

		public static TLocalizedText TMonsterDesc220003 => null;

		public static TLocalizedText TMonsterBossRadioText220003 => null;

		public static TLocalizedText TMonsterName220005 => null;

		public static TLocalizedText TMonsterDesc220005 => null;

		public static TLocalizedText TMonsterBossRadioText220005 => null;

		public static TLocalizedText TMonsterName220006 => null;

		public static TLocalizedText TMonsterDesc220006 => null;

		public static TLocalizedText TMonsterBossRadioText220006 => null;

		public static TLocalizedText TMonsterName220007 => null;

		public static TLocalizedText TMonsterDesc220007 => null;

		public static TLocalizedText TMonsterBossRadioText220007 => null;

		public static TLocalizedText TMonsterName220008 => null;

		public static TLocalizedText TMonsterDesc220008 => null;

		public static TLocalizedText TMonsterBossRadioText220008 => null;

		public static TLocalizedText TMonsterName220009 => null;

		public static TLocalizedText TMonsterDesc220009 => null;

		public static TLocalizedText TMonsterBossRadioText220009 => null;

		public static TLocalizedText TMonsterName230001 => null;

		public static TLocalizedText TMonsterDesc230001 => null;

		public static TLocalizedText TMonsterBossRadioText230001 => null;

		public static TLocalizedText TMonsterName240001 => null;

		public static TLocalizedText TMonsterDesc240001 => null;

		public static TLocalizedText TMonsterBossRadioText240001 => null;

		public static TLocalizedText TMonsterName240002 => null;

		public static TLocalizedText TMonsterDesc240002 => null;

		public static TLocalizedText TMonsterBossRadioText240002 => null;

		public static TLocalizedText TMonsterName240003 => null;

		public static TLocalizedText TMonsterDesc240003 => null;

		public static TLocalizedText TMonsterBossRadioText240003 => null;

		public static TLocalizedText TMonsterName240004 => null;

		public static TLocalizedText TMonsterDesc240004 => null;

		public static TLocalizedText TMonsterBossRadioText240004 => null;

		public static TLocalizedText TMonsterName240005 => null;

		public static TLocalizedText TMonsterDesc240005 => null;

		public static TLocalizedText TMonsterBossRadioText240005 => null;

		public static TLocalizedText TMonsterName240006 => null;

		public static TLocalizedText TMonsterDesc240006 => null;

		public static TLocalizedText TMonsterBossRadioText240006 => null;

		public static TLocalizedText TMonsterName240007 => null;

		public static TLocalizedText TMonsterDesc240007 => null;

		public static TLocalizedText TMonsterBossRadioText240007 => null;

		public static TLocalizedText TMonsterName240008 => null;

		public static TLocalizedText TMonsterDesc240008 => null;

		public static TLocalizedText TMonsterBossRadioText240008 => null;

		public static TLocalizedText TMonsterName240011 => null;

		public static TLocalizedText TMonsterDesc240011 => null;

		public static TLocalizedText TMonsterBossRadioText240011 => null;

		public static TLocalizedText TMonsterName240012 => null;

		public static TLocalizedText TMonsterDesc240012 => null;

		public static TLocalizedText TMonsterBossRadioText240012 => null;

		public static TLocalizedText TMonsterName240013 => null;

		public static TLocalizedText TMonsterDesc240013 => null;

		public static TLocalizedText TMonsterBossRadioText240013 => null;

		public static TLocalizedText TMonsterName240014 => null;

		public static TLocalizedText TMonsterDesc240014 => null;

		public static TLocalizedText TMonsterBossRadioText240014 => null;

		public static TLocalizedText TMonsterName240015 => null;

		public static TLocalizedText TMonsterDesc240015 => null;

		public static TLocalizedText TMonsterBossRadioText240015 => null;

		public static TLocalizedText TMonsterName240016 => null;

		public static TLocalizedText TMonsterDesc240016 => null;

		public static TLocalizedText TMonsterBossRadioText240016 => null;

		public static TLocalizedText TMonsterName240018 => null;

		public static TLocalizedText TMonsterDesc240018 => null;

		public static TLocalizedText TMonsterBossRadioText240018 => null;

		public static TLocalizedText TMonsterName240019 => null;

		public static TLocalizedText TMonsterDesc240019 => null;

		public static TLocalizedText TMonsterBossRadioText240019 => null;

		public static TLocalizedText TMonsterName240021 => null;

		public static TLocalizedText TMonsterDesc240021 => null;

		public static TLocalizedText TMonsterBossRadioText240021 => null;

		public static TLocalizedText TMonsterName240023 => null;

		public static TLocalizedText TMonsterDesc240023 => null;

		public static TLocalizedText TMonsterBossRadioText240023 => null;

		public static TLocalizedText TMonsterName240024 => null;

		public static TLocalizedText TMonsterDesc240024 => null;

		public static TLocalizedText TMonsterBossRadioText240024 => null;

		public static TLocalizedText TMonsterName240025 => null;

		public static TLocalizedText TMonsterDesc240025 => null;

		public static TLocalizedText TMonsterBossRadioText240025 => null;

		public static TLocalizedText TMonsterName240026 => null;

		public static TLocalizedText TMonsterDesc240026 => null;

		public static TLocalizedText TMonsterBossRadioText240026 => null;

		public static TLocalizedText TMonsterName240030 => null;

		public static TLocalizedText TMonsterDesc240030 => null;

		public static TLocalizedText TMonsterBossRadioText240030 => null;

		public static TLocalizedText TMonsterName250001 => null;

		public static TLocalizedText TMonsterDesc250001 => null;

		public static TLocalizedText TMonsterBossRadioText250001 => null;

		public static TLocalizedText TMonsterName250002 => null;

		public static TLocalizedText TMonsterDesc250002 => null;

		public static TLocalizedText TMonsterBossRadioText250002 => null;

		public static TLocalizedText TMonsterName250003 => null;

		public static TLocalizedText TMonsterDesc250003 => null;

		public static TLocalizedText TMonsterBossRadioText250003 => null;

		public static TLocalizedText TMonsterName250004 => null;

		public static TLocalizedText TMonsterDesc250004 => null;

		public static TLocalizedText TMonsterBossRadioText250004 => null;

		public static TLocalizedText TMonsterName250005 => null;

		public static TLocalizedText TMonsterDesc250005 => null;

		public static TLocalizedText TMonsterBossRadioText250005 => null;

		public static TLocalizedText TMonsterName250011 => null;

		public static TLocalizedText TMonsterDesc250011 => null;

		public static TLocalizedText TMonsterBossRadioText250011 => null;

		public static TLocalizedText TMonsterName250012 => null;

		public static TLocalizedText TMonsterDesc250012 => null;

		public static TLocalizedText TMonsterBossRadioText250012 => null;

		public static TLocalizedText TMonsterName250013 => null;

		public static TLocalizedText TMonsterDesc250013 => null;

		public static TLocalizedText TMonsterBossRadioText250013 => null;

		public static TLocalizedText TMonsterName250014 => null;

		public static TLocalizedText TMonsterDesc250014 => null;

		public static TLocalizedText TMonsterBossRadioText250014 => null;

		public static TLocalizedText TMonsterName250015 => null;

		public static TLocalizedText TMonsterDesc250015 => null;

		public static TLocalizedText TMonsterBossRadioText250015 => null;

		public static TLocalizedText TMonsterName250021 => null;

		public static TLocalizedText TMonsterDesc250021 => null;

		public static TLocalizedText TMonsterBossRadioText250021 => null;

		public static TLocalizedText TMonsterName250022 => null;

		public static TLocalizedText TMonsterDesc250022 => null;

		public static TLocalizedText TMonsterBossRadioText250022 => null;

		public static TLocalizedText TMonsterName250023 => null;

		public static TLocalizedText TMonsterDesc250023 => null;

		public static TLocalizedText TMonsterBossRadioText250023 => null;

		public static TLocalizedText TMonsterName250024 => null;

		public static TLocalizedText TMonsterDesc250024 => null;

		public static TLocalizedText TMonsterBossRadioText250024 => null;

		public static TLocalizedText TMonsterName250025 => null;

		public static TLocalizedText TMonsterDesc250025 => null;

		public static TLocalizedText TMonsterBossRadioText250025 => null;

		public static TLocalizedText TMonsterName250031 => null;

		public static TLocalizedText TMonsterDesc250031 => null;

		public static TLocalizedText TMonsterBossRadioText250031 => null;

		public static TLocalizedText TMonsterName250032 => null;

		public static TLocalizedText TMonsterDesc250032 => null;

		public static TLocalizedText TMonsterBossRadioText250032 => null;

		public static TLocalizedText TMonsterName250033 => null;

		public static TLocalizedText TMonsterDesc250033 => null;

		public static TLocalizedText TMonsterBossRadioText250033 => null;

		public static TLocalizedText TMonsterName250034 => null;

		public static TLocalizedText TMonsterDesc250034 => null;

		public static TLocalizedText TMonsterBossRadioText250034 => null;

		public static TLocalizedText TMonsterName250035 => null;

		public static TLocalizedText TMonsterDesc250035 => null;

		public static TLocalizedText TMonsterBossRadioText250035 => null;

		public static TLocalizedText TMonsterName300001 => null;

		public static TLocalizedText TMonsterDesc300001 => null;

		public static TLocalizedText TMonsterBossRadioText300001 => null;

		public static TLocalizedText TMonsterName300002 => null;

		public static TLocalizedText TMonsterDesc300002 => null;

		public static TLocalizedText TMonsterBossRadioText300002 => null;

		public static TLocalizedText TMonsterName300003 => null;

		public static TLocalizedText TMonsterDesc300003 => null;

		public static TLocalizedText TMonsterBossRadioText300003 => null;

		public static TLocalizedText TMonsterName300004 => null;

		public static TLocalizedText TMonsterDesc300004 => null;

		public static TLocalizedText TMonsterBossRadioText300004 => null;

		public static TLocalizedText TMonsterName300005 => null;

		public static TLocalizedText TMonsterDesc300005 => null;

		public static TLocalizedText TMonsterBossRadioText300005 => null;

		public static TLocalizedText TMonsterName300006 => null;

		public static TLocalizedText TMonsterDesc300006 => null;

		public static TLocalizedText TMonsterBossRadioText300006 => null;

		public static TLocalizedText TMonsterName300007 => null;

		public static TLocalizedText TMonsterDesc300007 => null;

		public static TLocalizedText TMonsterBossRadioText300007 => null;

		public static TLocalizedText TMonsterName300008 => null;

		public static TLocalizedText TMonsterDesc300008 => null;

		public static TLocalizedText TMonsterBossRadioText300008 => null;

		public static TLocalizedText TMonsterName300009 => null;

		public static TLocalizedText TMonsterDesc300009 => null;

		public static TLocalizedText TMonsterBossRadioText300009 => null;

		public static TLocalizedText TMonsterName300010 => null;

		public static TLocalizedText TMonsterDesc300010 => null;

		public static TLocalizedText TMonsterBossRadioText300010 => null;

		public static TLocalizedText TMonsterName300011 => null;

		public static TLocalizedText TMonsterDesc300011 => null;

		public static TLocalizedText TMonsterBossRadioText300011 => null;

		public static TLocalizedText TMonsterName300012 => null;

		public static TLocalizedText TMonsterDesc300012 => null;

		public static TLocalizedText TMonsterBossRadioText300012 => null;

		public static TLocalizedText TMonsterName300013 => null;

		public static TLocalizedText TMonsterDesc300013 => null;

		public static TLocalizedText TMonsterBossRadioText300013 => null;

		public static TLocalizedText TMonsterName300014 => null;

		public static TLocalizedText TMonsterDesc300014 => null;

		public static TLocalizedText TMonsterBossRadioText300014 => null;

		public static TLocalizedText TMonsterName300015 => null;

		public static TLocalizedText TMonsterDesc300015 => null;

		public static TLocalizedText TMonsterBossRadioText300015 => null;

		public static TLocalizedText TMonsterName300016 => null;

		public static TLocalizedText TMonsterDesc300016 => null;

		public static TLocalizedText TMonsterBossRadioText300016 => null;

		public static TLocalizedText TMonsterName300017 => null;

		public static TLocalizedText TMonsterDesc300017 => null;

		public static TLocalizedText TMonsterBossRadioText300017 => null;

		public static TLocalizedText TMonsterName300018 => null;

		public static TLocalizedText TMonsterDesc300018 => null;

		public static TLocalizedText TMonsterBossRadioText300018 => null;

		public static TLocalizedText TMonsterName300019 => null;

		public static TLocalizedText TMonsterDesc300019 => null;

		public static TLocalizedText TMonsterBossRadioText300019 => null;

		public static TLocalizedText TMonsterName300020 => null;

		public static TLocalizedText TMonsterDesc300020 => null;

		public static TLocalizedText TMonsterBossRadioText300020 => null;

		public static TLocalizedText TMonsterName300021 => null;

		public static TLocalizedText TMonsterDesc300021 => null;

		public static TLocalizedText TMonsterBossRadioText300021 => null;

		public static TLocalizedText TMonsterName300022 => null;

		public static TLocalizedText TMonsterDesc300022 => null;

		public static TLocalizedText TMonsterBossRadioText300022 => null;

		public static TLocalizedText TMonsterName300023 => null;

		public static TLocalizedText TMonsterDesc300023 => null;

		public static TLocalizedText TMonsterBossRadioText300023 => null;

		public static TLocalizedText TMonsterName300024 => null;

		public static TLocalizedText TMonsterDesc300024 => null;

		public static TLocalizedText TMonsterBossRadioText300024 => null;

		public static TLocalizedText TMonsterName300025 => null;

		public static TLocalizedText TMonsterDesc300025 => null;

		public static TLocalizedText TMonsterBossRadioText300025 => null;

		public static TLocalizedText TMonsterName300026 => null;

		public static TLocalizedText TMonsterDesc300026 => null;

		public static TLocalizedText TMonsterBossRadioText300026 => null;

		public static TLocalizedText TMonsterName300027 => null;

		public static TLocalizedText TMonsterDesc300027 => null;

		public static TLocalizedText TMonsterBossRadioText300027 => null;

		public static TLocalizedText TMonsterName300028 => null;

		public static TLocalizedText TMonsterDesc300028 => null;

		public static TLocalizedText TMonsterBossRadioText300028 => null;

		public static TLocalizedText TMonsterName301001 => null;

		public static TLocalizedText TMonsterDesc301001 => null;

		public static TLocalizedText TMonsterBossRadioText301001 => null;

		public static TLocalizedText TMonsterName301004 => null;

		public static TLocalizedText TMonsterDesc301004 => null;

		public static TLocalizedText TMonsterBossRadioText301004 => null;

		public static TLocalizedText TMonsterName301005 => null;

		public static TLocalizedText TMonsterDesc301005 => null;

		public static TLocalizedText TMonsterBossRadioText301005 => null;

		public static TLocalizedText TMonsterName301006 => null;

		public static TLocalizedText TMonsterDesc301006 => null;

		public static TLocalizedText TMonsterBossRadioText301006 => null;

		public static TLocalizedText TMonsterName301010 => null;

		public static TLocalizedText TMonsterDesc301010 => null;

		public static TLocalizedText TMonsterBossRadioText301010 => null;

		public static TLocalizedText TMonsterName301012 => null;

		public static TLocalizedText TMonsterDesc301012 => null;

		public static TLocalizedText TMonsterBossRadioText301012 => null;

		public static TLocalizedText TMonsterName301013 => null;

		public static TLocalizedText TMonsterDesc301013 => null;

		public static TLocalizedText TMonsterBossRadioText301013 => null;

		public static TLocalizedText TMonsterName301014 => null;

		public static TLocalizedText TMonsterDesc301014 => null;

		public static TLocalizedText TMonsterBossRadioText301014 => null;

		public static TLocalizedText TMonsterName301015 => null;

		public static TLocalizedText TMonsterDesc301015 => null;

		public static TLocalizedText TMonsterBossRadioText301015 => null;

		public static TLocalizedText TMonsterName301016 => null;

		public static TLocalizedText TMonsterDesc301016 => null;

		public static TLocalizedText TMonsterBossRadioText301016 => null;

		public static TLocalizedText TMonsterName302001 => null;

		public static TLocalizedText TMonsterDesc302001 => null;

		public static TLocalizedText TMonsterBossRadioText302001 => null;

		public static TLocalizedText TMonsterName302004 => null;

		public static TLocalizedText TMonsterDesc302004 => null;

		public static TLocalizedText TMonsterBossRadioText302004 => null;

		public static TLocalizedText TMonsterName302005 => null;

		public static TLocalizedText TMonsterDesc302005 => null;

		public static TLocalizedText TMonsterBossRadioText302005 => null;

		public static TLocalizedText TMonsterName302010 => null;

		public static TLocalizedText TMonsterDesc302010 => null;

		public static TLocalizedText TMonsterBossRadioText302010 => null;

		public static TLocalizedText TMonsterName302012 => null;

		public static TLocalizedText TMonsterDesc302012 => null;

		public static TLocalizedText TMonsterBossRadioText302012 => null;

		public static TLocalizedText TMonsterName302013 => null;

		public static TLocalizedText TMonsterDesc302013 => null;

		public static TLocalizedText TMonsterBossRadioText302013 => null;

		public static TLocalizedText TMonsterName302014 => null;

		public static TLocalizedText TMonsterDesc302014 => null;

		public static TLocalizedText TMonsterBossRadioText302014 => null;

		public static TLocalizedText TMonsterName302015 => null;

		public static TLocalizedText TMonsterDesc302015 => null;

		public static TLocalizedText TMonsterBossRadioText302015 => null;

		public static TLocalizedText TMonsterName302016 => null;

		public static TLocalizedText TMonsterDesc302016 => null;

		public static TLocalizedText TMonsterBossRadioText302016 => null;

		public static TLocalizedText TMonsterName310001 => null;

		public static TLocalizedText TMonsterDesc310001 => null;

		public static TLocalizedText TMonsterBossRadioText310001 => null;

		public static TLocalizedText TMonsterName310002 => null;

		public static TLocalizedText TMonsterDesc310002 => null;

		public static TLocalizedText TMonsterBossRadioText310002 => null;

		public static TLocalizedText TMonsterName310003 => null;

		public static TLocalizedText TMonsterDesc310003 => null;

		public static TLocalizedText TMonsterBossRadioText310003 => null;

		public static TLocalizedText TMonsterName310004 => null;

		public static TLocalizedText TMonsterDesc310004 => null;

		public static TLocalizedText TMonsterBossRadioText310004 => null;

		public static TLocalizedText TMonsterName310005 => null;

		public static TLocalizedText TMonsterDesc310005 => null;

		public static TLocalizedText TMonsterBossRadioText310005 => null;

		public static TLocalizedText TMonsterName310006 => null;

		public static TLocalizedText TMonsterDesc310006 => null;

		public static TLocalizedText TMonsterBossRadioText310006 => null;

		public static TLocalizedText TMonsterName310007 => null;

		public static TLocalizedText TMonsterDesc310007 => null;

		public static TLocalizedText TMonsterBossRadioText310007 => null;

		public static TLocalizedText TMonsterName310008 => null;

		public static TLocalizedText TMonsterDesc310008 => null;

		public static TLocalizedText TMonsterBossRadioText310008 => null;

		public static TLocalizedText TMonsterName310009 => null;

		public static TLocalizedText TMonsterDesc310009 => null;

		public static TLocalizedText TMonsterBossRadioText310009 => null;

		public static TLocalizedText TMonsterName310010 => null;

		public static TLocalizedText TMonsterDesc310010 => null;

		public static TLocalizedText TMonsterBossRadioText310010 => null;

		public static TLocalizedText TMonsterName310011 => null;

		public static TLocalizedText TMonsterDesc310011 => null;

		public static TLocalizedText TMonsterBossRadioText310011 => null;

		public static TLocalizedText TMonsterName310012 => null;

		public static TLocalizedText TMonsterDesc310012 => null;

		public static TLocalizedText TMonsterBossRadioText310012 => null;

		public static TLocalizedText TMonsterName310013 => null;

		public static TLocalizedText TMonsterDesc310013 => null;

		public static TLocalizedText TMonsterBossRadioText310013 => null;

		public static TLocalizedText TMonsterName310014 => null;

		public static TLocalizedText TMonsterDesc310014 => null;

		public static TLocalizedText TMonsterBossRadioText310014 => null;

		public static TLocalizedText TMonsterName310015 => null;

		public static TLocalizedText TMonsterDesc310015 => null;

		public static TLocalizedText TMonsterBossRadioText310015 => null;

		public static TLocalizedText TMonsterName310051 => null;

		public static TLocalizedText TMonsterDesc310051 => null;

		public static TLocalizedText TMonsterBossRadioText310051 => null;

		public static TLocalizedText TMonsterName310052 => null;

		public static TLocalizedText TMonsterDesc310052 => null;

		public static TLocalizedText TMonsterBossRadioText310052 => null;

		public static TLocalizedText TMonsterName310053 => null;

		public static TLocalizedText TMonsterDesc310053 => null;

		public static TLocalizedText TMonsterBossRadioText310053 => null;

		public static TLocalizedText TMonsterName310054 => null;

		public static TLocalizedText TMonsterDesc310054 => null;

		public static TLocalizedText TMonsterBossRadioText310054 => null;

		public static TLocalizedText TMonsterName310055 => null;

		public static TLocalizedText TMonsterDesc310055 => null;

		public static TLocalizedText TMonsterBossRadioText310055 => null;

		public static TLocalizedText TMonsterName310056 => null;

		public static TLocalizedText TMonsterDesc310056 => null;

		public static TLocalizedText TMonsterBossRadioText310056 => null;

		public static TLocalizedText TMonsterName310057 => null;

		public static TLocalizedText TMonsterDesc310057 => null;

		public static TLocalizedText TMonsterBossRadioText310057 => null;

		public static TLocalizedText TMonsterName310058 => null;

		public static TLocalizedText TMonsterDesc310058 => null;

		public static TLocalizedText TMonsterBossRadioText310058 => null;

		public static TLocalizedText TMonsterName310059 => null;

		public static TLocalizedText TMonsterDesc310059 => null;

		public static TLocalizedText TMonsterBossRadioText310059 => null;

		public static TLocalizedText TMonsterName310060 => null;

		public static TLocalizedText TMonsterDesc310060 => null;

		public static TLocalizedText TMonsterBossRadioText310060 => null;

		public static TLocalizedText TMonsterName310061 => null;

		public static TLocalizedText TMonsterDesc310061 => null;

		public static TLocalizedText TMonsterBossRadioText310061 => null;

		public static TLocalizedText TMonsterName310062 => null;

		public static TLocalizedText TMonsterDesc310062 => null;

		public static TLocalizedText TMonsterBossRadioText310062 => null;

		public static TLocalizedText TMonsterName500031 => null;

		public static TLocalizedText TMonsterDesc500031 => null;

		public static TLocalizedText TMonsterBossRadioText500031 => null;

		public static TLocalizedText TMonsterName500032 => null;

		public static TLocalizedText TMonsterDesc500032 => null;

		public static TLocalizedText TMonsterBossRadioText500032 => null;

		public static TLocalizedText TMonsterName700002 => null;

		public static TLocalizedText TMonsterDesc700002 => null;

		public static TLocalizedText TMonsterBossRadioText700002 => null;

		public static TLocalizedText TMonsterName710001 => null;

		public static TLocalizedText TMonsterDesc710001 => null;

		public static TLocalizedText TMonsterBossRadioText710001 => null;

		public static TLocalizedText TMonsterName710002 => null;

		public static TLocalizedText TMonsterDesc710002 => null;

		public static TLocalizedText TMonsterBossRadioText710002 => null;

		public static TLocalizedText TMonsterName800001 => null;

		public static TLocalizedText TMonsterDesc800001 => null;

		public static TLocalizedText TMonsterBossRadioText800001 => null;

		public static TLocalizedText TMonsterName800002 => null;

		public static TLocalizedText TMonsterDesc800002 => null;

		public static TLocalizedText TMonsterBossRadioText800002 => null;

		public static TLocalizedText TMonsterName800003 => null;

		public static TLocalizedText TMonsterDesc800003 => null;

		public static TLocalizedText TMonsterBossRadioText800003 => null;

		public static TLocalizedText TMonsterName800004 => null;

		public static TLocalizedText TMonsterDesc800004 => null;

		public static TLocalizedText TMonsterBossRadioText800004 => null;

		public static TLocalizedText TMonsterName800005 => null;

		public static TLocalizedText TMonsterDesc800005 => null;

		public static TLocalizedText TMonsterBossRadioText800005 => null;

		public static TLocalizedText TMonsterName800006 => null;

		public static TLocalizedText TMonsterDesc800006 => null;

		public static TLocalizedText TMonsterBossRadioText800006 => null;

		public static TLocalizedText TMonsterName800007 => null;

		public static TLocalizedText TMonsterDesc800007 => null;

		public static TLocalizedText TMonsterBossRadioText800007 => null;

		public static TLocalizedText TMonsterName800008 => null;

		public static TLocalizedText TMonsterDesc800008 => null;

		public static TLocalizedText TMonsterBossRadioText800008 => null;

		public static TLocalizedText TMonsterName800009 => null;

		public static TLocalizedText TMonsterDesc800009 => null;

		public static TLocalizedText TMonsterBossRadioText800009 => null;

		public static TLocalizedText TMonsterName800010 => null;

		public static TLocalizedText TMonsterDesc800010 => null;

		public static TLocalizedText TMonsterBossRadioText800010 => null;

		public static TLocalizedText TMonsterName800011 => null;

		public static TLocalizedText TMonsterDesc800011 => null;

		public static TLocalizedText TMonsterBossRadioText800011 => null;

		public static TLocalizedText TMonsterName810001 => null;

		public static TLocalizedText TMonsterDesc810001 => null;

		public static TLocalizedText TMonsterBossRadioText810001 => null;

		public static TLocalizedText TMonsterName820001 => null;

		public static TLocalizedText TMonsterDesc820001 => null;

		public static TLocalizedText TMonsterBossRadioText820001 => null;

		public static TLocalizedText TMonsterName820002 => null;

		public static TLocalizedText TMonsterDesc820002 => null;

		public static TLocalizedText TMonsterBossRadioText820002 => null;

		public static TLocalizedText TMonsterName820003 => null;

		public static TLocalizedText TMonsterDesc820003 => null;

		public static TLocalizedText TMonsterBossRadioText820003 => null;

		public static TLocalizedText TMonsterName820004 => null;

		public static TLocalizedText TMonsterDesc820004 => null;

		public static TLocalizedText TMonsterBossRadioText820004 => null;

		public static TLocalizedText TMonsterName820005 => null;

		public static TLocalizedText TMonsterDesc820005 => null;

		public static TLocalizedText TMonsterBossRadioText820005 => null;

		public static TLocalizedText TMonsterName820006 => null;

		public static TLocalizedText TMonsterDesc820006 => null;

		public static TLocalizedText TMonsterBossRadioText820006 => null;

		public static TLocalizedText TMonsterName820007 => null;

		public static TLocalizedText TMonsterDesc820007 => null;

		public static TLocalizedText TMonsterBossRadioText820007 => null;

		public static TLocalizedText TMonsterName900001 => null;

		public static TLocalizedText TMonsterDesc900001 => null;

		public static TLocalizedText TMonsterBossRadioText900001 => null;

		public static TLocalizedText TMonsterName900002 => null;

		public static TLocalizedText TMonsterDesc900002 => null;

		public static TLocalizedText TMonsterBossRadioText900002 => null;

		public static TLocalizedText TMonsterName900003 => null;

		public static TLocalizedText TMonsterDesc900003 => null;

		public static TLocalizedText TMonsterBossRadioText900003 => null;

		public static TLocalizedText TMonsterName900004 => null;

		public static TLocalizedText TMonsterDesc900004 => null;

		public static TLocalizedText TMonsterBossRadioText900004 => null;

		public static TLocalizedText TMonsterName900005 => null;

		public static TLocalizedText TMonsterDesc900005 => null;

		public static TLocalizedText TMonsterBossRadioText900005 => null;

		public static TLocalizedText TMonsterName900006 => null;

		public static TLocalizedText TMonsterDesc900006 => null;

		public static TLocalizedText TMonsterBossRadioText900006 => null;

		public static TLocalizedText TMonsterName900007 => null;

		public static TLocalizedText TMonsterDesc900007 => null;

		public static TLocalizedText TMonsterBossRadioText900007 => null;

		public static TLocalizedText TMonsterName900100 => null;

		public static TLocalizedText TMonsterDesc900100 => null;

		public static TLocalizedText TMonsterBossRadioText900100 => null;

		public static TLocalizedText TNpcName600001 => null;

		public static TLocalizedText TNpcSubName600001 => null;

		public static TLocalizedText TNpcDesc600001 => null;

		public static TLocalizedText TNpcDefaultDialog600001 => null;

		public static TLocalizedText TNpcToastText600001 => null;

		public static TLocalizedText TNpcName600002 => null;

		public static TLocalizedText TNpcSubName600002 => null;

		public static TLocalizedText TNpcDesc600002 => null;

		public static TLocalizedText TNpcDefaultDialog600002 => null;

		public static TLocalizedText TNpcToastText600002 => null;

		public static TLocalizedText TNpcName600003 => null;

		public static TLocalizedText TNpcSubName600003 => null;

		public static TLocalizedText TNpcDesc600003 => null;

		public static TLocalizedText TNpcDefaultDialog600003 => null;

		public static TLocalizedText TNpcToastText600003 => null;

		public static TLocalizedText TNpcName600004 => null;

		public static TLocalizedText TNpcSubName600004 => null;

		public static TLocalizedText TNpcDesc600004 => null;

		public static TLocalizedText TNpcDefaultDialog600004 => null;

		public static TLocalizedText TNpcToastText600004 => null;

		public static TLocalizedText TNpcName600005 => null;

		public static TLocalizedText TNpcSubName600005 => null;

		public static TLocalizedText TNpcDesc600005 => null;

		public static TLocalizedText TNpcDefaultDialog600005 => null;

		public static TLocalizedText TNpcToastText600005 => null;

		public static TLocalizedText TNpcName600006 => null;

		public static TLocalizedText TNpcSubName600006 => null;

		public static TLocalizedText TNpcDesc600006 => null;

		public static TLocalizedText TNpcDefaultDialog600006 => null;

		public static TLocalizedText TNpcToastText600006 => null;

		public static TLocalizedText TNpcName600007 => null;

		public static TLocalizedText TNpcSubName600007 => null;

		public static TLocalizedText TNpcDesc600007 => null;

		public static TLocalizedText TNpcDefaultDialog600007 => null;

		public static TLocalizedText TNpcToastText600007 => null;

		public static TLocalizedText TNpcName600008 => null;

		public static TLocalizedText TNpcSubName600008 => null;

		public static TLocalizedText TNpcDesc600008 => null;

		public static TLocalizedText TNpcDefaultDialog600008 => null;

		public static TLocalizedText TNpcToastText600008 => null;

		public static TLocalizedText TNpcName600009 => null;

		public static TLocalizedText TNpcSubName600009 => null;

		public static TLocalizedText TNpcDesc600009 => null;

		public static TLocalizedText TNpcDefaultDialog600009 => null;

		public static TLocalizedText TNpcToastText600009 => null;

		public static TLocalizedText TNpcName600010 => null;

		public static TLocalizedText TNpcSubName600010 => null;

		public static TLocalizedText TNpcDesc600010 => null;

		public static TLocalizedText TNpcDefaultDialog600010 => null;

		public static TLocalizedText TNpcToastText600010 => null;

		public static TLocalizedText TNpcName600011 => null;

		public static TLocalizedText TNpcSubName600011 => null;

		public static TLocalizedText TNpcDesc600011 => null;

		public static TLocalizedText TNpcDefaultDialog600011 => null;

		public static TLocalizedText TNpcToastText600011 => null;

		public static TLocalizedText TNpcName600012 => null;

		public static TLocalizedText TNpcSubName600012 => null;

		public static TLocalizedText TNpcDesc600012 => null;

		public static TLocalizedText TNpcDefaultDialog600012 => null;

		public static TLocalizedText TNpcToastText600012 => null;

		public static TLocalizedText TNpcName600013 => null;

		public static TLocalizedText TNpcSubName600013 => null;

		public static TLocalizedText TNpcDesc600013 => null;

		public static TLocalizedText TNpcDefaultDialog600013 => null;

		public static TLocalizedText TNpcToastText600013 => null;

		public static TLocalizedText TNpcName700000 => null;

		public static TLocalizedText TNpcSubName700000 => null;

		public static TLocalizedText TNpcDesc700000 => null;

		public static TLocalizedText TNpcDefaultDialog700000 => null;

		public static TLocalizedText TNpcToastText700000 => null;

		public static TLocalizedText TNpcName700001 => null;

		public static TLocalizedText TNpcSubName700001 => null;

		public static TLocalizedText TNpcDesc700001 => null;

		public static TLocalizedText TNpcDefaultDialog700001 => null;

		public static TLocalizedText TNpcToastText700001 => null;

		public static TLocalizedText TNpcName700002 => null;

		public static TLocalizedText TNpcSubName700002 => null;

		public static TLocalizedText TNpcDesc700002 => null;

		public static TLocalizedText TNpcDefaultDialog700002 => null;

		public static TLocalizedText TNpcToastText700002 => null;

		public static TLocalizedText TNpcName700003 => null;

		public static TLocalizedText TNpcSubName700003 => null;

		public static TLocalizedText TNpcDesc700003 => null;

		public static TLocalizedText TNpcDefaultDialog700003 => null;

		public static TLocalizedText TNpcToastText700003 => null;

		public static TLocalizedText TNpcName700004 => null;

		public static TLocalizedText TNpcSubName700004 => null;

		public static TLocalizedText TNpcDesc700004 => null;

		public static TLocalizedText TNpcDefaultDialog700004 => null;

		public static TLocalizedText TNpcToastText700004 => null;

		public static TLocalizedText TNpcName700005 => null;

		public static TLocalizedText TNpcSubName700005 => null;

		public static TLocalizedText TNpcDesc700005 => null;

		public static TLocalizedText TNpcDefaultDialog700005 => null;

		public static TLocalizedText TNpcToastText700005 => null;

		public static TLocalizedText TNpcName700006 => null;

		public static TLocalizedText TNpcSubName700006 => null;

		public static TLocalizedText TNpcDesc700006 => null;

		public static TLocalizedText TNpcDefaultDialog700006 => null;

		public static TLocalizedText TNpcToastText700006 => null;

		public static TLocalizedText TNpcName700008 => null;

		public static TLocalizedText TNpcSubName700008 => null;

		public static TLocalizedText TNpcDesc700008 => null;

		public static TLocalizedText TNpcDefaultDialog700008 => null;

		public static TLocalizedText TNpcToastText700008 => null;

		public static TLocalizedText TNpcName700009 => null;

		public static TLocalizedText TNpcSubName700009 => null;

		public static TLocalizedText TNpcDesc700009 => null;

		public static TLocalizedText TNpcDefaultDialog700009 => null;

		public static TLocalizedText TNpcToastText700009 => null;

		public static TLocalizedText TNpcName700010 => null;

		public static TLocalizedText TNpcSubName700010 => null;

		public static TLocalizedText TNpcDesc700010 => null;

		public static TLocalizedText TNpcDefaultDialog700010 => null;

		public static TLocalizedText TNpcToastText700010 => null;

		public static TLocalizedText TNpcName700011 => null;

		public static TLocalizedText TNpcSubName700011 => null;

		public static TLocalizedText TNpcDesc700011 => null;

		public static TLocalizedText TNpcDefaultDialog700011 => null;

		public static TLocalizedText TNpcToastText700011 => null;

		public static TLocalizedText TNpcName700012 => null;

		public static TLocalizedText TNpcSubName700012 => null;

		public static TLocalizedText TNpcDesc700012 => null;

		public static TLocalizedText TNpcDefaultDialog700012 => null;

		public static TLocalizedText TNpcToastText700012 => null;

		public static TLocalizedText TNpcName700013 => null;

		public static TLocalizedText TNpcSubName700013 => null;

		public static TLocalizedText TNpcDesc700013 => null;

		public static TLocalizedText TNpcDefaultDialog700013 => null;

		public static TLocalizedText TNpcToastText700013 => null;

		public static TLocalizedText TNpcName700014 => null;

		public static TLocalizedText TNpcSubName700014 => null;

		public static TLocalizedText TNpcDesc700014 => null;

		public static TLocalizedText TNpcDefaultDialog700014 => null;

		public static TLocalizedText TNpcToastText700014 => null;

		public static TLocalizedText TNpcName700015 => null;

		public static TLocalizedText TNpcSubName700015 => null;

		public static TLocalizedText TNpcDesc700015 => null;

		public static TLocalizedText TNpcDefaultDialog700015 => null;

		public static TLocalizedText TNpcToastText700015 => null;

		public static TLocalizedText TNpcName700016 => null;

		public static TLocalizedText TNpcSubName700016 => null;

		public static TLocalizedText TNpcDesc700016 => null;

		public static TLocalizedText TNpcDefaultDialog700016 => null;

		public static TLocalizedText TNpcToastText700016 => null;

		public static TLocalizedText TNpcName700017 => null;

		public static TLocalizedText TNpcSubName700017 => null;

		public static TLocalizedText TNpcDesc700017 => null;

		public static TLocalizedText TNpcDefaultDialog700017 => null;

		public static TLocalizedText TNpcToastText700017 => null;

		public static TLocalizedText TNpcName700101 => null;

		public static TLocalizedText TNpcSubName700101 => null;

		public static TLocalizedText TNpcDesc700101 => null;

		public static TLocalizedText TNpcDefaultDialog700101 => null;

		public static TLocalizedText TNpcToastText700101 => null;

		public static TLocalizedText TNpcName700102 => null;

		public static TLocalizedText TNpcSubName700102 => null;

		public static TLocalizedText TNpcDesc700102 => null;

		public static TLocalizedText TNpcDefaultDialog700102 => null;

		public static TLocalizedText TNpcToastText700102 => null;

		public static TLocalizedText TNpcName700103 => null;

		public static TLocalizedText TNpcSubName700103 => null;

		public static TLocalizedText TNpcDesc700103 => null;

		public static TLocalizedText TNpcDefaultDialog700103 => null;

		public static TLocalizedText TNpcToastText700103 => null;

		public static TLocalizedText TNpcName700104 => null;

		public static TLocalizedText TNpcSubName700104 => null;

		public static TLocalizedText TNpcDesc700104 => null;

		public static TLocalizedText TNpcDefaultDialog700104 => null;

		public static TLocalizedText TNpcToastText700104 => null;

		public static TLocalizedText TNpcName700105 => null;

		public static TLocalizedText TNpcSubName700105 => null;

		public static TLocalizedText TNpcDesc700105 => null;

		public static TLocalizedText TNpcDefaultDialog700105 => null;

		public static TLocalizedText TNpcToastText700105 => null;

		public static TLocalizedText TNpcName700106 => null;

		public static TLocalizedText TNpcSubName700106 => null;

		public static TLocalizedText TNpcDesc700106 => null;

		public static TLocalizedText TNpcDefaultDialog700106 => null;

		public static TLocalizedText TNpcToastText700106 => null;

		public static TLocalizedText TNpcName700107 => null;

		public static TLocalizedText TNpcSubName700107 => null;

		public static TLocalizedText TNpcDesc700107 => null;

		public static TLocalizedText TNpcDefaultDialog700107 => null;

		public static TLocalizedText TNpcToastText700107 => null;

		public static TLocalizedText TNpcName700108 => null;

		public static TLocalizedText TNpcSubName700108 => null;

		public static TLocalizedText TNpcDesc700108 => null;

		public static TLocalizedText TNpcDefaultDialog700108 => null;

		public static TLocalizedText TNpcToastText700108 => null;

		public static TLocalizedText TNpcName700109 => null;

		public static TLocalizedText TNpcSubName700109 => null;

		public static TLocalizedText TNpcDesc700109 => null;

		public static TLocalizedText TNpcDefaultDialog700109 => null;

		public static TLocalizedText TNpcToastText700109 => null;

		public static TLocalizedText TNpcName700110 => null;

		public static TLocalizedText TNpcSubName700110 => null;

		public static TLocalizedText TNpcDesc700110 => null;

		public static TLocalizedText TNpcDefaultDialog700110 => null;

		public static TLocalizedText TNpcToastText700110 => null;

		public static TLocalizedText TNpcName700111 => null;

		public static TLocalizedText TNpcSubName700111 => null;

		public static TLocalizedText TNpcDesc700111 => null;

		public static TLocalizedText TNpcDefaultDialog700111 => null;

		public static TLocalizedText TNpcToastText700111 => null;

		public static TLocalizedText TNpcName700112 => null;

		public static TLocalizedText TNpcSubName700112 => null;

		public static TLocalizedText TNpcDesc700112 => null;

		public static TLocalizedText TNpcDefaultDialog700112 => null;

		public static TLocalizedText TNpcToastText700112 => null;

		public static TLocalizedText TNpcName700113 => null;

		public static TLocalizedText TNpcSubName700113 => null;

		public static TLocalizedText TNpcDesc700113 => null;

		public static TLocalizedText TNpcDefaultDialog700113 => null;

		public static TLocalizedText TNpcToastText700113 => null;

		public static TLocalizedText TNpcName700114 => null;

		public static TLocalizedText TNpcSubName700114 => null;

		public static TLocalizedText TNpcDesc700114 => null;

		public static TLocalizedText TNpcDefaultDialog700114 => null;

		public static TLocalizedText TNpcToastText700114 => null;

		public static TLocalizedText TNpcName700115 => null;

		public static TLocalizedText TNpcSubName700115 => null;

		public static TLocalizedText TNpcDesc700115 => null;

		public static TLocalizedText TNpcDefaultDialog700115 => null;

		public static TLocalizedText TNpcToastText700115 => null;

		public static TLocalizedText TNpcName700150 => null;

		public static TLocalizedText TNpcSubName700150 => null;

		public static TLocalizedText TNpcDesc700150 => null;

		public static TLocalizedText TNpcDefaultDialog700150 => null;

		public static TLocalizedText TNpcToastText700150 => null;

		public static TLocalizedText TNpcName700151 => null;

		public static TLocalizedText TNpcSubName700151 => null;

		public static TLocalizedText TNpcDesc700151 => null;

		public static TLocalizedText TNpcDefaultDialog700151 => null;

		public static TLocalizedText TNpcToastText700151 => null;

		public static TLocalizedText TNpcName700152 => null;

		public static TLocalizedText TNpcSubName700152 => null;

		public static TLocalizedText TNpcDesc700152 => null;

		public static TLocalizedText TNpcDefaultDialog700152 => null;

		public static TLocalizedText TNpcToastText700152 => null;

		public static TLocalizedText TNpcName700200 => null;

		public static TLocalizedText TNpcSubName700200 => null;

		public static TLocalizedText TNpcDesc700200 => null;

		public static TLocalizedText TNpcDefaultDialog700200 => null;

		public static TLocalizedText TNpcToastText700200 => null;

		public static TLocalizedText TNpcName700201 => null;

		public static TLocalizedText TNpcSubName700201 => null;

		public static TLocalizedText TNpcDesc700201 => null;

		public static TLocalizedText TNpcDefaultDialog700201 => null;

		public static TLocalizedText TNpcToastText700201 => null;

		public static TLocalizedText TNpcName700202 => null;

		public static TLocalizedText TNpcSubName700202 => null;

		public static TLocalizedText TNpcDesc700202 => null;

		public static TLocalizedText TNpcDefaultDialog700202 => null;

		public static TLocalizedText TNpcToastText700202 => null;

		public static TLocalizedText TNpcName700203 => null;

		public static TLocalizedText TNpcSubName700203 => null;

		public static TLocalizedText TNpcDesc700203 => null;

		public static TLocalizedText TNpcDefaultDialog700203 => null;

		public static TLocalizedText TNpcToastText700203 => null;

		public static TLocalizedText TNpcName700204 => null;

		public static TLocalizedText TNpcSubName700204 => null;

		public static TLocalizedText TNpcDesc700204 => null;

		public static TLocalizedText TNpcDefaultDialog700204 => null;

		public static TLocalizedText TNpcToastText700204 => null;

		public static TLocalizedText TNpcName700205 => null;

		public static TLocalizedText TNpcSubName700205 => null;

		public static TLocalizedText TNpcDesc700205 => null;

		public static TLocalizedText TNpcDefaultDialog700205 => null;

		public static TLocalizedText TNpcToastText700205 => null;

		public static TLocalizedText TNpcName700206 => null;

		public static TLocalizedText TNpcSubName700206 => null;

		public static TLocalizedText TNpcDesc700206 => null;

		public static TLocalizedText TNpcDefaultDialog700206 => null;

		public static TLocalizedText TNpcToastText700206 => null;

		public static TLocalizedText TNpcName700207 => null;

		public static TLocalizedText TNpcSubName700207 => null;

		public static TLocalizedText TNpcDesc700207 => null;

		public static TLocalizedText TNpcDefaultDialog700207 => null;

		public static TLocalizedText TNpcToastText700207 => null;

		public static TLocalizedText TNpcName700208 => null;

		public static TLocalizedText TNpcSubName700208 => null;

		public static TLocalizedText TNpcDesc700208 => null;

		public static TLocalizedText TNpcDefaultDialog700208 => null;

		public static TLocalizedText TNpcToastText700208 => null;

		public static TLocalizedText TNpcName700209 => null;

		public static TLocalizedText TNpcSubName700209 => null;

		public static TLocalizedText TNpcDesc700209 => null;

		public static TLocalizedText TNpcDefaultDialog700209 => null;

		public static TLocalizedText TNpcToastText700209 => null;

		public static TLocalizedText TNpcName700210 => null;

		public static TLocalizedText TNpcSubName700210 => null;

		public static TLocalizedText TNpcDesc700210 => null;

		public static TLocalizedText TNpcDefaultDialog700210 => null;

		public static TLocalizedText TNpcToastText700210 => null;

		public static TLocalizedText TNpcName700211 => null;

		public static TLocalizedText TNpcSubName700211 => null;

		public static TLocalizedText TNpcDesc700211 => null;

		public static TLocalizedText TNpcDefaultDialog700211 => null;

		public static TLocalizedText TNpcToastText700211 => null;

		public static TLocalizedText TNpcName700212 => null;

		public static TLocalizedText TNpcSubName700212 => null;

		public static TLocalizedText TNpcDesc700212 => null;

		public static TLocalizedText TNpcDefaultDialog700212 => null;

		public static TLocalizedText TNpcToastText700212 => null;

		public static TLocalizedText TNpcName700213 => null;

		public static TLocalizedText TNpcSubName700213 => null;

		public static TLocalizedText TNpcDesc700213 => null;

		public static TLocalizedText TNpcDefaultDialog700213 => null;

		public static TLocalizedText TNpcToastText700213 => null;

		public static TLocalizedText TNpcName700214 => null;

		public static TLocalizedText TNpcSubName700214 => null;

		public static TLocalizedText TNpcDesc700214 => null;

		public static TLocalizedText TNpcDefaultDialog700214 => null;

		public static TLocalizedText TNpcToastText700214 => null;

		public static TLocalizedText TNpcName700215 => null;

		public static TLocalizedText TNpcSubName700215 => null;

		public static TLocalizedText TNpcDesc700215 => null;

		public static TLocalizedText TNpcDefaultDialog700215 => null;

		public static TLocalizedText TNpcToastText700215 => null;

		public static TLocalizedText TNpcName700216 => null;

		public static TLocalizedText TNpcSubName700216 => null;

		public static TLocalizedText TNpcDesc700216 => null;

		public static TLocalizedText TNpcDefaultDialog700216 => null;

		public static TLocalizedText TNpcToastText700216 => null;

		public static TLocalizedText TNpcName700217 => null;

		public static TLocalizedText TNpcSubName700217 => null;

		public static TLocalizedText TNpcDesc700217 => null;

		public static TLocalizedText TNpcDefaultDialog700217 => null;

		public static TLocalizedText TNpcToastText700217 => null;

		public static TLocalizedText TNpcName700218 => null;

		public static TLocalizedText TNpcSubName700218 => null;

		public static TLocalizedText TNpcDesc700218 => null;

		public static TLocalizedText TNpcDefaultDialog700218 => null;

		public static TLocalizedText TNpcToastText700218 => null;

		public static TLocalizedText TNpcName700219 => null;

		public static TLocalizedText TNpcSubName700219 => null;

		public static TLocalizedText TNpcDesc700219 => null;

		public static TLocalizedText TNpcDefaultDialog700219 => null;

		public static TLocalizedText TNpcToastText700219 => null;

		public static TLocalizedText TNpcName700220 => null;

		public static TLocalizedText TNpcSubName700220 => null;

		public static TLocalizedText TNpcDesc700220 => null;

		public static TLocalizedText TNpcDefaultDialog700220 => null;

		public static TLocalizedText TNpcToastText700220 => null;

		public static TLocalizedText TNpcName700221 => null;

		public static TLocalizedText TNpcSubName700221 => null;

		public static TLocalizedText TNpcDesc700221 => null;

		public static TLocalizedText TNpcDefaultDialog700221 => null;

		public static TLocalizedText TNpcToastText700221 => null;

		public static TLocalizedText TNpcName700222 => null;

		public static TLocalizedText TNpcSubName700222 => null;

		public static TLocalizedText TNpcDesc700222 => null;

		public static TLocalizedText TNpcDefaultDialog700222 => null;

		public static TLocalizedText TNpcToastText700222 => null;

		public static TLocalizedText TNpcName700230 => null;

		public static TLocalizedText TNpcSubName700230 => null;

		public static TLocalizedText TNpcDesc700230 => null;

		public static TLocalizedText TNpcDefaultDialog700230 => null;

		public static TLocalizedText TNpcToastText700230 => null;

		public static TLocalizedText TNpcName700231 => null;

		public static TLocalizedText TNpcSubName700231 => null;

		public static TLocalizedText TNpcDesc700231 => null;

		public static TLocalizedText TNpcDefaultDialog700231 => null;

		public static TLocalizedText TNpcToastText700231 => null;

		public static TLocalizedText TNpcName700232 => null;

		public static TLocalizedText TNpcSubName700232 => null;

		public static TLocalizedText TNpcDesc700232 => null;

		public static TLocalizedText TNpcDefaultDialog700232 => null;

		public static TLocalizedText TNpcToastText700232 => null;

		public static TLocalizedText TNpcName700233 => null;

		public static TLocalizedText TNpcSubName700233 => null;

		public static TLocalizedText TNpcDesc700233 => null;

		public static TLocalizedText TNpcDefaultDialog700233 => null;

		public static TLocalizedText TNpcToastText700233 => null;

		public static TLocalizedText TNpcName700234 => null;

		public static TLocalizedText TNpcSubName700234 => null;

		public static TLocalizedText TNpcDesc700234 => null;

		public static TLocalizedText TNpcDefaultDialog700234 => null;

		public static TLocalizedText TNpcToastText700234 => null;

		public static TLocalizedText TNpcName700300 => null;

		public static TLocalizedText TNpcSubName700300 => null;

		public static TLocalizedText TNpcDesc700300 => null;

		public static TLocalizedText TNpcDefaultDialog700300 => null;

		public static TLocalizedText TNpcToastText700300 => null;

		public static TLocalizedText TNpcName700301 => null;

		public static TLocalizedText TNpcSubName700301 => null;

		public static TLocalizedText TNpcDesc700301 => null;

		public static TLocalizedText TNpcDefaultDialog700301 => null;

		public static TLocalizedText TNpcToastText700301 => null;

		public static TLocalizedText TNpcName700303 => null;

		public static TLocalizedText TNpcSubName700303 => null;

		public static TLocalizedText TNpcDesc700303 => null;

		public static TLocalizedText TNpcDefaultDialog700303 => null;

		public static TLocalizedText TNpcToastText700303 => null;

		public static TLocalizedText TNpcName700306 => null;

		public static TLocalizedText TNpcSubName700306 => null;

		public static TLocalizedText TNpcDesc700306 => null;

		public static TLocalizedText TNpcDefaultDialog700306 => null;

		public static TLocalizedText TNpcToastText700306 => null;

		public static TLocalizedText TNpcName700308 => null;

		public static TLocalizedText TNpcSubName700308 => null;

		public static TLocalizedText TNpcDesc700308 => null;

		public static TLocalizedText TNpcDefaultDialog700308 => null;

		public static TLocalizedText TNpcToastText700308 => null;

		public static TLocalizedText TNpcName700309 => null;

		public static TLocalizedText TNpcSubName700309 => null;

		public static TLocalizedText TNpcDesc700309 => null;

		public static TLocalizedText TNpcDefaultDialog700309 => null;

		public static TLocalizedText TNpcToastText700309 => null;

		public static TLocalizedText TNpcName700401 => null;

		public static TLocalizedText TNpcSubName700401 => null;

		public static TLocalizedText TNpcDesc700401 => null;

		public static TLocalizedText TNpcDefaultDialog700401 => null;

		public static TLocalizedText TNpcToastText700401 => null;

		public static TLocalizedText TNpcName700402 => null;

		public static TLocalizedText TNpcSubName700402 => null;

		public static TLocalizedText TNpcDesc700402 => null;

		public static TLocalizedText TNpcDefaultDialog700402 => null;

		public static TLocalizedText TNpcToastText700402 => null;

		public static TLocalizedText TNpcName700403 => null;

		public static TLocalizedText TNpcSubName700403 => null;

		public static TLocalizedText TNpcDesc700403 => null;

		public static TLocalizedText TNpcDefaultDialog700403 => null;

		public static TLocalizedText TNpcToastText700403 => null;

		public static TLocalizedText TNpcName700404 => null;

		public static TLocalizedText TNpcSubName700404 => null;

		public static TLocalizedText TNpcDesc700404 => null;

		public static TLocalizedText TNpcDefaultDialog700404 => null;

		public static TLocalizedText TNpcToastText700404 => null;

		public static TLocalizedText TNpcName700405 => null;

		public static TLocalizedText TNpcSubName700405 => null;

		public static TLocalizedText TNpcDesc700405 => null;

		public static TLocalizedText TNpcDefaultDialog700405 => null;

		public static TLocalizedText TNpcToastText700405 => null;

		public static TLocalizedText TNpcName700411 => null;

		public static TLocalizedText TNpcSubName700411 => null;

		public static TLocalizedText TNpcDesc700411 => null;

		public static TLocalizedText TNpcDefaultDialog700411 => null;

		public static TLocalizedText TNpcToastText700411 => null;

		public static TLocalizedText TNpcName701001 => null;

		public static TLocalizedText TNpcSubName701001 => null;

		public static TLocalizedText TNpcDesc701001 => null;

		public static TLocalizedText TNpcDefaultDialog701001 => null;

		public static TLocalizedText TNpcToastText701001 => null;

		public static TLocalizedText TNpcName701002 => null;

		public static TLocalizedText TNpcSubName701002 => null;

		public static TLocalizedText TNpcDesc701002 => null;

		public static TLocalizedText TNpcDefaultDialog701002 => null;

		public static TLocalizedText TNpcToastText701002 => null;

		public static TLocalizedText TNpcName701003 => null;

		public static TLocalizedText TNpcSubName701003 => null;

		public static TLocalizedText TNpcDesc701003 => null;

		public static TLocalizedText TNpcDefaultDialog701003 => null;

		public static TLocalizedText TNpcToastText701003 => null;

		public static TLocalizedText TNpcName701004 => null;

		public static TLocalizedText TNpcSubName701004 => null;

		public static TLocalizedText TNpcDesc701004 => null;

		public static TLocalizedText TNpcDefaultDialog701004 => null;

		public static TLocalizedText TNpcToastText701004 => null;

		public static TLocalizedText TNpcName701005 => null;

		public static TLocalizedText TNpcSubName701005 => null;

		public static TLocalizedText TNpcDesc701005 => null;

		public static TLocalizedText TNpcDefaultDialog701005 => null;

		public static TLocalizedText TNpcToastText701005 => null;

		public static TLocalizedText TNpcName701011 => null;

		public static TLocalizedText TNpcSubName701011 => null;

		public static TLocalizedText TNpcDesc701011 => null;

		public static TLocalizedText TNpcDefaultDialog701011 => null;

		public static TLocalizedText TNpcToastText701011 => null;

		public static TLocalizedText TNpcName701012 => null;

		public static TLocalizedText TNpcSubName701012 => null;

		public static TLocalizedText TNpcDesc701012 => null;

		public static TLocalizedText TNpcDefaultDialog701012 => null;

		public static TLocalizedText TNpcToastText701012 => null;

		public static TLocalizedText TNpcName701013 => null;

		public static TLocalizedText TNpcSubName701013 => null;

		public static TLocalizedText TNpcDesc701013 => null;

		public static TLocalizedText TNpcDefaultDialog701013 => null;

		public static TLocalizedText TNpcToastText701013 => null;

		public static TLocalizedText TNpcName701014 => null;

		public static TLocalizedText TNpcSubName701014 => null;

		public static TLocalizedText TNpcDesc701014 => null;

		public static TLocalizedText TNpcDefaultDialog701014 => null;

		public static TLocalizedText TNpcToastText701014 => null;

		public static TLocalizedText TNpcName701015 => null;

		public static TLocalizedText TNpcSubName701015 => null;

		public static TLocalizedText TNpcDesc701015 => null;

		public static TLocalizedText TNpcDefaultDialog701015 => null;

		public static TLocalizedText TNpcToastText701015 => null;

		public static TLocalizedText TNpcName701016 => null;

		public static TLocalizedText TNpcSubName701016 => null;

		public static TLocalizedText TNpcDesc701016 => null;

		public static TLocalizedText TNpcDefaultDialog701016 => null;

		public static TLocalizedText TNpcToastText701016 => null;

		public static TLocalizedText TNpcName701017 => null;

		public static TLocalizedText TNpcSubName701017 => null;

		public static TLocalizedText TNpcDesc701017 => null;

		public static TLocalizedText TNpcDefaultDialog701017 => null;

		public static TLocalizedText TNpcToastText701017 => null;

		public static TLocalizedText TNpcName701018 => null;

		public static TLocalizedText TNpcSubName701018 => null;

		public static TLocalizedText TNpcDesc701018 => null;

		public static TLocalizedText TNpcDefaultDialog701018 => null;

		public static TLocalizedText TNpcToastText701018 => null;

		public static TLocalizedText TNpcName701021 => null;

		public static TLocalizedText TNpcSubName701021 => null;

		public static TLocalizedText TNpcDesc701021 => null;

		public static TLocalizedText TNpcDefaultDialog701021 => null;

		public static TLocalizedText TNpcToastText701021 => null;

		public static TLocalizedText TNpcName701022 => null;

		public static TLocalizedText TNpcSubName701022 => null;

		public static TLocalizedText TNpcDesc701022 => null;

		public static TLocalizedText TNpcDefaultDialog701022 => null;

		public static TLocalizedText TNpcToastText701022 => null;

		public static TLocalizedText TNpcName701023 => null;

		public static TLocalizedText TNpcSubName701023 => null;

		public static TLocalizedText TNpcDesc701023 => null;

		public static TLocalizedText TNpcDefaultDialog701023 => null;

		public static TLocalizedText TNpcToastText701023 => null;

		public static TLocalizedText TNpcName701024 => null;

		public static TLocalizedText TNpcSubName701024 => null;

		public static TLocalizedText TNpcDesc701024 => null;

		public static TLocalizedText TNpcDefaultDialog701024 => null;

		public static TLocalizedText TNpcToastText701024 => null;

		public static TLocalizedText TNpcName701025 => null;

		public static TLocalizedText TNpcSubName701025 => null;

		public static TLocalizedText TNpcDesc701025 => null;

		public static TLocalizedText TNpcDefaultDialog701025 => null;

		public static TLocalizedText TNpcToastText701025 => null;

		public static TLocalizedText TNpcName701026 => null;

		public static TLocalizedText TNpcSubName701026 => null;

		public static TLocalizedText TNpcDesc701026 => null;

		public static TLocalizedText TNpcDefaultDialog701026 => null;

		public static TLocalizedText TNpcToastText701026 => null;

		public static TLocalizedText TNpcName710101 => null;

		public static TLocalizedText TNpcSubName710101 => null;

		public static TLocalizedText TNpcDesc710101 => null;

		public static TLocalizedText TNpcDefaultDialog710101 => null;

		public static TLocalizedText TNpcToastText710101 => null;

		public static TLocalizedText TNpcName710102 => null;

		public static TLocalizedText TNpcSubName710102 => null;

		public static TLocalizedText TNpcDesc710102 => null;

		public static TLocalizedText TNpcDefaultDialog710102 => null;

		public static TLocalizedText TNpcToastText710102 => null;

		public static TLocalizedText TNpcName710103 => null;

		public static TLocalizedText TNpcSubName710103 => null;

		public static TLocalizedText TNpcDesc710103 => null;

		public static TLocalizedText TNpcDefaultDialog710103 => null;

		public static TLocalizedText TNpcToastText710103 => null;

		public static TLocalizedText TNpcName710104 => null;

		public static TLocalizedText TNpcSubName710104 => null;

		public static TLocalizedText TNpcDesc710104 => null;

		public static TLocalizedText TNpcDefaultDialog710104 => null;

		public static TLocalizedText TNpcToastText710104 => null;

		public static TLocalizedText TNpcName710105 => null;

		public static TLocalizedText TNpcSubName710105 => null;

		public static TLocalizedText TNpcDesc710105 => null;

		public static TLocalizedText TNpcDefaultDialog710105 => null;

		public static TLocalizedText TNpcToastText710105 => null;

		public static TLocalizedText TNpcName710106 => null;

		public static TLocalizedText TNpcSubName710106 => null;

		public static TLocalizedText TNpcDesc710106 => null;

		public static TLocalizedText TNpcDefaultDialog710106 => null;

		public static TLocalizedText TNpcToastText710106 => null;

		public static TLocalizedText TNpcName710107 => null;

		public static TLocalizedText TNpcSubName710107 => null;

		public static TLocalizedText TNpcDesc710107 => null;

		public static TLocalizedText TNpcDefaultDialog710107 => null;

		public static TLocalizedText TNpcToastText710107 => null;

		public static TLocalizedText TNpcName710108 => null;

		public static TLocalizedText TNpcSubName710108 => null;

		public static TLocalizedText TNpcDesc710108 => null;

		public static TLocalizedText TNpcDefaultDialog710108 => null;

		public static TLocalizedText TNpcToastText710108 => null;

		public static TLocalizedText TNpcName710109 => null;

		public static TLocalizedText TNpcSubName710109 => null;

		public static TLocalizedText TNpcDesc710109 => null;

		public static TLocalizedText TNpcDefaultDialog710109 => null;

		public static TLocalizedText TNpcToastText710109 => null;

		public static TLocalizedText TNpcName710110 => null;

		public static TLocalizedText TNpcSubName710110 => null;

		public static TLocalizedText TNpcDesc710110 => null;

		public static TLocalizedText TNpcDefaultDialog710110 => null;

		public static TLocalizedText TNpcToastText710110 => null;

		public static TLocalizedText TNpcName710111 => null;

		public static TLocalizedText TNpcSubName710111 => null;

		public static TLocalizedText TNpcDesc710111 => null;

		public static TLocalizedText TNpcDefaultDialog710111 => null;

		public static TLocalizedText TNpcToastText710111 => null;

		public static TLocalizedText TNpcName710112 => null;

		public static TLocalizedText TNpcSubName710112 => null;

		public static TLocalizedText TNpcDesc710112 => null;

		public static TLocalizedText TNpcDefaultDialog710112 => null;

		public static TLocalizedText TNpcToastText710112 => null;

		public static TLocalizedText TNpcName710113 => null;

		public static TLocalizedText TNpcSubName710113 => null;

		public static TLocalizedText TNpcDesc710113 => null;

		public static TLocalizedText TNpcDefaultDialog710113 => null;

		public static TLocalizedText TNpcToastText710113 => null;

		public static TLocalizedText TNpcName710114 => null;

		public static TLocalizedText TNpcSubName710114 => null;

		public static TLocalizedText TNpcDesc710114 => null;

		public static TLocalizedText TNpcDefaultDialog710114 => null;

		public static TLocalizedText TNpcToastText710114 => null;

		public static TLocalizedText TNpcName710115 => null;

		public static TLocalizedText TNpcSubName710115 => null;

		public static TLocalizedText TNpcDesc710115 => null;

		public static TLocalizedText TNpcDefaultDialog710115 => null;

		public static TLocalizedText TNpcToastText710115 => null;

		public static TLocalizedText TNpcName800001 => null;

		public static TLocalizedText TNpcSubName800001 => null;

		public static TLocalizedText TNpcDesc800001 => null;

		public static TLocalizedText TNpcDefaultDialog800001 => null;

		public static TLocalizedText TNpcToastText800001 => null;

		public static TLocalizedText TNpcName800002 => null;

		public static TLocalizedText TNpcSubName800002 => null;

		public static TLocalizedText TNpcDesc800002 => null;

		public static TLocalizedText TNpcDefaultDialog800002 => null;

		public static TLocalizedText TNpcToastText800002 => null;

		public static TLocalizedText TNpcName800003 => null;

		public static TLocalizedText TNpcSubName800003 => null;

		public static TLocalizedText TNpcDesc800003 => null;

		public static TLocalizedText TNpcDefaultDialog800003 => null;

		public static TLocalizedText TNpcToastText800003 => null;

		public static TLocalizedText TNpcName800004 => null;

		public static TLocalizedText TNpcSubName800004 => null;

		public static TLocalizedText TNpcDesc800004 => null;

		public static TLocalizedText TNpcDefaultDialog800004 => null;

		public static TLocalizedText TNpcToastText800004 => null;

		public static TLocalizedText TNpcName800005 => null;

		public static TLocalizedText TNpcSubName800005 => null;

		public static TLocalizedText TNpcDesc800005 => null;

		public static TLocalizedText TNpcDefaultDialog800005 => null;

		public static TLocalizedText TNpcToastText800005 => null;

		public static TLocalizedText TNpcName800006 => null;

		public static TLocalizedText TNpcSubName800006 => null;

		public static TLocalizedText TNpcDesc800006 => null;

		public static TLocalizedText TNpcDefaultDialog800006 => null;

		public static TLocalizedText TNpcToastText800006 => null;

		public static TLocalizedText TNpcName800007 => null;

		public static TLocalizedText TNpcSubName800007 => null;

		public static TLocalizedText TNpcDesc800007 => null;

		public static TLocalizedText TNpcDefaultDialog800007 => null;

		public static TLocalizedText TNpcToastText800007 => null;

		public static TLocalizedText TNpcName800008 => null;

		public static TLocalizedText TNpcSubName800008 => null;

		public static TLocalizedText TNpcDesc800008 => null;

		public static TLocalizedText TNpcDefaultDialog800008 => null;

		public static TLocalizedText TNpcToastText800008 => null;

		public static TLocalizedText TNpcName800009 => null;

		public static TLocalizedText TNpcSubName800009 => null;

		public static TLocalizedText TNpcDesc800009 => null;

		public static TLocalizedText TNpcDefaultDialog800009 => null;

		public static TLocalizedText TNpcToastText800009 => null;

		public static TLocalizedText TNpcName800010 => null;

		public static TLocalizedText TNpcSubName800010 => null;

		public static TLocalizedText TNpcDesc800010 => null;

		public static TLocalizedText TNpcDefaultDialog800010 => null;

		public static TLocalizedText TNpcToastText800010 => null;

		public static TLocalizedText TNpcName800011 => null;

		public static TLocalizedText TNpcSubName800011 => null;

		public static TLocalizedText TNpcDesc800011 => null;

		public static TLocalizedText TNpcDefaultDialog800011 => null;

		public static TLocalizedText TNpcToastText800011 => null;

		public static TLocalizedText TNpcName800012 => null;

		public static TLocalizedText TNpcSubName800012 => null;

		public static TLocalizedText TNpcDesc800012 => null;

		public static TLocalizedText TNpcDefaultDialog800012 => null;

		public static TLocalizedText TNpcToastText800012 => null;

		public static TLocalizedText TNpcName1000001 => null;

		public static TLocalizedText TNpcSubName1000001 => null;

		public static TLocalizedText TNpcDesc1000001 => null;

		public static TLocalizedText TNpcDefaultDialog1000001 => null;

		public static TLocalizedText TNpcToastText1000001 => null;

		public static TLocalizedText TNpcName1000002 => null;

		public static TLocalizedText TNpcSubName1000002 => null;

		public static TLocalizedText TNpcDesc1000002 => null;

		public static TLocalizedText TNpcDefaultDialog1000002 => null;

		public static TLocalizedText TNpcToastText1000002 => null;

		public static TLocalizedText TParadoxName1001 => null;

		public static TLocalizedText TParadoxDesc1001 => null;

		public static TLocalizedText TParadoxName1002 => null;

		public static TLocalizedText TParadoxDesc1002 => null;

		public static TLocalizedText TParadoxName1003 => null;

		public static TLocalizedText TParadoxDesc1003 => null;

		public static TLocalizedText TParadoxName1004 => null;

		public static TLocalizedText TParadoxDesc1004 => null;

		public static TLocalizedText TParadoxName1005 => null;

		public static TLocalizedText TParadoxDesc1005 => null;

		public static TLocalizedText TParadoxName1006 => null;

		public static TLocalizedText TParadoxDesc1006 => null;

		public static TLocalizedText TParadoxName1007 => null;

		public static TLocalizedText TParadoxDesc1007 => null;

		public static TLocalizedText TParadoxName1008 => null;

		public static TLocalizedText TParadoxDesc1008 => null;

		public static TLocalizedText TParadoxName1009 => null;

		public static TLocalizedText TParadoxDesc1009 => null;

		public static TLocalizedText TParadoxName1010 => null;

		public static TLocalizedText TParadoxDesc1010 => null;

		public static TLocalizedText TParadoxName1011 => null;

		public static TLocalizedText TParadoxDesc1011 => null;

		public static TLocalizedText TParadoxName1012 => null;

		public static TLocalizedText TParadoxDesc1012 => null;

		public static TLocalizedText TParadoxName1013 => null;

		public static TLocalizedText TParadoxDesc1013 => null;

		public static TLocalizedText TParadoxName1014 => null;

		public static TLocalizedText TParadoxDesc1014 => null;

		public static TLocalizedText TParadoxName1015 => null;

		public static TLocalizedText TParadoxDesc1015 => null;

		public static TLocalizedText TParadoxName1016 => null;

		public static TLocalizedText TParadoxDesc1016 => null;

		public static TLocalizedText TParadoxName1017 => null;

		public static TLocalizedText TParadoxDesc1017 => null;

		public static TLocalizedText TParadoxName1018 => null;

		public static TLocalizedText TParadoxDesc1018 => null;

		public static TLocalizedText TParadoxName1019 => null;

		public static TLocalizedText TParadoxDesc1019 => null;

		public static TLocalizedText TParadoxName1020 => null;

		public static TLocalizedText TParadoxDesc1020 => null;

		public static TLocalizedText TParadoxName1021 => null;

		public static TLocalizedText TParadoxDesc1021 => null;

		public static TLocalizedText TParadoxName1022 => null;

		public static TLocalizedText TParadoxDesc1022 => null;

		public static TLocalizedText TParadoxName1023 => null;

		public static TLocalizedText TParadoxDesc1023 => null;

		public static TLocalizedText TParadoxName1024 => null;

		public static TLocalizedText TParadoxDesc1024 => null;

		public static TLocalizedText TParadoxName1025 => null;

		public static TLocalizedText TParadoxDesc1025 => null;

		public static TLocalizedText TParadoxName1101 => null;

		public static TLocalizedText TParadoxDesc1101 => null;

		public static TLocalizedText TParadoxName1102 => null;

		public static TLocalizedText TParadoxDesc1102 => null;

		public static TLocalizedText TParadoxName1103 => null;

		public static TLocalizedText TParadoxDesc1103 => null;

		public static TLocalizedText TParadoxName1104 => null;

		public static TLocalizedText TParadoxDesc1104 => null;

		public static TLocalizedText TParadoxName1105 => null;

		public static TLocalizedText TParadoxDesc1105 => null;

		public static TLocalizedText TParadoxName1106 => null;

		public static TLocalizedText TParadoxDesc1106 => null;

		public static TLocalizedText TParadoxName1107 => null;

		public static TLocalizedText TParadoxDesc1107 => null;

		public static TLocalizedText TParadoxName1108 => null;

		public static TLocalizedText TParadoxDesc1108 => null;

		public static TLocalizedText TParadoxName1109 => null;

		public static TLocalizedText TParadoxDesc1109 => null;

		public static TLocalizedText TParadoxName1110 => null;

		public static TLocalizedText TParadoxDesc1110 => null;

		public static TLocalizedText TParadoxName1111 => null;

		public static TLocalizedText TParadoxDesc1111 => null;

		public static TLocalizedText TParadoxName1112 => null;

		public static TLocalizedText TParadoxDesc1112 => null;

		public static TLocalizedText TParadoxName1113 => null;

		public static TLocalizedText TParadoxDesc1113 => null;

		public static TLocalizedText TParadoxName1114 => null;

		public static TLocalizedText TParadoxDesc1114 => null;

		public static TLocalizedText TParadoxName1115 => null;

		public static TLocalizedText TParadoxDesc1115 => null;

		public static TLocalizedText TParadoxName1116 => null;

		public static TLocalizedText TParadoxDesc1116 => null;

		public static TLocalizedText TParadoxName1117 => null;

		public static TLocalizedText TParadoxDesc1117 => null;

		public static TLocalizedText TParadoxName1118 => null;

		public static TLocalizedText TParadoxDesc1118 => null;

		public static TLocalizedText TParadoxName1119 => null;

		public static TLocalizedText TParadoxDesc1119 => null;

		public static TLocalizedText TParadoxName1120 => null;

		public static TLocalizedText TParadoxDesc1120 => null;

		public static TLocalizedText TParadoxName1121 => null;

		public static TLocalizedText TParadoxDesc1121 => null;

		public static TLocalizedText TParadoxName1122 => null;

		public static TLocalizedText TParadoxDesc1122 => null;

		public static TLocalizedText TParadoxName1123 => null;

		public static TLocalizedText TParadoxDesc1123 => null;

		public static TLocalizedText TParadoxName1124 => null;

		public static TLocalizedText TParadoxDesc1124 => null;

		public static TLocalizedText TParadoxName1125 => null;

		public static TLocalizedText TParadoxDesc1125 => null;

		public static TLocalizedText TParadoxName2001 => null;

		public static TLocalizedText TParadoxDesc2001 => null;

		public static TLocalizedText TParadoxName2002 => null;

		public static TLocalizedText TParadoxDesc2002 => null;

		public static TLocalizedText TParadoxName2003 => null;

		public static TLocalizedText TParadoxDesc2003 => null;

		public static TLocalizedText TParadoxName2004 => null;

		public static TLocalizedText TParadoxDesc2004 => null;

		public static TLocalizedText TParadoxName2005 => null;

		public static TLocalizedText TParadoxDesc2005 => null;

		public static TLocalizedText TParadoxName2006 => null;

		public static TLocalizedText TParadoxDesc2006 => null;

		public static TLocalizedText TParadoxName2007 => null;

		public static TLocalizedText TParadoxDesc2007 => null;

		public static TLocalizedText TParadoxName2008 => null;

		public static TLocalizedText TParadoxDesc2008 => null;

		public static TLocalizedText TParadoxName2009 => null;

		public static TLocalizedText TParadoxDesc2009 => null;

		public static TLocalizedText TParadoxName2010 => null;

		public static TLocalizedText TParadoxDesc2010 => null;

		public static TLocalizedText TParadoxName2011 => null;

		public static TLocalizedText TParadoxDesc2011 => null;

		public static TLocalizedText TParadoxName2012 => null;

		public static TLocalizedText TParadoxDesc2012 => null;

		public static TLocalizedText TParadoxName2013 => null;

		public static TLocalizedText TParadoxDesc2013 => null;

		public static TLocalizedText TParadoxName2014 => null;

		public static TLocalizedText TParadoxDesc2014 => null;

		public static TLocalizedText TParadoxName2015 => null;

		public static TLocalizedText TParadoxDesc2015 => null;

		public static TLocalizedText TParadoxName2016 => null;

		public static TLocalizedText TParadoxDesc2016 => null;

		public static TLocalizedText TParadoxName2017 => null;

		public static TLocalizedText TParadoxDesc2017 => null;

		public static TLocalizedText TParadoxName2018 => null;

		public static TLocalizedText TParadoxDesc2018 => null;

		public static TLocalizedText TParadoxName2019 => null;

		public static TLocalizedText TParadoxDesc2019 => null;

		public static TLocalizedText TParadoxName2020 => null;

		public static TLocalizedText TParadoxDesc2020 => null;

		public static TLocalizedText TParadoxName2021 => null;

		public static TLocalizedText TParadoxDesc2021 => null;

		public static TLocalizedText TParadoxName2022 => null;

		public static TLocalizedText TParadoxDesc2022 => null;

		public static TLocalizedText TParadoxName2023 => null;

		public static TLocalizedText TParadoxDesc2023 => null;

		public static TLocalizedText TParadoxName2024 => null;

		public static TLocalizedText TParadoxDesc2024 => null;

		public static TLocalizedText TParadoxName2025 => null;

		public static TLocalizedText TParadoxDesc2025 => null;

		public static TLocalizedText TParadoxName2101 => null;

		public static TLocalizedText TParadoxDesc2101 => null;

		public static TLocalizedText TParadoxName2102 => null;

		public static TLocalizedText TParadoxDesc2102 => null;

		public static TLocalizedText TParadoxName2103 => null;

		public static TLocalizedText TParadoxDesc2103 => null;

		public static TLocalizedText TParadoxName2104 => null;

		public static TLocalizedText TParadoxDesc2104 => null;

		public static TLocalizedText TParadoxName2105 => null;

		public static TLocalizedText TParadoxDesc2105 => null;

		public static TLocalizedText TParadoxName2106 => null;

		public static TLocalizedText TParadoxDesc2106 => null;

		public static TLocalizedText TParadoxName2107 => null;

		public static TLocalizedText TParadoxDesc2107 => null;

		public static TLocalizedText TParadoxName2108 => null;

		public static TLocalizedText TParadoxDesc2108 => null;

		public static TLocalizedText TParadoxName2109 => null;

		public static TLocalizedText TParadoxDesc2109 => null;

		public static TLocalizedText TParadoxName2110 => null;

		public static TLocalizedText TParadoxDesc2110 => null;

		public static TLocalizedText TParadoxName2111 => null;

		public static TLocalizedText TParadoxDesc2111 => null;

		public static TLocalizedText TParadoxName2112 => null;

		public static TLocalizedText TParadoxDesc2112 => null;

		public static TLocalizedText TParadoxName2113 => null;

		public static TLocalizedText TParadoxDesc2113 => null;

		public static TLocalizedText TParadoxName2114 => null;

		public static TLocalizedText TParadoxDesc2114 => null;

		public static TLocalizedText TParadoxName2115 => null;

		public static TLocalizedText TParadoxDesc2115 => null;

		public static TLocalizedText TParadoxName2116 => null;

		public static TLocalizedText TParadoxDesc2116 => null;

		public static TLocalizedText TParadoxName2117 => null;

		public static TLocalizedText TParadoxDesc2117 => null;

		public static TLocalizedText TParadoxName2118 => null;

		public static TLocalizedText TParadoxDesc2118 => null;

		public static TLocalizedText TParadoxName2119 => null;

		public static TLocalizedText TParadoxDesc2119 => null;

		public static TLocalizedText TParadoxName2120 => null;

		public static TLocalizedText TParadoxDesc2120 => null;

		public static TLocalizedText TParadoxName2121 => null;

		public static TLocalizedText TParadoxDesc2121 => null;

		public static TLocalizedText TParadoxName2122 => null;

		public static TLocalizedText TParadoxDesc2122 => null;

		public static TLocalizedText TParadoxName2123 => null;

		public static TLocalizedText TParadoxDesc2123 => null;

		public static TLocalizedText TParadoxName2124 => null;

		public static TLocalizedText TParadoxDesc2124 => null;

		public static TLocalizedText TParadoxName2125 => null;

		public static TLocalizedText TParadoxDesc2125 => null;

		public static TLocalizedText TParadoxName3001 => null;

		public static TLocalizedText TParadoxDesc3001 => null;

		public static TLocalizedText TParadoxName3002 => null;

		public static TLocalizedText TParadoxDesc3002 => null;

		public static TLocalizedText TParadoxName3003 => null;

		public static TLocalizedText TParadoxDesc3003 => null;

		public static TLocalizedText TParadoxName3004 => null;

		public static TLocalizedText TParadoxDesc3004 => null;

		public static TLocalizedText TParadoxName3005 => null;

		public static TLocalizedText TParadoxDesc3005 => null;

		public static TLocalizedText TParadoxName3006 => null;

		public static TLocalizedText TParadoxDesc3006 => null;

		public static TLocalizedText TParadoxName3007 => null;

		public static TLocalizedText TParadoxDesc3007 => null;

		public static TLocalizedText TParadoxName3008 => null;

		public static TLocalizedText TParadoxDesc3008 => null;

		public static TLocalizedText TParadoxName3009 => null;

		public static TLocalizedText TParadoxDesc3009 => null;

		public static TLocalizedText TParadoxName3010 => null;

		public static TLocalizedText TParadoxDesc3010 => null;

		public static TLocalizedText TParadoxName3011 => null;

		public static TLocalizedText TParadoxDesc3011 => null;

		public static TLocalizedText TParadoxName3012 => null;

		public static TLocalizedText TParadoxDesc3012 => null;

		public static TLocalizedText TParadoxName3013 => null;

		public static TLocalizedText TParadoxDesc3013 => null;

		public static TLocalizedText TParadoxName3014 => null;

		public static TLocalizedText TParadoxDesc3014 => null;

		public static TLocalizedText TParadoxName3015 => null;

		public static TLocalizedText TParadoxDesc3015 => null;

		public static TLocalizedText TParadoxName3016 => null;

		public static TLocalizedText TParadoxDesc3016 => null;

		public static TLocalizedText TParadoxName3017 => null;

		public static TLocalizedText TParadoxDesc3017 => null;

		public static TLocalizedText TParadoxName3018 => null;

		public static TLocalizedText TParadoxDesc3018 => null;

		public static TLocalizedText TParadoxName3019 => null;

		public static TLocalizedText TParadoxDesc3019 => null;

		public static TLocalizedText TParadoxName3020 => null;

		public static TLocalizedText TParadoxDesc3020 => null;

		public static TLocalizedText TParadoxName3101 => null;

		public static TLocalizedText TParadoxDesc3101 => null;

		public static TLocalizedText TParadoxName3102 => null;

		public static TLocalizedText TParadoxDesc3102 => null;

		public static TLocalizedText TParadoxName3103 => null;

		public static TLocalizedText TParadoxDesc3103 => null;

		public static TLocalizedText TParadoxName3104 => null;

		public static TLocalizedText TParadoxDesc3104 => null;

		public static TLocalizedText TParadoxName3105 => null;

		public static TLocalizedText TParadoxDesc3105 => null;

		public static TLocalizedText TParadoxName3106 => null;

		public static TLocalizedText TParadoxDesc3106 => null;

		public static TLocalizedText TParadoxName3107 => null;

		public static TLocalizedText TParadoxDesc3107 => null;

		public static TLocalizedText TParadoxName3108 => null;

		public static TLocalizedText TParadoxDesc3108 => null;

		public static TLocalizedText TParadoxName3109 => null;

		public static TLocalizedText TParadoxDesc3109 => null;

		public static TLocalizedText TParadoxName3110 => null;

		public static TLocalizedText TParadoxDesc3110 => null;

		public static TLocalizedText TParadoxName3111 => null;

		public static TLocalizedText TParadoxDesc3111 => null;

		public static TLocalizedText TParadoxName3112 => null;

		public static TLocalizedText TParadoxDesc3112 => null;

		public static TLocalizedText TParadoxName3113 => null;

		public static TLocalizedText TParadoxDesc3113 => null;

		public static TLocalizedText TParadoxName3114 => null;

		public static TLocalizedText TParadoxDesc3114 => null;

		public static TLocalizedText TParadoxName3115 => null;

		public static TLocalizedText TParadoxDesc3115 => null;

		public static TLocalizedText TParadoxName3116 => null;

		public static TLocalizedText TParadoxDesc3116 => null;

		public static TLocalizedText TParadoxName3117 => null;

		public static TLocalizedText TParadoxDesc3117 => null;

		public static TLocalizedText TParadoxName3118 => null;

		public static TLocalizedText TParadoxDesc3118 => null;

		public static TLocalizedText TParadoxName3119 => null;

		public static TLocalizedText TParadoxDesc3119 => null;

		public static TLocalizedText TParadoxName3120 => null;

		public static TLocalizedText TParadoxDesc3120 => null;

		public static TLocalizedText TParadoxName4001 => null;

		public static TLocalizedText TParadoxDesc4001 => null;

		public static TLocalizedText TParadoxName4002 => null;

		public static TLocalizedText TParadoxDesc4002 => null;

		public static TLocalizedText TParadoxName4003 => null;

		public static TLocalizedText TParadoxDesc4003 => null;

		public static TLocalizedText TParadoxName4004 => null;

		public static TLocalizedText TParadoxDesc4004 => null;

		public static TLocalizedText TParadoxName4005 => null;

		public static TLocalizedText TParadoxDesc4005 => null;

		public static TLocalizedText TParadoxName4006 => null;

		public static TLocalizedText TParadoxDesc4006 => null;

		public static TLocalizedText TParadoxName4007 => null;

		public static TLocalizedText TParadoxDesc4007 => null;

		public static TLocalizedText TParadoxName4008 => null;

		public static TLocalizedText TParadoxDesc4008 => null;

		public static TLocalizedText TParadoxName4009 => null;

		public static TLocalizedText TParadoxDesc4009 => null;

		public static TLocalizedText TPetName1001 => null;

		public static TLocalizedText TPetCombatText1001 => null;

		public static TLocalizedText TRewardBoxName100 => null;

		public static TLocalizedText TRewardBoxName110 => null;

		public static TLocalizedText TRewardBoxName1000 => null;

		public static TLocalizedText TRewardBoxName1001 => null;

		public static TLocalizedText TRewardBoxName1002 => null;

		public static TLocalizedText TRewardBoxName10000 => null;

		public static TLocalizedText TRewardBoxName10100 => null;

		public static TLocalizedText TRewardBoxName10200 => null;

		public static TLocalizedText TRewardBoxName10300 => null;

		public static TLocalizedText TRewardBoxName10310 => null;

		public static TLocalizedText TRewardBoxName10400 => null;

		public static TLocalizedText TRewardBoxName10410 => null;

		public static TLocalizedText TRewardBoxName10500 => null;

		public static TLocalizedText TRewardBoxName10510 => null;

		public static TLocalizedText TRewardBoxName10600 => null;

		public static TLocalizedText TRewardBoxName10610 => null;

		public static TLocalizedText TRewardBoxName10700 => null;

		public static TLocalizedText TRewardBoxName10800 => null;

		public static TLocalizedText TRewardBoxName10900 => null;

		public static TLocalizedText TRewardBoxName11000 => null;

		public static TLocalizedText TRewardBoxName11100 => null;

		public static TLocalizedText TRewardBoxName11101 => null;

		public static TLocalizedText TRewardBoxName11200 => null;

		public static TLocalizedText TRewardBoxName11201 => null;

		public static TLocalizedText TRewardBoxName11202 => null;

		public static TLocalizedText TRewardBoxName11300 => null;

		public static TLocalizedText TRewardBoxName11301 => null;

		public static TLocalizedText TRewardBoxName11302 => null;

		public static TLocalizedText TRewardBoxName11303 => null;

		public static TLocalizedText TRewardBoxName11304 => null;

		public static TLocalizedText TRewardBoxName11305 => null;

		public static TLocalizedText TRewardBoxName11306 => null;

		public static TLocalizedText TRewardBoxName11307 => null;

		public static TLocalizedText TRewardBoxName11308 => null;

		public static TLocalizedText TRewardBoxName11309 => null;

		public static TLocalizedText TRewardBoxName11310 => null;

		public static TLocalizedText TRewardBoxName11311 => null;

		public static TLocalizedText TRewardBoxName11312 => null;

		public static TLocalizedText TRewardBoxName11313 => null;

		public static TLocalizedText TRewardBoxName11314 => null;

		public static TLocalizedText TRewardBoxName11400 => null;

		public static TLocalizedText TRewardBoxName11401 => null;

		public static TLocalizedText TRewardBoxName20000 => null;

		public static TLocalizedText TRewardBoxName20100 => null;

		public static TLocalizedText TRewardBoxName20200 => null;

		public static TLocalizedText TRewardBoxName20300 => null;

		public static TLocalizedText TRewardBoxName20310 => null;

		public static TLocalizedText TRewardBoxName20400 => null;

		public static TLocalizedText TRewardBoxName20410 => null;

		public static TLocalizedText TRewardBoxName20500 => null;

		public static TLocalizedText TRewardBoxName20510 => null;

		public static TLocalizedText TRewardBoxName20600 => null;

		public static TLocalizedText TRewardBoxName20610 => null;

		public static TLocalizedText TRewardBoxName20700 => null;

		public static TLocalizedText TRewardBoxName20800 => null;

		public static TLocalizedText TRewardBoxName20900 => null;

		public static TLocalizedText TRewardBoxName21000 => null;

		public static TLocalizedText TRewardBoxName21100 => null;

		public static TLocalizedText TRewardBoxName21101 => null;

		public static TLocalizedText TRewardBoxName21200 => null;

		public static TLocalizedText TRewardBoxName21201 => null;

		public static TLocalizedText TRewardBoxName21202 => null;

		public static TLocalizedText TRewardBoxName21300 => null;

		public static TLocalizedText TRewardBoxName21301 => null;

		public static TLocalizedText TRewardBoxName21302 => null;

		public static TLocalizedText TRewardBoxName21303 => null;

		public static TLocalizedText TRewardBoxName21304 => null;

		public static TLocalizedText TRewardBoxName21305 => null;

		public static TLocalizedText TRewardBoxName21306 => null;

		public static TLocalizedText TRewardBoxName21307 => null;

		public static TLocalizedText TRewardBoxName21308 => null;

		public static TLocalizedText TRewardBoxName21309 => null;

		public static TLocalizedText TRewardBoxName21310 => null;

		public static TLocalizedText TRewardBoxName21311 => null;

		public static TLocalizedText TRewardBoxName21312 => null;

		public static TLocalizedText TRewardBoxName21313 => null;

		public static TLocalizedText TRewardBoxName21314 => null;

		public static TLocalizedText TRewardBoxName21400 => null;

		public static TLocalizedText TRewardBoxName21401 => null;

		public static TLocalizedText TRewardBoxName21500 => null;

		public static TLocalizedText TRewardBoxName21501 => null;

		public static TLocalizedText TSectorUnlockName1 => null;

		public static TLocalizedText TSkillName10000000 => null;

		public static TLocalizedText TSkillDesc10000000 => null;

		public static TLocalizedText TSkillName10010000 => null;

		public static TLocalizedText TSkillDesc10010000 => null;

		public static TLocalizedText TSkillName10010001 => null;

		public static TLocalizedText TSkillDesc10010001 => null;

		public static TLocalizedText TSkillName10010002 => null;

		public static TLocalizedText TSkillDesc10010002 => null;

		public static TLocalizedText TSkillName10010100 => null;

		public static TLocalizedText TSkillDesc10010100 => null;

		public static TLocalizedText TSkillName10010300 => null;

		public static TLocalizedText TSkillDesc10010300 => null;

		public static TLocalizedText TSkillName10010400 => null;

		public static TLocalizedText TSkillDesc10010400 => null;

		public static TLocalizedText TSkillName10010401 => null;

		public static TLocalizedText TSkillDesc10010401 => null;

		public static TLocalizedText TSkillName10010500 => null;

		public static TLocalizedText TSkillDesc10010500 => null;

		public static TLocalizedText TSkillName10010600 => null;

		public static TLocalizedText TSkillDesc10010600 => null;

		public static TLocalizedText TSkillName10010700 => null;

		public static TLocalizedText TSkillDesc10010700 => null;

		public static TLocalizedText TSkillName10010800 => null;

		public static TLocalizedText TSkillDesc10010800 => null;

		public static TLocalizedText TSkillName10010801 => null;

		public static TLocalizedText TSkillDesc10010801 => null;

		public static TLocalizedText TSkillName10010802 => null;

		public static TLocalizedText TSkillDesc10010802 => null;

		public static TLocalizedText TSkillName10011000 => null;

		public static TLocalizedText TSkillDesc10011000 => null;

		public static TLocalizedText TSkillName10011001 => null;

		public static TLocalizedText TSkillDesc10011001 => null;

		public static TLocalizedText TSkillName10011100 => null;

		public static TLocalizedText TSkillDesc10011100 => null;

		public static TLocalizedText TSkillName10011101 => null;

		public static TLocalizedText TSkillDesc10011101 => null;

		public static TLocalizedText TSkillName10011200 => null;

		public static TLocalizedText TSkillDesc10011200 => null;

		public static TLocalizedText TSkillName10011201 => null;

		public static TLocalizedText TSkillDesc10011201 => null;

		public static TLocalizedText TSkillName10011300 => null;

		public static TLocalizedText TSkillDesc10011300 => null;

		public static TLocalizedText TSkillName10011301 => null;

		public static TLocalizedText TSkillDesc10011301 => null;

		public static TLocalizedText TSkillName10011310 => null;

		public static TLocalizedText TSkillDesc10011310 => null;

		public static TLocalizedText TSkillName10011311 => null;

		public static TLocalizedText TSkillDesc10011311 => null;

		public static TLocalizedText TSkillName10011320 => null;

		public static TLocalizedText TSkillDesc10011320 => null;

		public static TLocalizedText TSkillName10011321 => null;

		public static TLocalizedText TSkillDesc10011321 => null;

		public static TLocalizedText TSkillName10011330 => null;

		public static TLocalizedText TSkillDesc10011330 => null;

		public static TLocalizedText TSkillName10011331 => null;

		public static TLocalizedText TSkillDesc10011331 => null;

		public static TLocalizedText TSkillName10011332 => null;

		public static TLocalizedText TSkillDesc10011332 => null;

		public static TLocalizedText TSkillName10011333 => null;

		public static TLocalizedText TSkillDesc10011333 => null;

		public static TLocalizedText TSkillName10011334 => null;

		public static TLocalizedText TSkillDesc10011334 => null;

		public static TLocalizedText TSkillName10011400 => null;

		public static TLocalizedText TSkillDesc10011400 => null;

		public static TLocalizedText TSkillName10011401 => null;

		public static TLocalizedText TSkillDesc10011401 => null;

		public static TLocalizedText TSkillName10011500 => null;

		public static TLocalizedText TSkillDesc10011500 => null;

		public static TLocalizedText TSkillName10011501 => null;

		public static TLocalizedText TSkillDesc10011501 => null;

		public static TLocalizedText TSkillName10011600 => null;

		public static TLocalizedText TSkillDesc10011600 => null;

		public static TLocalizedText TSkillName10011601 => null;

		public static TLocalizedText TSkillDesc10011601 => null;

		public static TLocalizedText TSkillName10011700 => null;

		public static TLocalizedText TSkillDesc10011700 => null;

		public static TLocalizedText TSkillName10011701 => null;

		public static TLocalizedText TSkillDesc10011701 => null;

		public static TLocalizedText TSkillName10011702 => null;

		public static TLocalizedText TSkillDesc10011702 => null;

		public static TLocalizedText TSkillName10011703 => null;

		public static TLocalizedText TSkillDesc10011703 => null;

		public static TLocalizedText TSkillName10011704 => null;

		public static TLocalizedText TSkillDesc10011704 => null;

		public static TLocalizedText TSkillName10011705 => null;

		public static TLocalizedText TSkillDesc10011705 => null;

		public static TLocalizedText TSkillName10011706 => null;

		public static TLocalizedText TSkillDesc10011706 => null;

		public static TLocalizedText TSkillName10011800 => null;

		public static TLocalizedText TSkillDesc10011800 => null;

		public static TLocalizedText TSkillName10012000 => null;

		public static TLocalizedText TSkillDesc10012000 => null;

		public static TLocalizedText TSkillName10012100 => null;

		public static TLocalizedText TSkillDesc10012100 => null;

		public static TLocalizedText TSkillName10012300 => null;

		public static TLocalizedText TSkillDesc10012300 => null;

		public static TLocalizedText TSkillName10012400 => null;

		public static TLocalizedText TSkillDesc10012400 => null;

		public static TLocalizedText TSkillName10012500 => null;

		public static TLocalizedText TSkillDesc10012500 => null;

		public static TLocalizedText TSkillName10012600 => null;

		public static TLocalizedText TSkillDesc10012600 => null;

		public static TLocalizedText TSkillName10012700 => null;

		public static TLocalizedText TSkillDesc10012700 => null;

		public static TLocalizedText TSkillName10012800 => null;

		public static TLocalizedText TSkillDesc10012800 => null;

		public static TLocalizedText TSkillName10012801 => null;

		public static TLocalizedText TSkillDesc10012801 => null;

		public static TLocalizedText TSkillName10013000 => null;

		public static TLocalizedText TSkillDesc10013000 => null;

		public static TLocalizedText TSkillName10013100 => null;

		public static TLocalizedText TSkillDesc10013100 => null;

		public static TLocalizedText TSkillName10013200 => null;

		public static TLocalizedText TSkillDesc10013200 => null;

		public static TLocalizedText TSkillName10013300 => null;

		public static TLocalizedText TSkillDesc10013300 => null;

		public static TLocalizedText TSkillName10013400 => null;

		public static TLocalizedText TSkillDesc10013400 => null;

		public static TLocalizedText TSkillName10013500 => null;

		public static TLocalizedText TSkillDesc10013500 => null;

		public static TLocalizedText TSkillName10013501 => null;

		public static TLocalizedText TSkillDesc10013501 => null;

		public static TLocalizedText TSkillName10013600 => null;

		public static TLocalizedText TSkillDesc10013600 => null;

		public static TLocalizedText TSkillName10013800 => null;

		public static TLocalizedText TSkillDesc10013800 => null;

		public static TLocalizedText TSkillName10013801 => null;

		public static TLocalizedText TSkillDesc10013801 => null;

		public static TLocalizedText TSkillName10013802 => null;

		public static TLocalizedText TSkillDesc10013802 => null;

		public static TLocalizedText TSkillName10013803 => null;

		public static TLocalizedText TSkillDesc10013803 => null;

		public static TLocalizedText TSkillName10013804 => null;

		public static TLocalizedText TSkillDesc10013804 => null;

		public static TLocalizedText TSkillName10013805 => null;

		public static TLocalizedText TSkillDesc10013805 => null;

		public static TLocalizedText TSkillName10013806 => null;

		public static TLocalizedText TSkillDesc10013806 => null;

		public static TLocalizedText TSkillName10013807 => null;

		public static TLocalizedText TSkillDesc10013807 => null;

		public static TLocalizedText TSkillName10013808 => null;

		public static TLocalizedText TSkillDesc10013808 => null;

		public static TLocalizedText TSkillName20000000 => null;

		public static TLocalizedText TSkillDesc20000000 => null;

		public static TLocalizedText TSkillName20000001 => null;

		public static TLocalizedText TSkillDesc20000001 => null;

		public static TLocalizedText TSkillName20000050 => null;

		public static TLocalizedText TSkillDesc20000050 => null;

		public static TLocalizedText TSkillName20000100 => null;

		public static TLocalizedText TSkillDesc20000100 => null;

		public static TLocalizedText TSkillName20000101 => null;

		public static TLocalizedText TSkillDesc20000101 => null;

		public static TLocalizedText TSkillName20000102 => null;

		public static TLocalizedText TSkillDesc20000102 => null;

		public static TLocalizedText TSkillName20000103 => null;

		public static TLocalizedText TSkillDesc20000103 => null;

		public static TLocalizedText TSkillName20000200 => null;

		public static TLocalizedText TSkillDesc20000200 => null;

		public static TLocalizedText TSkillName20000201 => null;

		public static TLocalizedText TSkillDesc20000201 => null;

		public static TLocalizedText TSkillName20000202 => null;

		public static TLocalizedText TSkillDesc20000202 => null;

		public static TLocalizedText TSkillName20000300 => null;

		public static TLocalizedText TSkillDesc20000300 => null;

		public static TLocalizedText TSkillName20000301 => null;

		public static TLocalizedText TSkillDesc20000301 => null;

		public static TLocalizedText TSkillName20000302 => null;

		public static TLocalizedText TSkillDesc20000302 => null;

		public static TLocalizedText TSkillName20000400 => null;

		public static TLocalizedText TSkillDesc20000400 => null;

		public static TLocalizedText TSkillName20000401 => null;

		public static TLocalizedText TSkillDesc20000401 => null;

		public static TLocalizedText TSkillName20000402 => null;

		public static TLocalizedText TSkillDesc20000402 => null;

		public static TLocalizedText TSkillName20000403 => null;

		public static TLocalizedText TSkillDesc20000403 => null;

		public static TLocalizedText TSkillName20000404 => null;

		public static TLocalizedText TSkillDesc20000404 => null;

		public static TLocalizedText TSkillName20000405 => null;

		public static TLocalizedText TSkillDesc20000405 => null;

		public static TLocalizedText TSkillName20000406 => null;

		public static TLocalizedText TSkillDesc20000406 => null;

		public static TLocalizedText TSkillName20000407 => null;

		public static TLocalizedText TSkillDesc20000407 => null;

		public static TLocalizedText TSkillName20000500 => null;

		public static TLocalizedText TSkillDesc20000500 => null;

		public static TLocalizedText TSkillName20000501 => null;

		public static TLocalizedText TSkillDesc20000501 => null;

		public static TLocalizedText TSkillName20000502 => null;

		public static TLocalizedText TSkillDesc20000502 => null;

		public static TLocalizedText TSkillName20000600 => null;

		public static TLocalizedText TSkillDesc20000600 => null;

		public static TLocalizedText TSkillName20000601 => null;

		public static TLocalizedText TSkillDesc20000601 => null;

		public static TLocalizedText TSkillName20001000 => null;

		public static TLocalizedText TSkillDesc20001000 => null;

		public static TLocalizedText TSkillName20001001 => null;

		public static TLocalizedText TSkillDesc20001001 => null;

		public static TLocalizedText TSkillName20001002 => null;

		public static TLocalizedText TSkillDesc20001002 => null;

		public static TLocalizedText TSkillName20001010 => null;

		public static TLocalizedText TSkillDesc20001010 => null;

		public static TLocalizedText TSkillName20001011 => null;

		public static TLocalizedText TSkillDesc20001011 => null;

		public static TLocalizedText TSkillName20001012 => null;

		public static TLocalizedText TSkillDesc20001012 => null;

		public static TLocalizedText TSkillName20001013 => null;

		public static TLocalizedText TSkillDesc20001013 => null;

		public static TLocalizedText TSkillName20001014 => null;

		public static TLocalizedText TSkillDesc20001014 => null;

		public static TLocalizedText TSkillName20001015 => null;

		public static TLocalizedText TSkillDesc20001015 => null;

		public static TLocalizedText TSkillName20001020 => null;

		public static TLocalizedText TSkillDesc20001020 => null;

		public static TLocalizedText TSkillName20001021 => null;

		public static TLocalizedText TSkillDesc20001021 => null;

		public static TLocalizedText TSkillName20001022 => null;

		public static TLocalizedText TSkillDesc20001022 => null;

		public static TLocalizedText TSkillName20001023 => null;

		public static TLocalizedText TSkillDesc20001023 => null;

		public static TLocalizedText TSkillName20001030 => null;

		public static TLocalizedText TSkillDesc20001030 => null;

		public static TLocalizedText TSkillName20001031 => null;

		public static TLocalizedText TSkillDesc20001031 => null;

		public static TLocalizedText TSkillName20001032 => null;

		public static TLocalizedText TSkillDesc20001032 => null;

		public static TLocalizedText TSkillName20001033 => null;

		public static TLocalizedText TSkillDesc20001033 => null;

		public static TLocalizedText TSkillName20001040 => null;

		public static TLocalizedText TSkillDesc20001040 => null;

		public static TLocalizedText TSkillName20001041 => null;

		public static TLocalizedText TSkillDesc20001041 => null;

		public static TLocalizedText TSkillName20001050 => null;

		public static TLocalizedText TSkillDesc20001050 => null;

		public static TLocalizedText TSkillName20001051 => null;

		public static TLocalizedText TSkillDesc20001051 => null;

		public static TLocalizedText TSkillName20001052 => null;

		public static TLocalizedText TSkillDesc20001052 => null;

		public static TLocalizedText TSkillName20001060 => null;

		public static TLocalizedText TSkillDesc20001060 => null;

		public static TLocalizedText TSkillName20001061 => null;

		public static TLocalizedText TSkillDesc20001061 => null;

		public static TLocalizedText TSkillName20001062 => null;

		public static TLocalizedText TSkillDesc20001062 => null;

		public static TLocalizedText TSkillName20001063 => null;

		public static TLocalizedText TSkillDesc20001063 => null;

		public static TLocalizedText TSkillName20001064 => null;

		public static TLocalizedText TSkillDesc20001064 => null;

		public static TLocalizedText TSkillName20001065 => null;

		public static TLocalizedText TSkillDesc20001065 => null;

		public static TLocalizedText TSkillName20001070 => null;

		public static TLocalizedText TSkillDesc20001070 => null;

		public static TLocalizedText TSkillName20001071 => null;

		public static TLocalizedText TSkillDesc20001071 => null;

		public static TLocalizedText TSkillName20001072 => null;

		public static TLocalizedText TSkillDesc20001072 => null;

		public static TLocalizedText TSkillName20001073 => null;

		public static TLocalizedText TSkillDesc20001073 => null;

		public static TLocalizedText TSkillName20001080 => null;

		public static TLocalizedText TSkillDesc20001080 => null;

		public static TLocalizedText TSkillName20001081 => null;

		public static TLocalizedText TSkillDesc20001081 => null;

		public static TLocalizedText TSkillName20001082 => null;

		public static TLocalizedText TSkillDesc20001082 => null;

		public static TLocalizedText TSkillName20002001 => null;

		public static TLocalizedText TSkillDesc20002001 => null;

		public static TLocalizedText TSkillName20002002 => null;

		public static TLocalizedText TSkillDesc20002002 => null;

		public static TLocalizedText TSkillName20002101 => null;

		public static TLocalizedText TSkillDesc20002101 => null;

		public static TLocalizedText TSkillName20002106 => null;

		public static TLocalizedText TSkillDesc20002106 => null;

		public static TLocalizedText TSkillName20002107 => null;

		public static TLocalizedText TSkillDesc20002107 => null;

		public static TLocalizedText TSkillName20060000 => null;

		public static TLocalizedText TSkillDesc20060000 => null;

		public static TLocalizedText TSkillName20060001 => null;

		public static TLocalizedText TSkillDesc20060001 => null;

		public static TLocalizedText TSkillName20060002 => null;

		public static TLocalizedText TSkillDesc20060002 => null;

		public static TLocalizedText TSkillName20060003 => null;

		public static TLocalizedText TSkillDesc20060003 => null;

		public static TLocalizedText TSkillName20070000 => null;

		public static TLocalizedText TSkillDesc20070000 => null;

		public static TLocalizedText TSkillName21000000 => null;

		public static TLocalizedText TSkillDesc21000000 => null;

		public static TLocalizedText TSkillName21000001 => null;

		public static TLocalizedText TSkillDesc21000001 => null;

		public static TLocalizedText TSkillName21000002 => null;

		public static TLocalizedText TSkillDesc21000002 => null;

		public static TLocalizedText TSkillName21000003 => null;

		public static TLocalizedText TSkillDesc21000003 => null;

		public static TLocalizedText TSkillName21000004 => null;

		public static TLocalizedText TSkillDesc21000004 => null;

		public static TLocalizedText TSkillName21000050 => null;

		public static TLocalizedText TSkillDesc21000050 => null;

		public static TLocalizedText TSkillName21000051 => null;

		public static TLocalizedText TSkillDesc21000051 => null;

		public static TLocalizedText TSkillName21000052 => null;

		public static TLocalizedText TSkillDesc21000052 => null;

		public static TLocalizedText TSkillName21000053 => null;

		public static TLocalizedText TSkillDesc21000053 => null;

		public static TLocalizedText TSkillName21000054 => null;

		public static TLocalizedText TSkillDesc21000054 => null;

		public static TLocalizedText TSkillName21000055 => null;

		public static TLocalizedText TSkillDesc21000055 => null;

		public static TLocalizedText TSkillName21000056 => null;

		public static TLocalizedText TSkillDesc21000056 => null;

		public static TLocalizedText TSkillName21000057 => null;

		public static TLocalizedText TSkillDesc21000057 => null;

		public static TLocalizedText TSkillName21000058 => null;

		public static TLocalizedText TSkillDesc21000058 => null;

		public static TLocalizedText TSkillName21000059 => null;

		public static TLocalizedText TSkillDesc21000059 => null;

		public static TLocalizedText TSkillName21000060 => null;

		public static TLocalizedText TSkillDesc21000060 => null;

		public static TLocalizedText TSkillName21000061 => null;

		public static TLocalizedText TSkillDesc21000061 => null;

		public static TLocalizedText TSkillName21000062 => null;

		public static TLocalizedText TSkillDesc21000062 => null;

		public static TLocalizedText TSkillName21000063 => null;

		public static TLocalizedText TSkillDesc21000063 => null;

		public static TLocalizedText TSkillName21000064 => null;

		public static TLocalizedText TSkillDesc21000064 => null;

		public static TLocalizedText TSkillName21000065 => null;

		public static TLocalizedText TSkillDesc21000065 => null;

		public static TLocalizedText TSkillName21000070 => null;

		public static TLocalizedText TSkillDesc21000070 => null;

		public static TLocalizedText TSkillName21000071 => null;

		public static TLocalizedText TSkillDesc21000071 => null;

		public static TLocalizedText TSkillName21000072 => null;

		public static TLocalizedText TSkillDesc21000072 => null;

		public static TLocalizedText TSkillName21000073 => null;

		public static TLocalizedText TSkillDesc21000073 => null;

		public static TLocalizedText TSkillName21000074 => null;

		public static TLocalizedText TSkillDesc21000074 => null;

		public static TLocalizedText TSkillName21000075 => null;

		public static TLocalizedText TSkillDesc21000075 => null;

		public static TLocalizedText TSkillName21000076 => null;

		public static TLocalizedText TSkillDesc21000076 => null;

		public static TLocalizedText TSkillName21000077 => null;

		public static TLocalizedText TSkillDesc21000077 => null;

		public static TLocalizedText TSkillName21000078 => null;

		public static TLocalizedText TSkillDesc21000078 => null;

		public static TLocalizedText TSkillName21000079 => null;

		public static TLocalizedText TSkillDesc21000079 => null;

		public static TLocalizedText TSkillName21000080 => null;

		public static TLocalizedText TSkillDesc21000080 => null;

		public static TLocalizedText TSkillName21000100 => null;

		public static TLocalizedText TSkillDesc21000100 => null;

		public static TLocalizedText TSkillName21000101 => null;

		public static TLocalizedText TSkillDesc21000101 => null;

		public static TLocalizedText TSkillName21000102 => null;

		public static TLocalizedText TSkillDesc21000102 => null;

		public static TLocalizedText TSkillName21000103 => null;

		public static TLocalizedText TSkillDesc21000103 => null;

		public static TLocalizedText TSkillName21000104 => null;

		public static TLocalizedText TSkillDesc21000104 => null;

		public static TLocalizedText TSkillName21000105 => null;

		public static TLocalizedText TSkillDesc21000105 => null;

		public static TLocalizedText TSkillName21000106 => null;

		public static TLocalizedText TSkillDesc21000106 => null;

		public static TLocalizedText TSkillName21000150 => null;

		public static TLocalizedText TSkillDesc21000150 => null;

		public static TLocalizedText TSkillName21000151 => null;

		public static TLocalizedText TSkillDesc21000151 => null;

		public static TLocalizedText TSkillName21000152 => null;

		public static TLocalizedText TSkillDesc21000152 => null;

		public static TLocalizedText TSkillName21000153 => null;

		public static TLocalizedText TSkillDesc21000153 => null;

		public static TLocalizedText TSkillName21000154 => null;

		public static TLocalizedText TSkillDesc21000154 => null;

		public static TLocalizedText TSkillName21000155 => null;

		public static TLocalizedText TSkillDesc21000155 => null;

		public static TLocalizedText TSkillName21000200 => null;

		public static TLocalizedText TSkillDesc21000200 => null;

		public static TLocalizedText TSkillName21000201 => null;

		public static TLocalizedText TSkillDesc21000201 => null;

		public static TLocalizedText TSkillName21000250 => null;

		public static TLocalizedText TSkillDesc21000250 => null;

		public static TLocalizedText TSkillName21000251 => null;

		public static TLocalizedText TSkillDesc21000251 => null;

		public static TLocalizedText TSkillName21000252 => null;

		public static TLocalizedText TSkillDesc21000252 => null;

		public static TLocalizedText TSkillName21000253 => null;

		public static TLocalizedText TSkillDesc21000253 => null;

		public static TLocalizedText TSkillName21000254 => null;

		public static TLocalizedText TSkillDesc21000254 => null;

		public static TLocalizedText TSkillName21000300 => null;

		public static TLocalizedText TSkillDesc21000300 => null;

		public static TLocalizedText TSkillName21000301 => null;

		public static TLocalizedText TSkillDesc21000301 => null;

		public static TLocalizedText TSkillName21000302 => null;

		public static TLocalizedText TSkillDesc21000302 => null;

		public static TLocalizedText TSkillName21000303 => null;

		public static TLocalizedText TSkillDesc21000303 => null;

		public static TLocalizedText TSkillName21000304 => null;

		public static TLocalizedText TSkillDesc21000304 => null;

		public static TLocalizedText TSkillName21000350 => null;

		public static TLocalizedText TSkillDesc21000350 => null;

		public static TLocalizedText TSkillName21000351 => null;

		public static TLocalizedText TSkillDesc21000351 => null;

		public static TLocalizedText TSkillName21000352 => null;

		public static TLocalizedText TSkillDesc21000352 => null;

		public static TLocalizedText TSkillName21000353 => null;

		public static TLocalizedText TSkillDesc21000353 => null;

		public static TLocalizedText TSkillName21000354 => null;

		public static TLocalizedText TSkillDesc21000354 => null;

		public static TLocalizedText TSkillName21000355 => null;

		public static TLocalizedText TSkillDesc21000355 => null;

		public static TLocalizedText TSkillName21000400 => null;

		public static TLocalizedText TSkillDesc21000400 => null;

		public static TLocalizedText TSkillName21000401 => null;

		public static TLocalizedText TSkillDesc21000401 => null;

		public static TLocalizedText TSkillName21000402 => null;

		public static TLocalizedText TSkillDesc21000402 => null;

		public static TLocalizedText TSkillName21000403 => null;

		public static TLocalizedText TSkillDesc21000403 => null;

		public static TLocalizedText TSkillName21000404 => null;

		public static TLocalizedText TSkillDesc21000404 => null;

		public static TLocalizedText TSkillName21000450 => null;

		public static TLocalizedText TSkillDesc21000450 => null;

		public static TLocalizedText TSkillName21000451 => null;

		public static TLocalizedText TSkillDesc21000451 => null;

		public static TLocalizedText TSkillName21000452 => null;

		public static TLocalizedText TSkillDesc21000452 => null;

		public static TLocalizedText TSkillName21000453 => null;

		public static TLocalizedText TSkillDesc21000453 => null;

		public static TLocalizedText TSkillName21000500 => null;

		public static TLocalizedText TSkillDesc21000500 => null;

		public static TLocalizedText TSkillName21000501 => null;

		public static TLocalizedText TSkillDesc21000501 => null;

		public static TLocalizedText TSkillName21000502 => null;

		public static TLocalizedText TSkillDesc21000502 => null;

		public static TLocalizedText TSkillName21000503 => null;

		public static TLocalizedText TSkillDesc21000503 => null;

		public static TLocalizedText TSkillName21000504 => null;

		public static TLocalizedText TSkillDesc21000504 => null;

		public static TLocalizedText TSkillName21000505 => null;

		public static TLocalizedText TSkillDesc21000505 => null;

		public static TLocalizedText TSkillName21000510 => null;

		public static TLocalizedText TSkillDesc21000510 => null;

		public static TLocalizedText TSkillName21000511 => null;

		public static TLocalizedText TSkillDesc21000511 => null;

		public static TLocalizedText TSkillName21000512 => null;

		public static TLocalizedText TSkillDesc21000512 => null;

		public static TLocalizedText TSkillName21000513 => null;

		public static TLocalizedText TSkillDesc21000513 => null;

		public static TLocalizedText TSkillName21000514 => null;

		public static TLocalizedText TSkillDesc21000514 => null;

		public static TLocalizedText TSkillName21000515 => null;

		public static TLocalizedText TSkillDesc21000515 => null;

		public static TLocalizedText TSkillName21000520 => null;

		public static TLocalizedText TSkillDesc21000520 => null;

		public static TLocalizedText TSkillName21000521 => null;

		public static TLocalizedText TSkillDesc21000521 => null;

		public static TLocalizedText TSkillName21000522 => null;

		public static TLocalizedText TSkillDesc21000522 => null;

		public static TLocalizedText TSkillName21000523 => null;

		public static TLocalizedText TSkillDesc21000523 => null;

		public static TLocalizedText TSkillName21000524 => null;

		public static TLocalizedText TSkillDesc21000524 => null;

		public static TLocalizedText TSkillName21000525 => null;

		public static TLocalizedText TSkillDesc21000525 => null;

		public static TLocalizedText TSkillName21000526 => null;

		public static TLocalizedText TSkillDesc21000526 => null;

		public static TLocalizedText TSkillName21000530 => null;

		public static TLocalizedText TSkillDesc21000530 => null;

		public static TLocalizedText TSkillName21000531 => null;

		public static TLocalizedText TSkillDesc21000531 => null;

		public static TLocalizedText TSkillName21000532 => null;

		public static TLocalizedText TSkillDesc21000532 => null;

		public static TLocalizedText TSkillName21000533 => null;

		public static TLocalizedText TSkillDesc21000533 => null;

		public static TLocalizedText TSkillName21000534 => null;

		public static TLocalizedText TSkillDesc21000534 => null;

		public static TLocalizedText TSkillName21000535 => null;

		public static TLocalizedText TSkillDesc21000535 => null;

		public static TLocalizedText TSkillName21000536 => null;

		public static TLocalizedText TSkillDesc21000536 => null;

		public static TLocalizedText TSkillName21000537 => null;

		public static TLocalizedText TSkillDesc21000537 => null;

		public static TLocalizedText TSkillName21000538 => null;

		public static TLocalizedText TSkillDesc21000538 => null;

		public static TLocalizedText TSkillName21000540 => null;

		public static TLocalizedText TSkillDesc21000540 => null;

		public static TLocalizedText TSkillName21000541 => null;

		public static TLocalizedText TSkillDesc21000541 => null;

		public static TLocalizedText TSkillName21000542 => null;

		public static TLocalizedText TSkillDesc21000542 => null;

		public static TLocalizedText TSkillName21000543 => null;

		public static TLocalizedText TSkillDesc21000543 => null;

		public static TLocalizedText TSkillName21000550 => null;

		public static TLocalizedText TSkillDesc21000550 => null;

		public static TLocalizedText TSkillName21000551 => null;

		public static TLocalizedText TSkillDesc21000551 => null;

		public static TLocalizedText TSkillName21000552 => null;

		public static TLocalizedText TSkillDesc21000552 => null;

		public static TLocalizedText TSkillName21000553 => null;

		public static TLocalizedText TSkillDesc21000553 => null;

		public static TLocalizedText TSkillName21000554 => null;

		public static TLocalizedText TSkillDesc21000554 => null;

		public static TLocalizedText TSkillName21000555 => null;

		public static TLocalizedText TSkillDesc21000555 => null;

		public static TLocalizedText TSkillName21000560 => null;

		public static TLocalizedText TSkillDesc21000560 => null;

		public static TLocalizedText TSkillName21000561 => null;

		public static TLocalizedText TSkillDesc21000561 => null;

		public static TLocalizedText TSkillName21000562 => null;

		public static TLocalizedText TSkillDesc21000562 => null;

		public static TLocalizedText TSkillName21000563 => null;

		public static TLocalizedText TSkillDesc21000563 => null;

		public static TLocalizedText TSkillName21000564 => null;

		public static TLocalizedText TSkillDesc21000564 => null;

		public static TLocalizedText TSkillName21000565 => null;

		public static TLocalizedText TSkillDesc21000565 => null;

		public static TLocalizedText TSkillName21000570 => null;

		public static TLocalizedText TSkillDesc21000570 => null;

		public static TLocalizedText TSkillName21000571 => null;

		public static TLocalizedText TSkillDesc21000571 => null;

		public static TLocalizedText TSkillName21000572 => null;

		public static TLocalizedText TSkillDesc21000572 => null;

		public static TLocalizedText TSkillName21000573 => null;

		public static TLocalizedText TSkillDesc21000573 => null;

		public static TLocalizedText TSkillName21000574 => null;

		public static TLocalizedText TSkillDesc21000574 => null;

		public static TLocalizedText TSkillName21000575 => null;

		public static TLocalizedText TSkillDesc21000575 => null;

		public static TLocalizedText TSkillName21000576 => null;

		public static TLocalizedText TSkillDesc21000576 => null;

		public static TLocalizedText TSkillName21000577 => null;

		public static TLocalizedText TSkillDesc21000577 => null;

		public static TLocalizedText TSkillName21000578 => null;

		public static TLocalizedText TSkillDesc21000578 => null;

		public static TLocalizedText TSkillName21000580 => null;

		public static TLocalizedText TSkillDesc21000580 => null;

		public static TLocalizedText TSkillName21000581 => null;

		public static TLocalizedText TSkillDesc21000581 => null;

		public static TLocalizedText TSkillName21000582 => null;

		public static TLocalizedText TSkillDesc21000582 => null;

		public static TLocalizedText TSkillName21000583 => null;

		public static TLocalizedText TSkillDesc21000583 => null;

		public static TLocalizedText TSkillName21000584 => null;

		public static TLocalizedText TSkillDesc21000584 => null;

		public static TLocalizedText TSkillName21000585 => null;

		public static TLocalizedText TSkillDesc21000585 => null;

		public static TLocalizedText TSkillName21000590 => null;

		public static TLocalizedText TSkillDesc21000590 => null;

		public static TLocalizedText TSkillName21000591 => null;

		public static TLocalizedText TSkillDesc21000591 => null;

		public static TLocalizedText TSkillName21000592 => null;

		public static TLocalizedText TSkillDesc21000592 => null;

		public static TLocalizedText TSkillName21000593 => null;

		public static TLocalizedText TSkillDesc21000593 => null;

		public static TLocalizedText TSkillName21000594 => null;

		public static TLocalizedText TSkillDesc21000594 => null;

		public static TLocalizedText TSkillName21000600 => null;

		public static TLocalizedText TSkillDesc21000600 => null;

		public static TLocalizedText TSkillName21000601 => null;

		public static TLocalizedText TSkillDesc21000601 => null;

		public static TLocalizedText TSkillName21000602 => null;

		public static TLocalizedText TSkillDesc21000602 => null;

		public static TLocalizedText TSkillName21000603 => null;

		public static TLocalizedText TSkillDesc21000603 => null;

		public static TLocalizedText TSkillName21000604 => null;

		public static TLocalizedText TSkillDesc21000604 => null;

		public static TLocalizedText TSkillName21000605 => null;

		public static TLocalizedText TSkillDesc21000605 => null;

		public static TLocalizedText TSkillName21000610 => null;

		public static TLocalizedText TSkillDesc21000610 => null;

		public static TLocalizedText TSkillName21000611 => null;

		public static TLocalizedText TSkillDesc21000611 => null;

		public static TLocalizedText TSkillName21000613 => null;

		public static TLocalizedText TSkillDesc21000613 => null;

		public static TLocalizedText TSkillName21000614 => null;

		public static TLocalizedText TSkillDesc21000614 => null;

		public static TLocalizedText TSkillName21000615 => null;

		public static TLocalizedText TSkillDesc21000615 => null;

		public static TLocalizedText TSkillName21000620 => null;

		public static TLocalizedText TSkillDesc21000620 => null;

		public static TLocalizedText TSkillName21000621 => null;

		public static TLocalizedText TSkillDesc21000621 => null;

		public static TLocalizedText TSkillName21000622 => null;

		public static TLocalizedText TSkillDesc21000622 => null;

		public static TLocalizedText TSkillName21000623 => null;

		public static TLocalizedText TSkillDesc21000623 => null;

		public static TLocalizedText TSkillName21000624 => null;

		public static TLocalizedText TSkillDesc21000624 => null;

		public static TLocalizedText TSkillName21001000 => null;

		public static TLocalizedText TSkillDesc21001000 => null;

		public static TLocalizedText TSkillName21001001 => null;

		public static TLocalizedText TSkillDesc21001001 => null;

		public static TLocalizedText TSkillName21001002 => null;

		public static TLocalizedText TSkillDesc21001002 => null;

		public static TLocalizedText TSkillName21001003 => null;

		public static TLocalizedText TSkillDesc21001003 => null;

		public static TLocalizedText TSkillName21002000 => null;

		public static TLocalizedText TSkillDesc21002000 => null;

		public static TLocalizedText TSkillName21002001 => null;

		public static TLocalizedText TSkillDesc21002001 => null;

		public static TLocalizedText TSkillName21002002 => null;

		public static TLocalizedText TSkillDesc21002002 => null;

		public static TLocalizedText TSkillName21002003 => null;

		public static TLocalizedText TSkillDesc21002003 => null;

		public static TLocalizedText TSkillName21002004 => null;

		public static TLocalizedText TSkillDesc21002004 => null;

		public static TLocalizedText TSkillName21002005 => null;

		public static TLocalizedText TSkillDesc21002005 => null;

		public static TLocalizedText TSkillName21002006 => null;

		public static TLocalizedText TSkillDesc21002006 => null;

		public static TLocalizedText TSkillName21002007 => null;

		public static TLocalizedText TSkillDesc21002007 => null;

		public static TLocalizedText TSkillName21002008 => null;

		public static TLocalizedText TSkillDesc21002008 => null;

		public static TLocalizedText TSkillName21002009 => null;

		public static TLocalizedText TSkillDesc21002009 => null;

		public static TLocalizedText TSkillName21002010 => null;

		public static TLocalizedText TSkillDesc21002010 => null;

		public static TLocalizedText TSkillName21002011 => null;

		public static TLocalizedText TSkillDesc21002011 => null;

		public static TLocalizedText TSkillName21002012 => null;

		public static TLocalizedText TSkillDesc21002012 => null;

		public static TLocalizedText TSkillName21002013 => null;

		public static TLocalizedText TSkillDesc21002013 => null;

		public static TLocalizedText TSkillName21003001 => null;

		public static TLocalizedText TSkillDesc21003001 => null;

		public static TLocalizedText TSkillName21003002 => null;

		public static TLocalizedText TSkillDesc21003002 => null;

		public static TLocalizedText TSkillName21003003 => null;

		public static TLocalizedText TSkillDesc21003003 => null;

		public static TLocalizedText TSkillName21003004 => null;

		public static TLocalizedText TSkillDesc21003004 => null;

		public static TLocalizedText TSkillName21003005 => null;

		public static TLocalizedText TSkillDesc21003005 => null;

		public static TLocalizedText TSkillName21003006 => null;

		public static TLocalizedText TSkillDesc21003006 => null;

		public static TLocalizedText TSkillName21003007 => null;

		public static TLocalizedText TSkillDesc21003007 => null;

		public static TLocalizedText TSkillName21003008 => null;

		public static TLocalizedText TSkillDesc21003008 => null;

		public static TLocalizedText TSkillName21003009 => null;

		public static TLocalizedText TSkillDesc21003009 => null;

		public static TLocalizedText TSkillName21003010 => null;

		public static TLocalizedText TSkillDesc21003010 => null;

		public static TLocalizedText TSkillName21003011 => null;

		public static TLocalizedText TSkillDesc21003011 => null;

		public static TLocalizedText TSkillName21003012 => null;

		public static TLocalizedText TSkillDesc21003012 => null;

		public static TLocalizedText TSkillName21003013 => null;

		public static TLocalizedText TSkillDesc21003013 => null;

		public static TLocalizedText TSkillName21003014 => null;

		public static TLocalizedText TSkillDesc21003014 => null;

		public static TLocalizedText TSkillName21003015 => null;

		public static TLocalizedText TSkillDesc21003015 => null;

		public static TLocalizedText TSkillName21003016 => null;

		public static TLocalizedText TSkillDesc21003016 => null;

		public static TLocalizedText TSkillName21003017 => null;

		public static TLocalizedText TSkillDesc21003017 => null;

		public static TLocalizedText TSkillName21003018 => null;

		public static TLocalizedText TSkillDesc21003018 => null;

		public static TLocalizedText TSkillName21003019 => null;

		public static TLocalizedText TSkillDesc21003019 => null;

		public static TLocalizedText TSkillName21003020 => null;

		public static TLocalizedText TSkillDesc21003020 => null;

		public static TLocalizedText TSkillName21003021 => null;

		public static TLocalizedText TSkillDesc21003021 => null;

		public static TLocalizedText TSkillName21003022 => null;

		public static TLocalizedText TSkillDesc21003022 => null;

		public static TLocalizedText TSkillName21003050 => null;

		public static TLocalizedText TSkillDesc21003050 => null;

		public static TLocalizedText TSkillName21003051 => null;

		public static TLocalizedText TSkillDesc21003051 => null;

		public static TLocalizedText TSkillName21003052 => null;

		public static TLocalizedText TSkillDesc21003052 => null;

		public static TLocalizedText TSkillName21003053 => null;

		public static TLocalizedText TSkillDesc21003053 => null;

		public static TLocalizedText TSkillName21003054 => null;

		public static TLocalizedText TSkillDesc21003054 => null;

		public static TLocalizedText TSkillName21003055 => null;

		public static TLocalizedText TSkillDesc21003055 => null;

		public static TLocalizedText TSkillName21003056 => null;

		public static TLocalizedText TSkillDesc21003056 => null;

		public static TLocalizedText TSkillName21003057 => null;

		public static TLocalizedText TSkillDesc21003057 => null;

		public static TLocalizedText TSkillName21003058 => null;

		public static TLocalizedText TSkillDesc21003058 => null;

		public static TLocalizedText TSkillName21003059 => null;

		public static TLocalizedText TSkillDesc21003059 => null;

		public static TLocalizedText TSkillName21003060 => null;

		public static TLocalizedText TSkillDesc21003060 => null;

		public static TLocalizedText TSkillName21003061 => null;

		public static TLocalizedText TSkillDesc21003061 => null;

		public static TLocalizedText TSkillName21003063 => null;

		public static TLocalizedText TSkillDesc21003063 => null;

		public static TLocalizedText TSkillName21003064 => null;

		public static TLocalizedText TSkillDesc21003064 => null;

		public static TLocalizedText TSkillName21004001 => null;

		public static TLocalizedText TSkillDesc21004001 => null;

		public static TLocalizedText TSkillName21004002 => null;

		public static TLocalizedText TSkillDesc21004002 => null;

		public static TLocalizedText TSkillName21004003 => null;

		public static TLocalizedText TSkillDesc21004003 => null;

		public static TLocalizedText TSkillName21004004 => null;

		public static TLocalizedText TSkillDesc21004004 => null;

		public static TLocalizedText TSkillName21004005 => null;

		public static TLocalizedText TSkillDesc21004005 => null;

		public static TLocalizedText TSkillName21004006 => null;

		public static TLocalizedText TSkillDesc21004006 => null;

		public static TLocalizedText TSkillName21004007 => null;

		public static TLocalizedText TSkillDesc21004007 => null;

		public static TLocalizedText TSkillName21004008 => null;

		public static TLocalizedText TSkillDesc21004008 => null;

		public static TLocalizedText TSkillName21004009 => null;

		public static TLocalizedText TSkillDesc21004009 => null;

		public static TLocalizedText TSkillName21004011 => null;

		public static TLocalizedText TSkillDesc21004011 => null;

		public static TLocalizedText TSkillName21004012 => null;

		public static TLocalizedText TSkillDesc21004012 => null;

		public static TLocalizedText TSkillName21004013 => null;

		public static TLocalizedText TSkillDesc21004013 => null;

		public static TLocalizedText TSkillName21004014 => null;

		public static TLocalizedText TSkillDesc21004014 => null;

		public static TLocalizedText TSkillName21004015 => null;

		public static TLocalizedText TSkillDesc21004015 => null;

		public static TLocalizedText TSkillName21004016 => null;

		public static TLocalizedText TSkillDesc21004016 => null;

		public static TLocalizedText TSkillName21004017 => null;

		public static TLocalizedText TSkillDesc21004017 => null;

		public static TLocalizedText TSkillName21004018 => null;

		public static TLocalizedText TSkillDesc21004018 => null;

		public static TLocalizedText TSkillName21004019 => null;

		public static TLocalizedText TSkillDesc21004019 => null;

		public static TLocalizedText TSkillName21004020 => null;

		public static TLocalizedText TSkillDesc21004020 => null;

		public static TLocalizedText TSkillName21004021 => null;

		public static TLocalizedText TSkillDesc21004021 => null;

		public static TLocalizedText TSkillName21004022 => null;

		public static TLocalizedText TSkillDesc21004022 => null;

		public static TLocalizedText TSkillName21004023 => null;

		public static TLocalizedText TSkillDesc21004023 => null;

		public static TLocalizedText TSkillName21005001 => null;

		public static TLocalizedText TSkillDesc21005001 => null;

		public static TLocalizedText TSkillName21005002 => null;

		public static TLocalizedText TSkillDesc21005002 => null;

		public static TLocalizedText TSkillName21005004 => null;

		public static TLocalizedText TSkillDesc21005004 => null;

		public static TLocalizedText TSkillName21005005 => null;

		public static TLocalizedText TSkillDesc21005005 => null;

		public static TLocalizedText TSkillName21005006 => null;

		public static TLocalizedText TSkillDesc21005006 => null;

		public static TLocalizedText TSkillName21005007 => null;

		public static TLocalizedText TSkillDesc21005007 => null;

		public static TLocalizedText TSkillName21005008 => null;

		public static TLocalizedText TSkillDesc21005008 => null;

		public static TLocalizedText TSkillName21005011 => null;

		public static TLocalizedText TSkillDesc21005011 => null;

		public static TLocalizedText TSkillName21005012 => null;

		public static TLocalizedText TSkillDesc21005012 => null;

		public static TLocalizedText TSkillName21005013 => null;

		public static TLocalizedText TSkillDesc21005013 => null;

		public static TLocalizedText TSkillName21005014 => null;

		public static TLocalizedText TSkillDesc21005014 => null;

		public static TLocalizedText TSkillName21005015 => null;

		public static TLocalizedText TSkillDesc21005015 => null;

		public static TLocalizedText TSkillName21005016 => null;

		public static TLocalizedText TSkillDesc21005016 => null;

		public static TLocalizedText TSkillName21005017 => null;

		public static TLocalizedText TSkillDesc21005017 => null;

		public static TLocalizedText TSkillName21005020 => null;

		public static TLocalizedText TSkillDesc21005020 => null;

		public static TLocalizedText TSkillName21005021 => null;

		public static TLocalizedText TSkillDesc21005021 => null;

		public static TLocalizedText TSkillName21005022 => null;

		public static TLocalizedText TSkillDesc21005022 => null;

		public static TLocalizedText TSkillName21005024 => null;

		public static TLocalizedText TSkillDesc21005024 => null;

		public static TLocalizedText TSkillName21005025 => null;

		public static TLocalizedText TSkillDesc21005025 => null;

		public static TLocalizedText TSkillName21005026 => null;

		public static TLocalizedText TSkillDesc21005026 => null;

		public static TLocalizedText TSkillName21005027 => null;

		public static TLocalizedText TSkillDesc21005027 => null;

		public static TLocalizedText TSkillName21005028 => null;

		public static TLocalizedText TSkillDesc21005028 => null;

		public static TLocalizedText TSkillName21005029 => null;

		public static TLocalizedText TSkillDesc21005029 => null;

		public static TLocalizedText TSkillName21005030 => null;

		public static TLocalizedText TSkillDesc21005030 => null;

		public static TLocalizedText TSkillName21005031 => null;

		public static TLocalizedText TSkillDesc21005031 => null;

		public static TLocalizedText TSkillName21005032 => null;

		public static TLocalizedText TSkillDesc21005032 => null;

		public static TLocalizedText TSkillName21005033 => null;

		public static TLocalizedText TSkillDesc21005033 => null;

		public static TLocalizedText TSkillName21005034 => null;

		public static TLocalizedText TSkillDesc21005034 => null;

		public static TLocalizedText TSkillName21005035 => null;

		public static TLocalizedText TSkillDesc21005035 => null;

		public static TLocalizedText TSkillName21005036 => null;

		public static TLocalizedText TSkillDesc21005036 => null;

		public static TLocalizedText TSkillName21100001 => null;

		public static TLocalizedText TSkillDesc21100001 => null;

		public static TLocalizedText TSkillName21100002 => null;

		public static TLocalizedText TSkillDesc21100002 => null;

		public static TLocalizedText TSkillName21100003 => null;

		public static TLocalizedText TSkillDesc21100003 => null;

		public static TLocalizedText TSkillName21100004 => null;

		public static TLocalizedText TSkillDesc21100004 => null;

		public static TLocalizedText TSkillName22000001 => null;

		public static TLocalizedText TSkillDesc22000001 => null;

		public static TLocalizedText TSkillName22000002 => null;

		public static TLocalizedText TSkillDesc22000002 => null;

		public static TLocalizedText TSkillName22000003 => null;

		public static TLocalizedText TSkillDesc22000003 => null;

		public static TLocalizedText TSkillName22000004 => null;

		public static TLocalizedText TSkillDesc22000004 => null;

		public static TLocalizedText TSkillName22000005 => null;

		public static TLocalizedText TSkillDesc22000005 => null;

		public static TLocalizedText TSkillName22000006 => null;

		public static TLocalizedText TSkillDesc22000006 => null;

		public static TLocalizedText TSkillName22000007 => null;

		public static TLocalizedText TSkillDesc22000007 => null;

		public static TLocalizedText TSkillName22000008 => null;

		public static TLocalizedText TSkillDesc22000008 => null;

		public static TLocalizedText TSkillName22000009 => null;

		public static TLocalizedText TSkillDesc22000009 => null;

		public static TLocalizedText TSkillName22000010 => null;

		public static TLocalizedText TSkillDesc22000010 => null;

		public static TLocalizedText TSkillName22000011 => null;

		public static TLocalizedText TSkillDesc22000011 => null;

		public static TLocalizedText TSkillName22000012 => null;

		public static TLocalizedText TSkillDesc22000012 => null;

		public static TLocalizedText TSkillName22000013 => null;

		public static TLocalizedText TSkillDesc22000013 => null;

		public static TLocalizedText TSkillName22000020 => null;

		public static TLocalizedText TSkillDesc22000020 => null;

		public static TLocalizedText TSkillName22000021 => null;

		public static TLocalizedText TSkillDesc22000021 => null;

		public static TLocalizedText TSkillName22000022 => null;

		public static TLocalizedText TSkillDesc22000022 => null;

		public static TLocalizedText TSkillName22000030 => null;

		public static TLocalizedText TSkillDesc22000030 => null;

		public static TLocalizedText TSkillName22000031 => null;

		public static TLocalizedText TSkillDesc22000031 => null;

		public static TLocalizedText TSkillName22000032 => null;

		public static TLocalizedText TSkillDesc22000032 => null;

		public static TLocalizedText TSkillName22000033 => null;

		public static TLocalizedText TSkillDesc22000033 => null;

		public static TLocalizedText TSkillName22000034 => null;

		public static TLocalizedText TSkillDesc22000034 => null;

		public static TLocalizedText TSkillName22000035 => null;

		public static TLocalizedText TSkillDesc22000035 => null;

		public static TLocalizedText TSkillName22000036 => null;

		public static TLocalizedText TSkillDesc22000036 => null;

		public static TLocalizedText TSkillName22000037 => null;

		public static TLocalizedText TSkillDesc22000037 => null;

		public static TLocalizedText TSkillName22000038 => null;

		public static TLocalizedText TSkillDesc22000038 => null;

		public static TLocalizedText TSkillName22000039 => null;

		public static TLocalizedText TSkillDesc22000039 => null;

		public static TLocalizedText TSkillName22000040 => null;

		public static TLocalizedText TSkillDesc22000040 => null;

		public static TLocalizedText TSkillName22000041 => null;

		public static TLocalizedText TSkillDesc22000041 => null;

		public static TLocalizedText TSkillName22000042 => null;

		public static TLocalizedText TSkillDesc22000042 => null;

		public static TLocalizedText TSkillName22000043 => null;

		public static TLocalizedText TSkillDesc22000043 => null;

		public static TLocalizedText TSkillName22000044 => null;

		public static TLocalizedText TSkillDesc22000044 => null;

		public static TLocalizedText TSkillName23000001 => null;

		public static TLocalizedText TSkillDesc23000001 => null;

		public static TLocalizedText TSkillName29000001 => null;

		public static TLocalizedText TSkillDesc29000001 => null;

		public static TLocalizedText TSkillName29000002 => null;

		public static TLocalizedText TSkillDesc29000002 => null;

		public static TLocalizedText TSkillName29000003 => null;

		public static TLocalizedText TSkillDesc29000003 => null;

		public static TLocalizedText TSkillName29000004 => null;

		public static TLocalizedText TSkillDesc29000004 => null;

		public static TLocalizedText TSkillName29000005 => null;

		public static TLocalizedText TSkillDesc29000005 => null;

		public static TLocalizedText TSkillName29000006 => null;

		public static TLocalizedText TSkillDesc29000006 => null;

		public static TLocalizedText TSkillName29000007 => null;

		public static TLocalizedText TSkillDesc29000007 => null;

		public static TLocalizedText TSkillName29000008 => null;

		public static TLocalizedText TSkillDesc29000008 => null;

		public static TLocalizedText TSkillName29000009 => null;

		public static TLocalizedText TSkillDesc29000009 => null;

		public static TLocalizedText TSkillName29000010 => null;

		public static TLocalizedText TSkillDesc29000010 => null;

		public static TLocalizedText TSkillName30010000 => null;

		public static TLocalizedText TSkillDesc30010000 => null;

		public static TLocalizedText TSkillName30010001 => null;

		public static TLocalizedText TSkillDesc30010001 => null;

		public static TLocalizedText TSkillName30010002 => null;

		public static TLocalizedText TSkillDesc30010002 => null;

		public static TLocalizedText TSkillName30010005 => null;

		public static TLocalizedText TSkillDesc30010005 => null;

		public static TLocalizedText TSkillName30010006 => null;

		public static TLocalizedText TSkillDesc30010006 => null;

		public static TLocalizedText TSkillName30010007 => null;

		public static TLocalizedText TSkillDesc30010007 => null;

		public static TLocalizedText TSkillName30010010 => null;

		public static TLocalizedText TSkillDesc30010010 => null;

		public static TLocalizedText TSkillName30010011 => null;

		public static TLocalizedText TSkillDesc30010011 => null;

		public static TLocalizedText TSkillName30010012 => null;

		public static TLocalizedText TSkillDesc30010012 => null;

		public static TLocalizedText TSkillName30010015 => null;

		public static TLocalizedText TSkillDesc30010015 => null;

		public static TLocalizedText TSkillName30010016 => null;

		public static TLocalizedText TSkillDesc30010016 => null;

		public static TLocalizedText TSkillName30010017 => null;

		public static TLocalizedText TSkillDesc30010017 => null;

		public static TLocalizedText TSkillName30010020 => null;

		public static TLocalizedText TSkillDesc30010020 => null;

		public static TLocalizedText TSkillName30010021 => null;

		public static TLocalizedText TSkillDesc30010021 => null;

		public static TLocalizedText TSkillName30010022 => null;

		public static TLocalizedText TSkillDesc30010022 => null;

		public static TLocalizedText TSkillName30010030 => null;

		public static TLocalizedText TSkillDesc30010030 => null;

		public static TLocalizedText TSkillName30010040 => null;

		public static TLocalizedText TSkillDesc30010040 => null;

		public static TLocalizedText TSkillName30010041 => null;

		public static TLocalizedText TSkillDesc30010041 => null;

		public static TLocalizedText TSkillName31000000 => null;

		public static TLocalizedText TSkillDesc31000000 => null;

		public static TLocalizedText TSkillName31000001 => null;

		public static TLocalizedText TSkillDesc31000001 => null;

		public static TLocalizedText TSkillName31000002 => null;

		public static TLocalizedText TSkillDesc31000002 => null;

		public static TLocalizedText TSkillName40010001 => null;

		public static TLocalizedText TSkillDesc40010001 => null;

		public static TLocalizedText TSkillName40010003 => null;

		public static TLocalizedText TSkillDesc40010003 => null;

		public static TLocalizedText TSkillName40010004 => null;

		public static TLocalizedText TSkillDesc40010004 => null;

		public static TLocalizedText TSkillName40010005 => null;

		public static TLocalizedText TSkillDesc40010005 => null;

		public static TLocalizedText TSkillName40010006 => null;

		public static TLocalizedText TSkillDesc40010006 => null;

		public static TLocalizedText TSkillName40010007 => null;

		public static TLocalizedText TSkillDesc40010007 => null;

		public static TLocalizedText TSkillName40010008 => null;

		public static TLocalizedText TSkillDesc40010008 => null;

		public static TLocalizedText TSkillName40010009 => null;

		public static TLocalizedText TSkillDesc40010009 => null;

		public static TLocalizedText TSkillName40010010 => null;

		public static TLocalizedText TSkillDesc40010010 => null;

		public static TLocalizedText TSkillName50002000 => null;

		public static TLocalizedText TSkillDesc50002000 => null;

		public static TLocalizedText TSkillName50002001 => null;

		public static TLocalizedText TSkillDesc50002001 => null;

		public static TLocalizedText TSkillName50002002 => null;

		public static TLocalizedText TSkillDesc50002002 => null;

		public static TLocalizedText TSkillName50002003 => null;

		public static TLocalizedText TSkillDesc50002003 => null;

		public static TLocalizedText TSkillName50002006 => null;

		public static TLocalizedText TSkillDesc50002006 => null;

		public static TLocalizedText TSkillName50002011 => null;

		public static TLocalizedText TSkillDesc50002011 => null;

		public static TLocalizedText TSkillName50002012 => null;

		public static TLocalizedText TSkillDesc50002012 => null;

		public static TLocalizedText TSkillName50002013 => null;

		public static TLocalizedText TSkillDesc50002013 => null;

		public static TLocalizedText TSkillName50002016 => null;

		public static TLocalizedText TSkillDesc50002016 => null;

		public static TLocalizedText TSkillName50002021 => null;

		public static TLocalizedText TSkillDesc50002021 => null;

		public static TLocalizedText TSkillName50002031 => null;

		public static TLocalizedText TSkillDesc50002031 => null;

		public static TLocalizedText TSkillName50002032 => null;

		public static TLocalizedText TSkillDesc50002032 => null;

		public static TLocalizedText TSkillName50002041 => null;

		public static TLocalizedText TSkillDesc50002041 => null;

		public static TLocalizedText TSkillName50002042 => null;

		public static TLocalizedText TSkillDesc50002042 => null;

		public static TLocalizedText TSkillName50002051 => null;

		public static TLocalizedText TSkillDesc50002051 => null;

		public static TLocalizedText TSkillName50002052 => null;

		public static TLocalizedText TSkillDesc50002052 => null;

		public static TLocalizedText TSkillName50002061 => null;

		public static TLocalizedText TSkillDesc50002061 => null;

		public static TLocalizedText TSkillName50002062 => null;

		public static TLocalizedText TSkillDesc50002062 => null;

		public static TLocalizedText TSkillName50002071 => null;

		public static TLocalizedText TSkillDesc50002071 => null;

		public static TLocalizedText TSkillName50002072 => null;

		public static TLocalizedText TSkillDesc50002072 => null;

		public static TLocalizedText TSkillName50002091 => null;

		public static TLocalizedText TSkillDesc50002091 => null;

		public static TLocalizedText TSkillName50002092 => null;

		public static TLocalizedText TSkillDesc50002092 => null;

		public static TLocalizedText TSkillName50002093 => null;

		public static TLocalizedText TSkillDesc50002093 => null;

		public static TLocalizedText TSkillName50002094 => null;

		public static TLocalizedText TSkillDesc50002094 => null;

		public static TLocalizedText TSkillName50002095 => null;

		public static TLocalizedText TSkillDesc50002095 => null;

		public static TLocalizedText TSkillName50002096 => null;

		public static TLocalizedText TSkillDesc50002096 => null;

		public static TLocalizedText TSkillName50002097 => null;

		public static TLocalizedText TSkillDesc50002097 => null;

		public static TLocalizedText TSkillName50002101 => null;

		public static TLocalizedText TSkillDesc50002101 => null;

		public static TLocalizedText TSkillName50002102 => null;

		public static TLocalizedText TSkillDesc50002102 => null;

		public static TLocalizedText TSkillName50002111 => null;

		public static TLocalizedText TSkillDesc50002111 => null;

		public static TLocalizedText TSkillName50002112 => null;

		public static TLocalizedText TSkillDesc50002112 => null;

		public static TLocalizedText TSkillName50002121 => null;

		public static TLocalizedText TSkillDesc50002121 => null;

		public static TLocalizedText TSkillName50002122 => null;

		public static TLocalizedText TSkillDesc50002122 => null;

		public static TLocalizedText TSkillName50002131 => null;

		public static TLocalizedText TSkillDesc50002131 => null;

		public static TLocalizedText TSkillName50002141 => null;

		public static TLocalizedText TSkillDesc50002141 => null;

		public static TLocalizedText TSkillName50002142 => null;

		public static TLocalizedText TSkillDesc50002142 => null;

		public static TLocalizedText TSkillName50002201 => null;

		public static TLocalizedText TSkillDesc50002201 => null;

		public static TLocalizedText TSkillName50002202 => null;

		public static TLocalizedText TSkillDesc50002202 => null;

		public static TLocalizedText TSkillName50002203 => null;

		public static TLocalizedText TSkillDesc50002203 => null;

		public static TLocalizedText TSkillName50003001 => null;

		public static TLocalizedText TSkillDesc50003001 => null;

		public static TLocalizedText TSkillName50003002 => null;

		public static TLocalizedText TSkillDesc50003002 => null;

		public static TLocalizedText TSkillName50003003 => null;

		public static TLocalizedText TSkillDesc50003003 => null;

		public static TLocalizedText TSkillName50003004 => null;

		public static TLocalizedText TSkillDesc50003004 => null;

		public static TLocalizedText TSkillName50003005 => null;

		public static TLocalizedText TSkillDesc50003005 => null;

		public static TLocalizedText TSkillName50003006 => null;

		public static TLocalizedText TSkillDesc50003006 => null;

		public static TLocalizedText TSkillName50003007 => null;

		public static TLocalizedText TSkillDesc50003007 => null;

		public static TLocalizedText TSkillName50003008 => null;

		public static TLocalizedText TSkillDesc50003008 => null;

		public static TLocalizedText TSkillName50003009 => null;

		public static TLocalizedText TSkillDesc50003009 => null;

		public static TLocalizedText TSkillName50003010 => null;

		public static TLocalizedText TSkillDesc50003010 => null;

		public static TLocalizedText TSkillName61001014 => null;

		public static TLocalizedText TSkillDesc61001014 => null;

		public static TLocalizedText TSkillName61001021 => null;

		public static TLocalizedText TSkillDesc61001021 => null;

		public static TLocalizedText TSkillName61001031 => null;

		public static TLocalizedText TSkillDesc61001031 => null;

		public static TLocalizedText TSkillName61001032 => null;

		public static TLocalizedText TSkillDesc61001032 => null;

		public static TLocalizedText TSkillName61001041 => null;

		public static TLocalizedText TSkillDesc61001041 => null;

		public static TLocalizedText TSkillName61001042 => null;

		public static TLocalizedText TSkillDesc61001042 => null;

		public static TLocalizedText TSkillName61001043 => null;

		public static TLocalizedText TSkillDesc61001043 => null;

		public static TLocalizedText TSkillName61001061 => null;

		public static TLocalizedText TSkillDesc61001061 => null;

		public static TLocalizedText TSkillName61001062 => null;

		public static TLocalizedText TSkillDesc61001062 => null;

		public static TLocalizedText TSkillName61001063 => null;

		public static TLocalizedText TSkillDesc61001063 => null;

		public static TLocalizedText TSkillName62100001 => null;

		public static TLocalizedText TSkillDesc62100001 => null;

		public static TLocalizedText TSkillName62100002 => null;

		public static TLocalizedText TSkillDesc62100002 => null;

		public static TLocalizedText TSkillName65001001 => null;

		public static TLocalizedText TSkillDesc65001001 => null;

		public static TLocalizedText TSkillName65001002 => null;

		public static TLocalizedText TSkillDesc65001002 => null;

		public static TLocalizedText TSkillName65001003 => null;

		public static TLocalizedText TSkillDesc65001003 => null;

		public static TLocalizedText TSkillName65001004 => null;

		public static TLocalizedText TSkillDesc65001004 => null;

		public static TLocalizedText TSkillName65001005 => null;

		public static TLocalizedText TSkillDesc65001005 => null;

		public static TLocalizedText TSkillName65001006 => null;

		public static TLocalizedText TSkillDesc65001006 => null;

		public static TLocalizedText TSkillName65001007 => null;

		public static TLocalizedText TSkillDesc65001007 => null;

		public static TLocalizedText TSkillName65001008 => null;

		public static TLocalizedText TSkillDesc65001008 => null;

		public static TLocalizedText TSkillName65001009 => null;

		public static TLocalizedText TSkillDesc65001009 => null;

		public static TLocalizedText TSkillName65001010 => null;

		public static TLocalizedText TSkillDesc65001010 => null;

		public static TLocalizedText TSkillName65001011 => null;

		public static TLocalizedText TSkillDesc65001011 => null;

		public static TLocalizedText TSkillName65001012 => null;

		public static TLocalizedText TSkillDesc65001012 => null;

		public static TLocalizedText TSkillName65001013 => null;

		public static TLocalizedText TSkillDesc65001013 => null;

		public static TLocalizedText TSkillName65001014 => null;

		public static TLocalizedText TSkillDesc65001014 => null;

		public static TLocalizedText TSkillName65001015 => null;

		public static TLocalizedText TSkillDesc65001015 => null;

		public static TLocalizedText TSkillName65001016 => null;

		public static TLocalizedText TSkillDesc65001016 => null;

		public static TLocalizedText TSkillName65001017 => null;

		public static TLocalizedText TSkillDesc65001017 => null;

		public static TLocalizedText TSkillName65001018 => null;

		public static TLocalizedText TSkillDesc65001018 => null;

		public static TLocalizedText TSkillName65001019 => null;

		public static TLocalizedText TSkillDesc65001019 => null;

		public static TLocalizedText TSkillName65001020 => null;

		public static TLocalizedText TSkillDesc65001020 => null;

		public static TLocalizedText TSkillName65001021 => null;

		public static TLocalizedText TSkillDesc65001021 => null;

		public static TLocalizedText TSkillName65001022 => null;

		public static TLocalizedText TSkillDesc65001022 => null;

		public static TLocalizedText TSkillName65001023 => null;

		public static TLocalizedText TSkillDesc65001023 => null;

		public static TLocalizedText TSkillName65001024 => null;

		public static TLocalizedText TSkillDesc65001024 => null;

		public static TLocalizedText TSkillName65001025 => null;

		public static TLocalizedText TSkillDesc65001025 => null;

		public static TLocalizedText TSkillName65001026 => null;

		public static TLocalizedText TSkillDesc65001026 => null;

		public static TLocalizedText TSkillName65001027 => null;

		public static TLocalizedText TSkillDesc65001027 => null;

		public static TLocalizedText TSkillName65001028 => null;

		public static TLocalizedText TSkillDesc65001028 => null;

		public static TLocalizedText TSkillName65001029 => null;

		public static TLocalizedText TSkillDesc65001029 => null;

		public static TLocalizedText TSkillName65001030 => null;

		public static TLocalizedText TSkillDesc65001030 => null;

		public static TLocalizedText TSkillName65001031 => null;

		public static TLocalizedText TSkillDesc65001031 => null;

		public static TLocalizedText TSkillName65001032 => null;

		public static TLocalizedText TSkillDesc65001032 => null;

		public static TLocalizedText TSkillName65001033 => null;

		public static TLocalizedText TSkillDesc65001033 => null;

		public static TLocalizedText TSkillName65001034 => null;

		public static TLocalizedText TSkillDesc65001034 => null;

		public static TLocalizedText TSkillName65001035 => null;

		public static TLocalizedText TSkillDesc65001035 => null;

		public static TLocalizedText TSkillName65001036 => null;

		public static TLocalizedText TSkillDesc65001036 => null;

		public static TLocalizedText TSkillName65001037 => null;

		public static TLocalizedText TSkillDesc65001037 => null;

		public static TLocalizedText TSkillName65001038 => null;

		public static TLocalizedText TSkillDesc65001038 => null;

		public static TLocalizedText TSkillName65001039 => null;

		public static TLocalizedText TSkillDesc65001039 => null;

		public static TLocalizedText TSkillName65001040 => null;

		public static TLocalizedText TSkillDesc65001040 => null;

		public static TLocalizedText TSkillName65001041 => null;

		public static TLocalizedText TSkillDesc65001041 => null;

		public static TLocalizedText TSkillName65001042 => null;

		public static TLocalizedText TSkillDesc65001042 => null;

		public static TLocalizedText TSkillName65001043 => null;

		public static TLocalizedText TSkillDesc65001043 => null;

		public static TLocalizedText TSkillName65001044 => null;

		public static TLocalizedText TSkillDesc65001044 => null;

		public static TLocalizedText TSkillName65001045 => null;

		public static TLocalizedText TSkillDesc65001045 => null;

		public static TLocalizedText TSkillName65001046 => null;

		public static TLocalizedText TSkillDesc65001046 => null;

		public static TLocalizedText TSkillName65001047 => null;

		public static TLocalizedText TSkillDesc65001047 => null;

		public static TLocalizedText TSkillName65001048 => null;

		public static TLocalizedText TSkillDesc65001048 => null;

		public static TLocalizedText TSkillName65001049 => null;

		public static TLocalizedText TSkillDesc65001049 => null;

		public static TLocalizedText TSkillName65001050 => null;

		public static TLocalizedText TSkillDesc65001050 => null;

		public static TLocalizedText TSkillName65002001 => null;

		public static TLocalizedText TSkillDesc65002001 => null;

		public static TLocalizedText TSkillName65002002 => null;

		public static TLocalizedText TSkillDesc65002002 => null;

		public static TLocalizedText TSkillName65002003 => null;

		public static TLocalizedText TSkillDesc65002003 => null;

		public static TLocalizedText TSkillName65002004 => null;

		public static TLocalizedText TSkillDesc65002004 => null;

		public static TLocalizedText TSkillName65002005 => null;

		public static TLocalizedText TSkillDesc65002005 => null;

		public static TLocalizedText TSkillName65002006 => null;

		public static TLocalizedText TSkillDesc65002006 => null;

		public static TLocalizedText TSkillName65002007 => null;

		public static TLocalizedText TSkillDesc65002007 => null;

		public static TLocalizedText TSkillName65002008 => null;

		public static TLocalizedText TSkillDesc65002008 => null;

		public static TLocalizedText TSkillName65002009 => null;

		public static TLocalizedText TSkillDesc65002009 => null;

		public static TLocalizedText TSkillName65002010 => null;

		public static TLocalizedText TSkillDesc65002010 => null;

		public static TLocalizedText TSkillName65002011 => null;

		public static TLocalizedText TSkillDesc65002011 => null;

		public static TLocalizedText TSkillName65002012 => null;

		public static TLocalizedText TSkillDesc65002012 => null;

		public static TLocalizedText TSkillName65002013 => null;

		public static TLocalizedText TSkillDesc65002013 => null;

		public static TLocalizedText TSkillName65002014 => null;

		public static TLocalizedText TSkillDesc65002014 => null;

		public static TLocalizedText TSkillName65002015 => null;

		public static TLocalizedText TSkillDesc65002015 => null;

		public static TLocalizedText TSkillName65002016 => null;

		public static TLocalizedText TSkillDesc65002016 => null;

		public static TLocalizedText TSkillName65002017 => null;

		public static TLocalizedText TSkillDesc65002017 => null;

		public static TLocalizedText TSkillName65002018 => null;

		public static TLocalizedText TSkillDesc65002018 => null;

		public static TLocalizedText TSkillName65002019 => null;

		public static TLocalizedText TSkillDesc65002019 => null;

		public static TLocalizedText TSkillName65002020 => null;

		public static TLocalizedText TSkillDesc65002020 => null;

		public static TLocalizedText TSkillName65002021 => null;

		public static TLocalizedText TSkillDesc65002021 => null;

		public static TLocalizedText TSkillName65002022 => null;

		public static TLocalizedText TSkillDesc65002022 => null;

		public static TLocalizedText TSkillName65002023 => null;

		public static TLocalizedText TSkillDesc65002023 => null;

		public static TLocalizedText TSkillName65002024 => null;

		public static TLocalizedText TSkillDesc65002024 => null;

		public static TLocalizedText TSkillName65002025 => null;

		public static TLocalizedText TSkillDesc65002025 => null;

		public static TLocalizedText TSkillName65002026 => null;

		public static TLocalizedText TSkillDesc65002026 => null;

		public static TLocalizedText TSkillName65002027 => null;

		public static TLocalizedText TSkillDesc65002027 => null;

		public static TLocalizedText TSkillName65002028 => null;

		public static TLocalizedText TSkillDesc65002028 => null;

		public static TLocalizedText TSkillName65002029 => null;

		public static TLocalizedText TSkillDesc65002029 => null;

		public static TLocalizedText TSkillName65002030 => null;

		public static TLocalizedText TSkillDesc65002030 => null;

		public static TLocalizedText TSkillName65003001 => null;

		public static TLocalizedText TSkillDesc65003001 => null;

		public static TLocalizedText TSkillName65003002 => null;

		public static TLocalizedText TSkillDesc65003002 => null;

		public static TLocalizedText TSkillName65003003 => null;

		public static TLocalizedText TSkillDesc65003003 => null;

		public static TLocalizedText TSkillName65003004 => null;

		public static TLocalizedText TSkillDesc65003004 => null;

		public static TLocalizedText TSkillName65003005 => null;

		public static TLocalizedText TSkillDesc65003005 => null;

		public static TLocalizedText TSkillName65003006 => null;

		public static TLocalizedText TSkillDesc65003006 => null;

		public static TLocalizedText TSkillName65003007 => null;

		public static TLocalizedText TSkillDesc65003007 => null;

		public static TLocalizedText TSkillName65003008 => null;

		public static TLocalizedText TSkillDesc65003008 => null;

		public static TLocalizedText TSkillName65003009 => null;

		public static TLocalizedText TSkillDesc65003009 => null;

		public static TLocalizedText TSkillName65003010 => null;

		public static TLocalizedText TSkillDesc65003010 => null;

		public static TLocalizedText TSkillName65003011 => null;

		public static TLocalizedText TSkillDesc65003011 => null;

		public static TLocalizedText TSkillName65003012 => null;

		public static TLocalizedText TSkillDesc65003012 => null;

		public static TLocalizedText TSkillName65003013 => null;

		public static TLocalizedText TSkillDesc65003013 => null;

		public static TLocalizedText TSkillName65003014 => null;

		public static TLocalizedText TSkillDesc65003014 => null;

		public static TLocalizedText TSkillName65003015 => null;

		public static TLocalizedText TSkillDesc65003015 => null;

		public static TLocalizedText TSkillName65003016 => null;

		public static TLocalizedText TSkillDesc65003016 => null;

		public static TLocalizedText TSkillName65003017 => null;

		public static TLocalizedText TSkillDesc65003017 => null;

		public static TLocalizedText TSkillName65003018 => null;

		public static TLocalizedText TSkillDesc65003018 => null;

		public static TLocalizedText TSkillName65003019 => null;

		public static TLocalizedText TSkillDesc65003019 => null;

		public static TLocalizedText TSkillName65003020 => null;

		public static TLocalizedText TSkillDesc65003020 => null;

		public static TLocalizedText TSkillName65003021 => null;

		public static TLocalizedText TSkillDesc65003021 => null;

		public static TLocalizedText TSkillName65003022 => null;

		public static TLocalizedText TSkillDesc65003022 => null;

		public static TLocalizedText TSkillName65003023 => null;

		public static TLocalizedText TSkillDesc65003023 => null;

		public static TLocalizedText TSkillName65003024 => null;

		public static TLocalizedText TSkillDesc65003024 => null;

		public static TLocalizedText TSkillName65003025 => null;

		public static TLocalizedText TSkillDesc65003025 => null;

		public static TLocalizedText TSkillName65003026 => null;

		public static TLocalizedText TSkillDesc65003026 => null;

		public static TLocalizedText TSkillName65003027 => null;

		public static TLocalizedText TSkillDesc65003027 => null;

		public static TLocalizedText TSkillName65003028 => null;

		public static TLocalizedText TSkillDesc65003028 => null;

		public static TLocalizedText TSkillName65003029 => null;

		public static TLocalizedText TSkillDesc65003029 => null;

		public static TLocalizedText TSkillName65003030 => null;

		public static TLocalizedText TSkillDesc65003030 => null;

		public static TLocalizedText TSkillName65003031 => null;

		public static TLocalizedText TSkillDesc65003031 => null;

		public static TLocalizedText TSkillName65004001 => null;

		public static TLocalizedText TSkillDesc65004001 => null;

		public static TLocalizedText TSkillName65004002 => null;

		public static TLocalizedText TSkillDesc65004002 => null;

		public static TLocalizedText TSkillName65004003 => null;

		public static TLocalizedText TSkillDesc65004003 => null;

		public static TLocalizedText TSkillName65004004 => null;

		public static TLocalizedText TSkillDesc65004004 => null;

		public static TLocalizedText TSkillName65004005 => null;

		public static TLocalizedText TSkillDesc65004005 => null;

		public static TLocalizedText TSkillName65004006 => null;

		public static TLocalizedText TSkillDesc65004006 => null;

		public static TLocalizedText TSkillName65004007 => null;

		public static TLocalizedText TSkillDesc65004007 => null;

		public static TLocalizedText TSkillName65004008 => null;

		public static TLocalizedText TSkillDesc65004008 => null;

		public static TLocalizedText TSkillName65004009 => null;

		public static TLocalizedText TSkillDesc65004009 => null;

		public static TLocalizedText TSkillName65004010 => null;

		public static TLocalizedText TSkillDesc65004010 => null;

		public static TLocalizedText TSkillName65004011 => null;

		public static TLocalizedText TSkillDesc65004011 => null;

		public static TLocalizedText TSkillName65004012 => null;

		public static TLocalizedText TSkillDesc65004012 => null;

		public static TLocalizedText TSkillName65004013 => null;

		public static TLocalizedText TSkillDesc65004013 => null;

		public static TLocalizedText TSkillName65004014 => null;

		public static TLocalizedText TSkillDesc65004014 => null;

		public static TLocalizedText TSkillName65004015 => null;

		public static TLocalizedText TSkillDesc65004015 => null;

		public static TLocalizedText TSkillName70000001 => null;

		public static TLocalizedText TSkillDesc70000001 => null;

		public static TLocalizedText TSkillName70000002 => null;

		public static TLocalizedText TSkillDesc70000002 => null;

		public static TLocalizedText TSkillName71000001 => null;

		public static TLocalizedText TSkillDesc71000001 => null;

		public static TLocalizedText TSkillName90000000 => null;

		public static TLocalizedText TSkillDesc90000000 => null;

		public static TLocalizedText TSkillName90000001 => null;

		public static TLocalizedText TSkillDesc90000001 => null;

		public static TLocalizedText TSkillName90000002 => null;

		public static TLocalizedText TSkillDesc90000002 => null;

		public static TLocalizedText TSkillName90000003 => null;

		public static TLocalizedText TSkillDesc90000003 => null;

		public static TLocalizedText TSkillName90000004 => null;

		public static TLocalizedText TSkillDesc90000004 => null;

		public static TLocalizedText TSkillExecutorName10001 => null;

		public static TLocalizedText TStatusEffectTagToastTextBleed => null;

		public static TLocalizedText TStatusEffectTagToastTextBlind => null;

		public static TLocalizedText TStatusEffectTagToastTextBurn => null;

		public static TLocalizedText TStatusEffectTagToastTextConfuse => null;

		public static TLocalizedText TStatusEffectTagToastTextDefReduction => null;

		public static TLocalizedText TStatusEffectTagToastTextDespair => null;

		public static TLocalizedText TStatusEffectTagToastTextDullahanUltimate => null;

		public static TLocalizedText TStatusEffectTagToastTextFreeze => null;

		public static TLocalizedText TStatusEffectTagToastTextKnockback => null;

		public static TLocalizedText TStatusEffectTagToastTextLight => null;

		public static TLocalizedText TStatusEffectTagToastTextMadness => null;

		public static TLocalizedText TStatusEffectTagToastTextNone => null;

		public static TLocalizedText TStatusEffectTagToastTextPoison => null;

		public static TLocalizedText TStatusEffectTagToastTextPull => null;

		public static TLocalizedText TStatusEffectTagToastTextPunishment => null;

		public static TLocalizedText TStatusEffectTagToastTextRoot => null;

		public static TLocalizedText TStatusEffectTagToastTextSilence => null;

		public static TLocalizedText TStatusEffectTagToastTextSlow => null;

		public static TLocalizedText TStatusEffectTagToastTextStun => null;

		public static TLocalizedText TStatusEffectTagToastTextTimer => null;

		public static TLocalizedText TStatusEffectTagToastTextWeaken => null;

		public static TLocalizedText TTalentName1001 => null;

		public static TLocalizedText TTalentDesc1001 => null;

		public static TLocalizedText TTalentName1002 => null;

		public static TLocalizedText TTalentDesc1002 => null;

		public static TLocalizedText TTalentName1003 => null;

		public static TLocalizedText TTalentDesc1003 => null;

		public static TLocalizedText TTalentName1004 => null;

		public static TLocalizedText TTalentDesc1004 => null;

		public static TLocalizedText TTalentName1005 => null;

		public static TLocalizedText TTalentDesc1005 => null;

		public static TLocalizedText TTalentName1006 => null;

		public static TLocalizedText TTalentDesc1006 => null;

		public static TLocalizedText TTalentName1007 => null;

		public static TLocalizedText TTalentDesc1007 => null;

		public static TLocalizedText TTalentName1008 => null;

		public static TLocalizedText TTalentDesc1008 => null;

		public static TLocalizedText TTalentName1009 => null;

		public static TLocalizedText TTalentDesc1009 => null;

		public static TLocalizedText TTalentName1010 => null;

		public static TLocalizedText TTalentDesc1010 => null;

		public static TLocalizedText TTalentName1011 => null;

		public static TLocalizedText TTalentDesc1011 => null;

		public static TLocalizedText TTalentName1012 => null;

		public static TLocalizedText TTalentDesc1012 => null;

		public static TLocalizedText TTalentName1013 => null;

		public static TLocalizedText TTalentDesc1013 => null;

		public static TLocalizedText TTalentName1014 => null;

		public static TLocalizedText TTalentDesc1014 => null;

		public static TLocalizedText TTalentName1015 => null;

		public static TLocalizedText TTalentDesc1015 => null;

		public static TLocalizedText TTalentName1016 => null;

		public static TLocalizedText TTalentDesc1016 => null;

		public static TLocalizedText TTalentName1017 => null;

		public static TLocalizedText TTalentDesc1017 => null;

		public static TLocalizedText TTalentName1018 => null;

		public static TLocalizedText TTalentDesc1018 => null;

		public static TLocalizedText TTalentName1019 => null;

		public static TLocalizedText TTalentDesc1019 => null;

		public static TLocalizedText TTalentName1020 => null;

		public static TLocalizedText TTalentDesc1020 => null;

		public static TLocalizedText TTalentName1021 => null;

		public static TLocalizedText TTalentDesc1021 => null;

		public static TLocalizedText TTalentName1022 => null;

		public static TLocalizedText TTalentDesc1022 => null;

		public static TLocalizedText TTalentName1023 => null;

		public static TLocalizedText TTalentDesc1023 => null;

		public static TLocalizedText TTalentName1024 => null;

		public static TLocalizedText TTalentDesc1024 => null;

		public static TLocalizedText TTalentName1025 => null;

		public static TLocalizedText TTalentDesc1025 => null;

		public static TLocalizedText TTalentName1026 => null;

		public static TLocalizedText TTalentDesc1026 => null;

		public static TLocalizedText TTalentName1027 => null;

		public static TLocalizedText TTalentDesc1027 => null;

		public static TLocalizedText TTalentName1028 => null;

		public static TLocalizedText TTalentDesc1028 => null;

		public static TLocalizedText TTalentName1029 => null;

		public static TLocalizedText TTalentDesc1029 => null;

		public static TLocalizedText TTalentName1030 => null;

		public static TLocalizedText TTalentDesc1030 => null;

		public static TLocalizedText TTalentName1031 => null;

		public static TLocalizedText TTalentDesc1031 => null;

		public static TLocalizedText TTalentName1032 => null;

		public static TLocalizedText TTalentDesc1032 => null;

		public static TLocalizedText TTalentName1033 => null;

		public static TLocalizedText TTalentDesc1033 => null;

		public static TLocalizedText TTalentName1034 => null;

		public static TLocalizedText TTalentDesc1034 => null;

		public static TLocalizedText TTalentName1035 => null;

		public static TLocalizedText TTalentDesc1035 => null;

		public static TLocalizedText TTalentName1036 => null;

		public static TLocalizedText TTalentDesc1036 => null;

		public static TLocalizedText TTalentName1037 => null;

		public static TLocalizedText TTalentDesc1037 => null;

		public static TLocalizedText TTalentName1038 => null;

		public static TLocalizedText TTalentDesc1038 => null;

		public static TLocalizedText TTalentName1039 => null;

		public static TLocalizedText TTalentDesc1039 => null;

		public static TLocalizedText TTalentName1040 => null;

		public static TLocalizedText TTalentDesc1040 => null;

		public static TLocalizedText TTalentName1041 => null;

		public static TLocalizedText TTalentDesc1041 => null;

		public static TLocalizedText TTalentName1042 => null;

		public static TLocalizedText TTalentDesc1042 => null;

		public static TLocalizedText TTalentName1043 => null;

		public static TLocalizedText TTalentDesc1043 => null;

		public static TLocalizedText TTalentName1044 => null;

		public static TLocalizedText TTalentDesc1044 => null;

		public static TLocalizedText TTalentName1045 => null;

		public static TLocalizedText TTalentDesc1045 => null;

		public static TLocalizedText TTalentName1046 => null;

		public static TLocalizedText TTalentDesc1046 => null;

		public static TLocalizedText TTalentName1047 => null;

		public static TLocalizedText TTalentDesc1047 => null;

		public static TLocalizedText TTalentName1048 => null;

		public static TLocalizedText TTalentDesc1048 => null;

		public static TLocalizedText TTalentName1049 => null;

		public static TLocalizedText TTalentDesc1049 => null;

		public static TLocalizedText TTalentName1050 => null;

		public static TLocalizedText TTalentDesc1050 => null;

		public static TLocalizedText TTalentName1051 => null;

		public static TLocalizedText TTalentDesc1051 => null;

		public static TLocalizedText TTalentName1052 => null;

		public static TLocalizedText TTalentDesc1052 => null;

		public static TLocalizedText TTalentName2001 => null;

		public static TLocalizedText TTalentDesc2001 => null;

		public static TLocalizedText TTalentName2002 => null;

		public static TLocalizedText TTalentDesc2002 => null;

		public static TLocalizedText TTalentName2003 => null;

		public static TLocalizedText TTalentDesc2003 => null;

		public static TLocalizedText TTalentName2004 => null;

		public static TLocalizedText TTalentDesc2004 => null;

		public static TLocalizedText TTalentName2005 => null;

		public static TLocalizedText TTalentDesc2005 => null;

		public static TLocalizedText TTalentName2006 => null;

		public static TLocalizedText TTalentDesc2006 => null;

		public static TLocalizedText TTalentName2007 => null;

		public static TLocalizedText TTalentDesc2007 => null;

		public static TLocalizedText TTalentName2008 => null;

		public static TLocalizedText TTalentDesc2008 => null;

		public static TLocalizedText TTalentName2009 => null;

		public static TLocalizedText TTalentDesc2009 => null;

		public static TLocalizedText TTalentName2010 => null;

		public static TLocalizedText TTalentDesc2010 => null;

		public static TLocalizedText TTalentName2011 => null;

		public static TLocalizedText TTalentDesc2011 => null;

		public static TLocalizedText TTalentName2012 => null;

		public static TLocalizedText TTalentDesc2012 => null;

		public static TLocalizedText TTalentName2013 => null;

		public static TLocalizedText TTalentDesc2013 => null;

		public static TLocalizedText TTalentName2014 => null;

		public static TLocalizedText TTalentDesc2014 => null;

		public static TLocalizedText TTalentName2015 => null;

		public static TLocalizedText TTalentDesc2015 => null;

		public static TLocalizedText TTalentName2016 => null;

		public static TLocalizedText TTalentDesc2016 => null;

		public static TLocalizedText TTalentName2017 => null;

		public static TLocalizedText TTalentDesc2017 => null;

		public static TLocalizedText TTalentName2018 => null;

		public static TLocalizedText TTalentDesc2018 => null;

		public static TLocalizedText TTalentName2019 => null;

		public static TLocalizedText TTalentDesc2019 => null;

		public static TLocalizedText TTalentName2020 => null;

		public static TLocalizedText TTalentDesc2020 => null;

		public static TLocalizedText TTalentName2021 => null;

		public static TLocalizedText TTalentDesc2021 => null;

		public static TLocalizedText TTalentName2022 => null;

		public static TLocalizedText TTalentDesc2022 => null;

		public static TLocalizedText TTalentName2023 => null;

		public static TLocalizedText TTalentDesc2023 => null;

		public static TLocalizedText TTalentName2024 => null;

		public static TLocalizedText TTalentDesc2024 => null;

		public static TLocalizedText TTalentName2025 => null;

		public static TLocalizedText TTalentDesc2025 => null;

		public static TLocalizedText TTalentName2026 => null;

		public static TLocalizedText TTalentDesc2026 => null;

		public static TLocalizedText TTalentName2027 => null;

		public static TLocalizedText TTalentDesc2027 => null;

		public static TLocalizedText TTalentName2028 => null;

		public static TLocalizedText TTalentDesc2028 => null;

		public static TLocalizedText TTalentName2029 => null;

		public static TLocalizedText TTalentDesc2029 => null;

		public static TLocalizedText TTalentName2030 => null;

		public static TLocalizedText TTalentDesc2030 => null;

		public static TLocalizedText TTalentName3001 => null;

		public static TLocalizedText TTalentDesc3001 => null;

		public static TLocalizedText TTalentName3002 => null;

		public static TLocalizedText TTalentDesc3002 => null;

		public static TLocalizedText TTalentName3003 => null;

		public static TLocalizedText TTalentDesc3003 => null;

		public static TLocalizedText TTalentName3004 => null;

		public static TLocalizedText TTalentDesc3004 => null;

		public static TLocalizedText TTalentName3005 => null;

		public static TLocalizedText TTalentDesc3005 => null;

		public static TLocalizedText TTalentName3006 => null;

		public static TLocalizedText TTalentDesc3006 => null;

		public static TLocalizedText TTalentName3007 => null;

		public static TLocalizedText TTalentDesc3007 => null;

		public static TLocalizedText TTalentName3008 => null;

		public static TLocalizedText TTalentDesc3008 => null;

		public static TLocalizedText TTalentName3009 => null;

		public static TLocalizedText TTalentDesc3009 => null;

		public static TLocalizedText TTalentName3010 => null;

		public static TLocalizedText TTalentDesc3010 => null;

		public static TLocalizedText TTalentName3011 => null;

		public static TLocalizedText TTalentDesc3011 => null;

		public static TLocalizedText TTalentName3012 => null;

		public static TLocalizedText TTalentDesc3012 => null;

		public static TLocalizedText TTalentName3013 => null;

		public static TLocalizedText TTalentDesc3013 => null;

		public static TLocalizedText TTalentName3014 => null;

		public static TLocalizedText TTalentDesc3014 => null;

		public static TLocalizedText TTalentName3015 => null;

		public static TLocalizedText TTalentDesc3015 => null;

		public static TLocalizedText TTalentName3016 => null;

		public static TLocalizedText TTalentDesc3016 => null;

		public static TLocalizedText TTalentName3017 => null;

		public static TLocalizedText TTalentDesc3017 => null;

		public static TLocalizedText TTalentName3018 => null;

		public static TLocalizedText TTalentDesc3018 => null;

		public static TLocalizedText TTalentName3019 => null;

		public static TLocalizedText TTalentDesc3019 => null;

		public static TLocalizedText TTalentName3020 => null;

		public static TLocalizedText TTalentDesc3020 => null;

		public static TLocalizedText TTalentName3021 => null;

		public static TLocalizedText TTalentDesc3021 => null;

		public static TLocalizedText TTalentName3022 => null;

		public static TLocalizedText TTalentDesc3022 => null;

		public static TLocalizedText TTalentName3023 => null;

		public static TLocalizedText TTalentDesc3023 => null;

		public static TLocalizedText TTalentName3024 => null;

		public static TLocalizedText TTalentDesc3024 => null;

		public static TLocalizedText TTalentName3025 => null;

		public static TLocalizedText TTalentDesc3025 => null;

		public static TLocalizedText TTalentName3026 => null;

		public static TLocalizedText TTalentDesc3026 => null;

		public static TLocalizedText TTalentName3027 => null;

		public static TLocalizedText TTalentDesc3027 => null;

		public static TLocalizedText TTalentName3028 => null;

		public static TLocalizedText TTalentDesc3028 => null;

		public static TLocalizedText TTalentName3029 => null;

		public static TLocalizedText TTalentDesc3029 => null;

		public static TLocalizedText TTalentName3030 => null;

		public static TLocalizedText TTalentDesc3030 => null;

		public static TLocalizedText TTalentName3031 => null;

		public static TLocalizedText TTalentDesc3031 => null;

		public static TLocalizedText TTalentName4001 => null;

		public static TLocalizedText TTalentDesc4001 => null;

		public static TLocalizedText TTalentName4002 => null;

		public static TLocalizedText TTalentDesc4002 => null;

		public static TLocalizedText TTalentName4003 => null;

		public static TLocalizedText TTalentDesc4003 => null;

		public static TLocalizedText TTalentName4004 => null;

		public static TLocalizedText TTalentDesc4004 => null;

		public static TLocalizedText TTalentName4005 => null;

		public static TLocalizedText TTalentDesc4005 => null;

		public static TLocalizedText TTalentName4006 => null;

		public static TLocalizedText TTalentDesc4006 => null;

		public static TLocalizedText TTalentName4007 => null;

		public static TLocalizedText TTalentDesc4007 => null;

		public static TLocalizedText TTalentName4008 => null;

		public static TLocalizedText TTalentDesc4008 => null;

		public static TLocalizedText TTalentName4009 => null;

		public static TLocalizedText TTalentDesc4009 => null;

		public static TLocalizedText TTalentName4010 => null;

		public static TLocalizedText TTalentDesc4010 => null;

		public static TLocalizedText TTalentName4011 => null;

		public static TLocalizedText TTalentDesc4011 => null;

		public static TLocalizedText TTalentName4012 => null;

		public static TLocalizedText TTalentDesc4012 => null;

		public static TLocalizedText TTalentName4013 => null;

		public static TLocalizedText TTalentDesc4013 => null;

		public static TLocalizedText TTalentName4014 => null;

		public static TLocalizedText TTalentDesc4014 => null;

		public static TLocalizedText TTalentName4015 => null;

		public static TLocalizedText TTalentDesc4015 => null;

		public static TLocalizedText TTalentName4016 => null;

		public static TLocalizedText TTalentDesc4016 => null;

		public static TLocalizedText TTalentName4017 => null;

		public static TLocalizedText TTalentDesc4017 => null;

		public static TLocalizedText TTalentName4018 => null;

		public static TLocalizedText TTalentDesc4018 => null;

		public static TLocalizedText TTalentName4019 => null;

		public static TLocalizedText TTalentDesc4019 => null;

		public static TLocalizedText TTalentName4020 => null;

		public static TLocalizedText TTalentDesc4020 => null;

		public static TLocalizedText TTalentName4021 => null;

		public static TLocalizedText TTalentDesc4021 => null;

		public static TLocalizedText TTalentName4022 => null;

		public static TLocalizedText TTalentDesc4022 => null;

		public static TLocalizedText TTalentName4023 => null;

		public static TLocalizedText TTalentDesc4023 => null;

		public static TLocalizedText TTalentName4024 => null;

		public static TLocalizedText TTalentDesc4024 => null;

		public static TLocalizedText TTalentName4025 => null;

		public static TLocalizedText TTalentDesc4025 => null;

		public static TLocalizedText TTalentName4026 => null;

		public static TLocalizedText TTalentDesc4026 => null;

		public static TLocalizedText TTalentName4027 => null;

		public static TLocalizedText TTalentDesc4027 => null;

		public static TLocalizedText TTalentName4028 => null;

		public static TLocalizedText TTalentDesc4028 => null;

		public static TLocalizedText TTalentName4029 => null;

		public static TLocalizedText TTalentDesc4029 => null;

		public static TLocalizedText TTalentName4030 => null;

		public static TLocalizedText TTalentDesc4030 => null;

		public static TLocalizedText TTalentName4031 => null;

		public static TLocalizedText TTalentDesc4031 => null;

		public static TLocalizedText TTalentName4032 => null;

		public static TLocalizedText TTalentDesc4032 => null;

		public static TLocalizedText TTalentName4033 => null;

		public static TLocalizedText TTalentDesc4033 => null;

		public static TLocalizedText TTechTreeName1001 => null;

		public static TLocalizedText TTechTreeDesc1001 => null;

		public static TLocalizedText TTechTreeName1002 => null;

		public static TLocalizedText TTechTreeDesc1002 => null;

		public static TLocalizedText TTechTreeName1003 => null;

		public static TLocalizedText TTechTreeDesc1003 => null;

		public static TLocalizedText TTechTreeName1004 => null;

		public static TLocalizedText TTechTreeDesc1004 => null;

		public static TLocalizedText TTechTreeName1005 => null;

		public static TLocalizedText TTechTreeDesc1005 => null;

		public static TLocalizedText TTechTreeName1006 => null;

		public static TLocalizedText TTechTreeDesc1006 => null;

		public static TLocalizedText TTechTreeName1007 => null;

		public static TLocalizedText TTechTreeDesc1007 => null;

		public static TLocalizedText TTechTreeName3001 => null;

		public static TLocalizedText TTechTreeDesc3001 => null;

		public static TLocalizedText TTechTreeName3002 => null;

		public static TLocalizedText TTechTreeDesc3002 => null;

		public static TLocalizedText TTechTreeName3003 => null;

		public static TLocalizedText TTechTreeDesc3003 => null;

		public static TLocalizedText TTechTreeName3004 => null;

		public static TLocalizedText TTechTreeDesc3004 => null;

		public static TLocalizedText TTechTreeName4001 => null;

		public static TLocalizedText TTechTreeDesc4001 => null;

		public static TLocalizedText TTechTreeName4002 => null;

		public static TLocalizedText TTechTreeDesc4002 => null;

		public static TLocalizedText TTechTreeName4003 => null;

		public static TLocalizedText TTechTreeDesc4003 => null;

		public static TLocalizedText TTechTreeName4004 => null;

		public static TLocalizedText TTechTreeDesc4004 => null;

		public static TLocalizedText TTechTreeName5001 => null;

		public static TLocalizedText TTechTreeDesc5001 => null;

		public static TLocalizedText TTechTreeName5002 => null;

		public static TLocalizedText TTechTreeDesc5002 => null;

		public static TLocalizedText TTechTreeName5003 => null;

		public static TLocalizedText TTechTreeDesc5003 => null;

		public static TLocalizedText TTechTreeName5004 => null;

		public static TLocalizedText TTechTreeDesc5004 => null;

		public static TLocalizedText TTechTreeName6001 => null;

		public static TLocalizedText TTechTreeDesc6001 => null;

		public static TLocalizedText TTechTreeName6002 => null;

		public static TLocalizedText TTechTreeDesc6002 => null;

		public static TLocalizedText TTechTreeName6003 => null;

		public static TLocalizedText TTechTreeDesc6003 => null;

		public static TLocalizedText TTechTreeName7001 => null;

		public static TLocalizedText TTechTreeDesc7001 => null;

		public static TLocalizedText TTechTreeName7002 => null;

		public static TLocalizedText TTechTreeDesc7002 => null;

		public static TLocalizedText TTechTreeName7003 => null;

		public static TLocalizedText TTechTreeDesc7003 => null;

		public static TLocalizedText TTechTreeName8001 => null;

		public static TLocalizedText TTechTreeDesc8001 => null;

		public static TLocalizedText TTechTreeName8002 => null;

		public static TLocalizedText TTechTreeDesc8002 => null;

		public static TLocalizedText TTechTreeName8003 => null;

		public static TLocalizedText TTechTreeDesc8003 => null;

		public static TLocalizedText TTechTreeName8004 => null;

		public static TLocalizedText TTechTreeDesc8004 => null;

		public static TLocalizedText TTechTreeName9001 => null;

		public static TLocalizedText TTechTreeDesc9001 => null;

		public static TLocalizedText TTechTreeName9002 => null;

		public static TLocalizedText TTechTreeDesc9002 => null;

		public static TLocalizedText TTechTreeName9003 => null;

		public static TLocalizedText TTechTreeDesc9003 => null;

		public static TLocalizedText TTechTreeName10001 => null;

		public static TLocalizedText TTechTreeDesc10001 => null;

		public static TLocalizedText TTechTreeName10002 => null;

		public static TLocalizedText TTechTreeDesc10002 => null;

		public static TLocalizedText TTechTreeName11001 => null;

		public static TLocalizedText TTechTreeDesc11001 => null;

		public static TLocalizedText TTechTreeName11002 => null;

		public static TLocalizedText TTechTreeDesc11002 => null;

		public static TLocalizedText TTechTreeName11003 => null;

		public static TLocalizedText TTechTreeDesc11003 => null;

		public static TLocalizedText TTechTreeName12001 => null;

		public static TLocalizedText TTechTreeDesc12001 => null;

		public static TLocalizedText TTechTreeName12002 => null;

		public static TLocalizedText TTechTreeDesc12002 => null;

		public static TLocalizedText TTechTreeName12003 => null;

		public static TLocalizedText TTechTreeDesc12003 => null;

		public static TLocalizedText TTechTreeName12004 => null;

		public static TLocalizedText TTechTreeDesc12004 => null;

		public static TLocalizedText TTechTreeName12005 => null;

		public static TLocalizedText TTechTreeDesc12005 => null;

		public static TLocalizedText TTrapName80001 => null;

		public static TLocalizedText TTrapName80002 => null;

		public static TLocalizedText TTrapName80003 => null;

		public static TLocalizedText TTrapName80004 => null;

		public static TLocalizedText TTrapName80005 => null;

		public static TLocalizedText TTrapName80006 => null;

		public static TLocalizedText TTrapName80007 => null;

		public static TLocalizedText TTrapName90001 => null;

		public static TLocalizedText TTrapName90002 => null;

		public static TLocalizedText TTrapName90003 => null;

		public static TLocalizedText TTrapName90004 => null;

		public static TLocalizedText TTrapName90005 => null;

		public static TLocalizedText TTycoonSalesSlotName101 => null;

		public static TLocalizedText TTycoonSalesSlotName102 => null;

		public static TLocalizedText TTycoonSalesSlotName103 => null;

		public static TLocalizedText TTycoonSalesSlotName104 => null;

		public static TLocalizedText TTycoonSalesSlotName105 => null;

		public static TLocalizedText TTycoonSalesSlotName106 => null;

		public static TLocalizedText TTycoonSalesSlotName107 => null;

		public static TLocalizedText TTycoonSalesSlotName108 => null;

		public static TLocalizedText TTycoonSalesSlotName109 => null;

		public static TLocalizedText TTycoonSalesSlotName110 => null;

		public static TLocalizedText TTycoonSalesSlotName201 => null;

		public static TLocalizedText TWaveName1000 => null;

		public static TLocalizedText TWaveName1001 => null;

		public static TLocalizedText TWaveName1011 => null;

		public static TLocalizedText TWaveName1021 => null;

		public static TLocalizedText TWaveName11041 => null;

		public static TLocalizedText TWaveName12011 => null;

		public static TLocalizedText TWaveName12031 => null;

		public static TLocalizedText TWaveName600001 => null;

		public static TLocalizedText TWaveName600002 => null;

		public static TLocalizedText TWaveName600003 => null;

		public static TLocalizedText TWaveName600004 => null;

		public static TLocalizedText TWaveName610001 => null;

		public static TLocalizedText TWaveName610002 => null;

		public static TLocalizedText TWaveName610003 => null;

		public static TLocalizedText TWaveName801021 => null;

		public static TLocalizedText TWaveName802021 => null;

		public static TLocalizedText TWaveName803013 => null;

		public static TLocalizedText TZoneSpawnName1001 => null;

		public static TLocalizedText TZoneSpawnName1002 => null;

		public static TLocalizedText TZoneSpawnName1003 => null;

		public static TLocalizedText TZoneSpawnName1004 => null;

		public static TLocalizedText TZoneSpawnName1005 => null;

		public static TLocalizedText TZoneSpawnName1006 => null;

		public static TLocalizedText TZoneSpawnName1007 => null;

		public static TLocalizedText TZoneSpawnName1008 => null;

		public static TLocalizedText TZoneSpawnName1009 => null;

		public static TLocalizedText TZoneSpawnName1011 => null;

		public static TLocalizedText TZoneSpawnName1012 => null;

		public static TLocalizedText TZoneSpawnName1013 => null;

		public static TLocalizedText TZoneSpawnName1014 => null;

		public static TLocalizedText TZoneSpawnName1015 => null;

		public static TLocalizedText TZoneSpawnName1016 => null;

		public static TLocalizedText TZoneSpawnName1017 => null;

		public static TLocalizedText TZoneSpawnName1018 => null;

		public static TLocalizedText TZoneSpawnName1021 => null;

		public static TLocalizedText TZoneSpawnName1022 => null;

		public static TLocalizedText TZoneSpawnName1023 => null;

		public static TLocalizedText TZoneSpawnName1024 => null;

		public static TLocalizedText TZoneSpawnName1025 => null;

		public static TLocalizedText TZoneSpawnName1026 => null;

		public static TLocalizedText TZoneSpawnName1027 => null;

		public static TLocalizedText TZoneSpawnName1028 => null;

		public static TLocalizedText TZoneSpawnName1031 => null;

		public static TLocalizedText TZoneSpawnName1032 => null;

		public static TLocalizedText TZoneSpawnName1033 => null;

		public static TLocalizedText TZoneSpawnName1034 => null;

		public static TLocalizedText TZoneSpawnName1035 => null;

		public static TLocalizedText TZoneSpawnName1036 => null;

		public static TLocalizedText TZoneSpawnName1041 => null;

		public static TLocalizedText TZoneSpawnName1042 => null;

		public static TLocalizedText TZoneSpawnName1043 => null;

		public static TLocalizedText TZoneSpawnName1044 => null;

		public static TLocalizedText TZoneSpawnName1045 => null;

		public static TLocalizedText TZoneSpawnName1046 => null;

		public static TLocalizedText TZoneSpawnName1101 => null;

		public static TLocalizedText TZoneSpawnName1102 => null;

		public static TLocalizedText TZoneSpawnName1111 => null;

		public static TLocalizedText TZoneSpawnName1112 => null;

		public static TLocalizedText TZoneSpawnName1113 => null;

		public static TLocalizedText TZoneSpawnName1114 => null;

		public static TLocalizedText TZoneSpawnName1115 => null;

		public static TLocalizedText TZoneSpawnName1116 => null;

		public static TLocalizedText TZoneSpawnName2001 => null;

		public static TLocalizedText TZoneSpawnName2002 => null;

		public static TLocalizedText TZoneSpawnName3001 => null;

		public static TLocalizedText TZoneSpawnName3002 => null;

		public static TLocalizedText TZoneSpawnName3003 => null;

		public static TLocalizedText TZoneSpawnName3004 => null;

		public static TLocalizedText TZoneSpawnName3005 => null;

		public static TLocalizedText TZoneSpawnName10001 => null;

		public static TLocalizedText TZoneSpawnName10002 => null;

		public static TLocalizedText TZoneSpawnName10003 => null;

		public static TLocalizedText TZoneSpawnName10004 => null;

		public static TLocalizedText TZoneSpawnName11001 => null;

		public static TLocalizedText TZoneSpawnName11002 => null;

		public static TLocalizedText TZoneSpawnName11003 => null;

		public static TLocalizedText TZoneSpawnName11004 => null;

		public static TLocalizedText TZoneSpawnName11005 => null;

		public static TLocalizedText TZoneSpawnName11021 => null;

		public static TLocalizedText TZoneSpawnName11031 => null;

		public static TLocalizedText TZoneSpawnName11041 => null;

		public static TLocalizedText TZoneSpawnName11042 => null;

		public static TLocalizedText TZoneSpawnName11043 => null;

		public static TLocalizedText TZoneSpawnName11051 => null;

		public static TLocalizedText TZoneSpawnName11052 => null;

		public static TLocalizedText TZoneSpawnName12011 => null;

		public static TLocalizedText TZoneSpawnName12012 => null;

		public static TLocalizedText TZoneSpawnName12013 => null;

		public static TLocalizedText TZoneSpawnName12021 => null;

		public static TLocalizedText TZoneSpawnName12022 => null;

		public static TLocalizedText TZoneSpawnName12031 => null;

		public static TLocalizedText TZoneSpawnName12032 => null;

		public static TLocalizedText TZoneSpawnName12033 => null;

		public static TLocalizedText TZoneSpawnName12041 => null;

		public static TLocalizedText TZoneSpawnName12042 => null;

		public static TLocalizedText TZoneSpawnName12043 => null;

		public static TLocalizedText TZoneSpawnName12044 => null;

		public static TLocalizedText TZoneSpawnName82101 => null;

		public static TLocalizedText TZoneSpawnName82103 => null;

		public static TLocalizedText TZoneSpawnName82109 => null;

		public static TLocalizedText TZoneSpawnName82112 => null;

		public static TLocalizedText TZoneSpawnName82203 => null;

		public static TLocalizedText TZoneSpawnName82205 => null;

		public static TLocalizedText TZoneSpawnName82206 => null;

		public static TLocalizedText TZoneSpawnName82207 => null;

		public static TLocalizedText TZoneSpawnName91011 => null;

		public static TLocalizedText TZoneSpawnName91012 => null;

		public static TLocalizedText TZoneSpawnName91021 => null;

		public static TLocalizedText TZoneSpawnName91022 => null;

		public static TLocalizedText TZoneSpawnName91031 => null;

		public static TLocalizedText TZoneSpawnName91032 => null;

		public static TLocalizedText TZoneSpawnName91041 => null;

		public static TLocalizedText TZoneSpawnName91042 => null;

		public static TLocalizedText TZoneSpawnName91051 => null;

		public static TLocalizedText TZoneSpawnName91052 => null;

		public static TLocalizedText TZoneSpawnName91111 => null;

		public static TLocalizedText TZoneSpawnName91121 => null;

		public static TLocalizedText TZoneSpawnName91131 => null;

		public static TLocalizedText TZoneSpawnName91141 => null;

		public static TLocalizedText TZoneSpawnName91151 => null;

		public static TLocalizedText TZoneSpawnName91161 => null;

		public static TLocalizedText TZoneSpawnName91171 => null;

		public static TLocalizedText TZoneSpawnName91181 => null;

		public static TLocalizedText TZoneSpawnName91211 => null;

		public static TLocalizedText TZoneSpawnName91212 => null;

		public static TLocalizedText TZoneSpawnName91221 => null;

		public static TLocalizedText TZoneSpawnName91222 => null;

		public static TLocalizedText TZoneSpawnName91231 => null;

		public static TLocalizedText TZoneSpawnName91241 => null;

		public static TLocalizedText TZoneSpawnName91242 => null;

		public static TLocalizedText TZoneSpawnName91251 => null;

		public static TLocalizedText TZoneSpawnName91261 => null;

		public static TLocalizedText TZoneSpawnName400001 => null;

		public static TLocalizedText TZoneSpawnName400002 => null;

		public static TLocalizedText TZoneSpawnName600001 => null;

		public static TLocalizedText TZoneSpawnName600002 => null;

		public static TLocalizedText TZoneSpawnName600003 => null;

		public static TLocalizedText TZoneSpawnName600004 => null;

		public static TLocalizedText TZoneSpawnName610001 => null;

		public static TLocalizedText TZoneSpawnName610002 => null;

		public static TLocalizedText TZoneSpawnName610003 => null;

		public static TLocalizedText TZoneSpawnName610004 => null;

		public static TLocalizedText TZoneSpawnName620001 => null;

		public static TLocalizedText TZoneSpawnName620002 => null;

		public static TLocalizedText TZoneSpawnName620003 => null;

		public static TLocalizedText TZoneSpawnName620004 => null;

		public static TLocalizedText TZoneSpawnName620005 => null;

		public static TLocalizedText TZoneSpawnName631100 => null;

		public static TLocalizedText TZoneSpawnName631101 => null;

		public static TLocalizedText TZoneSpawnName631102 => null;

		public static TLocalizedText TZoneSpawnName631103 => null;

		public static TLocalizedText TZoneSpawnName631104 => null;

		public static TLocalizedText TZoneSpawnName631105 => null;

		public static TLocalizedText TZoneSpawnName631106 => null;

		public static TLocalizedText TZoneSpawnName631107 => null;

		public static TLocalizedText TZoneSpawnName631108 => null;

		public static TLocalizedText TZoneSpawnName631109 => null;

		public static TLocalizedText TZoneSpawnName631200 => null;

		public static TLocalizedText TZoneSpawnName631201 => null;

		public static TLocalizedText TZoneSpawnName631202 => null;

		public static TLocalizedText TZoneSpawnName631203 => null;

		public static TLocalizedText TZoneSpawnName631204 => null;

		public static TLocalizedText TZoneSpawnName631205 => null;

		public static TLocalizedText TZoneSpawnName631206 => null;

		public static TLocalizedText TZoneSpawnName631207 => null;

		public static TLocalizedText TZoneSpawnName631208 => null;

		public static TLocalizedText TZoneSpawnName631209 => null;

		public static TLocalizedText TZoneSpawnName631210 => null;

		public static TLocalizedText TZoneSpawnName631211 => null;

		public static TLocalizedText TZoneSpawnName631212 => null;

		public static TLocalizedText TZoneSpawnName631213 => null;

		public static TLocalizedText TZoneSpawnName631214 => null;

		public static TLocalizedText TZoneSpawnName631215 => null;

		public static TLocalizedText TZoneSpawnName631216 => null;

		public static TLocalizedText TZoneSpawnName632000 => null;

		public static TLocalizedText TZoneSpawnName632001 => null;

		public static TLocalizedText TZoneSpawnName632002 => null;

		public static TLocalizedText TZoneSpawnName632003 => null;

		public static TLocalizedText TZoneSpawnName632004 => null;

		public static TLocalizedText TZoneSpawnName632005 => null;

		public static TLocalizedText TZoneSpawnName632006 => null;

		public static TLocalizedText TZoneSpawnName632007 => null;

		public static TLocalizedText TZoneSpawnName632008 => null;

		public static TLocalizedText TZoneSpawnName632009 => null;

		public static TLocalizedText TZoneSpawnName632010 => null;

		public static TLocalizedText TZoneSpawnName632011 => null;

		public static TLocalizedText TZoneSpawnName632012 => null;

		public static TLocalizedText TZoneSpawnName632013 => null;

		public static TLocalizedText TZoneSpawnName632014 => null;

		public static TLocalizedText TZoneSpawnName632015 => null;

		public static TLocalizedText TZoneSpawnName632016 => null;

		public static TLocalizedText TZoneSpawnName633001 => null;

		public static TLocalizedText TZoneSpawnName633002 => null;

		public static TLocalizedText TZoneSpawnName633003 => null;

		public static TLocalizedText TZoneSpawnName633004 => null;

		public static TLocalizedText TZoneSpawnName633005 => null;

		public static TLocalizedText TZoneSpawnName641101 => null;

		public static TLocalizedText TZoneSpawnName641102 => null;

		public static TLocalizedText TZoneSpawnName641103 => null;

		public static TLocalizedText TZoneSpawnName641104 => null;

		public static TLocalizedText TZoneSpawnName641105 => null;

		public static TLocalizedText TZoneSpawnName641106 => null;

		public static TLocalizedText TZoneSpawnName641107 => null;

		public static TLocalizedText TZoneSpawnName641108 => null;

		public static TLocalizedText TZoneSpawnName641109 => null;

		public static TLocalizedText TZoneSpawnName641110 => null;

		public static TLocalizedText TZoneSpawnName641111 => null;

		public static TLocalizedText TZoneSpawnName641112 => null;

		public static TLocalizedText TZoneSpawnName641113 => null;

		public static TLocalizedText TZoneSpawnName641114 => null;

		public static TLocalizedText TZoneSpawnName641115 => null;

		public static TLocalizedText TZoneSpawnName800000 => null;

		public static TLocalizedText TZoneSpawnName800001 => null;

		public static TLocalizedText TZoneSpawnName800002 => null;

		public static TLocalizedText TZoneSpawnName800003 => null;

		public static TLocalizedText TZoneSpawnName800004 => null;

		public static TLocalizedText TZoneSpawnName800005 => null;

		public static TLocalizedText TZoneSpawnName800006 => null;

		public static TLocalizedText TZoneSpawnName800007 => null;

		public static TLocalizedText TZoneSpawnName800008 => null;

		public static TLocalizedText TZoneSpawnName800009 => null;

		public static TLocalizedText TZoneSpawnName800010 => null;

		public static TLocalizedText TZoneSpawnName800011 => null;

		public static TLocalizedText TZoneSpawnName800012 => null;

		public static TLocalizedText TZoneSpawnName800013 => null;

		public static TLocalizedText TZoneSpawnName801001 => null;

		public static TLocalizedText TZoneSpawnName801002 => null;

		public static TLocalizedText TZoneSpawnName801003 => null;

		public static TLocalizedText TZoneSpawnName801004 => null;

		public static TLocalizedText TZoneSpawnName801005 => null;

		public static TLocalizedText TZoneSpawnName801006 => null;

		public static TLocalizedText TZoneSpawnName801007 => null;

		public static TLocalizedText TZoneSpawnName802001 => null;

		public static TLocalizedText TZoneSpawnName802002 => null;

		public static TLocalizedText TZoneSpawnName802003 => null;

		public static TLocalizedText TZoneSpawnName802004 => null;

		public static TLocalizedText TZoneSpawnName802005 => null;

		public static TLocalizedText TZoneSpawnName802006 => null;

		public static TLocalizedText TZoneSpawnName802007 => null;

		public static TLocalizedText TZoneSpawnName802008 => null;

		public static TLocalizedText TZoneSpawnName802009 => null;

		public static TLocalizedText TZoneSpawnName802010 => null;

		public static TLocalizedText TZoneSpawnName802011 => null;

		public static TLocalizedText TZoneSpawnName802012 => null;

		public static TLocalizedText TZoneSpawnName802013 => null;

		public static TLocalizedText TZoneSpawnName802014 => null;

		public static TLocalizedText TZoneSpawnName802015 => null;

		public static TLocalizedText TZoneSpawnName802201 => null;

		public static TLocalizedText TZoneSpawnName802202 => null;

		public static TLocalizedText TZoneSpawnName802203 => null;

		public static TLocalizedText TZoneSpawnName802204 => null;

		public static TLocalizedText TZoneSpawnName802205 => null;

		public static TLocalizedText TZoneSpawnName802206 => null;

		public static TLocalizedText TZoneSpawnName802207 => null;

		public static TLocalizedText TZoneSpawnName802208 => null;

		public static TLocalizedText TZoneSpawnName802209 => null;

		public static TLocalizedText TZoneSpawnName802210 => null;

		public static TLocalizedText TZoneSpawnName802211 => null;

		public static TLocalizedText TZoneSpawnName802212 => null;

		public static TLocalizedText TZoneSpawnName803001 => null;

		public static TLocalizedText TZoneSpawnName803002 => null;

		public static TLocalizedText TZoneSpawnName803003 => null;

		public static TLocalizedText TZoneSpawnName803004 => null;

		public static TLocalizedText TZoneSpawnName803005 => null;

		public static TLocalizedText TZoneSpawnName803006 => null;

		public static TLocalizedText TZoneSpawnName803007 => null;

		public static TLocalizedText TZoneSpawnName803008 => null;

		public static TLocalizedText TZoneSpawnName803009 => null;

		public static TLocalizedText TZoneSpawnName803010 => null;

		public static TLocalizedText TZoneSpawnName803011 => null;

		public static TLocalizedText TZoneSpawnName803012 => null;

		public static TLocalizedText TZoneSpawnName803013 => null;

		public static TLocalizedText TZoneSpawnName803014 => null;

		public static TLocalizedText TZoneSpawnName803015 => null;

		public static TLocalizedText TZoneSpawnName900011 => null;

		public static TLocalizedText TZoneSpawnName900012 => null;

		public static TLocalizedText TZoneSpawnName900013 => null;

		public static TLocalizedText TZoneSpawnName900021 => null;

		public static TLocalizedText TZoneSpawnName900022 => null;

		public static TLocalizedText TZoneSpawnName900023 => null;

		public static TLocalizedText TZoneSpawnName900031 => null;

		public static TLocalizedText TZoneSpawnName900032 => null;

		public static TLocalizedText TZoneSpawnName900033 => null;

		public static TLocalizedText TZoneSpawnName900101 => null;

		public static TLocalizedText TZoneSpawnName900102 => null;

		public static TLocalizedText TZoneSpawnName900201 => null;

		public static TLocalizedText TZoneSpawnName900202 => null;

		public static TLocalizedText TZoneSpawnName900301 => null;

		public static TLocalizedText TZoneSpawnName900302 => null;

		public static TLocalizedText TZoneSpawnName900401 => null;

		public static TLocalizedText TZoneSpawnName900402 => null;

		public static TLocalizedText TZoneSpawnName900501 => null;

		public static TLocalizedText TZoneSpawnName900502 => null;

		public static TLocalizedText TZoneSpawnName900601 => null;

		public static TLocalizedText TZoneSpawnName900701 => null;

		public static TLocalizedText TZoneSpawnName900801 => null;

		public static TLocalizedText TZoneSpawnName900901 => null;

		public static TLocalizedText TZoneSpawnName901001 => null;

		public static TLocalizedText TZoneSpawnName901002 => null;

		public static TLocalizedText TZoneSpawnName901101 => null;

		public static TLocalizedText TZoneSpawnName901102 => null;

		public static TLocalizedText TZoneSpawnName901201 => null;

		public static TLocalizedText TZoneSpawnName901202 => null;

		public static TLocalizedText TZoneSpawnName901301 => null;

		public static TLocalizedText TZoneSpawnName901302 => null;

		public static TLocalizedText TZoneSpawnName901401 => null;

		public static TLocalizedText TZoneSpawnName901501 => null;

		public static TLocalizedText TZoneSpawnName901601 => null;

		public static TLocalizedText TZoneSpawnName1102011 => null;

		public static TLocalizedText TZoneSpawnName1103011 => null;

		public static TLocalizedText TZoneSpawnName1103012 => null;

		public static TLocalizedText TZoneSpawnName1104011 => null;

		public static TLocalizedText TZoneSpawnName1104012 => null;

		public static TLocalizedText UArtifactDealPanelTItleText => null;

		public static TLocalizedText UCampaignDetailPanelDifficultyCaption => null;

		public static TLocalizedText UCampaignDetailPanelHardTagText => null;

		public static TLocalizedText UCampaignDetailPanelExpertTagText => null;

		public static TLocalizedText UCampaignDetailPanelLevelCaption => null;

		public static TLocalizedText UCampaignDetailPanelRegionCaption => null;

		public static TLocalizedText UCampaignDetailPanelFirstRewardCaption => null;

		public static TLocalizedText UCampaignDetailPanelFirstRewardDesc => null;

		public static TLocalizedText UCraftingItemSlotAvailableText => null;

		public static TLocalizedText UParadoxCleanseSlotCostCaption => null;

		public static TLocalizedText UParadoxCleanseSlotCostLackCost => null;

		public static TLocalizedText UCharacterInfoPanelAtkCaption => null;

		public static TLocalizedText UCharacterInfoPanelDefCaption => null;

		public static TLocalizedText UCharacterInfoPanelHpCaption => null;

		public static TLocalizedText UCharacterInfoPanelStaminaCaption => null;

		public static TLocalizedText UCharacterInfoPanelCriticalChanceCaption => null;

		public static TLocalizedText UCharacterInfoPanelCriticalDamageBoostCaption => null;

		public static TLocalizedText UCharacterInfoPanelAttackSpeedCaption => null;

		public static TLocalizedText UCharacterInfoPanelDefensePenetrationCaption => null;

		public static TLocalizedText UCharacterInfoPanelCooldownReductionCaption => null;

		public static TLocalizedText UCharacterInfoPanelNaturalRecoveryHpCaption => null;

		public static TLocalizedText UCharacterInfoPanelBackAttackDamageBoostCaption => null;

		public static TLocalizedText UCharacterInfoPanelFireResistanceCaption => null;

		public static TLocalizedText UCharacterInfoPanelWaterResistanceCaption => null;

		public static TLocalizedText UCharacterInfoPanelWindResistanceCaption => null;

		public static TLocalizedText UCharacterStatusPanelCaption => null;

		public static TLocalizedText UCharacterStatusPanelAtkCaption => null;

		public static TLocalizedText UCharacterStatusPanelDefCaption => null;

		public static TLocalizedText UCharacterStatusPanelMoveSpeedCaption => null;

		public static TLocalizedText UCharacterStatusPanelAtkSpeedCaption => null;

		public static TLocalizedText UCharacterStatusPanelHpCaption => null;

		public static TLocalizedText UCharacterStatusPanelHpRegenCaption => null;

		public static TLocalizedText UCharacterStatusPanelHpRecoveryAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelHpDamageAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelCriticalChancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelCriticalDamagePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelStressRecoveryAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelStressDamageAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelBrightnessRecoveryAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelBrightnessDamageAmplifierCaption => null;

		public static TLocalizedText UCharacterStatusPanelWeaknessDamagePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelSlashResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelStrikeResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelProjectileResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelFireResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelWaterResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelWindResistancePercentCaption => null;

		public static TLocalizedText UCharacterStatusPanelPenetrationCaption => null;

		public static TLocalizedText UCharacterStatusPanelPenetrationPercentCaption => null;

		public static TLocalizedText UEquipmentStashPanelTItleCaption => null;

		public static TLocalizedText UKeyInteractionPanelTopCaption => null;

		public static TLocalizedText UKeyInteractionPanelNormalText => null;

		public static TLocalizedText UKeyInteractionPanelDisabledText => null;

		public static TLocalizedText UParadoxPanelTopTitleText => null;

		public static TLocalizedText UParadoxPanelNoParadoxText => null;

		public static TLocalizedText UMaterialPanelBodyCaption => null;

		public static TLocalizedText UMaterialPanelNeedCurrencyCaption => null;

		public static TLocalizedText UMaterialPanelGuideText => null;

		public static TLocalizedText UQuickSlotSettingPanelTopCaption => null;

		public static TLocalizedText UCharacterTalentTooltipActiveText => null;

		public static TLocalizedText UCharacterTalentTooltipCaptionText => null;

		public static TLocalizedText UItemTooltipSuggestPriceCaption => null;

		public static TLocalizedText UItemTooltipArtifactPriceCaption => null;

		public static TLocalizedText UItemTooltipPriceCaption => null;

		public static TLocalizedText UItemTooltipInventoryAmountCaption => null;

		public static TLocalizedText UItemTooltipStashAmountCaption => null;

		public static TLocalizedText UEquipmentTooltipAttackCaption => null;

		public static TLocalizedText UEquipmentTooltipElementalCaption => null;

		public static TLocalizedText UEquipmentTooltipDurabilityCaption => null;

		public static TLocalizedText UInGameKeyGuidePanelControlGuideDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelAttackDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelDashDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelRunDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelMiniMapDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelInventoryDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelEmojiDesc => null;

		public static TLocalizedText UInGameKeyGuidePanelPingDesc => null;

		public static TLocalizedText UInGameUserLevelPanelLevelUpTagText => null;

		public static TLocalizedText UInGameWatchingPanelObservingText => null;

		public static TLocalizedText UInGameWatchingPanelKeyGuideTextDesc => null;

		public static TLocalizedText UStageResultPlayerInfoEscapeSuccessText => null;

		public static TLocalizedText UStageResultPlayerInfoEscapeFailText => null;

		public static TLocalizedText UInventoryPanelArtifactFilterEmptyText => null;

		public static TLocalizedText USkillTalentPageSkillCaptionText => null;

		public static TLocalizedText USkillTalentPageTalentCaptionText => null;

		public static TLocalizedText UAfterCreateRoomPopupBodyTitle => null;

		public static TLocalizedText UAfterCreateRoomPopupAddressCodeCaption => null;

		public static TLocalizedText UAfterCreateRoomPopupBodyDesc => null;

		public static TLocalizedText UCampaignFailPopupBodyTitle => null;

		public static TLocalizedText UCampaignGiveUpVotePopupBodyMainText => null;

		public static TLocalizedText UCampaignGiveUpVotePopupBodySubText => null;

		public static TLocalizedText ULevelUpPopupRootDesc => null;

		public static TLocalizedText ULostShopTitleText => null;

		public static TLocalizedText ULostShopNoItem => null;

		public static TLocalizedText UParadoxConfirmPopupBodyMessage => null;

		public static TLocalizedText UParadoxConfirmPopupCostCaption => null;

		public static TLocalizedText UParadoxConfirmPopupLockCostCaption => null;

		public static TLocalizedText UTechTreePopupTitleGroupText => null;

		public static TLocalizedText UTechTreePopupLmbText => null;

		public static TLocalizedText UTechTreePopupLevelUpTagText => null;

		public static TLocalizedText UEscapeFailPopupBodyTitle => null;

		public static TLocalizedText UEscapeSuccessPopupBodyTitle => null;

		public static TLocalizedText UCreateRoomPopupBodyTextTitle => null;

		public static TLocalizedText UCreateRoomPopupCheckBoxPrivateRoomText => null;

		public static TLocalizedText UTechTreeConfirmPopupBodyMessage => null;

		public static TLocalizedText UGameRoomListPopupNetworkPingCaption => null;

		public static TLocalizedText UGameRoomListPopupHostLevelCaption => null;

		public static TLocalizedText UGameRoomListPopupCapacityCaption => null;

		public static TLocalizedText URoomListPopupDifficultyCaption => null;

		public static TLocalizedText URoomListPopupTitleCaption => null;

		public static TLocalizedText ULocalCreateRoomPopupBodyTextTitle => null;

		public static TLocalizedText ULocalCreateRoomPopupAddressIpText => null;

		public static TLocalizedText ULocalCreateRoomPopupAddressPortText => null;

		public static TLocalizedText ULocalCreateRoomPopupBodyTextDesc => null;

		public static TLocalizedText ULocalConnectRoomPopupBodyTextTitle => null;

		public static TLocalizedText ULocalConnectRoomPopupBodyTextDesc => null;

		public static TLocalizedText ULocalConnectRoomPopupAddressIpText => null;

		public static TLocalizedText ULocalConnectRoomPopupAddressPortText => null;

		public static TLocalizedText UChangeUserNamePopupBodyMainText => null;

		public static TLocalizedText UChangeUserNamePopupTextAreaUserNamePlaceholder => null;

		public static TLocalizedText USkillModeSelectPopupBodyMessage => null;

		public static TLocalizedText UItemCraftingConfirmPopupBodyTitle => null;

		public static TLocalizedText USteamConnectRoomPopupBodyTextTitle => null;

		public static TLocalizedText USteamConnectRoomPopupBodyTextDesc => null;

		public static TLocalizedText URestVotePopupBodyMainText => null;

		public static TLocalizedText URestVotePopupBodySubText => null;

		public static TLocalizedText UCampaignVotePopupHardTagText => null;

		public static TLocalizedText UCampaignVotePopupExpertTagText => null;

		public static TLocalizedText UCampaignVotePopupQuestCaption => null;

		public static TLocalizedText UCampaignVotePopupCollectibleGoodsCaption => null;

		public static TLocalizedText UCampaignVotePopupVoteTitleText => null;

		public static TLocalizedText UCampaignVotePopupRewardCaption => null;

		public static TLocalizedText UCampaignVotePopupAnomalyCaption => null;

		public static TLocalizedText UKeyInteractionPopupLmbText => null;

		public static TLocalizedText UKeyInteractionPopupRightDesc => null;

		public static TLocalizedText USkillSelectPopupTitleText => null;

		public static TLocalizedText USkillSelectPopupLmbText => null;

		public static TLocalizedText UCharacterSettingPopupTalentSettingListDescEmpty => null;

		public static TLocalizedText UCharacterTalentResetPopupBodyDesc => null;

		public static TLocalizedText UCharacterTalentResetPopupBodyCaption => null;

		public static TLocalizedText UCharacterTalentPopupTitleText => null;

		public static TLocalizedText UCharacterTalentPopupLmbText => null;

		public static TLocalizedText UCharacterTalentPopupNormalText => null;

		public static TLocalizedText UItemStashPopupTitleGroupCaption => null;

		public static TLocalizedText UItemStashPopupTopQuickSlotCaption => null;

		public static TLocalizedText UItemCraftingPopupTitleText => null;

		public static TLocalizedText UItemCraftingPopupAvailableText => null;

		public static TLocalizedText UEquipmentStashPopupTitleText => null;

		public static TLocalizedText UItemShopPopupTitleCaption => null;

		public static TLocalizedText UInGameMenuPopupLoungeCodeTitleCaption => null;

		public static TLocalizedText UStageResultQuestSlotTaskTitle => null;

		public static TLocalizedText UStageResultQuestSlotComplete => null;

		public static TLocalizedText UStageResultPopupTitleTitleText => null;

		public static TLocalizedText UStageResultPopupRequestDeadlineDeadlineText => null;

		public static TLocalizedText UStageResultPopupHorizGroupQuestObjectivesText => null;

		public static TLocalizedText UStageResultPopupHorizGroupMainLootText => null;

		public static TLocalizedText UStageResultPopupHorizGroupLostGoodsText => null;

		public static TLocalizedText UStageResultPopupHorizGroupExpText => null;

		public static TLocalizedText UStageResultPopupBottomPressAnyKey => null;

		public static TLocalizedText UCampaignCompletePopupExpClearTitle => null;

		public static TLocalizedText UCampaignCompletePopupExpBonusFormat => null;

		public static TLocalizedText UCampaignDetailPanelNormalTextTitle => null;

		public static TLocalizedText UOptionPageLanguageCaption => null;

		public static TLocalizedText UOptionPageScreenModeCaption => null;

		public static TLocalizedText UOptionPageMasterVolumeCaption => null;

		public static TLocalizedText UOptionPageBgmVolumeCaption => null;

		public static TLocalizedText UOptionPageSfxVolumeCaption => null;

		public static TLocalizedText UOptionPageFpsCaption => null;

		public static TLocalizedText UOptionPageVfxCaption => null;

		public static TLocalizedText UOptionPageQualityCaption => null;

		public static TLocalizedText UGameLoadingPopupLayoutLoadingText => null;

		public static TLocalizedText UMenuBuffPanelTopTitle => null;

		public static TLocalizedText UMenuBuffPanelNoBuff => null;

		public static TLocalizedText UChangeUserNamePopupBodySubText => null;

		public static TLocalizedText UMoveTutorialGuideText => null;

		public static TLocalizedText ULmcTutorialGuideText => null;

		public static TLocalizedText URmcTutorialGuideText => null;

		public static TLocalizedText UqTutorialGuideText => null;

		public static TLocalizedText UfTutorialGuideTextDoor => null;

		public static TLocalizedText URunTutorialGuideTextRun => null;

		public static TLocalizedText URunTutorialGuideTextDash => null;

		public static TLocalizedText UItemTutorialGuideText => null;

		public static TLocalizedText UItemUseTutorialGuideText => null;

		public static TLocalizedText UOptionPageGammaCaption => null;

		public static TLocalizedText UCampaignDetailPanelEndGaugeCaption => null;

		public static TLocalizedText UCampaignDetailPanelQuestCaption => null;

		public static TLocalizedText UCampaignDetailPanelFailConditionCaption => null;

		public static TLocalizedText UCampaignDetailPanelMonsterCaption => null;

		public static TLocalizedText UCampaignDetailPanelCollectibleGoodsCaption => null;

		public static TLocalizedText UCampaignDetailPanelRewardCaption => null;

		public static TLocalizedText UCampaignDetailPanelLayoutTextDesc => null;

		public static TLocalizedText UQuickSlotSettingPanelTopGuideText => null;

		public static TLocalizedText UQuickSlotSettingPanelTopPadGuideText => null;

		public static TLocalizedText UCharacterInfoPanelOnText => null;

		public static TLocalizedText UCharacterInfoPanelOffText => null;

		public static TLocalizedText UStageResultPlayerExpSlotExpMonsterKillTitle => null;

		public static TLocalizedText UStageResultPlayerExpSlotExpRewardBoxTitle => null;

		public static TLocalizedText UStageResultPlayerExpSlotExpEscapeTitle => null;

		public static TLocalizedText UStageResultPlayerExpSlotExpDifficultyTitle => null;

		public static TLocalizedText UStageResultPlayerExpSlotExpTalentTitle => null;

		public static TLocalizedText USquadPageParticipationText => null;

		public static TLocalizedText USquadPageParticipatingText => null;

		public static TLocalizedText USquadPageSettingText => null;

		public static TLocalizedText USquadPageLoungeOpenText => null;

		public static TLocalizedText ULoungeFilterPopupBodyTextTitle => null;

		public static TLocalizedText ULoungeFilterPopupShowPlayingLoungeCaption => null;

		public static TLocalizedText ULoungeFilterPopupBlockOtherAreaCaption => null;

		public static TLocalizedText ULoungeFilterPopupPingCaption => null;

		public static TLocalizedText ULoungeFilterPopupPingGood => null;

		public static TLocalizedText ULoungeFilterPopupPingNormal => null;

		public static TLocalizedText ULoungeFilterPopupPingAll => null;

		public static TLocalizedText ULoungeFilterPopupLoungeDifficultyCaption => null;

		public static TLocalizedText ULoungeFilterPopupLoungeDifficultyStory => null;

		public static TLocalizedText ULoungeFilterPopupLoungeDifficultyJunior => null;

		public static TLocalizedText ULoungeFilterPopupLoungeDifficultyClassic => null;

		public static TLocalizedText ULoungeFilterPopupLoungeDifficultyVeteran => null;

		public static TLocalizedText ULoungeFilterPopupMinHostLevelCaption => null;

		public static TLocalizedText UStoragePopupTitleText => null;

		public static TLocalizedText UStoragePopupDescDescText => null;

		public static TLocalizedText UInGameMiniMapPanelQuestText => null;

		public static TLocalizedText UInGameMiniMapPanelMissionText => null;

		public static TLocalizedText UInGameMiniMapPanelBossText => null;

		public static TLocalizedText UInGameMiniMapPanelExitText => null;

		public static TLocalizedText UInGameMiniMapPanelNpcText => null;

		public static TLocalizedText UInGameMiniMapPanelRewardBoxText => null;

		public static TLocalizedText UInGameMiniMapPanelDeadBodyText => null;

		public static TLocalizedText UInGameMiniMapPanelPortalText => null;

		public static TLocalizedText UInGameMiniMapPanelSanctuaryText => null;

		public static TLocalizedText UInGameMiniMapPanelPlayerMarkerText => null;

		public static TLocalizedText UParadoxLockPopupContentsEmpty => null;

		public static TLocalizedText UParadoxLockPopupContentsDesc => null;

		public static TLocalizedText UParadoxLockPopupTitleText => null;

		public static TLocalizedText UParadoxPurifyPopupContentsEmpty => null;

		public static TLocalizedText UParadoxPurifyPopupContentsDesc => null;

		public static TLocalizedText UParadoxPurifyPopupTitleText => null;

		public static TLocalizedText URoomCreationPopupTitleText => null;

		public static TLocalizedText URoomCreationPopupLoungeInfoDesc => null;

		public static TLocalizedText UHelpPopupPage1 => null;

		public static TLocalizedText UHelpPopupOrderingTitle => null;

		public static TLocalizedText UHelpPopupOrderingDesc => null;

		public static TLocalizedText UHelpPopupDiveTitle => null;

		public static TLocalizedText UHelpPopupDiveDesc => null;

		public static TLocalizedText UHelpPopupSellTitle => null;

		public static TLocalizedText UHelpPopupSellDesc => null;

		public static TLocalizedText UHelpPopupPage2 => null;

		public static TLocalizedText UHelpPopupHpGaugeTitle => null;

		public static TLocalizedText UHelpPopupHpGaugeDesc => null;

		public static TLocalizedText UHelpPopupStressGaugeTitle => null;

		public static TLocalizedText UHelpPopupStressGaugeDesc => null;

		public static TLocalizedText UHelpPopupLightFuelTitle => null;

		public static TLocalizedText UHelpPopupLightFuelDesc => null;

		public static TLocalizedText UHelpPopupPage3 => null;

		public static TLocalizedText UHelpPopupArtifactCorruptionTitle => null;

		public static TLocalizedText UHelpPopupArtifactCorruptionDesc => null;

		public static TLocalizedText UHelpPopupBattleTitle => null;

		public static TLocalizedText UHelpPopupBattleDesc => null;

		public static TLocalizedText UHelpPopupAvoidTitle => null;

		public static TLocalizedText UHelpPopupAvoidDesc => null;

		public static TLocalizedText UHelpPopupPage4 => null;

		public static TLocalizedText UHelpPopupEscapeTitle => null;

		public static TLocalizedText UHelpPopupEscapeDesc => null;

		public static TLocalizedText UHelpPopupFailTitle => null;

		public static TLocalizedText UHelpPopupFailDesc => null;

		public static TLocalizedText UHelpPopupPage5 => null;

		public static TLocalizedText UHelpPopupParadoxTitle => null;

		public static TLocalizedText UHelpPopupParadoxDesc => null;

		public static TLocalizedText UHelpPopupCraftingTitle => null;

		public static TLocalizedText UHelpPopupCraftingDesc => null;

		public static TLocalizedText UHelpPopupTitle => null;

		public static TLocalizedText UInGameLoungeQuestPanelTitleMainText => null;

		public static TLocalizedText UInGameLoungeQuestPanelTitleSubText => null;

		public static TLocalizedText UNewContentPopupTopTextTitle => null;

		public static TLocalizedText UInGameChatPanelTextAreaPlaceholder => null;

		public static TLocalizedText UCoopPlayerInfo => null;

		public static TLocalizedText UtechDevPossible => null;

		public static TLocalizedText ULoungeListTitle => null;

		public static TLocalizedText EGoodsSubTypeValuable => null;

		public static TLocalizedText RiskLevelSelected => null;

		public static TLocalizedText UQuestInfoPanelInfoGoalCondition => null;

		public static TLocalizedText UQuestInfoPanelRewardCaption => null;

		public static TLocalizedText DisplayScreen => null;

		public static TLocalizedText ShowDialog => null;

		public static TLocalizedText DialogView => null;

		public static TLocalizedText UQuestPageTitleTitleSub => null;

		public static TLocalizedText UQuestPageEmptyTextEmpty => null;

		public static TLocalizedText UQuestPageTopTitle => null;

		public static TLocalizedText UArchivePageTitleText => null;

		public static TLocalizedText UArchivePageListTitleText => null;

		public static TLocalizedText UArchivePageEmptyTextDesc => null;

		public static TLocalizedText UArtifactDealPopupKeyGuideConfitmText => null;

		public static TLocalizedText UArtifactDealPopupKeyGuideOtherText => null;

		public static TLocalizedText UArtifactDealPopupNormalGiveupText => null;

		public static TLocalizedText UArchiveGlossaryPopupEmptyTextEmpty => null;

		public static TLocalizedText UArchiveMonsterPopupEmptyTextEmpty => null;

		public static TLocalizedText UArchiveGoodsPopupEmptyTextEmpty => null;

		public static TLocalizedText UUnlockMaterialPanelLevelCaption => null;

		public static TLocalizedText UTalentTooltipUnlockableText => null;

		public static TLocalizedText UTalentTooltipUnlockedText => null;

		public static TLocalizedText UTalentTooltipLockedText => null;

		public static TLocalizedText ShowNote => null;

		public static TLocalizedText UWeaponRepairPanelTopCaption => null;

		public static TLocalizedText UWeaponRepairPanelDisabledText => null;

		public static TLocalizedText UWeaponRepairPopupTitleText => null;

		public static TLocalizedText UWeaponRepairPopupDisabledText => null;

		public static TLocalizedText UWeaponRepairPopupRightDesc => null;

		public static TLocalizedText URepairConfirmPopupTitleSubText => null;

		public static TLocalizedText URepairConfirmPopupCenterDescDurabilityText => null;

		public static TLocalizedText URepairConfirmPopupCenterDescText => null;

		public static TLocalizedText URepairConfirmPopupPriceText => null;

		public static TLocalizedText URepairConfirmPopupDurabilityText => null;

		public static TLocalizedText UInteriorPopupTitleText => null;

		public static TLocalizedText UInteriorPopupTitleSectorText => null;

		public static TLocalizedText UInteriorPopupListTitleInteriorText => null;

		public static TLocalizedText UScrapPanelGuideText => null;

		public static TLocalizedText UScrapPanelInvalidText => null;

		public static TLocalizedText UScrapPanelTopStartButton => null;

		public static TLocalizedText UScrapPanelTopTargetCaption => null;

		public static TLocalizedText UScrapPanelTopResultCaption => null;

		public static TLocalizedText UScrapPopupSelectText => null;

		public static TLocalizedText UScrapPopupQuickSubmitText => null;

		public static TLocalizedText UScrapPopupTitleText => null;

		public static TLocalizedText UBlueprintPanelTopTitleCaption => null;

		public static TLocalizedText UBlueprintPanelGuideText => null;

		public static TLocalizedText UBlueprintPanelInvalidText => null;

		public static TLocalizedText URecordBlueprintPopupTitleTitleText => null;

		public static TLocalizedText URecordBlueprintPopupSelectText => null;

		public static TLocalizedText UEmployeeSlotCharmTitle => null;

		public static TLocalizedText UEmployeeSlotPersuasionTitle => null;

		public static TLocalizedText UEmployeeSlotResistanceTitle => null;

		public static TLocalizedText UEmployeeSlotNormalTextPurify => null;

		public static TLocalizedText UEmployeeSlotNormalTextFire => null;

		public static TLocalizedText UTycoonHireConfirmPopupTitleText => null;

		public static TLocalizedText UTycoonCancelConfirmPopupTitleText => null;

		public static TLocalizedText UTycoonCancelConfirmPopupBodyDesc => null;

		public static TLocalizedText UTycoonCancelConfirmPopupBodyWarning => null;

		public static TLocalizedText UTycoonSellComplete => null;

		public static TLocalizedText UEmployeeChangeConfirmPopupTitleText => null;

		public static TLocalizedText UEmployeeChangeConfirmPopupArrowText => null;

		public static TLocalizedText UEmployeeFirePopupTitleTextMain => null;

		public static TLocalizedText UCharacterPageParadoxAndBuffText => null;

		public static TLocalizedText UCharacterPageTalaneAndSkilllText => null;

		public static TLocalizedText UCharacterPageSkilllText => null;

		public static TLocalizedText UTycoonMainPageTextBubbleCompleteText => null;

		public static TLocalizedText UTycoonMainPageTextBubbleEmptyText => null;

		public static TLocalizedText UTycoonMainPageBtnSetArtifactText => null;

		public static TLocalizedText UTycoonMainPageBtnCancelArtifactText => null;

		public static TLocalizedText UTycoonMainPageBtnSettingEmployeeText => null;

		public static TLocalizedText UTycoonMainPageBtnChangeEmployeeText => null;

		public static TLocalizedText UTycoonMainPageBtnStartText => null;

		public static TLocalizedText UTycoonMainPageServingNowText => null;

		public static TLocalizedText UTycoonMainPageServingNowTextDesc => null;

		public static TLocalizedText UTycoonMainPageBtnResultText => null;

		public static TLocalizedText UTycoonMenuPopupTycoonMainTabText => null;

		public static TLocalizedText UTycoonMenuPopupEmployeeInfoTabText => null;

		public static TLocalizedText UTycoonMenuPopupEmployeeHireTabText => null;

		public static TLocalizedText UiTycoonEmployeeList => null;

		public static TLocalizedText UiTycoonEmployeeListPlacement => null;

		public static TLocalizedText UiTycoonEmployeeListPanic => null;

		public static TLocalizedText UiTycoonRecruitmentList => null;

		public static TLocalizedText UCoinExchangePopupTitleText => null;

		public static TLocalizedText UCoinExchangePopupRegistrationBtn => null;

		public static TLocalizedText UCoinExchangePopupRegistrationText => null;

		public static TLocalizedText UCoinExchangePopupIssuanceBtn => null;

		public static TLocalizedText UCoinExchangePopupIssuanceText => null;

		public static TLocalizedText USkillSelectInfoSlotActiveOtherSlotText => null;

		public static TLocalizedText USkillSelectInfoSlotActiveSlotText => null;

		public static TLocalizedText USkillSelectPopupSkillTitleSelectSlotGuide => null;

		public static TLocalizedText USkillSelectPopupSlotTitleScrollerGuideText => null;

		public static TLocalizedText USkillSelectPopupSlotEmptyEmptySelectSkillText => null;

		public static TLocalizedText USquadPageNotJoinedPanelInfoText => null;

		public static TLocalizedText USquadPageNotJoinedPanelTextDesc => null;

		public static TLocalizedText UPlayerRestoreSlotNormalCaption => null;

		public static TLocalizedText URestorePlayerPopupTopTitle => null;

		public static TLocalizedText URestorePlayerPopupTitleTitleSub => null;

		public static TLocalizedText UCharacterInfoPanelMoveSpeedCaption => null;

		public static TLocalizedText UCharacterInfoPanelNaturalRecoverStaminaCaption => null;

		public static TLocalizedText UCharacterInfoPanelStressDamageResistanceCaption => null;

		public static TLocalizedText UiMarker => null;

		public static TLocalizedText ULostItemListPopupContentsTextDesc => null;

		public static TLocalizedText UItemTooltipUsageCaption => null;

		public static TLocalizedText UInteriorPopupEmptyTextDesc => null;

		public static TLocalizedText UTycoonResultPopupArtifactFailDesc => null;

		public static TLocalizedText UKeySettingSlotSelectKeyLayoutText => null;

		public static TLocalizedText UOptionGraphicPanelScreenCaption => null;

		public static TLocalizedText UOptionGraphicPanelGraphicsQualityCaption => null;

		public static TLocalizedText UOptionGraphicPanelVSyncCaption => null;

		public static TLocalizedText UOptionGraphicPanelFpsCaption => null;

		public static TLocalizedText UOptionGraphicPanelGammaCaption => null;

		public static TLocalizedText UOptionGraphicPanelLayoutTextTitle => null;

		public static TLocalizedText UOptionGraphicPanelCameraShakingCaption => null;

		public static TLocalizedText UOptionGraphicPanelResolutionCaption => null;

		public static TLocalizedText UOptionGamePlayPanelAimSupportCaption => null;

		public static TLocalizedText UOptionGamePlayPanelLanguageCaption => null;

		public static TLocalizedText UOptionGamePlayPanelRstickAttackCaption => null;

		public static TLocalizedText UOptionGamePlayPanelSightLineCaption => null;

		public static TLocalizedText UOptionGamePlayPanelLockMouseCaption => null;

		public static TLocalizedText UOptionGamePlayPanelAutoAttackCaption => null;

		public static TLocalizedText UOptionGamePlayPanelRunCaption => null;

		public static TLocalizedText UOptionSoundPanelTotalVolumeCaption => null;

		public static TLocalizedText UOptionSoundPanelBgmCaption => null;

		public static TLocalizedText UOptionSoundPanelSfxCaption => null;

		public static TLocalizedText USalesSlotDisabledDisableText => null;

		public static TLocalizedText UOptionGamePlayPanelTitle => null;

		public static TLocalizedText UGammaPopupContentsTextTitle => null;

		public static TLocalizedText UGammaPopupContentsTextDesc => null;

		public static TLocalizedText UTycoonMainPageContentTextBreak => null;

		public static TLocalizedText UTycoonMainPageTextBreakTextBreakDesc => null;

		public static TLocalizedText UArtifactShopPimDesc => null;

		public static TLocalizedText USelectPlayerPopupTitle => null;

		public static TLocalizedText USelectPlayerPopupDesc => null;

		public static TLocalizedText USelectPlayerPopupRestoreBtnText => null;

		public static TLocalizedText UCraftingPopupMenufacturable => null;

		public static TLocalizedText UTycoonResultPopupResultEmployeeStressTitleText => null;

		public static TLocalizedText UTycoonResultPopupResultEmployeeExpTitleText => null;

		public static TLocalizedText UCampaignDetailPanelAnomalyCaption => null;

		public static TLocalizedText UTalentInfoPanelNormalText => null;

		public static TLocalizedText UArtifactDealBackOutPopupTitleTextDeco => null;

		public static TLocalizedText UArtifactDealBackOutPopupTitleTextTitle => null;

		public static TLocalizedText UArtifactDealBackOutPopupTitleTextDesc => null;

		public static TLocalizedText UArtifactDealCompletePopupExpText => null;

		public static TLocalizedText UArtifactDealCompletePopupGoldText => null;

		public static TLocalizedText UArtifactDealCompletePopupTitleTextDeco => null;

		public static TLocalizedText UArtifactDealCompletePopupTitleTextTitle => null;

		public static TLocalizedText UArtifactDealCompletePopupTitleTextDesc => null;

		public static TLocalizedText UArtifactDealPopupDealRemainPlayCount => null;

		public static TLocalizedText UArtifactDealPopupNormalTextTitle => null;

		public static TLocalizedText UArtifactDealPopupNormalTextSuccess => null;

		public static TLocalizedText UArtifactDealPopupNormalTextFail => null;

		public static TLocalizedText URegisterItemUsedPopupSelectText => null;

		public static TLocalizedText URegisterItemUsedPanelNormalText => null;

		public static TLocalizedText URegisterItemUsedPanelGuideText => null;

		public static TLocalizedText URegisterItemUsedPanelInvalidText => null;

		public static TLocalizedText UArtifactDealExpText => null;

		public static TLocalizedText UArtifactDealCoinText => null;

		public static TLocalizedText USkillSelectPopupSlotLockedLockedSelectSkillText => null;

		public static TLocalizedText USkillSelectPopupSlotEmptyEquipmentLockedSelectSkillText => null;

		public static TLocalizedText UOptionGamePlayPanelPadSensitivityCaption => null;

		public static TLocalizedText UHelpPopupVipDealPageTitle => null;

		public static TLocalizedText UHelpPopupVipTitle => null;

		public static TLocalizedText UHelpPopupVipDesc => null;

		public static TLocalizedText UHelpPopupSugestTitle => null;

		public static TLocalizedText UHelpPopupSugestDesc => null;

		public static TLocalizedText UHelpPopupDealTitle => null;

		public static TLocalizedText UHelpPopupDealDesc => null;

		public static TLocalizedText UHelpPopupEmployeeTitle1 => null;

		public static TLocalizedText UHelpPopupHireEmployeeTitle => null;

		public static TLocalizedText UHelpPopupHireEmployeeDesc => null;

		public static TLocalizedText UHelpPopupArtifactDisplayTitle => null;

		public static TLocalizedText UHelpPopupArtifactDisplayDesc => null;

		public static TLocalizedText UHelpPopupEmployeeTitle2 => null;

		public static TLocalizedText UHelpPopupSaleEmployeeTitle => null;

		public static TLocalizedText UHelpPopupSaleEmployeeDesc => null;

		public static TLocalizedText UHelpPopupManagementEmployeeTitle => null;

		public static TLocalizedText UHelpPopupManagementEmployeeDesc => null;

		public static TLocalizedText UHelpPopupLoungeDecoPageTitle => null;

		public static TLocalizedText UHelpPopupInteriorTitle => null;

		public static TLocalizedText UHelpPopupInteriorDesc => null;

		public static TLocalizedText UHelpPopupFacilitiesTitle => null;

		public static TLocalizedText UHelpPopupFacilitiesDesc => null;

		public static TLocalizedText UCraftingConfirmPopupQuantityNotice => null;

		public static TLocalizedText UCharacterInfoPanelDamageReductionCaption => null;

		public static TLocalizedText UCharacterInfoPanelWeaknessDamageBoostCaption => null;

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

		public string Ko { get; set; }

		public string En { get; set; }

		public string Jp { get; set; }

		public string Tw { get; set; }

		public string Cn { get; set; }

		public string Ru { get; set; }

		public string Br { get; set; }

		public string De { get; set; }

		public string Fr { get; set; }

		public string Es { get; set; }

		public string It { get; set; }

		public string Pt { get; set; }

		public string Pl { get; set; }

		public string Vi { get; set; }

		public string Tr { get; set; }

		public Action<ITableData<string>> OnDataLoaded { get; set; }

		public string LocalText => null;

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
			Ko = TableParse.Str(columns[1]);
			En = TableParse.Str(columns[2]);
			Jp = TableParse.Str(columns[3]);
			Tw = TableParse.Str(columns[4]);
			Cn = TableParse.Str(columns[5]);
			Ru = TableParse.Str(columns[6]);
			Br = TableParse.Str(columns[7]);
			De = TableParse.Str(columns[8]);
			Fr = TableParse.Str(columns[9]);
			Es = TableParse.Str(columns[10]);
			It = TableParse.Str(columns[11]);
			Pt = TableParse.Str(columns[12]);
			Pl = TableParse.Str(columns[13]);
			Vi = TableParse.Str(columns[14]);
			Tr = TableParse.Str(columns[15]);
		}

		public string ToCsv()
		{
			return TableParse.ToCsv(Id, Ko, En, Jp, Tw, Cn, Ru, Br, De, Fr, Es, It, Pt, Pl, Vi, Tr);
		}

		public static void SaveCsv(string filePath)
		{
			global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\n" + GetTypesCsv() + "\n" + Table.ToCsv());
		}

		public static string GetColumnsCsv()
		{
			return "Id,Ko,En,Jp,Tw,Cn,Ru,Br,De,Fr,Es,It,Pt,Pl,Vi,Tr";
		}

		public static string GetTypesCsv()
		{
			return "string,string,string,string,string,string,string,string,string,string,string,string,string,string,string,string";
		}

		public static void LoadFromPath(string filePath)
		{
			LoadFromText(global::System.IO.File.ReadAllText(filePath));
		}

		public static void LoadFromText(string text)
		{
			var table = new LocalizedTextTable();
			table.Load(new CsvTableLoader<string, TLocalizedText>(text));
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
