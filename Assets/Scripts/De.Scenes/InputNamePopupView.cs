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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject ChangeNumber => null;

	public GameObject UserName => null;

	public TMP_InputField InputField => null;

	public GameObject Bottom => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;
}
