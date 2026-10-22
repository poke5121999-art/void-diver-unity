using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using UnityEngine;

public class SpeedVfxObject : MonoBase
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CApplySpeedSequence_003Ed__5 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public IList<AnimationSpeed> speeds;

		public SpeedVfxObject _003C_003E4__this;

		public CancellationToken token;

		private int _003Ci_003E5__2;

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
	private struct _003CSetSpeed_003Ed__3 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public SpeedVfxObject _003C_003E4__this;

		public IList<AnimationSpeed> animationSpeeds;

		public float loopDuration;

		public bool loop;

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

	protected IList<AnimationSpeed> _animSpeeds;

	protected ParticleSystem[] _particleSystems;

	private float _loopDuration;

	[AsyncStateMachine(typeof(_003CSetSpeed_003Ed__3))]
	public UniTask SetSpeed(IList<AnimationSpeed> animationSpeeds, bool loop = true, float loopDuration = -1f)
	{
		return default;
	}

	private void RestartAllParticles()
	{
	}

	[AsyncStateMachine(typeof(_003CApplySpeedSequence_003Ed__5))]
	private UniTask ApplySpeedSequence(IList<AnimationSpeed> speeds, CancellationToken token)
	{
		return default;
	}

	private float GetMaxRemainingDuration()
	{
		return 0f;
	}

	private void ResetSimulationSpeed()
	{
	}
}
