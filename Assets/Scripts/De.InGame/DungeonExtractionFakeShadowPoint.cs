using UnityEngine;
using UnityEngine.Serialization;

public class DungeonExtractionFakeShadowPoint : FakeShadowPoint
{
	[Header("[Deprecated] PlaneSystem Setting")]
	[Tooltip("FakeShadowPoint._usingPlaneSystemEnable로 마이그레이션 필요.")]
	[FormerlySerializedAs("_usingPlaneSystemEnable")]
	[SerializeField]
	private bool _legacyUsingPlaneSystemEnable;
}
