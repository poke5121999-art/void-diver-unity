using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class OptionPageView : View
{
	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject[] panel;

	[SerializeField]
	private Button resetButton;

	[SerializeField]
	private GameObject[] tabs;

	public GameObject Body => body;

	public GameObject[] Panel => panel;

	public Button ResetButtont => resetButton;

	public GameObject[] Tabs => tabs;
}
