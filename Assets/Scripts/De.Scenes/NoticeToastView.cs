using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class NoticeToastView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI messageText;

	public GameObject Contents => contents;

	public TextMeshProUGUI MessageText => messageText;
}
