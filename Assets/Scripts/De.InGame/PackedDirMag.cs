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
		float magnitude = (float)Math.Sqrt(dir.y * dir.y + dir.x * dir.x + dir.z * dir.z);
		if (magnitude < 0.0001f)
		{
			Angle = 0;
			Magnitude = 0;
			return;
		}
		float num = Mathf.Atan2(dir.z, dir.x);
		if (num < 0f)
		{
			num += TwoPi;
		}
		Angle = (byte)Mathf.RoundToInt(num / TwoPi * 256f);
		Magnitude = (byte)Mathf.Clamp(Mathf.RoundToInt(magnitude / MaxMag * 255f), 1, 255);
	}

	public Vector3 ToVector3()
	{
		if (Magnitude == 0)
		{
			return Vector3.zero;
		}
		float f = (float)(int)Angle * 0.00390625f * TwoPi;
		float num = (float)(int)Magnitude / 255f * MaxMag;
		return new Vector3(Mathf.Cos(f) * num, 0f, Mathf.Sin(f) * num);
	}

	public bool Equals(PackedDirMag other)
	{
		return Angle == other.Angle && Magnitude == other.Magnitude;
	}

	public override bool Equals(object obj)
	{
		if (obj is PackedDirMag other)
		{
			return Angle == other.Angle && Magnitude == other.Magnitude;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Angle | (Magnitude << 8);
	}
}
