using NemoLib.Mvp;
using UnityEngine;

public class OptionPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private OptionPagePresenter optionPage;

	public GameObject Contents => contents;

	public OptionPagePresenter OptionPage => optionPage;
}
