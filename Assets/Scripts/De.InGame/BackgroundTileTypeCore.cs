using System.Collections.Generic;
using GameCommon.Enum;
using UnityEngine;
using VContainer;

public class BackgroundTileTypeCore : MonoBase
{
	public static BackgroundTileTypeCore Instance;

	[Inject]
	private GameManager _gameManager;

	private readonly List<(Vector3 offset, GroundTypeData data)> _sectors;

	private readonly Dictionary<long, EGroundType> _cache;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnDestroy()
	{
	}

	public static EGroundType GetGroundType(Vector3 worldPos)
	{
		return EGroundType.None;
	}

	private EGroundType GetGroundTypeCached(Vector3 worldPos)
	{
		return EGroundType.None;
	}

	private EGroundType ResolveGroundType(Vector3 worldPos)
	{
		return EGroundType.None;
	}
}
