using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using SomeNet.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InGameMiniMapPresenter : Presenter
{
	private class PlayerSight
	{
		public CharacterModel CharacterModel;

		public PlaneSightUnit PlaneSightUnit;

		public Vector3 Position => CharacterModel.Position;

		public Vector3 Forward => CharacterModel.Forward;

		public float SightRange => PlaneSightUnit.SightRange;

		public float SightAngle => PlaneSightUnit.SightAngle;

		public float SightBackRange => PlaneSightUnit.SightBackRange;
	}

	private class MiniMapMarker
	{
		public readonly ulong NetworkObjectId;

		public readonly int CustomId;

		public Vector3 TargetPosition;

		public readonly GameObject MarkerGo;

		public readonly GameObject IndicatorGo;

		public int MapLayer;

		public bool ApplyDirection;

		public bool IsFound;

		public readonly CompositeDisposable Disposable;

		public int CampaignTaskId;

		public Transform MarkerTf => MarkerGo.transform;

		public Transform IndicatorTf => IndicatorGo.transform;

		public bool IsMarkerActive => MarkerGo.activeSelf;

		public bool IsIndicatorActive
		{
			get
			{
				if (IndicatorGo == null)
				{
					return false;
				}
				return IndicatorGo.activeSelf;
			}
		}

		public MiniMapMarker(ulong networkObjectId, GameObject markerGo, Vector3 targetPosition, GameObject indicatorGo = null)
		{
			Disposable = new CompositeDisposable();
			NetworkObjectId = networkObjectId;
			MarkerGo = markerGo;
			TargetPosition = targetPosition;
			IndicatorGo = indicatorGo;
		}

		public MiniMapMarker(int customId, GameObject markerGo, Vector3 targetPosition, GameObject indicatorGo = null)
		{
			Disposable = new CompositeDisposable();
			CustomId = customId;
			MarkerGo = markerGo;
			TargetPosition = targetPosition;
			IndicatorGo = indicatorGo;
		}

		public void SetActiveMarker(bool active)
		{
			if (MarkerGo.activeSelf != active)
			{
				MarkerGo.SetActive(active);
			}
		}

		public void SetActiveIndicator(bool active)
		{
			if (!(IndicatorGo == null) && IndicatorGo.activeSelf != active)
			{
				IndicatorGo.SetActive(active);
			}
		}

		public void SetActive(bool active)
		{
			if (MarkerGo.activeSelf != active)
			{
				MarkerGo.SetActive(active);
			}
			SetActiveIndicator(active);
		}

		public void SetLocalScale(Vector3 localScale)
		{
			MarkerGo.transform.localScale = localScale;
			if (IndicatorGo != null)
			{
				IndicatorGo.transform.localScale = localScale;
			}
		}

		public void Destroy()
		{
			Disposable.Dispose();
			UnityEngine.Object.Destroy(MarkerGo);
			if (IndicatorGo != null)
			{
				UnityEngine.Object.Destroy(IndicatorGo);
			}
		}
	}

	private const string BOSS_MARKER = "BossMarker";

	private const string MONSTER_MARKER = "MonsterMarker";

	private const string EXIT_MARKER = "ExitMarker";

	private const string SAFE_EXIT_MARKER = "SafeExitMarker";

	private const string NPC_MARKER = "NpcMarker";

	private const string PLAYER_MARKER = "PlayerMarker";

	private const string QUEST_MARKER = "QuestMarker";

	private const string PORTAL_MARKER = "PortalMarker";

	private const string PING_MARKER = "PingMarker";

	private const string TEXT_MARKER = "TextMarker";

	private const string QUEST_BOX_MARKER = "QuestBoxMarker";

	private const string PLAYER_POINT_MARKER = "PlayerPointMarker";

	[SerializeField]
	private float _pointMarkerPickRadius = 26f;

	[SerializeField]
	private float _pointMarkerPickCenterOffset = 18f;

	[View]
	private InGameMiniMapView _view;

	[InjectModel]
	private InGameMiniMapModel _model;

	[Inject]
	private MiniMapSettings _miniMapSettings;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameObjectManager _objectManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMapManager _mapManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GamePingManager _pingManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private CameraManager _cameraManager;

	[Inject]
	private PlayerInputController _inputController;

	private CharacterModel _characterModel;

	private Vector3 _markerLossyScale;

	private CompositeDisposable _disposable;

	private readonly List<Image> _sectorImages = new List<Image>();

	private readonly Dictionary<ulong, PlayerSight> _playerSights = new Dictionary<ulong, PlayerSight>();

	private readonly Dictionary<ulong, MiniMapMarker> _playerMarkers = new Dictionary<ulong, MiniMapMarker>();

	private readonly Dictionary<ulong, MiniMapMarker> _monsterMarkers = new Dictionary<ulong, MiniMapMarker>();

	private readonly Dictionary<int, MiniMapMarker> _questMarkers = new Dictionary<int, MiniMapMarker>();

	private readonly Dictionary<ulong, List<MiniMapMarker>> _pointMarkers = new Dictionary<ulong, List<MiniMapMarker>>();

	private readonly List<MiniMapMarker> _objectMarkers = new List<MiniMapMarker>();

	private readonly List<MiniMapMarker> _campaignTaskRelatedMarkers = new List<MiniMapMarker>();

	private float OrthographicSize => _miniMapSettings.OrthographicSize;

	private float TextureSize => _miniMapSettings.GetTextureSize();

	public (int Width, int Height) MapSize => _mapManager.MapSize;

	private bool IsRevealingMapMap => _gameManager.IsRevealingMapMap;

	private ulong ViewTargetClientId => _gameManager.RxViewTargetClientId.Value;

	private void Start()
	{
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_pingManager.OnPingTriggered.Subscribe(OnPingTriggered).AddTo(this);
	}

	private void OnDestroy()
	{
		_disposable?.Dispose();
	}

	private void OnInject(InGameMiniMapModel model)
	{
		Vector3 lossyScale = _view.MainPlayerMarker.transform.lossyScale;
		_markerLossyScale = lossyScale / model.BaseZoomScale;
		InitMiniMapImages();
		model.FullScreenZoomScale = GetFullScreenZoomScale();
		model.RxIsFullScreen.Subscribe(OnFullScreen).AddTo(this);
		_mapManager.RxMapLayer.Skip(1).Subscribe(OnMapLayerChanged).AddTo(this);
		_objectManager.UnitModels.ForEach(OnUnitSpawned);
		_objectManager.InteractiveObjects.Values.ForEach(OnInteractiveObjectSpawned);
		_objectManager.OnUnitSpawned.Subscribe(OnUnitSpawned).AddTo(this);
		_objectManager.OnUnitDespawned.Subscribe(OnUnitDespawned).AddTo(this);
		_objectManager.OnInteractiveObjectSpawned.Subscribe(OnInteractiveObjectSpawned).AddTo(this);
		_objectManager.OnInteractiveObjectDespawned.Subscribe(OnInteractiveObjectDespawned).AddTo(this);
		_objectManager.OnPingObjectSpawned.Subscribe(OnPingObjectSpawned).AddTo(this);
		_mapManager.CampaignMarkerDatas.Values.ForEach(OnSpawnQuestMarker);
		_mapManager.RxCampaignMarkerDatas.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<int, CampaignMarkerData> e)
		{
			OnSpawnQuestMarker(e.Value);
		}).AddTo(this);
		_mapManager.RxCampaignMarkerDatas.ObserveRemove().Subscribe(delegate(DictionaryRemoveEvent<int, CampaignMarkerData> e)
		{
			OnDespawnQuestMarker(e.Key);
		}).AddTo(this);
		_mapManager.CustomMarkerDatas.Values.ForEach(OnSpawnCustomMarker);
		_mapManager.RxCustomMarkerDatas.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<int, CustomMarkerData> e)
		{
			OnSpawnCustomMarker(e.Value);
		}).AddTo(this);
		_mapManager.RxCustomMarkerDatas.ObserveRemove().Subscribe(delegate(DictionaryRemoveEvent<int, CustomMarkerData> e)
		{
			OnDespawnCustomMarker(e.Key);
		}).AddTo(this);
		_mapManager.RxTextMarkerDatas.ForEach(OnSpawnTextMarker);
		_mapManager.RxTextMarkerDatas.ObserveAdd().Subscribe(delegate(CollectionAddEvent<TextMarkerData> e)
		{
			OnSpawnTextMarker(e.Value);
		}).AddTo(this);
		InitPlayerPointMarkers();
		UpdateMarkerScales();
	}

	private void InitMiniMapImages()
	{
		RectTransform component = _view.ImageRoot.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2((float)_mapManager.MapSize.Item1 * TextureSize, (float)_mapManager.MapSize.Item2 * TextureSize);
		GridLayoutGroup component2 = _view.ImageRoot.GetComponent<GridLayoutGroup>();
		component2.constraintCount = _mapManager.MapSize.Item1;
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			int num = _mapManager.SectorIds[i];
			GameObject gameObject = new GameObject(string.Format("{0}", num));
			gameObject.transform.SetParent(component, worldPositionStays: false);
			Image image = gameObject.AddComponent<Image>();
			image.enabled = _mapManager.GetSectorMapLayer(i) == 0;
			_sectorImages.Add(image);
			if (num > 0)
			{
				image.SetSprite(_assetManager.GetTextureMiniMapAsync(num));
			}
		}
		_view.MinimapOuterImage.Initialize(_mapManager.MapSize.Item1 * _mapManager.MapSize.Item2);
		Vector3[] array = new Vector3[_mapManager.SectorIds.Length];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = _mapManager.GetSectorOffset(j);
		}
		_view.MinimapSightMaskApplier.Initialize(array);
	}

	private void OnMapLayerChanged(int mapLayer)
	{
		for (int i = 0; i < _mapManager.SectorIds.Length; i++)
		{
			_sectorImages[i].enabled = _mapManager.GetSectorMapLayer(i) == mapLayer;
		}
		_model.FullScreenZoomScale = GetFullScreenZoomScale();
		if (!_model.RxIsFullScreen.Value)
		{
			ApplyLayerMarkers(mapLayer);
			return;
		}
		HideLayerMarkers();
		Observable.NextFrame().Subscribe(delegate
		{
			UpdateMarkerScales();
			ApplyLayerMarkers(mapLayer);
		}).AddTo(this);
	}

	private void HideLayerMarkers()
	{
		_questMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetActive(active: false);
		});
		_objectMarkers.Where((MiniMapMarker m) => m.IsFound).ForEach(delegate(MiniMapMarker m)
		{
			m.SetActiveMarker(active: false);
		});
		_playerMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetActive(active: false);
		});
		_pointMarkers.Values.ForEach(delegate(List<MiniMapMarker> list)
		{
			list.ForEach(delegate(MiniMapMarker m)
			{
				m.SetActiveMarker(active: false);
			});
		});
	}

	private void ApplyLayerMarkers(int mapLayer)
	{
		_questMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetActive(m.MapLayer == mapLayer);
		});
		_objectMarkers.Where((MiniMapMarker m) => m.IsFound).ForEach(delegate(MiniMapMarker m)
		{
			m.SetActiveMarker(m.MapLayer == mapLayer);
		});
		_playerMarkers.ForEach(delegate(KeyValuePair<ulong, MiniMapMarker> pair)
		{
			MiniMapMarker value = pair.Value;
			bool active = value.MapLayer == mapLayer && pair.Key != ViewTargetClientId;
			value.SetActive(active);
		});
		_pointMarkers.Values.ForEach(delegate(List<MiniMapMarker> list)
		{
			list.ForEach(delegate(MiniMapMarker m)
			{
				m.SetActiveMarker(m.MapLayer == mapLayer);
			});
		});
	}

	private void Update()
	{
		if (!(_characterModel == null) && _model != null)
		{
			UpdateMiniMapPosition();
			UpdateMarkersInSight();
			UpdateIndicators();
		}
	}

	private void OnPingTriggered((EPingType pingType, Vector2 inputPosition) data)
	{
		if (!_mapManager.RxIsFullScreen.Value || data.pingType == EPingType.None)
		{
			return;
		}
		RectTransform component = _view.ImageRoot.GetComponent<RectTransform>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(component, data.inputPosition, _cameraManager.UiCamera, out var localPoint);
		Vector3 position = new Vector3(localPoint.x / TextureSize * (OrthographicSize + OrthographicSize), 0f, localPoint.y / TextureSize * (OrthographicSize + OrthographicSize));
		EPingResult ePingResult = _pingManager.TrySendPingByPosition(data.pingType, position);
		if (ePingResult != EPingResult.Ok)
		{
			_toastManager.Notice(ePingResult.ToLocalText());
		}
	}

	private void OnFullScreen(bool isFullScreen)
	{
		_view.MainPlayerMarker.FindChildComponent<Image>("Vision").gameObject.SetActive(!isFullScreen);
		UpdateMarkerScales();
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		_characterModel = _gameManager.FindCharacterModel(clientId);
		if (_characterModel == null)
		{
			return;
		}
		_characterModel.RxPosition.Subscribe(OnMainPlayerPositionChanged).AddTo(_disposable);
		_characterModel.RxForward.Subscribe(OnMainPlayerForwardChanged).AddTo(_disposable);
		_playerMarkers.ForEach(delegate(KeyValuePair<ulong, MiniMapMarker> pair)
		{
			MiniMapMarker value = pair.Value;
			bool flag = pair.Key != clientId && value.MapLayer == _mapManager.MapLayer;
			if (flag)
			{
				value.MarkerGo.transform.localPosition = WorldToMiniMapPoint(value.TargetPosition);
			}
			value.SetActive(flag);
		});
	}

	private void OnMainPlayerPositionChanged(Vector3 position)
	{
		_view.MainPlayerMarker.transform.localPosition = WorldToMiniMapPoint(position);
	}

	private void OnMainPlayerForwardChanged(Vector3 forward)
	{
		_view.MainPlayerMarker.transform.localRotation = WorldToMiniMapRotation(forward);
	}

	private void OnUnitSpawned(UnitModel unitModel)
	{
		if (unitModel is CharacterModel characterModel)
		{
			OnCharacterSpawned(characterModel);
		}
		else if (unitModel is MonsterModel monsterModel)
		{
			OnMonsterSpawned(monsterModel);
		}
	}

	private void OnUnitSpawned(UnitController unitController)
	{
		UnitModel component = unitController.GetComponent<UnitModel>();
		if (component is CharacterModel characterModel)
		{
			OnCharacterSpawned(characterModel);
		}
		else if (component is MonsterModel monsterModel)
		{
			OnMonsterSpawned(monsterModel);
		}
	}

	private void OnUnitDespawned(UnitController unitController)
	{
		if (unitController is CharacterController characterController)
		{
			OnCharacterDespawned(characterController);
		}
		else if (unitController is MonsterController monsterController)
		{
			OnMonsterDespawned(monsterController);
		}
	}

	private void EnsurePlayerMarker(CharacterModel characterModel)
	{
		ulong clientId = characterModel.ClientId;
		if (_playerMarkers.ContainsKey(clientId))
		{
			return;
		}
		Vector3 position = characterModel.transform.position;
		Transform rootTf = _view.PlayerMarkerRoot.transform;
		GameObject gameObject = CreateMarker("PlayerMarker", position, clientId != _gameManager.RxViewTargetClientId.Value, rootTf);
		GameObject indicatorGo = CreateMarker("PlayerMarker", position, active: false, _view.IndicatorCanvas.transform);
		MiniMapMarker miniMapMarker = new MiniMapMarker(characterModel.NetworkObjectId, gameObject, position, indicatorGo);
		int pointLayer = _mapManager.GetPointLayer(position);
		miniMapMarker.MapLayer = pointLayer;
		if (clientId != _gameManager.RxViewTargetClientId.Value && pointLayer != _mapManager.MapLayer)
		{
			miniMapMarker.SetActive(active: false);
		}
		_playerMarkers.Add(clientId, miniMapMarker);
		characterModel.RxPosition.Subscribe(delegate(Vector3 pos)
		{
			OnCharacterPositionChanged(clientId, pos);
		}).AddTo(miniMapMarker.Disposable);
		characterModel.RxForward.Subscribe(delegate(Vector3 forward)
		{
			OnCharacterForwardChanged(clientId, forward);
		}).AddTo(miniMapMarker.Disposable);
		characterModel.RxState.Subscribe(delegate(EActionState state)
		{
			OnCharacterStateChanged(clientId, state);
		}).AddTo(miniMapMarker.Disposable);
	}

	private void OnCharacterSpawned(CharacterModel characterModel)
	{
		_playerSights.Add(characterModel.ClientId, new PlayerSight
		{
			CharacterModel = characterModel,
			PlaneSightUnit = characterModel.GetComponentInChildren<PlaneSightUnit>()
		});
		EnsurePlayerMarker(characterModel);
	}

	private void OnCharacterDespawned(CharacterController characterController)
	{
		ulong clientId = characterController.ClientId;
		_playerSights.Remove(clientId);
		if (_playerMarkers.Remove(clientId, out var value))
		{
			value.Destroy();
		}
	}

	private void OnCharacterPositionChanged(ulong clientId, Vector3 position)
	{
		if (!_playerMarkers.TryGetValue(clientId, out var value))
		{
			_logger.Warning(string.Format("ClientId ({0}) marker not found", clientId));
			return;
		}
		value.TargetPosition = position;
		value.MapLayer = _mapManager.GetPointLayer(position);
		if (clientId == _gameManager.RxViewTargetClientId.Value)
		{
			if (value.MarkerGo.activeSelf)
			{
				value.MarkerGo.SetActive(false);
			}
			value.SetActiveIndicator(active: false);
			return;
		}
		bool flag = value.MapLayer == _mapManager.MapLayer;
		if (value.MarkerGo.activeSelf != flag)
		{
			value.MarkerGo.SetActive(flag);
		}
		value.MarkerGo.transform.localPosition = WorldToMiniMapPoint(position);
	}

	private void OnCharacterForwardChanged(ulong clientId, Vector3 forward)
	{
		if (!_playerMarkers.TryGetValue(clientId, out var value))
		{
			_logger.Warning(string.Format("ClientId ({0}) marker not found", clientId));
		}
		else if (value.ApplyDirection)
		{
			value.MarkerGo.transform.localRotation = WorldToMiniMapRotation(forward);
		}
	}

	private void OnCharacterStateChanged(ulong clientId, EActionState state)
	{
		if (!_playerMarkers.TryGetValue(clientId, out var value))
		{
			_logger.Warning(string.Format("ClientId ({0}) marker not found", clientId));
			return;
		}
		if (state == EActionState.Escaped)
		{
			_playerMarkers.Remove(clientId);
			value.Destroy();
			return;
		}
		bool flag = state == EActionState.Dead;
		value.MarkerGo.FindChildComponent<Image>("Icon").gameObject.SetActive(!flag);
		value.MarkerGo.FindChildComponent<Image>("DeadIcon").gameObject.SetActive(flag);
		value.IndicatorGo.FindChildComponent<Image>("Icon").gameObject.SetActive(!flag);
		value.IndicatorGo.FindChildComponent<Image>("DeadIcon").gameObject.SetActive(flag);
		if (flag)
		{
			value.ApplyDirection = false;
			value.MarkerGo.transform.rotation = Quaternion.identity;
			value.IndicatorGo.transform.rotation = Quaternion.identity;
		}
		else
		{
			value.ApplyDirection = true;
		}
	}

	private void OnMonsterSpawned(MonsterModel monsterModel)
	{
		bool isBoss = monsterModel.IsBoss;
		string markerName = (isBoss ? "BossMarker" : "MonsterMarker");
		Vector3 position = monsterModel.transform.position;
		Transform rootTf = _view.MonsterMarkerRoot.transform;
		if (!TryCreateMarker(markerName, position, active: false, rootTf, out var go))
		{
			return;
		}
		ulong networkObjectId = monsterModel.NetworkObjectId;
		MiniMapMarker miniMapMarker = new MiniMapMarker(networkObjectId, go, position);
		miniMapMarker.MapLayer = _mapManager.GetPointLayer(position);
		_monsterMarkers.Add(networkObjectId, miniMapMarker);
		monsterModel.RxPosition.Subscribe(delegate(Vector3 pos)
		{
			OnMonsterPositionChanged(networkObjectId, pos);
		}).AddTo(this);
		monsterModel.RxState.Where((EActionState state) => state == EActionState.Dead).Subscribe(delegate
		{
			OnMonsterDead(networkObjectId, isBoss);
		}).AddTo(this);
	}

	private void OnMonsterDespawned(MonsterController monsterController)
	{
		if (_monsterMarkers.Remove(monsterController.NetworkObjectId, out var value))
		{
			value.Destroy();
		}
	}

	private void OnMonsterPositionChanged(ulong networkObjectId, Vector3 position)
	{
		if (_monsterMarkers.TryGetValue(networkObjectId, out var value))
		{
			value.TargetPosition = position;
			value.MarkerGo.transform.localPosition = WorldToMiniMapPoint(position);
		}
	}

	private void OnMonsterDead(ulong networkObjectId, bool isBoss)
	{
		if (!_monsterMarkers.Remove(networkObjectId, out var value))
		{
			return;
		}
		if (isBoss)
		{
			value.IsFound = true;
			value.SetActiveMarker(value.MapLayer == _mapManager.MapLayer);
			value.MarkerGo.FindChildComponent<Image>("Icon").gameObject.SetActive(false);
			value.MarkerGo.FindChildComponent<Image>("DeadIcon").gameObject.SetActive(false);
			_objectMarkers.Add(value);
		}
		else
		{
			value.Destroy();
		}
	}

	private void OnSpawnQuestMarker(CampaignMarkerData data)
	{
		if (_campaignManager.IsTaskAchieved(data.TaskId))
		{
			return;
		}
		GameObject markerGo = CreateMarker("QuestMarker", data.Position, active: true, _view.ObjectMarkerRoot.transform);
		GameObject indicatorGo = CreateMarker("QuestMarker", data.Position, active: true, _view.IndicatorCanvas.transform);
		MiniMapMarker miniMapMarker = new MiniMapMarker(0uL, markerGo, data.Position, indicatorGo);
		int pointLayer = _mapManager.GetPointLayer(data.Position);
		miniMapMarker.MapLayer = pointLayer;
		if (pointLayer != _mapManager.MapLayer)
		{
			miniMapMarker.SetActive(active: false);
		}
		_questMarkers.Add(data.TaskId, miniMapMarker);
	}

	private void OnDespawnQuestMarker(int campaignTaskId)
	{
		if (_questMarkers.Remove(campaignTaskId, out var value))
		{
			value.Destroy();
		}
	}

	private void OnInteractiveObjectSpawned(IInteractiveObject interactiveObject)
	{
		ulong networkObjectId;
		string markerName;
		Vector3 position;
		bool active;
		if (interactiveObject is NpcObject npcObject)
		{
			networkObjectId = npcObject.NetworkObjectId;
			position = npcObject.Position;
			active = false;
			markerName = "NpcMarker";
		}
		else if (interactiveObject is WaveExit waveExit)
		{
			networkObjectId = waveExit.NetworkObjectId;
			MiniMapMarker miniMapMarker = CreateObjectMarker(networkObjectId, "ExitMarker", waveExit.Position, waveExit.DisplayOnMinimapAtStart);
			if (miniMapMarker == null)
			{
				return;
			}
			waveExit.RxExitState.Where((EExitState state) => state == EExitState.Finished).Subscribe(delegate
			{
				DestroyObjectMarker(networkObjectId);
			}).AddTo(miniMapMarker.Disposable);
			return;
		}
		else if (interactiveObject is SafeExit safeExit)
		{
			networkObjectId = safeExit.NetworkObjectId;
			position = safeExit.Position;
			active = safeExit.DisplayOnMinimapAtStart;
			markerName = "SafeExitMarker";
		}
		else if (interactiveObject is Portal portal)
		{
			networkObjectId = portal.NetworkObjectId;
			position = portal.Position;
			active = true;
			markerName = "PortalMarker";
		}
		else if (interactiveObject is RewardBox rewardBox)
		{
			if (_objectManager.IsQuestRewardBox(rewardBox.RewardBoxId))
			{
				CreateQuestBoxMarker(rewardBox);
			}
			return;
		}
		else if (interactiveObject is DropGoods dropGoods)
		{
			if (_objectManager.IsQuestItem(dropGoods.DisplayGoods))
			{
				CreateQuestDropGoodsMarker(dropGoods);
			}
			return;
		}
		else
		{
			if (!(interactiveObject is SkillExecutor skillExecutor))
			{
				return;
			}
			TSkillExecutor tSkillExecutor = TSkillExecutor.Table[skillExecutor.SkillExecutorId];
			networkObjectId = skillExecutor.NetworkObjectId;
			markerName = tSkillExecutor.MiniMapMarker;
			position = skillExecutor.Position;
			active = false;
		}
		CreateObjectMarker(networkObjectId, markerName, position, active);
	}

	private void OnInteractiveObjectDespawned(IInteractiveObject interactiveObject)
	{
		if (interactiveObject is RewardBox || interactiveObject is DropGoods)
		{
			DestroyTaskRelatedMarker(interactiveObject.NetworkObjectId);
		}
		else
		{
			DestroyObjectMarker(interactiveObject.NetworkObjectId);
		}
	}

	private MiniMapMarker CreateObjectMarker(ulong networkObjectId, string markerName, Vector3 position, bool active, bool useIndicator = false)
	{
		Transform rootTf = _view.ObjectMarkerRoot.transform;
		if (!TryCreateMarker(markerName, position, active, rootTf, out var go))
		{
			return null;
		}
		GameObject go2 = null;
		if (useIndicator)
		{
			Transform rootTf2 = _view.IndicatorCanvas.transform;
			if (!TryCreateMarker(markerName, position, active: false, rootTf2, out go2))
			{
				UnityEngine.Object.Destroy(go);
				return null;
			}
		}
		MiniMapMarker miniMapMarker = new MiniMapMarker(networkObjectId, go, position, go2);
		int pointLayer = _mapManager.GetPointLayer(position);
		miniMapMarker.MapLayer = pointLayer;
		miniMapMarker.IsFound = active;
		if (active && pointLayer != _mapManager.MapLayer)
		{
			miniMapMarker.SetActiveMarker(active: false);
		}
		_objectMarkers.Add(miniMapMarker);
		return miniMapMarker;
	}

	private void DestroyObjectMarker(ulong networkObjectId)
	{
		MiniMapMarker miniMapMarker = _objectMarkers.FirstOrDefault((MiniMapMarker m) => m.NetworkObjectId == networkObjectId);
		if (miniMapMarker != null)
		{
			_objectMarkers.Remove(miniMapMarker);
			miniMapMarker.Destroy();
		}
	}

	private MiniMapMarker CreateQuestBoxMarker(RewardBox rewardBox)
	{
		ulong networkObjectId = rewardBox.NetworkObjectId;
		Transform rootTf = _view.ObjectMarkerRoot.transform;
		if (!TryCreateMarker("QuestBoxMarker", rewardBox.Position, active: false, rootTf, out var go))
		{
			return null;
		}
		TCampaignTask tCampaignTask = TCampaignTask.GroupTable[_gameContext.CampaignId].First((TCampaignTask t) => t.QuestRewardBoxIds != null && t.QuestRewardBoxIds.Contains(rewardBox.RewardBoxId));
		MiniMapMarker miniMapMarker = new MiniMapMarker(networkObjectId, go, rewardBox.Position);
		miniMapMarker.MapLayer = _mapManager.GetPointLayer(rewardBox.Position);
		miniMapMarker.CampaignTaskId = tCampaignTask.Id;
		_campaignTaskRelatedMarkers.Add(miniMapMarker);
		rewardBox.RxCanInteract.Where((bool canInteract) => !canInteract).Subscribe(delegate
		{
			DestroyTaskRelatedMarker(networkObjectId);
		}).AddTo(this);
		return miniMapMarker;
	}

	private MiniMapMarker CreateQuestDropGoodsMarker(DropGoods dropGoods)
	{
		ulong networkObjectId = dropGoods.NetworkObjectId;
		Transform rootTf = _view.ObjectMarkerRoot.transform;
		if (!TryCreateMarker("QuestMarker", dropGoods.Position, active: false, rootTf, out var go))
		{
			return null;
		}
		TCampaignTask tCampaignTask = TCampaignTask.GroupTable[_gameContext.CampaignId].First((TCampaignTask t) => t.QuestItemIds != null && t.QuestItemIds.Contains(dropGoods.GoodsId));
		MiniMapMarker miniMapMarker = new MiniMapMarker(networkObjectId, go, dropGoods.Position);
		miniMapMarker.MapLayer = _mapManager.GetPointLayer(dropGoods.Position);
		miniMapMarker.CampaignTaskId = tCampaignTask.Id;
		_campaignTaskRelatedMarkers.Add(miniMapMarker);
		return miniMapMarker;
	}

	private void DestroyTaskRelatedMarker(ulong networkObjectId)
	{
		MiniMapMarker miniMapMarker = _campaignTaskRelatedMarkers.FirstOrDefault((MiniMapMarker m) => m.NetworkObjectId == networkObjectId);
		if (miniMapMarker != null)
		{
			_campaignTaskRelatedMarkers.Remove(miniMapMarker);
			miniMapMarker.Destroy();
		}
	}

	private void OnPingObjectSpawned(PingObject pingObject)
	{
		Vector3 position = pingObject.transform.position;
		int instanceId = pingObject.GetInstanceID();
		GameObject gameObject = CreateMarker("PingMarker", position, active: true, _view.ObjectMarkerRoot.transform);
		GameObject gameObject2 = CreateMarker("PingMarker", position, active: false, _view.IndicatorCanvas.transform);
		EPingType pingType = pingObject.PingType;
		gameObject.FindChildComponent<Image>("Icon").sprite = _assetManager.GetIconPing(pingType);
		gameObject2.FindChildComponent<Image>("Icon").sprite = _assetManager.GetIconPing(pingType);
		MiniMapMarker miniMapMarker = new MiniMapMarker(instanceId, gameObject, position, gameObject2);
		int pointLayer = _mapManager.GetPointLayer(position);
		miniMapMarker.MapLayer = pointLayer;
		miniMapMarker.IsFound = true;
		if (pointLayer != _mapManager.MapLayer && miniMapMarker.MarkerGo.activeSelf)
		{
			miniMapMarker.MarkerGo.SetActive(false);
		}
		_objectMarkers.Add(miniMapMarker);
		Observable.Timer(TimeSpan.FromSeconds(TConst.MiniMapPingDuration.FloatValue)).Subscribe(delegate
		{
			OnPingObjectDespawned(instanceId);
		}).AddTo(this);
	}

	private void OnPingObjectDespawned(int instanceId)
	{
		MiniMapMarker miniMapMarker = _objectMarkers.FirstOrDefault((MiniMapMarker m) => m.CustomId == instanceId);
		if (miniMapMarker != null)
		{
			_objectMarkers.Remove(miniMapMarker);
			miniMapMarker.Destroy();
		}
	}

	private void OnSpawnCustomMarker(CustomMarkerData data)
	{
		Transform rootTf = _view.ObjectMarkerRoot.transform;
		if (!TryCreateMarker(data.MarkerName, data.Position, active: true, rootTf, out var go))
		{
			return;
		}
		Transform rootTf2 = _view.IndicatorCanvas.transform;
		if (TryCreateMarker(data.MarkerName, data.Position, active: true, rootTf2, out var go2))
		{
			MiniMapMarker miniMapMarker = new MiniMapMarker(data.CustomId, go, data.Position, go2);
			int pointLayer = _mapManager.GetPointLayer(data.Position);
			miniMapMarker.MapLayer = pointLayer;
			miniMapMarker.IsFound = true;
			if (pointLayer != _mapManager.MapLayer)
			{
				miniMapMarker.SetActiveMarker(active: false);
			}
			_objectMarkers.Add(miniMapMarker);
		}
	}

	private void OnDespawnCustomMarker(int id)
	{
		MiniMapMarker miniMapMarker = _objectMarkers.FirstOrDefault((MiniMapMarker m) => m.CustomId == id);
		if (miniMapMarker != null)
		{
			_objectMarkers.Remove(miniMapMarker);
			miniMapMarker.Destroy();
		}
	}

	private void OnSpawnTextMarker(TextMarkerData data)
	{
		Transform rootTf = _view.TextMarkerRoot.transform;
		if (TryCreateMarker("TextMarker", data.Position, active: true, rootTf, out var go))
		{
			go.FindChildComponent<TextMeshProUGUI>("Text").text = LocalizationUtility.GetLocalizedText(data.TextKey);
			MiniMapMarker miniMapMarker = new MiniMapMarker(0uL, go, data.Position);
			int pointLayer = _mapManager.GetPointLayer(data.Position);
			miniMapMarker.MapLayer = pointLayer;
			miniMapMarker.IsFound = true;
			if (pointLayer != _mapManager.MapLayer)
			{
				miniMapMarker.SetActiveMarker(active: false);
			}
			_objectMarkers.Add(miniMapMarker);
		}
	}

	private bool TryCreateMarker(string markerName, Vector3 position, bool active, Transform rootTf, out GameObject go)
	{
		go = null;
		if (string.IsNullOrEmpty(markerName))
		{
			return false;
		}
		go = CreateMarker(markerName, position, active, rootTf);
		return true;
	}

	private GameObject CreateMarker(string markerName, Vector3 position, bool active, Transform rootTf)
	{
		GameObject gameObject = _assetManager.InstantiateMiniMapMarker(markerName);
		gameObject.transform.parent = rootTf;
		gameObject.transform.localPosition = WorldToMiniMapPoint(position);
		gameObject.transform.localScale = GetMarkerScale();
		gameObject.transform.rotation = Quaternion.identity;
		gameObject.SetActive(active);
		return gameObject;
	}

	private void UpdateMiniMapPosition()
	{
		if (_characterModel == null)
		{
			return;
		}
		if (!_model.RxIsFullScreen.Value)
		{
			base.transform.localPosition = -WorldToMiniMapPoint(_characterModel.Position);
			return;
		}
		(int, int, int, int) layerBounds = GetLayerBounds();
		float x = (float)(layerBounds.Item2 + layerBounds.Item1 + 1) * TextureSize * 0.5f;
		float y = (float)(layerBounds.Item3 + 1 + layerBounds.Item4) * TextureSize * 0.5f;
		base.transform.localPosition = new Vector3(0f - x, 0f - y, 0f);
	}

	private void UpdateMarkersInSight()
	{
		int currentLayer = _mapManager.MapLayer;
		_objectMarkers.Where((MiniMapMarker m) => !m.IsFound).ForEach(delegate(MiniMapMarker m)
		{
			if (m.MapLayer == currentLayer && IsTargetInSight(MiniMapPointToWorld(m.MarkerGo.transform.localPosition)))
			{
				if (!m.MarkerGo.activeSelf)
				{
					m.MarkerGo.SetActive(true);
				}
				m.IsFound = true;
			}
		});
		_campaignTaskRelatedMarkers.ForEach(delegate(MiniMapMarker m)
		{
			Vector3 targetPosition = MiniMapPointToWorld(m.MarkerGo.transform.localPosition);
			if (!m.IsFound)
			{
				if (IsTargetInSight(targetPosition))
				{
					m.IsFound = true;
				}
				if (!m.IsFound)
				{
					return;
				}
			}
			bool flag = m.MapLayer == currentLayer && !_campaignManager.IsTaskAchieved(m.CampaignTaskId);
			if (m.MarkerGo.activeSelf != flag)
			{
				m.MarkerGo.SetActive(flag);
			}
		});
		_monsterMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			Vector3 targetPosition = MiniMapPointToWorld(m.MarkerGo.transform.localPosition);
			bool flag = m.MapLayer == currentLayer && IsTargetInSight(targetPosition);
			if (m.MarkerGo.activeSelf != flag)
			{
				m.MarkerGo.SetActive(flag);
			}
		});
	}

	private void UpdateIndicators()
	{
		Vector3 markerCenterPosition = _view.MainPlayerMarker.transform.position;
		Quaternion inverseIndicatorCanvasRotation = Quaternion.Inverse(_view.IndicatorCanvas.transform.rotation);

		void UpdateIndicator(MiniMapMarker marker, bool applyDirection)
		{
			if (marker.IndicatorGo == null)
			{
				return;
			}
			if (!marker.MarkerGo.activeSelf)
			{
				marker.SetActiveIndicator(active: false);
				return;
			}
			if (_model.RxIsFullScreen.Value)
			{
				marker.SetActiveIndicator(active: false);
				return;
			}
			Vector3 vector = marker.TargetPosition - _characterModel.Position;
			if (vector.magnitude / (OrthographicSize + OrthographicSize) * TextureSize < _model.MaskRadius)
			{
				marker.SetActiveIndicator(active: false);
				return;
			}
			Quaternion localRotation = WorldToMiniMapRotation(vector.normalized);
			Vector3 normalized = (marker.MarkerGo.transform.position - markerCenterPosition).normalized;
			Vector3 vector2 = inverseIndicatorCanvasRotation * normalized;
			marker.SetActiveIndicator(active: true);
			marker.IndicatorGo.transform.position = markerCenterPosition;
			marker.IndicatorGo.transform.localPosition += vector2 * _model.MaskRadius;
			if (applyDirection)
			{
				marker.IndicatorGo.transform.localRotation = localRotation;
			}
		}

		_objectMarkers.ForEach(delegate(MiniMapMarker m)
		{
			UpdateIndicator(m, m.ApplyDirection);
		});
		_playerMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			UpdateIndicator(m, m.ApplyDirection);
		});
		_questMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			UpdateIndicator(m, m.ApplyDirection);
		});
		_pointMarkers.Values.ForEach(delegate(List<MiniMapMarker> list)
		{
			list.ForEach(delegate(MiniMapMarker m)
			{
				UpdateIndicator(m, m.ApplyDirection);
			});
		});
	}

	private void UpdateMarkerScales()
	{
		Vector3 markerScale = GetMarkerScale();
		_view.MainPlayerMarker.transform.localScale = markerScale;
		_playerMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetLocalScale(markerScale);
		});
		_monsterMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetLocalScale(markerScale);
		});
		_objectMarkers.ForEach(delegate(MiniMapMarker m)
		{
			m.SetLocalScale(markerScale);
		});
		_campaignTaskRelatedMarkers.ForEach(delegate(MiniMapMarker m)
		{
			m.SetLocalScale(markerScale);
		});
		_questMarkers.Values.ForEach(delegate(MiniMapMarker m)
		{
			m.SetLocalScale(markerScale);
		});
		_pointMarkers.Values.ForEach(delegate(List<MiniMapMarker> list)
		{
			list.ForEach(delegate(MiniMapMarker m)
			{
				m.SetLocalScale(markerScale);
			});
		});
	}

	private Vector3 GetMarkerScale()
	{
		Vector3 lossyScale = base.transform.lossyScale;
		return new Vector3(_markerLossyScale.x / lossyScale.x, _markerLossyScale.y / lossyScale.y, _markerLossyScale.z / lossyScale.z);
	}

	private float WorldToMiniMapDistance(float worldDistance)
	{
		return TextureSize * (worldDistance / (OrthographicSize + OrthographicSize));
	}

	private float MiniMapToWorldDistance(float miniMapDistance)
	{
		return miniMapDistance / TextureSize * (OrthographicSize + OrthographicSize);
	}

	private Vector3 WorldToMiniMapPoint(Vector3 position)
	{
		return new Vector3(position.x / (OrthographicSize + OrthographicSize) * TextureSize, TextureSize * (position.z / (OrthographicSize + OrthographicSize)), 0f);
	}

	private Vector3 MiniMapPointToWorld(Vector3 miniMapPoint)
	{
		return new Vector3(miniMapPoint.x / TextureSize * (OrthographicSize + OrthographicSize), 0f, miniMapPoint.y / TextureSize * (OrthographicSize + OrthographicSize));
	}

	private Quaternion WorldToMiniMapRotation(Vector3 forward)
	{
		Vector2 vector = MathUtility.IsometricToTopDown(forward);
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		return Quaternion.Euler(0f, 0f, num - 45f);
	}

	private float GetFullScreenZoomScale()
	{
		(int, int, int, int) layerBounds = GetLayerBounds();
		// 0x184a51af0 applied to 0.785398 is cosf (Mathf.Cos, machine code checked)
		return 1080f / ((TextureSize * (float)(layerBounds.Item4 - layerBounds.Item3 + 1) + (float)(layerBounds.Item2 - layerBounds.Item1 + 1) * TextureSize) * Mathf.Cos(0.785398f)) * _model.FullScreenRatio;
	}

	private (int, int, int, int) GetLayerBounds()
	{
		if (!_gameContext.IsInLounge)
		{
			List<FixedSector> list = TCampaign.Table[_gameContext.CampaignId].FixedSectors.Where((FixedSector s) => s.Layer == _mapManager.MapLayer).ToList();
			if (list != null && list.Count > 0)
			{
				int item = list.Min((FixedSector s) => s.IndexX);
				int item2 = list.Max((FixedSector s) => s.IndexX);
				int item3 = list.Min((FixedSector s) => s.IndexZ);
				int item4 = list.Max((FixedSector s) => s.IndexZ);
				return (item, item2, item3, item4);
			}
		}
		return (0, _mapManager.MapSize.Item1 - 1, 0, _mapManager.MapSize.Item2 - 1);
	}

	private void InitPlayerPointMarkers()
	{
		_gameContext.Players.Values.ForEach(OnPlayerAdded);
		_gameContext.RxPlayers.ObserveAdd().Subscribe(delegate(DictionaryAddEvent<ulong, GamePlayer> e)
		{
			OnPlayerAdded(e.Value);
		}).AddTo(this);
		_gameContext.RxPlayers.ObserveRemove().Subscribe(delegate(DictionaryRemoveEvent<ulong, GamePlayer> e)
		{
			OnPlayerRemoved(e.Key);
		}).AddTo(this);
		_inputController.OnPointMinimapPressed.Subscribe(OnPointMinimapPressed).AddTo(this);
	}

	private void OnPlayerAdded(GamePlayer player)
	{
		player.RxPointMarkers.Where((PointMarkers pointMarkers) => pointMarkers != null).Subscribe(delegate
		{
			RebuildPointMarkers(player);
		}).AddTo(this);
	}

	private void OnPlayerRemoved(ulong clientId)
	{
		if (_pointMarkers.Remove(clientId, out var value))
		{
			value.ForEach(delegate(MiniMapMarker m)
			{
				m.Destroy();
			});
		}
	}

	private void OnPointMinimapPressed(Unit _)
	{
		if (!_mapManager.RxIsFullScreen.Value || _inputController.IsPingPressed || _inputController.IsEmojiPressed)
		{
			return;
		}
		Vector2 screenPoint = _inputController.GetAimScreenPoint();
		RectTransform component = _view.ImageRoot.GetComponent<RectTransform>();
		RectTransformUtility.ScreenPointToLocalPointInRectangle(component, screenPoint, _cameraManager.UiCamera, out var localPoint);
		Vector3 position = new Vector3(localPoint.x / TextureSize * (OrthographicSize + OrthographicSize), 0f, localPoint.y / TextureSize * (OrthographicSize + OrthographicSize));
		if (_mapManager.GetPointLayer(position) == _mapManager.MapLayer && PhysicsUtility.Raycast(position, out var _, Const.LayerMask.PLANE))
		{
			if (!_pointMarkers.TryGetValue(_mapManager.MyClientId, out var value) || GetClosestMarkerIndex(value, localPoint) < 0)
			{
				_mapManager.AddPointMarker(position);
			}
			else
			{
				_mapManager.RemovePointMarker(GetClosestMarkerIndex(value, localPoint));
			}
		}
		else
		{
			_toastManager.Notice(TLocalizedText.PointMarkerRestrictedArea.LocalText);
		}
	}

	private int GetClosestMarkerIndex(List<MiniMapMarker> markers, Vector2 localPoint)
	{
		if (!markers.Any())
		{
			return -1;
		}
		float num = _pointMarkerPickRadius / _model.FullScreenZoomScale;
		float num2 = num * num;
		Vector3 vector = _view.ObjectMarkerRoot.transform.InverseTransformDirection(Vector3.up);
		float num3 = vector.x * _pointMarkerPickCenterOffset / _model.FullScreenZoomScale;
		float num4 = vector.y * _pointMarkerPickCenterOffset / _model.FullScreenZoomScale;
		int result = -1;
		for (int i = 0; i < markers.Count; i++)
		{
			Vector3 localPosition = markers[i].MarkerGo.transform.localPosition;
			float num5 = num3 + localPosition.x - localPoint.x;
			float num6 = num4 + localPosition.y - localPoint.y;
			float num7 = num5 * num5 + num6 * num6;
			if (num7 < num2)
			{
				num2 = num7;
				result = i;
			}
		}
		return result;
	}

	private void RebuildPointMarkers(GamePlayer player)
	{
		ulong clientId = player.ClientId;
		Transform markerRootTf = _view.ObjectMarkerRoot.transform;
		Transform indicatorRootTf = _view.IndicatorCanvas.transform;
		List<MiniMapMarker> list = BuildMarkers();
		_pointMarkers[clientId] = list;
		if (!list.Any())
		{
			return;
		}
		ProtoVector3[] positions = player.RxPointMarkers.Value.Positions;
		Color playerColor = player.GetPlayerColor();
		for (int i = 0; i < list.Count; i++)
		{
			MiniMapMarker miniMapMarker = list[i];
			Vector3 vector = positions[i].ToVector3();
			miniMapMarker.TargetPosition = vector;
			miniMapMarker.MapLayer = _mapManager.GetPointLayer(vector);
			miniMapMarker.MarkerGo.transform.localPosition = WorldToMiniMapPoint(vector);
			string text = (i + 1).ToString();
			miniMapMarker.MarkerGo.FindChildComponent<TextMeshProUGUI>("Text").text = text;
			miniMapMarker.IndicatorGo.FindChildComponent<TextMeshProUGUI>("Text").text = text;
			miniMapMarker.MarkerGo.FindChildComponent<Image>("Icon").color = playerColor;
			miniMapMarker.IndicatorGo.FindChildComponent<Image>("Icon").color = playerColor;
			bool flag = miniMapMarker.MapLayer == _mapManager.MapLayer;
			if (miniMapMarker.MarkerGo.activeSelf != flag)
			{
				miniMapMarker.MarkerGo.SetActive(flag);
			}
		}

		List<MiniMapMarker> BuildMarkers()
		{
			if (!_pointMarkers.TryGetValue(clientId, out var value))
			{
				value = new List<MiniMapMarker>();
			}
			// disassembly: Positions?.Length is built as Nullable<int> (null-check on Value, then Positions, then Length) and read back with GetValueOrDefault (0) @0x18081fc70
			int num = player.RxPointMarkers.Value?.Positions?.Length ?? 0;
			if (value.Count < num)
			{
				int num2 = num - value.Count;
				for (int j = 0; j < num2; j++)
				{
					GameObject markerGo = CreateMarker("PlayerPointMarker", Vector3.zero, active: true, markerRootTf);
					GameObject indicatorGo = CreateMarker("PlayerPointMarker", Vector3.zero, active: false, indicatorRootTf);
					value.Add(new MiniMapMarker(0, markerGo, Vector3.zero, indicatorGo));
				}
			}
			else if (num < value.Count)
			{
				while (value.Count > num)
				{
					MiniMapMarker miniMapMarker2 = value[value.Count - 1];
					miniMapMarker2.Destroy();
					value.RemoveAt(value.Count - 1);
				}
			}
			return value;
		}
	}

	private bool IsTargetInSight(Vector3 targetPosition)
	{
		if (_gameManager.IsRevealingMapMap)
		{
			return true;
		}
		if (!_playerSights.Values.Any())
		{
			return false;
		}
		return _playerSights.Values.Any(delegate(PlayerSight sight)
		{
			Vector3 vector = targetPosition - sight.Position;
			float magnitude = vector.magnitude;
			if (magnitude <= sight.SightBackRange)
			{
				return true;
			}
			if (magnitude <= sight.SightRange)
			{
				Vector3 normalized = new Vector3(vector.x, 0f, vector.z).normalized;
				Vector3 forward = sight.Forward;
				Vector3 normalized2 = new Vector3(forward.x, 0f, forward.z).normalized;
				return Vector3.Angle(normalized2, normalized) <= sight.SightAngle * 0.5f;
			}
			return false;
		});
	}
}
