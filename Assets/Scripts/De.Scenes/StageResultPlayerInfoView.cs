using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPlayerInfoView : View
{
	[SerializeField]
	private Image escapeFail;

	[SerializeField]
	private Image escapeSuccess;

	[SerializeField]
	private TextMeshProUGUI nameText;

	[SerializeField]
	private GameObject profile;

	[SerializeField]
	private Image deadDimmer;

	[SerializeField]
	private Image imageFace;

	public Image EscapeFail => escapeFail;

	public Image EscapeSuccess => escapeSuccess;

	public TextMeshProUGUI NameText => nameText;

	public GameObject Profile => profile;

	public Image DeadDimmer => deadDimmer;

	public Image ImageFace => imageFace;
}
