using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Data;
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

	public ulong MyClientId => _networkManager.LocalClientId;

	public ReactiveDictionary<int, CustomMarkerData> RxCustomMarkerDatas { get; } = new ReactiveDictionary<int, CustomMarkerData>();

	public ReactiveDictionary<int, CampaignMarkerData> RxCampaignMarkerDatas { get; } = new ReactiveDictionary<int, CampaignMarkerData>();

	public ReactiveCollection<TextMarkerData> RxTextMarkerDatas { get; } = new ReactiveCollection<TextMarkerData>();

	public IDictionary<int, CustomMarkerData> CustomMarkerDatas => RxCustomMarkerDatas;

	public IDictionary<int, CampaignMarkerData> CampaignMarkerDatas => RxCampaignMarkerDatas;

	public IList<TextMarkerData> TextMarkerDatas => RxTextMarkerDatas;

	public ReactiveProperty<int> RxMapLayer { get; } = new ReactiveProperty<int>();

	public int MapLayer => RxMapLayer.Value;

	public ReactiveProperty<int> RxCurrentSectorId { get; } = new ReactiveProperty<int>();

	public int CurrentSectorId => RxCurrentSectorId.Value;

	public ReactiveProperty<bool> RxIsFullScreen { get; } = new ReactiveProperty<bool>(false);

	public (int Width, int Height) MapSize => (_gameContext.MapSize[0], _gameContext.MapSize[1]);

	public int[] SectorIds => _gameContext.SectorIds;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.PrepareLoad).Subscribe(delegate
		{
			OnPrepareLoad();
		}).AddTo(this);
		_gameManager.RxStageState.Where((EStageState _) => _ == EStageState.Closed).Subscribe(delegate
		{
			OnStageClosed();
		}).AddTo(this);
		_gameManager.RxViewTargetClientId.Subscribe(OnViewTargetChanged).AddTo(this);
		_gameChannel.Receive<NtfUpdateMapInfo>().Subscribe(OnUpdateMapInfo).AddTo(this);
		_campaignManager.OnTaskAchieved.Subscribe(OnCampaignTaskAchieved).AddTo(this);
		RxIsFullScreen.Subscribe(OnFullScreen).AddTo(this);
	}

	public void AddPointMarker(Vector3 worldPos)
	{
		_gameChannel.SendToHost(new ReqAddPointMarker
		{
			Position = worldPos.ToProtoVector3()
		});
	}

	public void RemovePointMarker(int markerIndex)
	{
		_gameChannel.SendToHost(new ReqRemovePointMarker
		{
			MarkerIndex = markerIndex
		});
	}

	public void SpawnCustomMarker(CustomMarkerData data)
	{
		if (data != null)
		{
			RxCustomMarkerDatas.Add(data.CustomId, data);
		}
	}

	public void DespawnCustomMarker(int id)
	{
		RxCustomMarkerDatas.Remove(id);
	}

	public void SpawnTextMarker(TextMarkerData data)
	{
		RxTextMarkerDatas.Add(data);
	}

	public void SpawnQuestMarker(CampaignMarkerData data)
	{
		if (data != null && !_campaignManager.IsTaskAchieved(data.TaskId))
		{
			RxCampaignMarkerDatas.Add(data.TaskId, data);
		}
	}

	private void ClearAllPointMarkers()
	{
		_gameManager.AssertIfNotHost();
		PointMarkers pointMarkers = new PointMarkers
		{
			Positions = Array.Empty<ProtoVector3>()
		};
		foreach (GamePlayer value in _gameContext.Players.Values)
		{
			value.PointMarkers = pointMarkers;
			_gameHost.SendToSnapshotApplied(new NtfUpdateGamePlayer
			{
				ChangeData = value.Flush()
			});
		}
	}

	private void OnPrepareLoad()
	{
		_gameManager.AssertIfNotHost();
		ClearAllPointMarkers();
		if (_gameContext.IsInLounge)
		{
			_gameContext.MapSize = _loungeMapSettings.MapSize;
			_gameContext.SectorIds = (int[])_loungeMapSettings.SectorIds.Clone();
		}
		else
		{
			TCampaign tCampaign = TCampaign.Table[_gameContext.CampaignId];
			_gameContext.MapSize = tCampaign.MapSize;
			int num = tCampaign.MapSize[0];
			int num2 = tCampaign.MapSize[1];
			List<int> list = (from s in tCampaign.FixedSectors
				where s.SectorId > 0
				select s.SectorId).ToList();
			if (tCampaign.RandomPlacedSectorIds != null)
			{
				list.AddRange(tCampaign.RandomPlacedSectorIds);
			}
			int[] sectorIds = PickStageSectorIds(tCampaign.ThemeType, ESectorAreaType.Playable, num * num2, list);
			ApplyFixedSectors(ref sectorIds, tCampaign);
			_gameContext.SectorIds = sectorIds;
		}
		_gameHost.SendToAll(new NtfUpdateMapInfo
		{
			MapSize = _gameContext.MapSize,
			SectorIds = _gameContext.SectorIds
		});
	}

	private void OnStageClosed()
	{
		RxIsFullScreen.Value = false;
		RxCampaignMarkerDatas.Clear();
		RxCustomMarkerDatas.Clear();
		RxTextMarkerDatas.Clear();
	}

	private void OnCampaignTaskAchieved(int campaignTaskId)
	{
		RxCampaignMarkerDatas.Remove(campaignTaskId);
	}

	private void OnFullScreen(bool isFullScreen)
	{
		if (isFullScreen)
		{
			_inputManager.BlockInput(EInputActionType.PlayerFunc);
		}
		else
		{
			_inputManager.UnblockInput(EInputActionType.PlayerFunc);
		}
	}

	private void OnViewTargetChanged(ulong clientId)
	{
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CharacterModel characterModel = _gameManager.FindCharacterModel(clientId);
		if (characterModel == null)
		{
			Clear();
		}
		else
		{
			characterModel.RxPosition.Subscribe(OnViewTargetPositionChanged).AddTo(_disposable);
		}
	}

	private void OnViewTargetPositionChanged(Vector3 position)
	{
		int sectorIndex = GetSectorIndex(position);
		int num = -1;
		if (sectorIndex >= 0)
		{
			num = _gameContext.SectorIds[sectorIndex];
		}
		RxCurrentSectorId.Value = num;
		if (_sectorIndex != sectorIndex)
		{
			_sectorIndex = sectorIndex;
			UpdateMapLayer(sectorIndex);
		}
	}

	private void UpdateMapLayer(int sectorIndex)
	{
		RxMapLayer.Value = GetSectorMapLayer(sectorIndex);
	}

	public int GetSectorMapLayer(int sectorIndex)
	{
		if (_gameContext.IsInLounge || sectorIndex < 0)
		{
			return 0;
		}
		FixedSector[] fixedSectors = TCampaign.Table[_gameContext.CampaignId].FixedSectors;
		if (fixedSectors == null || fixedSectors.Length == 0)
		{
			return 0;
		}
		for (int i = 0; i < fixedSectors.Length; i++)
		{
			FixedSector fixedSector = fixedSectors[i];
			if (fixedSector.IndexX == sectorIndex % MapSize.Width && fixedSector.IndexZ == sectorIndex / MapSize.Width)
			{
				return fixedSector.Layer;
			}
		}
		return -1;
	}

	public int GetPointLayer(Vector3 position)
	{
		int sectorIndex = (int)(position.z / 30f) * MapSize.Width + (int)(position.x / 30f);
		if (_gameContext.IsInLounge || sectorIndex < 0)
		{
			return 0;
		}
		FixedSector[] fixedSectors = TCampaign.Table[_gameContext.CampaignId].FixedSectors;
		if (fixedSectors == null || fixedSectors.Length == 0)
		{
			return 0;
		}
		for (int i = 0; i < fixedSectors.Length; i++)
		{
			FixedSector fixedSector = fixedSectors[i];
			if (fixedSector.IndexX == sectorIndex % MapSize.Width && fixedSector.IndexZ == sectorIndex / MapSize.Width)
			{
				return fixedSector.Layer;
			}
		}
		return -1;
	}

	private void OnUpdateMapInfo(NtfUpdateMapInfo message)
	{
		if (!_gameManager.IsHost)
		{
			_gameContext.MapSize = message.MapSize;
			_gameContext.SectorIds = message.SectorIds;
		}
	}

	public int[] PickStageSectorIds(EThemeType themeType, ESectorAreaType areaType, int count, IEnumerable<int> excludeSectorIds = null)
	{
		IList<int> list = (from s in TSector.Table.Values
			where s.ThemeType == themeType
			where s.AreaType == areaType
			where excludeSectorIds == null || !excludeSectorIds.Contains(s.Id)
			select s.Id).ToList();
		int[] array = new int[count];
		for (int i = 0; i < count; i++)
		{
			if (i % list.Count == 0)
			{
				list = list.Shuffle();
			}
			array[i] = list[i % list.Count];
		}
		return array;
	}

	private int GetSectorIndex(Vector3 position)
	{
		if (position.x >= 0f && position.z >= 0f && position.x < (float)MapSize.Width * 30f && position.z < (float)MapSize.Height * 30f)
		{
			return MapSize.Width * (int)(position.z / 30f) + (int)(position.x / 30f);
		}
		return -1;
	}

	private int GetSectorIndex(int indexX, int indexZ)
	{
		return MapSize.Width * indexZ + indexX;
	}

	public Vector3 GetSectorOffset(int index)
	{
		return new Vector3((float)(index % MapSize.Width) * 30f, 0f, (float)(index / MapSize.Width) * 30f);
	}

	public Vector3 GetSectorOffset(int indexX, int indexZ)
	{
		int index = GetSectorIndex(indexX, indexZ);
		return new Vector3((float)(index % MapSize.Width) * 30f, 0f, (float)(index / MapSize.Width) * 30f);
	}

	private void Clear()
	{
		RxIsFullScreen.Value = false;
		_sectorIndex = -1;
		RxMapLayer.Value = 0;
		RxCurrentSectorId.Value = -1;
	}

	private void ApplyFixedSectors(ref int[] sectorIds, TCampaign campaignData)
	{
		int num = sectorIds.Length;
		List<int> list = Enumerable.Range(0, num).ToList();
		FixedSector[] fixedSectors = campaignData.FixedSectors;
		if (fixedSectors == null)
		{
			return;
		}
		FixedSector[] array = fixedSectors;
		for (int i = 0; i < array.Length; i++)
		{
			FixedSector fixedSector = array[i];
			int num2 = fixedSector.IndexZ * MapSize.Width + fixedSector.IndexX;
			if (num2 >= 0 && num2 < num && fixedSector.SectorId != 0)
			{
				sectorIds[num2] = fixedSector.SectorId;
				list.Remove(num2);
			}
		}
		if (campaignData.RandomPlacedSectorIds == null)
		{
			return;
		}
		System.Random random = new System.Random();
		int[] randomPlacedSectorIds = campaignData.RandomPlacedSectorIds;
		foreach (int num3 in randomPlacedSectorIds)
		{
			if (list.Count == 0)
			{
				break;
			}
			int index = random.Next(list.Count);
			sectorIds[list[index]] = num3;
			list.RemoveAt(index);
		}
	}
}
