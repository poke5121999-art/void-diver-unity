using System;
using Unity.Netcode;

public struct QuantSpeed : INetworkSerializeByMemcpy, IEquatable<QuantSpeed>
{
	public byte Value;

	private const float MaxSpeed = 30f;

	public QuantSpeed(float speed)
	{
		Value = 0;
	}

	public float ToFloat()
	{
		return 0f;
	}

	public bool Equals(QuantSpeed other)
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
