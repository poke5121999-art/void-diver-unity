using GameCommon.Enum;
using UnityEngine;

public class BackgroundTileTypeResolver : MonoBehaviour
{
	public EGroundType GetCurrentGroundType()
	{
		return EGroundType.None;
	}

	public EGroundType GetGroundTypeAt(Vector3 worldPos)
	{
		return EGroundType.None;
	}
}
