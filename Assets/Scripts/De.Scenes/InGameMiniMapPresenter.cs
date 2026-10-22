using System.Collections.Generic;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
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

		public Vector3 Position => default;

		public Vector3 Forward => default;

		public float SightRange => 0f;

		public float SightAngle => 0f;

		public float SightBackRange => 0f;
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

		public Transform MarkerTf => null;

		public Transform IndicatorTf => null;

		public bool IsMarkerActive => false;

		public bool IsIndicatorActive => false;

		public MiniMapMarker(ulong networkObjectId, GameObject markerGo, Vector3 targetPosition, GameObject indicatorGo = null)
		{
		}

		public MiniMapMarker(int customId, GameObject markerGo, Vector3 targetPosition, GameObject indicatorGo = null)
		{
		}

		public void SetActiveMarker(bool active)
		{
		}

		public void SetActiveIndicator(bool active)
		{
		}

		public void SetActive(bool active)
		{
		}

		public void SetLocalScale(Vector3 localScale)
		{
		}

		public void Destroy()
		{
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
	private float _pointMarkerPickRadius;

	[SerializeField]
	private float _pointMarkerPickCenterOffset;

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

	private readonly List<Image> _sectorImages;

	private readonly Dictionary<ulong, PlayerSight> _playerSights;

	private readonly Dictionary<ulong, MiniMapMarker> _playerMarkers;

	private readonly Dictionary<ulong, MiniMapMarker> _monsterMarkers;

	private readonly Dictionary<int, MiniMapMarker> _questMarkers;

	private readonly Dictionary<ulong, List<MiniMapMarker>> _pointMarkers;

	private readonly List<MiniMapMarker> _objectMarkers;

	private readonly List<MiniMapMarker> _campaignTaskRelatedMarkers;

	private float OrthographicSize => 0f;

	private float TextureSize => 0f;

	public (int Width, int Height) MapSize => default;

	private bool IsRevealingMapMap => false;

	private ulong ViewTargetClientId => 0uL;

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnInject(InGameMiniMapModel model)
	{
	}

	private void InitMiniMapImages()
	{
	}

	private void OnMapLayerChanged(int mapLayer)
	{
	}

	private void HideLayerMarkers()
	{
	}

	private void ApplyLayerMarkers(int mapLayer)
	{
	}

	private void Update()
	{
	}

	private void OnPingTriggered((EPingType pingType, Vector2 inputPosition) data)
	{
	}

	private void OnFullScreen(bool isFullScreen)
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnMainPlayerPositionChanged(Vector3 position)
	{
	}

	private void OnMainPlayerForwardChanged(Vector3 forward)
	{
	}

	private void OnUnitSpawned(UnitModel unitModel)
	{
	}

	private void OnUnitSpawned(UnitController unitController)
	{
	}

	private void OnUnitDespawned(UnitController unitController)
	{
	}

	private void EnsurePlayerMarker(CharacterModel characterModel)
	{
	}

	private void OnCharacterSpawned(CharacterModel characterModel)
	{
	}

	private void OnCharacterDespawned(CharacterController characterController)
	{
	}

	private void OnCharacterPositionChanged(ulong clientId, Vector3 position)
	{
	}

	private void OnCharacterForwardChanged(ulong clientId, Vector3 forward)
	{
	}

	private void OnCharacterStateChanged(ulong clientId, EActionState state)
	{
	}

	private void OnMonsterSpawned(MonsterModel monsterModel)
	{
	}

	private void OnMonsterDespawned(MonsterController monsterController)
	{
	}

	private void OnMonsterPositionChanged(ulong networkObjectId, Vector3 position)
	{
	}

	private void OnMonsterDead(ulong networkObjectId, bool isBoss)
	{
	}

	private void OnSpawnQuestMarker(CampaignMarkerData data)
	{
	}

	private void OnDespawnQuestMarker(int campaignTaskId)
	{
	}

	private void OnInteractiveObjectSpawned(IInteractiveObject interactiveObject)
	{
	}

	private void OnInteractiveObjectDespawned(IInteractiveObject interactiveObject)
	{
	}

	private MiniMapMarker CreateObjectMarker(ulong networkObjectId, string markerName, Vector3 position, bool active, bool useIndicator = false)
	{
		return null;
	}

	private void DestroyObjectMarker(ulong networkObjectId)
	{
	}

	private MiniMapMarker CreateQuestBoxMarker(RewardBox rewardBox)
	{
		return null;
	}

	private MiniMapMarker CreateQuestDropGoodsMarker(DropGoods dropGoods)
	{
		return null;
	}

	private void DestroyTaskRelatedMarker(ulong networkObjectId)
	{
	}

	private void OnPingObjectSpawned(PingObject pingObject)
	{
	}

	private void OnPingObjectDespawned(int instanceId)
	{
	}

	private void OnSpawnCustomMarker(CustomMarkerData data)
	{
	}

	private void OnDespawnCustomMarker(int id)
	{
	}

	private void OnSpawnTextMarker(TextMarkerData data)
	{
	}

	private bool TryCreateMarker(string markerName, Vector3 position, bool active, Transform rootTf, out GameObject go)
	{
		go = null;
		return false;
	}

	private GameObject CreateMarker(string markerName, Vector3 position, bool active, Transform rootTf)
	{
		return null;
	}

	private void UpdateMiniMapPosition()
	{
	}

	private void UpdateMarkersInSight()
	{
	}

	private void UpdateIndicators()
	{
	}

	private void UpdateMarkerScales()
	{
	}

	private Vector3 GetMarkerScale()
	{
		return default;
	}

	private float WorldToMiniMapDistance(float worldDistance)
	{
		return 0f;
	}

	private float MiniMapToWorldDistance(float miniMapDistance)
	{
		return 0f;
	}

	private Vector3 WorldToMiniMapPoint(Vector3 position)
	{
		return default;
	}

	private Vector3 MiniMapPointToWorld(Vector3 miniMapPoint)
	{
		return default;
	}

	private Quaternion WorldToMiniMapRotation(Vector3 forward)
	{
		return default;
	}

	private float GetFullScreenZoomScale()
	{
		return 0f;
	}

	private (int, int, int, int) GetLayerBounds()
	{
		return default;
	}

	private void InitPlayerPointMarkers()
	{
	}

	private void OnPlayerAdded(GamePlayer player)
	{
	}

	private void OnPlayerRemoved(ulong clientId)
	{
	}

	private void OnPointMinimapPressed(Unit _)
	{
	}

	private int GetClosestMarkerIndex(List<MiniMapMarker> markers, Vector2 localPoint)
	{
		return 0;
	}

	private void RebuildPointMarkers(GamePlayer player)
	{
	}

	private bool IsTargetInSight(Vector3 targetPosition)
	{
		return false;
	}
}
