using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotView : View
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI questNameText;

	[SerializeField]
	private TextMeshProUGUI questTypeText;

	public Button Button => button;

	public Image Normal => normal;

	public TextMeshProUGUI QuestNameText => questNameText;

	public TextMeshProUGUI QuestTypeText => questTypeText;
}
