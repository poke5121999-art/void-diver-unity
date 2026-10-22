using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private GameObject loungeQuestMark;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject newTag;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => null;

	public GameObject LoungeQuestMark => null;

	public Image Normal => null;

	public Image Line => null;

	public GameObject NewTag => null;

	public TextMeshProUGUI Text => null;
}
