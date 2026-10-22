using System;
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
			return false;
		}
		set
		{
		}
	}

	[Category("01. User")]
	[DisplayName("[g:util_0] 라운지 튜토 무시")]
	[Sort(102)]
	public bool IgnoreLoungeTutorial
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private event PropertyChangedEventHandler PropertyChangedEventHandler
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	private bool CheckPlayingGame()
	{
		return false;
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp] 저장")]
	[Sort(11)]
	public void ApplyCharacterHp()
	{
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 회복")]
	[Sort(20)]
	public void RecoverCharacter()
	{
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 자살")]
	[Sort(21)]
	public void KillCharacter()
	{
	}

	[Category("02. Character >> Hp")]
	[DisplayName("[g:hp_func] 부활")]
	[Sort(22)]
	public void ReviveCharacter()
	{
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress] 저장")]
	[Sort(31)]
	public void ApplyCharacterStress()
	{
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 경고")]
	[Sort(32)]
	public void ChangeCharacterAlert()
	{
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 공포")]
	[Sort(33)]
	public void ChangeCharacterFear()
	{
	}

	[Category("02. Character >> Stress")]
	[DisplayName("[g:Stress_func] 절망")]
	[Sort(34)]
	public void ChangeCharacterDespair()
	{
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption] 적용")]
	[Sort(41)]
	public void ApplyCharacterCorruption()
	{
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Safe")]
	[Sort(42)]
	public void ChangeCorruptionSafe()
	{
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Caution")]
	[Sort(43)]
	public void ChangeCorruptionCaution()
	{
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Alert")]
	[Sort(44)]
	public void ChangeCorruptionAlert()
	{
	}

	[Category("02. Character >> Corruption")]
	[DisplayName("[g:corruption_func] Serious")]
	[Sort(45)]
	public void ChangeCorruptionSerious()
	{
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 적용")]
	[Sort(51)]
	public void ApplyCharacterSoul()
	{
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 소울 최대치")]
	[Sort(52)]
	public void ApplyMaxSoul()
	{
	}

	[Category("02. Character >> Soul")]
	[DisplayName("[g:soul] 소울 0으로")]
	[Sort(53)]
	public void ApplyMinSoul()
	{
	}

	[Category("02. Character >> Brightness")]
	[DisplayName("[g:brightness] 적용")]
	[Sort(61)]
	public void ApplyCharacterBrightness()
	{
	}

	[Category("02. Character >> Brightness")]
	[DisplayName("[g:brightness] 회복")]
	[Sort(62)]
	public void RecoverCharacterBrightness()
	{
	}

	[Category("02. Character >> Position")]
	[DisplayName("[g:pos] 이동")]
	[Sort(73)]
	public void MoveCharacter()
	{
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff] 버프 추가")]
	[Sort(91)]
	public void AddBuff()
	{
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff_func] 상태 버프 추가")]
	[Sort(93)]
	public void AddStatusEffectBuff()
	{
	}

	[Category("02. Character >> Buff")]
	[DisplayName("[g:buff_func] 버프 리스트 확인")]
	[Sort(94)]
	public void ShowBuffs()
	{
	}

	[Category("02. Character >> Options")]
	[DisplayName("저장")]
	[Sort(103)]
	public void SaveCharacterOptions()
	{
	}

	[Category("02. Character >> Options")]
	[DisplayName("[g:opt_func] 쿨타임 초기화")]
	[Sort(104)]
	public void ResetSkillCooldown()
	{
	}

	[Category("02. Character >> Options")]
	[DisplayName("[g:opt_func] 스킬 차지 최대")]
	[Sort(105)]
	public void MaxSkillCharge()
	{
	}

	[Category("02. Character >> Options")]
	[DisplayName("캐릭터 스킨 언락")]
	[Sort(106)]
	public void UnlockCharacterSkins()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 아이템 지급 (인벤)")]
	[Sort(13)]
	public void AddInventoryItem()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (창고)")]
	[Sort(14)]
	public void AddAllItems()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 도면 (창고)")]
	[Sort(15)]
	public void AddRandomBlueprintItems()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (Storage)")]
	[Sort(16)]
	public void AddRandomItemsToStorage()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:item_btn] 랜덤 아이템 (분실물)")]
	[Sort(17)]
	public void AddRandomItemsToLostItem()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 장비 지급 (인벤)")]
	[Sort(22)]
	public void AddInventoryEquipment()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 모든 장비 내구도 일괄 변경")]
	[Sort(25)]
	public void SetAllEquipmentDurability()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 랜덤 장비 (창고)")]
	[Sort(26)]
	public void AddAllEquipments()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:equip_btn] 레전드유물 10인벤)")]
	[Sort(27)]
	public void AddRandomEpicArtifacts()
	{
	}

	[Category("03. Inventory")]
	[DisplayName("[g:bag_btn] 가방 지급 (인벤)")]
	[Sort(31)]
	public void AddInventoryBag()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:reward_box] 생성")]
	[Sort(11)]
	public void SpawnRewardBox()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:entrance] 생성")]
	[Sort(21)]
	public void SpawnEntrance()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:breakable] 생성")]
	[Sort(31)]
	public void SpawnBreakableProp()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:special_field] 생성")]
	[Sort(41)]
	public void SpawnSpecialField()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:special_field] 제거")]
	[Sort(42)]
	public void DestroySpecialField()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:monster_2] 생성")]
	[Sort(202)]
	public void SpawnMonster()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:npc] 생성")]
	[Sort(302)]
	public void SpawnNpc()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:wave] 생성")]
	[Sort(312)]
	public void SpawnWaveExecutor()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:skill_exec] 생성")]
	[Sort(322)]
	public void SpawnSkillExecutor()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:trap_2] 생성")]
	[Sort(403)]
	public void SpawnTrap()
	{
	}

	[Category("04. Spawn")]
	[DisplayName("[g:mimic] Mimic 생성")]
	[Sort(502)]
	public void SpawnMimic()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:campaign] 캠페인 시작")]
	[Sort(2)]
	public void StartCampaign()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:mission] 미션 적용")]
	[Sort(4)]
	public void SetMissionIds()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:mission] 적용 취소")]
	[Sort(5)]
	public void CancelMissionIds()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 캠페인 퀘스트 완료")]
	[Sort(6)]
	public void CompleteCampaign()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 퀘스트 시작")]
	[Sort(7)]
	public void StartStage()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_1] 탈출")]
	[Sort(8)]
	public void EscapeCharacter()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 모드 캠페인 클리어")]
	[Sort(9)]
	public void ClearAllCampaigns()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 라운지 퀘스트 클리어")]
	[Sort(10)]
	public void ClearAllLoungeQuests()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:lounge_quest] 라운지 퀘스트 상태 적용")]
	[Sort(14)]
	public void SetLoungeQuestState()
	{
	}

	[Category("05. System >> Quest")]
	[DisplayName("[g:quest_2] 모든 미션 클리어")]
	[Sort(11)]
	public void ClearAllMissions()
	{
	}

	[Category("05. System >> Monster")]
	[DisplayName("[g:monster] HP 저장")]
	[Sort(2)]
	public void ApplyMonsterHp()
	{
	}

	[Category("05. System >> Monster")]
	[DisplayName("[g:monster] 모든 몬스터 죽이기")]
	[Sort(3)]
	public void KillAllMonsters()
	{
	}

	[Category("05. System >> Paradox")]
	[DisplayName("[g:paradox] 추가")]
	[Sort(2)]
	public void AddParadox()
	{
	}

	[Category("05. System >> Paradox")]
	[DisplayName("[g:paradox] 랜덤 적용")]
	[Sort(3)]
	public void ApplyParadox()
	{
	}

	[Category("05. System >> Map")]
	[DisplayName("[g:map_1] 미니맵 On/Off")]
	[Sort(1)]
	public void MiniMapOnOff()
	{
	}

	[Category("05. System >> Map")]
	[DisplayName("[g:map_2] 이상현상 변경 ")]
	[Sort(4)]
	public void ChangeAnomalyType()
	{
	}

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_3] Call Lua Function")]
	[Sort(3)]
	public void CallLuaFunction()
	{
	}

	[Category("05. System >> Lua")]
	[DisplayName("[g:lua_4] Reload Lua Scripts")]
	[Sort(4)]
	public void ReloadLuaScripts()
	{
	}

	[Category("05. System >> Blueprint")]
	[DisplayName("[g:blueprint] 설계도 해금")]
	[Sort(1)]
	public void UnlockAllBlueprint()
	{
	}

	[Category("05. System >> Blueprint")]
	[DisplayName("[g:blueprint] 해금 삭제")]
	[Sort(2)]
	public void LockAllBlueprint()
	{
	}

	[Category("05. System >> Interior")]
	[DisplayName("[g:interior_2] Id 구매처리")]
	[Sort(2)]
	public void UnlockInteriorShopItem()
	{
	}

	[Category("05. System >> Interior")]
	[DisplayName("[g:interior_2] 초기화")]
	[Sort(3)]
	public void ResetInteriorFlags()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] AppCheck 즉시 전송")]
	[Sort(1)]
	public void TriggerAppCheck()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] LogError 강제 전송")]
	[Sort(2)]
	public void TriggerLogError()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] LogException 강제 전송")]
	[Sort(3)]
	public void TriggerLogException()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] Exception 강제 발생")]
	[Sort(4)]
	public void TriggerException()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[analytics] SeriLogger Error 강제 전송")]
	[Sort(5)]
	public void TriggerSeriLoggerError()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:input] 스팀 가상 패드 강제 표시")]
	[Sort(2)]
	public void ShowSteamVirtualPad()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:crash] CrashAccessViolation")]
	[Sort(10)]
	public void CrashAccessViolation()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("[g:crash] CrashAbort")]
	[Sort(11)]
	public void CrashAbort()
	{
	}

	[Category("05. System >> Etc")]
	[DisplayName("언락 Npc 강제 생성")]
	[Sort(12)]
	public void SpawnLoungeNpcs()
	{
	}

	[Category("05. System >> ArtifactDeal")]
	[DisplayName("[g:artifact] Open ArtifactDeal")]
	[Sort(2)]
	public void OpenArtifactDealPopup()
	{
	}

	[Category("99. Temp")]
	[DisplayName("[g:give] 아이템 주기")]
	[Sort(5)]
	public void GiveLuaItem()
	{
	}

	[Category("99. Temp")]
	[DisplayName("[g:give] 장비 주기")]
	[Sort(5)]
	public void GiveLuaEquipment()
	{
	}

	[Category("08. Tycoon")]
	[DisplayName("[g:turn_btn] 1턴 넘기기")]
	[Sort(1)]
	public void SkipTycoonTurn()
	{
	}

	[Category("08. Tycoon")]
	[DisplayName("[g:stress_btn] 모든 직원 스트레스 일괄 변경")]
	[Sort(11)]
	public void SetAllEmployeesStress()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:gold] 적용")]
	[Sort(2)]
	public void SetGold()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:coin] 적용")]
	[Sort(12)]
	public void SetCoin()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:exp] 적용")]
	[Sort(4)]
	public void GainUserExp()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 유저 데이터 초기화")]
	[Sort(111)]
	public void ResetUserData()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 레벨업")]
	[Sort(112)]
	public void LevelUp()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_1] 특성 최대")]
	[Sort(113)]
	public void UpgradeTalentMax()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 아이템 수집 처리")]
	[Sort(122)]
	public void CollectAllItem()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 아이템 사용 처리")]
	[Sort(123)]
	public void UseAllNote()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 장비 수집 처리")]
	[Sort(124)]
	public void CollectAllEquipment()
	{
	}

	[Category("01. User")]
	[DisplayName("[g:util_2] 모든 몬스터 수집 처리")]
	[Sort(125)]
	public void CollectAllMonster()
	{
	}
}
