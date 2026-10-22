using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxSimpleSlotView : View
{
	[SerializeField]
	private Image lockIcon;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private TextMeshProUGUI text;

	public Image LockIcon => null;

	public Image Normal => null;

	public TextMeshProUGUI Text => null;
}
