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

	public GameObject Contents => null;

	public GameObject Body => null;

	public TextMeshProUGUI Title => null;
}
