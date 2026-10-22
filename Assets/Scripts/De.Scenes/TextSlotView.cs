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

	public Image Normal => normal;

	public TextMeshProUGUI Text => text;
}
