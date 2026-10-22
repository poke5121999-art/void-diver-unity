using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using NemoLib.Log;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using VContainer;

public class ChatScrollerPresenter : VerticalScrollerPresenter<ChatSlotModel>
{
	[SerializeField]
	private EnhancedScrollerCellView _cellViewPrefab;

	[SerializeField]
	private TextMeshProUGUI _previewText;

	[InjectModel]
	private ChatScrollerModel _model;

	[Inject]
	private GameChatManager _gameChatManager;

	[Inject]
	private SeriLogger _logger;

	protected override BaseScrollerModel<ChatSlotModel> ScrollerModel => null;

	protected override int GetColumnCount()
	{
		return 0;
	}

	private void OnInject(ChatScrollerModel model)
	{
	}

	protected override List<ChatSlotModel> BuildSlotModels()
	{
		return null;
	}

	private void SyncPreviewTextWithSlot()
	{
	}

	private Vector2 GetPreviewTextRenderValue(string text)
	{
		return default;
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		return null;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 0f;
	}
}
