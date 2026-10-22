using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class InGameMiniMapPanelPresenter : Presenter
{
	[View]
	private InGameMiniMapPanelView _view;

	[Inject]
	private MiniMapSettings _miniMapSettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameContext _gameContext;

	[SerializeField]
	[Range(0f, 10f)]
	private float _baseZoomScale;

	[SerializeField]
	[Range(0f, 1f)]
	private float _fullScreenRatio;

	private Vector2 _baseAnchorMin;

	private Vector2 _baseAnchorMax;

	private Vector2 _basePivot;

	private Vector2 _baseAnchoredPosition;

	private Vector2 _baseSizeDelta;

	private InGameMiniMapModel _miniMapModel;

	private void Start()
	{
	}

	private void Init()
	{
	}

	private void OnFullScreen(bool isFullScreen)
	{
	}
}
