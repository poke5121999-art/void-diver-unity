using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SalesSlotView : View
{
	[SerializeField]
	private Image normal;

	[SerializeField]
	private GameObject artifact;

	[SerializeField]
	private TextMeshProUGUI artifact_TextEmpty;

	[SerializeField]
	private TextMeshProUGUI artifact_TextGrade;

	[SerializeField]
	private TextMeshProUGUI artifact_TextName;

	[SerializeField]
	private GameObject employee;

	[SerializeField]
	private TextMeshProUGUI employee_TextEmpty;

	[SerializeField]
	private TextMeshProUGUI employee_TextLv;

	[SerializeField]
	private TextMeshProUGUI employee_TextName;

	[SerializeField]
	private TextMeshProUGUI employee_TextNamePanic;

	[SerializeField]
	private EmployeeSimpleSlotPresenter employeeSimpleSlot;

	[SerializeField]
	private Image employeeSlotEmpty;

	[SerializeField]
	private GameObject finished;

	[SerializeField]
	private InventoryGoodsSlotPresenter goodsSlot;

	[SerializeField]
	private Image goodsSlotEmpty;

	[SerializeField]
	private GameObject locked;

	[SerializeField]
	private Slider sellSlider;

	[SerializeField]
	private GameObject sellSlider_Contents;

	[SerializeField]
	private Image sellSlider_BG;

	[SerializeField]
	private Image sellSlider_Gauge;

	[SerializeField]
	private Image sellSlider_Time;

	[SerializeField]
	private TextMeshProUGUI text;

	public Image Normal => null;

	public GameObject Artifact => null;

	public TextMeshProUGUI Artifact_TextEmpty => null;

	public TextMeshProUGUI Artifact_TextGrade => null;

	public TextMeshProUGUI Artifact_TextName => null;

	public GameObject Employee => null;

	public TextMeshProUGUI Employee_TextEmpty => null;

	public TextMeshProUGUI Employee_TextLv => null;

	public TextMeshProUGUI Employee_TextName => null;

	public TextMeshProUGUI Employee_TextNamePanic => null;

	public EmployeeSimpleSlotPresenter EmployeeSimpleSlot => null;

	public Image EmployeeSlotEmpty => null;

	public GameObject Finished => null;

	public InventoryGoodsSlotPresenter GoodsSlot => null;

	public Image GoodsSlotEmpty => null;

	public GameObject Locked => null;

	public Slider SellSlider => null;

	public GameObject SellSlider_Contents => null;

	public Image SellSlider_BG => null;

	public Image SellSlider_Gauge => null;

	public Image SellSlider_Time => null;

	public TextMeshProUGUI Text => null;
}
