using System.Collections.Generic;

public static class Const
{
	public static class AssetLabel
	{
		public const string LOGIN = "Login";
	}

	public static class Atlas
	{
		public const string ICON_COMMON = "Icon_Common";

		public const string ICON_MONSTER = "Icon_Monster";

		public const string ICON_ITEM = "Icon_Item";

		public const string ICON_EQUIPMENT = "Icon_Equipment";

		public const string ICON_BAG = "Icon_Bag";

		public const string ICON_SKILL = "Icon_Skill";

		public const string ICON_STATUS_EFFECT_TAG = "Icon_StatusEffectTag";

		public const string ICON_BUFF = "Icon_Buff";

		public const string ICON_PING = "Icon_Ping";

		public const string ICON_EMOJI = "Icon_Emoji";

		public const string ICON_TALENT = "Icon_Talent";

		public const string ICON_WEAKPOINT = "Icon_WeakPoint";

		public const string ICON_GOODS_SUB_TYPE = "Icon_GoodsSubType";

		public const string ICON_UNIT_CHARACTER = "Icon_Unit_Character";

		public const string ICON_UNIT_MONSTER = "Icon_Unit_Monster";

		public const string ICON_UNIT_NPC = "Icon_Unit_Npc";

		public const string ICON_UNIT_ETC = "Icon_Unit_Etc";

		public const string ICON_AREA = "Icon_Area";

		public const string ICON_AREA_DECORATION = "Icon_AreaDecoration";

		public const string ICON_AREA_NPC = "Icon_Area_Npc";

		public const string ICON_EMPLOYEE_CONDITION = "Icon_EmployeeCondition";

		public const string ICON_EMPLOYEE_FACE = "Icon_EmployeeFace";

		public const string ICON_Anomaly = "Icon_Anomaly";
	}

	public static class Bgm
	{
		public const string TITLE = "Title";
	}

	public static class Buff
	{
		public const int CHEAT_INVINCIBILITY = 1000001;

		public const int CHEAT_INVISIBLE = 1000002;

		public const int CHEAT_SPEED_UP = 1000003;

		public const int HIGH_STRESS_ALL_IN_RANGE_DEBUFF = 2000001;

		public const int TEMPORARY_INVINCIBILITY_BUFF = 3000001;

		public const int ZERO_LIGHTFUEL_DEBUFF = 2003001;

		public const int WEAKPOINT_FIRE = 2004001;

		public const int WEAKPOINT_WATER = 2004002;

		public const int WEAKPOINT_WIND = 2004003;
	}

	public static class Campaign
	{
		public const int FIRST_TUTORIAL_CAMPAIGN = 1101;
	}

	public static class CharacterAnimation
	{
		public const string DEFAULT_IDLE = "default/idle";

		public const string DEFAULT_WALK = "default/walk";

		public const string DEFAULT_RUN = "default/run";

		public const string BATTLE_IDLE = "battle/idle";

		public const string BATTLE_WALK = "battle/walk";

		public const string BATTLE_RUN = "battle/run";

		public const string BATTLE_DEATH = "battle/death";

		public const string BATTLE_SEARCH = "battle/search";
	}

	public static class CharacterSkin
	{
		public const string DEFAULT_WEAPON = "weapon/dummy";
	}

	public static class CharacterSkinPrefix
	{
		public const string SKIN_BASE = "body/skinbase_";

		public const string SKIN_BASE_SUPER = "body_super/skinbase_";

		public const string HAIR = "body/hair_";

		public const string HAIR_SUPER = "body_super/hair_";

		public const string EYE = "body/eye_";

		public const string EYE_SUPER = "body_super/eye_";

		public const string COSTUME = "body/costume_";

		public const string COSTUME_SUPER = "body_super/costume_";

		public const string ACC = "body/acc_";

		public const string ACC_SUPER = "body_super/acc_";

		public const string WEAPON = "weapon/";

		public const string WEAPON_SUPER = "weapon_super/";
	}

	public static class ColorCode
	{
		public const string ENOUGH_MONEY_COUNT = "#323232";

		public const string ENOUGH_MONEY_PRICE = "#DCDCDC";

		public const string NOT_ENOUGH_MONEY_DEFAULT = "#a45646";

		public const string ITEM_MAX_COLOR = "#a74200";

