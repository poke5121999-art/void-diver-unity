using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using GameCommon.Data;
using ProtoBuf.Meta;
using UnityEngine;

public static class ProtoBufAotInitializer
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		RuntimeTypeModel model = RuntimeTypeModel.Default;
		model.Add(typeof(Goods), true);
		model.Add(typeof(GoldGoods), true);
		model.Add(typeof(CoinGoods), true);
		model.Add(typeof(ExpGoods), true);
		model.Add(typeof(ConsumableGoods), true);
		model.Add(typeof(ValuableGoods), true);
		model.Add(typeof(MiscGoods), true);
		model.Add(typeof(NoteGoods), true);
		model.Add(typeof(BlueprintGoods), true);
		model.Add(typeof(WeaponGoods), true);
		model.Add(typeof(AccessoryGoods), true);
		model.Add(typeof(ArtifactGoods), true);
		model.Add(typeof(BagGoods), true);
		model.Add(typeof(MusicDiscGoods), true);
		model.Add(typeof(BaseMessage), true);
		model.Add(typeof(ConnectionData), true);
		model.Add(typeof(ClientRtt), true);
		model.Add(typeof(StageResultPlayerInfo), true);
		model.Add(typeof(StageResultLocalInfo), true);
		model.Add(typeof(ZoneData), true);
		model.Add(typeof(PlayerContextData), true);
		model.Add(typeof(AccountContextData), true);
		model.Add(typeof(AccountRecordContextData), true);
		model.Add(typeof(SoundSettingContextData), true);
		model.Add(typeof(GraphicSettingContextData), true);
		model.Add(typeof(GamePlaySettingContextData), true);
		model.Add(typeof(Profile), true);
		model.Add(typeof(Talent), true);
		model.Add(typeof(Character), true);
		model.Add(typeof(GoodsSlot), true);
		model.Add(typeof(LostGoods), true);
		model.Add(typeof(LostItemSelectInfo), true);
		model.Add(typeof(LoungeQuest), true);
		model.Add(typeof(LoungeQuestTask), true);
		model.Add(typeof(Employee), true);
		model.Add(typeof(FiredEmployeeRecord), true);
		model.Add(typeof(SalesSlot), true);
		model.Add(typeof(MonsterRecord), true);
		model.Add(typeof(DialogReplayRecord), true);
		model.Add(typeof(ActiveAreaDecoration), true);
		model.Add(typeof(NpcLevelRecord), true);
		model.Add(typeof(PlayerStore), true);
		model.Add(typeof(PlayerStoreEntry), true);
		model.Add(typeof(ShopProduct), true);
		model.Add(typeof(PlayerSlot), true);
		model.Add(typeof(AccountRecordEntry), true);
		model.Add(typeof(GamePlayerData), true);
		model.Add(typeof(PointMarkers), true);
		model.Add(typeof(UnitStat), true);
		model.Add(typeof(UnitSkill), true);
		model.Add(typeof(UnitBuff), true);
		model.Add(typeof(UnitCc), true);
		model.Add(typeof(UnitItemCoolTime), true);
		model.Add(typeof(BuffStackEntry), true);
		model.Add(typeof(Stat), true);
		model.Add(typeof(EventCondition), true);
		model.Add(typeof(FixedSector), true);
		model.Add(typeof(TalentEffect), true);
		model.Add(typeof(BitFlags), true);
		model.Add(typeof(ProtoVector2), true);
		model.Add(typeof(ProtoVector3), true);
		model.Add(typeof(AppVersion), true);
		model.Add(typeof(AnimationSpeed), true);
		Prewarm(model);
	}

	private static void Prewarm(RuntimeTypeModel model)
	{
		List<Type> concreteTypes = new List<Type>();
		List<Type> abstractTypes = new List<Type>();
		foreach (MetaType metaType in model.GetTypes())
		{
			Type type = metaType.Type;
			if (type == null || type.ContainsGenericParameters)
			{
				continue;
			}
			if (type.IsAbstract || type.IsInterface)
			{
				abstractTypes.Add(type);
			}
			else
			{
				concreteTypes.Add(type);
			}
		}
		using MemoryStream stream = new MemoryStream(256);
		foreach (Type concrete in concreteTypes)
		{
			try
			{
				object instance = FormatterServices.GetUninitializedObject(concrete);
				stream.SetLength(0L);
				model.Serialize(stream, instance);
			}
			catch (Exception)
			{
			}
		}
		MethodInfo serialize = typeof(SerializeHelper).GetMethod("Serialize");
		foreach (Type abstractType in abstractTypes)
		{
			Type derived = concreteTypes.Find(abstractType.IsAssignableFrom);
			if (derived == null)
			{
				continue;
			}
			try
			{
				object instance = FormatterServices.GetUninitializedObject(derived);
				serialize.MakeGenericMethod(new Type[] { abstractType }).Invoke(null, new object[] { instance });
			}
			catch (Exception)
			{
			}
		}
	}
}
