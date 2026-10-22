using NemoLib.Mvp;
using UnityEngine;

public class InGameHudView : View
{
	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private PingEmojiPanelPresenter pingEmojiPanel;

	[SerializeField]
	private GameObject top;

	public GameObject Bottom => bottom;

	public PingEmojiPanelPresenter PingEmojiPanel => pingEmojiPanel;

	public GameObject Top => top;
}
