using System;
using System.Collections.Generic;
using GameCommon.Data;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using Tracker;
using UniRx;
using Unity.Netcode;
using VContainer;

public class GamePlayer : NativeBase
{
	[Inject]
	private NetworkManager _networkManager;

	private readonly TrackableProperty<Character> _trackableCharacter = new TrackableProperty<Character>();

	private readonly TrackableDictionary<int, GoodsSlot> _trackableInventorySlots = new TrackableDictionary<int, GoodsSlot>();

	private readonly TrackableDictionary<int, GoodsSlot> _trackableSafeInventorySlots = new TrackableDictionary<int, GoodsSlot>();

	private readonly TrackableProperty<Talent> _trackableTalent = new TrackableProperty<Talent>();

	private readonly TrackableProperty<Profile> _trackableProfile = new TrackableProperty<Profile>();

	private readonly TrackableProperty<PointMarkers> _trackablePointMarkers = new TrackableProperty<PointMarkers>();

	private readonly Dictionary<ETalentEffectType, List<TalentEffect>> _talentEffects = new Dictionary<ETalentEffectType, List<TalentEffect>>();

	private readonly Dictionary<ETalentEffectType, int> _talentAccumulatedEffectValues = new Dictionary<ETalentEffectType, int>();

	public ulong ClientId { get; set; }

	public string Name { get; set; }

	public bool IsHostPlayer { get; set; }

	public ulong SteamId { get; set; }

	public int PlayerIndex { get; set; }

	private bool IsHost => _networkManager.IsHost;

	public bool HasChange => _trackableCharacter.HasChange || _trackableInventorySlots.HasChange || _trackableSafeInventorySlots.HasChange || _trackableTalent.HasChange || _trackableProfile.HasChange || _trackablePointMarkers.HasChange;

	public ReactiveProperty<Character> RxCharacter { get; } = new ReactiveProperty<Character>();

	public ReactiveDictionary<int, GoodsSlot> RxInventorySlots { get; } = new ReactiveDictionary<int, GoodsSlot>();

	public ReactiveDictionary<int, GoodsSlot> RxSafeInventorySlots { get; } = new ReactiveDictionary<int, GoodsSlot>();

	public ReactiveProperty<Talent> RxTalent { get; } = new ReactiveProperty<Talent>();

	public ReactiveProperty<Profile> RxProfile { get; } = new ReactiveProperty<Profile>();

	public ReactiveProperty<PointMarkers> RxPointMarkers { get; } = new ReactiveProperty<PointMarkers>();

	public Character Character
	{
		get
		{
			if (!IsHost)
			{
				return RxCharacter.Value;
			}
			return _trackableCharacter.Value;
		}
		set
		{
			_networkManager.AssertIfNotHost();
			_trackableCharacter.Value = value;
		}
	}

	public IDictionary<int, GoodsSlot> InventorySlots => IsHost ? _trackableInventorySlots : RxInventorySlots;

	public IDictionary<int, GoodsSlot> SafeInventorySlots => IsHost ? _trackableSafeInventorySlots : RxSafeInventorySlots;

	public Talent Talent
	{
		get
		{
			if (!IsHost)
			{
				return RxTalent.Value;
			}
			return _trackableTalent.Value;
		}
		set
		{
			_networkManager.AssertIfNotHost();
			_trackableTalent.Value = value;
		}
	}

	public Profile Profile
	{
		get
		{
			if (!IsHost)
			{
				return RxProfile.Value;
			}
			return _trackableProfile.Value;
		}
		set
		{
			_networkManager.AssertIfNotHost();
			_trackableProfile.Value = value;
		}
	}

	public PointMarkers PointMarkers
	{
		get
		{
			if (!IsHost)
			{
				return RxPointMarkers.Value;
			}
			return _trackablePointMarkers.Value;
		}
		set
		{
			_networkManager.AssertIfNotHost();
			_trackablePointMarkers.Value = value;
		}
	}

	public int SlotCount => TConst.CharacterInventorySlotCount.IntValue + GetTalentEffectValue(ETalentEffectType.InventorySlotCount);

