using UnityEngine;

public class MapEditorObjectInfo : MonoBehaviour
{
	public enum PlacementType
	{
		Grid = 0,
		Prop = 1
	}

	[Header("Placement Information")]
	[Tooltip("이 객체가 어떻게 배치되었는지 나타냅니다 (Grid 또는 Prop).")]
	public PlacementType placementType;

	[HideInInspector]
	[Header("Level Information")]
	[Tooltip("이 객체가 속한 레벨의 인덱스.")]
	public int levelIndex;

	[Tooltip("이 객체가 배치될 당시 레벨의 이름입니다.")]
	public string sourceLevelName = "";

	[Tooltip("이 객체가 배치될 당시의 완전한 레벨 설정값(이름, 높이, 오프셋)을 가지고 있는지 여부입니다.")]
	public bool hasFullLevelContext;

	[Tooltip("이 객체가 배치될 당시 레벨의 Y 높이(baseHeight)입니다.")]
	public float sourceLevelBaseHeight;

	[Tooltip("이 객체가 배치될 당시 레벨의 X 오프셋입니다.")]
	public float sourceLevelXOffset;

	[Tooltip("이 객체가 배치될 당시 레벨의 Z 오프셋입니다.")]
	public float sourceLevelZOffset;

	[Header("Transform State")]
	[Tooltip("이 Prop 객체가 현재 좌우 반전된 상태인지 여부 (Prop 전용).")]
	public bool isFlipped;

	[Header("Grid Specific Info (If PlacementType is Grid)")]
	[Tooltip("객체가 원래 배치된 레이어 (Grid 배치 전용).")]
	public PrefabLayer originalLayer;

	[Tooltip("이 객체가 파생된 RuleTileSet의 이름 (Grid 배치 전용, 없으면 비어 있음).")]
	public string sourceRuleSetName = "";

	[Tooltip("이 객체가 파생된 SingleTileSet의 이름 (Grid 배치 전용, 없으면 비어 있음).")]
	public string sourceSingleTileSetName = "";

	[Tooltip("이 객체가 SingleTileSet의 Fill 타일인지 여부 (Grid 배치 전용).")]
	public bool isSingleTileFill;

	[Tooltip("객체가 배치된 레벨 내에서의 상대 그리드 좌표 (XZ 평면, Y=0). 추출 시 인접성 검사에 사용됩니다.")]
	public Vector3 gridPositionXZ = Vector3.zero;

	[Header("Source Asset")]
	[Tooltip("이 객체의 원본 프리팹 에셋 경로 (오브젝트 피커에서 사용).")]
	public string sourcePrefabAssetPath = "";

	[Header("Extraction Settings")]
	[Tooltip("맵 추출 시 바운더리 계산에서 이 객체를 제외합니다.")]
	public bool excludeFromBoundsOnExtract;
}
