using UnityEngine;

public class SectorSkinTarget : MonoBehaviour
{
	public int SectorId;

	public SkinSlotInfo[] SlotInfos;

	private MeshRenderer _targetRenderer;

	public MeshRenderer TargetRenderer
	{
		get
		{
			if ((object)_targetRenderer == null)
			{
				_targetRenderer = GetComponent<MeshRenderer>();
			}
			return _targetRenderer;
		}
	}
}
