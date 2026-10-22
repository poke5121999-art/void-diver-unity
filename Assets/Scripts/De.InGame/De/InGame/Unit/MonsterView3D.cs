using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

namespace De.InGame.Unit
{
	public class MonsterView3D : MonsterView
	{
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

		public override async UniTask PlayAnimationAsync(string animationName, bool isLoop, float animStartOffsetTime, AnimationSpeed[] animSpeeds, float defaultSpeed = 1f, string multiTrackAnimationName = null)
		{
			if (_animator == null)
			{
				return;
			}
			if (_animCancelToken != null)
			{
				_animCancelToken.Cancel();
				_animCancelToken.Dispose();
				_animCancelToken = null;
			}
			SetAnimationSpeed(defaultSpeed);
			if (!HasAnimatorState(animationName))
			{
				return;
			}
			float normalizedTimeOffset = 0f;
			if (animStartOffsetTime > 0f)
			{
				normalizedTimeOffset = animStartOffsetTime / GetAnimationDuration(animationName);
			}
			_animator.CrossFade(animationName, 0.15f, 0, normalizedTimeOffset);
			if (animSpeeds == null || animSpeeds.Length == 0)
			{
				return;
			}
			_animCancelToken = new CancellationTokenSource();
			do
			{
				_animator.Play(animationName, 0, 0f);
				for (int i = 0; i < animSpeeds.Length; i++)
				{
					if (animSpeeds[i].speed <= 0f)
					{
						continue;
					}
					SetAnimationSpeed(animSpeeds[i].speed);
					float duration = GetAnimationDuration(animationName);
					float currentTime = GetCurrentAnimatorTime();
					float delay = (duration - currentTime) / animSpeeds[i].speed;
					float previousEndTime = ((i >= 1) ? animSpeeds[i - 1].endTime : 0f);
					float endTime = animSpeeds[i].endTime;
					if (endTime <= currentTime)
					{
						continue;
					}
					if (endTime >= 0f)
					{
						float segment = (endTime - previousEndTime) / animSpeeds[i].speed;
						if (segment < delay)
						{
							delay = segment;
						}
					}
					if (!(delay <= 0f))
					{
						await UniTask.Delay(TimeSpan.FromSeconds(delay), false, PlayerLoopTiming.Update, _animCancelToken.Token);
					}
				}
			}
			while (isLoop);
		}

		public override void UpdateAnimation(string animationName, string beforeAnimationName)
		{
			if (!(_animator == null) && HasAnimatorState(animationName))
			{
				AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo.IsName(beforeAnimationName))
				{
					_animator.Play(animationName, 0, currentAnimatorStateInfo.normalizedTime);
				}
			}
		}

		public override void SetAnimationSpeed(float speed)
		{
			if (!(_animator == null))
			{
				_animator.speed = speed;
			}
		}

		public override float GetAnimationDuration(string animationName)
		{
			if (_animator == null || _animator.runtimeAnimatorController == null)
			{
				return 0f;
			}
			AnimationClip animationClip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault((AnimationClip c) => c.name == animationName);
			if (animationClip != null)
			{
				return animationClip.length;
			}
			return 0f;
		}

		public override string GetMoveAnimation()
		{
			if (UnitModel.MoveSpeed <= 0f)
			{
				return GetIdleAnimation();
			}
			bool flag = HasAnimationClip("run");
			if (UnitModel.MoveSpeed < TConst.MoveSpeedThresholdForRun.FloatValue)
			{
				flag = false;
			}
			string text = (flag ? "run" : "walk");
			if (MonsterModel != null && MonsterModel.IsPhaseMonster)
			{
				text = string.Format("{0}/{1}", MonsterModel.Phase, text);
			}
			return text;
		}

		public override Vector3 GetBoneOffset(EUnitBoneType boneType)
		{
			switch (boneType)
			{
			case EUnitBoneType.None:
				return Vector3.zero;
			case EUnitBoneType.Symbol:
				return Vector3.up * SymbolPositionY;
			}
			Transform transform;
			switch (boneType)
			{
			case EUnitBoneType.Head:
			case EUnitBoneType.Eye:
				transform = _boneHead;
				break;
			case EUnitBoneType.Body:
				transform = _boneBody;
				break;
			case EUnitBoneType.Death:
				transform = ((_boneDeath != null) ? _boneDeath : _boneBody);
				break;
			default:
				transform = null;
				break;
			}
			if (transform == null)
			{
				return Vector3.zero;
			}
			return transform.position - base.transform.position;
		}

		protected override void ChangeSkin(string skinName)
		{
		}

		private bool HasAnimatorState(string stateName)
		{
			if (_animator == null)
			{
				return false;
			}
			return _animator.HasState(0, Animator.StringToHash(stateName));
		}

		private bool HasAnimationClip(string clipName)
		{
			if (_animator == null)
			{
				return false;
			}
			if (_animator.runtimeAnimatorController == null)
			{
				return false;
			}
			return _animator.runtimeAnimatorController.animationClips.Any((AnimationClip c) => c.name == clipName);
		}

		private float GetCurrentAnimatorTime()
		{
			AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			return currentAnimatorStateInfo.length * currentAnimatorStateInfo.normalizedTime;
		}
	}
}
