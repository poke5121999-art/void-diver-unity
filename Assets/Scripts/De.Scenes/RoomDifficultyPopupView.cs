using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomDifficultyPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject difficulty;

	[SerializeField]
	private TextMeshProUGUI difficulty_Desc;

	[SerializeField]
	private GameObject notice;

	[SerializeField]
	private TextMeshProUGUI notice_Desc;

	[SerializeField]
	private GameObject[] slots;

	[SerializeField]
	private GameObject weaponProtect;

	[SerializeField]
	private GameObject weaponProtect_Box;

	[SerializeField]
	private Button weaponProtect_PadButton;

	[SerializeField]
	private Toggle weaponProtect_Toggle;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject cancel;

	[SerializeField]
	private Button cancel_Button;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GameObject title;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Difficulty => difficulty;

	public TextMeshProUGUI Difficulty_Desc => difficulty_Desc;

	public GameObject Notice => notice;

	public TextMeshProUGUI Notice_Desc => notice_Desc;

	public GameObject[] Slots => slots;

	public GameObject WeaponProtect => weaponProtect;

	public GameObject WeaponProtect_Box => weaponProtect_Box;

	public Button WeaponProtect_PadButton => weaponProtect_PadButton;

	public Toggle WeaponProtect_Toggle => weaponProtect_Toggle;

	public GameObject Bottom => bottom;

	public GameObject Cancel => cancel;

	public Button Cancel_Button => cancel_Button;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Title => title;
}
