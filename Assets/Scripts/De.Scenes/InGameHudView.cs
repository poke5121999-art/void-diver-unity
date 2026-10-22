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

	public GameObject Bottom => null;

	public PingEmojiPanelPresenter PingEmojiPanel => null;

	public GameObject Top => null;
}
