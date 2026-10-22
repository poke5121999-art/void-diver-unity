using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuffSlotView : View
{
	[SerializeField]
	private Button normal;

	[SerializeField]
	private Image bg;

	[SerializeField]
	private Image iconBuff;

	[SerializeField]
	private TextMeshProUGUI iconBuff_TextStack;

	[SerializeField]
	private TextMeshProUGUI textDesc;

	public Button Normal => null;

	public Image Bg => null;

	public Image IconBuff => null;

	public TextMeshProUGUI IconBuff_TextStack => null;

	public TextMeshProUGUI TextDesc => null;
}
