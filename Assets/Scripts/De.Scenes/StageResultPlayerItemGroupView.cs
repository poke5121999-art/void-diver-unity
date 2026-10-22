using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class StageResultPlayerItemGroupView : View
{
	[SerializeField]
	private TextMeshProUGUI userName;

	public TextMeshProUGUI UserName => userName;
}
