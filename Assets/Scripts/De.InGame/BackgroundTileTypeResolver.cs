using GameCommon.Enum;
using UnityEngine;

public class BackgroundTileTypeResolver : MonoBehaviour
{
	public EGroundType GetCurrentGroundType()
	{
		return BackgroundTileTypeCore.GetGroundType(base.transform.position);
	}

	public EGroundType GetGroundTypeAt(Vector3 worldPos)
	{
		return BackgroundTileTypeCore.GetGroundType(worldPos);
	}
}
