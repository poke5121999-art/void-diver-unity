using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeSimpleSlotView : View
{
	[SerializeField]
	private Slider expSlider;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image face;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Slider panicSlider;

	[SerializeField]
	private GameObject panicSlider_Contents;

	[SerializeField]
	private Image panicSlider_BG;

	[SerializeField]
	private Image panicSlider_Face;

	[SerializeField]
	private Image panicSlider_Gauge;

	[SerializeField]
	private Image panicSlider_Gauge_img_back;

	[SerializeField]
	private Image panicSlider_Gauge_img_front;

	public Slider ExpSlider => null;

	public Image Normal => null;

	public Image Face => null;

	public Image Line => null;

	public Slider PanicSlider => null;

	public GameObject PanicSlider_Contents => null;

	public Image PanicSlider_BG => null;

	public Image PanicSlider_Face => null;

	public Image PanicSlider_Gauge => null;

	public Image PanicSlider_Gauge_img_back => null;

	public Image PanicSlider_Gauge_img_front => null;
}
