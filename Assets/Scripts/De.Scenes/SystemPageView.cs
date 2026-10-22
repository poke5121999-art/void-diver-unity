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

	public GameObject Buttons => null;

	public Button CloseGameButton => null;

	public Button ContinueButton => null;

	public Button ExitTheLoungeButton => null;

	public Button GiveUpButton => null;

	public Button ManualButton => null;

	public Button ReturnToTitleButton => null;

	public GameObject DecoAni => null;
}
