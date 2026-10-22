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

	public BuffTooltipPresenter BuffTooltip => buffTooltip;

	public CharacterInfoPanelPresenter CharacterInfoPanel => characterInfoPanel;

	public EquipmentTooltipPresenter EquipmentTooltip => equipmentTooltip;

	public GameObject EscapeKeyGuide => escapeKeyGuide;

	public GameObject Pages => pages;

	public GameObject ParadoxBuffPage => paradoxBuffPage;

	public MenuBuffPanelPresenter BuffPanel => buffPanel;

	public ParadoxPanelPresenter NegativeParadoxPanel => negativeParadoxPanel;

	public ParadoxPanelPresenter PositiveParadoxPanel => positiveParadoxPanel;

	public GameObject SkillTalentPage => skillTalentPage;

	public GameObject Skill => skill;

	public MenuSkillPanelPresenter Skill_MenuSkillPanel => skill_MenuSkillPanel;

	public GameObject Skill_SkillCaption => skill_SkillCaption;

	public ParadoxTooltipPresenter ParadoxTooltip => paradoxTooltip;

	public InGameSkillTooltipPresenter SkillTooltip => skillTooltip;

	public GameObject Tabs => tabs;

	public GameObject ParadoxBuffTab => paradoxBuffTab;

	public Button ParadoxBuffTab_Button => paradoxBuffTab_Button;

	public GameObject SkillTalentTab => skillTalentTab;

	public Button SkillTalentTab_Button => skillTalentTab_Button;
}
