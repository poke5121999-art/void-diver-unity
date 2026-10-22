using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public sealed class SliderPadAccelerator
{
	private const float AccelStartDelay = 0.5f;

	private const float AccelRampTime = 1.2f;

	private const float MinRepeatRate = 0.025f;

	private InputSystemUIInputModule _module;

	private float _baseRepeatRate;

	private bool _captured;

	private float _holdStartTime = -1f;

	public void Tick(Selectable focused, Vector2 heldDir, bool isGamePad)
	{
		if (isGamePad && focused is Slider slider && IsHorizontal(slider) && Mathf.Abs(heldDir.x) > 0.5f)
		{
			if (Capture())
			{
				if (_holdStartTime < 0f)
				{
					_holdStartTime = Time.unscaledTime;
				}
				float t = Mathf.Clamp01((Time.unscaledTime - _holdStartTime - 0.5f) / 1.2f);
				_module.moveRepeatRate = Mathf.Lerp(_baseRepeatRate, 0.025f, t);
			}
		}
		else
		{
			Restore();
			_holdStartTime = -1f;
		}
	}

	public void Reset()
	{
		Restore();
		_holdStartTime = -1f;
	}

	private bool Capture()
	{
		if (!_captured)
		{
			EventSystem current = EventSystem.current;
			_module = ((current != null) ? (current.currentInputModule as InputSystemUIInputModule) : null);
			if (_module == null)
			{
				return false;
			}
			_baseRepeatRate = _module.moveRepeatRate;
			_captured = true;
		}
		return true;
	}

	private void Restore()
	{
		if (_captured)
		{
			_module.moveRepeatRate = _baseRepeatRate;
			_captured = false;
		}
	}

	private static bool IsHorizontal(Slider slider)
	{
		if (slider.direction != Slider.Direction.LeftToRight)
		{
			return slider.direction == Slider.Direction.RightToLeft;
		}
		return true;
	}
}
