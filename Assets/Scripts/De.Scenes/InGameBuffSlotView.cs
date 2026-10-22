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

	public Slider Gauge => null;

	public Image Fill => null;

	public Image Icon => null;

	public TextMeshProUGUI TextStack => null;
}
