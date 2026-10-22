using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPlayerExpSlotView : View
{
	[SerializeField]
	private Image expDifficulty;

	[SerializeField]
	private GameObject expDifficulty_Group;

	[SerializeField]
	private TextMeshProUGUI expDifficulty_BonusValue;

	[SerializeField]
	private TextMeshProUGUI expDifficulty_Value;

	[SerializeField]
	private Image expEscape;

	[SerializeField]
	private GameObject expEscape_Group;

	[SerializeField]
	private TextMeshProUGUI expEscape_BonusValue;

	[SerializeField]
	private TextMeshProUGUI expEscape_Value;

	[SerializeField]
	private Image expMonsterKill;

	[SerializeField]
	private GameObject expMonsterKill_Group;

	[SerializeField]
	private TextMeshProUGUI expMonsterKill_BonusValue;

	[SerializeField]
	private TextMeshProUGUI expMonsterKill_Value;

	[SerializeField]
	private Image expRewardBox;

	[SerializeField]
	private GameObject expRewardBox_Group;

	[SerializeField]
	private TextMeshProUGUI expRewardBox_BonusValue;

	[SerializeField]
	private TextMeshProUGUI expRewardBox_Value;

	[SerializeField]
	private Image expTalent;

	[SerializeField]
	private GameObject expTalent_Group;

	[SerializeField]
	private TextMeshProUGUI expTalent_BonusValue;

	[SerializeField]
	private TextMeshProUGUI expTalent_Value;

	[SerializeField]
	private TextMeshProUGUI userName;

	[SerializeField]
	private TextMeshProUGUI talentBonus;

	public Image ExpDifficulty => expDifficulty;

	public GameObject ExpDifficulty_Group => expDifficulty_Group;

	public TextMeshProUGUI ExpDifficulty_BonusValue => expDifficulty_BonusValue;

	public TextMeshProUGUI ExpDifficulty_Value => expDifficulty_Value;

	public Image ExpEscape => expEscape;

	public GameObject ExpEscape_Group => expEscape_Group;

	public TextMeshProUGUI ExpEscape_BonusValue => expEscape_BonusValue;

	public TextMeshProUGUI ExpEscape_Value => expEscape_Value;

	public Image ExpMonsterKill => expMonsterKill;

	public GameObject ExpMonsterKill_Group => expMonsterKill_Group;

	public TextMeshProUGUI ExpMonsterKill_BonusValue => expMonsterKill_BonusValue;

	public TextMeshProUGUI ExpMonsterKill_Value => expMonsterKill_Value;

	public Image ExpRewardBox => expRewardBox;

	public GameObject ExpRewardBox_Group => expRewardBox_Group;

	public TextMeshProUGUI ExpRewardBox_BonusValue => expRewardBox_BonusValue;

	public TextMeshProUGUI ExpRewardBox_Value => expRewardBox_Value;

	public Image ExpTalent => expTalent;

	public GameObject ExpTalent_Group => expTalent_Group;

	public TextMeshProUGUI ExpTalent_BonusValue => expTalent_BonusValue;

	public TextMeshProUGUI ExpTalent_Value => expTalent_Value;

	public TextMeshProUGUI UserName => userName;

	public TextMeshProUGUI TalentBonus => talentBonus;
}
