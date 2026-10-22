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
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	public void SpawnText(EFloatingTextType textType, string text, Vector3 position, bool showFrontIcon = false, bool showBackwardIcon = false)
	{
		GameFloatingText gameFloatingText = _assetManager.SpawnFloatingText(textType);
		gameFloatingText.transform.SetParent(_root.transform);
		gameFloatingText.Init(text, position, showFrontIcon, showBackwardIcon);
	}
}
