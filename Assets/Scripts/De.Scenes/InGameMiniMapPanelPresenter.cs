using UniRx;
using GameCommon.Enum;
using System.Linq;
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
	private float _baseZoomScale = 2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float _fullScreenRatio = 1f;

	private Vector2 _baseAnchorMin;

	private Vector2 _baseAnchorMax;

	private Vector2 _basePivot;

	private Vector2 _baseAnchoredPosition;

	private Vector2 _baseSizeDelta;

	private InGameMiniMapModel _miniMapModel;

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState state) => state == EStageState.LoadCompleted || state == EStageState.Playing || state == EStageState.Intro).Take(1).Subscribe(delegate
		{
			Init();
		}).AddTo(this);
	}

	private void Init()
	{
		RectTransform component = _view.GetComponent<RectTransform>();
		_baseAnchorMin = component.anchorMin;
		_baseAnchorMax = component.anchorMax;
		_basePivot = component.pivot;
		_baseAnchoredPosition = component.anchoredPosition;
		_baseSizeDelta = component.sizeDelta;
		_view.Viewport.transform.localScale = Vector3.one * _baseZoomScale;
		_miniMapModel = new InGameMiniMapModel
		{
			MaskRadius = _view.Viewport.GetComponent<RectTransform>().sizeDelta.x * 0.5f,
			BaseZoomScale = _baseZoomScale,
			FullScreenRatio = _fullScreenRatio
		};
		_view.MinMap.Inject(_miniMapModel);
		_mapManager.RxMapLayer.Subscribe(delegate
		{
			if (_miniMapModel.RxIsFullScreen.Value)
			{
				_view.Viewport.transform.localScale = Vector3.one * _miniMapModel.FullScreenZoomScale;
			}
		}).AddTo(this);
		_mapManager.RxIsFullScreen.Subscribe(OnFullScreen).AddTo(this);
	}

	private void OnFullScreen(bool isFullScreen)
	{
		if (_gameManager.IsInLounge)
		{
			base.gameObject.SetActive(isFullScreen);
			if (!isFullScreen)
			{
				return;
			}
		}
		if (_miniMapModel == null)
		{
			return;
		}
		RectTransform component = _view.GetComponent<RectTransform>();
		if (isFullScreen)
		{
			_view.Viewport.transform.localScale = Vector3.one * _miniMapModel.FullScreenZoomScale;
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			_view.Mask.enabled = false;
			_view.Frame.gameObject.SetActive(false);
			_view.Info.SetActive(true);
			for (int i = 0; i < _view.StatusCards.Length; i++)
			{
				bool flag = _gameContext.Players.Values.Any((GamePlayer p) => p.PlayerIndex == i);
				_view.StatusCards[i].SetActive(flag);
				if (flag)
				{
					_view.StatusCards[i].GetComponent<PlayerStatusCardPresenter>().Inject(new PlayerStatusCardModel
					{
						PlayerIndex = i
					});
				}
			}
		}
		else
		{
			_view.Viewport.transform.localScale = Vector3.one * _baseZoomScale;
			component.anchorMin = _baseAnchorMin;
			component.anchorMax = _baseAnchorMax;
			component.pivot = _basePivot;
			component.anchoredPosition = _baseAnchoredPosition;
			component.sizeDelta = _baseSizeDelta;
			_view.Mask.enabled = true;
			_view.Frame.gameObject.SetActive(true);
			_view.Info.SetActive(false);
		}
		_miniMapModel.RxIsFullScreen.Value = isFullScreen;
		_soundManager.PlaySfx("MapRolling");
	}
}
