using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomDifficultySlotView : View
{
	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject off;

	[SerializeField]
	private Image off_Icon;

	[SerializeField]
	private TextMeshProUGUI off_Text;

	[SerializeField]
	private GameObject on;

	[SerializeField]
	private Image on_Icon;

	[SerializeField]
	private GameObject on_Select;

	[SerializeField]
	private GameObject on_Select_Layout;

	[SerializeField]
	private Image on_Select_Line;

	[SerializeField]
	private GameObject[] on_Select_Vfx;

	[SerializeField]
	private TextMeshProUGUI on_Text;

	public GameObject Normal => null;

	public GameObject Off => null;

	public Image Off_Icon => null;

	public TextMeshProUGUI Off_Text => null;

	public GameObject On => null;

	public Image On_Icon => null;

	public GameObject On_Select => null;

	public GameObject On_Select_Layout => null;

	public Image On_Select_Line => null;

	public GameObject[] On_Select_Vfx => null;

	public TextMeshProUGUI On_Text => null;
}
