using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalRoomConnectionPopupView : View
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
	private GameObject cancel;

	[SerializeField]
	private Button btnCancel;

	[SerializeField]
	private GameObject connect;

	[SerializeField]
	private Button btnConnect;

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Address => address;

	public TMP_InputField InputFieldIp => inputFieldIp;

	public TMP_InputField InputFieldPort => inputFieldPort;

	public GameObject Bottom => bottom;

	public GameObject Cancel => cancel;

	public Button BtnCancel => btnCancel;

	public GameObject Connect => connect;

	public Button BtnConnect => btnConnect;
}
