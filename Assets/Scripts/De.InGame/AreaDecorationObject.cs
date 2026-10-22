using NemoLib.AssetManagement;
using NemoLib.Log;
using UnityEngine;
using VContainer;

public class AreaDecorationObject : MonoBase
{
	[Inject]
	private GameLoungeManager _gameLoungeManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	private int _groupId;

	private string[] _prefabNames;

	private bool _isFlipped;

	private GameObject _currentChild;

	public void Init(int groupId, string[] prefabNames, bool isFlipped)
	{
	}

	private void Apply(int areaDecorationSettingId)
	{
	}

	private void DespawnCurrent()
	{
	}
}