	public int SafeSlotCount => TConst.SafeInventorySlotBaseCount.IntValue + GetTalentEffectValue(ETalentEffectType.SafeInventorySlotCount);

	public int StashSlotCount => TConst.StashSlotCount.IntValue + GetTalentEffectValue(ETalentEffectType.StashSlotCount);

	public int ActiveSlotCount => GetTalentEffectValue(ETalentEffectType.SkillSlotCount) + 2;

	public int RecoveryItemCount => GetTalentEffectValue(ETalentEffectType.RecoveryItemCount);

	public int LightFuelEfficiency => GetTalentEffectValue(ETalentEffectType.LightFuelEfficiency);

	public int GuestCount => GetTalentEffectValue(ETalentEffectType.GuestCount);

	public GamePlayerData Flush()
	{
		_networkManager.AssertIfNotHost();
		if (!HasChange)
		{
			return null;
		}
		GamePlayerData gamePlayerData = new GamePlayerData
		{
			ClientId = ClientId,
			Character = _trackableCharacter.Flush(),
			InventorySlots = _trackableInventorySlots.Flush(),
			SafeInventorySlots = _trackableSafeInventorySlots.Flush(),
			Talent = _trackableTalent.Flush(),
			Profile = _trackableProfile.Flush(),
			PointMarkers = _trackablePointMarkers.Flush()
		};
		ApplyChanges(gamePlayerData);
		return gamePlayerData;
	}

	public GamePlayer(GamePlayerData data)
	{
		ClientId = data.ClientId;
		Name = data.Name;
		IsHostPlayer = data.IsHostPlayer;
		SteamId = data.SteamId;
		PlayerIndex = data.PlayerIndex;
		if (_networkManager.IsHost)
		{
			_trackableCharacter.Value = data.Character;
			data.InventorySlots?.ForEach(delegate(GoodsSlot slot)
			{
				if (slot != null)
				{
					_trackableInventorySlots.Add(slot.SlotIndex, slot);
				}
			});
			data.SafeInventorySlots?.ForEach(delegate(GoodsSlot slot)
			{
				if (slot != null)
				{
					_trackableSafeInventorySlots.Add(slot.SlotIndex, slot);
				}
			});
			_trackableTalent.Value = data.Talent;
			_trackableProfile.Value = data.Profile;
			_trackablePointMarkers.Value = data.PointMarkers;
			Flush();
			return;
		}
		RxCharacter.Value = data.Character;
		data.InventorySlots?.ForEach(delegate(GoodsSlot slot)
		{
			if (slot != null)
			{
				RxInventorySlots.Add(slot.SlotIndex, slot);
			}
		});
		data.SafeInventorySlots?.ForEach(delegate(GoodsSlot slot)
		{
			if (slot != null)
			{
				RxSafeInventorySlots.Add(slot.SlotIndex, slot);
			}
		});
		ApplyTalent(data.Talent);
		RxProfile.Value = data.Profile;
		RxPointMarkers.Value = data.PointMarkers;
	}

	public void ApplyChanges(GamePlayerData gamePlayerData)
	{
		if (gamePlayerData == null)
		{
			return;
		}
		Character character = gamePlayerData.Character;
		if (character != null)
		{
			RxCharacter.Value = ((character.Operation == EDataOperation.Remove) ? null : character);
		}
		ApplyInventorySlots(gamePlayerData.InventorySlots);
		ApplySafeInventorySlots(gamePlayerData.SafeInventorySlots);
		ApplyTalent(gamePlayerData.Talent);
		Profile profile = gamePlayerData.Profile;
		if (profile != null)
		{
			RxProfile.Value = ((profile.Operation == EDataOperation.Remove) ? null : profile);
		}
		PointMarkers pointMarkers = gamePlayerData.PointMarkers;
		if (pointMarkers != null)
		{
			RxPointMarkers.Value = pointMarkers;
		}
	}

	private void ApplyPointMarkers(PointMarkers data)
	{
		if (data != null)
		{
			RxPointMarkers.Value = data;
		}
	}

