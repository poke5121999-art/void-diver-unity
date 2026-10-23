using NemoLib.AssetManagement;
using System;
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
		_isPendingDestroy = false;
		_disposable = new CompositeDisposable();
	}

	private void OnDisable()
	{
		if (_disposable != null)
		{
			_disposable.Dispose();
		}
		_disposable = null;
	}

	public virtual void Init(GameObject ownerGo, HitBox hitBox = null, Vector3 positionOffset = default(Vector3))
	{
		OwnerGo = ownerGo;
		HitBox = hitBox;
		_positionOffset = positionOffset;
		_isTracking = hitBox != null;
		if (hitBox != null)
		{
			hitBox.RxDespawnReason.Where((EHitBoxDespawnReason reason) => reason != EHitBoxDespawnReason.None).Subscribe(PlayEnd).AddTo(_disposable);
		}
	}

	protected virtual void FixedUpdate()
	{
	}

	private void LateUpdate()
	{
		if (_isTracking)
		{
			if (!(HitBox == null) && HitBox.gameObject.activeInHierarchy)
			{
				base.transform.position = HitBox.transform.TransformPoint(_positionOffset);
				base.transform.rotation = HitBox.transform.rotation;
				return;
			}
			_isTracking = false;
			PlayEnd(EHitBoxDespawnReason.Destroy);
		}
	}

	private void PlayEnd(EHitBoxDespawnReason reason)
	{
		if (!_isPendingDestroy)
		{
			Animator component = GetComponent<Animator>();
			string endAnimationTrigger = GetEndAnimationTrigger(component, reason);
			if (!endAnimationTrigger.HasValue())
			{
				base.gameObject.DespawnOrDestroy();
				return;
			}
			_isPendingDestroy = true;
			component.SetTrigger(endAnimationTrigger);
		}
	}

	private static string GetEndAnimationTrigger(Animator vfxAnimator, EHitBoxDespawnReason reason)
	{
		if (vfxAnimator == null)
		{
			return null;
		}
		string text = null;
		switch (reason)
		{
		case EHitBoxDespawnReason.Timeout:
			text = "TimeoutTrigger";
			break;
		case EHitBoxDespawnReason.Destroy:
			text = "DestroyTrigger";
			break;
		}
		if (text != null && vfxAnimator.HasTrigger(text))
		{
			return text;
		}
		return vfxAnimator.HasTrigger("EndTrigger") ? "EndTrigger" : null;
	}

	private void OnAnimationEnded()
	{
		base.gameObject.DespawnOrDestroy();
	}
}
