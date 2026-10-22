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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Desc => desc;

	public TextMeshProUGUI Desc_Text => desc_Text;

	public Image Name => name;

	public TextMeshProUGUI Name_Text => name_Text;

	public GameObject Frame => frame;

	public GameObject Locked => locked;

	public GameObject Unlocked => unlocked;
}
