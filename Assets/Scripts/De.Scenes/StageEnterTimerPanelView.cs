using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageEnterTimerPanelView : View
{
	[SerializeField]
	private GameObject anchor;

	[SerializeField]
	private Image bG;

	[SerializeField]
	private TextMeshProUGUI noticeText;

	[SerializeField]
	private TextMeshProUGUI timeText;

	[SerializeField]
	private Image bG_Enter;

	public GameObject Anchor => null;

	public Image BG => null;

	public TextMeshProUGUI NoticeText => null;

	public TextMeshProUGUI TimeText => null;

	public Image BG_Enter => null;
}
