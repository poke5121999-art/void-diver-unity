using NemoLib.Log;
using GameCommon.Enum;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class RoomSlotPresenter : BaseSlotPresenter<RoomSlotModel>
{
	[InjectModel]
	private RoomSlotModel _model;

	[View]
	private RoomSlotView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	protected override RoomSlotModel SlotModel => null;

	private void OnInject(RoomSlotModel model)
	{
		InitSlot(model);
		_view.TextName.text = model.MatchingLobby.PlayerName;
		_view.TextSteamId.text = model.MatchingLobby.AccountName;
		_view.TextTitle.text = model.MatchingLobby.Title;
		SetDifficulty();
		SetMemberCount();
		SetLevel();
		SetPingBars();
		SetPlayingState();
	}

	private void SetPlayingState()
	{
		_model.RxIsLocked.Value = _model.MatchingLobby.IsGameStarted;
	}

	private void SetDifficulty()
	{
		ERoomDifficulty roomDifficulty = _model.MatchingLobby.RoomDifficulty;
		_view.TextDifficulty.color = roomDifficulty.GetColor();
		_view.TextDifficulty.text = roomDifficulty.ToLocalText();
	}

	private void SetMemberCount()
	{
		_view.TextCapacity.text = string.Format("{0}/{1}", _model.MatchingLobby.MemberCount, _model.MatchingLobby.PlayerCountMax);
	}

	private void SetLevel()
	{
		_view.TextHostLevel.text = _model.MatchingLobby.HostLevel.ToString();
	}

	private void SetPingBars()
	{
		int pingTime = _model.MatchingLobby.PingTime;
		int num = ((pingTime <= 100) ? ((pingTime > 50) ? 1 : 0) : ((pingTime > 200) ? 3 : 2));
		for (int i = 0; i < _view.Ping.Length; i++)
		{
			_view.Ping[i].SetActive(i == num);
		}
		_logger.Debug(string.Format("LobbyId: {0}, Ping: {1} ms, Bars: {2}", _model.MatchingLobby.LobbyId, pingTime, num), new Color(0f, 1f, 0f, 1f));
	}
}
