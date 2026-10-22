using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadioToastView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject pivot;

	[SerializeField]
	private TextMeshProUGUI message;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private GameObject portrait;

	[SerializeField]
	private Image portrait_Image;

	public GameObject Contents => null;

	public GameObject Pivot => null;

	public TextMeshProUGUI Message => null;

	public TextMeshProUGUI Name => null;

	public GameObject Portrait => null;

	public Image Portrait_Image => null;
}
