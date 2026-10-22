using System.Collections.Generic;
using De.InGame.Fsm;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Popup;
using UniRx;
using VContainer;

public class CharacterController : UnitController
{
	public class Args
	{
		public ulong PlayerClientId;

		public int HitLayer;
	}

	private Args _args;

	[Inject]
	private GameWaveManager _waveManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameCutsceneManager _gameCutsceneManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[Inject]
	private GameChatManager _chatManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameShopManager _shopManager;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameFloatingTextManager _floatingTextManager;

	private CharacterStateMachine _characterStateMachine;

	private OutlineObject _outlineObject;

	private CompositeDisposable _stressStateDisposable;

	private float _stressUpdateRemainTime;

	private float _corruptionStressUpdateRemainTime;

	private EStressState _currentStressDebuffState;

	private float _stressDebuffStartTime;

	private float _nextStressDebuffUpdateTime;

	private int _shadowSpawnRemainCount;

	private float _shadowSpawnRemainTime;

	private float _shadowSpawnRemainDelayTime;

	private bool _stressBuffToastDisplayed;

	private float _brightnessElapsedUpdateTime;

	private float _lastStaminaDecreasedTime;

	[Inject]
	private GameObjectManager _gameObjectManager;

	[Inject]
	private GameMissionManager _missionManager;

	private const int PointMarkerLimit = 9;

	private float _lastHpDamageTime;

	private int _accumulatedComboDamage;

	private float _recoveryOnCriticalCoolEndTime;

	protected override UnitStateMachine StateMachine => null;

	private CharacterView CharacterView => null;

	private CharacterModel CharacterModel => null;

	private PlayerInventory PlayerInventory => null;

	private GamePlayer Player => null;

	public override ulong ClientId => 0uL;

	private bool IsMine => false;

	private int CharacterId => 0;

	private OutlineObject OutlineObject => null;

	private float TimeNow => 0f;

	public bool IsGamePlaying => false;

	public Subject<int> OnShowEmoji { get; }

	public Subject<string> OnShowSelfReviveText { get; }

	public void Init(Args args)
	{
	}

	public override void OnHostSpawn()
	{
	}

	protected override void Start()
	{
	}

	public override void OnDestroy()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	private void InitPlayer(GamePlayer player)
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	protected override void FixedUpdate()
	{
	}

	protected override void OnStageStarted()
	{
	}

	private void SynchronizeGamePlayer()
	{
	}

	private void UpdateStress()
	{
	}

	private void UpdateLightFuel()
	{
	}

	private void UpdateStamina()
	{
	}

	private void OnLightOnChanged(bool isLightOn)
	{
	}

	private void OnLightFuelChanged(int lightFuel)
	{
	}

	private void OnStaminaChanged(float oldStamina, float newStamina)
	{
	}

	private void UpdateFogOfWar()
	{
	}

	private IList<int> GetOwnedEventItemIds()
	{
		return null;
	}

