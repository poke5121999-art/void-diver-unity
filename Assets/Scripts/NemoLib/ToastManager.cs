using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class ToastManager : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInstantiateToastAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder<GameObject> _003C_003Et__builder;

		public ToastManager _003C_003E4__this;

		public string baseName;

		private UniTask<GameObject>.Awaiter _003C_003Eu__1;

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
	private struct _003CProcessQueueAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public ToastManager _003C_003E4__this;

		private Model _003Cmodel_003E5__2;

		private GameObject _003Cgo_003E5__3;

		private UniTask<GameObject>.Awaiter _003C_003Eu__1;

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

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private string _defaultOpenSfx;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private const float _queueDelay = 0.4f;

	private readonly Queue<Model> _queue;

	private bool _isProcessing;

	private readonly Subject<string> _onCloseDuplicateToast;

	private Transform RootTf => null;

	protected override void Awake()
	{
	}

	[AsyncStateMachine(typeof(_003CInstantiateToastAsync_003Ed__11))]
	private UniTask<GameObject> InstantiateToastAsync(string baseName)
	{
		return default;
	}

	public void Show(Model model)
	{
	}

	[AsyncStateMachine(typeof(_003CProcessQueueAsync_003Ed__13))]
	private UniTaskVoid ProcessQueueAsync()
	{
		return default;
	}

	private void OnClose(BaseToastPresenter target)
	{
	}

	public void CloseDuplicateToasts(string message)
	{
	}
}
