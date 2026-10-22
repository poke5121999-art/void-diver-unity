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

	public GameObject CreateRoom => createRoom;

	public Button CreateRoom_Button => createRoom_Button;

	public GameObject JoinRoom => joinRoom;

	public Button JoinRoom_Button => joinRoom_Button;
}
