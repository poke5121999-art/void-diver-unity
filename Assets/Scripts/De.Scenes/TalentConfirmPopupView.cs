using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentConfirmPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GameObject material;

	[SerializeField]
	private GameObject layout;

	[SerializeField]
	private GameObject[] materialSlots;

	[SerializeField]
	private GameObject techTree;

	[SerializeField]
	private TalentSlotPresenter talentSlot;

	[SerializeField]
	private GameObject verticalLayout;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private GameObject needCoin;

	[SerializeField]
	private Image needCoin_Icon;

	[SerializeField]
	private TextMeshProUGUI needCoin_Text;

	[SerializeField]
	private GameObject needGold;

	[SerializeField]
	private Image needGold_Icon;

	[SerializeField]
	private TextMeshProUGUI needGold_Text;

	[SerializeField]
	private GameObject needRealCoin;

	[SerializeField]
	private Image needRealCoin_Icon;

	[SerializeField]
	private TextMeshProUGUI needRealCoin_Text;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Material => null;

	public GameObject Layout => null;

	public GameObject[] MaterialSlots => null;

	public GameObject TechTree => null;

	public TalentSlotPresenter TalentSlot => null;

	public GameObject VerticalLayout => null;

	public TextMeshProUGUI Name => null;

	public GameObject NeedCoin => null;

	public Image NeedCoin_Icon => null;

	public TextMeshProUGUI NeedCoin_Text => null;

	public GameObject NeedGold => null;

	public Image NeedGold_Icon => null;

	public TextMeshProUGUI NeedGold_Text => null;

	public GameObject NeedRealCoin => null;

	public Image NeedRealCoin_Icon => null;

	public TextMeshProUGUI NeedRealCoin_Text => null;
}
