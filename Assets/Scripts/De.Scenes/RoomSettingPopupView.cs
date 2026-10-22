using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSettingPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject loungeInfo;

	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private TMP_InputField inputFieldTitle;

	[SerializeField]
	private GameObject inputFieldTitle_TextArea;

	[SerializeField]
	private TextMeshProUGUI inputFieldTitle_TextArea_Placeholder;

	[SerializeField]
	private TextMeshProUGUI inputFieldTitle_TextArea_Text;

	[SerializeField]
	private Image loungeDifficulty;

	[SerializeField]
	private Image loungeDifficulty_Line;

	[SerializeField]
	private Button loungeDifficulty_NextButton;

	[SerializeField]
	private Button loungeDifficulty_PadButton;

	[SerializeField]
	private Button loungeDifficulty_PrevButton;

	[SerializeField]
	private TextMeshProUGUI loungeDifficulty_Text;

	[SerializeField]
	private Image loungeLock;

	[SerializeField]
	private Button loungeLock_NextButton;

	[SerializeField]
	private Button loungeLock_PadButton;

	[SerializeField]
	private Button loungeLock_PrevButton;

	[SerializeField]
	private TextMeshProUGUI loungeLock_Text;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private GameObject btnGroup;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Btn;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Btn;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject LoungeInfo => null;

	public TextMeshProUGUI Desc => null;

	public TMP_InputField InputFieldTitle => null;

	public GameObject InputFieldTitle_TextArea => null;

	public TextMeshProUGUI InputFieldTitle_TextArea_Placeholder => null;

	public TextMeshProUGUI InputFieldTitle_TextArea_Text => null;

	public Image LoungeDifficulty => null;

	public Image LoungeDifficulty_Line => null;

	public Button LoungeDifficulty_NextButton => null;

	public Button LoungeDifficulty_PadButton => null;

	public Button LoungeDifficulty_PrevButton => null;

	public TextMeshProUGUI LoungeDifficulty_Text => null;

	public Image LoungeLock => null;

	public Button LoungeLock_NextButton => null;

	public Button LoungeLock_PadButton => null;

	public Button LoungeLock_PrevButton => null;

	public TextMeshProUGUI LoungeLock_Text => null;

	public TextMeshProUGUI Title => null;

	public GameObject BtnGroup => null;

	public GameObject Cancel => null;

	public Button Cancel_Btn => null;

	public GameObject Confirm => null;

	public Button Confirm_Btn => null;
}
