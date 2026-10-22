using UnityEngine;
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

	private float _holdStartTime;

	public void Tick(Selectable focused, Vector2 heldDir, bool isGamePad)
	{
	}

	public void Reset()
	{
	}

	private bool Capture()
	{
		return false;
	}

	private void Restore()
	{
	}

	private static bool IsHorizontal(Slider slider)
	{
		return false;
	}
}
