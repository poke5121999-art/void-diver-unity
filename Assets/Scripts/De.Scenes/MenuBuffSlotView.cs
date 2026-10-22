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

	public Button Normal => normal;

	public Image Bg => bg;

	public Image IconBuff => iconBuff;

	public TextMeshProUGUI IconBuff_TextStack => iconBuff_TextStack;

	public TextMeshProUGUI TextDesc => textDesc;
}
