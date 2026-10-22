using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameWavePanelView : View
{
	[SerializeField]
	private GameObject anchor;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI noticeText;

	[SerializeField]
	private TextMeshProUGUI timeText;

	public GameObject Anchor => null;

	public Image BG => null;

	public TextMeshProUGUI NoticeText => null;

	public TextMeshProUGUI TimeText => null;
}
