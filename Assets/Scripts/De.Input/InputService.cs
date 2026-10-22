using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Table;
using NemoLib.Popup;
using UnityEngine.InputSystem;
using VContainer;

public class InputService : NativeBase
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public InputActionRebindingExtensions.RebindingOperation operation;

		internal void _003CRebindAsync_003Eb__2()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public InputControl cancelControl;

		public InputControl capturedControl;

		internal bool _003CWaitRebindControlReleaseAsync_003Eb__0()
		{
			return false;
		}
	}

	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CInitAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InputService _003C_003E4__this;

		public CancellationToken cancellationToken;

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
	private struct _003CLoadAsync_003Ed__11 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InputService _003C_003E4__this;

		public CancellationToken cancellationToken;

		private UniTask<byte[]>.Awaiter _003C_003Eu__1;

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
	private struct _003CRebindAsync_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InputService _003C_003E4__this;

		public int inputActionId;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

		private CancellationTokenRegistration _003C_003E7__wrap1;

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
	private struct _003CSaveAsync_003Ed__10 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public InputService _003C_003E4__this;

		public CancellationToken cancellationToken;

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
	private struct _003CWaitRebindControlReleaseAsync_003Ed__14 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public int inputActionId;

		public bool completed;

		public InputActionRebindingExtensions.RebindingOperation operation;

		public CancellationToken cancellationToken;

		private _003C_003Ec__DisplayClass14_0 _003C_003E8__1;

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

	private const string INPUT_BINDINGS_FILE = "InputBindings";

	[Inject]
	private GameInputManager _inputManager;

	[Inject]
	private BlockManager _blockManager;

	[Inject]
	private StorageService _storageService;

	[Inject]
	private InputActionAsset _inputActionAsset;

	private bool _isDirty;

	private bool _isInitialized;

	[AsyncStateMachine(typeof(_003CInitAsync_003Ed__7))]
	public UniTask InitAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public void MarkDirty()
	{
	}

	public void Flush()
	{
	}

	[AsyncStateMachine(typeof(_003CSaveAsync_003Ed__10))]
	private UniTask SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CLoadAsync_003Ed__11))]
	private UniTask LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	public bool CanRebind(int inputActionId)
	{
		return false;
	}

	[AsyncStateMachine(typeof(_003CRebindAsync_003Ed__13))]
	public UniTask RebindAsync(int inputActionId, CancellationToken cancellationToken = default(CancellationToken))
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CWaitRebindControlReleaseAsync_003Ed__14))]
	private UniTask WaitRebindControlReleaseAsync(InputActionRebindingExtensions.RebindingOperation operation, bool completed, int inputActionId, CancellationToken cancellationToken)
	{
		return default;
	}

	private InputActionRebindingExtensions.RebindingOperation BuildRebindingOperation(int inputActionId)
	{
		return null;
	}

	private void ApplySyncBinding(int inputActionId)
	{
	}

	public void ResetBindings(EControlDevice device)
	{
	}

	private void RemoveOverride(string actionName, int bindingIndex)
	{
	}

	private bool TryClearConflictingBindings(int rebindingActionId)
	{
		return false;
	}

	private bool TryGetEffectivePath(string actionName, int bindingIndex, out string path)
	{
		path = null;
		return false;
	}

	private string GetModifierPath(TInputAction data)
	{
		return null;
	}
}
