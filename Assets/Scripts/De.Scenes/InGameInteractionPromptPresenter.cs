using DG.Tweening;
using DarkTonic.MasterAudio;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InGameInteractionPromptPresenter : Presenter
{
	[View]
	private InGameInteractionPromptView _view;

	[Model]
	private InGameInteractionPromptModel _model;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private RoomContext _roomContext;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	private Tweener _tweener;

	private PlaySoundResult _interactionSoundResult;

	private Canvas _canvas;

	private CanvasScaler _canvasScaler;

	private RectTransform _rectTransform;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
	}

	private void InitInteraction()
	{
	}

	private void OnStateChanged(Pair<EActionState> pair)
	{
	}

	private void OnInteractableObjectChanged(IInteractiveObject interactiveObject)
	{
	}

	private void UpdateUi()
	{
	}

	private static string GetInteractionSfx(IInteractiveObject obj)
	{
		return null;
	}
}
