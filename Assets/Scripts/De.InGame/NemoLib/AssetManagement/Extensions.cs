using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public static class Extensions
	{
		public static Sprite GetIconSpeaker(this AssetManager assetManager, ESpeaker speaker, int tableId, int playerCharacterId, string postFix)
		{
			string spriteName;
			string atlasName;
			if (speaker == ESpeaker.PlayerCharacter)
			{
				spriteName = string.Format("{0}_{1}", playerCharacterId, postFix);
				atlasName = "Icon_Unit_Character";
			}
			else
			{
				spriteName = string.Format("{0}_{1}", tableId, postFix);
				switch (speaker)
				{
				case ESpeaker.Player:
				case ESpeaker.Character:
					atlasName = "Icon_Unit_Character";
					break;
				case ESpeaker.Npc:
					atlasName = "Icon_Unit_Npc";
					break;
				case ESpeaker.Monster:
					atlasName = "Icon_Unit_Monster";
					break;
				default:
					throw new ArgumentOutOfRangeException("speaker", speaker, "Invalid comparison type.");
				}
			}
			return assetManager.GetSprite(atlasName, spriteName);
		}

		public static Sprite GetIconUnitCharacter(this AssetManager assetManager, int characterId, string postFix = "Default")
		{
			string spriteName = string.Format("{0}_{1}", characterId, postFix);
			Sprite sprite = assetManager.GetSprite("Icon_Unit_Character", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconMonster(this AssetManager assetManager, int monsterId)
		{
			string spriteName = string.Format("{0}", monsterId);
			Sprite sprite = assetManager.GetSprite("Icon_Monster", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconStatusEffectTag(this AssetManager assetManager, EStatusEffectTag tagName)
		{
			string spriteName = tagName.ToString();
			return assetManager.GetSprite("Icon_StatusEffectTag", spriteName);
		}

		public static Sprite GetIconWeakPoint(this AssetManager assetManager, EElementalType elementType)
		{
			string spriteName = "WeakPoint_" + elementType;
			return assetManager.GetSprite("Icon_WeakPoint", spriteName);
		}

		public static Sprite GetIconBuff(this AssetManager assetManager, int buffId)
		{
			string spriteName = string.Format("{0}", buffId);
			Sprite sprite = assetManager.GetSprite("Icon_Buff", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconAnomaly(this AssetManager assetManager, EAnomalyType anomalyType)
		{
			string spriteName = string.Format("{0}", anomalyType);
			Sprite sprite = assetManager.GetSprite("Icon_Anomaly", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconGoods(this AssetManager assetManager, EGoodsType type, int id)
		{
			string text;
			string atlasName;
			switch (type.GoodsCategory())
			{
			case EGoodsCategory.Currency:
				return assetManager.GetIconCurrency(type);
			case EGoodsCategory.Item:
				text = string.Format("{0}", id);
				atlasName = "Icon_Item";
				break;
			case EGoodsCategory.Equipment:
				text = string.Format("{0}", id);
				atlasName = "Icon_Equipment";
				break;
			case EGoodsCategory.Bag:
				text = string.Format("{0}", id);
				atlasName = "Icon_Bag";
				break;
			default:
				return null;
			}
			Sprite sprite = assetManager.GetSprite(atlasName, text);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconCurrency(this AssetManager assetManager, EGoodsType type)
		{
			string spriteName;
			switch (type)
			{
			case EGoodsType.Gold:
				spriteName = "Gold";
				break;
			case EGoodsType.Coin:
				spriteName = "Coin";
				break;
			case EGoodsType.Exp:
				spriteName = "Exp";
				break;
			default:
				return null;
			}
			return assetManager.GetSprite("Icon_Common", spriteName);
		}

		public static Sprite GetIconGoodsSubType(this AssetManager assetManager, EGoodsType type)
		{
			string spriteName = string.Format("{0}", type);
			return assetManager.GetSprite("Icon_GoodsSubType", spriteName);
		}

		public static Sprite GetIconItem(this AssetManager assetManager, int itemId)
		{
			string spriteName = string.Format("{0}", itemId);
			Sprite sprite = assetManager.GetSprite("Icon_Item", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconEquipment(this AssetManager assetManager, int equipmentId)
		{
			string spriteName = string.Format("{0}", equipmentId);
			Sprite sprite = assetManager.GetSprite("Icon_Equipment", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconBag(this AssetManager assetManager, int bagId)
		{
			string spriteName = string.Format("{0}", bagId);
			Sprite sprite = assetManager.GetSprite("Icon_Bag", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconSkill(this AssetManager assetManager, int skillId)
		{
			string spriteName = string.Format("{0}", skillId);
			return assetManager.GetSprite("Icon_Skill", spriteName);
		}

		public static Sprite GetIconPing(this AssetManager assetManager, EPingType pingType)
		{
			string spriteName = string.Format("{0}", pingType);
			Sprite sprite = assetManager.GetSprite("Icon_Ping", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconEmoji(this AssetManager assetManager, int emojiId)
		{
			string spriteName = string.Format("{0}", emojiId);
			Sprite sprite = assetManager.GetSprite("Icon_Emoji", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconTalent(this AssetManager assetManager, int talentId)
		{
			string spriteName = string.Format("{0}", talentId);
			Sprite sprite = assetManager.GetSprite("Icon_Talent", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconEmployeeCondition(this AssetManager assetManager, int stress, bool isPanic)
		{
			int stressMax = TConst.StressMax.IntValue;
			int percent = ((stressMax > 0) ? (stress * 100 / stressMax) : 0);
			int level = (isPanic ? 5 : ((percent < 50) ? ((percent > 24) ? 2 : 1) : ((percent > 74) ? 4 : 3)));
			string spriteName = string.Format("Condition{0}", level);
			Sprite sprite = assetManager.GetSprite("Icon_EmployeeCondition", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconEmployeeFace(this AssetManager assetManager, int employeeId)
		{
			string spriteName = string.Format("{0}", employeeId);
			Sprite sprite = assetManager.GetSprite("Icon_EmployeeFace", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconRoomDifficulty(this AssetManager assetManager, ERoomDifficulty difficulty)
		{
			string spriteName = difficulty.ToString();
			return assetManager.GetSprite("Icon_Common", spriteName);
		}

		public static Sprite GetIconInterior(this AssetManager assetManager, EInteriorType type, int id)
		{
			string atlasName;
			switch (type)
			{
			case EInteriorType.Area:
				atlasName = "Icon_Area";
				break;
			case EInteriorType.NpcLevel:
				atlasName = "Icon_Area_Npc";
				break;
			case EInteriorType.AreaDecoration:
				atlasName = "Icon_AreaDecoration";
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			string spriteName = string.Format("{0}", id);
			Sprite sprite = assetManager.GetSprite(atlasName, spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconArea(this AssetManager assetManager, int areaId)
		{
			string spriteName = string.Format("{0}", areaId);
			Sprite sprite = assetManager.GetSprite("Icon_Area", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconAreaDecoration(this AssetManager assetManager, int areaDecorationId)
		{
			string spriteName = string.Format("{0}", areaDecorationId);
			Sprite sprite = assetManager.GetSprite("Icon_AreaDecoration", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static Sprite GetIconNpc(this AssetManager assetManager, int npcId)
		{
			string spriteName = string.Format("{0}", npcId);
			Sprite sprite = assetManager.GetSprite("Icon_Area_Npc", spriteName);
			if (sprite == null)
			{
				sprite = assetManager.GetSprite("Icon_Common", "Default");
			}
			return sprite;
		}

		public static UniTask<Sprite> GetTexturePortraitAsync(this AssetManager assetManager, ESpeaker speaker, string imageName)
		{
			string key = string.Format("{0}/{1}/{2}.png", "Portrait", (speaker == ESpeaker.PlayerCharacter) ? ESpeaker.Character : speaker, imageName);
			return assetManager.GetRecentlyCacheTextureAsync("Portrait", key);
		}

		public static UniTask<Sprite> GetTextureMiniMapAsync(this AssetManager assetManager, int sectorId)
		{
			string key = string.Format("{0}/{1}.png", "MiniMap", sectorId);
			return assetManager.GetRecentlyCacheTextureAsync("MiniMap", key);
		}

		public static UniTask<Sprite> GetDialogImageAsync(this AssetManager assetManager, string imageName)
		{
			string key = "DialogImage/" + imageName + ".png";
			return assetManager.GetRecentlyCacheTextureAsync("DialogImage", key);
		}

		public static async UniTask<UnitSkeletonData> GetRecentlyCacheUnitSkeletonAsync(this AssetManager manager, string category, string key)
		{
			return await manager.GetRecentlyCacheAsync<UnitSkeletonData>(category, key);
		}

		public static UniTask<UnitSkeletonData> GetCharacterSkeletonDataAsync(this AssetManager assetManager, int characterId)
		{
			string key = string.Format("{0}/{1}.asset", "Character", characterId);
			return assetManager.GetRecentlyCacheUnitSkeletonAsync("Character", key);
		}

		public static UniTask<UnitSkeletonData> GetNpcSkeletonDataAsync(this AssetManager assetManager, int npcId)
		{
			string key = string.Format("{0}/{1}.asset", "Npc", npcId);
			return assetManager.GetRecentlyCacheUnitSkeletonAsync("Npc", key);
		}

		public static IntroTimelineObject InstantiateIntroTimelineObject(this AssetManager assetManager)
		{
			return assetManager.Instantiate<IntroTimelineObject>("Timeline/IntroTimelineObject.prefab");
		}

		public static CutsceneTimelineObject InstantiateCutscene(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<CutsceneTimelineObject>("Timeline/" + prefabName + ".prefab");
		}

		public static GameObject InstantiateSequentialTimeline(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate("Timeline/" + prefabName + ".prefab");
		}

		public static CharacterController InstantiateCharacter(this AssetManager assetManager, int characterId)
		{
			string key = string.Format("{0}/{1}.prefab", "Unit/Character", characterId);
			return assetManager.Instantiate<CharacterController>(key);
		}

		public static PetController InstantiatePet(this AssetManager assetManager, int petId)
		{
			string key = string.Format("{0}/{1}.prefab", "Unit/Pet", petId);
			return assetManager.Instantiate<PetController>(key);
		}

		public static MonsterController InstantiateMonster(this AssetManager assetManager, int monsterId)
		{
			string key = string.Format("{0}/{1}.prefab", "Unit/Monster", monsterId);
			return assetManager.Instantiate<MonsterController>(key);
		}

		public static ExtraUnitController InstantiateExtraUnit(this AssetManager assetManager, int extraUnitId)
		{
			string key = string.Format("{0}/{1}.prefab", "Unit/ExtraUnit", extraUnitId);
			return assetManager.Instantiate<ExtraUnitController>(key);
		}

		public static NpcObject InstantiateNpc(this AssetManager assetManager, int npcId)
		{
			string key = string.Format("{0}/{1}.prefab", "Object/Npc", npcId);
			return assetManager.Instantiate<NpcObject>(key);
		}

		public static async UniTask<GameObject> InstantiateSectorAsync(this AssetManager assetManager, int sectorId, Transform parentTf, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			assetManager.SetCapacity<GameObject>("Sector", 15);
			string key = string.Format("{0}/{1}.prefab", "Sector", sectorId);
			GameObject original = await assetManager.GetRecentlyCacheAsync<GameObject>("Sector", key);
			GameObject gameObject = UnityEngine.Object.Instantiate(original, position, rotation, parentTf);
			gameObject.transform.parent = parentTf;
			gameObject.transform.position = position;
			return gameObject;
		}

		public static GameObject InstantiateAreaDecorationObject(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate("Object/AreaDecorationObject/" + prefabName + ".prefab");
		}

		public static LockedAreaObject InstantiateLockedAreaObject(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<LockedAreaObject>("Object/LockedAreaObject/" + prefabName + ".prefab");
		}

		public static EnvironmentProp InstantiateEnvironmentProp(this AssetManager assetManager, string prefab)
		{
			return assetManager.Instantiate<EnvironmentProp>("Object/EnvironmentProp/" + prefab + ".prefab");
		}

		public static BreakableProp InstantiateBreakableProp(this AssetManager assetManager, int breakablePropId)
		{
			TBreakableProp tBreakableProp = TBreakableProp.Table[breakablePropId];
			return assetManager.Instantiate<BreakableProp>("Object/BreakableProp/" + tBreakableProp.PrefabName + ".prefab");
		}

		public static SpecialField InstantiateSpecialField(this AssetManager assetManager, int specialFieldId)
		{
			TSpecialField tSpecialField = TSpecialField.Table[specialFieldId];
			return assetManager.Instantiate<SpecialField>("Object/SpecialField/" + tSpecialField.PrefabName + ".prefab");
		}

		public static PhoneBooth InstantiatePhoneBooth(this AssetManager assetManager)
		{
			return assetManager.Instantiate<PhoneBooth>("Object/Etc/PhoneBooth.prefab");
		}

		public static GameObject InstantiateMiniMapMarker(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate("EventUi/MiniMapMarker/" + prefabName + ".prefab");
		}

		public static GameObject InstantiateHitBox(this AssetManager assetManager, ECollisionType collisionType)
		{
			string key = string.Format("{0}/HitBox{1}.prefab", "Combat/HitBox", collisionType);
			return assetManager.Instantiate(key);
		}

		public static DisposableObject InstantiateDisposableObject(this AssetManager assetManager, int disposableObjectId)
		{
			TDisposableObject tDisposableObject = TDisposableObject.Table[disposableObjectId];
			return assetManager.Instantiate<DisposableObject>("Object/DisposableObject/" + tDisposableObject.PrefabName + ".prefab");
		}

		public static Portal InstantiatePortal(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<Portal>("Object/Portal/" + prefabName + ".prefab");
		}

		public static ExitObject InstantiateExit(this AssetManager assetManager, EExitType exitType)
		{
			string key = string.Format("{0}/{1}.prefab", "Object/Exit", exitType);
			return assetManager.Instantiate<ExitObject>(key);
		}

		public static TrapObject InstantiateTrap(this AssetManager assetManager, int trapId)
		{
			string key = string.Format("{0}/{1}.prefab", "Object/Trap", trapId);
			return assetManager.Instantiate<TrapObject>(key);
		}

		public static Entrance InstantiateEntrance(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<Entrance>("Object/Entrance/" + prefabName + ".prefab");
		}

		public static CollisionTrigger InstantiateCollisionTrigger(this AssetManager assetManager)
		{
			return assetManager.Instantiate<CollisionTrigger>("Object/Etc/CollisionTrigger.prefab");
		}

		public static InteractiveTrigger InstantiateInteractiveTrigger(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<InteractiveTrigger>("Object/InteractiveTrigger/" + prefabName + ".prefab");
		}

		public static VisualProp InstantiateVisualProp(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<VisualProp>("Object/VisualProp/" + prefabName + ".prefab");
		}

		public static GameObject InstantiateIndicatorVfx(this AssetManager assetManager, string vfxName)
		{
			return assetManager.Instantiate("Combat/SkillIndicator/Vfx/IndicatorVfx_" + vfxName + ".prefab");
		}

		public static DropGoods InstantiateDropGoods(this AssetManager assetManager)
		{
			return assetManager.Instantiate<DropGoods>("Object/DropProp/DropGoods.prefab");
		}

		public static RewardBox InstantiateRewardBox(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<RewardBox>("Object/RewardBox/" + prefabName + ".prefab");
		}

		public static MimicObject InstantiateMimic(this AssetManager assetManager, string prefabName)
		{
			return assetManager.Instantiate<MimicObject>("Object/Mimic/" + prefabName + ".prefab");
		}

		public static WaveExecutor InstantiateWaveExecutor(this AssetManager assetManager, int waveExecutorId)
		{
			TWaveExecutor tWaveExecutor = TWaveExecutor.Table[waveExecutorId];
			return assetManager.Instantiate<WaveExecutor>("Object/WaveExecutor/" + tWaveExecutor.PrefabName + ".prefab");
		}

		public static SkillExecutor InstantiateSkillExecutor(this AssetManager assetManager, int skillExecutorId)
		{
			TSkillExecutor tSkillExecutor = TSkillExecutor.Table[skillExecutorId];
			return assetManager.Instantiate<SkillExecutor>("Object/SkillExecutor/" + tSkillExecutor.PrefabName + ".prefab");
		}

		public static GameObject InstantiateGoodsSlot(this AssetManager assetManager)
		{
			return assetManager.Instantiate("Slot/GoodsSlot.prefab");
		}

		public static GameObject InstantiateStageResultParadoxSlot(this AssetManager assetManager)
		{
			return assetManager.Instantiate("Slot/StageResultParadoxSlot.prefab");
		}

		public static GameObject InstantiateBuffSlot(this AssetManager assetManager)
		{
			return assetManager.Instantiate("Slot/InGameBuffSlot.prefab");
		}

		public static GameObject InstantiateTalentSlot(this AssetManager assetManager)
		{
			return assetManager.Instantiate("Slot/TalentSlot.prefab");
		}

		public static UINodeLink InstantiateNodeLink(this AssetManager assetManager)
		{
			return assetManager.Instantiate<UINodeLink>("Component/NodeLink.prefab");
		}

		public static GameObject SpawnVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			string key = "Vfx/" + vfxName + ".prefab";
			return assetManager.EnsureSpawn(key, 1, 10, true, parentTf);
		}

		public static GameObject SpawnSkillVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			string key = "Vfx/Skill/" + vfxName + ".prefab";
			return assetManager.EnsureSpawn(key, 1, 10, true, parentTf);
		}

		public static StatusVfx SpawnStatusVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			string key = "Vfx/Status/" + vfxName + ".prefab";
			return assetManager.EnsureSpawn<StatusVfx>(key, 1, 10, true, parentTf);
		}

		public static GameObject SpawnPointerArrow(this AssetManager assetManager)
		{
			return assetManager.EnsureSpawn("Object/Etc/PointerArrow.prefab", 1, 10, true);
		}

		public static GameObject SpawnPing(this AssetManager assetManager, EPingType pingType)
		{
			string key = string.Format("{0}/{1}.prefab", "EventUi/Ping", pingType);
			return assetManager.EnsureSpawn(key, 1, 10, true);
		}

		public static GameFloatingText SpawnFloatingText(this AssetManager assetManager, EFloatingTextType textType)
		{
			string key = string.Format("{0}/{1}.prefab", "EventUi/FloatingText", textType);
			return assetManager.EnsureSpawn<GameFloatingText>(key, 1, 10, true);
		}

		public static GameObject SpawnEmoji(this AssetManager assetManager, int emojiId, Transform parentTf)
		{
			string key = string.Format("{0}/{1}.prefab", "EventUi/Emoji", emojiId);
			return assetManager.EnsureSpawn(key, 1, 10, true, parentTf);
		}

		public static UniTask<SectorSkinPreset> GetSectorSkinPresetAsync(this AssetManager assetManager, string presetName)
		{
			string key = "Scriptable/SectorSkinPreset/" + presetName + ".asset";
			return assetManager.GetRecentlyCacheAsync<SectorSkinPreset>("Scriptable/SectorSkinPreset", key);
		}
	}
}
