using GameCommon.Data;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(MinimapSightCore))]
public class MinimapSightBinder : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameMapManager _gameMapManager;

	[Inject]
	private GameContext _gameContext;

	[SerializeField]
	private MinimapSightCore _core;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnMapLayerChanged(int mapLayer)
	{
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
	}

	private void CalculateBound(Vector3[] sectorOffsets, out Vector3 boundMin, out Vector3 boundMax)
	{
		boundMin = default;
		boundMax = default;
	}

	private FixedSector[] ResolveFixedSectors()
	{
		return null;
	}

	private bool IsMapManagerValid()
	{
		return false;
	}
}
