using System;
using System.Linq;
using GameCommon.Table;
using System.Collections.Generic;
using GameCommon.Data;
using GameCommon.Enum;
using UniRx;
using Unity.Netcode;
using UnityEngine;

public class CharacterModel : UnitModel
{
	private bool isInitialized;

	public readonly NetworkVariable<ulong> SyncClientId = new NetworkVariable<ulong>(ulong.MaxValue);

	public readonly NetworkVariable<int> SyncHitLayer = new NetworkVariable<int>();

	public readonly NetworkVariable<bool> SyncIsWaitingInteractionFinish = new NetworkVariable<bool>();

	public int CharacterId;

	public readonly ReactiveProperty<string> RxPlayerName = new ReactiveProperty<string>();

	public readonly ReactiveProperty<EStressState> RxStressState = new ReactiveProperty<EStressState>();

	public readonly ReactiveProperty<ECorruptionState> RxCorruptionState = new ReactiveProperty<ECorruptionState>();

	public readonly Subject<Unit> OnInventoryInitialized = new Subject<Unit>();

	public bool CanCancelInputInteraction = true;

	private GamePlayer _player;

	public int SelfReviveCount;

	public float TryRunExpireTime;

	private List<int> _currentEquipmentBuffIds = new List<int>();

	private List<int> _currentEquipmentPassiveSkillIds = new List<int>();

	private List<int> _currentEquipmentActiveSkillIds = new List<int>();

	private List<int> _allEquipmentActiveSkillIds = new List<int>();

	private HashSet<EStatType> _previousEquipmentStatTypes = new HashSet<EStatType>();

	private NetworkVariable<bool> _syncIsLightOn = new NetworkVariable<bool>();

	private NetworkVariable<int> _syncLightFuel = new NetworkVariable<int>();

	private NetworkVariable<bool> _syncTryRun = new NetworkVariable<bool>();

	private NetworkVariable<bool> _syncIsMovingBackward = new NetworkVariable<bool>();

	private readonly ReactiveProperty<bool> _rxIsLightOn = new ReactiveProperty<bool>();

	private readonly ReactiveProperty<int> _rxLightFuel = new ReactiveProperty<int>();

	private readonly ReactiveProperty<bool> _rxTryRun = new ReactiveProperty<bool>();

	private readonly ReactiveProperty<bool> _rxIsMovingBackward = new ReactiveProperty<bool>();

	protected Dictionary<EStatType, float> EquipmentStats = new Dictionary<EStatType, float>();

	private Dictionary<EStatType, float> _stressConditionalStats = new Dictionary<EStatType, float>();

	private bool _stressConditionalStatsDirty;

	public override int HitLayer => SyncHitLayer.Value;

	public override int TargetHitLayer => 0;

	public override EUnitType UnitType => EUnitType.Character;

	public override int TableId => CharacterId;

	public override bool IsInitialized => isInitialized;

	public override float GlobalSkillCoolTime => 0f;

	public ulong ClientId => SyncClientId.Value;

	public bool IsWaitingInteractionFinish => SyncIsWaitingInteractionFinish.Value;

	public string PlayerName => RxPlayerName.Value;

	public override EElementalType ElementalType
	{
		get
		{
			if (TEquipment.Table.TryGetValue(WeaponId, out var value))
			{
				return value.ElementalType;
			}
			return EElementalType.None;
		}
	}

	public override float WeaponAtkMultiplier => TCharacter.Table[TableId].WeaponAtkMultiplier;

	public override EMonsterCategory[] MonsterCategories => null;

	public override EStressState StressState => RxStressState.Value;

	public ECorruptionState CorruptionState => RxCorruptionState.Value;

	public int SlotCount => TConst.CharacterInventorySlotCount.IntValue + _player.GetTalentEffectValue(ETalentEffectType.InventorySlotCount);

	public int SafeSlotCount => TConst.SafeInventorySlotBaseCount.IntValue + _player.GetTalentEffectValue(ETalentEffectType.SafeInventorySlotCount);

	public PlayerInventory Inventory { get; private set; }

	public override int WeaponId => _player?.Character?.WeaponId ?? 0;

	public IEnumerable<int> AccessoryIds => _player?.Character?.AccessoryIds?.Where((int id) => 0 < id) ?? Enumerable.Empty<int>();

	public IEnumerable<Goods> Artifacts => _player?.Character?.ArtifactSlots?.Select((GoodsSlot s) => s.Goods).Where((Goods g) => g != null) ?? Enumerable.Empty<Goods>();

