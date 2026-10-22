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

	public GameObject No => null;

	public GameObject Profile => null;

	public Image CharacterImage => null;

	public GameObject Waiting => null;

	public Image Yes => null;

	public GameObject Yes_Host => null;

	public GameObject Yes_Icon => null;
}
