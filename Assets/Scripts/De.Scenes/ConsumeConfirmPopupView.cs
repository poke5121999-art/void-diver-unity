using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeConfirmPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject coin;

	[SerializeField]
	private Image coin_Icon;

	[SerializeField]
	private TextMeshProUGUI coin_Text;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private GameObject gold;

	[SerializeField]
	private Image gold_Icon;

	[SerializeField]
	private TextMeshProUGUI gold_Text;

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
	private GameObject top;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_SubText;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Coin => null;

	public Image Coin_Icon => null;

	public TextMeshProUGUI Coin_Text => null;

	public TextMeshProUGUI Desc => null;

	public GameObject Gold => null;

	public Image Gold_Icon => null;

	public TextMeshProUGUI Gold_Text => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Top => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_SubText => null;

	public TextMeshProUGUI Title_Text => null;
}
