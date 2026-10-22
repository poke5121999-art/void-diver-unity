using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using UniRx;
using UnityEngine.SceneManagement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Popup;
using UnityEngine;
using VContainer;

public class SROptions : NativeBase, INotifyPropertyChanged
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
	{
		public DisplayNameAttribute(string displayName)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class IncrementAttribute : Attribute
	{
		public IncrementAttribute(double increment)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class NumberRangeAttribute : Attribute
	{
		public NumberRangeAttribute(double min, double max)
		{
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class SortAttribute : Attribute
	{
		public SortAttribute(int priority)
		{
		}
	}

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private ILuaRunner _luaRunner;

	[Inject]
	private GameLuaController _luaController;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private GameChatManager _gameChatManager;

	[Inject]
	private AppManager _appManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameEventSystem _eventSystem;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameLoungeQuestManager _loungeQuestManager;

	[CompilerGenerated]
	private PropertyChangedEventHandler m_PropertyChangedEventHandler;

	private const string CATEGORY_CHARACTER_HP = "02. Character >> Hp";

	private const string CATEGORY_CHARACTER_STRESS = "02. Character >> Stress";

	private const string CATEGORY_CHARACTER_CORRUPTION = "02. Character >> Corruption";

	private const string CATEGORY_CHARACTER_SOUL = "02. Character >> Soul";

	private const string CATEGORY_CHARACTER_BRIGHTNESS = "02. Character >> Brightness";

	private const string CATEGORY_CHARACTER_POSITION = "02. Character >> Position";

	private const string CATEGORY_CHARACTER_BUFF = "02. Character >> Buff";

	private const string CATEGORY_CHARACTER_OPTIONS = "02. Character >> Options";

	private Vector3 _characterPosition;

	private const string CATEGORY_INVENTORY = "03. Inventory";

	private const string CATEGORY_SPAWN = "04. Spawn";

	private const string CATEGORY_SYSTEM_QUEST = "05. System >> Quest";

	private const string CATEGORY_SYSTEM_MONSTER = "05. System >> Monster";

	private const string CATEGORY_SYSTEM_PARADOX = "05. System >> Paradox";

	private const string CATEGORY_SYSTEM_MAP = "05. System >> Map";

	private const string CATEGORY_SYSTEM_LUA = "05. System >> Lua";

	private const string CATEGORY_SYSTEM_BLUEPRINT = "05. System >> Blueprint";

	private const string CATEGORY_SYSTEM_INTERIOR = "05. System >> Interior";

	private const string CATEGORY_SYSTEM_ETC = "05. System >> Etc";

	private const string CATEGORY_SYSTEM_ARTIFACT_DEAL = "05. System >> ArtifactDeal";

	private const string CATEGORY_TEMP = "99. Temp";

	private const string CATEGORY_TYCOON = "08. Tycoon";

	private const string CATEGORY_USER = "01. User";

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp] HP")]
	[Sort(10)]
	public int CharacterHp { get; set; }

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress] Stress")]
	[Sort(30)]
	public float CharacterStress { get; set; }

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption] Corruption")]
	[Sort(40)]
	public int CharacterCorruption { get; set; }

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 소울 변화량")]
	[Sort(50)]
	public int CharacterSoul { get; set; }

	[Category("02. Character >> Brightness")]
	[DisplayName("[g:brightness] Brightness")]
	[Sort(60)]
	public int LightFuel { get; set; }

	[Category("02. Character >> Position")]
	[DisplayName("[g:pos] Position")]
	[Sort(70)]
	public float CharacterPositionX { get; set; }

	[Category("02. Character >> Position")]
	[DisplayName("[g:pos]")]
	[Sort(71)]
	public float CharacterPositionY { get; set; }

	[Category("02. Character >> Position")]
	[DisplayName("[g:pos]")]
	[Sort(72)]
	public float CharacterPositionZ { get; set; }

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff] 버프 ID")]
	[Sort(90)]
	public int BuffId { get; set; }

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff_func] 상태 타입")]
	[Sort(92)]
	public EStatusEffectTag StatusEffectTag { get; set; }

	[Category("02. Character >> Options")]
	[DisplayName("무적")]
	[Sort(100)]
	public bool CharacterInvincibility { get; set; }

	[Category("02. Character >> Options")]
	[DisplayName("투명")]
	[Sort(101)]
	public bool CharacterInvisible { get; set; }

	[Category("02. Character >> Options")]
	[DisplayName("가속")]
	[Sort(102)]
	public bool CharacterSpeedUp { get; set; }

	[Category("03. Inventory")]
	[DisplayName("[g:item] 아이템 ID")]
	[Sort(10)]
	public int ItemId { get; set; }

	[Category("03. Inventory")]
	[DisplayName("[g:item] 아이템 개수")]
	[Sort(11)]
	public int ItemCount { get; set; }

	[Category("03. Inventory")]
	[DisplayName("[g:equip] 장비 ID")]
	[Sort(20)]
	public int CreateEquipmentId { get; set; }

	[Category("03. Inventory")]
	[DisplayName("[g:equip] 내구도 비율 (%)")]
	[Sort(24)]
	[NumberRange(0.0, 100.0)]
	public int EquipmentDurabilityPercent { get; set; }

	[Category("03. Inventory")]
	[DisplayName("[g:bag] 가방 ID")]
	[Sort(30)]
	public int BagId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:reward_box] RewardBox ID")]
	[Sort(10)]
	public int RewardBoxId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:entrance] Entrance ID")]
	[Sort(20)]
	public int EntranceId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:breakable] BreakableProp ID")]
	[Sort(30)]
	public int BreakablePropId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:special_field] SpecialField ID")]
	[Sort(40)]
	public int SpecialFieldId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:monster_1] 몬스터 ID")]
	[Sort(200)]
	public int MonsterId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:monster_2] 몬스터 수")]
	[Sort(201)]
	public int MonsterCount { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:npc] Npc ID")]
	[Sort(301)]
	public int NpcId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:wave] 웨이브 ID")]
	[Sort(311)]
	public int WaveExecutorId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:skill_exec] 스킬 익스큐터 ID")]
	[Sort(321)]
	public int SkillExecutorId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:trap_1] Trap ID")]
	[Sort(401)]
	public int TrapId { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:trap_2] Rotation Y")]
	[Sort(402)]
	public float RotationY { get; set; }

	[Category("04. Spawn")]
	[DisplayName("[g:mimic] Mimic ID")]
	[Sort(501)]
	public int MimicId { get; set; }

	[Category("05. System >> Quest")]
	[DisplayName("[g:campaign] Campaign ID")]
	[Sort(1)]
	public int CampaignId { get; set; }

	[Category("05. System >> Quest")]
	[DisplayName("[g:mission] Mission Ids")]
	[Sort(3)]
	public string MissionIds { get; set; }

	[Category("05. System >> Quest")]
	[DisplayName("[g:lounge_quest] LoungeQuest ID")]
	[Sort(12)]
	public int LoungeQuestId { get; set; }

	[Category("05. System >> Quest")]
	[DisplayName("[g:lounge_quest] LoungeQuest State")]
	[Sort(13)]
	public ELoungeQuestState LoungeQuestState { get; set; }

	[Category("05. System >> Monster")]
	[DisplayName("[g:monster] 몬스터 HP")]
	[Sort(1)]
	public int MonsterHp { get; set; }

	[Category("05. System >> Paradox")]
	[DisplayName("[g:paradox] 패러독스 ID")]
	[Sort(1)]
	public int ParadoxId { get; set; }

	[Category("05. System >> Map")]
	[DisplayName("[g:map_2] 이상현상 타입")]
	[Sort(3)]
	public EAnomalyType AnomalyType { get; set; }

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_1] Lua Key")]
	[Sort(1)]
	public string LuaKey { get; set; }

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_2] Lua Function")]
	[Sort(2)]
	public string LuaFunction { get; set; }

	[Category("05. System >> Interior")]
	[DisplayName("[g:interior] InteriorShop ID")]
	[Sort(1)]
	public int InteriorShopId { get; set; }

	[Category("05. System >> ArtifactDeal")]
	[DisplayName("[g:artifact] 유물협상 Id")]
	[Sort(1)]
	public int ArtifactDealId { get; set; }

	[Category("08. Tycoon")]
	[DisplayName("[g:stress] 직원 스트레스 값")]
	[Sort(10)]
	public int EmployeeStressValue { get; set; }

	[Category("01. User")]
	[DisplayName("[g:gold] 골드")]
	[Sort(1)]
	public int GoldValue { get; set; }

	[Category("01. User")]
	[DisplayName("[g:coin] 코인")]
	[Sort(11)]
	public int CoinValue { get; set; }

	[Category("01. User")]
	[DisplayName("[g:exp] Exp")]
	[Sort(3)]
	public int ExpValue { get; set; }

	[Category("01. User")]
	[DisplayName("[g:util_0] 튜토리얼 무시")]
	[Sort(101)]
	public bool IgnoreTutorial
	{
		get
		{
			return _gameManager.IgnoreTutorial;
		}
		set
		{
			_gameManager.SetIgnoreTutorial(value);
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_0] 라운지 튜토 무시")]
	[Sort(102)]
	public bool IgnoreLoungeTutorial
	{
		get
		{
			return _gameManager.IgnoreLoungeTutorial;
		}
		set
		{
			_gameManager.SetIgnoreLoungeTutorial(value);
		}
	}

	private event PropertyChangedEventHandler PropertyChangedEventHandler
	{
		[CompilerGenerated]
		add
		{
			PropertyChangedEventHandler propertyChangedEventHandler = m_PropertyChangedEventHandler;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = System.Threading.Interlocked.CompareExchange(ref m_PropertyChangedEventHandler, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
		[CompilerGenerated]
		remove
		{
			PropertyChangedEventHandler propertyChangedEventHandler = m_PropertyChangedEventHandler;
			PropertyChangedEventHandler propertyChangedEventHandler2;
			do
			{
				propertyChangedEventHandler2 = propertyChangedEventHandler;
				PropertyChangedEventHandler value2 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
				propertyChangedEventHandler = System.Threading.Interlocked.CompareExchange(ref m_PropertyChangedEventHandler, value2, propertyChangedEventHandler2);
			}
			while ((object)propertyChangedEventHandler != propertyChangedEventHandler2);
		}
	}

	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
	{
		add
		{
			PropertyChangedEventHandler += value;
		}
		remove
		{
			PropertyChangedEventHandler -= value;
		}
	}

	public SROptions()
	{
		EquipmentDurabilityPercent = 100;
		GoldValue = 1000000;
		CoinValue = 1000000;
		ExpValue = 1000000;
	}

	private bool CheckPlayingGame()
	{
		if (_gameManager.MyCharacterModel != null)
		{
			return true;
		}
		_logger.Warning("Player is not currently in the game.");
		return false;
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp] 저장")]
	[Sort(11)]
	public void ApplyCharacterHp()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterHp { Hp = CharacterHp });
		}
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 회복")]
	[Sort(20)]
	public void RecoverCharacter()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterHp { Hp = (int)_gameManager.MyCharacterModel.HpMax });
		}
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 자살")]
	[Sort(21)]
	public void KillCharacter()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqKillCharacter());
		}
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 부활")]
	[Sort(22)]
	public void ReviveCharacter()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatReviveCharacter());
		}
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress] 저장")]
	[Sort(31)]
	public void ApplyCharacterStress()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterStress { Stress = CharacterStress });
		}
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 경고")]
	[Sort(32)]
	public void ChangeCharacterAlert()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterStress { Stress = 0f });
		}
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 공포")]
	[Sort(33)]
	public void ChangeCharacterFear()
	{
		if (CheckPlayingGame())
		{
			int stressMax = TStress.Table[EStressState.Alert].StressMax;
			_gameChannel.SendToHost(new ReqSetCharacterStress { Stress = stressMax + 1 });
		}
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 절망")]
	[Sort(34)]
	public void ChangeCharacterDespair()
	{
		if (CheckPlayingGame())
		{
			int stressMax = TStress.Table[EStressState.Fear].StressMax;
			_gameChannel.SendToHost(new ReqSetCharacterStress { Stress = stressMax + 1 });
		}
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption] 적용")]
	[Sort(41)]
	public void ApplyCharacterCorruption()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterCorruption { Corruption = CharacterCorruption });
		}
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Safe")]
	[Sort(42)]
	public void ChangeCorruptionSafe()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterCorruption { Corruption = TConst.CorruptionMax.IntValue });
		}
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Caution")]
	[Sort(43)]
	public void ChangeCorruptionCaution()
	{
		if (CheckPlayingGame())
		{
			int corruptionMax = TConst.CorruptionMax.IntValue;
			int threshold = TCorruption.Table[ECorruptionState.Caution].Threshold;
			_gameChannel.SendToHost(new ReqSetCharacterCorruption { Corruption = threshold + corruptionMax });
		}
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Alert")]
	[Sort(44)]
	public void ChangeCorruptionAlert()
	{
		if (CheckPlayingGame())
		{
			int corruptionMax = TConst.CorruptionMax.IntValue;
			int threshold = TCorruption.Table[ECorruptionState.Alert].Threshold;
			_gameChannel.SendToHost(new ReqSetCharacterCorruption { Corruption = threshold + corruptionMax });
		}
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Serious")]
	[Sort(45)]
	public void ChangeCorruptionSerious()
	{
		if (CheckPlayingGame())
		{
			int corruptionMax = TConst.CorruptionMax.IntValue;
			int threshold = TCorruption.Table[ECorruptionState.Serious].Threshold;
			_gameChannel.SendToHost(new ReqSetCharacterCorruption { Corruption = threshold + corruptionMax });
		}
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 적용")]
	[Sort(51)]
	public void ApplyCharacterSoul()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatApplySoul { Soul = CharacterSoul });
		}
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 소울 최대치")]
	[Sort(52)]
	public void ApplyMaxSoul()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatApplySoul { Soul = TConst.SoulMax.IntValue });
		}
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 소울 0으로")]
	[Sort(53)]
	public void ApplyMinSoul()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatApplySoul { Soul = -TConst.SoulMax.IntValue });
		}
	}

	[Category("02. Character >> Brightness")]
	[DisplayName("[g:brightness] 적용")]
	[Sort(61)]
	public void ApplyCharacterBrightness()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqSetCharacterLightFuel { LightFuel = LightFuel });
		}
	}

	[Category("02. Character >> Brightness")]
	[DisplayName("[g:brightness] 회복")]
	[Sort(62)]
	public void RecoverCharacterBrightness()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatRecoverBrightness());
		}
	}

	[Category("02. Character >> Position")]
	[DisplayName("[g:pos] 이동")]
	[Sort(73)]
	public void MoveCharacter()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatMovePosition
			{
				Position = new ProtoVector3
				{
					X = CharacterPositionX,
					Y = CharacterPositionY,
					Z = CharacterPositionZ
				}
			});
		}
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff] 버프 추가")]
	[Sort(91)]
	public void AddBuff()
	{
		if (!_gameManager.IsHost)
		{
			_logger.Warning("AddBuff can only be executed by the host.");
			return;
		}
		int buffId = BuffId;
		if (!TBuff.Table.ContainsKey(buffId))
		{
			_logger.Warning(string.Format("Buff with ID {0} does not exist in the table.", buffId));
			return;
		}
		_logger.Info(string.Format("{0}", buffId), new Color(0f, 1f, 0f, 1f));
		_gameManager.MyCharacterModel.AddBuff(buffId, _gameManager.MyCharacterModel, 1, 0uL, Vector3.zero);
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff_func] 상태 버프 추가")]
	[Sort(93)]
	public void AddStatusEffectBuff()
	{
		if (!_gameManager.IsHost)
		{
			_logger.Warning("AddBuff can only be executed by the host.");
			return;
		}
		int buffId = (int)StatusEffectTag + 1001000;
		if (!TBuff.Table.ContainsKey(buffId))
		{
			_logger.Warning(string.Format("Buff with ID {0} does not exist in the table.", buffId));
			return;
		}
		_logger.Info(string.Format("{0}", buffId), new Color(0f, 1f, 0f, 1f));
		_gameManager.MyCharacterModel.AddBuff(buffId, _gameManager.MyCharacterModel, 1, 0uL, Vector3.zero);
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff_func] 버프 리스트 확인")]
	[Sort(94)]
	public void ShowBuffs()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("Current Buffs : {0}", _gameManager.MyCharacterModel.Buffs.Count));
		foreach (KeyValuePair<int, UnitBuff> pair in _gameManager.MyCharacterModel.Buffs)
		{
			stringBuilder.AppendLine(string.Format("Buff ID : {0}, Stack : {1}", pair.Key, pair.Value.StackCount));
		}
		_logger.Info(stringBuilder.ToString(), new Color(0f, 1f, 0f, 1f));
	}

	[Category("02. Character >> Options")]
	[DisplayName("저장")]
	[Sort(103)]
	public void SaveCharacterOptions()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatApplyCharacterOptions
			{
				Invisible = CharacterInvisible,
				Invincibility = CharacterInvincibility,
				SpeedUp = CharacterSpeedUp
			});
		}
	}

	[Category("02. Character >> Options")]
	[DisplayName("[g:opt_func] 쿨타임 초기화")]
	[Sort(104)]
	public void ResetSkillCooldown()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatResetSkillCooldown());
		}
	}

	[Category("02. Character >> Options")]
	[DisplayName("[g:opt_func] 스킬 차지 최대")]
	[Sort(105)]
	public void MaxSkillCharge()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSkillCharge());
		}
	}

	[Category("02. Character >> Options")]
	[DisplayName("캐릭터 스킨 언락")]
	[Sort(106)]
	public void UnlockCharacterSkins()
	{
		if (CheckPlayingGame())
		{
			foreach (int skinId in TCharacterSkin.Table.Keys)
			{
				_playerManager.UnlockPlayerCharacterSkin(skinId);
			}
		}
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 아이템 지급 (인벤)")]
	[Sort(13)]
	public void AddInventoryItem()
	{
		if (CheckPlayingGame() && TItem.Table.ContainsKey(ItemId))
		{
			_gameChannel.SendToHost(new ReqCheatAddGoods
			{
				Goods = new Goods[1] { GoodsCreator.MakeItemGoods(ItemId, 0L, ItemCount) }
			});
		}
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (창고)")]
	[Sort(14)]
	public void AddAllItems()
	{
		Goods[] goods = TItem.Table.Values.Where((TItem item) => !item.IsEventItem).Select<TItem, Goods>(delegate(TItem item)
			{
				int id = item.Id;
				switch (TItem.Table[id].GoodsType)
				{
					case EGoodsType.Consumable:
						return new ConsumableGoods(id, 0L, 10L);
					case EGoodsType.Valuable:
						return new ValuableGoods(id, 0L, 10L);
					case EGoodsType.Misc:
						return new MiscGoods(id, 0L, 10L);
					case EGoodsType.Note:
						return new NoteGoods(id, 0L, 10L);
					case EGoodsType.Blueprint:
						return new BlueprintGoods(id, 0L, 10L);
					case EGoodsType.MusicDisc:
						return new MusicDiscGoods(id, 0L, 10L);
					default:
						throw new InvalidOperationException(string.Format("Unsupported item goods type: id={0}, type={1}", id, TItem.Table[id].GoodsType));
				}
			}).ToList().Shuffle().Take(10).ToArray();
		_gameChannel.SendToHost(new ReqCheatAddGoods { Goods = goods });
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 도면 (창고)")]
	[Sort(15)]
	public void AddRandomBlueprintItems()
	{
		Goods[] goods = TItem.Table.Values.Where((TItem item) => item.GoodsType == EGoodsType.Blueprint).Select<TItem, Goods>(delegate(TItem item)
			{
				int id = item.Id;
				switch (TItem.Table[id].GoodsType)
				{
					case EGoodsType.Consumable:
						return new ConsumableGoods(id, 0L, 1L);
					case EGoodsType.Valuable:
						return new ValuableGoods(id, 0L, 1L);
					case EGoodsType.Misc:
						return new MiscGoods(id, 0L, 1L);
					case EGoodsType.Note:
						return new NoteGoods(id, 0L, 1L);
					case EGoodsType.Blueprint:
						return new BlueprintGoods(id, 0L, 1L);
					case EGoodsType.MusicDisc:
						return new MusicDiscGoods(id, 0L, 1L);
					default:
						throw new InvalidOperationException(string.Format("Unsupported item goods type: id={0}, type={1}", id, TItem.Table[id].GoodsType));
				}
			}).ToList().Shuffle().Take(50).ToArray();
		_gameChannel.SendToHost(new ReqCheatAddGoods { Goods = goods });
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (Storage)")]
	[Sort(16)]
	public void AddRandomItemsToStorage()
	{
		if (!CheckPlayingGame())
		{
			return;
		}
		foreach (Goods goods in TItem.Table.Values.Where((TItem item) => !item.IsEventItem).ToList().Shuffle().Take(10).Select<TItem, Goods>(delegate(TItem item)
			{
				int id = item.Id;
				switch (TItem.Table[id].GoodsType)
				{
					case EGoodsType.Consumable:
						return new ConsumableGoods(id, 0L, 1L);
					case EGoodsType.Valuable:
						return new ValuableGoods(id, 0L, 1L);
					case EGoodsType.Misc:
						return new MiscGoods(id, 0L, 1L);
					case EGoodsType.Note:
						return new NoteGoods(id, 0L, 1L);
					case EGoodsType.Blueprint:
						return new BlueprintGoods(id, 0L, 1L);
					case EGoodsType.MusicDisc:
						return new MusicDiscGoods(id, 0L, 1L);
					default:
						throw new InvalidOperationException(string.Format("Unsupported item goods type: id={0}, type={1}", id, TItem.Table[id].GoodsType));
				}
			}))
		{
			_playerManager.AddGoodsToStorage(goods);
		}
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (분실물)")]
	[Sort(17)]
	public void AddRandomItemsToLostItem()
	{
		if (CheckPlayingGame())
		{
			IList<Goods> lostGoods = TItem.Table.Values.Where((TItem item) => !item.IsEventItem).ToList().Shuffle().Take(10).Select<TItem, Goods>(delegate(TItem item)
			{
				int id = item.Id;
				switch (TItem.Table[id].GoodsType)
				{
					case EGoodsType.Consumable:
						return new ConsumableGoods(id, 0L, 1L);
					case EGoodsType.Valuable:
						return new ValuableGoods(id, 0L, 1L);
					case EGoodsType.Misc:
						return new MiscGoods(id, 0L, 1L);
					case EGoodsType.Note:
						return new NoteGoods(id, 0L, 1L);
					case EGoodsType.Blueprint:
						return new BlueprintGoods(id, 0L, 1L);
					case EGoodsType.MusicDisc:
						return new MusicDiscGoods(id, 0L, 1L);
					default:
						throw new InvalidOperationException(string.Format("Unsupported item goods type: id={0}, type={1}", id, TItem.Table[id].GoodsType));
				}
			}).ToList();
			_playerManager.RegisterLostGoods(lostGoods);
		}
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 장비 지급 (인벤)")]
	[Sort(22)]
	public void AddInventoryEquipment()
	{
		if (!CheckPlayingGame() || !TEquipment.Table.ContainsKey(CreateEquipmentId))
		{
			return;
		}
		TEquipment tEquipment = TEquipment.Table[CreateEquipmentId];
		Goods goods;
		if (tEquipment.IsArtifact())
		{
			int[] prefixes = tEquipment.PickPrefixes();
			goods = GoodsCreator.MakeEquipmentGoods(CreateEquipmentId, 0L, 1L, prefixes, null, null, (byte)_gameContext.VariantDifficulty);
		}
		else
		{
			int maxDurability = tEquipment.GetMaxDurability(1f);
			goods = GoodsCreator.MakeEquipmentGoods(CreateEquipmentId, 0L, 1L, null, maxDurability, null, 1);
		}
		_gameChannel.SendToHost(new ReqCheatAddGoods { Goods = new Goods[1] { goods } });
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 모든 장비 내구도 일괄 변경")]
	[Sort(25)]
	public void SetAllEquipmentDurability()
	{
		if (!CheckPlayingGame())
		{
			return;
		}
		if (!_gameManager.IsHost)
		{
			_toastManager.NoticeFail("내구도 일괄 변경은 호스트만 실행할 수 있습니다.");
			return;
		}
		PlayerInventory myInventory = _gameManager.MyInventory;
		if (myInventory == null)
		{
			return;
		}
		int num = EquipmentDurabilityPercent;
		if (num < 0)
		{
			num = 0;
		}
		else if (num > 100)
		{
			num = 100;
		}
		float ratio = (float)num / 100f;
		foreach (int key in myInventory.Slots.Keys.ToList())
		{
			GoodsSlot goodsSlot = myInventory.Slots[key];
			if (goodsSlot != null && !goodsSlot.IsEmpty && goodsSlot.Goods.HasDurability())
			{
				GoodsSlot goodsSlot2 = goodsSlot.Clone();
				goodsSlot2.Goods = goodsSlot.Goods.CloneWithDurability((float)goodsSlot.Goods.GetMaxDurability() * ratio);
				myInventory.Slots[key] = goodsSlot2;
			}
		}
		foreach (int key2 in myInventory.SafeSlots.Keys.ToList())
		{
			GoodsSlot goodsSlot3 = myInventory.SafeSlots[key2];
			if (goodsSlot3 != null && !goodsSlot3.IsEmpty && goodsSlot3.Goods.HasDurability())
			{
				GoodsSlot goodsSlot4 = goodsSlot3.Clone();
				goodsSlot4.Goods = goodsSlot3.Goods.CloneWithDurability((float)goodsSlot3.Goods.GetMaxDurability() * ratio);
				myInventory.SafeSlots[key2] = goodsSlot4;
			}
		}
		GamePlayer myPlayer = _gameManager.MyPlayer;
		Character character = myPlayer.Character.Clone();
		bool changed = false;
		bool brokenStateChanged = false;
		foreach (GoodsSlot weaponSlot in character.WeaponSlots)
		{
			if (weaponSlot.Goods is WeaponGoods weaponGoods && weaponGoods.HasDurability())
			{
				bool wasBroken = weaponGoods.IsBrokenEquipment();
				WeaponGoods weaponGoods2 = weaponGoods.CloneWithDurability((float)weaponGoods.MaxDurability * ratio);
				weaponSlot.Goods = weaponGoods2;
				changed = true;
				if (wasBroken != weaponGoods2.IsBrokenEquipment())
				{
					brokenStateChanged = true;
				}
			}
		}
		foreach (GoodsSlot accessorySlot in character.AccessorySlots)
		{
			if (accessorySlot.Goods is AccessoryGoods accessoryGoods && accessoryGoods.HasDurability())
			{
				bool wasBroken2 = accessoryGoods.IsBrokenEquipment();
				AccessoryGoods accessoryGoods2 = accessoryGoods.CloneWithDurability((float)accessoryGoods.MaxDurability * ratio);
				accessorySlot.Goods = accessoryGoods2;
				changed = true;
				if (wasBroken2 != accessoryGoods2.IsBrokenEquipment())
				{
					brokenStateChanged = true;
				}
			}
		}
		if (changed)
		{
			myPlayer.Character = character;
			if (brokenStateChanged)
			{
				_gameManager.MyCharacterModel.RefreshEquipmentEffects(true);
			}
		}
		_toastManager.Notice(string.Format("모든 장비 내구도 {0}% 적용", EquipmentDurabilityPercent));
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 랜덤 장비 (창고)")]
	[Sort(26)]
	public void AddAllEquipments()
	{
		List<Goods> equipments = new List<Goods>();
		if (!CheckPlayingGame())
		{
			return;
		}
		Goods CreateEquipment(TEquipment data)
		{
			if (!data.IsArtifact())
			{
				int maxDurability = data.GetMaxDurability(1f);
				return GoodsCreator.MakeEquipmentGoods(data.Id, 0L, 1L, null, maxDurability, null, 1);
			}
			int[] prefixes = data.PickPrefixes();
			return GoodsCreator.MakeEquipmentGoods(data.Id, 0L, 1L, prefixes, null, null, (byte)_gameContext.VariantDifficulty);
		}
		foreach (TEquipment item in TEquipment.Table.Values.Where((TEquipment equipment) => equipment.IsWeapon() && equipment.WeaponType == EWeaponType.Sword).ToList().Shuffle().Take(2))
		{
			equipments.Add(CreateEquipment(item));
		}
		foreach (TEquipment item in TEquipment.Table.Values.Where((TEquipment equipment) => equipment.IsWeapon() && equipment.WeaponType == EWeaponType.Phone).ToList().Shuffle().Take(2))
		{
			equipments.Add(CreateEquipment(item));
		}
		foreach (TEquipment item in TEquipment.Table.Values.Where((TEquipment equipment) => equipment.IsWeapon() && equipment.WeaponType == EWeaponType.Shotgun).ToList().Shuffle().Take(2))
		{
			equipments.Add(CreateEquipment(item));
		}
		foreach (TEquipment item in TEquipment.Table.Values.Where((TEquipment equipment) => equipment.GoodsType == EGoodsType.Accessory).ToList().Shuffle().Take(4))
		{
			equipments.Add(CreateEquipment(item));
		}
		foreach (TEquipment item in TEquipment.Table.Values.Where((TEquipment equipment) => equipment.GoodsType == EGoodsType.Artifact).ToList().Shuffle().Take(4))
		{
			equipments.Add(CreateEquipment(item));
		}
		_gameChannel.SendToHost(new ReqCheatAddGoods { Goods = equipments.ToArray() });
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 레전드유물 10인벤)")]
	[Sort(27)]
	public void AddRandomEpicArtifacts()
	{
		if (CheckPlayingGame())
		{
			Goods[] goods = TEquipment.Table.Values.Where((TEquipment equipment) => equipment.IsArtifact() && equipment.Grade == EGoodsGradeType.Legend).ToList().Shuffle().Take(10).Select((TEquipment equipment) => GoodsCreator.MakeEquipmentGoods(equipment.Id, 0L, 1L, equipment.PickPrefixes(), null, null, (byte)_gameContext.VariantDifficulty))
				.ToArray();
			_gameChannel.SendToHost(new ReqCheatAddGoods { Goods = goods });
		}
	}

	[Category("03. Inventory")]
	[DisplayName("[g:bag_btn] 가방 지급 (인벤)")]
	[Sort(31)]
	public void AddInventoryBag()
	{
		if (CheckPlayingGame() && TBag.Table.ContainsKey(BagId))
		{
			_gameChannel.SendToHost(new ReqCheatAddGoods
			{
				Goods = new Goods[1] { GoodsCreator.MakeBagGoods(BagId, 0L, 1L, null) }
			});
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:reward_box] 생성")]
	[Sort(11)]
	public void SpawnRewardBox()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TRewardBox.Table.ContainsKey(RewardBoxId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnRewardBox(RewardBoxId, position, Vector3.back);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:entrance] 생성")]
	[Sort(21)]
	public void SpawnEntrance()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TEntrance.Table.ContainsKey(EntranceId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnEntrance(EntranceId, position, Vector3.forward);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:breakable] 생성")]
	[Sort(31)]
	public void SpawnBreakableProp()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TBreakableProp.Table.ContainsKey(BreakablePropId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnBreakableProp(BreakablePropId, position, Vector3.forward);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:special_field] 생성")]
	[Sort(41)]
	public void SpawnSpecialField()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TSpecialField.Table.ContainsKey(SpecialFieldId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnSpecialField(null, SpecialFieldId, position, Vector3.one * 6.5f);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:special_field] 제거")]
	[Sort(42)]
	public void DestroySpecialField()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TSpecialField.Table.ContainsKey(SpecialFieldId))
		{
			_objectManager.DespawnSpecialField(SpecialFieldId);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:monster_2] 생성")]
	[Sort(202)]
	public void SpawnMonster()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSpawnMonster { MonsterId = MonsterId, MonsterCount = MonsterCount });
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:npc] 생성")]
	[Sort(302)]
	public void SpawnNpc()
	{
		if (CheckPlayingGame() && _gameManager.IsHost)
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnNpc(NpcId, new Vector3(position.x - 2f, position.y + 0f, position.z + 2f), true);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:wave] 생성")]
	[Sort(312)]
	public void SpawnWaveExecutor()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSpawnWaveExecutor { WaveExecutorId = WaveExecutorId });
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:skill_exec] 생성")]
	[Sort(322)]
	public void SpawnSkillExecutor()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TSkillExecutor.Table.ContainsKey(SkillExecutorId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnSkillExecutor(SkillExecutorId, position);
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:trap_2] 생성")]
	[Sort(403)]
	public void SpawnTrap()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSpawnTrap
			{
				TrapId = TrapId,
				Forward = (Quaternion.Euler(0f, RotationY, 0f) * Vector3.forward).ToProtoVector3()
			});
		}
	}

	[Category("04. Spawn")]
	[DisplayName("[g:mimic] Mimic 생성")]
	[Sort(502)]
	public void SpawnMimic()
	{
		if (!CheckPlayingGame() || !_gameManager.IsHost)
		{
			return;
		}
		if (TMimic.Table.ContainsKey(MimicId))
		{
			Vector3 position = _gameManager.MyCharacterModel.Position;
			_objectManager.SpawnMimic(MimicId, position, Vector3.back);
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:campaign] 캠페인 시작")]
	[Sort(2)]
	public void StartCampaign()
	{
		if (!_gameManager.IsHost)
		{
			_toastManager.NoticeFail("캠페인 시작은 호스트만 실행할 수 있습니다.");
			return;
		}
		if (!TCampaign.Table.ContainsKey(CampaignId))
		{
			_toastManager.NoticeFail("유효한 캠페인 ID를 입력해주세요.");
			return;
		}
		EAnomalyType campaignAnomalyType = _campaignManager.GetCampaignAnomalyType(CampaignId);
		_gameManager.StartCampaign(CampaignId, EDifficulty.Normal, campaignAnomalyType);
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:mission] 미션 적용")]
	[Sort(4)]
	public void SetMissionIds()
	{
		if (string.IsNullOrWhiteSpace(MissionIds))
		{
			_toastManager.NoticeFail("Mission ID를 입력해주세요.");
			return;
		}
		int[] array = MissionIds.Split(',').Select((string s) => s.Trim()).Where((string s) => int.TryParse(s, out var _))
			.Select(int.Parse)
			.ToArray();
		if (array.Length == 0)
		{
			_toastManager.NoticeFail("유효한 Mission ID가 없습니다.");
			return;
		}
		int[] array2 = array.Where((int id) => !TMission.Table.ContainsKey(id)).ToArray();
		if (array2.Length != 0)
		{
			_toastManager.NoticeFail("존재하지 않는 Mission ID: " + string.Join(", ", array2));
			return;
		}
		_gameManager.SetMissionIds(array);
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:mission] 적용 취소")]
	[Sort(5)]
	public void CancelMissionIds()
	{
		_gameManager.SetMissionIds(null);
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 캠페인 퀘스트 완료")]
	[Sort(6)]
	public void CompleteCampaign()
	{
		if (CheckPlayingGame() && _gameContext.CampaignId > 0)
		{
			_campaignManager.CompleteQuest();
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 퀘스트 시작")]
	[Sort(7)]
	public void StartStage()
	{
		if (!_gameManager.IsHost)
		{
			_toastManager.NoticeFail("퀘스트 시작은 호스트만 실행할 수 있습니다.");
		}
		else
		{
			_gameManager.StartStage();
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 탈출")]
	[Sort(8)]
	public void EscapeCharacter()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqForceEscape());
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 모드 캠페인 클리어")]
	[Sort(9)]
	public void ClearAllCampaigns()
	{
		foreach (int campaignId in TCampaign.Table.Keys)
		{
			_playerManager.SetClearedCampaign(campaignId);
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 라운지 퀘스트 클리어")]
	[Sort(10)]
	public void ClearAllLoungeQuests()
	{
		foreach (int loungeQuestId in TLoungeQuest.Table.Keys)
		{
			_playerManager.SetClearedLoungeQuest(loungeQuestId);
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:lounge_quest] 라운지 퀘스트 상태 적용")]
	[Sort(14)]
	public void SetLoungeQuestState()
	{
		LoungeQuest quest = null;
		if (!TLoungeQuest.Table.ContainsKey(LoungeQuestId))
		{
			_toastManager.NoticeFail("Please enter a valid LoungeQuest ID.");
			return;
		}
		switch (LoungeQuestState)
		{
		case ELoungeQuestState.NotStarted:
			if (!_loungeQuestManager.TryGetQuest(LoungeQuestId, out quest))
			{
				_loungeQuestManager.AddQuest(LoungeQuestId);
			}
			break;
		case ELoungeQuestState.InProgress:
		case ELoungeQuestState.NotCompleted:
			if (!_loungeQuestManager.TryGetQuest(LoungeQuestId, out quest))
			{
				_loungeQuestManager.AddQuest(LoungeQuestId);
			}
			_loungeQuestManager.SetQuestState(LoungeQuestId, LoungeQuestState);
			break;
		case ELoungeQuestState.Completed:
			_loungeQuestManager.CompleteQuest(LoungeQuestId);
			break;
		}
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 모든 미션 클리어")]
	[Sort(11)]
	public void ClearAllMissions()
	{
		foreach (int missionId in TMission.Table.Keys)
		{
			_playerManager.SetClearedMission(missionId);
		}
	}

	[Category("05. System >> Monster")]
	[DisplayName("[g:monster] HP 저장")]
	[Sort(2)]
	public void ApplyMonsterHp()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatApplyMonsterHp { Hp = MonsterHp });
		}
	}

	[Category("05. System >> Monster")]
	[DisplayName("[g:monster] 모든 몬스터 죽이기")]
	[Sort(3)]
	public void KillAllMonsters()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSkillMonsterAll());
		}
	}

	[Category("05. System >> Paradox")]
	[DisplayName("[g:paradox] 추가")]
	[Sort(2)]
	public void AddParadox()
	{
		_gameChannel.SendToHost(new ReqCheatAddParadox { ParadoxId = ParadoxId });
	}

	[Category("05. System >> Paradox")]
	[DisplayName("[g:paradox] 랜덤 적용")]
	[Sort(3)]
	public void ApplyParadox()
	{
		_gameChannel.SendToHost(new ReqCheatAddRandomParadox());
	}

	[Category("05. System >> Map")]
	[DisplayName("[g:map_1] 미니맵 On/Off")]
	[Sort(1)]
	public void MiniMapOnOff()
	{
		_gameManager.RxIsRevealingMap.Value = !_gameManager.IsRevealingMapMap;
	}

	[Category("05. System >> Map")]
	[DisplayName("[g:map_2] 이상현상 변경 ")]
	[Sort(4)]
	public void ChangeAnomalyType()
	{
		if (!_gameManager.IsHost)
		{
			_toastManager.NoticeFail("이상현상 변경은 호스트만 실행할 수 있습니다.");
			return;
		}
		if (_gameContext.CampaignId > 0)
		{
			_toastManager.NoticeFail("캠페인 진행 중에는 이상현상을 변경할 수 없습니다.");
			return;
		}
		foreach (int key in TCampaign.Table.Keys)
		{
			_campaignManager.OverrideAnomalyType(key, AnomalyType);
		}
	}

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_3] Call Lua Function")]
	[Sort(3)]
	public void CallLuaFunction()
	{
		if (LuaKey.HasValue())
		{
			_luaController.RunLua(LuaKey, LuaFunction, Array.Empty<object>());
		}
		else
		{
			_luaController.RunLua(LuaFunction, Array.Empty<object>());
		}
	}

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_4] Reload Lua Scripts")]
	[Sort(4)]
	public void ReloadLuaScripts()
	{
		_luaRunner.LoadLuaScript();
	}

	[Category("05. System >> Blueprint")]
	[DisplayName("[g:blueprint] 설계도 해금")]
	[Sort(1)]
	public void UnlockAllBlueprint()
	{
		foreach (TItem item in TItem.Table.Values.Where((TItem item) => item.GoodsType == EGoodsType.Blueprint).ToList())
		{
			_playerManager.SetItemUsed(item.Id);
		}
	}

	[Category("05. System >> Blueprint")]
	[DisplayName("[g:blueprint] 해금 삭제")]
	[Sort(2)]
	public void LockAllBlueprint()
	{
		foreach (TItem item in TItem.Table.Values.Where((TItem item) => item.GoodsType == EGoodsType.Blueprint).ToList())
		{
			_playerManager.ClearItemUsed(item.Id);
		}
	}

	[Category("05. System >> Interior")]
	[DisplayName("[g:interior_2] Id 구매처리")]
	[Sort(2)]
	public void UnlockInteriorShopItem()
	{
		if (!CheckPlayingGame())
		{
			return;
		}
		if (!TInteriorShop.Table.TryGetValue(InteriorShopId, out var value))
		{
			_toastManager.NoticeFail("유효한 InteriorShop ID를 입력해주세요.");
			return;
		}
		switch (value.Interior.Type)
		{
		case EInteriorType.Area:
			if (!_gameLoungeManager.IsAreaUnlocked(value.Interior.Id))
			{
				_gameLoungeManager.UnlockArea(value.Interior.Id);
			}
			break;
		case EInteriorType.AreaDecoration:
			if (!_gameLoungeManager.IsAreaDecorationUnlocked(value.Interior.Id))
			{
				_gameLoungeManager.UnlockAreaDecoration(value.Interior.Id);
				_gameLoungeManager.SetActiveAreaDecoration(value.Interior.Id);
			}
			break;
		}
	}

	[Category("05. System >> Interior")]
	[DisplayName("[g:interior_2] 초기화")]
	[Sort(3)]
	public void ResetInteriorFlags()
	{
		if (CheckPlayingGame())
		{
			_gameLoungeManager.CheatResetInterior();
		}
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] AppCheck 즉시 전송")]
	[Sort(1)]
	public void TriggerAppCheck()
	{
		_eventSystem.Trigger(new AppCheckCheatEvent());
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] LogError 강제 전송")]
	[Sort(2)]
	public void TriggerLogError()
	{
		UnityEngine.Debug.LogError("Cheat: Forced LogError");
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] LogException 강제 전송")]
	[Sort(3)]
	public void TriggerLogException()
	{
		UnityEngine.Debug.LogException(new InvalidOperationException("Cheat: Forced LogException"));
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] Exception 강제 발생")]
	[Sort(4)]
	public void TriggerException()
	{
		throw new InvalidOperationException("Cheat: Forced Exception");
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] SeriLogger Error 강제 전송")]
	[Sort(5)]
	public void TriggerSeriLoggerError()
	{
		_logger.Error("Cheat: Forced SeriLogger Error");
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:input] 스팀 가상 패드 강제 표시")]
	[Sort(2)]
	public void ShowSteamVirtualPad()
	{
		UnityEngine.Debug.Log(string.Format("[Steam] Initialized={0}", SteamManager.Initialized));
		UIInputUtility.ShowGamepadTextInputAsync("테스트 입력", 20, string.Empty).Forget();
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:crash] CrashAccessViolation")]
	[Sort(10)]
	public void CrashAccessViolation()
	{
		UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.AccessViolation);
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:crash] CrashAbort")]
	[Sort(11)]
	public void CrashAbort()
	{
		UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
	}

	[Category("05. System >> Etc")]
	[DisplayName("언락 Npc 강제 생성")]
	[Sort(12)]
	public void SpawnLoungeNpcs()
	{
		if (CheckPlayingGame())
		{
			_objectManager.ForceSpawnNpcs();
		}
	}

	[Category("05. System >> ArtifactDeal")]
	[DisplayName("[g:artifact] Open ArtifactDeal")]
	[Sort(2)]
	public void OpenArtifactDealPopup()
	{
		if (!TArtifactDeal.Table.ContainsKey(ArtifactDealId))
		{
			_toastManager.NoticeFail(string.Format("Invalid ArtifactId {0}", ArtifactDealId));
			return;
		}
		_popupManager.Open<ArtifactDealPopupPresenter>(new ArtifactDealPopupModel { ArtifactDealId = ArtifactDealId });
	}

	[Category("99. Temp")]
	[DisplayName("[g:give] 아이템 주기")]
	[Sort(5)]
	public void GiveLuaItem()
	{
		int id = TItem.Table.Keys.ToList().Shuffle().First();
		bool isInLounge = _gameManager.IsInLounge;
		Goods goods = GoodsCreator.MakeItemGoods(id, isInLounge ? 10 : 0, isInLounge ? 0 : 10);
		_gameChannel.SendToHost(new ReqGainGoods { Goods = new Goods[1] { goods } });
	}

	[Category("99. Temp")]
	[DisplayName("[g:give] 장비 주기")]
	[Sort(5)]
	public void GiveLuaEquipment()
	{
		int id = TEquipment.Table.Keys.ToList().Shuffle().First();
		TEquipment data = TEquipment.Table[id];
		int[] prefixes = data.PickPrefixes();
		bool isInLounge = _gameManager.IsInLounge;
		Goods goods = GoodsCreator.MakeEquipmentGoods(id, isInLounge ? 1 : 0, (!isInLounge) ? 1 : 0, prefixes, null, null, 1);
		_gameChannel.SendToHost(new ReqGainGoods { Goods = new Goods[1] { goods } });
	}

	[Category("08. Tycoon")]
	[DisplayName("[g:turn_btn] 1턴 넘기기")]
	[Sort(1)]
	public void SkipTycoonTurn()
	{
		if (CheckPlayingGame() && !(_tycoonManager == null))
		{
			_tycoonManager.OnTurn();
			_toastManager.Notice("[치트] 경영 1턴을 처리했습니다.");
		}
	}

	[Category("08. Tycoon")]
	[DisplayName("[g:stress_btn] 모든 직원 스트레스 일괄 변경")]
	[Sort(11)]
	public void SetAllEmployeesStress()
	{
		if (!CheckPlayingGame() || _tycoonManager == null)
		{
			return;
		}
		int stressMax = TConst.StressMax.IntValue;
		int num = Math.Clamp(EmployeeStressValue, 0, stressMax);
		foreach (Employee value in _playerManager.Employees.Values)
		{
			value.Stress = num;
			if (num < stressMax)
			{
				value.IsPanic = false;
			}
		}
		_tycoonManager.OnEmployeesChanged.OnNext(Unit.Default);
		_toastManager.Notice(string.Format("[치트] 모든 직원 스트레스 {0} 적용", num));
	}

	[Category("01. User")]
	[DisplayName("[g:gold] 적용")]
	[Sort(2)]
	public void SetGold()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSetGold { Gold = GoldValue });
		}
	}

	[Category("01. User")]
	[DisplayName("[g:coin] 적용")]
	[Sort(12)]
	public void SetCoin()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSetCoin { Coin = CoinValue });
		}
	}

	[Category("01. User")]
	[DisplayName("[g:exp] 적용")]
	[Sort(4)]
	public void GainUserExp()
	{
		if (CheckPlayingGame())
		{
			_gameChannel.SendToHost(new ReqCheatSetExp { Exp = ExpValue });
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 유저 데이터 초기화")]
	[Sort(111)]
	public void ResetUserData()
	{
		if (_roomContext.RoomState == ERoomState.None)
		{
			string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SavesDir");
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
			SceneManager.LoadScene("SplashScene");
		}
		else
		{
			_logger.Warning("Unable to reset user data in the game.");
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 레벨업")]
	[Sort(112)]
	public void LevelUp()
	{
		_gameChannel.SendToHost(new ReqCheatLevelUp());
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 특성 최대")]
	[Sort(113)]
	public void UpgradeTalentMax()
	{
		_gameChannel.SendToHost(new ReqCheatUnlockAllTalent());
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 아이템 수집 처리")]
	[Sort(122)]
	public void CollectAllItem()
	{
		foreach (int itemId in TItem.Table.Keys)
		{
			_playerManager.SetItemCollection(itemId);
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 아이템 사용 처리")]
	[Sort(123)]
	public void UseAllNote()
	{
		foreach (TItem item in TItem.Table.Values)
		{
			_playerManager.SetItemUsed(item.Id);
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 장비 수집 처리")]
	[Sort(124)]
	public void CollectAllEquipment()
	{
		foreach (int equipmentId in TEquipment.Table.Keys)
		{
			_playerManager.SetEquipmentCollection(equipmentId);
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 몬스터 수집 처리")]
	[Sort(125)]
	public void CollectAllMonster()
	{
		foreach (int monsterId in TMonster.Table.Keys)
		{
			_playerManager.SetMonsterCollection(monsterId);
		}
	}
}
