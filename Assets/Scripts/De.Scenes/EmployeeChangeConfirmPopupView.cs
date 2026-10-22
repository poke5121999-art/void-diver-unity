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

	public GameObject Contents => contents;

	public GameObject Arrow => arrow;

	public EmployeeCardPresenter AfterEmployeeCard => _afterEmployeeCard;

	public EmployeeCardPresenter BeforeEmployeeCard => _beforeEmployeeCard;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Title => title;

	public TextMeshProUGUI Title_Text => title_Text;
}
