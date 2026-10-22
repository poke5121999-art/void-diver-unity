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

	public Image Normal => normal;

	public GameObject Artifact => artifact;

	public TextMeshProUGUI Artifact_TextEmpty => artifact_TextEmpty;

	public TextMeshProUGUI Artifact_TextGrade => artifact_TextGrade;

	public TextMeshProUGUI Artifact_TextName => artifact_TextName;

	public GameObject Employee => employee;

	public TextMeshProUGUI Employee_TextEmpty => employee_TextEmpty;

	public TextMeshProUGUI Employee_TextLv => employee_TextLv;

	public TextMeshProUGUI Employee_TextName => employee_TextName;

	public TextMeshProUGUI Employee_TextNamePanic => employee_TextNamePanic;

	public EmployeeSimpleSlotPresenter EmployeeSimpleSlot => employeeSimpleSlot;

	public Image EmployeeSlotEmpty => employeeSlotEmpty;

	public GameObject Finished => finished;

	public InventoryGoodsSlotPresenter GoodsSlot => goodsSlot;

	public Image GoodsSlotEmpty => goodsSlotEmpty;

	public GameObject Locked => locked;

	public Slider SellSlider => sellSlider;

	public GameObject SellSlider_Contents => sellSlider_Contents;

	public Image SellSlider_BG => sellSlider_BG;

	public Image SellSlider_Gauge => sellSlider_Gauge;

	public Image SellSlider_Time => sellSlider_Time;

	public TextMeshProUGUI Text => text;
}
