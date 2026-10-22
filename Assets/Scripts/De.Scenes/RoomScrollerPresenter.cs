using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class RoomScrollerPresenter : VerticalScrollerPresenter<RoomSlotModel>
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass16_0
	{
		public LoungeFilterPopupModel filter;

		public RoomScrollerPresenter _003C_003E4__this;

		public int maxPingMs;

		internal bool _003CRefreshRoomListAsync_003Eb__0(MatchingLobby lobby)
		{
			return false;
		}

		internal void _003CRefreshRoomListAsync_003Eb__2(RoomSlotModel _)
		{
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CRefreshRoomListAsync_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LoungeFilterPopupModel filter;

		public RoomScrollerPresenter _003C_003E4__this;

		private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

		private UniTask<MatchingLobby[]>.Awaiter _003C_003Eu__1;

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
	private EnhancedScrollerCellView cellViewPrefab;

	[SerializeField]
	private float cellSize;

	[InjectModel]
	private RoomScrollerModel _model;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _disposable;

	private CompositeDisposable _injectDisposable;

	protected override BaseScrollerModel<RoomSlotModel> ScrollerModel => null;

	public int ItemCount => 0;

	protected override int GetColumnCount()
	{
		return 0;
	}

	private void OnInject(RoomScrollerModel model)
	{
	}

	protected override void OnDestroy()
	{
	}

	protected override List<RoomSlotModel> BuildSlotModels()
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CRefreshRoomListAsync_003Ed__16))]
	public UniTask RefreshRoomListAsync(LoungeFilterPopupModel filter)
	{
		return default;
	}

	private static int GetMaxPingMs(ERoomPingFilterType filterType)
	{
		return 0;
	}

	private static bool IsDifficultyAllowed(MatchingLobby lobby, HashSet<ERoomDifficulty> blocked)
	{
		return false;
	}

	private static bool IsHostLevelAllowed(MatchingLobby lobby, int minHostLevel)
	{
		return false;
	}

	private bool IsPingAllowed(MatchingLobby lobby, int maxPingMs)
	{
		return false;
	}

	private void OnGameRoomCellClick(RoomSlotModel slotModel)
	{
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
