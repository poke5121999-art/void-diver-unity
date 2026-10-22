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
		X = 0;
		Z = 0;
	}

	public Vector3 ToVector3()
	{
		return default;
	}

	private static ushort Quantize(float value)
	{
		return 0;
	}

	private static float Dequantize(ushort value)
	{
		return 0f;
	}

	public bool Equals(QuantAimDelta other)
	{
		return false;
	}

	public override bool Equals(object obj)
	{
		return false;
	}

	public override int GetHashCode()
	{
		return 0;
	}
}
