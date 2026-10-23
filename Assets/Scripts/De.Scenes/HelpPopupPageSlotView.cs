using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelpPopupPageSlotView : View
{
	[SerializeField]
	private Image icon;

	[SerializeField]
	private TextMeshProUGUI text;

	public Image Icon => icon;

	public TextMeshProUGUI Text => text;
}
