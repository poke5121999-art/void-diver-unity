using GameCommon.Enum;
using UniRx;
using UnityEngine;
using VContainer;

public class MapVisualBoundManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameMapManager _gameMapManager;

	private Vector3 _boundMin;

	private Vector3 _boundMax;

	private static readonly int _overallMapBoundMaxId = Shader.PropertyToID("_GlobalOverallMapBoundMax");

	private static readonly int _overallMapBoundMinId = Shader.PropertyToID("_GlobalOverallMapBoundMin");

	private static readonly int _globalSectorWidthId = Shader.PropertyToID("_GlobalSectorWidth");

	private static readonly int _globalSectorHeightId = Shader.PropertyToID("_GlobalSectorHeight");

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
		{
			InitializeMapBound();
		}).AddTo(this);
	}

	private void InitializeMapBound()
	{
		if (_gameMapManager == null || _gameMapManager.SectorIds.Length == 0)
		{
			return;
		}
		Vector3 sectorOffset = _gameMapManager.GetSectorOffset(0);
		_boundMax = new Vector3(sectorOffset.x + 30f, sectorOffset.y + 0f, sectorOffset.z + 30f);
		_boundMin = _gameMapManager.GetSectorOffset(0);
		for (int i = 1; i < _gameMapManager.SectorIds.Length; i++)
		{
			Vector3 sectorOffset2 = _gameMapManager.GetSectorOffset(i);
			_boundMax = new Vector3(Mathf.Max(_boundMax.x, sectorOffset2.x + 30f), Mathf.Max(_boundMax.y, sectorOffset2.y + 0f), Mathf.Max(_boundMax.z, sectorOffset2.z + 30f));
			Vector3 sectorOffset3 = _gameMapManager.GetSectorOffset(i);
			_boundMin = new Vector3(Mathf.Min(_boundMin.x, sectorOffset3.x), Mathf.Min(_boundMin.y, sectorOffset3.y), Mathf.Min(_boundMin.z, sectorOffset3.z));
		}
		Shader.SetGlobalVector(_overallMapBoundMaxId, _boundMax);
		Shader.SetGlobalVector(_overallMapBoundMinId, _boundMin);
		Shader.SetGlobalFloat(_globalSectorWidthId, 30f);
		Shader.SetGlobalFloat(_globalSectorHeightId, 30f);
	}
}
