using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject display;

	[SerializeField]
	private SkillInfoDisplaySlotPresenter skillInfoDisplaySlot;

	[SerializeField]
	private Image slotEmpty;

	[SerializeField]
	private Image slotEmptyEquipment;

	[SerializeField]
	private Image slotLocked;

	[SerializeField]
	private GameObject emptySkillGuide;

	[SerializeField]
	private GameObject equipmentSkillGuide;

	[SerializeField]
	private GameObject skill;

	[SerializeField]
	private GameObject mask;

	[SerializeField]
	private Image characterImage;

	[SerializeField]
	private GameObject[] skillGroup;

	[SerializeField]
	private Image skillTitle;

	[SerializeField]
	private TextMeshProUGUI skillTitle_TextTitle;

	[SerializeField]
	private GameObject skillSelect;

	[SerializeField]
	private GameObject ignoreGroup;

	[SerializeField]
	private GameObject[] keyPrompt;

	[SerializeField]
	private Image scrollView;

	[SerializeField]
	private GameObject top;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Display => null;

	public SkillInfoDisplaySlotPresenter SkillInfoDisplaySlot => null;

	public Image SlotEmpty => null;

	public Image SlotEmptyEquipment => null;

	public Image SlotLocked => null;

	public GameObject EmptySkillGuide => null;

	public GameObject EquipmentSkillGuide => null;

	public GameObject Skill => null;

	public GameObject Mask => null;

	public Image CharacterImage => null;

	public GameObject[] SkillGroup => null;

	public Image SkillTitle => null;

	public TextMeshProUGUI SkillTitle_TextTitle => null;

	public GameObject SkillSelect => null;

	public GameObject IgnoreGroup => null;

	public GameObject[] KeyPrompt => null;

	public Image ScrollView => null;

	public GameObject Top => null;
}
