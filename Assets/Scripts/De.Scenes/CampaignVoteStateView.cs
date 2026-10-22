using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CampaignVoteStateView : View
{
	[SerializeField]
	private GameObject no;

	[SerializeField]
	private GameObject profile;

	[SerializeField]
	private Image characterImage;

	[SerializeField]
	private GameObject waiting;

	[SerializeField]
	private Image yes;

	[SerializeField]
	private GameObject yes_Host;

	[SerializeField]
	private GameObject yes_Icon;

	public GameObject No => no;

	public GameObject Profile => profile;

	public Image CharacterImage => characterImage;

	public GameObject Waiting => waiting;

	public Image Yes => yes;

	public GameObject Yes_Host => yes_Host;

	public GameObject Yes_Icon => yes_Icon;
}
