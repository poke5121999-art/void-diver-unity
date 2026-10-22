using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class StageResultPlayerParadoxGroupView : View
{
	[SerializeField]
	private GameObject[] lines;

	[SerializeField]
	private TextMeshProUGUI userName;

	public GameObject[] Lines => lines;

	public TextMeshProUGUI UserName => userName;
}
