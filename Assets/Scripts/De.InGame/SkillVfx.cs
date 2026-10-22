using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class SkillVfx : MonoBase
{
	protected GameObject OwnerGo;

	protected HitBox HitBox;

	private bool _isPendingDestroy;

	private Vector3 _positionOffset;

	private bool _isTracking;

	private CompositeDisposable _disposable;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public virtual void Init(GameObject ownerGo, HitBox hitBox = null, Vector3 positionOffset = default(Vector3))
	{
	}

	protected virtual void FixedUpdate()
	{
	}

	private void LateUpdate()
	{
	}

	private void PlayEnd(EHitBoxDespawnReason reason)
	{
	}

	private static string GetEndAnimationTrigger(Animator vfxAnimator, EHitBoxDespawnReason reason)
	{
		return null;
	}

	private void OnAnimationEnded()
	{
	}
}
