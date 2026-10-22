using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveSlotView : View
{
	[SerializeField]
	private TextMeshProUGUI desc;

	[SerializeField]
	private Image normal;

	[SerializeField]
	private new TextMeshProUGUI name;

	public TextMeshProUGUI Desc => null;

	public Image Normal => null;

	public TextMeshProUGUI Name => null;
}
