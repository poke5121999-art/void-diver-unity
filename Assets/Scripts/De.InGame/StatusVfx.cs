using System;
using NemoLib.AssetManagement;
using UnityEngine;
using UniRx;

public class StatusVfx : MonoBase
{
	private bool _isPendingDestroy;

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

	public void Despawn(float timeDelay = 0f)
	{
		if (timeDelay > 0f)
		{
			Observable.Timer(TimeSpan.FromSeconds(timeDelay)).Subscribe(delegate
			{
				OnDespawn();
			}).AddTo(_disposable);
		}
		else
		{
			OnDespawn();
		}
	}

	private void OnDespawn()
	{
		if (!_isPendingDestroy)
		{
			Animator component = GetComponent<Animator>();
			if (component != null && component.HasTrigger("EndTrigger"))
			{
				_isPendingDestroy = true;
				component.SetTrigger("EndTrigger");
			}
			else
			{
				base.gameObject.DespawnOrDestroy();
			}
		}
	}

	public void SetVisible(bool visible)
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = visible;
		}
	}

	private void OnAnimationEnded()
	{
		if (this == null && base.gameObject == null)
		{
			return;
		}
		base.gameObject.DespawnOrDestroy();
	}
}
