using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Data;
using GameCommon.Enum;
using UnityEngine;

namespace De.InGame.Unit
{
	public class MonsterView3D : MonsterView
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayAnimationAsync_003Ed__8 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public MonsterView3D _003C_003E4__this;

			public float defaultSpeed;

			public string animationName;

			public float animStartOffsetTime;

			public AnimationSpeed[] animSpeeds;

			public bool isLoop;

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

		[Header("3D Model")]
		[SerializeField]
		private Animator _animator;

		[Header("Bone Transforms")]
		[SerializeField]
		private Transform _boneHead;

		[SerializeField]
		private Transform _boneBody;

		[SerializeField]
		private Transform _boneDeath;

		private CancellationTokenSource _animCancelToken;

		protected override void InitAnimationVisible()
		{
		}

		protected override void InitSkeleton()
		{
		}

		protected override void LateUpdate()
		{
		}

		[AsyncStateMachine(typeof(_003CPlayAnimationAsync_003Ed__8))]
		public override UniTask PlayAnimationAsync(string animationName, bool isLoop, float animStartOffsetTime, AnimationSpeed[] animSpeeds, float defaultSpeed = 1f, string multiTrackAnimationName = null)
		{
			return default;
		}

		public override void UpdateAnimation(string animationName, string beforeAnimationName)
		{
		}

		public override void SetAnimationSpeed(float speed)
		{
		}

		public override float GetAnimationDuration(string animationName)
		{
			return 0f;
		}

		public override string GetMoveAnimation()
		{
			return null;
		}

		public override Vector3 GetBoneOffset(EUnitBoneType boneType)
		{
			return default;
		}

		protected override void ChangeSkin(string skinName)
		{
		}

		private bool HasAnimatorState(string stateName)
		{
			return false;
		}

		private bool HasAnimationClip(string clipName)
		{
			return false;
		}

		private float GetCurrentAnimatorTime()
		{
			return 0f;
		}
	}
}
