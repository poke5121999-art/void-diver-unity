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

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Description => description;

	public TextMeshProUGUI Description_Text => description_Text;

	public Image Title => title;

	public TextMeshProUGUI Title_Name => title_Name;

	public GameObject Frame => frame;

	public GameObject BaseFrame => baseFrame;
}
