using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class ReloadCharacterView : CharacterView
{
	[SerializeField]
	private int reloadBuffId;

	private const string RELOAD_IDLE = "battle/reload";

	private const string RELOAD_WALK = "battle/reload_walk";

	private const int RELOAD_WALK_TRACK = 1;

	private bool _isReloadPlaying;

	private bool _reserveReload;

	private float _reloadStartTime;

	private float _reloadAnimSpeed;

	private float _reloadPlayDuration;

	protected override void Start()
	{
		base.Start();
		UnitModel.RxBuffs.ObserveAdd().Where((DictionaryAddEvent<int, UnitBuff> e) => e.Key == reloadBuffId).Subscribe(delegate
		{
			TryStartReload();
		})
			.AddTo(this);
		UnitModel.RxBuffs.ObserveRemove().Where((DictionaryRemoveEvent<int, UnitBuff> e) => e.Key == reloadBuffId).Subscribe(delegate
		{
			StopReload();
		})
			.AddTo(this);
	}

	private void TryStartReload()
	{
		EActionState state = UnitModel.State;
		if (IsLoopingActionState(state))
		{
			StartReload(state);
		}
		else
		{
			_reserveReload = true;
		}
	}

	private void StartReload(EActionState state)
	{
		float animationDuration = GetAnimationDuration("battle/reload");
		if (animationDuration <= 0f)
		{
			return;
		}
		if (UnitModel.Buffs.TryGetValue(reloadBuffId, out var value))
		{
			float num = value.EndTime - GameManager.TimeNow;
			if (!(num <= 0f))
			{
				_isReloadPlaying = true;
				_reserveReload = false;
				_reloadStartTime = Time.time;
				_reloadAnimSpeed = animationDuration / num;
				_reloadPlayDuration = num;
				PlayAnimationByActionState(state);
			}
		}
	}

	private void StopReload()
	{
		_reserveReload = false;
		if (_isReloadPlaying)
		{
			_isReloadPlaying = false;
			ClearReloadWalk();
			if (IsLoopingActionState(UnitModel.State))
			{
				PlayAnimationByActionState(UnitModel.State);
			}
		}
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
		if (_reserveReload && !_isReloadPlaying && IsLoopingActionState(state))
		{
			StartReload(state);
			return;
		}
		if (_isReloadPlaying && !IsLoopingActionState(state))
		{
			_isReloadPlaying = false;
		}
		if (_isReloadPlaying)
		{
			if (CharacterModel.TryRun)
			{
				_isReloadPlaying = false;
			}
			if (_isReloadPlaying)
			{
				float num = Time.time - _reloadStartTime;
				if (_reloadPlayDuration <= num)
				{
					_isReloadPlaying = false;
				}
				else
				{
					if (state != EActionState.Move)
					{
						ClearReloadWalk();
						PlayAnimationAsync("battle/reload", isLoop: false, num * _reloadAnimSpeed, null, _reloadAnimSpeed).Forget();
						return;
					}
					base.PlayAnimationByActionState(EActionState.Move);
					PlayReloadWalk();
					return;
				}
			}
		}
		ClearReloadWalk();
		base.PlayAnimationByActionState(state);
	}

	private void PlayReloadWalk()
	{
		if (CurrentSkeletonAnimation == null || GetAnimationDuration("battle/reload_walk") <= 0f)
		{
			return;
		}
		Spine.TrackEntry trackEntry = CurrentSkeletonAnimation.state.SetAnimation(1, "battle/reload_walk", loop: true);
		if (trackEntry != null)
		{
			trackEntry.TrackTime = (Time.time - _reloadStartTime) * _reloadAnimSpeed;
			trackEntry.TimeScale = _reloadAnimSpeed;
		}
	}

	private void ClearReloadWalk()
	{
		if (!(CurrentSkeletonAnimation == null))
		{
			CurrentSkeletonAnimation.state.SetEmptyAnimation(1, 0f);
		}
	}

	private static bool IsLoopingActionState(EActionState state)
	{
		if (state != EActionState.Idle && state != EActionState.Move && state != EActionState.CrowdControlled)
		{
			return state == EActionState.Interact;
		}
		return true;
	}
}
