using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using Spine;
using UnityEngine;
using UnityEngine.AI;

public static class Extensions
{
	public static void ExecuteAction(this IActionExecutable actionExecutable, ActionEvent actionEvent, HitLayerInfo hitLayerInfo)
	{
		GameObjectManager objectManager = actionExecutable.ObjectManager;
		GameObject gameObject = actionExecutable.GameObject;
		Transform transform = gameObject.transform;
		ulong networkObjectId = actionExecutable.NetworkObjectId;
		if (actionEvent is HitBoxEvent hitBoxEvent)
		{
			int id = hitBoxEvent.Id;
			UnitModel component = gameObject.GetComponent<UnitModel>();
			ActionStatsData ownerStats = new ActionStatsData(actionExecutable);
			objectManager.SpawnHitBoxAsync(id, component, ownerStats, hitLayerInfo, transform.forward, transform.position, false, -1).Forget();
		}
		else if (actionEvent is SummonActionEvent summonActionEvent)
		{
			Vector3 spawnOffset = summonActionEvent.SpawnOffset;
			if (!summonActionEvent.IgnoreOffsetRotation)
			{
				spawnOffset = Quaternion.LookRotation(transform.forward, Vector3.up) * spawnOffset;
			}
			if (NavMesh.SamplePosition(transform.position + spawnOffset, out var hit, 5f, 1))
			{
				SummonMonsterInfo randomInfo = summonActionEvent.MonsterInfos.GetRandomInfo();
				if (randomInfo != null)
				{
					MonsterSpawnData spawnData = new MonsterSpawnData
					{
						MonsterId = randomInfo.Id,
						SpawnPosition = hit.position,
						IsRespawn = false,
						SpawnType = ESpawnType.SummonActionEvent,
						SummonerNetworkObjectId = networkObjectId,
						LifeTime = randomInfo.LifeTime
					};
					objectManager.TrySpawnMonster(spawnData);
				}
			}
		}
		else if (actionEvent is SpecialFieldActionEvent specialFieldActionEvent)
		{
			Vector3 vector = Quaternion.LookRotation(transform.forward, Vector3.up) * specialFieldActionEvent.SpawnOffset;
			Vector3 position = transform.position;
			objectManager.SpawnSpecialField(networkObjectId, specialFieldActionEvent.SpecialFieldId, position + vector, specialFieldActionEvent.Scale);
		}
	}

	public static ActionStatsData ToStatsData(this IActionStats actionStats)
	{
		return new ActionStatsData(actionStats);
	}

	public static void UpdateCharacterSkin(this Skeleton skeleton, int skinPresetId, int weaponId, bool isSuper = false)
	{
		if (skeleton.Data == null)
		{
			return;
		}
		TCharacterSkinPreset tCharacterSkinPreset = TCharacterSkinPreset.Table[skinPresetId];
		Skin skin = new Skin("custom");
		if (tCharacterSkinPreset.SkinBaseId > 0)
		{
			Skin skin2 = skeleton.Data.FindSkin(string.Format("{0}{1}", isSuper ? "body_super/skinbase_" : "body/skinbase_", tCharacterSkinPreset.SkinBaseId));
			if (skin2 != null)
			{
				skin.AddSkin(skin2);
			}
		}
		if (tCharacterSkinPreset.EyeSkinId > 0)
		{
			Skin skin3 = skeleton.Data.FindSkin(string.Format("{0}{1}", isSuper ? "body_super/eye_" : "body/eye_", tCharacterSkinPreset.EyeSkinId));
			if (skin3 != null)
			{
				skin.AddSkin(skin3);
			}
		}
		if (tCharacterSkinPreset.HairSkinId > 0)
		{
			Skin skin4 = skeleton.Data.FindSkin(string.Format("{0}{1}", isSuper ? "body_super/hair_" : "body/hair_", tCharacterSkinPreset.HairSkinId));
			if (skin4 != null)
			{
				skin.AddSkin(skin4);
			}
		}
		if (tCharacterSkinPreset.CostumeSkinId > 0)
		{
			Skin skin5 = skeleton.Data.FindSkin(string.Format("{0}{1}", isSuper ? "body_super/costume_" : "body/costume_", tCharacterSkinPreset.CostumeSkinId));
			if (skin5 != null)
			{
				skin.AddSkin(skin5);
			}
		}
		if (tCharacterSkinPreset.AccSkinId > 0)
		{
			Skin skin6 = skeleton.Data.FindSkin(string.Format("{0}{1}", isSuper ? "body_super/acc_" : "body/acc_", tCharacterSkinPreset.AccSkinId));
			if (skin6 != null)
			{
				skin.AddSkin(skin6);
			}
		}
		string text = ((weaponId > 0) ? TEquipment.Table[weaponId].WeaponSkinName : "dummy");
		Skin skin7 = skeleton.Data.FindSkin((isSuper ? "weapon_super/" : "weapon/") + text);
		if (skin7 == null)
		{
			skin7 = skeleton.Data.FindSkin("weapon/dummy");
		}
		if (skin7 != null)
		{
			skin.AddSkin(skin7);
		}
		skeleton.SetSkin(skin);
		skeleton.SetSlotsToSetupPose();
	}

