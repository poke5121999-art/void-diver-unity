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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Address => null;

	public TMP_InputField InputFieldIp => null;

	public TMP_InputField InputFieldPort => null;

	public GameObject Bottom => null;

	public GameObject Cancel => null;

	public Button BtnCancel => null;

	public GameObject Connect => null;

	public Button BtnConnect => null;
}
