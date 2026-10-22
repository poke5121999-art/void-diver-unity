using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using Tracker;
using UniRx;
using Unity.Netcode;
using VContainer;

public class GamePlayer : NativeBase
{
	[Inject]
	private NetworkManager _networkManager;

	private readonly TrackableProperty<Character> _trackableCharacter;

	private readonly TrackableDictionary<int, GoodsSlot> _trackableInventorySlots;

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSafeInventorySlots;

	private readonly TrackableProperty<Talent> _trackableTalent;

	private readonly TrackableProperty<Profile> _trackableProfile;

	private readonly TrackableProperty<PointMarkers> _trackablePointMarkers;

	private readonly Dictionary<ETalentEffectType, List<TalentEffect>> _talentEffects;

	private readonly Dictionary<ETalentEffectType, int> _talentAccumulatedEffectValues;

	public ulong ClientId { get; set; }

	public string Name { get; set; }

	public bool IsHostPlayer { get; set; }

	public ulong SteamId { get; set; }

	public int PlayerIndex { get; set; }

	private bool IsHost => false;

	public bool HasChange => false;

	public ReactiveProperty<Character> RxCharacter { get; }

	public ReactiveDictionary<int, GoodsSlot> RxInventorySlots { get; }

	public ReactiveDictionary<int, GoodsSlot> RxSafeInventorySlots { get; }

	public ReactiveProperty<Talent> RxTalent { get; }

	public ReactiveProperty<Profile> RxProfile { get; }

	public ReactiveProperty<PointMarkers> RxPointMarkers { get; }

	public Character Character
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public IDictionary<int, GoodsSlot> InventorySlots => null;

	public IDictionary<int, GoodsSlot> SafeInventorySlots => null;

	public Talent Talent
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public Profile Profile
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public PointMarkers PointMarkers
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public int SlotCount => 0;

	public int SafeSlotCount => 0;

	public int StashSlotCount => 0;

	public int ActiveSlotCount => 0;

	public int RecoveryItemCount => 0;

	public int LightFuelEfficiency => 0;

	public int GuestCount => 0;

	public GamePlayerData Flush()
	{
		return null;
	}

	public GamePlayer(GamePlayerData data)
	{
	}

	public void ApplyChanges(GamePlayerData gamePlayerData)
	{
	}

	private void ApplyPointMarkers(PointMarkers data)
	{
	}

	private void ApplyCharacter(Character character)
	{
	}

	private void ApplyInventorySlots(GoodsSlot[] inventorySlots)
	{
	}

	private void ApplySafeInventorySlots(GoodsSlot[] safeInventorySlots)
	{
	}

	private void ApplyUser(Profile profile)
	{
	}

	public int GetInventoryItemCountMax(int itemId)
	{
		return 0;
	}

	public List<int> GetSelectableActiveSkillIds()
	{
		return null;
	}

	public List<int> GetPassiveSkillIds()
	{
		return null;
	}

	public int GetBaseSkillId(int skillId)
	{
		return 0;
	}

	public bool IsModeSkill(int skillId)
	{
		return false;
	}

	public Character UpdateSkillLockState(Character character)
	{
		return null;
	}

	private void ApplyTalent(Talent talent)
	{
	}

	public IList<TalentEffect> GetTalentEffects(ETalentEffectType effectType)
	{
		return null;
	}

	public int GetTalentEffectValue(ETalentEffectType effectType)
	{
		return 0;
	}

	public void UnlockTalent(int talentId)
	{
	}

	public bool HasTalent(int talentId)
	{
		return false;
	}
}
