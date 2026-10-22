using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject desc;

	[SerializeField]
	private TextMeshProUGUI desc_Text;

	[SerializeField]
	private new Image name;

	[SerializeField]
	private TextMeshProUGUI name_Text;

	[SerializeField]
	private GameObject frame;

	[SerializeField]
	private GameObject locked;

	[SerializeField]
	private GameObject unlocked;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Desc => null;

	public TextMeshProUGUI Desc_Text => null;

	public Image Name => null;

	public TextMeshProUGUI Name_Text => null;

	public GameObject Frame => null;

	public GameObject Locked => null;

	public GameObject Unlocked => null;
}
