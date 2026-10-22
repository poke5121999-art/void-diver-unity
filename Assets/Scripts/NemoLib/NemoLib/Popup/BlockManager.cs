using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.Log;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NemoLib.Popup
{
	public class BlockManager : MonoBase
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnableCallingAsync_003Ed__13 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

			public BlockManager _003C_003E4__this;

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
		private GameObject _contents;

		[SerializeField]
		private Image _background;

		[SerializeField]
		private GameObject _calling;

		[SerializeField]
		private float _callingDelay;

		[Inject]
		private SeriLogger _logger;

		[Inject]
		private InputManager _inputManager;

		private int _stackCount;

		private int _callingStackCount;

		private int _backgroundStackCount;

		private CancellationTokenSource _cts;

		protected override void Awake()
		{
		}

		public void Block(bool calling = true, bool background = false)
		{
		}

		public void Release(bool calling = true, bool background = false)
		{
		}

		[AsyncStateMachine(typeof(_003CEnableCallingAsync_003Ed__13))]
		private UniTaskVoid EnableCallingAsync()
		{
			return default;
		}

		private void DisableCalling()
		{
		}

		private void SetBackgroundAlpha(float alpha)
		{
		}

		private void OnDestroy()
		{
		}
	}
}
