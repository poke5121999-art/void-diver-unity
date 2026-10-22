using System;
using UnityEngine;

[Serializable]
public class BlockChiselCell
{
	[Tooltip("coarse 셀 중심 (Pivot 상대 좌표).")]
	public Vector3 cellCenter;

	[Tooltip("4x4=16비트 sub-grid 마스크. bit(sz*Resolution+sx)=해당 sub-cell 채움.")]
	public int subMask;

	public const int Resolution = 4;

	public const int FullMask = 65535;

	public bool IsEmpty => false;

	public bool IsFull => false;

	public static int BitIndex(int sx, int sz)
	{
		return 0;
	}

	public bool Get(int sx, int sz)
	{
		return false;
	}

	public void Set(int sx, int sz, bool value)
	{
	}
}
