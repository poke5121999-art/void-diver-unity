using System;
using System.Collections.Generic;
using System.Threading;
using De.InGame.Fsm;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UniRx;
using UnityEngine;
using VContainer;

public abstract class UnitController : NetworkBase
{
	private class PassiveExecutionContext
	{
		public int SkillId;

		public SkillActionNode Node;

		public IUnitEvent TriggeredEvent;

		public List<ITarget> TriggerTargets;

		public bool ChildExecuted;
	}

	[Inject]
	protected GameManager GameManager;

	[Inject]
	protected SoundManager SoundManager;

	[Inject]
	protected RoomContext RoomContext;

	[Inject]
	protected GameContext GameContext;

	[Inject]
	protected GameObjectManager ObjectManager;

	[Inject]
	protected GameEventSystem EventSystem;

	[Inject]
	protected CameraManager CameraManager;

	[Inject]
	protected SeriLogger Logger;

	[Inject]
	protected GameMissionManager MissionManager;

	[SerializeField]
	protected UnitModel UnitModel;

	[SerializeField]
	protected UnitView UnitView;

	[SerializeField]
	protected Inventory Inventory;

	private readonly MessageBroker _eventMessageBroker;

	private PlaneSightUnit _planeSightUnit;

	private float _hpFraction;

	private readonly Stack<PassiveExecutionContext> _passiveExecutionStack;

	private float _freezeDurationReductionCooldownEndTime;

	protected GameHost GameHost => null;

	protected GameChannel GameChannel => null;

	protected IMessageReceiver EventReceiver => null;

	protected abstract UnitStateMachine StateMachine { get; }

	public abstract ulong ClientId { get; }

	protected PlaneSightUnit PlaneSightUnit => null;

	protected int StressMax => 0;

	public Subject<string> OnShowBubbleText { get; }

	public Subject<string> OnShowMonologueText { get; }

	protected virtual void Start()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	protected virtual void FixedUpdate()
	{
	}

	protected override void OnNetworkUpdate()
	{
	}

	protected virtual void OnStageStarted()
	{
	}

	protected void RegisterHostHandler<T>(Action<T> handler) where T : BaseMessage
	{
	}

	protected void RegisterClientHandler<T>(Action<T> handler) where T : BaseMessage
	{
	}

	protected virtual void OnUpdateSec()
	{
	}

	private void UpdateSkillCharge()
	{
	}

	private void UpdateRegenHp()
	{
	}

	private void UpdateSpecialFields()
	{
	}

	private void GenerateConsumeResultItems(int consumeItemId)
	{
	}

	private bool CheckCcInfoCondition(CcInfo ccInfo)
	{
		return false;
	}

	private void UpdateTickDamage()
	{
	}

	private static int ApplyAmplifier(float percent, int baseAmount)
	{
		return 0;
	}

	private float CalculateAmplifierPercent(EStatusEffectTag effectTag, EBuffEffectType effectType, IEnumerable<ulong> casterIds)
	{
		return 0f;
	}

	private static bool IsMatchingAmplifier(DotEffectAmplifierEffect buffEffect, EStatusEffectTag effectTag, EBuffEffectType effectType)
	{
		return false;
	}

	private void SendDotEffectTagFx(EStatusEffectTag effectTag, Vector3 position, ulong targetNetworkObjectId)
	{
	}

	private void SendDotBuffFx(int buffId, Vector3 position, ulong targetNetworkObjectId)
	{
	}

	protected virtual void RegisterCommonHandlers()
	{
	}

	protected virtual void RegisterHostHandlers()
	{
	}

	protected virtual void RegisterClientHandlers()
	{
	}

	private void OnPlayAnimation(NtfPlayAnimation message)
	{
	}

	private void OnUpdateAnimation(NtfUpdateAnimation message)
	{
	}

	private void OnChangeSkill(NtfChangeSkill message)
	{
	}

	public void ExecutePassiveSkill(int skillId, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
	}

	private void ExecutePassiveSkillAction(int skillId, SkillActionNode node, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
	}

	private void TryExecuteFallbackChildNode(PassiveExecutionContext context, int skillId, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
	}

	private void TryProceedPassiveChildNode(IUnitEvent unitEvent)
	{
	}

