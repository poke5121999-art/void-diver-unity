using DG.Tweening;
using System;
using DarkTonic.MasterAudio;
using GameCommon.Data;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using GameCommon.Table;
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
		_canvas = _view.GetComponentInParent<Canvas>();
		_canvasScaler = _canvas.GetComponent<CanvasScaler>();
		_rectTransform = _view.GetComponent<RectTransform>();
		_gameManager.RxViewTargetClientId.Where((ulong id) => id != ulong.MaxValue).Subscribe(OnViewTargetClientIdChanged).AddTo(this);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
		_tweener.Kill();
	}

	private void Update()
	{
		if (_model == null || _model.InteractiveObject == null || !_objectManager.IsInteractiveObjectExist(_model.InteractiveObject.NetworkObjectId) || _gameManager.StageState != EStageState.Playing)
		{
			return;
		}
		Camera mainCamera = _cameraManager.MainCamera;
		Vector3 position = _model.InteractiveObject.Position;
		Vector2 screenPoint = mainCamera.WorldToScreenPoint(position);
		Canvas overlayCanvas = _popupManager.OverlayCanvas;
		RectTransform component = _view.Pivot.GetComponent<RectTransform>();
		RectTransform rect = overlayCanvas.transform as RectTransform;
		Camera cam = ((overlayCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : overlayCanvas.worldCamera);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, cam, out var localPoint);
		component.anchoredPosition = localPoint;
	}

	private void OnViewTargetClientIdChanged(ulong targetClientId)
	{
		CharacterModel characterModel = _gameManager.FindCharacterModel(targetClientId);
		if (characterModel == null)
		{
			_logger.Warning($"targetClientId {targetClientId} is not found in PlayerCharacters");
			return;
		}
		_model.CharacterModel = characterModel;
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_tweener.Kill();
		InitInteraction();
	}

	private void InitInteraction()
	{
		_model.CharacterModel.GetComponent<CharacterView>().RxInteractiveObject.Subscribe(OnInteractableObjectChanged).AddTo(_disposable);
		_model.CharacterModel.RxState.Pairwise().Subscribe(OnStateChanged).AddTo(_disposable);
	}

	private void OnStateChanged(Pair<EActionState> pair)
	{
		if (pair.Previous == pair.Current)
		{
			return;
		}
		_tweener?.Kill();
		_view.GaugeFill.fillAmount = 0f;
		if (pair.Current == EActionState.Interact && _model.InteractiveObject != null)
		{
			float holdingTime = _model.InteractiveObject.HoldingTime;
			_tweener = _view.GaugeFill.DOFillAmount(1f, holdingTime).SetEase(Ease.Linear).SetLink(_view.GaugeFill.gameObject);
			string interactionSfx = GetInteractionSfx(_model.InteractiveObject);
			if (!string.IsNullOrEmpty(interactionSfx))
			{
				_interactionSoundResult = _soundManager.PlaySfx(interactionSfx);
			}
			return;
		}
		if (pair.Previous != EActionState.Interact || _model.InteractiveObject == null)
		{
			_interactionSoundResult?.StopSound();
			_interactionSoundResult = null;
		}
		UpdateUi();
	}

	private void OnInteractableObjectChanged(IInteractiveObject interactiveObject)
	{
		if (interactiveObject == null)
		{
			_model.InteractiveObject = null;
			_view.Pivot.SetActive(value: false);
		}
		else
		{
			_model.InteractiveObject = interactiveObject;
			_view.Pivot.SetActive(value: true);
			UpdateUi();
		}
	}

	private void UpdateUi()
	{
		IInteractiveObject interactiveObject = _model.InteractiveObject;
		if (interactiveObject is DropGoods dropGoods)
		{
			_view.GoodsCount.SetActive(value: true);
			_view.GoodsCount_Text.text = $"x{dropGoods.GoodsCount}";
		}
		else
		{
			_view.GoodsCount.SetActive(value: false);
		}
		string title;
		switch (_model.InteractiveObject)
		{
		case DisposableObject:
			title = TLocalizedText.DestructibleObject.LocalText;
			break;
		case CharacterBody characterBody:
			title = characterBody.GetComponentInParent<CharacterModel>().PlayerName ?? "";
			break;
		case DropGoods dropGoods2:
			title = dropGoods2.DisplayGoods.GetName();
			break;
		case WaveExit waveExit:
			// vtable+0x3f8 is slot 44 = ExitObject.get_ExitState (EExitState is a byte enum, result tested with test al,al; zero (Deactivated) gives Elara) @0x180815831
			title = ((waveExit.ExitState == EExitState.Deactivated) ? TLocalizedText.Elara : TLocalizedText.DivingBooth).LocalText;
			break;
		case MonsterBody monsterBody:
			title = GetMonsterTitle(monsterBody);
			break;
		case NpcObject:
			title = "";
			break;
		case Portal:
			title = TLocalizedText.Portal.LocalText;
			break;
		case RewardBox rewardBox:
			title = GetRewardBoxTitle(rewardBox);
			break;
		case MimicObject mimicObject:
			title = TMimic.Table[mimicObject.MimicId].Name ?? "";
			break;
		case Entrance entrance:
			title = TEntrance.Table[entrance.EntranceId].Name ?? "";
			break;
		case SkillExecutor skillExecutor:
			title = TSkillExecutor.Table[skillExecutor.SkillExecutorId].Name ?? "";
			break;
		default:
			title = "";
			break;
		}
		_view.TitleText.text = title;
		_view.GaugeFill.fillAmount = 0f;
		TLocalizedText tLocalizedText;
		switch (_model.InteractiveObject)
		{
		case null:
			tLocalizedText = TLocalizedText.Interact;
			break;
		case WaveExit waveExit2:
			tLocalizedText = ((waveExit2.ExitState == EExitState.Deactivated) ? TLocalizedText.CallDivingBooth : TLocalizedText.Escape);
			break;
		case SafeExit:
			tLocalizedText = TLocalizedText.Escape;
			break;
		case DropGoods:
			tLocalizedText = TLocalizedText.Pickup;
			break;
		case MonsterBody:
			tLocalizedText = TLocalizedText.Inspect;
			break;
		case CharacterBody characterBody2:
			tLocalizedText = ((characterBody2.State == EActionState.Dead) ? TLocalizedText.Rescue : TLocalizedText.MindSync);
			break;
		case RewardBox:
			tLocalizedText = TLocalizedText.Inspect;
			break;
		case MimicObject:
			tLocalizedText = TLocalizedText.Inspect;
			break;
		case Entrance entrance2:
			tLocalizedText = ((entrance2.LockState == ELockState.Locked || entrance2.LockState == ELockState.Closed) ? TLocalizedText.Open : TLocalizedText.Close);
			break;
		case DisposableObject:
			tLocalizedText = TLocalizedText.Break;
			break;
		case PhoneBooth:
			tLocalizedText = ((_model.CharacterModel.State == EActionState.StageReady) ? TLocalizedText.StopDeparture : TLocalizedText.Board);
			break;
		default:
			tLocalizedText = TLocalizedText.Interact;
			break;
		}
		_view.DescText.text = tLocalizedText.LocalText;

		static string GetMonsterTitle(MonsterBody monsterBody)
		{
			string text = TLocalizedText.DeadMonsterFormat.Format(TMonster.Table[monsterBody.GetComponentInParent<MonsterModel>().MonsterId].Name);
			if (monsterBody.IsAllRevealed())
			{
				text = text + " (" + TLocalizedText.Revealed.LocalText + ")";
			}
			return text;
		}

		static string GetRewardBoxTitle(RewardBox rewardBox)
		{
			string name = TRewardBox.Table[rewardBox.RewardBoxId].Name;
			if (rewardBox.LockState == ELockState.Opened && rewardBox.IsAllRevealed())
			{
				name = name + " (" + TLocalizedText.Revealed.LocalText + ")";
			}
			return name;
		}
	}

	private static string GetInteractionSfx(IInteractiveObject obj)
	{
		if (obj is WaveExit)
		{
			return "InteractionEscape";
		}
		if (obj is DropGoods || obj is MonsterBody || obj is RewardBox)
		{
			return "InteractionLooting";
		}
		return string.Empty;
	}
}
