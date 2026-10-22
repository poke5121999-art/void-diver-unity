using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using Unity.Netcode;

public class ExtraUnitModel : UnitModel
{
	private bool isInitialized;

	public NetworkVariable<int> SyncExtraUnitId = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

	public override int HitLayer => 1000;

	public override int TargetHitLayer => 0;

	public override EUnitType UnitType => EUnitType.Extra;

	public override EElementalType ElementalType => EElementalType.None;

	public override EMonsterCategory[] MonsterCategories => TExtraUnit.Table[SyncExtraUnitId.Value].MonsterCategories;

	public override EStressState StressState => EStressState.Alert;

	public override int WeaponId => 0;

	public override bool IsInitialized => isInitialized;

	public override int TableId => SyncExtraUnitId.Value;

	public override float GlobalSkillCoolTime { get; }

	public int ExtraUnitId => SyncExtraUnitId.Value;

	public override float WeaponAtkMultiplier => 1f;

	public void Init()
	{
		TExtraUnit tExtraUnit = TExtraUnit.Table[SyncExtraUnitId.Value];
		Dictionary<EStatType, float> baseStats = tExtraUnit.GetBaseStats();
		Init(baseStats, tExtraUnit.ActiveSkillIds, tExtraUnit.PassiveSkillIds, Vector3.zero);
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		isInitialized = true;
	}



}