	private void OnPassiveActionEvent(int skillId, ActionEvent actionEvent, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
	{
	}

	private void OnPassiveActionEventOnTarget(int skillId, ActionEvent actionEvent, ITarget actionTarget, ITarget triggerTarget)
	{
	}

	public virtual bool IsActionEventExcutable(ActionEvent actionEvent, ulong ownerNetworkObjectId, ulong targetNetworkObjectId)
	{
		return false;
	}

	public bool CheckBuffCondition(BuffCondition buffCondition, ulong skillOwnerId, ulong targetId)
	{
		return false;
	}

	public bool CheckBuffConditions(IList<BuffCondition> buffConditionList, ulong ownerNetworkObjectId, ulong targetNetworkObjectId)
	{
		return false;
	}

	public bool CheckTalentCondition(IList<TalentCondition> talentConditionList)
	{
		return false;
	}

	public void ApplySkillCost(int skillId, int hpCost, int sanityCost, int stamina)
	{
	}

	public void AdjustSkillCosts(TSkill skill, ref int hpCost, ref int stressCost, ref int staminaCost)
	{
	}

	public void OnBuffActionEvent(BuffActionEvent buffEvent, ITarget target)
	{
	}

	public void OnRemoveBuffActionEvent(RemoveStatusEffectActionEvent actionEvent, ITarget target)
	{
	}

	public void OnCrowdControlActionEvent(CrowdControlActionEvent actionEvent, ITarget target)
	{
	}

	public long OnVfxEvent(VfxEvent vfxEvent, ITarget target)
	{
		return 0L;
	}

	public void OnSfxEvent(SfxEvent sfxEvent, ITarget target)
	{
	}

	public void OnIndicatorVfxEvent(IndicatorVfxEvent indicatorVfxEvent, UnitModel casterUnitModel, Vector3 deltaAimPos, ITarget triggerTarget)
	{
	}

	public void OnHitBoxIteratorEvent(HitBoxIteratorEvent hitBoxIteratorEvent, ITarget target, int skillId, Vector3 deltaAimPos, SkillAction currentAction, float timeScale = 1f, CancellationTokenSource cancellationTokenSource = null)
	{
	}

	public void OnSummonActionEvent(SummonActionEvent summonActionEvent, ITarget target)
	{
	}

	public void OnSpecialFieldActionEvent(SpecialFieldActionEvent specialFieldActionEvent, ITarget target)
	{
	}

	public void OnDamageActionEvent(DamageActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnRecoveryActionEvent(RecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnStressDamageEvent(StressDamageActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnStressRecoveryEvent(StressRecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnSoulRecoveryEvent(SoulRecoveryActionEvent actionEvent, ITarget target)
	{
	}

	public void OnBrightnessDamageEvent(BrightnessDamageActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnBrightnessRecoveryEvent(BrightnessRecoveryActionEvent actionEvent, ITarget target, int skillId)
	{
	}

	public void OnConsumeItemActionEvent(ConsumeItemActionEvent consumeItemActionEvent, ITarget target)
	{
	}

	public void OnCameraShakeActionEvent(CameraShakeActionEvent cameraShakeActionEvent)
	{
	}

	public void OnKillActionEvent(ITarget target)
	{
	}

	public void OnTeleportActionEvent(TeleportActionEvent teleportActionEvent, ITarget teleportActor, ITarget triggerTarget = null)
	{
	}

	public void OnShieldActionEvent(ShieldActionEvent actionEvent, ITarget target)
	{
	}

	public void OnAddSuppressionActionEvent(AddSuppressionActionEvent actionEvent, ITarget target)
	{
	}

	public void OnAcquireParadoxActionEvent(AcquireParadoxActionEvent actionEvent, ITarget target)
	{
	}

	public void OnRemoveParadoxActionEvent(RemoveParadoxActionEvent actionEvent, ITarget target)
	{
	}

	public void Teleport(Vector3 deltaMovePos, TeleportActionEvent teleportActionEvent)
	{
	}

	public void OnForceUpdateTargetActionEvent(ForceUpdateTargetActionEvent forceUpdateTargetActionEvent, ITarget target)
	{
	}

	public void OnTargetableHitBoxEvent(HitBoxEvent hitBoxEvent, ITarget target, int skillId, Vector3 deltaAimPos, SkillAction currentAction)
	{
	}

	public void OnApplyPolymorphSkillsEvent(ApplyPolymorphSkillsActionEvent applyPolymorphSkillsActionEvent, ITarget target)
	{
	}

	public void OnRevertPolymorphSkillsEvent(RevertPolymorphSkillsActionEvent revertPolymorphSkillsActionEvent, ITarget target)
	{
	}

	private int ConvertToPolymorphSkillId(int oldSkillId, TCharacter characterData)
	{
		return 0;
	}

	private int ConvertToOriginalSkillId(int polymorphSkillId, TCharacter characterData)
	{
		return 0;
	}

	public void OnChangeSkinActionEvent(ChangeSkinActionEvent changeSkinActionEvent, ITarget target)
	{
	}

	public void OnSpreadAggroActionEvent(SpreadAggroActionEvent spreadAggroActionEvent, ITarget target)
	{
	}

	public void OnShowConsumeGaugeActionEvent(ShowConsumeGaugeActionEvent actionEvent)
	{
	}

	public void OnHitBoxCollectActionEvent(HitBoxCollectActionEvent hitBoxCollectActionEvent, ITarget target)
	{
	}

	public void OnMonologueActionEvent(MonologueActionEvent monologueActionEvent, ITarget target)
	{
	}

	public List<Vector3> GetTargetPosition(ESpawnPositionType spawnPositionType, ITarget target, Vector3 skillDeltaAimPos = default(Vector3), float playerMaxDistance = -1f)
	{
		return null;
	}

	protected virtual void RegisterUnitEventHandlers()
	{
	}

	public void HandleUnitEvent<T>(T unitEvent) where T : IUnitEvent
	{
	}

	protected void TriggerEvent(IUnitEvent unitEvent)
	{
	}

	private void TriggerPassiveSkill(IUnitEvent unitEvent)
	{
	}

	protected virtual int CalculateDamage(HpDamageEvent unitEvent)
	{
		return 0;
	}

	protected virtual void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
	}

	protected virtual void OnHpDamageApplied(HpDamageEvent unitEvent, int hpDamage)
	{
	}

	private void OnHpRecoveryEvent(HpRecoveryEvent unitEvent)
	{
	}

	private void OnBuffEvent(BuffEvent unitEvent)
	{
	}

	private void OnBuffRemovedEvent(BuffRemovedEvent unitEvent)
	{
	}

	private void OnRemoveStatusEffectEvent(RemoveStatusEffectEvent unitEvent)
	{
	}

	protected virtual void OnCrowdControlEvent(CrowdControlEvent unitEvent)
	{
	}

	protected virtual void OnDieEvent(DieEvent unitEvent)
	{
	}

	private void OnUseSkillEvent(UseSkillEvent unitEvent)
	{
	}

	private void OnMoveFinishEvent(MoveFinishEvent unitEvent)
	{
	}

	private void OnAddPassiveEvent(AddPassiveEvent unitEvent)
	{
	}

	protected virtual void OnReviveEvent(ReviveEvent unitEvent)
	{
	}

	private void OnCaptureEvent(CaptureEvent unitEvent)
	{
	}

	private void OnForceSkillTargetUpdateEvent(ForceSkillTargetUpdateEvent unitEvent)
	{
	}

	private void OnEnterSpecialFieldEvent(EnterSpecialFieldEvent unitEvent)
	{
	}

	private void OnExitSpecialFieldEvent(ExitSpecialFieldEvent unitEvent)
	{
	}

	protected virtual void OnNoticeDamage(NoticeDamageEvent unitEvent)
	{
	}

	protected virtual void OnNoticeCrowdControl(NoticeCrowdControlEvent unitEvent)
	{
	}

	protected virtual void OnNoticeDie(NoticeDieEvent unitEvent)
	{
	}

	protected virtual void OnNoticeRevive(NoticeReviveEvent unitEvent)
	{
	}

	protected virtual void OnNoticeRecovery(NoticeRecoveryEvent unitEvent)
	{
	}

	private void OnParryEvent(ParryEvent unitEvent)
	{
	}

	private void OnSkillChargeEvent(SkillChargeEvent unitEvent)
	{
	}

	private void OnConsumeItemUnitEvent(ConsumeItemEvent unitEvent)
	{
	}

	private void OnShieldEvent(ShieldEvent unitEvent)
	{
	}

	private void OnPerSecondEvent(PerSecondEvent unitEvent)
	{
	}

	private void OnStateChangeEvent(StateChangeEvent unitEvent)
	{
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
