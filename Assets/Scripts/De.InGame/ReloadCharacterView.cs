using GameCommon.Enum;
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
	}

	private void TryStartReload()
	{
	}

	private void StartReload(EActionState state)
	{
	}

	private void StopReload()
	{
	}

	protected override void PlayAnimationByActionState(EActionState state)
	{
	}

	private void PlayReloadWalk()
	{
	}

	private void ClearReloadWalk()
	{
	}

	private static bool IsLoopingActionState(EActionState state)
	{
		return false;
	}
}
