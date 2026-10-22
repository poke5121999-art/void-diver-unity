using System.Collections.Generic;
using System.Linq;
using EnhancedUI.EnhancedScroller;
using NemoLib.Log;
using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

	protected override BaseScrollerModel<ChatSlotModel> ScrollerModel => _model;

	protected override int GetColumnCount()
	{
		return 1;
	}

	private void OnInject(ChatScrollerModel model)
	{
		RefreshScroller(1f);
	}

	protected override List<ChatSlotModel> BuildSlotModels()
	{
		ChatSlotModel spacer = new ChatSlotModel
		{
			IsSpacer = true
		};
		List<ChatSlotModel> list = new List<ChatSlotModel> { spacer };
		SyncPreviewTextWithSlot();
		_previewText.alpha = 0f;
		list.AddRange(_model.ChatInfoList.Select(delegate(ChatInfo info)
		{
			_previewText.text = info.Text;
			_previewText.ForceMeshUpdate();
			LayoutRebuilder.ForceRebuildLayoutImmediate(_previewText.rectTransform);
			float y = _previewText.GetRenderedValues(onlyVisibleCharacters: false).y;
			return new ChatSlotModel
			{
				IsSpacer = false,
				ChatInfo = info,
				CellSize = y
			};
		}).ToList());
		float num = Scroller.padding.top + Scroller.padding.bottom;
		for (int i = 1; i < list.Count; i++)
		{
			if (i < list.Count - 1)
			{
				num += Scroller.spacing + list[i].CellSize;
			}
			else
			{
				num += list[i].CellSize + 0f;
			}
		}
		spacer.CellSize = Mathf.Max(0f, Scroller.ScrollRectSize - num);
		return list;
	}

	private void SyncPreviewTextWithSlot()
	{
		TextMeshProUGUI text = _cellViewPrefab.GetComponent<ChatSlotView>().Text;
		_previewText.font = text.font;
		_previewText.fontSize = text.fontSize;
		_previewText.alpha = 0f;
	}

	private Vector2 GetPreviewTextRenderValue(string text)
	{
		_previewText.text = text;
		_previewText.ForceMeshUpdate();
		LayoutRebuilder.ForceRebuildLayoutImmediate(_previewText.rectTransform);
		Bounds textBounds = _previewText.textBounds;
		return new Vector2(textBounds.extents.x * 2f, textBounds.extents.y * 2f);
	}

	public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		BaseCell baseCell = scroller.GetCellView(_cellViewPrefab) as BaseCell;
		ChatSlotModel chatSlotModel = CellViewModels[dataIndex];
		string name = (chatSlotModel.IsSpacer ? "Spacer" : string.Format("Cell : {0}", dataIndex));
		baseCell.name = name;
		baseCell.SetData(chatSlotModel);
		return baseCell;
	}

	public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return CellViewModels[dataIndex].CellSize;
	}
}
