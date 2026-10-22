using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class FpsPanelView : View
{
	[SerializeField]
	private GameObject fps;

	[SerializeField]
	private GameObject fps_Average;

	[SerializeField]
	private GameObject fps_Average_Pivot;

	[SerializeField]
	private TextMeshProUGUI fps_Average_Value;

	[SerializeField]
	private GameObject fps_Current;

	[SerializeField]
	private TextMeshProUGUI fps_Current_Value;

	[SerializeField]
	private GameObject fps_Max;

	[SerializeField]
	private GameObject fps_Max_Pivot;

	[SerializeField]
	private TextMeshProUGUI fps_Max_Value;

	[SerializeField]
	private GameObject fps_Min;

	[SerializeField]
	private GameObject fps_Min_Pivot;

	[SerializeField]
	private TextMeshProUGUI fps_Min_Value;

	[SerializeField]
	private GameObject network;

	[SerializeField]
	private GameObject network_ReceiveBps;

	[SerializeField]
	private GameObject network_ReceiveBps_Pivot;

	[SerializeField]
	private TextMeshProUGUI network_ReceiveBps_Value;

	[SerializeField]
	private GameObject network_RecvPacketsPerSec;

	[SerializeField]
	private GameObject network_RecvPacketsPerSec_Pivot;

	[SerializeField]
	private TextMeshProUGUI network_RecvPacketsPerSec_Value;

	[SerializeField]
	private GameObject network_Rtt;

	[SerializeField]
	private TextMeshProUGUI network_Rtt_Value;

	[SerializeField]
	private GameObject network_SendBps;

	[SerializeField]
	private GameObject network_SendBps_Pivot;

	[SerializeField]
	private TextMeshProUGUI network_SendBps_Value;

	[SerializeField]
	private GameObject network_SendPacketsPerSec;

	[SerializeField]
	private GameObject network_SendPacketsPerSec_Pivot;

	[SerializeField]
	private TextMeshProUGUI network_SendPacketsPerSec_Value;

	public GameObject Fps => null;

	public GameObject Fps_Average => null;

	public GameObject Fps_Average_Pivot => null;

	public TextMeshProUGUI Fps_Average_Value => null;

	public GameObject Fps_Current => null;

	public TextMeshProUGUI Fps_Current_Value => null;

	public GameObject Fps_Max => null;

	public GameObject Fps_Max_Pivot => null;

	public TextMeshProUGUI Fps_Max_Value => null;

	public GameObject Fps_Min => null;

	public GameObject Fps_Min_Pivot => null;

	public TextMeshProUGUI Fps_Min_Value => null;

	public GameObject Network => null;

	public GameObject Network_ReceiveBps => null;

	public GameObject Network_ReceiveBps_Pivot => null;

	public TextMeshProUGUI Network_ReceiveBps_Value => null;

	public GameObject Network_RecvPacketsPerSec => null;

	public GameObject Network_RecvPacketsPerSec_Pivot => null;

	public TextMeshProUGUI Network_RecvPacketsPerSec_Value => null;

	public GameObject Network_Rtt => null;

	public TextMeshProUGUI Network_Rtt_Value => null;

	public GameObject Network_SendBps => null;

	public GameObject Network_SendBps_Pivot => null;

	public TextMeshProUGUI Network_SendBps_Value => null;

	public GameObject Network_SendPacketsPerSec => null;

	public GameObject Network_SendPacketsPerSec_Pivot => null;

	public TextMeshProUGUI Network_SendPacketsPerSec_Value => null;
}
