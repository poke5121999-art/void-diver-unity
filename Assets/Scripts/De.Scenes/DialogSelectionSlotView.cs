using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSelectionSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private Image checkIcon;

	[SerializeField]
	private Image line;

	[SerializeField]
	private GameObject mainQuestIcon;

	[SerializeField]
	private GameObject newTag;

	[SerializeField]
	private TextMeshProUGUI text;

	public Button Button => button;

	public Image Normal => normal;

	public Image CheckIcon => checkIcon;

	public Image Line => line;

	public GameObject MainQuestIcon => mainQuestIcon;

	public GameObject NewTag => newTag;

	public TextMeshProUGUI Text => text;
}
