using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI text;

	public Image Normal => null;

	public TextMeshProUGUI Text => null;
}