		public const string CHAT_PING = "#58CECA";

		public const string CHAT_MESSAGE_OTHER = "#DCDCDC";

		public const string CHAT_MESSAGE_ME = "#b3d465";

		public const string CHAT_SYSTEM = "#898989";

		public const string CHAT_QUEST = "#A45646";

		public const string CHAT_RADIO = "#A45646";

		public const string CHAT_BOSS = "#A45646";

		public const string CHAT_DEFAULT = "#FF00FF";

		public const string ACTIVE = "#A45646";

		public const string INACTIVE = "#6C6D6D";

		public const string ICON_UNKNOWN = "#1B1B1B";

		public const string ELEMENTAL_RESISTANCE = "#EF7767";

		public const string ELEMENTAL_RESISTANCE_UNKNOWN = "#707070";

		public const string PLAYER_00 = "#26D0CC";

		public const string PLAYER_01 = "#FFD200";

		public const string PLAYER_02 = "#DC59EF";
	}

	public static class CustomerAnimation
	{
		public const string WALK = "walk";

		public const string IDLE = "idle";

		public const string RUN = "run";
	}

	public static class EmployeeAnimation
	{
		public const string NEGOTIATION = "negotiation";

		public const string IDLE = "idle";

		public const string PANIC_1 = "panic_1";

		public const string PANIC_2 = "panic_2";

		public const string PANIC_3 = "panic_3";

		public static readonly string[] PANIC_ANIMATIONS;
	}

	public static class Encrypt
	{
		public static string[] ExcludeTables;

		public static byte LUA_KEY => 0;

		public static byte TABLE_KEY => 0;
	}

	public static class HitLayer
	{
		public const int NONE = 0;

		public const int CHARACTER = 1000;

		public const int MONSTER = 2000;

		public const int OBSTACLE = 3000;

		public const int ENVIRONMENT = 4000;
	}

	public static class Inspector
	{
		public const string COMPONENT = "Component";

		public const string CACHE = "Cache";

		public const string SETTING = "Setting";

		public const string EXPRESSION = "Expression";

		public const string PROPERTY = "Property";

		public const string EVENT = "Event";

		public const string TEST = "Test";
	}

	public static class ItemId
	{
		public const int OVERWORLD_COIN = 10001;
	}

	public static class Layer
	{
		public static int PLAYER;

		public static int PLANE;

		public static int HIGH_OBSTACLE;

		public static int LOW_OBSTACLE;

		public static int UNIT;

		public static int INTERACTABLE;

		public static int BREAKABLE_PROP;

		public static int PING;
	}

	public static class LayerMask
	{
		public static int PLAYER;

		public static int PLANE;

		public static int HIGH_OBSTACLE;

		public static int LOW_OBSTACLE;

		public static int UNIT;

		public static int INTERACTABLE;

		public static int BREAKABLE_PROP;

		public static int Ping;
	}

	public static class Link
	{
		public const string DISCORD = "https://discord.gg/kC3aBdsWc5";

		public const string STEAM_FORMAT = "steam://store/{0}";

		public const string X = "https://x.com/studionemo_de";

		private static readonly Dictionary<string, string> _feedbackLinks;

		public static string GetFeedbackLink(string language)
		{
			return null;
		}
	}

	public static class NavMask
	{
		public const string HITABLE = "Hitable";
	}

	public static class PlayerPrefs
	{
		public const string CHAT_PANEL_YPOS_RATIO_WITH_MAX_POS = "ChatPanel_YPos_Ratio_WithMaxPos";

		public const string LOUNGE_FILTER_BLOCK_OTHER_AREAS = "LoungeFilter_BlockOtherAreas";

		public const string LOUNGE_FILTER_BLOCKED_DIFFICULTIES = "LoungeFilter_BlockedDifficulties";

		public const string LOUNGE_FILTER_MIN_HOST_LEVEL = "LoungeFilter_MinHostLevel";

		public const string LOUNGE_FILTER_MIN_PING_FILTER_TYPE = "LoungeFilter_MinPingFilterType";

		public const string LOUNGE_FILTER_SHOW_PLAYING_LOUNGE = "LoungeFilter_ShowPlayingLounge";

		public const string CHEAT_IGNORE_TUTORIAL = "Cheat_IgnoreTutorial";

