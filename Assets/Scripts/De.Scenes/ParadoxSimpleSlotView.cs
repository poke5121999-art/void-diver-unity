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

	public Image LockIcon => lockIcon;

	public Image Normal => normal;

	public TextMeshProUGUI Text => text;
}
