using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class PlayerNoticeToastView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI messageText;

	public GameObject Contents => contents;

	public TextMeshProUGUI MessageText => messageText;
}
