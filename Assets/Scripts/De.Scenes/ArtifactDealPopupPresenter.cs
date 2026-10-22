using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using GameCommon.Data;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArtifactDealPopupPresenter : GamePopupPresenter
{
	private enum EItemPanelType
	{
		Inventory = 0,
		Stash = 1
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass77_0
	{
		public CancellationToken ct;

		public float slotAddDuration;

		internal UniTask _003CAnimateRequirementSlotsAsync_003Eb__4(ArtifactDealSlotPresenter slot)
		{
			return default;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass78_0
	{
		public GameObject[] slots;

		public CancellationToken ct;

		internal ArtifactDealSlotPresenter _003CAnimateSlotTransitionsAsync_003Eb__1(int i)
		{
			return null;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass78_1
	{
		public float cancelHold;

		public _003C_003Ec__DisplayClass78_0 CS_0024_003C_003E8__locals1;

		internal UniTask _003CAnimateSlotTransitionsAsync_003Eb__0(int i)
		{
			return default;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass78_2
	{
		public float slotAddDuration;

		public _003C_003Ec__DisplayClass78_0 CS_0024_003C_003E8__locals2;

		internal UniTask _003CAnimateSlotTransitionsAsync_003Eb__6(ArtifactDealSlotPresenter _)
		{
			return default;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CAnimateDealPriceAsync_003Ed__74 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public bool showChangeFloating;

		public PriceChange change;

		public CancellationToken ct;

		private int _003CoriginalPrice_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CAnimateRemovedSlotsAsync_003Ed__79 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public IReadOnlyList<int> removedPrefer;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public float duration;

		public CancellationToken ct;

		public IReadOnlyList<int> removedDislike;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003CAnimateRequirementSlotsAsync_003Ed__77 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public CancellationToken ct;

		public TagDiff diff;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass77_0 _003C_003E8__1;

		private UniTask.Awaiter _003C_003Eu__1;

		private UniTask<bool>.Awaiter _003C_003Eu__2;

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
	private struct _003CAnimateSlotTransitionsAsync_003Ed__78 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public GameObject[] slots;

		public CancellationToken ct;

		public int[] before;

		public int[] after;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass78_1 _003C_003E8__1;

		private _003C_003Ec__DisplayClass78_0 _003C_003E8__2;

		public bool isPrefer;

		private _003C_003Ec__DisplayClass78_2 _003C_003E8__3;

		private List<int> _003CaddedIndices_003E5__2;

		private UniTask.Awaiter _003C_003Eu__1;

		private UniTask<bool>.Awaiter _003C_003Eu__2;

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
	private struct _003CAnimateTagDiffAsync_003Ed__70 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public int[] addedPrefer;

		public int[] removedPrefer;

		public int[] addedDislike;

		public int[] removedDislike;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003CConsumePendingEventsAsync_003Ed__58 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public EArtifactDealState finishState;

		private CancellationToken _003Cct_003E5__2;

		private IArtifactDealEvent[] _003C_003E7__wrap2;

		private int _003C_003E7__wrap3;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CFinishSequenceAfterAsync_003Ed__88 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public float seconds;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public EArtifactDealState sequence;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CHideDealSuggestAfterAsync_003Ed__66 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public GameObject suggest;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CHideTextBubbleAfterAsync_003Ed__91 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public float seconds;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public int generation;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnCardConsumedAsync_003Ed__67 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnCardResolvedAsync_003Ed__65 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public ArtifactDealCardResolvedEvent e;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnClickAcceptDealAsync_003Ed__122 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnClickGiveUpAsync_003Ed__124 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnDealStartedAsync_003Ed__56 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		public ArtifactDealStartedEvent e;

		private Animator _003ChandsAnimator_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003COnGoodsSwappedAsync_003Ed__57 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public ArtifactDealGoodsSwappedEvent e;

		public CancellationToken ct;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003COnHandRerolledAsync_003Ed__71 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private Animator _003ChandsAnimator_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnNpcHesitatingAsync_003Ed__64 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public ArtifactDealNpcHesitatingEvent e;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COnOpenAsync_003Ed__45 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003COnSlotClick_003Ed__117 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public InventoryGoodsSlotModel slotModel;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003COpenBackOutPopupAfterCloseAsync_003Ed__128 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003COpenCompletePopupAfterCloseAsync_003Ed__131 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public int gainExp;

		public int initialCoin;

		public int finalCoin;

		public int balancePercent;

		public Goods dealGoods;

		private UniTask.Awaiter _003C_003Eu__1;

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
	private struct _003CPlayCardCompleteAfterDelayAsync_003Ed__68 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public ArtifactDealCardPlayPresenter playSlot;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CPlayConcludingAsync_003Ed__129 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private int _003CdealCoin_003E5__2;

		private int _003CgainExp_003E5__3;

		private int _003CinitialCoin_003E5__4;

		private int _003CbalancePercent_003E5__5;

		private Goods _003CdisplayGoods_003E5__6;

		private UniTask<ResDealArtifact>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

		private UniTask<bool>.Awaiter _003C_003Eu__3;

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
	private struct _003CPlayEventAsync_003Ed__59 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public IArtifactDealEvent dealEvent;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003CPlayGivingUpAsync_003Ed__126 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

		private UniTask.Awaiter _003C_003Eu__2;

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
	private struct _003CPlayNoMorePlayCountAsync_003Ed__73 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private CancellationToken _003Cct_003E5__2;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CSetNpcSkeletonAsync_003Ed__85 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		public CancellationToken ct;

		private TArtifactDeal _003CdealData_003E5__2;

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
	private struct _003CWaitSequenceStepAsync_003Ed__84 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<bool> _003C_003Et__builder;

		public float seconds;

		public CancellationToken ct;

		private UniTask<bool>.Awaiter _003C_003Eu__1;

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
	private struct _003CWaitUntilSelfRemovedAsync_003Ed__130 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealPopupPresenter _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

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

	[SerializeField]
	private ArtifactDealSequenceTimeConfig _sequenceTimeConfig;

	[View]
	private ArtifactDealPopupView _view;

	[InjectModel]
	private ArtifactDealPopupModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private GameUiEventSystem _uiEventSystem;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameEventSystem _eventSystem;

	private CancellationTokenSource _skeletonCts;

	private Tweener _remainCountTweener;

	private Tweener _dealCoinTweener;

	private Tweener _dealExpTweener;

	private PlayerInventoryPanelModel _inventoryPanelModel;

	private StashPanelModel _stashPanelModel;

	private InventoryGoodsSlotModel _dealingSlotModel;

	private GoodsTooltipModel _dealingTooltipModel;

	private EItemPanelType _itemPanelType;

	private readonly string[] _guestSaySfxKeys;

	private int _lastPlayedCardIndex;

	private CompositeDisposable _cardDisposables;

	private readonly Dictionary<int, ArtifactDealCardPlayPresenter> _cardPlaySlotsByCardId;

	private int _textBubbleGeneration;

	private GameInputManager GameInputManager => null;

	public override string OpenSfx => null;

	public override bool IsInputActionPopup => false;

	private InputActions.UiActions UiActions => default;

	private Inventory MyInventory => null;

	private GameObject[] CategorySlots => null;

	private GameObject[] PreferPrefixSlots => null;

	private GameObject[] DislikePrefixSlots => null;

	private GameObject[] CardSlots => null;

	private TArtifactDeal DealData => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__45))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void DisableAllPanelInput()
	{
	}

	private static bool IsNegotiatingState(EArtifactDealState s)
	{
		return false;
	}

	private static bool IsClosingState(EArtifactDealState s)
	{
		return false;
	}

	private void ApplyStateUi(EArtifactDealState next)
	{
	}

	private void OnDealStateChanged(EArtifactDealState next)
	{
	}

	private void BindDealingArtifactView(Goods goods)
	{
	}

	private void PlayRollingSfx()
	{
	}

	private void SetRemainCount(int targetCount, float duration = 0f)
	{
	}

	[AsyncStateMachine(typeof(_003COnDealStartedAsync_003Ed__56))]
	private UniTask<bool> OnDealStartedAsync(ArtifactDealStartedEvent e, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnGoodsSwappedAsync_003Ed__57))]
	private UniTask<bool> OnGoodsSwappedAsync(ArtifactDealGoodsSwappedEvent e, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CConsumePendingEventsAsync_003Ed__58))]
	private UniTaskVoid ConsumePendingEventsAsync(EArtifactDealState finishState)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CPlayEventAsync_003Ed__59))]
	private UniTask<bool> PlayEventAsync(IArtifactDealEvent dealEvent, CancellationToken ct)
	{
		return default;
	}

	private string PickBubbleText(string[] texts)
	{
		return null;
	}

	private string[] CardResultBubbleTexts(ArtifactDealCardResolvedEvent e)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003COnNpcHesitatingAsync_003Ed__64))]
	private UniTask<bool> OnNpcHesitatingAsync(ArtifactDealNpcHesitatingEvent e, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnCardResolvedAsync_003Ed__65))]
	private UniTask<bool> OnCardResolvedAsync(ArtifactDealCardResolvedEvent e, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CHideDealSuggestAfterAsync_003Ed__66))]
	private UniTaskVoid HideDealSuggestAfterAsync(GameObject suggest)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnCardConsumedAsync_003Ed__67))]
	private UniTask<bool> OnCardConsumedAsync(ArtifactDealCardConsumedEvent e, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CPlayCardCompleteAfterDelayAsync_003Ed__68))]
	private UniTaskVoid PlayCardCompleteAfterDelayAsync(ArtifactDealCardPlayPresenter playSlot)
	{
		return default;
	}

	private void PlayPriceFloorFeedback()
	{
	}

	[AsyncStateMachine(typeof(_003CAnimateTagDiffAsync_003Ed__70))]
	private UniTask<bool> AnimateTagDiffAsync(int[] addedPrefer, int[] removedPrefer, int[] addedDislike, int[] removedDislike, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnHandRerolledAsync_003Ed__71))]
	private UniTask<bool> OnHandRerolledAsync(ArtifactDealHandRerolledEvent e, CancellationToken ct)
	{
		return default;
	}

	private void PlayRerollCountFeedback()
	{
	}

	[AsyncStateMachine(typeof(_003CPlayNoMorePlayCountAsync_003Ed__73))]
	private UniTaskVoid PlayNoMorePlayCountAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CAnimateDealPriceAsync_003Ed__74))]
	private UniTask AnimateDealPriceAsync(PriceChange change, CancellationToken ct, bool showChangeFloating = true)
	{
		return default;
	}

	private void SetPriceChangeText(int deltaCoin)
	{
	}

	private int ToBalancePercent(int price, int originalPrice)
	{
		return 0;
	}

	[AsyncStateMachine(typeof(_003CAnimateRequirementSlotsAsync_003Ed__77))]
	private UniTask AnimateRequirementSlotsAsync(TagDiff diff, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CAnimateSlotTransitionsAsync_003Ed__78))]
	private UniTask<bool> AnimateSlotTransitionsAsync(GameObject[] slots, int[] before, int[] after, bool isPrefer, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CAnimateRemovedSlotsAsync_003Ed__79))]
	private UniTask AnimateRemovedSlotsAsync(IReadOnlyList<int> removedPrefer, IReadOnlyList<int> removedDislike, float duration, CancellationToken ct)
	{
		return default;
	}

	private bool[] SnapshotActiveStates(GameObject[] slots)
	{
		return null;
	}

	private List<ArtifactDealSlotPresenter> CollectNewlyActiveSlots(GameObject[] slots, bool[] activeBefore)
	{
		return null;
	}

	private ArtifactDealSlotPresenter FindActiveSlotByPrefixId(GameObject[] slots, int prefixId)
	{
		return null;
	}

	private UniTask<bool> WaitSequenceStepAsync(EArtifactDealStepDelay step, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CWaitSequenceStepAsync_003Ed__84))]
	private UniTask<bool> WaitSequenceStepAsync(float seconds, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CSetNpcSkeletonAsync_003Ed__85))]
	private UniTaskVoid SetNpcSkeletonAsync(CancellationToken ct)
	{
		return default;
	}

	private void CancelSkeletonLoad()
	{
	}

	public override void Close(object result = null)
	{
	}

	[AsyncStateMachine(typeof(_003CFinishSequenceAfterAsync_003Ed__88))]
	private UniTaskVoid FinishSequenceAfterAsync(EArtifactDealState sequence, float seconds)
	{
		return default;
	}

	private void ShowTextBubble(string message, float time)
	{
	}

	[AsyncStateMachine(typeof(_003CHideTextBubbleAfterAsync_003Ed__91))]
	private UniTaskVoid HideTextBubbleAfterAsync(float seconds, int generation)
	{
		return default;
	}

	private void PlayGuestSaySfx()
	{
	}

	private void RebuildMiniGameNav(Selectable preferred = null)
	{
	}

	private void SetActiveMiniGameUi(bool isActive)
	{
	}

	private void SetActiveNpcStageUi(bool isActive)
	{
	}

	private void SetRequirementSlots()
	{
	}

	private void ApplyTagMatchedFx()
	{
	}

	private void UpdateHoverMatchedOutline(InventoryGoodsSlotModel slotModel)
	{
	}

	private void ApplyHoverMatchedOutline(GameObject[] slots, int[] hoveredPrefixes)
	{
	}

	private void SetCards()
	{
	}

	private void SetCardsInteractable(bool interactable)
	{
	}

	private Selectable ResolvePreferredCardFocus(int newHandCount)
	{
		return null;
	}

	private Selectable GetCardSelectable(GameObject cardSlot)
	{
		return null;
	}

	private void SetRemainPlayCount()
	{
	}

	private void SetRemainRerollCount()
	{
	}

	private void SetDealPrice(int dealPrice, int originalPrice, float duration = 0f)
	{
	}

	private void ApplyMatchBonusBubble()
	{
	}

	private void SetDealExp(int dealExp, float duration = 0f)
	{
	}

	private void SetAllToDomain()
	{
	}

	private void ClearDealPrice()
	{
	}

	private void ClearDealExp()
	{
	}

	private void ClearRemainPlayCount()
	{
	}

	private void Back()
	{
	}

	private bool ShouldAutoSwitchToStash()
	{
		return false;
	}

	private bool IsDealableArtifactSlot(GoodsSlot slot)
	{
		return false;
	}

	private void SwapItemPanel()
	{
	}

	[AsyncStateMachine(typeof(_003COnSlotClick_003Ed__117))]
	private UniTaskVoid OnSlotClick(InventoryGoodsSlotModel slotModel)
	{
		return default;
	}

	private void OnEquipmentSlotClick(InventoryGoodsSlotModel slotModel)
	{
	}

	private void OnCardSlotClick(ArtifactDealCardPlayModel model)
	{
	}

	private void OnClickReroll()
	{
	}

	private void OnClickAcceptDeal()
	{
	}

	[AsyncStateMachine(typeof(_003COnClickAcceptDealAsync_003Ed__122))]
	private UniTask OnClickAcceptDealAsync()
	{
		return default;
	}

	private void OnClickSwapArtifact()
	{
	}

	[AsyncStateMachine(typeof(_003COnClickGiveUpAsync_003Ed__124))]
	private UniTask OnClickGiveUpAsync()
	{
		return default;
	}

	private void PlayStopping()
	{
	}

	[AsyncStateMachine(typeof(_003CPlayGivingUpAsync_003Ed__126))]
	private UniTaskVoid PlayGivingUpAsync()
	{
		return default;
	}

	private void TriggerNegotiationVipAchievement(bool isAccepted, int coin)
	{
	}

	[AsyncStateMachine(typeof(_003COpenBackOutPopupAfterCloseAsync_003Ed__128))]
	private UniTaskVoid OpenBackOutPopupAfterCloseAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CPlayConcludingAsync_003Ed__129))]
	private UniTaskVoid PlayConcludingAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CWaitUntilSelfRemovedAsync_003Ed__130))]
	private UniTask WaitUntilSelfRemovedAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COpenCompletePopupAfterCloseAsync_003Ed__131))]
	private UniTaskVoid OpenCompletePopupAfterCloseAsync(int gainExp, int initialCoin, int finalCoin, int balancePercent, Goods dealGoods)
	{
		return default;
	}
}
