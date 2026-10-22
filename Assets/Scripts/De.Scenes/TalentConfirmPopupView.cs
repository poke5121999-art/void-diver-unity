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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Material => material;

	public GameObject Layout => layout;

	public GameObject[] MaterialSlots => materialSlots;

	public GameObject TechTree => techTree;

	public TalentSlotPresenter TalentSlot => talentSlot;

	public GameObject VerticalLayout => verticalLayout;

	public TextMeshProUGUI Name => name;

	public GameObject NeedCoin => needCoin;

	public Image NeedCoin_Icon => needCoin_Icon;

	public TextMeshProUGUI NeedCoin_Text => needCoin_Text;

	public GameObject NeedGold => needGold;

	public Image NeedGold_Icon => needGold_Icon;

	public TextMeshProUGUI NeedGold_Text => needGold_Text;

	public GameObject NeedRealCoin => needRealCoin;

	public Image NeedRealCoin_Icon => needRealCoin_Icon;

	public TextMeshProUGUI NeedRealCoin_Text => needRealCoin_Text;
}
