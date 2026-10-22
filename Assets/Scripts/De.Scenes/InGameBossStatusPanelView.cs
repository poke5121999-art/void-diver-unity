using NemoLib.Mvp;
using UnityEngine;

public class InGameBossStatusPanelView : View
{
	[SerializeField]
	private BuffTooltipPresenter buffTooltip;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject[] bossSlots;

	public BuffTooltipPresenter BuffTooltip => null;

	public GameObject Contents => null;

	public GameObject[] BossSlots => null;
}
