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

	public GameObject Normal => null;

	public Image Normal_ArtifactDealBg => null;

	public Image Normal_ArtifactDealLine => null;

	public Image Normal_Line => null;

	public TextMeshProUGUI Normal_Text => null;
}
