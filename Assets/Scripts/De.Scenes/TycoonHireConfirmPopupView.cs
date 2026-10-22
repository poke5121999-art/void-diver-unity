using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TycoonHireConfirmPopupView : View
{
	[SerializeField]
	private GameObject contents;

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
	private EmployeeCardPresenter _employeeInfoCard;

	[SerializeField]
	private GameObject title;

	[SerializeField]
	private TextMeshProUGUI title_Text;

	public GameObject Contents => contents;

	public GameObject Buttons => buttons;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public EmployeeCardPresenter EmployeeInfoCard => _employeeInfoCard;

	public GameObject Title => title;

	public TextMeshProUGUI Title_Text => title_Text;
}
