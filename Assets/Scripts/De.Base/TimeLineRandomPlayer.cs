using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineRandomPlayer : MonoBehaviour
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CStartOnceAnimationAsync_003Ed__7 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public TimeLineRandomPlayer _003C_003E4__this;

		private CancellationToken _003Ctoken_003E5__2;

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
	private PlayableDirector[] _loopList;

	[SerializeField]
	private PlayableDirector[] _onceList;

	[Range(0f, 100f)]
	[SerializeField]
	private float _loopPercent;

	private CancellationTokenSource _cancellationTokenSource;

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	private void StartLoopAnimation()
	{
	}

	[AsyncStateMachine(typeof(_003CStartOnceAnimationAsync_003Ed__7))]
	private UniTask StartOnceAnimationAsync()
	{
		return default;
	}

	private void DisableAll()
	{
	}
}
