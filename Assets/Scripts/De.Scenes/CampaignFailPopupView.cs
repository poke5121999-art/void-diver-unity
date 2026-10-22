using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CampaignFailPopupView : View
{
	[SerializeField]
	private Button baseBgDimmer;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	public Button BaseBgDimmer => baseBgDimmer;

	public GameObject Contents => contents;

	public GameObject Body => body;
}