	private void ApplyCharacter(Character character)
	{
		if (character != null)
		{
			RxCharacter.Value = ((character.Operation == EDataOperation.Remove) ? null : character);
		}
	}

	private void ApplyInventorySlots(GoodsSlot[] inventorySlots)
	{
		inventorySlots?.ForEach(delegate(GoodsSlot slot)
		{
			ApplySlot(RxInventorySlots, slot);
		});
	}

	private void ApplySafeInventorySlots(GoodsSlot[] safeInventorySlots)
	{
		safeInventorySlots?.ForEach(delegate(GoodsSlot slot)
		{
			ApplySlot(RxSafeInventorySlots, slot);
		});
	}

	private static void ApplySlot(ReactiveDictionary<int, GoodsSlot> slots, GoodsSlot slot)
	{
		if (slot == null)
		{
			return;
		}
		if (slot.Operation == EDataOperation.Remove)
		{
			slots.Remove(slot.SlotIndex);
		}
		if (slot.Operation == EDataOperation.Add)
		{
			slots.Add(slot.SlotIndex, slot);
		}
		if (slot.Operation == EDataOperation.Replace)
		{
			slots[slot.SlotIndex] = slot;
		}
	}

	private void ApplyUser(Profile profile)
	{
		if (profile != null)
		{
			RxProfile.Value = ((profile.Operation == EDataOperation.Remove) ? null : profile);
		}
	}

	public int GetInventoryItemCountMax(int itemId)
	{
		TItem tItem = TItem.Table[itemId];
		int num = tItem.InventoryCountMax;
		if (tItem.IsRecoveryItem())
		{
			num += GetTalentEffectValue(ETalentEffectType.RecoveryItemCount);
		}
		return num;
	}

	public List<int> GetSelectableActiveSkillIds()
	{
		List<int> skillIds = TCharacter.Table[RxCharacter.Value.Id].ActiveSkillIds.ToList();
		List<int> list = GetTalentEffects(ETalentEffectType.AddSkill).Where((TalentEffect e) => TSkill.Table[e.Value1].SkillType == ESkillType.Active).Select((TalentEffect e) => e.Value1).ToList();
		if (list.Any())
		{
			skillIds.AddRange(list);
		}
		List<int> list2 = GetTalentEffects(ETalentEffectType.UnlockSkillMode).Where((TalentEffect e) => skillIds.Contains(e.Value1)).Select((TalentEffect e) => e.Value2).ToList();
		if (list2.Any())
		{
			skillIds.AddRange(list2);
		}
		foreach (TalentEffect e in GetTalentEffects(ETalentEffectType.ChangeSkill))
		{
			int num = skillIds.IndexOf(e.Value1);
			if (num > -1)
			{
				skillIds[num] = e.Value2;
			}
		}
		return skillIds.Distinct().OrderBy((int id) => id).ToList();
	}

	public List<int> GetPassiveSkillIds()
	{
		TCharacter tCharacter = TCharacter.Table[RxCharacter.Value.Id];
		List<int> list = ((tCharacter.PassiveSkillIds != null) ? tCharacter.PassiveSkillIds.ToList() : new List<int>());
		List<int> list2 = GetTalentEffects(ETalentEffectType.AddSkill).Where((TalentEffect e) => TSkill.Table[e.Value1].SkillType == ESkillType.Passive).Select((TalentEffect e) => e.Value1).ToList();
		if (list2.Any())
		{
			list.AddRange(list2);
		}
		return list;
	}

	public int GetBaseSkillId(int skillId)
	{
		foreach (TalentEffect talentEffect in GetTalentEffects(ETalentEffectType.UnlockSkillMode))
		{
			if (talentEffect.Value2 == skillId)
			{
				return talentEffect.Value1;
			}
		}
		foreach (TalentEffect talentEffect2 in GetTalentEffects(ETalentEffectType.ChangeSkill))
		{
			if (talentEffect2.Value2 == skillId)
			{
				return talentEffect2.Value1;
			}
		}
		return skillId;
	}

