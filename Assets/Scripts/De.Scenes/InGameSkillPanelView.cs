using NemoLib.Mvp;
using UnityEngine;

public class InGameSkillPanelView : View
{
	[SerializeField]
	private InGameSkillTooltipPresenter inGameSkillTooltip;

	[SerializeField]
	private GameObject[] slots;

	public InGameSkillTooltipPresenter InGameSkillTooltip => inGameSkillTooltip;

	public GameObject[] Slots => slots;
}
