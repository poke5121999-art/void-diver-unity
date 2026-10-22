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

	public TextMeshProUGUI Desc => desc;

	public Image Normal => normal;

	public TextMeshProUGUI Name => name;
}
