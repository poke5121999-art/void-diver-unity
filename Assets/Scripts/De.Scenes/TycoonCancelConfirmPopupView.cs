using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class TycoonCancelConfirmPopupView : View
{
	[SerializeField]
	private Button background;

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
	private GameObject top;

	[SerializeField]
	private GameObject title;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Top => null;

	public GameObject Title => null;
}
