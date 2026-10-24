using GameCommon.Enum;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ChatSlotPresenter : BaseSlotPresenter<ChatSlotModel>
{
	[InjectModel]
	private ChatSlotModel _model;

	[View]
	private ChatSlotView _view;

	[Inject]
	private GameManager _gameManager;

	protected override ChatSlotModel SlotModel => _model;

	protected override Button Button => null;

	protected void OnInject(ChatSlotModel model)
	{
		InitSlot(model);
		if (model.IsSpacer)
		{
			_view.Text.text = string.Empty; // the spacer text is **(0x1864889d0 + 0xb8) = string.Empty (machine code checked)
			return;
		}
		_view.Text.gameObject.SetActive(!model.IsSpacer);
		_view.Text.text = model.ChatInfo.Text;
		string html;
		switch (_model.ChatInfo.Type)
		{
		case EChatType.User:
			html = ((_model.ChatInfo.ClientId == _gameManager.LocalClientId) ? "#b3d465" : "#DCDCDC");
			break;
		case EChatType.Ping:
			html = "#58CECA";
			break;
		case EChatType.System:
			html = "#898989";
			break;
		case EChatType.Quest:
		case EChatType.Radio:
		case EChatType.Boss:
			html = "#A45646";
			break;
		default:
			html = "#FF00FF";
			break;
		}
		Color color;
		_view.Text.color = (ColorUtility.TryParseHtmlString(html, out color) ? color : Color.white);
	}
}
