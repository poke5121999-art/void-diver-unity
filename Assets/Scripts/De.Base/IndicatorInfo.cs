using GameCommon.Table;
using System.Text;
using System;
using GameCommon.Enum;
using UnityEngine;

[Serializable]
public class IndicatorInfo
{
	public EIndicatorVfxType IndicatorVfxType;

	public float CircleRadius;

	public Vector2 SquareSize;

	public float ConeRadius;

	public float ConeAngle;

	public Vector3 SpawnOffset;

	public float ShowDuration;

	public float FillDuration;

	public bool IsIndependent;

	public bool IsCancelOnOwnerDie;

	public int HitBoxId;

	public float HitBoxSpawnDelay;

	public bool SpawnHitBoxAtOwnerPosition;

	public bool Validate(out string errorMessage)
	{
		errorMessage = null;
		StringBuilder stringBuilder = new StringBuilder();
		bool result = true;
		if (HitBoxId > 0 && !THitBox.Table.ContainsKey(HitBoxId))
		{
			result = false;
			stringBuilder.AppendLine(string.Format("{0} HitBoxId [{1}] is not exist in  {2}", "IndicatorInfo", HitBoxId, "THitBox"));
		}
		errorMessage = stringBuilder.ToString();
		return result;
	}
}
