using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRestoreSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI timeText;

	public Image Normal => normal;

	public TextMeshProUGUI TimeText => timeText;
}
