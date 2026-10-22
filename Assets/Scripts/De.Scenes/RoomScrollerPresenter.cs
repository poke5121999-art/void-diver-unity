using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class RoomScrollerPresenter : VerticalScrollerPresenter<RoomSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView cellViewPrefab;

	[SerializeField]
	private float cellSize;

	[InjectModel]
	private RoomScrollerModel _model;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private SeriLogger _logger;

	private CompositeDisposable _disposable;

	private CompositeDisposable _injectDisposable;

	protected override BaseScrollerModel<RoomSlotModel> ScrollerModel => _model;

	public int ItemCount => CellViewModels.Count;

	protected override int GetColumnCount()
	{
		return 1;
	}

	private void OnInject(RoomScrollerModel model)
	{
		_injectDisposable?.Dispose();
		_injectDisposable = new CompositeDisposable();
		model.RxIsInputEnabled.Where((bool _) => _).Subscribe(delegate
		{
			OnInputEnabled();
		}).AddTo(_injectDisposable);
		model.RxIsInputEnabled.Where((bool _) => !_).Subscribe(delegate
		{
			OnInputDisabled();
		}).AddTo(_injectDisposable);
	}

	protected override void OnDestroy()
	{
		_disposable?.Dispose();
		_injectDisposable?.Dispose();
		base.OnDestroy();
	}

	protected override List<RoomSlotModel> BuildSlotModels()
	{
		return new List<RoomSlotModel>();
	}

	public async UniTask RefreshRoomListAsync(LoungeFilterPopupModel filter)
	{
		_logger.Debug("Refreshing game room list with filter...", new Color(0f, 1f, 0f, 1f));
		MatchingLobby[] source = await _roomMatchmaker.GetLobbyListAsync(filter.BlockOtherAreas, filter.ShowPlayingLounge).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
		int maxPingMs = GetMaxPingMs(filter.MinPingFilterType);
		MatchingLobby[] source2 = source.Where((MatchingLobby lobby) => IsDifficultyAllowed(lobby, filter.BlockedDifficulties) && IsHostLevelAllowed(lobby, filter.MinHostLevel) && IsPingAllowed(lobby, maxPingMs)).ToArray();
		_disposable?.Dispose();
		_disposable = new CompositeDisposable();
		CellViewModels.Clear();
		List<RoomSlotModel> list = source2.Select((MatchingLobby _) => new RoomSlotModel
		{
			MatchingLobby = _
		}).ToList();
		list.ForEach(delegate(RoomSlotModel _)
		{
			_.OnClick.Subscribe(OnGameRoomCellClick).AddTo(_disposable);
		});
		CellViewModels.AddRange(list);
		Scroller.Delegate = this;
		Scroller.ReloadData();
		RefreshNav();
	}

	private static int GetMaxPingMs(ERoomPingFilterType filterType)
	{
		return filterType switch
		{
			ERoomPingFilterType.ExcellentOnly => 50, 
			ERoomPingFilterType.Normal => 200, 
			_ => int.MaxValue, 
		};
	}

	private static bool IsDifficultyAllowed(MatchingLobby lobby, HashSet<ERoomDifficulty> blocked)
	{
		if (blocked == null || blocked.Count == 0)
		{
			return true;
		}
		return !blocked.Contains(lobby.RoomDifficulty);
	}

	private static bool IsHostLevelAllowed(MatchingLobby lobby, int minHostLevel)
	{
		return lobby.HostLevel >= minHostLevel;
	}

	private bool IsPingAllowed(MatchingLobby lobby, int maxPingMs)
	{
		if (maxPingMs == int.MaxValue)
		{
			return true;
		}
		return lobby.PingTime <= maxPingMs;
	}

	private void OnGameRoomCellClick(RoomSlotModel slotModel)
	{
		_model.OnGameRoomClick.OnNext(slotModel.MatchingLobby);
		CellViewModels.ForEach(delegate(RoomSlotModel _)
		{
			_.RxIsSelected.Value = _ == slotModel;
		});
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(cellViewPrefab) as BaseCell;
		baseCell.SetData(CellViewModels[dataIndex]);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return cellSize;
	}
}
