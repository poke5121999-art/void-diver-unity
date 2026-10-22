using System;
using Unity.Netcode;
using UnityEngine;

public struct PlanarPos : INetworkSerializeByMemcpy, IEquatable<PlanarPos>
{
	public float X;

	public float Z;

	public PlanarPos(Vector3 position)
	{
		X = 0f;
		Z = 0f;
	}

	public Vector3 ToVector3()
	{
		return default;
	}

	public bool Equals(PlanarPos other)
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
