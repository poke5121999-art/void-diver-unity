using System;
using Unity.Netcode;
using UnityEngine;

public struct QuantAimDelta : INetworkSerializeByMemcpy, IEquatable<QuantAimDelta>
{
	public ushort X;

	public ushort Z;

	private const float Min = -512f;

	private const float Max = 512f;

	private const float Range = 1024f;

	public QuantAimDelta(Vector3 delta)
	{
		X = Quantize(delta.x);
		Z = Quantize(delta.z);
	}

	public Vector3 ToVector3()
	{
		return new Vector3(Dequantize(X), 0f, Dequantize(Z));
	}

	private static ushort Quantize(float value)
	{
		return (ushort)Mathf.RoundToInt(Mathf.Clamp01((value - -512f) / 1024f) * 65535f);
	}

	private static float Dequantize(ushort value)
	{
		return (float)(int)value / 65535f * 1024f - 512f;
	}

	public bool Equals(QuantAimDelta other)
	{
		return X == other.X && Z == other.Z;
	}

	public override bool Equals(object obj)
	{
		if (obj is QuantAimDelta other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return X | (Z << 16);
	}
}
