using UnityEngine;

public class SectorSkinTarget : MonoBehaviour
{
	public int SectorId;

	public SkinSlotInfo[] SlotInfos;

	private MeshRenderer _targetRenderer;

	public MeshRenderer TargetRenderer => null;
}
