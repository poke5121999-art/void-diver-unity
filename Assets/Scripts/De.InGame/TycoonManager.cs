using System.Collections.Generic;
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
using UniRx;
using VContainer;

public class TycoonManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CCancelSalesSlotAsync_003Ed__29 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public TycoonManager _003C_003E4__this;

		public int slotIndex;

		private SalesSlot _003CsalesSlot_003E5__2;

		private UniTask<ResTycoonCancel>.Awaiter _003C_003Eu__1;

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
	private struct _003CFinishWithFailAsync_003Ed__68 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<TycoonFinishResult> _003C_003Et__builder;

		public SalesSlot salesSlot;

		public Employee employee;

		public TycoonManager _003C_003E4__this;

		private int _003CemployeeId_003E5__2;

		private int _003Cstress_003E5__3;

		private int _003Clevel_003E5__4;

		private UniTask<TycoonFinishResult>.Awaiter _003C_003Eu__1;

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
	private struct _003CFinishWithSuccessAsync_003Ed__57 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<TycoonFinishResult> _003C_003Et__builder;

		public SalesSlot salesSlot;

		public Employee employee;

		public TycoonManager _003C_003E4__this;

		private Goods _003Cartifact_003E5__2;

		private int _003CprevLevel_003E5__3;

		private int _003CnewLevel_003E5__4;

		private int[] _003CallSkillIds_003E5__5;

		private int[] _003CnewSkillIds_003E5__6;

		private int _003CpriceBonusPercent_003E5__7;

		private int _003CskillOnlyPercent_003E5__8;

		private long _003CgainExp_003E5__9;

		private UniTask<TycoonFinishResult>.Awaiter _003C_003Eu__1;

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
	private struct _003CProcessFinishedSlotAsync_003Ed__54 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<TycoonFinishResult> _003C_003Et__builder;

		public TycoonManager _003C_003E4__this;

		public int slotIndex;

		private BlockScope _003CblockScope_003E5__2;

		private UniTask<TycoonFinishResult>.Awaiter _003C_003Eu__1;

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
	private struct _003CProcessFinishedSlotAsync_003Ed__56 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<TycoonFinishResult> _003C_003Et__builder;

		public Employee employee;

		public TycoonManager _003C_003E4__this;

		public SalesSlot salesSlot;

		private UniTask<TycoonFinishResult>.Awaiter _003C_003Eu__1;

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
	private struct _003CProcessFinishedSlotsAsync_003Ed__55 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TycoonManager _003C_003E4__this;

		private List<int> _003CslotIds_003E5__2;

		private UniTask<TycoonFinishResult[]>.Awaiter _003C_003Eu__1;

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
	private struct _003CPurifyEmployeeAsync_003Ed__31 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public TycoonManager _003C_003E4__this;

		public int employeeId;

		private Employee _003Cemployee_003E5__2;

		private UniTask<ResTycoonPurify>.Awaiter _003C_003Eu__1;

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
	private struct _003CRegisterArtifactAsync_003Ed__28 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public SalesSlot slot;

		public EGoodsSlotCategory category;

		public TycoonManager _003C_003E4__this;

		public int slotIndex;

		private UniTask<ResTycoonRegisterArtifact>.Awaiter _003C_003Eu__1;

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
	private struct _003CSendTycoonFinishAsync_003Ed__70 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<TycoonFinishResult> _003C_003Et__builder;

		public bool isSuccess;

		public SalesSlot salesSlot;

		public TycoonManager _003C_003E4__this;

		public int slotIndex;

		public Goods artifact;

		public int priceBonusPercent;

		public long gainExp;

		private UniTask<ResTycoonFinish>.Awaiter _003C_003Eu__1;

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
	private struct _003CTryHireEmployee_003Ed__51 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public TycoonManager _003C_003E4__this;

		public int employeeId;

		private IDictionary<int, Employee> _003Crecruitable_003E5__2;

		private Employee _003Cemployee_003E5__3;

		private UniTask<ResTycoonHire>.Awaiter _003C_003Eu__1;

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

	public readonly Subject<Unit> OnEmployeesChanged;

	public readonly Subject<Unit> OnSalesSlotsChanged;

	public readonly Subject<Unit> OnSalesFinished;

	private static readonly string[] PanicAnimations;

	private const int FiredRehireCooldownDives = 3;

	private IDictionary<int, Employee> Employees => null;

	private IDictionary<int, SalesSlot> SalesSlots => null;

	private BitFlags TycoonUnlockFlags => null;

	public IDictionary<int, Employee> RecruitableEmployees => null;

	private IDictionary<int, int> FiredEmployeeCooldowns => null;

	private void NotifyEmployeesChanged()
	{
	}

	private void NotifySlotChanged(int slotId)
	{
	}

	private void NotifySalesFinished()
	{
	}

	public string GetEmployeeAnimationName(int employeeId)
	{
		return null;
	}

	private void SendEmployeeAnimation(int employeeId, string animationName, bool isLoop)
	{
	}

	protected override void Awake()
	{
	}

	public void CheckAndUnlockSalesSlots()
	{
	}

	[AsyncStateMachine(typeof(_003CRegisterArtifactAsync_003Ed__28))]
	public UniTask<bool> RegisterArtifactAsync(SalesSlot slot, EGoodsSlotCategory category, int slotIndex)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CCancelSalesSlotAsync_003Ed__29))]
	public UniTask<bool> CancelSalesSlotAsync(int slotIndex)
	{
		return default;
	}

	public long CalculatePurifyCost(int level)
	{
		return 0L;
	}

	[AsyncStateMachine(typeof(_003CPurifyEmployeeAsync_003Ed__31))]
	public UniTask<bool> PurifyEmployeeAsync(int employeeId)
	{
		return default;
	}

	public bool TrySetEmployee(SalesSlot slot, Employee employee)
	{
		return false;
	}

	public bool TrySetArtifact(SalesSlot slot, Goods artifact)
	{
		return false;
	}

	public bool StartSalesSlot(SalesSlot slot)
	{
		return false;
	}

	public bool TryRegisterSalesSlot(SalesSlot slot, Employee employee, Goods artifact)
	{
		return false;
	}

	public void UnregisterSalesSlot(SalesSlot slot)
	{
	}

	public void ClearEmployee(SalesSlot slot)
	{
	}

	public bool SwapSalesSlotEmployee(SalesSlot slot, Employee newEmployee, Goods artifact)
	{
		return false;
	}

	public float CalculatePreviewMaxWorkProgress(Goods artifact)
	{
		return 0f;
	}

	private float CalculateMaxWorkProgress(Goods artifact)
	{
		return 0f;
	}

	private bool IsRecentlyFired(int employeeId)
	{
		return false;
	}

	private void TickFiredEmployeeCooldowns()
	{
	}

	public void RefreshRecruitableEmployees()
	{
	}

	private int RollEmployeeLevel()
	{
		return 0;
	}

	public float CalculateWorkSpeedPerTurn(Employee employee)
	{
		return 0f;
	}

	public int CalculateStressPerTurn(Employee employee, Goods artifact)
	{
		return 0;
	}

	public int CalculateTurnsUntilPanic(Employee employee, Goods artifact)
	{
		return 0;
	}

	public int CalculateTurnsUntilComplete(SalesSlot slot, Employee employee)
	{
		return 0;
	}

	public bool TryFireEmployee(int employeeId)
	{
		return false;
	}

	public long CalculateHireCost(int employeeId, int level)
	{
		return 0L;
	}

	[AsyncStateMachine(typeof(_003CTryHireEmployee_003Ed__51))]
	public UniTask<bool> TryHireEmployee(int employeeId)
	{
		return default;
	}

	public void OnTurn()
	{
	}

	private void EvaluateResult(SalesSlot salesSlot, Employee employee)
	{
	}

	[AsyncStateMachine(typeof(_003CProcessFinishedSlotAsync_003Ed__54))]
	public UniTask<TycoonFinishResult> ProcessFinishedSlotAsync(int slotIndex)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CProcessFinishedSlotsAsync_003Ed__55))]
	public UniTask ProcessFinishedSlotsAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CProcessFinishedSlotAsync_003Ed__56))]
	private UniTask<TycoonFinishResult> ProcessFinishedSlotAsync(SalesSlot salesSlot, Employee employee)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CFinishWithSuccessAsync_003Ed__57))]
	private UniTask<TycoonFinishResult> FinishWithSuccessAsync(SalesSlot salesSlot, Employee employee)
	{
		return default;
	}

	public int GetEmployeeExpGain(Goods artifact)
	{
		return 0;
	}

	private void GiveEmployeeExp(Employee employee, Goods artifact)
	{
	}

	private void TryLearnSkill(Employee employee)
	{
	}

	private void AddSkill(Employee employee, int skillId, int currentCount)
	{
	}

	private bool IsSkillMatchArtifact(TEmployeeSkill skill, Goods artifact)
	{
		return false;
	}

	private int[] GetMatchingSkillIds(Employee employee, Goods artifact, EEmployeeSkillType skillType)
	{
		return null;
	}

	private float GetStressModifierRatio(Employee employee, Goods artifact)
	{
		return 0f;
	}

	public long CalculateExpectedSellGold(Goods artifact, Employee employee)
	{
		return 0L;
	}

	private int GetPriceBonusPercent(Employee employee, Goods artifact)
	{
		return 0;
	}

	private int GetSkillOnlyPriceBonusPercent(Employee employee, Goods artifact)
	{
		return 0;
	}

	[AsyncStateMachine(typeof(_003CFinishWithFailAsync_003Ed__68))]
	private UniTask<TycoonFinishResult> FinishWithFailAsync(SalesSlot salesSlot, Employee employee)
	{
		return default;
	}

	private long CalculateTycoonSellExp(Goods artifact, Employee employee)
	{
		return 0L;
	}

	[AsyncStateMachine(typeof(_003CSendTycoonFinishAsync_003Ed__70))]
	private UniTask<TycoonFinishResult> SendTycoonFinishAsync(int slotIndex, SalesSlot salesSlot, Goods artifact, bool isSuccess, int priceBonusPercent = 0, long gainExp = 0L)
	{
		return default;
	}

	private void ClearSalesSlot(SalesSlot salesSlot)
	{
	}

	private void RecoverRestingEmployees(HashSet<int> activeEmployeeIds)
	{
	}

	private int RollTycoonCustomerNpcId()
	{
		return 0;
	}
}
