using System;
using Unity.Netcode;
using UnityEngine;

public struct PlanarPos : INetworkSerializeByMemcpy, IEquatable<PlanarPos>
{
	public float X;

	public float Z;

	public PlanarPos(Vector3 position)
	{
		X = position.x;
		Z = position.z;
	}

	public Vector3 ToVector3()
	{
		return new Vector3(X, 0f, Z);
	}

	public bool Equals(PlanarPos other)
	{
		return X == other.X && Z == other.Z;
	}

	public override bool Equals(object obj)
	{
		if (obj is PlanarPos other)
		{
			return X == other.X && Z == other.Z;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ (Z.GetHashCode() * 2);
	}
}