	public bool IsModeSkill(int skillId)
	{
		foreach (TalentEffect talentEffect in GetTalentEffects(ETalentEffectType.UnlockSkillMode))
		{
			if (talentEffect.Value2 == skillId)
			{
				return true;
			}
		}
		return false;
	}

	public Character UpdateSkillLockState(Character character)
	{
		if (character == null || character.SelectedActiveSkillIds == null || character.SelectedActiveSkillIds.Length == 0)
		{
			return character;
		}
		List<int> selectableActiveSkillIds = GetSelectableActiveSkillIds();
		if (TCharacter.Table[character.Id].PolymorphActiveSkillIds != null)
		{
			selectableActiveSkillIds.AddRange(TCharacter.Table[character.Id].PolymorphActiveSkillIds);
		}
		HashSet<int> hashSet = new HashSet<int>(selectableActiveSkillIds);
		bool flag = false;
		bool[] array = character.LockedActiveSkillSlots ?? new bool[character.SelectedActiveSkillIds.Length];
		if (array.Length != character.SelectedActiveSkillIds.Length)
		{
			array = new bool[character.SelectedActiveSkillIds.Length];
		}
		for (int i = 0; i < character.SelectedActiveSkillIds.Length; i++)
		{
			int num = character.SelectedActiveSkillIds[i];
			bool flag2 = num > 0 && !hashSet.Contains(num);
			if (array[i] != flag2)
			{
				flag = true;
				array[i] = flag2;
			}
		}
		if (!flag)
		{
			return character;
		}
		Character character2 = character.Clone();
		character2.LockedActiveSkillSlots = array;
		return character2;
	}

	private void ApplyTalent(Talent talent)
	{
		if (talent == null)
		{
			return;
		}
		TTalent[] newTalentDatas = GetNewTalentDatas();
		if (newTalentDatas != null)
		{
			foreach (TalentEffect effect in newTalentDatas.Where((TTalent t) => t.Effects != null).SelectMany((TTalent t) => t.Effects))
			{
				if (effect.Type.IsAccumulatedType())
				{
					_talentAccumulatedEffectValues.TryGetValue(effect.Type, out var value);
					_talentAccumulatedEffectValues[effect.Type] = effect.Value1 + value;
				}
				if (!_talentEffects.TryGetValue(effect.Type, out var effects))
				{
					effects = new List<TalentEffect>();
					_talentEffects.Add(effect.Type, effects);
				}
				effects.Add(effect);
			}
		}
		RxTalent.Value = ((talent.Operation == EDataOperation.Remove) ? null : talent);

		TTalent[] GetNewTalentDatas()
		{
			BitFlags bitFlags = RxTalent.Value?.BitFlags ?? new BitFlags();
			int[] addedBitFlags = bitFlags.GetAddedBitFlags(talent.BitFlags);
			if (!addedBitFlags.Any())
			{
				return null;
			}
			return addedBitFlags.Select((int index) => TTalent.SubTable[index]).ToArray();
		}
	}

	public IList<TalentEffect> GetTalentEffects(ETalentEffectType effectType)
	{
		if (_talentEffects.TryGetValue(effectType, out var value))
		{
			return value;
		}
		return new List<TalentEffect>();
	}

	public int GetTalentEffectValue(ETalentEffectType effectType)
	{
		if (!_talentAccumulatedEffectValues.TryGetValue(effectType, out var value))
		{
			return 0;
		}
		return value;
	}

	public void UnlockTalent(int talentId)
	{
		int bitFlagIndex = TTalent.Table[talentId].BitFlagIndex;
		if (Talent == null || Talent.BitFlags == null || Talent.BitFlags.IsSet(bitFlagIndex))
		{
			return;
		}
		Talent talent = Talent.Clone();
		talent.BitFlags.Set(bitFlagIndex);
		_networkManager.AssertIfNotHost();
		_trackableTalent.Value = talent;
	}

	public bool HasTalent(int talentId)
	{
		Talent talent = Talent;
		if (talent == null || talent.BitFlags == null)
		{
			return false;
		}
		return talent.BitFlags.IsSet(TTalent.Table[talentId].BitFlagIndex);
	}
}
