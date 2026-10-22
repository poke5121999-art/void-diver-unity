using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class ArtifactDealBackOutPopupPresenter : GamePopupPresenter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CAutoCloseAsync_003Ed__15 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealBackOutPopupPresenter _003C_003E4__this;

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
	private struct _003COnOpenAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealBackOutPopupPresenter _003C_003E4__this;

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
	private struct _003CSetNpcSkeletonAsync_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ArtifactDealBackOutPopupPresenter _003C_003E4__this;

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

	private const float AutoCloseDelay = 2.5f;

	[InjectModel]
	private ArtifactDealBackOutPopupModel _model;

	[View]
	private ArtifactDealBackOutPopupView _view;

	[Inject]
	private AssetManager _assetManager;

	private CancellationTokenSource _skeletonCts;

	private GameInputManager GameInputManager => null;

	private InputActions.UiActions UiActions => default;

	public override bool IsInputActionPopup => false;

	public override string OpenSfx => null;

	[AsyncStateMachine(typeof(_003COnOpenAsync_003Ed__13))]
	protected override UniTask OnOpenAsync()
	{
		return default;
	}

	protected override void OnInputEnabled()
	{
	}

	[AsyncStateMachine(typeof(_003CAutoCloseAsync_003Ed__15))]
	private UniTaskVoid AutoCloseAsync()
	{
		return default;
	}

	private void ShowTextBubble(string message)
	{
	}

	private string PickBubbleText(string[] texts)
	{
		return null;
	}

	[AsyncStateMachine(typeof(_003CSetNpcSkeletonAsync_003Ed__18))]
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
}
