using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalRoomCreationPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject address;

	[SerializeField]
	private TMP_InputField inputFieldIp;

	[SerializeField]
	private TMP_InputField inputFieldPort;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject btnBack;

	[SerializeField]
	private Button btnBack_Button;

	[SerializeField]
	private GameObject btnConfirm;

	[SerializeField]
	private Button btnConfirm_Button;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Address => null;

	public TMP_InputField InputFieldIp => null;

	public TMP_InputField InputFieldPort => null;

	public GameObject Bottom => null;

	public GameObject BtnBack => null;

	public Button BtnBack_Button => null;

	public GameObject BtnConfirm => null;

	public Button BtnConfirm_Button => null;
}
