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

	public GameObject Empty => null;

	public GameObject Info => null;

	public TalentTooltipPresenter TalentTooltip => null;

	public UnlockMaterialPanelPresenter UnlockMaterialPanel => null;

	public GameObject Upgrade => null;

	public Button Upgrade_Button => null;
}
