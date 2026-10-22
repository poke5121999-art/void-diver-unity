using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPageView : View
{
	[SerializeField]
	private BuffTooltipPresenter buffTooltip;

	[SerializeField]
	private CharacterInfoPanelPresenter characterInfoPanel;

	[SerializeField]
	private EquipmentTooltipPresenter equipmentTooltip;

	[SerializeField]
	private GameObject escapeKeyGuide;

	[SerializeField]
	private GameObject pages;

	[SerializeField]
	private GameObject paradoxBuffPage;

	[SerializeField]
	private MenuBuffPanelPresenter buffPanel;

	[SerializeField]
	private ParadoxPanelPresenter negativeParadoxPanel;

	[SerializeField]
	private ParadoxPanelPresenter positiveParadoxPanel;

	[SerializeField]
	private GameObject skillTalentPage;

	[SerializeField]
	private GameObject skill;

	[SerializeField]
	private MenuSkillPanelPresenter skill_MenuSkillPanel;

	[SerializeField]
	private GameObject skill_SkillCaption;

	[SerializeField]
	private ParadoxTooltipPresenter paradoxTooltip;

	[SerializeField]
	private InGameSkillTooltipPresenter skillTooltip;

	[SerializeField]
	private GameObject tabs;

	[SerializeField]
	private GameObject paradoxBuffTab;

	[SerializeField]
	private Button paradoxBuffTab_Button;

	[SerializeField]
	private GameObject skillTalentTab;

	[SerializeField]
	private Button skillTalentTab_Button;

	public BuffTooltipPresenter BuffTooltip => null;

	public CharacterInfoPanelPresenter CharacterInfoPanel => null;

	public EquipmentTooltipPresenter EquipmentTooltip => null;

	public GameObject EscapeKeyGuide => null;

	public GameObject Pages => null;

	public GameObject ParadoxBuffPage => null;

	public MenuBuffPanelPresenter BuffPanel => null;

	public ParadoxPanelPresenter NegativeParadoxPanel => null;

	public ParadoxPanelPresenter PositiveParadoxPanel => null;

	public GameObject SkillTalentPage => null;

	public GameObject Skill => null;

	public MenuSkillPanelPresenter Skill_MenuSkillPanel => null;

	public GameObject Skill_SkillCaption => null;

	public ParadoxTooltipPresenter ParadoxTooltip => null;

	public InGameSkillTooltipPresenter SkillTooltip => null;

	public GameObject Tabs => null;

	public GameObject ParadoxBuffTab => null;

	public Button ParadoxBuffTab_Button => null;

	public GameObject SkillTalentTab => null;

	public Button SkillTalentTab_Button => null;
}
