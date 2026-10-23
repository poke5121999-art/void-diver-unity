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

	public GameObject Contents => contents;

	public GameObject Pivot => pivot;

	public TextMeshProUGUI Message => message;

	public TextMeshProUGUI Name => name;

	public GameObject Portrait => portrait;

	public Image Portrait_Image => portrait_Image;
}
