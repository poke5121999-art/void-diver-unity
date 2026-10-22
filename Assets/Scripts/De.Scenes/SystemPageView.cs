using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SystemPageView : View
{
	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private Button closeGameButton;

	[SerializeField]
	private Button continueButton;

	[SerializeField]
	private Button exitTheLoungeButton;

	[SerializeField]
	private Button giveUpButton;

	[SerializeField]
	private Button manualButton;

	[SerializeField]
	private Button returnToTitleButton;

	[SerializeField]
	private GameObject decoAni;

	public GameObject Buttons => buttons;

	public Button CloseGameButton => closeGameButton;

	public Button ContinueButton => continueButton;

	public Button ExitTheLoungeButton => exitTheLoungeButton;

	public Button GiveUpButton => giveUpButton;

	public Button ManualButton => manualButton;

	public Button ReturnToTitleButton => returnToTitleButton;

	public GameObject DecoAni => decoAni;
}
