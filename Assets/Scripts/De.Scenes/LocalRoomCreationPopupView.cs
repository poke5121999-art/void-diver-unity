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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Address => address;

	public TMP_InputField InputFieldIp => inputFieldIp;

	public TMP_InputField InputFieldPort => inputFieldPort;

	public GameObject Bottom => bottom;

	public GameObject BtnBack => btnBack;

	public Button BtnBack_Button => btnBack_Button;

	public GameObject BtnConfirm => btnConfirm;

	public Button BtnConfirm_Button => btnConfirm_Button;
}