		public const string CHEAT_IGNORE_LOUNGE_TUTORIAL = "Cheat_IgnoreLoungeTutorial";
	}

	public static class Prefab
	{
		public const string INGAME_HITBOX = "Combat/HitBox";

		public const string INGAME_SKILL_INDICATOR = "Combat/SkillIndicator/Input";

		public const string INGAME_SKILL_INDICATOR_VFX = "Combat/SkillIndicator/Vfx";

		public const string INGAME_EMOJI = "EventUi/Emoji";

		public const string INGAME_FLOATING_TEXT = "EventUi/FloatingText";

		public const string INGAME_MINIMAP_MARKER = "EventUi/MiniMapMarker";

		public const string INGAME_PING = "EventUi/Ping";

		public const string INGAME_NPC = "Object/Npc";

		public const string INGAME_LOCKED_AREA_OBJECT = "Object/LockedAreaObject";

		public const string INGAME_AREA_DECORATION_OBJECT = "Object/AreaDecorationObject";

		public const string INGAME_SECTOR_SKIN_PRESET = "Scriptable/SectorSkinPreset";

		public const string INGAME_GUEST = "Object/Guest";

		public const string INGAME_EXIT = "Object/Exit";

		public const string INGAME_TRAP = "Object/Trap";

		public const string INGAME_SKILLEXECUTOR = "Object/SkillExecutor";

		public const string INGAME_REWARD_BOX = "Object/RewardBox";

		public const string INGAME_MIMIC = "Object/Mimic";

		public const string INGAME_VISUAL_PROP = "Object/VisualProp";

		public const string INGAME_ENTRANCE = "Object/Entrance";

		public const string INGAME_WAVE_EXECUTOR = "Object/WaveExecutor";

		public const string INGAME_PORTAL = "Object/Portal";

		public const string INGAME_DISPOSABLE_OBJECT = "Object/DisposableObject";

		public const string INGAME_BREAKABLE_PROP = "Object/BreakableProp";

		public const string INGAME_WAVE_ENVIRONMENT_PROP = "Object/EnvironmentProp";

		public const string INGAME_SPECIAL_FIELD = "Object/SpecialField";

		public const string INGAME_DROP_PROP = "Object/DropProp";

		public const string INGAME_MAP_OBJECT = "Object/Etc";

		public const string INGAME_INTERACTIVETRIGGER = "Object/InteractiveTrigger";

		public const string INGAME_OBJECT_ETC = "Object/Etc";

		public const string INGAME_SECTOR = "Sector";

		public const string INGAME_TIMELINE = "Timeline";

		public const string INGAME_CHARACTER = "Unit/Character";

		public const string INGAME_PET = "Unit/Pet";

		public const string INGAME_MONSTER = "Unit/Monster";

		public const string INGAME_EXTRAUNIT = "Unit/ExtraUnit";

		public const string INGAME_EFFECT = "Vfx";

		public const string INGAME_SLOT = "Slot";

		public const string INGAME_Component = "Component";
	}

	public static class Scene
	{
		public const string START_SCENE = "Scene/StartScene.unity";

		public const string LOADING_SCENE = "Scene/LoadingScene.unity";

		public const string LOUNGE_SCENE = "LoungeScene";

		public const string STAGE_SCENE = "StageScene";
	}

	public static class Scriptable
	{
		public const string STRESS_CAMERA_EFFECT_SETTINGS = "Scriptable/Art/CameraEffect/StressCameraEffectSettings";
	}

	public static class Scroller
	{
		public static float DECELERATION_RATE;

		public static float SCROLL_SENSITIVITY;
	}

	public static class Skill
	{
		public const int ACTIVE_SKILL_COUNT_MAX = 4;

		public const int ACTIVE_SKILL_COUNT_DEFAULT = 2;

		public const int NONE_ID = 0;

		public const int JUMP_ID = 10000100;

		public const string SKILL_VFX_FORMAT = "Skill/{0}";

		public const string STATUS_VFX_FORMAT = "Status/{0}";
	}

	public static class SfxBus
	{
		public const string GAME = "SFX_GAME";

		public const string SYSTEM = "SFX_SYSTEM";

		public const string CUTSCENE = "SFX_CUTSCENE";
	}

	public static class Sfx
	{
		public const string BUTTON_CLICK = "ButtonClick";

