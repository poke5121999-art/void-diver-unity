using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TextMeshProUGUI additionalMessage;

	[SerializeField]
	private Image line;

	[SerializeField]
	private TextMeshProUGUI message;

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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public TextMeshProUGUI AdditionalMessage => additionalMessage;

	public Image Line => line;

	public TextMeshProUGUI Message => message;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;
}
