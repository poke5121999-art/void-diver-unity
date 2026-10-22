using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class StressLineView : View
{
	[SerializeField]
	private GameObject stress;

	[SerializeField]
	private TextMeshProUGUI stress_Max;

	[SerializeField]
	private TextMeshProUGUI stress_Value;

	public GameObject Stress => stress;

	public TextMeshProUGUI Stress_Max => stress_Max;

	public TextMeshProUGUI Stress_Value => stress_Value;
}
