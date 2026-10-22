using System;
using Unity.Netcode;
using UnityEngine;

public struct PackedDir : INetworkSerializeByMemcpy, IEquatable<PackedDir>
{
	public byte Angle;

	private const float TwoPi = (float)Math.PI * 2f;

	public PackedDir(Vector3 dir)
	{
		Angle = 0;
	}

	public Vector3 ToVector3()
	{
		return default;
	}

	public bool Equals(PackedDir other)
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
