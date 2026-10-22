using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

[DefaultExecutionOrder(-10)]
[ExecuteAlways]
public class DecorationVisualManager : MonoBehaviour
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CPlayTransitionIn_003Ed__18 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public DecorationVisualManager _003C_003E4__this;

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
	private struct _003CPlayTransitionOut_003Ed__16 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public DecorationVisualManager _003C_003E4__this;

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
	private PlayableDirector _directorOut;

	[SerializeField]
	private PlayableDirector _directorIn;

	[Header("Dissolve Settings")]
	[SerializeField]
	private bool _dissolveEnabled;

	[SerializeField]
	private Color _edgeColor;

	[Header("Debug: 타이밍 테스트용 수동 등록")]
	[SerializeField]
	private List<DecorationObjectDissolveTarget> _debugObjectTargets;

	[SerializeField]
	private List<DecorationSkinDissolveTarget> _debugSkinTargets;

	private readonly List<IDissolveTarget> _targets;

	public static DecorationVisualManager Instance { get; private set; }

	public Color EdgeColor => default;

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
	}

	[AsyncStateMachine(typeof(_003CPlayTransitionOut_003Ed__16))]
	public UniTask PlayTransitionOut()
	{
		return default;
	}

	public void StopTransitionOut()
	{
	}

	[AsyncStateMachine(typeof(_003CPlayTransitionIn_003Ed__18))]
	public UniTask PlayTransitionIn()
	{
		return default;
	}

	public void StopTransitionIn()
	{
	}

	public void ResetTransitions()
	{
	}

	public void SetAllProgress(float progress)
	{
	}

	public bool IsRegistered(IDissolveTarget target)
	{
		return false;
	}

	public void Register(IDissolveTarget target)
	{
	}

	public void Unregister(IDissolveTarget target)
	{
	}

	public void ClearTargets()
	{
	}
}
