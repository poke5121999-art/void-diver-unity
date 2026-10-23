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

	public GameObject Normal => normal;

	public GameObject Off => off;

	public Image Off_Icon => off_Icon;

	public TextMeshProUGUI Off_Text => off_Text;

	public GameObject On => on;

	public Image On_Icon => on_Icon;

	public GameObject On_Select => on_Select;

	public GameObject On_Select_Layout => on_Select_Layout;

	public Image On_Select_Line => on_Select_Line;

	public GameObject[] On_Select_Vfx => on_Select_Vfx;

	public TextMeshProUGUI On_Text => on_Text;
}
