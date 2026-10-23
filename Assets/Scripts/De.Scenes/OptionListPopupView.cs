using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionListPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private GameObject textSlotScroller;

	[SerializeField]
	private Image scroller;

	public GameObject Contents => contents;

	public TextMeshProUGUI Text => text;

	public GameObject TextSlotScroller => textSlotScroller;

	public Image Scroller => scroller;
}
