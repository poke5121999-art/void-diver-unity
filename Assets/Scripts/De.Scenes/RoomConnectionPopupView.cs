using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomConnectionPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject roomCode;

	[SerializeField]
	private TMP_InputField inputField;

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

	public GameObject RoomCode => roomCode;

	public TMP_InputField InputField => inputField;

	public GameObject Bottom => bottom;

	public GameObject Cancel => cancel;

	public Button BtnCancel => btnCancel;

	public GameObject Connect => connect;

	public Button BtnConnect => btnConnect;
}
