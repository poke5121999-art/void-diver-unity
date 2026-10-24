using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public static class AotInitializer
{
	[Preserve]
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void EnsureReferenced()
	{
		if (Environment.HasShutdownStarted)
		{
			AotGenericInstantiationHints();
		}
	}

	[Preserve]
	public static void AotGenericInstantiationHints()
	{
		if (Environment.HasShutdownStarted)
		{
			HintProtoBuf();
			HintNewtonsoftJson();
			HintJsonUtility();
			HintEnumTryParse();
			HintCollections();
			HintUniRx();
			HintUniTask();
			HintTrackable();
		}
	}

	[Preserve]
	private static void HintProtoBuf()
	{
		MemoryStream memoryStream = new MemoryStream();
		Serializer.Serialize(memoryStream, default(GameCommon.Data.Stat));
		Serializer.Deserialize<GameCommon.Data.Stat>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.Stat?));
		Serializer.Deserialize<GameCommon.Data.Stat?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.Stat[]));
		Serializer.Deserialize<GameCommon.Data.Stat[]>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.EventCondition));
		Serializer.Deserialize<GameCommon.Data.EventCondition>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.EventCondition?));
		Serializer.Deserialize<GameCommon.Data.EventCondition?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.EventCondition[]));
		Serializer.Deserialize<GameCommon.Data.EventCondition[]>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.FixedSector));
		Serializer.Deserialize<GameCommon.Data.FixedSector>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.FixedSector?));
		Serializer.Deserialize<GameCommon.Data.FixedSector?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.FixedSector[]));
		Serializer.Deserialize<GameCommon.Data.FixedSector[]>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.TalentEffect));
		Serializer.Deserialize<GameCommon.Data.TalentEffect>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.TalentEffect?));
		Serializer.Deserialize<GameCommon.Data.TalentEffect?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.TalentEffect[]));
		Serializer.Deserialize<GameCommon.Data.TalentEffect[]>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.GoodsData));
		Serializer.Deserialize<GameCommon.Data.GoodsData>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Data.ArtifactDealCardEffect));
		Serializer.Deserialize<GameCommon.Data.ArtifactDealCardEffect>(memoryStream);
		Serializer.Serialize(memoryStream, default(bool?));
		Serializer.Deserialize<bool?>(memoryStream);
		Serializer.Serialize(memoryStream, default(byte?));
		Serializer.Deserialize<byte?>(memoryStream);
		Serializer.Serialize(memoryStream, default(int?));
		Serializer.Deserialize<int?>(memoryStream);
		Serializer.Serialize(memoryStream, default(long?));
		Serializer.Deserialize<long?>(memoryStream);
		Serializer.Serialize(memoryStream, default(float?));
		Serializer.Deserialize<float?>(memoryStream);
		Serializer.Serialize(memoryStream, default(double?));
		Serializer.Deserialize<double?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Enum.EGoodsType?));
		Serializer.Deserialize<GameCommon.Enum.EGoodsType?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Enum.EGoodsCategory?));
		Serializer.Deserialize<GameCommon.Enum.EGoodsCategory?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Enum.EStatType?));
		Serializer.Deserialize<GameCommon.Enum.EStatType?>(memoryStream);
		Serializer.Serialize(memoryStream, default(Tracker.EDataOperation?));
		Serializer.Deserialize<Tracker.EDataOperation?>(memoryStream);
		Serializer.Serialize(memoryStream, default(GameCommon.Enum.ERoomDifficulty?));
		Serializer.Deserialize<GameCommon.Enum.ERoomDifficulty?>(memoryStream);
		TouchProto<GameCommon.Data.Goods>(memoryStream);
		TouchProto<BaseMessage>(memoryStream);
		TouchProto<GoodsSlot>(memoryStream);
		TouchProto<PlayerContextData>(memoryStream);
		TouchProto<BitFlags>(memoryStream);
		TouchProto<GameCommon.Data.GoldGoods>(memoryStream);
		TouchProto<GameCommon.Data.CoinGoods>(memoryStream);
		TouchProto<GameCommon.Data.ExpGoods>(memoryStream);
		TouchProto<GameCommon.Data.ConsumableGoods>(memoryStream);
		TouchProto<GameCommon.Data.ValuableGoods>(memoryStream);
		TouchProto<GameCommon.Data.MiscGoods>(memoryStream);
		TouchProto<GameCommon.Data.NoteGoods>(memoryStream);
		TouchProto<GameCommon.Data.BlueprintGoods>(memoryStream);
		TouchProto<GameCommon.Data.WeaponGoods>(memoryStream);
		TouchProto<GameCommon.Data.AccessoryGoods>(memoryStream);
		TouchProto<GameCommon.Data.ArtifactGoods>(memoryStream);
		TouchProto<GameCommon.Data.BagGoods>(memoryStream);
	}

	[Preserve]
	private static void TouchProto<T>(MemoryStream stream)
	{
		// shared body @0x180da0c90 (machine code checked): Serializer.Serialize(stream, default(T)) then a tail call to Serializer.Deserialize<T>(stream)
		Serializer.Serialize(stream, default(T));
		Serializer.Deserialize<T>(stream);
	}

	[Preserve]
	private static void HintNewtonsoftJson()
	{
		JsonConvert.SerializeObject(default(GameCommon.Data.Stat));
		JsonConvert.DeserializeObject<GameCommon.Data.Stat>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.Stat>());
		JsonConvert.DeserializeObject<GameCommon.Data.Stat[]>(string.Empty);
		JsonConvert.SerializeObject(default(GameCommon.Data.EventCondition));
		JsonConvert.DeserializeObject<GameCommon.Data.EventCondition>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.EventCondition>());
		JsonConvert.DeserializeObject<GameCommon.Data.EventCondition[]>(string.Empty);
		JsonConvert.SerializeObject(default(GameCommon.Data.FixedSector));
		JsonConvert.DeserializeObject<GameCommon.Data.FixedSector>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.FixedSector>());
		JsonConvert.DeserializeObject<GameCommon.Data.FixedSector[]>(string.Empty);
		JsonConvert.SerializeObject(default(GameCommon.Data.TalentEffect));
		JsonConvert.DeserializeObject<GameCommon.Data.TalentEffect>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.TalentEffect>());
		JsonConvert.DeserializeObject<GameCommon.Data.TalentEffect[]>(string.Empty);
		JsonConvert.SerializeObject(default(GameCommon.Data.GoodsData));
		JsonConvert.DeserializeObject<GameCommon.Data.GoodsData>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.GoodsData>());
		JsonConvert.DeserializeObject<GameCommon.Data.GoodsData[]>(string.Empty);
		JsonConvert.SerializeObject(default(GameCommon.Data.ArtifactDealCardEffect));
		JsonConvert.DeserializeObject<GameCommon.Data.ArtifactDealCardEffect>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<GameCommon.Data.ArtifactDealCardEffect>());
		JsonConvert.DeserializeObject<GameCommon.Data.ArtifactDealCardEffect[]>(string.Empty);
		JsonConvert.SerializeObject(default(UnityEngine.Vector2));
		JsonConvert.DeserializeObject<UnityEngine.Vector2>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<UnityEngine.Vector2>());
		JsonConvert.DeserializeObject<UnityEngine.Vector2[]>(string.Empty);
		JsonConvert.SerializeObject(default(UnityEngine.Vector3));
		JsonConvert.DeserializeObject<UnityEngine.Vector3>(string.Empty);
		JsonConvert.SerializeObject(Array.Empty<UnityEngine.Vector3>());
		JsonConvert.DeserializeObject<UnityEngine.Vector3[]>(string.Empty);
		JsonConvert.DeserializeObject<GameCommon.Data.Interior>(string.Empty);
		JsonConvert.DeserializeObject<GameCommon.Data.Interior[]>(string.Empty);
	}

	[Preserve]
	private static void HintJsonUtility()
	{
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.Stat[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.EventCondition>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.EventCondition[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.FixedSector[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.TalentEffect[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.GoodsData>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.GoodsData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.ArtifactDealCardEffect[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<UnityEngine.Vector3>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.Interior>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.HitBoxInfo>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.AiSkillCondition>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.SkillBuffStackCondition>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<WavePhase[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<MatchingLobby>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.BuffEffect[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.BuffTriggerBase[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.BuffDurationOverride>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.ActionEvent[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.SkillActionNode>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.ZoneSpawnDistanceData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GameCommon.Data.ZoneObjectSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<ZonePointData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<MonsterSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<DropRewardSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<RewardBoxSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<TrapSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<ExitSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<SkillExecutorSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<WaveExecutorSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<PhoneBoothSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<NpcSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<GuestSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<DisposableObjectSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<BreakablePropSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<EntranceSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<CollisionTriggerSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<TextMarkerSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<PortalSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<PortalExitData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<SpecialFieldSpawnGroupData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<LockedAreaObjectSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<AreaDecorationObjectSpawnData[]>(string.Empty);
		SomeNet.Utility.JsonUtility.Deserialize<MimicSpawnGroupData[]>(string.Empty);
	}

	[Preserve]
	private static void HintEnumTryParse()
	{
		Enum.TryParse(string.Empty, out GameCommon.Enum.EStatType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.ETalentEffectType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EEventConditionType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EArtifactDealCardEffectType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EGoodsDataType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EInteriorType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.ERoomDifficulty _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.ERoomPingFilterType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EDialogType _);
		Enum.TryParse(string.Empty, out GameCommon.Enum.EDialogPurpose _);
	}

	[Preserve]
	private static void HintCollections()
	{
		new System.Collections.Generic.List<GameCommon.Data.Stat>();
		new System.Collections.Generic.List<GameCommon.Data.EventCondition>();
		new System.Collections.Generic.List<GameCommon.Data.FixedSector>();
		new System.Collections.Generic.List<GameCommon.Data.TalentEffect>();
		new System.Collections.Generic.List<GameCommon.Data.GoodsData>();
		new System.Collections.Generic.List<GameCommon.Data.ArtifactDealCardEffect>();
		new System.Collections.Generic.List<GameCommon.Enum.EArtifactCategory>();
		new System.Collections.Generic.List<GameCommon.Enum.EWeaponType>();
		new System.Collections.Generic.List<GameCommon.Enum.EStatType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EStatType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EGoodsType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EGoodsCategory>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EWeaponType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EArtifactCategory>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EConsumableType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.ECrowdControlType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.ETalentEffectType>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EHitTarget>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.ERoomDifficulty>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EActionState>();
		new System.Collections.Generic.HashSet<GameCommon.Enum.EFeatureUnlock>();
		new System.Collections.Generic.HashSet<EArtifactDealStepDelay>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EStatType, float>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EStatType, UnitStat>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EStatType, GameCommon.Enum.EElementalType>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EGoodsType, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EGoodsCategory, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.ERoomDifficulty, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EConsumableType, UnitItemCoolTime>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.ECrowdControlType, UnitCc>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.ETalentEffectType, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.ETalentEffectType, System.Collections.Generic.List<GameCommon.Data.TalentEffect>>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EWeaponType, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EArtifactCategory, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EEventConditionType, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.ENpcFunctionType, int>();
		new System.Collections.Generic.Dictionary<EInputActionType, int>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EStatusEffectTag, StatusVfx>();
		new System.Collections.Generic.Dictionary<GameCommon.Enum.EActionState, De.InGame.Fsm.UnitStateBase>();
		new System.Collections.Generic.Dictionary<EPetState, De.InGame.Fsm.PetStateBase>();
		new System.Collections.Generic.Dictionary<ulong, GameCommon.Enum.ECampaignState>();
	}

	[Preserve]
	private static void HintUniRx()
	{
		new UniRx.Subject<int>();
		new UniRx.Subject<bool>();
		new UniRx.Subject<float>();
		new UniRx.Subject<long>();
		new UniRx.Subject<ulong>();
		new UniRx.Subject<UnityEngine.Vector3>();
		new UniRx.Subject<UnityEngine.Vector2>();
		new UniRx.Subject<GameCommon.Enum.EStatType>();
		new UniRx.Subject<GameCommon.Enum.EActionState>();
		new UniRx.Subject<GameCommon.Enum.EStatusEffectTag>();
		new UniRx.Subject<GameCommon.Enum.EElementalType>();
		new UniRx.Subject<GameCommon.Enum.EPingType>();
		new UniRx.Subject<GameCommon.Enum.EGoodsType>();
		new UniRx.Subject<GameCommon.Enum.EGoodsCategory>();
		new UniRx.Subject<GameCommon.Enum.EGoodsSlotCategory>();
		new UniRx.Subject<GameCommon.Enum.ECampaignState>();
		new UniRx.Subject<GameCommon.Enum.ERoomDifficulty>();
		new UniRx.Subject<GameCommon.Enum.ELanguage>();
		new UniRx.ReactiveProperty<int>();
		new UniRx.ReactiveProperty<bool>();
		new UniRx.ReactiveProperty<float>();
		new UniRx.ReactiveProperty<long>();
		new UniRx.ReactiveProperty<ulong>();
		new UniRx.ReactiveProperty<UnityEngine.Vector3>();
		new UniRx.ReactiveProperty<UnityEngine.Vector2>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EStatType>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EActionState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EScreenMode>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EResolution>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EVoteStateLayout>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EGoodsSlotCategory>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EDifficulty>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ERoomDifficulty>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ERoomPingFilterType>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EStressState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ECorruptionState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EHitBoxDespawnReason>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EElementalType>();
		new UniRx.ReactiveProperty<ERoomState>();
		new UniRx.ReactiveProperty<ERoomConnectState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ECampaignState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EButtonState>();
		new UniRx.ReactiveProperty<EPetState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EStageState>();
		new UniRx.ReactiveProperty<EQualityLevel>();
		new UniRx.ReactiveProperty<EFrameRate>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ELanguage>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EAnomalyType>();
		new UniRx.ReactiveProperty<GameCommon.Enum.EExitState>();
		new UniRx.ReactiveProperty<GameCommon.Enum.ENpcState>();
		new UniRx.ReactiveProperty<EArtifactDealState>();
		new UniRx.ReactiveProperty<float>().Pairwise();
		new UniRx.ReactiveProperty<int>().Pairwise();
		new UniRx.ReactiveProperty<bool>().Pairwise();
		new UniRx.ReactiveProperty<UnityEngine.Vector3>().Pairwise();
		new UniRx.ReactiveProperty<GameCommon.Enum.EActionState>().Pairwise();
		new UniRx.ReactiveProperty<GameCommon.Enum.EStatType>().Pairwise();
		new UniRx.ReactiveProperty<GameCommon.Enum.ECorruptionState>().Pairwise();
	}

	[Preserve]
	private static void HintUniTask()
	{
		UniTask.FromResult(false);
		UniTask.FromResult(0);
		UniTask.FromResult(0L);
		UniTask.FromResult(0f);
		UniTask.FromResult(0UL);
		UniTask.FromResult(default(GameCommon.Enum.ELoadResult));
		new UniTaskCompletionSource<bool>();
		new UniTaskCompletionSource<int>();
		new UniTaskCompletionSource<float>();
		new UniTaskCompletionSource<GameCommon.Enum.ELoadResult>();
	}

	[Preserve]
	private static void HintTrackable()
	{
		new Tracker.TrackableDictionary<GameCommon.Enum.EStatType, UnitStat>();
		new Tracker.TrackableDictionary<GameCommon.Enum.ECrowdControlType, UnitCc>();
		new Tracker.TrackableDictionary<GameCommon.Enum.EConsumableType, UnitItemCoolTime>();
		new Tracker.TrackableDictionary<int, UnitSkill>();
		new Tracker.TrackableDictionary<int, UnitBuff>();
		new Tracker.TrackableDictionary<int, GoodsSlot>();
		new Tracker.TrackableDictionary<int, ShopProduct>();
	}
}
