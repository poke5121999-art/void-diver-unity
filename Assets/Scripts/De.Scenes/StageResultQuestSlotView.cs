using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class StageResultQuestSlotView : View
{
	[SerializeField]
	private TextMeshProUGUI complete;

	[SerializeField]
	private TextMeshProUGUI count;

	[SerializeField]
	private TextMeshProUGUI taskTitle;

	public TextMeshProUGUI Complete => complete;

	public TextMeshProUGUI Count => count;

	public TextMeshProUGUI TaskTitle => taskTitle;
}
