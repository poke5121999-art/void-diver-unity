using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;
using UniRx;
using VContainer;

public class BackgroundTileTypeCore : MonoBase
{
	public static BackgroundTileTypeCore Instance;

	[Inject]
	private GameManager _gameManager;

	private readonly List<(Vector3 offset, GroundTypeData data)> _sectors = new List<(Vector3 offset, GroundTypeData data)>();

	private readonly Dictionary<long, EGroundType> _cache = new Dictionary<long, EGroundType>();

	protected override void Awake()
	{
		base.Awake();
		if (Instance != null && Instance != this)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(gameObject);
			}
			else
			{
				Object.DestroyImmediate(gameObject);
			}
			return;
		}
		Instance = this;
	}

	private void Start()
	{
		if (_gameManager != null)
		{
			_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.LoadCompleted).Subscribe(delegate
			{
				OnStageLoadCompleted();
			}).AddTo(this);
		}
	}

	private void OnStageLoadCompleted()
	{
		_sectors.Clear();
		_cache.Clear();
		GroundTypeData[] array = Object.FindObjectsByType<GroundTypeData>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			GroundTypeData groundTypeData = array[i];
			if (groundTypeData.groundTypeGrid != null && groundTypeData.groundTypeGrid.Length != 0)
			{
				_sectors.Add((groundTypeData.transform.position, groundTypeData));
			}
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public static EGroundType GetGroundType(Vector3 worldPos)
	{
		if (Instance == null)
		{
			return EGroundType.None;
		}
		return Instance.GetGroundTypeCached(worldPos);
	}

	private EGroundType GetGroundTypeCached(Vector3 worldPos)
	{
		int cellX = (int)System.Math.Floor(worldPos.x);
		int cellZ = (int)System.Math.Floor(worldPos.z);
		long key = ((long)cellX << 32) | (uint)cellZ;
		if (!_cache.TryGetValue(key, out var value))
		{
			value = ResolveGroundType(worldPos);
			_cache[key] = value;
		}
		return value;
	}

	private EGroundType ResolveGroundType(Vector3 worldPos)
	{
		for (int i = 0; i < _sectors.Count; i++)
		{
			var (offset, data) = _sectors[i];
			float dx = worldPos.x - offset.x;
			float dz = worldPos.z - offset.z;
			int cellX = (int)System.Math.Floor(dx / data.gridSize) - data.gridMinX;
			int cellZ = (int)System.Math.Floor(dz / data.gridSize) - data.gridMinZ;
			if (cellX >= 0 && cellX < data.gridWidth && cellZ >= 0 && cellZ < data.gridHeight)
			{
				EGroundType groundType = (EGroundType)data.groundTypeGrid[cellZ * data.gridWidth + cellX];
				if (groundType != EGroundType.None)
				{
					return groundType;
				}
			}
		}
		return EGroundType.None;
	}
}
