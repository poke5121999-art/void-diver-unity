using NemoLib.Mvp;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettingPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject chaInfo;

	[SerializeField]
	private TextMeshProUGUI chaInfo_ChaName;

	[SerializeField]
	private TextMeshProUGUI chaInfo_ChaSubTitle;

	[SerializeField]
	private Image chaInfo_Line;

	[SerializeField]
	private GameObject[] chaInfo_TagGroup;

	[SerializeField]
	private Image characterSettingScroller;

	[SerializeField]
	private GameObject desc;

	[SerializeField]
	private TextMeshProUGUI desc_Text;

	[SerializeField]
	private GameObject[] pagenationCells;

	[SerializeField]
	private Image portraitMask;

	[SerializeField]
	private Image portrait;

	[SerializeField]
	private GameObject skill;

	[SerializeField]
	private GameObject[] skill_Group;

	[SerializeField]
	private Button skill_SettingButton;

	[SerializeField]
	private TextMeshProUGUI skill_TextTitle;

	[SerializeField]
	private GameObject skin;

	[SerializeField]
	private GameObject skin_Name;

	[SerializeField]
	private Button skin_Button;

	[SerializeField]
	private TextMeshProUGUI skin_Text;

	[SerializeField]
	private Button skin_NextButton;

	[SerializeField]
	private Button skin_PrevButton;

	[SerializeField]
	private GameObject skinPreset;

	[SerializeField]
	private Button skinPreset_NextButton;

	[SerializeField]
	private Button skinPreset_PrevButton;

	[SerializeField]
	private SkeletonGraphic skinPreset_Skeleton;

	[SerializeField]
	private Button skinPreset_Button;

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject ChaInfo => chaInfo;

	public TextMeshProUGUI ChaInfo_ChaName => chaInfo_ChaName;

	public TextMeshProUGUI ChaInfo_ChaSubTitle => chaInfo_ChaSubTitle;

	public Image ChaInfo_Line => chaInfo_Line;

	public GameObject[] ChaInfo_TagGroup => chaInfo_TagGroup;

	public Image CharacterSettingScroller => characterSettingScroller;

	public GameObject Desc => desc;

	public TextMeshProUGUI Desc_Text => desc_Text;

	public GameObject[] PagenationCells => pagenationCells;

	public Image PortraitMask => portraitMask;

	public Image Portrait => portrait;

	public GameObject Skill => skill;

	public GameObject[] Skill_Group => skill_Group;

	public Button Skill_SettingButton => skill_SettingButton;

	public TextMeshProUGUI Skill_TextTitle => skill_TextTitle;

	public GameObject Skin => skin;

	public GameObject Skin_Name => skin_Name;

	public Button Skin_Button => skin_Button;

	public TextMeshProUGUI Skin_Text => skin_Text;

	public Button Skin_NextButton => skin_NextButton;

	public Button Skin_PrevButton => skin_PrevButton;

	public GameObject SkinPreset => skinPreset;

	public Button SkinPreset_NextButton => skinPreset_NextButton;

	public Button SkinPreset_PrevButton => skinPreset_PrevButton;

	public SkeletonGraphic SkinPreset_Skeleton => skinPreset_Skeleton;

	public Button SkinPreset_Button => skinPreset_Button;
}
