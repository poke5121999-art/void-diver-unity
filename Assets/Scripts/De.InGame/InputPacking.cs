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
		float num = (float)Math.Sqrt(forward.x * forward.x + forward.y * forward.y);
		if (num < 0.0001f)
		{
			return 0u;
		}
		float num2 = (float)Math.Atan2(forward.y, forward.x);
		if (num2 < 0f)
		{
			num2 += (float)Math.PI * 2f;
		}
		uint num3 = (uint)Mathf.RoundToInt(num2 / ((float)Math.PI * 2f) * 256f);
		int num4 = Mathf.RoundToInt(num / 2f * 255f);
		num4 = Mathf.Clamp(num4, 1, 255);
		return ((num3 & 0xFF) << 8) | (uint)num4;
	}

	public static Vector2 UnpackForward(uint packed)
	{
		if (packed == 0)
		{
			return Vector2.zero;
		}
		float num = (float)((packed >> 8) & 0xFF) / 256f * ((float)Math.PI * 2f);
		float num2 = (float)(packed & 0xFF) / 255f * 2f;
		return new Vector2((float)Math.Cos(num) * num2, (float)Math.Sin(num) * num2);
	}

	public static uint PackAim(Vector3 worldAim)
	{
		if (worldAim == Vector3.zero)
		{
			return 0u;
		}
		return (uint)((QuantizePos(worldAim.x) << 16) | QuantizePos(worldAim.z));
	}

	public static bool TryUnpackAim(uint packed, out Vector3 worldAim)
	{
		if (packed == 0)
		{
			worldAim = Vector3.zero;
			return false;
		}
		worldAim = new Vector3(DequantizePos((ushort)(packed >> 16)), 0f, DequantizePos((ushort)(packed & 0xFFFF)));
		return true;
	}

	private static ushort QuantizePos(float value)
	{
		float num = (value - -64f) / 2048f;
		num = Mathf.Clamp01(num);
		return (ushort)Mathf.RoundToInt(num * 65535f);
	}

	private static float DequantizePos(ushort value)
	{
		return (float)(int)value / 65535f * 2048f + -64f;
	}
}
