using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class NoticeToastView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI messageText;

	public GameObject Contents => null;

	public TextMeshProUGUI MessageText => null;
}
