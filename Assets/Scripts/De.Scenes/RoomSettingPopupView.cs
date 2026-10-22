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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject LoungeInfo => loungeInfo;

	public TextMeshProUGUI Desc => desc;

	public TMP_InputField InputFieldTitle => inputFieldTitle;

	public GameObject InputFieldTitle_TextArea => inputFieldTitle_TextArea;

	public TextMeshProUGUI InputFieldTitle_TextArea_Placeholder => inputFieldTitle_TextArea_Placeholder;

	public TextMeshProUGUI InputFieldTitle_TextArea_Text => inputFieldTitle_TextArea_Text;

	public Image LoungeDifficulty => loungeDifficulty;

	public Image LoungeDifficulty_Line => loungeDifficulty_Line;

	public Button LoungeDifficulty_NextButton => loungeDifficulty_NextButton;

	public Button LoungeDifficulty_PadButton => loungeDifficulty_PadButton;

	public Button LoungeDifficulty_PrevButton => loungeDifficulty_PrevButton;

	public TextMeshProUGUI LoungeDifficulty_Text => loungeDifficulty_Text;

	public Image LoungeLock => loungeLock;

	public Button LoungeLock_NextButton => loungeLock_NextButton;

	public Button LoungeLock_PadButton => loungeLock_PadButton;

	public Button LoungeLock_PrevButton => loungeLock_PrevButton;

	public TextMeshProUGUI LoungeLock_Text => loungeLock_Text;

	public TextMeshProUGUI Title => title;

	public GameObject BtnGroup => btnGroup;

	public GameObject Cancel => cancel;

	public Button Cancel_Btn => cancel_Btn;

	public GameObject Confirm => confirm;

	public Button Confirm_Btn => confirm_Btn;
}