		public const string MOUSE_OVER_CHANGE = "MouseOverChange";

		public const string DIALOG_NEXT_CLICK = "DialogNext";

		public const string DIALOGUE_PAGE_CHANGED = "select";

		public const string PET_ALERT = "PetAlert";

		public const string DANGER_ALERT = "DangerAlert";

		public const string FAILURE_ALERT = "Fail";

		public const string GAME_END_DEFEAT = "GameEndDefeat";

		public const string GAME_END_ESCAPE = "GameEndEscape";

		public const string WAVE_START = "WaveStart";

		public const string INTERACTION_ESCAPE = "InteractionEscape";

		public const string INTERACTION_LOOTING = "InteractionLooting";

		public const string ITEM_DROP = "ItemDrop";

		public const string ITEM_RELEASE = "ItemRelease";

		public const string LOOTING_COMPLETED = "LootingCompleted";

		public const string DOOR_INTERACTED = "DoorInteracted";

		public const string STORAGE_TO_STASH = "ItemRelease";

		public const string LEVEL_UP = "LevelUp";

		public const string MAP_ROLLING = "MapRolling";

		public const string SKILL_SELECT = "SkillSelect";

		public const string ROOM_DIFFICULTY_SELECT = "select";

		public const string ROOM_DIFFICULTY_CHANGE = "RiskLevel_Change";

		public const string SKILL_INPUT_SELECT = "select";

		public const string SKILL_SLOT_SELECT = "select2";

		public const string CHARACTER_SELECT = "select2";

		public const string INPUT_SKILL_FAIL = "InputSkillFail";

		public const string INVENTORY_POPUP_OPEN = "InventoryPopupOpen";

		public const string PARADOX_HEAL = "ParadoxHeal";

		public const string CRAFTING_SUCCESS = "EquipmentProduction";

		public const string TALENT_RESET = "Casher";

		public const string BUY_ITEM = "Casher";

		public const string BUY_ITEM_COIN_ONLINE = "Casher_Coin_Online";

		public const string BUY_ITEM_COIN_ACTUAL = "Casher_Coin_Actual";

		public const string BUY_ITEM_ETC = "Casher_Etc";

		public const string SELL_ITEM = "Casher";

		public const string SCRAP = "Scraping";

		public const string REPAIR_SUCCESS = "Repair_Success";

		public const string REROLL_LOSTITEM = "Casher";

		public const string EXCHANGE_COIN_TO_ACTUAL = "Casher_Coin_Actual";

		public const string EXCHANGE_COIN_TO_ONLINE = "Casher_Coin_Online";

		public const string COUNT_DOWN = "CountDown";

		public const string WAY_OUT_READY = "WayOutReady";

		public const string MINIMAP_TOGGLE = "MapRolling";

		public const string EQUIP = "Equip";

		public const string EQUIP_ARTIFACT = "Equip2";

		public const string WEAPON_SWAP = "Weapon_Swap";

		public const string WEAPON_BROKEN = "WeaponBroken";

		public const string PAPER = "Paper";

		public const string LIGHT_ON_OFF = "FlashLight_OnOff";

		public const string STRESS_RECOVERY = "San_Up";

		public const string STRESS_DAMAGE = "San_Down";

		public const string HP_RECOVERY = "ConsumeItem_Heal";

		public const string REVOLVER_SCROLL = "Revolver_Spin";

		public const string THRESHOLD_OF_DEATH = "ThresholdOfDeath";

		public const string THRESHOLD_OF_DEATH_RESTORE = "ThresholdOfDeathRestore";

		public const string ARTIFACT_SLOT_CLICK = "ButtonClick2";

		public const string ARTIFACT_SELL = "Artifact_Sell";

		public const string ARTIFACT_SELL_NO_CUSTOMER = "casher";

		public const string NPC_CUSTOMER_BUY = "NPC_Customer_Buy";

		public const string TRAIT_UNLOCK = "Casher";

		public const string TALENT_OPEN = "TalentOpen";

		public const string TYCOON_MENU_OPEN = "ArtifactUI";

		public const string TALENT_UNLOCK = "Talent_Success";

		public const string LOSTITEM_RECOVER = "Casher_Coin_Online";

		public const string LOSTITEM_SELECT = "ButtonClick";

