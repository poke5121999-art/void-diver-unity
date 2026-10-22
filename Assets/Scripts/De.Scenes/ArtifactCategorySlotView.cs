using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactCategorySlotView : View
{
	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private Image normal_ArtifactDealBg;

	[SerializeField]
	private Image normal_ArtifactDealLine;

	[SerializeField]
	private Image normal_Line;

	[SerializeField]
	private TextMeshProUGUI normal_Text;

	public GameObject Normal => normal;

	public Image Normal_ArtifactDealBg => normal_ArtifactDealBg;

	public Image Normal_ArtifactDealLine => normal_ArtifactDealLine;

	public Image Normal_Line => normal_Line;

	public TextMeshProUGUI Normal_Text => normal_Text;
}
