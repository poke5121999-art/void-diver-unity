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

	public GameObject Content => content;

	public GameObject BaseKeyGuideTextHold => baseKeyGuideTextHold;

	public TextMeshProUGUI DescText => descText;

	public GameObject KeyPrompt => keyPrompt;

	public Image Gauge => gauge;

	public Image GaugeFill => gaugeFill;

	public Image Image => image;

	public Image Dim => dim;
}
