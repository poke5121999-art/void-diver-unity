using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;

public class CharacterModel : UnitModel
{
	private bool isInitialized;

	public readonly NetworkVariable<ulong> SyncClientId;

	public readonly NetworkVariable<int> SyncHitLayer;

	public readonly NetworkVariable<bool> SyncIsWaitingInteractionFinish;

	public int CharacterId;

	public readonly ReactiveProperty<string> RxPlayerName;

	public readonly ReactiveProperty<EStressState> RxStressState;

	public readonly ReactiveProperty<ECorruptionState> RxCorruptionState;

	public readonly Subject<Unit> OnInventoryInitialized;

	public bool CanCancelInputInteraction;

	private GamePlayer _player;

	public int SelfReviveCount;

	public float TryRunExpireTime;

	private List<int> _currentEquipmentBuffIds;

	private List<int> _currentEquipmentPassiveSkillIds;

	private List<int> _currentEquipmentActiveSkillIds;

	private List<int> _allEquipmentActiveSkillIds;

	private HashSet<EStatType> _previousEquipmentStatTypes;

	private NetworkVariable<bool> _syncIsLightOn;

	private NetworkVariable<int> _syncLightFuel;

	private NetworkVariable<bool> _syncTryRun;

	private NetworkVariable<bool> _syncIsMovingBackward;

	private readonly ReactiveProperty<bool> _rxIsLightOn;

	private readonly ReactiveProperty<int> _rxLightFuel;

	private readonly ReactiveProperty<bool> _rxTryRun;

	private readonly ReactiveProperty<bool> _rxIsMovingBackward;

	protected Dictionary<EStatType, float> EquipmentStats;

	private Dictionary<EStatType, float> _stressConditionalStats;

	private bool _stressConditionalStatsDirty;

	public override int HitLayer => 0;

	public override int TargetHitLayer => 0;

	public override EUnitType UnitType => EUnitType.None;

	public override int TableId => 0;

	public override bool IsInitialized => false;

	public override float GlobalSkillCoolTime => 0f;

	public ulong ClientId => 0uL;

	public bool IsWaitingInteractionFinish => false;

	public string PlayerName => null;

	public override EElementalType ElementalType => EElementalType.None;

	public override float WeaponAtkMultiplier => 0f;

	public override EMonsterCategory[] MonsterCategories => null;

	public override EStressState StressState => EStressState.Alert;

	public ECorruptionState CorruptionState => ECorruptionState.Safe;

	public int SlotCount => 0;

	public int SafeSlotCount => 0;

	public PlayerInventory Inventory { get; private set; }

	public override int WeaponId => 0;

	public IEnumerable<int> AccessoryIds => null;

	public IEnumerable<Goods> Artifacts => null;

	private IEnumerable<int> EquipmentIds => null;

	private IEnumerable<int> ActiveEquipmentIds => null;

	public Subject<int> OnEquipmentPassiveSkillAdded { get; }

	public bool IsEquipmentActiveSkillLocked => false;

	public IReadOnlyReactiveProperty<bool> RxIsLightOn => null;

	public IReadOnlyReactiveProperty<int> RxLightFuel => null;

	public IReadOnlyReactiveProperty<bool> RxTryRun => null;

	public IReadOnlyReactiveProperty<bool> RxIsMovingBackward => null;

	public bool IsLightOn
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public int LightFuel
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public bool TryRun
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool IsMovingBackward
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public void Init(GamePlayer player)
	{
	}

	private void Start()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void UpdateAim()
	{
	}

	public override bool HasTalent(int talentId)
	{
		return false;
	}

	private void OnStressChanged(float stress)
	{
	}

	private void OnCorruptionChanged(int corruption)
	{
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
	}

	private void OnMaxHpChanged(UnitStat oldValue, UnitStat newValue)
	{
	}

	public long GetOwnedItemCount(int itemId)
	{
		return 0L;
	}

	public int CalculateGainWorth()
	{
		return 0;
	}

	public int CalculateTotalCorruption()
	{
		return 0;
	}

	protected override void OnChangeSkillEffectActivated(ChangeSkillEffect effect)
	{
	}

	protected override void OnChangeSkillEffectDeactivated(ChangeSkillEffect effect)
	{
	}

	public void RefreshEquipmentEffects(bool isHost)
	{
	}

	private void ApplyEquipmentStats()
	{
	}

	private void UpdateEquipmentSkillIds()
	{
	}

	private void ApplyEquipmentBuffEffects()
	{
	}

	public List<int> GetEquipmentPassiveSkillIds()
	{
		return null;
	}

	public List<int> GetEquipmentActiveSkillIds()
	{
		return null;
	}

	public List<int> GetAllEquipmentActiveSkillIds()
	{
		return null;
	}

	public bool IsEquipmentActiveSkillChargeBlocked(int skillId)
	{
		return false;
	}

	private int ResolveEquipmentActiveSkillId(int currentSlotSkillId)
	{
		return 0;
	}

	private void ResetEquipmentSkillGauge(int skillId)
	{
	}

	public bool UpdateEquipmentActiveSkillSlot(Character character, out Character updated)
	{
		updated = null;
		return false;
	}

	protected override void RegisterVariableEvents()
	{
	}

	protected override void UnregisterVariableEvents()
	{
	}

	protected void InitNetDataVariable()
	{
	}

	public override bool CheckSkill(int skillId)
	{
		return false;
	}

	private bool IsActiveSkillLocked(int skillId)
	{
		return false;
	}

	public override float GetAdditionalStat(EStatType statType)
	{
		return 0f;
	}

	protected override void OnBuffActivated(int buffId)
	{
	}

	protected override void OnBuffDeactivated(int buffId)
	{
	}

	private static bool HasStressConditionalStatsEffect(int buffId)
	{
		return false;
	}

	public void RefreshStressConditionalStats()
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
