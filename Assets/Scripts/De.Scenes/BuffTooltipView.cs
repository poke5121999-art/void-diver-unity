using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffTooltipView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject description;

	[SerializeField]
	private TextMeshProUGUI description_Text;

	[SerializeField]
	private Image title;

	[SerializeField]
	private TextMeshProUGUI title_Name;

	[SerializeField]
	private GameObject frame;

	[SerializeField]
	private GameObject baseFrame;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Description => null;

	public TextMeshProUGUI Description_Text => null;

	public Image Title => null;

	public TextMeshProUGUI Title_Name => null;

	public GameObject Frame => null;

	public GameObject BaseFrame => null;
}
