using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

public class MouseCursor : CursorBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlayClickEffectAsync_003Ed__12 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

		public GameObject effect;

		public Vector2 position;

		public CancellationToken token;

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

	[SerializeField]
	private GameObject[] _clickEffects;

	private const float CLICK_EFFECT_DURATION = 0.5f;

	private int _clickEffectIndex;

	private CancellationTokenSource[] _clickEffectCts;

	protected override Vector2 GetCursorPosition()
	{
		return default;
	}

	protected override bool IsCursorVisible()
	{
		return false;
	}

	protected override void Start()
	{
	}

	private void OnDestroy()
	{
	}

	protected override void Update()
	{
	}

	private void InitClickEffects()
	{
	}

	private void PlayClickEffect(Vector2 position)
	{
	}

	private CancellationToken ResetClickEffectCts(int index)
	{
		return default;
	}

	[AsyncStateMachine(typeof(_003CPlayClickEffectAsync_003Ed__12))]
	private UniTaskVoid PlayClickEffectAsync(GameObject effect, Vector2 position, CancellationToken token)
	{
		return default;
	}
}
