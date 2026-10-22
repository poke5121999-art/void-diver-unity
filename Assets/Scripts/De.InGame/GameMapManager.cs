using System.Collections.Generic;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using VContainer;

public class GameMapManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameHost _gameHost;

	[Inject]
	private GameChannel _gameChannel;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private LoungeMapSettings _loungeMapSettings;

	[Inject]
	private NetworkManager _networkManager;

	[Inject]
	private GameInputManager _inputManager;

	public const float SECTOR_SIZE = 30f;

	public const float BOUNDARY_SPECIAL_FIELD_THICKNESS = 2f;

	private int _sectorIndex;

	private CompositeDisposable _disposable;

	public ulong MyClientId => 0uL;

	public ReactiveDictionary<int, CustomMarkerData> RxCustomMarkerDatas { get; }

	public ReactiveDictionary<int, CampaignMarkerData> RxCampaignMarkerDatas { get; }

	public ReactiveCollection<TextMarkerData> RxTextMarkerDatas { get; }

	public IDictionary<int, CustomMarkerData> CustomMarkerDatas => null;

	public IDictionary<int, CampaignMarkerData> CampaignMarkerDatas => null;

	public IList<TextMarkerData> TextMarkerDatas => null;

	public ReactiveProperty<int> RxMapLayer { get; }

	public int MapLayer => 0;

	public ReactiveProperty<int> RxCurrentSectorId { get; }

	public int CurrentSectorId => 0;

	public ReactiveProperty<bool> RxIsFullScreen { get; }

	public (int Width, int Height) MapSize => default;

	public int[] SectorIds => null;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void AddPointMarker(Vector3 worldPos)
	{
	}

	public void RemovePointMarker(int markerIndex)
	{
	}

	public void SpawnCustomMarker(CustomMarkerData data)
	{
	}

	public void DespawnCustomMarker(int id)
	{
	}

	public void SpawnTextMarker(TextMarkerData data)
	{
	}

	public void SpawnQuestMarker(CampaignMarkerData data)
	{
	}

	private void ClearAllPointMarkers()
	{
	}

	private void OnPrepareLoad()
	{
	}

	private void OnStageClosed()
	{
	}

	private void OnCampaignTaskAchieved(int campaignTaskId)
	{
	}

	private void OnFullScreen(bool isFullScreen)
	{
	}

	private void OnViewTargetChanged(ulong clientId)
	{
	}

	private void OnViewTargetPositionChanged(Vector3 position)
	{
	}

	private void UpdateMapLayer(int sectorIndex)
	{
	}

	public int GetSectorMapLayer(int sectorIndex)
	{
		return 0;
	}

	public int GetPointLayer(Vector3 position)
	{
		return 0;
	}

	private void OnUpdateMapInfo(NtfUpdateMapInfo message)
	{
	}

	public int[] PickStageSectorIds(EThemeType themeType, ESectorAreaType areaType, int count, IEnumerable<int> excludeSectorIds = null)
	{
		return null;
	}

	private int GetSectorIndex(Vector3 position)
	{
		return 0;
	}

	private int GetSectorIndex(int indexX, int indexZ)
	{
		return 0;
	}

	public Vector3 GetSectorOffset(int index)
	{
		return default;
	}

	public Vector3 GetSectorOffset(int indexX, int indexZ)
	{
		return default;
	}

	private void Clear()
	{
	}

	private void ApplyFixedSectors(ref int[] sectorIds, TCampaign campaignData)
	{
	}
}