	public static void SetMultiSkin(this Skeleton skeleton, int skinId)
	{
		Skin skin = skeleton.Data.FindSkin(skinId.ToString());
		if (skin != null)
		{
			skeleton.SetSkin(skin);
		}
	}

	public static void AddStack(this UnitBuff unitBuff, UnitModel owner, IActionStats actionStats, float timeNow, int stack = 1)
	{
		TBuff buffData = TBuff.Table[unitBuff.Id];
		if (!(buffData.Duration < 0f))
		{
			ExtendDuration();
		}
		StoreStackEntries();

		void ExtendDuration()
		{
			if (!(buffData.Duration < 0f))
			{
				unitBuff.EndTime = timeNow + owner.GetModifiedBuffDuration(buffData);
			}
		}

		void StoreStackEntries()
		{
			int stackCount = unitBuff.StackCount;
			int num = Math.Min(stack + stackCount, buffData.MaxStackCount);
			int num2 = num - unitBuff.StackCount;
			if (stack >= 1 && num2 >= 1)
			{
				int stackCount2 = unitBuff.StackCount;
				bool flag = buffData.BuffEffects != null && buffData.BuffEffects.Any((BuffEffect _) => _.UseStatValue);
				UnitStat[] casterStats = ((actionStats != null && flag) ? GetUnitStats(buffData.BuffEffects, actionStats) : Array.Empty<UnitStat>());
				BuffStackEntry element = new BuffStackEntry
				{
					CasterStats = casterStats
				};
				unitBuff.StackEntries = Enumerable.Repeat(element, num2).Concat(unitBuff.StackEntries?.Take(stackCount2) ?? Enumerable.Empty<BuffStackEntry>()).ToArray();
				unitBuff.SyncStackCount = unitBuff.StackEntries.Length;
			}
		}

		UnitStat[] GetUnitStats(System.Collections.Generic.IEnumerable<BuffEffect> effects, IActionStats stats)
		{
			return (from _ in effects
				where _.UseStatValue
				group _ by _.StatType).Select((IGrouping<EStatType, BuffEffect> group) => new UnitStat
			{
				StatType = group.Key,
				Value = stats.GetStat(group.Key)
			}).ToArray();
		}
	}

	public static void RemoveStack(this UnitBuff unitBuff, int stack = 1)
	{
		if (unitBuff.StackCount > 0 && stack > 0)
		{
			int num = unitBuff.StackCount - stack;
			int stackCount = unitBuff.StackCount;
			if (num < 1)
			{
				num = 0;
			}
			unitBuff.StackEntries = unitBuff.StackEntries.Skip(stackCount - num).ToArray();
			unitBuff.SyncStackCount = unitBuff.StackEntries.Length;
		}
	}

	public static float GetStat(this UnitBuff unitBuff, EStatType statType)
	{
		TBuff tBuff = TBuff.Table[unitBuff.Id];
		if (tBuff.BuffEffects == null)
		{
			return 0f;
		}
		return tBuff.BuffEffects.Where((BuffEffect _) => _.EffectType == EBuffEffectType.Stat).Select((BuffEffect _) => _ as StatEffect).Where((StatEffect statEffect) => statEffect.statType == statType)
			.Sum(delegate(StatEffect statEffect)
			{
				float num = unitBuff.StackCount * statEffect.statValue;
				if (statEffect.UseStatValue)
				{
					float num2 = ((!statEffect.RealtimeStatValue) ? ((unitBuff.StackEntries != null) ? unitBuff.StackEntries.GetCasterStatSum(statEffect.StatType) : 0f) : unitBuff.GetRealtimeCasterStatSum(statEffect.StatType));
					num += num2 * statEffect.StatPercent / 100f;
				}
				return num;
			});
	}

	public static bool HasDirectPathBetween(this UnitModel unitModel, UnitModel target)
	{
		Vector3 vector = unitModel.transform.position + Vector3.up * 0.5f;
		Vector3 vector2 = target.transform.position + Vector3.up * 0.5f;
		Vector3 normalized = (vector2 - vector).normalized;
		float distance = (float)Math.Sqrt((vector - vector2).sqrMagnitude);
		return !Physics.Raycast(vector, normalized, out var _, distance, Const.LayerMask.HIGH_OBSTACLE);
	}

	public static HitLayerInfo GetHitLayerInfo(this IHitable hitable)
	{
		int factionId = ((hitable is MonsterModel monsterModel) ? monsterModel.FactionId : (-1));
		return new HitLayerInfo
		{
			FactionId = factionId,
			HitLayer = hitable.HitLayer
		};
	}
}
