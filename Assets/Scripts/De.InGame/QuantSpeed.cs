using System;
using Unity.Netcode;
using UnityEngine;

public struct QuantSpeed : INetworkSerializeByMemcpy, IEquatable<QuantSpeed>
{
	public byte Value;

	private const float MaxSpeed = 30f;

	public QuantSpeed(float speed)
	{
		if (speed < 0f)
		{
			Value = 0;
			return;
		}
		Value = (byte)Mathf.Clamp(Mathf.RoundToInt(speed / 30f * 254f) + 1, 1, 255);
	}

	public float ToFloat()
	{
		if (Value == 0)
		{
			return -1f;
		}
		return (float)(Value - 1) / 254f * 30f;
	}

	public bool Equals(QuantSpeed other)
	{
		return Value == other.Value;
	}

	public override bool Equals(object obj)
	{
		if (obj is QuantSpeed other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Value;
	}
}
