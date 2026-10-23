using System;
using Unity.Netcode;
using UnityEngine;

public struct PackedDir : INetworkSerializeByMemcpy, IEquatable<PackedDir>
{
	public byte Angle;

	private const float TwoPi = (float)Math.PI * 2f;

	public PackedDir(Vector3 dir)
	{
		// TODO(decomp): atan2/cos/sin are unnamed float intrinsics (@0x184A558A8/0x184A51AF0/0x184A533C0); mapping inferred from OnAimPad's easing

		if (dir.x * dir.x + dir.y * dir.y + dir.z * dir.z >= 1E-06f)
		{
			float num = Mathf.Atan2(dir.z, dir.x);
			if (num < 0f)
			{
				num += TwoPi;
			}
			Angle = (byte)(1 + Mathf.RoundToInt(num / TwoPi * 255f) % 255);
		}
		else
		{
			Angle = 0;
		}
	}

	public Vector3 ToVector3()
	{
		if (Angle == 0)
		{
			return Vector3.zero;
		}
		float f = (float)(Angle - 1) / 255f * TwoPi;
		return new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
	}

	public bool Equals(PackedDir other)
	{
		return Angle == other.Angle;
	}

	public override bool Equals(object obj)
	{
		if (obj is PackedDir other)
		{
			return Angle == other.Angle;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Angle;
	}
}
