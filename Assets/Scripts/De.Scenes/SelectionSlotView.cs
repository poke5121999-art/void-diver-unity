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

	public Button Button => button;

	public GameObject LoungeQuestMark => loungeQuestMark;

	public Image Normal => normal;

	public Image Line => line;

	public GameObject NewTag => newTag;

	public TextMeshProUGUI Text => text;
}