	private IEnumerable<int> EquipmentIds => new int[1] { WeaponId }.Concat(AccessoryIds).Concat(Artifacts.Select((Goods g) => g.Id));

	private IEnumerable<int> ActiveEquipmentIds
	{
		get
		{
			List<int> list = new List<int>();
			Character character = _player?.Character;
			if (character != null)
			{
				GoodsSlot activeWeaponSlot = character.ActiveWeaponSlot;
				if (activeWeaponSlot.Goods != null && !activeWeaponSlot.Goods.IsBrokenEquipment() && WeaponId > 0)
				{
					list.Add(WeaponId);
				}
			}
			GoodsSlot[] array = _player?.Character?.AccessorySlots;
			if (array != null)
			{
				GoodsSlot[] array2 = array;
				foreach (GoodsSlot goodsSlot in array2)
				{
					if (goodsSlot.Goods != null && 0 < goodsSlot.Goods.Id && !goodsSlot.Goods.IsBrokenEquipment())
					{
						list.Add(goodsSlot.Goods.Id);
					}
				}
			}
			list.AddRange(Artifacts.Select((Goods g) => g.Id));
			return list;
		}
	}

	public Subject<int> OnEquipmentPassiveSkillAdded { get; } = new Subject<int>();

	public bool IsEquipmentActiveSkillLocked
	{
		get
		{
			int equipmentActiveSkillId = _player?.Character?.EquipmentActiveSkillId ?? 0;
			if (equipmentActiveSkillId <= 0)
			{
				return false;
			}
			return !_currentEquipmentActiveSkillIds.Contains(equipmentActiveSkillId);
		}
	}

	public IReadOnlyReactiveProperty<bool> RxIsLightOn => _rxIsLightOn;

	public IReadOnlyReactiveProperty<int> RxLightFuel => _rxLightFuel;

	public IReadOnlyReactiveProperty<bool> RxTryRun => _rxTryRun;

	public IReadOnlyReactiveProperty<bool> RxIsMovingBackward => _rxIsMovingBackward;

