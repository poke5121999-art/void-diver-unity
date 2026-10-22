using System;
using UnityEngine;

public static class InputPacking
{
	private const float TwoPi = (float)Math.PI * 2f;

	private const float ForwardMaxMag = 2f;

	private const float PosMin = -64f;

	private const float PosMax = 1984f;

	private const float PosRange = 2048f;

	public static uint PackForward(Vector2 forward)
	{
		return 0u;
	}

	public static Vector2 UnpackForward(uint packed)
	{
		return default;
	}

	public static uint PackAim(Vector3 worldAim)
	{
		return 0u;
	}

	public static bool TryUnpackAim(uint packed, out Vector3 worldAim)
	{
		worldAim = default;
		return false;
	}

	private static ushort QuantizePos(float value)
	{
		return 0;
	}

	private static float DequantizePos(ushort value)
	{
		return 0f;
	}
}
