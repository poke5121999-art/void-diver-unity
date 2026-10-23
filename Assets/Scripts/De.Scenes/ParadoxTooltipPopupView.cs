using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxTooltipPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private ParadoxTooltipPresenter paradoxTooltip;

	public Button Background => background;

	public GameObject Contents => contents;

	public ParadoxTooltipPresenter ParadoxTooltip => paradoxTooltip;
}
