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

	public BuffTooltipPresenter BuffTooltip => buffTooltip;

	public GameObject Contents => contents;

	public GameObject[] BossSlots => bossSlots;
}
