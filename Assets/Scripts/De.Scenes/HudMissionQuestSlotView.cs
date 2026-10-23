using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudMissionQuestSlotView : View
{
	[SerializeField]
	private GameObject checkBoxGroup;

	[SerializeField]
	private Image mark;

	[SerializeField]
	private TextMeshProUGUI text;

	public GameObject CheckBoxGroup => checkBoxGroup;

	public Image Mark => mark;

	public TextMeshProUGUI Text => text;
}
