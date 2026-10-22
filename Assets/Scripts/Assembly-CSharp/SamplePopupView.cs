using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SamplePopupView : View
{
	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private Button cancel_Button;

	public Button Confirm_Button => confirm_Button;

	public Button Cancel_Button => cancel_Button;
}
