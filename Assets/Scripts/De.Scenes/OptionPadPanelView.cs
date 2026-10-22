using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class OptionPadPanelView : View
{
	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private KeySettingSlotRowScrollerPresenter scroller;

	public Button ResetButton => null;

	public KeySettingSlotRowScrollerPresenter Scroller => null;
}
