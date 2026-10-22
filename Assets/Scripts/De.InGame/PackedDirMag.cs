using System;
using Unity.Netcode;
using UnityEngine;

public struct PackedDirMag : INetworkSerializeByMemcpy, IEquatable<PackedDirMag>
{
	public byte Angle;

	public byte Magnitude;

	private const float TwoPi = (float)Math.PI * 2f;

	private const float MaxMag = 2f;

	public PackedDirMag(Vector3 dir)
	{
		Angle = 0;
		Magnitude = 0;
	}

	public Vector3 ToVector3()
	{
		return default;
	}

	public bool Equals(PackedDirMag other)
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
