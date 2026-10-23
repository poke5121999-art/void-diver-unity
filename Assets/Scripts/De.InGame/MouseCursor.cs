using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursor : CursorBase
{
	[SerializeField]
	private GameObject[] _clickEffects;

	private const float CLICK_EFFECT_DURATION = 0.5f;

	private int _clickEffectIndex;

	private CancellationTokenSource[] _clickEffectCts;

	protected override Vector2 GetCursorPosition()
	{
		return Mouse.current.position.ReadValue();
	}

	protected override bool IsCursorVisible()
	{
		return true;
	}

	protected override void Start()
	{
		base.Start();
		InitClickEffects();
	}

	private void OnDestroy()
	{
		if (_clickEffectCts == null)
		{
			return;
		}
		for (int i = 0; i < _clickEffectCts.Length; i++)
		{
			CancellationTokenSource cancellationTokenSource = _clickEffectCts[i];
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
				cancellationTokenSource.Dispose();
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
		{
			PlayClickEffect(Mouse.current.position.ReadValue());
		}
	}

	private void InitClickEffects()
	{
		if (_clickEffects == null || _clickEffects.Length == 0)
		{
			return;
		}
		_clickEffectCts = new CancellationTokenSource[_clickEffects.Length];
		for (int i = 0; i < _clickEffects.Length; i++)
		{
			GameObject gameObject = _clickEffects[i];
			if (gameObject != null)
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	private void PlayClickEffect(Vector2 position)
	{
		if (_clickEffects != null && _clickEffects.Length != 0)
		{
			int num = _clickEffectIndex;
			_clickEffectIndex = (_clickEffectIndex + 1) % _clickEffects.Length;
			GameObject gameObject = _clickEffects[num];
			if (!(gameObject == null))
			{
				CancellationToken token = ResetClickEffectCts(num);
				PlayClickEffectAsync(gameObject, position, token).Forget();
			}
		}
	}

	private CancellationToken ResetClickEffectCts(int index)
	{
		if (_clickEffectCts[index] != null)
		{
			_clickEffectCts[index].Cancel();
		}
		if (_clickEffectCts[index] != null)
		{
			_clickEffectCts[index].Dispose();
		}
		_clickEffectCts[index] = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
		return _clickEffectCts[index].Token;
	}

	private async UniTaskVoid PlayClickEffectAsync(GameObject effect, Vector2 position, CancellationToken token)
	{
		effect.transform.position = position;
		effect.SetActive(value: false);
		effect.SetActive(value: true);
		if (!await UniTask.Delay(TimeSpan.FromSeconds(0.5), ignoreTimeScale: false, PlayerLoopTiming.Update, token).SuppressCancellationThrow())
		{
			effect.SetActive(value: false);
		}
	}
}
