using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeChangeConfirmPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject arrow;

	[SerializeField]
	private EmployeeCardPresenter _afterEmployeeCard;

	[SerializeField]
	private EmployeeCardPresenter _beforeEmployeeCard;

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
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public GameObject Contents => null;

	public GameObject Arrow => null;

	public EmployeeCardPresenter AfterEmployeeCard => null;

	public EmployeeCardPresenter BeforeEmployeeCard => null;

	public GameObject Buttons => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Title => null;

	public TextMeshProUGUI Title_Text => null;
}
