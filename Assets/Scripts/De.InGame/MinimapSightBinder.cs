using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
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
		base.Awake();
		if (_core == null)
		{
			_core = GetComponent<MinimapSightCore>();
		}
	}

	private void Start()
	{
		if (!(_core == null) && !(_gameManager == null) && !(_gameMapManager == null))
		{
			_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
			{
				OnStageLoadCompleted();
			})
				.AddTo(this);
			_gameMapManager.RxMapLayer.Subscribe(OnMapLayerChanged).AddTo(this);
			_gameManager.RxIsRevealingMap.Subscribe(OnRevealingMapChanged).AddTo(this);
		}
	}

	private void OnStageLoadCompleted()
	{
		if (!(_gameMapManager == null) && _gameMapManager.SectorIds.Length != 0)
		{
			int num = _gameMapManager.SectorIds.Length;
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _gameMapManager.GetSectorOffset(i);
			}
			(int Width, int Height) mapSize = _gameMapManager.MapSize;
			(int Width, int Height) mapSize2 = _gameMapManager.MapSize;
			CalculateBound(array, out var boundMin, out var boundMax);
			_core.BindMapData(array, mapSize.Width, mapSize2.Height, boundMin, boundMax);
			_core.BindLayer(_gameMapManager.MapLayer, ResolveFixedSectors());
			_core.BindRevealing(_gameManager.IsInStage, _gameManager.RxIsRevealingMap.Value);
		}
	}

	private void OnMapLayerChanged(int mapLayer)
	{
		if (!(_core == null))
		{
			_core.BindLayer(mapLayer, ResolveFixedSectors());
		}
	}

	private void OnRevealingMapChanged(bool isRevealing)
	{
		if (!(_core == null) && !(_gameManager == null))
		{
			_core.BindRevealing(_gameManager.IsInStage, isRevealing);
		}
	}

	private void CalculateBound(Vector3[] sectorOffsets, out Vector3 boundMin, out Vector3 boundMax)
	{
		boundMin = sectorOffsets[0];
		boundMax = sectorOffsets[0] + new Vector3(30f, 0f, 30f);
		for (int i = 1; i < sectorOffsets.Length; i++)
		{
			boundMax = Vector3.Max(boundMax, sectorOffsets[i] + new Vector3(30f, 0f, 30f));
			boundMin = Vector3.Min(boundMin, sectorOffsets[i]);
		}
	}

	private FixedSector[] ResolveFixedSectors()
	{
		if (_gameContext == null || _gameContext.CampaignId < 1)
		{
			return null;
		}
		if (!TCampaign.Table.ContainsKey(_gameContext.CampaignId))
		{
			return null;
		}
		return TCampaign.Table[_gameContext.CampaignId].FixedSectors;
	}

	private bool IsMapManagerValid()
	{
		if (_gameMapManager == null)
		{
			return false;
		}
		return _gameMapManager.SectorIds.Length != 0;
	}
}
