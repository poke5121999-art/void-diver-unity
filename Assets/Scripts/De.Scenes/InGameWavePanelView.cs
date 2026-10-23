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

	public GameObject Anchor => anchor;

	public Image BG => bG;

	public TextMeshProUGUI NoticeText => noticeText;

	public TextMeshProUGUI TimeText => timeText;
}
