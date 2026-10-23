using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class FadePopupView : View
{
	[SerializeField]
	private CanvasGroup contents;

	[SerializeField]
	private Image background;

	public CanvasGroup Contents => contents;

	public Image Background => background;
}
