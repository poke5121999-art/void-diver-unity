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

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Difficulty => null;

	public TextMeshProUGUI Difficulty_Desc => null;

	public GameObject Notice => null;

	public TextMeshProUGUI Notice_Desc => null;

	public GameObject[] Slots => null;

	public GameObject WeaponProtect => null;

	public GameObject WeaponProtect_Box => null;

	public Button WeaponProtect_PadButton => null;

	public Toggle WeaponProtect_Toggle => null;

	public GameObject Bottom => null;

	public GameObject Cancel => null;

	public Button Cancel_Button => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Title => null;
}
