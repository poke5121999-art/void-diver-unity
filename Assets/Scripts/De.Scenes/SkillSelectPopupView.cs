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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Display => display;

	public SkillInfoDisplaySlotPresenter SkillInfoDisplaySlot => skillInfoDisplaySlot;

	public Image SlotEmpty => slotEmpty;

	public Image SlotEmptyEquipment => slotEmptyEquipment;

	public Image SlotLocked => slotLocked;

	public GameObject EmptySkillGuide => emptySkillGuide;

	public GameObject EquipmentSkillGuide => equipmentSkillGuide;

	public GameObject Skill => skill;

	public GameObject Mask => mask;

	public Image CharacterImage => characterImage;

	public GameObject[] SkillGroup => skillGroup;

	public Image SkillTitle => skillTitle;

	public TextMeshProUGUI SkillTitle_TextTitle => skillTitle_TextTitle;

	public GameObject SkillSelect => skillSelect;

	public GameObject IgnoreGroup => ignoreGroup;

	public GameObject[] KeyPrompt => keyPrompt;

	public Image ScrollView => scrollView;

	public GameObject Top => top;
}
