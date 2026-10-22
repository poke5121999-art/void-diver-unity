using NemoLib.AssetManagement;
using UnityEngine;
using VContainer;

public class GameFloatingTextManager : MonoBase
{
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private GameObject _root;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private AssetManager _assetManager;

	protected override void Awake()
	{
	}

	public void SpawnText(EFloatingTextType textType, string text, Vector3 position, bool showFrontIcon = false, bool showBackwardIcon = false)
	{
	}
}
