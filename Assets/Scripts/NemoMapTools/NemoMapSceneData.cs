using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class NemoMapSceneData : MonoBehaviour
{
	[HideInInspector]
	[Tooltip("이 씬의 길찾기 방해(Blocking Area) 영역 좌표 목록입니다.")]
	public List<Vector3> blockingAreaPositions = new List<Vector3>();

	[HideInInspector]
	[Tooltip("B 영역 좌표를 Pivot 기준 상대 좌표로 저장할지 여부입니다. (원점 단일화: 항상 true로 수렴)")]
	public bool useRelativeBlockCoordinates;

	[HideInInspector]
	[Tooltip("치즐된 B 영역 셀 목록입니다. 한 셀을 4x4 sub-grid로 쪼개 부분 채움(마인크래프트 치즐). full 셀은 blockingAreaPositions에 유지.")]
	public List<BlockChiselCell> blockingChiselCells = new List<BlockChiselCell>();

	[HideInInspector]
	[Tooltip("이 씬의 VisualArea 영역 좌표 목록입니다.")]
	[FormerlySerializedAs("waterAreaPositions")]
	public List<Vector3> areaPositions = new List<Vector3>();

	[HideInInspector]
	[Tooltip("이 씬의 물 바닥면(Water Bottom) 영역 좌표 목록입니다.")]
	public List<Vector3> waterBottomPositions = new List<Vector3>();

	[HideInInspector]
	[Tooltip("물 바닥면의 Y 높이입니다.")]
	public float waterBottomHeight = -0.5f;

	[HideInInspector]
	[Tooltip("이 씬의 모든 레벨(층)에 대한 구조적 데이터 목록입니다.")]
	public List<LevelInfo> levelDataList = new List<LevelInfo>();

	[HideInInspector]
	[Tooltip("레벨 목록의 UI 표시 순서를 저장합니다.")]
	public List<int> levelDisplayOrder = new List<int>();

	[HideInInspector]
	[SerializeField]
	private MapEditorPivotMarker _mapEditorPivotMarker;

	public MapEditorPivotMarker MapEditorPivotMarker => _mapEditorPivotMarker;

	public void InitializeMapData()
	{
		MapEditorDefines.DefaultLevelInfos.Where((LevelInfo _) => _ != null).ToList().ForEach(delegate(LevelInfo _)
		{
			levelDataList.Add(_);
		});
		InitializePivotIfNeeded();
	}

	public void InitializePivotIfNeeded()
	{
		if (!(_mapEditorPivotMarker != null))
		{
			GameObject gameObject = new GameObject(MapEditorDefines.MapPivotName);
			_mapEditorPivotMarker = gameObject.AddComponent<MapEditorPivotMarker>();
			_mapEditorPivotMarker.gameObject.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			_mapEditorPivotMarker.gameObject.transform.position = new Vector3(-0.5f, 0f, -0.5f);
		}
	}

	public void InitializePivotIfNeeded(Bounds worldBound)
	{
		if (!(_mapEditorPivotMarker != null))
		{
			float x = worldBound.center.x - worldBound.extents.x;
			float z = worldBound.center.z - worldBound.extents.z;
			GameObject gameObject = new GameObject(MapEditorDefines.MapPivotName);
			_mapEditorPivotMarker = gameObject.AddComponent<MapEditorPivotMarker>();
			_mapEditorPivotMarker.gameObject.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			_mapEditorPivotMarker.gameObject.transform.position = new Vector3(x, 0f, z);
		}
	}
}
