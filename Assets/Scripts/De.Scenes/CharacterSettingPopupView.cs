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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject ChaInfo => null;

	public TextMeshProUGUI ChaInfo_ChaName => null;

	public TextMeshProUGUI ChaInfo_ChaSubTitle => null;

	public Image ChaInfo_Line => null;

	public GameObject[] ChaInfo_TagGroup => null;

	public Image CharacterSettingScroller => null;

	public GameObject Desc => null;

	public TextMeshProUGUI Desc_Text => null;

	public GameObject[] PagenationCells => null;

	public Image PortraitMask => null;

	public Image Portrait => null;

	public GameObject Skill => null;

	public GameObject[] Skill_Group => null;

	public Button Skill_SettingButton => null;

	public TextMeshProUGUI Skill_TextTitle => null;

	public GameObject Skin => null;

	public GameObject Skin_Name => null;

	public Button Skin_Button => null;

	public TextMeshProUGUI Skin_Text => null;

	public Button Skin_NextButton => null;

	public Button Skin_PrevButton => null;

	public GameObject SkinPreset => null;

	public Button SkinPreset_NextButton => null;

	public Button SkinPreset_PrevButton => null;

	public SkeletonGraphic SkinPreset_Skeleton => null;

	public Button SkinPreset_Button => null;
}
