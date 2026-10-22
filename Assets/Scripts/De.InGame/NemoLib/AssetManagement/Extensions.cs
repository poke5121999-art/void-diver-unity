using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using UnityEngine;

namespace NemoLib.AssetManagement
{
	public static class Extensions
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CGetRecentlyCacheUnitSkeletonAsync_003Ed__27 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<UnitSkeletonData> _003C_003Et__builder;

			public AssetManager manager;

			public string category;

			public string key;

			private UniTask<UnitSkeletonData>.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CInstantiateSectorAsync_003Ed__38 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder<GameObject> _003C_003Et__builder;

			public AssetManager assetManager;

			public int sectorId;

			public Vector3 position;

			public Quaternion rotation;

			public Transform parentTf;

			private UniTask<GameObject>.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		public static Sprite GetIconSpeaker(this AssetManager assetManager, ESpeaker speaker, int tableId, int playerCharacterId, string postFix)
		{
			return null;
		}

		public static Sprite GetIconUnitCharacter(this AssetManager assetManager, int characterId, string postFix = "Default")
		{
			return null;
		}

		public static Sprite GetIconMonster(this AssetManager assetManager, int monsterId)
		{
			return null;
		}

		public static Sprite GetIconStatusEffectTag(this AssetManager assetManager, EStatusEffectTag tagName)
		{
			return null;
		}

		public static Sprite GetIconWeakPoint(this AssetManager assetManager, EElementalType elementType)
		{
			return null;
		}

		public static Sprite GetIconBuff(this AssetManager assetManager, int buffId)
		{
			return null;
		}

		public static Sprite GetIconAnomaly(this AssetManager assetManager, EAnomalyType anomalyType)
		{
			return null;
		}

		public static Sprite GetIconGoods(this AssetManager assetManager, EGoodsType type, int id)
		{
			return null;
		}

		public static Sprite GetIconCurrency(this AssetManager assetManager, EGoodsType type)
		{
			return null;
		}

		public static Sprite GetIconGoodsSubType(this AssetManager assetManager, EGoodsType type)
		{
			return null;
		}

		public static Sprite GetIconItem(this AssetManager assetManager, int itemId)
		{
			return null;
		}

		public static Sprite GetIconEquipment(this AssetManager assetManager, int equipmentId)
		{
			return null;
		}

		public static Sprite GetIconBag(this AssetManager assetManager, int bagId)
		{
			return null;
		}

		public static Sprite GetIconSkill(this AssetManager assetManager, int skillId)
		{
			return null;
		}

		public static Sprite GetIconPing(this AssetManager assetManager, EPingType pingType)
		{
			return null;
		}

		public static Sprite GetIconEmoji(this AssetManager assetManager, int emojiId)
		{
			return null;
		}

		public static Sprite GetIconTalent(this AssetManager assetManager, int talentId)
		{
			return null;
		}

		public static Sprite GetIconEmployeeCondition(this AssetManager assetManager, int stress, bool isPanic)
		{
			return null;
		}

		public static Sprite GetIconEmployeeFace(this AssetManager assetManager, int employeeId)
		{
			return null;
		}

		public static Sprite GetIconRoomDifficulty(this AssetManager assetManager, ERoomDifficulty difficulty)
		{
			return null;
		}

		public static Sprite GetIconInterior(this AssetManager assetManager, EInteriorType type, int id)
		{
			return null;
		}

		public static Sprite GetIconArea(this AssetManager assetManager, int areaId)
		{
			return null;
		}

		public static Sprite GetIconAreaDecoration(this AssetManager assetManager, int areaDecorationId)
		{
			return null;
		}

		public static Sprite GetIconNpc(this AssetManager assetManager, int npcId)
		{
			return null;
		}

		public static UniTask<Sprite> GetTexturePortraitAsync(this AssetManager assetManager, ESpeaker speaker, string imageName)
		{
			return default;
		}

		public static UniTask<Sprite> GetTextureMiniMapAsync(this AssetManager assetManager, int sectorId)
		{
			return default;
		}

