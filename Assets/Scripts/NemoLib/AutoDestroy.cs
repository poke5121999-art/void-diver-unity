using UniRx;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	private float _delay = -1f;

	private bool _isDestroyed;

	private void OnDestroy()
	{
		_isDestroyed = true;
	}

	public void SetDelay(float delay)
	{
		if (!_isDestroyed)
		{
			_delay = delay;
			_isDestroyed = true;
			Observable.Timer(System.TimeSpan.FromSeconds(delay)).Subscribe(delegate
			{
				if (this != null)
				{
					Object.Destroy(base.gameObject);
				}
			}).AddTo(this);
		}
	}
}
