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

	public GameObject Contents => null;

	public Button CloseButton => null;

	public GameObject Gamma => null;

	public Slider Gamma_Slider => null;

	public GameObject Gamma_Slider_HandleArea => null;

	public Image Gamma_Slider_Handle => null;

	public TextMeshProUGUI Gamma_Slider_Handle_Text => null;
}
