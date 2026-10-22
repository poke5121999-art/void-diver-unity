using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactPrefixSlotView : View
{
	[SerializeField]
	private GameObject negative;

	[SerializeField]
	private Image negative_ArtifactDealBg;

	[SerializeField]
	private Image negative_ArtifactDealLine;

	[SerializeField]
	private Image negative_DefaultLine;

	[SerializeField]
	private TextMeshProUGUI negative_Text;

	[SerializeField]
	private GameObject positive;

	[SerializeField]
	private Image positive_ArtifactDealBg;

	[SerializeField]
	private Image positive_ArtifactDealLine;

	[SerializeField]
	private Image positive_DefaultLine;

	[SerializeField]
	private TextMeshProUGUI positive_Text;

	public GameObject Negative => null;

	public Image Negative_ArtifactDealBg => null;

	public Image Negative_ArtifactDealLine => null;

	public Image Negative_DefaultLine => null;

	public TextMeshProUGUI Negative_Text => null;

	public GameObject Positive => null;

	public Image Positive_ArtifactDealBg => null;

	public Image Positive_ArtifactDealLine => null;

	public Image Positive_DefaultLine => null;

	public TextMeshProUGUI Positive_Text => null;
}
