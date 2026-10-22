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

	public Button Background => null;

	public GameObject Contents => null;

	public ParadoxTooltipPresenter ParadoxTooltip => null;
}
