using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using NemoLib.Mvp;
using VContainer;

public class LoungeScene : InGameScene
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003COnPlaying_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public LoungeScene _003C_003E4__this;

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
	private struct _003COpenStageResultPopupAsync_003Ed__10 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LoungeScene _003C_003E4__this;

		private UniTask.Awaiter _003C_003Eu__1;

		private UniTask<StageResultPopupPresenter>.Awaiter _003C_003Eu__2;

		private UniTask<object>.Awaiter _003C_003Eu__3;

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
	private struct _003CTryShowHostLoungeCode_003Ed__8 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LoungeScene _003C_003E4__this;

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
	private struct _003CTryStartTutorialCampaignAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public LoungeScene _003C_003E4__this;

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

	[View]
	private LoungeSceneView _view;

	[Inject]
	private TycoonManager _tycoonManager;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	private bool _isTutorialStarting;

	private bool IsHost => false;

	protected override void Start()
	{
	}

	[AsyncStateMachine(typeof(_003CTryStartTutorialCampaignAsync_003Ed__7))]
	private UniTask TryStartTutorialCampaignAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CTryShowHostLoungeCode_003Ed__8))]
	private UniTask TryShowHostLoungeCode()
	{
		return default;
	}

	private void OnStageLoadCompleted()
	{
	}

	[AsyncStateMachine(typeof(_003COpenStageResultPopupAsync_003Ed__10))]
	private UniTask OpenStageResultPopupAsync()
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003COnPlaying_003Ed__11))]
	private UniTaskVoid OnPlaying()
	{
		return default;
	}

	private void OnCampaignStarted((int campaignId, EDifficulty difficulty) campaignData)
	{
	}

	private void OnCampaignCompleted(int campaignId, bool isFirstClear)
	{
	}

	private void OnQuestFailed()
	{
	}

	private void OnClientCaughtUp(ulong clientId)
	{
	}
}