		public const string GUEST_ENTER = "GuestEnter";

		public const string GUEST_LEAVE = "GuestLeave";

		public const string RECIPE_SUCCESS = "Recipe_Success";

		public const string LOBBY_CREATE = "Lounge_Create";

		public const string BAG_USE = "Bag_Use";

		public const string NPC_OPEN = "NPC_Open";

		public const string RESTORE = "Talent_Success";

		public const string NPC_HOLDING = "NPC_default";

		public const string ENTRANCE_HOLDING = "DoorInteractied2";

		public const string ENTRANCE_HOLDING_LOCKED = "DoorKey";

		public const string ENTRANCE_INTERACTION = "DoorOpen";

		public const string PORTAL_HOLDING = "FootStep";

		public const string PORTAL_INTERACTION = "Portal_Move";

		public const string DUNGEONENTRANCE_HOLDING = "InteractionDoor";

		public const string DUNGEONENTRANCE_INTERACTION = "EnterPhoneBooth";

		public const string LOOTING_HOLDING = "DoorInteractied2";

		public const string LOOTING_HOLDING_LOCKED = "DoorKey";

		public const string BOOTH_ESCAPE_START_DELAY = "BoothEscape_Start_Safe";

		public const string BOOTH_ESCAPE_START = "BoothEscape_Start";

		public const string BOOTH_ESCAPE_END = "BoothEscape_End";

		public const string WEAKPOINT_FIRE = "Elemental_Fire_Hit";

		public const string WEAKPOINT_WATER = "Elemental_Water_Hit";

		public const string WEAKPOINT_WIND = "Elemental_Wind_Hit";

		public const string SAN_DESPAIR = "San_Despair";

		public const string GOODS_REVEAL_LOOP = "Looting_Loop";

		public const string GOODS_REVEAL_COMPLETE_LOW = "Looting_low";

		public const string GOODS_REVEAL_COMPLETE_MIDDLE = "Looting_middle";

		public const string GOODS_REVEAL_COMPLETE_HIGH = "Looting_high";

		public const string GOODS_REVEAL_COMPLETE_VERY_HIGH = "Looting_veryhigh";

		public const string EMPLOYEE_STAMP = "Employee_Stamp";

		public const string EMPLOYEE_DEPLOYMENT = "Employee_Deployment";

		public const string EMPLOYEE_CHANGE = "Employee_Change";

		public const string EMPLOYEE_START = "Employee_Start";

		public const string EMPLOYEE_PURCHASE_SUCCESS = "Employee_Purchase_Success";

		public const string EMPLOYEE_PANIC = "Employee_Panic";

		public const string EMPLOYEE_PANIC_DESTROY = "Employee_Panic_Destroy";

		public const string EMPLOYEE_PANIC_DESTROY2 = "Employee_Panic_Destroy2";

		public const string EMPLOYEE_FIRE = "Employee_Fire";

		public const string EMPLOYEE_LEVEL_UP = "Employee_LevelUp";

		public const string EMPLOYEE_PURIFY = "ParadoxHeal";

		public const string EMPLOYEE_SAY1 = "Employee_Say1";

		public const string EMPLOYEE_SAY2 = "Employee_Say2";

		public const string EMPLOYEE_SAY3 = "Employee_Say3";

		public const string TYCOON_ARTIFACT_PLACE = "ArtifactUI";

		public const string TYCOON_ARTIFACT_REMOVE = "ArtifactUI";

		public const string TYCOON_SALE_CANCEL = "ArtifactUI";

		public const string INTERIOR_CHANGE = "Interior_Change";

		public const string ARTIFACT_DEAL_START = "ArtifactDeal_Start";

		public const string ARTIFACT_DEAL_CARD_SHUFFLE1 = "ArtifactDeal_Card_Shuffle1";

		public const string ARTIFACT_DEAL_CARD_SHUFFLE2 = "ArtifactDeal_Card_Shuffle2";

		public const string ARTIFACT_DEAL_CARD_SHUFFLE3 = "ArtifactDeal_Card_Shuffle3";

		public const string ARTIFACT_DEAL_CARD_SHUFFLE_START = "ArtifactDeal_Card_Shuffle_Start";

		public const string ARTIFACT_DEAL_CARD_USE = "ArtifactDeal_Card_Use";