	public bool IsLightOn
	{
		get
		{
			return _rxIsLightOn.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncIsLightOn, value);
		}
	}

	public int LightFuel
	{
		get
		{
			return _rxLightFuel.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncLightFuel, value);
		}
	}

	public bool TryRun
	{
		get
		{
			return _rxTryRun.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncTryRun, value);
		}
	}

	public bool IsMovingBackward
	{
		get
		{
			return _rxIsMovingBackward.Value;
		}
		set
		{
			SetNetworkVariable(ref _syncIsMovingBackward, value);
		}
	}

	public void Init(GamePlayer player)
	{
		Character updated = null;
		_player = player;
		SelfReviveCount = 0;
		RxPlayerName.Value = player.Name;
		CharacterId = player.Character.Id;
		SkillLevels = player.Character.GetSkillLevels();
		UpdateEquipmentSkillIds();
		Character character = player.Character;
		if (NetworkManager.IsHost)
		{
			Character character2 = character.RevertPolymorphSkills();
			if (UpdateEquipmentActiveSkillSlot(character2, out updated))
			{
				character2 = updated;
			}
			character2 = player.UpdateSkillLockState(character2);
			if (!character2.HasSameSkills(character))
			{
				character = character2;
			}
			Observable.NextFrame().Subscribe(delegate
			{
				player.Character = character;
			}).AddTo(this);
		}
		TCharacter tCharacter = TCharacter.Table[character.Id];
		List<int> inputActiveSkillIds = character.GetInputActiveSkillIds();
		List<int> passiveSkillIds = player.GetPassiveSkillIds();
		Dictionary<EStatType, float> baseStats = tCharacter.CreateBaseStats();
		Init(baseStats, inputActiveSkillIds, passiveSkillIds, Vector3.zero);
		if (NetworkManager.IsHost)
		{
			ApplyEquipmentBuffEffects();
			ApplyEquipmentStats();
		}
		InitNetDataVariable();
		OnStatChanged.Where(((EStatType statType, UnitStat oldValue, UnitStat newValue) _) => _.statType == EStatType.HpMax).Subscribe(delegate(( EStatType statType, UnitStat oldValue, UnitStat newValue) _)
		{
			OnMaxHpChanged(_.oldValue, _.newValue);
		}).AddTo(this);
		player.RxCharacter.Pairwise().Subscribe(delegate(Pair<Character> _)
		{
			OnCharacterModified(_.Previous, _.Current);
		}).AddTo(this);
	}

	private void Start()
	{
		Inventory = GetComponent<PlayerInventory>();
		OnInventoryInitialized.OnNext(Unit.Default);
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		if (GameManager.RxViewTargetClientId.Value == SyncClientId.Value)
		{
			GameManager.RxViewTargetClientId.Value = ulong.MaxValue;
			GameManager.RxViewTargetClientId.Value = SyncClientId.Value;
		}
		RxStress.Subscribe(OnStressChanged).AddTo(this);
		RxCorruption.Subscribe(OnCorruptionChanged).AddTo(this);
		RxCorruptionMax.Subscribe(delegate
		{
			OnCorruptionChanged(Corruption);
		}).AddTo(this);
		isInitialized = true;
	}

	public override void UpdateAim()
	{
		AssertIfNotHost();
		Vector3 value = DeltaAimPos;
		if (value != Vector3.zero && TConst.AimIgnoreSqrRadius.FloatValue < value.x * value.x + value.z * value.z && (value - CalculatedDeltaAimPos).sqrMagnitude >= 0.0001f)
		{
			CalculatedDeltaAimPos = value;
		}
	}

	public override bool HasTalent(int talentId)
	{
		Talent talent = _player.Talent;
		TTalent tTalent = TTalent.Table[talentId];
		if (tTalent != null && talent != null && talent.BitFlags != null)
		{
			return talent.BitFlags.IsSet(tTalent.BitFlagIndex);
		}
		return false;
	}

	private void OnStressChanged(float stress)
	{
		RxStressState.Value = TStress.Table.Values.Where((TStress s) => stress <= (float)s.StressMax).MinBy((TStress s) => (int)s.State)?.State ?? EStressState.Despair;
	}

	private void OnCorruptionChanged(int corruption)
	{
		float overflow = Math.Max(0f, (float)corruption - GetStat(EStatType.CorruptionMax));
		RxCorruptionState.Value = TCorruption.Table.Values.Where((TCorruption c) => (float)c.Threshold <= overflow).MaxBy((TCorruption c) => (int)c.State)?.State ?? ECorruptionState.Safe;
	}

	private void OnCharacterModified(Character oldCharacter, Character newCharacter)
	{
		Character updated = null;
		if (oldCharacter.Id != newCharacter.Id)
		{
			return;
		}
		bool flag = IsWeaponChanged();
		bool flag2 = IsAccessoryChanged();
		bool flag3 = IsArtifactChanged();
		bool flag4 = IsSkillChanged();
		if (flag3 || flag2 || flag)
		{
			UpdateEquipmentSkillIds();
		}
		if (NetworkManager.IsHost && (flag3 || flag2 || flag))
		{
			if (flag)
			{
				Logger.Debug(string.Format("WeaponChanged {0}->{1}", oldCharacter.WeaponId, newCharacter.WeaponId), Color.green);
			}
			if (flag3)
			{
				List<int> list = oldCharacter.ArtifactSlots.Select((GoodsSlot s) => s.Goods?.Id ?? 0).ToList();
				List<int> list2 = newCharacter.ArtifactSlots.Select((GoodsSlot s) => s.Goods?.Id ?? 0).ToList();
				Logger.Debug("ArtifactChanged " + string.Join(",", list) + "->" + string.Join(",", list2), Color.green);
			}
			ApplyEquipmentStats();
			ApplyEquipmentBuffEffects();
		}
		if (!NetworkManager.IsHost || (!flag4 && !flag3 && !flag2 && !flag))
		{
			return;
		}
		Character character = _player.Character;
		if ((flag3 || flag2 || flag) && UpdateEquipmentActiveSkillSlot(character, out updated))
		{
			character = updated;
		}
		character = _player.UpdateSkillLockState(character);
		if (!character.HasSameSkills(_player.Character))
		{
			_player.Character = character;
		}

		bool IsWeaponChanged()
		{
			if (oldCharacter.WeaponId != newCharacter.WeaponId)
			{
				return true;
			}
			bool flag5 = oldCharacter.ActiveWeaponSlot.Goods.IsBrokenEquipment();
			return newCharacter.ActiveWeaponSlot.Goods.IsBrokenEquipment() != flag5;
		}
		bool IsAccessoryChanged()
		{
			for (int i = 0; i < TConst.CharacterAccessorySlotCount.IntValue; i++)
			{
				Goods goods = oldCharacter.AccessorySlots[i].Goods;
				Goods goods2 = newCharacter.AccessorySlots[i].Goods;
				if ((goods?.Id ?? 0) != (goods2?.Id ?? 0))
				{
					return true;
				}
				if (goods.IsBrokenEquipment() != goods2.IsBrokenEquipment())
				{
					return true;
				}
			}
			return false;
		}
		bool IsArtifactChanged()
		{
			for (int j = 0; j < TConst.CharacterArtifactSlotCount.IntValue; j++)
			{
				if ((oldCharacter.ArtifactSlots[j].Goods?.Id ?? 0) != (newCharacter.ArtifactSlots[j].Goods?.Id ?? 0))
				{
					return true;
				}
			}
			return false;
		}
		bool IsSkillChanged()
		{
			if (oldCharacter.EquipmentActiveSkillId == newCharacter.EquipmentActiveSkillId && oldCharacter.AttackSkillId == newCharacter.AttackSkillId && oldCharacter.DashSkillId == newCharacter.DashSkillId)
			{
				int[] selectedActiveSkillIds = oldCharacter.SelectedActiveSkillIds;
				int[] selectedActiveSkillIds2 = newCharacter.SelectedActiveSkillIds;
				if (selectedActiveSkillIds == null)
				{
					return selectedActiveSkillIds2 != null;
				}
				if (selectedActiveSkillIds2 == null || selectedActiveSkillIds.Length != selectedActiveSkillIds2.Length)
				{
					return true;
				}
				for (int k = 0; k < selectedActiveSkillIds.Length; k++)
				{
					if (selectedActiveSkillIds[k] != selectedActiveSkillIds2[k])
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}
	}

	private void OnMaxHpChanged(UnitStat oldValue, UnitStat newValue)
	{
		int num = (int)(oldValue?.Value ?? 0f);
		int num2 = (int)(newValue?.Value ?? 0f);
		if (num < num2)
		{
			if (!GameManager.IsInLounge || HasBuffEffect(EBuffEffectType.TrainingRoom))
			{
				return;
			}
		}
		else
		{
			num2 = Math.Min(Hp, num2);
		}
		Hp = num2;
	}

	public long GetOwnedItemCount(int itemId)
	{
		if (State == EActionState.Dead)
		{
			return 0L;
		}
		return Inventory.GetItemCount(itemId);
	}

	public int CalculateGainWorth()
	{
		if (State == EActionState.Dead)
		{
			return 0;
		}
		Func<GoodsSlot, int> worth = (GoodsSlot s) => s.Goods?.GainWorth ?? 0;
		Character character = _player.Character;
		int num = character.WeaponSlots.Sum(worth);
		int num2 = character.AccessorySlots.Sum(worth);
		int num3 = character.ArtifactSlots.Sum(worth);
		int num4 = Inventory.Slots.Values.Sum(worth);
		int num5 = Inventory.SafeSlots.Values.Sum(worth);
		return num5 + num3 + num + num2 + num4;
	}

	public int CalculateTotalCorruption()
	{
		int num = 0;
		foreach (Goods artifact in Artifacts)
		{
			if (TEquipment.Table.TryGetValue(artifact.Id, out var value))
			{
				num += value.Corruption;
			}
		}
		foreach (GoodsSlot value3 in Inventory.Slots.Values)
		{
			if (!value3.IsEmpty && value3.Category == EGoodsCategory.Equipment && TEquipment.Table.TryGetValue(value3.Id, out var value2) && value2.GoodsType == EGoodsType.Artifact)
			{
				num += value2.Corruption;
			}
		}
		foreach (GoodsSlot value4 in Inventory.SafeSlots.Values)
		{
			if (!value4.IsEmpty && value4.Category == EGoodsCategory.Equipment && TEquipment.Table.TryGetValue(value4.Id, out var value5) && value5.GoodsType == EGoodsType.Artifact)
			{
				num += value5.Corruption;
			}
		}
		return num;
	}

	protected override void OnChangeSkillEffectActivated(ChangeSkillEffect effect)
	{
		ApplyChangeSkillOverride(effect.OriginalSkillId, effect.ReplacementSkillId);
		GameHost?.SendToAll(new NtfChangeSkill
		{
			NetworkObjectId = NetworkObjectId,
			OriginalSkillId = effect.OriginalSkillId,
			ReplacementSkillId = effect.ReplacementSkillId,
			IsRemove = false
		});
	}

	protected override void OnChangeSkillEffectDeactivated(ChangeSkillEffect effect)
	{
		RemoveChangeSkillOverride(effect.OriginalSkillId, effect.ReplacementSkillId);
		GameHost?.SendToAll(new NtfChangeSkill
		{
			NetworkObjectId = NetworkObjectId,
			OriginalSkillId = effect.OriginalSkillId,
			ReplacementSkillId = effect.ReplacementSkillId,
			IsRemove = true
		});
	}

	public void RefreshEquipmentEffects(bool isHost)
	{
		Character updated = null;
		ApplyEquipmentStats();
		if (isHost)
		{
			ApplyEquipmentBuffEffects();
			if (_player != null && UpdateEquipmentActiveSkillSlot(_player.Character, out updated))
			{
				_player.Character = updated;
			}
		}
		else
		{
			UpdateEquipmentSkillIds();
		}
	}

	private void ApplyEquipmentStats()
	{
		EquipmentStats.Clear();
		HashSet<EStatType> hashSet = new HashSet<EStatType>(_previousEquipmentStatTypes);
		_previousEquipmentStatTypes.Clear();
		GoodsSlot activeWeaponSlot = _player.Character.ActiveWeaponSlot;
		if (activeWeaponSlot.Goods != null && WeaponId > 0)
		{
			foreach (KeyValuePair<EStatType, float> item in activeWeaponSlot.Goods.GetEquipmentStat(checkBrokenStat: true))
			{
				EquipmentStats[item.Key] = EquipmentStats.GetValueOrDefault(item.Key) + item.Value;
				hashSet.Add(item.Key);
				_previousEquipmentStatTypes.Add(item.Key);
			}
		}
		GoodsSlot[] accessorySlots = _player.Character.AccessorySlots;
		foreach (GoodsSlot goodsSlot in accessorySlots)
		{
			if (goodsSlot.Goods == null || goodsSlot.Goods.Id <= 0)
			{
				continue;
			}
			foreach (KeyValuePair<EStatType, float> item2 in goodsSlot.Goods.GetEquipmentStat(checkBrokenStat: true))
			{
				EquipmentStats[item2.Key] = EquipmentStats.GetValueOrDefault(item2.Key) + item2.Value;
				hashSet.Add(item2.Key);
				_previousEquipmentStatTypes.Add(item2.Key);
			}
		}
		foreach (Goods artifact in Artifacts)
		{
			foreach (KeyValuePair<EStatType, float> item3 in artifact.GetEquipmentStat(checkBrokenStat: false))
			{
				EquipmentStats[item3.Key] = item3.Value + EquipmentStats.GetValueOrDefault(item3.Key);
				hashSet.Add(item3.Key);
				_previousEquipmentStatTypes.Add(item3.Key);
			}
		}
		foreach (EStatType item4 in hashSet)
		{
			UpdateStat(item4);
		}
	}

	private void UpdateEquipmentSkillIds()
	{
		List<int> list = new List<int>();
		list.AddRange(TEquipment.Table.GetEffectSkillIds(ActiveEquipmentIds));
		list.RemoveAll((int _) => _ < 1);
		_currentEquipmentPassiveSkillIds.Clear();
		_currentEquipmentActiveSkillIds.Clear();
		foreach (int item in list)
		{
			if (TSkill.Table[item].SkillType == ESkillType.Passive)
			{
				_currentEquipmentPassiveSkillIds.Add(item);
			}
			else if (TSkill.Table[item].SkillType == ESkillType.Active)
			{
				_currentEquipmentActiveSkillIds.Add(item);
			}
		}
		_allEquipmentActiveSkillIds.Clear();
		List<int> list2 = new List<int>();
		list2.AddRange(TEquipment.Table.GetEffectSkillIds(EquipmentIds));
		list2.RemoveAll((int _) => _ < 1);
		foreach (int item2 in list2)
		{
			if (TSkill.Table[item2].SkillType == ESkillType.Active)
			{
				_allEquipmentActiveSkillIds.Add(item2);
			}
		}
	}

	private void ApplyEquipmentBuffEffects()
	{
		List<int> list = new List<int>();
		list.AddRange(TEquipment.Table.GetEffectBuffIds(ActiveEquipmentIds));
		list.AddRange(TEquipment.Table.GetSetBuffIds(ActiveEquipmentIds));
		foreach (int item in _currentEquipmentBuffIds.Except(list))
		{
			RemoveBuff(item);
		}
		foreach (int item2 in list.Except(_currentEquipmentBuffIds))
		{
			AddBuff(item2, this);
		}
		_currentEquipmentBuffIds = list;
		List<int> list2 = new List<int>();
		list2.AddRange(TEquipment.Table.GetEffectSkillIds(EquipmentIds));
		list2.RemoveAll((int _) => _ < 1);
		_currentEquipmentPassiveSkillIds.ForEach(delegate(int _)
		{
			trackableSkills.Remove(_);
		});
		_currentEquipmentPassiveSkillIds.Where((int _) => TSkill.Table[_].RootActionNode != null).Select((int _) => TSkill.Table[_].RootActionNode).SelectMany((SkillActionNode _) => _.skillAction.actionEvents)
			.Select((ActionEvent _) => _ as BuffActionEvent)
			.Where((BuffActionEvent _) => _ != null)
			.Select((BuffActionEvent _) => _.Id)
			.ToList()
			.ForEach(delegate(int _)
			{
				RemoveBuff(_);
			});
		UpdateEquipmentSkillIds();
		_currentEquipmentPassiveSkillIds.ForEach(delegate(int _)
		{
			if (!trackableSkills.ContainsKey(_))
			{
				trackableSkills.Add(_, new UnitSkill(_));
				OnEquipmentPassiveSkillAdded.OnNext(_);
			}
		});
	}

	public List<int> GetEquipmentPassiveSkillIds()
	{
		return _currentEquipmentPassiveSkillIds ?? new List<int>();
	}

	public List<int> GetEquipmentActiveSkillIds()
	{
		return _currentEquipmentActiveSkillIds ?? new List<int>();
	}

	public List<int> GetAllEquipmentActiveSkillIds()
	{
		return _allEquipmentActiveSkillIds ?? new List<int>();
	}

	public bool IsEquipmentActiveSkillChargeBlocked(int skillId)
	{
		int equipmentActiveSkillId = _player?.Character?.EquipmentActiveSkillId ?? 0;
		if (skillId != equipmentActiveSkillId)
		{
			return false;
		}
		return IsEquipmentActiveSkillLocked;
	}

	private int ResolveEquipmentActiveSkillId(int currentSlotSkillId)
	{
		if (0 < currentSlotSkillId && _allEquipmentActiveSkillIds.Contains(currentSlotSkillId))
		{
			return currentSlotSkillId;
		}
		if (_allEquipmentActiveSkillIds.Count < 1)
		{
			return 0;
		}
		return _allEquipmentActiveSkillIds[0];
	}

	private void ResetEquipmentSkillGauge(int skillId)
	{
		if (skillId >= 1 && trackableSkills.ContainsKey(skillId))
		{
			UnitSkill unitSkill = trackableSkills[skillId].Clone();
			unitSkill.CoolEndTime = 0f;
			unitSkill.Stack = TSkill.Table[skillId].StackCount;
			unitSkill.ChargeAmount = 0f;
			trackableSkills[skillId] = unitSkill;
		}
	}

	public bool UpdateEquipmentActiveSkillSlot(Character character, out Character updated)
	{
		updated = character;
		int num = ResolveEquipmentActiveSkillId(character.EquipmentActiveSkillId);
		if (num == character.EquipmentActiveSkillId)
		{
			return false;
		}
		if (0 < character.EquipmentActiveSkillId)
		{
			ResetEquipmentSkillGauge(character.EquipmentActiveSkillId);
		}
		updated = character.Clone();
		updated.EquipmentActiveSkillId = num;
		return true;
	}

	protected override void RegisterVariableEvents()
	{
		base.RegisterVariableEvents();
		RegisterVariableEvent(_syncIsLightOn, _rxIsLightOn);
		RegisterVariableEvent(_syncLightFuel, _rxLightFuel);
		RegisterVariableEvent(_syncTryRun, _rxTryRun);
		RegisterVariableEvent(_syncIsMovingBackward, _rxIsMovingBackward);
	}

	protected override void UnregisterVariableEvents()
	{
		base.UnregisterVariableEvents();
		UnregisterVariableEvent(_syncIsLightOn);
		UnregisterVariableEvent(_syncLightFuel);
		UnregisterVariableEvent(_syncTryRun);
		UnregisterVariableEvent(_syncIsMovingBackward);
	}

	protected void InitNetDataVariable()
	{
		_rxLightFuel.Value = TConst.LightFuelDefault.IntValue;
		_syncLightFuel.Value = TConst.LightFuelDefault.IntValue;
		_rxIsLightOn.Value = false;
		_syncIsLightOn.Value = false;
		_rxTryRun.Value = false;
		_syncTryRun.Value = false;
		_rxIsMovingBackward.Value = false;
		_syncIsMovingBackward.Value = false;
	}

	public override bool CheckSkill(int skillId)
	{
		if (!base.CheckSkill(skillId))
		{
			return false;
		}
		Character character = _player?.Character;
		if (character != null)
		{
			bool flag;
			if (skillId >= 1 && skillId == character.EquipmentActiveSkillId)
			{
				flag = IsEquipmentActiveSkillLocked;
			}
			else
			{
				int[] selectedActiveSkillIds = character.SelectedActiveSkillIds;
				if (selectedActiveSkillIds == null || selectedActiveSkillIds.Length == 0)
				{
					return true;
				}
				int num = Array.IndexOf(selectedActiveSkillIds, skillId);
				if (num < 0)
				{
					return true;
				}
				bool[] lockedActiveSkillSlots = character.LockedActiveSkillSlots;
				if (lockedActiveSkillSlots == null || num >= lockedActiveSkillSlots.Length)
				{
					return true;
				}
				flag = lockedActiveSkillSlots[num];
			}
			if (flag)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsActiveSkillLocked(int skillId)
	{
		Character character = _player?.Character;
		if (character != null)
		{
			if (skillId > 0 && skillId == character.EquipmentActiveSkillId)
			{
				return IsEquipmentActiveSkillLocked;
			}
			int[] selectedActiveSkillIds = character.SelectedActiveSkillIds;
			if (selectedActiveSkillIds != null && selectedActiveSkillIds.Length != 0)
			{
				int num = Array.IndexOf(selectedActiveSkillIds, skillId);
				if (num >= 0)
				{
					bool[] lockedActiveSkillSlots = character.LockedActiveSkillSlots;
					if (lockedActiveSkillSlots != null && num < lockedActiveSkillSlots.Length)
					{
						return lockedActiveSkillSlots[num];
					}
				}
			}
		}
		return false;
	}

	public override float GetAdditionalStat(EStatType statType)
	{
		if (_stressConditionalStatsDirty)
		{
			RefreshStressConditionalStats();
		}
		float valueOrDefault = EquipmentStats.GetValueOrDefault(statType);
		float valueOrDefault2 = _stressConditionalStats.GetValueOrDefault(statType);
		return valueOrDefault2 + valueOrDefault;
	}

	protected override void OnBuffActivated(int buffId)
	{
		base.OnBuffActivated(buffId);
		if (HasStressConditionalStatsEffect(buffId))
		{
			_stressConditionalStatsDirty = true;
		}
	}

	protected override void OnBuffDeactivated(int buffId)
	{
		base.OnBuffDeactivated(buffId);
		if (HasStressConditionalStatsEffect(buffId))
		{
			_stressConditionalStatsDirty = true;
		}
	}

	private static bool HasStressConditionalStatsEffect(int buffId)
	{
		return TBuff.Table[buffId].BuffEffects.Any((BuffEffect e) => e is StressConditionalStatsEffect);
	}

	public void RefreshStressConditionalStats()
	{
		_stressConditionalStatsDirty = false;
		HashSet<EStatType> hashSet = new HashSet<EStatType>(_stressConditionalStats.Keys);
		_stressConditionalStats.Clear();
		foreach (var (key, num) in GetBuffEffects<StressConditionalStatsEffect>().Where((BuffEffectInfo<StressConditionalStatsEffect> effect) => effect?.BuffEffect?.Entries != null).SelectMany((BuffEffectInfo<StressConditionalStatsEffect> effect) => effect.BuffEffect.Entries.Where((StressStatEntry entry) => entry.StressState == StressState).Select((StressStatEntry entry) => (entry.StatType, (float)effect.Stack * entry.StatValue))))
		{
			_stressConditionalStats.TryGetValue(key, out var value);
			_stressConditionalStats[key] = num + value;
			hashSet.Add(key);
		}
		foreach (EStatType item in hashSet)
		{
			UpdateStat(item);
		}
	}



}
