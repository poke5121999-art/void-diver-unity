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

	public Image EscapeFail => null;

	public Image EscapeSuccess => null;

	public TextMeshProUGUI NameText => null;

	public GameObject Profile => null;

	public Image DeadDimmer => null;

	public Image ImageFace => null;
}
