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

	private static readonly int _overallMapBoundMaxId;

	private static readonly int _overallMapBoundMinId;

	private static readonly int _globalSectorWidthId;

	private static readonly int _globalSectorHeightId;

	private void Start()
	{
	}

	private void InitializeMapBound()
	{
	}
}
