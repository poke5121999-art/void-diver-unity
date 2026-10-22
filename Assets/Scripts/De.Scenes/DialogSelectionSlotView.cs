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

	public Button Button => null;

	public Image Normal => null;

	public Image CheckIcon => null;

	public Image Line => null;

	public GameObject MainQuestIcon => null;

	public GameObject NewTag => null;

	public TextMeshProUGUI Text => null;
}
