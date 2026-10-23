using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class QuestCompletePopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private TextMeshProUGUI title;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public TextMeshProUGUI Title => title;
}
