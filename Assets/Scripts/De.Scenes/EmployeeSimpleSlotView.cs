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

	public Slider ExpSlider => expSlider;

	public Image Normal => normal;

	public Image Face => face;

	public Image Line => line;

	public Slider PanicSlider => panicSlider;

	public GameObject PanicSlider_Contents => panicSlider_Contents;

	public Image PanicSlider_BG => panicSlider_BG;

	public Image PanicSlider_Face => panicSlider_Face;

	public Image PanicSlider_Gauge => panicSlider_Gauge;

	public Image PanicSlider_Gauge_img_back => panicSlider_Gauge_img_back;

	public Image PanicSlider_Gauge_img_front => panicSlider_Gauge_img_front;
}
