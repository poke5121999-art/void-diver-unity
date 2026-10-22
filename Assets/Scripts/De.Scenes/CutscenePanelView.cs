using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject baseKeyGuideTextHold;

	[SerializeField]
	private TextMeshProUGUI descText;

	[SerializeField]
	private GameObject keyPrompt;

	[SerializeField]
	private Image gauge;

	[SerializeField]
	private Image gaugeFill;

	[SerializeField]
	private Image image;

	[SerializeField]
	private Image dim;

	public GameObject Content => null;

	public GameObject BaseKeyGuideTextHold => null;

	public TextMeshProUGUI DescText => null;

	public GameObject KeyPrompt => null;

	public Image Gauge => null;

	public Image GaugeFill => null;

	public Image Image => null;

	public Image Dim => null;
}
