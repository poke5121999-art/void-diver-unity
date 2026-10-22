using NemoLib.Mvp;
using UnityEngine;

public class OptionPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private OptionPagePresenter optionPage;

	public GameObject Contents => null;

	public OptionPagePresenter OptionPage => null;
}
