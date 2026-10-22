using NemoLib.Mvp;
using UnityEngine;

public class EscapeSuccessPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	public GameObject Contents => contents;

	public GameObject Body => body;
}
