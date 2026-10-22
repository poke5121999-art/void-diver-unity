using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class TalentInfoPanelView : View
{
	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private TalentTooltipPresenter talentTooltip;

	[SerializeField]
	private UnlockMaterialPanelPresenter unlockMaterialPanel;

	[SerializeField]
	private GameObject upgrade;

	[SerializeField]
	private Button upgrade_Button;

	public GameObject Empty => empty;

	public GameObject Info => info;

	public TalentTooltipPresenter TalentTooltip => talentTooltip;

	public UnlockMaterialPanelPresenter UnlockMaterialPanel => unlockMaterialPanel;

	public GameObject Upgrade => upgrade;

	public Button Upgrade_Button => upgrade_Button;
}
