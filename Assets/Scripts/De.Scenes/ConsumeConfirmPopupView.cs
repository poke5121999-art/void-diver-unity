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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Coin => coin;

	public Image Coin_Icon => coin_Icon;

	public TextMeshProUGUI Coin_Text => coin_Text;

	public TextMeshProUGUI Desc => desc;

	public GameObject Gold => gold;

	public Image Gold_Icon => gold_Icon;

	public TextMeshProUGUI Gold_Text => gold_Text;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Top => top;

	public GameObject Title => title;

	public TextMeshProUGUI Title_SubText => title_SubText;

	public TextMeshProUGUI Title_Text => title_Text;
}
