using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using NemoLib.Popup;
using SomeNet.Utility;
using UnityEngine;
using UniRx;
using VContainer;

public class TycoonManager : MonoBase
{
	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private SoundManager _soundManager;

	public readonly Subject<Unit> OnEmployeesChanged = new Subject<Unit>();

	public readonly Subject<Unit> OnSalesSlotsChanged = new Subject<Unit>();

	public readonly Subject<Unit> OnSalesFinished = new Subject<Unit>();

	private static readonly string[] PanicAnimations = new string[3] { "panic_1", "panic_2", "panic_3" };

	private const int FiredRehireCooldownDives = 3;

	private IDictionary<int, Employee> Employees => _playerManager.Employees;

	private IDictionary<int, SalesSlot> SalesSlots => _playerManager.SalesSlots;

	private BitFlags TycoonUnlockFlags => _playerManager.TycoonUnlockFlags;

	public IDictionary<int, Employee> RecruitableEmployees => _playerManager.RecruitableEmployees;

	private IDictionary<int, int> FiredEmployeeCooldowns => _playerManager.FiredEmployeeCooldowns;

	private void NotifyEmployeesChanged()
	{
		OnEmployeesChanged.OnNext(Unit.Default);
	}

	private void NotifySlotChanged(int slotId)
	{
		OnSalesSlotsChanged.OnNext(Unit.Default);
		_objectManager.RefreshSlotNpcs(slotId);
	}

	private void NotifySalesFinished()
	{
		OnSalesFinished.OnNext(Unit.Default);
	}

	public string GetEmployeeAnimationName(int employeeId)
	{
		SalesSlot salesSlot = _playerManager.SalesSlots.Values.FirstOrDefault((SalesSlot s) => s.EmployeeId == employeeId);
		if (salesSlot == null || !salesSlot.IsStarted)
		{
			return "idle";
		}
		if (!_playerManager.Employees.TryGetValue(employeeId, out var value))
		{
			return "idle";
		}
		if (!value.IsPanic)
		{
			if (!salesSlot.IsFinished)
			{
				return "negotiation";
			}
			return "idle";
		}
		return PanicAnimations[UnityEngine.Random.Range(0, PanicAnimations.Length)];
	}