		public const string ARTIFACT_DEAL_CARD_FAIL = "ArtifactDeal_Card_Fail";

		public const string ARTIFACT_DEAL_CARD_SUCCESS = "ArtifactDeal_Card_Success";

		public const string ARTIFACT_DEAL_GUEST_SAY1 = "ArtifactDeal_GuestSay1";

		public const string ARTIFACT_DEAL_GUEST_SAY2 = "ArtifactDeal_GuestSay2";

		public const string ARTIFACT_DEAL_GUEST_SAY3 = "ArtifactDeal_GuestSay3";

		public const string ARTIFACT_DEAL_CARD_UNUSABLE = "Fail";

		public const string ARTIFACT_DEAL_MINUS = "ArtifactDeal_Minus";

		public const string ARTIFACT_DEAL_PLUS = "ArtifactDeal_Plus";

		public const string ARTIFACT_DEAL_TAG_CREATE = "ArtifactDeal_Tag_Create";

		public const string ARTIFACT_DEAL_TAG_DELETE = "ArtifactDeal_Tag_Delete";

		public const string ARTIFACT_DEAL_TAG_MATCH_GOOD = "ArtifactDeal_Tag_Match_Good";

		public const string ARTIFACT_DEAL_TAG_MATCH_BAD = "ArtifactDeal_Tag_Match_Bad";

		public const string ARTIFACT_DEAL_TAG_SET_MATCH = "ArtifactDeal_Tag_SetMatch";

		public const string ARTIFACT_DEAL_TAG_MATCH_SET = "ArtifactDeal_Tag_Match_Set";

		public const string ARTIFACT_DEAL_ROLLING = "ArtifactDeal_Rolling";

		public const string ARTIFACT_DEAL_SUCCESS = "ArtifactDeal_Success";

		public const string ARTIFACT_DEAL_GIVE_UP = "CampaignFail";
	}

	public static class Spine
	{
		public const string CHARACTER = "Character";

		public const string NPC = "Npc";
	}

	public static class Steam
	{
		public const uint APP_ID = 3438920u;

		public const string KEY_VERSION = "Version";

		public const string KEY_HOST_LEVEL = "HostLevel";

		public const string KEY_HOST_DIFFICULTY = "Difficulty";

		public const string KEY_ROOM_PUBLISH = "RoomPublish";

		public const string KEY_GAME_STARTED = "GameStarted";
	}

	public static class Text
	{
		public const string RAND_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+[]{}|;:'\",.<>?/`~";

		public const string RAND_CHARS_KOR = "삶과죽음을가르는경계는본디희미하고모호한법.과연누가그것의끝과시작을말할수있으랴?";
	}

	public static class Texture
	{
		public const string MINI_MAP = "MiniMap";

		public const string PORTRAIT = "Portrait";

		public const string DialogImage = "DialogImage";
	}

	public static class Tween
	{
		public const float GAUGE_DURATION_FAST = 0.125f;

		public const float GAUGE_DURATION_DEFAULT = 0.25f;

		public const float GAUGE_DURATION_SLOW = 0.5f;
	}

	public static class UnitAnimation
	{
		public const string DEATH = "death";

		public const string REVIVE = "revive";

		public const string IDLE = "idle";

		public const string WALK = "walk";

		public const string RUN = "run";

		public const string ALERT = "alert";
	}

	public static class Vfx
	{
		public const string WEAKPOINT_FIRE = "Common/Elemental_Fire_Hit";

		public const string WEAKPOINT_WATER = "Common/Elemental_Water_Hit";

		public const string WEAKPOINT_WIND = "Common/Elemental_Wind_Hit";

		public const string STRESS_DAMAGE = "Common/StressDamage";

		public const string STRESS_RECOVERY = "Common/StressRecovery";

		public const string HP_RECOVERY = "Common/Heal_Middle";

		public const string FROZEN_END = "Status/Frozen_end";

		public const string NPC_SPAWN = "Common/Npc_Spawn";

		public const string WEAPON_BROKEN = "Status/WeaponBroken";
	}

	public static class VfxAnimationTrigger
	{
		public const string END_TRIGGER = "EndTrigger";

		public const string TIMEOUT_TRIGGER = "TimeoutTrigger";

		public const string DESTROY_TRIGGER = "DestroyTrigger";
	}
}
