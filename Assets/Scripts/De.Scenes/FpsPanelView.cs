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

	public GameObject Fps => fps;

	public GameObject Fps_Average => fps_Average;

	public GameObject Fps_Average_Pivot => fps_Average_Pivot;

	public TextMeshProUGUI Fps_Average_Value => fps_Average_Value;

	public GameObject Fps_Current => fps_Current;

	public TextMeshProUGUI Fps_Current_Value => fps_Current_Value;

	public GameObject Fps_Max => fps_Max;

	public GameObject Fps_Max_Pivot => fps_Max_Pivot;

	public TextMeshProUGUI Fps_Max_Value => fps_Max_Value;

	public GameObject Fps_Min => fps_Min;

	public GameObject Fps_Min_Pivot => fps_Min_Pivot;

	public TextMeshProUGUI Fps_Min_Value => fps_Min_Value;

	public GameObject Network => network;

	public GameObject Network_ReceiveBps => network_ReceiveBps;

	public GameObject Network_ReceiveBps_Pivot => network_ReceiveBps_Pivot;

	public TextMeshProUGUI Network_ReceiveBps_Value => network_ReceiveBps_Value;

	public GameObject Network_RecvPacketsPerSec => network_RecvPacketsPerSec;

	public GameObject Network_RecvPacketsPerSec_Pivot => network_RecvPacketsPerSec_Pivot;

	public TextMeshProUGUI Network_RecvPacketsPerSec_Value => network_RecvPacketsPerSec_Value;

	public GameObject Network_Rtt => network_Rtt;

	public TextMeshProUGUI Network_Rtt_Value => network_Rtt_Value;

	public GameObject Network_SendBps => network_SendBps;

	public GameObject Network_SendBps_Pivot => network_SendBps_Pivot;

	public TextMeshProUGUI Network_SendBps_Value => network_SendBps_Value;

	public GameObject Network_SendPacketsPerSec => network_SendPacketsPerSec;

	public GameObject Network_SendPacketsPerSec_Pivot => network_SendPacketsPerSec_Pivot;

	public TextMeshProUGUI Network_SendPacketsPerSec_Value => network_SendPacketsPerSec_Value;
}
