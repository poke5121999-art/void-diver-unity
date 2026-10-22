using System.Collections.Generic;
using De.InGame.Fsm;
using De.InGame.Unit;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : UnitController
{
	public class Args
	{
		public int MonsterId;

		public int MonsterSpawnId;

		public ESpawnType SpawnType;

		public Vector3 Forward;

		public float LifeTime;

		public int HitLayer;

		public ulong TargetNetworkObjectId;

		public ulong SummonerNetworkObjectId;

		public bool DieOnSummonerDeath;

		public Vector3 WaveStartPosition;

		public bool IsPatrolMonster;

		public Vector3[] PatrolWanderPositions;

		public bool UseFixedAimPos;

		public Vector3 FixedAimPos;
	}

	private Args _args;

	private MonsterStateMachine _monsterStateMachine;

	private PlaneSightDetectableObject _planeSightDetectableObject;

	private OutlineObject _outlineObject;

	private readonly Subject<Unit> _combatEventSubject;

	private readonly NetworkVariable<Vector3> _syncInitForward;

	private readonly NetworkVariable<ESpawnType> _syncInitSpawnType;

	private readonly List<MonsterModel> _summonMonsterModels;

	private List<ulong> _lightDetectingUnitIds;

	private float _nextFleeCheckTime;

	public NavMeshAgent NavMeshAgent => null;

	protected override UnitStateMachine StateMachine => null;

	private TMonster MonsterData => null;

	public override ulong ClientId => 0uL;

	private MonsterView MonsterView => null;

	private MonsterModel MonsterModel => null;

	private PlaneSightDetectableObject PlaneSightDetectableObject => null;

	private OutlineObject OutlineObject => null;

	public int MonsterId => 0;

	public int MonsterSpawnId => 0;

	public bool IsActivateInStage => false;

	public ESpawnType SpawnType => ESpawnType.None;

	public bool HasLifeTime => false;

	public Vector3 WaveStartPosition => default;

	public bool IsPatrolMonster => false;

	public bool UseFixedAimPos => false;

	public Vector3 FixedAimPos => default;

	private float TimeNow => 0f;

	public bool IsBoss => false;

	public bool IsBasePhase => false;

	public int SummonMonsterCount => 0;

	private void TriggerCombatEvent()
	{
	}

	public void Init(Args args)
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	protected override void OnNetworkUpdate()
	{
	}

	protected override void OnUpdateSec()
	{
	}

	private IList<GoodsSlot> CreateInventorySlots(int dropRewardGroupId)
	{
		return null;
	}

	private void OnCaptured()
	{
	}

	private void OnDead()
	{
	}

	private void OnStateChanged()
	{
	}

	private void OnTargetChanged(ulong targetNetworkObjectId)
	{
	}

	private void OnViewTargetChanged()
	{
	}

	private void UpdateShadowMonsterEffect()
	{
	}

	private void OnCombatEvent()
	{
	}

	private void CheckCombatExitTime()
	{
	}

	private void CooldownWeakPointDamage()
	{
	}

	private void OnStatChanged()
	{
	}

	private void OnWeakPointTypeChanged(EElementalType type)
	{
	}

	protected override void RegisterClientHandlers()
	{
	}

	protected override void RegisterHostHandlers()
	{
	}

	public void AddSummonMonsterModel(MonsterModel model)
	{
	}

	public void RemoveSummonMonsterModel(MonsterModel model)
	{
	}

	public bool CanSummonMonster()
	{
		return false;
	}

	private void OnUpdateLightCount()
	{
	}

	private void UpdateLightStackBuff()
	{
	}

	private void RegisterCheatHandlers()
	{
	}

	private void OnCheatApplyHp(ReqCheatApplyMonsterHp message)
	{
	}

	private void OnBuffEventForSuppression(BuffEvent unitEvent)
	{
	}

	private void AddSuppression(float amount)
	{
	}

	private void UpdateSuppressionTick()
	{
	}

	private void OnSuppressionDamageEvent(SuppressionDamageEvent unitEvent)
	{
	}

	private void OnGroggyEvent(GroggyEvent unitEvent)
	{
	}

	protected override void RegisterUnitEventHandlers()
	{
	}

	protected void OnReceiveAggroSpreadEvent(ReceiveAggroSpreadEvent unitEvent)
	{
	}

	protected override void OnDieEvent(DieEvent unitEvent)
	{
	}

	protected override void OnNoticeDie(NoticeDieEvent unitEvent)
	{
	}

	protected override int CalculateDamage(HpDamageEvent unitEvent)
	{
		return 0;
	}

	protected override void OnHpDamageEvent(HpDamageEvent unitEvent)
	{
	}

	private void OnForceMonsterSetTargetEvent(ForceMonsterSetTargetEvent unitEvent)
	{
	}

	private void OnTryMonsterCuriousStateEvent(TryMonsterCuriousStateEvent unitEvent)
	{
	}

	private void OnMonsterPhaseUpEvent(MonsterPhaseUpEvent unitEvent)
	{
	}

	protected override void OnCrowdControlEvent(CrowdControlEvent unitEvent)
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