	private void SendEmployeeAnimation(int employeeId, string animationName, bool isLoop)
	{
		NpcObject npcObject = _objectManager.FindEmployeeNpc(employeeId);
		if (!(npcObject == null))
		{
			_gameHost.SendToAll(new NtfPlayNpcAnimation
			{
				NetworkObjectId = npcObject.NetworkObjectId,
				AnimationName = animationName,
				IsLoop = isLoop
			});
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void CheckAndUnlockSalesSlots()
	{
		bool flag = false;
		foreach (TTycoonSalesSlot value in TTycoonSalesSlot.Table.Values)
		{
			if (!_playerManager.TycoonUnlockFlags.IsSet(value.BitFlagIndex) && _playerManager.CheckStateConditions(value.UnlockConditions))
			{
				_playerManager.TycoonUnlockFlags.Set(value.BitFlagIndex);
				if (!_playerManager.SalesSlots.ContainsKey(value.Id))
				{
					_playerManager.SalesSlots.Add(value.Id, new SalesSlot { Id = value.Id });
				}
				flag = true;
			}
		}
		if (flag)
		{
			_playerManager.SaveAsync().Forget();
		}
	}

	public async UniTask<bool> RegisterArtifactAsync(SalesSlot slot, EGoodsSlotCategory category, int slotIndex)
	{
		if (slot == null || slot.IsStarted || slot.IsDisabledForTurn)
		{
			return false;
		}
		Goods artifact = null;
		if (category == EGoodsSlotCategory.StashSlot)
		{
			if (!_playerManager.StashSlots.TryGetValue(slotIndex, out var value))
			{
				return false;
			}
			artifact = value.Goods.Clone();
		}
		ResTycoonRegisterArtifact resTycoonRegisterArtifact = await _gameChannel.RequestAsync<ResTycoonRegisterArtifact>(new ReqTycoonRegisterArtifact
		{
			SalesSlotIndex = slot.Id,
			ArtifactSlotCategory = category,
			ArtifactSlotIndex = slotIndex,
			Artifact = artifact
		});
		if (resTycoonRegisterArtifact == null || !resTycoonRegisterArtifact.IsSuccess)
		{
			return false;
		}
		slot.Goods = resTycoonRegisterArtifact.Artifact;
		_soundManager.PlaySfx("ArtifactUI");
		NotifySlotChanged(slot.Id);
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public async UniTask<bool> CancelSalesSlotAsync(int slotIndex)
	{
		if (!_playerManager.SalesSlots.TryGetValue(slotIndex, out var salesSlot))
		{
			return false;
		}
		if (salesSlot.Goods == null)
		{
			return false;
		}
		ResTycoonCancel resTycoonCancel = await _gameChannel.RequestAsync<ResTycoonCancel>(new ReqTycoonCancel
		{
			SalesSlotIndex = slotIndex,
			Artifact = salesSlot.Goods
		});
		if (resTycoonCancel == null || !resTycoonCancel.IsSuccess)
		{
			return false;
		}
		ClearSalesSlot(salesSlot);
		_soundManager.PlaySfx("ArtifactUI");
		NotifySlotChanged(slotIndex);
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public long CalculatePurifyCost(int level)
	{
		int intValue = TConst.TycoonEmployeePurifyCostBase.IntValue;
		float floatValue = TConst.TycoonEmployeePurifyCostFactor.FloatValue;
		return (long)(Math.Pow(level, 1.5) * (double)floatValue) + intValue;
	}

	public async UniTask<bool> PurifyEmployeeAsync(int employeeId)
	{
		if (!_playerManager.Employees.TryGetValue(employeeId, out var employee))
		{
			return false;
		}
		if (employee.Stress < 1 && !employee.IsPanic)
		{
			return false;
		}
		ResTycoonPurify resTycoonPurify = await _gameChannel.RequestAsync<ResTycoonPurify>(new ReqTycoonPurify
		{
			EmployeeId = employeeId,
			Level = employee.Level
		});
		if (resTycoonPurify == null || !resTycoonPurify.IsSuccess)
		{
			return false;
		}
		employee.Stress = 0;
		employee.IsPanic = false;
		NotifyEmployeesChanged();
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public bool TrySetEmployee(SalesSlot slot, Employee employee)
	{
		if (employee.IsPanic || slot == null || slot.IsStarted || slot.IsDisabledForTurn)
		{
			return false;
		}
		if (_playerManager.SalesSlots.Values.Any((SalesSlot s) => s != slot && s.EmployeeId == employee.Id))
		{
			return false;
		}
		slot.EmployeeId = employee.Id;
		_soundManager.PlaySfx("Employee_Deployment");
		NotifySlotChanged(slot.Id);
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public bool TrySetArtifact(SalesSlot slot, Goods artifact)
	{
		if (slot == null || slot.IsStarted)
		{
			return false;
		}
		if (artifact == null || artifact.Type == EGoodsType.Artifact)
		{
			slot.Goods = artifact;
			NotifySlotChanged(slot.Id);
			return true;
		}
		return false;
	}

	public bool StartSalesSlot(SalesSlot slot)
	{
		if (slot == null)
		{
			return false;
		}
		if (slot.IsStarted)
		{
			return false;
		}
		if (slot.EmployeeId == 0)
		{
			return false;
		}
		if (slot.Goods == null)
		{
			return false;
		}
		if (!_playerManager.Employees.TryGetValue(slot.EmployeeId, out var value))
		{
			return false;
		}
		if (value.IsPanic)
		{
			return false;
		}
		slot.IsStarted = true;
		slot.MaxWorkProgress = CalculateMaxWorkProgress(slot.Goods);
		_soundManager.PlaySfx("Employee_Start");
		NotifySlotChanged(slot.Id);
		SendEmployeeAnimation(slot.EmployeeId, "negotiation", true);
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public bool TryRegisterSalesSlot(SalesSlot slot, Employee employee, Goods artifact)
	{
		if (employee.IsPanic || slot == null || slot.EmployeeId != 0 || slot.IsDisabledForTurn || artifact == null || artifact.Type != EGoodsType.Artifact)
		{
			return false;
		}
		if (_playerManager.SalesSlots.Values.Any((SalesSlot s) => s.EmployeeId == employee.Id))
		{
			return false;
		}
		slot.EmployeeId = employee.Id;
		slot.Goods = artifact;
		slot.IsStarted = true;
		slot.MaxWorkProgress = CalculateMaxWorkProgress(artifact);
		NotifySlotChanged(slot.Id);
		SendEmployeeAnimation(employee.Id, "negotiation", true);
		return true;
	}

	public void UnregisterSalesSlot(SalesSlot slot)
	{
		if (slot != null)
		{
			ClearSalesSlot(slot);
			_soundManager.PlaySfx("ArtifactUI");
			NotifySlotChanged(slot.Id);
			_playerManager.SaveAsync().Forget();
		}
	}

	public void ClearEmployee(SalesSlot slot)
	{
		if (slot != null && !slot.IsStarted)
		{
			slot.EmployeeId = 0;
			NotifySlotChanged(slot.Id);
			_playerManager.SaveAsync().Forget();
		}
	}

	public bool SwapSalesSlotEmployee(SalesSlot slot, Employee newEmployee, Goods artifact)
	{
		if (newEmployee.IsPanic || slot == null || artifact == null)
		{
			return false;
		}
		float workProgress = slot.WorkProgress;
		ClearSalesSlot(slot);
		slot.EmployeeId = newEmployee.Id;
		slot.Goods = artifact;
		slot.IsStarted = true;
		slot.MaxWorkProgress = CalculateMaxWorkProgress(artifact);
		if (0f < workProgress)
		{
			slot.WorkProgress = (1f - TConst.TycoonEmployeeSwapPenaltyRatio.FloatValue) * workProgress;
		}
		_soundManager.PlaySfx("Employee_Change");
		NotifySlotChanged(slot.Id);
		SendEmployeeAnimation(newEmployee.Id, "negotiation", true);
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public float CalculatePreviewMaxWorkProgress(Goods artifact)
	{
		return CalculateMaxWorkProgress(artifact);
	}

	private float CalculateMaxWorkProgress(Goods artifact)
	{
		TConst tConst;
		switch (artifact.GetGrade())
		{
		case EGoodsGradeType.Rare:
			tConst = TConst.TycoonDefaultMaxWorkProgressRare;
			break;
		case EGoodsGradeType.Elite:
			tConst = TConst.TycoonDefaultMaxWorkProgressElite;
			break;
		case EGoodsGradeType.Epic:
			tConst = TConst.TycoonDefaultMaxWorkProgressEpic;
			break;
		case EGoodsGradeType.Legend:
			tConst = TConst.TycoonDefaultMaxWorkProgressLegendary;
			break;
		case EGoodsGradeType.Unique:
			tConst = TConst.TycoonDefaultMaxWorkProgressUnique;
			break;
		default:
			tConst = TConst.TycoonDefaultMaxWorkProgressNormal;
			break;
		}
		int intValue = tConst.IntValue;
		float num = artifact.GetPrefixes().Sum((int _) => TArtifactPrefix.Table[_].MaxWorkProgressWeight);
		float floatValue = TConst.TycoonMaxWorkProgressFactor.FloatValue;
		float floatValue2 = TConst.TycoonMaxWorkProgressMinRatio.FloatValue;
		float floatValue3 = TConst.TycoonMaxWorkProgressMaxRatio.FloatValue;
		return Math.Clamp((float)intValue + num * floatValue, (float)intValue * floatValue2, (float)intValue * floatValue3);
	}

	private bool IsRecentlyFired(int employeeId)
	{
		return _playerManager.FiredEmployeeCooldowns.ContainsKey(employeeId);
	}

	private void TickFiredEmployeeCooldowns()
	{
		List<int> list = _playerManager.FiredEmployeeCooldowns.Where((KeyValuePair<int, int> kv) => kv.Value < 2).Select((KeyValuePair<int, int> kv) => kv.Key).ToList();
		foreach (int item in list)
		{
			_playerManager.FiredEmployeeCooldowns.Remove(item);
		}
		foreach (int item2 in _playerManager.FiredEmployeeCooldowns.Keys.ToList())
		{
			_playerManager.FiredEmployeeCooldowns[item2] = _playerManager.FiredEmployeeCooldowns[item2] - 1;
		}
	}

	public void RefreshRecruitableEmployees()
	{
		_playerManager.RecruitableEmployees.Clear();
		HashSet<int> hiredIds = new HashSet<int>(_playerManager.Employees.Keys);
		List<int> list = TEmployee.Table.Keys.Where((int id) => !hiredIds.Contains(id)).Where((int id) => !IsRecentlyFired(id)).ToList();
		int num = Math.Min(3, list.Count);
		for (int i = 0; i < num; i++)
		{
			int index = UnityEngine.Random.Range(i, list.Count);
			int value = list[index];
			int value2 = list[i];
			list[i] = value;
			list[index] = value2;
			int num2 = list[i];
			Employee value3 = Employee.Create(num2, RollEmployeeLevel(), withRandomSkills: true);
			_playerManager.RecruitableEmployees.Add(num2, value3);
		}
	}

	private int RollEmployeeLevel()
	{
		int level = _playerManager.Level;
		int num = TEmployeeLevel.Table.Keys.Max();
		int num2 = UnityEngine.Random.Range(0, 100);
		int value;
		int value2;
		if (num2 < 10)
		{
			value = level - 1;
			value2 = level;
		}
		else
		{
			value = level / 2;
			if (num2 < 40)
			{
				value2 = level - 2;
			}
			else
			{
				value2 = value - 1;
				value = 1;
			}
		}
		int num3 = Mathf.Clamp(value, 1, num);
		int num4 = Mathf.Clamp(value2, 1, num);
		if (num4 < num3)
		{
			return 1;
		}
		return RandomUtility.GetRandomInt(num3, num4 + 1);
	}

	public float CalculateWorkSpeedPerTurn(Employee employee)
	{
		float floatValue = TConst.TycoonWorkProgressPerTurn.FloatValue;
		int charm = employee.Charm;
		float floatValue2 = TConst.TycoonWorkProgressCharmFactor.FloatValue;
		int persuasion = employee.Persuasion;
		return (float)charm * floatValue2 + floatValue + (float)persuasion * TConst.TycoonWorkProgressPersuasionFactor.FloatValue;
	}

	public int CalculateStressPerTurn(Employee employee, Goods artifact)
	{
		int corruption = TEquipment.Table[artifact.Id].Corruption;
		int num = corruption - employee.Resistance;
		if (num < 1)
		{
			return 0;
		}
		float stressModifierRatio = GetStressModifierRatio(employee, artifact);
		return Math.Max(0, (int)((float)num * Math.Max(0f, 1f - stressModifierRatio)));
	}

	public int CalculateTurnsUntilPanic(Employee employee, Goods artifact)
	{
		int num = CalculateStressPerTurn(employee, artifact);
		if (num < 1)
		{
			return int.MaxValue;
		}
		int num2 = TConst.StressMax.IntValue - employee.Stress;
		if (num2 < 1)
		{
			return 0;
		}
		return (int)Math.Ceiling((float)num2 / (float)num);
	}

	public int CalculateTurnsUntilComplete(SalesSlot slot, Employee employee)
	{
		float num = CalculateWorkSpeedPerTurn(employee);
		if (num <= 0f)
		{
			return int.MaxValue;
		}
		float num2 = slot.MaxWorkProgress;
		if (num2 <= 0f)
		{
			num2 = CalculateMaxWorkProgress(slot.Goods);
		}
		float num3 = num2 - slot.WorkProgress;
		if (num3 <= 0f)
		{
			return 0;
		}
		return (int)Math.Ceiling(num3 / num);
	}

	public bool TryFireEmployee(int employeeId)
	{
		if (!_playerManager.Employees.ContainsKey(employeeId))
		{
			return false;
		}
		SalesSlot salesSlot = _playerManager.SalesSlots.Values.FirstOrDefault((SalesSlot s) => s.EmployeeId == employeeId);
		if (salesSlot != null)
		{
			ClearSalesSlot(salesSlot);
		}
		_playerManager.Employees.Remove(employeeId);
		_playerManager.FiredEmployeeCooldowns[employeeId] = 3;
		NotifyEmployeesChanged();
		if (salesSlot != null)
		{
			NotifySlotChanged(salesSlot.Id);
		}
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public long CalculateHireCost(int employeeId, int level)
	{
		double num = Math.Pow(Math.Max(0, level - 1), 2.0);
		float floatValue = TConst.TycoonHireCostPerLevelFactor.FloatValue;
		return (long)((double)floatValue * num) + TEmployee.Table[employeeId].HireCost;
	}

	public async UniTask<bool> TryHireEmployee(int employeeId)
	{
		if (_playerManager.Employees.Count >= TConst.MaxEmployeeCountBase.IntValue)
		{
			return false;
		}
		IDictionary<int, Employee> recruitable = _playerManager.RecruitableEmployees;
		if (!recruitable.TryGetValue(employeeId, out var employee))
		{
			return false;
		}
		ResTycoonHire resTycoonHire = await _gameChannel.RequestAsync<ResTycoonHire>(new ReqTycoonHire
		{
			EmployeeId = employeeId,
			Level = employee.Level
		});
		if (resTycoonHire == null || !resTycoonHire.IsSuccess)
		{
			return false;
		}
		_playerManager.Employees.Add(employeeId, employee);
		recruitable.Remove(employeeId);
		NotifyEmployeesChanged();
		_playerManager.SaveAsync().Forget();
		return true;
	}

	public void OnTurn()
	{
		TickFiredEmployeeCooldowns();
		HashSet<int> hashSet = new HashSet<int>(from s in _playerManager.SalesSlots.Values
			where s.EmployeeId != 0 && s.IsStarted
			select s.EmployeeId);
		foreach (SalesSlot value in _playerManager.SalesSlots.Values)
		{
			if (!value.IsFinished && !value.IsDisabledForTurn)
			{
				if (value.IsStarted && _playerManager.Employees.TryGetValue(value.EmployeeId, out var value2))
				{
					value.WorkProgress += CalculateWorkSpeedPerTurn(value2);
					value2.Stress = Math.Min(value2.Stress + CalculateStressPerTurn(value2, value.Goods), TConst.StressMax.IntValue);
					if (value.TycoonCustomerNpcId == 0)
					{
						value.TycoonCustomerNpcId = RollTycoonCustomerNpcId();
					}
					UnityEngine.Debug.Log("Slot Info - EmployeeId: " + value.EmployeeId + ", ArtifactId: " + value.Goods.Id + ", WorkProgress: " + value.WorkProgress + "stress : " + _playerManager.Employees[value.EmployeeId].Stress);
					EvaluateResult(value, value2);
					_objectManager.RefreshSlotNpcs(value.Id);
				}
			}
			else
			{
				value.IsDisabledForTurn = false;
			}
		}
		RecoverRestingEmployees(hashSet);
		NotifyEmployeesChanged();
		OnSalesSlotsChanged.OnNext(Unit.Default);
		_playerManager.SaveAsync().Forget();
	}

	private void EvaluateResult(SalesSlot salesSlot, Employee employee)
	{
		if (employee.Stress < TConst.StressMax.IntValue)
		{
			if (!(salesSlot.WorkProgress < salesSlot.MaxWorkProgress))
			{
				salesSlot.IsFinished = true;
				_soundManager.PlaySfx("Casher_Coin_Online");
				SendEmployeeAnimation(employee.Id, "idle", true);
			}
		}
		else
		{
			employee.IsPanic = true;
			salesSlot.IsFinished = true;
			string animationName = PanicAnimations[UnityEngine.Random.Range(0, PanicAnimations.Length)];
			SendEmployeeAnimation(employee.Id, animationName, true);
		}
	}

	public async UniTask<TycoonFinishResult> ProcessFinishedSlotAsync(int slotIndex)
	{
		using (new BlockScope(true, false))
		{
			if (!_playerManager.SalesSlots.TryGetValue(slotIndex, out var salesSlot))
			{
				return null;
			}
			if (!salesSlot.IsFinished)
			{
				return null;
			}
			if (!_playerManager.Employees.TryGetValue(salesSlot.EmployeeId, out var employee))
			{
				return null;
			}
			TycoonFinishResult result = await ProcessFinishedSlotAsync(salesSlot, employee);
			NotifyEmployeesChanged();
			NotifySlotChanged(slotIndex);
			NotifySalesFinished();
			_playerManager.SaveAsync().Forget();
			return result;
		}
	}

	public async UniTask ProcessFinishedSlotsAsync()
	{
		List<UniTask<TycoonFinishResult>> tasks = new List<UniTask<TycoonFinishResult>>();
		List<int> slotIds = new List<int>();
		foreach (SalesSlot value in _playerManager.SalesSlots.Values)
		{
			if (value.IsFinished && _playerManager.Employees.TryGetValue(value.EmployeeId, out var value2))
			{
				slotIds.Add(value.Id);
				tasks.Add(ProcessFinishedSlotAsync(value, value2));
			}
		}
		if (tasks.Count == 0)
		{
			return;
		}
		await UniTask.WhenAll(tasks);
		foreach (int item in slotIds)
		{
			_objectManager.RefreshSlotNpcs(item);
		}
		NotifyEmployeesChanged();
		OnSalesSlotsChanged.OnNext(Unit.Default);
		NotifySalesFinished();
		_playerManager.SaveAsync().Forget();
	}

	private async UniTask<TycoonFinishResult> ProcessFinishedSlotAsync(SalesSlot salesSlot, Employee employee)
	{
		if (!employee.IsPanic)
		{
			return await FinishWithSuccessAsync(salesSlot, employee);
		}
		return await FinishWithFailAsync(salesSlot, employee);
	}

	private async UniTask<TycoonFinishResult> FinishWithSuccessAsync(SalesSlot salesSlot, Employee employee)
	{
		int slotIndex = salesSlot.Id;
		Goods artifact = salesSlot.Goods;
		int prevLevel = employee.Level;
		int[] prevSkillIds = ((employee.LearnedSkillIds != null) ? employee.LearnedSkillIds.ToArray() : null) ?? Array.Empty<int>();
		GiveEmployeeExp(employee, artifact);
		int newLevel = employee.Level;
		int[] allSkillIds = ((employee.LearnedSkillIds != null) ? employee.LearnedSkillIds.ToArray() : null) ?? Array.Empty<int>();
		int[] newSkillIds = allSkillIds.Where((int id) => !prevSkillIds.Contains(id)).ToArray();
		int priceBonusPercent = GetPriceBonusPercent(employee, artifact);
		int skillOnlyPercent = GetSkillOnlyPriceBonusPercent(employee, artifact);
		long gainExp = CalculateTycoonSellExp(artifact, employee);
		ClearSalesSlot(salesSlot);
		TycoonFinishResult tycoonFinishResult = await SendTycoonFinishAsync(slotIndex, salesSlot, artifact, true, priceBonusPercent, gainExp);
		tycoonFinishResult.EmployeeId = employee.Id;
		tycoonFinishResult.Stress = employee.Stress;
		tycoonFinishResult.ResultExp = gainExp;
		tycoonFinishResult.PrevLevel = prevLevel;
		tycoonFinishResult.NewLevel = newLevel;
		tycoonFinishResult.EmployeeLevel = newLevel;
		tycoonFinishResult.EmployeeExp = employee.Exp;
		tycoonFinishResult.LearnedSkillIds = allSkillIds;
		tycoonFinishResult.NewlyLearnedSkillIds = newSkillIds;
		tycoonFinishResult.EmployeeExpGain = ((employee.Level < TEmployeeLevel.Table.Keys.Max()) ? GetEmployeeExpGain(artifact) : 0);
		tycoonFinishResult.StressSkillIds = GetMatchingSkillIds(employee, artifact, EEmployeeSkillType.StressModifier);
		tycoonFinishResult.PriceSkillIds = GetMatchingSkillIds(employee, artifact, EEmployeeSkillType.PriceModifier);
		tycoonFinishResult.SkillPriceBonusPercent = skillOnlyPercent;
		tycoonFinishResult.SkillCoinBonus = ((priceBonusPercent > 0) ? ((long)skillOnlyPercent * tycoonFinishResult.ResultCoin / (priceBonusPercent + 100)) : 0);
		return tycoonFinishResult;
	}

	public int GetEmployeeExpGain(Goods artifact)
	{
		TConst tConst;
		switch (artifact.GetGrade())
		{
		case EGoodsGradeType.Rare:
			tConst = TConst.TycoonEmployeeExpGainRare;
			break;
		case EGoodsGradeType.Elite:
			tConst = TConst.TycoonEmployeeExpGainElite;
			break;
		case EGoodsGradeType.Epic:
			tConst = TConst.TycoonEmployeeExpGainEpic;
			break;
		case EGoodsGradeType.Legend:
			tConst = TConst.TycoonEmployeeExpGainLegendary;
			break;
		case EGoodsGradeType.Unique:
			tConst = TConst.TycoonEmployeeExpGainUnique;
			break;
		default:
			tConst = TConst.TycoonEmployeeExpGainNormal;
			break;
		}
		return tConst.IntValue;
	}

	private void GiveEmployeeExp(Employee employee, Goods artifact)
	{
		int num = TEmployeeLevel.Table.Keys.Max();
		if (employee.Level >= num)
		{
			return;
		}
		employee.Exp += GetEmployeeExpGain(artifact);
		while (employee.Level < num)
		{
			TEmployeeLevel tEmployeeLevel = TEmployeeLevel.Table[employee.Level + 1];
			if (employee.Exp < tEmployeeLevel.NeedExp)
			{
				break;
			}
			employee.Exp -= tEmployeeLevel.NeedExp;
			employee.Level++;
			TryLearnSkill(employee);
		}
	}

	private void TryLearnSkill(Employee employee)
	{
		int num = ((employee.LearnedSkillIds != null) ? employee.LearnedSkillIds.Length : 0);
		if (num > 1)
		{
			return;
		}
		TEmployee employeeData = TEmployee.Table[employee.Id];
		if (employeeData.SkillIds == null || employeeData.SkillIds.Length == 0)
		{
			return;
		}
		if (employeeData.ForcedSkillLearnLevels != null)
		{
			for (int i = 0; i < employeeData.SkillIds.Length; i++)
			{
				if (i < employeeData.ForcedSkillLearnLevels.Length && employeeData.ForcedSkillLearnLevels[i] > 0 && employeeData.ForcedSkillLearnLevels[i] <= employee.Level)
				{
					int num2 = employeeData.SkillIds[i];
					if (employee.LearnedSkillIds == null || !employee.LearnedSkillIds.Contains(num2))
					{
						AddSkill(employee, num2, num);
						return;
					}
				}
			}
		}
		int num3 = employeeData.SkillIds.Select((int id, int index) => new
		{
			SkillId = id,
			ForcedLevel = ((employeeData.ForcedSkillLearnLevels != null && index < employeeData.ForcedSkillLearnLevels.Length) ? employeeData.ForcedSkillLearnLevels[index] : int.MaxValue)
		}).Where(x => employee.LearnedSkillIds == null || !employee.LearnedSkillIds.Contains(x.SkillId)).OrderBy(x => x.ForcedLevel).Select(x => x.SkillId).FirstOrDefault();
		if (num3 != 0)
		{
			float floatValue = TConst.TycoonEmployeeSkillLearnPercent.FloatValue;
			float floatValue2 = TConst.TycoonEmployeeSkillLearnPercentPerLevel.FloatValue;
			if (RandomUtility.GetRandomBoolFromPercentage((float)(employee.Level - 1) * floatValue2 + floatValue))
			{
				AddSkill(employee, num3, num);
			}
		}
	}

	private void AddSkill(Employee employee, int skillId, int currentCount)
	{
		int[] array = new int[currentCount + 1];
		employee.LearnedSkillIds.CopyTo(array, 0);
		array[currentCount] = skillId;
		employee.LearnedSkillIds = array;
	}

	private bool IsSkillMatchArtifact(TEmployeeSkill skill, Goods artifact)
	{
		TEquipment tEquipment = TEquipment.Table[artifact.Id];
		if (skill.TargetGrade != null && skill.TargetGrade.Length != 0 && !skill.TargetGrade.Contains(tEquipment.Grade))
		{
			return false;
		}
		if (skill.TargetCategory != null && skill.TargetCategory.Length != 0 && !skill.TargetCategory.Contains(tEquipment.ArtifactCategory))
		{
			return false;
		}
		if (skill.TargetPrefix == null || skill.TargetPrefix.Length == 0)
		{
			return true;
		}
		int[] prefixes = artifact.GetPrefixes();
		if (prefixes != null && prefixes.Length != 0)
		{
			return prefixes.Any((int _) => skill.TargetPrefix.Contains(_));
		}
		return false;
	}

	private int[] GetMatchingSkillIds(Employee employee, Goods artifact, EEmployeeSkillType skillType)
	{
		if (employee.LearnedSkillIds == null || employee.LearnedSkillIds.Length == 0)
		{
			return Array.Empty<int>();
		}
		return employee.LearnedSkillIds.Where(delegate(int id)
		{
			TEmployeeSkill tEmployeeSkill = TEmployeeSkill.Table[id];
			if (tEmployeeSkill.SkillType != skillType)
			{
				return false;
			}
			return IsSkillMatchArtifact(tEmployeeSkill, artifact);
		}).ToArray();
	}

	private float GetStressModifierRatio(Employee employee, Goods artifact)
	{
		return GetMatchingSkillIds(employee, artifact, EEmployeeSkillType.StressModifier).Sum((int id) => TEmployeeSkill.Table[id].Value);
	}

	public long CalculateExpectedSellGold(Goods artifact, Employee employee)
	{
		if (artifact == null || employee == null || employee.IsPanic)
		{
			return 0L;
		}
		int priceBonusPercent = GetPriceBonusPercent(employee, artifact);
		int sellPriceTotal = artifact.SellPriceTotal;
		return (long)((float)sellPriceTotal * ((float)priceBonusPercent / 100f + 1f));
	}

	private int GetPriceBonusPercent(Employee employee, Goods artifact)
	{
		int persuasion = employee.Persuasion;
		float floatValue = TConst.TycoonSalePricePersuasionFactor.FloatValue;
		float num = GetMatchingSkillIds(employee, artifact, EEmployeeSkillType.PriceModifier).Sum((int id) => TEmployeeSkill.Table[id].Value * 100f);
		return (int)(num + (float)persuasion * floatValue);
	}

	private int GetSkillOnlyPriceBonusPercent(Employee employee, Goods artifact)
	{
		return (int)GetMatchingSkillIds(employee, artifact, EEmployeeSkillType.PriceModifier).Sum((int id) => TEmployeeSkill.Table[id].Value * 100f);
	}

	private async UniTask<TycoonFinishResult> FinishWithFailAsync(SalesSlot salesSlot, Employee employee)
	{
		int slotIndex = salesSlot.Id;
		Goods artifact = salesSlot.Goods;
		int employeeId = employee.Id;
		int stress = employee.Stress;
		int level = employee.Level;
		ClearSalesSlot(salesSlot);
		TycoonFinishResult tycoonFinishResult = await SendTycoonFinishAsync(slotIndex, salesSlot, artifact, false);
		tycoonFinishResult.IsPanic = true;
		tycoonFinishResult.EmployeeId = employeeId;
		tycoonFinishResult.Stress = stress;
		tycoonFinishResult.EmployeeLevel = level;
		tycoonFinishResult.EmployeeExp = employee.Exp;
		return tycoonFinishResult;
	}

	private long CalculateTycoonSellExp(Goods artifact, Employee employee)
	{
		EGoodsGradeType grade = TEquipment.Table[artifact.Id].Grade;
		float artifactDealExpMultiplier = grade.GetArtifactDealExpMultiplier();
		if (0f < artifactDealExpMultiplier && employee != null && !employee.IsPanic)
		{
			int priceBonusPercent = GetPriceBonusPercent(employee, artifact);
			int sellPriceTotal = artifact.SellPriceTotal;
			long num = (long)((float)sellPriceTotal * ((float)priceBonusPercent / 100f + 1f));
			if (0 < num)
			{
				return (long)((float)num * artifactDealExpMultiplier);
			}
		}
		return 0L;
	}

	private async UniTask<TycoonFinishResult> SendTycoonFinishAsync(int slotIndex, SalesSlot salesSlot, Goods artifact, bool isSuccess, int priceBonusPercent = 0, long gainExp = 0L)
	{
		if (!isSuccess)
		{
			salesSlot.IsDisabledForTurn = true;
		}
		ResTycoonFinish resTycoonFinish = await _gameChannel.RequestAsync<ResTycoonFinish>(new ReqTycoonFinish
		{
			SalesSlotIndex = slotIndex,
			Artifact = artifact,
			IsSuccess = isSuccess,
			PriceBonusPercent = priceBonusPercent,
			GainExp = gainExp
		});
		if (resTycoonFinish == null || !resTycoonFinish.IsSuccess)
		{
			salesSlot.IsDisabledForTurn = false;
			return null;
		}
		if (resTycoonFinish.ReturnArtifact != null)
		{
			_playerManager.AddGoodsToStash(resTycoonFinish.ReturnArtifact);
		}
		salesSlot.IsDisabledForTurn = resTycoonFinish.FailType == ETycoonFailType.SlotDamage;
		return new TycoonFinishResult
		{
			IsSold = ((isSuccess || resTycoonFinish.FailType == ETycoonFailType.ArtifactDamage) ? true : (resTycoonFinish.FailType == ETycoonFailType.CheapSale)),
			IsPanic = false,
			Artifact = artifact,
			ResultCoin = resTycoonFinish.GainCoin,
			FailType = resTycoonFinish.FailType
		};
	}

	private void ClearSalesSlot(SalesSlot salesSlot)
	{
		salesSlot.EmployeeId = 0;
		salesSlot.Goods = null;
		salesSlot.WorkProgress = 0f;
		salesSlot.MaxWorkProgress = 0f;
		salesSlot.IsFinished = false;
		salesSlot.TycoonCustomerNpcId = 0;
		salesSlot.IsStarted = false;
	}

	private void RecoverRestingEmployees(HashSet<int> activeEmployeeIds)
	{
		int intValue = TConst.TycoonEmployeeStressRecoveryPerTurn.IntValue;
		foreach (Employee value in _playerManager.Employees.Values)
		{
			if (!activeEmployeeIds.Contains(value.Id))
			{
				value.Stress = Math.Max(0, value.Stress - intValue);
				if (value.Stress == 0)
				{
					value.IsPanic = false;
				}
			}
		}
	}

	private int RollTycoonCustomerNpcId()
	{
		List<int> list = (from _ in TNpcFunction.Table.Values
			where _.Type == ENpcFunctionType.TycoonCustomer
			select _.NpcId).Distinct().ToList();
		if (list.Count == 0)
		{
			return 0;
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}
}
