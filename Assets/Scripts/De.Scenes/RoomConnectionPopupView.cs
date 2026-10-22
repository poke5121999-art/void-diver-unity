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

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject RoomCode => null;

	public TMP_InputField InputField => null;

	public GameObject Bottom => null;

	public GameObject Cancel => null;

	public Button BtnCancel => null;

	public GameObject Connect => null;

	public Button BtnConnect => null;
}
