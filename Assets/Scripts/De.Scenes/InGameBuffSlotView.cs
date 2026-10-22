using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameBuffSlotView : View
{
	[SerializeField]
	private Slider gauge;

	[SerializeField]
	private Image fill;

	[SerializeField]
	private Image icon;

	[SerializeField]
	private TextMeshProUGUI textStack;

	public Slider Gauge => gauge;

	public Image Fill => fill;

	public Image Icon => icon;

	public TextMeshProUGUI TextStack => textStack;
}