		public static UniTask<Sprite> GetDialogImageAsync(this AssetManager assetManager, string imageName)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CGetRecentlyCacheUnitSkeletonAsync_003Ed__27))]
		private static UniTask<UnitSkeletonData> GetRecentlyCacheUnitSkeletonAsync(this AssetManager manager, string category, string key)
		{
			return default;
		}

		public static UniTask<UnitSkeletonData> GetCharacterSkeletonDataAsync(this AssetManager assetManager, int characterId)
		{
			return default;
		}

		public static UniTask<UnitSkeletonData> GetNpcSkeletonDataAsync(this AssetManager assetManager, int npcId)
		{
			return default;
		}

		public static IntroTimelineObject InstantiateIntroTimelineObject(this AssetManager assetManager)
		{
			return null;
		}

		public static CutsceneTimelineObject InstantiateCutscene(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static GameObject InstantiateSequentialTimeline(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static CharacterController InstantiateCharacter(this AssetManager assetManager, int characterId)
		{
			return null;
		}

		public static PetController InstantiatePet(this AssetManager assetManager, int petId)
		{
			return null;
		}

		public static MonsterController InstantiateMonster(this AssetManager assetManager, int monsterId)
		{
			return null;
		}

		public static ExtraUnitController InstantiateExtraUnit(this AssetManager assetManager, int extraUnitId)
		{
			return null;
		}

		public static NpcObject InstantiateNpc(this AssetManager assetManager, int npcId)
		{
			return null;
		}

		[AsyncStateMachine(typeof(_003CInstantiateSectorAsync_003Ed__38))]
		public static UniTask<GameObject> InstantiateSectorAsync(this AssetManager assetManager, int sectorId, Transform parentTf, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			return default;
		}

		public static GameObject InstantiateAreaDecorationObject(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static LockedAreaObject InstantiateLockedAreaObject(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static EnvironmentProp InstantiateEnvironmentProp(this AssetManager assetManager, string prefab)
		{
			return null;
		}

		public static BreakableProp InstantiateBreakableProp(this AssetManager assetManager, int breakablePropId)
		{
			return null;
		}

		public static SpecialField InstantiateSpecialField(this AssetManager assetManager, int specialFieldId)
		{
			return null;
		}

		public static PhoneBooth InstantiatePhoneBooth(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject InstantiateMiniMapMarker(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static GameObject InstantiateHitBox(this AssetManager assetManager, ECollisionType collisionType)
		{
			return null;
		}

		public static DisposableObject InstantiateDisposableObject(this AssetManager assetManager, int disposableObjectId)
		{
			return null;
		}

		public static Portal InstantiatePortal(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static ExitObject InstantiateExit(this AssetManager assetManager, EExitType exitType)
		{
			return null;
		}

		public static TrapObject InstantiateTrap(this AssetManager assetManager, int trapId)
		{
			return null;
		}

		public static Entrance InstantiateEntrance(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static CollisionTrigger InstantiateCollisionTrigger(this AssetManager assetManager)
		{
			return null;
		}

		public static InteractiveTrigger InstantiateInteractiveTrigger(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static VisualProp InstantiateVisualProp(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static GameObject InstantiateIndicatorVfx(this AssetManager assetManager, string vfxName)
		{
			return null;
		}

		public static DropGoods InstantiateDropGoods(this AssetManager assetManager)
		{
			return null;
		}

		public static RewardBox InstantiateRewardBox(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static MimicObject InstantiateMimic(this AssetManager assetManager, string prefabName)
		{
			return null;
		}

		public static WaveExecutor InstantiateWaveExecutor(this AssetManager assetManager, int waveExecutorId)
		{
			return null;
		}

		public static SkillExecutor InstantiateSkillExecutor(this AssetManager assetManager, int skillExecutorId)
		{
			return null;
		}

		public static GameObject InstantiateGoodsSlot(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject InstantiateStageResultParadoxSlot(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject InstantiateBuffSlot(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject InstantiateTalentSlot(this AssetManager assetManager)
		{
			return null;
		}

		public static UINodeLink InstantiateNodeLink(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject SpawnVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			return null;
		}

		public static GameObject SpawnSkillVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			return null;
		}

		public static StatusVfx SpawnStatusVfx(this AssetManager assetManager, string vfxName, Transform parentTf = null)
		{
			return null;
		}

		public static GameObject SpawnPointerArrow(this AssetManager assetManager)
		{
			return null;
		}

		public static GameObject SpawnPing(this AssetManager assetManager, EPingType pingType)
		{
			return null;
		}

		public static GameFloatingText SpawnFloatingText(this AssetManager assetManager, EFloatingTextType textType)
		{
			return null;
		}

		public static GameObject SpawnEmoji(this AssetManager assetManager, int emojiId, Transform parentTf)
		{
			return null;
		}

		public static UniTask<SectorSkinPreset> GetSectorSkinPresetAsync(this AssetManager assetManager, string presetName)
		{
			return default;
		}
	}
}