	private void OnInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnSafeInventoryChanged(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnDead()
	{
	}

	private void OnRevive()
	{
	}

	private void OnStashChanged()
	{
	}

	private void TriggerOwnedItemEvent(int itemId)
	{
	}

	private void TriggerGainWorthEvent()
	{
	}

	private void ApplyParadox()
	{
	}

	private void UpdateParadox(int[] oldParadoxIds, int[] newParadoxIds)
	{
	}

	private void OnStressChanged(float value)
	{
	}

	private void OnStressStateChanged(EStressState state)
	{
	}

	private void OnCorruptionStateChanged(ECorruptionState before, ECorruptionState after)
	{
	}

	private void OnCharacterUpdated(Character oldCharacter, Character newCharacter)
	{
	}

	private void UpdateCorruption()
	{
	}

	private void OnInventoryChangedForCorruption(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnSafeInventoryChangedForCorruption(IList<GoodsSlotChangeData> changeDatas)
	{
	}

	private void OnBuffUpdate()
	{
	}

	private void OnTryRunChanged()
	{
	}

	private void OnIsMovingBackwardChanged()
	{
	}

	private void OnStateChanged(EActionState current, EActionState previous)
	{
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
	}

	private bool IsEquipmentWorthChanged(Character oldCharacter, Character newCharacter)
	{
		return false;
	}

	private int GetEquipmentWorth(Character character)
	{
		return 0;
	}

	private void OnTalentChanged(Talent oldTalent, Talent newTalent)
	{
	}

	private void UpdateTalentUnlock(int talentId)
	{
	}

	private void UpdateAim()
	{
	}

	private void UpdateRun()
	{
	}

	protected override void RegisterClientHandlers()
	{
	}

	private void OnStartInteraction(NtfStartInteraction message)
	{
	}

	private void OnEndInteraction(NtfEndInteraction message)
	{
	}

	private void OnChangeCharacterSkin(NtfChangeCharacterSkin message)
	{
	}

	private void OnChangeActiveSkill(NtfChangeActiveSkill message)
	{
	}

	private void OnEmoji(NtfEmoji message)
	{
	}

	private void OnSelfReviveText(NtfSelfReviveText message)
	{
	}

	private void OnMonsterKill(NtfMonsterKill message)
	{
	}

	private void OnItemUsed(NtfItemUsed message)
	{
	}

	private void RegisterClientInventoryHandlers()
	{
	}

	private void OnBagSlotToStash(ResBagSlotToStash message)
	{
	}

	private void OnInventorySlotToStashBagSlot(ResInventorySlotToStashBagSlot message)
	{
	}

	private void OnMoveAllInventoryToStash(ResMoveAllInventoryToStash message)
	{
	}

	private void OnInventorySlotToStashSlot(ResInventorySlotToStashSlot message)
	{
	}

	private void OnSafeSlotToStashSlot(ResSafeSlotToStashSlot message)
	{
	}

	private void OnEquipStashSlot(ResEquipStashSlot message)
	{
	}

	private void OnEquipLootingSlot(ResEquipLootingSlot message)
	{
	}

	private void OnUnequipToStashSlot(ResUnequipToStashSlot message)
	{
	}

	private void RegisterClientPlayerActionHandlers()
	{
	}

	private void OnLevelUp(ResLevelUp message)
	{
	}

	private void OnUnlockTalent(ResUnlockTalent message)
	{
	}

	private void OnShopBuyGoods(ResShopBuyGoods message)
	{
	}

	private void OnShopSellGoods(ResShopSellGoods message)
	{
	}

	private void OnCraftingGoods(ResCraftingGoods message)
	{
	}

	private void OnScrapGoods(ResScrapGoods message)
	{
	}

	private void OnRegisterItemUsed(ResRegisterItemUsed message)
	{
	}

	private void OnSellArtifact(ResSellArtifact message)
	{
	}

	private void OnDealArtifact(ResDealArtifact message)
	{
	}

	private void OnTycoonRegisterArtifact(ResTycoonRegisterArtifact message)
	{
	}

	private void OnTycoonFinish(ResTycoonFinish message)
	{
	}

	private void OnTycoonHire(ResTycoonHire message)
	{
	}

	private void OnTycoonCancel(ResTycoonCancel message)
	{
	}

	private void OnTycoonPurify(ResTycoonPurify message)
	{
	}

	private void OnRest(ResRest message)
	{
	}

	private void OnParadoxPurify(ResParadoxPurify message)
	{
	}

	private void OnParadoxLock(ResParadoxLock message)
	{
	}

	private void OnRepairGoods(ResRepairGoods message)
	{
	}

	private void OnRepairAllGoods(ResRepairAllGoods message)
	{
	}

	private void OnTakeLostItem(ResTakeLostItem message)
	{
	}

	private void OnRerollLostItems(ResRerollLostItems message)
	{
	}

	private void OnExchangeCoin(ResExchangeCoin message)
	{
	}

	private void OnBuyInterior(ResBuyInterior message)
	{
	}

	private void OnSetPet(ResSetPet message)
	{
	}

	protected override void RegisterHostHandlers()
	{
	}

	private void OnTryKeyInteraction(ReqTryKeyInteraction message)
	{
	}

	private void OnInputMoveAim(ReqInputMoveAim message)
	{
	}

	private void OnInputRun(ReqInputRun message)
	{
	}

	private void OnInputSkill(ReqInputSkill message)
	{
	}

	private void UpdateBasicAttackHold()
	{
	}

	private void OnInputUseItem(ReqInputUseItem message)
	{
	}

	private void OnInputTryInteract(ReqInputTryInteract message)
	{
	}

	private void OnInputCancelInteract(ReqInputCancelInteract message)
	{
	}

	private void OnFinishInteractionWaiting(ReqFinishInteractionWaiting message)
	{
	}

	private void OnKillCharacter(ReqKillCharacter message)
	{
	}

	private void OnSpawnDropGoods(ReqSpawnDropGoods message)
	{
	}

	private void OnSpawnMonster(ReqSpawnMonster message)
	{
	}

	private void OnSpawnWaveExecutor(ReqSpawnWaveExecutor message)
	{
	}

	private void OnSpawnExit(ReqSpawnExit message)
	{
	}

	private void OnRewardBox(ReqRewardBox message)
	{
	}

	private void OnPlayCutscene(ReqPlayCutscene message)
	{
	}

	private void OnFinishCutscene(ReqFinishCutscene message)
	{
	}

	private void OnForceEscape(ReqForceEscape message)
	{
	}

	private void RegisterHostCheatHandlers()
	{
	}

	private void OnCheatApplySoul(ReqCheatApplySoul message)
	{
	}

	private void OnCheatReviveCharacter(ReqCheatReviveCharacter message)
	{
	}

	private void OnCheatMovePosition(ReqCheatMovePosition message)
	{
	}

	private void OnCheatAddGoods(ReqCheatAddGoods message)
	{
	}

	private void OnCheatSetGold(ReqCheatSetGold message)
	{
	}

	private void OnCheatSetCoin(ReqCheatSetCoin message)
	{
	}

	private void OnCheatSetExp(ReqCheatSetExp message)
	{
	}

	private void OnCheatApplyCharacterOptions(ReqCheatApplyCharacterOptions message)
	{
	}

	private void OnCheatResetSkillCooldown(ReqCheatResetSkillCooldown message)
	{
	}

	private void OnCheatSkillCharge(ReqCheatSkillCharge message)
	{
	}

	private void OnCheatLevelUp(ReqCheatLevelUp message)
	{
	}

	private void OnCheatUnlockAllTalent(ReqCheatUnlockAllTalent message)
	{
	}

	private void OnCheatSpawnMonster(ReqCheatSpawnMonster message)
	{
	}

	private void OnCheatSpawnTrap(ReqCheatSpawnTrap message)
	{
	}

	private void OnCheatSpawnWaveExecutor(ReqCheatSpawnWaveExecutor message)
	{
	}

	private void OnCheatSkillMonsterAll(ReqCheatSkillMonsterAll message)
	{
	}

	private void OnCheatAddParadox(ReqCheatAddParadox message)
	{
	}

	private void OnCheatAddRandomParadox(ReqCheatAddRandomParadox message)
	{
	}

	private void RegisterHostEtcHandlers()
	{
	}

	private void OnVoteCampaign(ReqVoteCampaign message)
	{
	}

	private void OnAcquireParadox(ReqAcquireParadox message)
	{
	}

	private void OnRemoveParadox(ReqRemoveParadox message)
	{
	}

	private void OnUpdateCampaignState(ReqUpdateCampaignState message)
	{
	}

	private void OnTriggerLuaEvent(ReqTriggerLuaEvent message)
	{
	}

	private void OnAddLuaBuff(ReqAddLuaBuff message)
	{
	}

	private void OnPing(ReqPing message)
	{
	}

	private void OnEmoji(ReqEmoji message)
	{
	}

	private void OnSetCharacterHp(ReqSetCharacterHp message)
	{
	}

	private void OnSetCharacterStress(ReqSetCharacterStress message)
	{
	}

	private void OnSetCharacterCorruption(ReqSetCharacterCorruption message)
	{
	}

	private void OnSetCharacterLightFuel(ReqSetCharacterLightFuel message)
	{
	}

	private void OnSetCharacterPosition(ReqSetCharacterPosition message)
	{
	}

	private void OnSetCharacterForward(ReqSetCharacterForward message)
	{
	}

	private void OnVoteGiveUpCampaign(ReqVoteGiveUpCampaign message)
	{
	}

	private void OnAddPointMarker(ReqAddPointMarker message)
	{
	}

	private void OnRemovePointMarker(ReqRemovePointMarker message)
	{
	}

	private void RegisterHostInventoryHandlers()
	{
	}

	private void Equip(Goods equipmentGoods, EGoodsSlotCategory slotCategory, int slotIndex, out Goods unequippedGoods)
	{
		unequippedGoods = null;
	}

	public void UnequipByEquipmentId(int equipmentId)
	{
	}

	private Goods Unequip(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		return null;
	}

	private void OnInventorySlotToSafeSlot(ReqInventorySlotToSafeSlot message)
	{
	}

	private void OnOnInventorySlotToLootingSlot(ReqInventorySlotToLootingSlot message)
	{
	}

	private void OnInventorySlotToStashSlot(ReqInventorySlotToStashSlot message)
	{
	}

	private void OnMoveAllInventoryToStash(ReqMoveAllInventoryToStash message)
	{
	}

	private void OnDropInventorySlot(ReqDropInventorySlot message)
	{
	}

	private void OnSwapInventorySlots(ReqSwapInventorySlots message)
	{
	}

	private void OnSafeSlotToInventorySlot(ReqSafeSlotToInventorySlot message)
	{
	}

	private void OnSafeSlotToLootingSlot(ReqSafeSlotToLootingSlot message)
	{
	}

	private void OnSafeSlotToStashSlot(ReqSafeSlotToStashSlot message)
	{
	}

	private void OnDropSafeSlot(ReqDropSafeSlot message)
	{
	}

	private void OnSwapSafeSlots(ReqSwapSafeSlots message)
	{
	}

	private void OnLootingSlotToSafeSlot(ReqLootingSlotToSafeSlot message)
	{
	}

	private void OnLootingSlotToInventorySlot(ReqLootingSlotToInventorySlot message)
	{
	}

	private void OnSwapLootingSlots(ReqSwapLootingSlots message)
	{
	}

	private void OnStashSlotToSafeSlot(ReqStashSlotToSafeSlot message)
	{
	}

	private void OnStashSlotToInventorySlot(ReqStashSlotToInventorySlot message)
	{
	}

	private void OnEquipInventorySlot(ReqEquipInventorySlot message)
	{
	}

	private void OnEquipSafeSlot(ReqEquipSafeSlot message)
	{
	}

	private void OnEquipLootingSlot(ReqEquipLootingSlot message)
	{
	}

	private void OnEquipStashSlot(ReqEquipStashSlot message)
	{
	}

	private void OnUnequipToInventorySlot(ReqUnequipToInventorySlot message)
	{
	}

	private void OnUnequipToSafeSlot(ReqUnequipToSafeSlot message)
	{
	}

	private void OnUnequipToLootingSlot(ReqUnequipToLootingSlot message)
	{
	}

	private void OnUnequipToStashSlot(ReqUnequipToStashSlot message)
	{
	}

	private void OnSwapEquipmentSlots(ReqSwapEquipmentSlots message)
	{
	}

	private void OnSwapWeapon(ReqSwapWeapon message)
	{
	}

	private void OnOrganizeInventorySlots(ReqOrganizeInventorySlots message)
	{
	}

	private void OnOrganizeInventorySafeSlots(ReqOrganizeInventorySafeSlots message)
	{
	}

	private void OnGainGoods(ReqGainGoods message)
	{
	}

	private void OnConsumeGoods(ReqConsumeGoods message)
	{
	}

	private void OnRemoveInventorySlot(ReqRemoveInventorySlot message)
	{
	}

	private void OnRemoveSafeSlot(ReqRemoveSafeSlot message)
	{
	}

	private void OnAnyToBagSlot(ReqAnyToBagSlot message)
	{
	}

	private void HandleEquipmentToBag(ReqAnyToBagSlot message)
	{
	}

	private void HandleStashSourceToBag(ReqAnyToBagSlot message)
	{
	}

	private Goods GetEquippedGoods(EGoodsSlotCategory category, int slotIndex)
	{
		return null;
	}

	private IDictionary<int, GoodsSlot> GetSourceSlots(EGoodsSlotCategory category, ulong networkObjectId, ulong senderId)
	{
		return null;
	}

	private void OnBagSlotToAny(ReqBagSlotToAny message)
	{
	}

	private bool TryPushToTarget(ReqBagSlotToAny message, Goods pulledGoods)
	{
		return false;
	}

	private bool TryPushAndSwap(IDictionary<int, GoodsSlot> targetSlots, ReqBagSlotToAny message, Goods pulledGoods)
	{
		return false;
	}

	private bool TryEquipFromBag(ReqBagSlotToAny message, Goods pulledGoods)
	{
		return false;
	}

	private long ResolveBagInnerItemMaxStack(IDictionary<int, GoodsSlot> bagHolderSlots, int bagHolderSlotIndex, int innerSlotIndex)
	{
		return 0L;
	}

	private IDictionary<int, GoodsSlot> GetBagHolderSlots(EGoodsSlotCategory category, ulong networkObjectId, ulong senderId)
	{
		return null;
	}

	private void RegisterHostPlayerActionHandlers()
	{
	}

	private void GainGold(long amount)
	{
	}

	private bool TryConsumeGold(long amount)
	{
		return false;
	}

	private void GainCoin(long amount)
	{
	}

	private bool TryConsumeCoin(long amount)
	{
		return false;
	}

	private bool TryConsumeExp(long amount)
	{
		return false;
	}

	private bool TryConsumeGoods(Goods goods, bool allowStashFallback, out long consumeGold, out long consumeCoin, out Goods remainGoods)
	{
		consumeGold = default;
		consumeCoin = default;
		remainGoods = null;
		return false;
	}

	private bool TryConsumeGoods(Goods[] goodsArray, out long consumeGold, out long consumeCoin, out Goods[] remainMaterials)
	{
		consumeGold = default;
		consumeCoin = default;
		remainMaterials = null;
		return false;
	}

	private void GainExp(long amount)
	{
	}

	private void GainCurrency(Goods goods)
	{
	}

	private void OnLevelUp(ReqLevelUp message)
	{
	}

	private void OnGainAccumulatedExp(ReqGainAccumulatedExp message)
	{
	}

	private void OnUnlockTalent(ReqUnlockTalent message)
	{
	}

	private void OnChangeCharacter(ReqChangeCharacter message)
	{
	}

	private void OnSetPet(ReqSetPet message)
	{
	}

	private void OnChangeCharacterSkin(ReqChangeCharacterSkin message)
	{
	}

	private void OnChangeCharacterSkills(ReqChangeCharacterSkills message)
	{
	}

	private void OnShopBuyGoods(ReqShopBuyGoods message)
	{
	}

	private void OnShopSellGoods(ReqShopSellGoods message)
	{
	}

	private void OnCraftingGoods(ReqCraftingGoods message)
	{
	}

	private void OnScrapGoods(ReqScrapGoods message)
	{
	}

	private void OnRegisterItemUsed(ReqRegisterItemUsed message)
	{
	}

	private void OnSellArtifact(ReqSellArtifact message)
	{
	}

	private void OnDealArtifact(ReqDealArtifact message)
	{
	}

	private void OnRest(ReqRest message)
	{
	}

	private void OnParadoxPurify(ReqParadoxPurify message)
	{
	}

	private void OnParadoxLock(ReqParadoxLock message)
	{
	}

	private void OnParadoxUnlock(ReqParadoxUnlock message)
	{
	}

	private void OnRepairGoods(ReqRepairGoods message)
	{
	}

	private Goods GetRepairTargetGoods(EGoodsSlotCategory slotCategory, int slotIndex)
	{
		return null;
	}

	private void SetRepairTargetGoods(EGoodsSlotCategory slotCategory, int slotIndex, Goods repairedGoods)
	{
	}

	private void OnRepairAllGoods(ReqRepairAllGoods message)
	{
	}

	private ETycoonFailType RollTycoonFailType()
	{
		return ETycoonFailType.ArtifactDamage;
	}

	private void FlushPlayerStateBeforeResponse()
	{
	}

	private void OnTycoonHire(ReqTycoonHire message)
	{
	}

	private void OnTycoonRegisterArtifact(ReqTycoonRegisterArtifact message)
	{
	}

	private void OnTycoonFinish(ReqTycoonFinish message)
	{
	}

	private void OnTycoonCancel(ReqTycoonCancel message)
	{
	}

	private void OnTycoonPurify(ReqTycoonPurify message)
	{
	}

	private void OnTakeLostItem(ReqTakeLostItem message)
	{
	}

	private void OnRerollLostItems(ReqRerollLostItems message)
	{
	}

	private void OnExchangeCoin(ReqExchangeCoin message)
	{
	}

	private void OnBuyInterior(ReqBuyInterior message)
	{
	}

	private bool IsInteriorUnlocked(Interior interior)
	{
		return false;
	}

	public override bool IsActionEventExcutable(ActionEvent actionEvent, ulong ownerId, ulong targetId)
	{
		return false;
	}

	protected override void RegisterUnitEventHandlers()
	{
	}

	protected override void OnDieEvent(DieEvent unitEvent)
	{
	}

	protected override void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
	}

	protected override void OnHpDamageApplied(HpDamageEvent unitEvent, int hpDamage)
	{
	}

	private void OnSkillInputEvent(SkillInputEvent unitEvent)
	{
	}

	private void OnItemUseEvent(UseItemEvent unitEvent)
	{
	}

	private void OnTryInteractEvent(TryInteractEvent unitEvent)
	{
	}

	private void OnCancelInteractEvent(CancelInteractEvent unitEvent)
	{
	}

	private void OnEscapeEvent(EscapeEvent unitEvent)
	{
	}

	private void OnLightFuelDamageEvent(LightFuelDamageEvent unitEvent)
	{
	}

	private void OnLightFuelRecoveryEvent(LightFuelRecoveryEvent unitEvent)
	{
	}

	private void OnStageReadyEvent(StageReadyEvent unitEvent)
	{
	}

	protected override void OnNoticeDamage(NoticeDamageEvent unitEvent)
	{
	}

	protected override void OnNoticeDie(NoticeDieEvent unitEvent)
	{
	}

	protected override void OnNoticeRevive(NoticeReviveEvent unitEvent)
	{
	}

	private void OnStressDamageEvent(StressDamageEvent unitEvent)
	{
	}

	private void OnStressRecoveryEvent(StressRecoveryEvent unitEvent)
	{
	}

	private void OnStaminaDamageEvent(StaminaDamageEvent unitEvent)
	{
	}

	private void OnStaminaRecoveryEvent(StaminaRecoveryEvent unitEvent)
	{
	}

	private void OnSoulDamageEvent(SoulDamageEvent unitEvent)
	{
	}

	private void OnSoulRecoveryEvent(SoulRecoveryEvent unitEvent)
	{
	}

	private void OnCutsceneStartEvent(CutsceneStartEvent unitEvent)
	{
	}

	private void OnCutsceneFinishEvent(CutsceneFinishEvent unitEvent)
	{
	}

	private void DecreaseAccessoryDurability()
	{
	}

	public void DecreaseWeaponDurability(float decreaseAmount)
	{
	}

	private void TryShowAllyDamageBubble(HpDamageEvent unitEvent)
	{
	}

	private void OnEquipmentBroken(int equipmentId)
	{
	}



}
