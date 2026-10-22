using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GammaPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private GameObject gamma;

	[SerializeField]
	private Slider gamma_Slider;

	[SerializeField]
	private GameObject gamma_Slider_HandleArea;

	[SerializeField]
	private Image gamma_Slider_Handle;

	[SerializeField]
	private TextMeshProUGUI gamma_Slider_Handle_Text;

	public GameObject Contents => contents;

	public Button CloseButton => closeButton;

	public GameObject Gamma => gamma;

	public Slider Gamma_Slider => gamma_Slider;

	public GameObject Gamma_Slider_HandleArea => gamma_Slider_HandleArea;

	public Image Gamma_Slider_Handle => gamma_Slider_Handle;

	public TextMeshProUGUI Gamma_Slider_Handle_Text => gamma_Slider_Handle_Text;
}
