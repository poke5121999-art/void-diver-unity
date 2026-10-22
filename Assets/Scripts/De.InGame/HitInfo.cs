using System;
using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class HitInfo
{
	public ulong CasterNetworkObjectId;

	public EHitPointType HitPointType;

	public EDamageTagType DamageTagType;

	public string Vfx;

	public float VfxDuration;

	public string Sfx;

	public string CriticalSfx;

	public List<CollisionEventInfo> CollisionEventInfo;

	public Vector3 hitPosition;
}
