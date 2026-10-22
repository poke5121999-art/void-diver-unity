using GameCommon.Enum;
using Unity.Netcode;

public class ExtraUnitModel : UnitModel
{
	private bool isInitialized;

	public NetworkVariable<int> SyncExtraUnitId;

	public override int HitLayer => 0;

	public override int TargetHitLayer => 0;

	public override EUnitType UnitType => EUnitType.None;

	public override EElementalType ElementalType => EElementalType.None;

	public override EMonsterCategory[] MonsterCategories => null;

	public override EStressState StressState => EStressState.Alert;

	public override int WeaponId => 0;

	public override bool IsInitialized => false;

	public override int TableId => 0;

	public override float GlobalSkillCoolTime { get; }

	public int ExtraUnitId => 0;

	public override float WeaponAtkMultiplier => 0f;

	public void Init()
	{
	}

	public override void OnNetworkSpawn()
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
