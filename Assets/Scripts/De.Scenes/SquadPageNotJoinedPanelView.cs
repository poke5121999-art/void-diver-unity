using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class SquadPageNotJoinedPanelView : View
{
	[SerializeField]
	private GameObject createRoom;

	[SerializeField]
	private Button createRoom_Button;

	[SerializeField]
	private GameObject joinRoom;

	[SerializeField]
	private Button joinRoom_Button;

	public GameObject CreateRoom => null;

	public Button CreateRoom_Button => null;

	public GameObject JoinRoom => null;

	public Button JoinRoom_Button => null;
}
