using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNamePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject changeNumber;

	[SerializeField]
	private GameObject userName;

	[SerializeField]
	private TMP_InputField inputField;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject ChangeNumber => changeNumber;

	public GameObject UserName => userName;

	public TMP_InputField InputField => inputField;

	public GameObject Bottom => bottom;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;
}
