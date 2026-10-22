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

	public GameObject Negative => negative;

	public Image Negative_ArtifactDealBg => negative_ArtifactDealBg;

	public Image Negative_ArtifactDealLine => negative_ArtifactDealLine;

	public Image Negative_DefaultLine => negative_DefaultLine;

	public TextMeshProUGUI Negative_Text => negative_Text;

	public GameObject Positive => positive;

	public Image Positive_ArtifactDealBg => positive_ArtifactDealBg;

	public Image Positive_ArtifactDealLine => positive_ArtifactDealLine;

	public Image Positive_DefaultLine => positive_DefaultLine;

	public TextMeshProUGUI Positive_Text => positive_Text;
}
