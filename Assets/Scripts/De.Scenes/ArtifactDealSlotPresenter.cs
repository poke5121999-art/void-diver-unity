using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ArtifactDealSlotPresenter : BaseSlotPresenter<ArtifactDealSlotModel>
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlayAddAnimationAsync_003Ed__21 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealSlotPresenter _003C_003E4__this;

		public float duration;

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
	private struct _003CPlayRemoveAnimationAsync_003Ed__20 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public ArtifactDealSlotPresenter _003C_003E4__this;

		public float duration;

		public CancellationToken ct;

		private GameObject _003Ccancel_003E5__2;

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

	[InjectModel]
	private ArtifactDealSlotModel _model;

	[View]
	private ArtifactDealSlotView _view;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	protected override ArtifactDealSlotModel SlotModel => null;

	protected override Button Button => null;

	public int PrefixId => 0;

	public bool IsMatched { get; private set; }

	private bool IsPreferPrefix => false;

	private bool IsNegativeFramePrefix => false;

	private void UpdateMatch()
	{
	}

	public void SetHoverMatchedOutline(int[] hoveredPrefixes)
	{
	}

	private void UpdateBenefit(GameObject positiveBadge, TextMeshProUGUI positiveTextNum, GameObject negativeBadge, TextMeshProUGUI negativeTextNum, bool isMatched)
	{
	}

	[AsyncStateMachine(typeof(_003CPlayRemoveAnimationAsync_003Ed__20))]
	public UniTask PlayRemoveAnimationAsync(float duration, CancellationToken ct)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CPlayAddAnimationAsync_003Ed__21))]
	public UniTask PlayAddAnimationAsync(float duration, CancellationToken ct)
	{
		return default;
	}

	private CanvasGroup GetOrAddCanvasGroup()
	{
		return null;
	}

	private void OnInject(ArtifactDealSlotModel model)
	{
	}
}
